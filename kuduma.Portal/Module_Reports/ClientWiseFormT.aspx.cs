using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class ClientWiseFormT : System.Web.UI.Page
    {
        string CmpIDPrefix = "";
        string BranchID = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                    FillClientList();
                    FillClientNameList();
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

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                ddlclientid.SelectedValue = ddlcname.SelectedValue;
            }
            else
            {

                ddlclientid.SelectedIndex = 0;
            }

        }

        protected void ddlclientid_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlclientid.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void FillClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dt;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");

        }

        protected void FillClientNameList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "Clientname";
                ddlcname.DataSource = dt;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");
        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();

        }

        public void ClearData()
        {
            GVData.DataSource = null;
            GVData.DataBind();
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client Id/Name');", true);

                return;
            }

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);

            DataTable dt = null;
            string ClientID = "";

            ClientID = ddlclientid.SelectedValue;

            string SPName = "SP_FORMT";
            Hashtable ht = new Hashtable();
            ht.Add("@clientid", ClientID);
            ht.Add("@month", month + Year);

            dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
            if (dt.Rows.Count > 0)
            {
                GVData.DataSource = dt;
                GVData.DataBind();
                lbtn_Export.Visible = true;
            }
            else
            {
                lbtn_Export.Visible = false;
            }

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("FormT.xls", this.GVData);
        }
    }
}