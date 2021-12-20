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
    public partial class InvVendorMaster : System.Web.UI.Page
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
                ItemIdAuto();
                GetGridData();
            }

        }

        float VATCmp1per = 0;
        float VATCmp2per = 0;
        float VATCmp3per = 0;
        float VATCmp4per = 0;
        float VATCmp5per = 0;


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            UserID = Session["UserId"].ToString();
            InvIDPrefix = Session["InvPrefix"].ToString();
            BranchID= Session["BranchID"].ToString();
        }
        protected void GVItemList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVItemList.PageIndex = e.NewPageIndex;
            GetGridData();

        }

        public void GetGridData()
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

                string qry = "select itemid,itemname,HSNNumber,BuyingPrice,GSTPer,case  VatCmp1  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp1per + "')/100) end VatCmp1,((BuyingPrice*GSTPer)/100) as GSTAmt," +
               " case  VATCmp2  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp2per + "')/100) end VATCmp2,case  VATCmp3  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp3per + "')/100) end VATCmp3, " +
                " case  VATCmp4  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp4per + "')/100) end VATCmp4,case  VATCmp5  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp5per + "')/100) end VATCmp5,(BuyingPrice+(case  VatCmp1  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp1per + "')/100) end)+case  VATCmp2  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp2per + "')/100) end +case  VATCmp3  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp3per + "')/100) end+case  VATCmp4  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp4per + "')/100) end+case  VATCmp5  when cast(0 as bit) then 0 else (BuyingPrice*('" + VATCmp5per + "')/100) end)++((BuyingPrice*GSTPer)/100) as Total from InvStockItemList ";


                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
                if (dt.Rows.Count > 0)
                {
                    GVItemList.DataSource = dt;
                    GVItemList.DataBind();
                }
                else
                {
                    GVItemList.DataSource = null;
                    GVItemList.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void GetGridDataintoTable()
        {
            string itemid = ""; string itemname = ""; string buyingprice = ""; string vendorid = ""; int status = 0;
            try
            {


                vendorid = txtVendorId.Text;


                for (int i = 0; i < GVItemList.Rows.Count; i++)
                {
                    Label lblitemid = GVItemList.Rows[i].FindControl("lblitemid") as Label;
                    Label lblitemname = GVItemList.Rows[i].FindControl("lblitemname") as Label;
                    TextBox lblBuyingPrice = GVItemList.Rows[i].FindControl("lblBuyingPrice") as TextBox;

                    CheckBox chkitemid = GVItemList.Rows[i].FindControl("chkindividual") as CheckBox;

                    if (chkitemid.Checked == true)
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

                        if (lblBuyingPrice.Text.Trim().Length == 0)
                        {
                            buyingprice = "0";
                        }
                        else
                        {
                            buyingprice = lblBuyingPrice.Text;
                        }

                        DateTime Created_On = DateTime.Now;
                        string sqlqry = "insert into invvendoritemmaster(vendorid,itemid,BuyingPrice,created_by,created_on) values('" + vendorid + "','" + itemid + "','" + buyingprice + "','" + UserID + "', '" + Created_On + "')";
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

        //int VendorID;
        public void ItemIdAuto()
        {
            int VendorID = 1;
            string selectqueryclientid = "select (max(right(VendorID,4))) as VendorID from InvVendorMaster ";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;
            string invPrefix = string.Empty;

            if (dt.Rows.Count > 0)
            {

                if (String.IsNullOrEmpty(dt.Rows[0]["VendorID"].ToString()) == false)
                {
                    VendorID = Convert.ToInt32(dt.Rows[0]["VendorID"].ToString()) + 1;
                }
                else
                {
                    VendorID = int.Parse("1");
                }
            }

            txtVendorId.Text = "VD" + (VendorID).ToString("0000");

        }


        protected void BtnSave_Click(object sender, EventArgs e)
        {
            lblresult.Text = "";
            lblresult.Visible = true;
            try
            {

                string VendorId = txtVendorId.Text;
                string VendorName = txtVendorName.Text;
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

                DateTime Created_On = DateTime.Now;

                string contactperson = txtContactPerson.Text;
                string contactnos = txtContactNos.Text;
                string address = txtAddress.Text;
                string remarks = txtRemarks.Text;
                string emailid = txtemailid.Text;
                var VendorGSTNO = txtvendorgst.Text;
                string insertquery = string.Format("insert into InvVendorMaster(VendorID,VendorName,Address,ContactPerson,ContactNo,Remarks,Created_By,Created_On,EMailId,VendorGSTNO" +
                " ) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                  VendorId, VendorName, address, contactperson, contactnos, remarks, UserID, Created_On, emailid, VendorGSTNO);
                int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                if (status != 0)
                {
                    lblresult.Text = "Record Inserted Successfully";
                }
                else
                {
                    lblresult.Text = "Record Not Inserted Successfully";
                }

                GetGridDataintoTable();
                cleardata();

            }
            catch (Exception ex)
            {
                lblresult.Text = ex.Message;

            }

        }


        public void cleardata()
        {
            ItemIdAuto();
            txtVendorName.Text = txtAddress.Text = txtContactPerson.Text = txtContactNos.Text = txtRemarks.Text = txtemailid.Text = txtvendorgst.Text = string.Empty;
            GVItemList.DataSource = null;
            GVItemList.DataBind();
        }

        protected void lblBuyingPrice_OnTextChanged(object sender, EventArgs e)
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


            TextBox lblBuyingPrice = sender as TextBox;
            GridViewRow row = null;
            if (lblBuyingPrice == null)
                return;

            row = (GridViewRow)lblBuyingPrice.NamingContainer;
            if (row == null)
                return;

            Label lblitemid = row.FindControl("lblitemid") as Label;



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

            TextBox lblBuyingPricev = row.FindControl("lblBuyingPrice") as TextBox;
            Label lblVATCmp1 = row.FindControl("lblVATCmp1") as Label;
            Label lblVATCmp2 = row.FindControl("lblVATCmp2") as Label;
            Label lblVATCmp3 = row.FindControl("lblVATCmp3") as Label;
            Label lblVATCmp4 = row.FindControl("lblVATCmp4") as Label;
            Label lblVATCmp5 = row.FindControl("lblVATCmp5") as Label;
            Label lblTotal = row.FindControl("lblTotal") as Label;
            Label lblGSTPer = row.FindControl("lblGSTPer") as Label;
            Label lblGSTAmt = row.FindControl("lblGSTAmt") as Label;

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
                lblGSTAmt.Text = (float.Parse(lblBuyingPricev.Text) * float.Parse(lblGSTPer.Text) / 100).ToString("#.##");
            }

            if(lblGSTAmt.Text=="")
            {
                lblGSTAmt.Text = "0";
            }

            lblTotal.Text = (float.Parse(lblBuyingPricev.Text) + float.Parse(lblVATCmp1.Text) + float.Parse(lblVATCmp2.Text) + float.Parse(lblVATCmp3.Text) + float.Parse(lblVATCmp4.Text) + float.Parse(lblVATCmp5.Text) + float.Parse(lblGSTAmt.Text)).ToString("#.##");

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