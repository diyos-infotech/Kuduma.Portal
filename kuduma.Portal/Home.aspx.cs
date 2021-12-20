using System;
using System.Data;
using System.Web.UI;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class Home : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string UserName = "";
        string previligeid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            UserName = Session["UserId"].ToString();
            try
            {

                lblerror.Text = "";
                if (!IsPostBack)
                {
                    Session["EmpIDPrefix"] = string.Empty;
                    Session["CmpIDPrefix"] = string.Empty;
                    Session["BillnoWithoutST"] = string.Empty;
                    Session["BillnoWithST"] = string.Empty;
                    Session["BillprefixWithST"] = string.Empty;
                    Session["BillprefixWithoutST"] = string.Empty;
                    Session["InvPrefix"] = string.Empty;
                    Session["POPrefix"] = string.Empty;
                    Session["GRVPrefix"] = string.Empty;
                    Session["DCPrefix"] = string.Empty;
                    Session["InvBillPrefixwithST"] = string.Empty;
                    Session["InvBillPrefixwithoutST"] = string.Empty;


                    LoadAllCompanies();

                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login.aspx");

            }

        }
        protected void btn_Click(object sender, EventArgs e)
        {
            var PageLink = "";
            #region  Prefix values of the Client,Employee
            if (ddlbranchnames.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select The Branch');", true);
                return;
            }
            else
            {
                string SqlQryBranchPrefix = "Select * from Branchdetails  Where branchid='" + ddlbranchnames.SelectedValue + "'";
                DataTable DtBranchPrefix = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryBranchPrefix).Result;
                if (DtBranchPrefix.Rows.Count > 0)
                {
                    Session["EmpIDPrefix"] = DtBranchPrefix.Rows[0]["EmpPrefix"].ToString();
                    Session["CmpIDPrefix"] = DtBranchPrefix.Rows[0]["ClientIDPrefix"].ToString();
                    Session["BillnoWithST"] = DtBranchPrefix.Rows[0]["BillnoWithServicetax"].ToString();
                    Session["BillnoWithoutST"] = DtBranchPrefix.Rows[0]["BillNoWithoutServiceTax"].ToString();
                    Session["BillprefixWithST"] = DtBranchPrefix.Rows[0]["BillprefixWithST"].ToString();
                    Session["BillprefixWithoutST"] = DtBranchPrefix.Rows[0]["BillprefixWithoutST"].ToString();
                    Session["InvPrefix"] = DtBranchPrefix.Rows[0]["InvPrefix"].ToString();
                    Session["POPrefix"] = DtBranchPrefix.Rows[0]["POPrefix"].ToString();
                    Session["GRVPrefix"] = DtBranchPrefix.Rows[0]["GRVPrefix"].ToString();
                    Session["DCPrefix"] = DtBranchPrefix.Rows[0]["DCPrefix"].ToString();
                    Session["Branch"] = DtBranchPrefix.Rows[0]["BranchID"].ToString();
                    Session["BranchID"] = DtBranchPrefix.Rows[0]["BranchID"].ToString();
                    Session["InvBillPrefixwithST"] = DtBranchPrefix.Rows[0]["InvBillPrefixwithST"].ToString();
                    Session["InvBillPrefixwithoutST"] = DtBranchPrefix.Rows[0]["InvBillPrefixwithoutST"].ToString();

                }
               
            }

            #endregion

            string RedirectPage = "";
            string ID = "";
            string Query = "select distinct  M.REDIRECT_PAGE,M.ID from MENU_PREVILIGE PM inner join MENU M on M.MENU_ID=PM.Menu_ID inner join LoginDetails L on l.previligeid=pm.previligeid where M.PARENT_ID='PARENT' and L.UserName='" + UserName + "' and Access=1 order by id";

            DataTable DtPrefix = config.ExecuteAdaptorAsyncWithQueryParams(Query).Result;
            if (DtPrefix.Rows.Count > 0)
            {
                RedirectPage = DtPrefix.Rows[0]["REDIRECT_PAGE"].ToString();
                ID = DtPrefix.Rows[0]["ID"].ToString();
                if (ID == "1" && RedirectPage == "Reminders.aspx")
                {
                    Response.Redirect("~/Reminders.aspx");
                }
                if (ID == "2" && RedirectPage == "Employees.aspx")
                {
                    Response.Redirect("~/Module_Employees/Employees.aspx");
                }
                if (ID == "3" && RedirectPage == "Clients.aspx")
                {
                    Response.Redirect("~/Module_Clients/Clients.aspx");
                }
                if (ID == "4" && RedirectPage == "CompanyInfo.aspx")
                {
                    Response.Redirect("~/RedirectPage");
                }
                if (ID == "5" && RedirectPage == "Reports.aspx")
                {
                    Response.Redirect("~/Module_Reports/Reports.aspx");
                }
                if (ID == "6" && RedirectPage == "ViewItems.aspx")
                {
                    Response.Redirect("~/Module_Inventory/ViewItems.aspx");
                }
                if (ID == "7" && RedirectPage == "Settings.aspx")
                {
                    Response.Redirect("~/Module_Settings/Settings.aspx");
                }
            }
            Session["homepage"] = "Reminders.aspx";
            Response.Redirect("Reminders.aspx");

        }

        protected void LoadAllCompanies()
        {
            string Sqlqry = "select CompanyName,Branchid from Branchdetails where BranchID in (" + Session["BranchID"].ToString() + ") Order By BranchID";

            DataTable dtbranchnames = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dtbranchnames.Rows.Count > 0)
            {
                ddlbranchnames.DataTextField = "CompanyName";
                ddlbranchnames.DataValueField = "Branchid";
                ddlbranchnames.DataSource = dtbranchnames;
                ddlbranchnames.DataBind();
            }

            ddlbranchnames.Items.Insert(0, "--Select--");

        }
    }
}