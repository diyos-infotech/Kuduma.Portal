using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class NYARejectedListBranchWise : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
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
                    if (this.Master != null)
                    {
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli1");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }
                    LoadBranchNames();
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void GetWebConfigdata()
        {
            BranchID = Session["BranchID"].ToString();
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void LoadBranchNames()
        {
            string query = "select BranchId,UPPER(BranchName) BranchName  From  BranchDetails Order by Branchid,BranchName";
            DataTable DtStateNames = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (DtStateNames.Rows.Count > 0)
            {
                ddlBranch.DataValueField = "BranchId";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataSource = DtStateNames;
                ddlBranch.DataBind();
            }

            ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            ddlBranch.Items.Insert(1, new System.Web.UI.WebControls.ListItem("ALL", "1"));
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "NYARejectedListBranchWise";

            string Branch = ddlBranch.SelectedValue;
            if (ddlBranch.SelectedIndex == 1)
            {
                Branch = "%";
            }

            HTPaysheet.Add("@Branch", Branch);
            HTPaysheet.Add("@EmpIDPrefix", EmpIDPrefix);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('No records found');", true);

            }

        }

        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("NYARejectedListBranchWise.xls", this.GVListEmployees);
        }
    }
}