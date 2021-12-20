using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using KLTS.Data;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal.Module_Reports
{
    public partial class BillsVsActuals : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string BranchID = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil grvutil = new GridViewExportUtil();
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }
        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID= Session["BranchID"].ToString();
        }

        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix,dtBranch);
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

            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix,dtBranch);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
            ddlclientid.Items.Insert(1, "ALL");
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

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVBillsVsActuals.DataSource = null;
            GVBillsVsActuals.DataBind();

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

            {
                if (txtmonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                }

                string pryearval = "";
                string month = DateTime.Parse(date).Month.ToString();
                string Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                string Yearval = DateTime.Parse(date).Year.ToString();

                string Prevonemonth = string.Empty;
                string Prmonth = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).AddMonths(-1).ToString();
                Prevonemonth = DateTime.Parse(Prmonth).Month.ToString();

                string PrYear = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).Year.ToString().Substring(2, 2);


                if (month == "1")
                {
                    string PrYearone = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).AddYears(-1).ToString();
                    Yearval = DateTime.Parse(PrYearone).Year.ToString().Substring(2, 2);
                    string pryearval1 = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).AddYears(-1).ToString();
                    pryearval = DateTime.Parse(PrYearone).Year.ToString();
                }
                else
                {
                    PrYear = Year;
                    pryearval = Yearval;
                }

                string prevmonth = Prevonemonth + PrYear;

                int currentmonthdays = GlobalData.Instance.GetNoOfDaysForThisMonthNew(int.Parse(Yearval), int.Parse(month));
                int prevmonthdays = GlobalData.Instance.GetNoOfDaysForThisMonthNew(int.Parse(pryearval), int.Parse(Prevonemonth));



                DateTime DtLastDay = DateTime.Now;
                DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                string sqlqry = string.Empty;

                DataTable dt = null;


                string ClientID = "";

                if (ddlclientid.SelectedIndex == 1)
                {
                    ClientID = "%";
                }
                else
                {
                    ClientID = ddlclientid.SelectedValue;
                }

                if (ddlselection.SelectedIndex == 0)
                {
                    string SPName = "BillsVsActuals";
                    Hashtable ht = new Hashtable();
                    ht.Add("@clientid", ClientID);
                    ht.Add("@month", month + Year);
                    ht.Add("@LastDay", DtLastDay);
                    ht.Add("@PrMonthdays", prevmonthdays);
                    ht.Add("@premonth", prevmonth);
                    ht.Add("@Currentmonthdays", currentmonthdays);


                    dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
                }

                else if (ddlselection.SelectedIndex == 1)
                {
                    string SPName = "BillsVsActualsClientWise";
                    Hashtable ht = new Hashtable();
                    ht.Add("@clientid", ClientID);
                    ht.Add("@month", month + Year);
                    ht.Add("@LastDay", DtLastDay);
                    ht.Add("@PrMonthdays", prevmonthdays);
                    ht.Add("@premonth", prevmonth);
                    ht.Add("@Currentmonthdays", currentmonthdays);


                    dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
                }
                if (dt.Rows.Count > 0)
                {
                    GVBillsVsActuals.DataSource = dt;
                    GVBillsVsActuals.DataBind();
                    lbtn_Export.Visible = true;
                }
                else
                {
                    lbtn_Export.Visible = false;

                }
            }
        }





        protected void ClearData()
        {
            LblResult.Text = "";
            GVBillsVsActuals.DataSource = null;
            GVBillsVsActuals.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            grvutil.Export("BillVsActuals.xls", this.GVBillsVsActuals);
        }

        decimal ActualBillingAmt = 0;
        decimal BillingAmt = 0;
        decimal Difference = 0;


        protected void GVBillsVsActuals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ActualBillingAmt += decimal.Parse(e.Row.Cells[3].Text);
            //    BillingAmt += decimal.Parse(e.Row.Cells[4].Text);
            //    Difference += decimal.Parse(e.Row.Cells[5].Text);
            //}
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    e.Row.Cells[0].Text = "Total";
            //    e.Row.Cells[3].Text = ActualBillingAmt.ToString();
            //    e.Row.Cells[4].Text = BillingAmt.ToString();
            //    e.Row.Cells[5].Text = Difference.ToString();
            //}
        }
    }
}
