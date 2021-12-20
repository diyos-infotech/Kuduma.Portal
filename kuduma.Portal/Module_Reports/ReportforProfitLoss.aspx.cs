using System;
using System.Web.UI;
using System.Data;
using KLTS.Data;
using System.Web.UI.HtmlControls;
using Kuduma.Portal.DAL;
using System.Collections;
using System.Globalization;

namespace Kuduma.Portal
{
    public partial class ReportforProfitLoss : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string CmpIDPrefix = "";
        string BranchID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {

                    //Loadclientids();
                    //Loadclientnames();
                    LoadBranches();
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
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void LoadBranches()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtBranches = GlobalData.Instance.LoadLoginBranch(dtBranch);
            if (DtBranches.Rows.Count > 0)
            {
                ddlBranch.DataValueField = "BranchId";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataSource = DtBranches;
                ddlBranch.DataBind();
            }
            ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-", "0"));
            ddlBranch.Items.Insert(1, new System.Web.UI.WebControls.ListItem("-All-", "1"));
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVProfitmargin.DataSource = null;
            GVProfitmargin.DataBind();

            ddlclientid.Items.Clear();
            ddlcname.Items.Clear();

            Loadclientids();
            Loadclientnames();
        }

        protected void Loadclientids()
        {
            string Branches = "";
            if (ddlBranch.SelectedIndex == 1)
            {
                Branches = BranchID;
            }
            else
            {
                Branches = ddlBranch.SelectedValue;
            }

            string Qry = "Select clientid,clientname from clients where BranchID in (" + Branches + ")";
            DataTable dtids = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
            if (dtids.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dtids;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "--Select--");
            ddlclientid.Items.Insert(1, "All");
        }

        protected void Loadclientnames()
        {
            string Branches = "";
            if (ddlBranch.SelectedIndex == 1)
            {
                Branches = BranchID;
            }
            else
            {
                Branches = ddlBranch.SelectedValue;
            }

            string Qry = "Select clientid,clientname from clients where BranchID in (" + Branches + ")";
            DataTable dtnames = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
            if (dtnames.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = dtnames;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "--Select--");
            ddlcname.Items.Insert(1, "All");
        }

        protected void Fillclientname()
        {
            if (ddlclientid.SelectedIndex > 1)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            if (ddlclientid.SelectedIndex == 1)
            {
                ddlcname.SelectedIndex = 1;
            }

            if (ddlclientid.SelectedIndex == 0)
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void Fillclientid()
        {
            if (ddlcname.SelectedIndex > 1)
            {
                ddlclientid.SelectedValue = ddlcname.SelectedValue;
            }
            if (ddlcname.SelectedIndex == 1)
            {
                ddlclientid.SelectedIndex = 1;
            }

            if (ddlcname.SelectedIndex == 0)
            {
                ddlclientid.SelectedIndex = 0;
            }

        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVProfitmargin.DataSource = null;
            GVProfitmargin.DataBind();
            if (ddlclientid.SelectedIndex > 0)
            {
                Fillclientname();
                //Displaydata();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }

        }

        protected void ddlclientname_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVProfitmargin.DataSource = null;
            GVProfitmargin.DataBind();
            if (ddlcname.SelectedIndex > 0)
            {
                Fillclientid();
                // Displaydata();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {

            if (ddlBranch.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Please Select Branch');", true);
                return;
            }

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select The Month')", true);
                return;
            }


            string date = string.Empty;
            string month = "";
            string Year = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            DataTable dt = null;

            GVProfitmargin.DataSource = null;
            GVProfitmargin.DataBind();


            string Clientid = ddlclientid.SelectedValue;

            if (ddlclientid.SelectedIndex == 1)
            {
                Clientid = "%";
            }

            month = DateTime.Parse(date).Month.ToString();
            Year = DateTime.Parse(date).Year.ToString();


            string Type = "0";

            if (ddlclientid.SelectedIndex == 1)
            {
                Type = "1";
            }
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

             string Branches = "";
            if (ddlBranch.SelectedIndex == 1)
            {
                Branches = BranchID;
            }
            else
            {
                Branches = ddlBranch.SelectedValue;
            }

            string SPName = "ProfitMargin";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientId", Clientid);
            ht.Add("@month", month + Year.Substring(2, 2));
            ht.Add("@Type", Type);
            ht.Add("@BranchID", dtBranch);
            ht.Add("@Branch", Branches);
            ht.Add("@Option", ddlBranch.SelectedIndex);

            dt = config.ExecuteAdaptorWithParams(SPName, ht).Result;

            if (dt.Rows.Count > 0)
            {
                GVProfitmargin.DataSource = dt;
                GVProfitmargin.DataBind();
            }

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("ProfitLoss.xls", this.GVProfitmargin);
        }

    }
}