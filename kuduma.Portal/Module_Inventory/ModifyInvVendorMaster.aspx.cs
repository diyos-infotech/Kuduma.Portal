using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ModifyInvVendorMaster : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string InvIDPrefix = "";
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
            }

        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            UserID = Session["UserId"].ToString();
            InvIDPrefix = Session["InvPrefix"].ToString();
            BranchID= Session["BranchID"].ToString();
        }

        public void loadVendorIDs()
        {
            string sqlqry = "Select VendorId  from InvVendorMaster";
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
            string sqlqry = "Select VendorId,Vendorname  from InvVendorMaster";
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

        protected void ddlVendorID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblresult.Text = "";
            GetVendorName();
            GetData();
            GetGridDataintoTable();

        }
        protected void ddlVendorName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblresult.Text = "";
            GetVendorID();
            GetData();
            GetGridDataintoTable();

        }

        public void GetData()
        {
            string qry = "select vendorid,vendorname,ContactPerson,ContactNo,Address,Remarks,EMailId,VendorGSTNO,isnull(BranchId,0) as Branch from InvVendorMaster where vendorid='" + ddlVendorID.SelectedValue + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                txtContactPerson.Text = dt.Rows[0]["ContactPerson"].ToString();
                txtContactNos.Text = dt.Rows[0]["ContactNo"].ToString();
                txtAddress.Text = dt.Rows[0]["Address"].ToString();
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                txtemailid.Text = dt.Rows[0]["EMailId"].ToString();
                txtvendorgst.Text = dt.Rows[0]["VendorGSTNO"].ToString();
            }
        }




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


                string sqry = "select isil.itemname,ivim.ItemId,ivim.BuyingPrice,HSNNumber,GSTPer,round( ((ivim.BuyingPrice*GSTPer)/100),2) as GSTAmt," +
                                "case cast(isil.VatCmp1 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp1per + "'))/100),2) end as VatCmp1," +
                                "case cast(isil.VatCmp2 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp2per + "'))/100),2) end as VatCmp2," +
                                "case cast(isil.VatCmp3 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp3per + "'))/100),2) end as VatCmp3," +
                                "case cast(isil.VatCmp4 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp4per + "'))/100),2) end as VatCmp4," +
                                "case cast(isil.VatCmp5 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp5per + "'))/100),2) end as VatCmp5," +
                                "(ivim.BuyingPrice+(case cast(isil.VatCmp1 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp1per + "'))/100),2) end)+(case cast(isil.VatCmp2 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp2per + "'))/100),2) end)+(case cast(isil.VatCmp3 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp3per + "'))/100),2) end)+(case cast(isil.VatCmp4 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp4per + "'))/100),2) end)+(case cast(isil.VatCmp5 as bit) when 0 then 0 else  round( ((ivim.BuyingPrice*('" + VATCmp5per + "'))/100),2) end))+ round( ((ivim.BuyingPrice*GSTPer)/100),2) as Total," +
                                "vendorid,'C' as type from invvendoritemmaster ivim inner join InvStockItemList isil on isil.itemid=ivim.itemid where vendorid='" + ddlVendorID.SelectedValue + "'" +
                                " union " +
                                " select itemname,itemid,BuyingPrice,HSNNumber,GSTPer,round((BuyingPrice*GSTPer/100),2) as GSTAmt, " +
                                " case  VatCmp1  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp1per + "')/100) end VatCmp1, " +
                                " case  VatCmp2  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp2per + "')/100) end VatCmp2, " +
                                " case  VatCmp3  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp3per + "')/100) end VatCmp3, " +
                                " case  VatCmp4  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp4per + "')/100) end VatCmp4 ," +
                                " case  VatCmp5  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp5per + "')/100) end VatCmp5," +
                                " (BuyingPrice+(case  VatCmp1  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp1per + "')/100) end )+(case  VatCmp2  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp2per + "')/100) end)+(case  VatCmp3  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp3per + "')/100) end)+(case  VatCmp4  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp4per + "')/100) end)+(case  VatCmp5  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp5per + "')/100) end))+ round( ((BuyingPrice*GSTPer)/100),2)  as Total,'','N' as Type from InvStockItemList where itemid not in (select ItemId from invvendoritemmaster where VendorID='" + ddlVendorID.SelectedValue + "') order by type,ItemName ";

                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqry).Result;

                if (dt.Rows.Count > 0)
                {
                    GVModifyItemList.DataSource = dt;
                    GVModifyItemList.DataBind();
                    // DeleteorinsertintoTable();

                    for (int j = 0; j < GVModifyItemList.Rows.Count; j++)
                    {

                        CheckBox chkitem = GVModifyItemList.Rows[j].FindControl("chkindividual") as CheckBox;
                        Label lbltype = GVModifyItemList.Rows[j].FindControl("lbltype") as Label;
                        if (lbltype.Text == "C")
                        {
                            chkitem.Checked = true;
                        }
                        else
                        {
                            chkitem.Checked = false;
                        }
                    }
                }
                else
                {
                    GVModifyItemList.DataSource = null;
                    GVModifyItemList.DataBind();
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

            for (int i = 0; i < GVModifyItemList.Rows.Count; i++)
            {
                DateTime Created_On = DateTime.Now;

                Label lblitemid = GVModifyItemList.Rows[i].FindControl("lblitemid") as Label;
                TextBox lblBuyingPrice = GVModifyItemList.Rows[i].FindControl("lblBuyingPrice") as TextBox;
                CheckBox chkitemid = GVModifyItemList.Rows[i].FindControl("chkindividual") as CheckBox;

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


        //int VendorID;


        protected void BtnSave_Click(object sender, EventArgs e)
        {
            lblresult.Text = "";
            lblresult.Visible = true;
            try
            {

                string VendorId = ddlVendorID.SelectedValue;
                string VendorName = ddlVendorName.SelectedValue;
                if (VendorName.Trim().Length == 0)
                {
                    lblresult.Text = "Don't Leave Empty Item Name";
                    return;
                }

                string Selvendorname = "select * from InvVendorMaster where VendorName='" + VendorName + "'";
                DataTable dtvendorname = config.ExecuteAdaptorAsyncWithQueryParams(Selvendorname).Result;
                if (dtvendorname.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Vendor Name is alredy Exist');", true);
                    return;
                }

                DateTime Modified_On = DateTime.Now;

                string contactperson = txtContactPerson.Text;
                string contactnos = txtContactNos.Text;
                string address = txtAddress.Text;
                string remarks = txtRemarks.Text;
                string emailid = txtemailid.Text;
                var VendorGSTNO = txtvendorgst.Text;

                string qry = "update InvVendorMaster set Address='" + address + "',ContactPerson='" + contactperson + "',ContactNo='" + contactnos + "',Remarks='" + remarks + "',emailid='" + emailid + "',modified_by='" + UserID + "',modified_on='" + Modified_On + "',VendorGSTNO='" + VendorGSTNO + "' where vendorid='" + ddlVendorID.SelectedValue + "'";
                int status = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                if (status != 0)
                {
                    lblresult.Text = "Record Modified Successfully";
                }
                else
                {
                    lblresult.Text = "Record Not Modified Successfully";
                }


                DeleteorinsertintoTable();
                cleardata();


            }
            catch (Exception ex)
            {
                lblresult.Text = ex.Message;

            }

        }


        public void cleardata()
        {
            txtAddress.Text = txtContactPerson.Text = txtContactNos.Text = txtRemarks.Text = txtemailid.Text = txtvendorgst.Text = string.Empty;
            ddlVendorName.SelectedIndex = ddlVendorID.SelectedIndex = 0;
            GVModifyItemList.DataSource = null;
            GVModifyItemList.DataBind();
        }

        float VATCmp1per = 0;
        float VATCmp2per = 0;
        float VATCmp3per = 0;
        float VATCmp4per = 0;
        float VATCmp5per = 0;

        protected void lblBuyingPrice_OnTextChanged(object sender, EventArgs e)
        {
            TextBox lblBuyingPrice = sender as TextBox;
            GridViewRow row = null;
            if (lblBuyingPrice == null)
                return;

            row = (GridViewRow)lblBuyingPrice.NamingContainer;
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

            TextBox lblBuyingPricev = row.FindControl("lblBuyingPrice") as TextBox;
            Label lblGSTPer = row.FindControl("lblGSTPer") as Label;
            Label lblGSTAmt = row.FindControl("lblGSTAmt") as Label;
            Label lblVATCmp1 = row.FindControl("lblVATCmp1") as Label;
            Label lblVATCmp2 = row.FindControl("lblVATCmp2") as Label;
            Label lblVATCmp3 = row.FindControl("lblVATCmp3") as Label;
            Label lblVATCmp4 = row.FindControl("lblVATCmp4") as Label;
            Label lblVATCmp5 = row.FindControl("lblVATCmp5") as Label;
            Label lblTotal = row.FindControl("lblTotal") as Label;


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

            if (lblVATCmp1.Text == "")
            {
                lblVATCmp1.Text = "0";
            }
            else
            {
                if (VATCmp1 == "True")
                {
                    lblVATCmp1.Text = (float.Parse(lblBuyingPricev.Text) * (VATCmp1per) / 100).ToString("#.##");
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
                    lblVATCmp2.Text = (float.Parse(lblBuyingPricev.Text) * (VATCmp2per) / 100).ToString("#.##");
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
                    lblVATCmp3.Text = (float.Parse(lblBuyingPricev.Text) * (VATCmp3per) / 100).ToString("#.##");
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
                    lblVATCmp4.Text = (float.Parse(lblBuyingPricev.Text) * (VATCmp4per) / 100).ToString("#.##");
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
                    lblVATCmp5.Text = (float.Parse(lblBuyingPricev.Text) * (VATCmp5per) / 100).ToString("#.##");
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
                lblGSTAmt.Text = (float.Parse(lblBuyingPricev.Text) * float.Parse(lblGSTPer.Text) / 100).ToString();
            }

            lblTotal.Text = (float.Parse(lblBuyingPricev.Text) + float.Parse(lblVATCmp1.Text) + float.Parse(lblVATCmp2.Text) + float.Parse(lblVATCmp3.Text) + float.Parse(lblVATCmp4.Text) + float.Parse(lblVATCmp5.Text) + float.Parse(lblGSTAmt.Text)).ToString();



            lblBuyingPrice.Focus();



        }

        protected void GVItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = GlobalData.Instance.LoadTaxComponents();

            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                if (dt.Rows.Count > 0)
                {
                    //VATcmp1
                    if (dt.Rows[10]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[7].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[7].Text = dt.Rows[10]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[7].Visible = false;
                    }

                    //VATcmp2
                    if (dt.Rows[11]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[8].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[8].Text = dt.Rows[11]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[8].Visible = false;

                    }


                    //VATcmp3
                    if (dt.Rows[12]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[9].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[9].Text = dt.Rows[12]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[9].Visible = false;
                    }

                    //VATcmp4
                    if (dt.Rows[13]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[10].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[10].Text = dt.Rows[13]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[10].Visible = false;
                    }


                    //VATcmp5
                    if (dt.Rows[14]["Visibility"].ToString() == "Y")
                    {
                        e.Row.Cells[11].Visible = true;
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[11].Text = dt.Rows[14]["TaxCmpName"].ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[11].Visible = false;
                    }

                }
            }

        }
    }
}