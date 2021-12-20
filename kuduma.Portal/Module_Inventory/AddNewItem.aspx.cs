using System;
using System.Web.UI;
using System.Data;
using System.IO;
using System.Data.OleDb;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class AddNewItem : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string InvIDPrefix = "";
        string Username = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            InvIDPrefix = Session["InvPrefix"].ToString();
            Username = Session["UserId"].ToString();

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
                Measuredunits();
                ddlmesure.Items.Insert(0, "--Select--");

                VATDisplay();

            }

        }

        public void VATDisplay()
        {
            string qry = "select TaxCmpID,TaxCmpName,Visibility from taxcomponentsmaster";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count > 0)
            {
                //VATcmp1
                if (dt.Rows[10]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp1.Visible = true;
                    ChkVATCmp1.Text = " " + dt.Rows[10]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp1.Visible = false;
                }

                //VATcmp2
                if (dt.Rows[11]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp2.Visible = true;
                    ChkVATCmp2.Text = " " + dt.Rows[11]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp2.Visible = false;
                }


                //VATcmp3
                if (dt.Rows[12]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp3.Visible = true;
                    ChkVATCmp3.Text = " " + dt.Rows[12]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp3.Visible = false;
                }

                //VATcmp4
                if (dt.Rows[13]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp4.Visible = true;
                    ChkVATCmp4.Text = " " + dt.Rows[13]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp4.Visible = false;
                }

                //VATcmp5
                if (dt.Rows[14]["Visibility"].ToString() == "Y")
                {
                    ChkVATCmp5.Visible = true;
                    ChkVATCmp5.Text = " " + dt.Rows[14]["TaxCmpName"].ToString();
                }
                else
                {
                    ChkVATCmp5.Visible = false;
                }

            }
        }

        protected void Measuredunits()
        {
            string Sqlqry = "Select * from units";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlmesure.DataSource = dt;
                ddlmesure.DataValueField = "Unitmeasure";
                ddlmesure.DataTextField = "unitmeasure";
                ddlmesure.DataBind();
            }

        }


        public void ItemIdAuto()
        {


            int ItemId = 1;
            string selectqueryclientid = "select (max(right(ItemId,3))) as ItemId from InvStockItemList  where ItemId like '" + InvIDPrefix + "%'";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;
            string invPrefix = string.Empty;

            if (dt.Rows.Count > 0)
            {
                //  DtEmpId = dtempid.Rows[dtempid.Rows.Count - 1][0].ToString();
                if (String.IsNullOrEmpty(dt.Rows[0]["ItemId"].ToString()) == false)
                {
                    ItemId = Convert.ToInt32(dt.Rows[0]["ItemId"].ToString()) + 1;
                }
                else
                {
                    ItemId = int.Parse("1");
                }
            }


            txtitemid.Text = InvIDPrefix + (ItemId).ToString("000");


        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {

            lblresult.Text = "";
            lblresult.Visible = true;

            try
            {

                string itemid = txtitemid.Text;
                string itemname = txtitemname.Text;

                if (itemname.Trim().Length == 0)
                {
                    lblresult.Text = "Don't Leave Empty Item Name";
                    return;
                }

                string Selitemname = "select * from InvStockItemList where itemname='" + itemname + "'";
                DataTable dtItemname = config.ExecuteAdaptorAsyncWithQueryParams(Selitemname).Result;
                if (dtItemname.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Item Name already Exists');", true);
                    return;
                }

                string manifacturedby = txtmy.Text;

                if (ddlmesure.SelectedIndex == 0)
                {
                    lblresult.Text = "Please Select Measured Units";
                    return;
                }
                string mesureunits = ddlmesure.SelectedValue;
                if (ddlmesure.SelectedIndex == 0)
                    mesureunits = "Units";

                //New code add as on 12/03/2014

                float minquantity = 0;
                if (txtmq.Text.Trim().Length > 0)
                {
                    minquantity = float.Parse(txtmq.Text);

                }
                else
                {
                    minquantity = 0;
                }
                string buyingprice = txtprice.Text;
                if (buyingprice.Trim().Length == 0)
                {
                    buyingprice = "0";
                }

                string sellingprice = txtsellingprice.Text;
                if (sellingprice.Trim().Length == 0)
                {
                    sellingprice = "0";
                }
                string Category = ddlCategory.SelectedValue;

                string HSNNumber = txtHSNNumber.Text;

                string GSTPer = ddlGSTPer.SelectedValue;

                var VATCmp1 = 0;
                var VATCmp2 = 0;
                var VATCmp3 = 0;
                var VATCmp4 = 0;
                var VATCmp5 = 0;

                if (ChkVATCmp1.Checked == true)
                {
                    VATCmp1 = 1;
                }
                if (ChkVATCmp2.Checked == true)
                {
                    VATCmp2 = 1;
                }
                if (ChkVATCmp3.Checked == true)
                {
                    VATCmp3 = 1;
                }
                if (ChkVATCmp4.Checked == true)
                {
                    VATCmp4 = 1;
                }
                if (ChkVATCmp5.Checked == true)
                {
                    VATCmp5 = 1;
                }

                if (minquantity > 0)
                {
                    string Transactionid = DateTime.Now.Month + "/" + DateTime.Now.Year + "/" + 0/*TransactionId.ToString()*/;
                    string date = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Year.ToString();

                }

                DateTime createdon = DateTime.Now;

                string insertquery = string.Format("insert into InvStockItemList(itemid,itemname,UnitMeasure,Brand," +
                    " MinimumQty,BuyingPrice,Sellingprice,ActualQuantity,Category,VATCmp1,VATCmp2,VATCmp3,VATCmp4,VATCmp5,HSNNumber,GSTper,Created_By,Created_On) values('{0}','{1}','{2}','{3}',{4},{5},{6},{7},'{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')",
                      itemid, itemname, mesureunits, manifacturedby, minquantity, buyingprice, sellingprice, 0, Category, VATCmp1, VATCmp2, VATCmp3, VATCmp4, VATCmp5, HSNNumber, GSTPer, Username, createdon);
                int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                if (status != 0)
                {
                    lblresult.Text = "Record Inserted Successfully";
                }
                else
                {
                    lblresult.Text = "Record Not Inserted Successfully";
                }
                ItemIdAuto();
                txtitemname.Text = txtmy.Text = txtmq.Text = txtprice.Text = txtsellingprice.Text = txtHSNNumber.Text = string.Empty;
                ddlmesure.SelectedIndex = ddlCategory.SelectedIndex = ddlGSTPer.SelectedIndex = 0;
                ChkVATCmp1.Checked = ChkVATCmp2.Checked = ChkVATCmp3.Checked = ChkVATCmp4.Checked = false;

            }
            catch (Exception ex)
            {
                lblresult.Text = ex.Message;

            }



        }


        public string GetExcelSheetNames()
        {
            string ExcelSheetname = "";
            OleDbConnection con = null;
            DataTable dt = null;
            string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
            fileupload1.PostedFile.SaveAs(filename);
            string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
            string conStr = string.Empty;
            if (extn.ToLower() == ".xls")
            {
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extn.ToLower() == ".xlsx")
            {
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
            }

            con = new OleDbConnection(conStr);
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();
            ////foreach (DataRow row in dt.Rows)
            ////{
            ////    ExcelSheetname = row["TABLE_NAME"].ToString();
            ////}

            return ExcelSheetname;
        }


        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = Path.Combine(Server.MapPath("ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
                fileupload1.PostedFile.SaveAs(filename);
                string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                string constring = string.Empty;

                if (extn.ToLower() == ".xls")
                {
                    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filename + "';Extended Properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                }
                if (extn.ToLower() == ".xlsx")
                {
                    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filename + "';Extended Properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                }
                string qry = "select [Item Name],[Manufactured By],[Units of measure],[Min Quantity],[Buying Price]," +
                    "[Selling Price] from [" + GetExcelSheetNames() + "]" + "";
                OleDbConnection con = new OleDbConnection(constring);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                OleDbCommand cmd = new OleDbCommand(qry, con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                con.Close();
                con.Dispose();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string itemname = string.Empty;
                    string manufacturedby = string.Empty;
                    string unitsofmeasure = string.Empty;
                    int minQuantity = 0;
                    float buyingprice = 0;
                    float sellingprice = 0;

                    itemname = dr["Item Name"].ToString();
                    manufacturedby = dr["Manufactured By"].ToString();
                    unitsofmeasure = dr["Units of measure"].ToString();

                    if (itemname.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Please Enter Item Name');", true);
                        return;
                    }
                    if (unitsofmeasure.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Please Enter unit');", true);
                        return;
                    }

                    if (String.IsNullOrEmpty(dr["Min Quantity"].ToString()) == false)
                    {
                        minQuantity = int.Parse(dr["Min Quantity"].ToString());
                    }

                    if (String.IsNullOrEmpty(dr["Buying Price"].ToString()) == false)
                    {
                        buyingprice = float.Parse(dr["Buying Price"].ToString());
                    }

                    if (String.IsNullOrEmpty(dr["Selling Price"].ToString()) == false)
                    {
                        sellingprice = float.Parse(dr["Selling Price"].ToString());
                    }

                    string selectItemname = "select ItemName from StockItemList where itemname='" + itemname + "'";
                    DataTable dtItemname = config.ExecuteAdaptorAsyncWithQueryParams(selectItemname).Result;
                    if (dtItemname.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('" + itemname + " Item Name already exist');", true);
                        return;
                    }
                    else
                    {
                        string insertquery = string.Format("insert into Stockitemlist( itemname,UnitMeasure,ManifactureBy,MinimumQty," +
                            " BuyingPrice,Sellingprice,ActualQuantity) values({0},'{1}','{2}','{3}',{4},{5},{6})",
                               itemname, unitsofmeasure, manufacturedby, minQuantity, buyingprice, sellingprice, 0);
                        int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                        if (status > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Items added Successfully');", true);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}