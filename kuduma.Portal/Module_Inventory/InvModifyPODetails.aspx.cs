using System;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class InvModifyPODetails : System.Web.UI.Page
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
                GetPONos();
                LoadClientList();                
                txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        protected void GetPONos()
        {
            ddlPONo.Items.Clear();
            string sqlqry = "Select distinct pono from InvpoMaster where vendorid='" + ddlVendorID.SelectedValue + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlPONo.DataValueField = "pono";
                ddlPONo.DataTextField = "pono";
                ddlPONo.DataSource = dt;
                ddlPONo.DataBind();

            }

            ddlPONo.Items.Insert(0, "-Select-");

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
                BranchID= Session["BranchId"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }

        public void GetData()
        {
            string date = "";
            string qry = "select DeliveryAt,clientid,isnull(Remarks,'') as Remarks,convert(varchar(10),DeliveryDate,103) as DeliveryDate,isnull(month,'') as month,isnull(BranchId,0) as BranchId from InvPOMaster where pono='" + ddlPONo.SelectedValue + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlDeliveryAt.SelectedValue = dt.Rows[0]["DeliveryAt"].ToString();
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                txtdate.Text = dt.Rows[0]["DeliveryDate"].ToString();
                if (ddlDeliveryAt.SelectedValue == "Client")
                {

                    if (dt.Rows[0]["clientid"].ToString() != null && dt.Rows[0]["clientid"].ToString().Length > 0)
                    {
                        lblclientid.Visible = true;
                        ddlclientid.Visible = true;
                        ddlclientid.SelectedValue = dt.Rows[0]["clientid"].ToString();
                        date = dt.Rows[0]["month"].ToString();
                        txtMonth.Visible = true;
                        lblmonth.Visible = true;
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
                    else
                    {
                        ddlclientid.SelectedIndex = 0;
                        txtMonth.Text = "";
                        //txtMonth.Visible = false;

                    }

                }
                else
                {
                    lblclientid.Visible = false;
                    ddlclientid.Visible = false;
                    txtMonth.Text = "";
                    txtMonth.Visible = false;
                    lblmonth.Visible = false;
                }

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

        float subtotal = 0;
        public void GetGridDataintoTable()
        {

            try
            {


                string sqry = "select ipm.ItemId,itemname,qty,HSNNumber,GSTPer,GSTAmt,isil.ActualQuantity,ipm.BuyingPrice,TotalBuyingPrice,ipm.VATCmp1,ipm.VATCmp2,ipm.VATCmp3,ipm.VATCmp4,ipm.VATCmp5,Total,ItemRemarks,'C' as type " +
                              " from InvPOMaster ipm inner join InvStockItemList isil on isil.ItemId=ipm.ItemId where PONo='" + ddlPONo.SelectedValue + "' " +
                            " union " +
                           " select ivim.ItemId,itemname,0 as qty,HSNNumber,GSTPer,0 as GSTAmt,isil.ActualQuantity,ivim.BuyingPrice,0 as TotalBuyingPrice,0 as VATCmp1,0 as VATCmp2,0 as VATCmp3,0 as VATCmp4,0 as VATCmp5,0 as Total,'' as ItemRemarks,'N' as type " +
                     " from invvendoritemmaster ivim inner join InvStockItemList isil on isil.ItemId=ivim.ItemId and isil.ItemId not in (select ItemId  from InvPOMaster where PONo='" + ddlPONo.SelectedValue + "') order by type,itemname";


                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqry).Result;


                if (dt.Rows.Count > 0)
                {
                    GVInvPODetails.DataSource = dt;
                    GVInvPODetails.DataBind();


                    for (int i = 0; i < GVInvPODetails.Rows.Count; i++)
                    {
                        CheckBox chkitem = GVInvPODetails.Rows[i].FindControl("chkindividual") as CheckBox;
                        Label lbltype = GVInvPODetails.Rows[i].FindControl("lbltype") as Label;
                        if (lbltype.Text == "C")
                        {
                            chkitem.Checked = true;
                        }
                        else
                        {
                            chkitem.Checked = false;
                        }



                        Label lblSubTotal = GVInvPODetails.Rows[i].FindControl("lblSubTotal") as Label;
                        Label lblTotal = GVInvPODetails.Rows[i].FindControl("lblTotal") as Label;

                        if (lblTotal.Text != "0")
                        {
                            subtotal += float.Parse(lblTotal.Text);
                            lblSubTotal.Text = subtotal.ToString("0.00");
                        }
                        else
                        {
                            lblSubTotal.Text = "0";
                        }

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

        public void DeleteorinsertintoTable()
        {
            int result = 0;


            string deleteqry = "delete invvendoritemmaster where vendorid='" + ddlVendorID.SelectedValue + "'";
            result = config.ExecuteNonQueryWithQueryAsync(deleteqry).Result;

            for (int i = 0; i < GVInvPODetails.Rows.Count; i++)
            {
                DateTime Created_On = DateTime.Now;

                Label lblitemid = GVInvPODetails.Rows[i].FindControl("lblitemid") as Label;
                TextBox lblBuyingPrice = GVInvPODetails.Rows[i].FindControl("lblBuyingPrice") as TextBox;
                CheckBox chkitemid = GVInvPODetails.Rows[i].FindControl("chkindividual") as CheckBox;

                if (chkitemid.Checked == true)
                {

                    string insertqry = "insert into invvendoritemmaster(vendorid,itemid,buyingprice,Modified_By,Modified_On)values('" + ddlVendorID.SelectedValue + "','" + lblitemid.Text + "','" + lblBuyingPrice.Text + "','" + UserID + "','" + Created_On + "')";
                    result = config.ExecuteNonQueryWithQueryAsync(insertqry).Result;
                    if (result > 0)
                    {
                        lblmsg.Text = "Data inserted Successfully...";
                    }
                }
            }
        }

        float VATCmp1per = 0;
        float VATCmp2per = 0;
        float VATCmp3per = 0;
        float VATCmp4per = 0;
        float VATCmp5per = 0;


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
                if (lblTotal.Text == "")
                {
                    lblTotal.Text = "0";
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
            string sqlqry = "Select VendorId  from InvVendorMaster where Vendorid =  '" + ddlVendorName.SelectedValue + "'  ";

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
            }
            else
            {
                ddlclientid.Visible = false;
                lblclientid.Visible = false;

            }
        }

        protected void ddlVendorID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            cleardata();
            lblresult.Visible = false;
            GetVendorName();
            GetPONos();

        }
        protected void ddlVendorName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            cleardata();
            lblresult.Visible = false;
            GetVendorID();
            GetPONos();

        }
        protected void ddlPONo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
            GetGridDataintoTable();

        }

        public void cleardata()
        {
            lblclientid.Visible = false;
            ddlclientid.Visible = false;
            GVInvPODetails.DataSource = null;
            GVInvPODetails.DataBind();
            ddlDeliveryAt.SelectedIndex = 0;
            txtdate.Text = "";
            txtRemarks.Text = "";
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int result = 0;
            lblresult.Text = "";
            lblresult.Visible = true;
            var ApproveStatus = "0";
            try
            {
                string pono = ddlPONo.SelectedValue;
                string VendorId = ddlVendorID.SelectedValue;
              
                if (ddlVendorID.SelectedIndex == 0)
                {
                    lblresult.Text = "Please select Vendor ID/Name";
                    return;
                }
                DateTime Modified_On = DateTime.Now;
                string deliverat = ddlDeliveryAt.SelectedValue;

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

                string remarks = "";

                remarks = txtRemarks.Text;

                string date = string.Empty;
                string month = "";
                string Year = "";

                if (txtMonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    month = DateTime.Parse(date).Month.ToString("00");
                    Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);

                }



                string deleteqry = "delete InvPOMaster where vendorid='" + ddlVendorID.SelectedValue + "' and pono='" + ddlPONo.SelectedValue + "'";
                result = config.ExecuteNonQueryWithQueryAsync(deleteqry).Result;
                for (int i = 0; i < GVInvPODetails.Rows.Count; i++)
                {
                    Label lblSubTotal = GVInvPODetails.Rows[i].FindControl("lblSubTotal") as Label;
                    Label lblTotal = GVInvPODetails.Rows[i].FindControl("lblTotal") as Label;
                    Label lblitemid = GVInvPODetails.Rows[i].FindControl("lblitemid") as Label;
                    TextBox txtQuantity = GVInvPODetails.Rows[i].FindControl("txtQuantity") as TextBox;
                    Label lblBuyingPrice = GVInvPODetails.Rows[i].FindControl("lblBuyingPrice") as Label;
                    Label lblTotalBuyingPrice = GVInvPODetails.Rows[i].FindControl("lblTotalBuyingPrice") as Label;
                    Label lblGSTPer = GVInvPODetails.Rows[i].FindControl("lblGSTPer") as Label;
                    Label lblGSTAmt = GVInvPODetails.Rows[i].FindControl("lblGSTAmt") as Label;
                    Label lblVATCmp1 = GVInvPODetails.Rows[i].FindControl("lblVATCmp1") as Label;
                    Label lblVATCmp2 = GVInvPODetails.Rows[i].FindControl("lblVATCmp2") as Label;
                    Label lblVATCmp3 = GVInvPODetails.Rows[i].FindControl("lblVATCmp3") as Label;
                    Label lblVATCmp4 = GVInvPODetails.Rows[i].FindControl("lblVATCmp4") as Label;
                    Label lblVATCmp5 = GVInvPODetails.Rows[i].FindControl("lblVATCmp5") as Label;
                    TextBox txtItemRemarks = GVInvPODetails.Rows[i].FindControl("txtItemRemarks") as TextBox;

                    if (float.Parse(txtQuantity.Text) > 0)
                    {

                        string insertquery = string.Format("insert into InvPOMaster(pono,VendorID,DeliveryAt,Clientid,DeliveryDate,ItemId,Qty,ItemRemarks,BuyingPrice,TotalBuyingPrice,VATCmp1,VATCmp2,VATCmp3,VATCmp4,VATCmp5,Total,Month,DeliveryStatus,Remarks,Modified_By,Modified_On,GSTAmt" +
                        " ) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}')",
                          pono, VendorId, deliverat, clientid, DeliveryDt, lblitemid.Text, txtQuantity.Text, txtItemRemarks.Text, lblBuyingPrice.Text, lblTotalBuyingPrice.Text, lblVATCmp1.Text, lblVATCmp2.Text, lblVATCmp3.Text, lblVATCmp4.Text, lblVATCmp5.Text, lblTotal.Text, Year + month, deliverystatus, remarks, UserID, Modified_On, lblGSTAmt.Text);
                        int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                        if (status != 0)
                        {
                            lblresult.Text = "Record Modified Successfully";
                        }
                        else
                        {
                            lblresult.Text = "Record Not Modified Successfully";
                        }
                    }

                }
                ddlVendorID.SelectedIndex = 0;
                ddlVendorName.SelectedIndex = 0;
                ddlPONo.SelectedIndex = 0;
                ddlclientid.Visible = false;
                ddlclientid.SelectedIndex = 0;
                txtRemarks.Text = string.Empty;
                ddlDeliveryAt.SelectedIndex = 0;
                GVInvPODetails.DataSource = null;
                GVInvPODetails.DataBind();
                txtdate.Text = "";
                lblclientid.Visible = false;
                lblmonth.Visible = false;
                txtMonth.Visible = false;
                txtMonth.Text = "";
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