using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using KLTS.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ReportForStock : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        DataTable dt;
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

                    LoadFieldOfficer();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            GVInflowOutflowDetails.DataSource = null;
            GVInflowOutflowDetails.DataBind();
            if (ddloptions.SelectedIndex == 1)
            {
                if (ddlarmofc.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select ARM');", true);
                    return;
                }
            }

            if (txtFromDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select From Date');", true);
                return;
            }

            if (txtToDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select To Date');", true);
                return;
            }
            string spname = "";
            DataTable dtIOM = null;
            Hashtable HashtableIOM = new Hashtable();
            string FromDate = "";
            string ToDate = "";
            int options = 0;
            spname = "StockConsumptionReport";
           

            if (txtFromDate.Text.Trim().Length > 0)
            {
                FromDate = DateTime.Parse(txtFromDate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            if (txtToDate.Text.Trim().Length > 0)
            {
                ToDate = DateTime.Parse(txtToDate.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            if(ddloptions.SelectedIndex==0)
            { 
            options = ddloptions.SelectedIndex;
            HashtableIOM.Add("@fromdate", FromDate);
            HashtableIOM.Add("@todate", ToDate);
            HashtableIOM.Add("@Option", options);
            }

            else
            {
                string armofficer = "";
                if(ddlarmofc.SelectedIndex==1)
                {
                    armofficer = "%";
                }

                else
                {
                    armofficer = ddlarmofc.SelectedValue;
                }
                options = ddloptions.SelectedIndex;
                HashtableIOM.Add("@fromdate", FromDate);
                HashtableIOM.Add("@todate", ToDate);
                HashtableIOM.Add("@Option", options);
                HashtableIOM.Add("@ARMID", armofficer);
            }

            dtIOM = config.ExecuteAdaptorAsyncWithParams(spname, HashtableIOM).Result;
            if (dtIOM.Rows.Count > 0)
            {
                GVInflowOutflowDetails.DataSource = dtIOM;
                GVInflowOutflowDetails.DataBind();
            }
            else
            {
                GVInflowOutflowDetails.DataSource = null;
                GVInflowOutflowDetails.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Details For This Client');", true);

            }

        }

        protected void ClearData()
        {
            GVInflowOutflowDetails.DataSource = null;
            GVInflowOutflowDetails.DataBind();
        }



        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("StockConsumptionReport.xls", this.GVInflowOutflowDetails);
        }

        protected void LoadFieldOfficer()
        {
            #region New Code for Prefered Units as on 24/12/2013 by venkat

            string Query = "Select Empid,(EmpFname+' '+EmpMname+' '+EmpLname) as Empname from Empdetails where EmployeeType='S' and Empid like '" + CmpIDPrefix + "%' order by (EmpFname+' '+EmpMname+' '+EmpLname)";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlarmofc.DataValueField = "Empid";
                ddlarmofc.DataTextField = "Empname";
                ddlarmofc.DataSource = dt;
                ddlarmofc.DataBind();
            }
            ddlarmofc.Items.Insert(0,"Select");
            ddlarmofc.Items.Insert(1, "All");

            #endregion

        }

        protected void ddloptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddloptions.SelectedIndex==1)
            {
                ddlarmofc.Visible = true;
                txtFromDate.Text = "";
                txtToDate.Text = "";

            }
            else
            {
                ddlarmofc.Visible = false;
                txtFromDate.Text = "";
                txtToDate.Text = "";
            }
        }
    }
}