using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class InvInflowDetails : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string GRVPrefix = "";
        string CmpIDPrefix = "";
        string EmpIDPrefix = "";
        string UserID = "";
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

                GetPONos();
                txtInflowID.Text = GRVIDAuto().ToString();
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

        }


        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                UserID = Session["UserId"].ToString();
                GRVPrefix = Session["GRVPrefix"].ToString();
                BranchID= Session["BranchID"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }


        public string GRVIDAuto()
        {


            int GRVID = 1;
            string selectqueryclientid = "select (max(right(InflowID,4))) as GRVID from InvInflowMaster where InflowID like '" + GRVPrefix + "%'";
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
            return GRVPrefix + (GRVID).ToString("0000");
        }

        protected void GetPONos()
        {
            string sqlqry = "Select distinct pono from InvpoMaster";
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


        public void GetItemList()
        {
            string qry = "";


            qry = "select ipm.ItemId,isil.itemname,ipm.qty,isil.UnitMeasure,ipm.BuyingPrice,ipm.itemremarks from invpomaster ipm inner join invstockitemlist isil on isil.itemid=ipm.itemid where pono='" + ddlPONo.SelectedValue + "' and (ipm.DeliveryStatus='NYD' or ipm.deliverystatus='PartiallyDelivered')";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                gvresources.DataSource = dt;
                gvresources.DataBind();

                foreach (GridViewRow gvrow in gvresources.Rows)
                {
                    string itemid = ((Label)gvrow.FindControl("lblItemID")).Text;
                    Label lblOrderedQty = ((Label)gvrow.FindControl("lblOrderedQty")) as Label;
                    Label lblAlreadyDeliveredQty = ((Label)gvrow.FindControl("lblAlreadyDeliveredQty")) as Label;
                    Label lblBalance = ((Label)gvrow.FindControl("lblBalance")) as Label;
                    TextBox txtDeliverdQty = ((TextBox)gvrow.FindControl("txtDeliverdQty")) as TextBox;

                    qry = "select isnull(sum(DeliveredQty),0) as DeliveredQty from invinflowmaster where pono='" + ddlPONo.SelectedValue + "' and itemid = '" + itemid + "'";
                    dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["DeliveredQty"].ToString().Length > 0)
                        {
                            lblAlreadyDeliveredQty.Text = dt.Rows[0]["DeliveredQty"].ToString();
                            lblBalance.Text = (float.Parse(lblOrderedQty.Text) - float.Parse(lblAlreadyDeliveredQty.Text)).ToString();
                        }
                        else
                        {
                            lblAlreadyDeliveredQty.Text = "0";
                            lblBalance.Text = (float.Parse(lblOrderedQty.Text) - float.Parse(lblAlreadyDeliveredQty.Text)).ToString();

                        }
                    }
                    else
                    {
                        lblAlreadyDeliveredQty.Text = "0";
                        lblBalance.Text = (float.Parse(lblOrderedQty.Text) - float.Parse(lblAlreadyDeliveredQty.Text)).ToString();

                    }

                }


            }
            else
            {
                gvresources.DataSource = null;
                gvresources.DataBind();
            }
        }



        protected void ddlPONo_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            lblresult.Text = "";
            lblresult.Visible = false;
            GetItemList();
            ManualInfID();
            uppanel1.Update();

        }

        public void ManualInfID()
        {
            txtManualInfID.Text = "";
            string qry = "select deliveryat,month from InvPOMaster where pono='" + ddlPONo.SelectedValue + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            string DeliveryAt = "";
            string date = "";
            if (dt.Rows.Count > 0)
            {
                DeliveryAt = dt.Rows[0]["deliveryat"].ToString();
                if (DeliveryAt == "Office")
                {
                    txtManualInfID.Enabled = false;
                    txtManualInfID.Text = txtInflowID.Text;
                    lblmonth.Visible = false;
                    txtMonth.Visible = false;

                }
                else if (DeliveryAt == "Client")
                {
                    txtManualInfID.Enabled = true;
                    lblmonth.Visible = true;
                    txtMonth.Visible = true;
                    txtMonth.Enabled = false;
                    if (dt.Rows.Count > 0)
                    {
                        date = dt.Rows[0]["month"].ToString();
                        if (dt.Rows[0]["month"].ToString().Length > 0)
                        {
                            string year = dt.Rows[0]["month"].ToString().Substring(0, 2);
                            string month = dt.Rows[0]["month"].ToString().Substring(2, 2);
                            string monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(month)).Substring(0, 3);

                            txtMonth.Text = monthname.ToString() + "-20" + year;
                        }
                        else
                        {
                            txtMonth.Text = "";
                        }

                    }

                }
            }
        }

        protected void txtDeliverdQty_OnTextChanged(object sender, EventArgs e)
        {
            float TotalQty = 0;

            TextBox txtDeliverdQty = sender as TextBox;
            GridViewRow row = null;
            if (txtDeliverdQty == null)
                return;

            row = (GridViewRow)txtDeliverdQty.NamingContainer;
            if (row == null)
                return;


            Label lblOrderedQty = row.FindControl("lblOrderedQty") as Label;
            Label lblAlreadyDeliveredQty = row.FindControl("lblAlreadyDeliveredQty") as Label;


            if (lblOrderedQty.Text == "")
            {
                lblOrderedQty.Text = "0";
            }

            if (lblAlreadyDeliveredQty.Text == "")
            {
                lblAlreadyDeliveredQty.Text = "0";
            }

            if (txtDeliverdQty.Text == "")
            {
                txtDeliverdQty.Text = "0";
            }

            TotalQty = (float.Parse(txtDeliverdQty.Text) + float.Parse(lblAlreadyDeliveredQty.Text));

            if (TotalQty > float.Parse(lblOrderedQty.Text))
            {
                row.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Delivery quantity is more than the Ordered quantity');", true);
                txtDeliverdQty.Text = "0";
                txtDeliverdQty.Focus();
                return;
            }
            else
            {
                row.ForeColor = System.Drawing.Color.Black;

            }

        }

        float VATCmp1per = 0;
        float VATCmp2per = 0;
        float VATCmp3per = 0;
        float VATCmp4per = 0;
        float VATCmp5per = 0;

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(5000);

            float TotalQty = 0;
            if (txtDate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Inflow Date');", true);
                return;
            }

            string InflowDate = "";

            if (txtDate.Text.Trim().Length != 0)
            {
                InflowDate = Timings.Instance.CheckDateFormat(txtDate.Text);

            }
            else
            {
                InflowDate = "01/01/1900";
            }

            //lblresult.Text = "";
            //lblresult.Visible = true;
            try
            {



                string CheckInflowID = "select InflowId from  InvInflowMaster where InflowId='" + txtInflowID.Text + "' ";
                DataTable dtInflowID = config.ExecuteAdaptorAsyncWithQueryParams(CheckInflowID).Result;
                if (dtInflowID.Rows.Count > 0)
                {
                    txtInflowID.Text = GRVIDAuto().ToString();

                }


                string pono = ddlPONo.SelectedValue;

                if (ddlPONo.SelectedIndex == 0)
                {
                    lblresult.Text = "Please select PO No";
                    return;
                }



                DateTime Created_On = DateTime.Now;



                // bool flag = false;
                for (int i = 0; i < gvresources.Rows.Count; i++)
                {
                    double TotalInflowPrice = 0;
                    double VatCmp1Val = 0;
                    double VatCmp2Val = 0;
                    double VatCmp3Val = 0;
                    double VatCmp4Val = 0;
                    double VatCmp5Val = 0;


                    Label lblOrderedQty = gvresources.Rows[i].FindControl("lblOrderedQty") as Label;
                    Label lblAlreadyDeliveredQty = gvresources.Rows[i].FindControl("lblAlreadyDeliveredQty") as Label;
                    TextBox txtDeliverdQty = gvresources.Rows[i].FindControl("txtDeliverdQty") as TextBox;
                    Label lblItemID = gvresources.Rows[i].FindControl("lblItemID") as Label;
                    Label lblBuyingPrice = gvresources.Rows[i].FindControl("lblBuyingPrice") as Label;

                    if (lblBuyingPrice.Text == "")
                    {
                        lblBuyingPrice.Text = "0";
                    }

                    if (txtDeliverdQty.Text == "")
                    {
                        txtDeliverdQty.Text = "0";
                    }

                    TotalInflowPrice = Math.Round(double.Parse(txtDeliverdQty.Text) * double.Parse(lblBuyingPrice.Text), 2);

                    #region for VAT percent yes/no Values

                    DataTable dt = GlobalData.Instance.LoadVATCheckforItem(lblItemID.Text);

                    string VATCmp1 = "False";
                    string VATCmp2 = "False";
                    string VATCmp3 = "False";
                    string VATCmp4 = "False";
                    string VATCmp5 = "False";

                    if (dt.Rows.Count > 0)
                    {
                        VATCmp1 = dt.Rows[0]["VATCmp1"].ToString();
                        VATCmp2 = dt.Rows[0]["VATCmp2"].ToString();
                        VATCmp3 = dt.Rows[0]["VATCmp3"].ToString();
                        VATCmp4 = dt.Rows[0]["VATCmp4"].ToString();
                        VATCmp5 = dt.Rows[0]["VATCmp5"].ToString();

                    }

                    #endregion for VAT percent yes/no Values

                    #region for VAT percent Values

                    DataTable dtTaxCmpts = GlobalData.Instance.LoadTaxComponents();
                    if (dtTaxCmpts.Rows.Count > 0)
                    {
                        VATCmp1per = float.Parse(dtTaxCmpts.Rows[10]["TaxCmpPer"].ToString());
                        VATCmp2per = float.Parse(dtTaxCmpts.Rows[11]["TaxCmpPer"].ToString());
                        VATCmp3per = float.Parse(dtTaxCmpts.Rows[12]["TaxCmpPer"].ToString());
                        VATCmp4per = float.Parse(dtTaxCmpts.Rows[13]["TaxCmpPer"].ToString());
                        VATCmp5per = float.Parse(dtTaxCmpts.Rows[14]["TaxCmpPer"].ToString());
                    }


                    #endregion for VAT percent Values


                    if (VATCmp1 == "True")
                    {
                        VatCmp1Val = Math.Round(TotalInflowPrice * (VATCmp1per) / 100, 2);
                    }

                    if (VATCmp2 == "True")
                    {
                        VatCmp2Val = Math.Round(TotalInflowPrice * (VATCmp2per) / 100, 2);
                    }

                    if (VATCmp3 == "True")
                    {
                        VatCmp3Val = Math.Round(TotalInflowPrice * (VATCmp3per) / 100, 2);
                    }

                    if (VATCmp4 == "True")
                    {
                        VatCmp4Val = Math.Round(TotalInflowPrice * (VATCmp4per) / 100, 2);
                    }

                    if (VATCmp5 == "True")
                    {
                        VatCmp5Val = Math.Round(TotalInflowPrice * (VATCmp5per) / 100, 2);
                    }
               
                    if (float.Parse(txtDeliverdQty.Text) > 0)
                    {

                        string insertquery = string.Format("insert into InvInflowMaster(pono,InflowId,ItemId,OrderedQty,DeliveredQty,InflowPrice,TotalInflowPrice,VATCmp1,VATCmp2,VATCmp3,VATCmp4,VATCmp5,ManualInflowId,InflowDate,Created_By,Created_On" +
                        " ) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",
                          pono, txtInflowID.Text, lblItemID.Text, lblOrderedQty.Text, txtDeliverdQty.Text, lblBuyingPrice.Text, TotalInflowPrice, VatCmp1Val, VatCmp2Val, VatCmp3Val, VatCmp4Val, VatCmp5Val, txtManualInfID.Text, InflowDate, UserID, Created_On);
                        int status =config.ExecuteNonQueryWithQueryAsync(insertquery).Result;

                        #region for updation

                        if (status != 0)
                        {

                            TotalQty = float.Parse(txtDeliverdQty.Text) + float.Parse(lblAlreadyDeliveredQty.Text);

                            if (float.Parse(lblOrderedQty.Text) == (TotalQty))
                            {
                                insertquery = "update invpomaster set deliverystatus='Delivered' where pono='" + ddlPONo.SelectedValue + "' and itemid='" + lblItemID.Text + "'";
                             int del=  config.ExecuteNonQueryWithQueryAsync(insertquery).Result;

                            }
                            else if (TotalQty < float.Parse(lblOrderedQty.Text) && float.Parse(txtDeliverdQty.Text) > 0)
                            {
                                insertquery = "update invpomaster set deliverystatus='PartiallyDelivered' where pono='" + ddlPONo.SelectedValue + "' and itemid='" + lblItemID.Text + "'";
                                int inner = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                            }

                            string UpdateQuery = " update invStockItemlist set ActualQuantity = ActualQuantity + " + float.Parse(txtDeliverdQty.Text) + " where itemid = '" + lblItemID.Text + "'";
                          int updf=config.ExecuteNonQueryWithQueryAsync(UpdateQuery).Result;

                            lblresult.Text = "Record Inserted Successfully";
                        }
                        else
                        {
                            lblresult.Text = "Record Not Inserted Successfully";
                        }

                        #endregion for updation

                    }
                }
                txtInflowID.Text = GRVIDAuto().ToString();
                ddlPONo.SelectedIndex = 0;
                txtManualInfID.Text = "";
                txtManualInfID.Enabled = false;
                gvresources.DataSource = null;
                gvresources.DataBind();
                txtMonth.Text = "";
                txtDate.Text = "";
                txtMonth.Visible = false;
                lblmonth.Text = "";
            }
            catch (Exception ex)
            {
                lblresult.Text = ex.Message;

            }

        }
    }
}