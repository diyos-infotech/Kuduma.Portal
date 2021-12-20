using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KLTS.Data;
using Kuduma.Portal.DAL;


namespace Kuduma.Portal
{
    public partial class ReceiptDetails : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                        
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                    if (this.Master != null)
                    {
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("c5");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }
                    LoadClientIdAndName();
                    ddlReceiptNos.Items.Insert(0, "--Select--");
                    LoadReceipts();
                }
            }
            catch (Exception eX)
            {
                Response.Redirect("Login.aspx");
                return;
            }
        }

        protected void LoadClientIdAndName()
        {
            // string SqlqryForClientIdAndName = "select clientid from clients  Where Clientid like '%" + CmpIDPrefix + "%' order by  clientid";
            string SqlqryForClientIdAndName = string.Empty;
            string CompPrefix = "01/";

            SqlqryForClientIdAndName = "Select clientid from clients where clientid like '%" + CmpIDPrefix + "%'   Order By Clientid";



            DataTable dtForClientIdAndName = config.ExecuteAdaptorAsyncWithQueryParams(SqlqryForClientIdAndName).Result;

            if (dtForClientIdAndName.Rows.Count > 0)
            {
                ddlClientID.DataTextField = "Clientid";
                ddlClientID.DataValueField = "Clientid";
                ddlClientID.DataSource = dtForClientIdAndName;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "-Select-");
            ddlClientID.Items.Insert(1, "All");

            dtForClientIdAndName = null;
            //SqlqryForClientIdAndName = "select clientid,Clientname from clients  Where Clientid like '%"+CmpIDPrefix+"%' order by  Clientname";

            if (CmpIDPrefix == CompPrefix)
            {
                SqlqryForClientIdAndName = "Select clientid,Clientname from clients and BranchID='" + BranchID + "'  Order By Clientname";

            }
            else
            {
                SqlqryForClientIdAndName = "Select clientid,Clientname from clients where clientid like '" + CmpIDPrefix + "%' and BranchID='" + BranchID + "' Order By Clientname";
            }



            dtForClientIdAndName = config.ExecuteAdaptorAsyncWithQueryParams(SqlqryForClientIdAndName).Result;

            if (dtForClientIdAndName.Rows.Count > 0)
            {
                ddlCName.DataTextField = "Clientname";
                ddlCName.DataValueField = "Clientid";
                ddlCName.DataSource = dtForClientIdAndName;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "-Select-");
            ddlCName.Items.Insert(1, "All");

        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }


        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClientID.SelectedIndex > 0)
            {
                Session["checkdownloadstatus"] = "0";
                ddlCName.SelectedValue = ddlClientID.SelectedValue;
                LoadReceiptDetails();
            }
            else
            {
                ddlCName.SelectedIndex = 0;
            }
        }

        protected void ddlCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCName.SelectedIndex > 0)
            {
                Session["checkdownloadstatus"] = "0";
                ddlClientID.SelectedValue = ddlCName.SelectedValue;
                LoadReceiptDetails();

            }
            else
            {
                ddlClientID.SelectedIndex = 0;
            }
        }

        public void LoadReceiptDetails()
        {
            string clientid = ddlClientID.SelectedValue;
            if (ddlClientID.SelectedIndex == 1)
            {
                clientid = "%";
            }
            string qry = "select distinct RecieptNo from receiptmaster where clientid like '" + clientid + "' order by RecieptNo";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlReceiptNos.DataTextField = "RecieptNo";
                ddlReceiptNos.DataValueField = "RecieptNo";
                ddlReceiptNos.DataSource = dt;
                ddlReceiptNos.DataBind();
                ddlReceiptNos.Items.Insert(0, "All");

            }
            else
            {
                ddlReceiptNos.Items.Clear();
            }
        }

        public void LoadReceipts()
        {
            string qry = "select distinct rm.clientid,c.clientname,rm.RecieptNo,STUFF(( SELECT ' / '+BillNo+ ' ' from receiptmaster  where receiptmaster.RecieptNo =rm.RecieptNo   for xml path(''),Type).value('(./text())[1]','VARCHAR(MAX)'),1,2,'') as Billno from receiptmaster rm " +
                         " inner join clients c on c.clientid=rm.Clientid order by RecieptNo desc";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count > 0)
            {
                gvreciepts.DataSource = dt;
                gvreciepts.DataBind();
            }
            else
            {
                gvreciepts.DataSource = null;
                gvreciepts.DataBind();
            }
        }


        string mvalue = "";
        string monthval = "";
        string yearvalue = "";

        public void PDFDOWNLOAD(string clientid, string receiptno)
        {

            try
            {
                string type = "ReceiptPDF";
                var SPName = "";
                Hashtable HTPaysheet = new Hashtable();
                SPName = "ReceiptReports";
                HTPaysheet.Add("@clientid", clientid);
                HTPaysheet.Add("@type", type);
                HTPaysheet.Add("@receiptno", receiptno);
                DataTable dt = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTPaysheet);

                if (dt.Rows.Count > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    Document document = new Document(PageSize.A4);
                    var writer = PdfWriter.GetInstance(document, ms);
                    document.Open();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);


                    string strQry = "Select * from CompanyInfo ";
                    DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                    string companyName = "Your Company Name";
                    string companyAddress = "Your Company Address";
                    string companyaddressline = " ";
                    string emailid = "";
                    string website = "";
                    string phoneno = "";
                    if (compInfo.Rows.Count > 0)
                    {
                        companyName = compInfo.Rows[0]["CompanyName"].ToString();
                        companyAddress = compInfo.Rows[0]["Address"].ToString();
                        companyaddressline = compInfo.Rows[0]["Addresslineone"].ToString();
                        emailid = compInfo.Rows[0]["Emailid"].ToString();
                        website = compInfo.Rows[0]["Website"].ToString();
                        phoneno = compInfo.Rows[0]["Phoneno"].ToString();
                    }

                    string FontStyle = "verdana";

                    int Fontsize = 9;
                    int i = 0;
                    string clientname = "";




                    PdfPTable tablen = new PdfPTable(3);
                    tablen.TotalWidth = 500f;
                    tablen.LockedWidth = true;
                    float[] widths = new float[] { 2f, 2.29f, 1.71f };
                    tablen.SetWidths(widths);

                    PdfPCell cellspace = new PdfPCell(new Phrase(companyName, FontFactory.GetFont(FontStyle, Fontsize + 3, Font.BOLD, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 1;
                    cellspace.Colspan = 3;
                    cellspace.Border = 0;
                    tablen.AddCell(cellspace);

                    cellspace = new PdfPCell(new Phrase(companyAddress, FontFactory.GetFont(FontStyle, Fontsize + 1, Font.NORMAL, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 1;
                    cellspace.Colspan = 3;
                    cellspace.Border = 0;
                    cellspace.SetLeading(0, 1.2f);
                    tablen.AddCell(cellspace);

                    //cellspace = new PdfPCell(new Phrase("Email :" + emailid + " | " + "Website :" + website, FontFactory.GetFont(FontStyle, Fontsize + 1, Font.NORMAL, BaseColor.BLACK)));
                    //cellspace.HorizontalAlignment = 1;
                    //cellspace.Colspan = 3;
                    //cellspace.Border = 0;
                    //cellspace.SetLeading(0, 1.2f);
                    //tablen.AddCell(cellspace);

                    cellspace = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize + 3, Font.BOLD, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 1;
                    cellspace.Colspan = 3;
                    cellspace.Border = 0;
                    tablen.AddCell(cellspace);

                    cellspace = new PdfPCell(new Phrase("Receipt No : " + receiptno, FontFactory.GetFont(FontStyle, Fontsize + 1, Font.NORMAL, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 0;
                    cellspace.PaddingTop = 20;
                    cellspace.Border = 0;
                    tablen.AddCell(cellspace);

                    cellspace = new PdfPCell(new Phrase("RECEIPT", FontFactory.GetFont(FontStyle, Fontsize + 3, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 2;
                    cellspace.PaddingRight = 100;
                    cellspace.Border = 0;
                    tablen.AddCell(cellspace);

                    cellspace = new PdfPCell(new Phrase("Date:" + dt.Rows[0]["RecievedDate"].ToString(), FontFactory.GetFont(FontStyle, Fontsize + 1, Font.NORMAL, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 2;
                    //cellspace.PaddingRight = 50;
                    cellspace.PaddingTop = 20;
                    cellspace.Border = 0;
                    tablen.AddCell(cellspace);

                    document.Add(tablen);

                    PdfPTable table = new PdfPTable(2);
                    table.TotalWidth = 500f;
                    table.LockedWidth = true;
                    float[] width = new float[] { 2f, 3.3f };
                    table.SetWidths(width);


                    clientname = dt.Rows[i]["ClientName"].ToString();

                    PdfPCell cellsno = new PdfPCell(new Phrase("Client Name", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellsno.HorizontalAlignment = 0;
                    cellsno.PaddingBottom = 12;
                    cellsno.PaddingTop = 12;
                    cellsno.PaddingRight = 50;
                    table.AddCell(cellsno);

                    cellsno = new PdfPCell(new Phrase(clientname + " (" + clientid + ")\n" + dt.Rows[i]["Addr"].ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellsno.HorizontalAlignment = 0;
                    cellsno.PaddingTop = 12;
                    cellsno.SetLeading(0f, 1.2f);
                    table.AddCell(cellsno);


                    PdfPCell CellBiLLno = new PdfPCell(new Phrase("Bill No", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    CellBiLLno.HorizontalAlignment = 0;
                    CellBiLLno.PaddingBottom = 12;
                    CellBiLLno.PaddingTop = 12;
                    CellBiLLno.PaddingRight = 50;
                    table.AddCell(CellBiLLno);

                    CellBiLLno = new PdfPCell(new Phrase(dt.Rows[i]["Billno"].ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CellBiLLno.HorizontalAlignment = 0;
                    CellBiLLno.PaddingTop = 12;
                    CellBiLLno.SetLeading(0f, 1.2f);
                    table.AddCell(CellBiLLno);

                    PdfPCell cellnameofwrkman1 = new PdfPCell(new Phrase("Amount Received", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellnameofwrkman1.HorizontalAlignment = 0;
                    cellnameofwrkman1.PaddingBottom = 12;
                    cellnameofwrkman1.PaddingTop = 12;
                    cellnameofwrkman1.PaddingRight = 50;
                    table.AddCell(cellnameofwrkman1);

                    float amt = 0;
                    amt = float.Parse(dt.Rows[0]["Recievdamt"].ToString());

                    string gtotal = NumberToEnglish.Instance.changeNumericToWords(amt.ToString("#"));

                    cellnameofwrkman1 = new PdfPCell(new Phrase("Rs. " + amt.ToString("#,##0.00") + "\n" + "Rupees " + gtotal + " Only", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellnameofwrkman1.HorizontalAlignment = 0;
                    cellnameofwrkman1.PaddingTop = 12;
                    cellnameofwrkman1.SetLeading(0f, 1.2f);


                    table.AddCell(cellnameofwrkman1);

                    cellnameofwrkman1 = new PdfPCell(new Phrase("Received Mode", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellnameofwrkman1.HorizontalAlignment = 0;
                    cellnameofwrkman1.PaddingBottom = 7;
                    cellnameofwrkman1.PaddingTop = 7;
                    cellnameofwrkman1.PaddingRight = 50;
                    table.AddCell(cellnameofwrkman1);

                    cellnameofwrkman1 = new PdfPCell(new Phrase(dt.Rows[0]["mode"].ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellnameofwrkman1.HorizontalAlignment = 0;
                    cellnameofwrkman1.PaddingBottom = 7;
                    cellnameofwrkman1.PaddingTop = 7;
                    cellnameofwrkman1.PaddingRight = 50;
                    table.AddCell(cellnameofwrkman1);

                    PdfPCell cellsex1 = new PdfPCell(new Phrase("Cheque.No/DD/Transaction.No\nDate", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellsex1.HorizontalAlignment = 0;
                    cellsex1.PaddingBottom = 12;
                    cellsex1.PaddingTop = 12;
                    cellsex1.PaddingRight = 50;
                    table.AddCell(cellsex1);

                    cellsex1 = new PdfPCell(new Phrase(dt.Rows[0]["DDorCheckno"].ToString() + "        " + "Dt : " + "       " + dt.Rows[0]["DDorCheckDate"].ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellsex1.HorizontalAlignment = 0;
                    cellsex1.PaddingTop = 12;
                    cellsex1.SetLeading(0f, 1.2f);
                    table.AddCell(cellsex1);

                    PdfPCell celldesign1 = new PdfPCell(new Phrase("Bank Name", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    celldesign1.HorizontalAlignment = 0;
                    celldesign1.PaddingBottom = 7;
                    celldesign1.PaddingTop = 7;
                    celldesign1.PaddingRight = 50;
                    table.AddCell(celldesign1);

                    cellsex1 = new PdfPCell(new Phrase(dt.Rows[0]["Bankname"].ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellsex1.HorizontalAlignment = 0;
                    cellsex1.PaddingTop = 7;
                    cellsex1.PaddingBottom = 7;
                    cellsex1.SetLeading(0f, 1.2f);
                    table.AddCell(cellsex1);

                    PdfPCell celldaily = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    celldaily.HorizontalAlignment = 0;
                    table.AddCell(celldaily);

                    celldaily = new PdfPCell(new Phrase("Authorised Signatory", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    celldaily.HorizontalAlignment = 2;
                    celldaily.FixedHeight = 50f;
                    celldaily.PaddingTop = 25;
                    table.AddCell(celldaily);



                    document.Add(table);


                    string filename = "Receipt For " + clientname + ".pdf";

                    document.NewPage();
                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void lbtn_download_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblClientid = (Label)thisGridViewRow.FindControl("lblClientid");
                Label lblReceiptNo = (Label)thisGridViewRow.FindControl("lblReceiptNo");

                PDFDOWNLOAD(lblClientid.Text, lblReceiptNo.Text);

            }
            catch (Exception ex)
            {

            }
        }

        protected void lbtn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblClientid = (Label)thisGridViewRow.FindControl("lblClientid");
                Label lblReceiptNo = (Label)thisGridViewRow.FindControl("lblReceiptNo");

                Response.Redirect("ViewReceipts.aspx?clientid=" + lblClientid.Text + "&ReceiptNo=" + lblReceiptNo.Text, false);
            }
            catch (Exception ex)
            {
                // lblMsg.Text = "Your Session Expired . Please Login";

            }

        }

        protected void gvreciepts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


            string qry = "";
            int status = 0;

            Label ClientId = gvreciepts.Rows[e.RowIndex].FindControl("lblclientid") as Label;
            Label lblReceiptNo = gvreciepts.Rows[e.RowIndex].FindControl("lblReceiptNo") as Label;



            qry = "select ClientId,RecievedDate,RecieptNo,Billno,GrandTotal,DueAmt,TDSAmt,RecievedAmt,Disallowance from Receiptmaster where RecieptNo='" + lblReceiptNo.Text + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            string clientid = "";
            string RecievedDate = "";
            string RecieptNo = "";
            string BillNo = "";
            string GrandTotal = "";
            string DueAmt = "";
            string TDSAmt = "";
            string ReceivedAmt = "";
            string DisAllowance = "";
            string Deleted_By = "";
            string Deleted_On = "";

            if (dt.Rows.Count > 0)
            {
                clientid = dt.Rows[0]["clientid"].ToString();
                RecievedDate = dt.Rows[0]["RecievedDate"].ToString();
                RecieptNo = dt.Rows[0]["RecieptNo"].ToString();
                BillNo = dt.Rows[0]["Billno"].ToString();
                GrandTotal = dt.Rows[0]["GrandTotal"].ToString();
                DueAmt = dt.Rows[0]["DueAmt"].ToString();
                TDSAmt = dt.Rows[0]["TDSAmt"].ToString();
                ReceivedAmt = dt.Rows[0]["RecievedAmt"].ToString();
                DisAllowance = dt.Rows[0]["Disallowance"].ToString();
                Deleted_By = Session["UserId"].ToString();
                Deleted_On = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");



                qry = "select * from receiptextraamt where clientid='" + clientid + "'";
                dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

                if (dt.Rows.Count > 0)
                {
                    string qry1 = "select sum(RecievedAmt) as RecievedAmt from receiptmaster where recieptno='" + RecieptNo + "'";
                    DataTable dtnew = config.ExecuteAdaptorAsyncWithQueryParams(qry1).Result;
                    float BillReceivedAmt = 0;
                    if (dtnew.Rows.Count > 0)
                    {
                        BillReceivedAmt = float.Parse(dtnew.Rows[0]["RecievedAmt"].ToString());

                    }


                    string qry3 = "select Recievdamt from receiptdetails where receiptno='" + RecieptNo + "'";
                    DataTable dtnew3 = config.ExecuteAdaptorAsyncWithQueryParams(qry3).Result;
                    float ChequeReceivedAmt = 0;
                    if (dtnew3.Rows.Count > 0)
                    {
                        ChequeReceivedAmt = float.Parse(dtnew3.Rows[0]["Recievdamt"].ToString());

                    }

                    string qry2 = "update receiptextraamt set extraamt=extraamt+(" + BillReceivedAmt + " - " + ChequeReceivedAmt + ") where clientid='" + clientid + "'";
                    SqlHelper.Instance.ExecuteDMLQry(qry2);

                }


                string qry4 = "insert into DeletedReceipts (ClientId,RecievedDate,RecieptNo,Billno,GrandTotal,DueAmt,TDSAmt,RecievedAmt,Disallowance,Deleted_By,Deleted_On) " +
                    "values('" + clientid + "','" + RecievedDate + "','" + RecieptNo + "','" + BillNo + "','" + GrandTotal + "','" + DueAmt + "','" + TDSAmt + "','" + ReceivedAmt + "','" + DisAllowance + "','" + Deleted_By + "','" + Deleted_On + "')";
                status = config.ExecuteNonQueryWithQueryAsync(qry4).Result;

                string qry5 = "delete from ReceiptDetails where ReceiptNo='" + lblReceiptNo.Text + "'";
                status = config.ExecuteNonQueryWithQueryAsync(qry5).Result;

                string qry6 = "select BillNo from Receiptmaster where RecieptNo='" + lblReceiptNo.Text + "'";
                DataTable dtdelete = config.ExecuteAdaptorAsyncWithQueryParams(qry6).Result;

                String billlist = "";

                if (dtdelete.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdelete.Rows.Count; i++)
                    {
                        billlist = dtdelete.Rows[i]["BillNo"].ToString();

                        string qry7 = "update unitbill set billcompletedstatus=0 where billno='" + billlist + "'";
                        status = config.ExecuteNonQueryWithQueryAsync(qry7).Result;

                    }
                }


                string qry8 = "delete from Receiptmaster where RecieptNo='" + lblReceiptNo.Text + "'";
                status = config.ExecuteNonQueryWithQueryAsync(qry8).Result;



                string qry9 = "delete from ExtraBillAmounts where RecieptNo='" + lblReceiptNo.Text + "'";
                status = config.ExecuteNonQueryWithQueryAsync(qry9).Result;





                string qry10 = "select BillNo from munitbill where billno='" + BillNo + "'";
                dtdelete = config.ExecuteAdaptorAsyncWithQueryParams(qry10).Result;

                if (dtdelete.Rows.Count > 0)
                {

                    string qry11 = "update munitbill set billcompletedstatus=0 where billno='" + BillNo + "'";
                    status = config.ExecuteNonQueryWithQueryAsync(qry11).Result;

                }


                if (status != 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Receipt deleted successfully');", true);

                    // return;
                }

                BindReceipts();

            }

        }

        protected void gvreciepts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvreciepts.PageIndex = e.NewPageIndex;
            LoadReceipts();
        }

        public void BindReceipts()
        {


            string qry = "select rm.clientid,c.clientname,rm.RecieptNo,STUFF(( SELECT ' / '+ billno  from Receiptmaster rms where rms.RecieptNo = rm.RecieptNo for xml path(''), Type).value('(./text())[1]', 'VARCHAR(MAX)'),1,2,''  ) as billno from receiptmaster rm inner join clients c on c.clientid=rm.Clientid order by RecieptNo";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count > 0)
            {
                gvreciepts.DataSource = dt;
                gvreciepts.DataBind();
            }
            else
            {

                gvreciepts.DataSource = null;
                gvreciepts.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('No receipts are there for selected client');", true);
                return;
            }


        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlClientID.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Client ID/Name');", true);
                }

                string clientid = ddlClientID.SelectedValue;
                if (ddlClientID.SelectedIndex == 1)
                {
                    clientid = "%";
                }


                string receiptno = ddlReceiptNos.SelectedValue;
                if (ddlReceiptNos.SelectedIndex == 0)
                {
                    receiptno = "%";
                }


                string qry = "select distinct rm.clientid,c.clientname,rm.RecieptNo,STUFF(( SELECT ' / '+ billno  from Receiptmaster rms where rms.RecieptNo = rm.RecieptNo  for xml path(''), Type).value('(./text())[1]', 'VARCHAR(MAX)'),1,2,''  ) as billno from receiptmaster rm inner join clients c on c.clientid=rm.Clientid where rm.clientid like '" + clientid + "' and RecieptNo like '" + receiptno + "' order by RecieptNo";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

                if (dt.Rows.Count > 0)
                {
                    gvreciepts.DataSource = dt;
                    gvreciepts.DataBind();
                }
                else
                {

                    gvreciepts.DataSource = null;
                    gvreciepts.DataBind();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('No receipts are there for selected client');", true);
                    return;
                }


            }
            catch (Exception ex)
            {

            }
        }


    }
}