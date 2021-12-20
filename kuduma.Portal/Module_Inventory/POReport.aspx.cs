using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class POReport : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string GRVPrefix = "";
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

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

                GetPONos();
                GRVIDAuto();


            }

        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            GRVPrefix = Session["GRVPrefix"].ToString();

        }

        protected void GetPONos()
        {
            string sqlqry = "Select distinct pono from InvpoMaster  ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlPONo.DataTextField = "pono";
                ddlPONo.DataValueField = "pono";
                ddlPONo.DataSource = dt;
                ddlPONo.DataBind();

            }

            ddlPONo.Items.Insert(0, "-Select-");

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("POReport.xls", this.GVPODetails);
        }

        public void GRVIDAuto()
        {


            int GRVID = 1;
            string selectqueryclientid = "select (max(right(InflowID,4))) as GRVID from InvInflowMaster  where InflowID like '" + GRVPrefix + "%'";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;
            string invPrefix = string.Empty;

            if (dt.Rows.Count > 0)
            {

                if (String.IsNullOrEmpty(dt.Rows[0]["GRVID"].ToString()) == false)
                {
                    GRVID = Convert.ToInt32(dt.Rows[0]["GRVID"].ToString()) + 1;
                }
                else
                {
                    GRVID = int.Parse("1");
                }
            }






        }



        protected void btnsearch_Click(object sender, EventArgs e)
        {

            GVPODetails.DataSource = null;
            GVPODetails.DataBind();
            lbtn_Export.Visible = true;


            string spname = "";
            DataTable dtIOM = null;
            Hashtable HashtableIOM = new Hashtable();
            string PONo = "";
            string date = "";

            spname = "POPrint";
            PONo = ddlPONo.SelectedValue;


            //string month = DateTime.Parse(date).Month.ToString("00");
            //string Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);

            HashtableIOM.Add("@PONo", PONo);


            dtIOM =config.ExecuteAdaptorAsyncWithParams(spname, HashtableIOM).Result;
            if (dtIOM.Rows.Count > 0)
            {
                GVPODetails.DataSource = dtIOM;
                GVPODetails.DataBind();
            }
            else
            {
                GVPODetails.DataSource = null;
                GVPODetails.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Details For This Client');", true);

            }

        }

        protected void ClearData()
        {

            GVPODetails.DataSource = null;
            GVPODetails.DataBind();
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();


            //DateTime date = Convert.ToDateTime(ddlPONo.SelectedValue, CultureInfo.GetCultureInfo("en-gb"));
            //monthname = mfi.GetMonthName(date.Month).ToString();
            //payMonth = GetMonth(monthname);

            return monthname;
        }

        float totalQty = 0;
        float totalBuyingpriceUnit = 0;
        float totalBuyingprice = 0;
        float totalVat5Per = 0;
        float totalVat14Per = 0;
        float totalGrandTotal = 0;


        protected void GVPODetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                float lblQty = float.Parse(((Label)e.Row.FindControl("lblQty")).Text);
                totalQty += lblQty;

                float lblbuyingpriceunit = float.Parse(((Label)e.Row.FindControl("lblbuyingpriceunit")).Text);
                totalBuyingpriceUnit += lblbuyingpriceunit;

                if (((Label)e.Row.FindControl("lbltotalprice")).Text == "")
                {
                    ((Label)e.Row.FindControl("lbltotalprice")).Text = "0";
                }

                float lbltotalprice = float.Parse(((Label)e.Row.FindControl("lbltotalprice")).Text);
                totalBuyingprice += lbltotalprice;

                float lblvat5 = float.Parse(((Label)e.Row.FindControl("lblvat5")).Text);
             
                totalVat5Per += lblvat5;

                float lblvat14 = float.Parse(((Label)e.Row.FindControl("lblvat14")).Text);
                totalVat14Per += lblvat14;

                float lbltotal = float.Parse(((Label)e.Row.FindControl("lbltotal")).Text);
                totalGrandTotal += lbltotal;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalQty")).Text = totalQty.ToString();
                ((Label)e.Row.FindControl("lblTotalBuyingPriceunit")).Text = totalBuyingpriceUnit.ToString();
                ((Label)e.Row.FindControl("lblTotalBuyingPrice")).Text = totalBuyingprice.ToString();
                ((Label)e.Row.FindControl("lblTotalvat5")).Text = totalVat5Per.ToString();
                ((Label)e.Row.FindControl("lblTotalvat14")).Text = totalVat14Per.ToString();
                ((Label)e.Row.FindControl("lblTotaamount")).Text = totalGrandTotal.ToString();

            }
        }

    

    }
}