using System;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class InvPODetails : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string POIDPrefix = "";
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

                loadVendorIDs();
                loadVendorNames();
                txtPONo.Text = ItemIdAuto().ToString();
                LoadClientList();
                txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }

        }


        protected void LoadClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dt;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "--Select--");
        }



        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                UserID = Session["UserId"].ToString();
                POIDPrefix = Session["POPrefix"].ToString();
                BranchID = Session["BranchID"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }


        public void loadVendorIDs()
        {
            string sqlqry = "Select distinct(VendorId) as VendorId  from InvVendorMaster";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlVendorID.DataValueField = "VendorId";
                ddlVendorID.DataTextField = "VendorId";
                ddlVendorID.DataSource = dt;
                ddlVendorID.DataBind();
            }

            ddlVendorID.Items.Insert(0, "-Select-");
        }

        public void loadVendorNames()
        {
            string sqlqry = "Select distinct VendorId,Vendorname  from InvVendorMaster";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlVendorName.DataValueField = "VendorId";
                ddlVendorName.DataTextField = "Vendorname";
                ddlVendorName.DataSource = dt;
                ddlVendorName.DataBind();
            }

            ddlVendorName.Items.Insert(0, "-Select-");
        }

        public void GetGridData()
        {
            string itemid = ""; string itemname = ""; string HSNNo = ""; string buyingprice = ""; string GSTPer = ""; string GSTAmt = ""; string vendorid = ""; string Quantity = "";
            string StockInHand = ""; string VAT = ""; string Total = ""; string SubTotal = ""; string VATfourteen = ""; string VATfive = ""; string ApproveStatus = "0";
            int status = 0;
            try
            {


                for (int i = 0; i < GVInvPODetails.Rows.Count; i++)
                {
                    Label lblitemid = GVInvPODetails.Rows[i].FindControl("lblitemid") as Label;
                    Label lblitemname = GVInvPODetails.Rows[i].FindControl("lblitemname") as Label;
                    Label lblHSNNo = GVInvPODetails.Rows[i].FindControl("lblHSNNo") as Label;
                    TextBox txtQuantity = GVInvPODetails.Rows[i].FindControl("txtQuantity") as TextBox;
                    Label lblStockInHand = GVInvPODetails.Rows[i].FindControl("lblStockInHand") as Label;
                    TextBox lblBuyingPrice = GVInvPODetails.Rows[i].FindControl("lblBuyingPrice") as TextBox;
                    TextBox lblGStPer = GVInvPODetails.Rows[i].FindControl("lblGStPer") as TextBox;
                    TextBox lblGSTAmt = GVInvPODetails.Rows[i].FindControl("lblGSTAmt") as TextBox;
                    TextBox lblVATfive = GVInvPODetails.Rows[i].FindControl("lblVATfive") as TextBox;
                    TextBox lblVATfourteen = GVInvPODetails.Rows[i].FindControl("lblVATfourteen") as TextBox;
                    TextBox lblTotal = GVInvPODetails.Rows[i].FindControl("lblTotal") as TextBox;
                    //  Label lblSubTotal = GVInvPODetails.Rows[i].FindControl("lblSubTotal") as Label;

                    // CheckBox chkitemid = GVInvPODetails.Rows[i].FindControl("chkindividual") as CheckBox;

                    // if (chkitemid.Checked == true)
                    {


                        if (lblitemid.Text.Trim().Length == 0)
                        {
                            itemid = "0";
                        }
                        else
                        {
                            itemid = lblitemid.Text;
                        }
                        if (lblitemname.Text.Trim().Length == 0)
                        {
                            itemname = "0";
                        }
                        else
                        {
                            itemname = lblitemname.Text;
                        }
                        if (lblHSNNo.Text.Trim().Length == 0)
                        {
                            HSNNo = "0";
                        }
                        else
                        {
                            HSNNo = lblHSNNo.Text;
                        }
                        if (txtQuantity.Text.Trim().Length == 0)
                        {
                            Quantity = "0";
                        }
                        else
                        {
                            Quantity = txtQuantity.Text;
                        }

                        if (lblStockInHand.Text.Trim().Length == 0)
                        {
                            StockInHand = "0";
                        }
                        else
                        {
                            StockInHand = lblStockInHand.Text;
                        }

                        if (lblBuyingPrice.Text.Trim().Length == 0)
                        {
                            buyingprice = "0";
                        }
                        else
                        {
                            buyingprice = lblBuyingPrice.Text;
                        }

                        if (lblGStPer.Text.Trim().Length == 0)
                        {
                            GSTPer = "0";
                        }
                        else
                        {
                            GSTPer = lblGStPer.Text;
                        }

                        if (lblGSTAmt.Text.Trim().Length == 0)
                        {
                            GSTAmt = "0";
                        }
                        else
                        {
                            GSTAmt = lblGSTAmt.Text;
                        }

                        if (lblVATfive.Text.Trim().Length == 0)
                        {
                            VATfive = "0";
                        }
                        else
                        {
                            VATfive = lblVATfive.Text;
                        }
                        if (lblVATfourteen.Text.Trim().Length == 0)
                        {
                            VATfourteen = "0";
                        }
                        else
                        {
                            VATfourteen = lblVATfourteen.Text;
                        }
                        if (lblTotal.Text.Trim().Length == 0)
                        {
                            Total = "0";
                        }
                        else
                        {
                            Total = lblTotal.Text;
                        }

                        string sqlqry = "insert into InvPOMaster(pono,vendorid,itemid,Qty,StockInHand,BuyingPrice,VAT5per,VAT14per,Total,GSTAmt,ApproveStatus,Created_By,Created_On) values('" + txtPONo.Text + "','" + ddlVendorID.SelectedValue + "','" + itemid + "','" + Quantity + "','" + StockInHand + "','" + buyingprice + "','" + VATfive + "','" + VATfourteen + "','" + Total + "','" + GSTAmt + "','" + ApproveStatus + "','" + UserID + "',GetDate())";
                        status = config.ExecuteNonQueryWithQueryAsync(sqlqry).Result;
                        if (status > 0)
                        {
                            lblresult.Text = "Record inserted Successfully...";
                        }
                        else
                        {
                            lblresult.Text = "Record Not Inserted Successfully";
                        }

                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        float subtotal = 0;
        float VATCmp1per = 0;
        float VATCmp2per = 0;
        float VATCmp3per = 0;
        float VATCmp4per = 0;
        float VATCmp5per = 0;

        public void GetGridDataintoTable()
        {

            try
            {

                DataTable dtTaxCmpts = GlobalData.Instance.LoadTaxComponents();

                if (dtTaxCmpts.Rows.Count > 0)
                {
                    VATCmp1per = float.Parse(dtTaxCmpts.Rows[10]["TaxCmpPer"].ToString());
                    VATCmp2per = float.Parse(dtTaxCmpts.Rows[11]["TaxCmpPer"].ToString());
                    VATCmp3per = float.Parse(dtTaxCmpts.Rows[12]["TaxCmpPer"].ToString());
                    VATCmp4per = float.Parse(dtTaxCmpts.Rows[13]["TaxCmpPer"].ToString());
                    VATCmp5per = float.Parse(dtTaxCmpts.Rows[14]["TaxCmpPer"].ToString());
                }

                string sqry = "select isil.itemname,HSNNumber,ivim.ItemId,isil.UnitMeasure,GSTPer,ivim.BuyingPrice,(isnull(openingstock,0)+ActualQuantity) as ActualQuantity," +
                               "case cast(isil.VATCmp1 as bit) when 0 then 0 else   ((ivim.BuyingPrice*('" + VATCmp1per + "'))/100) end as VATCmp1," +
                               "case cast(isil.VATCmp2 as bit) when 0 then 0 else ((ivim.BuyingPrice*('" + VATCmp2per + "'))/100) end as VATCmp2," +
                               "case cast(isil.VATCmp3 as bit) when 0 then 0 else ((ivim.BuyingPrice*('" + VATCmp3per + "'))/100) end as VATCmp3," +
                               "case cast(isil.VATCmp4 as bit) when 0 then 0 else ((ivim.BuyingPrice*('" + VATCmp4per + "'))/100) end as VATCmp4," +
                               "case cast(isil.VATCmp5 as bit) when 0 then 0 else ((ivim.BuyingPrice*('" + VATCmp5per + "'))/100) end as VATCmp5," +
                               "(ivim.BuyingPrice+case cast(isil.VATCmp1 as bit) when 0 then 0 else   ((ivim.BuyingPrice*('" + VATCmp1per + "'))/100) end+case cast(isil.VATCmp2 as bit) when 0 then 0 else ((ivim.BuyingPrice*('" + VATCmp2per + "'))/100) end +case cast(isil.VATCmp3 as bit) when 0 then 0 else ((ivim.BuyingPrice*('" + VATCmp3per + "'))/100) end+case cast(isil.VATCmp4 as bit) when 0 then 0 else ((ivim.BuyingPrice*('" + VATCmp4per + "'))/100) end+case cast(isil.VATCmp5 as bit) when 0 then 0 else ((ivim.BuyingPrice*('" + VATCmp5per + "'))/100) end) as Total," +
                               "vendorid from invvendoritemmaster ivim inner join InvStockItemList isil on isil.itemid=ivim.itemid where vendorid='" + ddlVendorName.SelectedValue + "'";

                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqry).Result;

                if (dt.Rows.Count > 0)
                {
                    GVInvPODetails.DataSource = dt;
                    GVInvPODetails.DataBind();


                    for (int i = 0; i < GVInvPODetails.Rows.Count; i++)
                    {
                        Label lblSubTotal = GVInvPODetails.Rows[i].FindControl("lblSubTotal") as Label;
                        Label lblTotal = GVInvPODetails.Rows[i].FindControl("lblTotal") as Label;

                        subtotal += float.Parse(lblTotal.Text);
                        lblSubTotal.Text = subtotal.ToString();

                    }


                }
                else
                {
                    GVInvPODetails.DataSource = null;
                    GVInvPODetails.DataBind();
                }


            }
            catch (Exception ex)
            {

            }

        }

        protected void txtQuantity_OnTextChanged(object sender, EventArgs e)
        {

            TextBox txtQuantity = sender as TextBox;
            GridViewRow row = null;
            if (txtQuantity == null)
                return;

            row = (GridViewRow)txtQuantity.NamingContainer;
            if (row == null)
                return;

            Label lblitemid = row.FindControl("lblitemid") as Label;

            DataTable dtTaxCmpts = GlobalData.Instance.LoadTaxComponents();
            if (dtTaxCmpts.Rows.Count > 0)
            {
                VATCmp1per = float.Parse(dtTaxCmpts.Rows[10]["TaxCmpPer"].ToString());
                VATCmp2per = float.Parse(dtTaxCmpts.Rows[11]["TaxCmpPer"].ToString());
                VATCmp3per = float.Parse(dtTaxCmpts.Rows[12]["TaxCmpPer"].ToString());
                VATCmp4per = float.Parse(dtTaxCmpts.Rows[13]["TaxCmpPer"].ToString());
                VATCmp5per = float.Parse(dtTaxCmpts.Rows[14]["TaxCmpPer"].ToString());
            }


            DataTable dt = GlobalData.Instance.LoadVATCheckforItem(lblitemid.Text);

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



            Label lblBuyingPricev = row.FindControl("lblBuyingPrice") as Label;
            Label lblTotalBuyingPrice = row.FindControl("lblTotalBuyingPrice") as Label;
            Label lblGSTAmt = row.FindControl("lblGSTAmt") as Label;
            Label lblGStPer = row.FindControl("lblGStPer") as Label;
            Label lblVATCmp1 = row.FindControl("lblVATCmp1") as Label;
            Label lblVATCmp2 = row.FindControl("lblVATCmp2") as Label;
            Label lblVATCmp3 = row.FindControl("lblVATCmp3") as Label;
            Label lblVATCmp4 = row.FindControl("lblVATCmp4") as Label;
            Label lblVATCmp5 = row.FindControl("lblVATCmp5") as Label;
            Label lblTotal = row.FindControl("lblTotal") as Label;

            if (float.Parse(txtQuantity.Text) > 0)
            {

                if (lblGStPer.Text == "")
                {
                    lblGStPer.Text = "0";
                }


                if (lblTotalBuyingPrice.Text == "")
                {
                    lblTotalBuyingPrice.Text = "0";
                }
                else
                {
                    lblTotalBuyingPrice.Text = (float.Parse(txtQuantity.Text) * float.Parse(lblBuyingPricev.Text)).ToString("0.00");
                }


                if (lblVATCmp1.Text == "")
                {
                    lblVATCmp1.Text = "0";
                }
                else
                {
                    if (VATCmp1 == "True")
                    {
                        lblVATCmp1.Text = ((float.Parse(txtQuantity.Text) * float.Parse(lblBuyingPricev.Text)) * (VATCmp1per) / 100).ToString("0.00");
                    }
                    else
                    {
                        lblVATCmp1.Text = "0";
                    }


                }

                if (lblVATCmp2.Text == "")
                {
                    lblVATCmp2.Text = "0";
                }
                else
                {

                    if (VATCmp2 == "True")
                    {
                        lblVATCmp2.Text = ((float.Parse(txtQuantity.Text) * float.Parse(lblBuyingPricev.Text)) * (VATCmp2per) / 100).ToString("0.00");
                    }
                    else
                    {
                        lblVATCmp2.Text = "0";
                    }
                }

                if (lblVATCmp3.Text == "")
                {
                    lblVATCmp3.Text = "0";
                }
                else
                {

                    if (VATCmp3 == "True")
                    {
                        lblVATCmp3.Text = ((float.Parse(txtQuantity.Text) * float.Parse(lblBuyingPricev.Text)) * (VATCmp3per) / 100).ToString("0.00");
                    }
                    else
                    {
                        lblVATCmp3.Text = "0";
                    }
                }

                if (lblVATCmp4.Text == "")
                {
                    lblVATCmp4.Text = "0";
                }
                else
                {

                    if (VATCmp4 == "True")
                    {
                        lblVATCmp4.Text = ((float.Parse(txtQuantity.Text) * float.Parse(lblBuyingPricev.Text)) * (VATCmp4per) / 100).ToString("0.00");
                    }
                    else
                    {
                        lblVATCmp4.Text = "0";
                    }
                }


                if (lblVATCmp5.Text == "")
                {
                    lblVATCmp5.Text = "0";
                }
                else
                {

                    if (VATCmp5 == "True")
                    {
                        lblVATCmp5.Text = ((float.Parse(txtQuantity.Text) * float.Parse(lblBuyingPricev.Text)) * (VATCmp5per) / 100).ToString("0.00");
                    }
                    else
                    {
                        lblVATCmp5.Text = "0";
                    }
                }

                if (lblTotal.Text == "")
                {
                    lblTotal.Text = "0";
                }

                if (lblGSTAmt.Text == "")
                {
                    lblGSTAmt.Text = "0";
                }
                else
                {
                    lblGSTAmt.Text = ((float.Parse(lblTotalBuyingPrice.Text) * float.Parse(lblGStPer.Text)) / 100).ToString("0.00");
                }


                lblTotal.Text = (float.Parse(lblTotalBuyingPrice.Text) + float.Parse(lblVATCmp1.Text) + float.Parse(lblVATCmp2.Text) + float.Parse(lblVATCmp3.Text) + float.Parse(lblVATCmp4.Text) + float.Parse(lblVATCmp5.Text) + float.Parse(lblGSTAmt.Text)).ToString("0.00");
            }
            else
            {
                lblTotal.Text = "0";
                lblVATCmp1.Text = "0";
                lblVATCmp2.Text = "0";
                lblVATCmp3.Text = "0";
                lblVATCmp4.Text = "0";
                lblVATCmp5.Text = "0";
                lblTotalBuyingPrice.Text = "0";
                lblGStPer.Text = "0";
                lblGSTAmt.Text = "0";

            }


            for (int i = 0; i < GVInvPODetails.Rows.Count; i++)
            {
                TextBox txtqty = GVInvPODetails.Rows[i].FindControl("txtQuantity") as TextBox;
                Label lblSubTotal = GVInvPODetails.Rows[i].FindControl("lblSubTotal") as Label;
                Label lblTotal2 = GVInvPODetails.Rows[i].FindControl("lblTotal") as Label;
                if (float.Parse(txtqty.Text) > 0)
                {
                    subtotal += float.Parse(lblTotal2.Text);
                    lblSubTotal.Text = subtotal.ToString();

                }
                else
                {
                    lblSubTotal.Text = "0";
                }


            }

            txtQuantity.Focus();


        }



        public string ItemIdAuto()
        {


            int pono = 1;
            string selectqueryclientid = "select (max(right(pono,4))) as pono from InvPOMaster  where pono like '" + POIDPrefix + "%'";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;
            string invPrefix = string.Empty;

            if (dt.Rows.Count > 0)
            {

                if (String.IsNullOrEmpty(dt.Rows[0]["pono"].ToString()) == false)
                {
                    pono = Convert.ToInt32(dt.Rows[0]["pono"].ToString()) + 1;
                }
                else
                {
                    pono = int.Parse("1");
                }
            }



            return POIDPrefix + (pono).ToString("0000");


        }

        protected void GetVendorName()
        {
            string sqlqry = "Select Vendorid from InvVendorMaster where VendorID = '" + ddlVendorID.SelectedValue + "'  ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    ddlVendorName.SelectedValue = dt.Rows[0]["Vendorid"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                ddlVendorName.SelectedIndex = 0;
            }


        }


        protected void GetVendorID()
        {

            #region  Old Code
            string sqlqry = "Select VendorId  from InvVendorMaster where vendorid =  '" + ddlVendorName.SelectedValue + "'  ";

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    ddlVendorID.SelectedValue = dt.Rows[0]["VendorId"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                ddlVendorID.SelectedIndex = 0;
            }

            #endregion


        }

        protected void ddlDeliveryAt_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDeliveryAt.SelectedIndex == 1)
            {
                ddlclientid.Visible = true;
                lblclientid.Visible = true;
                lblmonth.Visible = true;
                txtMonth.Visible = true;
            }
            else
            {
                ddlclientid.Visible = false;
                lblclientid.Visible = false;
                lblmonth.Visible = false;
                txtMonth.Visible = false;
            }
        }

        protected void ddlVendorID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblresult.Visible = false;
            GetVendorName();
            GetGridDataintoTable();

        }
        protected void ddlVendorName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblresult.Visible = false;
            GetVendorID();
            GetGridDataintoTable();

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            lblresult.Text = "";
            string ApproveStatus = "0";
            lblresult.Visible = true;
            try
            {
                string CheckPONo = "select PONO from  InvPOMaster where pono='" + txtPONo.Text + "' ";
                DataTable dtPONo = config.ExecuteAdaptorAsyncWithQueryParams(CheckPONo).Result;
                if (dtPONo.Rows.Count > 0)
                {
                    txtPONo.Text = ItemIdAuto().ToString();

                }

                string VendorId = ddlVendorID.SelectedValue;

                if (ddlVendorID.SelectedIndex == 0)
                {
                    lblresult.Text = "Please select Vendor ID/Name";
                    return;
                }



                DateTime Created_On = DateTime.Now;
                string deliverat = ddlDeliveryAt.SelectedValue;


                string date = string.Empty;
                string month = "";
                string Year = "";

                if (txtMonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    month = DateTime.Parse(date).Month.ToString("00");
                    Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);

                }


                var DeliveryDt = "01/01/1900";

                if (txtdate.Text.Trim().Length != 0)
                {
                    DeliveryDt = Timings.Instance.CheckDateFormat(txtdate.Text);

                }


                string deliverystatus = "NYD";

                string clientid = ddlclientid.SelectedValue;
                if (ddlclientid.SelectedIndex == 0)
                {
                    clientid = "0";
                }

                string Remarks = "";
                Remarks = txtRemarks.Text;
                string Branch = "";

                for (int i = 0; i < GVInvPODetails.Rows.Count; i++)
                {
                    Label lblSubTotal = GVInvPODetails.Rows[i].FindControl("lblSubTotal") as Label;
                    Label lblTotal = GVInvPODetails.Rows[i].FindControl("lblTotal") as Label;
                    Label lblitemid = GVInvPODetails.Rows[i].FindControl("lblitemid") as Label;
                    TextBox txtQuantity = GVInvPODetails.Rows[i].FindControl("txtQuantity") as TextBox;
                    Label lblBuyingPrice = GVInvPODetails.Rows[i].FindControl("lblBuyingPrice") as Label;
                    Label lblGSTAmt = GVInvPODetails.Rows[i].FindControl("lblGSTAmt") as Label;
                    Label lblTotalBuyingPrice = GVInvPODetails.Rows[i].FindControl("lblTotalBuyingPrice") as Label;
                    Label lblVATCmp1 = GVInvPODetails.Rows[i].FindControl("lblVATCmp1") as Label;
                    Label lblVATCmp2 = GVInvPODetails.Rows[i].FindControl("lblVATCmp2") as Label;
                    Label lblVATCmp3 = GVInvPODetails.Rows[i].FindControl("lblVATCmp3") as Label;
                    Label lblVATCmp4 = GVInvPODetails.Rows[i].FindControl("lblVATCmp4") as Label;
                    Label lblVATCmp5 = GVInvPODetails.Rows[i].FindControl("lblVATCmp5") as Label;
                    TextBox txtItemRemarks = GVInvPODetails.Rows[i].FindControl("txtItemRemarks") as TextBox;

                    if (float.Parse(txtQuantity.Text) > 0)
                    {

                        string insertquery = string.Format("insert into InvPOMaster(pono,VendorID,DeliveryAt,Clientid,DeliveryDate,ItemId,Qty,ItemRemarks,BuyingPrice,TotalBuyingPrice,VATCmp1,VATCmp2,VATCmp3,VATCmp4,VATCmp5,Total,Month,DeliveryStatus,Remarks,Created_By,Created_On,GSTAmt" +
                        " ) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}')",
                          txtPONo.Text, VendorId, deliverat, clientid, DeliveryDt, lblitemid.Text, txtQuantity.Text, txtItemRemarks.Text, lblBuyingPrice.Text, lblTotalBuyingPrice.Text, lblVATCmp1.Text, lblVATCmp2.Text, lblVATCmp3.Text, lblVATCmp4.Text, lblVATCmp5.Text, lblTotal.Text, Year + month, deliverystatus, Remarks, UserID, Created_On, lblGSTAmt.Text);
                        int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                        if (status != 0)
                        {
                            lblresult.Text = "Record Inserted Successfully";
                        }
                        else
                        {
                            lblresult.Text = "Record Not Inserted Successfully";
                        }
                    }

                }
                txtPONo.Text = ItemIdAuto().ToString();
                ddlVendorID.SelectedIndex = 0;
                ddlVendorName.SelectedIndex = 0;
                ddlclientid.Visible = false;
                ddlclientid.SelectedIndex = 0;
                lblmonth.Visible = false;
                txtMonth.Visible = false;
                txtRemarks.Text = string.Empty;
                ddlDeliveryAt.SelectedIndex = 0;
                GVInvPODetails.DataSource = null;
                GVInvPODetails.DataBind();
                lblclientid.Visible = false;
            }
            catch (Exception ex)
            {
                lblresult.Text = ex.Message;

            }



        }

        protected void GVInvPODetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = GlobalData.Instance.LoadTaxComponents();

            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                if (dt.Rows.Count > 0)
                {
                    //VATcmp1
                    if (dt.Rows[10]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[11].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[11].Text = dt.Rows[10]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[11].Visible = false;
                    }

                    //VATcmp2
                    if (dt.Rows[11]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[12].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[12].Text = dt.Rows[11]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[12].Visible = false;

                    }


                    //VATcmp3
                    if (dt.Rows[12]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[13].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[13].Text = dt.Rows[12]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[13].Visible = false;
                    }

                    //VATcmp4
                    if (dt.Rows[13]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[14].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[14].Text = dt.Rows[13]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[14].Visible = false;
                    }

                    //VATcmp5
                    if (dt.Rows[14]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[15].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[15].Text = dt.Rows[14]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[15].Visible = false;
                    }

                }
            }

        }
    }
}