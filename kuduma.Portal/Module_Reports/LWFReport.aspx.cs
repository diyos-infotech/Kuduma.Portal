using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using KLTS.Data;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class LWFReport : System.Web.UI.Page
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
                    LoadStatenames();
                }
            }
            catch (Exception ex)
            {

            }

        }
        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void LoadStatenames()
        {

            DataTable DtStateNames = GlobalData.Instance.LoadStateNames();
            if (DtStateNames.Rows.Count > 0)
            {
                ddlLWFState.DataValueField = "StateID";
                ddlLWFState.DataTextField = "State";
                ddlLWFState.DataSource = DtStateNames;
                ddlLWFState.DataBind();
            }

            ddlLWFState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            if (txtFromMonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please select from month');", true);
                return;
            }

            if (txtToMonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please select to month');", true);
                return;
            }


            string frommonth = string.Empty;
            string tomonth = string.Empty;
            string frommn = "";
            string tomn = "";

            if (txtFromMonth.Text.Trim().Length > 0)
            {
                frommonth = DateTime.Parse(txtFromMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();

                string month = DateTime.Parse(frommonth).Month.ToString("00");
                string Year = DateTime.Parse(frommonth).Year.ToString().Substring(2,2);

                frommn = Year + month;

            }


            if (txtToMonth.Text.Trim().Length > 0)
            {
                tomonth = DateTime.Parse(txtToMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();

                string month = DateTime.Parse(tomonth).Month.ToString("00");
                string Year = DateTime.Parse(tomonth).Year.ToString().Substring(2, 2);

                tomn = Year + month;

            }


            string sqlqry = string.Empty;

            var SPName = "";
            Hashtable HTPaysheet = new Hashtable();
            SPName = "LWFReport";

            string LWFState = ddlLWFState.SelectedValue;

            if (ddlLWFState.SelectedIndex == 0)
            {
                LWFState = "%";
            }

            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            HTPaysheet.Add("@frommonth", frommn);
            HTPaysheet.Add("@tomonth", tomn);
            HTPaysheet.Add("@LWFState", LWFState);
            HTPaysheet.Add("@Branch", dtBranch);

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
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("LWFReport.xls", this.GVListEmployees);
        }



        decimal TotalLWF = 0;

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");

                decimal LWF = decimal.Parse(((Label)e.Row.FindControl("lblLWF")).Text);
                TotalLWF += LWF;


            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalLWF = (Label)e.Row.FindControl("lblTotalLWF") as Label;
                lblTotalLWF.Text = TotalLWF.ToString();

            }

        }
    }
}