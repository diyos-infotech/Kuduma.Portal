using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using Kuduma.Portal.DAL;


namespace Kuduma.Portal
{

    public partial class SampleAttendance : System.Web.UI.Page
    {

        GridViewExportUtil GVUtil = new GridViewExportUtil();
        AppConfiguration Config = new AppConfiguration();


        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
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
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli1");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }

                    LoadBranches();
                    LoadClientList();
                    LoadClientNames();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void LoadBranches()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtBranches = GlobalData.Instance.LoadLoginBranch(dtBranch);
            if (DtBranches.Rows.Count > 0)
            {
                ddlBranch.DataValueField = "BranchId";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataSource = DtBranches;
                ddlBranch.DataBind();
            }
            ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-All-", "0"));
        }

        protected void LoadClientNames()
        {
            string qry = "";
            if (ddlBranch.SelectedIndex == 0)
            {
                qry = "select Clientid,clientname from clients where clientid like '%" + CmpIDPrefix + "%'  order by clientid ";
            }
            else
            {
                qry = "select Clientid,clientname from clients where clientid like '%" + CmpIDPrefix + "%' and BranchID='" + ddlBranch.SelectedValue + "'  order by clientid ";
            }
            DataTable dt = SqlHelper.Instance.GetTableByQuery(qry);
            if (dt.Rows.Count > 0)
            {
                ddlCName.DataValueField = "Clientid";
                ddlCName.DataTextField = "clientname";
                ddlCName.DataSource = dt;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "-Select-");
            ddlCName.Items.Insert(1, "ALL");

        }

        protected void LoadClientList()
        {

            string qry = "";
            if (ddlBranch.SelectedIndex == 0)
            {
                qry = "select Clientid,clientname from clients where clientid like '%" + CmpIDPrefix + "%'  order by clientid ";
            }
            else
            {
                qry = "select Clientid,clientname from clients where clientid like '%" + CmpIDPrefix + "%' and BranchID='" + ddlBranch.SelectedValue + "'  order by clientid ";
            }

            DataTable dt = SqlHelper.Instance.GetTableByQuery(qry);
            if (dt.Rows.Count > 0)
            {
                ddlClientID.DataValueField = "Clientid";
                ddlClientID.DataTextField = "Clientid";
                ddlClientID.DataSource = dt;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "-Select-");
            ddlClientID.Items.Insert(1, "ALL");
        }

