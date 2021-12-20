using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Collections;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class AndroidAttendanceSummarisedReport : System.Web.UI.Page
    {

        GridViewExportUtil GVUtil = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();
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

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            GvDayWiseAttendance.DataSource = null;
            GvDayWiseAttendance.DataBind();

            float totalTotalEmployees = 0;
            float totalAttendanceGiven = 0;
            float totalAttendanceNotGiven = 0;

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
            }


            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string AttMonth = month + Year.Substring(2, 2);

            string spname = "";
            DataTable dtBP = null;
            Hashtable HashtableBP = new Hashtable();

            spname = "AndroidAttendanceSummarisedReport";
            HashtableBP.Add("@Date", date);
            dtBP = config.ExecuteAdaptorAsyncWithParams(spname, HashtableBP).Result;
            if (dtBP.Rows.Count > 0)
            {

                GvDayWiseAttendance.DataSource = dtBP;
                GvDayWiseAttendance.DataBind();
                lbtn_Export.Visible = true;

                for (int i = 0; i < dtBP.Rows.Count; i++)
                {
                    string strTotalEmployees = dtBP.Rows[i]["TotalEmployees"].ToString();
                    if (strTotalEmployees.Trim().Length > 0)
                    {
                        totalTotalEmployees += Convert.ToSingle(strTotalEmployees);
                    }
                    string strAttendanceGiven = dtBP.Rows[i]["AttendanceGiven"].ToString();
                    if (strAttendanceGiven.Trim().Length > 0)
                    {
                        totalAttendanceGiven += Convert.ToSingle(strAttendanceGiven);
                    }
                    string strAttendanceNotGiven = dtBP.Rows[i]["AttendanceNotGiven"].ToString();
                    if (strAttendanceNotGiven.Trim().Length > 0)
                    {
                        totalAttendanceNotGiven += Convert.ToSingle(strAttendanceNotGiven);
                    }
                }
                Label lbltotalTotalEmployees = GvDayWiseAttendance.FooterRow.FindControl("lbltotalTotalEmployees") as Label;
                lbltotalTotalEmployees.Text = Math.Round(totalTotalEmployees).ToString();

                Label lbltotalAttendanceGiven = GvDayWiseAttendance.FooterRow.FindControl("lbltotalAttendanceGiven") as Label;
                lbltotalAttendanceGiven.Text = Math.Round(totalAttendanceGiven).ToString();

                Label lbltotalAttendanceNotGiven = GvDayWiseAttendance.FooterRow.FindControl("lbltotalAttendanceNotGiven") as Label;
                lbltotalAttendanceNotGiven.Text = Math.Round(totalAttendanceNotGiven).ToString();
                
            }
            else
            {
                GvDayWiseAttendance.DataSource = null;
                GvDayWiseAttendance.DataBind();
                lbtn_Export.Visible = false;
            }

        }

        protected void ClearData()
        {
            GvDayWiseAttendance.DataSource = null;
            GvDayWiseAttendance.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("Android Attendance Summarised Report.xls", this.GvDayWiseAttendance);

        }
    }
}