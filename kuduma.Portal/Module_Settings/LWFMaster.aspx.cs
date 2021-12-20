using System;
using System.Collections;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KLTS.Data;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class LWFMaster : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                }
                else
                {
                    Response.Redirect("login.aspx");
                }

                LoadStatenames();
                LoadMonths();


            }
        }

        protected void LoadStatenames()
        {
            DataTable DtStateNames = GlobalData.Instance.LoadStateNames();
            if (DtStateNames.Rows.Count > 0)
            {
                DdlStates.DataValueField = "StateID";
                DdlStates.DataTextField = "State";
                DdlStates.DataSource = DtStateNames;
                DdlStates.DataBind();
            }
            DdlStates.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void LoadMonths()
        {
            DataTable DtStateNames = GlobalData.Instance.GetMonthNames();
            if (DtStateNames.Rows.Count > 0)
            {
                DdlMonth.DataValueField = "number";
                DdlMonth.DataTextField = "MonthName";
                DdlMonth.DataSource = DtStateNames;
                DdlMonth.DataBind();
            }
            DdlMonth.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            var State = "0";
            var Month = "0";
            var MonthID = "0";

            var Type = 0;
            var EmployeeContribution = "0";
            var EmployeerContribution = "0";
            var Maximum = "0";
            var PerOn = 0;
            var DeductType = 0;
            if (DdlStates.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select State');", true);
                return;
            }
            if (ddlDeductType.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Deduct Type');", true);
                return;
            }

            if (ddlDeductType.SelectedIndex == 3)
            {
                if (DdlMonth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Month');", true);
                    return;
                }
            }

            State = DdlStates.SelectedValue;
            Type = ddltype.SelectedIndex;
            PerOn = ddlperon.SelectedIndex;
            DeductType = ddlDeductType.SelectedIndex;
            if (txtEmployeeContribution.Text.Trim().Length > 0)
            {
                EmployeeContribution = txtEmployeeContribution.Text;
            }
            if (txtEmployeerContribution.Text.Trim().Length > 0)
            {
                EmployeerContribution = txtEmployeerContribution.Text;
            }
            if (txtMaximum.Text.Trim().Length > 0)
            {
                Maximum = txtMaximum.Text;
            }

            if (DdlMonth.SelectedIndex > 0)
            {
                Month = DdlMonth.SelectedValue;
                if (Month.Trim().Length == 2)
                {
                    MonthID = Month;
                }
                else
                {
                    MonthID = "0" + Month;
                }
            }


            var ProcedureName = string.Empty;
            var IRecordStatus = 0;
            Hashtable HtSPParameters = new Hashtable();

            ProcedureName = "AddorModifyLWFMaster";
            HtSPParameters.Add("@State", State);
            HtSPParameters.Add("@Month", MonthID);
            HtSPParameters.Add("@Type", Type);
            HtSPParameters.Add("@EmployeeContribution", EmployeeContribution);
            HtSPParameters.Add("@EmployeerContribution", EmployeerContribution);
            HtSPParameters.Add("@Maximum", Maximum);
            HtSPParameters.Add("@PerOn", PerOn);
            HtSPParameters.Add("@DeductType", DeductType);

            IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
            if (IRecordStatus > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Details are Updated SucessFully.');", true);
                DdlStates.SelectedIndex = DdlMonth.SelectedIndex = ddltype.SelectedIndex = 0;
                txtEmployeeContribution.Text = txtEmployeerContribution.Text = txtMaximum.Text = "0";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Details are not  Updated.');", true);
            }


        }

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltype.SelectedIndex == 0)
            {
                lblperon.Visible = false;
                ddlperon.Visible = false;
            }
            else if (ddltype.SelectedIndex == 1)
            {
                lblperon.Visible = true;
                ddlperon.Visible = true;
            }
        }

        protected void DdlStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDeductType.SelectedIndex = 0;
            string GetQry = "Select * from LWFMaster where State='" + DdlStates.SelectedValue + "'";
            DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(GetQry).Result;
            if (Dt.Rows.Count > 0)
            {
                ddltype.SelectedIndex = int.Parse(Dt.Rows[0]["Type"].ToString());
                ddlperon.SelectedIndex = int.Parse(Dt.Rows[0]["PerOn"].ToString());
                DdlMonth.SelectedIndex = int.Parse(Dt.Rows[0]["Month"].ToString());
                ddlDeductType.SelectedIndex = int.Parse(Dt.Rows[0]["DeductType"].ToString());
                txtEmployeeContribution.Text = Dt.Rows[0]["EmployeeContribution"].ToString();
                txtEmployeerContribution.Text = Dt.Rows[0]["EmployeerContribution"].ToString();
                txtMaximum.Text = Dt.Rows[0]["Maximum"].ToString();

                if (ddltype.SelectedIndex == 0)
                {
                    lblperon.Visible = false;
                    ddlperon.Visible = false;
                }
                else
                {
                    lblperon.Visible = true;
                    ddlperon.Visible = true;
                }

                if (ddlDeductType.SelectedIndex == 3)
                {
                    lblmonth.Visible = true;
                    DdlMonth.Visible = true;
                }
                else
                {
                    lblmonth.Visible = false;
                    DdlMonth.Visible = false;
                }
            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            Hashtable HtsearchEmp = new Hashtable();
            string sp = "";

            sp = "LWFMasterExcel";
            DataTable dt = config.ExecuteReaderWithSPAsync(sp).Result;
            if (dt.Rows.Count > 0)
            {
                gve.ExporttoExcelLWfmaster(dt, "LWFMaster");

            }
        }

        protected void ddlDeductType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddlDeductType.SelectedIndex == 4)
            {
                lblmonth.Visible = true;
                DdlMonth.Visible = true;
            }
            else
            {
                lblmonth.Visible = false;
                DdlMonth.Visible = false;
            }

        }
    }
}