using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KLTS.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class Monthwisefreezebillsandpaysheets : System.Web.UI.Page
    {
        AppConfiguration Config = new AppConfiguration();
        GridViewExportUtil GVUtill = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";

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

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void PreviligeUsers(int previligerid)
        {
           
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
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

            qry = "select * from emppaysheet where month='" + month + Year + "'";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {
                qry = "update emppaysheet set Freezestatus=1  where  month='" + month + Year + "'";
                status = Config.ExecuteNonQueryWithQueryAsync(qry).Result;
            }
            qry = "select * from unitbill where month='" + month + Year + "'";
            DataTable dtu = Config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dtu.Rows.Count > 0)
            {
                qry = "update unitbill set freezestatus=1 where  month='" + month + Year + "' ";
                status = Config.ExecuteNonQueryWithQueryAsync(qry).Result;
            }

            qry = "select * from munitbill where month='" + month + Year + "'";
            DataTable dtm = Config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dtm.Rows.Count > 0)
            {
                qry = "update munitbill set freezestatus=1 where  month='" + month + Year + "' ";
                status = Config.ExecuteNonQueryWithQueryAsync(qry).Result;
            }

            if (status > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('bills and Paysheets are Freeze for select month');", true);
                return;
            }

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


    }
}