using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Globalization;
using System.Collections;
using Kuduma.Portal.DAL;


namespace Kuduma.Portal.Module_Reports
{
    public partial class ReportforBillingAndPaysheet : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetWebConfigdata();
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
            Elength = (EmpIDPrefix.Trim().Length + 1).ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Clength = (CmpIDPrefix.Trim().Length + 1).ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Select Month');", true);
                return;
            }

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string monthnew = string.Empty;

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            Hashtable References1 = new Hashtable();
            string SPReferencesName1 = "MonthWiseTotalBillsandPaysheetAmounts";
            References1.Add("@month", month + Year.Substring(2, 2));
            DataTable dtbillandPaysheet1 = config.ExecuteAdaptorAsyncWithParams(SPReferencesName1, References1).Result;
            if (dtbillandPaysheet1.Rows.Count > 0)
            {
                txtBduties.Text= dtbillandPaysheet1.Rows[0]["NormalBillingDuties"].ToString();
                txtBAmount.Text = dtbillandPaysheet1.Rows[0]["NormalBillingAmount"].ToString();
                txtBNoofClients.Text = dtbillandPaysheet1.Rows[0]["NormalNoofClients"].ToString();

                txtMduties.Text = dtbillandPaysheet1.Rows[0]["ManualBillingDuties"].ToString();
                txtMAmount.Text = dtbillandPaysheet1.Rows[0]["ManualBillingAmount"].ToString();
                txtMNoofClients.Text = dtbillandPaysheet1.Rows[0]["ManualNoofClients"].ToString();

                txtPDuties.Text = dtbillandPaysheet1.Rows[0]["PaysheetDuties"].ToString();
                txtPAmount.Text = dtbillandPaysheet1.Rows[0]["PaysheetAmount"].ToString();
                txtMNoofPaysheet.Text = dtbillandPaysheet1.Rows[0]["PaysheetNoofClients"].ToString();
            }


            Hashtable References = new Hashtable();
            string SPReferencesName = "BillsandPaysshetGenerateStatus";
            References.Add("@month", month + Year.Substring(2, 2));
            DataTable dtbillandPaysheet = config.ExecuteAdaptorAsyncWithParams(SPReferencesName, References).Result;
            if (dtbillandPaysheet.Rows.Count > 0)
            {
                GVBillingandpaysheet.DataSource = dtbillandPaysheet;
                GVBillingandpaysheet.DataBind();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('details are not available');", true);
                return;
            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("BillsandPaysheetStatus.xls", this.GVBillingandpaysheet);
        }
    }
}
