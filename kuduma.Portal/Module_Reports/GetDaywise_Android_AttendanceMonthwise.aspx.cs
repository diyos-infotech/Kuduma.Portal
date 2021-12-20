using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class GetDaywise_Android_AttendanceMonthwise : System.Web.UI.Page
    {
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();
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
                    LoadBranches();

                    txtmonth.Visible = true;

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
            var Branch = "";
            if (ddlBranch.SelectedIndex == 1)
            {
                Branch = "%";
            }
            else
            {
                Branch = ddlBranch.SelectedValue;
            }
            string querybranch = "select clientid,Clientname from clients where branchid like'" + Branch + "' order by clientid";
            DataTable dtbranch = config.ExecuteAdaptorAsyncWithQueryParams(querybranch).Result;
            if (dtbranch.Rows.Count > 0)
            {
                ddlCName.DataValueField = "clientid";
                ddlCName.DataTextField = "Clientname";
                ddlCName.DataSource = dtbranch;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "--Select--");
            ddlCName.Items.Insert(1, "ALL");


        }
        protected void LoadBranches()
        {
            string querybranch = "select * from branchdetails order by branchid";
            DataTable dtbranch = config.ExecuteAdaptorAsyncWithQueryParams(querybranch).Result;
            if (dtbranch.Rows.Count > 0)
            {
                ddlBranch.DataValueField = "branchid";
                ddlBranch.DataTextField = "branchname";
                ddlBranch.DataSource = dtbranch;
                ddlBranch.DataBind();
            }
            ddlBranch.Items.Insert(0, "--Select--");
            ddlBranch.Items.Insert(1, "ALL");
        }

        protected void LoadClientList()
        {
            var Branch = "";
            if (ddlBranch.SelectedIndex == 1)
            {
                Branch = "%";
            }
            else
            {
                Branch = ddlBranch.SelectedValue;
            }
            string querybranch = "select clientid,Clientname from clients where branchid like'" + Branch + "' order by clientid";
            DataTable dtbranch = config.ExecuteAdaptorAsyncWithQueryParams(querybranch).Result;
            if (dtbranch.Rows.Count > 0)
            {
                ddlClientID.DataValueField = "clientid";
                ddlClientID.DataTextField = "clientid";
                ddlClientID.DataSource = dtbranch;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "--Select--");

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
            GvDayWiseAttendance.DataSource = null;
            GvDayWiseAttendance.DataBind();


            string clientid = "";
            var Branch = "";
            if (ddlBranch.SelectedIndex == 1)
            {
                Branch = "%";
            }
            else
            {
                Branch = ddlBranch.SelectedValue;
            }
            if (ddlClientID.SelectedIndex == 1)
            {
                clientid = ddlClientID.SelectedValue;
            }
            else
            {
                clientid = ddlClientID.SelectedValue;
            }


            string month = "";
            string Year = "";
            string AttMonth = "";



            if (txtmonth.Text != "")
            {
                string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString();

                AttMonth = month + Year.Substring(2, 2);
            }



            string spname = "";
            DataTable dtBP = null;
            Hashtable HashtableBP = new Hashtable();

            spname = "GetDaywise_Android_Attendance_Monthwise";
            HashtableBP.Add("@clientid", clientid);
            HashtableBP.Add("@Month", AttMonth);
            HashtableBP.Add("@Branch", Branch);


            dtBP = config.ExecuteAdaptorAsyncWithParams(spname, HashtableBP).Result;
            if (dtBP.Rows.Count > 0)
            {

                GvDayWiseAttendance.DataSource = dtBP;
                GvDayWiseAttendance.DataBind();
                lbtn_Export.Visible = true;
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
            GVUtil.Export("Get_Android_Attendance.xls", this.GvDayWiseAttendance);

        }


        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            GvDayWiseAttendance.DataSource = null;
            GvDayWiseAttendance.DataBind();
            LoadClientList();
            LoadClientNames();
        }

    }
}