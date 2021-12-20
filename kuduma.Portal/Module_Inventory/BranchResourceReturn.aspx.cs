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
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Inventory
{
    public partial class BranchResourceReturn : System.Web.UI.Page
    {
        AppConfiguration Config = new AppConfiguration();
        GridViewExportUtil GVUtill = new GridViewExportUtil();
        DataTable dt;

        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string FromBranch = "";
        string Fontstyle = "";
        string CFontstyle = "";
        string Created_By = "";
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
                        Response.Redirect("Login.aspx");
                    }
                    LoadBranches();
                    FillTransactionId();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Created_By = Session["UserID"].ToString();
            BranchID = Session["BranchID"].ToString();
            FromBranch= Session["Branch"].ToString();
        }

        private void ClearData()
        {
            ddlTransactionId.SelectedIndex = 0;
            GVBranchResoure.DataSource = null;
            GVBranchResoure.DataBind();
        }

        protected void LoadBranches()
        {

            string qry = "select Branchid,branchname from invBranch where branchid in (" + BranchID + ") ";
            DataTable DtBranches = Config.ExecuteReaderWithQueryAsync(qry).Result;
            if (DtBranches.Rows.Count > 0)
            {
                ddlBranch.DataValueField = "BranchId";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataSource = DtBranches;
                ddlBranch.DataBind();
            }
            ddlBranch.Items.Insert(0, "--select--");
        }
        private void FillTransactionId()
        {
            var Tobranch = "";
            if (ddlBranch.SelectedIndex == 0)
            {
                Tobranch = "%";
            }
            else
            {
                Tobranch = ddlBranch.SelectedValue;
            }
            string query = "select distinct TransactionId from BranchResourceDetails where ToBranchID like'%" + Tobranch + "%'";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlTransactionId.DataValueField = "TransactionId";
                ddlTransactionId.DataTextField = "TransactionId";
                ddlTransactionId.DataSource = dt;
                ddlTransactionId.DataBind();
            }
            ddlTransactionId.Items.Insert(0, "-Select-");
            ddlTransactionId.Items.Insert(1, "ALL");
        }
        public void GetTotal()
        {
            float Total = 0;

            for (int i = 0; i < GVBranchResoure.Rows.Count; i++)
            {
                TextBox txtQty = GVBranchResoure.Rows[i].FindControl("txtQty") as TextBox;
                TextBox txtresourceprice = GVBranchResoure.Rows[i].FindControl("txtresourceprice") as TextBox;
                TextBox lblTotalAmount = GVBranchResoure.Rows[i].FindControl("lblTotalAmount") as TextBox;

                if (float.Parse(txtQty.Text) > 0)
                {
                    lblTotalAmount.Text = (Convert.ToSingle(txtQty.Text) * Convert.ToSingle(txtresourceprice.Text)).ToString();
                    Total += (Convert.ToSingle(txtQty.Text) * Convert.ToSingle(txtresourceprice.Text));
                }
            }
        }
        public void LoadItems()
        {
            GVBranchResoure.DataSource = null;
            GVBranchResoure.DataBind();

            string qry = "";
            var ToBranch = "";
            var TransactionId = "";
            if (ddlBranch.SelectedIndex == 0)
            {
                ToBranch = "%";
            }
            else
            {
                ToBranch = ddlBranch.SelectedValue;
            }
            if (ddlTransactionId.SelectedIndex == 0 || ddlTransactionId.SelectedIndex == 1)
            {
                TransactionId = "%";
            }
            else
            {
                TransactionId = ddlTransactionId.SelectedValue;
            }
            qry = "select itemid as ResourceID,ItemName,sellingprice as Price,0 as TotalAmt,qty as ActualQuantity,0 as Qty,'' as Balance,category from invStockItemList inv inner join BranchResourceDetails BRD on BRD.ResourceId=inv.ItemId  where BRD.TransactionId like'%" + TransactionId + "%' and BRD.ToBranchID like'%" + ToBranch + "%' order by category,ItemName";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(qry).Result;

            if (dt.Rows.Count > 0)
            {
                GVBranchResoure.DataSource = dt;
                GVBranchResoure.DataBind();

                GetTotal();
            }

        }
        protected void txtQty_TextChanged1(object sender, EventArgs e)
        {
            GetTotal();
        }

        protected void txtPaidAmnt_TextChanged(object sender, EventArgs e)
        {
            GetTotal();
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTransactionId.Items.Clear();
            FillTransactionId();
            ClearData();
        }

        protected void ddlTransactionId_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {


            #region check employee id
            if (ddlBranch.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Please select branch');", true);
                return;
            }
            #endregion

            string qry = "";
            DataTable dt = null;



            foreach (GridViewRow gvr in GVBranchResoure.Rows)
            {
                TextBox cbcheck = sender as TextBox;
                Control ctrlone = gvr.FindControl("TxtQty") as TextBox;
                TextBox TxtQty = (TextBox)ctrlone;

                if (float.Parse(TxtQty.Text) > 0)
                {
                    #region Begin Individual Resource Details of the employee

                    float Qty = 0;
                    float StockInHand = 0;

                    TextBox tb = (TextBox)gvr.FindControl("txtresourceprice");
                    Label resourcename = (Label)gvr.FindControl("lblresourcename");
                    Label lblresourceid = (Label)gvr.FindControl("lblresourceid");
                    TextBox txtQty = (TextBox)gvr.FindControl("txtQty");
                    Label lblCategory = (Label)gvr.FindControl("lblCategory");
                    Label txtIssuedQty = (Label)gvr.FindControl("lblactualqty");


                    if (txtIssuedQty.Text == "")
                    {
                        txtIssuedQty.Text = "0";
                    }

                    if (txtQty.Text == "")
                    {
                        txtQty.Text = "0";
                    }

                    if (float.Parse(txtQty.Text) > float.Parse(txtIssuedQty.Text))
                    {
                        gvr.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Resources cant be returned as return quantity is exceeding the issued quantity ');", true);
                        return;

                    }
                    else
                    {
                        gvr.ForeColor = System.Drawing.Color.Black;
                    }


                    #endregion
                }
            }

            string TransactionID = ddlTransactionId.SelectedValue;
            string LRNumber = "";

            float amount = 0;
            double sum = 0;
            double loanamnt = 0;

            string ToBranch = ddlBranch.SelectedValue;

            DataTable DtAddResource = null;

            Hashtable HTInserResource = new Hashtable();
            string SPName = "AddBranchResourceDetails";


            if (GVBranchResoure.Rows.Count > 0)
            {

                #region For Each for Gridview Indvidual Rows
                foreach (GridViewRow gvr in GVBranchResoure.Rows)
                {
                    TextBox cbcheck = sender as TextBox;
                    Control ctrlone = gvr.FindControl("txtQty") as TextBox;
                    TextBox txtQuantity = (TextBox)ctrlone;

                    if (float.Parse(txtQuantity.Text) > 0)
                    {
                        #region Begin Individual Resource Details of the employee

                        int Qty = 0;
                        float TotalPrice = 0;
                        string ResourceID = "";

                        TextBox tb = (TextBox)gvr.FindControl("txtresourceprice");
                        Label resourcename = (Label)gvr.FindControl("lblresourcename");
                        Label lblresourceid = (Label)gvr.FindControl("lblresourceid");
                        TextBox txtQty = (TextBox)gvr.FindControl("txtQty");

                        Qty = int.Parse(txtQty.Text);
                        ResourceID = lblresourceid.Text;
                        amount = float.Parse(tb.Text);
                        TotalPrice = Qty * amount;
                        sum += TotalPrice;

                        #region Begin New code for Insert Resource Details as on 19/07/2014

                        HTInserResource.Clear();
                        HTInserResource.Add("@FromBranch", FromBranch);
                        HTInserResource.Add("@ToBranch", ToBranch);
                        HTInserResource.Add("@Resourceid", ResourceID);
                        HTInserResource.Add("@Qty", Qty);
                        HTInserResource.Add("@Price", amount);
                        HTInserResource.Add("@TotalAmount", TotalPrice);
                        HTInserResource.Add("@LRNumber", LRNumber);
                        HTInserResource.Add("@TotalTransactionID", TransactionID);
                        HTInserResource.Add("@Created_By", Created_By);
                        HTInserResource.Add("@Type", 'R');
                        DtAddResource = Config.ExecuteAdaptorAsyncWithParams(SPName, HTInserResource).Result;

                        #endregion End New code for Insert Resource Details as on 19/07/2014


                        #endregion  //End  Individual Resource Details of the employee
                    }

                }
                #endregion  //End For Each for Gridview Indvidual Rows

                lblresult.Text = "Resource Returned successfully";

            }//end Gridview No Of

            ClearData();

        }
    }
}