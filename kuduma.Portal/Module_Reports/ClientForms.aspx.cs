using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KLTS.Data;
using System.Data;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ClientForms : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string BranchID = "";
        AppConfiguration Config = new AppConfiguration();
        GridViewExportUtil GVUtill = new GridViewExportUtil();

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
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli2");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }
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

        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");

        }

        protected void LoadClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlclientid.SelectedValue = ddlcname.SelectedValue;

                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();

            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddlclientid.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlcname.SelectedValue = ddlclientid.SelectedValue;

                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString().Substring(0, 3);

            return monthname;
        }

        public string GetMonthOfYear()
        {
            string MonthYear = "";

            int month = GetMonthBasedOnSelectionDateorMonth();
            if (month.ToString().Length == 4)
            {

                MonthYear = "20" + month.ToString().Substring(2, 2);

            }
            if (month.ToString().Length == 3)
            {

                MonthYear = "20" + month.ToString().Substring(1, 2);

            }
            return MonthYear;
        }

        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";

            #region Validation

            if (txtmonth.Text.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You have entered invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You have entered invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return 0;
                }
            }
            #endregion


            #region  Month Get Based on the Control Selection
            int month = 0;

            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            month = Timings.Instance.GetIdForEnteredMOnth(date);
            //return month;

            return month;

            #endregion
        }

        protected void ddlForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            if (ddlForms.SelectedIndex == 0)
            {
                lblclientid.Visible = false;
                lblclientname.Visible = false;
                lblDOJ.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                txtmonth.Visible = false;
                ddlclientid.Visible = false;
                ddlcname.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;

            }



            if (ddlForms.SelectedIndex == 1)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                btnSubmit.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
            }

            if (ddlForms.SelectedIndex == 2)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = true;

            }

            if (ddlForms.SelectedIndex == 3)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;
            }



            if (ddlForms.SelectedIndex == 4)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;
            }

            if (ddlForms.SelectedIndex == 5)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;
            }

            if (ddlForms.SelectedIndex == 6)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;

            }

            if (ddlForms.SelectedIndex == 7)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                btnSubmit.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
            }


            if (ddlForms.SelectedIndex == 8)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;
            }

            if (ddlForms.SelectedIndex == 9)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
            }

            if (ddlForms.SelectedIndex == 10)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = false;
                txtmonth.Visible = false;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = true;
                lblto.Visible = true;
                txtfrom.Visible = true;
                txtto.Visible = true;
                btnSubmit.Visible = false;
            }

            if (ddlForms.SelectedIndex == 11)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = false;
                txtmonth.Visible = false;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = true;
                lblto.Visible = true;
                txtfrom.Visible = true;
                txtto.Visible = true;
                btnSubmit.Visible = false;
            }
            if (ddlForms.SelectedIndex == 12)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;

            }

            if (ddlForms.SelectedIndex == 13)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;

            }
            if (ddlForms.SelectedIndex == 14)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;

            }
            if (ddlForms.SelectedIndex == 15)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;

            }

            if (ddlForms.SelectedIndex == 16)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;

            }

            if (ddlForms.SelectedIndex == 17)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                btnSubmit.Visible = false;
            }

            if (ddlForms.SelectedIndex == 18)
            {
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlcname.Visible = true;
                ddlclientid.Visible = true;
                lblmonth.Visible = false;
                txtmonth.Visible = false;
                lblDOJ.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblfrom.Visible = true;
                lblto.Visible = true;
                txtfrom.Visible = true;
                txtto.Visible = true;
                btnSubmit.Visible = false;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {

            if (ddlForms.SelectedIndex == 1)
            {
                btnformsXIV_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 2)
            {
                BtnformXV_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 3)
            {
                BtnformXX_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 4)
            {
                BtnformXXI_Click1(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 5)
            {
                BtnformXXII_Click1(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 6)
            {
                BtnformXXIII_Click1(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 7)
            {
                BtnformXXV_Click1(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 8)
            {
                btnformXIII_Click(sender, e);
                return;
            }
            if (ddlForms.SelectedIndex == 9)
            {
                btnFormQ_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 10)
            {
                btnFormLeaveWagesNew_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 11)
            {
                btnformD_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 12)
            {
                btnnewPaySheetExcel_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 13)
            {
                btnFormTPaysheet_Click(sender, e);
                return;
            }
            if (ddlForms.SelectedIndex == 14)
            {
                btnnewPaySheetAttendance_Click(sender, e);
                return;
            }
            if (ddlForms.SelectedIndex == 15)
            {
                btnnewPaySheetSalary_Click(sender, e);
                return;
            }
            if (ddlForms.SelectedIndex == 16)
            {
                btnnewPaySheetDed_Click(sender, e);
                return;
            }
            if (ddlForms.SelectedIndex == 17)
            {
                btnformXIIIExcel(sender, e);
                return;
            }
            if (ddlForms.SelectedIndex == 18)
            {
                btnFormXXIV_Click(sender, e);
                return;
            }
        }

        protected void btnFormXXIV_Click(object sender, EventArgs e)
        {
            int fontsize = 10;
            string fontstyle = "calibri";

            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.A4);
            var writer = PdfWriter.GetInstance(document, ms);
            document.Open();

            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Client ID');", true);
                return;
            }

            if (txtfrom.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select From Date');", true);
                return;
            }

            if (txtto.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select To Date');", true);
                return;
            }

            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }



            DateTime frmdate;
            string FromDate = "";
            string Frmonth = "";
            string FrYear = "";
            string FrMn = "";
            decimal Maleemps = 0;
            decimal FeMaleemps = 0;
            decimal MaleNoofduties = 0;
            decimal FeMaleNoofduties = 0;
            decimal MaleGross = 0;
            decimal FeMaleGross = 0;
            string ClientName = "";
            decimal PF = 0;
            decimal ESI = 0;
            decimal ProfTax = 0;

            if (txtfrom.Text.Trim().Length > 0)
            {
                frmdate = DateTime.ParseExact(txtfrom.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Frmonth = frmdate.ToString("MM");
                FrYear = frmdate.ToString("yy");
                FrMn = frmdate.Month.ToString();
            }



            FromDate = FrYear + Frmonth;


            DateTime tdate;
            string ToDate = "";
            string Tomonth = "";
            string ToYear = "";
            string ToMn = "";
            if (txtto.Text.Trim().Length > 0)
            {
                tdate = DateTime.ParseExact(txtto.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Tomonth = tdate.ToString("MM");
                ToYear = tdate.ToString("yy");
                ToMn = tdate.Month.ToString();

            }

            ToDate = ToYear + Tomonth;


            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@fromMonth", FromDate);
            ht.Add("@Tomonth", ToDate);
            ht.Add("@type", type);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;

            if (dt.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dt.Rows[0]["maleemps"].ToString()) == false)
                {
                    Maleemps = decimal.Parse(dt.Rows[0]["maleemps"].ToString());
                }
                if (String.IsNullOrEmpty(dt.Rows[0]["femaleemps"].ToString()) == false)
                {
                    FeMaleemps = decimal.Parse(dt.Rows[0]["femaleemps"].ToString());
                }

                if (String.IsNullOrEmpty(dt.Rows[0]["malenoofduties"].ToString()) == false)
                {
                    MaleNoofduties = decimal.Parse(dt.Rows[0]["malenoofduties"].ToString());
                }
                if (String.IsNullOrEmpty(dt.Rows[0]["femalenoofduties"].ToString()) == false)
                {
                    FeMaleNoofduties = decimal.Parse(dt.Rows[0]["femalenoofduties"].ToString());
                }

                if (String.IsNullOrEmpty(dt.Rows[0]["malegross"].ToString()) == false)
                {
                    MaleGross = decimal.Parse(dt.Rows[0]["malegross"].ToString());
                }
                if (String.IsNullOrEmpty(dt.Rows[0]["femalegross"].ToString()) == false)
                {
                    FeMaleGross = decimal.Parse(dt.Rows[0]["femalegross"].ToString());
                }
              
                ClientName = (dt.Rows[0]["ClientName"].ToString());
                PF = decimal.Parse(dt.Rows[0]["PF"].ToString());
                ESI = decimal.Parse(dt.Rows[0]["ESI"].ToString());
                ProfTax = decimal.Parse(dt.Rows[0]["ProfTax"].ToString());

                PdfPTable firstTable = new PdfPTable(6);
                firstTable.TotalWidth = 550f;
                firstTable.LockedWidth = true;
                float[] width = new float[] { 0.5f, 2f, 1.5f, 1.5f, 3f, 3f };
                firstTable.SetWidths(width);

                PdfPCell Cell;


                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 6;
                Cell.Border = 0;
                Cell.MinimumHeight = 40;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("FORM XXIV\nThe Contract Labour (Regulation & Abolition) (Karnataka) Rules, 1974:", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 6;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("[See Rule 82 (1)]", FontFactory.GetFont("italic", fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.BorderWidthBottom = 0;
                Cell.BorderWidthLeft = 0.5f;
                Cell.BorderWidthTop = 0;
                Cell.BorderWidthRight = 0.5f;
                Cell.Colspan = 6;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("\nReturn to be sent by the Contractor to Licencing Officer for the Half year ending " + txtto.Text, FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.BorderWidthBottom = 0;
                Cell.BorderWidthLeft = 0.5f;
                Cell.BorderWidthTop = 0;
                Cell.BorderWidthRight = 0.5f;
                Cell.Colspan = 6;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.BorderWidthBottom = 0.5f;
                Cell.BorderWidthLeft = 0.5f;
                Cell.BorderWidthTop = 0;
                Cell.BorderWidthRight = 0.5f;
                Cell.Colspan = 6;
                Cell.MinimumHeight = 5;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("1", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Name and address of the contractor :", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(companyName + "\n" + companyAddress, FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2; Cell.SetLeading(0, 1.3f);
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("2", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Name and address of the Principal Employer:", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(ClientName, FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2; Cell.SetLeading(0, 1.3f);
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("3", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Name and address of the Establishment:", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(ClientName, FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2; Cell.SetLeading(0, 1.3f);
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("4", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Duration of contract:", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                //txtfrom.Text + " to " + txtto.Text
                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("5", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("No of days during the half year on which", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("184", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(a) the establishment of the principal employer had worked", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3; Cell.SetLeading(0, 1.3f);
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("150", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(b) the contractors establishment had worked", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3; Cell.SetLeading(0, 1.3f);
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("184", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("6", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Maximum number of contract labour employed on any day during the half year:", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3; Cell.SetLeading(0, 1.3f);
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Men : " + Maleemps.ToString() + "  Women : " + FeMaleemps.ToString() + "  Children : Nil  Total : " + (Maleemps + FeMaleemps).ToString(), FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("7", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(i) Daily hoursof work and spread over", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3; Cell.SetLeading(0, 1.3f);
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("8hrs & 1 hrs spread over", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(ii) (a) Whether weekly holiday observed and on what day", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3; Cell.SetLeading(0, 1.3f);
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Yes, i.e Substituted Holiday", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(b) If so, whether it was paid for", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Yes", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(iii) Number of man hours of overtime worked", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3; Cell.SetLeading(0, 1.3f);
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Nill", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("8", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Number of man days worked by", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3; Cell.SetLeading(0, 1.3f);
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Men : " + MaleNoofduties.ToString() + "  Women : " + FeMaleNoofduties.ToString() + "  Children : Nil  Total : " + (MaleNoofduties + FeMaleNoofduties).ToString(), FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("9", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Amount of wages paid", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Men : " + MaleGross.ToString() + "  Women : " + FeMaleGross.ToString() + "  Children : Nil\nTotal : " + (MaleGross + FeMaleGross).ToString(), FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Amount of deductions from wages, if any, affected : ", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3; Cell.SetLeading(0, 1.3f);
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("PF : " + PF.ToString() + "  ESI : " + ESI.ToString() + "  PT : " + ProfTax.ToString(), FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Whether the contractor has provided", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(i) Canteen", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Yes", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(ii) Rest Rooms", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Yes", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(iii) Drinking water", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Yes", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(iv) Creches", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Yes", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 1;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("(v) First Aid", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Colspan = 3;
                firstTable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("Yes", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 1;
                Cell.Colspan = 2;
                Cell.MinimumHeight = 20;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, fontsize, Font.BOLD, BaseColor.BLACK)));
                Cell.Border = 0;
                Cell.Colspan = 6;
                Cell.MinimumHeight = 15;
                Cell.HorizontalAlignment = 1;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("Place : Bangalore", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Border = 0;
                Cell.Colspan = 6;
                firstTable.AddCell(Cell);



                Cell = new PdfPCell(new Phrase("Dated :" + DateTime.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 0;
                Cell.Border = 0;
                Cell.Colspan = 4;
                firstTable.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("Signature of the Contractor", FontFactory.GetFont(fontstyle, fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cell.HorizontalAlignment = 2;
                Cell.Border = 0;
                Cell.Colspan = 2;
                firstTable.AddCell(Cell);

                document.Add(firstTable);

                string filename = "FormXXIV.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }

        }

        protected void btnformXVII_Click(object sender, EventArgs e)
        {
            int Fontsize = 10;
            string FontStyle = "calibri";
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();




            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string qry = "select 1 SlNo,ep.empid,EP.Clientid,CN.Description,C.Clientname,(Ed.Empfname + Ed.EmpMname + Ed.emplname) as Empname ,Ed.EmpBankAcNo,Case Ed.EmpSex when 'M' then 'Male' when 'F' then 'Female' when 'T' then 'Transgender' end EmpSex,"
                + " c.ClientName,(c.ClientAddrHno + ' '+ c.ClientAddrStreet+''+ c.ClientAddrColony+''+ c.ClientAddrArea+''+ c.ClientAddrCity+''+ c.ClientAddrState+''+ c.ClientAddrPin+'') as ClientAddress,"
                + "d.design as design,ep.ots,ep.otamt,ep.da,ep.hra,EP.Proftax,ep.TempGross,ep.Bonus,ep.LeaveEncashAmt,ep.Nfhs,ep.DA,ep.pf,ep.esi,ep.noofduties,ep.LeaveEncashAmt,ep.otherallowance ,ep.gross,ep.TotalDeductions,(ep.ActualAmount-ep.OTAmt) as ActualAmount,'0' as Fines,ep.basic from  emppaysheet EP "
                        + "inner join Clients C on  C.Clientid = EP.Clientid"
                        + " inner join  empdetails Ed on Ed.Empid = EP.Empid "
                        + "inner join contracts CN on CN.Clientid = ep.Clientid"
                        + " inner join Designations d on EP.Desgn=d.DesignId  and ep.clientid='" + ddlclientid.SelectedValue + "' and EP.month = '" + month + Year.Substring(2, 2) + " ' order by empid";
            DataTable dtqry = SqlHelper.Instance.GetTableByQuery(qry);


            if (dtqry.Rows.Count > 0)
            {
                string tempDescription = dtqry.Rows[0]["Description"].ToString();
                string ClientName = dtqry.Rows[0]["ClientName"].ToString();
                string ClientAddress = dtqry.Rows[0]["ClientAddress"].ToString();
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL.Rotate());
                // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");
                #region


                PdfPTable tables = new PdfPTable(19);
                tables.TotalWidth = 999f;
                tables.LockedWidth = true;
                float[] widths = new float[] { 1f, 2.5f, 3f, 3f, 1.5f, 1.5f, 2f, 2f, 2f, 2f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 2f, 2f, 2f, 2f };
                tables.SetWidths(widths);

                PdfPCell cellspace = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellspace.HorizontalAlignment = 1;
                cellspace.Colspan = 19;
                cellspace.Border = 0;
                tables.AddCell(cellspace);

                PdfPCell cellform = new PdfPCell(new Phrase("FORM XVII REGISTER OF WAGES", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
                cellform.HorizontalAlignment = 1;
                cellform.Colspan = 19;
                cellform.Border = 0;
                tables.AddCell(cellform);
                tables.AddCell(cellspace);
                tables.AddCell(cellspace);

                PdfPCell cellrule = new PdfPCell(new Phrase("[Vide Rule 78 (1) (a) (i) of Contract Labour (Reg. & Abolition) Central & T.S.Rules]", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellrule.HorizontalAlignment = 1;
                cellrule.Colspan = 19;
                cellrule.Border = 0;
                tables.AddCell(cellrule);


                tables.AddCell(cellspace);
                tables.AddCell(cellspace);

                PdfPCell cellwage = new PdfPCell(new Phrase("Name and Address of Contractor :", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellwage.HorizontalAlignment = 0;
                cellwage.Colspan = 4;
                cellwage.Border = 0;
                tables.AddCell(cellwage);
                PdfPCell cellyearem = new PdfPCell(new Phrase(companyName + "\n" + companyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellyearem.HorizontalAlignment = 0;
                cellyearem.Colspan = 4;
                cellyearem.Border = 0;
                tables.AddCell(cellyearem);


                PdfPCell cellweek = new PdfPCell(new Phrase(" Name and Address of Establishment in /\n under which contract is carried on : ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellweek.HorizontalAlignment = 0;
                cellweek.Colspan = 6;
                cellweek.Border = 0;
                tables.AddCell(cellweek);
                PdfPCell cellweek1 = new PdfPCell(new Phrase(tempDescription, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellweek1.HorizontalAlignment = 0;
                cellweek1.Colspan = 5;
                cellweek1.Border = 0;
                tables.AddCell(cellweek1);

                tables.AddCell(cellspace);


                PdfPCell cellyeare = new PdfPCell(new Phrase("Wage Period : ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellyeare.HorizontalAlignment = 0;
                cellyeare.Colspan = 4;
                cellyeare.Border = 0;
                tables.AddCell(cellyeare);
                PdfPCell cellyeare1 = new PdfPCell(new Phrase(GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellyeare1.HorizontalAlignment = 0;
                cellyeare1.Colspan = 4;
                cellyeare1.Border = 0;
                tables.AddCell(cellyeare1);
                tables.AddCell(cellspace);

                PdfPCell cellname = new PdfPCell(new Phrase("Name and location Work :", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellname.HorizontalAlignment = 0;
                cellname.Colspan = 4;
                cellname.Border = 0;
                tables.AddCell(cellname);
                PdfPCell cellname1 = new PdfPCell(new Phrase("maintenance & Electrical Services", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellname1.HorizontalAlignment = 0;
                cellname1.Colspan = 4;
                cellname1.Border = 0;
                tables.AddCell(cellname1);

                PdfPCell cellnameofemp = new PdfPCell(new Phrase(" Name and address of principal employer :", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellnameofemp.HorizontalAlignment = 0;
                cellnameofemp.Colspan = 6;
                cellnameofemp.Border = 0;
                tables.AddCell(cellnameofemp);
                //ClientName + "\n" +
                PdfPCell cellnameofemp1 = new PdfPCell(new Phrase(ClientAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellnameofemp1.HorizontalAlignment = 0;
                cellnameofemp1.Colspan = 5;
                cellnameofemp1.Border = 0;
                tables.AddCell(cellnameofemp1);
                tables.AddCell(cellspace);
                tables.AddCell(cellspace);
                tables.AddCell(cellspace);
                tables.AddCell(cellspace);
                document.Add(tables);
                PdfPTable table = new PdfPTable(22);
                table.TotalWidth = 999f;
                table.LockedWidth = true;
                table.HeaderRows = 1;
                float[] width = new float[] { 1f, 2.5f, 2f, 3f, 3f, 1.5f, 1.5f, 2f, 2f, 2f, 2f, 2f, 2f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 2f, 2f, 2f, 2f };
                table.SetWidths(width);


                PdfPCell cellsno = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellsno.HorizontalAlignment = 1;
                cellsno.Colspan = 1;
                table.AddCell(cellsno);


                PdfPCell cellsex1 = new PdfPCell(new Phrase("Name of Workmen", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellsex1.HorizontalAlignment = 1;
                cellsex1.Colspan = 2;
                table.AddCell(cellsex1);


                PdfPCell cellnameofwrkman1 = new PdfPCell(new Phrase("Serial No.\nin the\nRegister of\nWorkmen", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellnameofwrkman1.HorizontalAlignment = 1;
                cellnameofwrkman1.Colspan = 1;
                table.AddCell(cellnameofwrkman1);


                PdfPCell celldesign1 = new PdfPCell(new Phrase("Designation/ \n nature of work done", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                celldesign1.HorizontalAlignment = 1;
                celldesign1.Colspan = 1;
                table.AddCell(celldesign1);

                PdfPCell celldaily = new PdfPCell(new Phrase("No of days Worked", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                celldaily.HorizontalAlignment = 1;
                celldaily.Colspan = 1;
                table.AddCell(celldaily);

                PdfPCell cellunits1 = new PdfPCell(new Phrase("Units of work done", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellunits1.HorizontalAlignment = 1;
                cellunits1.Colspan = 1;
                table.AddCell(cellunits1);


                PdfPCell celldailyrate1 = new PdfPCell(new Phrase("Monthly rate of wages \n /piece rate", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                celldailyrate1.HorizontalAlignment = 1;
                celldailyrate1.Colspan = 1;
                table.AddCell(celldailyrate1);

                PdfPCell cellwagespe1 = new PdfPCell(new Phrase("Basic", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellwagespe1.HorizontalAlignment = 1;
                cellwagespe1.Colspan = 1;
                table.AddCell(cellwagespe1);

                PdfPCell cellda1 = new PdfPCell(new Phrase("DA", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellda1.HorizontalAlignment = 1;
                cellda1.Colspan = 1;
                table.AddCell(cellda1);

                PdfPCell totalnoofdays1 = new PdfPCell(new Phrase("HRA", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                totalnoofdays1.HorizontalAlignment = 1;
                totalnoofdays1.Colspan = 1;
                table.AddCell(totalnoofdays1);

                PdfPCell cellover1 = new PdfPCell(new Phrase("Nfhs", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellover1.HorizontalAlignment = 1;
                cellover1.Colspan = 1;
                table.AddCell(cellover1);

                PdfPCell cellover2 = new PdfPCell(new Phrase("Bonus 8.33% on basic", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellover2.HorizontalAlignment = 1;
                cellover2.Colspan = 1;
                table.AddCell(cellover2);
                PdfPCell cellover3 = new PdfPCell(new Phrase("LEAVE & PH", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellover3.HorizontalAlignment = 1;
                cellover3.Colspan = 1;
                table.AddCell(cellover3);

                PdfPCell cellbasic1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellbasic1.HorizontalAlignment = 1;
                cellbasic1.Colspan = 1;
                table.AddCell(cellbasic1);


                PdfPCell cellpf1 = new PdfPCell(new Phrase("PF", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellpf1.HorizontalAlignment = 1;
                cellpf1.Colspan = 1;
                table.AddCell(cellpf1);

                PdfPCell cellesi = new PdfPCell(new Phrase("ESI ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellesi.HorizontalAlignment = 1;
                cellesi.Colspan = 1;
                table.AddCell(cellesi);


                PdfPCell cellOA1 = new PdfPCell(new Phrase("PT", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellOA1.HorizontalAlignment = 1;
                cellOA1.Colspan = 1;
                table.AddCell(cellOA1);

                PdfPCell cellOV1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellOV1.HorizontalAlignment = 0;
                cellOV1.Colspan = 1;
                table.AddCell(cellOV1);

                PdfPCell cellLW1 = new PdfPCell(new Phrase("Net Amount Paid", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellLW1.HorizontalAlignment = 1;
                cellLW1.Colspan = 1;
                table.AddCell(cellLW1);

                PdfPCell cellgross1 = new PdfPCell(new Phrase("Signature/\nThumb \nImpression \nof workman", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellgross1.HorizontalAlignment = 0;
                cellgross1.Colspan = 1;
                table.AddCell(cellgross1);



                PdfPCell celloDEd1 = new PdfPCell(new Phrase("Initial of \ncontractor\n of his \nrepresentitive", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                celloDEd1.HorizontalAlignment = 0;
                celloDEd1.Colspan = 1;
                table.AddCell(celloDEd1);





                #region for nos heading

                //PdfPCell cellsno2 = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellsno2.HorizontalAlignment = 1;
                //cellsno2.Colspan = 1;
                //table.AddCell(cellsno2);


                //PdfPCell cellnameofwrkman2 = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellnameofwrkman2.HorizontalAlignment = 1;
                //cellnameofwrkman2.Colspan = 2;
                //table.AddCell(cellnameofwrkman2);

                //PdfPCell cellsex2 = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellsex2.HorizontalAlignment = 1;
                //cellsex2.Colspan = 1;
                //table.AddCell(cellsex2);

                //PdfPCell celldesign2 = new PdfPCell(new Phrase("4", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //celldesign2.HorizontalAlignment = 1;
                //celldesign2.Colspan = 1;
                //table.AddCell(celldesign2);

                //PdfPCell celldaily2 = new PdfPCell(new Phrase("5", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //celldaily2.HorizontalAlignment = 1;
                //celldaily2.Colspan = 1;
                //table.AddCell(celldaily2);

                //PdfPCell cellwagespe2 = new PdfPCell(new Phrase("6", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellwagespe2.HorizontalAlignment = 1;
                //cellwagespe2.Colspan = 1;
                //table.AddCell(cellwagespe2);

                //PdfPCell totalnoofdays2 = new PdfPCell(new Phrase("7", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //totalnoofdays2.HorizontalAlignment = 1;
                //totalnoofdays2.Colspan = 1;
                //table.AddCell(totalnoofdays2);

                //PdfPCell cellunits2 = new PdfPCell(new Phrase("8", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellunits2.HorizontalAlignment = 1;
                //cellunits2.Colspan = 1;
                //table.AddCell(cellunits2);

                //PdfPCell celldailyrate2 = new PdfPCell(new Phrase("9", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //celldailyrate2.HorizontalAlignment = 1;
                //celldailyrate2.Colspan = 1;
                //table.AddCell(celldailyrate2);

                //PdfPCell cellovers2 = new PdfPCell(new Phrase("10", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellovers2.HorizontalAlignment = 1;
                //cellovers2.Colspan = 1;
                //table.AddCell(cellovers2);

                //PdfPCell cellbasic2 = new PdfPCell(new Phrase("11", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellbasic2.HorizontalAlignment = 1;
                //cellbasic2.Colspan = 1;
                //table.AddCell(cellbasic2);
                //PdfPCell cellbasic3 = new PdfPCell(new Phrase("12", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellbasic3.HorizontalAlignment = 1;
                //cellbasic3.Colspan = 1;
                //table.AddCell(cellbasic3);

                //PdfPCell cellbasic4 = new PdfPCell(new Phrase("13", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellbasic4.HorizontalAlignment = 1;
                //cellbasic4.Colspan = 1;
                //table.AddCell(cellbasic4);

                //PdfPCell cellda2 = new PdfPCell(new Phrase("14", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellda2.HorizontalAlignment = 1;
                //cellda2.Colspan = 1;
                //table.AddCell(cellda2);

                //PdfPCell cellOA2 = new PdfPCell(new Phrase("15", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellOA2.HorizontalAlignment = 1;
                //cellOA2.Colspan = 1;
                //table.AddCell(cellOA2);

                //PdfPCell cellOV2 = new PdfPCell(new Phrase("16", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellOV2.HorizontalAlignment = 1;
                //cellOV2.Colspan = 1;
                //table.AddCell(cellOV2);

                //PdfPCell cellLW2 = new PdfPCell(new Phrase("17", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellLW2.HorizontalAlignment = 1;
                //cellLW2.Colspan = 2;
                //table.AddCell(cellLW2);

                //PdfPCell cellgross2 = new PdfPCell(new Phrase("18", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellgross2.HorizontalAlignment = 1;
                //cellgross2.Colspan = 1;
                //table.AddCell(cellgross2);

                //PdfPCell cellgross3 = new PdfPCell(new Phrase("19", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                //cellgross3.HorizontalAlignment = 1;
                //cellgross3.Colspan = 1;
                //table.AddCell(cellgross3);
                #endregion

                int k = 1;

                for (int i = 0; i < dtqry.Rows.Count; i++)
                {
                    string empname = dtqry.Rows[i]["Empname"].ToString();
                    string empdesign = dtqry.Rows[i]["design"].ToString();
                    string empid = dtqry.Rows[i]["empid"].ToString();
                    string OTs = dtqry.Rows[i]["ots"].ToString();
                    string OTsamt = dtqry.Rows[i]["otamt"].ToString();
                    string gross = dtqry.Rows[i]["gross"].ToString();
                    string pf = dtqry.Rows[i]["pf"].ToString();
                    string esi = dtqry.Rows[i]["esi"].ToString();
                    string PT = dtqry.Rows[i]["Proftax"].ToString();
                    string empsex = dtqry.Rows[i]["EmpSex"].ToString();
                    string basic = dtqry.Rows[i]["basic"].ToString();
                    string fines = dtqry.Rows[i]["Fines"].ToString();
                    string noofduties = dtqry.Rows[i]["noofduties"].ToString();
                    string totalded = dtqry.Rows[i]["TotalDeductions"].ToString();
                    string actualamt = dtqry.Rows[i]["ActualAmount"].ToString();
                    string tempgross = dtqry.Rows[i]["TempGross"].ToString();
                    string DA = dtqry.Rows[i]["DA"].ToString();
                    string Nfhs = dtqry.Rows[i]["Nfhs"].ToString();
                    string LeaveEncashAmt = dtqry.Rows[i]["LeaveEncashAmt"].ToString();
                    string Bonus = dtqry.Rows[i]["Bonus"].ToString();
                    // string otherallowance = dtqry.Rows[i]["otherallowance"].ToString();ep. 

                    float hra = Convert.ToSingle(dtqry.Rows[i]["hra"].ToString());
                    float otherallowance = Convert.ToSingle(dtqry.Rows[i]["otherallowance"].ToString());

                    string SlNo = dtqry.Rows[i]["SlNo"].ToString();
                    string EmpBankAcNo = dtqry.Rows[i]["EmpBankAcNo"].ToString();


                    //values

                    PdfPCell cellsno3 = new PdfPCell(new Phrase(k.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellsno3.HorizontalAlignment = 1;
                    cellsno3.Colspan = 1;
                    table.AddCell(cellsno3);
                    PdfPCell cellsex3 = new PdfPCell(new Phrase(empname, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellsex3.HorizontalAlignment = 0;
                    cellsex3.Colspan = 2;
                    table.AddCell(cellsex3);
                    PdfPCell cellnameofwrkman3 = new PdfPCell(new Phrase(SlNo, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellnameofwrkman3.HorizontalAlignment = 0;
                    cellnameofwrkman3.Colspan = 1;
                    table.AddCell(cellnameofwrkman3);
                    PdfPCell celldesign3 = new PdfPCell(new Phrase(empdesign, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    celldesign3.HorizontalAlignment = 0;
                    celldesign3.Colspan = 1;
                    table.AddCell(celldesign3);
                    PdfPCell celldaily3 = new PdfPCell(new Phrase(noofduties, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    celldaily3.HorizontalAlignment = 1;
                    celldaily3.Colspan = 1;
                    table.AddCell(celldaily3);
                    PdfPCell cellwagespe3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellwagespe3.HorizontalAlignment = 0;
                    cellwagespe3.Colspan = 1;
                    table.AddCell(cellwagespe3);
                    PdfPCell totalnoofdays3 = new PdfPCell(new Phrase(tempgross, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    totalnoofdays3.HorizontalAlignment = 2;
                    totalnoofdays3.Colspan = 1;
                    table.AddCell(totalnoofdays3);
                    PdfPCell cellunits3 = new PdfPCell(new Phrase(basic, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellunits3.HorizontalAlignment = 2;
                    cellunits3.Colspan = 1;
                    table.AddCell(cellunits3);
                    PdfPCell celldailyrate3 = new PdfPCell(new Phrase(DA, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    celldailyrate3.HorizontalAlignment = 2;
                    celldailyrate3.Colspan = 1;
                    table.AddCell(celldailyrate3);
                    PdfPCell cellovers3 = new PdfPCell(new Phrase((hra + otherallowance).ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellovers3.HorizontalAlignment = 2;
                    cellovers3.Colspan = 1;
                    table.AddCell(cellovers3);
                    PdfPCell cellbasics3 = new PdfPCell(new Phrase(Nfhs, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellbasics3.HorizontalAlignment = 2;
                    cellbasics3.Colspan = 1;
                    table.AddCell(cellbasics3);
                    PdfPCell cellbonous = new PdfPCell(new Phrase(Bonus, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellbonous.HorizontalAlignment = 2;
                    cellbonous.Colspan = 1;
                    table.AddCell(cellbonous);
                    PdfPCell celllhc = new PdfPCell(new Phrase(LeaveEncashAmt, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    celllhc.HorizontalAlignment = 2;
                    celllhc.Colspan = 1;
                    table.AddCell(celllhc);
                    PdfPCell cellda3 = new PdfPCell(new Phrase(gross, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellda3.HorizontalAlignment = 2;
                    cellda3.Colspan = 1;
                    table.AddCell(cellda3);
                    PdfPCell cellOA3 = new PdfPCell(new Phrase(pf, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellOA3.HorizontalAlignment = 2;
                    cellOA3.Colspan = 1;
                    table.AddCell(cellOA3);
                    PdfPCell cellOV3 = new PdfPCell(new Phrase(esi, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellOV3.HorizontalAlignment = 2;
                    cellOV3.Colspan = 1;
                    table.AddCell(cellOV3);
                    PdfPCell cellLW3 = new PdfPCell(new Phrase(PT, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellLW3.HorizontalAlignment = 2;
                    cellLW3.Colspan = 1;
                    table.AddCell(cellLW3);
                    PdfPCell cellgross31 = new PdfPCell(new Phrase(totalded, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellgross31.HorizontalAlignment = 2;
                    cellgross31.Colspan = 1;
                    table.AddCell(cellgross31);
                    PdfPCell cellpf3 = new PdfPCell(new Phrase(actualamt, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellpf3.HorizontalAlignment = 2;
                    cellpf3.Colspan = 1;
                    table.AddCell(cellpf3);
                    if (EmpBankAcNo.Length > 0)
                    {
                        PdfPCell cellesi3 = new PdfPCell(new Phrase(EmpBankAcNo, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellesi3.HorizontalAlignment = 0;
                        cellesi3.Colspan = 2;
                        table.AddCell(cellesi3);
                    }
                    else
                    {
                        PdfPCell cellesi3 = new PdfPCell(new Phrase("Cheque", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellesi3.HorizontalAlignment = 1;
                        cellesi3.Colspan = 2;
                        table.AddCell(cellesi3);
                    }
                    PdfPCell celloDEd3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    celloDEd3.HorizontalAlignment = 2;
                    celloDEd3.Colspan = 1;
                    //table.AddCell(celloDEd3);
                    k++;
                }
                document.Add(table);


                document.NewPage();

                #endregion


                string filename = ddlcname.SelectedItem.Text + "FormXVII.pdf";

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

        protected void btnformsXIV_Click(object sender, EventArgs e)
        {
            string FontStyle = "Verdana";

            int Fontsize = 10;

            if (ddlclientid.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert()", "alert('Please Select Client ID/Name')", true);
                return;
            }

            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string addressData = "";
            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString();
            int slipsCount = 0;

            string selectmonth = string.Empty;
            var list = new List<string>();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkempid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblempid = GVListEmployees.Rows[i].FindControl("lblempid") as Label;

                    if (chkempid.Checked == true)
                    {
                        list.Add(lblempid.Text);
                    }
                }
            }

            if (list.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select atleast one employee');", true);
                return;
            }


            DataTable dtEmpids = new DataTable();
            dtEmpids.Columns.Add("Empid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtEmpids.NewRow();
                    row["Empid"] = s;
                    dtEmpids.Rows.Add(row);
                }
            }

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@month", month + Year.Substring(2, 2));
            ht.Add("@monthval", month);
            ht.Add("@yearval", Year);
            ht.Add("@type", type);
            ht.Add("@EmpIds", dtEmpids);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;

            MemoryStream ms = new MemoryStream();
            if (dt.Rows.Count > 0)
            {
                Document document = new Document(PageSize.LEGAL);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                #region
                string empname = "";
                string typeofwork = "";
                string tempDescription = "";
                int SNo = 1;
                tempDescription = dt.Rows[0]["Description"].ToString();


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    float Wagerate = 0;
                    if (dt.Rows[i]["tempgross"].ToString().Trim().Length > 0)
                        Wagerate = Convert.ToSingle(dt.Rows[i]["tempgross"].ToString());

                    empname = dt.Rows[i]["EmpmName"].ToString();
                    typeofwork = dt.Rows[i]["typeofwork"].ToString();
                    companyAddress = dt.Rows[0]["companyaddress"].ToString();
                    companyName = dt.Rows[0]["companyname"].ToString();
                    addressData = dt.Rows[0]["clientaddress"].ToString();

                    PdfPTable tablecall23 = new PdfPTable(9);
                    tablecall23.TotalWidth = 500f;
                    tablecall23.LockedWidth = true;
                    float[] widthx = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
                    tablecall23.SetWidths(widthx);

                    PdfPCell cellheadspace = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize + 4, Font.BOLD, BaseColor.BLACK)));
                    cellheadspace.HorizontalAlignment = 1;
                    cellheadspace.Colspan = 9;
                    cellheadspace.Border = 0;
                    cellheadspace.PaddingTop = 60;
                    tablecall23.AddCell(cellheadspace);

                    PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellspace.Colspan = 9;
                    cellspace.Border = 0;
                    cellspace.PaddingTop = -5;


                    PdfPCell cellHead = new PdfPCell(new Phrase("Form-XIV ", FontFactory.GetFont(FontStyle, Fontsize + 4, Font.BOLD, BaseColor.BLACK)));
                    cellHead.HorizontalAlignment = 1;
                    cellHead.Colspan = 9;
                    cellHead.Border = 0;
                    tablecall23.AddCell(cellHead);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);

                    PdfPCell cellSERVICE = new PdfPCell(new Phrase("EMPLOYMENT CARD", FontFactory.GetFont(FontStyle, 12, Font.BOLD, BaseColor.BLACK)));
                    cellSERVICE.HorizontalAlignment = 1;
                    cellSERVICE.Colspan = 9;
                    cellSERVICE.Border = 0;
                    tablecall23.AddCell(cellSERVICE);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);


                    PdfPCell cellrule = new PdfPCell(new Phrase("[Vide rule 76 of Under Contract Labour Act, 1971]", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellrule.HorizontalAlignment = 1;
                    cellrule.Colspan = 9;
                    cellrule.Border = 0;
                    tablecall23.AddCell(cellrule);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);
                    document.Add(tablecall23);

                    PdfPTable tble = new PdfPTable(8);
                    tble.TotalWidth = 500f;
                    tble.LockedWidth = true;
                    float[] width = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
                    tble.SetWidths(width);



                    PdfPCell celladdressofcon = new PdfPCell(new Phrase("\nName and Address of contractor", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    celladdressofcon.HorizontalAlignment = 0;
                    celladdressofcon.Colspan = 4;
                    celladdressofcon.MinimumHeight = 30;
                    //celladdressofcon.Border = 0;
                    tble.AddCell(celladdressofcon);

                    PdfPCell cellestablishment = new PdfPCell(new Phrase("Name & Address of the Est. to/under which contract is carried on", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellestablishment.HorizontalAlignment = 0;
                    cellestablishment.Colspan = 4;
                    //cellestablishment.Border = 0;
                    tble.AddCell(cellestablishment);

                    PdfPCell cellRule741 = new PdfPCell(new Phrase(companyName + "\n" + companyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellRule741.HorizontalAlignment = 0;
                    cellRule741.Colspan = 4;
                    tble.AddCell(cellRule741);
                    PdfPCell cellspaces1 = new PdfPCell(new Phrase(addressData, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellspaces1.HorizontalAlignment = 0;
                    cellspaces1.Colspan = 4;
                    //cellspaces1.Border = 0;
                    tble.AddCell(cellspaces1);

                    PdfPCell cellRule1 = new PdfPCell(new Phrase("\nNature and location of work", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellRule1.HorizontalAlignment = 0;
                    cellRule1.Colspan = 4;
                    cellRule1.MinimumHeight = 30;
                    //cellRule1.Border = 0;
                    tble.AddCell(cellRule1);
                    PdfPCell cellempty15 = new PdfPCell(new Phrase("Security\n\n" + addressData, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty15.HorizontalAlignment = 0;
                    cellempty15.Colspan = 4;
                    tble.AddCell(cellempty15);


                    PdfPCell cellempname = new PdfPCell(new Phrase("\n1. Name and address of the workman", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempname.HorizontalAlignment = 0;
                    cellempname.Colspan = 4;
                    cellempname.MinimumHeight = 30;
                    tble.AddCell(cellempname);
                    PdfPCell cellempname1 = new PdfPCell(new Phrase("\n" + empname, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempname1.HorizontalAlignment = 1;
                    cellempname1.Colspan = 4;
                    tble.AddCell(cellempname1);



                    PdfPCell cellempty13 = new PdfPCell(new Phrase("\n2. S.No. in the Register of Workmen", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempty13.HorizontalAlignment = 0;
                    cellempty13.Colspan = 4;
                    cellempty13.MinimumHeight = 30;
                    tble.AddCell(cellempty13);
                    PdfPCell cellempty13s = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty13s.HorizontalAlignment = 1;
                    cellempty13s.Colspan = 4;
                    tble.AddCell(cellempty13s);

                    PdfPCell cellempty12 = new PdfPCell(new Phrase("\n3. Nature of Employment /Designation", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempty12.HorizontalAlignment = 0;
                    cellempty12.Colspan = 4;
                    cellempty12.MinimumHeight = 30;
                    tble.AddCell(cellempty12);
                    PdfPCell cellempty12S = new PdfPCell(new Phrase("\n" + dt.Rows[i]["Desgn"].ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty12S.HorizontalAlignment = 1;
                    cellempty12S.Colspan = 4;
                    tble.AddCell(cellempty12S);


                    PdfPCell cellempty11 = new PdfPCell(new Phrase("\n4. Date of entry into service", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempty11.HorizontalAlignment = 0;
                    cellempty11.Colspan = 4;
                    cellempty11.MinimumHeight = 30;
                    tble.AddCell(cellempty11);
                    PdfPCell cellempty11s = new PdfPCell(new Phrase("\n" + dt.Rows[i]["EmpDtofJoining"].ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty11s.HorizontalAlignment = 1;
                    cellempty11s.Colspan = 4;
                    tble.AddCell(cellempty11s);

                    PdfPCell cellempty9 = new PdfPCell(new Phrase("\n5. Wage rate (With particulars of unit in \ncase of piece - work)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempty9.HorizontalAlignment = 0;
                    cellempty9.Colspan = 4;
                    cellempty9.MinimumHeight = 30;
                    tble.AddCell(cellempty9);
                    PdfPCell cellempty8 = new PdfPCell(new Phrase("\n" + "Rs." + Wagerate.ToString("#,##00") + "/" + "-", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty8.HorizontalAlignment = 1;
                    cellempty8.Colspan = 4;
                    tble.AddCell(cellempty8);



                    PdfPCell cellformSlNo1 = new PdfPCell(new Phrase("\n6. Wage period", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellformSlNo1.HorizontalAlignment = 0;
                    cellformSlNo1.Colspan = 4;
                    cellformSlNo1.MinimumHeight = 30;
                    tble.AddCell(cellformSlNo1);
                    PdfPCell cellformfrom = new PdfPCell(new Phrase("\n" + "Monthly", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellformfrom.HorizontalAlignment = 1;
                    cellformfrom.Colspan = 4;
                    tble.AddCell(cellformfrom);

                    PdfPCell cellformto = new PdfPCell(new Phrase("\n7. Tenure of Employment", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellformto.HorizontalAlignment = 0;
                    cellformto.Colspan = 4;
                    cellformto.MinimumHeight = 30;
                    tble.AddCell(cellformto);
                    PdfPCell cellformnature = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellformnature.HorizontalAlignment = 1;
                    cellformnature.Colspan = 4;
                    tble.AddCell(cellformnature);

                    PdfPCell cellSerial1 = new PdfPCell(new Phrase("\n8. Remark ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellSerial1.HorizontalAlignment = 0;
                    cellSerial1.Colspan = 4;
                    cellSerial1.MinimumHeight = 30;
                    tble.AddCell(cellSerial1);
                    PdfPCell cellform1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellform1.HorizontalAlignment = 1;
                    cellform1.Colspan = 4;
                    cellform1.PaddingTop = 7;

                    tble.AddCell(cellform1);


                    PdfPCell cellsign = new PdfPCell(new Phrase("Signture of the Employee", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                    cellsign.HorizontalAlignment = 0;
                    cellsign.Colspan = 4;
                    cellsign.Border = 0;
                    cellsign.PaddingTop = 60;
                    tble.AddCell(cellsign);


                    cellsign = new PdfPCell(new Phrase("Signture of the Contractor", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                    cellsign.HorizontalAlignment = 2;
                    cellsign.Colspan = 4;
                    cellsign.Border = 0;
                    cellsign.PaddingTop = 60;
                    tble.AddCell(cellsign);


                    document.Add(tble);

                    slipsCount++;
                    if (slipsCount == 1)
                    {
                        slipsCount = 0;
                        document.NewPage();
                    }
                }

                #endregion Basic Information of the Employee

                // document.NewPage();

                string filename = "FormXIV.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }
        }

        protected void BtnformXV_Click(object sender, EventArgs e)
        {
            string FontStyle = "Verdana";

            int Fontsize = 10;

            if (ddlclientid.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert()", "alert('Please Select Client ID/Name')", true);
                return;
            }



            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string addressData = "";
            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString().Substring(2, 2);
            int slipsCount = 0;

            string selectmonth = string.Empty;

            var list = new List<string>();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkempid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblempid = GVListEmployees.Rows[i].FindControl("lblempid") as Label;

                    if (chkempid.Checked == true)
                    {
                        list.Add(lblempid.Text);
                    }
                }
            }

            if (list.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select atleast one employee');", true);
                return;
            }


            DataTable dtEmpids = new DataTable();
            dtEmpids.Columns.Add("Empid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtEmpids.NewRow();
                    row["Empid"] = s;
                    dtEmpids.Rows.Add(row);
                }
            }

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@month", month + Year);
            ht.Add("@type", type);
            ht.Add("@EmpIds", dtEmpids);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;

            MemoryStream ms = new MemoryStream();

            if (dt.Rows.Count > 0)
            {
                Document document = new Document(PageSize.LEGAL);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                string tempDescription = dt.Rows[0]["Description"].ToString();
                string ClientName = dt.Rows[0]["ClientName"].ToString();
                string ClientAddress = dt.Rows[0]["ClientAddress"].ToString();
                string typeofofwork = dt.Rows[0]["typeofwork"].ToString();
                companyName = dt.Rows[0]["companyname"].ToString();
                companyAddress = dt.Rows[0]["companyaddress"].ToString();

                #region
                int k = 1;
                string doj = "";
                string dol = "";
                string empname = "";
                string desgn = "";
                string fathername = "";
                string idmarks1 = "";
                string idmarks2 = "";
                string dob = "";
                string TempGross = "";
                string empstatus = "";
                string remarks = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    doj = dt.Rows[i]["EmpDtofJoining"].ToString();
                    empstatus = dt.Rows[i]["EmpStatus"].ToString();
                    if (empstatus == "False")
                    {
                        dol = dt.Rows[i]["EmpDtofLeaving"].ToString();
                    }
                    else
                    {
                        dol = "";
                    }
                    if (empstatus == "False")
                    {
                        remarks = "Resigned";
                    }
                    else
                    {
                        remarks = "";
                    }
                    empname = dt.Rows[i]["EmpmName"].ToString();
                    desgn = dt.Rows[i]["Desgn"].ToString();
                    dob = dt.Rows[i]["EmpDtofBirth"].ToString();
                    fathername = dt.Rows[i]["EmpFatherName"].ToString();
                    idmarks1 = dt.Rows[i]["EmpIdMark1"].ToString();
                    idmarks2 = dt.Rows[i]["EmpIdMark2"].ToString();
                    TempGross = dt.Rows[i]["TempGross"].ToString();

                    PdfPTable tablecall23 = new PdfPTable(9);
                    tablecall23.TotalWidth = 500f;
                    tablecall23.LockedWidth = true;
                    float[] widthx = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
                    tablecall23.SetWidths(widthx);

                    PdfPCell cellheadspace = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize + 4, Font.BOLD, BaseColor.BLACK)));
                    cellheadspace.HorizontalAlignment = 1;
                    cellheadspace.Colspan = 9;
                    cellheadspace.Border = 0;
                    cellheadspace.PaddingTop = 60;
                    tablecall23.AddCell(cellheadspace);

                    PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellspace.Colspan = 9;
                    cellspace.Border = 0;
                    cellspace.PaddingTop = -5;


                    PdfPCell cellHead = new PdfPCell(new Phrase("Form-XV ", FontFactory.GetFont(FontStyle, Fontsize + 4, Font.BOLD, BaseColor.BLACK)));
                    cellHead.HorizontalAlignment = 1;
                    cellHead.Colspan = 9;
                    cellHead.Border = 0;
                    tablecall23.AddCell(cellHead);

                    PdfPCell cellrule = new PdfPCell(new Phrase("(See rule 77)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellrule.HorizontalAlignment = 1;
                    cellrule.Colspan = 9;
                    cellrule.Border = 0;
                    tablecall23.AddCell(cellrule);
                    tablecall23.AddCell(cellspace);

                    PdfPCell cellSERVICE = new PdfPCell(new Phrase("Service Certificate", FontFactory.GetFont(FontStyle, 12, Font.BOLDITALIC, BaseColor.BLACK)));
                    cellSERVICE.HorizontalAlignment = 1;
                    cellSERVICE.Colspan = 9;
                    cellSERVICE.Border = 0;
                    tablecall23.AddCell(cellSERVICE);



                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);

                    PdfPCell celladdressofcon = new PdfPCell(new Phrase("Name and address of contractor", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    celladdressofcon.HorizontalAlignment = 0;
                    celladdressofcon.Colspan = 4;
                    celladdressofcon.Border = 0;
                    tablecall23.AddCell(celladdressofcon);
                    PdfPCell cellspaces = new PdfPCell(new Phrase(": " + companyName + "," + companyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellspaces.HorizontalAlignment = 0;
                    cellspaces.Colspan = 5;
                    cellspaces.Border = 0;
                    tablecall23.AddCell(cellspaces);

                    PdfPCell cellestablishment = new PdfPCell(new Phrase("Name and address of establishment in/under which contract is carried on", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellestablishment.HorizontalAlignment = 0;
                    cellestablishment.Colspan = 4;
                    cellestablishment.Border = 0;
                    tablecall23.AddCell(cellestablishment);
                    PdfPCell cellRule741 = new PdfPCell(new Phrase(": " + ClientName, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellRule741.HorizontalAlignment = 0;
                    cellRule741.Colspan = 5;
                    cellRule741.Border = 0;
                    tablecall23.AddCell(cellRule741);




                    PdfPCell cellRule1 = new PdfPCell(new Phrase("Nature and location of work", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellRule1.HorizontalAlignment = 0;
                    cellRule1.Colspan = 4;
                    cellRule1.Border = 0;
                    tablecall23.AddCell(cellRule1);
                    PdfPCell cellspaces2 = new PdfPCell(new Phrase(": " + typeofofwork, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellspaces2.HorizontalAlignment = 0;
                    cellspaces2.Colspan = 5;
                    cellspaces2.Border = 0;
                    tablecall23.AddCell(cellspaces2);

                    PdfPCell cellempty14 = new PdfPCell(new Phrase("Name and adress of principal employer", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty14.HorizontalAlignment = 0;
                    cellempty14.Colspan = 4;
                    cellempty14.Border = 0;
                    tablecall23.AddCell(cellempty14);
                    PdfPCell cellempty14s = new PdfPCell(new Phrase(": " + ClientAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempty14s.HorizontalAlignment = 0;
                    cellempty14s.Colspan = 5;
                    cellempty14s.Border = 0;
                    tablecall23.AddCell(cellempty14s);

                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);

                    PdfPCell cellempname = new PdfPCell(new Phrase("Name and address of the workman", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempname.HorizontalAlignment = 0;
                    cellempname.Colspan = 4;
                    cellempname.Border = 0;
                    tablecall23.AddCell(cellempname);
                    PdfPCell cellempname1 = new PdfPCell(new Phrase(": " + empname, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempname1.HorizontalAlignment = 0;
                    cellempname1.Colspan = 5;
                    cellempname1.Border = 0;
                    tablecall23.AddCell(cellempname1);

                    tablecall23.AddCell(cellspace);

                    PdfPCell cellempty13 = new PdfPCell(new Phrase("Age or Date of Birth", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty13.HorizontalAlignment = 0;
                    cellempty13.Colspan = 4;
                    cellempty13.Border = 0;
                    tablecall23.AddCell(cellempty13);
                    PdfPCell cellempty13s = new PdfPCell(new Phrase(": " + dob, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempty13s.HorizontalAlignment = 0;
                    cellempty13s.Colspan = 5;
                    cellempty13s.Border = 0;
                    tablecall23.AddCell(cellempty13s);

                    tablecall23.AddCell(cellspace);

                    //PdfPCell cellempty12 = new PdfPCell(new Phrase("Identification Marks", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    //cellempty12.HorizontalAlignment = 0;
                    //cellempty12.Colspan = 4;
                    //cellempty12.Border = 0;
                    //tablecall23.AddCell(cellempty12);
                    //PdfPCell cellempty12S = new PdfPCell(new Phrase(": " + idmarks1 + "\n" + idmarks2, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    //cellempty12S.HorizontalAlignment = 0;
                    //cellempty12S.Colspan = 5;
                    //cellempty12S.Border = 0;
                    //tablecall23.AddCell(cellempty12S);

                    tablecall23.AddCell(cellspace);
                    PdfPCell cellempty11 = new PdfPCell(new Phrase("Father's/Husband's name", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty11.HorizontalAlignment = 0;
                    cellempty11.Colspan = 4;
                    cellempty11.Border = 0;
                    tablecall23.AddCell(cellempty11);
                    PdfPCell cellempty11s = new PdfPCell(new Phrase(": " + fathername, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellempty11s.HorizontalAlignment = 0;
                    cellempty11s.Colspan = 5;
                    cellempty11s.Border = 0;
                    tablecall23.AddCell(cellempty11s);

                    tablecall23.AddCell(cellspace);
                    tablecall23.AddCell(cellspace);
                    document.Add(tablecall23);

                    PdfPTable tablecall2 = new PdfPTable(6);
                    tablecall2.TotalWidth = 500f;
                    tablecall2.LockedWidth = true;
                    float[] width = new float[] { 1.5f, 2.2f, 2f, 3f, 3.8f, 2f, };
                    tablecall2.SetWidths(width);

                    PdfPCell cellempty9 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
                    cellempty9.HorizontalAlignment = 0;
                    cellempty9.Colspan = 1;
                    tablecall2.AddCell(cellempty9);
                    PdfPCell cellempty8 = new PdfPCell(new Phrase("Total period for work employed", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty8.HorizontalAlignment = 1;
                    cellempty8.Colspan = 2;
                    tablecall2.AddCell(cellempty8);
                    PdfPCell cellempty7 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempty7.HorizontalAlignment = 0;
                    cellempty7.Colspan = 3;
                    tablecall2.AddCell(cellempty7);



                    PdfPCell cellformSlNo1 = new PdfPCell(new Phrase("Sl.No. ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellformSlNo1.HorizontalAlignment = 1;
                    cellformSlNo1.Colspan = 0;
                    tablecall2.AddCell(cellformSlNo1);
                    PdfPCell cellformfrom = new PdfPCell(new Phrase("From", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellformfrom.HorizontalAlignment = 1;
                    cellformfrom.Colspan = 0;
                    tablecall2.AddCell(cellformfrom);
                    PdfPCell cellformto = new PdfPCell(new Phrase("To ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellformto.HorizontalAlignment = 1;
                    cellformto.Colspan = 0;
                    tablecall2.AddCell(cellformto);
                    PdfPCell cellformnature = new PdfPCell(new Phrase("Nature of work done", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellformnature.HorizontalAlignment = 1;
                    cellformnature.Colspan = 0;
                    tablecall2.AddCell(cellformnature);
                    PdfPCell cellformRate = new PdfPCell(new Phrase("Rate of wages(with particulars of unit in caseof piece-work)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellformRate.HorizontalAlignment = 1;
                    cellformRate.Colspan = 0;
                    tablecall2.AddCell(cellformRate);
                    PdfPCell cellSerial1 = new PdfPCell(new Phrase("Remark ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSerial1.HorizontalAlignment = 1;
                    cellSerial1.Colspan = 0;
                    tablecall2.AddCell(cellSerial1);

                    PdfPCell cellform1 = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellform1.HorizontalAlignment = 1;
                    cellform1.Colspan = 0;
                    cellform1.PaddingBottom = 10f;
                    tablecall2.AddCell(cellform1);
                    PdfPCell cellform2 = new PdfPCell(new Phrase(doj, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellform2.HorizontalAlignment = 1;
                    cellform2.Colspan = 0;
                    cellform2.PaddingBottom = 10f;
                    tablecall2.AddCell(cellform2);
                    PdfPCell cellform3 = new PdfPCell(new Phrase(dol, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellform3.HorizontalAlignment = 1;
                    cellform3.Colspan = 0;
                    cellform3.PaddingBottom = 10f;
                    tablecall2.AddCell(cellform3);
                    PdfPCell cellform4 = new PdfPCell(new Phrase(desgn, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellform4.HorizontalAlignment = 1;
                    cellform4.Colspan = 0;
                    cellform4.PaddingBottom = 10f;
                    tablecall2.AddCell(cellform4);
                    PdfPCell cellform5 = new PdfPCell(new Phrase(TempGross, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellform5.HorizontalAlignment = 1;
                    cellform5.Colspan = 0;
                    cellform5.PaddingBottom = 10f;
                    tablecall2.AddCell(cellform5);
                    PdfPCell cellform6 = new PdfPCell(new Phrase(remarks, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellform6.HorizontalAlignment = 1;
                    cellform6.Colspan = 0;
                    cellform6.PaddingBottom = 10f;
                    tablecall2.AddCell(cellform6);

                    PdfPCell cellsingn = new PdfPCell(new Phrase("singature of contractor", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellsingn.HorizontalAlignment = 2;
                    cellsingn.Colspan = 6;
                    cellsingn.Border = 0;
                    cellsingn.PaddingTop = 50;
                    tablecall2.AddCell(cellsingn);
                    k++;

                    document.Add(tablecall2);

                    slipsCount++;
                    if (slipsCount == 1)
                    {
                        slipsCount = 0;
                        document.NewPage();
                    }
                }

                #endregion Basic Information of the Employee


                string filename = "FormXV.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }
        }

        protected void btnFormQ_Click(object sender, EventArgs e)
        {

            string fontsyle = "Verdana";

            int Fontsize = 10;

            if (ddlclientid.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert()", "alert('Please Select Client ID/Name')", true);
                return;
            }



            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string addressData = "";

            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString();
            int slipsCount = 0;

            string selectmonth = string.Empty;

            var list = new List<string>();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkempid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblempid = GVListEmployees.Rows[i].FindControl("lblempid") as Label;

                    if (chkempid.Checked == true)
                    {
                        list.Add(lblempid.Text);
                    }
                }
            }

            if (list.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select atleast one employee');", true);
                return;
            }


            DataTable dtEmpids = new DataTable();
            dtEmpids.Columns.Add("Empid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtEmpids.NewRow();
                    row["Empid"] = s;
                    dtEmpids.Rows.Add(row);
                }
            }

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@type", type);
            ht.Add("@EmpIds", dtEmpids);
            ht.Add("@month", month + Year.Substring(2, 2));
            ht.Add("@monthval", month);
            ht.Add("@yearval", Year);
            DataTable dtEmpdetails = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;



            string Empid = "";
            string EmpName = "";
            string PostalAddress = "";
            string PermanentAdd = "";
            string FatherName = "";
            string DateOfBirth = "";
            string EmpDtofJoining = "";
            string DOJ = "";
            string Desgn = "";
            string NatureOfWork = "";
            float Basic = 0;
            float DA = 0;
            float OA = 0;
            float Total = 0;

            if (dtEmpdetails.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");
                string strQry = "";

                for (int i = 0; i < dtEmpdetails.Rows.Count; i++)
                {



                    #region Assining data to Variables

                    strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + dtEmpdetails.Rows[i]["EmpId"].ToString() + "'";
                    string esiNo = "";
                    DataTable esiTable = SqlHelper.Instance.GetTableByQuery(strQry);
                    if (esiTable.Rows.Count > 0)
                    {
                        esiNo = esiTable.Rows[0]["EmpESINo"].ToString();
                    }

                    strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + dtEmpdetails.Rows[i]["EmpId"].ToString() + "'";
                    string pfNo = "";
                    esiTable = SqlHelper.Instance.GetTableByQuery(strQry);
                    if (esiTable.Rows.Count > 0)
                    {
                        pfNo = esiTable.Rows[0]["EmpEpfNo"].ToString();
                    }


                    Empid = dtEmpdetails.Rows[i]["empid"].ToString();
                    EmpName = dtEmpdetails.Rows[i]["EmpmName"].ToString();
                    FatherName = dtEmpdetails.Rows[i]["EmpFatherName"].ToString();
                    DateOfBirth = dtEmpdetails.Rows[i]["EmpDtofBirth"].ToString();
                    DOJ = dtEmpdetails.Rows[i]["EmpDtofJoining"].ToString();
                    Desgn = dtEmpdetails.Rows[i]["Desgn"].ToString();
                    Basic = float.Parse(dtEmpdetails.Rows[i]["Basic"].ToString());
                    DA = float.Parse(dtEmpdetails.Rows[i]["DA"].ToString());
                    OA = float.Parse(dtEmpdetails.Rows[i]["Otherallowances"].ToString());
                    Total = float.Parse(dtEmpdetails.Rows[i]["Totalamount"].ToString());
                    EmpDtofJoining = dtEmpdetails.Rows[i]["EmpDtofJoining"].ToString();


                    string prLmark = dtEmpdetails.Rows[i]["prLmark"].ToString();
                    string prTown = dtEmpdetails.Rows[i]["prTown"].ToString();
                    string prPostoffice = dtEmpdetails.Rows[i]["prPostoffice"].ToString();
                    string prTaluka = dtEmpdetails.Rows[i]["prTaluka"].ToString();
                    string prPoliceStation = dtEmpdetails.Rows[i]["prPoliceStation"].ToString();
                    string prCity = dtEmpdetails.Rows[i]["prCity"].ToString();
                    string prstate = dtEmpdetails.Rows[i]["prstate"].ToString();
                    string prpincode = dtEmpdetails.Rows[i]["prpincode"].ToString();

                    string peLmark = dtEmpdetails.Rows[i]["peLmark"].ToString();
                    string peTown = dtEmpdetails.Rows[i]["peTown"].ToString();
                    string pePostoffice = dtEmpdetails.Rows[i]["pePostoffice"].ToString();
                    string peTaluka = dtEmpdetails.Rows[i]["peTaluka"].ToString();
                    string pePoliceStation = dtEmpdetails.Rows[i]["pePoliceStation"].ToString();
                    string peCity = dtEmpdetails.Rows[i]["peCity"].ToString();
                    string pestate = dtEmpdetails.Rows[i]["pestate"].ToString();
                    string pepincode = dtEmpdetails.Rows[i]["pepincode"].ToString();

                    companyAddress = dtEmpdetails.Rows[0]["companyAddress"].ToString();
                    companyName = dtEmpdetails.Rows[0]["companyName"].ToString();
                    addressData = dtEmpdetails.Rows[0]["clientaddress"].ToString();

                    string presentadd = prLmark + ' ' + prTown + ' ' + prPostoffice + ' ' + prTaluka + ' ' + prPoliceStation + ' ' + prCity + ' ' + prstate + ' ' + prpincode;
                    string permanentadd = peLmark + ' ' + peTown + ' ' + pePostoffice + ' ' + peTaluka + ' ' + pePoliceStation + ' ' + peCity + ' ' + pestate + ' ' + pepincode;

                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 500f;
                    table.LockedWidth = true;
                    float[] width = new float[] { 1.5f, 2f, 2f, 2f, 1.5f, 2f };
                    table.SetWidths(width);



                    #endregion

                    PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(fontsyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
                    cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellspace.Colspan = 6;
                    cellspace.Border = 0;
                    cellspace.PaddingTop = -5;

                    PdfPCell celllogoHead = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 4, Font.NORMAL, BaseColor.BLACK)));
                    celllogoHead.HorizontalAlignment = 2;
                    celllogoHead.Colspan = 3;
                    celllogoHead.Border = 0;
                    table.AddCell(celllogoHead);

                    PdfPCell cellHead = new PdfPCell(new Phrase("FORM Q", FontFactory.GetFont(fontsyle, Fontsize + 4, Font.NORMAL, BaseColor.BLACK)));
                    cellHead.HorizontalAlignment = 0;
                    cellHead.Colspan = 3;
                    cellHead.Border = 0;
                    cellHead.PaddingLeft = -30;
                    table.AddCell(cellHead);

                    PdfPCell cellrule = new PdfPCell(new Phrase("(see rule 24(9A)) ", FontFactory.GetFont(fontsyle, 12, Font.NORMAL, BaseColor.BLACK)));
                    cellrule.HorizontalAlignment = 0;
                    cellrule.Colspan = 6;
                    cellrule.Border = 0;
                    cellrule.PaddingLeft = 198;
                    table.AddCell(cellrule);


                    PdfPCell cellHead4 = new PdfPCell(new Phrase("APPOINTMENT ORDER", FontFactory.GetFont(fontsyle, Fontsize + 3, Font.NORMAL, BaseColor.BLACK)));
                    cellHead4.HorizontalAlignment = 1;
                    cellHead4.Colspan = 6;
                    cellHead4.Border = 0;
                    table.AddCell(cellHead4);
                    document.Add(table);


                    PdfPTable table2 = new PdfPTable(4);
                    table2.TotalWidth = 500f;
                    table2.LockedWidth = true;
                    float[] width1 = new float[] { 0.8f, 6f, 3f, 2f };
                    table2.SetWidths(width1);

                    PdfPCell cellspace3 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellspace3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellspace3.Colspan = 4;
                    cellspace3.Border = 0;
                    table2.AddCell(cellspace3);

                    PdfPCell cellSno1 = new PdfPCell(new Phrase("\n1. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno1);

                    PdfPCell cellName2 = new PdfPCell(new Phrase("\nName and address of Establishment\n\n\n\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName2.HorizontalAlignment = 0;
                    cellName2.Colspan = 0;
                    cellName2.Border = 0;
                    cellName2.BorderWidthLeft = 0.5f;
                    cellName2.BorderWidthRight = 0.2f;
                    cellName2.BorderWidthTop = 0.5f;
                    cellName2.BorderWidthBottom = 0.5f;
                    cellName2.PaddingLeft = 10;
                    table2.AddCell(cellName2);


                    PdfPCell cellName3 = new PdfPCell(new Phrase(companyName + "\n" + companyAddress, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellName3.HorizontalAlignment = 0;
                    cellName3.Colspan = 2;
                    cellName3.Border = 0;
                    cellName3.PaddingLeft = 10;
                    cellName3.BorderWidthLeft = 0;
                    cellName3.BorderWidthRight = 0.9f;
                    cellName3.BorderWidthTop = 0.5f;
                    cellName3.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellName3);

                    PdfPCell cellSno2 = new PdfPCell(new Phrase("\n2. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno2);

                    PdfPCell cellName11 = new PdfPCell(new Phrase("\nName and address of the Employer\n\n\n\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName11.HorizontalAlignment = 0;
                    cellName11.Colspan = 0;
                    cellName11.Border = 0;
                    cellName11.BorderWidthLeft = 0.5f;
                    cellName11.BorderWidthRight = 0.2f;
                    cellName11.BorderWidthTop = 0;
                    cellName11.BorderWidthBottom = 0.5f;
                    cellName11.PaddingLeft = 10;
                    table2.AddCell(cellName11);


                    PdfPCell cellName10 = new PdfPCell(new Phrase(companyName + "\n" + companyAddress, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellName10.HorizontalAlignment = 0;
                    cellName10.Colspan = 2;
                    cellName10.Border = 0;
                    cellName10.BorderWidthLeft = 0;
                    cellName10.BorderWidthRight = 0.9f;
                    cellName10.BorderWidthTop = 0;
                    cellName10.BorderWidthBottom = 0.5f;
                    cellName10.PaddingLeft = 10;
                    table2.AddCell(cellName10);


                    PdfPCell cellSno3 = new PdfPCell(new Phrase("\n3. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno3);

                    PdfPCell cellName16 = new PdfPCell(new Phrase("\nName of the Employee\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName16.HorizontalAlignment = 0;
                    cellName16.Colspan = 0;
                    cellName16.Border = 0;
                    cellName16.BorderWidthLeft = 0.5f;
                    cellName16.BorderWidthRight = 0.2f;
                    cellName16.BorderWidthTop = 0;
                    cellName16.BorderWidthBottom = 0.5f;
                    cellName16.PaddingLeft = 10;
                    table2.AddCell(cellName16);

                    PdfPCell cellName17 = new PdfPCell(new Phrase("\n" + EmpName, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellName17.HorizontalAlignment = 0;
                    cellName17.Colspan = 2;
                    cellName17.Border = 0;
                    cellName17.BorderWidthLeft = 0;
                    cellName17.BorderWidthRight = 0.9f;
                    cellName17.BorderWidthTop = 0;
                    cellName17.BorderWidthBottom = 0.5f;
                    cellName17.PaddingLeft = 10;
                    table2.AddCell(cellName17);


                    PdfPCell cellSno4 = new PdfPCell(new Phrase("\n4. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno4);

                    PdfPCell cellName19 = new PdfPCell(new Phrase("\nHis / Her Postal Address\n\n\n\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName19.HorizontalAlignment = 0;
                    cellName19.Colspan = 0;
                    cellName19.Border = 0;
                    cellName19.BorderWidthLeft = 0.5f;
                    cellName19.BorderWidthRight = 0.2f;
                    cellName19.BorderWidthTop = 0;
                    cellName19.BorderWidthBottom = 0.5f;
                    cellName19.PaddingLeft = 10;
                    table2.AddCell(cellName19);



                    PdfPCell cellName18 = new PdfPCell(new Phrase("\n" + presentadd.Trim(), FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellName18.HorizontalAlignment = 0;
                    cellName18.Colspan = 2;
                    cellName18.Border = 0;
                    cellName18.BorderWidthLeft = 0;
                    cellName18.BorderWidthRight = 0.9f;
                    cellName18.BorderWidthTop = 0;
                    cellName18.BorderWidthBottom = 0.5f;
                    cellName18.PaddingLeft = 10;
                    table2.AddCell(cellName18);


                    PdfPCell cellSno5 = new PdfPCell(new Phrase("\n5. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno5);


                    PdfPCell cellPermanentAdd = new PdfPCell(new Phrase("\nHis / Her Permanent Address\n\n\n\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellPermanentAdd.HorizontalAlignment = 0;
                    cellPermanentAdd.Colspan = 0;
                    cellPermanentAdd.Border = 0;
                    cellPermanentAdd.BorderWidthLeft = 0.5f;
                    cellPermanentAdd.BorderWidthRight = 0.2f;
                    cellPermanentAdd.BorderWidthTop = 0;
                    cellPermanentAdd.BorderWidthBottom = 0.5f;
                    cellPermanentAdd.PaddingLeft = 10;
                    table2.AddCell(cellPermanentAdd);


                    PdfPCell cellpermaddval = new PdfPCell(new Phrase("\n" + permanentadd.Trim(), FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellpermaddval.HorizontalAlignment = 0;
                    cellpermaddval.Colspan = 2;
                    cellpermaddval.Border = 0;
                    cellpermaddval.BorderWidthLeft = 0;
                    cellpermaddval.BorderWidthRight = 0.9f;
                    cellpermaddval.BorderWidthTop = 0;
                    cellpermaddval.PaddingLeft = 10;
                    cellpermaddval.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellpermaddval);

                    PdfPCell cellSno6 = new PdfPCell(new Phrase("\n6. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno6);

                    PdfPCell cellFatherHusband = new PdfPCell(new Phrase("\nFather's / Husband's Name\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellFatherHusband.HorizontalAlignment = 0;
                    cellFatherHusband.Colspan = 0;
                    cellFatherHusband.Border = 0;
                    cellFatherHusband.BorderWidthLeft = 0.5f;
                    cellFatherHusband.BorderWidthRight = 0.2f;
                    cellFatherHusband.BorderWidthTop = 0;
                    cellFatherHusband.BorderWidthBottom = 0.5f;
                    cellFatherHusband.PaddingLeft = 10;
                    table2.AddCell(cellFatherHusband);




                    PdfPCell cellFatherHusbandval = new PdfPCell(new Phrase("\n" + FatherName, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellFatherHusbandval.HorizontalAlignment = 0;
                    cellFatherHusbandval.Colspan = 2;
                    cellFatherHusbandval.Border = 0;
                    cellFatherHusbandval.PaddingLeft = 10;
                    cellFatherHusbandval.BorderWidthLeft = 0;
                    cellFatherHusbandval.BorderWidthRight = 0.9f;
                    cellFatherHusbandval.BorderWidthTop = 0;
                    cellFatherHusbandval.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellFatherHusbandval);


                    PdfPCell cellSno7 = new PdfPCell(new Phrase("\n7. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno7.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno7);

                    PdfPCell cellDOB = new PdfPCell(new Phrase("\nDate of Birth / Age\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellDOB.HorizontalAlignment = 0;
                    cellDOB.Colspan = 0;
                    cellDOB.Border = 0;
                    cellDOB.BorderWidthLeft = 0.5f;
                    cellDOB.BorderWidthRight = 0.2f;
                    cellDOB.BorderWidthTop = 0;
                    cellDOB.BorderWidthBottom = 0.5f;
                    cellDOB.PaddingLeft = 10;
                    table2.AddCell(cellDOB);


                    PdfPCell cellDOBval = new PdfPCell(new Phrase("\n" + DateOfBirth, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellDOBval.HorizontalAlignment = 0;
                    cellDOBval.Colspan = 2;
                    cellDOBval.Border = 0;
                    cellDOBval.PaddingLeft = 10;
                    cellDOBval.BorderWidthLeft = 0;
                    cellDOBval.BorderWidthRight = 0.9f;
                    cellDOBval.BorderWidthTop = 0;
                    cellDOBval.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellDOBval);


                    PdfPCell cellSno8 = new PdfPCell(new Phrase("\n8. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno8.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno8);

                    PdfPCell cellDOJ = new PdfPCell(new Phrase("\nDate of His / Her entry into employment\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellDOJ.HorizontalAlignment = 0;
                    cellDOJ.Colspan = 0;
                    cellDOJ.Border = 0;
                    cellDOJ.BorderWidthLeft = 0.5f;
                    cellDOJ.BorderWidthRight = 0.2f;
                    cellDOJ.BorderWidthTop = 0;
                    cellDOJ.BorderWidthBottom = 0.5f;
                    cellDOJ.PaddingLeft = 10;
                    table2.AddCell(cellDOJ);


                    PdfPCell cellDOJVal = new PdfPCell(new Phrase("\n" + DOJ, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellDOJVal.HorizontalAlignment = 0;
                    cellDOJVal.Colspan = 2;
                    cellDOJVal.Border = 0;
                    cellDOJVal.BorderWidthLeft = 0;
                    cellDOJVal.BorderWidthRight = 0.9f;
                    cellDOJVal.BorderWidthTop = 0;
                    cellDOJVal.BorderWidthBottom = 0.5f;
                    cellDOJVal.PaddingLeft = 10;
                    table2.AddCell(cellDOJVal);


                    PdfPCell cellSno9 = new PdfPCell(new Phrase("\n9. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno9.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno9);


                    PdfPCell cellName21 = new PdfPCell(new Phrase("\nDesignation\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName21.HorizontalAlignment = 0;
                    cellName21.Colspan = 0;
                    cellName21.Border = 0;
                    cellName21.BorderWidthLeft = 0.5f;
                    cellName21.BorderWidthRight = 0.2f;
                    cellName21.BorderWidthTop = 0;
                    cellName21.BorderWidthBottom = 0.5f;
                    cellName21.PaddingLeft = 10;
                    table2.AddCell(cellName21);


                    PdfPCell cellName20 = new PdfPCell(new Phrase("\n" + Desgn, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellName20.HorizontalAlignment = 0;
                    cellName20.Colspan = 2;
                    cellName20.Border = 0;
                    cellName20.BorderWidthLeft = 0;
                    cellName20.BorderWidthRight = 0.9f;
                    cellName20.BorderWidthTop = 0;
                    cellName20.PaddingLeft = 10;
                    cellName20.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellName20);

                    PdfPCell cellSno10 = new PdfPCell(new Phrase("\n10. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno10.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno10);


                    PdfPCell cellNatureofwork = new PdfPCell(new Phrase("\nNature of entrusted to him\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellNatureofwork.HorizontalAlignment = 0;
                    cellNatureofwork.Colspan = 0;
                    cellNatureofwork.Border = 0;
                    cellNatureofwork.BorderWidthLeft = 0.5f;
                    cellNatureofwork.BorderWidthRight = 0.2f;
                    cellNatureofwork.BorderWidthTop = 0;
                    cellNatureofwork.BorderWidthBottom = 0.5f;
                    cellNatureofwork.PaddingLeft = 10;
                    table2.AddCell(cellNatureofwork);


                    PdfPCell cellNatureofworkVal = new PdfPCell(new Phrase("\nSecurity", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellNatureofworkVal.HorizontalAlignment = 0;
                    cellNatureofworkVal.Colspan = 2;
                    cellNatureofworkVal.Border = 0;
                    cellNatureofworkVal.BorderWidthLeft = 0;
                    cellNatureofworkVal.BorderWidthRight = 0.9f;
                    cellNatureofworkVal.BorderWidthTop = 0;
                    cellNatureofworkVal.BorderWidthBottom = 0.5f;
                    cellNatureofworkVal.PaddingLeft = 10;
                    table2.AddCell(cellNatureofworkVal);

                    PdfPCell cellSno11 = new PdfPCell(new Phrase("\n11. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno11.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cellSno11);



                    PdfPCell cellName14 = new PdfPCell(new Phrase("\nHis / Her serial number in the Register of Employment\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName14.HorizontalAlignment = 0;
                    cellName14.Colspan = 0;
                    cellName14.Border = 0;
                    cellName14.BorderWidthLeft = 0.5f;
                    cellName14.BorderWidthRight = 0.2f;
                    cellName14.BorderWidthTop = 0;
                    cellName14.BorderWidthBottom = 0.5f;
                    cellName14.PaddingLeft = 10;
                    table2.AddCell(cellName14);


                    PdfPCell cellName15 = new PdfPCell(new Phrase("\n" + Empid + "\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellName15.HorizontalAlignment = 0;
                    cellName15.Colspan = 2;
                    cellName15.Border = 0;
                    cellName15.BorderWidthLeft = 0;
                    cellName15.BorderWidthRight = 0.9f;
                    cellName15.BorderWidthTop = 0;
                    cellName15.BorderWidthBottom = 0.5f;
                    cellName15.PaddingLeft = 10;
                    table2.AddCell(cellName15);



                    PdfPCell cellSno12 = new PdfPCell(new Phrase("\n12. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellSno12.BorderWidthLeft = 0.5f;
                    cellSno12.BorderWidthRight = 0.2f;
                    cellSno12.BorderWidthTop = 0;
                    cellSno12.BorderWidthBottom = 0;
                    table2.AddCell(cellSno12);


                    PdfPCell cellName22 = new PdfPCell(new Phrase("\nRates of Wages payable to him/her as on   ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName22.HorizontalAlignment = 0;
                    cellName22.Colspan = 0;
                    cellName22.Border = 0;
                    cellName22.BorderWidthLeft = 0.5f;
                    cellName22.BorderWidthRight = 0.2f;
                    cellName22.BorderWidthTop = 0;
                    cellName22.BorderWidthBottom = 0;
                    cellName22.PaddingLeft = 10;
                    table2.AddCell(cellName22);

                    PdfPCell cellName29 = new PdfPCell(new Phrase("a)Basic", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName29.HorizontalAlignment = 0;
                    cellName29.Colspan = 1;
                    cellName29.Border = 0;
                    cellName29.PaddingTop = 20;
                    cellName29.BorderWidthLeft = 0;
                    cellName29.BorderWidthRight = 0;
                    cellName29.BorderWidthTop = 0;
                    cellName29.BorderWidthBottom = 0;
                    table2.AddCell(cellName29);


                    cellName29 = new PdfPCell(new Phrase(": " + Basic, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName29.HorizontalAlignment = 0;
                    cellName29.Colspan = 1;
                    cellName29.Border = 0;
                    cellName29.PaddingTop = 20;
                    cellName29.BorderWidthLeft = 0;
                    cellName29.BorderWidthRight = 0.9f;
                    cellName29.BorderWidthTop = 0;
                    cellName29.BorderWidthBottom = 0;
                    table2.AddCell(cellName29);

                    /////////////

                    cellSno12 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellSno12.BorderWidthLeft = 0.5f;
                    cellSno12.BorderWidthRight = 0.2f;
                    cellSno12.BorderWidthTop = 0;
                    cellSno12.BorderWidthBottom = 0;
                    table2.AddCell(cellSno12);

                    cellName22 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName22.HorizontalAlignment = 0;
                    cellName22.Colspan = 0;
                    cellName22.Border = 0;
                    cellName22.BorderWidthLeft = 0.5f;
                    cellName22.BorderWidthRight = 0.2f;
                    cellName22.BorderWidthTop = 0;
                    cellName22.BorderWidthBottom = 0;
                    cellName22.PaddingLeft = 10;
                    table2.AddCell(cellName22);

                    cellName29 = new PdfPCell(new Phrase("b)VDA", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName29.HorizontalAlignment = 0;
                    cellName29.Colspan = 1;
                    cellName29.Border = 0;
                    cellName29.BorderWidthLeft = 0;
                    cellName29.BorderWidthRight = 0;
                    cellName29.BorderWidthTop = 0;
                    cellName29.BorderWidthBottom = 0;
                    table2.AddCell(cellName29);


                    cellName29 = new PdfPCell(new Phrase(": " + DA, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName29.HorizontalAlignment = 0;
                    cellName29.Colspan = 1;
                    cellName29.Border = 0;
                    cellName29.BorderWidthLeft = 0;
                    cellName29.BorderWidthRight = 0.9f;
                    cellName29.BorderWidthTop = 0;
                    cellName29.BorderWidthBottom = 0;
                    table2.AddCell(cellName29);

                    //////////


                    /////////////

                    cellSno12 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellSno12.BorderWidthLeft = 0.5f;
                    cellSno12.BorderWidthRight = 0.2f;
                    cellSno12.BorderWidthTop = 0;
                    cellSno12.BorderWidthBottom = 0;
                    table2.AddCell(cellSno12);

                    cellName22 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName22.HorizontalAlignment = 0;
                    cellName22.Colspan = 0;
                    cellName22.Border = 0;
                    cellName22.BorderWidthLeft = 0.5f;
                    cellName22.BorderWidthRight = 0.2f;
                    cellName22.BorderWidthTop = 0;
                    cellName22.BorderWidthBottom = 0;
                    cellName22.PaddingLeft = 10;
                    table2.AddCell(cellName22);

                    cellName29 = new PdfPCell(new Phrase("c)Other Allowances if any ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName29.HorizontalAlignment = 0;
                    cellName29.Colspan = 1;
                    cellName29.Border = 0;
                    cellName29.BorderWidthLeft = 0;
                    cellName29.BorderWidthRight = 0;
                    cellName29.BorderWidthTop = 0;
                    cellName29.BorderWidthBottom = 0;
                    table2.AddCell(cellName29);


                    cellName29 = new PdfPCell(new Phrase(": " + OA, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName29.HorizontalAlignment = 0;
                    cellName29.Colspan = 1;
                    cellName29.Border = 0;
                    cellName29.BorderWidthLeft = 0;
                    cellName29.BorderWidthRight = 0.9f;
                    cellName29.BorderWidthTop = 0;
                    cellName29.BorderWidthBottom = 0;
                    table2.AddCell(cellName29);

                    //////////

                    /////////////

                    cellSno12 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellSno12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellSno12.BorderWidthLeft = 0.5f;
                    cellSno12.BorderWidthRight = 0.2f;
                    cellSno12.BorderWidthTop = 0;
                    cellSno12.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellSno12);

                    cellName22 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName22.HorizontalAlignment = 0;
                    cellName22.Colspan = 0;
                    cellName22.Border = 0;
                    cellName22.BorderWidthLeft = 0.5f;
                    cellName22.BorderWidthRight = 0.2f;
                    cellName22.BorderWidthTop = 0;
                    cellName22.BorderWidthBottom = 0.5f;
                    cellName22.PaddingLeft = 10;
                    table2.AddCell(cellName22);

                    cellName29 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName29.HorizontalAlignment = 0;
                    cellName29.Colspan = 1;
                    cellName29.Border = 0;
                    cellName29.BorderWidthLeft = 0;
                    cellName29.BorderWidthRight = 0;
                    cellName29.BorderWidthTop = 0;
                    cellName29.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellName29);


                    cellName29 = new PdfPCell(new Phrase(": " + Total, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName29.HorizontalAlignment = 0;
                    cellName29.Colspan = 1;
                    cellName29.Border = 0;
                    cellName29.BorderWidthLeft = 0;
                    cellName29.BorderWidthRight = 0.9f;
                    cellName29.BorderWidthTop = 0;
                    cellName29.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellName29);

                    //////////


                    PdfPCell cellName291 = new PdfPCell(new Phrase("\nPlace :Bangalore                                                                                         Signature of the Contractor     ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName291.HorizontalAlignment = 0;
                    cellName291.Colspan = 4;
                    cellName291.PaddingTop = 10;
                    cellName291.BorderWidthLeft = 0.5f;
                    cellName291.BorderWidthRight = 0.5f;
                    cellName291.BorderWidthTop = 0;
                    cellName291.BorderWidthBottom = 0;

                    table2.AddCell(cellName291);

                    PdfPCell cellNames28 = new PdfPCell(new Phrase("Date :" + EmpDtofJoining, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellNames28.HorizontalAlignment = 0;
                    cellNames28.Colspan = 4;
                    cellNames28.Border = 0;
                    cellNames28.BorderWidthRight = 0.5f;
                    cellNames28.BorderWidthLeft = 0.5f;
                    cellNames28.BorderWidthBottom = 0;
                    table2.AddCell(cellNames28);



                    cellName291 = new PdfPCell(new Phrase("\nAcknowledgement by employee                                                          Seal of the establishment     ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName291.HorizontalAlignment = 0;
                    cellName291.Colspan = 4;
                    cellName291.PaddingTop = 20;
                    cellName291.BorderWidthLeft = 0.5f;
                    cellName291.BorderWidthRight = 0.5f;
                    cellName291.BorderWidthTop = 0;
                    cellName291.BorderWidthBottom = 0;
                    table2.AddCell(cellName291);

                    cellName291 = new PdfPCell(new Phrase("With date and Signature", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellName291.HorizontalAlignment = 0;
                    cellName291.Colspan = 4;
                    //cellName291.PaddingTop = 20;
                    cellName291.BorderWidthLeft = 0.5f;
                    cellName291.BorderWidthRight = 0.5f;
                    cellName291.BorderWidthTop = 0;
                    cellName291.BorderWidthBottom = 0.5f;
                    table2.AddCell(cellName291);

                    document.Add(table2);

                    slipsCount++;
                    if (slipsCount == 1)
                    {
                        slipsCount = 0;
                        document.NewPage();
                    }
                }




                string filename = ddlcname.SelectedItem.Text + "/FormQ.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();


            }
        }

        protected void btnFormLeaveWagesNew_Click(object sender, EventArgs e)
        {
            try
            {


                int Fontsize = 9;
                string fontsyle = "verdana";

                #region Variable Declaration


                string Idno = "";
                string date = "";
                string noofduties = "0";
                string name = "";
                string fathername = "";
                string LeaveWages = "";
                float wkgdays = 0;
                string paymonth = "";
                string month = "";
                string year = "";
                DateTime firstday = DateTime.Now;
                DateTime lastday = DateTime.Now;

                float MonthDays = 0;
                string MDays = "";





                #endregion


                if (ddlclientid.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Client ID');", true);
                    return;
                }

                DateTime frmdate;
                string FromDate = "";
                string Frmonth = "";
                string FrYear = "";
                string FrMn = "";

                if (txtfrom.Text.Trim().Length > 0)
                {
                    frmdate = DateTime.ParseExact(txtfrom.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    Frmonth = frmdate.ToString("MM");
                    FrYear = frmdate.ToString("yy");
                    FrMn = frmdate.Month.ToString();
                }



                FromDate = FrYear + Frmonth;


                DateTime tdate;
                string ToDate = "";
                string Tomonth = "";
                string ToYear = "";
                string ToMn = "";
                if (txtto.Text.Trim().Length > 0)
                {
                    tdate = DateTime.ParseExact(txtto.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    Tomonth = tdate.ToString("MM");
                    ToYear = tdate.ToString("yy");
                    ToMn = tdate.Month.ToString();

                }

                ToDate = ToYear + Tomonth;


                #region Begin Variable Declaration

                DataTable dtEmpdetails = null;

                #endregion end Variable Declaration


                string qry = "select eps.empid from emppaysheet eps inner join empdetails e on e.empid=eps.empid where eps.ClientId='" + ddlclientid.SelectedValue + "' and  eps.monthnew >= '" + FromDate.ToString() + "' and eps.monthnew<='" + ToDate.ToString()  + "'  ";
                DataTable dtEmpList = SqlHelper.Instance.GetTableByQuery(qry);

                if (dtEmpList.Rows.Count > 0)
                {

                    string EmpID = "";

                    MemoryStream ms = new MemoryStream();
                    Document document = new Document(PageSize.LEGAL.Rotate());
                    var writer = PdfWriter.GetInstance(document, ms);
                    document.Open();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    string imagepath1 = Server.MapPath("images");


                    for (int h = 0; h < dtEmpList.Rows.Count; h++)
                    {


                        EmpID = dtEmpList.Rows[h]["empid"].ToString();

                        string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                        DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
                        string companyName = "Your Company Name";
                        string companyAddress = "Your Company Address";

                        if (compInfo.Rows.Count > 0)
                        {
                            companyName = compInfo.Rows[0]["CompanyName"].ToString();
                            companyAddress = compInfo.Rows[0]["Address"].ToString();
                        }

                        #region for Headings

                        PdfPTable tablesnewone = new PdfPTable(19);
                        tablesnewone.TotalWidth = 940f;
                        tablesnewone.HeaderRows = 6;
                        tablesnewone.LockedWidth = true;
                        float[] width = new float[] { 2f, 4f, 6f, 6f, 4f, 4f, 3f, 3f, 3f, 3f, 3f, 2.5f, 3f, 3f, 3.2f, 2.5f, 2.5f, 2.5f, 3.5f };
                        tablesnewone.SetWidths(width);



                        PdfPCell CForm = new PdfPCell(new Phrase("Name Address of Contractor ", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        CForm.HorizontalAlignment = 1;
                        CForm.Colspan = 5;
                        CForm.Border = 0;
                        tablesnewone.AddCell(CForm);

                        CForm = new PdfPCell(new Phrase(": " + companyName + "\n   " + companyAddress, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        CForm.HorizontalAlignment = 1;
                        CForm.Colspan = 14;
                        CForm.Border = 0;
                        tablesnewone.AddCell(CForm);

                        CForm = new PdfPCell(new Phrase("Name address of Principle Employer ", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        CForm.HorizontalAlignment = 1;
                        CForm.Colspan = 5;
                        CForm.Border = 0;
                        CForm.PaddingTop = 5;
                        tablesnewone.AddCell(CForm);

                        CForm = new PdfPCell(new Phrase(": " + ddlcname.SelectedItem, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        CForm.HorizontalAlignment = 1;
                        CForm.Colspan = 14;
                        CForm.Border = 0;
                        CForm.PaddingTop = 5;
                        tablesnewone.AddCell(CForm);



                        CForm = new PdfPCell(new Phrase("FORM F", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        CForm.HorizontalAlignment = 1;
                        CForm.Colspan = 19;
                        CForm.Border = 0;
                        CForm.PaddingTop = 8;
                        tablesnewone.AddCell(CForm);

                        PdfPCell Crule = new PdfPCell(new Phrase("(See Rule 9)", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        Crule.HorizontalAlignment = 1;
                        Crule.Colspan = 19;
                        Crule.Border = 0;
                        tablesnewone.AddCell(Crule);

                        PdfPCell CRLW = new PdfPCell(new Phrase("REGISTER OF LEAVE WITH WAGES", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        CRLW.HorizontalAlignment = 1;
                        CRLW.Colspan = 19;
                        CRLW.Border = 0;
                        CRLW.PaddingBottom = 10;
                        tablesnewone.AddCell(CRLW);


                        PdfPCell heading = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading.HorizontalAlignment = 1;
                        heading.Colspan = 1;
                        heading.BorderWidthBottom = 0;
                        heading.BorderWidthLeft = .5f;
                        heading.BorderWidthRight = 0;
                        heading.BorderWidthTop = .5f;
                        tablesnewone.AddCell(heading);


                        PdfPCell headingd = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        headingd.HorizontalAlignment = 1;
                        headingd.Colspan = 1;
                        headingd.BorderWidthBottom = 0;
                        headingd.BorderWidthLeft = .5f;
                        headingd.BorderWidthRight = 0;
                        headingd.BorderWidthTop = .5f;
                        tablesnewone.AddCell(headingd);

                        PdfPCell headingb = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        headingb.HorizontalAlignment = 1;
                        headingb.Colspan = 1;
                        headingb.BorderWidthBottom = 0;
                        headingb.BorderWidthLeft = .5f;
                        headingb.BorderWidthRight = 0;
                        headingb.BorderWidthTop = .5f;
                        tablesnewone.AddCell(headingb);

                        PdfPCell headinga = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        headinga.HorizontalAlignment = 1;
                        headinga.Colspan = 1;
                        headinga.BorderWidthBottom = 0;
                        headinga.BorderWidthLeft = .5f;
                        headinga.BorderWidthRight = 0;
                        headinga.BorderWidthTop = .5f;
                        tablesnewone.AddCell(headinga);



                        PdfPCell heading1 = new PdfPCell(new Phrase("                             Part - I                                                  Earned Leave", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading1.HorizontalAlignment = 0;
                        heading1.Colspan = 11;
                        tablesnewone.AddCell(heading1);


                        PdfPCell heading2 = new PdfPCell(new Phrase("Part - II       Sick / Accident\nLeave with Pay ", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading2.HorizontalAlignment = 1;
                        heading2.Colspan = 4;
                        tablesnewone.AddCell(heading2);



                        PdfPCell heading3 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        heading3.HorizontalAlignment = 1;
                        heading3.Colspan = 1;
                        heading3.BorderWidthBottom = 0;
                        heading3.BorderWidthLeft = .5f;
                        heading3.BorderWidthRight = 0;
                        heading3.BorderWidthTop = 0;
                        tablesnewone.AddCell(heading3);

                        PdfPCell heading3a = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        heading3a.HorizontalAlignment = 1;
                        heading3a.Colspan = 1;
                        heading3a.BorderWidthBottom = 0;
                        heading3a.BorderWidthLeft = .5f;
                        heading3a.BorderWidthRight = 0;
                        heading3a.BorderWidthTop = 0;
                        tablesnewone.AddCell(heading3a);

                        PdfPCell heading3b = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        heading3b.HorizontalAlignment = 1;
                        heading3b.Colspan = 1;
                        heading3b.BorderWidthBottom = 0;
                        heading3b.BorderWidthLeft = .5f;
                        heading3b.BorderWidthRight = 0;
                        heading3b.BorderWidthTop = 0;
                        tablesnewone.AddCell(heading3b);

                        PdfPCell heading3c = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        heading3c.HorizontalAlignment = 1;
                        heading3c.Colspan = 1;
                        heading3c.BorderWidthBottom = 0;
                        heading3c.BorderWidthLeft = .5f;
                        heading3c.BorderWidthRight = 0;
                        heading3c.BorderWidthTop = 0;
                        tablesnewone.AddCell(heading3c);



                        PdfPCell heading4 = new PdfPCell(new Phrase("No Of days worked", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading4.HorizontalAlignment = 1;
                        heading4.Colspan = 3;
                        tablesnewone.AddCell(heading4);


                        PdfPCell heading5 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        heading5.HorizontalAlignment = 1;
                        heading5.Colspan = 1;
                        heading5.BorderWidthBottom = 0;
                        heading5.BorderWidthLeft = 0;
                        heading5.BorderWidthRight = 0;
                        heading5.BorderWidthTop = .5f;
                        tablesnewone.AddCell(heading5);


                        PdfPCell heading5a = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        heading5a.HorizontalAlignment = 1;
                        heading5a.Colspan = 1;
                        heading5a.BorderWidthBottom = 0;
                        heading5a.BorderWidthLeft = .5f;
                        heading5a.BorderWidthRight = 0;
                        heading5a.BorderWidthTop = .5f;
                        tablesnewone.AddCell(heading5a);


                        PdfPCell heading6 = new PdfPCell(new Phrase("Leave Availed", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading6.HorizontalAlignment = 1;
                        heading6.Colspan = 3;
                        tablesnewone.AddCell(heading6);


                        PdfPCell heading7 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading7.HorizontalAlignment = 1;
                        heading7.Colspan = 1;
                        heading7.BorderWidthBottom = .5f;
                        heading7.BorderWidthLeft = 0;
                        heading7.BorderWidthRight = 0;
                        heading7.BorderWidthTop = .5f;
                        tablesnewone.AddCell(heading7);


                        PdfPCell heading7a = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading7a.HorizontalAlignment = 1;
                        heading7a.Colspan = 1;
                        heading7a.BorderWidthBottom = .5f;
                        heading7a.BorderWidthLeft = 0;
                        heading7a.BorderWidthRight = 0;
                        heading7a.BorderWidthTop = .5f;
                        tablesnewone.AddCell(heading7a);


                        PdfPCell heading7b = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading7b.HorizontalAlignment = 1;
                        heading7b.Colspan = 1;
                        heading7b.BorderWidthBottom = 0;
                        heading7b.BorderWidthLeft = .5f;
                        heading7b.BorderWidthRight = 0;
                        heading7b.BorderWidthTop = .5f;
                        tablesnewone.AddCell(heading7b);

                        PdfPCell heading8 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading8.HorizontalAlignment = 1;
                        heading8.Colspan = 1;
                        heading8.BorderWidthBottom = 0;
                        heading8.BorderWidthLeft = .5f;
                        heading8.BorderWidthRight = 0;
                        heading8.BorderWidthTop = .5f;
                        tablesnewone.AddCell(heading8);

                        PdfPCell heading9 = new PdfPCell(new Phrase("Sick/Accident\nLeave", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading9.HorizontalAlignment = 1;
                        heading9.Colspan = 2;
                        tablesnewone.AddCell(heading9);

                        PdfPCell heading10 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        heading10.HorizontalAlignment = 1;
                        heading10.Colspan = 1;
                        heading10.BorderWidthBottom = 0;
                        heading10.BorderWidthLeft = 0;
                        heading10.BorderWidthRight = .5f;
                        heading10.BorderWidthTop = .5f;
                        tablesnewone.AddCell(heading10);


                        PdfPCell register = new PdfPCell(new Phrase("Sl.No. in the Register\nof Audit/Young person", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        register.VerticalAlignment = 1;
                        register.Rowspan = 1;
                        register.Rotation = 90;
                        register.BorderWidthBottom = .5f;
                        register.BorderWidthLeft = .5f;
                        register.BorderWidthRight = .5f;
                        register.BorderWidthTop = 0;
                        tablesnewone.AddCell(register);

                        PdfPCell dateofentry = new PdfPCell(new Phrase("Date of entry into service  ", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        dateofentry.VerticalAlignment = 1;
                        dateofentry.Rowspan = 1;
                        dateofentry.Rotation = 90;
                        dateofentry.BorderWidthBottom = .5f;
                        dateofentry.BorderWidthLeft = .5f;
                        dateofentry.BorderWidthRight = .5f;
                        dateofentry.BorderWidthTop = 0;
                        tablesnewone.AddCell(dateofentry);

                        PdfPCell nameofemp = new PdfPCell(new Phrase("Name of the Person", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        nameofemp.VerticalAlignment = 1;
                        nameofemp.Rowspan = 1;
                        nameofemp.Rotation = 90;
                        nameofemp.BorderWidthBottom = .5f;
                        nameofemp.BorderWidthLeft = .5f;
                        nameofemp.BorderWidthRight = 0;
                        nameofemp.BorderWidthTop = 0;
                        tablesnewone.AddCell(nameofemp);

                        PdfPCell fathername1 = new PdfPCell(new Phrase("Father/Husband Name", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        fathername1.VerticalAlignment = 1;
                        fathername1.Rowspan = 1;
                        fathername1.Rotation = 90;
                        fathername1.BorderWidthBottom = .5f;
                        fathername1.BorderWidthLeft = .5f;
                        fathername1.BorderWidthRight = .5f;
                        fathername1.BorderWidthTop = 0;
                        tablesnewone.AddCell(fathername1);

                        PdfPCell from = new PdfPCell(new Phrase("From", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        from.VerticalAlignment = 1;
                        from.Rowspan = 1;
                        from.Rotation = 90;
                        tablesnewone.AddCell(from);


                        PdfPCell to = new PdfPCell(new Phrase("To", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        to.VerticalAlignment = 1;
                        to.Rowspan = 1;
                        to.Rotation = 90;
                        to.Border = 0;
                        tablesnewone.AddCell(to);


                        PdfPCell wo = new PdfPCell(new Phrase("Total Days Worked", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        wo.VerticalAlignment = 0;
                        wo.Rowspan = 1;
                        wo.Rotation = 90;
                        tablesnewone.AddCell(wo);

                        PdfPCell leavearned = new PdfPCell(new Phrase("Leave Earned", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        leavearned.VerticalAlignment = 0;
                        leavearned.Rowspan = 1;
                        leavearned.Rotation = 90;
                        leavearned.BorderWidthBottom = .5f;
                        leavearned.BorderWidthLeft = .5f;
                        leavearned.BorderWidthRight = 0f;
                        leavearned.BorderWidthTop = 0;
                        tablesnewone.AddCell(leavearned);

                        PdfPCell leavecredit = new PdfPCell(new Phrase("Leave at Credit (including\nBalance if any on return\nfrom leave on last\noccasion", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        leavecredit.VerticalAlignment = 0;
                        leavecredit.Rowspan = 1;
                        leavecredit.Rotation = 90;
                        leavecredit.BorderWidthBottom = .5f;
                        leavecredit.BorderWidthLeft = .5f;
                        leavecredit.BorderWidthRight = .5f;
                        leavecredit.BorderWidthTop = 0;
                        tablesnewone.AddCell(leavecredit);

                        PdfPCell froml = new PdfPCell(new Phrase("From", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        froml.VerticalAlignment = 1;
                        froml.Rowspan = 1;
                        froml.Rotation = 90;
                        tablesnewone.AddCell(froml);

                        PdfPCell tol = new PdfPCell(new Phrase("To", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        tol.VerticalAlignment = 1;
                        tol.Rowspan = 1;
                        tol.Rotation = 90;
                        tablesnewone.AddCell(tol);

                        PdfPCell noofdays = new PdfPCell(new Phrase("No. of Days", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        noofdays.VerticalAlignment = 0;
                        noofdays.Rowspan = 1;
                        noofdays.Rotation = 90;
                        tablesnewone.AddCell(noofdays);

                        PdfPCell balance = new PdfPCell(new Phrase("Balance on return\nfrom Leave", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        balance.VerticalAlignment = 0;
                        balance.Rowspan = 1;
                        balance.Rotation = 90;
                        balance.BorderWidthBottom = .5f;
                        balance.BorderWidthLeft = .5f;
                        balance.BorderWidthRight = 0;
                        balance.BorderWidthTop = 0;
                        tablesnewone.AddCell(balance);

                        PdfPCell wages = new PdfPCell(new Phrase("Date on which wages for\nleave paid & amount paid", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        wages.VerticalAlignment = 1;
                        wages.Rowspan = 1;
                        wages.Rotation = 90;
                        wages.BorderWidthBottom = .5f;
                        wages.BorderWidthLeft = .5f;
                        wages.BorderWidthRight = 0;
                        wages.BorderWidthTop = 0;
                        tablesnewone.AddCell(wages);

                        PdfPCell Remarks = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        Remarks.VerticalAlignment = 1;
                        Remarks.Rowspan = 1;
                        Remarks.Rotation = 90;
                        Remarks.BorderWidthBottom = .5f;
                        Remarks.BorderWidthLeft = .5f;
                        Remarks.BorderWidthRight = 0;
                        Remarks.BorderWidthTop = 0;
                        tablesnewone.AddCell(Remarks);

                        PdfPCell Year = new PdfPCell(new Phrase("Year", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        Year.VerticalAlignment = 1;
                        Year.Rowspan = 1;
                        Year.Rotation = 90;
                        Year.BorderWidthBottom = .5f;
                        Year.BorderWidthLeft = .5f;
                        Year.BorderWidthRight = .5f;
                        Year.BorderWidthTop = 0;
                        tablesnewone.AddCell(Year);

                        PdfPCell cred = new PdfPCell(new Phrase("at credit", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        cred.VerticalAlignment = 1;
                        cred.Rowspan = 1;
                        cred.Rotation = 90;
                        tablesnewone.AddCell(cred);

                        PdfPCell Avile = new PdfPCell(new Phrase("Availed", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        Avile.VerticalAlignment = 1;
                        Avile.Rowspan = 1;
                        Avile.Rotation = 90;
                        tablesnewone.AddCell(Avile);

                        PdfPCell balanceat = new PdfPCell(new Phrase("Balance at the end\nof the year", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                        balanceat.VerticalAlignment = 1;
                        balanceat.Rowspan = 1;
                        balanceat.Rotation = 90;
                        balanceat.BorderWidthBottom = .5f;
                        balanceat.BorderWidthLeft = .5f;
                        balanceat.BorderWidthRight = .5f;
                        balanceat.BorderWidthTop = 0;
                        tablesnewone.AddCell(balanceat);


                        #endregion for Headings


                        int k = 1;
                        int LeaveDays = 1; 
                        
                        for (int S = 1; S <= int.Parse(ToMn); S++)
                        {
                            qry = "select top 1 eps.ClientId,eps.empid,(empfname+' '+empmname+' '+emplname) as empname,case convert(varchar(10),EmpDtofJoining,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofJoining,103) end EmpDtofJoining ,e.empfathername,month,(eps.noofduties+eps.Nhs) as wkgdays,noofduties,isnull(LeaveEncashAmt,'0') as LeaveWages from emppaysheet eps inner join empdetails e on e.empid=eps.empid where eps.ClientId='" + ddlclientid.SelectedValue + "' and  eps.month = '" + S.ToString() + ToYear + "'  and eps.empid='" + EmpID + "' " +
                                  "union all " +
                                  "select top 1 '' as ClientId,eps.empid,(empfname+' '+empmname+' '+emplname) as empname,case convert(varchar(10),EmpDtofJoining,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofJoining,103) end EmpDtofJoining ,e.empfathername, '" + S.ToString() + ToYear + "',0,0,0 as LeaveWages  from emppaysheet eps inner join empdetails e on e.empid=eps.empid where eps.ClientId='" + ddlclientid.SelectedValue + "' and eps.empid='" + EmpID + "' and eps.EmpId not in (select EmpId from EmpPaySheet where eps.month = '" + S.ToString() + ToYear + "') ";
                            dtEmpdetails = SqlHelper.Instance.GetTableByQuery(qry);


                            if (dtEmpdetails.Rows.Count > 0)
                            {
                                #region Assining data to Variables

                                Idno = dtEmpdetails.Rows[0]["EmpId"].ToString();
                                name = dtEmpdetails.Rows[0]["empname"].ToString();
                                fathername = dtEmpdetails.Rows[0]["EmpFatherName"].ToString();
                                date = dtEmpdetails.Rows[0]["EmpDtofJoining"].ToString();
                                wkgdays = Convert.ToSingle(dtEmpdetails.Rows[0]["wkgdays"].ToString());
                                LeaveWages = dtEmpdetails.Rows[0]["LeaveWages"].ToString();

                                paymonth = S + ToYear;

                                if (paymonth.Length == 3)
                                {
                                    month = paymonth.Substring(0, 1);
                                    year = "20" + paymonth.Substring(1, 2);

                                }
                                else if (paymonth.Length == 4)
                                {
                                    month = paymonth.Substring(0, 2);
                                    year = "20" + paymonth.Substring(2, 2);
                                }

                                firstday = GlobalData.Instance.GetFirstDayMonth(int.Parse(year), int.Parse(month));
                                lastday = GlobalData.Instance.GetLastDayMonth(int.Parse(year), int.Parse(month));

                                MDays = lastday.ToString("dd/MM/yyyy").Substring(0, 2);

                                MonthDays = Convert.ToSingle(MDays);



                                if (wkgdays > MonthDays)
                                {
                                    wkgdays = MonthDays;
                                }


                                if (wkgdays < 20)
                                {
                                    noofduties = "0";
                                }

                                else if (wkgdays >= 20 && wkgdays < 30 && paymonth.Substring(0, 1).ToString() != "2")
                                {
                                    noofduties = "1";
                                }
                                else if (paymonth.Substring(0, 1).ToString() == "2" && wkgdays >= 20 && wkgdays < 28)
                                {
                                    noofduties = "1";
                                }
                                else if (paymonth.Substring(0, 1).ToString() == "2" && wkgdays >= 28)
                                {
                                    noofduties = "1.5";
                                }
                                else if (wkgdays >= 30 && wkgdays <= 40)
                                {
                                    noofduties = "1.5";
                                }

                                PdfPCell register1 = new PdfPCell(new Phrase(k.ToString() + "\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                register1.HorizontalAlignment = 1;
                                register1.Colspan = 1;
                                register1.PaddingBottom = 5;
                                tablesnewone.AddCell(register1);

                                PdfPCell dateofentry1 = new PdfPCell(new Phrase(date, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                dateofentry1.HorizontalAlignment = 1;
                                dateofentry1.Colspan = 1;
                                dateofentry1.PaddingBottom = 5;

                                tablesnewone.AddCell(dateofentry1);

                                PdfPCell nameofemp1 = new PdfPCell(new Phrase(name, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                nameofemp1.HorizontalAlignment = 0;
                                nameofemp1.Colspan = 1;
                                tablesnewone.AddCell(nameofemp1);

                                PdfPCell fathername11 = new PdfPCell(new Phrase(fathername, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                fathername11.HorizontalAlignment = 0;
                                fathername11.Colspan = 1;
                                tablesnewone.AddCell(fathername11);

                                PdfPCell from1 = new PdfPCell(new Phrase(firstday.ToString("dd.MM.yyyy"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                from1.HorizontalAlignment = 1;
                                from1.Colspan = 1;
                                tablesnewone.AddCell(from1);


                                PdfPCell to1 = new PdfPCell(new Phrase(lastday.ToString("dd.MM.yyyy"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                to1.HorizontalAlignment = 1;
                                to1.Colspan = 1;
                                tablesnewone.AddCell(to1);



                                PdfPCell wo1 = new PdfPCell(new Phrase(wkgdays.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                wo1.HorizontalAlignment = 1;
                                wo1.Colspan = 1;
                                tablesnewone.AddCell(wo1);


                                PdfPCell leavearned1 = new PdfPCell(new Phrase(LeaveDays.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                leavearned1.HorizontalAlignment = 1;
                                leavearned1.Colspan = 1;
                                tablesnewone.AddCell(leavearned1);

                                PdfPCell leavecredit1 = new PdfPCell(new Phrase("" + LeaveDays.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                leavecredit1.HorizontalAlignment = 1;
                                leavecredit1.Colspan = 1;
                                tablesnewone.AddCell(leavecredit1);

                                PdfPCell froml1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                froml1.HorizontalAlignment = 1;
                                froml1.Colspan = 1;
                                tablesnewone.AddCell(froml1);

                                PdfPCell tol1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                tol1.HorizontalAlignment = 1;
                                tol1.Colspan = 1;
                                tablesnewone.AddCell(tol1);

                                PdfPCell noofdays1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                noofdays1.HorizontalAlignment = 1;
                                noofdays1.Colspan = 1;
                                tablesnewone.AddCell(noofdays1);



                                PdfPCell balance1 = new PdfPCell(new Phrase(LeaveDays.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                balance1.HorizontalAlignment = 1;
                                balance1.Colspan = 1;
                                tablesnewone.AddCell(balance1);

                                PdfPCell wages1 = new PdfPCell(new Phrase("Nill", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                wages1.HorizontalAlignment = 1;
                                wages1.Colspan = 1;
                                tablesnewone.AddCell(wages1);

                                PdfPCell Remarks1 = new PdfPCell(new Phrase("Nill", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                Remarks1.HorizontalAlignment = 1;
                                Remarks1.Colspan = 1;
                                tablesnewone.AddCell(Remarks1);

                                PdfPCell Year1 = new PdfPCell(new Phrase(year, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                Year1.HorizontalAlignment = 1;
                                Year1.Colspan = 1;
                                tablesnewone.AddCell(Year1);

                                PdfPCell cred1 = new PdfPCell(new Phrase("0", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                cred1.HorizontalAlignment = 1;
                                cred1.Colspan = 1;
                                tablesnewone.AddCell(cred1);

                                PdfPCell Avile1 = new PdfPCell(new Phrase("0", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                Avile1.HorizontalAlignment = 1;
                                Avile1.Colspan = 1;
                                tablesnewone.AddCell(Avile1);

                                if (dtEmpdetails.Rows.Count == LeaveDays)
                                {
                                    PdfPCell balanceat1 = new PdfPCell(new Phrase(LeaveDays.ToString(), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                    balanceat1.HorizontalAlignment = 1;
                                    balanceat1.Colspan = 1;
                                    tablesnewone.AddCell(balanceat1);
                                }
                                else
                                {
                                    PdfPCell balanceat1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                                    balanceat1.HorizontalAlignment = 1;
                                    balanceat1.Colspan = 1;
                                    tablesnewone.AddCell(balanceat1);
                                }

                               

                                #endregion Basic Information of the Employee

                                k++;
                                LeaveDays++;

                            }
                        }


                        strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                        compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
                        companyName = "Your Company Name";
                        companyAddress = "Your Company Address";

                        if (compInfo.Rows.Count > 0)
                        {
                            companyName = compInfo.Rows[0]["CompanyName"].ToString();
                            companyAddress = compInfo.Rows[0]["Address"].ToString();
                        }


                        PdfPCell cellcmnyadd1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 2, Font.NORMAL, BaseColor.BLACK)));
                        cellcmnyadd1.HorizontalAlignment = 2;
                        cellcmnyadd1.Colspan = 19;
                        cellcmnyadd1.Border = 0;
                        cellcmnyadd1.PaddingBottom = 20;
                        tablesnewone.AddCell(cellcmnyadd1);

                        PdfPCell companyname1 = new PdfPCell(new Phrase("For  " + companyName, FontFactory.GetFont(fontsyle, Fontsize + 2, Font.NORMAL, BaseColor.BLACK)));
                        companyname1.HorizontalAlignment = 2;
                        companyname1.Colspan = 19;
                        companyname1.Border = 0;
                        companyname1.PaddingBottom = 30;
                        tablesnewone.AddCell(companyname1);


                        PdfPCell cellsignature = new PdfPCell(new Phrase("Authorised Signature", FontFactory.GetFont(fontsyle, Fontsize + 2, Font.NORMAL, BaseColor.BLACK)));
                        cellsignature.HorizontalAlignment = 2;
                        cellsignature.Colspan = 19;
                        cellsignature.Border = 0;
                        tablesnewone.AddCell(cellsignature);

                        document.NewPage();

                        document.Add(tablesnewone);
                    }




                    string filename = ddlcname.SelectedItem.Text + "FormF(LeaveWages).pdf";

                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
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

        protected void btnformXIII_Click(object sender, EventArgs e)
        {
            int totalfonts = FontFactory.RegisterDirectory("c:\\WINDOWS\\fonts");
            //StringBuilder sa = new StringBuilder();
            foreach (string fontname in FontFactory.RegisteredFonts)
            {
                //sa.Append(fontname + "\n");
            }
            Font FontStyle1 = FontFactory.GetFont("Tahoma", BaseFont.CP1252, BaseFont.EMBEDDED, 10, Font.NORMAL, BaseColor.BLACK);
            Font FontStyle2 = FontFactory.GetFont("Tahoma", BaseFont.CP1252, BaseFont.EMBEDDED, 12, Font.BOLD, BaseColor.BLACK);
            Font FontStyle3 = FontFactory.GetFont("Tahoma", BaseFont.CP1252, BaseFont.EMBEDDED, 10, Font.BOLD, BaseColor.BLACK);
            int Fontsize = 10;
            string fontsyle = "verdana";
            string EmpmName = "";
            string Empid = "";
            string sino = "";
            string empfathername = "";
            string desgn = "";
            string SEX = "";
            string permenantadress = "";
            string presentadress = "";
            string typeofwork = "";
            string EmpDtofLeaving = "";
            MemoryStream ms = new MemoryStream();


            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string addressData = "";
            string Age = "";
            string ClientNameForms = "";
            string clientadress = "";
            string EmpDtofJoining = "";
            string dateofleaving = "";
            string Formdate = "";
            string DtofBirth = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString();
            string selectmonth = string.Empty;

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month + Year.Substring(2, 2));
            ht.Add("@yearval", Year);
            ht.Add("@type", type);
            ht.Add("@monthval", month);
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;



            if (dt.Rows.Count > 0)
            {

                addressData = dt.Rows[0]["clientaddress"].ToString();

                //string tempDescription = dt.Rows[0]["Description"].ToString();
                Document document = new Document(PageSize.LEGAL.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);



                PdfPTable table2 = new PdfPTable(13);
                table2.TotalWidth = 980f;
                table2.LockedWidth = true;
                float[] width1 = new float[] { 1f, 3f, 1f, 3f, 2f, 3f, 3f, 2f, 3f, 2f, 2f, 2f, 2f };
                table2.SetWidths(width1);
                PdfPCell cell;

                companyName = dt.Rows[0]["companyName"].ToString();
                companyAddress = dt.Rows[0]["companyAddress"].ToString();
                string TypeOfWork = dt.Rows[0]["TypeOfWork"].ToString();

                #region for headers
                cell = new PdfPCell(new Phrase("FORM - XIII", FontStyle2));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 13;
                cell.Border = 0;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase("(See rule 75)", FontStyle1));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 13;
                cell.Border = 0;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase("REGISTER OF WORKMEN EMPLOYED BY CONTRACTOR", FontStyle2));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 13;
                cell.Border = 0;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase("Name and address of Contractor:", FontStyle3));
                cell.HorizontalAlignment = 0;
                cell.Colspan = 4;
                cell.Border = 0;
                table2.AddCell(cell);
                cell = new PdfPCell(new Phrase(": " + companyName + "\n" + companyAddress, FontStyle1));
                cell.HorizontalAlignment = 0;
                cell.Colspan = 10;
                cell.Border = 0;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase("Name and address of the establishment under which work is carried on ", FontStyle3));
                cell.HorizontalAlignment = 0;
                cell.Colspan = 4;
                cell.Border = 0;
                table2.AddCell(cell);
                cell = new PdfPCell(new Phrase(": " + ddlcname.SelectedItem, FontStyle1));
                cell.HorizontalAlignment = 0;
                cell.Colspan = 10;
                cell.Border = 0;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase("Nature and location of work", FontStyle3));
                cell.HorizontalAlignment = 0;
                cell.Colspan = 3;
                cell.Border = 0;
                table2.AddCell(cell);
                cell = new PdfPCell(new Phrase(": SECURITY\n" + addressData, FontStyle1));
                cell.HorizontalAlignment = 0;
                cell.Colspan = 10;
                cell.Border = 0;
                table2.AddCell(cell);

                document.Add(table2);

                PdfPTable table1 = new PdfPTable(14);
                table1.TotalWidth = 980f;
                table1.LockedWidth = true;
                table1.HeaderRows = 1;
                float[] width = new float[] { 0.8f, 1.7f, 3f, 1.7f, 1f, 2f, 3f, 3f, 2f, 1.7f, 2f, 2f, 2f, 2.5f };
                table1.SetWidths(width);

                cell = new PdfPCell(new Phrase("S.No", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("EMP ID", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("NAME AND ADRESS OF WORKMEN", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("DATE OF BIRTH", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("SEX", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("FATHER / HUSBAND NAME", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("DESIGNATION", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("PERMANENT ADDRESS", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("LOCAL ADDRESS", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("DATE OF JOINING", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("SIGNATURE OF WORKMEN", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("DATE OF TERMINATION OF SERVICE", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("REASONS FOR TERMINATION", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Remarks", FontStyle3));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table1.AddCell(cell);

                #endregion

                int Snos = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sino = dt.Rows[i]["SlNo"].ToString();
                    EmpmName = dt.Rows[i]["EmpmName"].ToString();
                    Empid = dt.Rows[i]["Empid"].ToString();
                    Age = dt.Rows[i]["Age"].ToString();
                    empfathername = dt.Rows[i]["empfathername"].ToString();
                    desgn = dt.Rows[i]["desgn"].ToString();
                    SEX = dt.Rows[i]["EmpMaritalStatus"].ToString();
                    DtofBirth = dt.Rows[i]["EmpDtofBirth"].ToString();
                    EmpDtofJoining = dt.Rows[i]["EmpDtofJoining"].ToString();

                    string prLmark = dt.Rows[i]["prLmark"].ToString();
                    string prTown = dt.Rows[i]["prTown"].ToString();
                    string prPostoffice = dt.Rows[i]["prPostoffice"].ToString();
                    string prTaluka = dt.Rows[i]["prTaluka"].ToString();
                    string prPoliceStation = dt.Rows[i]["prPoliceStation"].ToString();
                    string prCity = dt.Rows[i]["prCity"].ToString();
                    string prstate = dt.Rows[i]["prstate"].ToString();
                    string prpincode = dt.Rows[i]["prpincode"].ToString();

                    string peLmark = dt.Rows[i]["peLmark"].ToString();
                    string peTown = dt.Rows[i]["peTown"].ToString();
                    string pePostoffice = dt.Rows[i]["pePostoffice"].ToString();
                    string peTaluka = dt.Rows[i]["peTaluka"].ToString();
                    string pePoliceStation = dt.Rows[i]["pePoliceStation"].ToString();
                    string peCity = dt.Rows[i]["peCity"].ToString();
                    string pestate = dt.Rows[i]["pestate"].ToString();
                    string pepincode = dt.Rows[i]["pepincode"].ToString();
                    string empRemarks = dt.Rows[i]["empRemarks"].ToString();

                    string presentadd = prLmark + ' ' + prTown + ' ' + prPostoffice + ' ' + prTaluka + ' ' + prPoliceStation + ' ' + prCity + ' ' + prstate + ' ' + prpincode;
                    string permanentadd = peLmark + ' ' + peTown + ' ' + pePostoffice + ' ' + peTaluka + ' ' + pePoliceStation + ' ' + peCity + ' ' + pestate + ' ' + pepincode;


                    cell = new PdfPCell(new Phrase(Snos.ToString(), FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(Empid, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(EmpmName, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(DtofBirth, FontStyle1));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(SEX, FontStyle1));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(empfathername, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(desgn, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(permanentadd, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(presentadd, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("  " + EmpDtofJoining, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("", FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("  " + EmpDtofLeaving, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    if (EmpDtofLeaving.Length > 0)
                    {
                        cell = new PdfPCell(new Phrase("    Resigned", FontStyle1));
                        cell.HorizontalAlignment = 0;
                        cell.Colspan = 1;
                        table1.AddCell(cell);
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase("", FontStyle1));
                        cell.HorizontalAlignment = 0;
                        cell.Colspan = 1;
                        table1.AddCell(cell);
                    }

                    cell = new PdfPCell(new Phrase(empRemarks, FontStyle1));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    Snos++;
                }
                document.Add(table1);

                string filename = "Form XIII.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There is no details for selected client ');", true);

            }
        }

        protected void BtnformXX_Click(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string FontStyle = "verdana";
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string addressData = "";
            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString().Substring(2, 2);



            var list = new List<string>();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkempid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblempid = GVListEmployees.Rows[i].FindControl("lblempid") as Label;

                    if (chkempid.Checked == true)
                    {
                        list.Add(lblempid.Text);
                    }
                }
            }



            DataTable dtEmpids = new DataTable();
            dtEmpids.Columns.Add("Empid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtEmpids.NewRow();
                    row["Empid"] = s;
                    dtEmpids.Rows.Add(row);
                }
            }

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@month", month + Year);
            ht.Add("@type", type);
            ht.Add("@EmpIds", dtEmpids);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;



            if (dt.Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                string tempDescription = dt.Rows[0]["Description"].ToString();
                string typeofwork = dt.Rows[0]["typeofwork"].ToString();
                addressData = dt.Rows[0]["ClientName"].ToString();
                string ClientAddress = dt.Rows[0]["ClientAddress"].ToString();
                companyName = dt.Rows[0]["companyname"].ToString();
                companyAddress = dt.Rows[0]["companyAddress"].ToString();

                #region


                PdfPTable tblenew = new PdfPTable(13);
                tblenew.TotalWidth = 950f;
                tblenew.LockedWidth = true;
                float[] width = new float[] { 1.5f, 3f, 3f, 3f, 2f, 2f, 3f, 3f, 2f, 2f, 2f, 2f, 2f };
                tblenew.SetWidths(width);

                PdfPCell cellspace = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
                cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellspace.Colspan = 13;
                cellspace.Border = 0;
                cellspace.PaddingTop = 0;



                PdfPCell cellreturn12a1 = new PdfPCell(new Phrase("REGISTER OF DEDUCTIONS FOR DAMAGE OR LOSS", FontFactory.GetFont(FontStyle, 12, Font.BOLD, BaseColor.BLACK)));
                cellreturn12a1.HorizontalAlignment = 1;
                cellreturn12a1.Colspan = 13;
                cellreturn12a1.Border = 0;
                tblenew.AddCell(cellreturn12a1);


                PdfPCell cellHead = new PdfPCell(new Phrase("Form-XX See Rule-78 (1) (a) (ii)", FontFactory.GetFont(FontStyle, Fontsize + 4, Font.BOLD, BaseColor.BLACK)));
                cellHead.HorizontalAlignment = 1;
                cellHead.Colspan = 13;
                cellHead.Border = 0;
                tblenew.AddCell(cellHead);

                PdfPCell cellNamess401 = new PdfPCell(new Phrase("Name and address of contractor", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess401.HorizontalAlignment = 0;
                cellNamess401.Colspan = 3;
                cellNamess401.Border = 0;
                cellNamess401.PaddingTop = 10;
                tblenew.AddCell(cellNamess401);
                PdfPCell cellNamess431 = new PdfPCell(new Phrase(": " + companyName + "\n" + companyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNamess431.HorizontalAlignment = 0;
                cellNamess431.Colspan = 10;
                cellNamess431.Border = 0;
                cellNamess431.SetLeading(0, 1.3f);
                cellNamess431.PaddingTop = 10;
                tblenew.AddCell(cellNamess431);


                PdfPCell cellNamess421 = new PdfPCell(new Phrase("Nature and location of work", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess421.HorizontalAlignment = 0;
                cellNamess421.Colspan = 3;
                cellNamess421.Border = 0;
                tblenew.AddCell(cellNamess421);
                PdfPCell cellNamess441 = new PdfPCell(new Phrase(": " + typeofwork, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNamess441.HorizontalAlignment = 0;
                cellNamess441.Colspan = 10;
                cellNamess441.Border = 0;
                tblenew.AddCell(cellNamess441);

                tblenew.AddCell(cellspace);


                PdfPCell cellNamess411 = new PdfPCell(new Phrase("Name and address of estabilishment in/\nunder which contract is carried on", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess411.HorizontalAlignment = 0;
                cellNamess411.Colspan = 3;
                cellNamess411.Border = 0;
                cellNamess411.PaddingTop = 10;
                tblenew.AddCell(cellNamess411);
                PdfPCell nameandadd = new PdfPCell(new Phrase(": " + addressData, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                nameandadd.HorizontalAlignment = 0;
                nameandadd.Colspan = 10;
                nameandadd.Border = 0;
                nameandadd.PaddingTop = 10;
                tblenew.AddCell(nameandadd);

                PdfPCell cellNamess451 = new PdfPCell(new Phrase("Name and address of principal employer", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess451.HorizontalAlignment = 0;
                cellNamess451.Colspan = 3;
                cellNamess451.Border = 0;
                tblenew.AddCell(cellNamess451);
                PdfPCell cellpricemp = new PdfPCell(new Phrase(": " + ClientAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellpricemp.HorizontalAlignment = 0;
                cellpricemp.Colspan = 10;
                cellpricemp.Border = 0;
                tblenew.AddCell(cellpricemp);

                tblenew.AddCell(cellspace);
                tblenew.AddCell(cellspace);
                tblenew.AddCell(cellspace);

                PdfPCell cellssNames471 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellssNames471.HorizontalAlignment = 0;
                cellssNames471.Colspan = 9;
                tblenew.AddCell(cellssNames471);
                PdfPCell cellsNamesss471 = new PdfPCell(new Phrase(" Date of recovery ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellsNamesss471.HorizontalAlignment = 1;
                cellsNamesss471.Colspan = 3;
                tblenew.AddCell(cellsNamesss471);
                PdfPCell cellsssNames471 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellsssNames471.HorizontalAlignment = 0;
                cellsssNames471.Colspan = 1;
                tblenew.AddCell(cellsssNames471);


                PdfPCell cellNamess461 = new PdfPCell(new Phrase("Sl.No. ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess461.HorizontalAlignment = 1;
                cellNamess461.Colspan = 0;
                tblenew.AddCell(cellNamess461);
                PdfPCell cellNamess471 = new PdfPCell(new Phrase("Name of Employee", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess471.HorizontalAlignment = 1;
                cellNamess471.Colspan = 0;
                tblenew.AddCell(cellNamess471);
                PdfPCell cellNamess481 = new PdfPCell(new Phrase("Father's/\nhusband's name ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess481.HorizontalAlignment = 1;
                cellNamess481.Colspan = 0;
                tblenew.AddCell(cellNamess481);
                PdfPCell cellNamess501 = new PdfPCell(new Phrase("Nature of employment/\nDesignation ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess501.HorizontalAlignment = 1;
                cellNamess501.Colspan = 0;
                tblenew.AddCell(cellNamess501);
                PdfPCell cellNamess51 = new PdfPCell(new Phrase("Particulars of Damagse or Loss", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess51.HorizontalAlignment = 1;
                cellNamess51.Colspan = 0;
                tblenew.AddCell(cellNamess51);
                PdfPCell cellNamess521 = new PdfPCell(new Phrase("Date of damage or Loss", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess521.HorizontalAlignment = 1;
                cellNamess521.Colspan = 0;
                tblenew.AddCell(cellNamess521);
                PdfPCell cellNamess531 = new PdfPCell(new Phrase("Whether Workman showed cause against deduction", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess531.HorizontalAlignment = 1;
                cellNamess531.Colspan = 0;
                tblenew.AddCell(cellNamess531);
                PdfPCell cellNamess541 = new PdfPCell(new Phrase("Name of person in whose presence employee's explaination was heard", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess541.HorizontalAlignment = 1;
                cellNamess541.Colspan = 0;
                tblenew.AddCell(cellNamess541);
                PdfPCell cellNamess551 = new PdfPCell(new Phrase("Amount of  deduction imposed", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess551.HorizontalAlignment = 1;
                cellNamess551.Colspan = 0;
                tblenew.AddCell(cellNamess551);
                PdfPCell cellNamess561 = new PdfPCell(new Phrase("No. of instalments", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess561.HorizontalAlignment = 1;
                cellNamess561.Colspan = 0;
                tblenew.AddCell(cellNamess561);
                PdfPCell cellNamess5510 = new PdfPCell(new Phrase("First Instalments", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess5510.HorizontalAlignment = 1;
                cellNamess5510.Colspan = 0;
                tblenew.AddCell(cellNamess5510);
                PdfPCell cellNamess5610 = new PdfPCell(new Phrase("Last Instalments", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess5610.HorizontalAlignment = 1;
                cellNamess5610.Colspan = 0;
                tblenew.AddCell(cellNamess5610);
                PdfPCell cellNamess571 = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNamess571.HorizontalAlignment = 1;
                cellNamess571.Colspan = 0;
                tblenew.AddCell(cellNamess571);
                PdfPCell cell;

                #region for empty Rows
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                //2ndline
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);


                #endregion

                //values
                PdfPCell cellssNames4710 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellssNames4710.HorizontalAlignment = 1;
                cellssNames4710.Colspan = 0;
                tblenew.AddCell(cellssNames4710);
                PdfPCell cellsNamesss4710 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamesss4710.HorizontalAlignment = 1;
                cellsNamesss4710.Colspan = 0;
                tblenew.AddCell(cellsNamesss4710);
                PdfPCell cellsNamess4810 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess4810.HorizontalAlignment = 1;
                cellsNamess4810.Colspan = 0;
                tblenew.AddCell(cellsNamess4810);
                PdfPCell cellsNamess491 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess491.HorizontalAlignment = 1;
                cellsNamess491.Colspan = 0;
                tblenew.AddCell(cellsNamess491);
                PdfPCell cellsNamess501 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess501.HorizontalAlignment = 1;
                cellsNamess501.Colspan = 0;
                tblenew.AddCell(cellsNamess501);
                PdfPCell cellsNamess511 = new PdfPCell(new Phrase(GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellsNamess511.HorizontalAlignment = 1;
                cellsNamess511.Colspan = 0;
                tblenew.AddCell(cellsNamess511);
                PdfPCell cellsNamess521 = new PdfPCell(new Phrase("   ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess521.HorizontalAlignment = 1;
                cellsNamess521.Colspan = 0;
                tblenew.AddCell(cellsNamess521);
                PdfPCell cellsNamess531 = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess531.HorizontalAlignment = 1;
                cellsNamess531.Colspan = 0;
                tblenew.AddCell(cellsNamess531);
                PdfPCell cellsNamess541 = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess541.HorizontalAlignment = 1;
                cellsNamess541.Colspan = 0;
                tblenew.AddCell(cellsNamess541);
                PdfPCell cellsNamess551 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess551.HorizontalAlignment = 1;
                cellsNamess551.Colspan = 0;
                tblenew.AddCell(cellsNamess551);
                PdfPCell cellsNamess561 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess561.HorizontalAlignment = 1;
                cellsNamess561.Colspan = 0;
                tblenew.AddCell(cellsNamess561);
                PdfPCell cellsNamess571 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess571.HorizontalAlignment = 1;
                cellsNamess571.Colspan = 0;
                tblenew.AddCell(cellsNamess571);
                PdfPCell cellsNamess5711 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsNamess5711.HorizontalAlignment = 1;
                cellsNamess5711.Colspan = 0;
                tblenew.AddCell(cellsNamess5711);


                PdfPCell Cellss4710 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cellss4710.HorizontalAlignment = 1;
                Cellss4710.Colspan = 0;
                tblenew.AddCell(Cellss4710);
                PdfPCell Cellsss4710 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cellsss4710.HorizontalAlignment = 1;
                Cellsss4710.Colspan = 0;
                tblenew.AddCell(Cellsss4710);
                PdfPCell Cells4810 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cells4810.HorizontalAlignment = 1;
                Cells4810.Colspan = 0;
                tblenew.AddCell(Cells4810);
                PdfPCell Cells491 = new PdfPCell(new Phrase("There are no deductions on account of damage or loss caused by the employees during this month", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cells491.HorizontalAlignment = 1;
                Cells491.Colspan = 6;
                tblenew.AddCell(Cells491);
                PdfPCell Cells501 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cells501.HorizontalAlignment = 1;
                Cells501.Colspan = 0;
                tblenew.AddCell(Cells501);
                PdfPCell Cells511 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cells511.HorizontalAlignment = 1;
                Cells511.Colspan = 0;
                tblenew.AddCell(Cells511);
                PdfPCell Cells521 = new PdfPCell(new Phrase("   ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cells521.HorizontalAlignment = 1;
                Cells521.Colspan = 0;
                tblenew.AddCell(Cells521);
                PdfPCell Cells531 = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Cells531.HorizontalAlignment = 1;
                Cells531.Colspan = 0;
                tblenew.AddCell(Cells531);

                #region for empty  Rows
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                //lastline
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                tblenew.AddCell(cell);
                #endregion

                document.Add(tblenew);

                #endregion Basic Information of the Employee



                string filename = "formXX.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }

        protected void Btnesideclaration_Click(object sender, EventArgs e)
        {
            int Fontsize = 10;
            string FontStyle = "verdana";

            string strQrys = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQrys);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString();

            string selectmonth = "select eps.empid, (ed.Empfname +' ' + ed.EmpMname + ' ' + ed.emplname) as EmpmName,Designations.design as Desgn, " +
                      " ed.EmpFatherName,eps.ESIWages,(round(eps.esi,0)) as esi,(round(eps.ESIEmpr,0)) as ESIEmpr  from EmpPaySheet as Eps " +
                      " INNER JOIN EmpDetails ed ON Eps.EmpId = ed.EmpId " +
                      " INNER JOIN Designations ON Designations.designid=Eps.desgn " +
                      " AND Eps.ClientId='" + ddlclientid.SelectedValue + "'  AND Eps.Month=" + month + Year.Substring(2, 2) + "order by empid";



            DataTable dt = SqlHelper.Instance.GetTableByQuery(selectmonth);


            if (dt.Rows.Count > 0)
            {


                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL);

                // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                PdfPTable table = new PdfPTable(7);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] width = new float[] { 1.5f, 2.5f, 4f, 3f, 2f, 2f, 2.5f };
                table.SetWidths(width);
                PdfPCell cell;

                cell = new PdfPCell(new Phrase("The ESI Statement for the month of  " + GetMonthName() + " ' " + Year + " ,pertaining employees deployed at " + ddlcname.SelectedItem, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.SetLeading(0f, 1.5f);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 15;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("S.No.", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("ESI Number", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Name of the employee ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("ESI Gross Wages", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("ESI 1.75%", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("ESI 4.75%", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                //values
                float Forconvert = 0;
                int k = 1;
                float esiwages = 0;
                float esi = 0;
                float esiempr = 0;
                string empname = "";
                float totalesiwages = 0;
                float totalesiempr = 0;
                float totalesi = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + dt.Rows[i]["EmpId"].ToString() + "'";
                    string esiNo = "";
                    DataTable esiTable = SqlHelper.Instance.GetTableByQuery(strQry);
                    if (esiTable.Rows.Count > 0)
                    {
                        esiNo = esiTable.Rows[0]["EmpESINo"].ToString();
                    }
                    empname = dt.Rows[i]["empmname"].ToString();
                    esi = float.Parse(dt.Rows[i]["esi"].ToString());
                    esiwages = float.Parse(dt.Rows[i]["ESIWages"].ToString());
                    esiempr = float.Parse(dt.Rows[i]["ESIEmpr"].ToString());


                    cell = new PdfPCell(new Phrase(k.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(esiNo, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(empname, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(esiwages.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalesiwages += esiwages;
                    cell = new PdfPCell(new Phrase(esi.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalesi += esi;
                    cell = new PdfPCell(new Phrase(esiempr.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalesiempr += esiempr;
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    k++;
                }

                cell = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 3;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalesiwages.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalesi.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalesiempr.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("We hereby certify that the above mentioned employees were deployed in  " + ddlcname.SelectedItem + "  and covered under ESI Act.  We also confirm that all the above mentioned payments were correct to the best of our  knowledge.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 15;
                cell.SetLeading(0f, 1.5f);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("For :" + companyName, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 50;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Authorized Signatory  ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 50;
                table.AddCell(cell);

                document.Add(table);


                string filename = "Esideclaration.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }

        protected void Btnpfdeclaration_Click(object sender, EventArgs e)
        {
            int Fontsize = 10;
            string FontStyle = "verdana";

            string strQrys = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQrys);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string PFnoofForms = "";
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
                PFnoofForms = compInfo.Rows[0]["PFnoofForms"].ToString();
            }

            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString();

            string selectmonth = "select eps.empid, (ed.Empfname +' ' + ed.EmpMname + ' ' + ed.emplname) as EmpmName,ed.EmpUANNumber, " +
                      " case when DATEDIFF(year,ed.EmpDtofBirth, GETDATE()) < 58 then (round((eps.PFWages * 8.33 / 100), 0)) else '' end EPSpension," +
                      " ed.EmpFatherName,(eps.Basic+eps.DA) as basicda,eps.PFWages,eps.pf,eps.PFEmpr,round(((eps.Basic+eps.DA)*3.67/100),0) as pfemplyr from EmpPaySheet as Eps " +
                      " INNER JOIN EmpDetails ed ON Eps.EmpId = ed.EmpId " +
                      " AND Eps.ClientId='" + ddlclientid.SelectedValue + "'  AND Eps.Month=" + month + Year.Substring(2, 2) + "order by empid";



            DataTable dt = SqlHelper.Instance.GetTableByQuery(selectmonth);


            if (dt.Rows.Count > 0)
            {


                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL);

                // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                PdfPTable table = new PdfPTable(7);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] width = new float[] { 1.5f, 2.8f, 4f, 2.9f, 2f, 2f, 2.5f };
                table.SetWidths(width);
                PdfPCell cell;

                cell = new PdfPCell(new Phrase("The PF Statement for the month of  " + GetMonthName() + " ' " + Year + " ,pertaining employees deployed at " + ddlcname.SelectedItem, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.SetLeading(0f, 1.5f);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 15;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("S.No.", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("PF/UAN NO.", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Name of the employee ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Basic + DA", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("PF employee\n12%", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("PF employer\n(3.67%)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("EPS Pension\n(8.33%)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                //values
                float Forconvert = 0;
                int k = 1;
                float pfbasicda = 0;
                float pf = 0;
                float pfempr = 0;
                float EPSpension = 0;
                string empname = "";
                float totalbasicda = 0;
                float totalpfempr = 0;
                float totalpf = 0;
                float totalEPSpension = 0;
                float totalpfemplyr = 0;
                string EmpUANNumber = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + dt.Rows[i]["EmpId"].ToString() + "'";
                    string pfNo = "";
                    DataTable esiTable = SqlHelper.Instance.GetTableByQuery(strQry);
                    if (esiTable.Rows.Count > 0)
                    {
                        pfNo = esiTable.Rows[0]["EmpEpfNo"].ToString();
                    }
                    empname = dt.Rows[i]["empmname"].ToString();
                    pf = float.Parse(dt.Rows[i]["pf"].ToString());
                    pfbasicda = float.Parse(dt.Rows[i]["Basicda"].ToString());
                    pfempr = float.Parse(dt.Rows[i]["PFEmpr"].ToString());
                    EPSpension = float.Parse(dt.Rows[i]["EPSpension"].ToString());
                    totalpfemplyr = float.Parse(dt.Rows[i]["pfemplyr"].ToString());
                    EmpUANNumber = (dt.Rows[i]["EmpUANNumber"].ToString());

                    cell = new PdfPCell(new Phrase(k.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(EmpUANNumber, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(empname, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(pfbasicda.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalbasicda += pfbasicda;
                    cell = new PdfPCell(new Phrase(pf.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalpf += pf;
                    cell = new PdfPCell(new Phrase(totalpfemplyr.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalpfempr += pfempr;
                    cell = new PdfPCell(new Phrase(EPSpension.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalEPSpension += EPSpension;
                    k++;
                }

                cell = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 3;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalbasicda.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalpf.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalpfempr.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalEPSpension.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("We hereby certify that the above mentioned employees were deployed in  " + ddlcname.SelectedItem + "  and covered under PF Act.  We also confirm that all the above mentioned payments were correct to the best of our  knowledge.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 15;
                cell.SetLeading(0f, 1.5f);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("FOR " + companyName, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 50;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Authorized Signatory  ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 50;
                table.AddCell(cell);

                document.Add(table);


                string filename = "EPFdeclaration.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }

        protected void BtnformXXI_Click1(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string FontStyle = "verdana";


            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string addressData = "";
            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString().Substring(2, 2);


            var list = new List<string>();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkempid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblempid = GVListEmployees.Rows[i].FindControl("lblempid") as Label;

                    if (chkempid.Checked == true)
                    {
                        list.Add(lblempid.Text);
                    }
                }
            }




            DataTable dtEmpids = new DataTable();
            dtEmpids.Columns.Add("Empid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtEmpids.NewRow();
                    row["Empid"] = s;
                    dtEmpids.Rows.Add(row);
                }
            }

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@month", month + Year);
            ht.Add("@type", type);
            ht.Add("@EmpIds", dtEmpids);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;


            if (dt.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");


                string tempDescription = dt.Rows[0]["Description"].ToString();
                string ClientName = dt.Rows[0]["ClientName"].ToString();
                addressData = dt.Rows[0]["ClientAddress"].ToString();
                companyAddress = dt.Rows[0]["companyAddress"].ToString();
                companyName = dt.Rows[0]["companyName"].ToString();
                string typeofwork = dt.Rows[0]["typeofwork"].ToString();

                #region

                PdfPTable tablenew7 = new PdfPTable(12);
                tablenew7.TotalWidth = 950f;
                tablenew7.LockedWidth = true;
                float[] width = new float[] { 1f, 3f, 3f, 3f, 2f, 2f, 2f, 3.5f, 2f, 2f, 2f, 0.5f };
                tablenew7.SetWidths(width);

                PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
                cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellspace.Colspan = 12;
                cellspace.Border = 0;
                cellspace.PaddingTop = -5;

                PdfPCell cellHead = new PdfPCell(new Phrase("REGISTER OF FINES", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cellHead.HorizontalAlignment = 1;
                cellHead.Colspan = 12;
                cellHead.Border = 0;
                tablenew7.AddCell(cellHead);

                PdfPCell cellreturn123 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cellreturn123.HorizontalAlignment = 1;
                cellreturn123.Colspan = 12;
                cellreturn123.Border = 0;
                tablenew7.AddCell(cellreturn123);

                PdfPCell cellreturns1234 = new PdfPCell(new Phrase("FORM XXI See Rule-78 (1) (a) (ii) ", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.BOLD, BaseColor.BLACK)));
                cellreturns1234.HorizontalAlignment = 1;
                cellreturns1234.Colspan = 12;
                cellreturns1234.Border = 0;
                tablenew7.AddCell(cellreturns1234);

                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);

                PdfPCell cellNames40s = new PdfPCell(new Phrase("Name & Address of Contractor", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames40s.HorizontalAlignment = 0;
                cellNames40s.Colspan = 3;
                cellNames40s.Border = 0;
                tablenew7.AddCell(cellNames40s);
                PdfPCell cellNames43s = new PdfPCell(new Phrase(": " + companyName, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames43s.HorizontalAlignment = 0;
                cellNames43s.Colspan = 9;
                cellNames43s.Border = 0;
                tablenew7.AddCell(cellNames43s);



                PdfPCell cellNames42s = new PdfPCell(new Phrase("Nature and Location of Work", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames42s.HorizontalAlignment = 0;
                cellNames42s.Colspan = 3;
                cellNames42s.Border = 0;
                tablenew7.AddCell(cellNames42s);
                PdfPCell cellNames44s = new PdfPCell(new Phrase(": " + typeofwork, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames44s.HorizontalAlignment = 0;
                cellNames44s.Colspan = 9;
                cellNames44s.Border = 0;
                tablenew7.AddCell(cellNames44s);

                PdfPCell cellNames41s = new PdfPCell(new Phrase("Name &  Address of  Establishment  In / under  which contract is carried on", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames41s.HorizontalAlignment = 0;
                cellNames41s.Colspan = 3;
                cellNames41s.Border = 0;
                tablenew7.AddCell(cellNames41s);
                PdfPCell cellNames434 = new PdfPCell(new Phrase(": " + ClientName, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames434.HorizontalAlignment = 0;
                cellNames434.Colspan = 9;
                cellNames434.Border = 0;
                tablenew7.AddCell(cellNames434);

                tablenew7.AddCell(cellspace);

                PdfPCell cellNames45s = new PdfPCell(new Phrase("Name &  Address of Principal Employer", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames45s.HorizontalAlignment = 0;
                cellNames45s.Colspan = 3;
                cellNames45s.Border = 0;
                tablenew7.AddCell(cellNames45s);
                PdfPCell cellNames45ss = new PdfPCell(new Phrase(": " + addressData, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames45ss.HorizontalAlignment = 0;
                cellNames45ss.Colspan = 9;
                cellNames45ss.Border = 0;
                tablenew7.AddCell(cellNames45ss);

                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);
                document.Add(tablenew7);
                PdfPTable table = new PdfPTable(12);
                table.TotalWidth = 950f;
                table.LockedWidth = true;
                table.HeaderRows = 1;
                float[] widths = new float[] { 1f, 3f, 3f, 3f, 2f, 2f, 2f, 3.5f, 2f, 2f, 0.5f, 2f };
                table.SetWidths(widths);

                PdfPCell cellNames46s = new PdfPCell(new Phrase("Sl.No. ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames46s.HorizontalAlignment = 1;
                cellNames46s.Colspan = 0;
                table.AddCell(cellNames46s);
                PdfPCell cellNames47s = new PdfPCell(new Phrase("Name of workman", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames47s.HorizontalAlignment = 1;
                cellNames47s.Colspan = 0;
                table.AddCell(cellNames47s);
                PdfPCell cellNames48s = new PdfPCell(new Phrase("Father's/ Husband's  Name", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames48s.HorizontalAlignment = 1;
                cellNames48s.Colspan = 0;
                table.AddCell(cellNames48s);
                PdfPCell cellNames49s = new PdfPCell(new Phrase("Designation/nature of employment", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames49s.HorizontalAlignment = 1;
                cellNames49s.Colspan = 0;
                table.AddCell(cellNames49s);
                PdfPCell cellNames50s = new PdfPCell(new Phrase("Act/Ommision for which fine imposed", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames50s.HorizontalAlignment = 1;
                cellNames50s.Colspan = 0;
                table.AddCell(cellNames50s);
                PdfPCell cellNames51s = new PdfPCell(new Phrase("Date of Offence", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames51s.HorizontalAlignment = 1;
                cellNames51s.Colspan = 0;
                table.AddCell(cellNames51s);
                PdfPCell cellNames52s = new PdfPCell(new Phrase("Name of person in whose presence employee's explanation was heared", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames52s.HorizontalAlignment = 1;
                cellNames52s.Colspan = 0;
                table.AddCell(cellNames52s);
                PdfPCell cellNames54s = new PdfPCell(new Phrase("Wage period and wage payable", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames54s.HorizontalAlignment = 1;
                cellNames54s.Colspan = 0;
                table.AddCell(cellNames54s);
                PdfPCell cellNames55s = new PdfPCell(new Phrase("Amount of fine Imposed", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames55s.HorizontalAlignment = 1;
                cellNames55s.Colspan = 0;
                table.AddCell(cellNames55s);
                PdfPCell cellNames57s = new PdfPCell(new Phrase("Date on which fine Realised", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames57s.HorizontalAlignment = 1;
                cellNames57s.Colspan = 2;
                table.AddCell(cellNames57s);
                PdfPCell cellNames53 = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames53.HorizontalAlignment = 1;
                cellNames53.Colspan = 0;
                table.AddCell(cellNames53);
                PdfPCell cell;
                //values
                #region for empty Rows
                cell = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("3", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("4", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("5", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("6", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("7", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("8", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("9", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("11", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);


                //
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);


                #endregion


                #region for values
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                //
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("NO FINES WERE IMPOSSED OR COLLECTED FROM ANY OF THE EMPLOYEE DURING THIS MONTH", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 5;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);



                #endregion


                #region for empty Rows
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                //

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);


                #endregion

                document.Add(table);

                #endregion Basic Information of the Employee



                string filename = "FormXXI.pdf";
                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }

        protected void BtnformXXII_Click1(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string FontStyle = "verdana";


            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string addressData = "";
            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString().Substring(2, 2);


            var list = new List<string>();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkempid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblempid = GVListEmployees.Rows[i].FindControl("lblempid") as Label;

                    if (chkempid.Checked == true)
                    {
                        list.Add(lblempid.Text);
                    }
                }
            }




            DataTable dtEmpids = new DataTable();
            dtEmpids.Columns.Add("Empid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtEmpids.NewRow();
                    row["Empid"] = s;
                    dtEmpids.Rows.Add(row);
                }
            }

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@month", month + Year);
            ht.Add("@type", type);
            ht.Add("@EmpIds", dtEmpids);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;

            if (dt.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                string tempDescription = dt.Rows[0]["Description"].ToString();
                companyName = dt.Rows[0]["companyName"].ToString();
                companyAddress = dt.Rows[0]["companyAddress"].ToString();
                addressData = dt.Rows[0]["ClientAddress"].ToString();
                string typeofwork = dt.Rows[0]["typeofwork"].ToString();
                string ClientName = dt.Rows[0]["ClientName"].ToString();

                #region

                PdfPTable tablenew7 = new PdfPTable(12);
                tablenew7.TotalWidth = 950f;
                tablenew7.LockedWidth = true;
                float[] width = new float[] { 1f, 3f, 3f, 3f, 2f, 2f, 2f, 3.5f, 2f, 2f, 2f, 0.5f };
                tablenew7.SetWidths(width);

                PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
                cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellspace.Colspan = 12;
                cellspace.Border = 0;
                cellspace.PaddingTop = -5;

                PdfPCell cellHead = new PdfPCell(new Phrase("REGISTER OF ADVANCES ", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cellHead.HorizontalAlignment = 1;
                cellHead.Colspan = 12;
                cellHead.Border = 0;
                tablenew7.AddCell(cellHead);

                PdfPCell cellreturn123 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cellreturn123.HorizontalAlignment = 1;
                cellreturn123.Colspan = 12;
                cellreturn123.Border = 0;
                tablenew7.AddCell(cellreturn123);

                PdfPCell cellreturns1234 = new PdfPCell(new Phrase("FORM XXII See Rule-78 (1) (a) (iii) ", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.BOLD, BaseColor.BLACK)));
                cellreturns1234.HorizontalAlignment = 1;
                cellreturns1234.Colspan = 12;
                cellreturns1234.Border = 0;
                tablenew7.AddCell(cellreturns1234);

                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);


                PdfPCell cellNames40s = new PdfPCell(new Phrase("Name & Address of Contractor", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames40s.HorizontalAlignment = 0;
                cellNames40s.Colspan = 3;
                cellNames40s.Border = 0;
                tablenew7.AddCell(cellNames40s);
                PdfPCell cellNames43s = new PdfPCell(new Phrase(": " + companyName, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames43s.HorizontalAlignment = 0;
                cellNames43s.Colspan = 9;
                cellNames43s.Border = 0;
                tablenew7.AddCell(cellNames43s);



                PdfPCell cellNames42s = new PdfPCell(new Phrase("Nature and Location of Work", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames42s.HorizontalAlignment = 0;
                cellNames42s.Colspan = 3;
                cellNames42s.Border = 0;
                tablenew7.AddCell(cellNames42s);
                PdfPCell cellNames44s = new PdfPCell(new Phrase(": " + typeofwork, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames44s.HorizontalAlignment = 0;
                cellNames44s.Colspan = 9;
                cellNames44s.Border = 0;
                tablenew7.AddCell(cellNames44s);

                PdfPCell cellNames41s = new PdfPCell(new Phrase("Name &  Address of  Establishment  In / under  which contract is carried on", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames41s.HorizontalAlignment = 0;
                cellNames41s.Colspan = 3;
                cellNames41s.Border = 0;
                tablenew7.AddCell(cellNames41s);
                PdfPCell cellNames434 = new PdfPCell(new Phrase(": " + ClientName, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames434.HorizontalAlignment = 0;
                cellNames434.Colspan = 9;
                cellNames434.Border = 0;
                tablenew7.AddCell(cellNames434);

                tablenew7.AddCell(cellspace);

                PdfPCell cellNames45s = new PdfPCell(new Phrase("Name &  Address of Principal Employer", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames45s.HorizontalAlignment = 0;
                cellNames45s.Colspan = 3;
                cellNames45s.Border = 0;
                tablenew7.AddCell(cellNames45s);

                PdfPCell cellNames45ss = new PdfPCell(new Phrase(": " + addressData, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames45ss.HorizontalAlignment = 0;
                cellNames45ss.Colspan = 9;
                cellNames45ss.Border = 0;
                tablenew7.AddCell(cellNames45ss);

                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);

                document.Add(tablenew7);

                PdfPTable table = new PdfPTable(12);
                table.TotalWidth = 950f;
                table.LockedWidth = true;
                table.HeaderRows = 1;
                float[] widths = new float[] { 1f, 3f, 3f, 3f, 2f, 2f, 2f, 3.5f, 2f, 2f, 2f, 0.5f };
                table.SetWidths(widths);
                #region

                PdfPCell cellNames4s7 = new PdfPCell(new Phrase("Sl.No. ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames4s7.HorizontalAlignment = 1;
                cellNames4s7.Colspan = 0;
                table.AddCell(cellNames4s7);
                PdfPCell cellNames4s8 = new PdfPCell(new Phrase("Name Of the Employee", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames4s8.HorizontalAlignment = 1;
                cellNames4s8.Colspan = 0;
                table.AddCell(cellNames4s8);
                PdfPCell cellNames4s9 = new PdfPCell(new Phrase("Father's/Husband's  Name", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames4s9.HorizontalAlignment = 1;
                cellNames4s9.Colspan = 0;
                table.AddCell(cellNames4s9);
                PdfPCell cellNames4s0 = new PdfPCell(new Phrase("Nature of Employment/ Designation", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames4s0.HorizontalAlignment = 1;
                cellNames4s0.Colspan = 0;
                table.AddCell(cellNames4s0);
                PdfPCell cellNames5s0 = new PdfPCell(new Phrase("Wage Peroid and wages Payable", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s0.HorizontalAlignment = 1;
                cellNames5s0.Colspan = 0;
                table.AddCell(cellNames5s0);
                PdfPCell cellNames5s1 = new PdfPCell(new Phrase("Date and amount of advance given", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s1.HorizontalAlignment = 1;
                cellNames5s1.Colspan = 0;
                table.AddCell(cellNames5s1);
                PdfPCell cellNames5s2 = new PdfPCell(new Phrase("Purpose(s) for Which advance mace", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s2.HorizontalAlignment = 1;
                cellNames5s2.Colspan = 0;
                table.AddCell(cellNames5s2);
                PdfPCell cellNames5s3 = new PdfPCell(new Phrase("No of Instalments by which advance to be repaid", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s3.HorizontalAlignment = 1;
                cellNames5s3.Colspan = 0;
                table.AddCell(cellNames5s3);
                PdfPCell cellNames5s4 = new PdfPCell(new Phrase("Date and amount of each instalment was paid", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s4.HorizontalAlignment = 1;
                cellNames5s4.Colspan = 0;
                table.AddCell(cellNames5s4);
                PdfPCell cellNames5s5 = new PdfPCell(new Phrase("Date on which last instalment was repaid", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s5.HorizontalAlignment = 1;
                cellNames5s5.Colspan = 0;
                table.AddCell(cellNames5s5);
                PdfPCell cellNames5s6 = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s6.HorizontalAlignment = 1;
                cellNames5s6.Colspan = 2;
                table.AddCell(cellNames5s6);
                #endregion
                PdfPCell cell;

                #region for empty Rows

                cell = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("3", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("4", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("5", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("6", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("7", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("8", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("9", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);


                //
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);


                #endregion


                #region for values
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);

                //
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("NO EMPLOYEE WAS PAID ANY ADVANCE DURING THIS MONTH", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 5;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);



                #endregion


                #region for empty Rows
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);

                //

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                table.AddCell(cell);


                #endregion
                document.Add(table);

                #endregion Basic Information of the Employee


                document.NewPage();

                string filename = "FormXXII.pdf";


                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }

        protected void BtnformXXIII_Click1(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string FontStyle = "verdana";


            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string addressData = "";
            string Formdate = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString().Substring(2, 2);


            var list = new List<string>();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkempid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblempid = GVListEmployees.Rows[i].FindControl("lblempid") as Label;

                    if (chkempid.Checked == true)
                    {
                        list.Add(lblempid.Text);
                    }
                }
            }




            DataTable dtEmpids = new DataTable();
            dtEmpids.Columns.Add("Empid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtEmpids.NewRow();
                    row["Empid"] = s;
                    dtEmpids.Rows.Add(row);
                }
            }

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@month", month + Year);
            ht.Add("@type", type);
            ht.Add("@EmpIds", dtEmpids);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;



            if (dt.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");
                string tempDescription = dt.Rows[0]["Description"].ToString();
                companyAddress = dt.Rows[0]["companyAddress"].ToString();
                companyName = dt.Rows[0]["companyName"].ToString();
                addressData = dt.Rows[0]["clientaddress"].ToString();
                string ClientName = dt.Rows[0]["ClientName"].ToString();
                string typeofwork = dt.Rows[0]["typeofwork"].ToString();

                #region

                PdfPTable tablenew7 = new PdfPTable(12);
                tablenew7.TotalWidth = 950f;
                tablenew7.LockedWidth = true;
                float[] width = new float[] { 1f, 3f, 3f, 1.5f, 2f, 2f, 3f, 2f, 2f, 2f, 2f, 3f };
                tablenew7.SetWidths(width);

                PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
                cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellspace.Colspan = 12;
                cellspace.Border = 0;
                cellspace.PaddingTop = -5;

                PdfPCell cellHead = new PdfPCell(new Phrase("REGISTER OF OVERTIME ", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cellHead.HorizontalAlignment = 1;
                cellHead.Colspan = 12;
                cellHead.Border = 0;
                tablenew7.AddCell(cellHead);

                PdfPCell cellreturn123 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cellreturn123.HorizontalAlignment = 1;
                cellreturn123.Colspan = 12;
                cellreturn123.Border = 0;
                tablenew7.AddCell(cellreturn123);

                PdfPCell cellreturns1234 = new PdfPCell(new Phrase("FORM-XXIII See Rule 78 (1) (a) (iii)", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.BOLD, BaseColor.BLACK)));
                cellreturns1234.HorizontalAlignment = 1;
                cellreturns1234.Colspan = 12;
                cellreturns1234.Border = 0;
                tablenew7.AddCell(cellreturns1234);

                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);

                PdfPCell cellNames40s = new PdfPCell(new Phrase("Name and address of contractor", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames40s.HorizontalAlignment = 0;
                cellNames40s.Colspan = 3;
                cellNames40s.Border = 0;
                tablenew7.AddCell(cellNames40s);
                PdfPCell cellNames43s = new PdfPCell(new Phrase(": " + companyName + "\n" + companyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames43s.HorizontalAlignment = 0;
                cellNames43s.Colspan = 9;
                cellNames43s.Border = 0;
                tablenew7.AddCell(cellNames43s);

                PdfPCell cellNames41s = new PdfPCell(new Phrase("Name and address of establishment in/under	which contract is carried on", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames41s.HorizontalAlignment = 0;
                cellNames41s.Colspan = 3;
                cellNames41s.Border = 0;
                tablenew7.AddCell(cellNames41s);
                PdfPCell cellNames434 = new PdfPCell(new Phrase(": " + ClientName, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames434.HorizontalAlignment = 0;
                cellNames434.Colspan = 9;
                cellNames434.Border = 0;
                tablenew7.AddCell(cellNames434);

                tablenew7.AddCell(cellspace);

                PdfPCell cellNames42s = new PdfPCell(new Phrase("Nature and location of work", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames42s.HorizontalAlignment = 0;
                cellNames42s.Colspan = 3;
                cellNames42s.Border = 0;
                tablenew7.AddCell(cellNames42s);
                PdfPCell cellNames44s = new PdfPCell(new Phrase(": " + typeofwork, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames44s.HorizontalAlignment = 0;
                cellNames44s.Colspan = 9;
                cellNames44s.Border = 0;
                tablenew7.AddCell(cellNames44s);

                PdfPCell cellNames45s = new PdfPCell(new Phrase("Name and address of principal employer", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames45s.HorizontalAlignment = 0;
                cellNames45s.Colspan = 3;
                cellNames45s.Border = 0;
                tablenew7.AddCell(cellNames45s);
                PdfPCell cellNames45ss = new PdfPCell(new Phrase(": " + addressData, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames45ss.HorizontalAlignment = 0;
                cellNames45ss.Colspan = 9;
                cellNames45ss.Border = 0;
                tablenew7.AddCell(cellNames45ss);

                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);
                tablenew7.AddCell(cellspace);
                document.Add(tablenew7);
                PdfPTable table = new PdfPTable(12);
                table.TotalWidth = 950f;
                table.LockedWidth = true;
                table.HeaderRows = 1;
                float[] widths = new float[] { 1f, 3f, 3f, 2f, 3f, 2f, 3f, 2f, 2f, 2f, 2f, 2f };
                table.SetWidths(widths);
                #region

                PdfPCell cellNames4s7 = new PdfPCell(new Phrase("Sl.No. ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames4s7.HorizontalAlignment = 1;
                cellNames4s7.Colspan = 0;
                table.AddCell(cellNames4s7);
                PdfPCell cellNames4s8 = new PdfPCell(new Phrase("Name of workmen", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames4s8.HorizontalAlignment = 1;
                cellNames4s8.Colspan = 0;
                table.AddCell(cellNames4s8);
                PdfPCell cellNames4s9 = new PdfPCell(new Phrase("Father's/husband's name ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames4s9.HorizontalAlignment = 1;
                cellNames4s9.Colspan = 0;
                table.AddCell(cellNames4s9);
                PdfPCell cellsex = new PdfPCell(new Phrase("Sex", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellsex.HorizontalAlignment = 1;
                cellsex.Colspan = 0;
                table.AddCell(cellsex);
                PdfPCell cellNames4s0 = new PdfPCell(new Phrase("Designation/nature of employment", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames4s0.HorizontalAlignment = 1;
                cellNames4s0.Colspan = 0;
                table.AddCell(cellNames4s0);
                PdfPCell cellNames5s0 = new PdfPCell(new Phrase("Date on which overime worked", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s0.HorizontalAlignment = 1;
                cellNames5s0.Colspan = 0;
                table.AddCell(cellNames5s0);
                PdfPCell cellNames5s1 = new PdfPCell(new Phrase("Total overtime worked or production in case of piece -rated", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s1.HorizontalAlignment = 1;
                cellNames5s1.Colspan = 0;
                table.AddCell(cellNames5s1);
                PdfPCell cellNames5s2 = new PdfPCell(new Phrase("Normal rate of Wages", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s2.HorizontalAlignment = 1;
                cellNames5s2.Colspan = 0;
                table.AddCell(cellNames5s2);
                PdfPCell cellNames5s3 = new PdfPCell(new Phrase("Overtime rate of wages", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s3.HorizontalAlignment = 1;
                cellNames5s3.Colspan = 0;
                table.AddCell(cellNames5s3);
                PdfPCell cellNames5s4 = new PdfPCell(new Phrase("Overtime  earnings", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s4.HorizontalAlignment = 1;
                cellNames5s4.Colspan = 0;
                table.AddCell(cellNames5s4);
                PdfPCell cellNames5s5 = new PdfPCell(new Phrase("Date on which overtime\nwages paid", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s5.HorizontalAlignment = 1;
                cellNames5s5.Colspan = 0;
                table.AddCell(cellNames5s5);
                PdfPCell cellNames5s6 = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNames5s6.HorizontalAlignment = 1;
                cellNames5s6.Colspan = 2;
                table.AddCell(cellNames5s6);
                #endregion
                //values
                PdfPCell cell;

                #region for empty Rows
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);


                //
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);


                #endregion


                #region for values
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                //
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("NO EMPLOYEE WORKED OVER TIME AND HENCE NO OVER TIME PAYMENTS WERE MADE DURING THIS MONTH", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 6;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                #endregion


                #region for empty Rows
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                //

                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=NORMAL
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("\n", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table.AddCell(cell);


                #endregion
                document.Add(table);

                #endregion Basic Information of the Employee



                string filename = "FormXXIII.pdf";


                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }

        protected void BtnformXXV_Click1(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string FontStyle = "verdana";

            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            int montha = DateTime.Parse(txtmonth.Text.Trim()).Month;
            int Yeara = DateTime.Parse(txtmonth.Text.Trim()).Year;

            int days = DateTime.DaysInMonth(Yeara, montha);


            string addressData = "";

            string Formdate = "";
            DateTime firstday = DateTime.Now;
            DateTime lastday = DateTime.Now;
            string months = "";
            string years = "";
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                Formdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(Formdate).Month.ToString();
            string Year = DateTime.Parse(Formdate).Year.ToString().Substring(2, 2);


            var list = new List<string>();
            if (GVListEmployees.Rows.Count > 0)
            {
                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkempid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblempid = GVListEmployees.Rows[i].FindControl("lblempid") as Label;

                    if (chkempid.Checked == true)
                    {
                        list.Add(lblempid.Text);
                    }
                }
            }

            if (list.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select atleast one employee');", true);
                return;
            }


            DataTable dtEmpids = new DataTable();
            dtEmpids.Columns.Add("Empid");
            if (list.Count != 0)
            {
                foreach (string s in list)
                {
                    DataRow row = dtEmpids.NewRow();
                    row["Empid"] = s;
                    dtEmpids.Rows.Add(row);
                }
            }

            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            ht.Add("@month", month + Year);
            ht.Add("@type", type);
            ht.Add("@EmpIds", dtEmpids);
            DataTable dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;


            if (dt.Rows.Count > 0)
            {

                float MonthDays = 0;
                string MDays = "";
                string noofduties = "0";
                float wkgdays = 0;
                string paymonth = dt.Rows[0]["month"].ToString();

                companyAddress = dt.Rows[0]["companyAddress"].ToString();
                companyName = dt.Rows[0]["companyName"].ToString();
                addressData = dt.Rows[0]["clientaddress"].ToString();


                if (paymonth.Length == 3)
                {
                    months = paymonth.Substring(0, 1);
                    //year = DateTime.Now.Year.ToString().Substring(0, 2) + paymonth.Substring(1, 2);
                    years = "20" + paymonth.Substring(1, 2);

                }
                else if (paymonth.Length == 4)
                {
                    months = paymonth.Substring(0, 2);
                    // year = DateTime.Now.Year.ToString().Substring(0, 2) + paymonth.Substring(2, 2);
                    years = "20" + paymonth.Substring(2, 2);
                }

                firstday = GlobalData.Instance.GetFirstDayMonth(int.Parse(years), int.Parse(month));
                lastday = GlobalData.Instance.GetLastDayMonth(int.Parse(years), int.Parse(month));

                MDays = lastday.ToString("dd/MM/yyyy").Substring(0, 2);

                MonthDays = Convert.ToSingle(MDays);



                if (wkgdays > MonthDays)
                {
                    wkgdays = MonthDays;
                }


                if (wkgdays < 20)
                {
                    noofduties = "0";
                }

                else if (wkgdays >= 20 && wkgdays < 30 && paymonth.Substring(0, 1).ToString() != "2")
                {
                    noofduties = "1";
                }
                else if (paymonth.Substring(0, 1).ToString() == "2" && wkgdays >= 20 && wkgdays < 28)
                {
                    noofduties = "1";
                }
                else if (paymonth.Substring(0, 1).ToString() == "2" && wkgdays >= 28)
                {
                    noofduties = "1.5";
                }
                else if (wkgdays >= 30 && wkgdays <= 40)
                {
                    noofduties = "1.5";
                }
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                #region

                PdfPTable table = new PdfPTable(7);
                table.TotalWidth = 970f;
                table.LockedWidth = true;
                float[] width = new float[] { 1.5f, 2.5f, 4f, 3f, 2f, 2f, 2.5f };
                table.SetWidths(width);
                PdfPCell cell;

                cell = new PdfPCell(new Phrase("FORM-25", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("(See Rule 29 (6))", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Register of Leave", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Name of The Establishment:", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(companyName, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("FOR THE MONTH OF: ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(GetMonthName() + "'" + Year, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("Location / Postal Address :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(companyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 5;
                cell.Border = 0;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 15;
                table.AddCell(cell);

                document.Add(table);
                #region
                PdfPTable table1 = new PdfPTable(13);
                table1.TotalWidth = 970f;
                table1.LockedWidth = true;
                table1.HeaderRows = 1;
                float[] width1 = new float[] { 1.5f, 3f, 2f, 2f, 2f, 2f, 2.5f, 3f, 2.5f, 2.5f, 2.5f, 3.5f, 2f };
                table1.SetWidths(width1);

                cell = new PdfPCell(new Phrase("S.NO", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Name Of Employee", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Calender Month of Service", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("No.of Days EST.. actually worked  the Month", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("No. of Days EST.. worked during the Month", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total Leaves For The Particular Month", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total of Leaves Earned", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("No. of Days and date from which the worker  is allowed leave  with wages", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Balance of leave to the credit of the worker", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total Leave Amount (Actual)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Earned Leave Amount", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Payment Mode", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                cell = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Colspan = 1;
                table1.AddCell(cell);
                #endregion
                //values
                string empname = "";
                string desgn = "";
                string fathername = "";
                string wrkgdays = "";
                //string noofduties = "";
                string EmpBankAcNo = "";
                float LeaveEncashAmt = 0;

                int k = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    empname = dt.Rows[i]["EmpmName"].ToString();
                    desgn = dt.Rows[i]["Desgn"].ToString();
                    fathername = dt.Rows[i]["EmpFatherName"].ToString();
                    wrkgdays = dt.Rows[i]["wrkngdays"].ToString();
                    noofduties = dt.Rows[i]["noofduties"].ToString();
                    EmpBankAcNo = dt.Rows[i]["EmpBankAcNo"].ToString();
                    LeaveEncashAmt = Convert.ToSingle(dt.Rows[i]["LeaveEncashAmt"].ToString());



                    cell = new PdfPCell(new Phrase(k.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(empname, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("" + days, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(noofduties, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(firstday.ToString("dd.MM.yyyy") + "\n to\n " + lastday.ToString("dd.MM.yyyy"), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(LeaveEncashAmt.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    cell = new PdfPCell(new Phrase(LeaveEncashAmt.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=NORMAL
                    cell.Colspan = 1;
                    table1.AddCell(cell);
                    if (EmpBankAcNo.Length > 0)
                    {
                        cell = new PdfPCell(new Phrase(EmpBankAcNo, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell.Colspan = 1;
                        table1.AddCell(cell);
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase("Cheque Mode", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell.Colspan = 1;
                        table1.AddCell(cell);
                    }
                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    table1.AddCell(cell);



                    k++;
                }
                document.Add(table1);

                #endregion Basic Information of the Employee


                document.NewPage();

                string filename = "FormXXV.pdf";


                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }

        protected void btnformc_Click(object sender, EventArgs e)
        {


            int Fontsize = 10;
            string FontStyle = "verdana";

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedItem.ToString() + "'";
            DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);
            string addressData = "";
            string ClientNameForms = "";
            if (dtclientaddress.Rows.Count > 0)
            {
                // ClientNameForms = dtclientaddress.Rows[0]["ClientNameForms"].ToString() + " ";
                addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString() + " ";
                addressData += dtclientaddress.Rows[0]["ClientAddrStreet"].ToString() + " ";
                addressData += dtclientaddress.Rows[0]["ClientAddrArea"].ToString() + " ";
                addressData += dtclientaddress.Rows[0]["ClientAddrColony"].ToString() + " ";
                addressData += dtclientaddress.Rows[0]["ClientAddrcity"].ToString() + " ";
                addressData += dtclientaddress.Rows[0]["ClientAddrstate"].ToString() + " ";
                addressData += dtclientaddress.Rows[0]["ClientAddrpin"].ToString() + " ";
            }

            string qry = "select ED.EmpBankAcNo,ep.empid,EP.Clientid,ep.noofduties,ep.Gross,ep.basic,ep.da,(Ed.Empfname + Ed.EmpMname + Ed.emplname) as Empname,ed.EmpFatherName,d.design as design,ep.Bonus from EmpPaySheet ep  " +
                 " inner join  empdetails Ed on Ed.Empid = EP.Empid " +
                " inner join Designations d on EP.Desgn=d.DesignId  and ep.clientid='" + ddlclientid.SelectedValue + "' and EP.month = '" + month + Year.Substring(2, 2) + " ' order by empid";

            DataTable dtqry = SqlHelper.Instance.GetTableByQuery(qry);

            if (dtqry.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL.Rotate());
                // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                #region
                PdfPTable table2 = new PdfPTable(16);
                table2.TotalWidth = 990f;
                table2.LockedWidth = true;
                float[] width2 = new float[] { 0.7f, 2.5f, 5f, 1.5f, 4f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
                table2.SetWidths(width2);

                PdfPCell mt = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                mt.HorizontalAlignment = 1;
                mt.Colspan = 16;
                mt.Border = 0;
                table2.AddCell(mt);

                PdfPCell cellformc = new PdfPCell(new Phrase("FORM C-BONUS PAID TO EMPLOYEES FOR THE ACCOUNTING YEAR ENDING ON THE " + GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize + 4, Font.BOLD, BaseColor.BLACK)));
                cellformc.HorizontalAlignment = 1;
                cellformc.Colspan = 16;
                cellformc.Border = 0;
                table2.AddCell(cellformc);
                table2.AddCell(mt);
                PdfPCell cellrule = new PdfPCell(new Phrase("[See Rule 4 (c)  of the Payment of Bonus Rules 1975]", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cellrule.HorizontalAlignment = 1;
                cellrule.Colspan = 16;
                cellrule.Border = 0;
                table2.AddCell(cellrule);
                table2.AddCell(mt);
                PdfPCell billgetmonth = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                billgetmonth.HorizontalAlignment = 1;
                billgetmonth.Colspan = 16;
                billgetmonth.Border = 0;
                table2.AddCell(billgetmonth);
                table2.AddCell(billgetmonth);
                table2.AddCell(billgetmonth);

                PdfPCell celladds1 = new PdfPCell(new Phrase("Name of the Establishment:", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                celladds1.HorizontalAlignment = 0;
                celladds1.Colspan = 3;
                celladds1.Border = 0;
                table2.AddCell(celladds1);
                PdfPCell celladds2 = new PdfPCell(new Phrase(companyName + "\n" + companyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                celladds2.HorizontalAlignment = 1;
                celladds2.Colspan = 4;
                celladds2.Border = 0;
                table2.AddCell(celladds2);
                PdfPCell celladds6 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                celladds6.HorizontalAlignment = 0;
                celladds6.Colspan = 1;
                celladds6.Border = 0;
                table2.AddCell(celladds6);
                PdfPCell celladds3 = new PdfPCell(new Phrase("Name and address of Principal Employer:", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                celladds3.HorizontalAlignment = 0;
                celladds3.Colspan = 4;
                celladds3.Border = 0;
                table2.AddCell(celladds3);
                PdfPCell celladds4 = new PdfPCell(new Phrase(addressData, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                celladds4.HorizontalAlignment = 1;
                celladds4.Colspan = 4;
                celladds4.Border = 0;
                table2.AddCell(celladds4);
                table2.AddCell(mt);
                table2.AddCell(mt);
                table2.AddCell(mt);
                document.Add(table2);

                PdfPTable table1 = new PdfPTable(16);
                table1.TotalWidth = 990f;
                table1.LockedWidth = true;
                table1.HeaderRows = 2;
                float[] width1 = new float[] { 1f, 4f, 3f, 3f, 3f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
                table1.SetWidths(width1);

                PdfPCell cellmt1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellmt1.HorizontalAlignment = 1;
                cellmt1.Colspan = 8;
                table1.AddCell(cellmt1);
                PdfPCell cellded = new PdfPCell(new Phrase("Deductions", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellded.HorizontalAlignment = 1;
                cellded.Colspan = 5;
                table1.AddCell(cellded);
                PdfPCell cellmt = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellmt.HorizontalAlignment = 1;
                cellmt.Colspan = 3;
                table1.AddCell(cellmt);


                PdfPCell c1ellloanno = new PdfPCell(new Phrase("SL No", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c1ellloanno.HorizontalAlignment = 1;
                c1ellloanno.Colspan = 1;
                table1.AddCell(c1ellloanno);
                PdfPCell c1ellloantype = new PdfPCell(new Phrase("Name of the employee", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c1ellloantype.HorizontalAlignment = 1;
                c1ellloantype.Colspan = 1;
                table1.AddCell(c1ellloantype);
                PdfPCell c1ellissuedamount = new PdfPCell(new Phrase("Father Name", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c1ellissuedamount.HorizontalAlignment = 1;
                c1ellissuedamount.Colspan = 1;
                table1.AddCell(c1ellissuedamount);
                PdfPCell c1ellnoof = new PdfPCell(new Phrase("Whether he has completed\n15 years of age at\nthe beginning of\nthe accounting year", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c1ellnoof.HorizontalAlignment = 1;
                c1ellnoof.Colspan = 1;
                table1.AddCell(c1ellnoof);
                PdfPCell c1ellstatus = new PdfPCell(new Phrase("Designation", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c1ellstatus.HorizontalAlignment = 1;
                c1ellstatus.Colspan = 1;
                table1.AddCell(c1ellstatus);
                PdfPCell c1ellissueddate = new PdfPCell(new Phrase("No. of days worked\nfor month " + GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c1ellissueddate.HorizontalAlignment = 1;
                c1ellissueddate.Colspan = 1;
                table1.AddCell(c1ellissueddate);
                PdfPCell c2ellloanno = new PdfPCell(new Phrase("Total salary or wage\nin respect for \nthe month Febaruary", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c2ellloanno.HorizontalAlignment = 1;
                c2ellloanno.Colspan = 1;
                table1.AddCell(c2ellloanno);
                PdfPCell c2ellloantype = new PdfPCell(new Phrase("Amount of bonus payable \nunder section10 or \nsection11 as the case maybe", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c2ellloantype.HorizontalAlignment = 1;
                c2ellloantype.Colspan = 1;
                table1.AddCell(c2ellloantype);
                PdfPCell c2ellissuedamount = new PdfPCell(new Phrase("Puja bonus or other \ncustomary bonus paid \nduring the year", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c2ellissuedamount.HorizontalAlignment = 1;
                c2ellissuedamount.Colspan = 1;
                table1.AddCell(c2ellissuedamount);
                PdfPCell c2ellnoof = new PdfPCell(new Phrase("Interim bonus or \nbonus paid in advance", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c2ellnoof.HorizontalAlignment = 1;
                c2ellnoof.Colspan = 1;
                table1.AddCell(c2ellnoof);
                PdfPCell c2ellstatus = new PdfPCell(new Phrase("Amount of income-tax deducted", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c2ellstatus.HorizontalAlignment = 1;
                c2ellstatus.Colspan = 1;
                table1.AddCell(c2ellstatus);
                PdfPCell c2ellissueddate = new PdfPCell(new Phrase("Deduction on account of financial\nloss, if any, caused by\nmisconduct of the employee", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                c2ellissueddate.HorizontalAlignment = 1;
                c2ellissueddate.Colspan = 1;
                table1.AddCell(c2ellissueddate);
                PdfPCell ccell = new PdfPCell(new Phrase("Total sum deducted under\ncois 9 , 10 10A and 11", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                ccell.HorizontalAlignment = 1;
                ccell.Colspan = 1;
                table1.AddCell(ccell);
                PdfPCell cc1ellloanno = new PdfPCell(new Phrase("Net Amount Payable", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellloanno.HorizontalAlignment = 1;
                cc1ellloanno.Colspan = 1;
                table1.AddCell(cc1ellloanno);
                PdfPCell cc1ellloantype = new PdfPCell(new Phrase("Amount Actually paid", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellloantype.HorizontalAlignment = 1;
                cc1ellloantype.Colspan = 1;
                table1.AddCell(cc1ellloantype);
                PdfPCell cc1ellissuedamount = new PdfPCell(new Phrase("Date on which paid and Signature\n/thumb impression of the employee", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellissuedamount.HorizontalAlignment = 1;
                cc1ellissuedamount.Colspan = 1;
                table1.AddCell(cc1ellissuedamount);



                PdfPCell cc1ellnoof = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellnoof.HorizontalAlignment = 1;
                cc1ellnoof.Colspan = 1;
                table1.AddCell(cc1ellnoof);

                PdfPCell cc1ellissueddate = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellissueddate.HorizontalAlignment = 1;
                cc1ellissueddate.Colspan = 1;
                table1.AddCell(cc1ellissueddate);
                PdfPCell cellloanno = new PdfPCell(new Phrase("3", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellloanno.HorizontalAlignment = 1;
                cellloanno.Colspan = 1;
                table1.AddCell(cellloanno);
                PdfPCell cellloantype = new PdfPCell(new Phrase("4", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellloantype.HorizontalAlignment = 1;
                cellloantype.Colspan = 1;
                table1.AddCell(cellloantype);
                PdfPCell cellissuedamount = new PdfPCell(new Phrase("5", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellissuedamount.HorizontalAlignment = 1;
                cellissuedamount.Colspan = 1;
                table1.AddCell(cellissuedamount);
                PdfPCell cellnoof = new PdfPCell(new Phrase("6", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellnoof.HorizontalAlignment = 1;
                cellnoof.Colspan = 1;
                table1.AddCell(cellnoof);
                PdfPCell cellstatus = new PdfPCell(new Phrase("7", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellstatus.HorizontalAlignment = 1;
                cellstatus.Colspan = 1;
                table1.AddCell(cellstatus);
                PdfPCell cellissueddate = new PdfPCell(new Phrase("8", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellissueddate.HorizontalAlignment = 1;
                cellissueddate.Colspan = 1;
                table1.AddCell(cellissueddate);
                PdfPCell cc1ellnoof1 = new PdfPCell(new Phrase("9", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellnoof1.HorizontalAlignment = 1;
                cc1ellnoof1.Colspan = 1;
                table1.AddCell(cc1ellnoof1);
                PdfPCell cc1ellstatus1 = new PdfPCell(new Phrase("10", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellstatus1.HorizontalAlignment = 1;
                cc1ellstatus1.Colspan = 1;
                table1.AddCell(cc1ellstatus1);
                PdfPCell cc1ellissueddate1 = new PdfPCell(new Phrase("11", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellissueddate1.HorizontalAlignment = 1;
                cc1ellissueddate1.Colspan = 1;
                table1.AddCell(cc1ellissueddate1);
                PdfPCell cellloanno1 = new PdfPCell(new Phrase("12", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellloanno1.HorizontalAlignment = 1;
                cellloanno1.Colspan = 1;
                table1.AddCell(cellloanno1);
                PdfPCell cellloantype1 = new PdfPCell(new Phrase("13", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellloantype1.HorizontalAlignment = 1;
                cellloantype1.Colspan = 1;
                table1.AddCell(cellloantype);
                PdfPCell cellissuedamount1 = new PdfPCell(new Phrase("14", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellissuedamount1.HorizontalAlignment = 1;
                cellissuedamount1.Colspan = 1;
                table1.AddCell(cellissuedamount1);
                PdfPCell cellnoof1 = new PdfPCell(new Phrase("15", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellnoof1.HorizontalAlignment = 1;
                cellnoof1.Colspan = 1;
                table1.AddCell(cellnoof1);
                PdfPCell cc1ellstatus = new PdfPCell(new Phrase("16", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cc1ellstatus.HorizontalAlignment = 1;
                cc1ellstatus.Colspan = 1;
                table1.AddCell(cc1ellstatus);

                int k = 1;

                //values
                for (int i = 0; i < dtqry.Rows.Count; i++)
                {
                    string empname = dtqry.Rows[i]["Empname"].ToString();
                    string empid = dtqry.Rows[i]["empid"].ToString();
                    string design = dtqry.Rows[i]["design"].ToString();
                    string empfathername = dtqry.Rows[i]["EmpFatherName"].ToString();
                    string bonus = dtqry.Rows[i]["Bonus"].ToString();
                    string noofduties = dtqry.Rows[i]["noofduties"].ToString();
                    string gross = dtqry.Rows[i]["Gross"].ToString();
                    float basic = Convert.ToSingle(dtqry.Rows[i]["basic"].ToString());
                    float da = Convert.ToSingle(dtqry.Rows[i]["da"].ToString());
                    string EmpBankAcNo = dtqry.Rows[i]["EmpBankAcNo"].ToString();


                    PdfPCell cc1ellnoof3 = new PdfPCell(new Phrase(k.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cc1ellnoof3.HorizontalAlignment = 1;
                    cc1ellnoof3.Colspan = 1;
                    table1.AddCell(cc1ellnoof3);
                    PdfPCell cc1ellstatus3 = new PdfPCell(new Phrase(empname, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cc1ellstatus3.HorizontalAlignment = 0;
                    cc1ellstatus3.Colspan = 1;
                    table1.AddCell(cc1ellstatus3);
                    PdfPCell cc1ellissueddate3 = new PdfPCell(new Phrase(empfathername, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cc1ellissueddate3.HorizontalAlignment = 0;
                    cc1ellissueddate3.Colspan = 1;
                    table1.AddCell(cc1ellissueddate3);
                    PdfPCell cellloanno3 = new PdfPCell(new Phrase("YES", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellloanno3.HorizontalAlignment = 1;
                    cellloanno3.Colspan = 1;
                    table1.AddCell(cellloanno3);
                    PdfPCell cellloantype3 = new PdfPCell(new Phrase(design, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellloantype3.HorizontalAlignment = 1;
                    cellloantype3.Colspan = 1;
                    table1.AddCell(cellloantype3);
                    PdfPCell cellissuedamount3 = new PdfPCell(new Phrase(noofduties, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellissuedamount3.HorizontalAlignment = 1;
                    cellissuedamount3.Colspan = 1;
                    table1.AddCell(cellissuedamount3);
                    PdfPCell cellnoof3 = new PdfPCell(new Phrase((basic + da).ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellnoof3.HorizontalAlignment = 2;
                    cellnoof3.Colspan = 1;
                    table1.AddCell(cellnoof3);
                    PdfPCell cellstatus3 = new PdfPCell(new Phrase(bonus, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellstatus3.HorizontalAlignment = 2;
                    cellstatus3.Colspan = 1;
                    table1.AddCell(cellstatus3);
                    PdfPCell cellissueddate3 = new PdfPCell(new Phrase("NA", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellissueddate3.HorizontalAlignment = 1;
                    cellissueddate3.Colspan = 1;
                    table1.AddCell(cellissueddate3);
                    PdfPCell cc1ellnoof13 = new PdfPCell(new Phrase("NA", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cc1ellnoof13.HorizontalAlignment = 1;
                    cc1ellnoof13.Colspan = 1;
                    table1.AddCell(cc1ellnoof13);
                    PdfPCell cc1ellstatus4 = new PdfPCell(new Phrase("NA", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cc1ellstatus4.HorizontalAlignment = 1;
                    cc1ellstatus4.Colspan = 1;
                    table1.AddCell(cc1ellstatus4);
                    PdfPCell cc1ellissueddate4 = new PdfPCell(new Phrase("NIL", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cc1ellissueddate4.HorizontalAlignment = 1;
                    cc1ellissueddate4.Colspan = 1;
                    table1.AddCell(cc1ellissueddate4);
                    PdfPCell cellloanno4 = new PdfPCell(new Phrase("NIL", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellloanno4.HorizontalAlignment = 1;
                    cellloanno4.Colspan = 1;
                    table1.AddCell(cellloanno4);
                    PdfPCell cellloantype4 = new PdfPCell(new Phrase(bonus, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellloantype4.HorizontalAlignment = 2;
                    cellloantype4.Colspan = 1;
                    table1.AddCell(cellloantype4);
                    PdfPCell cellissuedamount4 = new PdfPCell(new Phrase(bonus, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellissuedamount4.HorizontalAlignment = 2;
                    cellissuedamount4.Colspan = 1;
                    table1.AddCell(cellissuedamount4);
                    PdfPCell cellnoof4 = new PdfPCell(new Phrase(EmpBankAcNo, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellnoof4.HorizontalAlignment = 1;
                    cellnoof4.Colspan = 1;
                    table1.AddCell(cellnoof4);
                    k++;
                }
                document.Add(table1);

                document.NewPage();

                #endregion

                string filename = ddlcname.SelectedItem.Text + "FormC.pdf";
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

        protected void btnform_Click(object sender, EventArgs e)
        {
            string FontStyle = "Verdana";

            int Fontsize = 9;

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Month');", true);
                return;
            }

            string date = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            var bBillDates = 0;
            var Gendays = 0;
            var G_Sdays = 0;


            string selectmonth = string.Empty;




            selectmonth = " select cd.ClientID,c.ClientName,ubb.NoofDays,SUM(cd.Quantity) as Quantity,(ubb.NoofDays*SUM(cd.Quantity))  as AgreedManDays , SUM(CAST(DutyHours AS decimal)) as TotalWorkingDays, " +
                                           " (select count(EmpId)  from EmpPaySheet ep where EmpId not like '90%' and month = '" + month + Year.Substring(2, 2) + " ' and ep.clientid = cd.clientid and ep.ContractId = cd.ContractId group by ep.ClientID) as employess, " +
                                        "sum(cd.Quantity * cd.Amount) as agreddbill,ub.dutiestotalamount,ub.ServiceTax,(ub.dutiestotalamount +   ub.ServiceTax) as totalbill, CONVERT(VARCHAR(10),isnull(ub.BillDt,'01/01/1900'), 104) as  BillDt , " +
                                        "(case BillDates when 0 then '1st to 1st' when 2 then '26 to 25' when 3 then '21 to 20'  end) as billdates, ub.BillNo " +
                                            "from unitbill ub " +
                                            "inner join ContractDetails cd on ub.UnitId = cd.ClientID and ub.Contractid = cd.ContractId" +
                                         "   inner join Clients c on cd.ClientID = c.ClientId" +
                                          "  inner join Contracts Ct on Ct.ClientId = cd.ClientID and Ct.Contractid = cd.ContractId" +
                                           " inner join UnitBillBreakup ubb on ubb.UnitId = ub.UnitId and ubb.Contractid = ub.ContractId and ubb.Month = ub.Month and ubb.Designation = cd.Designations" +
                                            " where ub.MONTH ='" + month + Year.Substring(2, 2) + " ' " +
                                             "group by cd.ClientID,cd.ContractId,c.ClientName ,ub.dutiestotalamount,ub.ServiceTax,ub.BillNo,BillDates,ub.BillDt,ubb.NoofDays ";
            DataTable dt = SqlHelper.Instance.GetTableByQuery(selectmonth);

            //string tempDescription = dt.Rows[0]["Description"].ToString();

            MemoryStream ms = new MemoryStream();
            if (dt.Rows.Count > 0)
            {

                Document document = new Document(PageSize.LEGAL.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                #region for PDF by anil on 16-07-2016

                PdfPCell cell;

                PdfPTable table = new PdfPTable(13);
                table.TotalWidth = 990f;
                table.LockedWidth = true;
                table.HeaderRows = 2;
                float[] width2 = new float[] { 1f, 5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f, 2.3f, 2.3f };
                table.SetWidths(width2);

                #region for heading by anil on 16-07-2016
                cell = new PdfPCell(new Phrase("FM Billing for " + GetMonthName() + " ' " + Year, FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 13;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Site Name", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Agreed Man Power", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Manpower as Per Pay sheet ", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Agreed Man Days", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Billing Cycle ", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total Working Days", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Agreed Bill Value sum", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total Claimed", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Service Tax 14.5%", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total Bill", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Inv Date", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Inv No", FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                #endregion

                #region for Values by anil on 16-07-2016
                string clientname = "";
                string billdates = "";
                string billno = "";
                float Quantity = 0;
                // float employess = 0;
                float WorkingDays = 0;
                float AgreedManDays = 0;

                float billvalue = 0;
                float billwoservicetax = 0;
                float servicetax = 0;
                float totalbill = 0;
                string invoicedate = "";
                //
                float totalQuantity = 0;
                string totalemployess = "";
                //float totalemployess = 0;
                float totalbillvalue = 0;
                float totalbillwoservicetax = 0;
                float totalservicetax = 0;
                float Ttotalbill = 0;
                float totalWorkingDays = 0;
                float totalAgreedManDays = 0;
                string employess = "";

                int j = 1;


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    clientname = dt.Rows[i]["ClientName"].ToString();
                    employess = dt.Rows[i]["employess"].ToString();
                    billdates = dt.Rows[i]["billdates"].ToString();
                    billno = dt.Rows[i]["BillNo"].ToString();
                    invoicedate = dt.Rows[i]["BillDt"].ToString();
                    Quantity = float.Parse(dt.Rows[i]["Quantity"].ToString());
                    // employess = Convert.Parse(dt.Rows[i]["employess"].ToString());
                    billwoservicetax = float.Parse(dt.Rows[i]["dutiestotalamount"].ToString());
                    servicetax = float.Parse(dt.Rows[i]["ServiceTax"].ToString());
                    billvalue = float.Parse(dt.Rows[i]["agreddbill"].ToString());
                    totalbill = float.Parse(dt.Rows[i]["totalbill"].ToString());
                    WorkingDays = float.Parse(dt.Rows[i]["TotalWorkingDays"].ToString());
                    AgreedManDays = float.Parse(dt.Rows[i]["AgreedManDays"].ToString());

                    cell = new PdfPCell(new Phrase(j.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(clientname, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(Quantity.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalQuantity += Quantity;
                    if (employess.Length > 0)
                    {
                        cell = new PdfPCell(new Phrase(employess.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 1;
                        table.AddCell(cell);
                    }
                    totalemployess += employess;

                    cell = new PdfPCell(new Phrase(AgreedManDays.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalAgreedManDays += AgreedManDays;
                    cell = new PdfPCell(new Phrase(billdates, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(WorkingDays.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalWorkingDays += WorkingDays;
                    cell = new PdfPCell(new Phrase(billvalue.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalbillvalue += billvalue;
                    cell = new PdfPCell(new Phrase(billwoservicetax.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalbillwoservicetax += billwoservicetax;
                    cell = new PdfPCell(new Phrase(servicetax.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    totalservicetax += servicetax;
                    cell = new PdfPCell(new Phrase(totalbill.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    Ttotalbill += totalbill;
                    cell = new PdfPCell(new Phrase(invoicedate, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(billno, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    j++;
                #endregion
                }
                #region for totals
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalQuantity.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalAgreedManDays.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalWorkingDays.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalbillvalue.ToString("0,00"), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalbillwoservicetax.ToString("0,00"), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(totalservicetax.ToString("0,00"), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(Ttotalbill.ToString("0,00"), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2;
                cell.Colspan = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2;
                cell.Colspan = 1;
                table.AddCell(cell);

                document.Add(table);
                #endregion

                #endregion

                // document.NewPage();

                string filename = "FormXIV.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }
        }

        protected void btnformD_Click(object sender, EventArgs e)
        {

            try
            {

                string FontStyle = "verdana";
                string fontsyle = "verdana";
                int Fontsize = 9;
                if (ddlclientid.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert()", "alert('Please Select Client ID/Name')", true);
                    return;
                }

                DateTime frmdate;
                string FromDate = "";
                string Frmonth = "";
                string FrYear = "";
                string FrMn = "";

                if (txtfrom.Text.Trim().Length > 0)
                {
                    frmdate = DateTime.ParseExact(txtfrom.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    Frmonth = frmdate.ToString("MM");
                    FrYear = frmdate.ToString("yy");
                    FrMn = frmdate.Month.ToString();
                }



                FromDate = FrYear + Frmonth;


                DateTime tdate;
                string ToDate = "";
                string Tomonth = "";
                string ToYear = "";
                string ToMn = "";
                if (txtto.Text.Trim().Length > 0)
                {
                    tdate = DateTime.ParseExact(txtto.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    Tomonth = tdate.ToString("MM");
                    ToYear = tdate.ToString("yy");
                    ToMn = tdate.Month.ToString();

                }

                ToDate = ToYear + Tomonth;

                string qry = "select (empfname+' '+empmname+' '+emplname) as EmployeeName,sum(cdsw.Bonus)  as cdswbonus,Sum(eps.Bonus) as bonus ,e.empfathername from emppaysheet eps inner join empdetails e on e.empid=eps.empid  left join ContractDetailsSW cdsw on cdsw.ClientID=eps.ClientId and cdsw.Designations=eps.Desgn where eps.ClientId='" + ddlclientid.SelectedValue + "' and  eps.monthnew between '" + FromDate.ToString() + "' and '" + ToDate.ToString() + "'  group by  empfname,empmname,emplname,e.empfathername ";
                DataTable dtEmpdetails = SqlHelper.Instance.GetTableByQuery(qry);



                MemoryStream ms = new MemoryStream();
                if (dtEmpdetails.Rows.Count > 0)
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();
                    document.AddTitle("FaMS");
                    document.AddAuthor("DIYOS");
                    document.AddSubject("Wage Slips");
                    document.AddKeywords("Keyword1, keyword2, …");//
                    string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                    DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
                    string companyName = "Your Company Name";
                    string companyAddress = "Your Company Address";
                    if (compInfo.Rows.Count > 0)
                    {
                        companyName = compInfo.Rows[0]["CompanyName"].ToString();
                        companyAddress = compInfo.Rows[0]["Address"].ToString();
                    }

                    string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedValue + "'";
                    DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);
                    string addressData = "";

                    if (dtclientaddress.Rows.Count > 0)
                    {
                        addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString() + " ";
                        addressData += dtclientaddress.Rows[0]["ClientAddrStreet"].ToString() + " ";
                        addressData += dtclientaddress.Rows[0]["ClientAddrArea"].ToString() + " ";
                        addressData += dtclientaddress.Rows[0]["ClientAddrColony"].ToString() + " ";
                        addressData += dtclientaddress.Rows[0]["ClientAddrcity"].ToString() + " ";
                        addressData += dtclientaddress.Rows[0]["ClientAddrstate"].ToString() + " ";
                        addressData += dtclientaddress.Rows[0]["ClientAddrpin"].ToString() + " ";
                    }



                    #region

                    PdfPCell cell;

                    PdfPTable tablesnewone = new PdfPTable(7);
                    tablesnewone.TotalWidth = 520f;
                    tablesnewone.LockedWidth = true;
                    float[] width = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f };
                    tablesnewone.SetWidths(width);

                    PdfPCell cellHead1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cellHead1.HorizontalAlignment = 1;
                    cellHead1.Colspan = 7;
                    cellHead1.Border = 0;
                    cellHead1.PaddingTop = 15;
                    tablesnewone.AddCell(cellHead1);


                    PdfPCell cellmuster = new PdfPCell(new Phrase("FORM D", FontFactory.GetFont(fontsyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                    cellmuster.HorizontalAlignment = 1;
                    cellmuster.Colspan = 7;
                    cellmuster.Border = 0;
                    cellmuster.PaddingTop = 10; ;
                    tablesnewone.AddCell(cellmuster);

                    PdfPCell cellRule32 = new PdfPCell(new Phrase("(See rule 6)", FontFactory.GetFont(fontsyle, 10, Font.ITALIC, BaseColor.BLACK)));
                    cellRule32.HorizontalAlignment = 1;
                    cellRule32.Colspan = 7;
                    cellRule32.Border = 0;
                    tablesnewone.AddCell(cellRule32);

                    PdfPCell cellHead2 = new PdfPCell(new Phrase("Register to be maintained by the employer under Rule 6 of the Equal Remuneration", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellHead2.HorizontalAlignment = 1;
                    cellHead2.Colspan = 7;
                    cellHead2.Border = 0;
                    cellHead2.PaddingTop = 5;
                    tablesnewone.AddCell(cellHead2);

                    cell = new PdfPCell(new Phrase("Rules, 1976", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1;
                    cell.Colspan = 7;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Name of the Establishment\nWith full address", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 3;
                    cell.Border = 0;
                    cell.PaddingTop = 10;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase(": " + ddlcname.SelectedItem + "\n  " + addressData, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 4;
                    cell.Border = 0;
                    cell.PaddingTop = 10;
                    tablesnewone.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Name & Address of Contractor", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 3;
                    cell.Border = 0;
                    cell.PaddingTop = 8;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase(": " + companyName + "\n  " + companyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 4;
                    cell.Border = 0;
                    cell.PaddingTop = 8;
                    tablesnewone.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Total number of workers employed :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 7;
                    cell.Border = 0;
                    cell.PaddingTop = 8;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Total number of men workers employed :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 7;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Total number of women workers employed :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 7;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);


                    document.Add(tablesnewone);


                    tablesnewone = new PdfPTable(10);
                    tablesnewone.TotalWidth = 520f;
                    tablesnewone.LockedWidth = true;
                    width = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 3f };
                    tablesnewone.SetWidths(width);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 10;
                    cell.PaddingTop = 10;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    tablesnewone.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Category\nof\nworkers", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Brief\nDiscription\nof work", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("No. of men\nemployed", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("No. of women\nemployed", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Rate of remuneration paid", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Basic\nwage or\nsalary", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Dearness\nallowance", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("House Rent\nallowance", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Other\nallowance", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Cash value of\nconcessional\nsupply of\nessential commodities", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 10;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    tablesnewone.AddCell(cell);

                    cell = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("3", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("4", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("5", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("6", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("7", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("8", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("9", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);
                    cell = new PdfPCell(new Phrase("10", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 1;
                    cell.Border = 0;
                    tablesnewone.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 10;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    tablesnewone.AddCell(cell);


                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 10;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    cell.PaddingTop = 20;
                    tablesnewone.AddCell(cell);



                    cell = new PdfPCell(new Phrase("Remarks: Salary/ wages vary according to the qualification & experience of workers. Starting salary/wages p. m. is the same for both the men & women workers.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0;
                    cell.Colspan = 10;
                    cell.Border = 0;
                    cell.PaddingTop = 10;
                    tablesnewone.AddCell(cell);


                    document.Add(tablesnewone);

                    #endregion Basic Information of the Employee


                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Form D.pdf");
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();



            string qry = "select eps.empid,(empfname+' '+empmname+' '+emplname) as Name from emppaysheet eps inner join empdetails ed on ed.empid=eps.empid where month='" + month + Year.Substring(2, 2) + "' and clientid='" + ddlclientid.SelectedValue + "'  and  month(ed.EmpDtofJoining)='" + month + "' and YEAR(ed.EmpDtofJoining)='" + Year + "'";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(qry).Result;

            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
            }
        }

        protected void btnnewPaySheetExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string date = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            month = month + Year.Substring(2, 2);
            int NewMonth = Convert.ToInt32(month);

            string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedValue + "'";
            DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);
            string AddrHno = ""; string AddrColony = ""; string AddrArea = ""; string AddrStreet = ""; string Addrcity = ""; string Addrstate = ""; string Addrpin = "";
            if (dtclientaddress.Rows.Count > 0)
            {

                AddrHno += dtclientaddress.Rows[0]["ClientAddrHno"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrStreet"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrArea"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrColony"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrcity"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrstate"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrpin"].ToString() + "";
            }


            string heading = ": " + companyName + "\n" + companyAddress + "\n\n";

            string line = "FORM T<br>COMBINED MUSTER ROLL cum REGISTER OF WAGES<br>The Karnataka Shops & Commercial Establishments Act, 1961 & Rules 1963. ";

            string line1 = "(See Rule 24(9-B) of Karnataka Shops & Commercial Establishment Rules,1963) in Lieu of<br>1. Form I,II of Rule 22(4); Form V & VII of Rules 29(1) & (5) of the Karnataka Minimum Wages Rules,1958.<br>2. Form I of Rules 3(1) of the Karnataka Payment of Wages Rules 1963<br>3. Form XIII of Rules 75; Form XV,XVII,XX,XXI,XXII,XXIII of Rule 78(1)(a)(i,(ii)&(iii) of the Contract Labour (Regulation & Abolition) Karnataka Rules 1974.<br>4. Form XIII of Rules 43; Form XVII,XVIII,XIX,XX,XXII of Rule 46(2)(a),(c) & (d) of Karnataka Interstate Migrant Workmen Rules, 1981";

            string line2 = "Name Of The Establishment : <br>" + companyName + "<br>" + companyAddress + "";

            string line3 = "Name & Address of Establishment (Location of Work) :<br>" + companyName + "<br>" + companyAddress + "";

            string line4 = "Name & Address of Principal Employer : " + ddlcname.SelectedItem + " <br> " + AddrHno;

            string line5 = "";

            string line6 = "" + GetMonthName().ToUpper() + "/" + Year;

            var Gendays = 0;
            var G_Sdays = 0;

            var ContractID = "";
            var bPaySheetDates = 0;

            DateTime LastDay = DateTime.Now;

            LastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

            #region  Begin Get Contract Id Based on The Last Day


            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
            HtGetContractID.Add("@LastDay", LastDay);
            DataTable DTContractID = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPNameForGetContractID, HtGetContractID);

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
                bPaySheetDates = int.Parse(DTContractID.Rows[0]["PaySheetDates"].ToString());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not Avaialable For This Client.');", true);
                return;
            }

            #endregion  End Get Contract Id Based on The Last Day



            DateTime mGendays = DateTime.Now;
            DateTime dates = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            mGendays = DateTime.Parse(dates.ToString());
            Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bPaySheetDates);

            G_Sdays = Timings.Instance.Get_GS_Days(NewMonth, Gendays);
            int type = ddlForms.SelectedIndex;

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "ClientForms";
            HTPaysheet.Add("@ClientId", ddlclientid.SelectedValue);
            HTPaysheet.Add("@month", NewMonth);
            HTPaysheet.Add("@type", type);
            HTPaysheet.Add("@Gendays", Gendays);

            dt = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTPaysheet);
            string Filename = ddlcname.SelectedItem + "" + GetMonthName().ToUpper() + "" + GetMonthOfYear();
            if (dt.Rows.Count > 0)
            {
                GVUtill.ExporttoExcelNew(dt, Filename, line, line1, line2, line3, line4, line5, line6);
            }

        }

        protected void btnFormTPaysheet_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string date = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            month = month + Year.Substring(2, 2);
            int NewMonth = Convert.ToInt32(month);

            string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedValue + "'";
            DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);
            string AddrHno = ""; string AddrColony = ""; string AddrArea = ""; string AddrStreet = ""; string Addrcity = ""; string Addrstate = ""; string Addrpin = "";
            if (dtclientaddress.Rows.Count > 0)
            {

                AddrHno += dtclientaddress.Rows[0]["ClientAddrHno"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrStreet"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrArea"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrColony"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrcity"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrstate"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrpin"].ToString() + "";
            }


            string heading = ": " + companyName + "\n" + companyAddress + "\n\n";

            string line = "FORM T<br>COMBINED MUSTER ROLL cum REGISTER OF WAGES<br>The Karnataka Shops & Commercial Establishments Act, 1961 & Rules 1963. ";

            string line1 = "(See Rule 24(9-B) of Karnataka Shops & Commercial Establishment Rules,1963) in Lieu of<br>1. Form I,II of Rule 22(4); Form V & VII of Rules 29(1) & (5) of the Karnataka Minimum Wages Rules,1958.<br>2. Form I of Rules 3(1) of the Karnataka Payment of Wages Rules 1963<br>3. Form XIII of Rules 75; Form XV,XVII,XX,XXI,XXII,XXIII of Rule 78(1)(a)(i,(ii)&(iii) of the Contract Labour (Regulation & Abolition) Karnataka Rules 1974.<br>4. Form XIII of Rules 43; Form XVII,XVIII,XIX,XX,XXII of Rule 46(2)(a),(c) & (d) of Karnataka Interstate Migrant Workmen Rules, 1981";

            string line2 = "Name Of The Establishment : <br>" + companyName + "<br>" + companyAddress + "";

            string line3 = "Name & Address of Establishment (Location of Work) :<br>" + companyName + "<br>" + companyAddress + "";

            string line4 = "Name & Address of Principal Employer : " + ddlcname.SelectedItem + " <br> " + AddrHno;

            string line5 = "";

            string line6 = "" + GetMonthName().ToUpper() + "/" + Year;

            var Gendays = 0;
            var G_Sdays = 0;

            var ContractID = "";
            var bPaySheetDates = 0;

            DateTime LastDay = DateTime.Now;

            LastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

            #region  Begin Get Contract Id Based on The Last Day


            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
            HtGetContractID.Add("@LastDay", LastDay);
            DataTable DTContractID = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPNameForGetContractID, HtGetContractID);

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
                bPaySheetDates = int.Parse(DTContractID.Rows[0]["PaySheetDates"].ToString());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not Avaialable For This Client.');", true);
                return;
            }

            #endregion  End Get Contract Id Based on The Last Day



            DateTime mGendays = DateTime.Now;
            DateTime dates = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            mGendays = DateTime.Parse(dates.ToString());
            Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bPaySheetDates);

            G_Sdays = Timings.Instance.Get_GS_Days(NewMonth, Gendays);
            int type = ddlForms.SelectedIndex;

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "ClientForms";
            HTPaysheet.Add("@ClientId", ddlclientid.SelectedValue);
            HTPaysheet.Add("@month", NewMonth);
            HTPaysheet.Add("@type", type);
            HTPaysheet.Add("@Gendays", Gendays);

            dt = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTPaysheet);
            string Filename = ddlcname.SelectedItem + "" + GetMonthName().ToUpper() + "" + GetMonthOfYear();
            if (dt.Rows.Count > 0)
            {
                GVUtill.ExporttoExceFromTpaysheet(dt, Filename, line, line1, line2, line3, line4, line5, line6);
            }

        }

        protected void btnnewPaySheetAttendance_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string date = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            month = month + Year.Substring(2, 2);
            int NewMonth = Convert.ToInt32(month);

            string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedValue + "'";
            DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);
            string AddrHno = ""; string AddrColony = ""; string AddrArea = ""; string AddrStreet = ""; string Addrcity = ""; string Addrstate = ""; string Addrpin = "";
            if (dtclientaddress.Rows.Count > 0)
            {

                AddrHno += dtclientaddress.Rows[0]["ClientAddrHno"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrStreet"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrArea"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrColony"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrcity"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrstate"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrpin"].ToString() + "";
            }

            string selectcontracts = "select typeofwork from Contracts where clientid= '" + ddlclientid.SelectedValue + "'";
            DataTable dtcontraccts = SqlHelper.Instance.GetTableByQuery(selectcontracts);
            string typeofwork = "";
            if (dtcontraccts.Rows.Count > 0)
            {
                typeofwork = dtcontraccts.Rows[0]["typeofwork"].ToString() + "";
            }

            //string heading = ": " + companyName + "\n" + companyAddress + "\n\n";

            //string line = "FORM T<br>COMBINED MUSTER ROLL cum REGISTER OF WAGES<br>The Karnataka Shops & Commercial Establishments Act, 1961 & Rules 1963. ";

            //string line1 = "(See Rule 24(9-B) of Karnataka Shops & Commercial Establishment Rules,1963) in Lieu of<br>1. Form I,II of Rule 22(4); Form V & VII of Rules 29(1) & (5) of the Karnataka Minimum Wages Rules,1958.<br>2. Form I of Rules 3(1) of the Karnataka Payment of Wages Rules 1963<br>3. Form XIII of Rules 75; Form XV,XVII,XX,XXI,XXII,XXIII of Rule 78(1)(a)(i,(ii)&(iii) of the Contract Labour (Regulation & Abolition) Karnataka Rules 1974.<br>4. Form XIII of Rules 43; Form XVII,XVIII,XIX,XX,XXII of Rule 46(2)(a),(c) & (d) of Karnataka Interstate Migrant Workmen Rules, 1981";
            string line1 = "";

            string line2 = "Name Of The Establishment : <br>" + companyName + "<br>" + companyAddress + "";

            string line3 = "Contractor (if any)  :<br>" + companyName + "<br>" + companyAddress + "";

            string line4 = "Name & Address of Principal Employer : <br>" + ddlcname.SelectedItem + " <br> " + AddrHno;

            string line5 = "Place of work : " + typeofwork;

            string line6 = "MONTH & YEAR :" + GetMonthName().ToUpper() + "/" + Year;

            var Gendays = 0;
            var G_Sdays = 0;

            var ContractID = "";
            var bPaySheetDates = 0;

            DateTime LastDay = DateTime.Now;

            LastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

            #region  Begin Get Contract Id Based on The Last Day


            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
            HtGetContractID.Add("@LastDay", LastDay);
            DataTable DTContractID = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPNameForGetContractID, HtGetContractID);

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
                bPaySheetDates = int.Parse(DTContractID.Rows[0]["PaySheetDates"].ToString());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not Avaialable For This Client.');", true);
                return;
            }

            #endregion  End Get Contract Id Based on The Last Day



            DateTime mGendays = DateTime.Now;
            DateTime dates = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            mGendays = DateTime.Parse(dates.ToString());
            Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bPaySheetDates);

            G_Sdays = Timings.Instance.Get_GS_Days(NewMonth, Gendays);
            int type = ddlForms.SelectedIndex;

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "ClientForms";
            HTPaysheet.Add("@ClientId", ddlclientid.SelectedValue);
            HTPaysheet.Add("@month", NewMonth);
            HTPaysheet.Add("@type", type);
            HTPaysheet.Add("@Gendays", Gendays);

            dt = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTPaysheet);
            string Filename = ddlcname.SelectedItem + "" + GetMonthName().ToUpper() + "" + GetMonthOfYear();
            if (dt.Rows.Count > 0)
            {
                GVUtill.ExporttoExcelAttendance(dt, Filename, line1, line2, line3, line4, line5, line6);
            }

        }

        protected void btnnewPaySheetDed_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string date = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            month = month + Year.Substring(2, 2);
            int NewMonth = Convert.ToInt32(month);

            string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedValue + "'";
            DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);
            string AddrHno = ""; string AddrColony = ""; string AddrArea = ""; string AddrStreet = ""; string Addrcity = ""; string Addrstate = ""; string Addrpin = "";
            if (dtclientaddress.Rows.Count > 0)
            {

                AddrHno += dtclientaddress.Rows[0]["ClientAddrHno"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrStreet"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrArea"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrColony"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrcity"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrstate"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrpin"].ToString() + "";
            }


            //string heading = ": " + companyName + "\n" + companyAddress + "\n\n";

            //string line = "FORM T<br>COMBINED MUSTER ROLL cum REGISTER OF WAGES<br>The Karnataka Shops & Commercial Establishments Act, 1961 & Rules 1963. ";

            //string line1 = "(See Rule 24(9-B) of Karnataka Shops & Commercial Establishment Rules,1963) in Lieu of<br>1. Form I,II of Rule 22(4); Form V & VII of Rules 29(1) & (5) of the Karnataka Minimum Wages Rules,1958.<br>2. Form I of Rules 3(1) of the Karnataka Payment of Wages Rules 1963<br>3. Form XIII of Rules 75; Form XV,XVII,XX,XXI,XXII,XXIII of Rule 78(1)(a)(i,(ii)&(iii) of the Contract Labour (Regulation & Abolition) Karnataka Rules 1974.<br>4. Form XIII of Rules 43; Form XVII,XVIII,XIX,XX,XXII of Rule 46(2)(a),(c) & (d) of Karnataka Interstate Migrant Workmen Rules, 1981";
            string line1 = "";

            string line2 = "Name Of The Establishment : <br>" + companyName + "<br>" + companyAddress + "";

            string line3 = "Contractor (if any)  :<br>" + companyName + "<br>" + companyAddress + "";

            string line4 = "Name & Address of Principal Employer : <br>" + ddlcname.SelectedItem + " <br> " + AddrHno;

            string line5 = "Place of work : ";

            string line6 = "MONTH & YEAR :" + GetMonthName().ToUpper() + "/" + Year;

            var Gendays = 0;
            var G_Sdays = 0;

            var ContractID = "";
            var bPaySheetDates = 0;

            DateTime LastDay = DateTime.Now;

            LastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

            #region  Begin Get Contract Id Based on The Last Day


            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
            HtGetContractID.Add("@LastDay", LastDay);
            DataTable DTContractID = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPNameForGetContractID, HtGetContractID);

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
                bPaySheetDates = int.Parse(DTContractID.Rows[0]["PaySheetDates"].ToString());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not Avaialable For This Client.');", true);
                return;
            }

            #endregion  End Get Contract Id Based on The Last Day



            DateTime mGendays = DateTime.Now;
            DateTime dates = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            mGendays = DateTime.Parse(dates.ToString());
            Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bPaySheetDates);

            G_Sdays = Timings.Instance.Get_GS_Days(NewMonth, Gendays);
            int type = ddlForms.SelectedIndex;

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "ClientForms";
            HTPaysheet.Add("@ClientId", ddlclientid.SelectedValue);
            HTPaysheet.Add("@month", NewMonth);
            HTPaysheet.Add("@type", type);
            HTPaysheet.Add("@Gendays", Gendays);

            dt = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTPaysheet);
            string Filename = ddlcname.SelectedItem + "" + GetMonthName().ToUpper() + "" + GetMonthOfYear();
            if (dt.Rows.Count > 0)
            {
                GVUtill.ExporttoExcelded(dt, Filename, line1, line2, line3, line4, line5, line6);
            }

        }

        protected void btnnewPaySheetSalary_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string date = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            month = month + Year.Substring(2, 2);
            int NewMonth = Convert.ToInt32(month);

            string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedValue + "'";
            DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);
            string AddrHno = ""; string AddrColony = ""; string AddrArea = ""; string AddrStreet = ""; string Addrcity = ""; string Addrstate = ""; string Addrpin = "";
            if (dtclientaddress.Rows.Count > 0)
            {

                AddrHno += dtclientaddress.Rows[0]["ClientAddrHno"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrStreet"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrArea"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrColony"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrcity"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrstate"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrpin"].ToString() + "";
            }


            //string heading = ": " + companyName + "\n" + companyAddress + "\n\n";

            //string line = "FORM T<br>COMBINED MUSTER ROLL cum REGISTER OF WAGES<br>The Karnataka Shops & Commercial Establishments Act, 1961 & Rules 1963. ";

            //string line1 = "(See Rule 24(9-B) of Karnataka Shops & Commercial Establishment Rules,1963) in Lieu of<br>1. Form I,II of Rule 22(4); Form V & VII of Rules 29(1) & (5) of the Karnataka Minimum Wages Rules,1958.<br>2. Form I of Rules 3(1) of the Karnataka Payment of Wages Rules 1963<br>3. Form XIII of Rules 75; Form XV,XVII,XX,XXI,XXII,XXIII of Rule 78(1)(a)(i,(ii)&(iii) of the Contract Labour (Regulation & Abolition) Karnataka Rules 1974.<br>4. Form XIII of Rules 43; Form XVII,XVIII,XIX,XX,XXII of Rule 46(2)(a),(c) & (d) of Karnataka Interstate Migrant Workmen Rules, 1981";
            string line1 = "";

            string line2 = "Name Of The Establishment : <br>" + companyName + "<br>" + companyAddress + "";

            string line3 = "Contractor (if any)  :<br>" + companyName + "<br>" + companyAddress + "";

            string line4 = "Name & Address of Principal Employer : <br>" + ddlcname.SelectedItem + " <br> " + AddrHno;

            string line5 = "Place of work : ";

            string line6 = "MONTH & YEAR :" + GetMonthName().ToUpper() + "/" + Year;

            var Gendays = 0;
            var G_Sdays = 0;

            var ContractID = "";
            var bPaySheetDates = 0;

            DateTime LastDay = DateTime.Now;

            LastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

            #region  Begin Get Contract Id Based on The Last Day


            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
            HtGetContractID.Add("@LastDay", LastDay);
            DataTable DTContractID = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPNameForGetContractID, HtGetContractID);

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
                bPaySheetDates = int.Parse(DTContractID.Rows[0]["PaySheetDates"].ToString());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not Avaialable For This Client.');", true);
                return;
            }

            #endregion  End Get Contract Id Based on The Last Day



            DateTime mGendays = DateTime.Now;
            DateTime dates = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            mGendays = DateTime.Parse(dates.ToString());
            Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, bPaySheetDates);

            G_Sdays = Timings.Instance.Get_GS_Days(NewMonth, Gendays);
            int type = ddlForms.SelectedIndex;

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "ClientForms";
            HTPaysheet.Add("@ClientId", ddlclientid.SelectedValue);
            HTPaysheet.Add("@month", NewMonth);
            HTPaysheet.Add("@type", type);
            HTPaysheet.Add("@Gendays", Gendays);

            dt = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTPaysheet);
            string Filename = ddlcname.SelectedItem + "" + GetMonthName().ToUpper() + "" + GetMonthOfYear();
            if (dt.Rows.Count > 0)
            {
                GVUtill.ExporttoExcelsalaryded(dt, Filename, line1, line2, line3, line4, line5, line6);
            }

        }

        protected void btnformXIIIExcel(object sender, EventArgs e)
        {
            DataTable dt = null;
            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = SqlHelper.Instance.GetTableByQuery(strQry);
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";

            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            string date = "";
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedValue + "'";
            DataTable dtclientaddress = SqlHelper.Instance.GetTableByQuery(selectclientaddress);
            string AddrHno = ""; string AddrColony = ""; string AddrArea = ""; string AddrStreet = ""; string Addrcity = ""; string Addrstate = ""; string Addrpin = "";
            if (dtclientaddress.Rows.Count > 0)
            {

                AddrHno += dtclientaddress.Rows[0]["ClientAddrHno"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrStreet"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrArea"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrColony"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrcity"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrstate"].ToString() + "";
                AddrHno += dtclientaddress.Rows[0]["ClientAddrpin"].ToString() + "";
            }

            string selectcontracts = "select typeofwork from Contracts where clientid= '" + ddlclientid.SelectedValue + "'";
            DataTable dtcontraccts = SqlHelper.Instance.GetTableByQuery(selectcontracts);
            string typeofwork = "";
            if (dtcontraccts.Rows.Count > 0)
            {
                typeofwork = dtcontraccts.Rows[0]["typeofwork"].ToString() + "";
            }

            string line1 = "FORM - XIII <br> (See rule 75)<br> REGISTER OF WORKMEN EMPLOYED BY CONTRACTOR";

            string line2 = "Name and address of Contractor : " + companyName + " < br>" + companyAddress + "";

            string line3 = "Name and address of the establishment under which work is carried on : " + ddlcname.SelectedItem + " " + AddrHno + "";

            string line4 = "Nature and location of work : " + typeofwork;


            int type = ddlForms.SelectedIndex;
            string Spname = "ClientForms";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month);
            ht.Add("@year", Year);
            ht.Add("@type", type);
            ht.Add("@ClientID", ddlclientid.SelectedValue);
            dt = Config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;

            string Filename = ddlcname.SelectedItem + "";
            if (dt.Rows.Count > 0)
            {
                GVUtill.ExporttoExcelXIII(dt, Filename, line1, line2, line3, line4);
            }

        }

    }
}