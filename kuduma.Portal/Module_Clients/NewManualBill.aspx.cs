using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class NewManualBill : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string FontStyle = "Tahoma";
        string BranchID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            if (!IsPostBack)
            {
                LoadClientIDAndData();
                //month
                var formatInfoinfo = new DateTimeFormatInfo();
                string[] monthName = formatInfoinfo.MonthNames;
                string currentMonth = monthName[DateTime.Now.Month - 1];
                string month = "";
                string LastMonth = "";
                try
                {
                    month = monthName[DateTime.Now.Month - 2];
                }
                catch (IndexOutOfRangeException ex)
                {
                    month = monthName[12 - (2 - DateTime.Now.Month)];
                }
                try
                {
                    LastMonth = monthName[DateTime.Now.Month - 3];
                }
                catch (IndexOutOfRangeException ex)
                {
                    LastMonth = monthName[12 - (3 - DateTime.Now.Month)];
                }
                ddlmonth.Items.Add(currentMonth);
                ddlmonth.Items.Add(month);
                ddlmonth.Items.Add(LastMonth);

                try
                {
                    LastMonth = monthName[DateTime.Now.Month - 4];
                }
                catch (IndexOutOfRangeException ex)
                {
                    LastMonth = monthName[12 - (4 - DateTime.Now.Month)];
                }
                ddlmonth.Items.Add(LastMonth);
                ClearData();
                ddlMBBillnos.Items.Insert(0, "--Select--");

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                List<string> list = DtDesignation.AsEnumerable()
                                   .Select(r => r.Field<string>("Design"))
                                   .ToList();
                var result = new JavaScriptSerializer().Serialize(list);
                hdDesignations.Value = result;

                FillDefaultGird();
            }

        }

        protected void LoadClientIDAndData()
        {
            string selectclientid = "select clientid from clients where ClientStatus=1 and clientid like '%" + CmpIDPrefix + "%'  Order By right(clientid,4) ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectclientid).Result;
            ddlmonth.Items.Add("--Select--");
            if (dt.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dt;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "--Select--");
            dt = null;


            dt = config.ExecuteAdaptorAsyncWithQueryParams("select ClientName,Clientid from clients where ClientStatus=1 and clientid like '%" + CmpIDPrefix + "%'  order by Clientname").Result;
            if (dt.Rows.Count > 0)
            {
                ddlCname.DataValueField = "clientid";
                ddlCname.DataTextField = "ClientName";
                ddlCname.DataSource = dt;
                ddlCname.DataBind();
            }
            ddlCname.Items.Insert(0, "--Select--");
        }

        protected void AddNewRow()
        {
            try
            {


                //DataTable dt = (DataTable)ViewState["DTDefaultManual"];
                //DataRow dr = dt.NewRow();
                //dr["Sid"] = dt.Rows.Count + 1;
                //dr["Designation"] = "";
                //dr["NoofEmps"] = 0;
                //dr["DutyHrs"] = 0;
                //dr["DutyHours"] = 0;
                //dr["payrate"] = 0;
                //dr["paytype"] = 0;
                //dr["BasicDa"] = 0;
                //dr["OTAmount"] = 0;
                //dr["NoOfDays"] = 1;
                //dr["Totalamount"] = 0;
                //dt.Rows.Add(dr);
                //gvClientBilling.DataSource = dt;
                //gvClientBilling.DataBind();
                //ScriptManager.RegisterStartupScript(this, GetType(), "bindautofilldesgs", "bindautofilldesgs();", true);




                int rowIndex = 0;

                if (ViewState["DTDefaultManual"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["DTDefaultManual"];
                    DataRow drCurrentRow = null;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            //extract the TextBox values
                            TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[rowIndex].Cells[1].FindControl("txtgvdesgn");
                            TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[rowIndex].Cells[2].FindControl("txtnoofemployees");
                            TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[rowIndex].Cells[3].FindControl("txtNoOfDuties");
                            TextBox txtPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[4].FindControl("txtPayRate");
                            DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[5].FindControl("ddldutytype");
                            DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[6].FindControl("ddlnod");
                            TextBox txtda = (TextBox)gvClientBilling.Rows[rowIndex].Cells[7].FindControl("txtda");
                            TextBox txtAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[8].FindControl("txtAmount");


                            drCurrentRow = dtCurrentTable.NewRow();
                            // drCurrentRow["Sid"] = i + 1;

                            dtCurrentTable.Rows[i - 1]["Designation"] = txtgvdesgn.Text;
                            dtCurrentTable.Rows[i - 1]["NoofEmps"] = txtnoofemployees.Text.Trim() == "" ? 0 : Convert.ToInt32(txtnoofemployees.Text);
                            dtCurrentTable.Rows[i - 1]["DutyHours"] = txtNoOfDuties.Text.Trim() == "" ? 0 : Convert.ToSingle(txtNoOfDuties.Text);
                            dtCurrentTable.Rows[i - 1]["payrate"] = txtPayRate.Text.Trim() == "" ? 0 : Convert.ToSingle(txtPayRate.Text);
                            dtCurrentTable.Rows[i - 1]["paytype"] = ddldutytype.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["NoOfDays"] = ddlnod.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["BasicDa"] = txtda.Text.Trim() == "" ? 0 : Convert.ToSingle(txtda.Text);
                            dtCurrentTable.Rows[i - 1]["Totalamount"] = txtAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(txtAmount.Text);


                            rowIndex++;
                        }
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["DTDefaultManual"] = dtCurrentTable;

                        gvClientBilling.DataSource = dtCurrentTable;
                        gvClientBilling.DataBind();
                    }
                }
                else
                {
                    Response.Write("ViewState is null");
                }
                SetPreviousData();

            }
            catch (Exception ex)
            {

            }
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["DTDefaultManual"] != null)
            {
                DataTable dt = (DataTable)ViewState["DTDefaultManual"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox txtgvdesgn = (TextBox)gvClientBilling.Rows[rowIndex].Cells[1].FindControl("txtgvdesgn");
                        TextBox txtnoofemployees = (TextBox)gvClientBilling.Rows[rowIndex].Cells[2].FindControl("txtnoofemployees");
                        TextBox txtNoOfDuties = (TextBox)gvClientBilling.Rows[rowIndex].Cells[3].FindControl("txtNoOfDuties");
                        TextBox txtPayRate = (TextBox)gvClientBilling.Rows[rowIndex].Cells[4].FindControl("txtPayRate");
                        DropDownList ddldutytype = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[5].FindControl("ddldutytype");
                        DropDownList ddlnod = (DropDownList)gvClientBilling.Rows[rowIndex].Cells[6].FindControl("ddlnod");
                        TextBox txtda = (TextBox)gvClientBilling.Rows[rowIndex].Cells[7].FindControl("txtda");
                        TextBox txtAmount = (TextBox)gvClientBilling.Rows[rowIndex].Cells[8].FindControl("txtAmount");


                        txtgvdesgn.Text = dt.Rows[i]["Designation"].ToString();
                        txtnoofemployees.Text = dt.Rows[i]["NoofEmps"].ToString();
                        txtNoOfDuties.Text = dt.Rows[i]["DutyHours"].ToString();
                        txtPayRate.Text = dt.Rows[i]["payrate"].ToString();
                        ddldutytype.SelectedValue = dt.Rows[i]["paytype"].ToString();
                        ddlnod.SelectedValue = dt.Rows[i]["NoOfDays"].ToString();
                        txtda.Text = dt.Rows[i]["BasicDa"].ToString();
                        txtAmount.Text = dt.Rows[i]["Totalamount"].ToString();

                        rowIndex++;
                    }
                }
            }
        }

        protected void LoadMonths()
        {
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            string currentMonth = monthName[DateTime.Now.Month - 1];
            string month = "";
            string LastMonth = "";
            try
            {
                month = monthName[DateTime.Now.Month - 2];
            }
            catch (IndexOutOfRangeException ex)
            {
                month = monthName[12 - (2 - DateTime.Now.Month)];
            }
            try
            {
                LastMonth = monthName[DateTime.Now.Month - 3];
            }
            catch (IndexOutOfRangeException ex)
            {
                LastMonth = monthName[12 - (3 - DateTime.Now.Month)];
            }
            ddlmonth.Items.Add(currentMonth);
            ddlmonth.Items.Add(month);
            ddlmonth.Items.Add(LastMonth);
            ddlmonth.Items.Insert(0, "-select-");
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();


        }
        protected void ClearData()
        {
            lblTotalResources.Text = "0";
            lblServiceTax.Text = "0";
            lblGrandTotal.Text = "0";
            lblCESS.Text = "0";
            lblSBCESS.Text = "0";
            lblKKCESS.Text = "0";
            lblSheCESS.Text = "0";
            lblSubTotal.Text = "0";
            lblServiceCharges.Text = "0";
            Txtservicechrg.Text = "0";
        }

        protected void DisplayDataInGrid()
        {
            #region Variable Declaration
            ClearData();
            int month = 0;

            #endregion

            #region  Select Month

            month = GetMonthBasedOnSelectionDateorMonth();

            //if (ddlmonth.SelectedIndex == 1)
            //{
            //    month = GlobalData.Instance.GetIDForNextMonth();
            //}
            //else if (ddlmonth.SelectedIndex == 2)
            //{
            //    month = GlobalData.Instance.GetIDForThisMonth();
            //}
            //else
            //{
            //    month = GlobalData.Instance.GetIDForPrviousMonth();
            //}
            #endregion

            #region Empty And Assign Data To Gridview
            lbltotalamount.Text = "";
            DataTable Dtunit = null;
            gvClientBilling.DataSource = Dtunit;
            gvClientBilling.DataBind();

            #endregion


            #region  Begin Get Contract Id Based on The Last Day

            DateTime DtLastDay = DateTime.Now;
            if (Chk_Month.Checked == false)
            {
                DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
            }
            if (Chk_Month.Checked == true)
            {
                DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            }
            var ContractID = "";
            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
            HtGetContractID.Add("@LastDay", DtLastDay);
            DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                return;
            }

            #endregion  End Get Contract Id Based on The Last Day


            #region New Coding For Manual Billing

            var query = @"select 
                             Ubb.Designation,
                             Ubb.BasicDA as BasicDa,
                             ISNULL(Ubb.NoofEmps,0) as NoofEmps,
                             ISNULL(Ubb.DutyHours,0) as DutyHrs,
                             Round(Ubb.PayRate,2) as payrate,  
                             Ubb.PayRateType as paytype,
                             Ubb.monthlydays,
                             Ubb.DutyHours,
                             Ubb.OTAmount,
                             Ubb.Totalamount,
                             Ubb.Remarks, 
                             Ubb.Description,
                             ISNULL(cd.Servicecharge,0) as ServiceCharge,
                             cd.NoOfDays,
                             mub.ServiceChrg as ServiceChrg,
                             mub.BillDt as BillDate
                               
                    from MUnitBillBreakup as Ubb 
                    inner join Contracts cd on cd.ClientID = ubb.UnitId inner join MUnitBill mub on Ubb.UnitId=mub.UnitId and Ubb.MunitidBillno=mub.Billno
                    where Ubb.unitid ='" + ddlclientid.SelectedValue + "' and Ubb.month=" + month + " and  Ubb.MunitidBillno='" + ddlMBBillnos.SelectedValue
                        + "' and cd.contractid='" + ContractID + "' order by ubb.sino";

            //Group by  Ubb.UnitId, Ubb.Designation,Ubb.BasicDA, Ubb.NoofEmps,Ubb.DutyHours,Ubb.monthlydays,Ubb.PayRate,Ubb.PayRateType,Ubb.DutyHours,Ubb.otamount,Ubb.Remarks,Ubb.Description,cd.NoOfDays, mub.ServiceChrg,mub.BillDt,cd.ServiceCharge,Ubb.Totalamount";

            DataTable DtForUBB = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;


            var SumTotal = @"select sum(isnull(Totalamount,0)) as Total from  MUnitBillBreakup where unitid ='" + ddlclientid.SelectedValue + "' and month=" + month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue
                        + "'";

            DataTable DtForSumUBB = config.ExecuteAdaptorAsyncWithQueryParams(SumTotal).Result;


            if (DtForUBB.Rows.Count > 0)
            {

                gvClientBilling.DataSource = DtForUBB;
                gvClientBilling.DataBind();
                for (int i = 0; i < DtForUBB.Rows.Count; i++)
                {
                    DropDownList Nods = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;

                    if (Nods != null)
                    {



                        int noofdays = int.Parse(DtForUBB.Rows[i]["monthlydays"].ToString());
                        Nods.SelectedValue = DtForUBB.Rows[i]["monthlydays"].ToString();

                    }


                    DropDownList Dtype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;

                    if (Dtype != null)
                    {



                        int amt = int.Parse(DtForUBB.Rows[i]["PayType"].ToString());
                        Dtype.SelectedValue = DtForUBB.Rows[i]["PayType"].ToString();

                    }
                }
                ViewState["DTDefaultManual"] = DtForUBB;

                //  lblServiceCharges.Text=DtForUBB.Rows[0]["ServiceChrg"].ToString();
                if (DtForSumUBB.Rows.Count > 0)
                {
                    lblTotalResources.Text = DtForSumUBB.Rows[0]["Total"].ToString();
                }
                // txtbilldate.Text=
                txtremarks.Text = DtForUBB.Rows[0]["Remarks"].ToString();
                txtdescription.Text = DtForUBB.Rows[0]["Description"].ToString();

                #region    Retrive Data From munitbill  table data based on the bill no

                string SqlQryForunitbill = "Select *,convert(varchar(10),BillDt,103) as Billdate,convert(varchar(10),FromDt,103) as FromDate,convert(varchar(10),ToDt,103) as ToDate from munitbill   Where  unitid='" + ddlclientid.SelectedValue +
                                           "'  and  Month='" + month + "'  and billno='" + ddlMBBillnos.SelectedValue + "'";

                DataTable DtForUnitBill = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForunitbill).Result;
                if (DtForUnitBill.Rows.Count > 0)
                {
                    System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
                    //DateTime dtb = Convert.ToDateTime(DtForUnitBill.Rows[0]["BillDt"].ToString(), enGB);
                    // string billdate = dtb.ToString("dd/MM/yyyy");


                    string billdate = DtForUnitBill.Rows[0]["Billdate"].ToString();
                    txtbilldate.Text = billdate;

                    txtfromdate.Text = DtForUnitBill.Rows[0]["FromDate"].ToString();
                    txttodate.Text = DtForUnitBill.Rows[0]["ToDate"].ToString();

                    lblbillnolatest.Text = DtForUnitBill.Rows[0]["BillNo"].ToString();
                    Txtservicechrg.Text = DtForUnitBill.Rows[0]["ServiceChrgPer"].ToString();
                    lblSubTotal.Text = DtForUnitBill.Rows[0]["Subtotal"].ToString();
                    lblServiceCharges.Text = DtForUnitBill.Rows[0]["ServiceChrg"].ToString();
                    lblTotalResources.Text = DtForUnitBill.Rows[0]["TotalChrg"].ToString();
                    lblServiceTax.Text = DtForUnitBill.Rows[0]["ServiceTax"].ToString();
                    lblSBCESS.Text = DtForUnitBill.Rows[0]["SBCessAmt"].ToString();
                    lblKKCESS.Text = DtForUnitBill.Rows[0]["KKCessAmt"].ToString();
                    lblCESS.Text = DtForUnitBill.Rows[0]["CESS"].ToString();
                    lblSheCESS.Text = DtForUnitBill.Rows[0]["SHECESS"].ToString();
                    lblGrandTotal.Text = DtForUnitBill.Rows[0]["GrandTotal"].ToString();

                    lblServiceTaxTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblServiceTax.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblSBCESSTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblSBCESS.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblKKCESSTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblKKCESS.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblCESSTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblCESS.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblSheCESSTitle.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblSheCESS.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));
                    lblGrandTotal.Visible = (!string.IsNullOrEmpty(lblServiceTax.Text));

                    string GTotal = Convert.ToSingle(lblGrandTotal.Text).ToString("0.00");
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
                        String p = NumberToEnglish.Instance.NumbersToWords(I, true);
                        paise = p;
                        rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), false);
                        inwords = " Rupees " + rupee + "" + paise + " Paise Only";

                    }
                    else
                    {
                        rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                        inwords = " Rupees " + rupee + " Only";
                    }



                    lblamtinwords.Text = inwords;
                }

                #endregion
            }
            else
            {

                gvClientBilling.DataSource = null;
                gvClientBilling.DataBind();
                FillDefaultGird();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "bindautofilldesgs", "bindautofilldesgs();", true);
            #endregion
        }

        protected void LoadOldBillnos()
        {

            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert()", "alert('please select Client ID/Name')", true);
                return;
            }

            int month = 0;

            month = GetMonthBasedOnSelectionDateorMonth();

            //if (ddlmonth.SelectedIndex == 1)
            //{
            //    month = GlobalData.Instance.GetIDForNextMonth();
            //}
            //else if (ddlmonth.SelectedIndex == 2)
            //{
            //    month = GlobalData.Instance.GetIDForThisMonth();
            //}
            //else
            //{
            //    month = GlobalData.Instance.GetIDForPrviousMonth();
            //}


            string SqlQryFBill = "SElect BillNo  From unitbill Where  unitid ='" + ddlclientid.SelectedValue + "'  and month ='" + month + "'";
            DataTable DtFBill = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryFBill).Result;
            if (DtFBill.Rows.Count > 0)
            {
                lblbillnolatest.Text = DtFBill.Rows[0]["BillNo"].ToString();
            }
            else
            {
                lblbillnolatest.Text = "";
            }


            string SqlQry = "SElect BillNo  From Munitbill Where  unitid ='" + ddlclientid.SelectedValue + "'  and month ='" + month + "'";
            DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
            ddlMBBillnos.Items.Clear();
            if (Dt.Rows.Count > 0)
            {
                ddlMBBillnos.DataTextField = "BillNo";
                ddlMBBillnos.DataValueField = "BillNo";
                ddlMBBillnos.DataSource = Dt;
                ddlMBBillnos.DataBind();
            }
            ddlMBBillnos.Items.Insert(0, "--Select--");


        }




        protected void btninvoice_Click(object sender, EventArgs e)
        {
            int month = 0;
            month = GetMonthBasedOnSelectionDateorMonth();
            if (gvClientBilling.Rows.Count > 0)
            {
                //try
                //{
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                string companyName = "Your Company Name";
                string companyAddress = "Your Company Address";
                string companyaddressline = " ";
                if (compInfo.Rows.Count > 0)
                {
                    companyName = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                    companyaddressline = compInfo.Rows[0]["Addresslineone"].ToString();
                }


                DateTime DtLastDay = DateTime.Now;

                var ContractID = "";


                #region  Begin Get Contract Id Based on The Last Day

                Hashtable HtGetContractID = new Hashtable();
                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                HtGetContractID.Add("@LastDay", DtLastDay);
                DataTable DTContractID =config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                if (DTContractID.Rows.Count > 0)
                {
                    ContractID = DTContractID.Rows[0]["contractid"].ToString();

                }
                #endregion

                string SqlQuryForServiCharge = "select ContractId,servicecharge,ServiceChargeType,Description,IncludeST,ServiceTax75 from contracts where " +
                    " clientid ='" + ddlclientid.SelectedValue + "' and ContractId='" + ContractID + "'";
                DataTable DtServicecharge = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuryForServiCharge).Result;
                string ServiceCharge = "0";
                string strSCType = "";
                string strDescription = "";
                bool bSCType = false;
                string strIncludeST = "";
                string strST75 = "";
                bool bIncludeST = false;
                bool bST75 = false;
                if (DtServicecharge.Rows.Count > 0)
                {
                    if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceCharge"].ToString()) == false)
                    {
                        ServiceCharge = DtServicecharge.Rows[0]["ServiceCharge"].ToString();
                    }
                    if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceChargeType"].ToString()) == false)
                    {
                        strSCType = DtServicecharge.Rows[0]["ServiceChargeType"].ToString();
                    }
                    // string tempDescription = .Rows[0]["Description"].ToString();
                    //if (tempDescription.Trim().Length > 0)
                    //{
                    //    //strDescription = tempDescription;
                    //}
                    if (strSCType.Length > 0)
                    {
                        bSCType = Convert.ToBoolean(strSCType);
                    }
                    strIncludeST = DtServicecharge.Rows[0]["IncludeST"].ToString();
                    strST75 = DtServicecharge.Rows[0]["ServiceTax75"].ToString();
                    if (strIncludeST == "True")
                    {
                        bIncludeST = true;
                    }
                    if (strST75 == "True")
                    {
                        bST75 = true;
                    }
                }
                document.AddTitle(companyName);
                document.AddAuthor("DIYOS");
                document.AddSubject("Invoice");
                document.AddKeywords("Keyword1, keyword2, …");
                string imagepath = Server.MapPath("~/assets/BillLogo.png");
                if (File.Exists(imagepath))
                {
                    iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);

                    gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                    // gif2.SpacingBefore = 50;
                    gif2.ScalePercent(70f);
                    gif2.SetAbsolutePosition(34f, 755f);
                    //document.Add(new Paragraph(" "));
                    document.Add(gif2);
                }

                PdfPTable tablelogo = new PdfPTable(2);
                tablelogo.TotalWidth = 350f;
                tablelogo.LockedWidth = true;
                float[] widtlogo = new float[] { 2f, 2f };
                tablelogo.SetWidths(widtlogo);

                string FontStyle = "Verdana";

                PdfPCell celll = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                celll.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                celll.Border = 0;
                celll.Colspan = 2;


                PdfPCell CCompName = new PdfPCell(new Paragraph(companyName, FontFactory.GetFont(FontStyle, 18, Font.BOLD, BaseColor.BLACK)));
                CCompName.HorizontalAlignment = 1;
                CCompName.Border = 0;
                CCompName.Colspan = 2;
                CCompName.PaddingTop = -20;
                tablelogo.AddCell(CCompName);

                PdfPCell CCompAddress = new PdfPCell(new Paragraph(companyAddress, FontFactory.GetFont(FontStyle, 11, Font.BOLD, BaseColor.BLACK)));
                CCompAddress.HorizontalAlignment = 1;
                CCompAddress.Border = 0;
                CCompAddress.Colspan = 2;

                CCompAddress.SetLeading(0f, 1.3f);
                tablelogo.AddCell(CCompAddress);

                PdfPCell cellline = new PdfPCell(new Paragraph(companyaddressline, FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                cellline.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cellline.Border = 0;
                cellline.Colspan = 2;
                tablelogo.AddCell(cellline);
                //For Space

                tablelogo.AddCell(celll);

                PdfPCell CInvoice = new PdfPCell(new Paragraph("INVOICE", FontFactory.GetFont(FontStyle, 18, Font.UNDERLINE | Font.BOLD, BaseColor.BLACK)));
                CInvoice.HorizontalAlignment = 1;
                CInvoice.Border = 0;
                CInvoice.Colspan = 2;
                tablelogo.AddCell(CInvoice);

                tablelogo.AddCell(celll);

                document.Add(tablelogo);

                PdfPTable address = new PdfPTable(2);
                address.TotalWidth = 500f;
                address.LockedWidth = true;
                float[] addreslogo = new float[] { 2f, 2f };
                address.SetWidths(addreslogo);

                PdfPTable tempTable1 = new PdfPTable(1);
                tempTable1.TotalWidth = 250f;
                tempTable1.LockedWidth = true;
                float[] tempWidth1 = new float[] { 1f };
                tempTable1.SetWidths(tempWidth1);

                string Invoicedesc = "select Description,Remarks from MUnitBillBreakup where UnitId='" + ddlclientid.SelectedItem.ToString() + "' and MunitIdBillno='" + ddlMBBillnos.SelectedValue + "' and  month='" + month + "'";
                DataTable dtinvoicedesc = config.ExecuteAdaptorAsyncWithQueryParams(Invoicedesc).Result;
                string tempDescription = dtinvoicedesc.Rows[0]["Description"].ToString();
                string Remarks = dtinvoicedesc.Rows[0]["Remarks"].ToString();

                if (tempDescription.Trim().Length > 0)
                {
                    strDescription = tempDescription;
                }


                string selectclientaddress = "select * from clients where clientid= '" + ddlclientid.SelectedItem.ToString() + "'";
                DataTable dtclientaddress = config.ExecuteAdaptorAsyncWithQueryParams(selectclientaddress).Result;

                string SelectBillNo = "Select * from MUnitBill where BillNo= '" + ddlMBBillnos.SelectedValue + "' and  month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' ";
                DataTable DtBilling = config.ExecuteAdaptorAsyncWithQueryParams(SelectBillNo).Result;
                string BillNo = "";
                DateTime BillDate;
                DateTime DueDate;

                #region Variables for data Fields as on 11/03/2014 by venkat


                float servicecharge = 0;
                float servicechargePer = 0;
                float servicetax = 0;
                float sbcess = 0;
                float kkcess = 0;
                float cess = 0;
                float shecess = 0;
                float totalamount = 0;
                float Grandtotal = 0;

                float ServiceTax75 = 0;
                float ServiceTax25 = 0;

                float machinarycost = 0;
                float materialcost = 0;
                float maintenancecost = 0;
                float extraonecost = 0;
                float extratwocost = 0;
                float discountone = 0;
                float discounttwo = 0;

                string machinarycosttitle = "";
                string materialcosttitle = "";
                string maintenancecosttitle = "";
                string extraonecosttitle = "";
                string extratwocosttitle = "";
                string discountonetitle = "";
                string discounttwotitle = "";

                bool Extradatacheck = false;
                bool ExtraDataSTcheck = false;

                bool STMachinary = false;
                bool STMaterial = false;
                bool STMaintenance = false;
                bool STExtraone = false;
                bool STExtratwo = false;

                bool SCMachinary = false;
                bool SCMaterial = false;
                bool SCMaintenance = false;
                bool SCExtraone = false;
                bool SCExtratwo = false;

                bool STDiscountone = false;
                bool STDiscounttwo = false;

                string strExtradatacheck = "";
                string strExtrastcheck = "";

                string strSTMachinary = "";
                string strSTMaterial = "";
                string strSTMaintenance = "";
                string strSTExtraone = "";
                string strSTExtratwo = "";

                string strSCMachinary = "";
                string strSCMaterial = "";
                string strSCMaintenance = "";
                string strSCExtraone = "";
                string strSCExtratwo = "";

                string strSTDiscountone = "";
                string strSTDiscounttwo = "";

                float staxamtonservicecharge = 0;



                #endregion

                if (DtBilling.Rows.Count > 0)
                {
                    BillNo = DtBilling.Rows[0]["billno"].ToString();
                    BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                    // DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());

                    #region Begin New code for values taken from database as on 11/03/2014 by venkat

                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalChrg"].ToString()) == false)
                    {
                        totalamount = float.Parse(DtBilling.Rows[0]["TotalChrg"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrg"].ToString()) == false)
                    {
                        servicecharge = float.Parse(DtBilling.Rows[0]["ServiceChrg"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrgPer"].ToString()) == false)
                    {
                        servicechargePer = float.Parse(DtBilling.Rows[0]["ServiceChrgPer"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                    {
                        servicetax = float.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                    {
                        kkcess = float.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
                    }

                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                    {
                        sbcess = float.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["CESS"].ToString()) == false)
                    {
                        cess = float.Parse(DtBilling.Rows[0]["CESS"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SHECess"].ToString()) == false)
                    {
                        shecess = float.Parse(DtBilling.Rows[0]["SHECess"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["GrandTotal"].ToString()) == false)
                    {
                        Grandtotal = float.Parse(DtBilling.Rows[0]["GrandTotal"].ToString());
                    }


                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["MachinaryCost"].ToString()) == false)
                    {
                        machinarycost = float.Parse(DtBilling.Rows[0]["MachinaryCost"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["MaterialCost"].ToString()) == false)
                    {
                        materialcost = float.Parse(DtBilling.Rows[0]["MaterialCost"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ElectricalChrg"].ToString()) == false)
                    {
                        maintenancecost = float.Parse(DtBilling.Rows[0]["ElectricalChrg"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtone"].ToString()) == false)
                    {
                        extraonecost = float.Parse(DtBilling.Rows[0]["ExtraAmtone"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtTwo"].ToString()) == false)
                    {
                        extratwocost = float.Parse(DtBilling.Rows[0]["ExtraAmtTwo"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discount"].ToString()) == false)
                    {
                        discountone = float.Parse(DtBilling.Rows[0]["Discount"].ToString());
                    }
                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discounttwo"].ToString()) == false)
                    {
                        discounttwo = float.Parse(DtBilling.Rows[0]["Discounttwo"].ToString());
                    }

                    //machinarycosttitle = DtBilling.Rows[0]["Machinarycosttitle"].ToString();
                    //materialcosttitle = DtBilling.Rows[0]["Materialcosttitle"].ToString();
                    //maintenancecosttitle = DtBilling.Rows[0]["Maintanancecosttitle"].ToString();
                    //extraonecosttitle = DtBilling.Rows[0]["Extraonetitle"].ToString();
                    //extratwocosttitle = DtBilling.Rows[0]["Extratwotitle"].ToString();
                    //discountonetitle = DtBilling.Rows[0]["Discountonetitle"].ToString();
                    //discounttwotitle = DtBilling.Rows[0]["Discounttwotitle"].ToString();

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["Extradatacheck"].ToString()) == false)
                    //{
                    //    strExtradatacheck = DtBilling.Rows[0]["Extradatacheck"].ToString();
                    //    if (strExtradatacheck == "True")
                    //    {
                    //        Extradatacheck = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraDataSTcheck"].ToString()) == false)
                    //{
                    //    strExtrastcheck = DtBilling.Rows[0]["ExtraDataSTcheck"].ToString();
                    //    if (strExtrastcheck == "True")
                    //    {
                    //        ExtraDataSTcheck = true;
                    //    }
                    //}



                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMachinary"].ToString()) == false)
                    //{
                    //    strSTMachinary = DtBilling.Rows[0]["STMachinary"].ToString();
                    //    if (strSTMachinary == "True")
                    //    {
                    //        STMachinary = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaterial"].ToString()) == false)
                    //{
                    //    strSTMaterial = DtBilling.Rows[0]["STMaterial"].ToString();
                    //    if (strSTMaterial == "True")
                    //    {
                    //        STMaterial = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaintenance"].ToString()) == false)
                    //{
                    //    strSTMaintenance = DtBilling.Rows[0]["STMaintenance"].ToString();
                    //    if (strSTMaintenance == "True")
                    //    {
                    //        STMaintenance = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtraone"].ToString()) == false)
                    //{
                    //    strSTExtraone = DtBilling.Rows[0]["STExtraone"].ToString();
                    //    if (strSTExtraone == "True")
                    //    {
                    //        STExtraone = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtratwo"].ToString()) == false)
                    //{
                    //    strSTExtratwo = DtBilling.Rows[0]["STExtratwo"].ToString();
                    //    if (strSTExtratwo == "True")
                    //    {
                    //        STExtratwo = true;
                    //    }
                    //}


                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMachinary"].ToString()) == false)
                    //{
                    //    strSCMachinary = DtBilling.Rows[0]["SCMachinary"].ToString();
                    //    if (strSCMachinary == "True")
                    //    {
                    //        SCMachinary = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaterial"].ToString()) == false)
                    //{
                    //    strSCMaterial = DtBilling.Rows[0]["SCMaterial"].ToString();
                    //    if (strSCMaterial == "True")
                    //    {
                    //        SCMaterial = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaintenance"].ToString()) == false)
                    //{
                    //    strSCMaintenance = DtBilling.Rows[0]["SCMaintenance"].ToString();
                    //    if (strSCMaintenance == "True")
                    //    {
                    //        SCMaintenance = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtraone"].ToString()) == false)
                    //{
                    //    strSCExtraone = DtBilling.Rows[0]["SCExtraone"].ToString();
                    //    if (strSCExtraone == "True")
                    //    {
                    //        SCExtraone = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtratwo"].ToString()) == false)
                    //{
                    //    strSCExtratwo = DtBilling.Rows[0]["SCExtratwo"].ToString();
                    //    if (strSCExtratwo == "True")
                    //    {
                    //        SCExtratwo = true;
                    //    }
                    //}


                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                    //{
                    //    strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                    //    if (strSTDiscountone == "True")
                    //    {
                    //        STDiscountone = true;
                    //    }
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                    //{
                    //    strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                    //    if (strSTDiscounttwo == "True")
                    //    {
                    //        STDiscounttwo = true;
                    //    }
                    //}


                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax75"].ToString()) == false)
                    //{
                    //    ServiceTax75 = float.Parse(DtBilling.Rows[0]["ServiceTax75"].ToString());
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax25"].ToString()) == false)
                    //{
                    //    ServiceTax25 = float.Parse(DtBilling.Rows[0]["ServiceTax25"].ToString());
                    //}

                    //if (String.IsNullOrEmpty(DtBilling.Rows[0]["Staxonservicecharge"].ToString()) == false)
                    //{
                    //    staxamtonservicecharge = float.Parse(DtBilling.Rows[0]["Staxonservicecharge"].ToString());
                    //}

                    #endregion
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                    return;
                }
                string Year = DateTime.Now.Year.ToString();

                PdfPCell cell11 = new PdfPCell(new Paragraph("To,", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell11.Border = 0;
                tempTable1.AddCell(cell11);
                string addressData = "";
                //addressData = dtclientaddress.Rows[0]["clientname"].ToString();
                //if (addressData.Trim().Length > 0)
                //{
                //    PdfPCell clientname = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 12, Font.BOLD, BaseColor.BLACK)));
                //    clientname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    clientname.Border = 0;
                //    tempTable1.AddCell(clientname);
                //}
                addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString();
                if (addressData.Trim().Length > 0)
                {
                    PdfPCell clientaddrhno = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clientaddrhno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //clientaddrhno.Colspan = 0;
                    clientaddrhno.Border = 0;
                    tempTable1.AddCell(clientaddrhno);
                }
                addressData = dtclientaddress.Rows[0]["ClientAddrStreet"].ToString();
                if (addressData.Trim().Length > 0)
                {
                    PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    clientstreet.Border = 0;
                    tempTable1.AddCell(clientstreet);
                }


                addressData = dtclientaddress.Rows[0]["ClientAddrArea"].ToString();
                if (addressData.Trim().Length > 0)
                {
                    PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    clientstreet.Border = 0;
                    tempTable1.AddCell(clientstreet);
                }


                addressData = dtclientaddress.Rows[0]["ClientAddrColony"].ToString();
                if (addressData.Trim().Length > 0)
                {
                    PdfPCell clientcolony = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clientcolony.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    clientcolony.Colspan = 2;
                    clientcolony.Border = 0;
                    tempTable1.AddCell(clientcolony);
                }
                addressData = dtclientaddress.Rows[0]["ClientAddrcity"].ToString();
                if (addressData.Trim().Length > 0)
                {
                    PdfPCell clientcity = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clientcity.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    clientcity.Colspan = 2;
                    clientcity.Border = 0;
                    tempTable1.AddCell(clientcity);
                }
                addressData = dtclientaddress.Rows[0]["ClientAddrstate"].ToString();
                if (addressData.Trim().Length > 0)
                {
                    PdfPCell clientstate = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clientstate.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    clientstate.Colspan = 2;
                    clientstate.Border = 0;
                    tempTable1.AddCell(clientstate);
                }
                addressData = dtclientaddress.Rows[0]["ClientAddrpin"].ToString();
                if (addressData.Trim().Length > 0)
                {
                    PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    clietnpin.Colspan = 2;
                    clietnpin.Border = 0;
                    tempTable1.AddCell(clietnpin);
                }
                PdfPCell childTable1 = new PdfPCell(tempTable1);
                childTable1.Border = 0;
                childTable1.HorizontalAlignment = 0;
                address.AddCell(childTable1);

                PdfPTable tempTable2 = new PdfPTable(1);
                tempTable2.TotalWidth = 250f;
                tempTable2.LockedWidth = true;
                float[] tempWidth2 = new float[] { 1f };
                tempTable2.SetWidths(tempWidth2);


                PdfPCell cell12 = new PdfPCell(new Paragraph("Invoice No: " + BillNo, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell12.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell12.Border = 0;

                tempTable2.AddCell(cell12);
                PdfPCell cell13 = new PdfPCell(new Paragraph("Date: " + BillDate.Day.ToString("00") + "/" + BillDate.ToString("MM") + "/" +
                    BillDate.Year, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell13.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell13.Border = 0;
                tempTable2.AddCell(cell13);


                PdfPCell cell14 = new PdfPCell(new Paragraph("For Month: " +
                    GetMonthName() + " - " + GetMonthOfYear() +
                    "      ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell14.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell14.Border = 0;
                tempTable2.AddCell(cell14);

                PdfPCell childTable2 = new PdfPCell(tempTable2);
                childTable2.Border = 0;
                childTable2.HorizontalAlignment = 0;
                address.AddCell(childTable2);
                address.AddCell(celll);


                document.Add(address);

                PdfPTable bodytablelogo = new PdfPTable(2);
                bodytablelogo.TotalWidth = 500f;//600f
                bodytablelogo.LockedWidth = true;
                float[] widthlogo = new float[] { 2f, 2f };
                bodytablelogo.SetWidths(widthlogo);

                PdfPCell cell9 = new PdfPCell(new Phrase("Unit Name : " + dtclientaddress.Rows[0]["clientname"].ToString(),
                    FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell9.Colspan = 2;
                cell9.Border = 0;
                bodytablelogo.AddCell(cell9);

                string Fromdate = txtfromdate.Text;
                string Todate = txttodate.Text;

                PdfPCell cell10 = new PdfPCell(new Phrase("Bill period : " + Fromdate + "  to  " +
                    Todate + " ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell10.Colspan = 2;
                cell10.Border = 0;
                bodytablelogo.AddCell(cell10);
                bodytablelogo.AddCell(celll);

                PdfPCell cell19 = new PdfPCell(new Phrase("Dear Sir, ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell19.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell19.Colspan = 2;
                cell19.Border = 0;
                bodytablelogo.AddCell(cell19);
                bodytablelogo.AddCell(celll);

                PdfPCell cell20 = new PdfPCell(new Phrase(strDescription, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell20.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell20.Colspan = 2;
                cell20.Border = 0;
                bodytablelogo.AddCell(cell20);
                bodytablelogo.AddCell(celll);
                PdfPCell cell21 = new PdfPCell(new Phrase("The Details are given below : ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell21.Colspan = 1;
                cell21.Border = 0;
                bodytablelogo.AddCell(cell21);
                bodytablelogo.AddCell(celll);
                // bodytablelogo.AddCell(celll);
                document.Add(bodytablelogo);
                int colCount = 6;// gvClientBilling.Columns.Count;
                //Create a table

                PdfPTable table = new PdfPTable(colCount);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.HorizontalAlignment = 1;

                //create an array to store column widths
                // int[] colWidths = new int[5];
                float[] colWidths = new float[] { 4.6f, 1.2f, 1.8f, 1.2f, 1.2f, 2.4f };
                table.SetWidths(colWidths);
                PdfPCell cell;
                string cellText;
                //create the header row
                for (int colIndex = 1; colIndex < 7; colIndex++)
                {
                    //set the column width
                    if (colIndex < 3)
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[colIndex].Text);
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                    }
                    if (colIndex == 3)
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[colIndex].Text);
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                    }
                    if (colIndex == 4)
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[4].Text);
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 2;
                        table.AddCell(cell);
                    }

                    if (colIndex == 6)
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = Server.HtmlDecode(gvClientBilling.HeaderRow.Cells[7].Text);
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                    }

                }
                ////export rows from GridView to table
                for (int rowIndex = 0; rowIndex < gvClientBilling.Rows.Count; rowIndex++)
                {
                    if (gvClientBilling.Rows[rowIndex].RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddldutytype = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddldutytype"));
                        TextBox lblamount = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtAmount"));
                        if (lblamount != null)
                        {
                            string dtype = ddldutytype.Text;
                            string strAmount = lblamount.Text;
                            float amount = 0;
                            if (strAmount.Length > 0)
                                amount = Convert.ToSingle(strAmount);
                            if (amount >= 0)
                            {
                                for (int j = 1; j < 7; j++)
                                {
                                    //fetch the column value of the current row
                                    if (j == 1)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtgvdesgn"));

                                        string summaryQry = "select summary from contractdetails " +
                                            "  where clientid='" + ddlclientid.SelectedValue + "' and Designations='" + label1.Text + "'";
                                        DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(summaryQry).Result;
                                        cellText = label1.Text;
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["summary"].ToString().Trim().Length > 0)
                                                cellText += " (" + dt.Rows[0]["summary"].ToString() + ")";
                                        }

                                        //create a new cell with column value
                                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        table.AddCell(cell);
                                    }

                                    if (j == 2)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtnoofemployees"));
                                        if (label1.Text == "0")
                                        {
                                            cellText = "";
                                        }
                                        else
                                        {
                                            cellText = label1.Text;
                                        }
                                        //create a new cell with column value
                                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        table.AddCell(cell);
                                    }
                                    if (j == 3)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtNoOfDuties"));
                                        cellText = label1.Text;
                                        //create a new cell with column value
                                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        table.AddCell(cell);
                                    }
                                    if (j == 4)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtPayRate"));
                                        cellText = label1.Text;

                                        if (cellText == "0")
                                        {
                                            cellText = "";
                                        }

                                        //create a new cell with column value
                                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.Colspan = 2;
                                        table.AddCell(cell);
                                    }
                                    //if (j == 5)
                                    //{
                                    //    DropDownList label1 = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddldutytype"));
                                    //    cellText = label1.SelectedItem.Text;
                                    //    //create a new cell with column value
                                    //    cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    //    cell.HorizontalAlignment = 2;
                                    //    table.AddCell(cell);
                                    //}
                                    if (j == 6)
                                    {
                                        TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtAmount"));
                                        cellText = label1.Text;

                                        if (cellText == "0")
                                        {
                                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 2;
                                            table.AddCell(cell);
                                        }
                                        else
                                        {
                                            cell = new PdfPCell(new Phrase(float.Parse(cellText).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 2;
                                            table.AddCell(cell);
                                        }
                                        //create a new cell with column value

                                    }
                                }
                            }
                        }
                    }
                }
                document.Add(table);

                tablelogo.AddCell(celll);

                PdfPTable tabled = new PdfPTable(colCount);
                tabled.TotalWidth = 500;//432f;
                tabled.LockedWidth = true;
                float[] widthd = new float[] { 4.6f, 1.2f, 1.8f, 1.2f, 1.2f, 2.4f };
                tabled.SetWidths(widthd);

                PdfPCell celldz1 = new PdfPCell(new Phrase("Total ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                celldz1.Colspan = 5;
                tabled.AddCell(celldz1);

                PdfPCell celldz4 = new PdfPCell(new Phrase(" " + totalamount.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                celldz4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                tabled.AddCell(celldz4);

                string SqlQryForTaxes = "select * from  Tbloptions ";
                DataTable DtTaxes = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForTaxes).Result;
                string SCPersent = "";
                if (DtTaxes.Rows.Count > 0)
                {
                    SCPersent = DtTaxes.Rows[0]["ServiceTaxSeparate"].ToString();
                }
                else
                {
                    lblResult.Text = "There Is No Tax Values For Generating Bills ";
                    return;
                }
                if (servicecharge > 0)//bSCType == true)
                {
                    float scharge = servicecharge;
                    if (scharge > 0)
                    {
                        PdfPCell celldc2 = new PdfPCell(new Phrase("Service Charges @ ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldc2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldc2.Colspan = 4;
                        tabled.AddCell(celldc2);

                        string SCharge = "";
                        if (bSCType == false)
                        {
                            SCharge = servicechargePer.ToString() + " %";
                        }
                        else
                        {
                            SCharge = servicechargePer.ToString();
                        }
                        PdfPCell celldc3 = new PdfPCell(new Phrase(SCharge, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldc3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldc3);

                        PdfPCell celldc4 = new PdfPCell(new Phrase(servicecharge.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldc4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldc4);
                    }
                }

                #region When Extra data is checked and STcheck is true


                if (Extradatacheck == true)
                {
                    //float machineryCostwithst = 0;
                    //if (lblMachinerywithst.Text.Length > 0)
                    //    machineryCostwithst = Convert.ToSingle(lblMachinerywithst.Text);
                    if (ExtraDataSTcheck == true)
                    {
                        if (machinarycost > 0)
                        {
                            if (STMachinary == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }

                        //float materialcostwithst = 0;
                        //if (lblMaterialwithst.Text.Length > 0)
                        //    materialcostwithst = Convert.ToSingle(lblMaterialwithst.Text);
                        if (materialcost > 0)
                        {
                            if (STMaterial == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }


                        //float electricalcostwithst = 0;
                        //if (lblElectricalwithst.Text.Length > 0)
                        //    electricalcostwithst = Convert.ToSingle(lblElectricalwithst.Text);
                        if (maintenancecost > 0)
                        {
                            if (STMaintenance == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }

                        //float extraamtwithst = 0;
                        //if (lblextraonewithst.Text.Length > 0)
                        //    extraamtwithst = Convert.ToSingle(lblextraonewithst.Text);
                        if (extraonecost > 0)
                        {
                            if (STExtraone == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                        //float Extraamtwithst1 = 0;
                        //if (lblextratwowithst.Text.Length > 0)
                        //    Extraamtwithst1 = Convert.ToSingle(lblextratwowithst.Text);
                        if (extratwocost > 0)
                        {
                            if (STExtratwo == true)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 6;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                    }

                }

                #endregion


                if (servicetax > 0)
                {
                    string scpercent = "";
                    if (bST75 == true)
                    {
                        scpercent = "3";
                    }
                    else
                    {
                        scpercent = SCPersent;
                    }

                    PdfPCell celldd2 = new PdfPCell(new Phrase("Service Tax @ " + scpercent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    celldd2.Colspan = 5;
                    tabled.AddCell(celldd2);

                    PdfPCell celldd4 = new PdfPCell(new Phrase(servicetax.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    tabled.AddCell(celldd4);


                    if (sbcess > 0)
                    {

                        string SBCESSPresent = DtTaxes.Rows[0]["sbcess"].ToString();
                        PdfPCell cellde2 = new PdfPCell(new Phrase("Swachh Bharat Cess @ " + SBCESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellde2.Colspan = 5;
                        tabled.AddCell(cellde2);



                        PdfPCell cellde4 = new PdfPCell(new Phrase(sbcess.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(cellde4);

                    }


                    if (kkcess > 0)
                    {

                        string KKCESSPresent = DtTaxes.Rows[0]["KKcess"].ToString();
                        PdfPCell cellde2 = new PdfPCell(new Phrase("Krishi Kalyan Cess @ " + KKCESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellde2.Colspan = 5;
                        tabled.AddCell(cellde2);


                        PdfPCell cellde4 = new PdfPCell(new Phrase(kkcess.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(cellde4);

                    }

                    if (cess > 0)
                    {

                        string CESSPresent = DtTaxes.Rows[0]["Cess"].ToString();
                        PdfPCell cellde2 = new PdfPCell(new Phrase("CESS @ " + CESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellde2.Colspan = 5;
                        tabled.AddCell(cellde2);


                        PdfPCell cellde4 = new PdfPCell(new Phrase(cess.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        cellde4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(cellde4);

                    }

                    if (shecess > 0)
                    {
                        string SHECESSPresent = DtTaxes.Rows[0]["shecess"].ToString();
                        PdfPCell celldf2 = new PdfPCell(new Phrase("S&H Ed.CESS @ " + SHECESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldf2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldf2.Colspan = 4;
                        tabled.AddCell(celldf2);




                        PdfPCell celldf4 = new PdfPCell(new Phrase((servicetax * (double.Parse(SHECESSPresent) / 100)).ToString("0.00"),
                            FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldf4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldf4);
                    }

                }
                //if (bST75)
                //{
                //    if (ServiceTax75 > 0)
                //    {
                //        PdfPCell celldMeci1 = new PdfPCell(new Phrase("Less 75% Service Tax ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                //        celldMeci1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //        //celld7.Border = 1;
                //        celldMeci1.Colspan = 4;
                //        tabled.AddCell(celldMeci1);

                //        PdfPCell celldMeci3 = new PdfPCell(new Phrase(ServiceTax75.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                //        celldMeci3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //        tabled.AddCell(celldMeci3);
                //    }
                //    if (ServiceTax25 > 0)
                //    {

                //        PdfPCell cellST25h = new PdfPCell(new Phrase("Service Tax Chargable @3.09% ", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                //        cellST25h.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //        //celld7.Border = 1;
                //        cellST25h.Colspan = 4;
                //        tabled.AddCell(cellST25h);

                //        PdfPCell cellST25d = new PdfPCell(new Phrase(ServiceTax25.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                //        cellST25d.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //        tabled.AddCell(cellST25d);
                //    }


                //}

                #region When Extradata check is false and STcheck is false


                if (Extradatacheck == true)
                {
                    if (ExtraDataSTcheck == false)
                    {
                        if (machinarycost > 0)
                        {
                            PdfPCell celldMeci1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMeci1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMeci1.Colspan = 5;
                            tabled.AddCell(celldMeci1);

                            PdfPCell celldMeci3 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMeci3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMeci3);
                        }

                        if (materialcost > 0)
                        {
                            PdfPCell celldMt1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 5;
                            tabled.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMt3);
                        }
                        if (maintenancecost > 0)
                        {
                            PdfPCell celldMt1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 5;
                            tabled.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMt3);
                        }

                        if (extraonecost > 0)
                        {
                            PdfPCell celldMt1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 5;
                            tabled.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMt3);
                        }
                        if (extratwocost > 0)
                        {
                            PdfPCell celldMt1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 5;
                            tabled.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tabled.AddCell(celldMt3);
                        }

                    }

                    if (ExtraDataSTcheck == true)
                    {
                        if (machinarycost > 0)
                        {
                            if (STMachinary == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                        if (materialcost > 0)
                        {
                            if (STMaterial == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }


                        if (maintenancecost > 0)
                        {
                            if (STMaintenance == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }

                        if (extraonecost > 0)
                        {
                            if (STExtraone == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                        if (extratwocost > 0)
                        {
                            if (STExtratwo == false)
                            {
                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 5;
                                tabled.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                tabled.AddCell(celldcst2);
                            }
                        }
                    }

                    if (discountone > 0)
                    {
                        PdfPCell celldMt1 = new PdfPCell(new Phrase(discountonetitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldMt1.Colspan = 5;
                        tabled.AddCell(celldMt1);

                        PdfPCell celldMt3 = new PdfPCell(new Phrase(discountone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldMt3);
                    }
                    if (discounttwo > 0)
                    {
                        PdfPCell celldMt1 = new PdfPCell(new Phrase(discounttwotitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldMt1.Colspan = 5;
                        tabled.AddCell(celldMt1);

                        PdfPCell celldMt3 = new PdfPCell(new Phrase(discounttwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        tabled.AddCell(celldMt3);
                    }
                }
                #endregion

                PdfPCell cellremarks = new PdfPCell(new Phrase("Remarks : " + Remarks, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cellremarks.HorizontalAlignment = 0;
                cellremarks.Colspan = 3;//0=Left, 1=Centre, 2=Right
                tabled.AddCell(cellremarks);

                // PdfPCell cellremarks1 = new PdfPCell(new Phrase(Remarks, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                // cellremarks1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                ////  cellremarks1.Colspan = 1;
                // tabled.AddCell(cellremarks1);

                PdfPCell celldg6 = new PdfPCell(new Phrase("Grand Total(Rs.)", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                celldg6.HorizontalAlignment = 2;
                celldg6.Colspan = 2;//0=Left, 1=Centre, 2=Right
                tabled.AddCell(celldg6);

                PdfPCell celldg8 = new PdfPCell(new Phrase(Grandtotal.ToString("#,##"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                celldg8.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                celldg8.Colspan = 1;
                tabled.AddCell(celldg8);
                document.Add(tabled);

                PdfPTable tablecon = new PdfPTable(2);
                tablecon.TotalWidth = 500f;
                tablecon.LockedWidth = true;
                float[] widthcon = new float[] { 2f, 3.5f };
                tablecon.SetWidths(widthcon);




                PdfPCell cellBreak = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 15, Font.NORMAL, BaseColor.BLACK)));
                cellBreak.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellBreak.Colspan = 2;
                cellBreak.BorderWidthBottom = 0;
                cellBreak.BorderWidthLeft = .5f;
                cellBreak.BorderWidthTop = 0;
                cellBreak.BorderWidthRight = .5f;
                //cellBreak.Border = 0;
                tablecon.AddCell(cellBreak);

                string Amountinwords = NumberToEnglish.Instance.changeNumericToWords(Grandtotal.ToString());

                PdfPCell cellcamt = new PdfPCell(new Phrase(" In Words: Rupees " + Amountinwords.Trim() + " Only",
                    FontFactory.GetFont(FontStyle, 10, Font.BOLDITALIC, BaseColor.BLACK)));
                cellcamt.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cellcamt.Colspan = 2;
                cellcamt.BorderWidthBottom = 0;
                cellcamt.BorderWidthLeft = .5f;
                cellcamt.BorderWidthTop = 0;
                cellcamt.BorderWidthRight = .5f;
                //cellcamt.Border = 1;
                tablecon.AddCell(cellcamt);
                tablecon.AddCell(cellBreak);

                string Servicetax = string.Empty;
                string PANNO = string.Empty;
                string PFNo = string.Empty;
                string Esino = string.Empty;
                string PTno = string.Empty;
                string Notes = string.Empty;

                if (compInfo.Rows.Count > 0)
                {
                    Servicetax = compInfo.Rows[0]["BillNotes"].ToString();
                    PANNO = compInfo.Rows[0]["Labourrule"].ToString();
                    PFNo = compInfo.Rows[0]["PFNo"].ToString();
                    Esino = compInfo.Rows[0]["ESINo"].ToString();
                    PTno = compInfo.Rows[0]["bankname"].ToString();
                    Notes = compInfo.Rows[0]["Notes"].ToString();

                }

                if (Servicetax.Trim().Length > 0)
                {
                    PdfPCell cellc6 = new PdfPCell(new Phrase("SERVICE TAX NO: " + Servicetax, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    cellc6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellc6.Colspan = 7;
                    cellc6.BorderWidthBottom = 0;
                    cellc6.BorderWidthLeft = .5f;
                    cellc6.BorderWidthTop = .5f;
                    cellc6.BorderWidthRight = .5f;
                    //cellc6.Border = 0;
                    tablecon.AddCell(cellc6);
                }

                //var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                //var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                //var phrase = new Phrase();
                //phrase.Add(new Chunk("REASON(S) FOR CANCELLATION:", boldFont));
                //phrase.Add(new Chunk(" See Statutoryreason(s) designated by Code No(s) 1 on the reverse side hereof", normalFont));

                if (PANNO.Trim().Length > 0)
                {
                    PdfPCell cellc7 = new PdfPCell(new Phrase("PAN NO: " + PANNO, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    cellc7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellc7.Colspan = 7;
                    cellc7.BorderWidthBottom = .5f;
                    cellc7.BorderWidthLeft = .5f;
                    cellc7.BorderWidthTop = 0;
                    cellc7.BorderWidthRight = .5f;
                    //cellc7.Border = 0;
                    tablecon.AddCell(cellc7);
                }
                if (PFNo.Trim().Length > 0)
                {
                    PdfPCell Pfno = new PdfPCell(new Phrase("EPF NO: " + PFNo, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    Pfno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    Pfno.Colspan = 7;
                    Pfno.BorderWidthBottom = .5f;
                    Pfno.BorderWidthLeft = .5f;
                    Pfno.BorderWidthTop = 0;
                    Pfno.BorderWidthRight = .5f;
                    //Pfno.Border = 0;
                    tablecon.AddCell(Pfno);
                }

                if (Esino.Trim().Length > 0)
                {
                    PdfPCell ESino = new PdfPCell(new Phrase("ESIC NO: " + Esino, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    ESino.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    ESino.Colspan = 7;
                    ESino.BorderWidthBottom = .5f;
                    ESino.BorderWidthLeft = .5f;
                    ESino.BorderWidthTop = 0;
                    ESino.BorderWidthRight = .5f;
                    //ESino.Border = 0;
                    tablecon.AddCell(ESino);
                }

                if (PTno.Trim().Length > 0)
                {
                    PdfPCell Ptno = new PdfPCell(new Phrase("P Tax No: " + PTno, FontFactory.GetFont(FontStyle, 9, Font.BOLD, BaseColor.BLACK)));
                    Ptno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    Ptno.Colspan = 7;
                    Ptno.BorderWidthBottom = .5f;
                    Ptno.BorderWidthLeft = .5f;
                    Ptno.BorderWidthTop = 0;
                    Ptno.BorderWidthRight = .5f;
                    //Ptno.Border = 0;
                    tablecon.AddCell(Ptno);
                }

                PdfPCell cellspace = new PdfPCell(new Paragraph(" ", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                cellspace.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cellspace.Border = 0;
                cellspace.Colspan = 2;
                tablecon.AddCell(cellspace);
                //tablecon.AddCell(cellspace);



                if (Notes.Trim().Length > 0)
                {

                    //PdfPCell note = new PdfPCell(new Phrase("Terms & Conditions:", FontFactory.GetFont(FontStyle, 9, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
                    //note.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //note.Colspan = 7;
                    //note.Border = 0;
                    //tablecon.AddCell(note);

                    PdfPCell note1 = new PdfPCell(new Phrase(Notes.ToString(), FontFactory.GetFont(FontStyle, 8, Font.BOLD, BaseColor.BLACK)));
                    note1.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                    note1.Colspan = 7;
                    note1.Border = 0;
                    note1.SetLeading(0, 1.5f);
                    tablecon.AddCell(note1);

                }


                PdfPCell cellc41 = new PdfPCell(new Phrase("For " + companyName, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cellc41.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cellc41.Colspan = 7;
                cellc41.Border = 0;
                cellc41.PaddingTop = 10;
                tablecon.AddCell(cellc41);

                PdfPCell cellc4 = new PdfPCell(new Phrase("Authorized Signatory", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cellc4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cellc4.Colspan = 7;
                cellc4.Border = 0;
                cellc4.PaddingTop = 30;
                tablecon.AddCell(cellc4);


                document.Add(tablecon);
                document.NewPage();
                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
                //}
                //catch (Exception ex)
                //{
                //    //LblResult.Text = ex.Message;
                //}
            }
            else
            {
                // LblResult.Text = "There is no bill generated for selected client";
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There is no bill generated for selected client ');", true);

            }
        }


        protected void btninvoiceNew_Click(object sender, EventArgs e)
        {
            int month = 0;
            month = GetMonthBasedOnSelectionDateorMonth();
            if (gvClientBilling.Rows.Count > 0)
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    Document document = new Document(PageSize.A4);
                    Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);

                    PdfWriter writer = PdfWriter.GetInstance(document, ms);



                    document.Open();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    string strQry = "Select * from CompanyInfo   where   branchid='" + BranchID + "'";
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
                    string Servicetax = "";
                    string ServiceText = "";
                    string Category = "";
                    if (compInfo.Rows.Count > 0)
                    {
                        companyName = compInfo.Rows[0]["CompanyName"].ToString();
                        companyAddress = compInfo.Rows[0]["Address"].ToString();
                        //companyAddress = companyAddress.Replace("\r\n", string.Empty);
                        companyaddressline = compInfo.Rows[0]["Addresslineone"].ToString();
                        //CINNO = compInfo.Rows[0]["CINNO"].ToString();
                        PANNO = compInfo.Rows[0]["Labourrule"].ToString();
                        PFNo = compInfo.Rows[0]["PFNo"].ToString();
                        Esino = compInfo.Rows[0]["ESINo"].ToString();
                        Servicetax = compInfo.Rows[0]["BillNotes"].ToString();
                        emailid = compInfo.Rows[0]["Emailid"].ToString();
                        website = compInfo.Rows[0]["Website"].ToString();
                        phoneno = compInfo.Rows[0]["Phoneno"].ToString();
                        Category = compInfo.Rows[0]["Category"].ToString();
                    }


                    DateTime DtLastDay = DateTime.Now;
                    if (Chk_Month.Checked == false)
                    {
                        DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                    }
                    if (Chk_Month.Checked == true)
                    {
                        DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                    }
                    var ContractID = "";


                    #region  Begin Get Contract Id Based on The Last Day

                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                    if (DTContractID.Rows.Count > 0)
                    {
                        ContractID = DTContractID.Rows[0]["contractid"].ToString();

                    }
                    #endregion


                    DataTable dtheadings = null;
                    var SPNameD = "GetInvHeadings";
                    Hashtable htheadings = new Hashtable();
                    htheadings.Add("@clientid", ddlclientid.SelectedValue);
                    // htheadings.Add("@LastDay", DtLastDay);
                    dtheadings = config.ExecuteAdaptorAsyncWithParams(SPNameD, htheadings).Result;

                    string InvDescription = "";
                    string InvNoOfEmps = "";
                    string InvNoofDuties = "";
                    string InvPayrate = "";
                    string InvAmount = "";
                    string InvDescriptionVisible = "N";
                    string InvNoOfEmpsVisible = "N";
                    string InvNoofDutiesVisible = "N";
                    string InvPayrateVisible = "N";
                    string InvAmountVisible = "N";


                    if (dtheadings.Rows.Count > 0)
                    {
                        InvDescription = dtheadings.Rows[0]["InvDescription"].ToString();
                        InvNoOfEmps = dtheadings.Rows[0]["InvNoOfEmps"].ToString();
                        InvNoofDuties = dtheadings.Rows[0]["InvNoofDuties"].ToString();
                        InvPayrate = dtheadings.Rows[0]["InvPayrate"].ToString();
                        InvAmount = dtheadings.Rows[0]["InvAmount"].ToString();
                        InvDescriptionVisible = dtheadings.Rows[0]["InvDescriptionVisible"].ToString();
                        InvNoOfEmpsVisible = dtheadings.Rows[0]["InvNoOfEmpsVisible"].ToString();
                        InvNoofDutiesVisible = dtheadings.Rows[0]["InvNoofDutiesVisible"].ToString();
                        InvPayrateVisible = dtheadings.Rows[0]["InvPayrateVisible"].ToString();
                        InvAmountVisible = dtheadings.Rows[0]["InvAmountVisible"].ToString();
                    }





                    //

                    string SqlQuryForServiCharge = "select ContractId,servicecharge, convert(nvarchar(20), ContractStartDate, 103) as ContractStartDate,ServiceChargeType,Description,IncludeST,ServiceTax75,Pono,typeofwork from contracts where " +
                        " clientid ='" + ddlclientid.SelectedValue + "' and ContractId='" + ContractID + "'";
                    DataTable DtServicecharge = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuryForServiCharge).Result;
                    string Typeofwork = "";
                    string ServiceCharge = "0";
                    string strSCType = "";
                    string strDescription = "We are presenting our bill for the House Keeping Services Provided at your establishment. Kindly release the payment at the earliest";
                    bool bSCType = false;
                    string strIncludeST = "";
                    string ContractStartDate = "";
                    string strST75 = "";
                    bool bIncludeST = false;
                    bool bST75 = false;
                    string POContent = "";
                    // string ServiceTaxCategory = "";


                    string Invoicedesc = "select Description,Remarks from MUnitBillBreakup where UnitId='" + ddlclientid.SelectedItem.ToString() + "' and MunitIdBillno='" + ddlMBBillnos.SelectedValue + "' and  month='" + month + "'";
                    DataTable dtinvoicedesc = config.ExecuteAdaptorAsyncWithQueryParams(Invoicedesc).Result;
                    string tempDescription = dtinvoicedesc.Rows[0]["Description"].ToString();
                    string Remarks = dtinvoicedesc.Rows[0]["Remarks"].ToString();

                    if (tempDescription.Trim().Length > 0)
                    {
                        strDescription = tempDescription;
                    }
                    if (DtServicecharge.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceCharge"].ToString()) == false)
                        {
                            ServiceCharge = DtServicecharge.Rows[0]["ServiceCharge"].ToString();
                        }
                        if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceChargeType"].ToString()) == false)
                        {
                            strSCType = DtServicecharge.Rows[0]["ServiceChargeType"].ToString();
                        }

                        if (strSCType.Length > 0)
                        {
                            bSCType = Convert.ToBoolean(strSCType);
                        }
                        strIncludeST = DtServicecharge.Rows[0]["IncludeST"].ToString();
                        strST75 = DtServicecharge.Rows[0]["ServiceTax75"].ToString();
                        ContractStartDate = DtServicecharge.Rows[0]["ContractStartDate"].ToString();
                        if (strIncludeST == "True")
                        {
                            bIncludeST = true;
                        }
                        if (strST75 == "True")
                        {
                            bST75 = true;
                        }
                        POContent = DtServicecharge.Rows[0]["pono"].ToString();
                        Typeofwork = DtServicecharge.Rows[0]["typeofwork"].ToString();
                        // ServiceTaxCategory = DtServicecharge.Rows[0]["ServiceTaxCategory"].ToString();
                    }
                    document.AddTitle(companyName);
                    document.AddAuthor("DIYOS");
                    document.AddSubject("Invoice");
                    document.AddKeywords("Keyword1, keyword2, …");
                    string imagepath = Server.MapPath("~/assets/" + CmpIDPrefix + "Billlogo.png");
                    string imagepath1 = Server.MapPath("~/assets/BillLogo1.png");

                    if (File.Exists(imagepath))
                    {
                        iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);

                        gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                        // gif2.SpacingBefore = 50;
                        gif2.ScalePercent(60f);
                        gif2.SetAbsolutePosition(27f, 757f);
                        //document.Add(new Paragraph(" "));
                        document.Add(gif2);
                    }

                    if (File.Exists(imagepath1))
                    {
                        iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath1);

                        gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                        // gif2.SpacingBefore = 50;
                        gif2.ScalePercent(60f);
                        gif2.SetAbsolutePosition(510f, 743f);
                        //document.Add(new Paragraph(" "));
                        document.Add(gif2);
                    }


                    PdfPTable tablelogo = new PdfPTable(2);
                    tablelogo.TotalWidth = 560f;
                    tablelogo.LockedWidth = true;
                    float[] widtlogo = new float[] { 2f, 2f };
                    tablelogo.SetWidths(widtlogo);


                    PdfPCell CCompName = new PdfPCell(new Paragraph(companyName, FontFactory.GetFont(FontStyle, 12, Font.BOLD, BaseColor.BLACK)));
                    CCompName.HorizontalAlignment = 1;
                    CCompName.Colspan = 2;
                    CCompName.PaddingLeft = 50;
                    CCompName.BorderWidthBottom = 0;
                    CCompName.BorderWidthTop = 1.5f;
                    CCompName.BorderWidthRight = 1.5f;
                    CCompName.BorderWidthLeft = 1.5f;
                    tablelogo.AddCell(CCompName);

                    PdfPCell CCompAddress = new PdfPCell(new Paragraph(companyAddress, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    CCompAddress.HorizontalAlignment = 1;
                    CCompAddress.BorderWidthBottom = 0;
                    CCompAddress.BorderWidthTop = 0;
                    CCompAddress.BorderWidthRight = 1.5f;
                    CCompAddress.BorderWidthLeft = 1.5f;
                    CCompAddress.Colspan = 2;
                    CCompAddress.PaddingLeft = 50;
                    // CCompAddress.FixedHeight = 0;
                    CCompAddress.SetLeading(0f, 1.3f);
                    tablelogo.AddCell(CCompAddress);


                    PdfPCell Celemail = new PdfPCell(new Paragraph("Email :" + emailid + " | " + "Website :" + website, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    Celemail.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Celemail.BorderWidthBottom = 0.5f;
                    Celemail.BorderWidthTop = 0;
                    Celemail.BorderWidthRight = 1.5f;
                    Celemail.BorderWidthLeft = 1.5f;
                    Celemail.Colspan = 2;
                    Celemail.FixedHeight = 20;
                    tablelogo.AddCell(Celemail);


                    //For Space

                    PdfPCell celll = new PdfPCell(new Paragraph("\n", FontFactory.GetFont(FontStyle, 12, Font.NORMAL, BaseColor.BLACK)));
                    celll.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                    celll.Colspan = 2;
                    //tablelogo.AddCell(celll);

                    // tablelogo.AddCell(celll);

                    PdfPCell CInvoice = new PdfPCell(new Paragraph("INVOICE", FontFactory.GetFont(FontStyle, 18, Font.BOLD, BaseColor.BLACK)));
                    CInvoice.HorizontalAlignment = 1;
                    CInvoice.BorderWidthBottom = 0.5f;
                    CInvoice.BorderWidthTop = 0;
                    CInvoice.BorderWidthRight = 1.5f;
                    CInvoice.BorderWidthLeft = 1.5f;
                    CInvoice.Colspan = 2;
                    tablelogo.AddCell(CInvoice);

                    //tablelogo.AddCell(celll);

                    document.Add(tablelogo);

                    PdfPTable address = new PdfPTable(5);
                    address.TotalWidth = 560f;
                    address.LockedWidth = true;
                    float[] addreslogo = new float[] { 2f, 2f, 2f, 2f, 2f };
                    address.SetWidths(addreslogo);

                    PdfPTable tempTable1 = new PdfPTable(3);
                    tempTable1.TotalWidth = 336f;
                    tempTable1.LockedWidth = true;
                    float[] tempWidth1 = new float[] { 2f, 2f, 2f };
                    tempTable1.SetWidths(tempWidth1);

                    #region

                    string selectclientaddress = "select sg.segname,c.* from clients c inner join Segments sg on c.ClientSegment = sg.SegId where clientid= '" + ddlclientid.SelectedItem.ToString() + "'";
                    DataTable dtclientaddress = config.ExecuteAdaptorAsyncWithQueryParams(selectclientaddress).Result;


                    string SelectBillNo = "Select * from MUnitBill where BillNo= '" + ddlMBBillnos.SelectedValue + "' and  month='" + month + "' and unitid='" + ddlclientid.SelectedValue + "' ";
                    DataTable DtBilling = config.ExecuteAdaptorAsyncWithQueryParams(SelectBillNo).Result;

                    string BillNo = "";
                    string DisplayBillNo = "";
                    string area = "";
                    string location = "";
                    if (dtclientaddress.Rows.Count > 0)
                    {
                        area = dtclientaddress.Rows[0]["segname"].ToString();
                        location = dtclientaddress.Rows[0]["location"].ToString();

                    }

                    DateTime BillDate;
                    DateTime DueDate;

                    #region Variables for data Fields as on 11/03/2014 by venkat


                    float servicecharge = 0;
                    float servicechargeper = 0;
                    float servicetax = 0;
                    float cess = 0;
                    float sbcess = 0;
                    float kkcess = 0;
                    float shecess = 0;
                    float totalamount = 0;
                    float Grandtotal = 0;

                    float ServiceTax75 = 0;
                    float ServiceTax25 = 0;

                    float machinarycost = 0;
                    float materialcost = 0;
                    float maintenancecost = 0;
                    float extraonecost = 0;
                    float extratwocost = 0;
                    float discountone = 0;
                    float discounttwo = 0;

                    string machinarycosttitle = "";
                    string materialcosttitle = "";
                    string maintenancecosttitle = "";
                    string extraonecosttitle = "";
                    string extratwocosttitle = "";
                    string discountonetitle = "";
                    string discounttwotitle = "";

                    bool Extradatacheck = false;
                    bool ExtraDataSTcheck = false;

                    bool STMachinary = false;
                    bool STMaterial = false;
                    bool STMaintenance = false;
                    bool STExtraone = false;
                    bool STExtratwo = false;

                    bool SCMachinary = false;
                    bool SCMaterial = false;
                    bool SCMaintenance = false;
                    bool SCExtraone = false;
                    bool SCExtratwo = false;

                    bool STDiscountone = false;
                    bool STDiscounttwo = false;

                    string strExtradatacheck = "";
                    string strExtrastcheck = "";

                    string strSTMachinary = "";
                    string strSTMaterial = "";
                    string strSTMaintenance = "";
                    string strSTExtraone = "";
                    string strSTExtratwo = "";

                    string strSCMachinary = "";
                    string strSCMaterial = "";
                    string strSCMaintenance = "";
                    string strSCExtraone = "";
                    string strSCExtratwo = "";

                    string strSTDiscountone = "";
                    string strSTDiscounttwo = "";

                    float staxamtonservicecharge = 0;
                    float RelChrgAmt = 0;


                    #endregion

                    if (DtBilling.Rows.Count > 0)
                    {
                        BillNo = DtBilling.Rows[0]["billno"].ToString();
                        BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                        // DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());

                        #region Begin New code for values taken from database as on 11/03/2014 by venkat

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalChrg"].ToString()) == false)
                        {
                            totalamount = float.Parse(DtBilling.Rows[0]["TotalChrg"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrg"].ToString()) == false)
                        {
                            servicecharge = float.Parse(DtBilling.Rows[0]["ServiceChrg"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrgPer"].ToString()) == false)
                        {
                            servicechargeper = float.Parse(DtBilling.Rows[0]["ServiceChrgPer"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                        {
                            servicetax = float.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                        {
                            kkcess = float.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
                        }

                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                        {
                            sbcess = float.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["CESS"].ToString()) == false)
                        {
                            cess = float.Parse(DtBilling.Rows[0]["CESS"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["SHECess"].ToString()) == false)
                        {
                            shecess = float.Parse(DtBilling.Rows[0]["SHECess"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["GrandTotal"].ToString()) == false)
                        {
                            Grandtotal = float.Parse(DtBilling.Rows[0]["GrandTotal"].ToString());
                        }


                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["MachinaryCost"].ToString()) == false)
                        {
                            machinarycost = float.Parse(DtBilling.Rows[0]["MachinaryCost"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["MaterialCost"].ToString()) == false)
                        {
                            materialcost = float.Parse(DtBilling.Rows[0]["MaterialCost"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ElectricalChrg"].ToString()) == false)
                        {
                            maintenancecost = float.Parse(DtBilling.Rows[0]["ElectricalChrg"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtone"].ToString()) == false)
                        {
                            extraonecost = float.Parse(DtBilling.Rows[0]["ExtraAmtone"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtTwo"].ToString()) == false)
                        {
                            extratwocost = float.Parse(DtBilling.Rows[0]["ExtraAmtTwo"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discount"].ToString()) == false)
                        {
                            discountone = float.Parse(DtBilling.Rows[0]["Discount"].ToString());
                        }
                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discounttwo"].ToString()) == false)
                        {
                            discounttwo = float.Parse(DtBilling.Rows[0]["Discounttwo"].ToString());
                        }

                        //machinarycosttitle = DtBilling.Rows[0]["Machinarycosttitle"].ToString();
                        //materialcosttitle = DtBilling.Rows[0]["Materialcosttitle"].ToString();
                        //maintenancecosttitle = DtBilling.Rows[0]["Maintanancecosttitle"].ToString();
                        //extraonecosttitle = DtBilling.Rows[0]["Extraonetitle"].ToString();
                        //extratwocosttitle = DtBilling.Rows[0]["Extratwotitle"].ToString();
                        //discountonetitle = DtBilling.Rows[0]["Discountonetitle"].ToString();
                        //discounttwotitle = DtBilling.Rows[0]["Discounttwotitle"].ToString();

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["Extradatacheck"].ToString()) == false)
                        //{
                        //    strExtradatacheck = DtBilling.Rows[0]["Extradatacheck"].ToString();
                        //    if (strExtradatacheck == "True")
                        //    {
                        //        Extradatacheck = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraDataSTcheck"].ToString()) == false)
                        //{
                        //    strExtrastcheck = DtBilling.Rows[0]["ExtraDataSTcheck"].ToString();
                        //    if (strExtrastcheck == "True")
                        //    {
                        //        ExtraDataSTcheck = true;
                        //    }
                        //}



                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMachinary"].ToString()) == false)
                        //{
                        //    strSTMachinary = DtBilling.Rows[0]["STMachinary"].ToString();
                        //    if (strSTMachinary == "True")
                        //    {
                        //        STMachinary = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaterial"].ToString()) == false)
                        //{
                        //    strSTMaterial = DtBilling.Rows[0]["STMaterial"].ToString();
                        //    if (strSTMaterial == "True")
                        //    {
                        //        STMaterial = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaintenance"].ToString()) == false)
                        //{
                        //    strSTMaintenance = DtBilling.Rows[0]["STMaintenance"].ToString();
                        //    if (strSTMaintenance == "True")
                        //    {
                        //        STMaintenance = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtraone"].ToString()) == false)
                        //{
                        //    strSTExtraone = DtBilling.Rows[0]["STExtraone"].ToString();
                        //    if (strSTExtraone == "True")
                        //    {
                        //        STExtraone = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtratwo"].ToString()) == false)
                        //{
                        //    strSTExtratwo = DtBilling.Rows[0]["STExtratwo"].ToString();
                        //    if (strSTExtratwo == "True")
                        //    {
                        //        STExtratwo = true;
                        //    }
                        //}


                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMachinary"].ToString()) == false)
                        //{
                        //    strSCMachinary = DtBilling.Rows[0]["SCMachinary"].ToString();
                        //    if (strSCMachinary == "True")
                        //    {
                        //        SCMachinary = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaterial"].ToString()) == false)
                        //{
                        //    strSCMaterial = DtBilling.Rows[0]["SCMaterial"].ToString();
                        //    if (strSCMaterial == "True")
                        //    {
                        //        SCMaterial = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaintenance"].ToString()) == false)
                        //{
                        //    strSCMaintenance = DtBilling.Rows[0]["SCMaintenance"].ToString();
                        //    if (strSCMaintenance == "True")
                        //    {
                        //        SCMaintenance = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtraone"].ToString()) == false)
                        //{
                        //    strSCExtraone = DtBilling.Rows[0]["SCExtraone"].ToString();
                        //    if (strSCExtraone == "True")
                        //    {
                        //        SCExtraone = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtratwo"].ToString()) == false)
                        //{
                        //    strSCExtratwo = DtBilling.Rows[0]["SCExtratwo"].ToString();
                        //    if (strSCExtratwo == "True")
                        //    {
                        //        SCExtratwo = true;
                        //    }
                        //}


                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                        //{
                        //    strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                        //    if (strSTDiscountone == "True")
                        //    {
                        //        STDiscountone = true;
                        //    }
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                        //{
                        //    strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                        //    if (strSTDiscounttwo == "True")
                        //    {
                        //        STDiscounttwo = true;
                        //    }
                        //}


                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax75"].ToString()) == false)
                        //{
                        //    ServiceTax75 = float.Parse(DtBilling.Rows[0]["ServiceTax75"].ToString());
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax25"].ToString()) == false)
                        //{
                        //    ServiceTax25 = float.Parse(DtBilling.Rows[0]["ServiceTax25"].ToString());
                        //}

                        //if (String.IsNullOrEmpty(DtBilling.Rows[0]["Staxonservicecharge"].ToString()) == false)
                        //{
                        //    staxamtonservicecharge = float.Parse(DtBilling.Rows[0]["Staxonservicecharge"].ToString());
                        //}

                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                        return;
                    }
                    string Year = DateTime.Now.Year.ToString();

                    PdfPCell mress = new PdfPCell(new Paragraph("Bill-To-Party", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    mress.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    mress.BorderWidthBottom = 0;
                    mress.BorderWidthTop = 0.5f;
                    mress.BorderWidthLeft = 1.5f;
                    mress.BorderWidthRight = 0.5f;
                    //tempTable1.AddCell(mress);

                    string addressData = "";
                    //addressData = dtclientaddress.Rows[0]["clientname"].ToString();
                    //if (addressData.Trim().Length > 0)
                    //{
                    //    PdfPCell clientname = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 12, Font.BOLD, BaseColor.BLACK)));
                    //    clientname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //    clientname.Border = 0;
                    //    tempTable1.AddCell(clientname);
                    //}


                    addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientaddrhno = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        clientaddrhno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientaddrhno.Colspan = 3;                                 //clientaddrhno.Colspan = 0;
                        clientaddrhno.BorderWidthBottom = 0;
                        clientaddrhno.BorderWidthTop = 0;
                        clientaddrhno.BorderWidthLeft = 1.5f;
                        clientaddrhno.BorderWidthRight = 0.5f;
                        //clientaddrhno.clientaddrhno = 20;
                        tempTable1.AddCell(clientaddrhno);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrStreet"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstreet.BorderWidthBottom = 0;
                        clientstreet.BorderWidthTop = 0;
                        clientstreet.Colspan = 3;
                        clientstreet.BorderWidthLeft = 1.5f;
                        clientstreet.BorderWidthRight = 0.5f;
                        //clientstreet.PaddingLeft = 20;
                        tempTable1.AddCell(clientstreet);
                    }


                    addressData = dtclientaddress.Rows[0]["ClientAddrArea"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstreet.BorderWidthBottom = 0;
                        clientstreet.BorderWidthTop = 0;
                        clientstreet.Colspan = 3;

                        clientstreet.BorderWidthLeft = 1.5f;
                        clientstreet.BorderWidthRight = 0.5f;
                        // clientstreet.PaddingLeft = 20;
                        tempTable1.AddCell(clientstreet);
                    }


                    addressData = dtclientaddress.Rows[0]["ClientAddrColony"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientcolony = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        clientcolony.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientcolony.Colspan = 3;
                        clientcolony.BorderWidthBottom = 0;
                        clientcolony.BorderWidthTop = 0;
                        clientcolony.BorderWidthLeft = 1.5f;
                        clientcolony.BorderWidthRight = 0.5f;
                        //clientcolony.PaddingLeft = 20;
                        tempTable1.AddCell(clientcolony);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrcity"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientcity = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        clientcity.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientcity.Colspan = 3;
                        clientcity.BorderWidthBottom = 0;
                        clientcity.BorderWidthTop = 0;
                        clientcity.BorderWidthLeft = 1.5f;
                        clientcity.BorderWidthRight = 0.5f;
                        //  clientcity.PaddingLeft = 20;
                        tempTable1.AddCell(clientcity);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrstate"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clientstate = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        clientstate.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clientstate.Colspan = 3;
                        clientstate.BorderWidthBottom = 0;
                        clientstate.BorderWidthTop = 0;
                        clientstate.BorderWidthLeft = 1.5f;
                        clientstate.BorderWidthRight = 0.5f;
                        // clientstate.PaddingLeft = 20;
                        tempTable1.AddCell(clientstate);
                    }
                    addressData = dtclientaddress.Rows[0]["ClientAddrpin"].ToString();
                    if (addressData.Trim().Length > 0)
                    {
                        PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        clietnpin.Colspan = 3;
                        clietnpin.BorderWidthBottom = 0;
                        clietnpin.BorderWidthTop = 0;
                        clietnpin.BorderWidthLeft = 1.5f;
                        clietnpin.BorderWidthRight = 0.5f;
                        //  clietnpin.PaddingLeft = 20;
                        tempTable1.AddCell(clietnpin);
                    }
                    #endregion

                    PdfPCell childTable1 = new PdfPCell(tempTable1);
                    childTable1.Border = 0;
                    childTable1.Colspan = 3;
                    // childTable1.FixedHeight = 100;
                    childTable1.HorizontalAlignment = 0;
                    address.AddCell(childTable1);

                    PdfPTable tempTable2 = new PdfPTable(2);
                    tempTable2.TotalWidth = 224f;
                    tempTable2.LockedWidth = true;
                    float[] tempWidth2 = new float[] { 1f, 1f };
                    tempTable2.SetWidths(tempWidth2);



                    var phrase = new Phrase();
                    phrase.Add(new Chunk("Invoice No :  ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    phrase.Add(new Chunk("     " + BillNo, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    PdfPCell cell13 = new PdfPCell();
                    cell13.AddElement(phrase);
                    cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell13.BorderWidthBottom = 0;
                    cell13.BorderWidthTop = 0;
                    cell13.Colspan = 2;
                    cell13.BorderWidthLeft = 0;
                    cell13.BorderWidthRight = 1.5f;
                    cell13.PaddingTop = -5;
                    // cell13.PaddingLeft = 20;
                    tempTable2.AddCell(cell13);


                    var phrase2 = new Phrase();
                    phrase2.Add(new Chunk("Invoice Date :  ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    phrase2.Add(new Chunk("  " + BillDate.Day.ToString("00") + "/" + BillDate.ToString("MM") + "/" +
                        BillDate.Year, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    PdfPCell cell14 = new PdfPCell();
                    cell14.AddElement(phrase2);
                    cell14.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell14.BorderWidthBottom = 0;
                    cell14.BorderWidthTop = 0;
                    cell14.Colspan = 2;
                    cell14.BorderWidthLeft = 0;
                    cell14.BorderWidthRight = 1.5f;
                    cell14.PaddingTop = -5;
                    //cell14.PaddingLeft = 20;
                    tempTable2.AddCell(cell14);

                    var phrase3 = new Phrase();
                    phrase3.Add(new Chunk("Invoice Month :  ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    phrase3.Add(new Chunk("" + GetMonthName() + "'" + GetMonthOfYear(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    PdfPCell cell15xc = new PdfPCell();
                    cell15xc.AddElement(phrase3);
                    cell15xc.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell15xc.BorderWidthBottom = 0;
                    cell15xc.BorderWidthTop = 0;
                    cell15xc.BorderWidthLeft = 0;
                    cell15xc.BorderWidthRight = 1.5f;
                    cell15xc.PaddingTop = -5;
                    cell15xc.Colspan = 2;
                    tempTable2.AddCell(cell15xc);


                    var phrase4 = new Phrase();
                    phrase4.Add(new Chunk("Area :  ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    phrase4.Add(new Chunk("                " + area, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    PdfPCell cell15xcc = new PdfPCell();
                    cell15xcc.AddElement(phrase4);
                    cell15xcc.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell15xcc.BorderWidthBottom = 0;
                    cell15xcc.BorderWidthTop = 0;
                    cell15xcc.BorderWidthLeft = 0;
                    cell15xcc.BorderWidthRight = 1.5f;
                    cell15xcc.PaddingTop = -5;
                    cell15xcc.Colspan = 2;
                    tempTable2.AddCell(cell15xcc);


                    PdfPCell childTable2 = new PdfPCell(tempTable2);
                    childTable2.Border = 0;
                    childTable2.Colspan = 2;
                    //childTable2.FixedHeight = 100;
                    childTable2.HorizontalAlignment = 0;
                    address.AddCell(childTable2);
                    // address.AddCell(celll);


                    document.Add(address);




                    PdfPTable address1 = new PdfPTable(1);
                    address1.TotalWidth = 560f;
                    address1.LockedWidth = true;
                    float[] addreslogo1 = new float[] { 2f };
                    address1.SetWidths(addreslogo1);


                    PdfPCell cellser = new PdfPCell(new Phrase(strDescription, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellser.HorizontalAlignment = 0;
                    cellser.BorderWidthBottom = 0.5f;
                    cellser.BorderWidthLeft = 1.5f;
                    cellser.BorderWidthTop = 0.5f;
                    cellser.BorderWidthRight = 1.5f;
                    // cellser.FixedHeight = 30;
                    address1.AddCell(cellser);


                    document.Add(address1);


                    #region


                    int hcount = 0;//gvClientBilling.Columns.Count.

                    if (InvDescriptionVisible == "Y")
                    {
                        hcount = 1;
                    }

                    if (InvNoOfEmpsVisible == "Y")
                    {
                        hcount += 1;
                    }

                    if (InvNoofDutiesVisible == "Y")
                    {
                        hcount += 1;
                    }

                    if (InvPayrateVisible == "Y")
                    {
                        hcount += 1;
                    }

                    if (InvAmountVisible == "Y")
                    {
                        hcount += 1;
                    }


                    int colCount = hcount;



                    //int colCount = 5;// gvClientBilling.Columns.Count;
                    //Create a table

                    PdfPTable table = new PdfPTable(colCount);
                    table.TotalWidth = 560f;
                    table.LockedWidth = true;
                    table.HorizontalAlignment = 1;


                    //create an array to store column widths
                    // int[] colWidths = new int[5];
                    float[] colWidths = new float[colCount];

                    if (hcount == 5)
                    {
                        colWidths = new float[] { 5.5f, 2.4f, 2.4f, 2.4f, 1.8f };
                    }

                    if (hcount == 4)
                    {
                        colWidths = new float[] { 7.9f, 2.4f, 2.4f, 1.8f };
                    }

                    if (hcount == 3)
                    {
                        colWidths = new float[] { 10.3f, 2.4f, 1.8f };
                    }

                    if (hcount == 2)
                    {
                        colWidths = new float[] { 12.7f, 1.8f };
                    }

                    table.SetWidths(colWidths);
                    PdfPCell cell;
                    string cellText;


                    //create the header row


                    if (InvDescriptionVisible == "Y")
                    {


                        cell = new PdfPCell(new Phrase(InvDescription, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = .5f;
                        cell.BorderWidthLeft = 1.5f;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = .5f;
                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    if (InvNoOfEmpsVisible == "Y")
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cell = new PdfPCell(new Phrase(InvNoOfEmps, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = .5f;
                        cell.BorderWidthLeft = .5f;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0;
                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    if (InvPayrateVisible == "Y")
                    {
                        cell = new PdfPCell(new Phrase(InvPayrate, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = .5f;
                        cell.BorderWidthLeft = .5f;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = .5f;
                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    if (InvNoofDutiesVisible == "Y")
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cell = new PdfPCell(new Phrase(InvNoofDuties, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        //cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = .5f;
                        cell.BorderWidthLeft = .5f;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = .5f;
                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }


                    if (InvAmountVisible == "Y")
                    {
                        // colWidths[colIndex] = (int)gvClientBilling.Columns[colIndex].ItemStyle.Width.Value;
                        //fetch the header text
                        cellText = "Amount(Rs)";
                        //create a new cell with header text
                        cell = new PdfPCell(new Phrase(InvAmount, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        //set the background color for the header cell
                        cell.HorizontalAlignment = 1;
                        //cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = .5f;
                        cell.BorderWidthLeft = .5f;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 1.5f;
                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }


                    ////export rows from GridView to table
                    for (int rowIndex = 0; rowIndex < gvClientBilling.Rows.Count; rowIndex++)
                    {
                        if (gvClientBilling.Rows[rowIndex].RowType == DataControlRowType.DataRow)
                        //gvClientBilling.RowStyle.BorderColor = System.Drawing.Color.Gray;
                        {
                            TextBox lblamount = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtAmount"));
                            if (lblamount != null)
                            {
                                string strAmount = lblamount.Text;
                                float amount = 0;
                                if (strAmount.Length > 0)
                                    amount = Convert.ToSingle(strAmount);
                                //if (amount >= 0)
                                {
                                    //for (int j = 1; j < 6; j++)
                                    {
                                        //fetch the column value of the current row
                                        //if (j == 1)
                                        //{
                                        //    TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtgvdesgn"));

                                        //    string summaryQry = "select summary from contractdetails " +
                                        //        "  where clientid='" + ddlclientid.SelectedValue + "' and Designations='" + label1.Text + "'";
                                        //    DataTable dt = SqlHelper.Instance.GetTableByQuery(summaryQry);
                                        //    cellText = label1.Text;
                                        //    if (dt.Rows.Count > 0)
                                        //    {
                                        //        if (dt.Rows[0]["summary"].ToString().Trim().Length > 0)
                                        //            cellText += " (" + dt.Rows[0]["summary"].ToString() + ")";
                                        //    }

                                        //    //create a new cell with column value
                                        //    cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        //    table.AddCell(cell);
                                        //}


                                        if (InvDescriptionVisible == "Y")
                                        {
                                            //TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtnoofemployees"));
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtgvdesgn"));
                                            if (label1.Text == "0")
                                            {
                                                cellText = "";
                                            }
                                            else
                                            {
                                                cellText = label1.Text;
                                            }
                                            //create a new cell with column value
                                            cell = new PdfPCell(new Phrase(cellText + "\n\n", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 0;
                                            cell.Colspan = 1;

                                            cell.BorderWidthLeft = 1.5f;
                                            table.AddCell(cell);
                                        }
                                        if (InvNoOfEmpsVisible == "Y")
                                        {
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtnoofemployees"));

                                            if (label1.Text == "0")
                                            {
                                                cellText = "";
                                            }
                                            else
                                            {
                                                cellText = label1.Text;
                                            }
                                            //create a new cell with column value
                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.Colspan = 1;
                                            table.AddCell(cell);
                                        }
                                        if (InvPayrateVisible == "Y")
                                        {
                                            // DropDownList label1 = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddldutytype"));
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtPayRate"));

                                            //  cellText = label1.SelectedItem.Text;
                                            //create a new cell with column value
                                            if (label1.Text == "0")
                                            {
                                                cellText = "";
                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.Colspan = 1;
                                                table.AddCell(cell);
                                            }
                                            else
                                            {
                                                cellText = label1.Text;
                                                cell = new PdfPCell(new Phrase(float.Parse(cellText).ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.Colspan = 1;
                                                table.AddCell(cell);
                                            }

                                        }
                                        if (InvNoofDutiesVisible == "Y")
                                        {
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtNoOfDuties"));

                                            cellText = label1.Text;

                                            if (cellText == "0")
                                            {
                                                cellText = "";
                                            }

                                            //create a new cell with column value
                                            cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            //cell.Colspan = 2;
                                            cell.Colspan = 1;
                                            table.AddCell(cell);
                                        }

                                        //if (j == 5)
                                        //{
                                        //    DropDownList label1 = (DropDownList)(gvClientBilling.Rows[rowIndex].FindControl("ddldutytype"));
                                        //    cellText = label1.SelectedItem.Text;
                                        //    //create a new cell with column value
                                        //    cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        //    cell.HorizontalAlignment = 2;
                                        //    cell.Colspan = 2;
                                        //    table.AddCell(cell);
                                        //}
                                        if (InvAmountVisible == "Y")
                                        {
                                            TextBox label1 = (TextBox)(gvClientBilling.Rows[rowIndex].FindControl("txtAmount"));
                                            cellText = label1.Text;

                                            if (cellText == "0")
                                            {
                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.Colspan = 1;
                                                cell.BorderWidthRight = 1.5f;
                                                table.AddCell(cell);
                                            }
                                            else
                                            {
                                                cell = new PdfPCell(new Phrase(float.Parse(cellText).ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.Colspan = 1;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.BorderWidthBottom = 0;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0.5f;

                                                table.AddCell(cell);
                                            }
                                            //create a new cell with column value

                                        }
                                    }
                                }
                            }
                        }
                    }
                    document.Add(table);

                    // tablelogo.AddCell(celll);

                    PdfPTable tabled = new PdfPTable(5);
                    tabled.TotalWidth = 560;//432f;
                    tabled.LockedWidth = true;
                    float[] widthdnew = new float[] { 5.5f, 2.4f, 2.4f, 2.4f, 1.8f };
                    tabled.SetWidths(widthdnew);

                    PdfPTable tempTable111 = new PdfPTable(2);
                    tempTable111.TotalWidth = 305f;
                    tempTable111.LockedWidth = true;
                    float[] tempWidth111 = new float[] { 5.5f, 2.4f };
                    tempTable111.SetWidths(tempWidth111);

                    string Fromdate = txtfromdate.Text;
                    string Todate = txttodate.Text;

                    //note column

                    PdfPCell CCMTS1 = new PdfPCell(new Phrase(Typeofwork + " rendered at your premises " + location + " from " + Fromdate + " to " + Todate, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    CCMTS1.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                    CCMTS1.Colspan = 2;
                    //cellemptyval.MinimumHeight = 50;
                    CCMTS1.BorderWidthBottom = 0;
                    CCMTS1.BorderWidthLeft = 1.5f;
                    CCMTS1.BorderWidthTop = 0;
                    CCMTS1.BorderWidthRight = .5f;
                    CCMTS1.SetLeading(0, 1.3f);
                    //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                    tempTable111.AddCell(CCMTS1);

                    PdfPCell childTable111 = new PdfPCell(tempTable111);
                    childTable111.Border = 0;
                    childTable111.Colspan = 2;
                    childTable111.HorizontalAlignment = 0;
                    tabled.AddCell(childTable111);


                    PdfPTable tempTable222 = new PdfPTable(3);
                    tempTable222.TotalWidth = 255f;
                    tempTable222.LockedWidth = true;
                    float[] tempWidth222 = new float[] { 2.4f, 2.4f, 1.8f };
                    tempTable222.SetWidths(tempWidth222);


                    #region
                    if (RelChrgAmt > 0)
                    {


                        PdfPCell celldz5 = new PdfPCell(new Phrase("1/6 Reliever Charges", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldz5.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldz5.Colspan = 2;
                        celldz5.BorderWidthBottom = 0;
                        celldz5.BorderWidthLeft = .5f;
                        celldz5.BorderWidthTop = 0;
                        celldz5.BorderWidthRight = .5f;
                        tempTable222.AddCell(celldz5);

                        PdfPCell celldz6 = new PdfPCell(new Phrase(" " + RelChrgAmt.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        celldz6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldz6.BorderWidthBottom = 0;
                        celldz6.BorderWidthLeft = .5f;
                        celldz6.BorderWidthTop = 0;
                        celldz6.BorderWidthRight = 1.5f;
                        tempTable222.AddCell(celldz6);
                    }

                    #region When Extradata check is false and STcheck is false




                    if (Extradatacheck == true)
                    {
                        if (machinarycost > 0)
                        {
                            if (STMachinary == false)
                            {
                                if (SCMachinary == false)
                                {



                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = .5f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }
                        if (materialcost > 0)
                        {
                            if (STMaterial == false)
                            {
                                if (SCMaterial == false)
                                {


                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = .5f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }


                        if (maintenancecost > 0)
                        {
                            if (STMaintenance == false)
                            {
                                if (SCMaintenance == false)
                                {



                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = .5f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }

                        if (extraonecost > 0)
                        {
                            if (STExtraone == false)
                            {
                                if (SCExtraone == false)
                                {



                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    // celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = .5f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }
                        if (extratwocost > 0)
                        {
                            if (STExtratwo == false)
                            {
                                if (SCExtratwo == false)
                                {



                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    // celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = .5f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }

                        if (discountone > 0)
                        {




                            PdfPCell celldMt1 = new PdfPCell(new Phrase(discountonetitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.Colspan = 2;
                            celldMt1.BorderWidthBottom = 0;
                            celldMt1.BorderWidthLeft = .5f;
                            celldMt1.BorderWidthTop = 0;
                            celldMt1.BorderWidthRight = .5f;
                            // celldMt1.BorderColor = BaseColor.LIGHT_GRAY;
                            tempTable222.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(discountone.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt3.BorderWidthBottom = 0;
                            celldMt3.BorderWidthLeft = .5f;
                            celldMt3.BorderWidthTop = 0;
                            celldMt3.BorderWidthRight = 1.5f;
                            // celldMt3.BorderColor = BaseColor.LIGHT_GRAY;
                            tempTable222.AddCell(celldMt3);
                        }
                        if (discounttwo > 0)
                        {


                            PdfPCell celldMt1 = new PdfPCell(new Phrase(discounttwotitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt1.BorderWidthBottom = 0;
                            celldMt1.BorderWidthLeft = .5f;
                            celldMt1.BorderWidthTop = 0;
                            celldMt1.BorderWidthRight = .5f;
                            //celldMt1.BorderColor = BaseColor.LIGHT_GRAY;
                            celldMt1.Colspan = 2;
                            tempTable222.AddCell(celldMt1);

                            PdfPCell celldMt3 = new PdfPCell(new Phrase(discounttwo.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldMt3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldMt3.BorderWidthBottom = 0;
                            celldMt3.BorderWidthLeft = .5f;
                            celldMt3.BorderWidthTop = 0;
                            celldMt3.BorderWidthRight = 1.5f;
                            // celldMt3.BorderColor = BaseColor.LIGHT_GRAY;
                            tempTable222.AddCell(celldMt3);
                        }
                    }
                    #endregion
                    #endregion









                    PdfPCell celldz1 = new PdfPCell(new Phrase("Sub Total", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    celldz1.Colspan = 2;
                    celldz1.BorderWidthBottom = 0;
                    celldz1.BorderWidthLeft = .5f;
                    celldz1.BorderWidthTop = 0;
                    celldz1.BorderWidthRight = 0.5f;
                    //celldz1.BorderColor = BaseColor.LIGHT_GRAY;
                    tempTable222.AddCell(celldz1);

                    PdfPCell celldz4 = new PdfPCell(new Phrase(" " + totalamount.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    celldz4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    celldz4.BorderWidthBottom = 0;
                    celldz4.BorderWidthLeft = 0.5f;
                    celldz4.BorderWidthTop = 0.5f;
                    celldz4.BorderWidthRight = 1.5f;
                    //celldz4.BorderColor = BaseColor.LIGHT_GRAY;
                    tempTable222.AddCell(celldz4);

                    #region

                    string SqlQryForTaxes = "select * from  Tbloptions ";
                    DataTable DtTaxes = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForTaxes).Result;
                    string SCPersent = "";
                    if (DtTaxes.Rows.Count > 0)
                    {
                        SCPersent = DtTaxes.Rows[0]["ServiceTaxSeparate"].ToString();
                    }
                    else
                    {
                        lblResult.Text = "There Is No Tax Values For Generating Bills ";
                        return;
                    }

                    if (machinarycost > 0)
                    {
                        if (STMachinary == true)
                        {
                            if (SCMachinary == true)
                            {

                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }

                        // bool SCMachinary = false;
                        //bool SCMaterial = false;
                        // bool SCMaintenance = false;
                        //bool SCExtraone = false;
                        //bool SCExtratwo = false;


                    }
                    if (materialcost > 0)
                    {
                        if (STMaterial == true)
                        {
                            if (SCMaterial == true)
                            {



                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }
                    }
                    if (maintenancecost > 0)
                    {
                        if (STMaintenance == true)
                        {
                            if (SCMaintenance == true)
                            {

                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }
                    }

                    if (extraonecost > 0)
                    {
                        if (STExtraone == true)
                        {
                            if (SCExtraone == true)
                            {

                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }
                    }
                    if (extratwocost > 0)
                    {
                        if (STExtratwo == true)
                        {
                            if (SCExtratwo == true)
                            {



                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }
                    }
                    #region When Extra data is checked and STcheck is false and SCcheck is true

                    if (machinarycost > 0)
                    {
                        if (STMachinary == false)
                        {
                            if (SCMachinary == true)
                            {


                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }

                        // bool SCMachinary = false;
                        //bool SCMaterial = false;
                        // bool SCMaintenance = false;
                        //bool SCExtraone = false;
                        //bool SCExtratwo = false;


                    }
                    if (materialcost > 0)
                    {
                        if (STMaterial == false)
                        {
                            if (SCMaterial == true)
                            {



                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }
                    }
                    if (maintenancecost > 0)
                    {
                        if (STMaintenance == false)
                        {
                            if (SCMaintenance == true)
                            {

                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }
                    }

                    if (extraonecost > 0)
                    {
                        if (STExtraone == false)
                        {
                            if (SCExtraone == true)
                            {


                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }
                    }
                    if (extratwocost > 0)
                    {
                        if (STExtratwo == false)
                        {
                            if (SCExtratwo == true)
                            {



                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldcst1.Colspan = 2;
                                tempTable222.AddCell(celldcst1);

                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                //celldcst2.BorderWidthBottom = .5f;
                                //celldcst2.BorderWidthLeft = 0;
                                //celldcst2.BorderWidthTop = .5f;
                                //celldcst2.BorderWidthRight = .5f;
                                // celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                tempTable222.AddCell(celldcst2);
                            }
                        }
                    }

                    #endregion
                    if (servicecharge > 0)//bSCType == true)
                    {
                        float scharge = servicecharge;
                        if (scharge > 0)
                        {
                            string SCharge = "";
                            if (bSCType == false)
                            {
                                SCharge = ServiceCharge + " %";
                            }
                            else
                            {
                                SCharge = ServiceCharge;
                            }




                            PdfPCell celldc2 = new PdfPCell(new Phrase("Service Charges @ " + servicechargeper + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldc2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldc2.Colspan = 2;
                            celldc2.BorderWidthBottom = 0;
                            celldc2.BorderWidthLeft = .5f;
                            celldc2.BorderWidthTop = 0;
                            celldc2.BorderWidthRight = .5f;
                            //celldc2.BorderColor = BaseColor.LIGHT_GRAY;
                            tempTable222.AddCell(celldc2);


                            PdfPCell celldc4 = new PdfPCell(new Phrase(servicecharge.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldc4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldc4.BorderWidthBottom = 0;
                            celldc4.BorderWidthLeft = 0.5f;
                            celldc4.BorderWidthTop = 0;
                            celldc4.BorderWidthRight = 1.5f;
                            //celldc4.BorderColor = BaseColor.LIGHT_GRAY;
                            tempTable222.AddCell(celldc4);
                        }
                    }

                    #endregion

                    #region When Extra data is checked and STcheck is true
                    if (Extradatacheck == true)
                    {
                        //float machineryCostwithst = 0;
                        //if (lblMachinerywithst.Text.Length > 0)
                        //    machineryCostwithst = Convert.ToSingle(lblMachinerywithst.Text);

                        if (machinarycost > 0)
                        {
                            if (STMachinary == true)
                            {
                                if (SCMachinary == false)
                                {


                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;

                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }

                        //float materialcostwithst = 0;
                        //if (lblMaterialwithst.Text.Length > 0)
                        //    materialcostwithst = Convert.ToSingle(lblMaterialwithst.Text);
                        if (materialcost > 0)
                        {
                            if (STMaterial == true)
                            {
                                if (SCMaterial == false)
                                {



                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }


                        //float electricalcostwithst = 0;
                        //if (lblElectricalwithst.Text.Length > 0)
                        //    electricalcostwithst = Convert.ToSingle(lblElectricalwithst.Text);
                        if (maintenancecost > 0)
                        {
                            if (STMaintenance == true)
                            {
                                if (SCMaintenance == false)
                                {



                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }

                        //float extraamtwithst = 0;
                        //if (lblextraonewithst.Text.Length > 0)
                        //    extraamtwithst = Convert.ToSingle(lblextraonewithst.Text);
                        if (extraonecost > 0)
                        {
                            if (STExtraone == true)
                            {
                                if (SCExtraone == false)
                                {


                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }
                        //float Extraamtwithst1 = 0;
                        //if (lblextratwowithst.Text.Length > 0)
                        //    Extraamtwithst1 = Convert.ToSingle(lblextratwowithst.Text);
                        if (extratwocost > 0)
                        {
                            if (STExtratwo == true)
                            {
                                if (SCExtratwo == false)
                                {



                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = 2;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .5f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = .5f;
                                    //celldcst1.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = 1.5f;
                                    //celldcst2.BorderColor = BaseColor.LIGHT_GRAY;
                                    tempTable222.AddCell(celldcst2);
                                }
                            }
                        }


                    }

                    #endregion




                    if (!bIncludeST)
                    {

                        string scpercent = "";
                        if (bST75 == true)
                        {
                            scpercent = "3";
                        }
                        else
                        {
                            scpercent = SCPersent;
                        }

                        if (servicetax > 0)
                        {




                            PdfPCell celldd2 = new PdfPCell(new Phrase("Service Tax @ " + scpercent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd2.Colspan = 2;
                            celldd2.BorderWidthBottom = 0;
                            celldd2.BorderWidthLeft = .5f;
                            celldd2.BorderWidthTop = 0;
                            celldd2.BorderWidthRight = 0.5f;
                            //celldd2.PaddingBottom = 5;
                            //celldd2.PaddingTop = 5;
                            tempTable222.AddCell(celldd2);


                            PdfPCell celldd4 = new PdfPCell(new Phrase(servicetax.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd4.BorderWidthBottom = 0;
                            celldd4.BorderWidthLeft = 0.5f;
                            celldd4.BorderWidthTop = 0;
                            celldd4.BorderWidthRight = 1.5f;
                            //celldd4.PaddingBottom = 5;
                            //celldd4.PaddingTop = 5;
                            tempTable222.AddCell(celldd4);

                        }

                        if (sbcess > 0)
                        {



                            string SBCESSPresent = DtTaxes.Rows[0]["SBCess"].ToString();
                            PdfPCell celldd2 = new PdfPCell(new Phrase("Swachh Bharat Cess @ " + SBCESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd2.Colspan = 2;
                            celldd2.BorderWidthBottom = 0;
                            celldd2.BorderWidthLeft = .5f;
                            celldd2.BorderWidthTop = 0;
                            celldd2.BorderWidthRight = 0.5f;
                            // celldd2.PaddingBottom = 5;
                            // celldd2.PaddingTop = 5;
                            tempTable222.AddCell(celldd2);


                            PdfPCell celldd4 = new PdfPCell(new Phrase(sbcess.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd4.BorderWidthBottom = 0;
                            celldd4.BorderWidthLeft = 0.5f;
                            celldd4.BorderWidthTop = 0;
                            celldd4.BorderWidthRight = 1.5f;
                            //celldd4.PaddingBottom = 5;
                            //celldd4.PaddingTop = 5;
                            tempTable222.AddCell(celldd4);

                        }


                        if (kkcess > 0)
                        {



                            string KKCESSPresent = DtTaxes.Rows[0]["KKCess"].ToString();
                            PdfPCell Cellmtcesskk1 = new PdfPCell(new Phrase("Krishi Kalyan Cess @ " + KKCESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            Cellmtcesskk1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            Cellmtcesskk1.Colspan = 2;
                            Cellmtcesskk1.BorderWidthBottom = 0;
                            Cellmtcesskk1.BorderWidthLeft = .5f;
                            Cellmtcesskk1.BorderWidthTop = 0;
                            Cellmtcesskk1.BorderWidthRight = 0.5f;
                            // celldd2.PaddingBottom = 5;
                            // celldd2.PaddingTop = 5;
                            tempTable222.AddCell(Cellmtcesskk1);

                            PdfPCell Cellmtcesskk2 = new PdfPCell(new Phrase(kkcess.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            Cellmtcesskk2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            Cellmtcesskk2.BorderWidthBottom = 0;
                            Cellmtcesskk2.BorderWidthLeft = 0.5f;
                            Cellmtcesskk2.BorderWidthTop = 0;
                            Cellmtcesskk2.BorderWidthRight = 1.5f;
                            //celldd4.PaddingBottom = 5;
                            //celldd4.PaddingTop = 5;
                            tempTable222.AddCell(Cellmtcesskk2);

                        }

                        if (cess > 0)
                        {



                            string CESSPresent = DtTaxes.Rows[0]["Cess"].ToString();
                            PdfPCell celldd2 = new PdfPCell(new Phrase("CESS @ " + CESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd2.Colspan = 2;
                            celldd2.BorderWidthBottom = 0;
                            celldd2.BorderWidthLeft = .5f;
                            celldd2.BorderWidthTop = 0;
                            celldd2.BorderWidthRight = 0.5f;
                            //celldd2.PaddingBottom = 5;
                            //celldd2.PaddingTop = 5;
                            tempTable222.AddCell(celldd2);


                            PdfPCell celldd4 = new PdfPCell(new Phrase(cess.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldd4.BorderWidthBottom = 0;
                            celldd4.BorderWidthLeft = 0.5f;
                            celldd4.BorderWidthTop = 0;
                            celldd4.BorderWidthRight = .5f;
                            //celldd4.PaddingBottom = 5;
                            //celldd4.PaddingTop = 5;
                            tempTable222.AddCell(celldd4);

                        }

                        if (shecess > 0)
                        {




                            string SHECESSPresent = DtTaxes.Rows[0]["shecess"].ToString();
                            PdfPCell celldf2 = new PdfPCell(new Phrase("S&H Ed.CESS @ " + SHECESSPresent + "%", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldf2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldf2.Colspan = 2;
                            celldf2.BorderWidthBottom = 0;
                            celldf2.BorderWidthLeft = .5f;
                            celldf2.BorderWidthTop = 0;
                            celldf2.BorderWidthRight = 0.5f;
                            //celldf2.PaddingBottom = 5;
                            //celldf2.PaddingTop = 5;
                            tempTable222.AddCell(celldf2);


                            PdfPCell celldf4 = new PdfPCell(new Phrase(shecess.ToString("0.00"),
                                FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            celldf4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldf4.BorderWidthBottom = 0;
                            celldf4.BorderWidthLeft = .5f;
                            celldf4.BorderWidthTop = 0;
                            celldf4.BorderWidthRight = .5f;
                            ////celldf4.PaddingBottom = 5;
                            //celldf4.PaddingTop = 5;
                            //celldf4.BorderColor = BaseColor.LIGHT_GRAY;
                            tempTable222.AddCell(celldf4);
                        }
                    }







                    PdfPCell celldg6 = new PdfPCell(new Phrase("Grand Total(Rs.)", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    celldg6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    celldg6.BorderWidthBottom = 0;
                    celldg6.BorderWidthLeft = 0.5f;
                    celldg6.BorderWidthTop = 0;
                    celldg6.BorderWidthRight = 0.5f;
                    celldg6.Colspan = 2;
                    // celldg6.BorderColor = BaseColor.LIGHT_GRAY;
                    tempTable222.AddCell(celldg6);

                    PdfPCell celldg8 = new PdfPCell(new Phrase(Grandtotal.ToString("N2", CultureInfo.CreateSpecificCulture("hi-IN")), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    celldg8.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    celldg8.BorderWidthBottom = 0;
                    celldg8.BorderWidthLeft = 0.5f;
                    celldg8.BorderWidthTop = .5f;
                    celldg8.BorderWidthRight = 1.5f;
                    //celldg8.BorderColor = BaseColor.LIGHT_GRAY;
                    tempTable222.AddCell(celldg8);






                    PdfPCell childTable222 = new PdfPCell(tempTable222);
                    childTable222.Border = 0;
                    childTable222.Colspan = 3;
                    childTable222.HorizontalAlignment = 0;
                    tabled.AddCell(childTable222);

                    PdfPCell cellcamt = new PdfPCell(new Phrase(lblamtinwords.Text.Trim(), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cellcamt.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellcamt.Colspan = 5;
                    cellcamt.BorderWidthBottom = .5f;
                    cellcamt.BorderWidthLeft = 1.5f;
                    cellcamt.BorderWidthTop = .5f;
                    cellcamt.BorderWidthRight = 1.5f;
                    tabled.AddCell(cellcamt);


                    document.Add(tabled);





                    PdfPTable tablecon = new PdfPTable(5);
                    tablecon.TotalWidth = 560f;
                    tablecon.LockedWidth = true;
                    float[] widthcon = new float[] { 2f, 2f, 2f, 2f, 2f };
                    tablecon.SetWidths(widthcon);


                    PdfPTable tempTable11 = new PdfPTable(2);
                    tempTable11.TotalWidth = 224f;
                    tempTable11.LockedWidth = true;
                    float[] tempWidth11 = new float[] { 2f, 2f };
                    tempTable11.SetWidths(tempWidth11);


                    if (PANNO.Length > 0)
                    {
                        PdfPCell panno = new PdfPCell(new Phrase("  PAN NO : " + PANNO, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        panno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        panno.Colspan = 3;
                        panno.BorderWidthTop = 0;
                        panno.BorderWidthBottom = 0;
                        panno.BorderWidthLeft = 1.5f;
                        panno.BorderWidthRight = 0;
                        tempTable11.AddCell(panno);
                    }

                    if (Servicetax.Length > 0)
                    {
                        PdfPCell service = new PdfPCell(new Phrase("  SER. TAX REG.NO : " + Servicetax, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        service.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        service.Colspan = 3;
                        service.BorderWidthTop = 0;
                        service.BorderWidthBottom = 0;
                        service.BorderWidthLeft = 1.5f;
                        service.BorderWidthRight = 0;
                        tempTable11.AddCell(service);
                    }

                    if (Category.Length > 0)
                    {
                        PdfPCell CategoryP = new PdfPCell(new Phrase("  SC-CATEGORY : " + Category, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        CategoryP.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        CategoryP.Colspan = 3;
                        CategoryP.BorderWidthTop = 0;
                        CategoryP.BorderWidthBottom = 0;
                        CategoryP.BorderWidthLeft = 1.5f;
                        CategoryP.BorderWidthRight = 0;
                        tempTable11.AddCell(CategoryP);
                    }


                    if (PFNo.Length > 0)
                    {
                        PdfPCell pfcode = new PdfPCell(new Phrase("  PF CODE NO : " + PFNo, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        pfcode.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        pfcode.Colspan = 3;
                        pfcode.BorderWidthTop = 0;
                        pfcode.BorderWidthBottom = 0;
                        pfcode.BorderWidthLeft = 1.5f;
                        pfcode.BorderWidthRight = 0;
                        tempTable11.AddCell(pfcode);
                    }
                    if (Esino.Length > 0)
                    {
                        PdfPCell esicode = new PdfPCell(new Phrase("  ESI CODE NO : " + Esino, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                        esicode.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        esicode.Colspan = 3;
                        esicode.BorderWidthTop = 0;
                        esicode.BorderWidthBottom = 0;
                        esicode.BorderWidthLeft = 1.5f;
                        esicode.BorderWidthRight = 0;
                        tempTable11.AddCell(esicode);

                    }
                    PdfPCell esicodeaa = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    esicodeaa.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    esicodeaa.Colspan = 3;
                    esicodeaa.BorderWidthTop = 0;
                    esicodeaa.BorderWidthBottom = 0;
                    esicodeaa.BorderWidthLeft = 1.5f;
                    esicodeaa.BorderWidthRight = 0;
                    tempTable11.AddCell(esicodeaa);


                    PdfPCell childTable11 = new PdfPCell(tempTable11);
                    childTable11.Border = 0;
                    childTable11.Colspan = 2;
                    childTable11.HorizontalAlignment = 0;
                    tablecon.AddCell(childTable11);

                    PdfPTable tempTable22 = new PdfPTable(3);
                    tempTable22.TotalWidth = 336f;
                    tempTable22.LockedWidth = true;
                    float[] tempWidth22 = new float[] { 1f, 1f, 1f };
                    tempTable22.SetWidths(tempWidth22);

                    PdfPCell cellc41 = new PdfPCell(new Phrase("For " + companyName + "\n\n", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cellc41.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cellc41.Colspan = 3;
                    cellc41.BorderWidthTop = .5f;
                    cellc41.BorderWidthBottom = 0;
                    cellc41.BorderWidthLeft = 0;
                    cellc41.BorderWidthRight = 1.5f;
                    tempTable22.AddCell(cellc41);


                    PdfPCell clsign = new PdfPCell(new Paragraph("Authorised Signatory", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clsign.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    clsign.Colspan = 5;
                    clsign.BorderWidthTop = 0;
                    clsign.BorderWidthBottom = 0;
                    clsign.BorderWidthLeft = 0f;
                    clsign.BorderWidthRight = 1.5f;
                    clsign.PaddingTop = 30;
                    tempTable22.AddCell(clsign);




                    PdfPCell childTable22 = new PdfPCell(tempTable22);
                    childTable22.Border = 0;
                    childTable22.Colspan = 3;
                    childTable22.HorizontalAlignment = 0;
                    tablecon.AddCell(childTable22);





                    PdfPCell cheq = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cheq.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cheq.Colspan = 5;
                    cheq.BorderWidthTop = 0;
                    cheq.BorderWidthBottom = .5f;
                    cheq.BorderWidthLeft = 1.5f;
                    cheq.BorderWidthRight = 1.5f;
                    cheq.FixedHeight = 30;
                    tablecon.AddCell(cheq);

                    PdfPCell Prepared = new PdfPCell(new Phrase("\n     Prepared by :", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    Prepared.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    Prepared.Colspan = 5;
                    Prepared.BorderWidthTop = 0;
                    Prepared.BorderWidthBottom = .5f;
                    Prepared.BorderWidthLeft = 1.5f;
                    Prepared.BorderWidthRight = 1.5f;
                    Prepared.FixedHeight = 40;
                    // tablecon.AddCell(Prepared);


                    PdfPCell Checked = new PdfPCell(new Phrase("\n     Checked by :", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    Checked.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    Checked.Colspan = 5;
                    Checked.BorderWidthTop = 0;
                    Checked.BorderWidthBottom = 1.5f;
                    Checked.BorderWidthLeft = 1.5f;
                    Checked.BorderWidthRight = 1.5f;
                    Checked.FixedHeight = 40;
                    // tablecon.AddCell(Checked);

                    PdfPCell clsignc = new PdfPCell(new Paragraph("''This is computer generated Invoice, requires no signature''", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    clsignc.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    clsignc.Colspan = 5;
                    clsignc.BorderWidthTop = 0;
                    clsignc.BorderWidthBottom = 0;
                    clsignc.BorderWidthLeft = 0f;
                    clsignc.BorderWidthRight = 0;
                    // clsignc.PaddingTop = 20;
                    tablecon.AddCell(clsignc);

                    document.Add(tablecon);



                    #endregion

                    string filename = DisplayBillNo + "/" + ddlclientid.SelectedValue + "/" + GetMonthName().Substring(0, 3) + "/" + GetMonthOfYear() + "/Invoice.pdf";


                    // document.Add(tablecon);

                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();
                }

                catch (Exception ex)
                {
                    //LblResult.Text = ex.Message;
                }
            }
            else
            {
                // LblResult.Text = "There is no bill generated for selected client";
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There is no bill generated for selected client ');", true);

            }
        }

        protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            FillMonthDetails();
            LoadOldBillnos();
            DisplayDataInGrid();
        }



        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {
            AddNewRow();

        }

        protected void btnCalculateTotals_Click(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void CalculateTotals()
        {

            decimal totalamt = 0;

            for (int i = 0; i < gvClientBilling.Rows.Count; i++)
            {
                DropDownList ddldtype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;
                DropDownList ddlnod = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                TextBox txtDesg = gvClientBilling.Rows[i].FindControl("txtgvdesgn") as TextBox;
                TextBox txtpayrate = gvClientBilling.Rows[i].FindControl("txtPayRate") as TextBox;
                HiddenField hdNOD = gvClientBilling.Rows[i].FindControl("hdNOD") as HiddenField;
                TextBox txtnod = gvClientBilling.Rows[i].FindControl("txtNoOfDuties") as TextBox;
                TextBox txtdutyamt = gvClientBilling.Rows[i].FindControl("txtda") as TextBox;
                TextBox txtTotal = gvClientBilling.Rows[i].FindControl("txtAmount") as TextBox;

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

                            txtdutyamt.Text = txtTotal.Text = Math.Round(Convert.ToDecimal(txtpayrate.Text) * Convert.ToDecimal(txtnod.Text)).ToString("#.##");

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


                            txtdutyamt.Text = txtTotal.Text = Math.Round(Convert.ToDecimal(txtpayrate.Text) / Convert.ToDecimal(ddlnod.SelectedValue) * Convert.ToDecimal(txtnod.Text)).ToString("#.##");
                            break;
                        case 6:
                            if (txtpayrate.Text == "")
                            {
                                txtpayrate.Text = "0";
                            }

                            if (txtnod.Text == "")
                            {
                                txtnod.Text = "0";
                            }

                            double nods = 0;

                            nods = Convert.ToSingle(txtnod.Text) * 1.5;

                            txtdutyamt.Text = txtTotal.Text = Math.Round(Convert.ToDouble(txtpayrate.Text) / Convert.ToDouble(ddlnod.SelectedValue) * nods).ToString("#.##");
                            break;

                        default:
                            txtdutyamt.Text = txtTotal.Text = txtpayrate.Text;
                            break;
                    }
                    if (!string.IsNullOrEmpty(txtTotal.Text.Trim()))
                        totalamt += Math.Round(Convert.ToDecimal(txtTotal.Text.ToString()));

                }
            }
            var cid = ddlclientid.SelectedValue;
            var query = @"select * from Contracts where ClientID =  '" + cid + "'";
            var query1 = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess from TblOptions";

            var contractdetails = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            var optiondetails =config.ExecuteAdaptorAsyncWithQueryParams(query1).Result;

            decimal ServiceTaxSeparate = Convert.ToDecimal(optiondetails.Rows[0]["ServiceTaxSeparate"].ToString());
            decimal Cess = Convert.ToDecimal(optiondetails.Rows[0]["Cess"].ToString());
            decimal SHEcess = Convert.ToDecimal(optiondetails.Rows[0]["SHECess"].ToString());
            decimal SBcess = Convert.ToDecimal(optiondetails.Rows[0]["SBCess"].ToString());
            decimal KKcess = Convert.ToDecimal(optiondetails.Rows[0]["KKCess"].ToString());

            decimal servicetax = 0;
            decimal cesstax = 0;
            decimal sbcesstax = 0;
            decimal kkcesstax = 0;
            decimal educess = 0;
            decimal gtotal = 0;
            decimal servicecharge = 0;
            decimal subtotal = 0;
            decimal Servicechargeamt = 0;



            if (contractdetails.Rows.Count > 0)
            {

                //  Txtservicechrg.Text = contractdetails.Rows[0]["ServiceCharge"].ToString();
                servicecharge = Convert.ToDecimal(Txtservicechrg.Text);
                lblServiceCharges.Text = Math.Round(totalamt * (servicecharge / 100)).ToString();
                Servicechargeamt = Convert.ToDecimal(lblServiceCharges.Text);
                subtotal = totalamt + Servicechargeamt;
                lblSubTotal.Text = Math.Round(subtotal).ToString("#.##");

                if (contractdetails.Rows[0]["IncludeST"].ToString() == "True")
                {
                    servicetax = 0;
                    cesstax = 0;
                    educess = 0;
                    sbcesstax = 0;
                    kkcesstax = 0;
                }
                else if (contractdetails.Rows[0]["ServiceTax75"].ToString() == "True")
                {
                    servicetax = 3 * totalamt / 100;
                    sbcesstax = 3 * totalamt / 100;
                    kkcesstax = 3 * totalamt / 100;
                    cesstax = 2 * servicetax / 100;
                    educess = 1 * servicetax / 100;
                }
                else
                {
                    servicetax = Math.Round(ServiceTaxSeparate * (totalamt + Servicechargeamt) / 100);
                    sbcesstax = Math.Round(SBcess * (totalamt + Servicechargeamt) / 100);
                    kkcesstax = Math.Round(KKcess * (totalamt + Servicechargeamt) / 100);
                    cesstax = Math.Round(Cess * servicetax / 100);
                    educess = Math.Round(SHEcess * servicetax / 100);

                }



                gtotal = subtotal + servicetax + cesstax + educess + sbcesstax + kkcesstax;
            }


            if (totalamt > 0)
            {
                lblTotalResources.Text = totalamt.ToString("#.##");
                lblTotalResources.Visible = true;
            }


            if (servicetax > 0)
            {
                lblServiceTax.Text = servicetax.ToString("#.##");
                lblServiceTaxTitle.Visible = true;
                lblServiceTax.Visible = true;
            }
            if (sbcesstax > 0)
            {
                lblSBCESS.Text = sbcesstax.ToString("#.##");
                lblSBCESSTitle.Visible = true;
                lblSBCESS.Visible = true;
            }
            if (kkcesstax > 0)
            {
                lblKKCESS.Text = kkcesstax.ToString("#.##");
                lblKKCESSTitle.Visible = true;
                lblKKCESS.Visible = true;
            }

            if (cesstax > 0)
            {
                lblCESS.Text = cesstax.ToString("#.##");
                lblCESSTitle.Visible = true;
                lblCESS.Visible = true;
            }
            if (educess > 0)
            {
                lblSheCESS.Text = educess.ToString("#.##");
                lblSheCESSTitle.Visible = true;
                lblSheCESS.Visible = true;
            }
            if (gtotal > 0)
            {
                lblGrandTotal.Text = gtotal.ToString("#");
                lblGrandTotal.Visible = true;
            }
        }

        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";

            #region Validation

            if (txtmonth.Text.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return 0;
                }
            }
            #endregion

            #region  Month Get Based on the Control Selection
            int month = 0;
            if (Chk_Month.Checked == false)
            {
                month = Timings.Instance.GetIdForSelectedMonth(ddlmonth.SelectedIndex);
                //return month;
            }
            if (Chk_Month.Checked == true)
            {
                DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                month = Timings.Instance.GetIdForEnteredMOnth(date);
                //return month;
            }
            return month;




            #endregion

        }


        public void FillDefaultGird()
        {
            DataTable DefaultTable = new DataTable();
            DefaultTable.Columns.Add("Sid", typeof(int));
            DefaultTable.Columns.Add("Designation", typeof(string));
            // DefaultTable.Columns.Add("DutyHrs", typeof(string));
            DefaultTable.Columns.Add("NoofEmps", typeof(string));
            DefaultTable.Columns.Add("DutyHours", typeof(string));
            DefaultTable.Columns.Add("payrate", typeof(string));
            DefaultTable.Columns.Add("paytype", typeof(string));
            DefaultTable.Columns.Add("BasicDa", typeof(string));
            // DefaultTable.Columns.Add("OTAmount", typeof(string));
            DefaultTable.Columns.Add("NoOfDays", typeof(string));
            DefaultTable.Columns.Add("Totalamount", typeof(string));

            var cid = ddlclientid.SelectedValue;



            if (!cid.Equals("--Select--"))
            {
                #region Month Selection
                int month = 0;
                month = GetMonthBasedOnSelectionDateorMonth();




                //if (ddlmonth.SelectedIndex == 1)
                //{
                //    month = GlobalData.Instance.GetIDForNextMonth();
                //}
                //else if (ddlmonth.SelectedIndex == 2)
                //{
                //    month = GlobalData.Instance.GetIDForThisMonth();
                //}
                //else
                //{
                //    month = GlobalData.Instance.GetIDForPrviousMonth();
                //}
                #endregion


                #region  Begin Get Contract Id Based on The Last Day

                DateTime DtLastDay = DateTime.Now;
                if (Chk_Month.Checked == false)
                {
                    DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
                }
                if (Chk_Month.Checked == true)
                {
                    DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                }
                var ContractID = "";
                Hashtable HtGetContractID = new Hashtable();
                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
                HtGetContractID.Add("@LastDay", DtLastDay);
                DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                if (DTContractID.Rows.Count > 0)
                {
                    ContractID = DTContractID.Rows[0]["contractid"].ToString();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                    return;
                }

                #endregion  End Get Contract Id Based on The Last Day

                int prevmonth = 0;

                if (ddlmonth.SelectedIndex == 1)
                {
                    prevmonth = Timings.Instance.GetIdForPreviousMonth();
                }

                if (ddlmonth.SelectedIndex == 2)
                {
                    prevmonth = Timings.Instance.GetIdForPreviousOneMonth();
                }

                if (ddlmonth.SelectedIndex == 3)
                {
                    prevmonth = Timings.Instance.GetIdForPreviousTwoMonth();
                }




                string sqlqry = "select max(isnull(munitidbillno,'')) as billno from MUnitBillBreakup where  UnitId='" + ddlclientid.SelectedValue + "' and month='" + prevmonth + "'";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
                string MaxBillno = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    MaxBillno = dt.Rows[0]["billno"].ToString();

                    if (MaxBillno != "")
                    {
                        var query = @"select Designation,NoofEmps,DutyHours,PayRate,PayRateType as paytype,monthlydays as NoOfDays,BasicDa,Totalamount from MUnitBillBreakup  where UnitId='" + ddlclientid.SelectedValue + "' and month='" + prevmonth + "' and munitidbillno='" + MaxBillno + "' order by sino ";
                        var griddata = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                        DefaultTable = griddata;
                    }
                    else
                    {
                        var query = @"select d.Design as Designation,
	                               ISNULL(cd.Quantity,0) as NoofEmps,
                                   ISNULL(cd.DutyHrs,0) as DutyHrs,
	                               ISNULL(cad.Duties,0) as DutyHours,
	                               ISNULL(Amount,0) as payrate,
	                               cd.PayType as paytype,0 as BasicDa,0 as OTAmount,cd.NoOfDays,0 as Totalamount
	                        from Designations d 
                            inner join ContractDetails cd on d.DesignId = cd.Designations
                            left outer join ClientAttenDance cad on cd.ClientID = cad.ClientId and cd.Designations = cad.Desingnation 
                            and cad.[month]= " + month.ToString() + " where cd.ClientID = '" + cid + "' and cd.contractid='" + ContractID + "'  ";

                        var griddata = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                        DefaultTable = griddata;
                    }
                }
                else
                {
                    var query = @"select d.Design as Designation,
	                               ISNULL(cd.Quantity,0) as NoofEmps,
                                   ISNULL(cd.DutyHrs,0) as DutyHrs,
	                               ISNULL(cad.Duties,0) as DutyHours,
	                               ISNULL(Amount,0) as payrate,
	                               cd.PayType as paytype,0 as BasicDa,0 as OTAmount,cd.NoOfDays,0 as Totalamount
	                        from Designations d 
                            inner join ContractDetails cd on d.DesignId = cd.Designations
                            left outer join ClientAttenDance cad on cd.ClientID = cad.ClientId and cd.Designations = cad.Desingnation 
                            and cad.[month]= " + month.ToString() + " where cd.ClientID = '" + cid + "' and cd.contractid='" + ContractID + "'  ";

                    var griddata = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                    DefaultTable = griddata;
                }
            }

            var sl = DefaultTable.Rows.Count;
            var count = DefaultTable.Rows.Count > 0 ? DefaultTable.Rows.Count + 5 : 1;

            for (int i = sl + 1; i < count; i++)
            {
                DataRow dr = DefaultTable.NewRow();
                //dr["Sid"] = i;
                dr["Designation"] = "";
                dr["NoofEmps"] = 0;
                //dr["DutyHrs"] = 0;
                dr["DutyHours"] = 0;
                dr["payrate"] = 0;
                dr["paytype"] = 0;
                dr["BasicDa"] = 0;
                // dr["OTAmount"] = 0;
                dr["NoOfDays"] = 1;
                dr["Totalamount"] = 0;
                DefaultTable.Rows.Add(dr);
            }

            ViewState["DTDefaultManual"] = DefaultTable;
            gvClientBilling.DataSource = DefaultTable;
            gvClientBilling.DataBind();

            for (int i = 0; i < DefaultTable.Rows.Count; i++)
            {
                DropDownList CDutytype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;

                if (String.IsNullOrEmpty(DefaultTable.Rows[i]["paytype"].ToString()) != false)
                {
                    CDutytype.SelectedIndex = 0;
                }
                else
                {
                    if (DefaultTable.Rows[i]["paytype"].ToString().Trim().Length > 0)
                    {
                        CDutytype.SelectedIndex = Convert.ToInt32(DefaultTable.Rows[i]["paytype"].ToString().Trim());
                    }
                }
            }

            btnAddNewRow.Visible = (gvClientBilling.Rows.Count > 0);
            btnCalculateTotals.Visible = (gvClientBilling.Rows.Count > 0);
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {

            lblResult.Text = "";
            lbltotalamount.Text = "";
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            txtfromdate.Text = "";
            txttodate.Text = "";
            ddlmonth.SelectedIndex = 0;
            txtmonth.Text = string.Empty;
            rdbcreatebill.Checked = true;
            rdbmodifybill.Checked = false;
            ddlMBBillnos.SelectedIndex = 0;
            lblbillnolatest.Text = "";

            ClearExtraDataForBilling();
            if (ddlclientid.SelectedIndex > 0)
            {
                string SqlQryGetCname = "select clientid from clients where clientid='" + ddlclientid.SelectedValue + "'  and clientid like '%" + CmpIDPrefix + "%'";
                DataTable dt;
                dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryGetCname).Result;
                ddlCname.SelectedValue = dt.Rows[0]["clientid"].ToString();
                ddlmonth.SelectedIndex = 0;
                if (ddlmonth.SelectedIndex > 0)
                {
                    FillMonthDetails();
                }
            }
            else
            {
                ddlCname.SelectedIndex = 0;
            }
        }

        protected void ddlCname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblResult.Text = "";
            ddlmonth.SelectedIndex = 0;
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            txtfromdate.Text = "";
            txttodate.Text = "";
            txtmonth.Text = string.Empty;
            rdbcreatebill.Checked = true;
            rdbmodifybill.Checked = false;
            ddlMBBillnos.SelectedIndex = 0;
            lblbillnolatest.Text = "";


            ClearExtraDataForBilling();
            if (ddlCname.SelectedIndex > 0)
            {
                string SqlQryGetCname = "select clientid from clients where clientid='" + ddlCname.SelectedValue + "' and clientid like '%" + CmpIDPrefix + "%' ";
                DataTable dt;
                dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryGetCname).Result;
                if (dt.Rows.Count > 0)
                {
                    ddlclientid.SelectedValue = dt.Rows[0]["clientid"].ToString();
                }

                if (ddlmonth.SelectedIndex > 0)
                {
                    FillMonthDetails();
                }
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ClearExtraDataForBilling()
        {
            lblResult.Text = "";

            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            lblTotalResources.Text = "";
            lblServiceTax.Text = "";
            lblCESS.Text = "";
            lblSBCESS.Text = "";
            lblKKCESS.Text = "";
            lblSheCESS.Text = "";
            lblGrandTotal.Text = "";
            lblSubTotal.Text = "";
            lblServiceCharges.Text = "";
            Txtservicechrg.Text = "";
        }

        protected int GetMonthBasedonSelection()
        {

            int Month = 0;
            if (ddlmonth.SelectedIndex == 1)
            {
                Month = GlobalData.Instance.GetIDForNextMonth();
            }
            else if (ddlmonth.SelectedIndex == 2)
            {
                Month = GlobalData.Instance.GetIDForThisMonth();
            }
            else if (ddlmonth.SelectedIndex == 3)
            {
                Month = GlobalData.Instance.GetIDForPrviousMonth();
            }
            else if (ddlmonth.SelectedIndex == 4)
            {
                Month = GlobalData.Instance.GetIDForPrviousoneMonth();
            }

            return Month;
        }

        protected void ddlMBBillnos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbmodifybill.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Bill Type Modify');", true);
                ddlMBBillnos.SelectedIndex = 0;
            }
            else
            {
                DisplayDataInGrid();
            }
        }

        protected void btngenratepayment_Click(object sender, EventArgs e)
        {
            btninvoice.Visible = true;
            lbltotalamount.Visible = true;

            #region Validations

            if (ddlclientid.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Select Client Id ');", true);

                return;
            }
            #region  Begin New code As on [10-03-2014]

            if (Chk_Month.Checked == true)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Enter Month for Bill ');", true);
                    return;
                }
                if (Timings.Instance.CheckEnteredDate(txtmonth.Text) == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid TO DATE .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }
            }
            else
            {
                if (ddlmonth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Select Month for Bill ');", true);

                    return;
                }
            }
            #endregion  End Old Code As on [14-02-2014]
            if (txtbilldate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' Please Fill The Billdate  ');", true);
                return;
            }

            if (ddlMBBillnos.SelectedIndex > 0)
            {
                if (rdbmodifybill.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select The Bill No.');", true);
                    return;
                }
            }
            #endregion
            int month = 0;
            string SelectedClient = ddlclientid.SelectedValue;
            #region Month Selection

            month = GetMonthBasedOnSelectionDateorMonth();

            //if (ddlmonth.SelectedIndex == 1)
            //{
            //    month = GlobalData.Instance.GetIDForNextMonth();
            //}
            //else if (ddlmonth.SelectedIndex == 2)
            //{
            //    month = GlobalData.Instance.GetIDForThisMonth();
            //}
            //else
            //{
            //    month = GlobalData.Instance.GetIDForPrviousMonth();
            //}
            #endregion

            #region  Query For Delete Unitbill Break Up Data

            /** Delete previously generated UnitBillBreakup data */

            if (rdbmodifybill.Checked)
            {
                string DeleteQueryForSelectedMonth = "Delete from Munitbillbreakup where unitid ='" + SelectedClient + "' and month =" +
                                                                     month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue + "'";
               int deld=config.ExecuteNonQueryWithQueryAsync(DeleteQueryForSelectedMonth).Result;
            }
            //Unitbill details are not deleted now due to Billno(avoid regeneration)
            /** Delete **/

            #endregion

            #region   Query for  Get  Contracts  Details

            string sqlQry = "Select ContractId,ContractStartDate,ContractEndDate,PaymentType,MaterialCostPerMonth, " +
                " MachinaryCostPerMonth,NoOfDays,servicecharge,OTPersent,PayLumpsum,ServiceChargeType,ServiceTax75,IncludeST, " +
                "  ServiceTaxType,BillDates from Contracts where ClientId='" + ddlclientid.SelectedValue + "'";
            DataTable dtContracts = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;

            if (dtContracts.Rows.Count > 0)
            {
                //CalculateTotals();

                string strSTType = dtContracts.Rows[0]["ServiceTaxType"].ToString();
                string NoOfDaysFromContract = dtContracts.Rows[0]["NoOfDays"].ToString();
                string strServiceChargetType = dtContracts.Rows[0]["ServiceChargeType"].ToString();
                string ServiceCharge = dtContracts.Rows[0]["ServiceCharge"].ToString();

                bool bSTType = (strSTType == "True");
                string billno = (rdbmodifybill.Checked)
                                ? ddlMBBillnos.SelectedValue
                                : BillnoAutoGenrate(bSTType, ddlclientid.SelectedValue, month);



                #region   Get Data From GridView and Saving In the Munitbillbreakup Table

                if (gvClientBilling.Rows.Count > 0)
                {
                    string invoicedesc = txtdescription.Text;
                    string remarks = txtremarks.Text;
                    string Unitid = ddlclientid.SelectedValue;
                    int totalstatus = 0;
                    int i = 0;

                    foreach (GridViewRow GvRow in gvClientBilling.Rows)
                    {
                        string sno = ((Label)GvRow.FindControl("lblgvSlno")).Text;
                        string Desgn = ((TextBox)GvRow.FindControl("txtgvdesgn")).Text;
                        string NoOfEmps = ((TextBox)GvRow.FindControl("txtnoofemployees")).Text;
                        string NoOfDuties = ((TextBox)GvRow.FindControl("txtNoOfDuties")).Text;
                        string Payrate = ((TextBox)GvRow.FindControl("txtPayRate")).Text; //lblda
                        // string Payratetype = ((TextBox)GvRow.FindControl("txtPayRatetype")).Text;
                        string DutiesAmount = ((TextBox)GvRow.FindControl("txtda")).Text;
                        //string OtAmount = ((TextBox)GvRow.FindControl("lblOtAmount")).Text;
                        string Total = ((TextBox)GvRow.FindControl("txtAmount")).Text;
                        float ToatlAmount = 0;
                        float basicda = 0;
                        ToatlAmount = (Total.Trim().Length != 0) ? float.Parse(Total) : 0;
                        basicda = (DutiesAmount.Trim().Length != 0) ? float.Parse(DutiesAmount) : 0;
                        DropDownList ddlnodays = gvClientBilling.Rows[i].FindControl("ddlnod") as DropDownList;
                        int ddlnod = int.Parse(ddlnodays.SelectedItem.Text);
                        DropDownList ddldttype = gvClientBilling.Rows[i].FindControl("ddldutytype") as DropDownList;
                        int ddldutytype = int.Parse(ddldttype.SelectedValue);
                        i = i + 1;

                        if (Desgn != "")
                        {
                            string Sqlqry = string.Format("insert into Munitbillbreakup(unitid,designation,DutyHours,NoofEmps,BasicDa, " +
                                "PayRate,PayRateType,Month,OTamount,Totalamount,MunitidBillno,monthlydays,Description,Remarks,sino) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
                                Unitid, Desgn, NoOfDuties, NoOfEmps, DutiesAmount, Payrate, ddldutytype, month, 0, Total, billno, ddlnod, invoicedesc, remarks, sno);
                            int Status =config.ExecuteNonQueryWithQueryAsync(Sqlqry).Result;
                            if (Status != 0)
                            {
                                totalstatus++;
                                if (totalstatus == 1)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Manual Billing Details Added Sucessfully');", true);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region   Storing Details about the  Unit Bill Table


                bool bServiceChargeType = false;
                if (strServiceChargetType == "True")
                {
                    bServiceChargeType = true;
                }

                //float serviceCharge = 0;
                //if (ServiceCharge.Length > 0)
                //{
                //    serviceCharge = Convert.ToSingle(ServiceCharge);
                //}

                if (lblTotalResources.Text.Trim().Length == 0)
                {
                    lblTotalResources.Text = "0";
                }

                double totalCharges = double.Parse(lblTotalResources.Text);

                string grandtotal = lblGrandTotal.Text;
                if (grandtotal.Trim().Length == 0)
                {
                    grandtotal = "0";
                }

                string ServiceTax = lblServiceTax.Text;

                if (ServiceTax.Trim().Length == 0)
                {
                    ServiceTax = "0";
                }

                string sbcesstax = lblSBCESS.Text;

                if (sbcesstax.Trim().Length == 0)
                {
                    sbcesstax = "0";
                }

                string kkcesstax = lblKKCESS.Text;


                if (kkcesstax.Trim().Length == 0)
                {
                    kkcesstax = "0";
                }

                string cesstax = lblCESS.Text;

                if (cesstax.Trim().Length == 0)
                {
                    cesstax = "0";
                }


                string Shesstax = lblSheCESS.Text;

                if (Shesstax.Trim().Length == 0)
                {
                    Shesstax = "0";
                }

                string ServiceChargePer = Txtservicechrg.Text;
                if (ServiceChargePer.Trim().Length == 0)
                {
                    ServiceChargePer = "0";
                }

                string SubTotal = lblSubTotal.Text;
                if (SubTotal.Trim().Length == 0)
                {
                    SubTotal = "0";
                }
                string Servicechrg = lblServiceCharges.Text;
                if (Servicechrg.Trim().Length == 0)
                {
                    Servicechrg = "0";
                }

                System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
                DateTime dtb = Convert.ToDateTime(txtbilldate.Text, enGB);
                string billdate = dtb.ToString("yyyy-MM-dd hh:mm:ss");
                DateTime dtf = Convert.ToDateTime(txtfromdate.Text, enGB);
                string tfrom = dtf.ToString("yyyy-MM-dd hh:mm:ss");
                DateTime dtt = Convert.ToDateTime(txttodate.Text, enGB);
                string tto = dtt.ToString("yyyy-MM-dd hh:mm:ss");


                if (rdbcreatebill.Checked)
                {
                    string InsertQueryForUnitBill = "insert into Munitbill(billno,billdt,unitid,fromdt,todt,TotalChrg," +
                                                    " monthlydays,grandtotal,ServiceChrg,servicetax,cess,shecess,ServiceTax75percentage,ServiceChrgPer,Subtotal," +
                                                    " month,MachinaryCost,MaterialCost,Discount,ElectricalChrg,Remarks,SBCessAmt,kkcessamt) values('"
                                                    + billno + "','"
                                                    + billdate + "','"
                                                    + ddlclientid.SelectedValue + "','"
                                                    + tfrom + "','"
                                                    + tto + "','"
                                                    + totalCharges + "','0','"
                                                    + grandtotal + "',"
                                                    + lblServiceCharges.Text + ","
                                                    + ServiceTax + ","
                                                    + cesstax + ","
                                                    + Shesstax + ","
                                                    + "null,"
                                                    + Txtservicechrg.Text + ","
                                                    + lblSubTotal.Text + ","
                                                    + month + ","
                                                    + " null,null,null,null,null,'" + lblSBCESS.Text + "','" + lblKKCESS.Text + "' )";
                    int insrt=config.ExecuteNonQueryWithQueryAsync(InsertQueryForUnitBill).Result;
                }

                if (rdbmodifybill.Checked)
                {
                    string ServiceCharge1 = lblServiceCharges.Text;
                    string ServiceChargePer1 = Txtservicechrg.Text;
                    string totalCharges1 = lblTotalResources.Text;
                    string ServiceTax1 = lblServiceTax.Text;
                    string cesstax1 = lblCESS.Text;
                    string sbcesstax1 = lblSBCESS.Text;
                    string kkcesstax1 = lblKKCESS.Text;
                    string Shesstax1 = lblSheCESS.Text;
                    string SubTotal1 = lblSubTotal.Text;
                    string grandtotal1 = lblGrandTotal.Text;
                    string desc = txtdescription.Text;
                    string remark = txtremarks.Text;



                    string SqlQryForUnitBill = "Select * from MUnitbill  where unitid ='" + SelectedClient + "' and month=" + month + "   and  billno='" + ddlMBBillnos.SelectedValue + "'";
                    DataTable dtUnitBill = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForUnitBill).Result;

                    string SqlQryForudateUnitBillbreakup = "Select Description,Remarks from MUnitbillBreakUp  where unitid ='" + SelectedClient + "' and month=" + month + " and MunitidBillno='" + ddlMBBillnos.SelectedValue + "'  ";
                    DataTable dtMUnitBill = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForudateUnitBillbreakup).Result;


                    // txtremarks.Text = dtUnitBill.Rows[0]["Remarks"].ToString();
                    //&& txtremarks.Text != "" && txtdescription.Text!=""

                    if (dtUnitBill.Rows.Count > 0)
                    {
                        string InsertQueryForUnitBill = string.Format("update Munitbill set billdt='{1}',unitid='{2}',fromdt='{3}',todt='{4}', " +
                           " monthlydays='{5}',grandtotal='{6}',servicetax='{7}',cess='{8}',shecess='{9}',month='{10}',TotalChrg='{11}',ServiceChrg='{12}',ServiceChrgPer='{13}',Subtotal='{14}',sbcessamt='{15}',kkcessamt='{16}'" +
                           " where  billno='{0}'  ",
                           billno, billdate, ddlclientid.SelectedValue, tfrom, tto,
                           0, grandtotal1, ServiceTax1, cesstax1, Shesstax1, month, totalCharges1, ServiceCharge1, ServiceChargePer1, SubTotal1, sbcesstax1, kkcesstax1);

                       int ins= config.ExecuteNonQueryWithQueryAsync(InsertQueryForUnitBill).Result;
                        string InsertQueryForMUnitBill = string.Format("update MunitbillBreakUp set Description='" + desc + "',Remarks='" + remark + "' where unitid='" + ddlclientid.SelectedValue + "' and month='" + month + "' and MunitidBillno='" + billno + "'");
                        int res = config.ExecuteNonQueryWithQueryAsync(InsertQueryForMUnitBill).Result;




                    }


                }
                #endregion
                LoadDefaultData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('ContractId not available for this client.');", true);
            }
            #endregion

        }

        protected void LoadDefaultData()
        {
            //ddlclientid.SelectedIndex = 0;
            //ddlCname.SelectedIndex = 0;
            txtfromdate.Text = "";
            txttodate.Text = "";
            ddlmonth.SelectedIndex = 0;
            txtbilldate.Text = "";
            ddlMBBillnos.SelectedIndex = 0;
            FillDefaultGird();
            lblTotalResources.Text = "0";
            lblServiceTax.Text = "0";
            lblGrandTotal.Text = "0";
            lblCESS.Text = "0";
            lblSBCESS.Text = "0";
            lblKKCESS.Text = "0";
            lblSheCESS.Text = "0";
            lblServiceCharges.Text = "0";
            lblSubTotal.Text = "0";
            Txtservicechrg.Text = "";
            txtremarks.Text = "";
            txtmonth.Text = "";
            lblbillnolatest.Text = "";
        }







        private string BillnoAutoGenrate(bool StType, string unitId, int month)
        {
            string billno = "00001";
            //string strQry = "Select BillNo from UnitBill where UnitId='" + unitId + "' And Month=" + month;
            //DataTable noTable = SqlHelper.Instance.GetTableByQuery(strQry);
            //if (noTable.Rows.Count > 0)
            //{
            //    if (noTable.Rows[0]["billno"].ToString().Length > 0)
            //    {
            //        billno = noTable.Rows[0]["billno"].ToString();
            //    }
            //}
            //else
            {

                string selectqueryclientid = "";
                string selectqueryclientidMB = "";


                if (StType)
                {
                    string Type = GlobalData.Instance.GetType(ddlclientid.SelectedValue);
                    //string billPrefix = GlobalData.Instance.GetBillPrefix(false);
                    string billPrefix = GlobalData.Instance.GetBillPrefix(false, BranchID);
                    string billStartNo = GlobalData.Instance.GetBillStartingNo(false);
                    string billSeq = GlobalData.Instance.GetBillSequence();
                    billno = billPrefix + "/" + billSeq + "/" + billStartNo;

                    int startingNumberPart = billno.Length - 5 + 1;


                    selectqueryclientid = "select MAX( RIGHT(billno,5)) as billno from unitbill where billno like '"
                    + billPrefix + "/" + billSeq + "%'";

                    DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;
                    int BILLNO = 0;
                    int BILLNOMB = 0;
                    if (dt.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[0]["billno"].ToString()) == false)
                            BILLNO = int.Parse(dt.Rows[0]["billno"].ToString());
                    }


                    selectqueryclientidMB = "select MAX( RIGHT(billno,5)) as billno from Munitbill where billno like '"
                     + billPrefix + "/" + billSeq + "%'";


                    DataTable dtMB = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientidMB).Result;

                    if (dtMB.Rows.Count > 0)
                    {

                        if (String.IsNullOrEmpty(dtMB.Rows[0]["billno"].ToString()) == false)
                            BILLNOMB = int.Parse(dtMB.Rows[0]["billno"].ToString());
                    }

                    if (BILLNO > BILLNOMB)
                    {
                        billno = billPrefix + "/" + billSeq + "/" + (Convert.ToInt32(BILLNO) + 1).ToString("00000");
                    }
                    else
                    {
                        billno = billPrefix + "/" + billSeq + "/" + (Convert.ToInt32(BILLNOMB) + 1).ToString("00000");
                    }

                }
                else
                {
                    string Type = GlobalData.Instance.GetType(ddlclientid.SelectedValue);
                    string billPrefix = GlobalData.Instance.GetBillPrefix(true, BranchID);
                    string billStartNo = GlobalData.Instance.GetBillStartingNo(true);
                    string billSeq = GlobalData.Instance.GetBillSequence();
                    billno = billSeq + "/" + billStartNo;
                    int startingNumberPart = billno.Length - 5 + 1;

                    selectqueryclientid = "select MAX( RIGHT(billno,5)) as billno from unitbill where billno like '"
                     + billPrefix + "/" + billSeq + "%'";


                    DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;
                    int BILLNO = 0;
                    int BILLNOMB = 0;
                    if (dt.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[0]["billno"].ToString()) == false)
                            BILLNO = int.Parse(dt.Rows[0]["billno"].ToString());
                    }

                    selectqueryclientidMB = "select MAX( RIGHT(billno,5)) as billno from Munitbill where billno like '"
                      + billPrefix + "/" + billSeq + "%'";


                    DataTable dtMB = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientidMB).Result;

                    if (dtMB.Rows.Count > 0)
                    {

                        if (String.IsNullOrEmpty(dtMB.Rows[0]["billno"].ToString()) == false)
                            BILLNOMB = int.Parse(dtMB.Rows[0]["billno"].ToString());
                    }

                    if (BILLNO > BILLNOMB)
                    {
                        billno = billPrefix + "/" + billSeq + "/" + (Convert.ToInt32(BILLNO) + 1).ToString("00000");
                    }
                    else
                    {
                        billno = billPrefix + "/" + billSeq + "/" + (Convert.ToInt32(BILLNOMB) + 1).ToString("00000");
                    }

                }
            }
            return billno;
        }

        protected void FillMonthDetails()
        {

            if (Chk_Month.Checked == true)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    return;
                }
                if (Timings.Instance.CheckEnteredDate(txtmonth.Text) == 1)
                {
                    return;
                }
            }
            else
            {
                if (ddlmonth.SelectedIndex == 0)
                {
                    return;
                }
            }


            DateTime DtLastDay = DateTime.Now;
            if (Chk_Month.Checked == false)
            {
                DtLastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlmonth.SelectedIndex);
            }
            if (Chk_Month.Checked == true)
            {
                DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            }




            var ContractID = "";
            var bBillDates = 0;
            var ServiceTaxType = false;
            #region  Begin Get Contract Id Based on The Last Day


            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlclientid.SelectedValue);
            HtGetContractID.Add("@LastDay", DtLastDay);
            DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
                bBillDates = int.Parse(DTContractID.Rows[0]["BillDates"].ToString());
                ServiceTaxType = bool.Parse(DTContractID.Rows[0]["ServiceTaxType"].ToString());

                string ContractStartDate = DTContractID.Rows[0]["ContractStartDate"].ToString();
                string ContractEndDate = DTContractID.Rows[0]["ContractEndDate"].ToString();
                string strBillDates = DTContractID.Rows[0]["BillDates"].ToString();

                DateTime CSdate = DateTime.Parse(ContractStartDate);
                DateTime CurrentDate = DateTime.Now.Date;
                DateTime lastDay = DateTime.Now.Date;
                int monval = GetMonthBasedOnSelectionDateorMonth();
                string mntchk = "0";
                if (monval.ToString().Length == 3)
                {
                    mntchk = monval.ToString().Substring(0, 1);

                }
                else if (monval.ToString().Length == 4)
                {
                    mntchk = monval.ToString().Substring(0, 2);

                }

                if (Chk_Month.Checked == false)
                {
                    if (ddlmonth.SelectedIndex == 1)
                    {
                        CurrentDate = CurrentDate.AddMonths(0);
                        lastDay = Timings.Instance.GetLastDayOfThisMonth();
                        txtyear.Text = GetMonthOfYear();
                    }
                    else if (ddlmonth.SelectedIndex == 2)
                    {
                        txtyear.Text = GetMonthOfYear();

                        if (CurrentDate.Month == 1)
                        {
                            CurrentDate = CurrentDate.AddMonths(11);
                            CurrentDate = CurrentDate.AddYears(-1);

                        }
                        else
                        {
                            CurrentDate = CurrentDate.AddMonths(-1);
                        }

                        lastDay = Timings.Instance.GetLastDayOfPreviousMonth();
                    }
                    else if (ddlmonth.SelectedIndex == 3)
                    {
                        txtyear.Text = GetMonthOfYear();
                        if (CurrentDate.Month == 2)
                        {
                            CurrentDate = CurrentDate.AddMonths(10);
                            CurrentDate = CurrentDate.AddYears(-1);
                        }
                        else
                        {
                            CurrentDate = CurrentDate.AddMonths(-2);
                        }

                        lastDay = Timings.Instance.GetLastDayOfPreviousOneMonth();
                    }
                    else if (ddlmonth.SelectedIndex == 4)
                    {
                        txtyear.Text = GetMonthOfYear();
                        if (CurrentDate.Month == 3)
                        {
                            CurrentDate = CurrentDate.AddMonths(9);
                            CurrentDate = CurrentDate.AddYears(-1);
                        }
                        else
                        {
                            CurrentDate = CurrentDate.AddMonths(-3);
                        }
                        lastDay = Timings.Instance.GetLastDayOfPreviousTwoMonth();
                    }
                }

                #region  Begin Old Code As on [02-03-2014]


                if (Chk_Month.Checked == true)
                {
                    DateTime sdate = DateTime.Now.Date;
                    int month = 0;
                    int year = 0;


                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        sdate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb"));
                    }

                    month = sdate.Month;
                    year = sdate.Year;
                    DateTime finalday = new DateTime(sdate.Year, sdate.Month, DateTime.DaysInMonth(sdate.Year, sdate.Month));

                    CurrentDate = sdate;
                    lastDay = finalday;
                    txtyear.Text = year.ToString();
                }



                DateTime CEdate = DateTime.Parse(ContractEndDate);
                if (CSdate <= lastDay)
                {
                    if (bBillDates == 1)
                    {

                        if (CurrentDate.Month == 2 && (CSdate.Day > 28 || CSdate.Day > 29))
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month, 28).ToString("dd/MM/yyyy"));
                        }
                        if (CurrentDate.Month == 1)
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year, 12, CSdate.Day).ToString("dd/MM/yyyy"));
                        }
                        else
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month - 1, CSdate.Day).ToString("dd/MM/yyyy"));

                        }
                        DateTime tempDate = CurrentDate.AddMonths(1);

                        if (CSdate.Day == 1)
                        {
                            txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month, CSdate.Day).ToString("dd/MM/yyyy"));
                        }
                        else
                            if (CurrentDate.Month == 1)
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, 1, CSdate.Day - 1).ToString("dd/MM/yyyy"));
                            }
                            else
                                if (tempDate.Month == 1)
                                {
                                    txttodate.Text = (new DateTime(tempDate.Year, 12, CSdate.Day - 1).ToString("dd/MM/yyyy"));
                                }
                                else
                                {
                                    txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month - 1, CSdate.Day - 1).ToString("dd/MM/yyyy"));
                                }

                    }
                    if (bBillDates == 0)
                    {
                        txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, 1).ToString("dd/MM/yyyy"));

                        if (CSdate.Day == 1)
                        {
                            txttodate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month)).ToString("dd/MM/yyyy"));
                        }
                        else
                        {
                            txttodate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month)).ToString("dd/MM/yyyy"));

                        }
                    }


                    if (bBillDates == 2)
                    {
                        if (CurrentDate.Month == 1)
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year - 1, 12, 26).ToString("dd/MM/yyyy"));
                        }
                        else

                            txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month - 1, 26).ToString("dd/MM/yyyy"));

                        DateTime tempDate = CurrentDate.AddMonths(1);

                        if (mntchk == "12")
                        {
                            tempDate = CurrentDate;
                            txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month, 25).ToString("dd/MM/yyyy"));

                        }
                        else
                        {
                            if (tempDate.Month == 1)
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, 12, 25).ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month - 1, 25).ToString("dd/MM/yyyy"));
                            }
                        }

                    }


                    if (bBillDates == 3)
                    {
                        if (CurrentDate.Month == 1)
                        {
                            txtfromdate.Text = (new DateTime(CurrentDate.Year - 1, 12, 21).ToString("dd/MM/yyyy"));
                        }
                        else

                            txtfromdate.Text = (new DateTime(CurrentDate.Year, CurrentDate.Month - 1, 21).ToString("dd/MM/yyyy"));
                        DateTime tempDate = CurrentDate.AddMonths(1);

                        if (mntchk == "12")
                        {
                            tempDate = CurrentDate;
                            txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month, 20).ToString("dd/MM/yyyy"));

                        }
                        else
                        {
                            if (tempDate.Month == 1)
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, 12, 20).ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                txttodate.Text = (new DateTime(tempDate.Year, tempDate.Month - 1, 20).ToString("dd/MM/yyyy"));
                            }
                        }

                    }




                    //if (CurrentDate.Month/* - 1*/ == CSdate.Month && CurrentDate.Year == CSdate.Year)
                    //{
                    //    DateTime date = new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, CSdate.Day);
                    //    txtfromdate.Text = date.ToString("dd/MM/yyyy");
                    //}
                    if (CurrentDate.Month/* - 1*/ == CEdate.Month && CurrentDate.Year == CEdate.Year)
                    {
                        DateTime date = DateTime.Now.Date;
                        if (CSdate.Day == 1)
                        {
                            date = new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, CEdate.Day);
                        }
                        else
                        {
                            date = new DateTime(CurrentDate.Year, CurrentDate.Month/* - 1*/, CEdate.Day - 1);

                        }

                        txttodate.Text = date.ToString("dd/MM/yyyy");
                    }
                    btngenratepayment.Enabled = true;
                    btninvoice.Enabled = true;
                }
                else
                {
                    btngenratepayment.Enabled = false;
                    btninvoice.Enabled = false;
                    //LblResult.Text = "There Is No Valid Contract for this month";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert(' There Is No Valid Contract for this month ');", true);

                }


                #endregion End Old Code As on [02-03-2014]


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                return;
            }

            #endregion  End Get Contract Id Based on The Last Day
        }




        public string GetMonthOfYear()
        {
            string MonthYear = "";

            int month = GetMonthBasedOnSelectionDateorMonth();
            if (month.ToString().Length == 4)
            {

                MonthYear = "20" + month.ToString().Substring(2, 2);

            }
            if (month.ToString().Length == 3)
            {

                MonthYear = "20" + month.ToString().Substring(1, 2);

            }
            return MonthYear;
        }

        protected int GetMonth(string NameOfmonth)
        {
            int month = -1;
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            for (int i = 0; i < monthName.Length; i++)
            {
                if (monthName[i].CompareTo(NameOfmonth) == 0)
                {
                    month = i + 1;
                    break;
                }
            }
            return month;
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            if (Chk_Month.Checked == false)
            {
                payMonth = GetMonth(ddlmonth.SelectedValue);
                monthname = mfi.GetMonthName(payMonth).ToString();
            }
            if (Chk_Month.Checked == true)
            {

                DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                monthname = mfi.GetMonthName(date.Month).ToString();
                //payMonth = GetMonth(monthname);
            }
            return monthname;
        }




        protected void txtmonthOnTextChanged(object sender, EventArgs e)
        {
            lblbillnolatest.Text = "";
            txtbilldate.Text = "";
            FillMonthDetails();
            LoadOldBillnos();
            DisplayDataInGrid();
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            var password = string.Empty;
            var SPName = string.Empty;
            password = txtPassword.Text.Trim();
            string sqlPassword = "select password from IouserDetails where password='" + txtPassword.Text + "'";
            DataTable dtpassword =config.ExecuteAdaptorAsyncWithQueryParams(sqlPassword).Result;
            if (dtpassword.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Invalid Password');", true);
                return;
            }

            #region Validation

            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            txtbilldate.Text = string.Empty;
            txtfromdate.Text = string.Empty;
            txttodate.Text = string.Empty;
            lblbillnolatest.Text = string.Empty;
            txtmonth.Text = string.Empty;
            ddlmonth.SelectedIndex = 0;
            ddlMBBillnos.SelectedIndex = 0;
            //txtduedate.Text = string.Empty;
            ClearExtraDataForBilling();
            ClearData();

            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The client Id');", true);
                Chk_Month.Checked = false;
                return;
            }

            #endregion

            Chk_Month.Checked = true;

            if (Chk_Month.Checked)
            {
                txtmonth.Visible = true;
                ddlmonth.SelectedIndex = 0;
                ddlmonth.Visible = false;

            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            modelLogindetails.Hide();
            Chk_Month.Checked = false;
            gvClientBilling.DataSource = null;
            gvClientBilling.DataBind();
            txtbilldate.Text = string.Empty;
            txtfromdate.Text = string.Empty;
            txttodate.Text = string.Empty;
            lblbillnolatest.Text = string.Empty;
            txtmonth.Text = string.Empty;
            ddlmonth.SelectedIndex = 0;
            ddlMBBillnos.SelectedIndex = 0;
            //txtduedate.Text = string.Empty;
            ClearExtraDataForBilling();
            ClearData();
            if (Chk_Month.Checked == false)
            {
                txtmonth.Visible = false;
                txtmonth.Text = "";
                ddlmonth.SelectedIndex = 0;
                ddlmonth.Visible = true;
            }
        }

        protected void ddldutytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddldutytype = sender as DropDownList;
            GridViewRow row = null;
            if (ddldutytype == null)
                return;

            row = (GridViewRow)ddldutytype.NamingContainer;
            if (row == null)
                return;


            TextBox txtnoofemployees = row.FindControl("txtnoofemployees") as TextBox;
            TextBox txtNoOfDuties = row.FindControl("txtNoOfDuties") as TextBox;
            TextBox txtPayRate = row.FindControl("txtPayRate") as TextBox;
            TextBox txtda = row.FindControl("txtda") as TextBox;
            TextBox txtAmount = row.FindControl("txtAmount") as TextBox;

            if (ddldutytype.SelectedIndex == 5)
            {
                txtnoofemployees.Text = "";
                txtNoOfDuties.Text = "";
                txtPayRate.Text = "";
                txtda.Text = "";
                txtAmount.Text = "";
            }




        }
    }
}