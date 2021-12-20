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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using KLTS.Data;
using System.Globalization;
using System.Collections.Generic;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Inventory
{
    public partial class BranchResourceDetails : System.Web.UI.Page
    {
        AppConfiguration Config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Fontstyle = "";
        string CFontstyle = "";
        string Created_By = "";
        string FromBranch = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {

                        TransactionIDauto();
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                    LoadBranches();
                    LoadItems();
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
            FromBranch = Session["Branchid"].ToString();
        }

        protected void btncalculate_Click(object sender, EventArgs e)
        {
            GetTotal();

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

            TransactionIDauto();

            string qry = "";
            DataTable dt = null;



            foreach (GridViewRow gvr in GVUniformGrid.Rows)
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
                    //qry = "select actualquantity from InvStockItemList where itemid='" + lblresourceid.Text + "'"; //StockItemList
                    //dt = Config.ExecuteReaderWithQueryAsync(qry).Result;
                    qry = "Select ItemId,ItemName,Brand, (isnull(Openingstock,0))+((select isnull(sum(DeliveredQty), 0) from invinflowmaster im where im.itemid = isil.itemid and im.branchid = '" + FromBranch + "') +(select isnull(sum(Qty), 0) from BranchResourceDetails BRD where BRD.resourceid = isil.itemid and BRD.ToBranchID = '" + FromBranch + "') )-((select isnull(sum(Qty), 0) from BranchResourceDetails BRD where BRD.resourceid = isil.itemid and BRD.FromBranchID = '" + FromBranch + "')+(select isnull(sum(Qty), 0) from EmpResourceDetails im where im.resourceid = isil.itemid and branchid = '" + FromBranch + "')) as ActualQuantity,UnitMeasure,Sellingprice,GSTPer,HSNNumber,BuyingPrice from InvStockItemList isil where itemid='" + lblresourceid.Text + "'";
                    dt = Config.ExecuteReaderWithQueryAsync(qry).Result;

                    if (dt.Rows.Count > 0)
                    {
                        StockInHand = float.Parse(dt.Rows[0]["actualquantity"].ToString());
                    }

                    Qty = float.Parse(txtQty.Text);


                    if (Qty > StockInHand)//sandeep sir said as of now comment this alert
                    {
                        //lblresult.Text = "Resources are not allocated as quantity is exceeding stock in hand";
                        //gvr.ForeColor = System.Drawing.Color.Red;
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Resources are not allocated as quantity is exceeding stock in hand');", true);
                        //return;

                    }
                    else
                    {
                        gvr.ForeColor = System.Drawing.Color.Black;
                    }


                    #endregion
                }
            }

            string TransactionID = txtTransactionID.Text;
            string LRNumber = txtLRNumber.Text;

            float amount = 0;
            double sum = 0;
            double loanamnt = 0;

            string ToBranch = ddlBranch.SelectedValue;

            DataTable DtAddResource = null;

            Hashtable HTInserResource = new Hashtable();
            string SPName = "AddBranchResourceDetails";


            if (GVUniformGrid.Rows.Count > 0)
            {

                #region For Each for Gridview Indvidual Rows
                foreach (GridViewRow gvr in GVUniformGrid.Rows)
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
                        HTInserResource.Add("@Type", 'N');
                        DtAddResource = Config.ExecuteAdaptorAsyncWithParams(SPName, HTInserResource).Result;

                        #endregion End New code for Insert Resource Details as on 19/07/2014


                        #endregion  //End  Individual Resource Details of the employee
                    }
                }
                #endregion  //End For Each for Gridview Indvidual Rows

                lblresult.Text = "Resource Added successfully";

            }//end Gridview No Of

            ClearData();
        }

        protected void ClearData()
        {
            ddlBranch.SelectedIndex = 0;
            txtLRNumber.Text = "";
            LoadItems();
            TransactionIDauto();
        }

        public void GetTotal()
        {
            float Total = 0;

            for (int i = 0; i < GVUniformGrid.Rows.Count; i++)
            {
                TextBox txtQty = GVUniformGrid.Rows[i].FindControl("txtQty") as TextBox;
                TextBox txtresourceprice = GVUniformGrid.Rows[i].FindControl("txtresourceprice") as TextBox;
                TextBox lblTotalAmount = GVUniformGrid.Rows[i].FindControl("lblTotalAmount") as TextBox;

                if (float.Parse(txtQty.Text) > 0)
                {
                    lblTotalAmount.Text = (Convert.ToSingle(txtQty.Text) * Convert.ToSingle(txtresourceprice.Text)).ToString();
                    Total += (Convert.ToSingle(txtQty.Text) * Convert.ToSingle(txtresourceprice.Text));
                }
            }



            UpGv.Update();

        }

        private void TransactionIDauto()
        {
            int TransactionID;
            string selectqueryclientid = "select max(cast(TransactionID as int )) as TransactionID from BranchResourceDetails ";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(selectqueryclientid).Result;

            if (dt.Rows.Count > 0)
            {
                //  DtEmpId = dtempid.Rows[dtempid.Rows.Count - 1][0].ToString();
                if (String.IsNullOrEmpty(dt.Rows[0]["TransactionID"].ToString()) == false)
                {
                    TransactionID = Convert.ToInt32(dt.Rows[0]["TransactionID"].ToString()) + 1;
                    txtTransactionID.Text = TransactionID.ToString();
                }
                else
                {
                    TransactionID = int.Parse("1");
                    txtTransactionID.Text = TransactionID.ToString();
                }
            }
        }

        string Loanno = "";


        public void LoadItems()
        {
            GVUniformGrid.DataSource = null;
            GVUniformGrid.DataBind();

            string qry = "";

            qry = "select itemid as ResourceID,ItemName,sellingprice as Price,0 as TotalAmt,ActualQuantity,0 as Qty,'' as Balance,category from invStockItemList  order by category,ItemName";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(qry).Result;

            if (dt.Rows.Count > 0)
            {
                GVUniformGrid.DataSource = dt;
                GVUniformGrid.DataBind();

                GetTotal();
            }

        }

        protected void LoadBranches()
        {
            string qry = "select Branchid,branchname from invBranch where branchid in (" + FromBranch + ") ";
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

        protected void txtQty_TextChanged1(object sender, EventArgs e)
        {
            GetTotal();
        }

        protected void txtPaidAmnt_TextChanged(object sender, EventArgs e)
        {
            GetTotal();
        }
    }
}