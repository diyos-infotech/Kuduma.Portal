using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ReportforPBAttendance : System.Web.UI.Page
    {
        GridViewExportUtil gve = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string UserId = "";
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
                LoadClientList();
                LoadClientNames();

            }
        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            UserId = Session["UserId"].ToString();
            BranchID = Session["BranchID"].ToString();
        }


        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlclientid.SelectedValue = ddlcname.SelectedValue;

            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlclientid.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable dtnames = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
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

        protected void LoadClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable dtids = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
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

        protected void ClearData()
        {
            GVAttendance.DataSource = null;
            GVAttendance.DataBind();
        }


        protected void btnsearch_Click(object sender, EventArgs e)
        {

            GVAttendance.DataSource = null;
            GVAttendance.DataBind();

           

            if (ddlclientid.SelectedIndex == 0 && ddlcname.SelectedIndex == 0)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select Client ID/Name  Or Month')", true);
                    return;
                }
            }

            if (ddlclientid.SelectedIndex > 0 && ddlcname.SelectedIndex > 0)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select Client ID/Name  Or Month')", true);
                    return;
                }
                else
                {
                    string month = DateTime.Parse(txtmonth.Text.Trim()).Month.ToString();
                    string Year = DateTime.Parse(txtmonth.Text.Trim()).Year.ToString().Substring(2,2);

                    string clientid = ddlclientid.SelectedValue;

                    string Type = "1";

                    if(ddlclientid.SelectedIndex==1)
                    {
                        clientid = "%";
                        Type = "0";
                    }

                   

                    string SPName = "AttComparision";
                    Hashtable ht = new Hashtable();
                    ht.Add("@clientval", clientid);
                    ht.Add("@month", month+ Year);
                    ht.Add("@Type", Type);
                    ht.Add("@CmpIDPrefix", CmpIDPrefix);

                    DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName,ht).Result;

                    if(dt.Rows.Count>0)
                    {
                        GVAttendance.DataSource = dt;
                        GVAttendance.DataBind();
                    }

                }
            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("PBAttendance-(" + txtmonth.Text + ")" + ".xls", this.GVAttendance);
        }
    }
}