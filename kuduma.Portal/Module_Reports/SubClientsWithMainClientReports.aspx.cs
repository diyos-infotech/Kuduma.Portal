using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class SubClientsWithMainClientReports : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
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

                    FillClientList();
                    FillClientNameList();
                }
            }
            catch (Exception EX)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }


        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {

            #region Begin Code  For Validation as on [16-11-2013]
            if (ddlclient.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The client');", true);
                return;
            }
            #endregion End  Code  For Validation as on [16-11-2013]

            #region  Begin Code For Variable Declaration   as on [16-11-2013]
            var SPName = "";
            var Condition = 0;

            DataTable DtListOfClients = null;
            Hashtable HtListOfClients = new Hashtable();

            #endregion End  Code For Variable Declaration  as on [16-11-2013]


            #region  Begin Code For Assign Values to Variables   as on [16-11-2013]
            SPName = "ReportForLoadSubclients";

            HtListOfClients.Add("@clientid", ddlclient.SelectedValue);
            HtListOfClients.Add("@clientidprefix", CmpIDPrefix);


            #endregion End  Code For Assign Values to Variables  as on [16-11-2013]

            #region  Begin code For Calling Stored Procedue  and Data To Gridview  As on [16-11-2013]
            DtListOfClients = config.ExecuteAdaptorAsyncWithParams(SPName, HtListOfClients).Result;
            if (DtListOfClients.Rows.Count > 0)
            {
                GVListOfClients.DataSource = DtListOfClients;
                GVListOfClients.DataBind();
            }
            else
            {

                GVListOfClients.DataSource = null;
                GVListOfClients.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Client Details Are Not Avaialable');", true);
            }

            #endregion End Code For Calling Stored Procedue and Data To Gridview  As on [16-11-2013]
        }

        protected void ddlclient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlclient.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlclient.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }

        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcname.SelectedIndex > 0)
            {
                ddlclient.SelectedValue = ddlcname.SelectedValue;

            }
            else
            {
                ddlclient.SelectedIndex = 0;
            }
        }


        protected void FillClientList()
        {
            DataTable dt = GlobalData.Instance.LoadMainunitCIds(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                ddlclient.DataValueField = "clientid";
                ddlclient.DataTextField = "clientid";
                ddlclient.DataSource = dt;
                ddlclient.DataBind();
            }
            ddlclient.Items.Insert(0, "--Select--");

        }

        protected void FillClientNameList()
        {
            DataTable dt = GlobalData.Instance.LoadMainunitCNames(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "Clientname";
                ddlcname.DataSource = dt;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "--Select--");

        }
     
    }
}