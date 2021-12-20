using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using KLTS.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OfficeOpenXml;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class PFDetailsReportNew : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string BranchID = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();



        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            LblResult.Visible = true;
            LblResult.Text = "";
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
                LoadPFBranches();
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();

        }

        protected void LoadPFBranches()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtPFBranches = GlobalData.Instance.LoadPFbranches();
            if (DtPFBranches.Rows.Count > 0)
            {
                ddlPFBranch.DataValueField = "PFBranchid";
                ddlPFBranch.DataTextField = "PFBranchNo";
                ddlPFBranch.DataSource = DtPFBranches;
                ddlPFBranch.DataBind();
            }
            ddlPFBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-All-", "0"));
        }

        protected void ClearGridData()
        {
            LblResult.Text = "";
            GVPFDetails.DataSource = null;
            GVPFDetails.DataBind();
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString();
            //payMonth = GetMonth(monthname);

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
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return 0;
                }
            }
            #endregion


            #region  Month Get Based on the Control Selection
            int month = 0;

            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            month = Timings.Instance.GetIdForEnteredMOnth(date);
            return month;



            #endregion
        }

        public string GetMonth()
        {
            string month = "";
            string year = "";
            string DateVal = "";
            DateTime date;


            if (txtmonth.Text != "")
            {
                date = DateTime.ParseExact(txtmonth.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                month = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Month.ToString();
                year = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Year.ToString();

            }

            DateVal = month + year.Substring(2, 2);
            return DateVal;

        }

        protected void ddlPFBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtmonth.Text = "";
            GVListClients.DataSource = null;
            GVListClients.DataBind();

            GVPFDetails.DataSource = null;
            GVPFDetails.DataBind();
        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {
            GVPFDetails.DataSource = null;
            GVPFDetails.DataBind();

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The Month');", true);
                return;
            }

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string PFBranch = "%";
            if (ddlPFBranch.SelectedIndex > 0)
            {
                PFBranch = ddlPFBranch.SelectedValue;
            }

            string month = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).Month.ToString();
            string Year = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).Year.ToString();

            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            string Type = "PFClients";

            string SPName = "PFESIDetailsReport";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month + Year.Substring(2, 2));
            ht.Add("@Type", Type);
            ht.Add("@Branch", dtBranch);
            ht.Add("@PFbranch", PFBranch);


            DataTable dtone = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dtone.Rows.Count > 0)
            {
                GVListClients.DataSource = dtone;
                GVListClients.DataBind();
                lbtn_Export.Visible = true;
                lbtn_Export_Text.Visible = true;
                lbtn_Export_pfregister.Visible = true;
            }
            else
            {
                GVListClients.DataSource = null;
                GVListClients.DataBind();
            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVPFDetails.DataSource = null;
            GVPFDetails.DataBind();

            LblResult.Text = "";


            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);
                return;
            }

            string SelectDate = string.Empty;
            if (txtmonth.Text.Trim().Length > 0)
            {
                SelectDate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(SelectDate).Month.ToString();
            string Year = DateTime.Parse(SelectDate).Year.ToString();

            DateTime date = DateTime.Now;


            DateTime firstday = DateTime.Now;
            firstday = GlobalData.Instance.GetFirstDayMonth(int.Parse(Year), int.Parse(month));
            string fday = firstday.ToShortDateString();


            var list = new List<string>();
            
            string Date = "";
            if (month.Length == 1)
            {
                Date = "01/" + "0" + month + "/" + Year;
            }
            else
            {
                Date = "01/" + month + "/" + Year;
            }

            string PFBranch = "%";
            if (ddlPFBranch.SelectedIndex > 0)
            {
                PFBranch = ddlPFBranch.SelectedValue;
            }

            int monthdays = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(month));
            var datecheck = Timings.Instance.CheckDateFormat(Date);

            string Type = "PF";

            string SPName = "PFESIDetailsReport";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month + Year.Substring(2, 2));
            //ht.Add("@ClientIDList", dtClientList);
            ht.Add("@MonthDays", monthdays);
            ht.Add("@Type", Type);
            ht.Add("@Date", datecheck);
            ht.Add("@PFbranch", PFBranch);

            DataTable dtone = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dtone.Rows.Count > 0)
            {
                GVPFDetails.DataSource = dtone;
                GVPFDetails.DataBind();
            }
            else
            {
                GVPFDetails.DataSource = null;
                GVPFDetails.DataBind();
            }
            gve.Export("PFESIDetailsReport for the Month Of '" + GetMonthName() + '-' + GetMonthOfYear() + "'.xls", this.GVPFDetails);


        }

        protected void lbtn_Export_Text_Click(object sender, EventArgs e)
        {
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);
                return;
            }

            string SelectDate = string.Empty;
            if (txtmonth.Text.Trim().Length > 0)
            {
                SelectDate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(SelectDate).Month.ToString();
            string Year = DateTime.Parse(SelectDate).Year.ToString();
            
            DateTime date = DateTime.Now;


            DateTime firstday = DateTime.Now;
            firstday = GlobalData.Instance.GetFirstDayMonth(int.Parse(Year), int.Parse(month));
            string fday = firstday.ToShortDateString();


            

            string Date = "";
            if (month.Length == 1)
            {
                Date = "01/" + "0" + month + "/" + Year;
            }
            else
            {
                Date = "01/" + month + "/" + Year;
            }

            int monthdays = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(month));
            var datecheck = Timings.Instance.CheckDateFormat(Date);

            string PFBranch = "%";
            if (ddlPFBranch.SelectedIndex > 0)
            {
                PFBranch = ddlPFBranch.SelectedValue;
            }

            string Type = "PF";

            string SPName = "PFESIDetailsReport";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month + Year.Substring(2, 2));
            //ht.Add("@ClientIDList", dtClientList);
            ht.Add("@MonthDays", monthdays);
            ht.Add("@Type", Type);
            ht.Add("@Date", datecheck);
            ht.Add("@PFbranch", PFBranch);

            DataTable dtone = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dtone.Rows.Count > 0)
            {
                GVExportText.DataSource = dtone;
                GVExportText.DataBind();
            }
            else
            {
                GVExportText.DataSource = null;
                GVExportText.DataBind();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PFdetailsreporttocsv.txt");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder sBuilder = new System.Text.StringBuilder();

            for (int i = 0; i < GVExportText.Rows.Count; i++)
            {
                for (int k = 0; k < GVExportText.HeaderRow.Cells.Count; k++)
                {
                    sBuilder.Append(GVExportText.Rows[i].Cells[k].Text.TrimEnd().Replace(",", "#~#") + "#~#");
                }
                sBuilder.Remove(sBuilder.Length - 1, 1);
                sBuilder.Remove(sBuilder.Length - 1, 1);
                sBuilder.Remove(sBuilder.Length - 1, 1);
                sBuilder.Append("\r\n");
            }
            Response.Output.Write(sBuilder.ToString());
            Response.Flush();
            Response.End();
        }

        protected void lbtn_Export_pfregister_Click(object sender, EventArgs e)
        {
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);
                return;
            }

            string SelectDate = string.Empty;
            if (txtmonth.Text.Trim().Length > 0)
            {
                SelectDate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(SelectDate).Month.ToString();
            string Year = DateTime.Parse(SelectDate).Year.ToString();

            DateTime date = DateTime.Now;


            DateTime firstday = DateTime.Now;
            firstday = GlobalData.Instance.GetFirstDayMonth(int.Parse(Year), int.Parse(month));
            string fday = firstday.ToShortDateString();

            string PFBranch = "%";
            if (ddlPFBranch.SelectedIndex > 0)
            {
                PFBranch = ddlPFBranch.SelectedValue;
            }

            //var list = new List<string>();

            //if (GVListClients.Rows.Count > 0)
            //{
            //    for (int i = 0; i < GVListClients.Rows.Count; i++)
            //    {
            //        CheckBox chkclientid = GVListClients.Rows[i].FindControl("chkindividual") as CheckBox;
            //        Label lblclientid = GVListClients.Rows[i].FindControl("lblclientid") as Label;

            //        if (chkclientid.Checked == true)
            //        {
            //            list.Add(lblclientid.Text);
            //        }

            //    }
            //}

            //string Clientids = string.Join(",", list.ToArray());

            //if (list.Count == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select clientids');", true);
            //    return;
            //}

            //DataTable dtClientList = new DataTable();
            //dtClientList.Columns.Add("Clientid");
            //if (list.Count != 0)
            //{
            //    foreach (string s in list)
            //    {
            //        DataRow row = dtClientList.NewRow();
            //        row["Clientid"] = s;
            //        dtClientList.Rows.Add(row);
            //    }
            //}


            string Date = "";
            if (month.Length == 1)
            {
                Date = "01/" + "0" + month + "/" + Year;
            }
            else
            {
                Date = "01/" + month + "/" + Year;
            }

            int monthdays = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(month));
            var datecheck = Timings.Instance.CheckDateFormat(Date);

            string Type = "PFRegister";

            string SPName = "PFESIDetailsReport";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month + Year.Substring(2, 2));
           //ht.Add("@ClientIDList", dtClientList);
            ht.Add("@MonthDays", monthdays);
            ht.Add("@Type", Type);
            ht.Add("@Date", datecheck);
            ht.Add("@PFbranch", PFBranch);
            DataTable dtone = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dtone.Rows.Count > 0)
            {
                string filename = "PFRegister.xls";

                var products = dtone;
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("PFRegister");
                var totalCols = products.Columns.Count;
                var totalRows = products.Rows.Count;

                for (var col = 1; col <= totalCols; col++)
                {
                    workSheet.Cells[1, col].Value = products.Columns[col - 1].ColumnName;
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                }
                for (var row = 1; row <= totalRows; row++)
                {
                    for (var col = 0; col < totalCols; col++)
                    {
                        workSheet.Cells[row + 1, col + 1].Value = products.Rows[row - 1][col];
                    }

                }
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment ;filename=\"" + filename + "\"");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void GVPFDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Attributes.Add("class", "text");
            }
        }

        protected void GVExportText_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("class", "text");
            }
        }
    }
}