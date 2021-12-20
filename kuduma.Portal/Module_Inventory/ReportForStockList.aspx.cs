using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using KLTS.Data;

using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ReportForStockList : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string GRVPrefix = "";
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string BranchID = "";
        string Emp_Id = "";
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
            }

            GetData();

        }


        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                GRVPrefix = Session["GRVPrefix"].ToString();
                BranchID = Session["BranchID"].ToString();
                Emp_Id = Session["Emp_Id"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }


        }

        protected void GetData()
        {
            string sqlqry = "Select ItemId,ItemName,(isnull(openingstock,0)+ActualQuantity) as ActualQuantity,Category from InvStockItemList  ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVPODetails.DataSource = dt;
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
            return monthname;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("StockinHand.xls", this.GVPODetails);
        }

        float totalGrandTotal = 0;

        protected void GVPODetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {



                float lbltotal = float.Parse(((Label)e.Row.FindControl("lblStock")).Text);
                totalGrandTotal += lbltotal;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {

                ((Label)e.Row.FindControl("lbltotalStock")).Text = totalGrandTotal.ToString();

            }
        }

        protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
    }
}