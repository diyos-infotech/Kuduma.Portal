using System;
using System.Data;
using KLTS.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class LicenceDetailsReports : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string BranchID = "";

        protected void Page_Load(object sender, EventArgs e)
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
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli2");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                DataBinder();
                LoadClientNames();
                LoadClientList();

            }
        }

        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");
            ddlcname.Items.Insert(1, "ALL");

        }

        protected void LoadClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlClientId.DataValueField = "Clientid";
                ddlClientId.DataTextField = "Clientid";
                ddlClientId.DataSource = DtClientNames;
                ddlClientId.DataBind();
            }
            ddlClientId.Items.Insert(0, "-Select-");
            ddlClientId.Items.Insert(1, "ALL");
        }

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }     
        protected void DataBinder()
        {
            LblResult.Text = "";
            LblResult.Visible = true;
            string sqlQry = "Select Licenses.UnitId,LicenseStartDate,LicenseEndDate,LicenseExpired," +
                "LicenseOfficeLoc,Clients.ClientName from Clients INNER JOIN Licenses ON " +
                "Licenses.UnitId=Clients.ClientId";
            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;

            if (data.Rows.Count > 0)
            {
                dgLicExpire.DataSource = data;
                dgLicExpire.DataBind();
            }
            else
            {
                LblResult.Text = "There is no License Expiring Details";
            }
        }

        protected void btn_SubmitClick(object sender, EventArgs e)
        {
            LblResult.Text = "";
            LblResult.Visible = true;
            dgLicExpire.DataSource = null;
            dgLicExpire.DataBind();
            var clientid = "";
            if (ddlClientId.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Client ID/Client Name');", true);
                return;
            }

            if (ddlClientId.SelectedIndex > 0)
            {
                if(ddlClientId.SelectedIndex==1)
                {
                    clientid = "%";
                }
                else
                {
                    clientid = ddlClientId.SelectedValue;
                }
                LblResult.Text = "";
                LblResult.Visible = true;
                string sqlQry = "Select Licenses.UnitId,LicenseStartDate,LicenseEndDate,LicenseExpired," +
                    "LicenseOfficeLoc,Clients.ClientName from Clients INNER JOIN Licenses ON " +
                    "Licenses.UnitId=Clients.ClientId  and Licenses.UnitId like'" + clientid + "'";
                DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
                if (data.Rows.Count > 0)
                {
                    dgLicExpire.DataSource = data;
                    dgLicExpire.DataBind();
                }
                else
                {
                    LblResult.Text = "There IS No  License For The Selected Client";
                }
            }
            else
            {
                //LblResult.Text = "Plese Select The Client ID";
                DataBinder();
            }

        }        

        protected void GridData()
        {
            LblResult.Text = "";
            dgLicExpire.DataSource = null;
            dgLicExpire.DataBind();
        }
        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GridData();
            if (ddlcname.SelectedIndex > 0)
            {
                ddlClientId.SelectedValue = ddlcname.SelectedValue;

            }
            else
            {
                ddlClientId.SelectedIndex = 0;
            }
        }
        protected void ddlClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridData();
            if (ddlClientId.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlClientId.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
                Cleardata();
            }
        }
        protected void Cleardata()
        {
            LblResult.Text = "";
            ddlClientId.SelectedIndex = 0;
            ddlcname.SelectedIndex = 0;
            dgLicExpire.DataSource = null;
            dgLicExpire.DataBind();

        }
    }
}