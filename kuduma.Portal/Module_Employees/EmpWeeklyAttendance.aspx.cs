using System;
using System.Collections;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Globalization;
using Kuduma.Portal.DAL;


namespace Kuduma.Portal.Module_Employees
{
    public partial class EmpWeeklyAttendance : System.Web.UI.Page
    {

        AppConfiguration config = new AppConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GVAttendanceData.DataSource = null;
            GVAttendanceData.DataBind();

            var FromDate = DateTime.Now;
            var ToDate = DateTime.Now;

            if (txtFromdate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the From Date');", true);
                return;
            }
            if (txtTodate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the To Date');", true);
                return;
            }

            FromDate = DateTime.Parse(txtFromdate.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
            ToDate = DateTime.Parse(txtTodate.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));

            string SPName = "IMEmpWeeklyAttendance";
            Hashtable ht = new Hashtable();
            ht.Add("@Fromdate",FromDate);
            ht.Add("@ToDate", ToDate);

            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if(dt.Rows.Count>0)
            {
                GVAttendanceData.DataSource = dt;
                GVAttendanceData.DataBind();
            }
            



        }
    }
}