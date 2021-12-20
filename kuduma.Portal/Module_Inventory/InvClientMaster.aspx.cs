using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class InvClientMaster : System.Web.UI.Page
    {
        DataTable dt;
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Username = "";
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


                LoadClientList();
                LoadClientNames();
                LoadItems();

            }

        }

        public void LoadItems()
        {
            string qry = "";

            if (CbExemption.Checked == false)
            {


                qry = "select Itemid,Itemname,UnitMeasure,MinimumQty,Category,BuyingPrice,SellingPrice,0 as qty,case  Vat5per  when cast(0 as bit) then 0 else (SellingPrice*5/100) end Vat5per, " +
                           " case  vat14per  when cast(0 as bit) then 0 else (SellingPrice*14.5/100) end vat14per,(SellingPrice+(case Vat5per when cast(0 as bit) " +
                           " then 0 else (SellingPrice*5/100) end )+(case  cast(vat14per as bit) when cast(0 as bit) then 0 else (SellingPrice*14.5/100) end )) as Total,'N' as type from invstockitemlist ";

            }
            else
            {
                qry = "select Itemid,Itemname,UnitMeasure,MinimumQty,Category,BuyingPrice,SellingPrice,0 as Qty,0 as Vat5per, " +
                           " 0 as vat14per,(SellingPrice) as Total,'N' as type from invstockitemlist ";

            }

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                gvresources.DataSource = dt;
                gvresources.DataBind();
            }
            else
            {
                gvresources.DataSource = null;
                gvresources.DataBind();
            }
        }



        protected void Txtsellingprice_OnTextChanged(object sender, EventArgs e)
        {

            TextBox lblSellingPrice = sender as TextBox;
            GridViewRow row = null;
            if (lblSellingPrice == null)
                return;

            row = (GridViewRow)lblSellingPrice.NamingContainer;
            if (row == null)
                return;

            Label lblitemid = row.FindControl("lblitemid") as Label;


            string qry = "select cast(vat5per as bit) as vat5per,cast(vat14per as bit) as vat14per from invstockitemlist where itemid='" + lblitemid.Text + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            string vat5per = "False";
            string vat14per = "False";

            if (dt.Rows.Count > 0)
            {
                vat5per = dt.Rows[0]["vat5per"].ToString();
                vat14per = dt.Rows[0]["vat14per"].ToString();

            }

            TextBox lblSellingPricev = row.FindControl("lblSellingPrice") as TextBox;
            Label TxtVAT5v = row.FindControl("TxtVAT5") as Label;
            Label TxtVAT14 = row.FindControl("lblVAT14per") as Label;
            Label lblTotal = row.FindControl("lblTotal") as Label;

            if (TxtVAT5v.Text == "")
            {
                TxtVAT5v.Text = "0";
            }
            else
            {
                if (vat5per == "True")
                {
                    TxtVAT5v.Text = (float.Parse(lblSellingPricev.Text) * 5 / 100).ToString("#.##");
                }
                else
                {
                    TxtVAT5v.Text = "0";
                }
            }

            if (TxtVAT14.Text == "")
            {
                TxtVAT14.Text = "0";
            }
            else
            {
                if (vat14per == "True")
                {
                    TxtVAT14.Text = (float.Parse(lblSellingPricev.Text) * 14.5 / 100).ToString("#.##");
                }
                else
                {
                    TxtVAT14.Text = "0";
                }
            }

            if (lblTotal.Text == "")
            {
                lblTotal.Text = "0";
            }


            lblTotal.Text = (float.Parse(lblSellingPricev.Text) + float.Parse(TxtVAT5v.Text) + float.Parse(TxtVAT14.Text)).ToString("#.##");



        }


        public void loadexemption()
        {
            string qry = "";

            qry = "select icrm.itemid,isil.itemname,isil.category,icrm.buyingprice,icrm.sellingprice,icrm.vat5per,icrm.vat14per,icrm.Total,'C' as Type from invclientratemaster icrm inner join invstockitemlist isil on isil.itemid=icrm.itemid where clientid='" + ddlclientid.SelectedValue + "'" +
                   "union " +
                  "select itemid,itemname,category,buyingprice,sellingprice,case  Vat5per  when cast(0 as bit) then 0 else round((SellingPrice*5/100),2) end Vat5per,case  vat14per  when cast(0 as bit) then 0 else round((SellingPrice*14.5/100),2) end vat14per,(SellingPrice+(case Vat5per when cast(0 as bit) then 0 else round((SellingPrice*5/100),2) end )+(case  cast(vat14per as bit) when cast(0 as bit) then 0 else round((SellingPrice*14.5/100),2) end )) as Total,'N' as type from  invstockitemlist where ItemId not in (select ItemId from invclientratemaster where clientid='" + ddlclientid.SelectedValue + "') order by Type,itemname";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count > 0)
            {
                gvresources.DataSource = dt;
                gvresources.DataBind();


                for (int j = 0; j < gvresources.Rows.Count; j++)
                {

                    CheckBox chkitem = gvresources.Rows[j].FindControl("chkindividual") as CheckBox;
                    Label lbltype = gvresources.Rows[j].FindControl("lbltype") as Label;
                    Label TxtVAT5 = gvresources.Rows[j].FindControl("TxtVAT5") as Label;
                    Label lblVAT14per = gvresources.Rows[j].FindControl("lblVAT14per") as Label;
                    Label lblTotal = gvresources.Rows[j].FindControl("lblTotal") as Label;
                    TextBox lblSellingPrice = gvresources.Rows[j].FindControl("lblSellingPrice") as TextBox;

                    if (CbExemption.Checked == true)
                    {
                        TxtVAT5.Text = "0";
                        lblVAT14per.Text = "0";
                        lblTotal.Text = lblSellingPrice.Text;
                    }

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
                LoadItems();
            }

        }


        public void loaditemsforClientID()
        {
            string qry = "";


            qry = "select top 1 includest from contracts where clientid='" + ddlclientid.SelectedValue + "'";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            string STExemption = "False";

            if (dt.Rows.Count > 0)
            {
                STExemption = dt.Rows[0]["includest"].ToString();
            }


            if (STExemption == "False")
            {

                qry = "select icrm.itemid,isil.itemname,isil.category,icrm.buyingprice,icrm.sellingprice,isnull(icrm.qty,0) as qty, icrm.Vat5per,icrm.vat14per,icrm.Total,'C' as Type from invclientratemaster icrm inner join invstockitemlist isil on isil.itemid=icrm.itemid  where clientid='" + ddlclientid.SelectedValue + "' " +
                       " union " +
                      " select itemid,itemname,category,buyingprice,sellingprice,0 qty,case  Vat5per  when cast(0 as bit) then 0 else round((SellingPrice*5/100),2) end Vat5per,case  vat14per  when cast(0 as bit) then 0 else round((SellingPrice*14.5/100),2) end vat14per,(SellingPrice+(case Vat5per when cast(0 as bit) then 0 else round((SellingPrice*5/100),2) end )+(case  cast(vat14per as bit) when cast(0 as bit) then 0 else round((SellingPrice*14.5/100),2) end )) as Total,'N' as type from  invstockitemlist where ItemId not in (select ItemId from invclientratemaster where clientid='" + ddlclientid.SelectedValue + "') order by Type,itemname";
                CbExemption.Checked = false;


            }
            else
            {
                qry = "select icrm.itemid,isil.itemname,isil.category,icrm.buyingprice,icrm.sellingprice,isnull(icrm.qty,0) as qty, icrm.Vat5per,icrm.vat14per,icrm.Total,'C' as Type from invclientratemaster icrm inner join invstockitemlist isil on isil.itemid=icrm.itemid  where clientid='" + ddlclientid.SelectedValue + "' " +
                   " union " +
                  " select itemid,itemname,category,buyingprice,sellingprice,0 as qty,0 as Vat5per,0 as vat14per,(SellingPrice+(0 )+(0 )) as Total,'N' as type from  invstockitemlist where ItemId not in (select ItemId from invclientratemaster where clientid='" + ddlclientid.SelectedValue + "') order by Type,itemname";

                CbExemption.Checked = true;
            }




            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count > 0)
            {
                gvresources.DataSource = dt;
                gvresources.DataBind();


                for (int j = 0; j < gvresources.Rows.Count; j++)
                {

                    CheckBox chkitem = gvresources.Rows[j].FindControl("chkindividual") as CheckBox;
                    Label lbltype = gvresources.Rows[j].FindControl("lbltype") as Label;

                    if (lbltype.Text == "C")
                    {
                        chkitem.Checked = true;
                    }
                    else if (lbltype.Text == "N")
                    {
                        chkitem.Checked = false;
                    }

                }

            }
            else
            {
                LoadItems();
            }

        }


        public void loaditemsforClientName()
        {
            string qry = "";
            qry = "select icrm.itemid,isil.itemname,isil.category,icrm.buyingprice,icrm.sellingprice,icrm.vat5per,icrm.vat14per,icrm.Total,'C' as Type from invclientratemaster icrm inner join invstockitemlist isil on isil.itemid=icrm.itemid where clientid='" + ddlcname.SelectedValue + "'" +
                   "union " +
                  "select itemid,itemname,category,buyingprice,sellingprice,vat5per,vat14per,(sellingprice+vat5per+vat14per),'N' as type from  invstockitemlist where ItemId not in (select ItemId from invclientratemaster where clientid='" + ddlcname.SelectedValue + "') order by Type,itemname";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;


            if (dt.Rows.Count > 0)
            {
                gvresources.DataSource = dt;
                gvresources.DataBind();

                for (int j = 0; j < gvresources.Rows.Count; j++)
                {

                    CheckBox chkitem = gvresources.Rows[j].FindControl("chkindividual") as CheckBox;
                    Label lbltype = gvresources.Rows[j].FindControl("lbltype") as Label;

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
                LoadItems();
            }

        }



        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                Username = Session["UserId"].ToString();
                BranchID= Session["BranchID"].ToString();
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");

        }

        protected void LoadClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
        }
        protected void ddlclientid_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlclientid.SelectedIndex > 0)
            {
                Fillcname();
                loaditemsforClientID();
                uppanel2.Update();
            }
            else
            {
                ddlcname.SelectedIndex = 0;

            }
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcname.SelectedIndex > 0)
            {
                FillClientid();
                loaditemsforClientID();
                uppanel2.Update();

            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void Fillcname()
        {
            if (ddlclientid.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
                loaditemsforClientID();
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void FillClientid()
        {
            if (ddlcname.SelectedIndex > 0)
            {
                ddlclientid.SelectedValue = ddlcname.SelectedValue;
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }


        

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            LoadItems();

        }


        public void SaveItems()
        {
            int status = 0;
            string clientid = "";
            clientid = ddlclientid.SelectedValue;
            var list = new List<string>();

            if (gvresources.Rows.Count > 0)
            {
                for (int i = 0; i < gvresources.Rows.Count; i++)
                {

                    CheckBox chkclientid = gvresources.Rows[i].FindControl("chkindividual") as CheckBox;
                    Label lblItemID = gvresources.Rows[i].FindControl("lblItemID") as Label;

                    if (chkclientid.Checked == true)
                    {
                        list.Add("'" + lblItemID.Text + "'");



                    }

                }
                if (list.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select atleast one Item');", true);
                    return;
                }


                if (ddlclientid.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select client');", true);
                    return;
                }

                foreach (GridViewRow gvrow in gvresources.Rows)
                {
                    CheckBox chkclientid = gvrow.FindControl("chkindividual") as CheckBox;


                    string itemid = ((Label)gvrow.FindControl("lblItemID")).Text;
                    string Category = ((Label)gvrow.FindControl("lblCategory")).Text;
                    string buyingprice = ((Label)gvrow.FindControl("lblBuyingPrice")).Text;
                    float sellingprice = float.Parse(((TextBox)gvrow.FindControl("lblSellingPrice")).Text);
                    float VATfive = float.Parse(((Label)gvrow.FindControl("TxtVAT5")).Text);
                    float VATfourteen = float.Parse(((Label)gvrow.FindControl("lblVAT14per")).Text);
                    float lblTotal = float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
                    float Qty = float.Parse(((TextBox)gvrow.FindControl("TxtQty")).Text);

                    DateTime createdon = DateTime.Now;

                    if (chkclientid.Checked == true)
                    {

                        string insertquery = string.Format("insert into InvClientRateMaster(Clientid,itemid,BuyingPrice,Sellingprice,Qty,Category,VAT5Per,VAT14Per,total,Created_By,Created_On) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                        clientid, itemid, buyingprice, sellingprice, Qty, Category, VATfive, VATfourteen, lblTotal, Username, createdon);
                        status =config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                        if (status != 0)
                        {
                            lblresult.Visible = true;
                            lblresult.Text = "Record Inserted Successfully";

                            //ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Item list inserted successfully');", true);
                            //return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Item list not inserted successfully');", true);
                            return;
                        }
                    }

                }

            }
        }

        public void cleardata()
        {
            ddlclientid.SelectedIndex = 0;
            ddlcname.SelectedIndex = 0;
            gvresources.DataSource = null;
            gvresources.DataBind();
            CbExemption.Checked = false;
            LoadItems();
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {


            int status = 0;

            string query = "";


            query = " select * from InvClientRateMaster where clientid='" + ddlclientid.SelectedValue + "' ";
            DataTable dtqry = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;

            if (dtqry.Rows.Count > 0)
            {
                query = "delete from InvClientRateMaster where clientid='" + ddlclientid.SelectedValue + "' ";
                status = config.ExecuteNonQueryWithQueryAsync(query).Result;
                SaveItems();
            }
            else
            {
                SaveItems();
            }

            cleardata();


        }




        protected void CbExemption_CheckedChanged(object sender, EventArgs e)
        {
            loadexemption();
        }
    }
}