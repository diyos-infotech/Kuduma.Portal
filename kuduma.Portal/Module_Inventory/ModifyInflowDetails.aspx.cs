using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Data.OleDb;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class ModifyInflowDetails : System.Web.UI.Page
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
                BranchID = Session["BranchID"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void GetPONos()
        {
            string sqlqry = "Select distinct pono from InvInflowMaster";
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

        public void GetInflowIDs()
        {
            string sqlqry = "Select distinct InflowId from InvInflowMaster where pono='" + ddlPONo.SelectedValue + "'  ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlInflowID.DataTextField = "InflowId";
                ddlInflowID.DataValueField = "InflowId";
                ddlInflowID.DataSource = dt;
                ddlInflowID.DataBind();

            }

            ddlInflowID.Items.Insert(0, "-Select-");
        }


        public void GetItemList()
        {
            string qry = "select iim.itemid,itemname,UnitMeasure,OrderedQty,DeliveredQty,convert(varchar(10),Inflowdate,103) as Inflowdate,Inflowprice as BuyingPrice ,'C' as type from InvInflowMaster iim inner join invstockitemlist isil on isil.itemid=iim.itemid where InflowId='" + ddlInflowID.SelectedValue + "'" +
                         " union " +
                         " select ipm.ItemId,isil.itemname,UnitMeasure,ipm.qty as OrderedQty,0 as DeliveredQty,'' as Inflowdate,ipm.buyingprice,'N' as Type from invpomaster ipm inner join invstockitemlist isil on isil.itemid=ipm.itemid " +
                         " where pono='" + ddlPONo.SelectedValue + "' and (ipm.DeliveryStatus='NYD' or ipm.deliverystatus='PartiallyDelivered') and ipm.ItemId not in " +
                         " (select ItemId from InvInflowMaster where InflowId='" + ddlInflowID.SelectedValue + "') order by type,itemname";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                gvresources.DataSource = dt;
                gvresources.DataBind();

                txtDate.Text = dt.Rows[0]["Inflowdate"].ToString();

                for (int i = 0; i < gvresources.Rows.Count; i++)
                {
                    CheckBox chkitem = gvresources.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lbltype = gvresources.Rows[i].FindControl("lbltype") as Label;
                    Label lblItemID = gvresources.Rows[i].FindControl("lblItemID") as Label;

                    Label lblOrderedQty = gvresources.Rows[i].FindControl("lblOrderedQty") as Label;
                    Label lblAlreadyDeliveredQty = gvresources.Rows[i].FindControl("lblAlreadyDeliveredQty") as Label;
                    Label lblBalance = gvresources.Rows[i].FindControl("lblBalance") as Label;
                    TextBox txtDeliverdQty = gvresources.Rows[i].FindControl("txtDeliverdQty") as TextBox;



                    if (lbltype.Text == "C")
                    {
                        chkitem.Checked = true;
                    }
                    else
                    {
                        chkitem.Checked = false;
                    }


                    qry = "select isnull(sum(DeliveredQty),0) as DeliveredQty from invinflowmaster where pono='" + ddlPONo.SelectedValue + "' and itemid = '" + lblItemID.Text + "' and inflowid!='" + ddlInflowID.SelectedValue + "'";
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
            lblresult.Visible = false;
            gvresources.DataSource = null;
            gvresources.DataBind();
            ddlInflowID.Items.Clear();

            if (ddlPONo.SelectedIndex > 0)
            {
                GetInflowIDs();
            }

        }



        protected void ddlInflowID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInflowID.SelectedIndex > 0)
            {
                GetItemList();
                //ManualInfID();
            }
        }

        public void ManualInfID()
        {
            txtManualInfID.Text = "";
            string qry = "select deliveryat from invpo where pono='" + ddlPONo.SelectedValue + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            string DeliveryAt = "";
            if (dt.Rows.Count > 0)
            {
                DeliveryAt = dt.Rows[0]["deliveryat"].ToString();
                if (DeliveryAt == "Office")
                {
                    txtManualInfID.Enabled = false;


                }
                else if (DeliveryAt == "Client")
                {
                    txtManualInfID.Enabled = true;

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
            lblresult.Text = "";
            lblresult.Visible = true;
            try
            {
                string GRVId = ddlInflowID.SelectedValue;
                string pono = ddlPONo.SelectedValue;

                if (ddlPONo.SelectedIndex == 0)
                {
                    lblresult.Text = "Please select PO No";
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


                DateTime Created_On = DateTime.Now;

                float orderedqty = 0;
                float DeliveredQty = 0;
                float OrgQty = 0;


                for (int i = 0; i < gvresources.Rows.Count; i++)
                {
                    double TotalInflowPrice = 0;
                    double VatCmp1Val = 0;
                    double VatCmp2Val = 0;
                    double VatCmp3Val = 0;
                    double VatCmp4Val = 0;
                    double VatCmp5Val = 0;

                    string InvVat5per = "0";
                    string InvVat14per = "0";

                    Label lblOrderedQty = gvresources.Rows[i].FindControl("lblOrderedQty") as Label;
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



                    //if (float.Parse(TxtDelieverdQty.Text) > 0)
                    {

                        string qry = "select DeliveredQty,OrderedQty from InvInflowMaster where pono='" + ddlPONo.SelectedValue + "' and  inflowid='" + ddlInflowID.SelectedValue + "' and itemid='" + lblItemID.Text + "' ";
                        dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

                        if (dt.Rows.Count > 0)
                        {

                            orderedqty = float.Parse(dt.Rows[0]["OrderedQty"].ToString());
                            DeliveredQty = float.Parse(dt.Rows[0]["DeliveredQty"].ToString());


                            string updateqry = "update InvInflowMaster set Inflowdate='" + InflowDate + "', DeliveredQty='" + txtDeliverdQty.Text + "',TotalInflowPrice='" + TotalInflowPrice + "',VATCmp1='" + VatCmp1Val + "',VATCmp2='" + VatCmp2Val + "',VATCmp3='" + VatCmp3Val + "',VATCmp4='" + VatCmp4Val + "',VATCmp5='" + VatCmp5Val + "',Modified_By='" + UserID + "',Modified_On='" + Created_On + "' where pono='" + ddlPONo.SelectedValue + "' and inflowid='" + ddlInflowID.SelectedValue + "' and itemid='" + lblItemID.Text + "'";
                            int status = config.ExecuteNonQueryWithQueryAsync(updateqry).Result;
                            if (status != 0)
                            {

                                if (orderedqty == float.Parse(txtDeliverdQty.Text))
                                {
                                    updateqry = "update invpomaster set deliverystatus='Delivered' where pono='" + ddlPONo.SelectedValue + "' and itemid='" + lblItemID.Text + "'";
                                   int upd=config.ExecuteNonQueryWithQueryAsync(updateqry).Result;

                                }
                                else if (float.Parse(txtDeliverdQty.Text) < orderedqty && float.Parse(txtDeliverdQty.Text) > 0)
                                {
                                    updateqry = "update invpomaster set deliverystatus='PartiallyDelivered' where pono='" + ddlPONo.SelectedValue + "' and itemid='" + lblItemID.Text + "'";
                                   int upd2= config.ExecuteNonQueryWithQueryAsync(updateqry).Result;
                                }
                                else if (float.Parse(txtDeliverdQty.Text) == 0)
                                {
                                    updateqry = "update invpomaster set deliverystatus='NYD' where pono='" + ddlPONo.SelectedValue + "' and itemid='" + lblItemID.Text + "'";
                                   int upd3=config.ExecuteNonQueryWithQueryAsync(updateqry).Result;

                                }
                                //10>5

                                if (DeliveredQty > float.Parse(txtDeliverdQty.Text))
                                {
                                    OrgQty = DeliveredQty - float.Parse(txtDeliverdQty.Text);

                                    string UpdateQuery = " update invStockItemlist set ActualQuantity = ActualQuantity - " + OrgQty + " where itemid = '" + lblItemID.Text + "'";
                                  int upd4=config.ExecuteNonQueryWithQueryAsync(UpdateQuery).Result;
                                }

                                    //5<10

                                else if (DeliveredQty < float.Parse(txtDeliverdQty.Text))
                                {
                                    OrgQty = float.Parse(txtDeliverdQty.Text) - DeliveredQty;


                                    string UpdateQuery = " update invStockItemlist set ActualQuantity = ActualQuantity + " + OrgQty + " where itemid = '" + lblItemID.Text + "'";
                                   int upd6=config.ExecuteNonQueryWithQueryAsync(UpdateQuery).Result;
                                }

                                lblresult.Text = "Record Modified Successfully";
                            }
                            else
                            {
                                lblresult.Text = "Record Not modified Successfully";
                            }

                        }
                        else
                        {


                            string insertquery = string.Format("insert into InvInflowMaster(pono,InflowId,ItemId,OrderedQty,DeliveredQty,InflowPrice,TotalInflowPrice,VATCmp1,VATCmp2,VATCmp3,VATCmp4,VATCmp5,ManualInflowId,InflowDate,Created_By,Created_On" +
                       " ) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',{18})",
                         pono, GRVId, lblItemID.Text, lblOrderedQty.Text, txtDeliverdQty.Text, lblBuyingPrice.Text, TotalInflowPrice, VatCmp1Val, VatCmp2Val, VatCmp3Val, VatCmp4Val, VatCmp5Val, txtManualInfID.Text, InflowDate, UserID, Created_On);
                            int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;

                            if (status != 0)
                            {

                                if (float.Parse(lblOrderedQty.Text) == float.Parse(txtDeliverdQty.Text))
                                {
                                    insertquery = "update invpomaster set deliverystatus='Delivered' where pono='" + ddlPONo.SelectedValue + "' and itemid='" + lblItemID.Text + "'";
                                  int ins1= config.ExecuteNonQueryWithQueryAsync(insertquery).Result;

                                }
                                else if (float.Parse(txtDeliverdQty.Text) < float.Parse(lblOrderedQty.Text) && float.Parse(txtDeliverdQty.Text) > 0)
                                {
                                    insertquery = "update invpomaster set deliverystatus='PartiallyDelivered' where pono='" + ddlPONo.SelectedValue + "' and itemid='" + lblItemID.Text + "'";
                                 int ims2=config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                                }

                                string UpdateQuery = " update invStockItemlist set ActualQuantity = ActualQuantity + " + float.Parse(txtDeliverdQty.Text) + " where itemid = '" + lblItemID.Text + "'";
                              int ins3=config.ExecuteNonQueryWithQueryAsync(UpdateQuery).Result;

                                lblresult.Text = "Record Inserted Successfully";
                            }
                            else
                            {
                                lblresult.Text = "Record Not Inserted Successfully";
                            }

                        }

                    }
                }

                ddlPONo.SelectedIndex = 0;
                txtManualInfID.Text = "";
                txtDate.Text = "";
                txtManualInfID.Enabled = false;
                gvresources.DataSource = null;
                gvresources.DataBind();
                ddlInflowID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblresult.Text = ex.Message;

            }



        }

    }
}