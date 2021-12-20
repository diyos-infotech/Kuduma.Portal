using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;


namespace VPL.Portal
{
    public partial class MonthlyReport : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil grvutil = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                        
                        FinancialYears();
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
        }

        protected void ClearData()
        {

            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

        }

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (ddlFinancialYears.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Financial Year');", true);
                return;
            }

            string FinancialYears = "";
            string CurrentYear = "";
            string NextYear = "";

            FinancialYears = ddlFinancialYears.SelectedValue;
            CurrentYear = FinancialYears.Substring(0, 4);
            NextYear = FinancialYears.Substring(7, 4);

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Text = "Apr - " + CurrentYear;
                e.Row.Cells[3].Text = "May - " + CurrentYear;
                e.Row.Cells[4].Text = "Jun - " + CurrentYear;
                e.Row.Cells[5].Text = "Jul - " + CurrentYear;
                e.Row.Cells[6].Text = "Aug - " + CurrentYear;
                e.Row.Cells[7].Text = "Sept - " + CurrentYear;
                e.Row.Cells[8].Text = "Oct - " + CurrentYear;
                e.Row.Cells[9].Text = "Nov - " + CurrentYear;
                e.Row.Cells[10].Text = "Dec - " + CurrentYear;
                e.Row.Cells[11].Text = "Jan - " + NextYear;
                e.Row.Cells[12].Text = "Feb - " + NextYear;
                e.Row.Cells[13].Text = "Mar - " + NextYear;


            }


        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            grvutil.Export("MonthlyReport.xls", this.GVListEmployees);
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {

            if (ddlFinancialYears.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Financial Year');", true);
                return;
            }

            string FinancialYears = "";
            string CurrentYear = "";
            string NextYear = "";

            FinancialYears = ddlFinancialYears.SelectedValue;
            CurrentYear = FinancialYears.Substring(0, 4);
            NextYear = FinancialYears.Substring(7, 4);

            int option = 0;

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "MonthlyReport";

            HTPaysheet.Add("@year", CurrentYear);
            HTPaysheet.Add("@NextYear", NextYear);
            HTPaysheet.Add("@year1", CurrentYear.Substring(2,2));
            HTPaysheet.Add("@NextYear1", NextYear.Substring(2, 2));

            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;
            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
                lbtn_Export.Visible = true;
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
                lbtn_Export.Visible = false;
            }
        }

        public void FinancialYears()
        {
            DataTable DtFinancialYears = GlobalData.Instance.LoadFinancialYears();

            if (DtFinancialYears.Rows.Count > 0)
            {
                ddlFinancialYears.DataValueField = "FinancialYears";
                ddlFinancialYears.DataTextField = "FinancialYears";
                ddlFinancialYears.DataSource = DtFinancialYears;
                ddlFinancialYears.DataBind();
            }
            ddlFinancialYears.Items.Insert(0, "--Select--");
        }
    }
}