        protected void ddlCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlCName.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlClientID.SelectedValue = ddlCName.SelectedValue;

            }
            else
            {
                ddlClientID.SelectedIndex = 0;
            }
        }

        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlClientID.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlCName.SelectedValue = ddlClientID.SelectedValue;
            }
            else
            {
                ddlCName.SelectedIndex = 0;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            GVFullAttendance.DataSource = null;
            GVFullAttendance.DataBind();

            GVIndAttendance.DataSource = null;
            GVIndAttendance.DataBind();


            if (ddlClientID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client Id/Name');", true);

                return;
            }


            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            string FieldOfficer = "";


            string clientid = "";
            if (ddlClientID.SelectedIndex == 1)
            {
                clientid = "%";
            }
            else
            {
                clientid = ddlClientID.SelectedValue;
            }

            string Branch = "";
            if (ddlBranch.SelectedIndex == 0)
            {
                Branch = "%";
            }
            else
            {
                Branch = ddlBranch.SelectedValue;
            }

            string spname = "";
            DataTable dtBP = null;
            Hashtable HashtableBP = new Hashtable();

            int type = ddltype.SelectedIndex;

            spname = "SampleAttendance";
            HashtableBP.Add("@month", Month);
            HashtableBP.Add("@clientid", clientid);
            HashtableBP.Add("@type", type);
            HashtableBP.Add("@Option", ddloption.SelectedIndex);
            HashtableBP.Add("@ExcelType", 0);
            HashtableBP.Add("@Branch", Branch);

            dtBP = Config.ExecuteAdaptorAsyncWithParams(spname, HashtableBP).Result;



            if (dtBP.Rows.Count > 0)
            {

                if (ddltype.SelectedIndex == 0)
                {
                    GVFullAttendance.DataSource = dtBP;
                    GVFullAttendance.DataBind();

                    lbtn_Export.Visible = true;



                }
                else
                {
                    GVIndAttendance.DataSource = dtBP;
                    GVIndAttendance.DataBind();

                    lbtn_Export.Visible = true;
                    lbtn_ExportPDF.Visible = true;

                }


            }

        }

        protected void ClearData()
        {
            GVFullAttendance.DataSource = null;
            GVFullAttendance.DataBind();

            GVIndAttendance.DataSource = null;
            GVIndAttendance.DataBind();

            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            //GVUtil.Export("SampleAttendanceReports.xls", this.GVSampleAttendance);
            if (ddltype.SelectedIndex == 0)
            {
                GVUtil.Export("SampleAttendanceReports.xls", this.GVFullAttendance);
            }
            else
            {
                GVUtil.Export("SampleAttendanceReports.xls", this.GVIndAttendance);
            }

        }

        protected void SampleGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Attributes.Add("class", "text");
            }
        }

        protected void lbtn_Exportnew_Click(object sender, EventArgs e)
        {
            GVFullAttendance.DataSource = null;
            GVFullAttendance.DataBind();

            GVIndAttendance.DataSource = null;
            GVIndAttendance.DataBind();


            if (ddlClientID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client Id/Name');", true);
                return;
            }


            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            string FieldOfficer = "";
            string Branch = "";


            string clientid = "";
            if (ddlClientID.SelectedIndex == 1)
            {
                clientid = "%";
            }
            else
            {
                clientid = ddlClientID.SelectedValue;
            }

            if (ddlBranch.SelectedIndex == 0)
            {
                Branch = "%";
            }
            else
            {
                Branch = ddlBranch.SelectedValue;
            }


            string spname = "";
            DataTable dtBP = null;
            Hashtable HashtableBP = new Hashtable();

            int type = ddltype.SelectedIndex;

            spname = "SampleAttendance";
            HashtableBP.Add("@month", Month);
            HashtableBP.Add("@clientid", clientid);
            HashtableBP.Add("@type", type);
            HashtableBP.Add("@Option", ddloption.SelectedIndex);
            HashtableBP.Add("@ExcelType", 1);
            HashtableBP.Add("@Branch", Branch);

            dtBP = Config.ExecuteAdaptorAsyncWithParams(spname, HashtableBP).Result;

            if (dtBP.Rows.Count > 0)
            {
                GVUtil.ExporttoExcelAttendancesheet(dtBP);
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVFullAttendance.DataSource = null;
            GVFullAttendance.DataBind();

            GVIndAttendance.DataSource = null;
            GVIndAttendance.DataBind();

            txtmonth.Text = "";
            LoadClientList();
            LoadClientNames();
        }
        protected void btndownloadpdf_Click(object sender, EventArgs e)
        {
            string fontsyle = "Verdana";
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select The Month')", true);
                return;
            }

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            string spname = "";
            DataTable dtBP = null;
            Hashtable HashtableBP = new Hashtable();
            string Branch = "";
            string Clientid = "";

            if (ddlClientID.SelectedIndex == 1)
            {
                Clientid = CmpIDPrefix;
            }
            else
            {
                Clientid = ddlClientID.SelectedValue;
            }

            if (ddlBranch.SelectedIndex == 0)
            {
                Branch = "%";
            }
            else
            {
                Branch = ddlBranch.SelectedValue;
            }
            string monthprev = string.Empty;

            monthprev = DateTime.Parse(date).AddMonths(-1).Month.ToString();
            string Yearprev = "";
            if (month == "1" || month == "01")
            {
                Yearprev = DateTime.Parse(date).AddYears(-1).Year.ToString();
            }
            else
            {
                Yearprev = Year;
            }
            string monthyearprev = monthprev + Yearprev.Substring(2, 2);

            spname = "SampleAttendance";
            HashtableBP.Add("@month", monthyearprev);
            HashtableBP.Add("@clientid", Clientid);
            HashtableBP.Add("@type", 1);
            HashtableBP.Add("@Option", ddloption.SelectedIndex);
            HashtableBP.Add("@ExcelType", 1);
            HashtableBP.Add("@Branch", Branch);


            dt = Config.ExecuteAdaptorWithParams(spname, HashtableBP).Result;

            DataTable dtgroup = dt.DefaultView.ToTable(true, "ClientID", "ClientName");

            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = Config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }


            MemoryStream ms = new MemoryStream();

            int Fontsize = 7;
            if (dt.Rows.Count > 0)
            {
                Document document = new Document(PageSize.A4.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                for (int L = 0; L < dtgroup.Rows.Count; L++)
                {
                    string Contractid = string.Empty;
                    int PaysheetDates = 0;
                    var Gendays = 0;

                    int monthval = 0;
                    int yearval = 0;

                    if (Month.ToString().Length == 3)
                    {
                        monthval = int.Parse(Month.ToString().Substring(0, 1));
                        yearval = int.Parse("20" + Month.ToString().Substring(1, 2));

                    }


                    if (Month.ToString().Length == 4)
                    {
                        monthval = int.Parse(Month.ToString().Substring(0, 2));
                        yearval = int.Parse("20" + Month.ToString().Substring(2, 2));

                    }

                    int Selectdays = System.DateTime.DaysInMonth(yearval, monthval);
                    string dateCheck = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();



                    DateTime DtLastDay = DateTime.Now;
                    DtLastDay = DateTime.Parse(dateCheck, CultureInfo.GetCultureInfo("en-gb"));


                    #region  Begin Get Contract Id Based on The Last Day


                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", dtgroup.Rows[L]["ClientID"].ToString());
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = Config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                    if (DTContractID.Rows.Count > 0)
                    {
                        Contractid = DTContractID.Rows[0]["contractid"].ToString();
                        PaysheetDates = int.Parse(DTContractID.Rows[0]["PaySheetDates"].ToString());
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                        return;
                    }

                    DateTime mGendays = DateTime.Now;
                    DateTime dates = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                    mGendays = DateTime.Parse(dates.ToString());
                    Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, PaysheetDates);

                    #endregion  End Get Contract Id Based on The Last Day

                    document.NewPage();
                    PdfPTable Maintable = new PdfPTable(39);
                    if (Gendays == 31)
                    {
                        Maintable = new PdfPTable(39);
                    }
                    if (Gendays == 30)
                    {
                        Maintable = new PdfPTable(38);
                    }
                    if (Gendays == 28)
                    {
                        Maintable = new PdfPTable(36);
                    }
                    if (Gendays == 29)
                    {
                        Maintable = new PdfPTable(37);
                    }
                    Maintable.TotalWidth = 830;
                    Maintable.LockedWidth = true;
                    Maintable.HeaderRows = 5;
                    float[] width = new float[] { };
                    if (Gendays == 31)
                    {
                        width = new float[] { 1.7f, 3f, 8f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.8f };
                    }
                    if (Gendays == 30)
                    {
                        width = new float[] { 1.7f, 3f, 8f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.8f };
                    }
                    if (Gendays == 28)
                    {
                        width = new float[] { 1.7f, 3f, 8f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.8f };
                    }
                    if (Gendays == 29)
                    {
                        width = new float[] { 1.7f, 3f, 8f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.6f, 1.8f };
                    }
                    Maintable.SetWidths(width);

                    PdfPCell cell;

                    cell = new PdfPCell(new Phrase("MUSTER ROLL", FontFactory.GetFont(fontsyle, Fontsize + 4, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 39;
                    cell.Border = 0;
                    Maintable.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 4, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 39;
                    cell.Border = 0;
                    cell.FixedHeight = 10;
                    Maintable.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Name of Contractor :", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 20;
                    cell.Border = 0;
                    Maintable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("For the month of : " + txtmonth.Text, FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 19;
                    cell.Border = 0;
                    Maintable.AddCell(cell);


                    string imagepath = Server.MapPath("~/assets/" + CmpIDPrefix + "BillLogo.png");

                    if (File.Exists(imagepath))
                    {
                        iTextSharp.text.Image paysheetlogo = iTextSharp.text.Image.GetInstance(imagepath);
                        paysheetlogo.ScaleAbsolute(45f, 45f);
                        cell = new PdfPCell();
                        Paragraph cmplogo = new Paragraph();
                        cmplogo.Add(new Chunk(paysheetlogo, -7, 10));
                        cell.AddElement(cmplogo);
                        cell.HorizontalAlignment = 0;
                        cell.Colspan = 2;
                        cell.Border = 0;
                        Maintable.AddCell(cell);
                    }
                    cell = new PdfPCell(new Phrase(companyName, FontFactory.GetFont(fontsyle, Fontsize + 3, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 18;
                    cell.PaddingTop = 10;
                    cell.Border = 0;
                    Maintable.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Client ID : " + dtgroup.Rows[L]["ClientID"].ToString() + "\nName & Address of Principal Employer Name & Address of Establishment / Location in which contract is carried on _______________ : \n\n" + dtgroup.Rows[L]["ClientName"].ToString(), FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 19;
                    cell.Border = 0;
                    Maintable.AddCell(cell);

                    #region for Attendance

                    #region

                    cell = new PdfPCell(new Phrase("Sno", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Code", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Name\nDesignation", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable.AddCell(cell);
                    if (Gendays == 31 || Gendays == 30)
                    {
                        #region
                        if (PaysheetDates == 3)//21 to 20
                        {
                            #region 
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("29", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("30", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            if (Gendays == 31)
                            {
                                cell = new PdfPCell(new Phrase("31", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                Maintable.AddCell(cell);
                            }
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        if (PaysheetDates == 2)//26 to 25
                        {
                            #region 
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("29", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("30", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            if (Gendays == 31)
                            {
                                cell = new PdfPCell(new Phrase("31", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                Maintable.AddCell(cell);
                            }
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        if (PaysheetDates == 0)//1st to 1st
                        {
                            #region
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("29", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("30", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            if (Gendays == 31)
                            {
                                cell = new PdfPCell(new Phrase("31", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                Maintable.AddCell(cell);
                            }
                            #endregion
                        }
                        #endregion
                    }

                    if (Gendays == 28)
                    {
                        #region
                        if (PaysheetDates == 3)//21 to 20
                        {
                            #region 
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        if (PaysheetDates == 2)//26 to 25
                        {
                            #region 
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        if (PaysheetDates == 0)//1st to 1st
                        {
                            #region
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        #endregion
                    }


                    if (Gendays == 29)
                    {
                        #region
                        if (PaysheetDates == 3)//21 to 20
                        {
                            #region 
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("29", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        if (PaysheetDates == 2)//26 to 25
                        {
                            #region 
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("29", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        if (PaysheetDates == 0)//1st to 1st
                        {
                            #region
                            cell = new PdfPCell(new Phrase("01", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("02", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("03", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("04", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("05", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("06", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("07", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("08", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("09", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("13", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("14", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("15", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("16", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("17", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("18", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("19", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("20", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("21", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("22", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("23", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("24", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("25", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("26", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("27", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("28", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("29", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        #endregion
                    }


                    cell = new PdfPCell(new Phrase("PR", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("OT", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("WO", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("TOT", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("SIGN", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable.AddCell(cell);

                    #endregion

                    int SNo = 1;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region

                        if (dtgroup.Rows[L]["Clientid"].ToString() == dt.Rows[i]["Clientid"].ToString())
                        {
                            string OldEmpid = dt.Rows[i]["OldEmpid"].ToString();
                            string EmpName = dt.Rows[i]["EmpName"].ToString();
                            string Designation = dt.Rows[i]["Designation"].ToString();

                            cell = new PdfPCell(new Phrase(SNo++.ToString(), FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase(OldEmpid, FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase(EmpName + "\n" + Designation, FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            if (Gendays == 31 || Gendays == 30)
                            {
                                #region
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                if (Gendays == 31)
                                {
                                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                    cell.HorizontalAlignment = 0;
                                    Maintable.AddCell(cell);
                                }
                                #endregion

                            }

                            if (Gendays == 28)
                            {
                                #region
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);

                                #endregion
                            }

                            if (Gendays == 29)
                            {
                                #region
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);

                                #endregion
                            }

                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);

                        }

                        #endregion
                    }

                    for (int W = 0; W < 4; W++)
                    {
                        cell = new PdfPCell(new Phrase(SNo++.ToString(), FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable.AddCell(cell);
                        if (Gendays == 31 || Gendays == 30)
                        {
                            #region
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            if (Gendays == 31)
                            {
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                Maintable.AddCell(cell);
                            }
                            #endregion
                        }
                        if (Gendays == 28)
                        {
                            #region
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            #endregion
                        }

                        if (Gendays == 29)
                        {
                            #region
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            Maintable.AddCell(cell);
                            #endregion
                        }
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable.AddCell(cell);
                    }

                    #endregion

                    document.Add(Maintable);

                    #region For Summary

                    PdfPTable Maintable1 = new PdfPTable(13);
                    Maintable1.TotalWidth = 820;
                    Maintable1.LockedWidth = true;
                    float[] width1 = new float[] { 8f, 3f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1.3f, 1f, 1f, 1f };
                    Maintable1.SetWidths(width1);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 13;
                    cell.Border = 0;
                    cell.FixedHeight = 10;
                    Maintable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("ATTENDANCE SUMMARY", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 12;
                    cell.Border = 0;
                    Maintable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Rank", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("S/sup", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("S/G", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("A/G", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("S/W Full", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("SW (PT)", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Peon", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Pantry Boy", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Computer Operator", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Receptionist", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    Maintable1.AddCell(cell);

                    for (int M = 0; M < 3; M++)
                    {
                        string Rank = "";
                        if (M == 0)
                        {
                            Rank = "No of Head";
                        }
                        if (M == 1)
                        {
                            Rank = "Normal Duty";
                        }
                        if (M == 2)
                        {
                            Rank = "Extra Duty";
                        }
                        if (M == 3)
                        {
                            Rank = "Total Duty";
                        }

                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Colspan = 1;
                        cell.Border = 0;
                        Maintable1.AddCell(cell);

                        cell = new PdfPCell(new Phrase(Rank, FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        Maintable1.AddCell(cell);
                    }

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Border = 0;
                    cell.Colspan = 13;
                    cell.FixedHeight = 25;
                    Maintable1.AddCell(cell);

                    document.Add(Maintable1);

                    PdfPTable Maintable2 = new PdfPTable(4);
                    Maintable2.TotalWidth = 820;
                    Maintable2.LockedWidth = true;
                    float[] width2 = new float[] { 1f, 1f, 1f, 1f };
                    Maintable2.SetWidths(width2);


                    cell = new PdfPCell(new Phrase("Prepared By", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Border = 0; cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Checked By", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Border = 0; cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Sign S/Supervisor/ Incharge", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Border = 0; cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Sign of Client Rep", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Border = 0; cell.Colspan = 1;
                    Maintable2.AddCell(cell);

                    document.Add(Maintable2);

                    #endregion


                }


                string filename = "AttendanceSheet" + ".pdf";

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

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVFullAttendance.DataSource = null;
            GVFullAttendance.DataBind();

            GVIndAttendance.DataSource = null;
            GVIndAttendance.DataBind();

            if (ddltype.SelectedIndex == 1)
            {
                lbtn_ExportPDF.Visible = true;
                btn_Submit.Visible = false;
            }
            else
            {
                lbtn_ExportPDF.Visible = false;
                btn_Submit.Visible = true;

            }
        }
    }


}


