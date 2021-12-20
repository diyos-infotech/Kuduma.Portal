using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using KLTS.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Clients
{
    public partial class _1TimeBill : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
       
        string UserID = "";
        string InvBillPrefixWithST = "";
        string InvBillPrefixwithoutST = "";
        string FontStyle = "Calibri";
        string BranchID = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil grvutil = new GridViewExportUtil();
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
                        Response.Redirect("login.aspx");
                    }
                    SetInitialRow();

                    //DataTable DtDesignation = GlobalData.Instance.LoadItemNames();
                    //List<string> list = DtDesignation.AsEnumerable()
                    //                   .Select(r => r.Field<string>("ItemId"))
                    //                   .ToList();

                    //List<string> listitemname = DtDesignation.AsEnumerable()
                    //                  .Select(r => r.Field<string>("ItemName"))
                    //                  .ToList();
                    //var result = new JavaScriptSerializer().Serialize(list);
                    //var result1 = new JavaScriptSerializer().Serialize(listitemname);
                    //hdItemid.Value = result;
                    //hditemname.Value = result1;
                    //  txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    InvoiceNoAuto();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }
        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            UserID = Session["UserId"].ToString();
            InvBillPrefixWithST = Session["InvBillPrefixwithST"].ToString();
            InvBillPrefixwithoutST = Session["InvBillPrefixwithoutST"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        private void SetInitialRow()
        {
            DataTable DefaultTable = new DataTable();
            DefaultTable.Columns.Add("Sid", typeof(int));
            DefaultTable.Columns.Add("Designation", typeof(string));
            DefaultTable.Columns.Add("HSNNumber", typeof(string));
            DefaultTable.Columns.Add("NoofEmps", typeof(string));
            DefaultTable.Columns.Add("DutyHours", typeof(string));
            DefaultTable.Columns.Add("payrate", typeof(string));
            DefaultTable.Columns.Add("paytype", typeof(string));
            DefaultTable.Columns.Add("BasicDa", typeof(string));
            DefaultTable.Columns.Add("NoOfDays", typeof(string));
            DefaultTable.Columns.Add("Totalamount", typeof(string));
            DefaultTable.Columns.Add("GSTPer", typeof(string));
            // DefaultTable.Columns.Add("CGSTPrc", typeof(string));
            DefaultTable.Columns.Add("CGSTAmt", typeof(string));
            // DefaultTable.Columns.Add("SGSTPrc", typeof(string));
            DefaultTable.Columns.Add("SGSTAmt", typeof(string));
            // DefaultTable.Columns.Add("IGSTPrc", typeof(string));
            DefaultTable.Columns.Add("IGSTAmt", typeof(string));
            DefaultTable.Columns.Add("TotalTaxAmount", typeof(string));


            for (int i = 0; i < 6; i++)
            {
                DataRow dr = DefaultTable.NewRow();
                //dr["Sid"] = i;
                dr["Designation"] = "";
                dr["NoofEmps"] = 0;
                dr["DutyHours"] = 0;
                dr["payrate"] = 0;
                dr["paytype"] = 0;
                dr["BasicDa"] = 0;
                dr["NoOfDays"] = 1;
                dr["Totalamount"] = 0;

                dr["GSTPer"] = 0;
                // dr["CGSTPrc"] = 0;
                dr["CGSTAmt"] = 0;
                //dr["SGSTPrc"] = 0;
                dr["SGSTAmt"] = 0;
                // dr["IGSTPrc"] = 0;
                dr["IGSTAmt"] = 0;
                dr["TotalTaxAmount"] = 0;
                dr["HSNNumber"] = "";
                DefaultTable.Rows.Add(dr);
            }

            ViewState["CurrentTable"] = DefaultTable;
            gvClientBilling.DataSource = DefaultTable;
            gvClientBilling.DataBind();
        }

        private void AddNewRowToGrid()
        {

            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                //DataRow drCurrentRow1 = null;


                if (dtCurrentTable.Rows.Count > 0)
                {


                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[rowIndex].Cells[1].FindControl("lbldesgn");
                        TextBox txtHSNNumber = (TextBox)gvClientBilling.Rows[rowIndex].Cells[2].FindControl("txtHSNNumber");
                        TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[rowIndex].Cells[3].FindControl("lblnoofemployees");
                        TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[rowIndex].Cells[4].FindControl("lblNoOfDuties");
                        TextBox txtPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[5].FindControl("lblpayrate");
                        DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[6].FindControl("ddldutytype");
                        DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[7].FindControl("ddlnod");
                        TextBox txtda = (TextBox)gvClientBilling.Rows[rowIndex].Cells[8].FindControl("lblda");
                        TextBox txtAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[9].FindControl("lblAmount");
                        TextBox lblGSTper = (TextBox)gvClientBilling.Rows[rowIndex].Cells[10].FindControl("lblGSTper");
                        TextBox lblCGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[11].FindControl("lblCGSTAmount");
                        TextBox lblSGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[12].FindControl("lblSGSTAmount");
                        TextBox lblIGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[13].FindControl("lblIGSTAmount");
                        TextBox lblTotalTaxmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[14].FindControl("lblTotalTaxmount");


                        drCurrentRow = dtCurrentTable.NewRow();
                        // drCurrentRow["Sid"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Designation"] = txtgvdesgn.Text;
                        dtCurrentTable.Rows[i - 1]["HSNNumber"] = txtHSNNumber.Text;
                        dtCurrentTable.Rows[i - 1]["NoofEmps"] = txtnoofemployees.Text.Trim() == "" ? 0 : Convert.ToInt32(txtnoofemployees.Text);
                        dtCurrentTable.Rows[i - 1]["DutyHours"] = txtNoOfDuties.Text.Trim() == "" ? 0 : Convert.ToSingle(txtNoOfDuties.Text);
                        dtCurrentTable.Rows[i - 1]["payrate"] = txtPayRate.Text.Trim() == "" ? 0 : Convert.ToSingle(txtPayRate.Text);
                        dtCurrentTable.Rows[i - 1]["paytype"] = ddldutytype.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["NoOfDays"] = ddlnod.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["BasicDa"] = txtda.Text.Trim() == "" ? 0 : Convert.ToSingle(txtda.Text);
                        dtCurrentTable.Rows[i - 1]["Totalamount"] = txtAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(txtAmount.Text);
                        dtCurrentTable.Rows[i - 1]["GSTper"] = lblGSTper.Text.Trim() == "" ? 0 : Convert.ToSingle(lblGSTper.Text);
                        dtCurrentTable.Rows[i - 1]["CGSTAmt"] = lblCGSTAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(lblCGSTAmount.Text);
                        dtCurrentTable.Rows[i - 1]["SGSTAmt"] = lblSGSTAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(lblSGSTAmount.Text);
                        dtCurrentTable.Rows[i - 1]["IGSTAmt"] = lblIGSTAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(lblIGSTAmount.Text);
                        dtCurrentTable.Rows[i - 1]["TotalTaxAmount"] = lblTotalTaxmount.Text.Trim() == "" ? 0 : Convert.ToSingle(lblTotalTaxmount.Text);

                        rowIndex++;

                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    gvClientBilling.DataSource = dtCurrentTable;
                    gvClientBilling.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < gvClientBilling.Rows.Count; i++)
                    {

                        TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[rowIndex].Cells[1].FindControl("lbldesgn");
                        TextBox txtHSNNumber = (TextBox)gvClientBilling.Rows[rowIndex].Cells[2].FindControl("txtHSNNumber");
                        TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[rowIndex].Cells[3].FindControl("lblnoofemployees");
                        TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[rowIndex].Cells[4].FindControl("lblNoOfDuties");
                        TextBox txtPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[5].FindControl("lblpayrate");
                        DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[6].FindControl("ddldutytype");
                        DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[7].FindControl("ddlnod");
                        TextBox txtda = (TextBox)gvClientBilling.Rows[rowIndex].Cells[8].FindControl("lblda");
                        TextBox txtAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[9].FindControl("lblAmount");
                        TextBox lblGSTper = (TextBox)gvClientBilling.Rows[rowIndex].Cells[10].FindControl("lblGSTper");
                        TextBox lblCGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[11].FindControl("lblCGSTAmount");
                        TextBox lblSGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[12].FindControl("lblSGSTAmount");
                        TextBox lblIGSTAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[13].FindControl("lblIGSTAmount");
                        TextBox lblTotalTaxmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[14].FindControl("lblTotalTaxmount");

                        //if (dt.Rows[i]["Quantity"].ToString() == "")
                        //{
                        //    dt.Rows[i]["Quantity"] = "0";
                        //}

                        //if (dt.Rows[i]["BuyingPriceperunit"].ToString() == "")
                        //{
                        //    dt.Rows[i]["BuyingPriceperunit"] = "0";
                        //}

                        //if (dt.Rows[i]["TotalBuyingPrice"].ToString() == "")
                        //{
                        //    dt.Rows[i]["TotalBuyingPrice"] = "0";
                        //}

                        //if (dt.Rows[i]["GSTPer"].ToString() == "")
                        //{
                        //    dt.Rows[i]["GSTPer"] = "0";
                        //}

                        //if (dt.Rows[i]["GSTAmount"].ToString() == "")
                        //{
                        //    dt.Rows[i]["GSTAmount"] = "0";
                        //}

                        //if (dt.Rows[i]["Amount"].ToString() == "")
                        //{
                        //    dt.Rows[i]["Amount"] = "0";
                        //}

                        txtgvdesgn.Text = dt.Rows[i]["Designation"].ToString();
                        txtHSNNumber.Text = dt.Rows[i]["HSNNumber"].ToString();
                        txtnoofemployees.Text = dt.Rows[i]["NoofEmps"].ToString();
                        txtNoOfDuties.Text = dt.Rows[i]["DutyHours"].ToString();
                        txtPayRate.Text = dt.Rows[i]["payrate"].ToString();
                        ddldutytype.SelectedValue = dt.Rows[i]["paytype"].ToString();
                        ddlnod.SelectedValue = dt.Rows[i]["NoOfDays"].ToString();
                        txtda.Text = dt.Rows[i]["BasicDa"].ToString();
                        txtAmount.Text = dt.Rows[i]["Totalamount"].ToString();
                        lblGSTper.Text = dt.Rows[i]["GSTper"].ToString();
                        lblCGSTAmount.Text = dt.Rows[i]["CGSTAmt"].ToString();
                        lblSGSTAmount.Text = dt.Rows[i]["SGSTAmt"].ToString();
                        lblIGSTAmount.Text = dt.Rows[i]["IGSTAmt"].ToString();
                        lblTotalTaxmount.Text = dt.Rows[i]["TotalTaxAmount"].ToString();


                        rowIndex++;
                    }
                }
            }
        }

        protected void btnaddrow_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        protected void txtitemID_TextChanged(object sender, EventArgs e)
        {
            TextBox txtitemid = sender as TextBox;
            GridViewRow row = null;
            if (txtitemid == null)
                return;

            row = (GridViewRow)txtitemid.NamingContainer;
            if (row == null)
                return;

            TextBox txthsnnumber = row.FindControl("txthsnno") as TextBox;
            TextBox txtbuyingpriceperunit = row.FindControl("txtbuyingprice") as TextBox;
            TextBox txtitemidNo = row.FindControl("txtitemID") as TextBox;
            TextBox txtitemName = row.FindControl("txtItemname") as TextBox;
            TextBox txtgstper = row.FindControl("txtgstper") as TextBox;

            string ProcedureName = "GetItemNamesandIDs";

            Hashtable HtACtiveClients = new Hashtable();
            HtACtiveClients.Add("@ItemID", txtitemidNo.Text);

            DataTable DtInActiveClients = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtACtiveClients).Result;
            if (DtInActiveClients.Rows.Count > 0)
            {
                txtitemName.Text = DtInActiveClients.Rows[0]["ItemName"].ToString();
                txtbuyingpriceperunit.Text = DtInActiveClients.Rows[0]["Sellingprice"].ToString();
                txthsnnumber.Text = DtInActiveClients.Rows[0]["HSNNumber"].ToString();
                txtgstper.Text = DtInActiveClients.Rows[0]["GSTPer"].ToString();
            }

        }

        protected void txtItemname_TextChanged(object sender, EventArgs e)
        {
            TextBox txtitemid = sender as TextBox;
            GridViewRow row = null;
            if (txtitemid == null)
                return;

            row = (GridViewRow)txtitemid.NamingContainer;
            if (row == null)
                return;

            TextBox txthsnnumber = row.FindControl("txthsnno") as TextBox;
            TextBox txtbuyingpriceperunit = row.FindControl("txtbuyingprice") as TextBox;
            TextBox txtitemidNo = row.FindControl("txtitemID") as TextBox;
            TextBox txtitemName = row.FindControl("txtItemname") as TextBox;
            TextBox txtgstper = row.FindControl("txtgstper") as TextBox;

            string ProcedureName = "GetItemNamesandIDs";

            Hashtable HtACtiveClients = new Hashtable();
            HtACtiveClients.Add("@ItemName", txtitemName.Text);

            DataTable DtInActiveClients = config.ExecuteAdaptorAsyncWithParams(ProcedureName, HtACtiveClients).Result;
            if (DtInActiveClients.Rows.Count > 0)
            {
                txtitemidNo.Text = DtInActiveClients.Rows[0]["ItemId"].ToString();
                txtbuyingpriceperunit.Text = DtInActiveClients.Rows[0]["Sellingprice"].ToString();
                txthsnnumber.Text = DtInActiveClients.Rows[0]["HSNNumber"].ToString();
                txtgstper.Text = DtInActiveClients.Rows[0]["GSTPer"].ToString();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            ManualBillGenerateMethod();
        }

        public void ManualBillGenerateMethod()
        {
            try
            {

                #region Validations

                if (txtnewclientid.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill Client Id ');", true);

                    return;
                }

                if (txtnewclientname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill Client Name ');", true);

                    return;
                }

                #region  Begin New code As on [10-03-2014]

                if (txtmonth.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Enter Month for Bill ');", true);
                    return;
                }

                #endregion  End Old Code As on [14-02-2014]
                if (txtinvoicedate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Please Fill The Billdate  ');", true);
                    return;
                }

                if (txtinvoiceno.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please fill The Bill No.');", true);
                    return;
                }

                if (txtgstno.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please fill The GST No.');", true);
                    return;
                }

                if (txtGSTAddress.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please fill The GST Address.');", true);
                    return;
                }

                #endregion

                bool CGST = false;
                bool SGST = false;
                bool IGST = false;
                int month = GetMonthBasedOnSelectionDateorMonth();

                string SelectedClient = txtnewclientid.Text;
                string ClientName = txtnewclientname.Text;

                string GSTType = "";
                if (rdbwithGST.Checked == true)
                {
                    GSTType = "Y";
                }

                else if (rdbwithoutGST.Checked == true)
                {
                    GSTType = "N";
                }

                if (rdbwithGST.Checked == true)
                {
                    CGST = rdbSGST.Checked;
                    SGST = rdbSGST.Checked;
                    IGST = rdbIGST.Checked;
                }
                #region Month Selection

                //month = GetMonthBasedOnSelectionDateorMonth();

                #endregion

                DateTime dt = DateTime.ParseExact(txtinvoicedate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string billdt = dt.ToString("MM/dd/yyyy");

                var query1 = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2 from TblOptions where '" + billdt + "' between fromdate and todate ";//Gst
                var optiondetails = config.ExecuteAdaptorAsyncWithQueryParams(query1).Result;


                decimal CGSTprc = 0;
                decimal IGSTprc = 0;
                decimal SGSTprc = 0;


                if (optiondetails.Rows.Count > 0)
                {
                    CGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["CGST"].ToString());
                    IGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["IGST"].ToString());
                    SGSTprc = Convert.ToDecimal(optiondetails.Rows[0]["SGST"].ToString());
                }


                #region  Query For Delete Unitbill Break Up Data

                /** Delete previously generated UnitBillBreakup data */

                //if (rdbmodifybill.Checked)
                //{
                //    string DeleteQueryForSelectedMonth = "Delete from Munitbillbreakup where unitid ='" + SelectedClient + "' and month =" +
                //                                                         month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue + "'";
                //    int deleteop = config.ExecuteNonQueryWithQueryAsync(DeleteQueryForSelectedMonth).Result;
                //}
                //Unitbill details are not deleted now due to Billno(avoid regeneration)
                /** Delete **/

                #endregion

                #region   Query for  Get  Contracts  Details                                          

                #region   Get Data From GridView and Saving In the Munitbillbreakup Table

                if (gvClientBilling.Rows.Count > 0)
                {
                    string Unitid = txtnewclientid.Text;
                    string UnitName = txtnewclientname.Text;
                    int totalstatus = 0;
                    int i = 0;

                    foreach (GridViewRow GvRow in gvClientBilling.Rows)
                    {
                        string sno = ((Label)GvRow.FindControl("lblSno")).Text;
                        string Desgn = ((TextBox)GvRow.FindControl("lbldesgn")).Text;
                        string HSNNumber = ((TextBox)GvRow.FindControl("txtHSNNumber")).Text;
                        string NoOfEmps = ((TextBox)GvRow.FindControl("lblnoofemployees")).Text;
                        string NoOfDuties = ((TextBox)GvRow.FindControl("lblNoOfDuties")).Text;
                        string Payrate = ((TextBox)GvRow.FindControl("lblpayrate")).Text; //lblda                       

                        string DutiesAmount = ((TextBox)GvRow.FindControl("lblda")).Text;
                        string Total = ((TextBox)GvRow.FindControl("lblAmount")).Text;
                        string TotalTaxmount = ((TextBox)GvRow.FindControl("lblTotalTaxmount")).Text;
                        string GSTper = ((TextBox)GvRow.FindControl("lblGSTper")).Text;

                        float ToatlAmount = 0;
                        float basicda = 0;
                        ToatlAmount = (Total.Trim().Length != 0) ? float.Parse(Total) : 0;
                        basicda = (DutiesAmount.Trim().Length != 0) ? float.Parse(DutiesAmount) : 0;
                        DropDownList ddlnodays = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                        float ddlnod = float.Parse(ddlnodays.SelectedItem.Text);
                        DropDownList ddldttype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;
                        int ddldutytype = int.Parse(ddldttype.SelectedValue);

                        i = i + 1;

                        if (Desgn.Length > 0)
                        {
                            string Sqlqry = string.Format("insert into OneTimeBillBreakup(unitid,ClientName,designation,DutyHours,NoofEmps,BasicDa, " +
                                "PayRate,PayRateType,Month,Totalamount,MunitidBillno,monthlydays,noofdays,sno,HSNNumber) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
                                Unitid, UnitName, Desgn, NoOfDuties, NoOfEmps, DutiesAmount, Payrate, ddldutytype, month, Total, txtinvoiceno.Text, ddlnod, ddlnod, sno, HSNNumber);
                            int Status = config.ExecuteNonQueryWithQueryAsync(Sqlqry).Result;
                            if (Status != 0)
                            {
                                totalstatus++;
                                if (totalstatus == 1)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Billing Details Added Sucessfully');", true);

                                }
                            }
                        }
                    }
                }
                #endregion

                #region   Storing Details about the  Unit Bill Table


                string grandtotal = lblGrandTotal.Text;
                if (grandtotal.Trim().Length == 0)
                {
                    grandtotal = "0";
                }

                #region for GST as on 17-6-2017 by swathi

                string CGSTTax = lblCGST.Text;

                if (CGSTTax.Trim().Length == 0)
                {
                    CGSTTax = "0";
                }


                string SGSTTax = lblSGST.Text;

                if (SGSTTax.Trim().Length == 0)
                {
                    SGSTTax = "0";
                }


                string IGSTTax = lblIGST.Text;

                if (IGSTTax.Trim().Length == 0)
                {
                    IGSTTax = "0";
                }


                #endregion for GST as on 17-6-2017 by swathi



                string ServiceChargePer = TxtservicechrgPrc.Text;
                if (ServiceChargePer.Trim().Length == 0)
                {
                    ServiceChargePer = "0";
                }

                string SubTotal = TxtTotalbeforeTax.Text;
                if (SubTotal.Trim().Length == 0)
                {
                    SubTotal = "0";
                }

                string Servicechrg = lblServiceCharges.Text;

                if (Servicechrg.Trim().Length == 0)
                {
                    Servicechrg = "0";
                }

                string DutiesTotalAmount = txtResources.Text;
                if (DutiesTotalAmount.Trim().Length == 0)
                {
                    DutiesTotalAmount = "0";
                }


                System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
                DateTime dtb = Convert.ToDateTime(txtinvoicedate.Text, enGB);
                string billdate = dtb.ToString("yyyy-MM-dd hh:mm:ss");
                if (txtfrommonth.Text.Trim().Length == 0)
                {
                    txtfrommonth.Text = "01 / 01 / 1900";
                }

                if (txttoDate.Text.Trim().Length == 0)
                {
                    txttoDate.Text = "01 / 01 / 1900";
                }
                DateTime dtf = Convert.ToDateTime(txtfrommonth.Text, enGB);
                string tfrom = dtf.ToString("yyyy-MM-dd hh:mm:ss");
                DateTime dtt = Convert.ToDateTime(txttoDate.Text, enGB);
                string tto = dtt.ToString("yyyy-MM-dd hh:mm:ss");

                #region Client detils saving in manualbill

                var OURGSTNo = txtgstno.Text;
                var GSTAddress = txtGSTAddress.Text;
                int MonthNew = 0;
                #endregion


                string TotalServiceTax = float.Parse((CGSTTax) + float.Parse(SGSTTax) + float.Parse(IGSTTax)).ToString();


                DateTime Created_On = DateTime.Parse(DateTime.Now.ToString());
                string Created_By = UserID;

                string InsertQueryForUnitBill = "insert into OneTimeBill(billno,billdt,unitid,fromdt,todt,dutiestotalamount," +
                                               "grandtotal,Servicechrg,ServiceChrgPrc," +
                                               " month,ClientName,GSTType, " +
                                               "Created_By,Timings," +
                                               " CGSTAmt ,SGSTAmt , IGSTAmt , CGSTPrc , SGSTPrc ,IGSTPrc ,TaxAmount," +
                                               "TotalbeforeTax ,OURGSTNo,GSTAddress,MonthNew,CGST,SGST,IGST) values('"
                                               + txtinvoiceno.Text + "','"
                                               + billdate + "','"
                                               + txtnewclientid.Text + "','"
                                               + tfrom + "','"
                                               + tto + "','"
                                               + DutiesTotalAmount + "','"
                                               + grandtotal + "','"
                                               + lblServiceCharges.Text + "','"
                                               + ServiceChargePer + "','" + month + "','" + txtnewclientname.Text + "','"
                                               + GSTType + "','"
                                              + Created_By + "','" + Created_On + "','"
                                              + CGSTTax + "','" + SGSTTax + "','" + IGSTTax + "','" + CGSTprc + "','"
                                              + SGSTprc + "','" + IGSTprc + "','" + TotalServiceTax + "','"
                                              + SubTotal + "','" + OURGSTNo + "','"
                                              + GSTAddress + "','" + MonthNew + "','" + CGST + "','" + SGST + "','" + IGST + "')";
                int insrt = config.ExecuteNonQueryWithQueryAsync(InsertQueryForUnitBill).Result;

                #endregion
                ClearData();

                #endregion
            }

            catch (Exception ex)
            {

            }
        }

        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";


            #region  Month Get Based on the Control Selection
            int month = 0;

            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            month = Timings.Instance.GetIdForEnteredMOnth(date);

            return month;

            #endregion
        }

        public void InvoiceNoAuto()
        {
            int invInvoiceNo = 1;

            string billSeq = GlobalData.Instance.GetBillSequence();

            if (rdbwithGST.Checked == true)
            {
                string selectqueryclientid = "select isnull(max(right(BillNo,4)),0) as invInvoiceNo from OneTimeBill where BillNo like '" + InvBillPrefixWithST + "/" + billSeq + "/" + "%'";
                dt = SqlHelper.Instance.GetTableByQuery(selectqueryclientid);
                string invPrefix = string.Empty;
            }
            else
            {
                string selectqueryclientid = "select isnull(max(right(BillNo,4)),0) as invInvoiceNo from OneTimeBill where BillNo like '" + InvBillPrefixwithoutST + "/" + billSeq + "/" + "%'";
                dt = SqlHelper.Instance.GetTableByQuery(selectqueryclientid);
                string invPrefix = string.Empty;
            }

            if (dt.Rows.Count > 0)
            {

                if (String.IsNullOrEmpty(dt.Rows[0]["invInvoiceNo"].ToString()) == false)
                {
                    invInvoiceNo = Convert.ToInt32(dt.Rows[0]["invInvoiceNo"].ToString()) + 1;
                }
                else
                {
                    invInvoiceNo = int.Parse("1");
                }
            }

            txtinvoiceno.Text = InvBillPrefixWithST + "/" + billSeq + "/" + (invInvoiceNo).ToString("00000");

        }

        protected void btninvoiceDownload_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document();

            document = new Document(PageSize.A4, 0, 0, 0, 0);


            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            string filename = "";
            string CopyName = "";
            document.Open();

            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            string SelectBillNo = string.Empty;
            string DisplayBillNo = "";

            SelectBillNo = "Select max(BillNo) as DisplayBillNo from OneTimeBill";

            DataTable DtBilling = config.ExecuteReaderWithQueryAsync(SelectBillNo).Result;

            if (DtBilling.Rows.Count > 0)
            {
                DisplayBillNo = DtBilling.Rows[0]["DisplayBillNo"].ToString();
            }


            DownloadBill(document, ms);

            filename = DisplayBillNo + "/" + "/Invoice.pdf";

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
        }

        public void DownloadBill(Document document, MemoryStream ms)
        {

            int month = 0;
            int font = 10;

            if (gvClientBilling.Rows.Count > 0)
            {
                try
                {

                    //for (m = 0; m < Copycount; m++)
                    {
                        document.NewPage();
                        string CopyName = "";

                        PdfPCell cell;
                        #region for PDf
                        Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);

                        PdfWriter writer = PdfWriter.GetInstance(document, ms);

                        document.Open();
                        BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                        #region for CompanyInfo
                        string strQry = "select * from companyinfo where BranchID='" + Session["Branch"].ToString() + "' ";
                        DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                        string companyName = "Your Company Name";
                        string companyAddress = "Your Company Address";
                        string companyaddressline = " ";
                        string emailid = "";
                        string website = "";
                        string phoneno = "";
                        string PANNO = "";
                        string PFNo = "";
                        string Esino = "";
                        string CmpPFNo = "";
                        string CmpEsino = "";
                        string Servicetax = "";
                        string notes = "";
                        string ServiceText = "";
                        string PSARARegNo = "";
                        string Category = "";
                        string HSNNumber = "";
                        string SACCode = "";
                        string BillDesc = "";
                        string BankName = "";
                        string BankAcNumber = "";
                        string IFSCCode = "";
                        string BranchName = "";
                        string CINNo = "";
                        string MSMENo = "";
                        string BillSeq = "";
                        if (compInfo.Rows.Count > 0)
                        {
                            companyName = compInfo.Rows[0]["CompanyName"].ToString();
                            companyAddress = compInfo.Rows[0]["Address"].ToString();
                            //companyAddress = companyAddress.Replace("\r\n", string.Empty);
                            companyaddressline = compInfo.Rows[0]["Addresslineone"].ToString();
                            //CINNO = compInfo.Rows[0]["CINNO"].ToString();
                            PANNO = compInfo.Rows[0]["Labourrule"].ToString();
                            CmpPFNo = compInfo.Rows[0]["PFNo"].ToString();
                            Category = compInfo.Rows[0]["Category"].ToString();
                            CmpEsino = compInfo.Rows[0]["ESINo"].ToString();
                            Servicetax = compInfo.Rows[0]["BillNotes"].ToString();
                            emailid = compInfo.Rows[0]["Emailid"].ToString();
                            website = compInfo.Rows[0]["Website"].ToString();
                            phoneno = compInfo.Rows[0]["Phoneno"].ToString();
                            notes = compInfo.Rows[0]["notes"].ToString();
                            HSNNumber = compInfo.Rows[0]["HSNNumber"].ToString();
                            SACCode = compInfo.Rows[0]["SACCode"].ToString();
                            BillDesc = compInfo.Rows[0]["BillDesc"].ToString();
                            BankName = compInfo.Rows[0]["Bankname"].ToString();
                            BranchName = compInfo.Rows[0]["BranchName"].ToString();
                            BankAcNumber = compInfo.Rows[0]["bankaccountno"].ToString();
                            IFSCCode = compInfo.Rows[0]["IfscCode"].ToString();
                            CINNo = compInfo.Rows[0]["CINNo"].ToString();
                            MSMENo = compInfo.Rows[0]["MSMENo"].ToString();
                            BillSeq = compInfo.Rows[0]["BillSeq"].ToString();
                        }

                        #endregion


                        #region

                        string SelectBillNo = string.Empty;

                        SelectBillNo = "Select max(BillNo) as DisplayBillNo from OneTimeBill";

                        DataTable DtBilling = config.ExecuteAdaptorAsyncWithQueryParams(SelectBillNo).Result;
                        string BillNo = "";
                        string DisplayBillNo = "";
                        string GSTNo = "";
                        string GSTAddress = "";
                        string ClientID = "";
                        string ClientName = "";
                        string MonthName = "";
                        string BillDate = "";
                        string FromDate = "";
                        string ToDate = "";
                        DateTime BillDtfortbloptions = DateTime.Now;
                        if (DtBilling.Rows.Count > 0)
                        {
                            DisplayBillNo = DtBilling.Rows[0]["DisplayBillNo"].ToString();
                        }

                        #region Variables for data Fields as on 11/03/2014 by venkat


                        decimal servicecharge = 0;

                        string ServiceChargePer = "0";

                        #region for GST on 17-6-2017 by swathi

                        decimal CGST = 0;
                        decimal SGST = 0;
                        decimal IGST = 0;
                        decimal CGSTPrc = 0;
                        decimal SGSTPrc = 0;
                        decimal IGSTPrc = 0;


                        #endregion for GST on 17-6-2017 by swathi

                        decimal totalamount = 0;
                        decimal TotalbeforeTax = 0;
                        decimal Grandtotal = 0;

                        #endregion

                        string spUnitbillbreakup = "GetOnetimeInvoiceBillData";
                        Hashtable htunitbillbreakup = new Hashtable();
                        htunitbillbreakup.Add("@BIllNo", DisplayBillNo);
                        htunitbillbreakup.Add("@Option", 0);
                        DataTable dtunitbillbreakup = config.ExecuteAdaptorAsyncWithParams(spUnitbillbreakup, htunitbillbreakup).Result;

                        if (dtunitbillbreakup.Rows.Count > 0)
                        {
                            ClientID = dtunitbillbreakup.Rows[0]["UnitId"].ToString();
                            BillDate = dtunitbillbreakup.Rows[0]["BillDate"].ToString();
                            ClientName = dtunitbillbreakup.Rows[0]["ClientName"].ToString();
                            FromDate = dtunitbillbreakup.Rows[0]["FromDate"].ToString();
                            ToDate = dtunitbillbreakup.Rows[0]["ToDate"].ToString();
                            GSTNo = dtunitbillbreakup.Rows[0]["OURGSTNo"].ToString();
                            GSTAddress = dtunitbillbreakup.Rows[0]["GSTAddress"].ToString();
                            MonthName = dtunitbillbreakup.Rows[0]["Monthname"].ToString();
                            BillDtfortbloptions = Convert.ToDateTime(dtunitbillbreakup.Rows[0]["BillDt"]);
                        }

                        string BQry = "select * from TblOptions  where '" + BillDtfortbloptions + "' between fromdate and todate";
                        DataTable Bdt = config.ExecuteAdaptorAsyncWithQueryParams(BQry).Result;

                        string CGSTAlias = "";
                        string SGSTAlias = "";
                        string IGSTAlias = "";
                        string GSTINAlias = "";
                        string OurGSTINAlias = "";

                        string SqlQryForTaxes = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2,CGSTAlias,SGSTAlias,IGSTAlias,cess1Alias,cess2Alias,GSTINAlias,OurGSTINAlias from TblOptions where '" + BillDtfortbloptions + "' between fromdate and todate ";
                        DataTable DtTaxes = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForTaxes).Result;

                        if (DtTaxes.Rows.Count > 0)
                        {
                            CGSTAlias = DtTaxes.Rows[0]["CGSTAlias"].ToString();
                            SGSTAlias = DtTaxes.Rows[0]["SGSTAlias"].ToString();
                            IGSTAlias = DtTaxes.Rows[0]["IGSTAlias"].ToString();
                            GSTINAlias = DtTaxes.Rows[0]["GSTINAlias"].ToString();
                            OurGSTINAlias = DtTaxes.Rows[0]["OurGSTINAlias"].ToString();
                        }

                        if (dtunitbillbreakup.Rows.Count > 0)
                        {

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["ServiceChrg"].ToString()) == false)
                            {
                                servicecharge = decimal.Parse(dtunitbillbreakup.Rows[0]["ServiceChrg"].ToString());
                            }

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["ServiceChrgPrc"].ToString()) == false)
                            {
                                ServiceChargePer = dtunitbillbreakup.Rows[0]["ServiceChrgPrc"].ToString();
                            }


                            #region Begin New code for values taken from database as on 11/03/2014 by venkat

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["dutiestotalamount"].ToString()) == false)
                            {
                                totalamount = decimal.Parse(dtunitbillbreakup.Rows[0]["dutiestotalamount"].ToString());
                            }

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["TotalbeforeTax"].ToString()) == false)
                            {
                                TotalbeforeTax = decimal.Parse(dtunitbillbreakup.Rows[0]["TotalbeforeTax"].ToString());
                            }

                            #region for GST as on 17-6-2017 by swathi

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["CGSTAmt"].ToString()) == false)
                            {
                                CGST = decimal.Parse(dtunitbillbreakup.Rows[0]["CGSTAmt"].ToString());
                            }

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["SGSTAmt"].ToString()) == false)
                            {
                                SGST = decimal.Parse(dtunitbillbreakup.Rows[0]["SGSTAmt"].ToString());
                            }

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["IGSTAmt"].ToString()) == false)
                            {
                                IGST = decimal.Parse(dtunitbillbreakup.Rows[0]["IGSTAmt"].ToString());
                            }


                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["CGSTPrc"].ToString()) == false)
                            {
                                CGSTPrc = decimal.Parse(dtunitbillbreakup.Rows[0]["CGSTPrc"].ToString());
                            }

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["SGSTPrc"].ToString()) == false)
                            {
                                SGSTPrc = decimal.Parse(dtunitbillbreakup.Rows[0]["SGSTPrc"].ToString());
                            }

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["IGSTPrc"].ToString()) == false)
                            {
                                IGSTPrc = decimal.Parse(dtunitbillbreakup.Rows[0]["IGSTPrc"].ToString());
                            }

                            #endregion for GST as on 17-6-2017 by swathi

                            if (String.IsNullOrEmpty(dtunitbillbreakup.Rows[0]["GrandTotal"].ToString()) == false)
                            {
                                Grandtotal = decimal.Parse(dtunitbillbreakup.Rows[0]["GrandTotal"].ToString());
                            }

                            #endregion
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                            return;
                        }
                        string Year = DateTime.Now.Year.ToString();
                        #endregion

                        document.AddTitle(companyName);
                        document.AddAuthor("DIYOS");
                        document.AddSubject("Invoice");
                        document.AddKeywords("Keyword1, keyword2, …");
                        string imagepath = Server.MapPath("~/assets/" + CmpIDPrefix + "BillLogo.png");
                        //if (CmpIDPrefix == "STD")
                        //{

                        //}

                        //PdfContentByte content = writer.DirectContent;
                        PdfPTable tablelogo = new PdfPTable(2);
                        tablelogo.TotalWidth = 500f;
                        tablelogo.LockedWidth = true;
                        float[] widtlogo = new float[] { 0.4f, 2f };
                        tablelogo.SetWidths(widtlogo);
                        //if (chkletterhead.Checked == true)
                        //{
                        //    PdfPCell CCompName = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 18, Font.BOLD, BaseColor.BLACK)));
                        //    CCompName.HorizontalAlignment = 1;
                        //    CCompName.Border = 0;
                        //    CCompName.Colspan = 2;
                        //    // CCompName.PaddingTop =50;
                        //    tablelogo.AddCell(CCompName);

                        //    PdfPCell CCompAddress = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 11, Font.BOLD, BaseColor.BLACK)));
                        //    CCompAddress.HorizontalAlignment = 1;
                        //    CCompAddress.Border = 0;
                        //    CCompAddress.Colspan = 2;
                        //    // space between lines
                        //    CCompAddress.SetLeading(0f, 1.3f);
                        //    tablelogo.AddCell(CCompAddress);

                        //    PdfPCell cellline = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                        //    cellline.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        //    cellline.Border = 0;
                        //    cellline.Colspan = 2;
                        //    tablelogo.AddCell(cellline);
                        //    //For Space

                        //    PdfPCell celll = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                        //    celll.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        //    celll.Border = 0;
                        //    celll.Colspan = 2;
                        //    celll.PaddingTop = 25f;
                        //    tablelogo.AddCell(celll);
                        //    tablelogo.AddCell(celll);
                        //    document.Add(tablelogo);
                        //}
                        //else
                        {
                            if (File.Exists(imagepath))
                            {
                                iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);
                                gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                                gif2.ScalePercent(55f);
                                gif2.SetAbsolutePosition(50f, 760f); //745
                                document.Add(gif2);
                            }

                            //tablelogo.AddCell(celll);


                            var FontColour = new BaseColor(178, 34, 34);
                            Font FontStyle1 = FontFactory.GetFont("Belwe-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 30, Font.NORMAL, FontColour);

                            //if (CmpIDPrefix == "STD")
                            {
                                PdfPCell CCompName1 = new PdfPCell(new Paragraph("" + companyName, FontFactory.GetFont(FontStyle, 15, Font.BOLD, BaseColor.BLACK)));
                                CCompName1.HorizontalAlignment = 0;
                                CCompName1.Colspan = 2;
                                CCompName1.PaddingLeft = 120f;
                                CCompName1.PaddingTop = 10f;
                                CCompName1.Border = 0;
                                tablelogo.AddCell(CCompName1);

                                PdfPCell CCompName = new PdfPCell(new Paragraph(companyAddress, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                CCompName.HorizontalAlignment = 0;
                                CCompName.Colspan = 2;
                                CCompName.Border = 0;
                                //CCompName.PaddingTop = 5f;
                                CCompName.PaddingLeft = 120;
                                CCompName.SetLeading(0, 1.2f);
                                tablelogo.AddCell(CCompName);
                            }

                            if (emailid.Length > 0)
                            {
                                PdfPCell CCompName2 = new PdfPCell(new Paragraph("Website :" + website + " | Email :" + emailid, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                CCompName2.HorizontalAlignment = 0;
                                CCompName2.Colspan = 2;
                                CCompName2.Border = 0;
                                //CCompName2.PaddingTop = 4f;
                                CCompName2.PaddingLeft = 120;
                                tablelogo.AddCell(CCompName2);
                            }
                            if (phoneno.Length > 0)
                            {
                                PdfPCell CCompName2 = new PdfPCell(new Paragraph("Phone :" + phoneno, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                CCompName2.HorizontalAlignment = 0;
                                CCompName2.Colspan = 2;
                                CCompName2.Border = 0;
                                CCompName2.PaddingBottom = 5;
                                CCompName2.PaddingLeft = 120;
                                tablelogo.AddCell(CCompName2);
                            }

                            document.Add(tablelogo);
                        }

                        PdfPTable address = new PdfPTable(5);
                        address.TotalWidth = 500f;
                        address.LockedWidth = true;
                        float[] addreslogo = new float[] { 2f, 2f, 2f, 2f, 2f };
                        address.SetWidths(addreslogo);

                        PdfPCell Celemail = new PdfPCell(new Paragraph("TAX INVOICE", FontFactory.GetFont(FontStyle, 13, Font.BOLD, BaseColor.BLACK)));
                        Celemail.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        Celemail.Colspan = 5;
                        Celemail.FixedHeight = 20;
                        Celemail.BorderWidthTop = .2f;
                        Celemail.BorderWidthBottom = .2f;
                        Celemail.BorderWidthLeft = .2f;
                        Celemail.BorderWidthRight = .2f;
                        Celemail.BorderColor = BaseColor.BLACK;
                        address.AddCell(Celemail);

                        PdfPTable tempTable1 = new PdfPTable(3);
                        tempTable1.TotalWidth = 300f;
                        tempTable1.LockedWidth = true;
                        float[] tempWidth1 = new float[] { 0.8f, 2f, 2f };
                        tempTable1.SetWidths(tempWidth1);

                        string addressData = "";

                        PdfPCell clientaddrhno1 = new PdfPCell(new Paragraph("Billing Address", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        clientaddrhno1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientaddrhno1.Colspan = 3;                                 //clientaddrhno.Colspan = 0;
                        clientaddrhno1.BorderWidthBottom = 0;
                        clientaddrhno1.BorderWidthTop = 0;
                        clientaddrhno1.BorderWidthLeft = .2f;
                        clientaddrhno1.BorderWidthRight = 0.2f;
                        clientaddrhno1.BorderColor = BaseColor.BLACK;
                        //clientaddrhno.clientaddrhno = 20;
                        tempTable1.AddCell(clientaddrhno1);
                        addressData = ClientName;
                        if (addressData.Trim().Length > 0)
                        {

                            PdfPCell clientaddrhno = new PdfPCell(new Paragraph("M/s. " + addressData, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            clientaddrhno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientaddrhno.Colspan = 3;                                 //clientaddrhno.Colspan = 0;
                            clientaddrhno.BorderWidthBottom = 0;
                            clientaddrhno.BorderWidthTop = 0;
                            clientaddrhno.BorderWidthLeft = .2f;
                            clientaddrhno.BorderWidthRight = 0.2f;
                            clientaddrhno.BorderColor = BaseColor.BLACK;
                            //clientaddrhno.clientaddrhno = 20;
                            tempTable1.AddCell(clientaddrhno);
                        }

                        if (GSTNo.Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph("GSTIN ", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 1;
                            clietnpin.Border = 0;
                            clietnpin.PaddingTop = 4f;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = .2f;
                            clietnpin.BorderWidthRight = 0;
                            //clietnpin.BorderColor = BaseColor.BLACK;
                            // clietnpin.PaddingLeft = 120;
                            tempTable1.AddCell(clietnpin);

                            clietnpin = new PdfPCell(new Paragraph(" : " + GSTNo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 2;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = 0;
                            clietnpin.BorderWidthRight = 0.2f;
                            clietnpin.BorderColor = BaseColor.BLACK;
                            //  clietnpin.PaddingLeft = 20;
                            tempTable1.AddCell(clietnpin);

                        }

                        #region copy
                        PdfPCell childTable1 = new PdfPCell(tempTable1);
                        childTable1.Border = 0;
                        childTable1.Colspan = 3;
                        // childTable1.FixedHeight = 100;
                        childTable1.HorizontalAlignment = 0;

                        address.AddCell(childTable1);

                        PdfPTable tempTable2 = new PdfPTable(2);
                        tempTable2.TotalWidth = 200f;
                        tempTable2.LockedWidth = true;
                        float[] tempWidth2 = new float[] { 0.8f, 1.2f };
                        tempTable2.SetWidths(tempWidth2);



                        var phrase = new Phrase();
                        phrase.Add(new Chunk("Invoice No", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        PdfPCell cell13 = new PdfPCell();
                        cell13.AddElement(phrase);
                        cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell13.BorderWidthBottom = 0;
                        cell13.BorderWidthTop = 0;
                        //cell13.FixedHeight = 35;
                        cell13.Colspan = 1;
                        cell13.BorderWidthLeft = 0f;
                        cell13.BorderWidthRight = 0f;
                        cell13.PaddingTop = -5;
                        cell13.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell13);

                        var phrase10 = new Phrase();
                        phrase10.Add(new Chunk(": " + DisplayBillNo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        PdfPCell cell13v = new PdfPCell();
                        cell13v.AddElement(phrase10);
                        cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell13v.BorderWidthBottom = 0;
                        cell13v.BorderWidthTop = 0;
                        //cell13.FixedHeight = 35;
                        cell13v.Colspan = 1;
                        cell13v.BorderWidthLeft = 0;
                        cell13v.BorderWidthRight = .2f;
                        cell13v.PaddingTop = -5;
                        cell13v.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell13v);

                        var phrase11 = new Phrase();
                        phrase11.Add(new Chunk("Invoice Date", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        PdfPCell cell131 = new PdfPCell();
                        cell131.AddElement(phrase11);
                        cell131.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell131.BorderWidthBottom = 0;
                        cell131.BorderWidthTop = 0;
                        // cell131.FixedHeight = 35;
                        cell131.Colspan = 1;
                        cell131.BorderWidthLeft = 0f;
                        cell131.BorderWidthRight = 0f;
                        cell131.PaddingTop = -5;
                        cell131.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell131);

                        var phrase11v = new Phrase();
                        phrase11v.Add(new Chunk(": " + BillDate, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        PdfPCell cell131v = new PdfPCell();
                        cell131v.AddElement(phrase11v);
                        cell131v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell131v.BorderWidthBottom = 0;
                        cell131v.BorderWidthTop = 0;
                        // cell131.FixedHeight = 35;
                        cell131v.Colspan = 1;
                        cell131v.BorderWidthLeft = 0;
                        cell131v.BorderWidthRight = .2f;
                        cell131v.PaddingTop = -5;
                        cell131v.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell131v);


                        var phraseim = new Phrase();
                        phraseim.Add(new Chunk("Invoice Month", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell13 = new PdfPCell();
                        cell13.AddElement(phraseim);
                        cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell13.BorderWidthBottom = 0;
                        cell13.BorderWidthTop = 0;
                        //cell13.FixedHeight = 35;
                        cell13.Colspan = 1;
                        cell13.BorderWidthLeft = 0f;
                        cell13.BorderWidthRight = 0f;
                        cell13.PaddingTop = -5;
                        cell13.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell13);

                        var phrase10im = new Phrase();
                        phrase10im.Add(new Chunk(": " + MonthName, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        cell13v = new PdfPCell();
                        cell13v.AddElement(phrase10im);
                        cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell13v.BorderWidthBottom = 0;
                        cell13v.BorderWidthTop = 0;
                        //cell13.FixedHeight = 35;
                        cell13v.Colspan = 1;
                        cell13v.BorderWidthLeft = 0;
                        cell13v.BorderWidthRight = .2f;
                        cell13v.PaddingTop = -5;
                        cell13v.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell13v);


                        var phraseperiod = new Phrase();
                        phraseperiod.Add(new Chunk("Invoice Period", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell13 = new PdfPCell();
                        cell13.AddElement(phraseperiod);
                        cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell13.BorderWidthBottom = 0;
                        cell13.BorderWidthTop = 0;
                        //cell13.FixedHeight = 35;
                        cell13.Colspan = 1;
                        cell13.BorderWidthLeft = 0f;
                        cell13.BorderWidthRight = 0f;
                        cell13.PaddingTop = -5;
                        cell13.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell13);


                        var phrase10p = new Phrase();
                        phrase10p.Add(new Chunk(": " + FromDate + " to " + ToDate, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        cell13v = new PdfPCell();
                        cell13v.AddElement(phrase10p);
                        cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell13v.BorderWidthBottom = 0;
                        cell13v.BorderWidthTop = 0;
                        cell13v.Colspan = 1;
                        cell13v.BorderWidthLeft = 0;
                        cell13v.BorderWidthRight = .2f;
                        cell13v.PaddingTop = -5;
                        cell13v.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell13v);

                        var phraseperiod1 = new Phrase();
                        phraseperiod1.Add(new Chunk("Contract Year", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell13 = new PdfPCell();
                        cell13.AddElement(phraseperiod1);
                        cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell13.BorderWidthBottom = 0;
                        cell13.BorderWidthTop = 0;
                        //cell13.FixedHeight = 35;
                        cell13.Colspan = 1;
                        cell13.BorderWidthLeft = 0f;
                        cell13.BorderWidthRight = 0f;
                        cell13.PaddingTop = -5;
                        cell13.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell13);



                        var phrase10p1 = new Phrase();
                        phrase10p1.Add(new Chunk(": " + BillSeq, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        cell13v = new PdfPCell();
                        cell13v.AddElement(phrase10p1);
                        cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell13v.BorderWidthBottom = 0;
                        cell13v.BorderWidthTop = 0;
                        cell13v.Colspan = 1;
                        cell13v.BorderWidthLeft = 0;
                        cell13v.BorderWidthRight = .2f;
                        cell13v.PaddingTop = -5;
                        cell13v.BorderColor = BaseColor.BLACK;
                        tempTable2.AddCell(cell13v);


                        PdfPCell childTable2 = new PdfPCell(tempTable2);
                        childTable2.Border = 0;
                        childTable2.Colspan = 2;
                        //childTable2.FixedHeight = 100;
                        childTable2.HorizontalAlignment = 0;
                        address.AddCell(childTable2);
                        // address.AddCell(celll);
                        document.Add(address);

                        #endregion


                        #region                                               
                        int colCount = 8;




                        PdfPTable table = new PdfPTable(colCount);
                        table.TotalWidth = 500f;
                        table.LockedWidth = true;
                        table.HorizontalAlignment = 1;
                        //float[] colWidths = new float[] { 1f, 6.4f, 2f, 2.2f, 2f, 2.7f };



                        float[] colWidths = new float[] { };
                        if (colCount == 8)
                        {
                            colWidths = new float[] { 1.5f, 5f, 2f, 2f, 2f, 2.2f, 2f, 2.7f };
                        }


                        table.SetWidths(colWidths);

                        cell = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = .2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.Colspan = 1;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);


                        cell = new PdfPCell(new Phrase("Description", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        //cell.Colspan = 1;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("SAC Code", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        //cell.Colspan = 1;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("No Of Days in a Month", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        //cell.Colspan = 1;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("No.of Emps", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        //cell.Colspan = 1;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("No Of Duties", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = .2f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        string spUnitbillbreakup1 = "GetInvoiceBillData";
                        Hashtable htunitbillbreakup1 = new Hashtable();
                        htunitbillbreakup1.Add("@BIllNo", DisplayBillNo);
                        htunitbillbreakup1.Add("@Option", 1);
                        DataTable dtunitbillbreakup1 = config.ExecuteAdaptorAsyncWithParams(spUnitbillbreakup1, htunitbillbreakup1).Result;
                        string SNo = "";
                        string Description = "";
                        string HSNCode = "";
                        string NoOfDaysInaMonth = "";
                        string NoOfEmps = "";
                        string PayRate = "";
                        string NoOfDuties = "";
                        decimal Amount = 0;

                        if (dtunitbillbreakup1.Rows.Count > 0)
                        {

                            for (int i = 0; i < dtunitbillbreakup1.Rows.Count; i++)
                            {
                                SNo = dtunitbillbreakup1.Rows[0]["SNo"].ToString();
                                Description = dtunitbillbreakup1.Rows[0]["Designation"].ToString();
                                HSNCode = dtunitbillbreakup1.Rows[0]["HSNNumber"].ToString();
                                NoOfDaysInaMonth = dtunitbillbreakup1.Rows[0]["noofdays"].ToString();
                                NoOfEmps = dtunitbillbreakup1.Rows[0]["NoofEmps"].ToString();
                                PayRate = dtunitbillbreakup1.Rows[0]["PayRate"].ToString();
                                NoOfDuties = dtunitbillbreakup1.Rows[0]["DutyHours"].ToString();
                                Amount = decimal.Parse(dtunitbillbreakup1.Rows[0]["BasicDA"].ToString());

                                #region
                                cell = new PdfPCell(new Phrase((SNo).ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.BorderWidthRight = 0f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthTop = 0;
                                cell.HorizontalAlignment = 1;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(Description, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0;
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthRight = 0f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(HSNCode, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0;
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthRight = 0f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = new PdfPCell(new Phrase(NoOfDaysInaMonth.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0f;
                                cell.HorizontalAlignment = 1;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(NoOfEmps.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                //cell.Border = 0;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0f;
                                cell.HorizontalAlignment = 2;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(PayRate.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);



                                cell = new PdfPCell(new Phrase(NoOfDuties.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 2;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(Amount.ToString("0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 2;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.2f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                #endregion
                            }
                        }

                        document.Add(table);

                        // tablelogo.AddCell(celll);


                        PdfPTable tempTable22 = new PdfPTable(colCount);
                        tempTable22.TotalWidth = 500f;
                        tempTable22.LockedWidth = true;
                        float[] tempWidth22 = new float[] { };
                        if (colCount == 8)
                        {
                            tempWidth22 = new float[] { 1f, 6f, 2f, 2f, 2f, 2.2f, 2f, 2.7f };
                        }

                        tempTable22.SetWidths(tempWidth22);

                        #region                      

                        PdfPTable Childterms11 = new PdfPTable(4);
                        Childterms11.TotalWidth = 275f;
                        Childterms11.LockedWidth = true;
                        float[] Celters11 = new float[] { 1f, 6f, 2f, 2f };
                        Childterms11.SetWidths(Celters11);

                        PdfPCell celldz1 = new PdfPCell(new Phrase(BillDesc, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        celldz1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        celldz1.Colspan = 4;
                        celldz1.BorderWidthBottom = 0;
                        celldz1.BorderWidthLeft = .2f;
                        celldz1.BorderWidthTop = 0.2f;
                        celldz1.BorderWidthRight = 0.2f;
                        celldz1.BorderColor = BaseColor.BLACK;
                        Childterms11.AddCell(celldz1);

                        PdfPCell Chid31 = new PdfPCell(Childterms11);
                        Chid31.Border = 0;
                        Chid31.Colspan = 4;
                        Chid31.HorizontalAlignment = 0;
                        tempTable22.AddCell(Chid31);


                        PdfPTable Childterms112 = new PdfPTable(4);
                        Childterms112.TotalWidth = 225f;
                        Childterms112.LockedWidth = true;
                        float[] Celters112 = new float[] { 2f, 2.2f, 2f, 2.7f };
                        Childterms112.SetWidths(Celters112);

                        celldz1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldz1.Colspan = 3;
                        celldz1.BorderWidthBottom = 0;
                        celldz1.BorderWidthLeft = .2f;
                        celldz1.BorderWidthTop = .2f;
                        celldz1.BorderWidthRight = 0;
                        celldz1.BorderColor = BaseColor.BLACK;
                        Childterms112.AddCell(celldz1);

                        PdfPCell celldz4 = new PdfPCell(new Phrase(" " + (totalamount).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        celldz4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldz4.BorderWidthBottom = 0;
                        celldz4.BorderWidthLeft = 0.2f;
                        celldz4.BorderWidthTop = .2f;
                        celldz4.BorderWidthRight = .2f;
                        celldz4.BorderColor = BaseColor.BLACK;
                        Childterms112.AddCell(celldz4);


                        #region for Service Charge                                                                     

                        if (servicecharge > 0)//bSCType == true)
                        {
                            decimal scharge = servicecharge;
                            if (scharge > 0)
                            {

                                PdfPCell Cellservice = new PdfPCell(new Phrase("Service Charges @ " + ServiceChargePer, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cellservice.Colspan = 3;
                                Cellservice.BorderWidthBottom = 0;
                                Cellservice.BorderWidthLeft = .2f;
                                Cellservice.BorderWidthTop = 0;
                                Cellservice.BorderWidthRight = 0f;
                                Cellservice.BorderColor = BaseColor.BLACK;
                                Childterms112.AddCell(Cellservice);

                                Cellservice = new PdfPCell(new Phrase(servicecharge.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cellservice.BorderWidthBottom = 0;
                                Cellservice.BorderWidthLeft = 0.2f;
                                Cellservice.BorderWidthTop = 0;
                                Cellservice.BorderWidthRight = .2f;
                                Cellservice.BorderColor = BaseColor.BLACK;
                                Childterms112.AddCell(Cellservice);
                            }
                        }
                        #endregion

                        #endregion

                        if (TotalbeforeTax > 0)
                        {
                            PdfPCell

                            CellbbCGST = new PdfPCell(new Phrase("Total Before Tax", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellbbCGST.Colspan = 3;
                            CellbbCGST.BorderWidthBottom = 0;
                            CellbbCGST.BorderWidthLeft = .2f;
                            CellbbCGST.BorderWidthTop = 0.2f;
                            CellbbCGST.BorderWidthRight = 0f;
                            CellbbCGST.BorderColor = BaseColor.BLACK;
                            Childterms112.AddCell(CellbbCGST);

                            CellbbCGST = new PdfPCell(new Phrase((TotalbeforeTax).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellbbCGST.BorderWidthBottom = 0;
                            CellbbCGST.BorderWidthLeft = 0.2f;
                            CellbbCGST.BorderWidthTop = 0.2f;
                            CellbbCGST.BorderWidthRight = .2f;
                            CellbbCGST.BorderColor = BaseColor.BLACK;
                            Childterms112.AddCell(CellbbCGST);
                        }



                        #region for taxes

                        #region for GST as on 17-6-2017

                        if (CGST > 0)
                        {
                            PdfPCell

                            CellCGST = new PdfPCell(new Phrase(CGSTAlias + " @ " + CGSTPrc + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGST.Colspan = 3;
                            CellCGST.BorderWidthBottom = 0;
                            CellCGST.BorderWidthLeft = .2f;
                            CellCGST.BorderWidthTop = 0.2f;
                            CellCGST.BorderWidthRight = 0f;
                            // CellCGST.PaddingBottom = 5;
                            // CellCGST.PaddingTop = 5;
                            CellCGST.BorderColor = BaseColor.BLACK;
                            Childterms112.AddCell(CellCGST);

                            PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(CGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGSTAmt.BorderWidthBottom = 0;
                            CellCGSTAmt.BorderWidthLeft = 0.2f;
                            CellCGSTAmt.BorderWidthTop = 0.2f;
                            CellCGSTAmt.BorderWidthRight = .2f;
                            CellCGSTAmt.BorderColor = BaseColor.BLACK;
                            //CellCGSTAmt.PaddingBottom = 5;
                            //CellCGSTAmt.PaddingTop = 5;
                            Childterms112.AddCell(CellCGSTAmt);

                        }


                        if (SGST > 0)
                        {
                            PdfPCell

                            CellSGST = new PdfPCell(new Phrase(SGSTAlias + " @ " + SGSTPrc + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                            CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellSGST.Colspan = 3;
                            CellSGST.BorderWidthBottom = 0;
                            CellSGST.BorderWidthLeft = .2f;
                            CellSGST.BorderWidthTop = 0.2f;
                            CellSGST.BorderWidthRight = 0f;
                            // CellSGST.PaddingBottom = 5;
                            // CellSGST.PaddingTop = 5;
                            CellSGST.BorderColor = BaseColor.BLACK;
                            Childterms112.AddCell(CellSGST);

                            PdfPCell CellSGSTAmt = new PdfPCell(new Phrase(SGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellSGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellSGSTAmt.BorderWidthBottom = 0;
                            CellSGSTAmt.BorderWidthLeft = 0.2f;
                            CellSGSTAmt.BorderWidthTop = 0.2f;
                            CellSGSTAmt.BorderWidthRight = .2f;
                            CellSGSTAmt.BorderColor = BaseColor.BLACK;
                            //CellSGSTAmt.PaddingBottom = 5;
                            //CellSGSTAmt.PaddingTop = 5;
                            Childterms112.AddCell(CellSGSTAmt);


                        }

                        if (IGST > 0)
                        {


                            PdfPCell CellIGST = new PdfPCell(new Phrase(IGSTAlias + " @ " + IGSTPrc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellIGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellIGST.Colspan = 3;
                            CellIGST.BorderWidthBottom = 0;
                            CellIGST.BorderWidthLeft = .2f;
                            CellIGST.BorderWidthTop = 0.2f;
                            CellIGST.BorderWidthRight = 0f;
                            // CellIGST.PaddingBottom = 5;
                            // CellIGST.PaddingTop = 5;
                            CellIGST.BorderColor = BaseColor.BLACK;
                            Childterms112.AddCell(CellIGST);

                            PdfPCell CellIGSTAmt = new PdfPCell(new Phrase(IGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellIGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellIGSTAmt.BorderWidthBottom = 0;
                            CellIGSTAmt.BorderWidthLeft = 0.2f;
                            CellIGSTAmt.BorderWidthTop = 0.2f;
                            CellIGSTAmt.BorderWidthRight = .2f;
                            CellIGSTAmt.BorderColor = BaseColor.BLACK;
                            //CellIGSTAmt.PaddingBottom = 5;
                            //CellIGSTAmt.PaddingTop = 5;
                            Childterms112.AddCell(CellIGSTAmt);


                        }

                        #endregion for GST                                                   



                        #endregion for taxes

                        decimal GrandTotalVal = Grandtotal;

                        decimal GrandtotalRoundOff = Math.Round(GrandTotalVal, 0);
                        decimal RoundOff = (GrandtotalRoundOff - GrandTotalVal);
                        decimal GrandtotalValue = (GrandTotalVal + RoundOff);

                        PdfPCell

                        cellgrandto = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellgrandto.Colspan = 3;
                        cellgrandto.BorderWidthBottom = 0;
                        cellgrandto.BorderWidthLeft = .2f;
                        cellgrandto.BorderWidthTop = 0.2f;
                        cellgrandto.BorderWidthRight = 0f;
                        cellgrandto.BorderColor = BaseColor.BLACK;
                        Childterms112.AddCell(cellgrandto);

                        cellgrandto = new PdfPCell(new Phrase(GrandtotalValue.ToString("N2"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellgrandto.BorderWidthBottom = 0;
                        cellgrandto.BorderWidthLeft = 0.2f;
                        cellgrandto.BorderWidthTop = 0.2f;
                        cellgrandto.BorderWidthRight = .2f;
                        cellgrandto.BorderColor = BaseColor.BLACK;
                        Childterms112.AddCell(cellgrandto);

                        PdfPCell Chid312 = new PdfPCell(Childterms112);
                        Chid312.Border = 0;
                        Chid312.Colspan = 4;
                        Chid312.HorizontalAlignment = 0;
                        tempTable22.AddCell(Chid312);


                        PdfPCell Cellnoofamout = new PdfPCell(new Phrase("Amount In Words:" + " " + AmountInWords(GrandtotalValue) + "", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        Cellnoofamout.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        Cellnoofamout.Colspan = colCount;
                        Cellnoofamout.BorderWidthBottom = .2f;
                        Cellnoofamout.BorderWidthLeft = .2f;
                        Cellnoofamout.BorderWidthTop = .2f;
                        Cellnoofamout.BorderWidthRight = 0.2f;
                        Cellnoofamout.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(Cellnoofamout);


                        document.Add(tempTable22);

                        #region footer

                        PdfPTable Addterms = new PdfPTable(6);
                        Addterms.TotalWidth = 500f;
                        Addterms.LockedWidth = true;
                        float[] widthrerms = new float[] { 1.2f, 6.2f, 2f, 2.2f, 2f, 2.7f };
                        Addterms.SetWidths(widthrerms);

                        if (notes.Length > 0)
                        {
                            cell = new PdfPCell(new Phrase(notes, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = .2f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 6;
                            cell.SetLeading(0, 1.3f);
                            cell.BorderColor = BaseColor.BLACK;
                            Addterms.AddCell(cell);
                        }

                        if (BankAcNumber.Length > 0 || BankName.Length > 0 || IFSCCode.Length > 0 || BranchName.Length > 0)
                        {
                            cell = new PdfPCell(new Phrase("Bank Details", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = .2f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 6;
                            // cell.SetLeading(0, 1.3f);
                            cell.BorderColor = BaseColor.BLACK;
                            Addterms.AddCell(cell);

                            cell = new PdfPCell();
                            Paragraph CcellHead5 = new Paragraph();
                            CcellHead5.Add(new Chunk("Bank NAME : ", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            CcellHead5.Add(new Chunk(BankName, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            cell.AddElement(CcellHead5);
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = .2f;
                            cell.BorderWidthLeft = .2f;
                            cell.PaddingTop = -3f;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell.Colspan = 6;
                            Addterms.AddCell(cell);

                            cell = new PdfPCell();
                            Paragraph CcellHead6 = new Paragraph();
                            CcellHead6.Add(new Chunk("A/C No : ", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            CcellHead6.Add(new Chunk(BankAcNumber, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.AddElement(CcellHead6);
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell.Colspan = 6;
                            cell.PaddingTop = -3f;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = .2f;
                            cell.BorderWidthLeft = .2f;
                            Addterms.AddCell(cell);

                            cell = new PdfPCell();
                            Paragraph CcellHead1 = new Paragraph();
                            CcellHead1.Add(new Chunk("Branch : ", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            CcellHead1.Add(new Chunk(BranchName, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.AddElement(CcellHead1);
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell.Colspan = 6;
                            cell.PaddingTop = -3f;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = .2f;
                            cell.BorderWidthLeft = .2f;
                            Addterms.AddCell(cell);

                            cell = new PdfPCell();
                            Paragraph CcellHead2 = new Paragraph();
                            CcellHead2.Add(new Chunk("IFSC Code : ", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            CcellHead2.Add(new Chunk(IFSCCode, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.AddElement(CcellHead2);
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell.Colspan = 6;
                            cell.PaddingTop = -3f;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = .2f;
                            cell.BorderWidthLeft = .2f;
                            Addterms.AddCell(cell);
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(BillDesc, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = .2f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 6;
                            cell.SetLeading(0, 1.3f);
                            cell.BorderColor = BaseColor.BLACK;
                            Addterms.AddCell(cell);
                        }


                        PdfPTable Childterms = new PdfPTable(3);
                        Childterms.TotalWidth = 290f;
                        Childterms.LockedWidth = true;
                        float[] Celters = new float[] { 1.5f, 2f, 2f };
                        Childterms.SetWidths(Celters);


                        #region for payment terms


                        cell = new PdfPCell(new Phrase(companyName, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = .2f;
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthLeft = .2f;
                        // cell.PaddingTop = 7;
                        cell.Colspan = 3;
                        cell.BorderColor = BaseColor.BLACK;
                        Childterms.AddCell(cell);

                        if (Bdt.Rows.Count > 0)
                        {


                            if (HSNNumber.Length > 0)
                            {
                                PdfPCell clietnpin = new PdfPCell(new Paragraph("HSN NUMBER", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.HorizontalAlignment = 0;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthRight = 0f;
                                clietnpin.BorderWidthLeft = .2f;
                                clietnpin.Colspan = 1;
                                clietnpin.BorderColor = BaseColor.BLACK;
                                Childterms.AddCell(clietnpin);


                                cell = new PdfPCell(new Paragraph(": " + HSNNumber, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0;
                                cell.BorderWidthLeft = 0;
                                cell.Colspan = 2;
                                cell.BorderColor = BaseColor.BLACK;
                                Childterms.AddCell(cell);

                            }



                            if (SACCode.Length > 0)
                            {
                                PdfPCell clietnpin = new PdfPCell(new Paragraph("SAC CODE", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthRight = 0f;
                                clietnpin.BorderWidthLeft = .2f;
                                clietnpin.Colspan = 1;
                                clietnpin.BorderColor = BaseColor.BLACK;
                                Childterms.AddCell(clietnpin);

                                cell = new PdfPCell(new Paragraph(": " + SACCode, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0;
                                cell.BorderWidthLeft = 0;
                                cell.Colspan = 2;
                                cell.BorderColor = BaseColor.BLACK;
                                Childterms.AddCell(cell);

                            }
                        }


                        if (PANNO.Length > 0)
                        {

                            cell = new PdfPCell(new Phrase("PAN NO", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);


                            cell = new PdfPCell(new Phrase(": " + PANNO, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);
                        }

                        //if (Bdt.Rows.Count > 0)
                        //{
                        //    if (OurGSTIN.Length > 0)
                        //    {


                        //        cell = new PdfPCell(new Phrase(OurGSTINAlias, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        //        cell.HorizontalAlignment = 0;
                        //        cell.BorderWidthBottom = 0;
                        //        cell.BorderWidthTop = 0;
                        //        cell.BorderWidthRight = 0f;
                        //        cell.BorderWidthLeft = .2f;
                        //        cell.Colspan = 1;
                        //        cell.BorderColor = BaseColor.BLACK;
                        //        Childterms.AddCell(cell);


                        //        cell = new PdfPCell(new Phrase(": " + OurGSTIN, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        //        cell.HorizontalAlignment = 0;
                        //        cell.BorderWidthBottom = 0;
                        //        cell.BorderWidthTop = 0;
                        //        cell.BorderWidthRight = 0;
                        //        cell.BorderWidthLeft = 0;
                        //        cell.Colspan = 2;
                        //        cell.BorderColor = BaseColor.BLACK;
                        //        Childterms.AddCell(cell);

                        //    }
                        //}
                        if (Servicetax.Length > 0)
                        {


                            cell = new PdfPCell(new Phrase("SER. TAX REG.NO", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                            cell = new PdfPCell(new Phrase(": " + Servicetax, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);
                        }
                        if (Category.Length > 0)
                        {
                            cell = new PdfPCell(new Phrase("SC-CATEGORY", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                            cell = new PdfPCell(new Phrase(": " + Category, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                        }
                        if (PFNo.Length > 0)
                        {

                            cell = new PdfPCell(new Phrase("PF CODE NO", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);


                            cell = new PdfPCell(new Phrase(": " + PFNo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);
                        }
                        else if (CmpPFNo.Length > 0)
                        {

                            cell = new PdfPCell(new Phrase("PF CODE NO", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                            cell = new PdfPCell(new Phrase(": " + CmpPFNo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);
                        }
                        if (Esino.Length > 0)
                        {

                            cell = new PdfPCell(new Phrase("ESI CODE NO", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                            cell = new PdfPCell(new Phrase(": " + Esino, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                        }
                        else if (CmpEsino.Length > 0)
                        {


                            cell = new PdfPCell(new Phrase("ESI CODE NO", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                            cell = new PdfPCell(new Phrase(": " + CmpEsino, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);
                        }

                        if (CINNo.Length > 0)
                        {

                            cell = new PdfPCell(new Phrase("CIN NO", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                            cell = new PdfPCell(new Phrase(": " + CINNo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                        }

                        if (MSMENo.Length > 0)
                        {
                            cell = new PdfPCell(new Phrase("MSME NO", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                            cell = new PdfPCell(new Phrase(": " + MSMENo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 0;
                            cell.BorderWidthBottom = 0;
                            cell.BorderWidthTop = 0;
                            cell.BorderWidthRight = 0;
                            cell.BorderWidthLeft = 0;
                            cell.Colspan = 2;
                            cell.BorderColor = BaseColor.BLACK;
                            Childterms.AddCell(cell);

                        }

                        cell = new PdfPCell(new Phrase("\n\n\nCustomer's seal and signature", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthBottom = .2f;
                        cell.BorderWidthLeft = .2f;
                        cell.PaddingTop = 5f;
                        cell.PaddingBottom = 5f;
                        cell.Colspan = 3;
                        cell.BorderColor = BaseColor.BLACK;
                        Childterms.AddCell(cell);


                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.BorderWidthBottom = .2f;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthLeft = .2f;
                        cell.Colspan = 3;
                        cell.BorderColor = BaseColor.BLACK;
                        // Childterms.AddCell(cell);




                        #endregion for payment terms


                        PdfPCell Chid3 = new PdfPCell(Childterms);
                        Chid3.Border = 0;
                        Chid3.Colspan = 3;
                        Chid3.HorizontalAlignment = 0;
                        Addterms.AddCell(Chid3);



                        PdfPTable chilk = new PdfPTable(3);
                        chilk.TotalWidth = 210f;
                        chilk.LockedWidth = true;
                        float[] Celterss = new float[] { 2.2f, 2f, 2.7f };
                        chilk.SetWidths(Celterss);




                        cell = new PdfPCell(new Phrase("For " + companyName, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        cell.BorderWidthBottom = 0;
                        cell.PaddingTop = 10f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = .2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.Colspan = 3;
                        cell.BorderColor = BaseColor.BLACK;
                        chilk.AddCell(cell);

                        cell = new PdfPCell(new Phrase("\n\n\n\n Authorised Signatory", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        cell.BorderWidthBottom = .2f;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = .2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.Colspan = 3;
                        cell.PaddingTop = 5;
                        cell.BorderColor = BaseColor.BLACK;
                        chilk.AddCell(cell);



                        cell = new PdfPCell(new Phrase("Computer Generated Invoice and Requires No Signature", FontFactory.GetFont(FontStyle, font, Font.ITALIC, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        cell.BorderWidthBottom = .2f;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = .2f;
                        cell.BorderWidthLeft = 0.2f;
                        cell.Colspan = 3;
                        cell.PaddingTop = 5;
                        cell.BorderColor = BaseColor.BLACK;
                        chilk.AddCell(cell);


                        PdfPCell Chid4 = new PdfPCell(chilk);
                        Chid4.Border = 0;
                        Chid4.Colspan = 3;
                        Chid4.HorizontalAlignment = 0;
                        Addterms.AddCell(Chid4);


                        cell = new PdfPCell(new Phrase("***Thank you for your business***", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0;
                        cell.BorderWidthLeft = 0;
                        cell.Colspan = 6;
                        Addterms.AddCell(cell);

                        document.Add(Addterms);


                        #endregion
                        #endregion

                        #endregion
                    }

                    document.Close();

                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There is no bill generated for selected client ');", true);

            }
        }

        public string AmountInWords(decimal GrandTotal)
        {
            string Inwords = "";

            string GTotal = GrandTotal.ToString("0.00");
            string[] arr = GTotal.ToString().Split("."[0]);
            string inwords = "";
            string rupee = (arr[0]);
            string paise = "";
            if (arr.Length == 2)
            {
                if (arr[1].Length > 0 && arr[1] != "00")
                {
                    paise = (arr[1]);
                }
            }

            if (paise != "0.00" && paise != "0" && paise != "")
            {
                int I = Int16.Parse(paise);
                string p = NumberToEnglish.Instance.NumbersToWordsDecimal(I, true);
                paise = p;
                rupee = NumberToEnglish.Instance.NumbersToWordsDecimal(Convert.ToInt64(arr[0]), false);
                inwords = " Rupees " + rupee + " and " + paise + " Paise Only";

            }
            else
            {
                rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                inwords = " Rupees " + rupee + " Only";
            }

            Inwords = inwords;

            return Inwords;

        }

        protected void btnCalculateTotals_Click(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void CalculateTotals()
        {
            try
            {

                DateTime DtLastDay = DateTime.Now;

                DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

                decimal totalamt = 0;

                decimal TotalbeforeTax = 0;
                bool CGSTType = rdbSGST.Checked;
                bool SGSType = rdbSGST.Checked;
                bool IGSTType = rdbIGST.Checked;
                for (int i = 0; i < gvClientBilling.Rows.Count; i++)
                {
                    DropDownList ddldtype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;
                    DropDownList ddlnod = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                    TextBox txtDesg = gvClientBilling.Rows[i].FindControl("lbldesgn") as TextBox;
                    TextBox txtpayrate = gvClientBilling.Rows[i].FindControl("lblpayrate") as TextBox;

                    //HiddenField hdNOD = gvClientBilling.Rows[i].FindControl("hdNOD") as HiddenField;
                    TextBox txtnod = gvClientBilling.Rows[i].FindControl("lblNoOfDuties") as TextBox;
                    TextBox txtdutyamt = gvClientBilling.Rows[i].FindControl("lblda") as TextBox;
                    TextBox txtTotal = gvClientBilling.Rows[i].FindControl("lblAmount") as TextBox;
                    TextBox txtnoofemplyes = gvClientBilling.Rows[i].FindControl("lblnoofemployees") as TextBox;
                    TextBox txtHSNNumber = gvClientBilling.Rows[i].FindControl("txtHSNNumber") as TextBox;
                    TextBox lblCGSTAmount = gvClientBilling.Rows[i].FindControl("lblCGSTAmount") as TextBox;
                    TextBox lblCGSTPrc = gvClientBilling.Rows[i].FindControl("lblCGSTPrc") as TextBox;
                    TextBox lblSGSTAmount = gvClientBilling.Rows[i].FindControl("lblSGSTAmount") as TextBox;
                    TextBox lblSGSTPrc = gvClientBilling.Rows[i].FindControl("lblSGSTPrc") as TextBox;
                    TextBox lblIGSTAmount = gvClientBilling.Rows[i].FindControl("lblIGSTAmount") as TextBox;
                    TextBox lblIGSTPrc = gvClientBilling.Rows[i].FindControl("lblIGSTPrc") as TextBox;
                    TextBox lblTotalTaxmount = gvClientBilling.Rows[i].FindControl("lblTotalTaxmount") as TextBox;
                    TextBox lblGSTper = gvClientBilling.Rows[i].FindControl("lblGSTper") as TextBox;

                    if (!string.IsNullOrEmpty(txtDesg.Text.Trim()))
                    {
                        switch (ddldtype.SelectedIndex)
                        {
                            case 4:

                                txtdutyamt.Text = txtTotal.Text = txtpayrate.Text;

                                break;
                            case 1:
                            case 2:
                            case 3:

                                if (txtpayrate.Text == "")
                                {
                                    txtpayrate.Text = "0";
                                }

                                if (txtnod.Text == "")
                                {
                                    txtnod.Text = "0";
                                }


                                //if (roundoff == true)
                                //{
                                //    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtpayrate.Text) * Convert.ToDecimal(txtnod.Text)).ToString("0.##");
                                //}
                                //else
                                {
                                    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtpayrate.Text) * Convert.ToDecimal(txtnod.Text)).ToString("0");
                                }


                                break;
                            case 0:
                                if (txtpayrate.Text == "")
                                {
                                    txtpayrate.Text = "0";
                                }

                                if (txtnod.Text == "")
                                {
                                    txtnod.Text = "0";
                                }


                                //if (roundoff == true)
                                //{
                                //    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtpayrate.Text) / Convert.ToDecimal(ddlnod.SelectedValue) * Convert.ToDecimal(txtnod.Text)).ToString("0.##");
                                //}
                                //else
                                {
                                    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtpayrate.Text) / Convert.ToDecimal(ddlnod.SelectedValue) * Convert.ToDecimal(txtnod.Text)).ToString("0");

                                }



                                break;
                            case 7:

                                //if (roundoff == true)
                                //{
                                //    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtnoofemplyes.Text) * Convert.ToDecimal(txtpayrate.Text)).ToString("0.##");
                                //}
                                //else
                                {
                                    txtdutyamt.Text = txtTotal.Text = (Convert.ToDecimal(txtnoofemplyes.Text) * Convert.ToDecimal(txtpayrate.Text)).ToString("0");

                                }



                                break;
                            default:
                                // if (ddlType.SelectedIndex == 1 || ddlType.SelectedIndex == 3)
                                {
                                    txtdutyamt.Text = txtTotal.Text = txtpayrate.Text;
                                }


                                break;
                        }

                        if (!string.IsNullOrEmpty(txtTotal.Text.Trim()))
                            totalamt += (Convert.ToDecimal(txtTotal.Text.ToString()));

                    }
                }

                var billdtnew = DateTime.Now.ToString("dd/MM/yyyy");

                if (txtinvoicedate.Text != "")
                {
                    billdtnew = txtinvoicedate.Text;
                }

                DateTime dt = DateTime.ParseExact(billdtnew, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                // for both "1/1/2000" or "25/1/2000" formats
                string billdt = dt.ToString("MM/dd/yyyy");


                var cid = txtnewclientid.Text;

                var query1 = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2 from TblOptions where '" + billdt + "' between fromdate and todate ";//Gst

                var optiondetails = config.ExecuteAdaptorAsyncWithQueryParams(query1).Result;



                decimal CGST = Convert.ToDecimal(optiondetails.Rows[0]["CGST"].ToString()); //Gst
                decimal SGST = Convert.ToDecimal(optiondetails.Rows[0]["SGST"].ToString());
                decimal IGST = Convert.ToDecimal(optiondetails.Rows[0]["IGST"].ToString());

                decimal gtotal = 0;
                decimal servicecharge = 0;
                decimal subtotal = 0;
                decimal Servicechargeamt = 0;
                decimal TotalDutiesAmount = 0;
                decimal CGSTTax = 0;   //gst
                decimal SGSTTax = 0;
                decimal IGSTTax = 0;

                if (TxtservicechrgPrc.Text == "")
                {
                    TxtservicechrgPrc.Text = "0";
                }
                servicecharge = Convert.ToDecimal(TxtservicechrgPrc.Text);
                //if (roundoff == true)
                //{
                //    lblServiceCharges.Text = (totalamt * (servicecharge / 100)).ToString("0.##");
                //}
                //else
                {
                    lblServiceCharges.Text = (totalamt * (servicecharge / 100)).ToString("0");

                }

                if (lblServiceCharges.Text == "")
                {
                    lblServiceCharges.Text = "0";
                }

                Servicechargeamt = Convert.ToDecimal(lblServiceCharges.Text);
                subtotal = totalamt + Servicechargeamt;




                if (rdbwithoutGST.Checked == true)
                {
                    CGSTTax = 0;
                    SGSTTax = 0;
                    IGSTTax = 0;
                }
                else if (rdbwithGST.Checked == true && rdbSGST.Checked == true)
                {
                    CGSTTax = Math.Round(CGST * ((totalamt + Servicechargeamt)) / 100, 0);
                    SGSTTax = Math.Round(SGST * ((totalamt + Servicechargeamt)) / 100, 0);
                    IGSTTax = 0;
                }

                else if (rdbwithGST.Checked == true && rdbIGST.Checked == true)
                {
                    CGSTTax = 0;
                    IGSTTax = 0;
                    IGSTTax = Math.Round(IGST * ((totalamt + Servicechargeamt)) / 100, 0);
                }






                gtotal = subtotal + CGSTTax + SGSTTax + IGSTTax;
                TotalDutiesAmount = totalamt;


                TotalbeforeTax = Math.Round((totalamt + Servicechargeamt), 0);

                if (TotalDutiesAmount > 0)
                {
                    txtResources.Text = TotalDutiesAmount.ToString();
                    lblResorces.Visible = true;
                    txtResources.Visible = true;
                }



                if (TotalbeforeTax > 0)
                {
                    TxtTotalbeforeTax.Text = TotalbeforeTax.ToString();
                    lblTotalbeforeTax.Visible = true;
                    TxtTotalbeforeTax.Visible = true;
                }


                #region for GST on 17-6-2017 by swathi

                if (CGSTTax > 0)
                {
                    lblCGST.Text = CGSTTax.ToString();
                    TxtCGSTPrc.Text = CGST.ToString();
                    lblCGSTTitle.Visible = true;
                    lblCGST.Visible = true;
                    TxtCGSTPrc.Visible = true;
                }
                else
                {
                    lblCGST.Text = CGSTTax.ToString();
                    lblCGSTTitle.Visible = false;
                    lblCGST.Visible = false;
                    TxtCGSTPrc.Visible = false;

                }



                if (SGSTTax > 0)
                {
                    lblSGST.Text = SGSTTax.ToString();
                    TxtSGSTPrc.Text = SGST.ToString();
                    lblSGSTTitle.Visible = true;
                    lblSGST.Visible = true;
                    TxtSGSTPrc.Visible = true;
                }
                else
                {
                    lblSGST.Text = SGSTTax.ToString();
                    lblSGSTTitle.Visible = false;
                    lblSGST.Visible = false;
                    TxtSGSTPrc.Visible = false;

                }



                if (IGSTTax > 0)
                {
                    lblIGST.Text = IGSTTax.ToString();
                    TxtIGSTPrc.Text = IGST.ToString();
                    lblIGSTTitle.Visible = true;
                    lblIGST.Visible = true;
                    TxtIGSTPrc.Visible = true;
                }
                else
                {
                    lblIGST.Text = IGSTTax.ToString();
                    lblIGSTTitle.Visible = false;
                    lblIGST.Visible = false;
                    TxtIGSTPrc.Visible = false;

                }

                #endregion for GST on 17-6-2017





                if (gtotal > 0)
                {
                    lblGrandTotal.Text = gtotal.ToString();
                    lblGrandTotal.Visible = true;
                    lblgrandtotalss.Visible = true;
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void rdbwithGST_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbwithGST.Checked == true)
            {
                rdbSGST.Visible = true;
                rdbIGST.Visible = true;
            }

            else if (rdbwithoutGST.Checked == true)
            {
                rdbSGST.Visible = false;
                rdbIGST.Visible = false;
            }
        }

        protected void ClearData()
        {
            txtnewclientid.Text = "";
            txtnewclientname.Text = "";
            txtfrommonth.Text = "";
            txttoDate.Text = "";
            txtgstno.Text = "";
            txtGSTAddress.Text = "";
            txtinvoicedate.Text = "";
            txtmonth.Text = "";
            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            rdbwithGST.Checked = true;
            rdbSGST.Checked = true;
            InvoiceNoAuto();
            SetInitialRow();
        }

      
    }
}