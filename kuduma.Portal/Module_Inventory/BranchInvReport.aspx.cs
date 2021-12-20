using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Inventory
{
    public partial class BranchInvReport : System.Web.UI.Page
    {
        GridViewExportUtil GVUtill = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();
        DataTable dt;

        string GRVPrefix = "";
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        string Created_By = "";
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
                        Response.Redirect("Login.aspx");
                    }
                    FillTransactionId();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Created_By = Session["UserID"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        private void FillTransactionId()
        {
            string query = "select distinct TransactionId from BranchResourceDetails order by TransactionId desc";
            DataTable dt = config.ExecuteReaderWithQueryAsync(query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlTransactionId.DataValueField = "TransactionId";
                ddlTransactionId.DataTextField = "TransactionId";
                ddlTransactionId.DataSource = dt;
                ddlTransactionId.DataBind();
            }
            ddlTransactionId.Items.Insert(0, "-Select-");
            ddlTransactionId.Items.Insert(1, "ALL");
        }

        protected void Btn_Submit_OnClick(object sender, EventArgs e)
        {
            var testDate = 0;
            var FromDate = DateTime.Now;
            var ToDate = DateTime.Now;

            if (Txt_From_Date.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select the From Date');", true);
                return;
            }
            else
            {
                testDate = GlobalData.Instance.CheckEnteredDate(Txt_From_Date.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You have Entered Invalid DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }

                FromDate = DateTime.Parse(Txt_From_Date.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));

            }
            if (Txt_ToDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select the To Date');", true);
                return;
            }
            else
            {
                testDate = GlobalData.Instance.CheckEnteredDate(Txt_ToDate.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You have Entered Invalid DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }

                ToDate = DateTime.Parse(Txt_ToDate.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
            }


            string Branch = "";
            var TransactionId = "";
            if (ddlTransactionId.SelectedIndex == 1)
            {
                TransactionId = "%";
            }
            else
            {
                TransactionId = ddlTransactionId.SelectedValue;
            }
            string Spname = "BranchInvReport";
            Hashtable ht = new Hashtable();
            ht.Add("@fromdate", FromDate);
            ht.Add("@todate", ToDate);
            ht.Add("@BranchID", Branch);
            //ht.Add("@TransactionId", TransactionId);
            DataTable envReports = config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;
            if (envReports.Rows.Count > 0)
            {
                GVListOfItems.DataSource = envReports;
                GVListOfItems.DataBind();
            }
            else
            {
                GVListOfItems.DataSource = null;
                GVListOfItems.DataBind();
            }
        }

        protected void Lnkbtnexcel_Click(object sender, EventArgs e)
        {
            var testDate = 0;
            var FromDate = DateTime.Now;
            var ToDate = DateTime.Now;

            if (Txt_From_Date.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select the From Date');", true);
                return;
            }
            else
            {
                testDate = GlobalData.Instance.CheckEnteredDate(Txt_From_Date.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You have Entered Invalid DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }

                FromDate = DateTime.Parse(Txt_From_Date.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));

            }
            if (Txt_ToDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select the To Date');", true);
                return;
            }
            else
            {
                testDate = GlobalData.Instance.CheckEnteredDate(Txt_ToDate.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You have Entered Invalid DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }

                ToDate = DateTime.Parse(Txt_ToDate.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
            }


            string companyName = "Your Company Name";

            string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
            }


            var TransactionId = "";
            if (ddlTransactionId.SelectedIndex == 1)
            {
                TransactionId = "%";
            }
            else
            {
                TransactionId = ddlTransactionId.SelectedValue;
            }

            string Spname = "BranchInvReport";
            Hashtable ht = new Hashtable();
            ht.Add("@fromdate", FromDate);
            ht.Add("@todate", ToDate);
            //ht.Add("@TransactionId", TransactionId);

            string line = companyName;
            string line1 = "From Date :  " + FromDate.ToString("dd/MM/yyyy") + "         " + "To Date :  " + ToDate.ToString("dd/MM/yyyy");
            string line2 = "InvReport";

            DataTable envReports = config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;


            if (envReports.Rows.Count > 0)
            {

                GVUtill.ExporttoExcel1(envReports, line, line1, line2);

            }
            else
            {
                GVListOfItems.DataSource = null;
                GVListOfItems.DataBind();
            }


        }
    }
}