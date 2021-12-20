using System;
using System.Web.UI;
using System.Data;
using System.Collections;
using KLTS.Data;
using System.Web.UI.HtmlControls;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class ActiveClientReports : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
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

                    // RadioActive_CheckedChanged(sender,e);
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


        protected void LoadAllCompanies()
        {
            string Sqlqry = "select CompanyName,Branchid from Branchdetails  Order By BranchID";
            //string Sqlqry = "select CompanyName,Branchid from Branchdetails  Order By BranchID";

            DataTable dtbranchnames = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dtbranchnames.Rows.Count > 0)
            {
                ddlbranchnames.DataTextField = "CompanyName";
                ddlbranchnames.DataValueField = "Branchid";
                ddlbranchnames.DataSource = dtbranchnames;
                ddlbranchnames.DataBind();
            }

            ddlbranchnames.Items.Insert(0, "ALL");

        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }
        protected void ClearData()
        {
            LblResult.Text = "";
            GVListOfClients.DataSource = null;
            GVListOfClients.DataBind();
        }

        private void BindData(string SqlQuery)
        {
            ClearData();
            LblResult.Visible = true;

            dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuery).Result;
            if (dt.Rows.Count > 0)
            {
                GVListOfClients.DataSource = dt;
                GVListOfClients.DataBind();
            }
            else
            {
                LblResult.Text = "There Is No Clients ";
            }
        }


        protected void Submit_Click(object sender, EventArgs e)
        {
            GVListOfClients.DataSource = null;
            GVListOfClients.DataBind();

            #region  Begin Code For Validation as on [16-11-2013]
            if (ddlClientsList.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Client Mode');", true);
                return;
            }
            #endregion End  Code For Validation as on [16-11-2013]

            #region  Begin Code For Variable Declaration   as on [16-11-2013]
            var SPName = "";
            var Condition = 0;
            string dts = "";

            DataTable DtListOfClients = null;
            Hashtable HtListOfClients = new Hashtable();

            #endregion End  Code For Variable Declaration  as on [16-11-2013]


            #region  Begin Code For Assign Values to Variables   as on [16-11-2013]
            Condition = ddlClientsList.SelectedIndex;
            SPName = "ReportForListOfclientsBasedonConditions";

            HtListOfClients.Add("@Condition", Condition);
            HtListOfClients.Add("@clientidprefix", CmpIDPrefix);
            if(ddlbranchnames.SelectedIndex==0)
            {
                dts = "%";
            }
            else
            {
                dts = ddlbranchnames.SelectedValue;
            }
            //DataTable dts = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            HtListOfClients.Add("@BranchID", dts);

            #endregion End  Code For Assign Values to Variables  as on [16-11-2013]

            #region  Begin code For Calling Stored Procedue  and Data To Gridview  As on [16-11-2013]
            DtListOfClients =config.ExecuteAdaptorAsyncWithParams(SPName, HtListOfClients).Result;
            if (DtListOfClients.Rows.Count > 0)
            {
                lbtn_Export.Visible = true;
                GVListOfClients.DataSource = DtListOfClients;
                GVListOfClients.DataBind();
            }
            else
            {
                lbtn_Export.Visible = false;
                GVListOfClients.DataSource = null;
                GVListOfClients.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Client Details Are Not Avaialable');", true);
            }

            #endregion End Code For Calling Stored Procedue and Data To Gridview  As on [16-11-2013]


        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("ClinetsList.xls", this.GVListOfClients);
        }

        protected void ddlClientsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllCompanies();
        }
    }
}