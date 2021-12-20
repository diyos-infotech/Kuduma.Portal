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
    public partial class MonthWiseFreezeandUnfreeze : System.Web.UI.Page
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
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();

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

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
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

            string qry = "";
            int status = 0;
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);

            if (ddloption.SelectedIndex == 1)
            {
                qry = "select * from emppaysheet where month='" + month + Year + "'";
                DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
                if (dt.Rows.Count > 0)
                {
                    qry = "update emppaysheet set Freezestatus=2  where  month='" + month + Year + "'";
                    status = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                }

                if (status > 0)
                {
                    lblalert.Text = "";
                    txtmonth.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Paysheets are Freezed for the selected month');", true);
                    return;
                }
            }
            else
            {
                qry = "select * from unitbill where month='" + month + Year + "'";
                DataTable dtu = config.ExecuteReaderWithQueryAsync(qry).Result;
                if (dtu.Rows.Count > 0)
                {
                    qry = "update unitbill set freezestatus=2 where  month='" + month + Year + "' ";
                    status = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                }

                qry = "select * from munitbill where month='" + month + Year + "'";
                DataTable dtm = config.ExecuteReaderWithQueryAsync(qry).Result;
                if (dtm.Rows.Count > 0)
                {
                    qry = "update munitbill set freezestatus=2 where  month='" + month + Year + "' ";
                    status = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                }

                if (status > 0)
                {
                    lblalert.Text = "";
                    txtmonth.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Bills are Freezed for the selected month');", true);
                    return;
                }
            }


        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string qry = "";

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
            if (ddloption.SelectedIndex == 1)
            {
                qry = "select * from emppaysheet where month='" + month + Year + "' and (FreezeStatus=0 or FreezeStatus=1)";
                DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
                if (dt.Rows.Count > 0)
                {
                    lblalert.Text = "Status: Few Paysheets are yet to be Freezed by Admin";
                }
                else
                {
                    lblalert.Text = "";
                }
            }
            else
            {
                qry = "select UnitId from UnitBill where month = '" + month + Year + "' and (FreezeStatus = 0 or FreezeStatus = 1 ) union select UnitId from MUnitBill where month='" + month + Year + "' and (FreezeStatus = 0 or FreezeStatus = 1 )";
                DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
                if (dt.Rows.Count > 0)
                {
                    lblalert.Text = "Status: Few Bills are yet to be Freezed by Admin";
                }
                else
                {
                    lblalert.Text = "";
                }
            }
        }

        protected void ddloption_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblalert.Text = "";
            txtmonth.Text = "";
        }
    }
}