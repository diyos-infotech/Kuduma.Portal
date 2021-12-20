using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class AddReceipts : System.Web.UI.Page
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
                    LoadBankNames();
                    txtRecieptNo.Text = loadMaxRecieptid().ToString();
                    txtDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception eX)
            {
                Response.Redirect("Login.aspx");
                return;
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void LoadBankNames()
        {

            DataTable DtBankNames = GlobalData.Instance.LoadBankNames();
            if (DtBankNames.Rows.Count > 0)
            {
                Ddl_Bank.DataValueField = "bankid";
                Ddl_Bank.DataTextField = "bankname";
                Ddl_Bank.DataSource = DtBankNames;
                Ddl_Bank.DataBind();


            }
            Ddl_Bank.Items.Insert(0, "-Select-");

        }

        protected void LoadClientIdAndName()
        {
            // string SqlqryForClientIdAndName = "select clientid from clients  Where Clientid like '%" + CmpIDPrefix + "%' order by  clientid";
            string SqlqryForClientIdAndName = string.Empty;
            string CompPrefix = "01/";

            SqlqryForClientIdAndName = "Select clientid from clients where clientid like '%" + CmpIDPrefix + "%'  Order By Clientid";
            DataTable dtForClientIdAndName = SqlHelper.Instance.GetTableByQuery(SqlqryForClientIdAndName);

            if (dtForClientIdAndName.Rows.Count > 0)
            {
                ddlClientID.DataTextField = "Clientid";
                ddlClientID.DataValueField = "Clientid";
                ddlClientID.DataSource = dtForClientIdAndName;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "-Select-");
            dtForClientIdAndName = null;
            //SqlqryForClientIdAndName = "select clientid,Clientname from clients  Where Clientid like '%"+CmpIDPrefix+"%' order by  Clientname";

            if (CmpIDPrefix == CompPrefix)
            {
                SqlqryForClientIdAndName = "Select clientid,Clientname from clients   Order By Clientname";

            }
            else
            {
                SqlqryForClientIdAndName = "Select clientid,Clientname from clients where clientid like '" + CmpIDPrefix + "%'  Order By Clientname";
            }



            dtForClientIdAndName = SqlHelper.Instance.GetTableByQuery(SqlqryForClientIdAndName);

            if (dtForClientIdAndName.Rows.Count > 0)
            {
                ddlCName.DataTextField = "Clientname";
                ddlCName.DataValueField = "Clientid";
                ddlCName.DataSource = dtForClientIdAndName;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "-Select-");
        }


        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClientID.SelectedIndex > 0)
            {
                Session["checkdownloadstatus"] = "0";
                ddlCName.SelectedValue = ddlClientID.SelectedValue;
                Clear();
                LoadAlltheBills();
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
                Clear();
                LoadAlltheBills();
            }
            else
            {
                ddlClientID.SelectedIndex = 0;
            }
        }

        protected void LoadAlltheBills()
        {


            //no  service tax  03/0035
            string SqlQryforBills = "";
            string SqlQryForST = "select isnull(servicetaxtype,0) as servicetaxtype  from contracts where clientid='" + ddlClientID.SelectedValue + "'";
            DataTable DtST = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForST).Result;
            if (DtST.Rows.Count <= 0)
            {
                txtDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                ddlReciveMode.Text = null;
                txtddorcheckdate.Text = null;
                ddlDDOrCheckstatus.Text = null;
                txtAmount.Text = null;
                txtddorcheckno.Text = null;
                txtbankname.Text = null;
                gvreciepts.DataSource = null;
                gvreciepts.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('There is no bills for Selected Client');", true);
                return;

            }
            if (bool.Parse(DtST.Rows[0]["servicetaxtype"].ToString()))
            {
                SqlQryforBills = "Select (unitid+' '+'('+c.clientname+')') as unitid, BillNo,CESS,SHECess,ServiceTax,  CONVERT(VARCHAR(10), fromdt, 103) AS fromdt  , " +
                                    " CONVERT(VARCHAR(10), todt, 103) AS todt  ," +
                                    " (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)) as TotalTax,   " +
                                    "( (isnull(GrandTotal,0)) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) )as Total , " +
                                    "  round(grandtotal,0,0) as  grandtotal ,BillDt,month,case isnull(tdson,0) when 1 then round((round(grandtotal,0)*(cts.tds/100)),0) when 0 then round( (( (isnull(GrandTotal,0) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) )*(cts.tds/100))),0) end tdsAmt , " +
                                   " (( (isnull(GrandTotal,0) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)))))-case isnull(tdson,0) when 1 then round((round(grandtotal,0)*(cts.tds/100)),0) when 0 then round( (( (isnull(GrandTotal,0) - ((Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0))) )*(cts.tds/100))),0) end  netpayable " +
                                    " From  Unitbill  inner join Clients C on C.clientid=Unitbill.unitid  inner join contracts cts on cts.clientid=Unitbill.unitid and cts.contractid=Unitbill.contractid  Where (unitid='" +
                                    ddlClientID.SelectedValue + "' or C.Mainunitid='" + ddlClientID.SelectedValue + "' )  and unitbill.BillCompletedStatus=0 " +
                                    "union" +
                                 " (Select (unitid+' '+'('+c.clientname+')') as unitid, BillNo,CESS,SHECess,ServiceTax,  CONVERT(VARCHAR(10), fromdt, 103) AS fromdt  , " +
                                   " CONVERT(VARCHAR(10), todt, 103) AS todt  ," +
                                   " (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)) as TotalTax,   " +
                                   "( (isnull(GrandTotal,0)) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) )as Total , " +
                                   "  round(grandtotal,0,0) as  grandtotal ,BillDt,month,case isnull(tdson,0) when 1 then (round(grandtotal,0)*(cts.tds/100)) when 0 then (( (isnull(GrandTotal,0) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) )*(cts.tds/100))) end tdsAmt, " +
                                   " (( (isnull(GrandTotal,0) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)))))-case isnull(tdson,0) when 1 then round((round(grandtotal,0)*(cts.tds/100)),0) when 0 then round( (( (isnull(GrandTotal,0) - ((Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0))) )*(cts.tds/100))),0) end  netpayable " +
                                   " From  MUnitBill inner join Clients C on C.clientid=MUnitBill.unitid inner join contracts cts on cts.clientid=MUnitBill.unitid   Where (unitid='" +
                                   ddlClientID.SelectedValue + "' or C.Mainunitid='" + ddlClientID.SelectedValue + "') and   isnull(munitbill.BillCompletedStatus,0)=0 )";


            }
            else
            {

                SqlQryforBills = "Select  (unitid+' '+'('+c.clientname+')') as unitid,BillNo,CESS,SHECess,ServiceTax,  CONVERT(VARCHAR(10), fromdt, 103) AS fromdt  , " +
                                 " CONVERT(VARCHAR(10), todt, 103) AS todt  ," +
                                 " (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) as TotalTax,   " +
                                 "( (isnull(GrandTotal,0)))as Total , " +
                                 "(( (isnull(GrandTotal,0)))-(case isnull(tdson,0) when 1 then (round(grandtotal,0)*(cts.tds/100)) when 0 then (( (isnull(GrandTotal,0) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) )*(cts.tds/100))) end)) as netpayable, " +
                                 "  round(grandtotal,0,0) as  grandtotal ,BillDt,month,case isnull(tdson,0) when 1 then (round(grandtotal,0)*(cts.tds/100)) when 0 then (( (isnull(GrandTotal,0) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) )*(cts.tds/100))) end tdsAmt     From  Unitbill inner join Clients C on C.clientid=Unitbill.unitid  inner join contracts cts on cts.clientid=UnitBill.unitid and cts.contractid=UnitBill.contractid   Where (unitid='" +
                                 ddlClientID.SelectedValue + "' or C.Mainunitid='" + ddlClientID.SelectedValue + "')  and   unitbill.BillCompletedStatus=0 " +
                                  "union" +
                                 " (Select (unitid+' '+'('+c.clientname+')') as unitid, BillNo,CESS,SHECess,ServiceTax,  CONVERT(VARCHAR(10), fromdt, 103) AS fromdt  , " +
                                   " CONVERT(VARCHAR(10), todt, 103) AS todt  ," +
                                   " (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) as TotalTax,   " +
                                   "( (isnull(GrandTotal,0)))as Total , " +
                                 "(( (isnull(GrandTotal,0)))-(case isnull(tdson,0) when 1 then (round(grandtotal,0)*(cts.tds/100)) when 0 then (( (isnull(GrandTotal,0) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) )*(cts.tds/100))) end)) as netpayable, " +
                                   "  round(grandtotal,0,0) as  grandtotal ,BillDt,month,case isnull(tdson,0) when 1 then (round(grandtotal,0)*(cts.tds/100)) when 0 then (( (isnull(GrandTotal,0) - (Isnull(CESS,0)+Isnull(SHECess,0)+Isnull(SBCessAmt,0)+Isnull(KKCessAmt,0)+Isnull(ServiceTax,0)+isnull(cgstamt,0)+isnull(sgstamt,0)+isnull(igstamt,0)) )*(cts.tds/100))) end tdsAmt       From  MUnitBill inner join Clients C on C.clientid=MUnitBill.unitid  inner join contracts cts on cts.clientid=MUnitBill.unitid   Where (unitid='" +
                                   ddlClientID.SelectedValue + "' or C.Mainunitid='" + ddlClientID.SelectedValue + "') and  munitbill.BillCompletedStatus=0 ) order by billno desc";



            }

            DataTable DtForBills = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryforBills).Result;

            if (DtForBills.Rows.Count > 0)
            {
                gvreciepts.DataSource = DtForBills;
                gvreciepts.DataBind();

                // Session["ReceiptDetails"] = dt.Rows.Count + 1;


                if (gvreciepts.Rows.Count > 0)
                {

                    foreach (GridViewRow gvrow in gvreciepts.Rows)
                    {
                        string Billno = ((Label)gvrow.FindControl("lblbillno")).Text;
                        float Grandtotal = float.Parse(((Label)gvrow.FindControl("lblgrandtotal")).Text);

                        //float Grandtotal = float.Parse(((Label)gvrow.FindControl("lblgrandtotal")).ToString());
                        Label lblpendingamt = ((Label)gvrow.FindControl("lblpendingamt"));
                        Label lblnetpaybleamt = ((Label)gvrow.FindControl("lblnetpaybleamt"));
                        TextBox txttdsamt = ((TextBox)gvrow.FindControl("txttdsamt"));
                        TextBox txtrecievedamt = ((TextBox)gvrow.FindControl("txtrecievedamt"));
                        Label lbldueamt = ((Label)gvrow.FindControl("lbldueamt"));


                        string SQlQryForRecieptDetails = "select Sum(isnull(RecievedAmt,0) ) as Recievedamt, " +
                            " Sum(isnull(TDSAmt,0)) as  TDSAmt, Sum(isnull(Disallowance,0)) as Disallowance    from ReceiptMaster Where  billno='" +
                             Billno + "'  Group by  billno,TDSAmt ,Disallowance  ";
                        DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(SQlQryForRecieptDetails).Result;
                        if (Dt.Rows.Count > 0)
                        {

                            float RecieveDAmt = 0;
                            float Disallowance = 0;
                            float RtdsAmt = 0;

                            for (int Tdsindex = 0; Tdsindex < Dt.Rows.Count; Tdsindex++)
                            {
                                RecieveDAmt += float.Parse(Dt.Rows[Tdsindex]["Recievedamt"].ToString());
                                RtdsAmt += float.Parse(Dt.Rows[Tdsindex]["TDSAmt"].ToString());
                                Disallowance += float.Parse(Dt.Rows[Tdsindex]["Disallowance"].ToString());
                            }

                            txttdsamt.Text = RtdsAmt.ToString();

                            lblpendingamt.Text = (Grandtotal - RecieveDAmt - float.Parse(txttdsamt.Text) - Disallowance).ToString();
                            lblnetpaybleamt.Text = (Grandtotal - float.Parse(txttdsamt.Text)).ToString();
                            txttdsamt.ReadOnly = true;
                        }
                        else
                        {
                            // lblpendingamt.Text = Grandtotal.ToString();

                            // txttdsamt.Text = "0.00";



                        }

                    }
                }

                #region  Begin code for Retriving Extra Amount  as on [12-09-2013]
                float RetriveExtraAmount = 0;

                //string SqlQryForRetriveExtraAmount = "Select  isnull( ExtraAmount,0) as ExtraAmount  from ExtraBillAmounts   Where Clientid='" + ddlClientID.SelectedValue + "'  and  status=0  Order by  RecieptNo desc";

                string SqlQryForRetriveExtraAmount = "Select  isnull( ExtraAmt,0) as ExtraAmount  from receiptextraamt   Where Clientid='" + ddlClientID.SelectedValue + "' ";
                DataTable DtForRetriveExtraAmount = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForRetriveExtraAmount).Result;
                if (DtForRetriveExtraAmount.Rows.Count > 0)
                {
                    RetriveExtraAmount = float.Parse(DtForRetriveExtraAmount.Rows[0]["ExtraAmount"].ToString());
                }
                else
                {
                    RetriveExtraAmount = 0;
                }

                txtextraAmount.Text = RetriveExtraAmount.ToString("0.00");

                #endregion End code for Retriving Extra Amount  as on [12-09-2013]
            }
            else
            {
                gvreciepts.DataSource = null;
                gvreciepts.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('There is no bills for Selected Client')", true);
                return;
            }

        }

        protected string loadMaxRecieptid()
        {
            int Recieptno = 1;
            string SqlqQryForRecieptid = "select max(Right(ReceiptNo,6)) ReceiptNo from   ReceiptDetails ";
            DataTable DtForRecieptid = config.ExecuteAdaptorAsyncWithQueryParams(SqlqQryForRecieptid).Result;
            if (DtForRecieptid.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(DtForRecieptid.Rows[0]["ReceiptNo"].ToString()) == false)
                    Recieptno = int.Parse(DtForRecieptid.Rows[0]["ReceiptNo"].ToString()) + 1;
            }
            return CmpIDPrefix + Recieptno.ToString("000000");
        }

        protected void txttdsamt_OnTextChanged(object sender, EventArgs e)
        {

            TextBox txtTDSAmt = sender as TextBox;
            GridViewRow row = null;
            if (txtTDSAmt == null)
                return;

            row = (GridViewRow)txtTDSAmt.NamingContainer;
            if (row == null)
                return;


            Label lblgrandtotal = row.FindControl("lblgrandtotal") as Label;
            Label lblnetpaybleamt = row.FindControl("lblnetpaybleamt") as Label;
            TextBox txttdsamt = row.FindControl("txttdsamt") as TextBox;


            if (lblgrandtotal == null)
                return;

            if (lblgrandtotal.Text.Trim().Length == 0)
            {
                lblnetpaybleamt.Text = "0.00";
                txttdsamt.Text = "0.00";
                return;
            }
            else
            {
                if (txttdsamt.Text.Trim().Length == 0)
                {
                    txttdsamt.Text = "0.00";
                }
                lblnetpaybleamt.Text = (float.Parse(lblgrandtotal.Text) - float.Parse(txttdsamt.Text)).ToString();
                //lbldueamt.Text = (float.Parse(lblgrandtotal.Text) - float.Parse(txttdsamt.Text) - float.Parse(txtrecievedamt.Text)).ToString();

            }

        }

        protected void txtrecievedamt_OnTextChanged(object sender, EventArgs e)
        {



            TextBox txtRECievedAmt = sender as TextBox;
            GridViewRow row = null;
            if (txtRECievedAmt == null)
                return;

            row = (GridViewRow)txtRECievedAmt.NamingContainer;
            if (row == null)
                return;

            Label lblgrandtotal = row.FindControl("lblgrandtotal") as Label;
            TextBox txtrecievedamt = row.FindControl("txtrecievedamt") as TextBox;
            Label lbldueamt = row.FindControl("lbldueamt") as Label;
            TextBox txttdsamt = row.FindControl("txttdsamt") as TextBox;
            Label lblnetpaybleamt = row.FindControl("lblnetpaybleamt") as Label;
            Label lblpendingamt = row.FindControl("lblpendingamt") as Label;
            TextBox txtDisallowance = row.FindControl("txtDisallowance") as TextBox;


            if (lblgrandtotal == null)
                return;

            if (lblgrandtotal.Text.Trim().Length == 0)
            {
                lbldueamt.Text = "0.00";
                txttdsamt.Text = "0.00";
                lblnetpaybleamt.Text = "0.00";
                lblpendingamt.Text = "0.00";
                txtDisallowance.Text = "0.00";
                txtrecievedamt.Text = "0.00";

                return;
            }
            else
            {
                if (txtrecievedamt.Text.Trim().Length == 0)
                {
                    txtrecievedamt.Text = "0.00";
                }

                if (txttdsamt.Text.Trim().Length == 0)
                {
                    txttdsamt.Text = "0.00";
                }

                if (lblpendingamt.Text.Trim().Length == 0)
                {
                    lblpendingamt.Text = "0.00";
                }
                if (lbldueamt.Text.Trim().Length == 0)
                {
                    lbldueamt.Text = "0.00";
                }


                if (txtDisallowance.Text.Trim().Length == 0)
                {
                    txtDisallowance.Text = "0.00";
                }


                lblnetpaybleamt.Text = (float.Parse(lblgrandtotal.Text) - float.Parse(txttdsamt.Text)).ToString();

                if (float.Parse(lblpendingamt.Text) < float.Parse(txtrecievedamt.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Check The Recieved Amount With Pending Amount. ');", true);
                    return;
                }

                lbldueamt.Text = (float.Parse(lblgrandtotal.Text) - float.Parse(txttdsamt.Text) - float.Parse(txtrecievedamt.Text) - float.Parse(txtDisallowance.Text)).ToString();

                if (txttdsamt.ReadOnly == true)
                {
                    lbldueamt.Text = (float.Parse(lblpendingamt.Text) - float.Parse(txtrecievedamt.Text)).ToString();
                }
                else
                {
                    lblpendingamt.Text = lblnetpaybleamt.Text;
                }
            }

        }

        protected void btnaddReciept_Click(object sender, EventArgs e)
        {

            if (Session["checkdownloadstatus"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select the Other client');", true);
                return;
            }


            #region    Begin  Variable Declaration
            string Clientid = "";
            string Recieptno = "";
            string RecievedAmt = "";
            string RecievedDate = "";
            int RecivedMode = 0;
            string DDorChequeno = "";
            string DDorChequDate = "";
            string DDostatus = "";
            string DDbankname = "";
            string DDorCheckStatus = "";
            #endregion  End Variable Declaration

            #region Begin Pdf Varaibles

            string ClientNamep = "";
            string Clientidp = "";
            string todaydate = "";
            string Recievedamountp = "";
            string Billnop = "";
            string selecteddatep = "";
            string checkdatep = "";
            string drawn = "";
            #endregion End PDF Variables



            string CheckReceiptNo = "select ReceiptNo from  ReceiptDetails where ReceiptNo='" + txtRecieptNo.Text + "' ";
            DataTable dtReceiptNo = config.ExecuteAdaptorAsyncWithQueryParams(CheckReceiptNo).Result;
            if (dtReceiptNo.Rows.Count > 0)
            {
                txtRecieptNo.Text = loadMaxRecieptid().ToString();

            }

            #region   Begin  Code for  Manidatory field messages  as on [12-09-2013]
            if (ddlClientID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Select the Client ID/Name');", true);
                return;
            }


            if (txtAmount.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Enter The Recieved Amount');", true);
                return;
            }

            if (txtDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please  Enter The Recieved Date');", true);
                return;
            }

            if (ddlReciveMode.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Select  the Recieved Mode ');", true);
                return;
            }




            #endregion   End Code for  Manidatory field messages  as on [12-09-2013]

            int Statusforbill = 0;

            #region  Begin code   Assign Values From The Front End as on [12-09-2013]
            Clientid = ddlClientID.SelectedValue;
            Recieptno = txtRecieptNo.Text;
            RecievedAmt = txtAmount.Text;
            if (txtDate.Text.Trim().Length != 0)
            {
                RecievedDate = Timings.Instance.CheckDateFormat(txtDate.Text);
            }
            else
            {
                RecievedDate = "01/01/1900";
            }
            RecivedMode = ddlReciveMode.SelectedIndex;
            DDorChequeno = txtddorcheckno.Text;
            if (txtddorcheckdate.Text.Trim().Length != 0)
            {
                DDorChequDate = Timings.Instance.CheckDateFormat(txtddorcheckdate.Text);
            }
            else
            {
                DDorChequDate = "01/01/1900";
            }
            DDbankname = txtbankname.Text;
            if (ddlDDOrCheckstatus.SelectedIndex > 0)
            {
                DDorCheckStatus = ddlDDOrCheckstatus.SelectedIndex.ToString();
            }
            else
            {
                DDorCheckStatus = "-1";
            }

            #endregion  End code   Assign Values From The Front End as on [12-09-2013]


            #region  Begin code for Retriving Extra Amount  as on [12-09-2013]
            float RetriveExtraAmount = 0;
            string SqlQryForRetriveExtraAmount = "Select  isnull( ExtraAmt,0) as ExtraAmount  from ReceiptExtraAmt   Where Clientid='" + Clientid + "' ";
            DataTable DtForRetriveExtraAmount = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForRetriveExtraAmount).Result;
            if (DtForRetriveExtraAmount.Rows.Count > 0)
            {
                RetriveExtraAmount = float.Parse(DtForRetriveExtraAmount.Rows[0]["ExtraAmount"].ToString());
            }
            else
            {
                RetriveExtraAmount = 0;
            }

            #endregion End code for Retriving Extra Amount  as on [12-09-2013]

            if (ddlReciveMode.SelectedIndex == 1 || ddlReciveMode.SelectedIndex == 2 || ddlReciveMode.SelectedIndex == 3 || ddlReciveMode.SelectedIndex == 4)
            {


                if (ddlReciveMode.SelectedIndex == 2 || ddlReciveMode.SelectedIndex == 3 || ddlReciveMode.SelectedIndex == 4)
                {
                    DDostatus = ddlDDOrCheckstatus.SelectedValue;
                    if (txtddorcheckno.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Enter Either Check/DD No. ');", true);
                        return;
                    }

                    string SqlQryForCheckDDorCheckExist = "Select * from Receiptdetails  Where DDorCheckno='" + DDorChequeno + "'";
                    int Status = config.ExecuteNonQueryWithQueryAsync(SqlQryForRetriveExtraAmount).Result;
                    if (Status > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('The Check details Already Exist . ');", true);
                        return;
                    }
                }
                float TotalRecievedAmt = 0;

                foreach (GridViewRow gvrow in gvreciepts.Rows)
                {
                    string RecievedAMT = ((TextBox)gvrow.FindControl("txtrecievedamt")).Text;
                    if (RecievedAMT.Trim().Length == 0)
                    {
                        RecievedAMT = "0";
                    }
                    TotalRecievedAmt += (float.Parse(RecievedAMT));
                }

                if (TotalRecievedAmt > (float.Parse(RecievedAmt) + RetriveExtraAmount))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Total Received Amount not Equal To all the Bills Received Amount');", true);
                    return;
                }




                string SqlQryforDDorCheckDetails = string.Format("insert into ReceiptDetails(ReceiptNo,Month,DDorCheckno,Recievdamt," +
                    " Clientid,DDorCheckDate,RecievedMode,Checkstatus,Bankname,Timings) " +
                    " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", Recieptno, RecievedDate, DDorChequeno, RecievedAmt, Clientid,
                    DDorChequDate, RecivedMode, DDorCheckStatus, DDbankname, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));
                int DtDDorCheckDetails = config.ExecuteNonQueryWithQueryAsync(SqlQryforDDorCheckDetails).Result;
                if (DtDDorCheckDetails > 0)
                {
                    int StatusofRecord = 0;




                    foreach (GridViewRow gvrow in gvreciepts.Rows)
                    {

                        string RBillno = ((Label)gvrow.FindControl("lblbillno")).Text;
                        string RGrandTotal = ((Label)gvrow.FindControl("lblgrandtotal")).Text;
                        string RNetPaybleamt = ((Label)gvrow.FindControl("lblnetpaybleamt")).Text;
                        string RPendingAmt = ((Label)gvrow.FindControl("lblpendingamt")).Text;
                        string RTdsAmt = ((TextBox)gvrow.FindControl("txttdsamt")).Text;
                        TextBox txtTdsamt = (TextBox)gvrow.FindControl("txttdsamt");
                        string RRecievedAmt = ((TextBox)gvrow.FindControl("txtrecievedamt")).Text;
                        string RDueAmt = ((Label)gvrow.FindControl("lbldueamt")).Text;
                        string RDisallowance = ((TextBox)gvrow.FindControl("txtDisallowance")).Text;
                        string RDisallowanceReason = ((TextBox)gvrow.FindControl("txtDisallowanceReason")).Text;
                        //string RBillDate = ((Label)gvrow.FindControl("lblbilldate")).Text;
                        //string RPeriod = ((Label)gvrow.FindControl("lblperiod")).Text;

                        if (RRecievedAmt.Trim().Length == 0)
                        {
                            RRecievedAmt = "0";
                        }

                        if (RDisallowance.Trim().Length == 0)
                        {
                            RDisallowance = "0";
                        }

                        if (txtTdsamt.ReadOnly == true)
                        {
                            RTdsAmt = "0";

                        }


                        if (float.Parse(RRecievedAmt) > 0)
                        {
                            string SqlQryForBillDetails = string.Format("insert into ReceiptMaster(RecieptNo,Billno,RecievedDate,GrandTotal,DueAmt,TDSAmt,  " +
                                                         " RecievedAmt,Month,Clientid,Disallowance,DisallowanceReason,timings) " +
                                                         " values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ",
                                                          Recieptno, RBillno, RecievedDate, RGrandTotal, RDueAmt, RTdsAmt,
                                                          RRecievedAmt, "-NO-", Clientid, RDisallowance, RDisallowanceReason, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));
                            Statusforbill = config.ExecuteNonQueryWithQueryAsync(SqlQryForBillDetails).Result;

                            if (Statusforbill > 0 && StatusofRecord == 0)
                            {
                                StatusofRecord = 1;
                                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Reciepts Added Sucessfully');", true);
                            }


                            if (RDueAmt.Trim().Length > 0)
                            {
                                if (float.Parse(RDueAmt) == 0 || TotalRecievedAmt == RetriveExtraAmount)
                                {


                                    string UpdateBillStatus = string.Format("update ExtraBillAmounts set  Status=1   Where Clientid='{0}'", Clientid);
                                    int StauesbillupdateUBC = config.ExecuteNonQueryWithQueryAsync(UpdateBillStatus).Result;



                                    if (float.Parse(RDueAmt) == 0)
                                    {
                                        UpdateBillStatus = string.Format("update unitbill set  BillCompletedStatus=1   Where Billno='{0}'", RBillno);
                                        int Stauesbillupdate = config.ExecuteNonQueryWithQueryAsync(UpdateBillStatus).Result;

                                        UpdateBillStatus = string.Format("update munitbill set  BillCompletedStatus=1   Where Billno='" + RBillno + "'");
                                        int StauesbillupdateUB = config.ExecuteNonQueryWithQueryAsync(UpdateBillStatus).Result;
                                    }
                                }
                            }
                        }

                    }



                    #region    Begin Code For As on [12-09-2013]

                    if ((float.Parse(RecievedAmt) + RetriveExtraAmount) > TotalRecievedAmt)
                    {

                        string UpdateBillStatus = string.Format("update ExtraBillAmounts set Status=1 Where Clientid='{0}'", Clientid);
                        SqlHelper.Instance.ExecuteDMLQry(UpdateBillStatus);

                        float ExtraAmount = (float.Parse(RecievedAmt) + RetriveExtraAmount) - TotalRecievedAmt;

                        string SqlQryForExtraAmount = string.Format("Insert into ExtraBillAmounts(Clientid,CheckorDDno,CheckDate,ExtraAmount,Status,RecieptNo)  " +
                                                 "  values('{0}','{1}','{2}','{3}','{4}','{5}')  ", Clientid, DDorChequeno, DDorChequDate, ExtraAmount, "0", Recieptno);
                        SqlHelper.Instance.ExecuteDMLQry(SqlQryForExtraAmount);







                    }

                    string SqlQryForExtraAmt = "";

                    string qry = "select * from ReceiptExtraAmt where clientid='" + ddlClientID.SelectedValue + "'";
                    DataTable dt = SqlHelper.Instance.GetTableByQuery(qry);

                    float ExtraAmt = (float.Parse(RecievedAmt) + RetriveExtraAmount) - TotalRecievedAmt;

                    //if (ExtraAmt > 0)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            SqlQryForExtraAmt = "update ReceiptExtraAmt set extraamt='" + ExtraAmt + "',Modified_On='" + DateTime.Now + "' where clientid='" + ddlClientID.SelectedValue + "'";
                            SqlHelper.Instance.ExecuteDMLQry(SqlQryForExtraAmt);

                        }
                        else
                        {
                            SqlQryForExtraAmt = "insert into ReceiptExtraAmt (clientid,extraamt,Created_On) values ('" + Clientid + "','" + ExtraAmt + "','" + DateTime.Now + "')";
                            SqlHelper.Instance.ExecuteDMLQry(SqlQryForExtraAmt);
                        }

                    }


                    #endregion End Code For As on [12-09-2013]




                    LoadClearData();


                }


            }
        }

        protected void Clear()
        {
            ddlReciveMode.SelectedIndex = 0;
            ddlDDOrCheckstatus.SelectedIndex = 0;
            txtddorcheckdate.Text = "";
            txtAmount.Text = "";
            txtbankname.Text = "";
            txtDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            txtddorcheckno.Text = "";
            txtRecieptNo.Text = loadMaxRecieptid().ToString();
            txtextraAmount.Text = "";
            gvreciepts.DataSource = null;
            gvreciepts.DataBind();
        }

        protected void LoadClearData()
        {

            ddlClientID.SelectedIndex = 0;
            ddlCName.SelectedIndex = 0;
            ddlReciveMode.SelectedIndex = 0;
            ddlDDOrCheckstatus.SelectedIndex = 0;
            txtddorcheckdate.Text = "";
            txtAmount.Text = "";
            txtbankname.Text = "";
            txtDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            txtddorcheckno.Text = "";
            txtRecieptNo.Text = loadMaxRecieptid().ToString();
            txtextraAmount.Text = "";
            gvreciepts.DataSource = null;
            gvreciepts.DataBind();
        }


        protected void btnreceipt_Click(object sender, EventArgs e)
        {
            int month = 0;
            string fontStyle = "Tahoma";

            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            string strQry = "Select * from CompanyInfo";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }


            string SqlQuryForServiCharge = "select * from ReceiptDetails where clientid ='" + ddlClientID.SelectedItem.ToString() + "'";
            DataTable DtServicecharge = SqlHelper.Instance.GetTableByQuery(SqlQuryForServiCharge);
            string ClientName = "";
            string txtddorcheckNo = txtddorcheckno.Text; ;



            document.AddTitle(companyName);
            document.AddAuthor("DIYOS");
            document.AddSubject("RECEIPT");
            document.AddSubject("No. TN/CHN/");
            document.AddKeywords("Keyword1, keyword2, …");
            string imagepath = Server.MapPath("~/assets/BillLogo.png");


            for (int i = 0; i < 2; i++)
            {


                if (File.Exists(imagepath))
                {
                    iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);

                    gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                    gif2.ScalePercent(70f);
                    document.Add(new Paragraph(" "));
                    document.Add(gif2);
                }

                PdfPTable tablelogo = new PdfPTable(2);
                tablelogo.TotalWidth = 500f;
                tablelogo.LockedWidth = true;
                float[] widtlogo = new float[] { 2f, 2f };
                tablelogo.SetWidths(widtlogo);


                PdfPCell CCompName = new PdfPCell(new Paragraph(companyName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 16, Font.BOLD, BaseColor.BLACK)));
                CCompName.HorizontalAlignment = 1;
                CCompName.Border = 0;
                CCompName.Colspan = 2;
                tablelogo.AddCell(CCompName);


                PdfPCell CCompAddress = new PdfPCell(new Paragraph(companyAddress, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, Font.BOLD, BaseColor.BLACK)));
                CCompAddress.HorizontalAlignment = 1;
                CCompAddress.Border = 0;
                CCompAddress.Colspan = 2;
                tablelogo.AddCell(CCompAddress);

                PdfPCell CReceipt = new PdfPCell(new Paragraph("\n\n\nRECEIPT", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
                CReceipt.HorizontalAlignment = 1;
                CReceipt.Border = 0;
                CReceipt.Colspan = 2;
                tablelogo.AddCell(CReceipt);


                PdfPCell CellNoleft = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CellNoleft.HorizontalAlignment = 0;
                CellNoleft.Border = 0;
                tablelogo.AddCell(CellNoleft);


                PdfPCell CellNo = new PdfPCell(new Paragraph("Date: " + DateTime.Now.Date, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CellNo.HorizontalAlignment = 2;
                CellNo.Border = 0;
                tablelogo.AddCell(CellNo);
                document.Add(tablelogo);


                PdfPTable tablemid = new PdfPTable(6);
                tablemid.TotalWidth = 500f;
                tablemid.LockedWidth = true;
                float[] widtmid = new float[] { 1f, 1f, 1f, 1f, 2f, 2f };
                tablemid.SetWidths(widtmid);



                PdfPCell CReceived = new PdfPCell(new Paragraph("Received with thanks from :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CReceived.HorizontalAlignment = 0;
                CReceived.Border = 0;
                CReceived.Colspan = 2;
                tablemid.AddCell(CReceived);


                PdfPCell CReceivedval = new PdfPCell(new Phrase("" + ddlCName.SelectedItem, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                CReceivedval.HorizontalAlignment = 0;
                CReceivedval.Border = 0;
                CReceivedval.Colspan = 4;
                tablemid.AddCell(CReceivedval);


                PdfPCell CUnitno = new PdfPCell(new Paragraph("\nUnit No :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CUnitno.HorizontalAlignment = 0;
                CUnitno.Border = 0;
                CUnitno.Colspan = 0;
                tablemid.AddCell(CUnitno);


                PdfPCell CUnitnoval = new PdfPCell(new Paragraph("\n" + ddlClientID.SelectedValue, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                CUnitnoval.HorizontalAlignment = 0;
                CUnitnoval.Border = 0;
                CUnitnoval.Colspan = 1;
                tablemid.AddCell(CUnitnoval);

                PdfPCell CSum = new PdfPCell(new Paragraph("\nSum of Rupees(in words) :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CSum.HorizontalAlignment = 0;
                CSum.Border = 0;
                CSum.Colspan = 2;
                tablemid.AddCell(CSum);


                PdfPCell CSumval = new PdfPCell(new Paragraph("\n(" + txtAmount.Text + ")- " + NumberToEnglish.Instance.changeNumericToWords(txtAmount.Text.ToString()) + "Rupees Only", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                CSumval.HorizontalAlignment = 0;
                CSumval.Border = 0;
                CSumval.Colspan = 3;
                tablemid.AddCell(CSumval);

                #region begin code For Print Invoice Nos As on [23-09-2013]

                string AllBillNos = "";
                foreach (GridViewRow gvrow in gvreciepts.Rows)
                {
                    string RBillno = ((Label)gvrow.FindControl("lblbillno")).Text;
                    string RRecievedAmt = ((TextBox)gvrow.FindControl("txtrecievedamt")).Text;
                    if (RRecievedAmt.Trim().Length > 0 && float.Parse(RRecievedAmt) > 0)
                    {
                        AllBillNos += RBillno + ",";
                    }

                }
                #endregion  End  code For Print Invoice Nos As on [23-09-2013]


                PdfPCell CProforma = new PdfPCell(new Paragraph("\ntowards our Proforma Invoice No./Invoice No. :" + AllBillNos, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CProforma.HorizontalAlignment = 0;
                CProforma.Border = 0;
                //CProforma.Rowspan = 2;
                CProforma.Colspan = 5;
                tablemid.AddCell(CProforma);


                //PdfPCell CProformaval = new PdfPCell(new Paragraph("\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                //CProformaval.HorizontalAlignment = 0;
                //CProformaval.Border = 0;
                //CProformaval.Colspan = 1;
                //tablemid.AddCell(CProformaval);

                PdfPCell CDated = new PdfPCell(new Paragraph("\nDated :" + txtDate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CDated.HorizontalAlignment = 0;
                CDated.Border = 0;
                tablemid.AddCell(CDated);


                //PdfPCell CDatedval = new PdfPCell(new Paragraph("\n" + txtDate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                //CDatedval.HorizontalAlignment = 0;
                //CDatedval.Border = 0;
                //CDatedval.Colspan = 1;
                //tablemid.AddCell(CDatedval);




                PdfPCell CTds = new PdfPCell(new Paragraph("\nTDS :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CTds.HorizontalAlignment = 0;
                CTds.Border = 0;
                CTds.Colspan = 1;
                tablemid.AddCell(CTds);


                PdfPCell CTdsval = new PdfPCell(new Paragraph("\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                CTdsval.HorizontalAlignment = 0;
                CTdsval.Border = 0;
                CTdsval.Colspan = 1;
                tablemid.AddCell(CTdsval);

                PdfPCell Ctdsp = new PdfPCell(new Paragraph("\nTDS % :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                Ctdsp.HorizontalAlignment = 0;
                Ctdsp.Border = 0;
                Ctdsp.Colspan = 1;
                tablemid.AddCell(Ctdsp);


                PdfPCell Ctdspval = new PdfPCell(new Paragraph("\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                Ctdspval.HorizontalAlignment = 0;
                Ctdspval.Border = 0;
                Ctdspval.Colspan = 1;
                tablemid.AddCell(Ctdspval);


                PdfPCell CVide = new PdfPCell(new Paragraph("\nVide Cash/Cheque/DD No. :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CVide.HorizontalAlignment = 0;
                CVide.Border = 0;
                CVide.Colspan = 1;
                tablemid.AddCell(CVide);


                PdfPCell CVideval = new PdfPCell(new Paragraph("\n" + txtddorcheckNo, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                CVideval.HorizontalAlignment = 0;
                CVideval.Border = 0;
                CVideval.Colspan = 1;
                tablemid.AddCell(CVideval);




                PdfPCell CDated1 = new PdfPCell(new Paragraph("\nDated :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CDated1.HorizontalAlignment = 0;
                CDated1.Border = 0;
                CDated1.Colspan = 1;
                tablemid.AddCell(CDated1);


                PdfPCell CDated1val = new PdfPCell(new Paragraph("\n" + txtddorcheckdate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                CDated1val.HorizontalAlignment = 0;
                CDated1val.Border = 0;
                CDated1val.Colspan = 2;
                tablemid.AddCell(CDated1val);

                PdfPCell CDrawn = new PdfPCell(new Paragraph("\nDrawn in  :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CDrawn.HorizontalAlignment = 0;
                CDrawn.Border = 0;
                CDrawn.Colspan = 1;
                tablemid.AddCell(CDrawn);


                PdfPCell CDrawnval = new PdfPCell(new Paragraph("\n" + txtbankname.Text + "(Bank)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                CDrawnval.HorizontalAlignment = 0;
                CDrawnval.Border = 0;
                CDrawnval.Colspan = 2;
                tablemid.AddCell(CDrawnval);




                PdfPCell CDisa = new PdfPCell(new Paragraph("\nDisallowance (If any) :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CDisa.HorizontalAlignment = 0;
                CDisa.Border = 0;
                CDisa.Colspan = 3;
                tablemid.AddCell(CDisa);

                PdfPCell CDisaval = new PdfPCell(new Paragraph("\n\n\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL | Font.UNDERLINE, BaseColor.BLACK)));
                CDisaval.HorizontalAlignment = 0;
                CDisaval.Border = 0;
                CDisaval.Colspan = 3;
                tablemid.AddCell(CDisaval);


                PdfPCell CRaddress = new PdfPCell(new Paragraph("\n\nRegional Office Address Stamp", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                CRaddress.HorizontalAlignment = 0;
                CRaddress.BorderWidth = 1;
                CRaddress.Colspan = 3;
                tablemid.AddCell(CRaddress);


                PdfPCell Cauth = new PdfPCell(new Paragraph("\n\nFor WARRIOR FMS INDIA PVT LIMITED\n\nAuthorised Signatory \n\n\n\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
                Cauth.HorizontalAlignment = 2;
                Cauth.Border = 0;
                Cauth.Colspan = 3;
                tablemid.AddCell(Cauth);

                document.Add(tablemid);

            }

            document.NewPage();
            document.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();

        }

        protected void lbtn_Add_Click(object sender, ImageClickEventArgs e)
        {
            if (gvreciepts.Rows.Count > 0)
            {

                TextBox txtDisallowance = ((TextBox)gvreciepts.FindControl("txtDisallowance"));
                Label lblnetpaybleamt = ((Label)gvreciepts.FindControl("lblnetpaybleamt"));
                Label lbldueamt = ((Label)gvreciepts.FindControl("lbldueamt"));
                TextBox txtrecievedamt = ((TextBox)gvreciepts.FindControl("txtrecievedamt"));

                txtDisallowance.Text = "0";
                txtrecievedamt.Text = txtAmount.Text;
                lbldueamt.Text = (Convert.ToDecimal(lblnetpaybleamt.Text) - Convert.ToDecimal(txtrecievedamt.Text)).ToString();



            }
        }

        string mvalue = "";
        string monthval = "";
        string yearvalue = "";


        float totalgrandtotal = 0;
        float totalpendingamt = 0;
        protected void gvreciepts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label month = (((Label)e.Row.FindControl("lblmonthn")));
                mvalue = month.Text.ToString();

                #region
                if (mvalue.Length == 3)
                {
                    monthval = mvalue.Substring(0, 1);

                    if (monthval == "1")
                    {
                        monthval = "Jan -";
                    }
                    if (monthval == "2")
                    {
                        monthval = "Feb -";
                    }
                    if (monthval == "3")
                    {
                        monthval = "Mar -";
                    }
                    if (monthval == "4")
                    {
                        monthval = "Apr -";
                    }
                    if (monthval == "5")
                    {
                        monthval = "May -";
                    }
                    if (monthval == "6")
                    {
                        monthval = "Jun -";
                    }
                    if (monthval == "7")
                    {
                        monthval = "Jul -";
                    }
                    if (monthval == "8")
                    {
                        monthval = "Aug -";
                    }
                    if (monthval == "9")
                    {
                        monthval = "Sep -";
                    }
                }
                else
                {
                    monthval = mvalue.Substring(0, 2);

                    if (monthval == "10")
                    {
                        monthval = "Oct -";
                    }

                    if (monthval == "11")
                    {
                        monthval = "Nov -";
                    }
                    if (monthval == "12")
                    {
                        monthval = "Dec -";
                    }
                }


                if (mvalue.Length == 3)
                {
                    yearvalue = mvalue.Substring(1, 2);
                }
                else
                {

                    yearvalue = mvalue.Substring(2, 2);
                }
                ((Label)e.Row.FindControl("lblmonth")).Text = monthval + "" + yearvalue;
                //e.Row.Cells[3].Attributes.Add("class", "text");
                #endregion

                float randtotal = float.Parse(((Label)e.Row.FindControl("lblgrandtotal")).Text);
                totalgrandtotal += randtotal;

                float pendingamt = float.Parse(((Label)e.Row.FindControl("lblpendingamt")).Text);
                totalpendingamt += pendingamt;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalgrandtotal")).Text = totalgrandtotal.ToString();
                ((Label)e.Row.FindControl("lblTotalpendingamt")).Text = totalpendingamt.ToString();
            }
        }


        protected void gvreciepts_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            float receivedamt = 0;

            var row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

            if (e.CommandName == "Add")
            {

                Label lblgrandtotal = row.FindControl("lblgrandtotal") as Label;
                TextBox txtrecievedamt = row.FindControl("txtrecievedamt") as TextBox;
                Label lbldueamt = row.FindControl("lbldueamt") as Label;
                TextBox txttdsamt = row.FindControl("txttdsamt") as TextBox;
                Label lblnetpaybleamt = row.FindControl("lblnetpaybleamt") as Label;
                Label lblpendingamt = row.FindControl("lblpendingamt") as Label;
                TextBox txtDisallowance = row.FindControl("txtDisallowance") as TextBox;

                if (txtAmount.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please fill cheque Amount');", true);
                    return;
                }

                if (lblgrandtotal == null)
                    return;

                if (lblgrandtotal.Text.Trim().Length == 0)
                {
                    lbldueamt.Text = "0.00";
                    txttdsamt.Text = "0.00";
                    lblnetpaybleamt.Text = "0.00";
                    lblpendingamt.Text = "0.00";
                    txtDisallowance.Text = "0.00";
                    txtrecievedamt.Text = "0.00";

                    return;
                }
                else
                {

                    if (txtrecievedamt.Text.Trim().Length == 0)
                    {
                        txtrecievedamt.Text = "0.00";
                    }


                    if (txttdsamt.Text.Trim().Length == 0)
                    {
                        txttdsamt.Text = "0.00";
                    }

                    if (lblpendingamt.Text.Trim().Length == 0)
                    {
                        lblpendingamt.Text = "0.00";
                    }
                    if (lbldueamt.Text.Trim().Length == 0)
                    {
                        lbldueamt.Text = "0.00";
                    }


                    if (txtDisallowance.Text.Trim().Length == 0)
                    {
                        txtDisallowance.Text = "0.00";
                    }

                    float leftamt = 0;

                    for (int i = 0; i < gvreciepts.Rows.Count; i++)
                    {
                        TextBox txtrecievedamnt = gvreciepts.Rows[i].FindControl("txtrecievedamt") as TextBox;

                        if (txtrecievedamnt.Text == "")
                        {
                            txtrecievedamnt.Text = "0";
                        }

                        receivedamt += float.Parse(txtrecievedamnt.Text);
                    }



                    if (float.Parse(txtextraAmount.Text) == 0)
                    {
                        leftamt = float.Parse(txtAmount.Text) - receivedamt;
                    }
                    else
                    {

                        leftamt = float.Parse(txtAmount.Text) + float.Parse(txtextraAmount.Text) - receivedamt;
                    }

                    if (leftamt > float.Parse(lblpendingamt.Text))
                    {
                        //txtrecievedamt.Text = ((float.Parse(txtAmount.Text)+float.Parse(txtextraAmount.Text)).ToString();
                        txtrecievedamt.Text = (float.Parse(lblpendingamt.Text)).ToString();
                    }
                    else
                    {

                        txtrecievedamt.Text = ((float.Parse(txtAmount.Text) + float.Parse(txtextraAmount.Text)) - receivedamt).ToString();


                        //    txtrecievedamt.Text = txtAmount.Text;
                    }



                    lblnetpaybleamt.Text = (float.Parse(lblgrandtotal.Text) - float.Parse(txttdsamt.Text)).ToString();

                    if (float.Parse(lblpendingamt.Text) < float.Parse(txtrecievedamt.Text))
                    {
                        txtrecievedamt.Text = "0";

                        ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Check The Received Amount With Pending Amount. ');", true);
                        return;
                    }

                    lbldueamt.Text = (float.Parse(lblgrandtotal.Text) - float.Parse(txttdsamt.Text) - float.Parse(txtrecievedamt.Text) - float.Parse(txtDisallowance.Text)).ToString();

                    if (txttdsamt.ReadOnly == true)
                    {
                        lbldueamt.Text = (float.Parse(lblpendingamt.Text) - float.Parse(txtrecievedamt.Text)).ToString();
                    }
                    else
                    {
                        lblpendingamt.Text = lblnetpaybleamt.Text;
                    }

                    //  row.Enabled = false;


                }
            }
        }
    }
}