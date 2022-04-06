using System;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class AddContract : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        //DataTable dt;
        DropDownList bind_dropdownlist;
        DropDownList bind_dropdownlistsw;
        DropDownList bind_dropdownlistswc;
        DropDownList bind_dropdownlistHSN;

        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        string Emp_Id = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            int i = 0;
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
                    if (this.Master != null)
                    {
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("c1");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }
                    LoadClientList();
                    LoadClientNames();
                    LoadEsibranches();
                    LoadPFbranches();
                    FillddlTakedata();
                    if (Request.QueryString["clientid"] != null || Request.QueryString["ContractID"] != null)
                    {

                        string username = Request.QueryString["clientid"].ToString();
                        ddlclientid.SelectedValue = username;
                        ddlclientid_OnSelectedIndexChanged(sender, e);
                        string ContractID = Request.QueryString["ContractID"].ToString();
                        ddlContractids.SelectedValue = ContractID;
                        ddlContractids_OnSelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        SetInitialRowForInvoice();

                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        private void SetInitialRowForInvoice()
        {
            string SPName = "GetContractDetailForInvoice";
            Hashtable ht = new Hashtable();
            ht.Add("@clientid", ddlclientid.SelectedValue);
            ht.Add("@contractid", ddlContractids.SelectedValue);
            DataTable dtcount = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
            if (dtcount.Rows.Count > 0)
            {

                gvdesignation.DataSource = dtcount;
                gvdesignation.DataBind();

                displaydataForInvoice();

                ViewState["InvoiceTable"] = dtcount;


            }

            else
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("Sno", typeof(int)));
                dt.Columns.Add(new DataColumn("Designations", typeof(string)));
                dt.Columns.Add(new DataColumn("type", typeof(float)));
                //dt.Columns.Add(new DataColumn("GroupBy", typeof(float)));
                dt.Columns.Add(new DataColumn("DutyHrs", typeof(float)));
                dt.Columns.Add(new DataColumn("Quantity", typeof(float)));
                dt.Columns.Add(new DataColumn("Amount", typeof(float)));
                dt.Columns.Add(new DataColumn("PayType", typeof(float)));
                dt.Columns.Add(new DataColumn("Noofdays", typeof(float)));
                dt.Columns.Add(new DataColumn("Summary", typeof(string)));
                // dt.Columns.Add(new DataColumn("Id", typeof(string)));
                dt.Columns.Add(new DataColumn("HSNNumber", typeof(string)));
                dt.Columns.Add(new DataColumn("cdCGST", typeof(int)));
                dt.Columns.Add(new DataColumn("cdSGST", typeof(float)));
                dt.Columns.Add(new DataColumn("cdIGST", typeof(float)));
                dt.Columns.Add(new DataColumn("cdPF", typeof(float)));
                dt.Columns.Add(new DataColumn("cdESI", typeof(float)));
                dt.Columns.Add(new DataColumn("cdSC", typeof(float)));
                dt.Columns.Add(new DataColumn("ServicechargePer", typeof(float)));
                dt.Columns.Add(new DataColumn("CS", typeof(float)));
                dt.Columns.Add(new DataColumn("cdSCOnPFESI", typeof(float)));
                dt.Columns.Add(new DataColumn("otrate", typeof(float)));
                dt.Columns.Add(new DataColumn("Nots", typeof(float)));
                dt.Columns.Add(new DataColumn("rcapplicable", typeof(int)));
                dt.Columns.Add(new DataColumn("Basic", typeof(float)));
                dt.Columns.Add(new DataColumn("DA", typeof(float)));
                dt.Columns.Add(new DataColumn("HRA", typeof(float)));
                dt.Columns.Add(new DataColumn("Conveyance", typeof(float)));
                dt.Columns.Add(new DataColumn("CCA", typeof(float)));
                dt.Columns.Add(new DataColumn("LeaveAmount", typeof(float)));
                dt.Columns.Add(new DataColumn("Gratuity", typeof(float)));
                dt.Columns.Add(new DataColumn("Bonus", typeof(float)));
                dt.Columns.Add(new DataColumn("AttBonus", typeof(float)));
                dt.Columns.Add(new DataColumn("WashAllowance", typeof(float)));
                dt.Columns.Add(new DataColumn("OtherAllowance", typeof(float)));
                dt.Columns.Add(new DataColumn("NFhs", typeof(float)));
                dt.Columns.Add(new DataColumn("RC", typeof(float)));
             
                //dt.Columns.Add(new DataColumn("LWF", typeof(float)));

                for (int i = 1; i < 6; i++)
                {

                    dr = dt.NewRow();
                    dr["Sno"] = 1;
                    dr["Designations"] = string.Empty;
                    dr["type"] = 0;
                    //dr["GroupBy"] = 0;
                    dr["DutyHrs"] = 0;
                    dr["Quantity"] = 0;
                    dr["Amount"] = 0;
                    dr["PayType"] = 0;
                    dr["Noofdays"] = 0;
                    dr["Summary"] = string.Empty;
                    //dr["Id"] = string.Empty;
                    dr["HSNNumber"] = string.Empty;
                    dr["cdCGST"] = 0;
                    dr["cdSGST"] = 0;
                    dr["cdIGST"] = 0;
                    dr["cdPF"] = 0;
                    dr["cdESI"] = 0;
                    dr["cdSC"] = 0;
                    dr["ServicechargePer"] = 0;
                    dr["CS"] = 0;
                    dr["cdSCOnPFESI"] = 0;
                    dr["otrate"] = 0;
                    dr["Nots"] = 0;
                    dr["rcapplicable"] = 0;
                    // dr["LWF"] = 0;
                    dr["Basic"] = 0;
                    dr["DA"] = 0;
                    dr["HRA"] = 0;
                    dr["Conveyance"] = 0;
                    dr["CCA"] = 0;
                    dr["LeaveAmount"] = 0;
                    dr["Gratuity"] = 0;
                    dr["Bonus"] = 0;
                    dr["AttBonus"] = 0;
                    dr["WashAllowance"] = 0;
                    dr["OtherAllowance"] = 0;
                    dr["NFhs"] = 0;
                    dr["RC"] = 0;

                    dt.Rows.Add(dr);

                }


                gvdesignation.DataSource = dt;
                gvdesignation.DataBind();

                displaydataForInvoice();


                //Store the DataTable in ViewState
                ViewState["InvoiceTable"] = dt;
            }

        }

        private void SetInitialRowForSW()
        {
            string SPName = "GetContractDetailForPaySheet";
            Hashtable ht = new Hashtable();
            ht.Add("@clientid", ddlclientid.SelectedValue);
            ht.Add("@contractid", ddlContractids.SelectedValue);
            DataTable dtcount = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
            if (dtcount.Rows.Count > 0)
            {

                gvSWDesignations.DataSource = dtcount;
                gvSWDesignations.DataBind();

                displaydataForSW();

                ViewState["CurrentTable"] = dtcount;


            }

            else
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("SNo", typeof(int)));
                dt.Columns.Add(new DataColumn("Designations", typeof(string)));
                dt.Columns.Add(new DataColumn("Shift", typeof(float)));
                dt.Columns.Add(new DataColumn("Category", typeof(string)));
                dt.Columns.Add(new DataColumn("Noofdays", typeof(float)));
                dt.Columns.Add(new DataColumn("Gross", typeof(float)));
                dt.Columns.Add(new DataColumn("NetPay", typeof(float)));
                dt.Columns.Add(new DataColumn("Basic", typeof(float)));
                dt.Columns.Add(new DataColumn("DA", typeof(float)));
                dt.Columns.Add(new DataColumn("HRA", typeof(float)));
                //dt.Columns.Add(new DataColumn("HRABILL", typeof(float)));
                dt.Columns.Add(new DataColumn("Conveyance", typeof(float)));
                dt.Columns.Add(new DataColumn("MedicalAllowance", typeof(float)));
                dt.Columns.Add(new DataColumn("FoodAllowance", typeof(float)));
                dt.Columns.Add(new DataColumn("SITEALLOW", typeof(float)));
                dt.Columns.Add(new DataColumn("SplAllowance", typeof(float)));
                dt.Columns.Add(new DataColumn("NoOfSplDays", typeof(string)));
                dt.Columns.Add(new DataColumn("OtherAllowance", typeof(float)));
                dt.Columns.Add(new DataColumn("ADDL4HR", typeof(float)));
                dt.Columns.Add(new DataColumn("QTRALLOW", typeof(float)));
                dt.Columns.Add(new DataColumn("WashAllowance", typeof(float)));
                dt.Columns.Add(new DataColumn("RELALLOW", typeof(float)));
                dt.Columns.Add(new DataColumn("Gunallw", typeof(float)));
                dt.Columns.Add(new DataColumn("GunAllwType", typeof(string)));
                dt.Columns.Add(new DataColumn("Fireallw", typeof(float)));
                dt.Columns.Add(new DataColumn("Bonus", typeof(float)));
                dt.Columns.Add(new DataColumn("Bonustype", typeof(int)));
                dt.Columns.Add(new DataColumn("Gratuity", typeof(float)));
                dt.Columns.Add(new DataColumn("Gratuitytype", typeof(float)));
                dt.Columns.Add(new DataColumn("LeaveAmount", typeof(float)));
                dt.Columns.Add(new DataColumn("latype", typeof(float)));
                dt.Columns.Add(new DataColumn("otrate", typeof(float)));
                dt.Columns.Add(new DataColumn("Nots", typeof(float)));
                dt.Columns.Add(new DataColumn("ESIRate", typeof(float)));
                dt.Columns.Add(new DataColumn("ESIDays", typeof(float)));
                dt.Columns.Add(new DataColumn("otesiWages", typeof(float)));
                dt.Columns.Add(new DataColumn("OTESIWagesDays", typeof(string)));
                dt.Columns.Add(new DataColumn("PFRate", typeof(float)));
                dt.Columns.Add(new DataColumn("PFDays", typeof(float)));
                dt.Columns.Add(new DataColumn("chkPF", typeof(float)));
                dt.Columns.Add(new DataColumn("Chkesi", typeof(float)));
                dt.Columns.Add(new DataColumn("CCA", typeof(float)));
                dt.Columns.Add(new DataColumn("AttBonus", typeof(float)));
                dt.Columns.Add(new DataColumn("AttBonustype", typeof(float)));
                dt.Columns.Add(new DataColumn("NFhs", typeof(float)));
                dt.Columns.Add(new DataColumn("RC", typeof(float)));
                dt.Columns.Add(new DataColumn("PLDays", typeof(float)));
                dt.Columns.Add(new DataColumn("PLAmount", typeof(float)));
                dt.Columns.Add(new DataColumn("TLDays", typeof(float)));
                dt.Columns.Add(new DataColumn("TLAmount", typeof(float)));
                dt.Columns.Add(new DataColumn("CS", typeof(float)));
                dt.Columns.Add(new DataColumn("ServicechargePer", typeof(float)));
                dt.Columns.Add(new DataColumn("NNhs", typeof(string)));
                dt.Columns.Add(new DataColumn("NhsRate", typeof(float)));
                dt.Columns.Add(new DataColumn("NWos", typeof(float)));
                dt.Columns.Add(new DataColumn("WoRate", typeof(float)));
                dt.Columns.Add(new DataColumn("TravelAllw", typeof(float)));
                dt.Columns.Add(new DataColumn("PerformanceAllw", typeof(float)));
                dt.Columns.Add(new DataColumn("MobileAllw", typeof(float)));
                dt.Columns.Add(new DataColumn("OTHrs", typeof(float)));
                dt.Columns.Add(new DataColumn("AdvDed", typeof(float)));
                dt.Columns.Add(new DataColumn("WCDed", typeof(float)));
                dt.Columns.Add(new DataColumn("UniformDed", typeof(float)));
                dt.Columns.Add(new DataColumn("ServiceWeightage", typeof(float)));
                dt.Columns.Add(new DataColumn("IncentiveRate", typeof(float)));
                dt.Columns.Add(new DataColumn("LCRate", typeof(float)));


                //dt.Columns.Add(new DataColumn("addlhrallw", typeof(float)));
                ////dt.Columns.Add(new DataColumn("Conveyancebill", typeof(float)));
                //dt.Columns.Add(new DataColumn("CCABILL", typeof(float)));
                //dt.Columns.Add(new DataColumn("LWType", typeof(int)));
                //dt.Columns.Add(new DataColumn("Incentives", typeof(float)));
                //dt.Columns.Add(new DataColumn("NoOfSplDays", typeof(float)));
                //dt.Columns.Add(new DataColumn("UniformAmtbill", typeof(float)));
                //dt.Columns.Add(new DataColumn("UniformAmtDaysbill", typeof(float)));


                for (int i = 1; i < 6; i++)
                {

                    dr = dt.NewRow();
                    dr["SNo"] = 1;
                    dr["Designations"] = string.Empty;
                    dr["Shift"] = 0;
                    dr["Category"] = string.Empty;
                    dr["Noofdays"] = 0;
                    dr["Gross"] = 0;
                    dr["NetPay"] = 0;
                    dr["Basic"] = 0;
                    dr["DA"] = 0;
                    dr["HRA"] = 0;
                    //dr["HRABILL"] = 0;
                    dr["Conveyance"] = 0;

                    dr["MedicalAllowance"] = 0;
                    dr["FoodAllowance"] = 0;
                    dr["SITEALLOW"] = 0;
                    dr["SplAllowance"] = 0;
                    dr["NoOfSplDays"] = 0;
                    dr["OtherAllowance"] = 0;
                    dr["QTRALLOW"] = 0;
                    dr["WashAllowance"] = 0;
                    dr["RELALLOW"] = 0;
                    dr["Gunallw"] = 0;
                    dr["GunAllwType"] = 0;
                    dr["Fireallw"] = 0;
                    dr["Bonus"] = 0;
                    dr["Bonustype"] = 0;
                    dr["Gratuity"] = 0;
                    dr["Gratuitytype"] = 0;
                    dr["LeaveAmount"] = 0;
                    dr["latype"] = 0;
                    dr["otrate"] = 0;
                    dr["Nots"] = 0;
                    dr["ESIRate"] = 0;
                    dr["ESIDays"] = 0;

                    dr["otesiWages"] = 0;
                    dr["OTESIWagesDays"] = 0;

                    dr["PFRate"] = 0;
                    dr["PFDays"] = 0;

                    dr["ChkPF"] = 0;
                    dr["Chkesi"] = 0;
                    dr["CCA"] = 0;
                    dr["AttBonus"] = 0;
                    dr["AttBonustype"] = 0;
                    dr["NFhs"] = 0;
                    dr["RC"] = 0;
                    dr["PLDays"] = 0;
                    dr["PLAmount"] = 0;
                    dr["TLDays"] = 0;
                    dr["TLAmount"] = 0;
                    dr["CS"] = 0;
                    dr["ServicechargePer"] = 0;
                    dr["NNhs"] = 0;
                    dr["NhsRate"] = 0;
                    dr["NWos"] = 0;
                    dr["WoRate"] = 0;
                    dr["TravelAllw"] = 0;
                    dr["PerformanceAllw"] = 0;
                    dr["MobileAllw"] = 0;
                    dr["OTHrs"] = 0;
                    dr["AdvDed"] = 0;
                    dr["WCDed"] = 0;
                    dr["UniformDed"] = 0;
                    dr["ServiceWeightage"] = 0;

                    //dr["Conveyancebill"] = 0;
                    //dr["CCABILL"] = 0;
                    //dr["LWType"] = 0;
                    //dr["WashAllowance"] = 0;
                    //dr["OtherAllowance"] = 0;
                    //dr["Incentives"] = 0;
                    //dr["FoodAllowance"] = 0;
                    //dr["MedicalAllowance"] = 0;
                    //dr["SplAllowance"] = 0;
                    //dr["SplAllwDays"] = 0;
                    //dr["NoOfSplDays"] = 0;
                    ////dr["otrate"] = 0;
                    //dr["UniformAmtbill"] = 0;
                    //dr["UniformAmtDaysbill"] = 0;
                    //dr["Nots"] = 0;
                    dt.Rows.Add(dr);

                }


                gvSWDesignations.DataSource = dt;
                gvSWDesignations.DataBind();

                displaydataForSW();


                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = dt;
            }

        }

        private void displaydataForInvoice()
        {

            DataTable DtDesignation = GlobalData.Instance.LoadDesigns();

            DataTable DtHSNNumber = GlobalData.Instance.LoadHSNNumbers();



            foreach (GridViewRow grdRow in gvdesignation.Rows)
            {
                bind_dropdownlist = (DropDownList)(gvdesignation.Rows[grdRow.RowIndex].Cells[1].FindControl("DdlDesign"));
                bind_dropdownlist.Items.Clear();

                if (DtDesignation.Rows.Count > 0)
                {
                    bind_dropdownlist.DataValueField = "Designid";
                    bind_dropdownlist.DataTextField = "Design";
                    bind_dropdownlist.DataSource = DtDesignation;
                    bind_dropdownlist.DataBind();

                }
                bind_dropdownlist.Items.Insert(0, "--Select--");
                bind_dropdownlist.SelectedIndex = 0;

                bind_dropdownlistHSN = (DropDownList)(gvdesignation.Rows[grdRow.RowIndex].Cells[9].FindControl("ddlHSNNumber"));

                if (DtHSNNumber.Rows.Count > 0)
                {
                    bind_dropdownlistHSN.DataValueField = "Id";
                    bind_dropdownlistHSN.DataTextField = "HSNNo";
                    bind_dropdownlistHSN.DataSource = DtHSNNumber;
                    bind_dropdownlistHSN.DataBind();

                }
            }

        }

        private void displaydataForSW()
        {
            DataTable DtDesignation = GlobalData.Instance.LoadDesigns();

            DataTable DtCategory = GlobalData.Instance.LoadWageCategory();

            foreach (GridViewRow grdRow in gvSWDesignations.Rows)
            {
                bind_dropdownlistsw = (DropDownList)(gvSWDesignations.Rows[grdRow.RowIndex].Cells[0].FindControl("DdlDesign"));
                bind_dropdownlistsw.Items.Clear();

                if (DtDesignation.Rows.Count > 0)
                {
                    bind_dropdownlistsw.DataValueField = "Designid";
                    bind_dropdownlistsw.DataTextField = "Design";
                    bind_dropdownlistsw.DataSource = DtDesignation;
                    bind_dropdownlistsw.DataBind();

                }
                bind_dropdownlistsw.Items.Insert(0, "--Select--");
                bind_dropdownlistsw.SelectedIndex = 0;


                bind_dropdownlistswc = (DropDownList)(gvSWDesignations.Rows[grdRow.RowIndex].Cells[1].FindControl("DdlCategory"));
                bind_dropdownlistswc.Items.Clear();

                if (DtCategory.Rows.Count > 0)
                {
                    bind_dropdownlistswc.DataValueField = "id";
                    bind_dropdownlistswc.DataTextField = "name";
                    bind_dropdownlistswc.DataSource = DtCategory;
                    bind_dropdownlistswc.DataBind();

                }
                bind_dropdownlistswc.Items.Insert(0, "--Select--");
                bind_dropdownlistswc.SelectedIndex = 0;

            }




        }

        protected void Displaydefaulrowsw()
        {
            for (int i = 0; i < gvSWDesignations.Rows.Count; i++)
            {
                if (i < 1)
                {
                    Session["ContractsAIndexsw"] = Convert.ToInt16(Session["ContractsAIndexsw"]) + 1;
                    int a = int.Parse(Session["ContractsAIndexsw"].ToString());
                    gvSWDesignations.Rows[i].Visible = true;
                    DefaultRowDatasw(i);
                }
                else
                    gvSWDesignations.Rows[i].Visible = false;
            }
            Session["ContractsAIndexsw"] = 1;
            int check = int.Parse(Session["ContractsAIndexsw"].ToString());
        }

        public void addInvoicedetails(string Clientid, string Contractid)
        {

            try
            {

                int clientdesigncount = 0;
                for (int i = 0; i < gvdesignation.Rows.Count; i++)
                {
                    #region for variables declaration
                    string SNo = "1";
                    string Cddldesignationsw = "";
                    string Cddltype = "";
                    string CDutyhrs = "0";
                    string CddlGroupBy = "";
                    string CQty = "0";
                    string CPayRate = "0";
                    string CddlDutytype = "";
                    string NoOfDaysForWages = "0";
                    string CSummary = "";
                    string CddlHSNNumber = "";
                    string CCGST = "0";
                    string CSGST = "0";
                    string CIGST = "0";
                    string NoOfOts = "0";
                    string CRCAppl = "0";
                    string Cbasicsw = "0";
                    string Cdasw = "0";
                    string Chrasw = "0";
                    string CConveyancesw = "0";
                    string Ccasw = "0";
                    string LeaveAmountsw = "0";
                    string Bonussw = "0";
                    string Gratutysw = "0";
                    string AttBonussw = "0";
                    string Cwashallowancesw = "0";
                    string Cawsw = "0";
                    string Nfhssw = "0";
                    string RC = "0";
                    string CS = "0";
                    string CSpersw = "0";
                    string OTRATE = "0";
                    string cdPF = "0";
                    string cdESI = "0";
                    string cdSC = "0";
                    string cdSCOnPFESI = "0";
                    string cdBonus = "0";
                    string PFRate = "0";
                    string ESIRate = "0";
                    string LWF = "0";
                    string CdRCOnSC = "0";
                    string CId = "";
                    #endregion for variables declaration

                    TextBox lblCSlno = gvdesignation.Rows[i].FindControl("lblCSlno") as TextBox;
                    DropDownList DdlDesign = gvdesignation.Rows[i].Cells[0].FindControl("DdlDesign") as DropDownList;
                    DropDownList ddlType = gvdesignation.Rows[i].Cells[1].FindControl("ddlType") as DropDownList;
                    // DropDownList ddlGroupBy = gvdesignation.Rows[i].Cells[2].FindControl("ddlGroupBy") as DropDownList;
                    TextBox txtdutyhrs = gvdesignation.Rows[i].Cells[2].FindControl("txtdutyhrs") as TextBox;
                    TextBox txtquantity = gvdesignation.Rows[i].Cells[3].FindControl("txtquantity") as TextBox;
                    TextBox txtPayRate = gvdesignation.Rows[i].Cells[4].FindControl("txtPayRate") as TextBox;
                    DropDownList ddldutytype = gvdesignation.Rows[i].Cells[5].FindControl("ddldutytype") as DropDownList;
                    DropDownList ddlNoOfDaysBilling = gvdesignation.Rows[i].Cells[6].FindControl("ddlNoOfDaysBilling") as DropDownList;
                    TextBox txtsummary = gvdesignation.Rows[i].Cells[7].FindControl("txtsummary") as TextBox;
                    //TextBox txtId = gvdesignation.Rows[i].Cells[9].FindControl("txtId") as TextBox;
                    DropDownList ddlHSNNumber = gvdesignation.Rows[i].Cells[8].FindControl("ddlHSNNumber") as DropDownList;
                    CheckBox chkcdCGST = gvdesignation.Rows[i].Cells[9].FindControl("chkcdCGST") as CheckBox;
                    CheckBox chkcdSGST = gvdesignation.Rows[i].Cells[10].FindControl("chkcdSGST") as CheckBox;
                    CheckBox chkcdIGST = gvdesignation.Rows[i].Cells[11].FindControl("chkcdIGST") as CheckBox;

                    CheckBox chkcdPF = gvdesignation.Rows[i].Cells[12].FindControl("chkcdPF") as CheckBox;
                    CheckBox chkcdESI = gvdesignation.Rows[i].Cells[13].FindControl("chkcdESI") as CheckBox;
                    CheckBox chkcdSC = gvdesignation.Rows[i].Cells[14].FindControl("chkcdSC") as CheckBox;
                    TextBox TxtScPer = gvdesignation.Rows[i].Cells[15].FindControl("TxtScPer") as TextBox;
                    TextBox TxtCs = gvdesignation.Rows[i].Cells[16].FindControl("TxtCs") as TextBox;
                    CheckBox chkcdSCOnPFESI = gvdesignation.Rows[i].Cells[17].FindControl("chkcdSCOnPFESI") as CheckBox;
                    TextBox TxtOTRate = gvdesignation.Rows[i].Cells[18].FindControl("TxtOTRate") as TextBox;
                    DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[i].Cells[19].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
                    CheckBox ChkRCApplicable = gvdesignation.Rows[i].Cells[20].FindControl("ChkRCApplicable") as CheckBox;
                    TextBox TxtBasic = gvdesignation.Rows[i].Cells[21].FindControl("TxtBasic") as TextBox;
                    TextBox txtda = gvdesignation.Rows[i].Cells[22].FindControl("txtda") as TextBox;
                    TextBox txthra = gvdesignation.Rows[i].Cells[23].FindControl("txthra") as TextBox;
                    TextBox txtConveyance = gvdesignation.Rows[i].Cells[24].FindControl("txtConveyance") as TextBox;
                    TextBox txtcca = gvdesignation.Rows[i].Cells[25].FindControl("txtcca") as TextBox;
                    TextBox txtleaveamount = gvdesignation.Rows[i].Cells[26].FindControl("txtleaveamount") as TextBox;
                    TextBox txtgratuty = gvdesignation.Rows[i].Cells[27].FindControl("txtgratuty") as TextBox;
                    TextBox txtbonus = gvdesignation.Rows[i].Cells[28].FindControl("txtbonus") as TextBox;
                    TextBox txtattbonus = gvdesignation.Rows[i].Cells[29].FindControl("txtattbonus") as TextBox;
                    TextBox txtwa = gvdesignation.Rows[i].Cells[30].FindControl("txtwa") as TextBox;
                    TextBox txtoa = gvdesignation.Rows[i].Cells[31].FindControl("txtoa") as TextBox;
                    TextBox txtNfhs1 = gvdesignation.Rows[i].Cells[32].FindControl("txtNfhs") as TextBox;
                    TextBox Txtrc = gvdesignation.Rows[i].Cells[33].FindControl("Txtrc") as TextBox;

                    //CheckBox chkcdBonus = gvdesignation.Rows[i].Cells[16].FindControl("chkcdBonus") as CheckBox;
                    //TextBox txtpfrate = gvdesignation.Rows[i].Cells[21].FindControl("txtpfrate") as TextBox;
                    //TextBox txtesirate = gvdesignation.Rows[i].Cells[22].FindControl("txtesirate") as TextBox;
                    //CheckBox ChkRCOnSC = gvdesignation.Rows[i].Cells[24].FindControl("ChkRCOnSC") as CheckBox;
                    //TextBox Txtlwf = gvdesignation.Rows[i].Cells[25].FindControl("Txtlwf") as TextBox;


                    #region for variables initialisation

                    SNo = lblCSlno.Text;
                    Cddldesignationsw = DdlDesign.SelectedValue;
                    Cddltype = ddlType.SelectedValue;

                    if (ddlNoOfDaysBilling.SelectedIndex == 0)
                    {
                        NoOfDaysForWages = "0";
                    }
                    if (ddlNoOfDaysBilling.SelectedIndex == 1)
                    {
                        NoOfDaysForWages = "1";
                    }
                    if (ddlNoOfDaysBilling.SelectedIndex == 2)
                    {
                        NoOfDaysForWages = "2";
                    }
                    if (ddlNoOfDaysBilling.SelectedIndex == 3)
                    {
                        NoOfDaysForWages = "3";
                    }
                    if (ddlNoOfDaysBilling.SelectedIndex == 4)
                    {
                        NoOfDaysForWages = "4";
                    }
                    if (ddlNoOfDaysBilling.SelectedIndex > 4)
                    {
                        NoOfDaysForWages = ddlNoOfDaysBilling.SelectedValue;
                    }

                    if (ddldutytype.SelectedIndex == 0)
                    {
                        CddlDutytype = "0";
                    }
                    if (ddldutytype.SelectedIndex == 1)
                    {
                        CddlDutytype = "1";
                    }
                    if (ddldutytype.SelectedIndex == 2)
                    {
                        CddlDutytype = "2";
                    }
                    if (ddldutytype.SelectedIndex == 3)
                    {
                        CddlDutytype = "3";
                    }
                    if (ddldutytype.SelectedIndex == 4)
                    {
                        CddlDutytype = "4";
                    }
                    if (ddldutytype.SelectedIndex == 5)
                    {
                        CddlDutytype = "5";
                    }
                    if (ddldutytype.SelectedIndex == 6)
                    {
                        CddlDutytype = "6";
                    }
                    if (ddldutytype.SelectedIndex == 7)
                    {
                        CddlDutytype = "7";
                    }

                    if (ddlNoOfOtsPaysheet.SelectedIndex == 0)
                    {
                        NoOfOts = "0";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 1)
                    {
                        NoOfOts = "1";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 2)
                    {
                        NoOfOts = "2";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 3)
                    {
                        NoOfOts = "3";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 4)
                    {
                        NoOfOts = "4";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex > 4)
                    {
                        NoOfOts = ddlNoOfOtsPaysheet.SelectedValue;
                    }

                    CddlHSNNumber = ddlHSNNumber.SelectedValue;

                    if (txtdutyhrs.Text == "")
                    {
                        CDutyhrs = "0";
                    }
                    else
                    {
                        CDutyhrs = txtdutyhrs.Text;
                    }

                    if (txtquantity.Text == "")
                    {
                        CQty = "0";
                    }
                    else
                    {
                        CQty = txtquantity.Text;
                    }

                    if (txtPayRate.Text == "")
                    {
                        CPayRate = "0";
                    }
                    else
                    {
                        CPayRate = txtPayRate.Text;
                    }

                    //if (Txtlwf.Text == "")
                    //{
                    //    LWF = "0";
                    //}
                    //else
                    //{
                    //    LWF = Txtlwf.Text;
                    //}

                    if (txtsummary.Text == "")
                    {
                        CSummary = "";
                    }
                    else
                    {
                        CSummary = txtsummary.Text;
                    }

                    //if (txtId.Text == "")
                    //{
                    //    CId = "0";
                    //}
                    //else
                    //{
                    //    CId = txtId.Text;
                    //}

                    if (chkcdCGST.Checked == true)
                    {
                        CCGST = "1";
                    }
                    else
                    {
                        CCGST = "0";
                    }

                    if (chkcdSGST.Checked == true)
                    {
                        CSGST = "1";
                    }
                    else
                    {
                        CSGST = "0";
                    }

                    if (chkcdIGST.Checked == true)
                    {
                        CIGST = "1";
                    }
                    else
                    {
                        CIGST = "0";
                    }
                    if (chkcdPF.Checked == true)
                    {
                        cdPF = "1";
                    }
                    else
                    {
                        cdPF = "0";
                    }
                    if (chkcdESI.Checked == true)
                    {
                        cdESI = "1";
                    }
                    else
                    {
                        cdESI = "0";
                    }

                    //if (chkcdBonus.Checked == true)
                    //{
                    //    cdBonus = "1";
                    //}
                    //else
                    //{
                    //    cdBonus = "0";
                    //}

                    //if (ChkRCOnSC.Checked == true)
                    //{
                    //    CdRCOnSC = "1";
                    //}
                    //else
                    //{
                    //    CdRCOnSC = "0";
                    //}

                    //if (txtpfrate.Text == "")
                    //{
                    //    PFRate = "0";
                    //}
                    //else
                    //{
                    //    PFRate = txtpfrate.Text;
                    //}

                    //if (txtesirate.Text == "")
                    //{
                    //    ESIRate = "0";
                    //}
                    //else
                    //{
                    //    ESIRate = txtesirate.Text;
                    //}

                    if (chkcdSC.Checked == true)
                    {
                        cdSC = "1";
                    }
                    else
                    {
                        cdSC = "0";
                    }

                    if (chkcdSCOnPFESI.Checked == true)
                    {
                        cdSCOnPFESI = "1";
                    }
                    else
                    {
                        cdSCOnPFESI = "0";
                    }

                    if (ChkRCApplicable.Checked == true)
                    {
                        CRCAppl = "1";
                    }
                    else
                    {
                        CRCAppl = "0";
                    }

                    if (TxtBasic.Text == "")
                    {
                        Cbasicsw = "0";
                    }
                    else
                    {
                        Cbasicsw = TxtBasic.Text;
                    }

                    if (txtda.Text == "")
                    {
                        Cdasw = "0";
                    }
                    else
                    {
                        Cdasw = txtda.Text;
                    }

                    if (txthra.Text == "")
                    {
                        Chrasw = "0";
                    }
                    else
                    {
                        Chrasw = txthra.Text;
                    }

                    if (txtConveyance.Text == "")
                    {
                        CConveyancesw = "0";
                    }
                    else
                    {
                        CConveyancesw = txtConveyance.Text;
                    }

                    if (txtcca.Text == "")
                    {
                        Ccasw = "0";
                    }
                    else
                    {
                        Ccasw = txtcca.Text;
                    }

                    if (txtleaveamount.Text == "")
                    {
                        LeaveAmountsw = "0";
                    }
                    else
                    {
                        LeaveAmountsw = txtleaveamount.Text;
                    }

                    if (txtleaveamount.Text == "")
                    {
                        LeaveAmountsw = "0";
                    }
                    else
                    {
                        LeaveAmountsw = txtleaveamount.Text;
                    }


                    if (txtbonus.Text == "")
                    {
                        Bonussw = "0";
                    }
                    else
                    {
                        Bonussw = txtbonus.Text;
                    }

                    if (txtgratuty.Text == "")
                    {
                        Gratutysw = "0";
                    }
                    else
                    {
                        Gratutysw = txtgratuty.Text;
                    }

                    if (txtattbonus.Text == "")
                    {
                        AttBonussw = "0";
                    }
                    else
                    {
                        AttBonussw = txtattbonus.Text;
                    }

                    if (txtwa.Text == "")
                    {
                        Cwashallowancesw = "0";
                    }
                    else
                    {
                        Cwashallowancesw = txtwa.Text;
                    }

                    if (txtoa.Text == "")
                    {
                        Cawsw = "0";
                    }
                    else
                    {
                        Cawsw = txtoa.Text;
                    }


                    //  NoOfDaysForWages = ddln;

                    if (txtNfhs1.Text == "")
                    {
                        Nfhssw = "0";
                    }
                    else
                    {
                        Nfhssw = txtNfhs1.Text;
                    }

                    if (Txtrc.Text == "")
                    {
                        RC = "0";
                    }
                    else
                    {
                        RC = Txtrc.Text;
                    }
                    if (TxtCs.Text == "")
                    {
                        CS = "0";
                    }
                    else
                    {
                        CS = TxtCs.Text;
                    }

                    if (TxtScPer.Text == "")
                    {
                        CSpersw = "0";
                    }
                    else
                    {
                        CSpersw = TxtScPer.Text;
                    }
                    if (TxtOTRate.Text == "")
                    {
                        OTRATE = "0";
                    }
                    else
                    {
                        OTRATE = TxtOTRate.Text;
                    }

                    #endregion for variables initialisation

                    string SPName = "GetContractDetails";
                    Hashtable HtContracts = new Hashtable();

                    if (DdlDesign.SelectedIndex > 0)
                    {

                        HtContracts.Add("@Sno", SNo);
                        HtContracts.Add("@ClientID", Clientid);
                        HtContracts.Add("@ContractId", Contractid);
                        HtContracts.Add("@Designations", Cddldesignationsw);
                        HtContracts.Add("@type", Cddltype);
                        HtContracts.Add("@DutyHrs", CDutyhrs);
                        //HtContracts.Add("@Groupby", CddlGroupBy);
                        HtContracts.Add("@Quantity", CQty);
                        HtContracts.Add("@Amount", CPayRate);
                        HtContracts.Add("@PayType", CddlDutytype);
                        HtContracts.Add("@NoOfDays", NoOfDaysForWages);
                        HtContracts.Add("@Summary", CSummary);
                        // HtContracts.Add("@Id", CId);
                        HtContracts.Add("@HSNNumber", CddlHSNNumber);
                        HtContracts.Add("@cdCGST", CCGST);
                        HtContracts.Add("@cdSGST", CSGST);
                        HtContracts.Add("@cdIGST", CIGST);
                        HtContracts.Add("@cdPF", cdPF);
                        HtContracts.Add("@cdESI", cdESI);

                        HtContracts.Add("@cdSC", cdSC);
                        HtContracts.Add("@cdSCOnPFESI", cdSCOnPFESI);
                        HtContracts.Add("@Nots", NoOfOts);
                        HtContracts.Add("@RCApplicable", CRCAppl);
                        //HtContracts.Add("@LWF", LWF);
                        HtContracts.Add("@Basic", Cbasicsw);
                        HtContracts.Add("@DA", Cdasw);
                        HtContracts.Add("@HRA", Chrasw);
                        HtContracts.Add("@Conveyance", CConveyancesw);
                        HtContracts.Add("@CCA", Ccasw);
                        HtContracts.Add("@LeaveAmount", LeaveAmountsw);
                        HtContracts.Add("@Bonus", Bonussw);
                        HtContracts.Add("@Gratuity", Gratutysw);
                        HtContracts.Add("@attbonus", AttBonussw);
                        HtContracts.Add("@WashAllownce", Cwashallowancesw);
                        HtContracts.Add("@OtherAllowance", Cawsw);
                        HtContracts.Add("@nfhs", Nfhssw);
                        HtContracts.Add("@Rc", RC);
                        HtContracts.Add("@cs", CS);
                        HtContracts.Add("@servicechargeper", CSpersw);
                        HtContracts.Add("@OTRATE", OTRATE);
                        HtContracts.Add("@testrecord", clientdesigncount);
                        //HtContracts.Add("@PFRate", PFRate);
                        //HtContracts.Add("@ESIRate", ESIRate);
                        //HtContracts.Add("@cdBonus", cdBonus);
                        // HtContracts.Add("@CdRCOnSC", CdRCOnSC);


                        int IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;

                        if (IRecordStatus != 0)
                        {
                            clientdesigncount++;
                        }
                    }
                }
            }
            catch (Exception cx)
            {
                throw cx;
            }

        }

        public void addSplWagesdetails(string Clientid, string Contractid)
        {
            try
            {

                int specialdesigncount = 0;
                for (int i = 0; i < gvSWDesignations.Rows.Count; i++)
                {
                    #region for variables declaration

                    string Cddldesignationsw = "";
                    string CddlCategory = "";
                    string Cgrosssw = "0";
                    string CTxtNetPaysw = "0";
                    string Cbasicsw = "0";
                    string Cdasw = "0";
                    string Chrasw = "0";
                    string RELALLOW = "0";
                    string CConveyancesw = "0";
                    string MedicalAllw = "0";
                    string CConveyancebillsw = "0";
                    string Cothrs = "0";
                    string Cservicewt = "0";
                    string MiscDed = "0";
                    string Cwashallowancesw = "0";
                    string BonusType = "0";
                    string FireAllw = "0";
                    string GunAllwType = "0";
                    string GunAllw = "0";
                    string ralAllw = "0";
                    string QTRALLOW = "0";
                    string ADDL4HR = "0";
                    string siteAllw = "0";
                    string LWType = "0";
                    string bonustype = "0";
                    string gratuitytype = "0";
                    string latype = "0";
                    string otesiwages = "0";
                    string OTESIWagesDays = "0";
                    string Attbonustype = "0";
                    string Ccasw = "0";
                    string Cawsw = "0";
                    string LeaveAmountsw = "0";
                    string Bonussw = "0";
                    string Gratutysw = "0";
                    string NoOfDaysForWages = "0";
                    string Nfhssw = "0";
                    string NoOfOts = "0";
                    string NoOfNhs = "0";
                    string NoOfWos = "0";
                    string Shift = "0";
                    string RC = "0";
                    string Incentives = "0";
                    string CS = "0";
                    string OTRATE = "0";
                    string OTHRS = "0";
                    string AttBonussw = "0";
                    string CSpersw = "0";
                    string CSWPLDays = "0";
                    string CSWPLAmount = "0";
                    string CSWTLDays = "0";
                    string CSWTLAmount = "0";
                    string CtxtTLnhsamt = "0";
                    string CtxtTLwoamt = "0";
                    string CtxtTLfoodamt = "0";
                    string CtxtTLmedicalamt = "0";
                    string CtxtTLSpecialamt = "0";
                    string ServiceWeightage = "0";
                    string HardshipAllw = "0";
                    string RankAllw = "0";
                    string uniformcharge = "0";
                    string SiteAllw = "0";
                    string CtxtTLadvded = "0";
                    string CtxtTLwcded = "0";
                    string CtxtTLunidedsw = "0";
                    string ESIDesgnPayOn = "0";
                    string CTxtPFDesgnLimitSw = "0";
                    string PFDesgnPayOn = "0";
                    string NoOfSplDays = "0";
                    string ESIPayRate = "0";
                    string PFPayRate = "0";
                    string CddlNoOfPFDaysPaysheet = "0";
                    string CddlNoOfESIDaysPaysheet = "0";
                    string CTxtTravelAllowance = "0";
                    string CTxtPerformanceAllowance = "0";
                    string CTxtMobileAllowance = "0";
                    string CdswPF = "0";
                    string CdswESI = "0";

                    string PFEmp = "0";
                    string ESIEmp = "0";
                    string PFEmpr = "0";
                    string ESIEmpr = "0";
                    string SCEmp = "0";
                    string CTC = "0";
                    string IncentiveRate = "0";
                    string LCRate = "0";

                    #endregion for variables declaration

                    TextBox lblCSlno = gvSWDesignations.Rows[i].FindControl("lblCSlno") as TextBox;
                    DropDownList DdlDesign = gvSWDesignations.Rows[i].FindControl("DdlDesign") as DropDownList;
                    DropDownList ddlshift = gvSWDesignations.Rows[i].FindControl("ddlshift") as DropDownList;
                    DropDownList DdlCategory = gvSWDesignations.Rows[i].FindControl("DdlCategory") as DropDownList;
                    DropDownList ddlNoOfDaysWages = gvSWDesignations.Rows[i].FindControl("ddlNoOfDaysWages") as DropDownList;
                    TextBox Txtgross = gvSWDesignations.Rows[i].FindControl("Txtgross") as TextBox;
                    TextBox TxtNetPay = gvSWDesignations.Rows[i].FindControl("TxtNetPay") as TextBox;
                    TextBox TxtBasic = gvSWDesignations.Rows[i].FindControl("TxtBasic") as TextBox;
                    TextBox txtda = gvSWDesignations.Rows[i].FindControl("txtda") as TextBox;
                    TextBox txthra = gvSWDesignations.Rows[i].FindControl("txthra") as TextBox;
                    // TextBox txthraBill = gvSWDesignations.Rows[i].FindControl("txthraBill") as TextBox;
                    TextBox txtConveyance = gvSWDesignations.Rows[i].FindControl("txtConveyance") as TextBox;
                    TextBox TxtMedicalAllowance = gvSWDesignations.Rows[i].FindControl("TxtMedicalAllowance") as TextBox;
                    TextBox TxtFoodAllowance = gvSWDesignations.Rows[i].FindControl("TxtFoodAllowance") as TextBox;
                    TextBox TxtsiteAllowance = gvSWDesignations.Rows[i].FindControl("TxtsiteAllowance") as TextBox;
                    TextBox TxtSplAllowance = gvSWDesignations.Rows[i].FindControl("TxtSplAllowance") as TextBox;
                    DropDownList ddlSplAllwDays = gvSWDesignations.Rows[i].FindControl("ddlSplAllwDays") as DropDownList;
                    TextBox txtoa = gvSWDesignations.Rows[i].FindControl("txtoa") as TextBox;
                    TextBox txtaddlhrallw = gvSWDesignations.Rows[i].FindControl("txtaddlhrallw") as TextBox;
                    TextBox txtqtrallw = gvSWDesignations.Rows[i].FindControl("txtqtrallw") as TextBox;
                    TextBox txtwa = gvSWDesignations.Rows[i].FindControl("txtwa") as TextBox;
                    TextBox txtrelallw = gvSWDesignations.Rows[i].FindControl("txtrelallw") as TextBox;
                    TextBox txtGunallw = gvSWDesignations.Rows[i].FindControl("txtGunallw") as TextBox;
                    DropDownList ddlGunAllwType = gvSWDesignations.Rows[i].FindControl("ddlGunAllwType") as DropDownList;
                    TextBox txtFireallw = gvSWDesignations.Rows[i].FindControl("txtFireallw") as TextBox;
                    TextBox txtbonus = gvSWDesignations.Rows[i].FindControl("txtbonus") as TextBox;
                    DropDownList ddlbonustype = gvSWDesignations.Rows[i].FindControl("ddlbonustype") as DropDownList;
                    TextBox txtgratuty = gvSWDesignations.Rows[i].FindControl("txtgratuty") as TextBox;
                    DropDownList ddlgratuitytype = gvSWDesignations.Rows[i].FindControl("ddlgratuitytype") as DropDownList;
                    TextBox txtleaveamount = gvSWDesignations.Rows[i].FindControl("txtleaveamount") as TextBox;
                    DropDownList ddllatype = gvSWDesignations.Rows[i].FindControl("ddllatype") as DropDownList;
                    TextBox TxtOTRate = gvSWDesignations.Rows[i].FindControl("TxtOTRate") as TextBox;
                    DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
                    TextBox TxtESIRate = gvSWDesignations.Rows[i].FindControl("TxtESIRate") as TextBox;
                    DropDownList ddlNoOfESIDaysPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfESIDaysPaysheet") as DropDownList;
                    TextBox txtotesiwages = gvSWDesignations.Rows[i].FindControl("txtotesiwages") as TextBox;
                    DropDownList ddlOTESIWagesDays = gvSWDesignations.Rows[i].FindControl("ddlOTESIWagesDays") as DropDownList;
                    TextBox TxtPFRate = gvSWDesignations.Rows[i].FindControl("TxtPFRate") as TextBox;
                    DropDownList ddlNoOfPFDaysPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfPFDaysPaysheet") as DropDownList;
                    CheckBox chkcdswPF = gvSWDesignations.Rows[i].FindControl("chkswPF") as CheckBox;
                    CheckBox chkcdswESI = gvSWDesignations.Rows[i].FindControl("chkswESI") as CheckBox;
                    TextBox txtcca = gvSWDesignations.Rows[i].FindControl("txtcca") as TextBox;
                    TextBox txtattbonus = gvSWDesignations.Rows[i].FindControl("txtattbonus") as TextBox;
                    DropDownList ddlAttbonustype = gvSWDesignations.Rows[i].FindControl("ddlAttbonustype") as DropDownList;
                    TextBox txtNfhs1 = gvSWDesignations.Rows[i].FindControl("txtNfhs1") as TextBox;
                    TextBox Txtrc = gvSWDesignations.Rows[i].FindControl("Txtrc") as TextBox;
                    TextBox TxtCSWPLDays = gvSWDesignations.Rows[i].FindControl("TxtCSWPLDays") as TextBox;
                    TextBox TxtCSWPLAmount = gvSWDesignations.Rows[i].FindControl("TxtCSWPLAmount") as TextBox;
                    TextBox TxtCSWTLDays = gvSWDesignations.Rows[i].FindControl("TxtCSWTLDays") as TextBox;
                    TextBox TxtCSWTLAmount = gvSWDesignations.Rows[i].FindControl("TxtCSWTLAmount") as TextBox;
                    TextBox TxtCs = gvSWDesignations.Rows[i].FindControl("TxtCs") as TextBox;
                    TextBox TxtScPer = gvSWDesignations.Rows[i].FindControl("TxtScPer") as TextBox;
                    DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfNhsPaysheet") as DropDownList;
                    TextBox TxtNHSRate = gvSWDesignations.Rows[i].FindControl("TxtNHSRate") as TextBox;
                    DropDownList ddlNoOfWosPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfWosPaysheet") as DropDownList;
                    TextBox TxtWORate = gvSWDesignations.Rows[i].FindControl("TxtWORate") as TextBox;
                    TextBox TxtTravelAllowance = gvSWDesignations.Rows[i].FindControl("TxtTravelAllowance") as TextBox;
                    TextBox TxtPerformanceAllowance = gvSWDesignations.Rows[i].FindControl("TxtPerformanceAllowance") as TextBox;
                    TextBox TxtMobileAllowance = gvSWDesignations.Rows[i].FindControl("TxtMobileAllowance") as TextBox;
                    TextBox TxtOThrs = gvSWDesignations.Rows[i].FindControl("TxtOThrs") as TextBox;
                    TextBox TxtADVDed = gvSWDesignations.Rows[i].FindControl("TxtADVDed") as TextBox;
                    TextBox TxtWCDed = gvSWDesignations.Rows[i].FindControl("TxtWCDed") as TextBox;
                    TextBox TxtUniformDed = gvSWDesignations.Rows[i].FindControl("TxtUniformDed") as TextBox;
                    TextBox txtservicewt = gvSWDesignations.Rows[i].FindControl("txtservicewt") as TextBox;
                    TextBox txtHardshipAllw = gvSWDesignations.Rows[i].FindControl("txtHardshipAllw") as TextBox;
                    TextBox txtRankAllw = gvSWDesignations.Rows[i].FindControl("txtRankAllw") as TextBox;
                    TextBox txtuniformcharge = gvSWDesignations.Rows[i].FindControl("txtuniformcharge") as TextBox;

                    TextBox txtPFEmp = gvSWDesignations.Rows[i].FindControl("txtPFEmp") as TextBox;
                    TextBox txtESIEmp = gvSWDesignations.Rows[i].FindControl("txtESIEmp") as TextBox;
                    TextBox txtPFEmpr = gvSWDesignations.Rows[i].FindControl("txtPFEmpr") as TextBox;
                    TextBox txtESIEmpr = gvSWDesignations.Rows[i].FindControl("txtESIEmpr") as TextBox;
                    TextBox txtSCEmp = gvSWDesignations.Rows[i].FindControl("txtSCEmp") as TextBox;
                    TextBox txtCTC = gvSWDesignations.Rows[i].FindControl("txtCTC") as TextBox;
                    TextBox TxtIncentiveRate = gvSWDesignations.Rows[i].FindControl("TxtIncentiveRate") as TextBox;
                    TextBox TxtLCRate = gvSWDesignations.Rows[i].FindControl("TxtLCRate") as TextBox;



                    ////TextBox txtConveyanceBill = gvSWDesignations.Rows[i].FindControl("txtConveyanceBill") as TextBox;
                    //TextBox txtmiscded = gvSWDesignations.Rows[i].FindControl("txtmiscded") as TextBox;
                    //TextBox txtccaBill = gvSWDesignations.Rows[i].FindControl("txtccaBill") as TextBox;
                    ////TextBox txtleaveamountBill = gvSWDesignations.Rows[i].FindControl("txtleaveamountBill") as TextBox;
                    //DropDownList ddlBonusType = gvSWDesignations.Rows[i].FindControl("ddlBonusType") as DropDownList;
                    //TextBox TxtSpecialAllowance = gvSWDesignations.Rows[i].FindControl("TxtSplAllowance") as TextBox;
                    //// TextBox TxtUniformAmt = gvSWDesignations.Rows[i].FindControl("TxtUniformAmt") as TextBox;
                    //DropDownList ddlUniformAllwDays = gvSWDesignations.Rows[i].FindControl("ddlUniformAllwDays") as DropDownList;

                    //#region for variables initialisation
                    #region for FoodAloowance and Medical Allowance
                    if (DdlDesign.SelectedIndex == 0)
                    {
                        Cddldesignationsw = "0";
                    }
                    else
                    {
                        Cddldesignationsw = DdlDesign.SelectedValue;
                    }

                    if (DdlCategory.SelectedIndex == 0)
                    {
                        CddlCategory = "0";
                    }
                    else
                    {
                        CddlCategory = DdlCategory.SelectedValue;
                    }

                    if (ddlNoOfDaysWages.SelectedIndex == 0)
                    {
                        NoOfDaysForWages = "0";
                    }
                    if (ddlNoOfDaysWages.SelectedIndex == 1)
                    {
                        NoOfDaysForWages = "1";
                    }
                    if (ddlNoOfDaysWages.SelectedIndex == 2)
                    {
                        NoOfDaysForWages = "2";
                    }
                    if (ddlNoOfDaysWages.SelectedIndex == 3)
                    {
                        NoOfDaysForWages = "3";
                    }
                    if (ddlNoOfDaysWages.SelectedIndex == 4)
                    {
                        NoOfDaysForWages = "4";
                    }
                    if (ddlNoOfDaysWages.SelectedIndex == 5)
                    {
                        NoOfDaysForWages = "5";
                    }
                    if (ddlNoOfDaysWages.SelectedIndex == 6)
                    {
                        NoOfDaysForWages = "6";
                    }
                    if (ddlNoOfDaysWages.SelectedIndex == 7)
                    {
                        NoOfDaysForWages = "7";
                    }
                    if (ddlNoOfDaysWages.SelectedIndex == 8)
                    {
                        NoOfDaysForWages = "8";
                    }

                    if (ddlNoOfDaysWages.SelectedIndex > 8)
                    {
                        NoOfDaysForWages = ddlNoOfDaysWages.SelectedValue;
                    }


                    if (Txtgross.Text == "")
                    {
                        Cgrosssw = "0";
                    }
                    else
                    {
                        Cgrosssw = Txtgross.Text;
                    }

                    if (TxtNetPay.Text == "")
                    {
                        CTxtNetPaysw = "0";
                    }
                    else
                    {
                        CTxtNetPaysw = TxtNetPay.Text;
                    }

                    if (TxtBasic.Text == "")
                    {
                        Cbasicsw = "0";
                    }
                    else
                    {
                        Cbasicsw = TxtBasic.Text;
                    }

                    if (txtda.Text == "")
                    {
                        Cdasw = "0";
                    }
                    else
                    {
                        Cdasw = txtda.Text;
                    }

                    if (txthra.Text == "")
                    {
                        Chrasw = "0";
                    }
                    else
                    {
                        Chrasw = txthra.Text;
                    }

                    if (txtConveyance.Text == "")
                    {
                        CConveyancesw = "0";
                    }
                    else
                    {
                        CConveyancesw = txtConveyance.Text;
                    }
                    if (TxtFoodAllowance.Text == "")
                    {
                        CtxtTLfoodamt = "0";
                    }
                    else
                    {
                        CtxtTLfoodamt = TxtFoodAllowance.Text;
                    }


                    if (TxtMedicalAllowance.Text == "")
                    {
                        CtxtTLmedicalamt = "0";
                    }
                    else
                    {
                        CtxtTLmedicalamt = TxtMedicalAllowance.Text;
                    }


                    if (TxtFoodAllowance.Text == "")
                    {
                        CtxtTLfoodamt = "0";
                    }
                    else
                    {
                        CtxtTLfoodamt = TxtFoodAllowance.Text;
                    }

                    if (TxtsiteAllowance.Text == "")
                    {
                        siteAllw = "0";
                    }
                    else
                    {
                        siteAllw = TxtsiteAllowance.Text;
                    }

                    if (TxtSplAllowance.Text == "")
                    {
                        CtxtTLSpecialamt = "0";
                    }
                    else
                    {
                        CtxtTLSpecialamt = TxtSplAllowance.Text;
                    }

                    if (ddlSplAllwDays.SelectedIndex == 0)
                    {
                        NoOfSplDays = "0";
                    }
                    if (ddlSplAllwDays.SelectedIndex == 1)
                    {
                        NoOfSplDays = "1";
                    }
                    if (ddlSplAllwDays.SelectedIndex == 2)
                    {
                        NoOfSplDays = "2";
                    }
                    if (ddlSplAllwDays.SelectedIndex == 3)
                    {
                        NoOfSplDays = "3";
                    }
                    if (ddlSplAllwDays.SelectedIndex == 4)
                    {
                        NoOfSplDays = "4";
                    }
                    if (ddlSplAllwDays.SelectedIndex > 4)
                    {
                        NoOfSplDays = ddlSplAllwDays.SelectedValue;
                    }
                    if (txtoa.Text == "")
                    {
                        Cawsw = "0";
                    }
                    else
                    {
                        Cawsw = txtoa.Text;
                    }
                    if (txtaddlhrallw.Text == "")
                    {
                        ADDL4HR = "0";
                    }
                    else
                    {
                        ADDL4HR = txtaddlhrallw.Text;
                    }
                    if (txtqtrallw.Text == "")
                    {
                        QTRALLOW = "0";
                    }
                    else
                    {
                        QTRALLOW = txtqtrallw.Text;
                    }
                    if (txtwa.Text == "")
                    {
                        Cwashallowancesw = "0";
                    }
                    else
                    {
                        Cwashallowancesw = txtwa.Text;
                    }
                    if (txtrelallw.Text == "")
                    {
                        RELALLOW = "0";
                    }
                    else
                    {
                        RELALLOW = txtrelallw.Text;
                    }
                    if (txtGunallw.Text == "")
                    {
                        GunAllw = "0";
                    }
                    else
                    {
                        GunAllw = txtGunallw.Text;
                    }

                    if (ddlGunAllwType.SelectedIndex == 0)
                    {
                        GunAllwType = "0";
                    }
                    if (ddlGunAllwType.SelectedIndex == 1)
                    {
                        GunAllwType = "1";
                    }
                    if (ddlGunAllwType.SelectedIndex > 1)
                    {
                        GunAllwType = ddlGunAllwType.SelectedValue;
                    }

                    if (txtFireallw.Text == "")
                    {
                        FireAllw = "0";
                    }
                    else
                    {
                        FireAllw = txtFireallw.Text;
                    }
                    if (txtbonus.Text == "")
                    {
                        Bonussw = "0";
                    }
                    else
                    {
                        Bonussw = txtbonus.Text;
                    }

                    if (ddlbonustype.SelectedIndex == 0)
                    {
                        bonustype = "0";
                    }
                    if (ddlbonustype.SelectedIndex == 1)
                    {
                        bonustype = "1";
                    }
                    if (ddlbonustype.SelectedIndex == 2)
                    {
                        bonustype = "2";
                    }
                    if (ddlbonustype.SelectedIndex == 3)
                    {
                        bonustype = "3";
                    }
                    if (ddlbonustype.SelectedIndex > 3)
                    {
                        bonustype = ddlbonustype.SelectedValue;
                    }
                    if (txtgratuty.Text == "")
                    {
                        Gratutysw = "0";
                    }
                    else
                    {
                        Gratutysw = txtgratuty.Text;
                    }
                    if (ddlgratuitytype.SelectedIndex == 0)
                    {
                        gratuitytype = "0";
                    }

                    if (ddlgratuitytype.SelectedIndex == 1)
                    {
                        gratuitytype = "1";
                    }
                    if (ddlgratuitytype.SelectedIndex == 2)
                    {
                        gratuitytype = "2";
                    }
                    if (ddlgratuitytype.SelectedIndex == 3)
                    {
                        gratuitytype = "3";
                    }
                    if (ddlgratuitytype.SelectedIndex > 3)
                    {
                        gratuitytype = ddlgratuitytype.SelectedValue;
                    }
                    if (txtleaveamount.Text == "")
                    {
                        LeaveAmountsw = "0";
                    }
                    else
                    {
                        LeaveAmountsw = txtleaveamount.Text;
                    }
                    if (ddllatype.SelectedIndex == 0)
                    {
                        latype = "0";
                    }
                    if (ddllatype.SelectedIndex == 1)
                    {
                        latype = "1";
                    }
                    if (ddllatype.SelectedIndex == 2)
                    {
                        latype = "2";
                    }
                    if (ddllatype.SelectedIndex == 3)
                    {
                        latype = "3";
                    }
                    if (ddllatype.SelectedIndex > 3)
                    {
                        latype = ddllatype.SelectedValue;
                    }

                    if (TxtOTRate.Text == "")
                    {
                        OTRATE = "0";
                    }
                    else
                    {
                        OTRATE = TxtOTRate.Text;
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 0)
                    {
                        NoOfOts = "0";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 1)
                    {
                        NoOfOts = "1";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 2)
                    {
                        NoOfOts = "2";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 3)
                    {
                        NoOfOts = "3";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 4)
                    {
                        NoOfOts = "4";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 5)
                    {
                        NoOfOts = "5";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 6)
                    {
                        NoOfOts = "6";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 7)
                    {
                        NoOfOts = "7";
                    }
                    if (ddlNoOfOtsPaysheet.SelectedIndex == 8)
                    {
                        NoOfOts = "8";
                    }

                    if (ddlNoOfOtsPaysheet.SelectedIndex > 8)
                    {
                        NoOfOts = ddlNoOfOtsPaysheet.SelectedValue;
                    }
                    if (TxtESIRate.Text == "")
                    {
                        ESIPayRate = "0";
                    }
                    else
                    {
                        ESIPayRate = TxtESIRate.Text;
                    }
                    if (ddlNoOfESIDaysPaysheet.SelectedIndex == 0)
                    {
                        CddlNoOfESIDaysPaysheet = "0";
                    }
                    if (ddlNoOfESIDaysPaysheet.SelectedIndex == 1)
                    {
                        CddlNoOfESIDaysPaysheet = "1";
                    }
                    if (ddlNoOfESIDaysPaysheet.SelectedIndex == 2)
                    {
                        CddlNoOfESIDaysPaysheet = "2";
                    }
                    if (ddlNoOfESIDaysPaysheet.SelectedIndex == 3)
                    {
                        CddlNoOfESIDaysPaysheet = "3";
                    }
                    if (ddlNoOfESIDaysPaysheet.SelectedIndex == 4)
                    {
                        CddlNoOfESIDaysPaysheet = "4";
                    }
                    if (ddlNoOfESIDaysPaysheet.SelectedIndex > 4)
                    {
                        CddlNoOfESIDaysPaysheet = ddlNoOfESIDaysPaysheet.SelectedValue;
                    }
                    if (txtotesiwages.Text == "")
                    {
                        otesiwages = "0";
                    }
                    else
                    {
                        otesiwages = txtotesiwages.Text;
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 0)
                    {
                        OTESIWagesDays = "0";
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 1)
                    {
                        OTESIWagesDays = "1";
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 2)
                    {
                        OTESIWagesDays = "2";
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 3)
                    {
                        OTESIWagesDays = "3";
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 4)
                    {
                        OTESIWagesDays = "4";
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 5)
                    {
                        OTESIWagesDays = "5";
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 6)
                    {
                        OTESIWagesDays = "6";
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 7)
                    {
                        OTESIWagesDays = "7";
                    }
                    if (ddlOTESIWagesDays.SelectedIndex == 8)
                    {
                        OTESIWagesDays = "8";
                    }


                    if (ddlOTESIWagesDays.SelectedIndex > 8)
                    {
                        OTESIWagesDays = ddlOTESIWagesDays.SelectedValue;
                    }
                    if (TxtPFRate.Text == "")
                    {
                        PFPayRate = "0";
                    }
                    else
                    {
                        PFPayRate = TxtPFRate.Text;
                    }

                    if (ddlNoOfPFDaysPaysheet.SelectedIndex == 0)
                    {
                        CddlNoOfPFDaysPaysheet = "0";
                    }
                    if (ddlNoOfPFDaysPaysheet.SelectedIndex == 1)
                    {
                        CddlNoOfPFDaysPaysheet = "1";
                    }
                    if (ddlNoOfPFDaysPaysheet.SelectedIndex == 2)
                    {
                        CddlNoOfPFDaysPaysheet = "2";
                    }
                    if (ddlNoOfPFDaysPaysheet.SelectedIndex == 3)
                    {
                        CddlNoOfPFDaysPaysheet = "3";
                    }
                    if (ddlNoOfPFDaysPaysheet.SelectedIndex == 4)
                    {
                        CddlNoOfPFDaysPaysheet = "4";
                    }

                    if (ddlNoOfPFDaysPaysheet.SelectedIndex > 4)
                    {
                        CddlNoOfPFDaysPaysheet = ddlNoOfPFDaysPaysheet.SelectedValue;
                    }
                    if (chkcdswPF.Checked == true)
                    {
                        CdswPF = "1";
                    }

                    if (chkcdswESI.Checked == true)
                    {
                        CdswESI = "1";
                    }
                    if (txtcca.Text == "")
                    {
                        Ccasw = "0";
                    }
                    else
                    {
                        Ccasw = txtcca.Text;
                    }
                    if (txtattbonus.Text == "")
                    {
                        AttBonussw = "0";
                    }
                    else
                    {
                        AttBonussw = txtattbonus.Text;
                    }
                    if (ddlAttbonustype.SelectedIndex == 0)
                    {
                        Attbonustype = "0";
                    }
                    if (ddlAttbonustype.SelectedIndex == 1)
                    {
                        Attbonustype = "1";
                    }
                    if (ddlAttbonustype.SelectedIndex == 2)
                    {
                        Attbonustype = "2";
                    }
                    if (ddlAttbonustype.SelectedIndex == 3)
                    {
                        Attbonustype = "3";
                    }
                    if (ddlAttbonustype.SelectedIndex > 3)
                    {
                        Attbonustype = ddlAttbonustype.SelectedValue;
                    }
                    if (txtNfhs1.Text == "")
                    {
                        Nfhssw = "0";
                    }
                    else
                    {
                        Nfhssw = txtNfhs1.Text;
                    }

                    if (Txtrc.Text == "")
                    {
                        RC = "0";
                    }
                    else
                    {
                        RC = Txtrc.Text;
                    }
                    if (TxtCSWPLDays.Text == "")
                    {
                        CSWPLDays = "0";
                    }
                    else
                    {
                        CSWPLDays = TxtCSWPLDays.Text;
                    }


                    if (TxtCSWPLAmount.Text == "")
                    {
                        CSWPLAmount = "0";
                    }
                    else
                    {
                        CSWPLAmount = TxtCSWPLAmount.Text;
                    }


                    if (TxtCSWTLDays.Text == "")
                    {
                        CSWTLDays = "0";
                    }
                    else
                    {
                        CSWTLDays = TxtCSWTLDays.Text;
                    }


                    if (TxtCSWTLAmount.Text == "")
                    {
                        CSWTLAmount = "0";
                    }
                    else
                    {
                        CSWTLAmount = TxtCSWTLAmount.Text;
                    }
                    if (TxtCs.Text == "")
                    {
                        CS = "0";
                    }
                    else
                    {
                        CS = TxtCs.Text;
                    }

                    if (TxtScPer.Text == "")
                    {
                        CSpersw = "0";
                    }
                    else
                    {
                        CSpersw = TxtScPer.Text;
                    }
                    if (ddlNoOfNhsPaysheet.SelectedIndex == 0)
                    {
                        NoOfNhs = "0";
                    }
                    if (ddlNoOfNhsPaysheet.SelectedIndex == 1)
                    {
                        NoOfNhs = "1";
                    }
                    if (ddlNoOfNhsPaysheet.SelectedIndex == 2)
                    {
                        NoOfNhs = "2";
                    }
                    if (ddlNoOfNhsPaysheet.SelectedIndex == 3)
                    {
                        NoOfNhs = "3";
                    }
                    if (ddlNoOfNhsPaysheet.SelectedIndex == 4)
                    {
                        NoOfNhs = "4";
                    }

                    if (ddlNoOfNhsPaysheet.SelectedIndex > 4)
                    {
                        NoOfNhs = ddlNoOfNhsPaysheet.SelectedValue;
                    }
                    if (TxtNHSRate.Text == "")
                    {
                        CtxtTLnhsamt = "0";
                    }
                    else
                    {
                        CtxtTLnhsamt = TxtNHSRate.Text;
                    }
                    if (ddlNoOfWosPaysheet.SelectedIndex == 0)
                    {
                        NoOfWos = "0";
                    }
                    if (ddlNoOfWosPaysheet.SelectedIndex == 1)
                    {
                        NoOfWos = "1";
                    }
                    if (ddlNoOfWosPaysheet.SelectedIndex == 2)
                    {
                        NoOfWos = "2";
                    }
                    if (ddlNoOfWosPaysheet.SelectedIndex == 3)
                    {
                        NoOfWos = "3";
                    }
                    if (ddlNoOfWosPaysheet.SelectedIndex == 4)
                    {
                        NoOfWos = "4";
                    }

                    if (ddlNoOfWosPaysheet.SelectedIndex > 4)
                    {
                        NoOfWos = ddlNoOfWosPaysheet.SelectedValue;
                    }
                    if (TxtWORate.Text == "")
                    {
                        CtxtTLwoamt = "0";
                    }
                    else
                    {
                        CtxtTLwoamt = TxtWORate.Text;
                    }
                    if (TxtTravelAllowance.Text == "")
                    {
                        CTxtTravelAllowance = "0";
                    }
                    else
                    {
                        CTxtTravelAllowance = TxtTravelAllowance.Text;
                    }


                    if (TxtPerformanceAllowance.Text == "")
                    {
                        CTxtPerformanceAllowance = "0";
                    }
                    else
                    {
                        CTxtPerformanceAllowance = TxtPerformanceAllowance.Text;
                    }

                    if (TxtMobileAllowance.Text == "")
                    {
                        CTxtMobileAllowance = "0";
                    }
                    else
                    {
                        CTxtMobileAllowance = TxtMobileAllowance.Text;
                    }


                    if (TxtOThrs.Text == "")
                    {
                        OTHRS = "0";
                    }
                    else
                    {
                        OTHRS = TxtOThrs.Text;
                    }

                    if (TxtADVDed.Text == "")
                    {
                        CtxtTLadvded = "0";
                    }
                    else
                    {
                        CtxtTLadvded = TxtADVDed.Text;
                    }

                    if (TxtWCDed.Text == "")
                    {
                        CtxtTLwcded = "0";
                    }
                    else
                    {
                        CtxtTLwcded = TxtWCDed.Text;
                    }


                    if (TxtUniformDed.Text == "")
                    {
                        CtxtTLunidedsw = "0";
                    }
                    else
                    {
                        CtxtTLunidedsw = TxtUniformDed.Text;
                    }
                    if (txtservicewt.Text == "")
                    {
                        ServiceWeightage = "0";
                    }
                    else
                    {
                        ServiceWeightage = txtservicewt.Text;
                    }

                    if (txtHardshipAllw.Text == "")
                    {
                        HardshipAllw = "0";
                    }
                    else
                    {
                        HardshipAllw = txtHardshipAllw.Text;
                    }

                    if (txtRankAllw.Text == "")
                    {
                        RankAllw = "0";
                    }
                    else
                    {
                        RankAllw = txtRankAllw.Text;
                    }
                    if (txtuniformcharge.Text == "")
                    {
                        uniformcharge = "0";
                    }
                    else
                    {
                        uniformcharge = txtuniformcharge.Text;
                    }

                    if (txtPFEmp.Text == "")
                    {
                        PFEmp = "0";
                    }
                    else
                    {
                        PFEmp = txtPFEmp.Text;
                    }

                    if (txtESIEmp.Text == "")
                    {
                        ESIEmp = "0";
                    }
                    else
                    {
                        ESIEmp = txtESIEmp.Text;
                    }

                    if (txtPFEmpr.Text == "")
                    {
                        PFEmpr = "0";
                    }
                    else
                    {
                        PFEmpr = txtPFEmpr.Text;
                    }

                    if (txtESIEmpr.Text == "")
                    {
                        ESIEmpr = "0";
                    }
                    else
                    {
                        ESIEmpr = txtESIEmpr.Text;
                    }

                    if (txtSCEmp.Text == "")
                    {
                        SCEmp = "0";
                    }
                    else
                    {
                        SCEmp = txtSCEmp.Text;
                    }

                    if (txtCTC.Text == "")
                    {
                        CTC = "0";
                    }
                    else
                    {
                        CTC = txtCTC.Text;
                    }


                    if (txtCTC.Text == "")
                    {
                        CTC = "0";
                    }
                    else
                    {
                        CTC = txtCTC.Text;
                    }
                    if (TxtIncentiveRate.Text == "")
                    {
                        IncentiveRate = "0";
                    }
                    else
                    {
                        IncentiveRate = TxtIncentiveRate.Text;
                    }

                    if (TxtLCRate.Text == "")
                    {
                        LCRate = "0";
                    }
                    else
                    {
                        LCRate = TxtLCRate.Text;
                    }

                    //New component Shift start
                    if (ddlshift.SelectedIndex >= 0)
                    {
                        Shift = ddlshift.SelectedValue;
                    }
                    //New component Shift End

                    #region
                    ////BonusType = ddlBonusType.SelectedValue;
                    ////LWType = ddlLWType.SelectedValue;
                    ////  NoOfDaysForWages = ddln;
                    //#endregion for FoodAloowance and Medical Allowance
                    //#region for ADVDed and WCDed

                    //if (TxtMobileAllowance.Text == "")
                    //{
                    //    CtxtTLadvded = "0";
                    //}
                    //else
                    //{
                    //    CtxtTLadvded = TxtMobileAllowance.Text;
                    //}
                    //if (txtmiscded.Text == "")
                    //{
                    //    MiscDed = "0";
                    //}
                    //else
                    //{
                    //    MiscDed = txtmiscded.Text;
                    //}

                    ////ESIDesgnPayOn = ddlESIPayOn.SelectedValue;
                    ////PFDesgnPayOn = ddlPFPayOn.SelectedValue;
                    #endregion

                    #endregion for variables initialisation

                    string SPName = "Insertcontractspecialdetails";
                    Hashtable HtContracts = new Hashtable();

                    if (DdlDesign.SelectedIndex > 0)
                    {
                        HtContracts.Add("@Sno", lblCSlno.Text);
                        HtContracts.Add("@clientid", Clientid);
                        HtContracts.Add("@ContractId", Contractid);
                        HtContracts.Add("@Designations", Cddldesignationsw);
                        HtContracts.Add("@Shift", Shift);
                        HtContracts.Add("@MinWagesCategory", CddlCategory);
                        HtContracts.Add("@NoOfDays", NoOfDaysForWages);
                        HtContracts.Add("@Gross", Cgrosssw);
                        HtContracts.Add("@NetPay", CTxtNetPaysw);
                        HtContracts.Add("@Basic", Cbasicsw);
                        HtContracts.Add("@DA", Cdasw);
                        HtContracts.Add("@HRA", Chrasw);
                        HtContracts.Add("@Conveyance", CConveyancesw);
                        HtContracts.Add("@MedicalAllowance", CtxtTLmedicalamt);
                        HtContracts.Add("@FoodAllowance", CtxtTLfoodamt);
                        HtContracts.Add("@SITEALLOW", siteAllw);
                        HtContracts.Add("@SplAllowance", CtxtTLSpecialamt);
                        HtContracts.Add("@NoOfSplDays", NoOfSplDays);
                        HtContracts.Add("@OtherAllowance", Cawsw);
                        HtContracts.Add("@ADDL4HR", ADDL4HR);
                        HtContracts.Add("@QTRALLOW", QTRALLOW);
                        HtContracts.Add("@WashAllownce", Cwashallowancesw);
                        HtContracts.Add("@RELALLOW", RELALLOW);
                        HtContracts.Add("@Gunallw ", GunAllw);
                        HtContracts.Add("@GunAllwType", GunAllwType);
                        HtContracts.Add("@Fireallw", FireAllw);
                        HtContracts.Add("@Bonus", Bonussw);
                        HtContracts.Add("@BonusType", bonustype);
                        HtContracts.Add("@gratuity", Gratutysw);
                        HtContracts.Add("@gratuityType", gratuitytype);
                        HtContracts.Add("@LeaveAmount", LeaveAmountsw);
                        HtContracts.Add("@LAType", latype);
                        HtContracts.Add("@OTRATE", OTRATE);
                        HtContracts.Add("@Nots", NoOfOts);
                        HtContracts.Add("@ESIRate", ESIPayRate);
                        HtContracts.Add("@ESIDays", CddlNoOfESIDaysPaysheet);
                        HtContracts.Add("@OTESICWAGES", otesiwages);
                        HtContracts.Add("@OTESIWagesDays", OTESIWagesDays);
                        HtContracts.Add("@PFRate", PFPayRate);
                        HtContracts.Add("@PFDays", CddlNoOfPFDaysPaysheet);
                        HtContracts.Add("@ChkPF", CdswPF);
                        HtContracts.Add("@ChkESI", CdswESI);
                        HtContracts.Add("@CCA", Ccasw);
                        HtContracts.Add("@attbonus", AttBonussw);
                        HtContracts.Add("@AttBonusType", Attbonustype);
                        HtContracts.Add("@NFHs", Nfhssw);
                        HtContracts.Add("@RC", RC);
                        HtContracts.Add("@CSWPLDays", CSWPLDays);
                        HtContracts.Add("@CSWPLAmount", CSWPLAmount);
                        HtContracts.Add("@CSWTLDays", CSWTLDays);
                        HtContracts.Add("@CSWTLAmount", CSWTLAmount);
                        HtContracts.Add("@CS", CS);
                        HtContracts.Add("@servicechargeper", CSpersw);
                        HtContracts.Add("@NNhs", NoOfNhs);
                        HtContracts.Add("@NHSRate", CtxtTLnhsamt);
                        HtContracts.Add("@NWos", NoOfWos);
                        HtContracts.Add("@WORate", CtxtTLwoamt);
                        HtContracts.Add("@TravelAllw", CTxtTravelAllowance);
                        HtContracts.Add("@PerformanceAllw", CTxtPerformanceAllowance);
                        HtContracts.Add("@MobileAllw", CTxtMobileAllowance);
                        HtContracts.Add("@OTHrs", OTHRS);
                        HtContracts.Add("@AdvDed", CtxtTLadvded);
                        HtContracts.Add("@WCDed", CtxtTLwcded);
                        HtContracts.Add("@Uniformded", CtxtTLunidedsw);
                        HtContracts.Add("@Serviceweightage", ServiceWeightage);
                        HtContracts.Add("@HardshipAllw", HardshipAllw);
                        HtContracts.Add("@RankAllw", RankAllw);
                        HtContracts.Add("@uniformcharge", uniformcharge);
                        HtContracts.Add("@PFEmp", PFEmp);
                        HtContracts.Add("@ESIEmp", ESIEmp);
                        HtContracts.Add("@PFEmpr", PFEmpr);
                        HtContracts.Add("@ESIEmpr", ESIEmpr);
                        HtContracts.Add("@SCEmp", SCEmp);
                        HtContracts.Add("@CTC", CTC);
                        HtContracts.Add("@IncentiveRate", IncentiveRate);
                        HtContracts.Add("@LCRate", LCRate);


                        HtContracts.Add("@testrecord", specialdesigncount);

                        int IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                        if (IRecordStatus != 0)
                        {
                            specialdesigncount++;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnadddesgn_Click1(object sender, EventArgs e)
        {
            int columns = 1;

            if (txtnoofrows.Text == "")
            {
                columns = 1;
            }
            else
            {
                columns = int.Parse(txtnoofrows.Text);
            }

            for (int i = 0; i < columns; i++)
            {
                AddNewRowToGridForInvoice();
            }

        }

        private void AddNewRowToGridForInvoice()
        {

            int rowIndex = 0;

            if (ViewState["InvoiceTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["InvoiceTable"];
                DataRow drCurrentRow = null;
                //DataRow drCurrentRow1 = null;


                if (dtCurrentTable.Rows.Count > 0)
                {

                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        TextBox lblCSlno = gvdesignation.Rows[rowIndex].FindControl("lblCSlno") as TextBox;
                        DropDownList DdlDesign = gvdesignation.Rows[rowIndex].Cells[0].FindControl("DdlDesign") as DropDownList;
                        DropDownList ddlType = gvdesignation.Rows[rowIndex].Cells[1].FindControl("ddlType") as DropDownList;
                        // DropDownList ddlGroupBy = gvdesignation.Rows[i].Cells[2].FindControl("ddlGroupBy") as DropDownList;
                        TextBox txtdutyhrs = gvdesignation.Rows[rowIndex].Cells[2].FindControl("txtdutyhrs") as TextBox;
                        TextBox txtquantity = gvdesignation.Rows[rowIndex].Cells[3].FindControl("txtquantity") as TextBox;
                        TextBox txtPayRate = gvdesignation.Rows[rowIndex].Cells[4].FindControl("txtPayRate") as TextBox;
                        DropDownList ddldutytype = gvdesignation.Rows[rowIndex].Cells[5].FindControl("ddldutytype") as DropDownList;
                        DropDownList ddlNoOfDaysBilling = gvdesignation.Rows[rowIndex].Cells[6].FindControl("ddlNoOfDaysBilling") as DropDownList;
                        TextBox txtsummary = gvdesignation.Rows[rowIndex].Cells[7].FindControl("txtsummary") as TextBox;
                        //TextBox txtId = gvdesignation.Rows[i].Cells[9].FindControl("txtId") as TextBox;
                        DropDownList ddlHSNNumber = gvdesignation.Rows[rowIndex].Cells[8].FindControl("ddlHSNNumber") as DropDownList;
                        CheckBox chkcdCGST = gvdesignation.Rows[rowIndex].Cells[9].FindControl("chkcdCGST") as CheckBox;
                        CheckBox chkcdSGST = gvdesignation.Rows[rowIndex].Cells[10].FindControl("chkcdSGST") as CheckBox;
                        CheckBox chkcdIGST = gvdesignation.Rows[rowIndex].Cells[11].FindControl("chkcdIGST") as CheckBox;

                        CheckBox chkcdPF = gvdesignation.Rows[rowIndex].Cells[12].FindControl("chkcdPF") as CheckBox;
                        CheckBox chkcdESI = gvdesignation.Rows[rowIndex].Cells[13].FindControl("chkcdESI") as CheckBox;
                        CheckBox chkcdSC = gvdesignation.Rows[rowIndex].Cells[14].FindControl("chkcdSC") as CheckBox;
                        TextBox TxtScPer = gvdesignation.Rows[rowIndex].Cells[15].FindControl("TxtScPer") as TextBox;
                        TextBox TxtCs = gvdesignation.Rows[rowIndex].Cells[16].FindControl("TxtCs") as TextBox;
                        CheckBox chkcdSCOnPFESI = gvdesignation.Rows[rowIndex].Cells[17].FindControl("chkcdSCOnPFESI") as CheckBox;
                        TextBox TxtOTRate = gvdesignation.Rows[rowIndex].Cells[18].FindControl("TxtOTRate") as TextBox;
                        DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[rowIndex].Cells[19].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
                        CheckBox ChkRCApplicable = gvdesignation.Rows[rowIndex].Cells[20].FindControl("ChkRCApplicable") as CheckBox;
                        TextBox TxtBasic = gvdesignation.Rows[rowIndex].Cells[21].FindControl("TxtBasic") as TextBox;
                        TextBox txtda = gvdesignation.Rows[rowIndex].Cells[22].FindControl("txtda") as TextBox;
                        TextBox txthra = gvdesignation.Rows[rowIndex].Cells[23].FindControl("txthra") as TextBox;
                        TextBox txtConveyance = gvdesignation.Rows[rowIndex].Cells[24].FindControl("txtConveyance") as TextBox;
                        TextBox txtcca = gvdesignation.Rows[rowIndex].Cells[25].FindControl("txtcca") as TextBox;
                        TextBox txtleaveamount = gvdesignation.Rows[rowIndex].Cells[26].FindControl("txtleaveamount") as TextBox;
                        TextBox txtgratuty = gvdesignation.Rows[rowIndex].Cells[27].FindControl("txtgratuty") as TextBox;
                        TextBox txtbonus = gvdesignation.Rows[rowIndex].Cells[28].FindControl("txtbonus") as TextBox;
                        TextBox txtattbonus = gvdesignation.Rows[rowIndex].Cells[29].FindControl("txtattbonus") as TextBox;
                        TextBox txtwa = gvdesignation.Rows[rowIndex].Cells[30].FindControl("txtwa") as TextBox;
                        TextBox txtoa = gvdesignation.Rows[rowIndex].Cells[31].FindControl("txtoa") as TextBox;
                        TextBox txtNfhs1 = gvdesignation.Rows[rowIndex].Cells[32].FindControl("txtNfhs") as TextBox;
                        TextBox Txtrc = gvdesignation.Rows[rowIndex].Cells[33].FindControl("Txtrc") as TextBox;


                        drCurrentRow = dtCurrentTable.NewRow();


                        dtCurrentTable.Rows[i - 1]["Sno"] = lblCSlno.Text;
                        dtCurrentTable.Rows[i - 1]["Designations"] = DdlDesign.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["type"] = ddlType.SelectedValue;
                        //dtCurrentTable.Rows[i - 1]["Groupby"] = ddlGroupBy.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["DutyHrs"] = txtdutyhrs.Text.Trim() == "" ? 0 : Convert.ToSingle(txtdutyhrs.Text);
                        dtCurrentTable.Rows[i - 1]["Quantity"] = txtquantity.Text.Trim() == "" ? 0 : Convert.ToSingle(txtquantity.Text);
                        dtCurrentTable.Rows[i - 1]["Amount"] = txtPayRate.Text.Trim() == "" ? 0 : Convert.ToSingle(txtPayRate.Text);
                        dtCurrentTable.Rows[i - 1]["PayType"] = ddldutytype.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["Noofdays"] = ddlNoOfDaysBilling.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["Summary"] = txtsummary.Text.Trim() == "" ? "" : txtsummary.Text.ToString();
                        //dtCurrentTable.Rows[i - 1]["Id"] = txtId.Text.Trim() == "" ? 0 : Convert.ToSingle(txtId.Text);
                        dtCurrentTable.Rows[i - 1]["HSNNumber"] = ddlHSNNumber.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["cdCGST"] = chkcdCGST.Checked.ToString();
                        dtCurrentTable.Rows[i - 1]["cdSGST"] = chkcdSGST.Checked.ToString();
                        dtCurrentTable.Rows[i - 1]["cdIGST"] = chkcdIGST.Checked.ToString();

                        dtCurrentTable.Rows[i - 1]["cdPF"] = chkcdPF.Checked.ToString();
                        dtCurrentTable.Rows[i - 1]["cdESI"] = chkcdESI.Checked.ToString();
                        dtCurrentTable.Rows[i - 1]["cdSC"] = chkcdSC.Checked.ToString();

                        dtCurrentTable.Rows[i - 1]["ServicechargePer"] = TxtScPer.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtScPer.Text);
                        dtCurrentTable.Rows[i - 1]["CS"] = TxtCs.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtCs.Text);

                        dtCurrentTable.Rows[i - 1]["cdSCOnPFESI"] = chkcdSCOnPFESI.Checked.ToString();

                        dtCurrentTable.Rows[i - 1]["otrate"] = TxtOTRate.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtOTRate.Text);
                        dtCurrentTable.Rows[i - 1]["Nots"] = ddlNoOfOtsPaysheet.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["rcapplicable"] = ChkRCApplicable.Checked.ToString();
                        dtCurrentTable.Rows[i - 1]["Basic"] = TxtBasic.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtBasic.Text);
                        dtCurrentTable.Rows[i - 1]["DA"] = txtda.Text.Trim() == "" ? 0 : Convert.ToSingle(txtda.Text);
                        dtCurrentTable.Rows[i - 1]["HRA"] = txthra.Text.Trim() == "" ? 0 : Convert.ToSingle(txthra.Text);
                        dtCurrentTable.Rows[i - 1]["Conveyance"] = txtConveyance.Text.Trim() == "" ? 0 : Convert.ToSingle(txtConveyance.Text);
                        dtCurrentTable.Rows[i - 1]["CCA"] = txtcca.Text.Trim() == "" ? 0 : Convert.ToSingle(txtcca.Text);
                        dtCurrentTable.Rows[i - 1]["LeaveAmount"] = txtleaveamount.Text.Trim() == "" ? 0 : Convert.ToSingle(txtleaveamount.Text);
                        dtCurrentTable.Rows[i - 1]["Gratuity"] = txtgratuty.Text.Trim() == "" ? 0 : Convert.ToSingle(txtgratuty.Text);
                        dtCurrentTable.Rows[i - 1]["Bonus"] = txtbonus.Text.Trim() == "" ? 0 : Convert.ToSingle(txtbonus.Text);
                        dtCurrentTable.Rows[i - 1]["AttBonus"] = txtattbonus.Text.Trim() == "" ? 0 : Convert.ToSingle(txtattbonus.Text);
                        dtCurrentTable.Rows[i - 1]["washallownce"] = txtwa.Text.Trim() == "" ? 0 : Convert.ToSingle(txtwa.Text);
                        dtCurrentTable.Rows[i - 1]["OtherAllowance"] = txtoa.Text.Trim() == "" ? 0 : Convert.ToSingle(txtoa.Text);
                        dtCurrentTable.Rows[i - 1]["NFhs"] = txtNfhs1.Text.Trim() == "" ? 0 : Convert.ToSingle(txtNfhs1.Text);
                        dtCurrentTable.Rows[i - 1]["RC"] = Txtrc.Text.Trim() == "" ? 0 : Convert.ToSingle(Txtrc.Text);


                        rowIndex++;

                    }


                    dtCurrentTable.Rows.Add(drCurrentRow);


                    gvdesignation.DataSource = dtCurrentTable;
                    gvdesignation.DataBind();

                    displaydataForInvoice();

                    ViewState["InvoiceTable"] = dtCurrentTable;

                }

            }
            else
            {
                Response.Write("ViewState is null");
            }


            //Set Previous Data on Postbacks
            SetPreviousDataForInvoice();
        }

        private void AddNewRowToGridForSW()
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

                        TextBox lblCSlno = gvSWDesignations.Rows[rowIndex].FindControl("lblCSlno") as TextBox;
                        DropDownList DdlDesign = gvSWDesignations.Rows[rowIndex].Cells[0].FindControl("DdlDesign") as DropDownList;
                        DropDownList ddlshift = gvSWDesignations.Rows[rowIndex].Cells[1].FindControl("ddlshift") as DropDownList;
                        DropDownList DdlCategory = gvSWDesignations.Rows[rowIndex].Cells[2].FindControl("DdlCategory") as DropDownList;
                        DropDownList ddlNoOfDaysWages = gvSWDesignations.Rows[rowIndex].Cells[3].FindControl("ddlNoOfDaysWages") as DropDownList;
                        TextBox Txtgross = gvSWDesignations.Rows[rowIndex].Cells[4].FindControl("Txtgross") as TextBox;
                        TextBox TxtNetPay = gvSWDesignations.Rows[rowIndex].Cells[5].FindControl("TxtNetPay") as TextBox;
                        TextBox TxtBasic = gvSWDesignations.Rows[rowIndex].Cells[6].FindControl("TxtBasic") as TextBox;
                        TextBox txtda = gvSWDesignations.Rows[rowIndex].Cells[7].FindControl("txtda") as TextBox;
                        TextBox txthra = gvSWDesignations.Rows[rowIndex].Cells[8].FindControl("txthra") as TextBox;
                        // TextBox txthraBill = gvSWDesignations.Rows[i].FindControl("txthraBill") as TextBox;
                        TextBox txtConveyance = gvSWDesignations.Rows[rowIndex].Cells[9].FindControl("txtConveyance") as TextBox;
                        TextBox TxtMedicalAllowance = gvSWDesignations.Rows[rowIndex].Cells[10].FindControl("TxtMedicalAllowance") as TextBox;
                        TextBox TxtFoodAllowance = gvSWDesignations.Rows[rowIndex].Cells[11].FindControl("TxtFoodAllowance") as TextBox;
                        TextBox TxtsiteAllowance = gvSWDesignations.Rows[rowIndex].Cells[12].FindControl("TxtsiteAllowance") as TextBox;
                        TextBox TxtSplAllowance = gvSWDesignations.Rows[rowIndex].Cells[13].FindControl("TxtSplAllowance") as TextBox;
                        DropDownList ddlSplAllwDays = gvSWDesignations.Rows[rowIndex].Cells[14].FindControl("ddlSplAllwDays") as DropDownList;
                        TextBox txtoa = gvSWDesignations.Rows[rowIndex].Cells[15].FindControl("txtoa") as TextBox;
                        TextBox txtaddlhrallw = gvSWDesignations.Rows[rowIndex].Cells[16].FindControl("txtaddlhrallw") as TextBox;
                        TextBox txtqtrallw = gvSWDesignations.Rows[rowIndex].Cells[17].FindControl("txtqtrallw") as TextBox;
                        TextBox txtwa = gvSWDesignations.Rows[rowIndex].Cells[18].FindControl("txtwa") as TextBox;
                        TextBox txtrelallw = gvSWDesignations.Rows[rowIndex].Cells[19].FindControl("txtrelallw") as TextBox;
                        TextBox txtGunallw = gvSWDesignations.Rows[rowIndex].Cells[20].FindControl("txtGunallw") as TextBox;
                        DropDownList ddlGunAllwType = gvSWDesignations.Rows[rowIndex].Cells[21].FindControl("ddlGunAllwType") as DropDownList;
                        TextBox txtFireallw = gvSWDesignations.Rows[rowIndex].Cells[22].FindControl("txtFireallw") as TextBox;
                        TextBox txtbonus = gvSWDesignations.Rows[rowIndex].Cells[23].FindControl("txtbonus") as TextBox;
                        DropDownList ddlbonustype = gvSWDesignations.Rows[rowIndex].Cells[24].FindControl("ddlbonustype") as DropDownList;
                        TextBox txtgratuty = gvSWDesignations.Rows[rowIndex].Cells[25].FindControl("txtgratuty") as TextBox;
                        DropDownList ddlgratuitytype = gvSWDesignations.Rows[rowIndex].Cells[26].FindControl("ddlgratuitytype") as DropDownList;
                        TextBox txtleaveamount = gvSWDesignations.Rows[rowIndex].Cells[27].FindControl("txtleaveamount") as TextBox;
                        DropDownList ddllatype = gvSWDesignations.Rows[rowIndex].Cells[28].FindControl("ddllatype") as DropDownList;
                        TextBox TxtOTRate = gvSWDesignations.Rows[rowIndex].Cells[29].FindControl("TxtOTRate") as TextBox;
                        DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[rowIndex].Cells[30].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
                        TextBox TxtESIRate = gvSWDesignations.Rows[rowIndex].Cells[31].FindControl("TxtESIRate") as TextBox;
                        DropDownList ddlNoOfESIDaysPaysheet = gvSWDesignations.Rows[rowIndex].Cells[32].FindControl("ddlNoOfESIDaysPaysheet") as DropDownList;
                        TextBox txtotesiwages = gvSWDesignations.Rows[rowIndex].Cells[33].FindControl("txtotesiwages") as TextBox;
                        DropDownList ddlOTESIWagesDays = gvSWDesignations.Rows[rowIndex].Cells[34].FindControl("ddlOTESIWagesDays") as DropDownList;
                        TextBox TxtPFRate = gvSWDesignations.Rows[rowIndex].Cells[35].FindControl("TxtPFRate") as TextBox;
                        DropDownList ddlNoOfPFDaysPaysheet = gvSWDesignations.Rows[rowIndex].Cells[36].FindControl("ddlNoOfPFDaysPaysheet") as DropDownList;
                        CheckBox chkcdswPF = gvSWDesignations.Rows[rowIndex].Cells[37].FindControl("chkswPF") as CheckBox;
                        CheckBox chkcdswESI = gvSWDesignations.Rows[rowIndex].Cells[38].FindControl("chkswESI") as CheckBox;
                        TextBox txtcca = gvSWDesignations.Rows[rowIndex].Cells[39].FindControl("txtcca") as TextBox;
                        TextBox txtattbonus = gvSWDesignations.Rows[rowIndex].Cells[40].FindControl("txtattbonus") as TextBox;
                        DropDownList ddlAttbonustype = gvSWDesignations.Rows[rowIndex].Cells[41].FindControl("ddlAttbonustype") as DropDownList;
                        TextBox txtNfhs1 = gvSWDesignations.Rows[rowIndex].Cells[42].FindControl("txtNfhs1") as TextBox;
                        TextBox Txtrc = gvSWDesignations.Rows[rowIndex].Cells[43].FindControl("Txtrc") as TextBox;
                        TextBox TxtCSWPLDays = gvSWDesignations.Rows[rowIndex].Cells[44].FindControl("TxtCSWPLDays") as TextBox;
                        TextBox TxtCSWPLAmount = gvSWDesignations.Rows[rowIndex].Cells[45].FindControl("TxtCSWPLAmount") as TextBox;
                        TextBox TxtCSWTLDays = gvSWDesignations.Rows[rowIndex].Cells[46].FindControl("TxtCSWTLDays") as TextBox;
                        TextBox TxtCSWTLAmount = gvSWDesignations.Rows[rowIndex].Cells[47].FindControl("TxtCSWTLAmount") as TextBox;
                        TextBox TxtCs = gvSWDesignations.Rows[rowIndex].Cells[48].FindControl("TxtCs") as TextBox;
                        TextBox TxtScPer = gvSWDesignations.Rows[rowIndex].Cells[49].FindControl("TxtScPer") as TextBox;
                        DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[rowIndex].Cells[50].FindControl("ddlNoOfNhsPaysheet") as DropDownList;
                        TextBox TxtNHSRate = gvSWDesignations.Rows[rowIndex].Cells[51].FindControl("TxtNHSRate") as TextBox;
                        DropDownList ddlNoOfWosPaysheet = gvSWDesignations.Rows[rowIndex].Cells[52].FindControl("ddlNoOfWosPaysheet") as DropDownList;
                        TextBox TxtWORate = gvSWDesignations.Rows[rowIndex].Cells[53].FindControl("TxtWORate") as TextBox;
                        TextBox TxtTravelAllowance = gvSWDesignations.Rows[rowIndex].Cells[54].FindControl("TxtTravelAllowance") as TextBox;
                        TextBox TxtPerformanceAllowance = gvSWDesignations.Rows[rowIndex].Cells[55].FindControl("TxtPerformanceAllowance") as TextBox;
                        TextBox TxtMobileAllowance = gvSWDesignations.Rows[rowIndex].Cells[56].FindControl("TxtMobileAllowance") as TextBox;
                        TextBox TxtOThrs = gvSWDesignations.Rows[rowIndex].Cells[57].FindControl("TxtOThrs") as TextBox;
                        TextBox TxtADVDed = gvSWDesignations.Rows[rowIndex].Cells[58].FindControl("TxtADVDed") as TextBox;
                        TextBox TxtWCDed = gvSWDesignations.Rows[rowIndex].Cells[59].FindControl("TxtWCDed") as TextBox;
                        TextBox TxtUniformDed = gvSWDesignations.Rows[rowIndex].Cells[60].FindControl("TxtUniformDed") as TextBox;
                        TextBox txtservicewt = gvSWDesignations.Rows[rowIndex].Cells[61].FindControl("txtservicewt") as TextBox;
                        TextBox txtHardshipAllw = gvSWDesignations.Rows[rowIndex].Cells[62].FindControl("txtHardshipAllw") as TextBox;
                        TextBox txtRankAllw = gvSWDesignations.Rows[rowIndex].Cells[63].FindControl("txtRankAllw") as TextBox;
                        TextBox txtuniformcharge = gvSWDesignations.Rows[rowIndex].Cells[64].FindControl("txtuniformcharge") as TextBox;

                        TextBox txtPFEmp = gvSWDesignations.Rows[rowIndex].Cells[65].FindControl("txtPFEmp") as TextBox;
                        TextBox txtESIEmp = gvSWDesignations.Rows[rowIndex].Cells[66].FindControl("txtESIEmp") as TextBox;
                        TextBox txtPFEmpr = gvSWDesignations.Rows[rowIndex].Cells[67].FindControl("txtPFEmpr") as TextBox;
                        TextBox txtESIEmpr = gvSWDesignations.Rows[rowIndex].Cells[68].FindControl("txtESIEmpr") as TextBox;
                        TextBox txtSCEmp = gvSWDesignations.Rows[rowIndex].Cells[69].FindControl("txtSCEmp") as TextBox;
                        TextBox txtCTC = gvSWDesignations.Rows[rowIndex].Cells[70].FindControl("txtCTC") as TextBox;
                        TextBox TxtIncentiveRate = gvSWDesignations.Rows[rowIndex].Cells[71].FindControl("TxtIncentiveRate") as TextBox;
                        TextBox TxtLCRate = gvSWDesignations.Rows[rowIndex].Cells[72].FindControl("TxtLCRate") as TextBox;


                        drCurrentRow = dtCurrentTable.NewRow();


                        dtCurrentTable.Rows[i - 1]["SNo"] = lblCSlno.Text;
                        dtCurrentTable.Rows[i - 1]["Designations"] = DdlDesign.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["MinWagesCategory"] = DdlCategory.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["Shift"] = ddlshift.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["Noofdays"] = ddlNoOfDaysWages.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["Gross"] = Txtgross.Text.Trim() == "" ? 0 : Convert.ToSingle(Txtgross.Text);
                        dtCurrentTable.Rows[i - 1]["Netpay"] = TxtNetPay.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtNetPay.Text);
                        dtCurrentTable.Rows[i - 1]["Basic"] = TxtBasic.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtBasic.Text);
                        dtCurrentTable.Rows[i - 1]["DA"] = txtda.Text.Trim() == "" ? 0 : Convert.ToSingle(txtda.Text);
                        dtCurrentTable.Rows[i - 1]["HRA"] = txthra.Text.Trim() == "" ? 0 : Convert.ToSingle(txthra.Text);
                        dtCurrentTable.Rows[i - 1]["Conveyance"] = txtConveyance.Text.Trim() == "" ? 0 : Convert.ToSingle(txtConveyance.Text);
                        dtCurrentTable.Rows[i - 1]["MedicalAllowance"] = TxtMedicalAllowance.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtMedicalAllowance.Text);
                        dtCurrentTable.Rows[i - 1]["FoodAllowance"] = TxtFoodAllowance.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtFoodAllowance.Text);
                        dtCurrentTable.Rows[i - 1]["SITEALLOW"] = TxtsiteAllowance.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtsiteAllowance.Text);
                        dtCurrentTable.Rows[i - 1]["SplAllowance"] = TxtSplAllowance.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtSplAllowance.Text);
                        dtCurrentTable.Rows[i - 1]["NoOfSplDays"] = ddlSplAllwDays.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["OtherAllowance"] = txtoa.Text.Trim() == "" ? 0 : Convert.ToSingle(txtoa.Text);
                        dtCurrentTable.Rows[i - 1]["ADDL4HR"] = txtaddlhrallw.Text.Trim() == "" ? 0 : Convert.ToSingle(txtaddlhrallw.Text);
                        dtCurrentTable.Rows[i - 1]["QTRALLOW"] = txtqtrallw.Text.Trim() == "" ? 0 : Convert.ToSingle(txtqtrallw.Text);
                        dtCurrentTable.Rows[i - 1]["washallownce"] = txtwa.Text.Trim() == "" ? 0 : Convert.ToSingle(txtwa.Text);
                        dtCurrentTable.Rows[i - 1]["RELALLOW"] = txtrelallw.Text.Trim() == "" ? 0 : Convert.ToSingle(txtrelallw.Text);
                        dtCurrentTable.Rows[i - 1]["Gunallw"] = txtGunallw.Text.Trim() == "" ? 0 : Convert.ToSingle(txtGunallw.Text);
                        dtCurrentTable.Rows[i - 1]["GunAllwType"] = ddlGunAllwType.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["Fireallw"] = txtFireallw.Text.Trim() == "" ? 0 : Convert.ToSingle(txtFireallw.Text);
                        dtCurrentTable.Rows[i - 1]["Bonus"] = txtbonus.Text.Trim() == "" ? 0 : Convert.ToSingle(txtbonus.Text);
                        dtCurrentTable.Rows[i - 1]["BonusType"] = ddlbonustype.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["Gratuity"] = txtgratuty.Text.Trim() == "" ? 0 : Convert.ToSingle(txtgratuty.Text);
                        dtCurrentTable.Rows[i - 1]["GratuityType"] = ddlgratuitytype.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["leaveamount"] = txtleaveamount.Text.Trim() == "" ? 0 : Convert.ToSingle(txtleaveamount.Text);
                        dtCurrentTable.Rows[i - 1]["latype"] = ddllatype.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["otrate"] = TxtOTRate.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtOTRate.Text);
                        dtCurrentTable.Rows[i - 1]["Nots"] = ddlNoOfOtsPaysheet.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["ESIRate"] = TxtESIRate.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtESIRate.Text);
                        dtCurrentTable.Rows[i - 1]["ESIDays"] = ddlNoOfESIDaysPaysheet.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["OTESICWAGES"] = txtotesiwages.Text.Trim() == "" ? 0 : Convert.ToSingle(txtotesiwages.Text);
                        dtCurrentTable.Rows[i - 1]["OTESIWagesDays"] = ddlOTESIWagesDays.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["PFRate"] = TxtPFRate.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtPFRate.Text);
                        dtCurrentTable.Rows[i - 1]["PFDays"] = ddlNoOfPFDaysPaysheet.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["ChkPF"] = chkcdswPF.Checked.ToString();
                        dtCurrentTable.Rows[i - 1]["Chkesi"] = chkcdswESI.Checked.ToString();
                        dtCurrentTable.Rows[i - 1]["CCA"] = txtcca.Text.Trim() == "" ? 0 : Convert.ToSingle(txtcca.Text);
                        dtCurrentTable.Rows[i - 1]["AttBonus"] = txtattbonus.Text.Trim() == "" ? 0 : Convert.ToSingle(txtattbonus.Text);
                        dtCurrentTable.Rows[i - 1]["Attbonustype"] = ddlAttbonustype.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["NFhs"] = txtNfhs1.Text.Trim() == "" ? 0 : Convert.ToSingle(txtNfhs1.Text);
                        dtCurrentTable.Rows[i - 1]["RC"] = Txtrc.Text.Trim() == "" ? 0 : Convert.ToSingle(Txtrc.Text);
                        dtCurrentTable.Rows[i - 1]["PLDays"] = TxtCSWPLDays.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtCSWPLDays.Text);
                        dtCurrentTable.Rows[i - 1]["PLAmount"] = TxtCSWPLAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtCSWPLAmount.Text);
                        dtCurrentTable.Rows[i - 1]["TLDays"] = TxtCSWTLDays.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtCSWTLDays.Text);
                        dtCurrentTable.Rows[i - 1]["TLAmount"] = TxtCSWTLAmount.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtCSWTLAmount.Text);
                        dtCurrentTable.Rows[i - 1]["CS"] = TxtCs.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtCs.Text);
                        dtCurrentTable.Rows[i - 1]["ServicechargePer"] = TxtScPer.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtScPer.Text);
                        dtCurrentTable.Rows[i - 1]["NNhs"] = ddlNoOfNhsPaysheet.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["NhsRate"] = TxtNHSRate.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtNHSRate.Text);
                        dtCurrentTable.Rows[i - 1]["NWos"] = ddlNoOfWosPaysheet.SelectedIndex;
                        dtCurrentTable.Rows[i - 1]["TravelAllw"] = TxtTravelAllowance.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtTravelAllowance.Text);
                        dtCurrentTable.Rows[i - 1]["PerformanceAllw"] = TxtPerformanceAllowance.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtPerformanceAllowance.Text);
                        dtCurrentTable.Rows[i - 1]["MobileAllw"] = TxtMobileAllowance.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtMobileAllowance.Text);
                        dtCurrentTable.Rows[i - 1]["OTHrs"] = TxtOThrs.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtOThrs.Text);
                        dtCurrentTable.Rows[i - 1]["AdvDed"] = TxtADVDed.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtADVDed.Text);
                        dtCurrentTable.Rows[i - 1]["WCDed"] = TxtWCDed.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtWCDed.Text);
                        dtCurrentTable.Rows[i - 1]["UniformDed"] = TxtUniformDed.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtUniformDed.Text);
                        dtCurrentTable.Rows[i - 1]["Serviceweightage"] = txtservicewt.Text.Trim() == "" ? 0 : Convert.ToSingle(txtservicewt.Text);
                        dtCurrentTable.Rows[i - 1]["HardshipAllw"] = txtHardshipAllw.Text.Trim() == "" ? 0 : Convert.ToSingle(txtHardshipAllw.Text);
                        dtCurrentTable.Rows[i - 1]["RankAllw"] = txtRankAllw.Text.Trim() == "" ? 0 : Convert.ToSingle(txtRankAllw.Text);
                        dtCurrentTable.Rows[i - 1]["uniformcharge"] = txtuniformcharge.Text.Trim() == "" ? 0 : Convert.ToSingle(txtuniformcharge.Text);

                        dtCurrentTable.Rows[i - 1]["PFEmp"] = txtPFEmp.Text.Trim() == "" ? 0 : Convert.ToSingle(txtPFEmp.Text);
                        dtCurrentTable.Rows[i - 1]["ESIEmp"] = txtESIEmp.Text.Trim() == "" ? 0 : Convert.ToSingle(txtESIEmp.Text);
                        dtCurrentTable.Rows[i - 1]["PFEmpr"] = txtPFEmpr.Text.Trim() == "" ? 0 : Convert.ToSingle(txtPFEmpr.Text);
                        dtCurrentTable.Rows[i - 1]["ESIEmpr"] = txtESIEmpr.Text.Trim() == "" ? 0 : Convert.ToSingle(txtESIEmpr.Text);
                        dtCurrentTable.Rows[i - 1]["SCEmp"] = txtSCEmp.Text.Trim() == "" ? 0 : Convert.ToSingle(txtSCEmp.Text);
                        dtCurrentTable.Rows[i - 1]["CTC"] = txtCTC.Text.Trim() == "" ? 0 : Convert.ToSingle(txtCTC.Text);
                        dtCurrentTable.Rows[i - 1]["IncentiveRate"] = TxtIncentiveRate.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtIncentiveRate.Text);
                        dtCurrentTable.Rows[i - 1]["LCRate"] = TxtLCRate.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtLCRate.Text);



                        //dtCurrentTable.Rows[i - 1]["Incentives"] = TxtIncentives.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtIncentives.Text);
                        //dtCurrentTable.Rows[i - 1]["WoRate"] = TxtWORate.Text.Trim() == "" ? 0 : Convert.ToSingle(TxtWORate.Text);
                        //dtCurrentTable.Rows[i - 1]["MiscDed"] = txtmiscded.Text.Trim() == "" ? 0 : Convert.ToSingle(txtmiscded.Text);

                        rowIndex++;

                    }


                    dtCurrentTable.Rows.Add(drCurrentRow);


                    gvSWDesignations.DataSource = dtCurrentTable;
                    gvSWDesignations.DataBind();

                    displaydataForSW();

                    ViewState["CurrentTable"] = dtCurrentTable;

                }

            }
            else
            {
                Response.Write("ViewState is null");
            }


            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousDataForInvoice()
        {
            int rowIndex = 0;
            if (ViewState["InvoiceTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["InvoiceTable"];
                int Rowcount = 0;
                Rowcount = dt.Rows.Count - 1;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < gvdesignation.Rows.Count; i++)
                    {

                        TextBox lblCSlno = gvdesignation.Rows[rowIndex].FindControl("lblCSlno") as TextBox;
                        DropDownList DdlDesign = gvdesignation.Rows[rowIndex].Cells[0].FindControl("DdlDesign") as DropDownList;
                        DropDownList ddlType = gvdesignation.Rows[rowIndex].Cells[1].FindControl("ddlType") as DropDownList;
                        // DropDownList ddlGroupBy = gvdesignation.Rows[i].Cells[2].FindControl("ddlGroupBy") as DropDownList;
                        TextBox txtdutyhrs = gvdesignation.Rows[rowIndex].Cells[2].FindControl("txtdutyhrs") as TextBox;
                        TextBox txtquantity = gvdesignation.Rows[rowIndex].Cells[3].FindControl("txtquantity") as TextBox;
                        TextBox txtPayRate = gvdesignation.Rows[rowIndex].Cells[4].FindControl("txtPayRate") as TextBox;
                        DropDownList ddldutytype = gvdesignation.Rows[rowIndex].Cells[5].FindControl("ddldutytype") as DropDownList;
                        DropDownList ddlNoOfDaysBilling = gvdesignation.Rows[rowIndex].Cells[6].FindControl("ddlNoOfDaysBilling") as DropDownList;
                        TextBox txtsummary = gvdesignation.Rows[rowIndex].Cells[7].FindControl("txtsummary") as TextBox;
                        //TextBox txtId = gvdesignation.Rows[i].Cells[9].FindControl("txtId") as TextBox;
                        DropDownList ddlHSNNumber = gvdesignation.Rows[rowIndex].Cells[8].FindControl("ddlHSNNumber") as DropDownList;
                        CheckBox chkcdCGST = gvdesignation.Rows[rowIndex].Cells[9].FindControl("chkcdCGST") as CheckBox;
                        CheckBox chkcdSGST = gvdesignation.Rows[rowIndex].Cells[10].FindControl("chkcdSGST") as CheckBox;
                        CheckBox chkcdIGST = gvdesignation.Rows[rowIndex].Cells[11].FindControl("chkcdIGST") as CheckBox;

                        CheckBox chkcdPF = gvdesignation.Rows[rowIndex].Cells[12].FindControl("chkcdPF") as CheckBox;
                        CheckBox chkcdESI = gvdesignation.Rows[rowIndex].Cells[13].FindControl("chkcdESI") as CheckBox;
                        CheckBox chkcdSC = gvdesignation.Rows[rowIndex].Cells[14].FindControl("chkcdSC") as CheckBox;
                        TextBox TxtScPer = gvdesignation.Rows[rowIndex].Cells[15].FindControl("TxtScPer") as TextBox;
                        TextBox TxtCs = gvdesignation.Rows[rowIndex].Cells[16].FindControl("TxtCs") as TextBox;
                        CheckBox chkcdSCOnPFESI = gvdesignation.Rows[rowIndex].Cells[17].FindControl("chkcdSCOnPFESI") as CheckBox;
                        TextBox TxtOTRate = gvdesignation.Rows[rowIndex].Cells[18].FindControl("TxtOTRate") as TextBox;
                        DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[rowIndex].Cells[19].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
                        CheckBox ChkRCApplicable = gvdesignation.Rows[rowIndex].Cells[20].FindControl("ChkRCApplicable") as CheckBox;
                        TextBox TxtBasic = gvdesignation.Rows[rowIndex].Cells[21].FindControl("TxtBasic") as TextBox;
                        TextBox txtda = gvdesignation.Rows[rowIndex].Cells[22].FindControl("txtda") as TextBox;
                        TextBox txthra = gvdesignation.Rows[rowIndex].Cells[23].FindControl("txthra") as TextBox;
                        TextBox txtConveyance = gvdesignation.Rows[rowIndex].Cells[24].FindControl("txtConveyance") as TextBox;
                        TextBox txtcca = gvdesignation.Rows[rowIndex].Cells[25].FindControl("txtcca") as TextBox;
                        TextBox txtleaveamount = gvdesignation.Rows[rowIndex].Cells[26].FindControl("txtleaveamount") as TextBox;
                        TextBox txtgratuty = gvdesignation.Rows[rowIndex].Cells[27].FindControl("txtgratuty") as TextBox;
                        TextBox txtbonus = gvdesignation.Rows[rowIndex].Cells[28].FindControl("txtbonus") as TextBox;
                        TextBox txtattbonus = gvdesignation.Rows[rowIndex].Cells[29].FindControl("txtattbonus") as TextBox;
                        TextBox txtwa = gvdesignation.Rows[rowIndex].Cells[30].FindControl("txtwa") as TextBox;
                        TextBox txtoa = gvdesignation.Rows[rowIndex].Cells[31].FindControl("txtoa") as TextBox;
                        TextBox txtNfhs1 = gvdesignation.Rows[rowIndex].Cells[32].FindControl("txtNfhs") as TextBox;
                        TextBox Txtrc = gvdesignation.Rows[rowIndex].Cells[33].FindControl("Txtrc") as TextBox;


                        DropDownList CDesgnsw = gvdesignation.Rows[i].FindControl("DdlDesign") as DropDownList;
                        if (String.IsNullOrEmpty(dt.Rows[i]["Designations"].ToString()) != false || dt.Rows[i]["Designations"].ToString() == "--Select--")
                        {
                            CDesgnsw.SelectedIndex = 0;
                        }
                        else
                        {
                            if (int.Parse(dt.Rows[i]["Designations"].ToString()) != 0)
                            {
                                CDesgnsw.SelectedValue = dt.Rows[i]["Designations"].ToString();
                            }
                            else
                            {
                                CDesgnsw.SelectedIndex = 0;

                            }
                        }

                        DropDownList CddlType = gvdesignation.Rows[i].FindControl("ddlType") as DropDownList;
                        if (String.IsNullOrEmpty(dt.Rows[i]["type"].ToString()) != false || dt.Rows[i]["type"].ToString() == "--Select--")
                        {
                            CddlType.SelectedIndex = 0;
                        }
                        else
                        {
                            if (int.Parse(dt.Rows[i]["type"].ToString()) != 0)
                            {
                                CddlType.SelectedValue = dt.Rows[i]["type"].ToString();
                            }
                            else
                            {
                                CddlType.SelectedIndex = 0;

                            }
                        }

                        //DropDownList CddlGroupBy = gvdesignation.Rows[i].FindControl("ddlGroupBy") as DropDownList;
                        //if (String.IsNullOrEmpty(dt.Rows[i]["Groupby"].ToString()) != false || dt.Rows[i]["Groupby"].ToString() == "--Select--")
                        //{
                        //    CddlGroupBy.SelectedIndex = 0;
                        //}
                        //else
                        //{
                        //    if (int.Parse(dt.Rows[i]["Groupby"].ToString()) != 0)
                        //    {
                        //        CddlGroupBy.SelectedValue = dt.Rows[i]["Groupby"].ToString();
                        //    }
                        //    else
                        //    {
                        //        CddlGroupBy.SelectedIndex = 0;

                        //    }
                        //}


                        txtdutyhrs.Text = dt.Rows[i]["DutyHrs"].ToString();
                        txtquantity.Text = dt.Rows[i]["Quantity"].ToString();
                        txtPayRate.Text = dt.Rows[i]["Amount"].ToString();

                        DropDownList Cddldutytype = gvdesignation.Rows[i].FindControl("ddldutytype") as DropDownList;

                        if (Cddldutytype != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["PayType"].ToString()) != false || dt.Rows[i]["PayType"].ToString() == "--Select--")
                            {
                                Cddldutytype.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["PayType"].ToString().Trim().Length > 0)
                                {

                                    int dutytype = int.Parse(dt.Rows[i]["PayType"].ToString());
                                    if (dutytype == 0)
                                        Cddldutytype.SelectedIndex = 0;
                                    else
                                        if (dutytype == 1)
                                        Cddldutytype.SelectedIndex = 1;
                                    else
                                            if (dutytype == 2)
                                        Cddldutytype.SelectedIndex = 2;
                                    else
                                                if (dutytype == 3)
                                        Cddldutytype.SelectedIndex = 3;
                                    else
                                                    if (dutytype == 4)
                                        Cddldutytype.SelectedIndex = 4;
                                    else
                                    if (dutytype == 5)
                                        Cddldutytype.SelectedIndex = 5;
                                    else
                                    if (dutytype == 6)
                                        Cddldutytype.SelectedIndex = 6;
                                    else
                                    if (dutytype == 7)
                                        Cddldutytype.SelectedIndex = 7;
                                    else
                                        Cddldutytype.SelectedValue = dt.Rows[i]["PayType"].ToString();

                                }
                            }
                        }

                        //No Of Days For  wages
                        DropDownList CNoOfDaysFoWages = gvdesignation.Rows[i].FindControl("ddlNoOfDaysBilling") as DropDownList;

                        if (CNoOfDaysFoWages != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["NoofDays"].ToString()) != false || dt.Rows[i]["NoofDays"].ToString() == "--Select--")
                            {
                                CNoOfDaysFoWages.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["NoofDays"].ToString().Trim().Length > 0)
                                {

                                    int noofdays = int.Parse(dt.Rows[i]["NoofDays"].ToString());
                                    if (noofdays == 0)
                                        CNoOfDaysFoWages.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        CNoOfDaysFoWages.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        CNoOfDaysFoWages.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        CNoOfDaysFoWages.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        CNoOfDaysFoWages.SelectedIndex = 4;
                                    else
                                        CNoOfDaysFoWages.SelectedValue = dt.Rows[i]["NoofDays"].ToString();

                                }
                            }
                        }

                        txtsummary.Text = dt.Rows[i]["Summary"].ToString();
                        //txtId.Text = dt.Rows[i]["Id"].ToString();

                        DropDownList CddlHSNNumber = gvdesignation.Rows[i].FindControl("ddlHSNNumber") as DropDownList;
                        if (String.IsNullOrEmpty(dt.Rows[i]["HSNNumber"].ToString()) != false || dt.Rows[i]["HSNNumber"].ToString() == "--Select--")
                        {
                            CddlHSNNumber.SelectedIndex = 0;
                        }
                        else
                        {
                            if (int.Parse(dt.Rows[i]["HSNNumber"].ToString()) != 0)
                            {
                                CddlHSNNumber.SelectedValue = dt.Rows[i]["HSNNumber"].ToString();
                            }
                            else
                            {
                                CddlHSNNumber.SelectedIndex = 0;

                            }
                        }
                        if (CDesgnsw.SelectedValue != "--Select--")
                        {
                            chkcdCGST.Checked = Convert.ToBoolean(dt.Rows[i]["cdCGST"].ToString());
                            chkcdSGST.Checked = Convert.ToBoolean(dt.Rows[i]["cdSGST"].ToString());
                            chkcdIGST.Checked = Convert.ToBoolean(dt.Rows[i]["cdIGST"].ToString());

                            chkcdPF.Checked = Convert.ToBoolean(dt.Rows[i]["cdPF"].ToString());
                            chkcdESI.Checked = Convert.ToBoolean(dt.Rows[i]["cdESI"].ToString());
                            chkcdSC.Checked = Convert.ToBoolean(dt.Rows[i]["cdSC"].ToString());

                            chkcdSCOnPFESI.Checked = Convert.ToBoolean(dt.Rows[i]["cdSCOnPFESI"].ToString());

                            ChkRCApplicable.Checked = Convert.ToBoolean(dt.Rows[i]["rcapplicable"].ToString());
                        }
                        else
                        {

                        }

                        TxtScPer.Text = dt.Rows[i]["ServicechargePer"].ToString();
                        TxtCs.Text = dt.Rows[i]["cs"].ToString();

                        TxtOTRate.Text = dt.Rows[i]["OTRate"].ToString();
                        DropDownList CddlNoOfOtsPaysheet = gvdesignation.Rows[i].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                        if (CddlNoOfOtsPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["Nots"].ToString()) != false || dt.Rows[i]["Nots"].ToString() == "--Select--")
                            {
                                CddlNoOfOtsPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["Nots"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dt.Rows[i]["Nots"].ToString());
                                    if (noofdays == 0)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 4;
                                    else

                                        ddlNoOfOtsPaysheet.SelectedValue = dt.Rows[i]["Nots"].ToString();
                                }
                            }

                        }

                        TxtBasic.Text = dt.Rows[i]["Basic"].ToString();
                        txtda.Text = dt.Rows[i]["da"].ToString();
                        txthra.Text = dt.Rows[i]["hra"].ToString();
                        txtConveyance.Text = dt.Rows[i]["Conveyance"].ToString();
                        txtcca.Text = dt.Rows[i]["cca"].ToString();
                        txtleaveamount.Text = dt.Rows[i]["LeaveAmount"].ToString();
                        txtgratuty.Text = dt.Rows[i]["Gratuity"].ToString();
                        txtbonus.Text = dt.Rows[i]["Bonus"].ToString();
                        txtattbonus.Text = dt.Rows[i]["attbonus"].ToString();
                        txtwa.Text = dt.Rows[i]["washallownce"].ToString();
                        txtoa.Text = dt.Rows[i]["OtherAllowance"].ToString();
                        txtNfhs1.Text = dt.Rows[i]["NFHs"].ToString();
                        Txtrc.Text = dt.Rows[i]["rc"].ToString();

                        rowIndex++;
                    }
                }
            }
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < gvSWDesignations.Rows.Count; i++)
                    {
                        TextBox lblCSlno = gvSWDesignations.Rows[rowIndex].FindControl("lblCSlno") as TextBox;
                        DropDownList DdlDesign = gvSWDesignations.Rows[rowIndex].Cells[0].FindControl("DdlDesign") as DropDownList;
                        DropDownList ddlshift = gvSWDesignations.Rows[rowIndex].Cells[1].FindControl("ddlshift") as DropDownList;
                        DropDownList DdlCategory = gvSWDesignations.Rows[rowIndex].Cells[2].FindControl("DdlCategory") as DropDownList;
                        DropDownList ddlNoOfDaysWages = gvSWDesignations.Rows[rowIndex].Cells[3].FindControl("ddlNoOfDaysWages") as DropDownList;
                        TextBox Txtgross = gvSWDesignations.Rows[rowIndex].Cells[4].FindControl("Txtgross") as TextBox;
                        TextBox TxtNetPay = gvSWDesignations.Rows[rowIndex].Cells[5].FindControl("TxtNetPay") as TextBox;
                        TextBox TxtBasic = gvSWDesignations.Rows[rowIndex].Cells[6].FindControl("TxtBasic") as TextBox;
                        TextBox txtda = gvSWDesignations.Rows[rowIndex].Cells[7].FindControl("txtda") as TextBox;
                        TextBox txthra = gvSWDesignations.Rows[rowIndex].Cells[8].FindControl("txthra") as TextBox;
                        // TextBox txthraBill = gvSWDesignations.Rows[i].FindControl("txthraBill") as TextBox;
                        TextBox txtConveyance = gvSWDesignations.Rows[rowIndex].Cells[9].FindControl("txtConveyance") as TextBox;
                        TextBox TxtMedicalAllowance = gvSWDesignations.Rows[rowIndex].Cells[10].FindControl("TxtMedicalAllowance") as TextBox;
                        TextBox TxtFoodAllowance = gvSWDesignations.Rows[rowIndex].Cells[11].FindControl("TxtFoodAllowance") as TextBox;
                        TextBox TxtsiteAllowance = gvSWDesignations.Rows[rowIndex].Cells[12].FindControl("TxtsiteAllowance") as TextBox;
                        TextBox TxtSplAllowance = gvSWDesignations.Rows[rowIndex].Cells[13].FindControl("TxtSplAllowance") as TextBox;
                        DropDownList ddlSplAllwDays = gvSWDesignations.Rows[rowIndex].Cells[14].FindControl("ddlSplAllwDays") as DropDownList;
                        TextBox txtoa = gvSWDesignations.Rows[rowIndex].Cells[15].FindControl("txtoa") as TextBox;
                        TextBox txtaddlhrallw = gvSWDesignations.Rows[rowIndex].Cells[16].FindControl("txtaddlhrallw") as TextBox;
                        TextBox txtqtrallw = gvSWDesignations.Rows[rowIndex].Cells[17].FindControl("txtqtrallw") as TextBox;
                        TextBox txtwa = gvSWDesignations.Rows[rowIndex].Cells[18].FindControl("txtwa") as TextBox;
                        TextBox txtrelallw = gvSWDesignations.Rows[rowIndex].Cells[19].FindControl("txtrelallw") as TextBox;
                        TextBox txtGunallw = gvSWDesignations.Rows[rowIndex].Cells[20].FindControl("txtGunallw") as TextBox;
                        DropDownList ddlGunAllwType = gvSWDesignations.Rows[rowIndex].Cells[21].FindControl("ddlGunAllwType") as DropDownList;
                        TextBox txtFireallw = gvSWDesignations.Rows[rowIndex].Cells[22].FindControl("txtFireallw") as TextBox;
                        TextBox txtbonus = gvSWDesignations.Rows[rowIndex].Cells[23].FindControl("txtbonus") as TextBox;
                        DropDownList ddlbonustype = gvSWDesignations.Rows[rowIndex].Cells[24].FindControl("ddlbonustype") as DropDownList;
                        TextBox txtgratuty = gvSWDesignations.Rows[rowIndex].Cells[25].FindControl("txtgratuty") as TextBox;
                        DropDownList ddlgratuitytype = gvSWDesignations.Rows[rowIndex].Cells[26].FindControl("ddlgratuitytype") as DropDownList;
                        TextBox txtleaveamount = gvSWDesignations.Rows[rowIndex].Cells[27].FindControl("txtleaveamount") as TextBox;
                        DropDownList ddllatype = gvSWDesignations.Rows[rowIndex].Cells[28].FindControl("ddllatype") as DropDownList;
                        TextBox TxtOTRate = gvSWDesignations.Rows[rowIndex].Cells[29].FindControl("TxtOTRate") as TextBox;
                        DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[rowIndex].Cells[30].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
                        TextBox TxtESIRate = gvSWDesignations.Rows[rowIndex].Cells[31].FindControl("TxtESIRate") as TextBox;
                        DropDownList ddlNoOfESIDaysPaysheet = gvSWDesignations.Rows[rowIndex].Cells[32].FindControl("ddlNoOfESIDaysPaysheet") as DropDownList;
                        TextBox txtotesiwages = gvSWDesignations.Rows[rowIndex].Cells[33].FindControl("txtotesiwages") as TextBox;
                        DropDownList ddlOTESIWagesDays = gvSWDesignations.Rows[rowIndex].Cells[34].FindControl("ddlOTESIWagesDays") as DropDownList;
                        TextBox TxtPFRate = gvSWDesignations.Rows[rowIndex].Cells[35].FindControl("TxtPFRate") as TextBox;
                        DropDownList ddlNoOfPFDaysPaysheet = gvSWDesignations.Rows[rowIndex].Cells[36].FindControl("ddlNoOfPFDaysPaysheet") as DropDownList;
                        CheckBox chkcdswPF = gvSWDesignations.Rows[i].Cells[37].FindControl("chkswPF") as CheckBox;
                        CheckBox chkcdswESI = gvSWDesignations.Rows[i].Cells[38].FindControl("chkswESI") as CheckBox;
                        TextBox txtcca = gvSWDesignations.Rows[rowIndex].Cells[39].FindControl("txtcca") as TextBox;
                        TextBox txtattbonus = gvSWDesignations.Rows[rowIndex].Cells[40].FindControl("txtattbonus") as TextBox;
                        DropDownList ddlAttbonustype = gvSWDesignations.Rows[rowIndex].Cells[41].FindControl("ddlAttbonustype") as DropDownList;
                        TextBox txtNfhs1 = gvSWDesignations.Rows[rowIndex].Cells[42].FindControl("txtNfhs1") as TextBox;
                        TextBox Txtrc = gvSWDesignations.Rows[rowIndex].Cells[43].FindControl("Txtrc") as TextBox;
                        TextBox TxtCSWPLDays = gvSWDesignations.Rows[rowIndex].Cells[44].FindControl("TxtCSWPLDays") as TextBox;
                        TextBox TxtCSWPLAmount = gvSWDesignations.Rows[rowIndex].Cells[45].FindControl("TxtCSWPLAmount") as TextBox;
                        TextBox TxtCSWTLDays = gvSWDesignations.Rows[rowIndex].Cells[46].FindControl("TxtCSWTLDays") as TextBox;
                        TextBox TxtCSWTLAmount = gvSWDesignations.Rows[rowIndex].Cells[47].FindControl("TxtCSWTLAmount") as TextBox;
                        TextBox TxtCs = gvSWDesignations.Rows[rowIndex].Cells[48].FindControl("TxtCs") as TextBox;
                        TextBox TxtScPer = gvSWDesignations.Rows[rowIndex].Cells[49].FindControl("TxtScPer") as TextBox;
                        DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[rowIndex].Cells[50].FindControl("ddlNoOfNhsPaysheet") as DropDownList;
                        TextBox TxtNHSRate = gvSWDesignations.Rows[rowIndex].Cells[51].FindControl("TxtNHSRate") as TextBox;
                        DropDownList ddlNoOfWosPaysheet = gvSWDesignations.Rows[rowIndex].Cells[52].FindControl("ddlNoOfWosPaysheet") as DropDownList;
                        TextBox TxtWORate = gvSWDesignations.Rows[rowIndex].Cells[53].FindControl("TxtWORate") as TextBox;
                        TextBox TxtTravelAllowance = gvSWDesignations.Rows[rowIndex].Cells[54].FindControl("TxtTravelAllowance") as TextBox;
                        TextBox TxtPerformanceAllowance = gvSWDesignations.Rows[rowIndex].Cells[55].FindControl("TxtPerformanceAllowance") as TextBox;
                        TextBox TxtMobileAllowance = gvSWDesignations.Rows[rowIndex].Cells[56].FindControl("TxtMobileAllowance") as TextBox;
                        TextBox TxtOThrs = gvSWDesignations.Rows[rowIndex].Cells[57].FindControl("TxtOThrs") as TextBox;
                        TextBox TxtADVDed = gvSWDesignations.Rows[rowIndex].Cells[58].FindControl("TxtADVDed") as TextBox;
                        TextBox TxtWCDed = gvSWDesignations.Rows[rowIndex].Cells[59].FindControl("TxtWCDed") as TextBox;
                        TextBox TxtUniformDed = gvSWDesignations.Rows[rowIndex].Cells[60].FindControl("TxtUniformDed") as TextBox;
                        TextBox txtservicewt = gvSWDesignations.Rows[rowIndex].Cells[61].FindControl("txtservicewt") as TextBox;
                        TextBox txtHardshipAllw = gvSWDesignations.Rows[rowIndex].Cells[62].FindControl("txtHardshipAllw") as TextBox;
                        TextBox txtRankAllw = gvSWDesignations.Rows[rowIndex].Cells[63].FindControl("txtRankAllw") as TextBox;
                        TextBox txtuniformcharge = gvSWDesignations.Rows[rowIndex].Cells[64].FindControl("txtuniformcharge") as TextBox;

                        TextBox txtPFEmp = gvSWDesignations.Rows[rowIndex].Cells[65].FindControl("txtPFEmp") as TextBox;
                        TextBox txtESIEmp = gvSWDesignations.Rows[rowIndex].Cells[66].FindControl("txtESIEmp") as TextBox;
                        TextBox txtPFEmpr = gvSWDesignations.Rows[rowIndex].Cells[67].FindControl("txtPFEmpr") as TextBox;
                        TextBox txtESIEmpr = gvSWDesignations.Rows[rowIndex].Cells[68].FindControl("txtESIEmpr") as TextBox;
                        TextBox txtSCEmp = gvSWDesignations.Rows[rowIndex].Cells[69].FindControl("txtSCEmp") as TextBox;
                        TextBox txtCTC = gvSWDesignations.Rows[rowIndex].Cells[70].FindControl("txtCTC") as TextBox;
                        TextBox TxtIncentiveRate = gvSWDesignations.Rows[rowIndex].Cells[71].FindControl("TxtIncentiveRate") as TextBox;
                        TextBox TxtLCRate = gvSWDesignations.Rows[rowIndex].Cells[71].FindControl("TxtLCRate") as TextBox;


                        DropDownList CDesgnsw = gvSWDesignations.Rows[i].FindControl("DdlDesign") as DropDownList;
                        if (String.IsNullOrEmpty(dt.Rows[i]["Designations"].ToString()) != false || dt.Rows[i]["Designations"].ToString() == "--Select--")
                        {
                            CDesgnsw.SelectedIndex = 0;
                        }
                        else
                        {
                            if (int.Parse(dt.Rows[i]["Designations"].ToString()) != 0)
                            {
                                CDesgnsw.SelectedValue = dt.Rows[i]["Designations"].ToString();
                            }
                            else
                            {
                                CDesgnsw.SelectedIndex = 0;

                            }
                        }

                        DropDownList CCategorysw = gvSWDesignations.Rows[i].FindControl("DdlCategory") as DropDownList;
                        if (String.IsNullOrEmpty(dt.Rows[i]["MinWagesCategory"].ToString()) != false || dt.Rows[i]["MinWagesCategory"].ToString() == "--Select--")
                        {
                            CCategorysw.SelectedIndex = 0;
                        }
                        else
                        {
                            if (int.Parse(dt.Rows[i]["MinWagesCategory"].ToString()) != 0)
                            {
                                CCategorysw.SelectedValue = dt.Rows[i]["MinWagesCategory"].ToString();
                            }
                            else
                            {
                                CCategorysw.SelectedIndex = 0;

                            }
                        }

                        //No Of Days For  wages
                        DropDownList CNoOfDaysFoWages = gvSWDesignations.Rows[i].FindControl("ddlNoOfDaysWages") as DropDownList;

                        if (CNoOfDaysFoWages != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["NoofDays"].ToString()) != false || dt.Rows[i]["NoofDays"].ToString() == "--Select--")
                            {
                                CNoOfDaysFoWages.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["NoofDays"].ToString().Trim().Length > 0)
                                {

                                    int noofdays = int.Parse(dt.Rows[i]["NoofDays"].ToString());
                                    if (noofdays == 0)
                                        CNoOfDaysFoWages.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        CNoOfDaysFoWages.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        CNoOfDaysFoWages.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        CNoOfDaysFoWages.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        CNoOfDaysFoWages.SelectedIndex = 4;
                                    else
                                                    if (noofdays == 5)
                                        CNoOfDaysFoWages.SelectedIndex = 5;
                                    else
                                                    if (noofdays == 6)
                                        CNoOfDaysFoWages.SelectedIndex = 6;
                                    else
                                                    if (noofdays == 7)
                                        CNoOfDaysFoWages.SelectedIndex = 7;
                                    else
                                                    if (noofdays == 8)
                                        CNoOfDaysFoWages.SelectedIndex = 8;
                                    else
                                                    if (noofdays == 9)
                                        CNoOfDaysFoWages.SelectedIndex = 9;
                                    else
                                        CNoOfDaysFoWages.SelectedValue = dt.Rows[i]["NoofDays"].ToString();

                                }
                            }
                        }

                        if (ddlshift != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["Shift"].ToString()) != false || dt.Rows[i]["Shift"].ToString() == "--Select--")
                            {
                                ddlshift.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlshift.SelectedValue = dt.Rows[i]["Shift"].ToString();
                            }

                        }

                        Txtgross.Text = dt.Rows[i]["Gross"].ToString();
                        TxtNetPay.Text = dt.Rows[i]["Netpay"].ToString();
                        TxtBasic.Text = dt.Rows[i]["Basic"].ToString();
                        txtda.Text = dt.Rows[i]["da"].ToString();
                        txthra.Text = dt.Rows[i]["hra"].ToString();
                        txtConveyance.Text = dt.Rows[i]["Conveyance"].ToString();
                        TxtMedicalAllowance.Text = dt.Rows[i]["MedicalAllowance"].ToString();
                        TxtFoodAllowance.Text = dt.Rows[i]["FoodAllowance"].ToString();
                        TxtsiteAllowance.Text = dt.Rows[i]["SITEALLOW"].ToString();
                        TxtSplAllowance.Text = dt.Rows[i]["SplAllowance"].ToString();


                        if (ddlSplAllwDays != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["NoOfSplDays"].ToString()) != false || dt.Rows[i]["NoOfSplDays"].ToString() == "--Select--")
                            {
                                ddlSplAllwDays.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["NoOfSplDays"].ToString().Trim().Length > 0)
                                {
                                    int SplAllwDays = int.Parse(dt.Rows[i]["NoOfSplDays"].ToString());
                                    if (SplAllwDays == 0)
                                        ddlSplAllwDays.SelectedIndex = 0;
                                    else
                                        if (SplAllwDays == 1)
                                        ddlSplAllwDays.SelectedIndex = 1;
                                    else
                                            if (SplAllwDays == 2)
                                        ddlSplAllwDays.SelectedIndex = 2;
                                    else
                                                if (SplAllwDays == 3)
                                        ddlSplAllwDays.SelectedIndex = 3;
                                    else
                                                    if (SplAllwDays == 4)
                                        ddlSplAllwDays.SelectedIndex = 4;
                                    else

                                        ddlSplAllwDays.SelectedValue = dt.Rows[i]["NoOfSplDays"].ToString();
                                }
                            }

                        }

                        txtoa.Text = dt.Rows[i]["OtherAllowance"].ToString();
                        txtaddlhrallw.Text = dt.Rows[i]["ADDL4HR"].ToString();
                        txtqtrallw.Text = dt.Rows[i]["QTRALLOW"].ToString();
                        txtrelallw.Text = dt.Rows[i]["RELALLOW"].ToString();
                        txtGunallw.Text = dt.Rows[i]["Gunallw"].ToString();

                        if (ddlGunAllwType != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["GunAllwType"].ToString()) != false || dt.Rows[i]["GunAllwType"].ToString() == "--Select--")
                            {
                                ddlGunAllwType.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["GunAllwType"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dt.Rows[i]["GunAllwType"].ToString());
                                    if (noofdays == 0)
                                        ddlGunAllwType.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlGunAllwType.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlGunAllwType.SelectedIndex = 2;
                                    else

                                        ddlGunAllwType.SelectedValue = dt.Rows[i]["GunAllwType"].ToString();
                                }
                            }

                        }

                        txtFireallw.Text = dt.Rows[i]["Fireallw"].ToString();
                        txtbonus.Text = dt.Rows[i]["Bonus"].ToString();

                        if (ddlbonustype != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["bonustype"].ToString()) != false || dt.Rows[i]["bonustype"].ToString() == "--Select--")
                            {
                                ddlbonustype.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["bonustype"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dt.Rows[i]["bonustype"].ToString());
                                    if (noofdays == 0)
                                        ddlbonustype.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlbonustype.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlbonustype.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlbonustype.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlbonustype.SelectedIndex = 4;
                                    else

                                        ddlbonustype.SelectedValue = dt.Rows[i]["bonustype"].ToString();
                                }
                            }

                        }

                        txtgratuty.Text = dt.Rows[i]["Gratuity"].ToString();

                        if (ddlgratuitytype != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["gratuitytype"].ToString()) != false || dt.Rows[i]["gratuitytype"].ToString() == "--Select--")
                            {
                                ddlgratuitytype.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["gratuitytype"].ToString().Trim().Length > 0)
                                {
                                    int gratuitytype = int.Parse(dt.Rows[i]["gratuitytype"].ToString());
                                    if (gratuitytype == 0)
                                        ddlgratuitytype.SelectedIndex = 0;
                                    else
                                        if (gratuitytype == 1)
                                        ddlgratuitytype.SelectedIndex = 1;
                                    else
                                            if (gratuitytype == 2)
                                        ddlgratuitytype.SelectedIndex = 2;
                                    else
                                            if (gratuitytype == 3)
                                        ddlgratuitytype.SelectedIndex = 3;
                                    else
                                            if (gratuitytype == 4)
                                        ddlgratuitytype.SelectedIndex = 4;
                                    else

                                        ddlgratuitytype.SelectedValue = dt.Rows[i]["gratuitytype"].ToString();
                                }
                            }

                        }

                        txtleaveamount.Text = dt.Rows[i]["LeaveAmount"].ToString();

                        if (ddllatype != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["latype"].ToString()) != false || dt.Rows[i]["latype"].ToString() == "--Select--")
                            {
                                ddllatype.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["latype"].ToString().Trim().Length > 0)
                                {
                                    int gratuitytype = int.Parse(dt.Rows[i]["latype"].ToString());
                                    if (gratuitytype == 0)
                                        ddllatype.SelectedIndex = 0;
                                    else
                                        if (gratuitytype == 1)
                                        ddllatype.SelectedIndex = 1;
                                    else
                                            if (gratuitytype == 2)
                                        ddllatype.SelectedIndex = 2;
                                    else
                                            if (gratuitytype == 3)
                                        ddllatype.SelectedIndex = 3;
                                    else

                                        ddllatype.SelectedValue = dt.Rows[i]["latype"].ToString();
                                }
                            }

                        }

                        TxtOTRate.Text = dt.Rows[i]["OTRate"].ToString();

                        if (ddlNoOfOtsPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["Nots"].ToString()) != false || dt.Rows[i]["Nots"].ToString() == "--Select--")
                            {
                                ddlNoOfOtsPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["Nots"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dt.Rows[i]["Nots"].ToString());
                                    if (noofdays == 0)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 4;
                                    else
                                                    if (noofdays == 5)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 5;
                                    else
                                                    if (noofdays == 6)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 6;
                                    else
                                                    if (noofdays == 7)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 7;
                                    else
                                                    if (noofdays == 8)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 8;
                                    else
                                                    if (noofdays == 9)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 9;
                                    else
                                         if (noofdays == 10)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 10;
                                    else
                                         if (noofdays == 11)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 11;
                                    else
                                         if (noofdays == 12)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 12;
                                    else
                                         if (noofdays == 13)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 13;
                                    else
                                         if (noofdays == 14)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 14;
                                    else
                                         if (noofdays == 15)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 15;
                                    else
                                         if (noofdays == 16)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 16;
                                    else
                                        ddlNoOfOtsPaysheet.SelectedValue = dt.Rows[i]["Nots"].ToString();
                                }
                            }

                        }

                        TxtESIRate.Text = dt.Rows[i]["ESIRate"].ToString();

                        if (ddlNoOfESIDaysPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["ESIDays"].ToString()) != false || dt.Rows[i]["ESIDays"].ToString() == "--Select--")
                            {
                                ddlNoOfESIDaysPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["ESIDays"].ToString().Trim().Length > 0)
                                {
                                    int ESIDays = int.Parse(dt.Rows[i]["ESIDays"].ToString());
                                    if (ESIDays == 0)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 0;
                                    else
                                        if (ESIDays == 1)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 1;
                                    else
                                            if (ESIDays == 2)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 2;
                                    else
                                                if (ESIDays == 3)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 3;
                                    else
                                    if (ESIDays == 4)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 4;
                                    else
                                    if (ESIDays == 5)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 5;
                                    else
                                    if (ESIDays == 6)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 6;
                                    else
                                    if (ESIDays == 7)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 7;
                                    else
                                    if (ESIDays == 8)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 8;
                                    else
                                    if (ESIDays == 9)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 9;
                                    else
                                    if (ESIDays == 10)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 10;
                                    else
                                    if (ESIDays == 11)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 11;
                                    else
                                    if (ESIDays == 12)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 12;
                                    else
                                        ddlNoOfESIDaysPaysheet.SelectedValue = dt.Rows[i]["ESIDays"].ToString();
                                }
                            }

                        }

                        txtotesiwages.Text = dt.Rows[i]["OTESICWAGES"].ToString();

                        if (ddlOTESIWagesDays != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["OTESIWagesDays"].ToString()) != false || dt.Rows[i]["OTESIWagesDays"].ToString() == "--Select--")
                            {
                                ddlOTESIWagesDays.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["OTESIWagesDays"].ToString().Trim().Length > 0)
                                {
                                    int OTESIWagesDays = int.Parse(dt.Rows[i]["OTESIWagesDays"].ToString());
                                    if (OTESIWagesDays == 0)
                                        ddlOTESIWagesDays.SelectedIndex = 0;
                                    else
                                        if (OTESIWagesDays == 1)
                                        ddlOTESIWagesDays.SelectedIndex = 1;
                                    else
                                            if (OTESIWagesDays == 2)
                                        ddlOTESIWagesDays.SelectedIndex = 2;
                                    else
                                                if (OTESIWagesDays == 3)
                                        ddlOTESIWagesDays.SelectedIndex = 3;
                                    else
                                                    if (OTESIWagesDays == 4)
                                        ddlOTESIWagesDays.SelectedIndex = 4;
                                    else
                                                    if (OTESIWagesDays == 5)
                                        ddlOTESIWagesDays.SelectedIndex = 5;
                                    else
                                                    if (OTESIWagesDays == 6)
                                        ddlOTESIWagesDays.SelectedIndex = 6;
                                    else
                                                    if (OTESIWagesDays == 7)
                                        ddlOTESIWagesDays.SelectedIndex = 7;
                                    else
                                                    if (OTESIWagesDays == 8)
                                        ddlOTESIWagesDays.SelectedIndex = 8;
                                    else
                                                    if (OTESIWagesDays == 9)
                                        ddlOTESIWagesDays.SelectedIndex = 9;
                                    else
                                         if (OTESIWagesDays == 10)
                                        ddlOTESIWagesDays.SelectedIndex = 10;
                                    else
                                         if (OTESIWagesDays == 11)
                                        ddlOTESIWagesDays.SelectedIndex = 11;
                                    else
                                         if (OTESIWagesDays == 12)
                                        ddlOTESIWagesDays.SelectedIndex = 12;
                                    else
                                         if (OTESIWagesDays == 13)
                                        ddlOTESIWagesDays.SelectedIndex = 13;
                                    else
                                         if (OTESIWagesDays == 14)
                                        ddlOTESIWagesDays.SelectedIndex = 14;
                                    else
                                         if (OTESIWagesDays == 15)
                                        ddlOTESIWagesDays.SelectedIndex = 15;
                                    else
                                         if (OTESIWagesDays == 16)
                                        ddlOTESIWagesDays.SelectedIndex = 16;
                                    else
                                        ddlOTESIWagesDays.SelectedValue = dt.Rows[i]["OTESIWagesDays"].ToString();
                                }
                            }

                        }

                        TxtPFRate.Text = dt.Rows[i]["PFRate"].ToString();

                        if (ddlNoOfPFDaysPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["PFDays"].ToString()) != false || dt.Rows[i]["PFDays"].ToString() == "--Select--")
                            {
                                ddlNoOfPFDaysPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["PFDays"].ToString().Trim().Length > 0)
                                {
                                    int PFDays = int.Parse(dt.Rows[i]["PFDays"].ToString());
                                    if (PFDays == 0)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 0;
                                    else
                                        if (PFDays == 1)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 1;
                                    else
                                            if (PFDays == 2)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 2;
                                    else
                                                if (PFDays == 3)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 3;
                                    else
                                                    if (PFDays == 4)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 4;
                                    else
                                         if (PFDays == 5)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 5;
                                    else
                                         if (PFDays == 6)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 6;
                                    else
                                         if (PFDays == 7)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 7;
                                    else
                                         if (PFDays == 8)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 8;
                                    else
                                         if (PFDays == 9)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 9;
                                    else
                                         if (PFDays == 10)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 10;
                                    else
                                         if (PFDays == 11)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 11;
                                    else
                                         if (PFDays == 12)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 12;
                                    else
                                        ddlNoOfPFDaysPaysheet.SelectedValue = dt.Rows[i]["PFDays"].ToString();
                                }
                            }

                        }

                        if (CDesgnsw.SelectedValue != "--Select--")
                        {
                            chkcdswPF.Checked = Convert.ToBoolean(dt.Rows[i]["ChkPF"].ToString());
                            chkcdswESI.Checked = Convert.ToBoolean(dt.Rows[i]["Chkesi"].ToString());
                        }
                        else
                        {

                        }

                        txtcca.Text = dt.Rows[i]["cca"].ToString();
                        txtattbonus.Text = dt.Rows[i]["attbonus"].ToString();

                        if (ddlAttbonustype != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["Attbonustype"].ToString()) != false || dt.Rows[i]["Attbonustype"].ToString() == "--Select--")
                            {
                                ddlAttbonustype.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["Attbonustype"].ToString().Trim().Length > 0)
                                {
                                    int Attbonustype = int.Parse(dt.Rows[i]["Attbonustype"].ToString());
                                    if (Attbonustype == 0)
                                        ddlAttbonustype.SelectedIndex = 0;
                                    else
                                        if (Attbonustype == 1)
                                        ddlAttbonustype.SelectedIndex = 1;
                                    else
                                            if (Attbonustype == 2)
                                        ddlAttbonustype.SelectedIndex = 2;
                                    if (Attbonustype == 3)
                                        ddlAttbonustype.SelectedIndex = 3;
                                    else
                                        ddlAttbonustype.SelectedValue = dt.Rows[i]["Attbonustype"].ToString();
                                }
                            }

                        }

                        txtwa.Text = dt.Rows[i]["washallownce"].ToString();
                        txtNfhs1.Text = dt.Rows[i]["NFHs"].ToString();
                        Txtrc.Text = dt.Rows[i]["rc"].ToString();
                        //TxtIncentives.Text = dt.Rows[i]["Incentives"].ToString();
                        TxtCSWPLDays.Text = dt.Rows[i]["PLDays"].ToString();
                        TxtCSWPLAmount.Text = dt.Rows[i]["PLAmount"].ToString();
                        TxtCSWTLDays.Text = dt.Rows[i]["TLDays"].ToString();
                        TxtCSWTLAmount.Text = dt.Rows[i]["TLAmount"].ToString();
                        TxtCs.Text = dt.Rows[i]["cs"].ToString();
                        TxtScPer.Text = dt.Rows[i]["ServicechargePer"].ToString();

                        TxtNHSRate.Text = dt.Rows[i]["NHSRate"].ToString();

                        if (ddlNoOfNhsPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["NNhs"].ToString()) != false || dt.Rows[i]["NNhs"].ToString() == "--Select--")
                            {
                                ddlNoOfNhsPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["NNhs"].ToString().Trim().Length > 0)
                                {
                                    int NoOfNhsPaysheet = int.Parse(dt.Rows[i]["NNhs"].ToString());
                                    if (NoOfNhsPaysheet == 0)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 0;
                                    else
                                        if (NoOfNhsPaysheet == 1)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 1;
                                    else
                                            if (NoOfNhsPaysheet == 2)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 2;
                                    else
                                                if (NoOfNhsPaysheet == 3)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 3;
                                    else
                                                    if (NoOfNhsPaysheet == 4)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 4;
                                    else
                                         if (NoOfNhsPaysheet == 5)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 5;
                                    else
                                         if (NoOfNhsPaysheet == 6)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 6;
                                    else
                                         if (NoOfNhsPaysheet == 7)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 7;
                                    else
                                         if (NoOfNhsPaysheet == 8)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 8;
                                    else
                                         if (NoOfNhsPaysheet == 9)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 9;
                                    else
                                         if (NoOfNhsPaysheet == 10)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 10;
                                    else
                                        ddlNoOfNhsPaysheet.SelectedValue = dt.Rows[i]["NNhs"].ToString();
                                }
                            }

                        }

                        if (ddlNoOfWosPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dt.Rows[i]["NWos"].ToString()) != false || dt.Rows[i]["NWos"].ToString() == "--Select--")
                            {
                                ddlNoOfWosPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dt.Rows[i]["NWos"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dt.Rows[i]["NWos"].ToString());
                                    if (noofdays == 0)
                                        ddlNoOfWosPaysheet.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlNoOfWosPaysheet.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlNoOfWosPaysheet.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlNoOfWosPaysheet.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlNoOfWosPaysheet.SelectedIndex = 4;
                                    else

                                        ddlNoOfWosPaysheet.SelectedValue = dt.Rows[i]["NWos"].ToString();
                                }
                            }

                        }
                        TxtWORate.Text = dt.Rows[i]["WORate"].ToString();

                        TxtTravelAllowance.Text = dt.Rows[i]["TravelAllw"].ToString();
                        TxtPerformanceAllowance.Text = dt.Rows[i]["PerformanceAllw"].ToString();
                        TxtMobileAllowance.Text = dt.Rows[i]["MobileAllw"].ToString();
                        TxtOThrs.Text = dt.Rows[i]["OThrs"].ToString();
                        TxtADVDed.Text = dt.Rows[i]["ADVDed"].ToString();
                        TxtWCDed.Text = dt.Rows[i]["WCDed"].ToString();
                        TxtUniformDed.Text = dt.Rows[i]["UniformDed"].ToString();
                        txtservicewt.Text = dt.Rows[i]["Serviceweightage"].ToString();
                        txtHardshipAllw.Text = dt.Rows[i]["HardshipAllw"].ToString();
                        txtRankAllw.Text = dt.Rows[i]["RankAllw"].ToString();
                        txtuniformcharge.Text = dt.Rows[i]["uniformcharge"].ToString();

                        txtPFEmp.Text = dt.Rows[i]["PFEmp"].ToString();
                        txtESIEmp.Text = dt.Rows[i]["ESIEmp"].ToString();
                        txtPFEmpr.Text = dt.Rows[i]["PFEmpr"].ToString();
                        txtESIEmpr.Text = dt.Rows[i]["ESIEmpr"].ToString();
                        txtSCEmp.Text = dt.Rows[i]["SCEmp"].ToString();
                        txtCTC.Text = dt.Rows[i]["CTC"].ToString();
                        TxtIncentiveRate.Text = dt.Rows[i]["IncentiveRate"].ToString();
                        TxtLCRate.Text = dt.Rows[i]["LCRate"].ToString();

                        //txtmiscded.Text = dt.Rows[i]["MiscDed"].ToString();


                        rowIndex++;
                    }
                }
            }
        }

        protected void Fillcname()
        {
            if (ddlclientid.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
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

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void DefaultRowDatasw(int row)
        {
            //Gvswdesignations Data 

            DropDownList ddlindexsw = gvSWDesignations.Rows[row].FindControl("DdlDesign") as DropDownList;
            ddlindexsw.SelectedIndex = 0;

            DropDownList ddlCategorysw = gvSWDesignations.Rows[row].FindControl("DdlCategory") as DropDownList;
            ddlCategorysw.SelectedIndex = 0;

            DropDownList ddlNoofDayssw = gvSWDesignations.Rows[row].FindControl("ddlNoOfDaysWages") as DropDownList;
            ddlNoofDayssw.SelectedIndex = 0;

            DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[row].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
            ddlNoOfOtsPaysheet.SelectedIndex = 0;

            DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[row].FindControl("ddlNoOfNhsPaysheet") as DropDownList;
            ddlNoOfNhsPaysheet.SelectedIndex = 0;

            DropDownList ddlNoOfWosPaysheet = gvSWDesignations.Rows[row].FindControl("ddlNoOfWosPaysheet") as DropDownList;
            ddlNoOfWosPaysheet.SelectedIndex = 0;

            DropDownList ddlNoOfPFDaysPaysheet = gvSWDesignations.Rows[row].FindControl("ddlNoOfPFDaysPaysheet") as DropDownList;
            ddlNoOfPFDaysPaysheet.SelectedIndex = 0;

            DropDownList ddlNoOfESIDaysPaysheet = gvSWDesignations.Rows[row].FindControl("ddlNoOfESIDaysPaysheet") as DropDownList;
            ddlNoOfESIDaysPaysheet.SelectedIndex = 0;

            DropDownList ddlOTESIWagesDays = gvSWDesignations.Rows[row].FindControl("ddlOTESIWagesDays") as DropDownList;
            ddlOTESIWagesDays.SelectedIndex = 0;

            DropDownList ddlSplAllwDays = gvSWDesignations.Rows[row].FindControl("ddlSplAllwDays") as DropDownList;
            ddlSplAllwDays.SelectedIndex = 0;

            DropDownList ddllatype = gvSWDesignations.Rows[row].FindControl("ddllatype") as DropDownList;
            ddllatype.SelectedIndex = 0;

            DropDownList ddlgratuitytype = gvSWDesignations.Rows[row].FindControl("ddlgratuitytype") as DropDownList;
            ddlgratuitytype.SelectedIndex = 0;

            DropDownList ddlbonustype = gvSWDesignations.Rows[row].FindControl("ddlbonustype") as DropDownList;
            ddlbonustype.SelectedIndex = 0;

            DropDownList ddlAttbonustype = gvSWDesignations.Rows[row].FindControl("ddlAttbonustype") as DropDownList;
            ddlAttbonustype.SelectedIndex = 0;

            DropDownList ddlGunAllwType = gvSWDesignations.Rows[row].FindControl("ddlGunAllwType") as DropDownList;
            ddlGunAllwType.SelectedIndex = 0;

            TextBox TxtPFRate = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtPFRate");
            TxtPFRate.Text = "";

            TextBox TxtESIRate = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtESIRate");
            TxtESIRate.Text = "";

            TextBox txtservice = (TextBox)gvSWDesignations.Rows[row].FindControl("txtservicewt");
            txtservice.Text = "";
            TextBox txtHardshipAllw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtHardshipAllw");
            txtHardshipAllw.Text = "";
            TextBox txtRankAllw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtRankAllw");
            txtRankAllw.Text = "";

            TextBox txtuniformcharge = (TextBox)gvSWDesignations.Rows[row].FindControl("txtuniformcharge");
            txtuniformcharge.Text = "";

            TextBox txtPFEmp = (TextBox)gvSWDesignations.Rows[row].FindControl("txtPFEmp");
            txtPFEmp.Text = "";

            TextBox txtESIEmp = (TextBox)gvSWDesignations.Rows[row].FindControl("txtESIEmp");
            txtESIEmp.Text = "";

            TextBox txtPFEmpr = (TextBox)gvSWDesignations.Rows[row].FindControl("txtPFEmpr");
            txtPFEmpr.Text = "";

            TextBox txtESIEmpr = (TextBox)gvSWDesignations.Rows[row].FindControl("txtESIEmpr");
            txtESIEmpr.Text = "";

            TextBox txtSCEmp = (TextBox)gvSWDesignations.Rows[row].FindControl("txtSCEmp");
            txtSCEmp.Text = "";

            TextBox txtCTC = (TextBox)gvSWDesignations.Rows[row].FindControl("txtCTC");
            txtCTC.Text = "";

            TextBox TxtIncentiveRate = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtIncentiveRate");
            TxtIncentiveRate.Text = "";


            TextBox TxtLCRate = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtLCRate");
            TxtLCRate.Text = "";

            TextBox Cgrosssw = (TextBox)gvSWDesignations.Rows[row].FindControl("Txtgross");
            Cgrosssw.Text = "";

            TextBox Cbasicsw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtBasic");
            Cbasicsw.Text = "";
            TextBox Cdasw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtda");
            Cdasw.Text = "";
            TextBox Chrasw = (TextBox)gvSWDesignations.Rows[row].FindControl("txthra");
            Chrasw.Text = "";
            TextBox CConveyancesw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtConveyance");
            CConveyancesw.Text = "";

            TextBox Cawsw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtoa");
            Cawsw.Text = "";
            TextBox Cwashallowancesw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtwa");
            Cwashallowancesw.Text = "";
            TextBox Ccasw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtcca");
            Ccasw.Text = "";
            TextBox CLeaveAmountsw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtleaveamount");
            CLeaveAmountsw.Text = "";
            TextBox CGratutysw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtgratuty");
            CGratutysw.Text = "";
            TextBox CBonussw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtbonus");
            CBonussw.Text = "";
            TextBox CWashallowancesw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtwa");
            CWashallowancesw.Text = "";
            TextBox COtherallowancesw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtoa");
            COtherallowancesw.Text = "";
            TextBox txtNfhs1 = (TextBox)gvSWDesignations.Rows[row].FindControl("txtNfhs1");
            txtNfhs1.Text = "";
            TextBox Txtrc = (TextBox)gvSWDesignations.Rows[row].FindControl("Txtrc");
            Txtrc.Text = "";

            TextBox TxtCs = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtCs");
            TxtCs.Text = "";
            TextBox TxtOTRate = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtOTRate");
            TxtOTRate.Text = "";
            TextBox TxtOTHrs = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtOThrs");
            TxtOTHrs.Text = "";

            TextBox CTxtScPersw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtScPer");
            CTxtScPersw.Text = "";
            TextBox Ctxtattbonussw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtattbonus");
            Ctxtattbonussw.Text = "";
            TextBox CtxtPLDayssw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtCSWPLDays");
            CtxtPLDayssw.Text = "";
            TextBox CtxtPLmountsw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtCSWPLAmount");
            CtxtPLmountsw.Text = "";
            TextBox CtxtTLDayssw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtCSWTLDays");
            CtxtTLDayssw.Text = "";
            TextBox CtxtTLnhssw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtNHSRate");
            CtxtTLnhssw.Text = "";
            TextBox CtxtTLwosw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtWORate");
            CtxtTLwosw.Text = "";
            TextBox CtxtTLfoodsw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtFoodAllowance");
            CtxtTLfoodsw.Text = "";
            TextBox CtxtTLmedicalsw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtMedicalAllowance");
            CtxtTLmedicalsw.Text = "";

            TextBox CtxtTLsplsw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtSplAllowance");
            CtxtTLsplsw.Text = "";

            TextBox TxtTravelAllowance = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtTravelAllowance");
            TxtTravelAllowance.Text = "";

            TextBox TxtPerformanceAllowance = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtPerformanceAllowance");
            TxtPerformanceAllowance.Text = "";

            TextBox TxtMobileAllowance = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtMobileAllowance");
            TxtMobileAllowance.Text = "";


            TextBox CtxtTLadvdedsw = (TextBox)gvSWDesignations.Rows[row].FindControl("Txtadvded");
            CtxtTLadvdedsw.Text = "";
            TextBox CtxtTLwcdedsw = (TextBox)gvSWDesignations.Rows[row].FindControl("Txtwcded");
            CtxtTLwcdedsw.Text = "";
            TextBox CtxtTLunidedsw = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtUniformDed");
            CtxtTLunidedsw.Text = "";

            TextBox TxtsiteAllowance = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtsiteAllowance");
            TxtsiteAllowance.Text = "";

            TextBox txtaddlhrallw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtaddlhrallw");
            txtaddlhrallw.Text = "";

            TextBox txtqtrallw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtqtrallw");
            txtqtrallw.Text = "";

            TextBox txtrelallw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtrelallw");
            txtrelallw.Text = "";

            TextBox txtGunallw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtGunallw");
            txtGunallw.Text = "";

            TextBox txtFireallw = (TextBox)gvSWDesignations.Rows[row].FindControl("txtFireallw");
            txtFireallw.Text = "";

            TextBox txtotesiwages = (TextBox)gvSWDesignations.Rows[row].FindControl("txtotesiwages");
            txtotesiwages.Text = "";

            TextBox TxtNetPay = (TextBox)gvSWDesignations.Rows[row].FindControl("TxtNetPay");
            TxtNetPay.Text = "";

        }

        protected void DefaultRowData(int row)
        {
            string Cddldesignation = ((DropDownList)gvdesignation.Rows[row].FindControl("DdlDesign")).Text;
            DropDownList ddlindex = gvdesignation.Rows[row].FindControl("DdlDesign") as DropDownList;
            ddlindex.SelectedIndex = 0;

            DropDownList ddlNoofDays = gvdesignation.Rows[row].FindControl("ddlNoOfDaysBilling") as DropDownList;
            ddlNoofDays.SelectedIndex = 0;

            DropDownList ddlddlType = gvdesignation.Rows[row].FindControl("ddlType") as DropDownList;
            ddlddlType.SelectedIndex = 0;


            DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[row].FindControl("ddlNoOfOtsPaysheet") as DropDownList;
            ddlNoOfOtsPaysheet.SelectedIndex = 0;

            string Cddldutytype = ((DropDownList)gvdesignation.Rows[row].FindControl("ddldutytype")).Text;
            DropDownList ddldutyindex = gvdesignation.Rows[row].FindControl("ddldutytype") as DropDownList;
            ddldutyindex.SelectedIndex = 0;
            TextBox Cdutyhrs = (TextBox)gvdesignation.Rows[row].FindControl("txtdutyhrs");
            Cdutyhrs.Text = "";
            TextBox Cquantity = (TextBox)gvdesignation.Rows[row].FindControl("txtquantity");
            Cquantity.Text = "";
            TextBox Csummary = (TextBox)gvdesignation.Rows[row].FindControl("txtsummary");
            Csummary.Text = "";
            TextBox Cbasic = (TextBox)gvdesignation.Rows[row].FindControl("TxtBasic");
            Cbasic.Text = "";
            TextBox Cda = (TextBox)gvdesignation.Rows[row].FindControl("txtda");
            Cda.Text = "";
            TextBox Chra = (TextBox)gvdesignation.Rows[row].FindControl("txthra");
            Chra.Text = "";
            TextBox CConveyance = (TextBox)gvdesignation.Rows[row].FindControl("txtConveyance");
            CConveyance.Text = "";

            TextBox Caw = (TextBox)gvdesignation.Rows[row].FindControl("txtoa");
            Caw.Text = "";
            TextBox Cwashallowance = (TextBox)gvdesignation.Rows[row].FindControl("txtwa");
            Cwashallowance.Text = "";
            TextBox Cca = (TextBox)gvdesignation.Rows[row].FindControl("txtcca");
            Cca.Text = "";
            TextBox CLeaveAmount = (TextBox)gvdesignation.Rows[row].FindControl("txtleaveamount");
            CLeaveAmount.Text = "";
            TextBox CGratuty = (TextBox)gvdesignation.Rows[row].FindControl("txtgratuty");
            CGratuty.Text = "";
            TextBox CBonus = (TextBox)gvdesignation.Rows[row].FindControl("txtbonus");
            CBonus.Text = "";
            TextBox CPayRate = (TextBox)gvdesignation.Rows[row].FindControl("txtPayRate");
            CPayRate.Text = "";
            TextBox txtNfhs = (TextBox)gvdesignation.Rows[row].FindControl("txtNfhs");
            txtNfhs.Text = "";

            TextBox Txtrc = (TextBox)gvdesignation.Rows[row].FindControl("Txtrc");
            Txtrc.Text = "";

            TextBox TxtCs = (TextBox)gvdesignation.Rows[row].FindControl("TxtCs");
            TxtCs.Text = "";

            TextBox TxtOTRate = (TextBox)gvdesignation.Rows[row].FindControl("TxtOTRate");
            TxtOTRate.Text = "";

            TextBox CTxtScPer = (TextBox)gvdesignation.Rows[row].FindControl("TxtScPer");
            CTxtScPer.Text = "";
            TextBox Ctxtattbonus = (TextBox)gvdesignation.Rows[row].FindControl("txtattbonus");
            CTxtScPer.Text = "";

            CheckBox ChkRCApplicable = (CheckBox)gvdesignation.Rows[row].FindControl("ChkRCApplicable");
            ChkRCApplicable.Checked = false;



        }
        //Display Data In GridView
        private void displaydata()
        {
            // string selectquery = "Select Design from designations ORDER BY Design";
            // DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);
            DataTable DtDesignation = GlobalData.Instance.LoadDesigns();

            DataTable DtCategory = GlobalData.Instance.LoadWageCategory();

            DataTable DtHSNNumber = GlobalData.Instance.LoadHSNNumbers();

            gvdesignation.DataSource = DtDesignation;
            gvdesignation.DataBind();

            gvSWDesignations.DataSource = DtDesignation;
            gvSWDesignations.DataBind();

            //gvSWDesignations.DataSource = DtCategory;
            //gvSWDesignations.DataBind();

            foreach (GridViewRow grdRow in gvdesignation.Rows)
            {
                bind_dropdownlist = (DropDownList)(gvdesignation.Rows[grdRow.RowIndex].Cells[0].FindControl("DdlDesign"));
                bind_dropdownlist.Items.Clear();
                //bind_dropdownlist.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    bind_dropdownlist.Items.Add(DtDesignation.Rows[i][0].ToString());
                //}

                if (DtDesignation.Rows.Count > 0)
                {
                    bind_dropdownlist.DataValueField = "Designid";
                    bind_dropdownlist.DataTextField = "Design";
                    bind_dropdownlist.DataSource = DtDesignation;
                    bind_dropdownlist.DataBind();

                }
                bind_dropdownlist.Items.Insert(0, "--Select--");
                bind_dropdownlist.SelectedIndex = 0;

                break;
            }
            foreach (GridViewRow grdRow in gvSWDesignations.Rows)
            {
                bind_dropdownlistsw = (DropDownList)(gvSWDesignations.Rows[grdRow.RowIndex].Cells[0].FindControl("DdlDesign"));
                bind_dropdownlistsw.Items.Clear();
                //bind_dropdownlistsw.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    bind_dropdownlistsw.Items.Add(DtDesignation.Rows[i][0].ToString());

                //}



                if (DtDesignation.Rows.Count > 0)
                {
                    bind_dropdownlistsw.DataValueField = "Designid";
                    bind_dropdownlistsw.DataTextField = "Design";
                    bind_dropdownlistsw.DataSource = DtDesignation;
                    bind_dropdownlistsw.DataBind();

                }
                bind_dropdownlistsw.Items.Insert(0, "--Select--");
                bind_dropdownlistsw.SelectedIndex = 0;


                bind_dropdownlistswc = (DropDownList)(gvSWDesignations.Rows[grdRow.RowIndex].Cells[1].FindControl("DdlCategory"));
                bind_dropdownlistswc.Items.Clear();

                if (DtCategory.Rows.Count > 0)
                {
                    bind_dropdownlistswc.DataValueField = "id";
                    bind_dropdownlistswc.DataTextField = "name";
                    bind_dropdownlistswc.DataSource = DtCategory;
                    bind_dropdownlistswc.DataBind();

                }
                bind_dropdownlistswc.Items.Insert(0, "--Select--");
                bind_dropdownlistswc.SelectedIndex = 0;

                bind_dropdownlistHSN = (DropDownList)(gvdesignation.Rows[grdRow.RowIndex].Cells[8].FindControl("ddlHSNNumber"));

                if (DtHSNNumber.Rows.Count > 0)
                {
                    bind_dropdownlistHSN.DataValueField = "Id";
                    bind_dropdownlistHSN.DataTextField = "HSNNo";
                    bind_dropdownlistHSN.DataSource = DtHSNNumber;
                    bind_dropdownlistHSN.DataBind();

                }
            }




        }

        private void ClearDataFromThePage()
        {
            //ddlclientid.SelectedIndex = ddlcname.SelectedIndex =
            DdlPf.SelectedIndex = DdlEsi.SelectedIndex = DdlOt.SelectedIndex = ddlWagesCalnOn.SelectedIndex = ddlPTOn.SelectedIndex = 0;
            txtStartingDate.Text = txtEndingDate.Text = txtValidityDate.Text = "";
            txtBGAmount.Text = txtcontractid.Text = TxtContractDescription.Text = txtEMDValue.Text = txtSecurityDeposit.Text = txtTypeOfWork.Text = txtMaterial.Text =
            txtMachinary.Text = txtEMDValue.Text = txtlampsum.Text = txtPerGurante.Text = txtOWF.Text = txtadmincharges.Text =

            txtservicecharge.Text = txtservicedesc.Text = txtWAWA.Text = txtotsalaryrate.Text = txttlamt.Text = txtadmincharges.Text = "";
            TxtPf.Text = TxtEsi.Text = "100";
            RadioManPower.Checked = radiono.Checked = chkStaxonservicecharge.Checked = chksconpfesi.Checked = RadioPercent.Checked = Radio1to1.Checked = RadioClient.Checked =
            RadioWithST.Checked = radiootregular.Checked = RdbIGST.Checked = true;
            chkCGST.Checked = RdbSGST.Checked = RadioLumpsum.Checked = RadioAmount.Checked = RadioStartDate.Checked = RadioCompany.Checked = Chkpdfs.Checked = ChkbillfromPaysheetduties.Checked =
             RadioWithoutST.Checked = radiootspecial.Checked = RadioSpecial.Checked = RadioIndividual.Checked = chkojt.Checked = ChkPFEmpr.Checked = ChkESIEmpr.Checked = chktl.Checked = chkCess1.Checked = chkCess2.Checked = chkNoUnif.Checked = chkNoSal.Checked = chkNoLWF.Checked = chkNoRegFee.Checked = false;
            ddlnoofdays.SelectedIndex = ddlNoOfDaysWages.SelectedIndex = ddlbilldates.SelectedIndex = ddlPaySheetDates.SelectedIndex = ddlpfon.SelectedIndex = ddlesion.SelectedIndex = 0;
            chkotsalaryrate.Checked = checkPFonOT.Checked = checkESIonOT.Checked = chkProfTax.Checked = chkspt.Checked = CheckIncludeST.Checked = Check75ST.Checked = Chkpf.Checked = ChkEsi.Checked = chkrc.Checked = chkGSTLineItem.Checked = false;

            //new pfon check boxes 

            ChkpfonBasic.Checked = ChkpfonDA.Checked = ChkpfonHRA.Checked = ChkpfonConv.Checked = ChkpfonMedicalAllw.Checked = ChkpfonFoodAllw.Checked = ChkpfonSiteAllw.Checked = ChkpfonSpecialAllw.Checked = ChkpfonOA.Checked = ChkpfonWA.Checked = ChkpfonGunAllw.Checked = ChkpfonFireAllw.Checked = ChkpfonBonus.Checked = ChkpfonGratuity.Checked = ChkpfonLA.Checked = ChkpfonOTRate.Checked = ChkpfonESIPayrate.Checked = ChkpfonPFPayrate.Checked = ChkpfonCCA.Checked /*= ChkpfonNFHS.Checked*/ = ChkpfonRC.Checked = ChkpfonPLAmount.Checked = ChkpfonTLAmount.Checked = ChkpfonServicecharge.Checked = ChkpfonNHSRate.Checked = ChkpfonWoRate.Checked = ChkpfonTravelAllw.Checked = ChkpfonOTHrs.Checked = ChkpfonPerformanceAllw.Checked = ChkpfonMobileAllw.Checked = ChkpfonUniformCharges.Checked = ChkpfonServiceWeightage.Checked = false;

            //end 

            //new ESIon check boxes 

            ChkESIonBasic.Checked = ChkESIonDA.Checked = ChkESIonHRA.Checked = ChkESIonConv.Checked = ChkESIonMedicalAllw.Checked = ChkESIonFoodAllw.Checked = ChkESIonSiteAllw.Checked = ChkESIonSpecialAllw.Checked = ChkESIonOA.Checked = ChkESIonWA.Checked = ChkESIonGunAllw.Checked = ChkESIonFireAllw.Checked = ChkESIonBonus.Checked = ChkESIonGratuity.Checked = ChkESIonLA.Checked = ChkESIonOTRate.Checked = ChkESIonESIPayrate.Checked = ChkESIonPFPayrate.Checked = ChkESIonCCA.Checked /*= ChkESIonNFHS.Checked*/ = ChkESIonRC.Checked = ChkESIonPLAmount.Checked = ChkESIonTLAmount.Checked = ChkESIonServicecharge.Checked = ChkESIonNHSRate.Checked = ChkESIonWoRate.Checked = ChkESIonTravelAllw.Checked = ChkESIonOTHrs.Checked = ChkESIonPerformanceAllw.Checked = ChkESIonMobileAllw.Checked = ChkESIonUniformCharges.Checked = ChkESIonServiceWeightage.Checked = false;

            //end 
            txtTds.Text = txtPono.Text = txtPoDate.Text = txtExpectdateofreceipt.Text = txtLumpsumtext.Text = string.Empty;

            if (radiono.Checked == true)
            {
                //RadioPercent.Visible = false;
                //RadioAmount.Visible = false;
                //txtservicecharge.Visible = false;
                //txtservicedesc.Visible = false;
                //lbldesc.Visible = false;
                //chkStaxonservicecharge.Visible = false;
            }

            ddlTDSon.SelectedIndex = 0;
            if (RadioManPower.Checked == true)
            {
                txtlampsum.Visible = false;
                txtLumpsumtext.Visible = false;
                lbllumpsumtext.Visible = false;
            }


            TxtInvDescription.Text = "Description";
            txtInvNoofDuties.Text = "No.of Duties";
            txtInvNoofEmployees.Text = "No.of Emps";
            txtInvPayrate.Text = "Payrate";
            txtInvAmount.Text = "Amount";

            txtInvmonthdays.Text = "No Of Days in a Month";
            txtInvsaccode.Text = "HSN/SAC Code";

            ddltypeofwork.SelectedIndex = 0;

        }

        protected void Btn_Save_Contracts_Click(object sender, EventArgs e)
        {
            try
            {
                var testDate = 0;

                #region  Begin Code For Validations As on [18-10-2013]

                #region     Begin Code For Check The Client Id/Name Selected Or Not   as on [18-10-2013]
                if (ddlclientid.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select Clientid";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' please Select Clientid ');", true);
                    clearcontractdetails();
                    return;
                }
                #endregion  End Code For Check The Client Id/Name Selected Or Not  as on [18-10-2013]

                #region     Begin Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]
                if (txtStartingDate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill start date.');", true);
                    return;
                }

                if (txtStartingDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtStartingDate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "show alert", "alert('You Are Entered Invalid Contract Start Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                    else
                    {
                        string CheckSD = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                        //string CheckSD = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();

                        string CheckStartDate = "";

                        if (ddlContractids.SelectedIndex == 0)
                        {
                            CheckStartDate = " select clientid from contracts  where ContractEndDate>='" +
                                CheckSD + "'  and Clientid='" + ddlclientid.SelectedValue + "'";

                            DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(CheckStartDate).Result;
                            if (Dt.Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(),
                                  "show alert", "alert('You Are Entered Invalid Contract Start Date.Start Date Should Not Be Interval of the Previous Contracts Start and End Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                return;
                            }
                        }
                        else
                        {
                            if (ddlContractids.SelectedIndex > 1)
                            {
                                string CIDForCheck = (txtcontractid.Text).ToString().Substring((txtcontractid.Text).Length - 2);
                                CheckStartDate = " select clientid from contracts  where ContractEndDate>='" +
                                    CheckSD + "'  and Clientid='" + ddlclientid.SelectedValue + "'  and Right(contractid,2)<" + CIDForCheck;

                                DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(CheckStartDate).Result;
                                if (Dt.Rows.Count > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(),
                                      "show alert", "alert('You Are Entered Invalid Contract Start Date.Start Date Should Not Be Interval of the Previous Contracts Start and End Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                    return;
                                }
                            }
                        }

                    }


                }


                #endregion  End Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]

                #region     Begin Code For Check The Contract End Date Enetered Or Not  as on [18-10-2013]
                if (txtEndingDate.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please fill End date.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill End date.');", true);
                    return;
                }

                if (txtEndingDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtEndingDate.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Contract End Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        // ScriptManager.RegisterStartupScript(this, GetType(),
                        //"show alert", "alert('You Are Entered Invalid Contract End Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }

                }



                #endregion  End Code For Check The Contract End Date Enetered Or Not as on [18-10-2013]

                #region     Begin Code For Check The Selected Dates are Valid Or Not  as on [18-10-2013]
                if (txtStartingDate.Text.Trim().Length != 0 && txtEndingDate.Text.Trim().Length != 0)
                {
                    DateTime Dtstartdate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb"));
                    DateTime DtEnddate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb"));


                    if (Dtstartdate >= DtEnddate)
                    {
                        lblMsg.Text = "Invalid Contract End Date . Contract End Date Always Should Be Greater Than To Start Date.";
                        return;
                    }
                }
                #endregion  End Code For Check Selected Dates are Valid Or Not   as on [18-10-2013]

                #region     Begin Code For Check The Lampsum if Lampsum Selected  as on [18-10-2013]
                if (RadioLumpsum.Checked)
                {
                    if (txtlampsum.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please Enter The Lampsum Amount.";
                        return;
                    }
                }

                #endregion  End Code For Check The Lampsum if Lampsum Selected    as on [18-10-2013]

                #region     Begin Code For Check The Service Charge if Service Charge Yes Selected  as on [18-10-2013]
                if (radioyes.Checked)
                {
                    if (txtservicecharge.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please Enter The Service Charge.";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter The Service Charge.');", true);
                        return;
                    }
                }

                #endregion  End Code For Check The Service Charge if Service Charge Yes Selected    as on [18-10-2013]
                #region begin code for RCM  and RCMText
                var RCM = 0;
                var RCMText = string.Empty;
                #endregion
                #endregion  Begin Code For Validations As on [18-10-2013]

                if (rdbGSTSplYes.Checked == true && ddlGSTSplPer.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select GST Spl Per.";
                    return;
                }

                #region  Begin Code For Declaring Variables as on [18-10-2013]

                #region  Begin Code For Variable Declaration Which is Stored into Contracts Table as on [18-10-2013]

                #region  Begin Code For V-D   (1) to (5)  : Ref Clientid To ContractID

                var ClientId = "";
                var ContractStartDate = "01/01/1900";
                var ContractEndDate = "01/01/1900";
                var BGAmount = "0";
                var ContractId = "";

                #endregion  End Code For V-D   (1) to (5)  : Ref Clientid To ContractID

                #region  Begin Code For V-D   (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                var SecurityDeposit = "0";
                var TypeOfWork = "";
                var MaterialCostPerMonth = "0";
                var ValidityDate = "01/01/1900";
                var MachinaryCostPerMonth = "0";

                #endregion  End Code For V-D   (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                #region  Begin Code For V-D   (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                var EMDValue = "0";
                var PaymentType = "0";
                var WageAct = "0";
                var PayLumpsum = "0";
                var PerformanceGuaranty = "0";

                #endregion  End Code For V-D   (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                #region  Begin Code For V-D     : Ref PF To ESI On OT

                var PF = "0";
                var PFFrom = 0;
                var PFonOT = 0;
                //new variables for PFon

                var pfonBasic = 0; var pfonDA = 0; var pfonHRA = 0; var pfonConv = 0; var pfonMedicalAllw = 0; var pfonFoodAllw = 0; var pfonSiteAllw = 0; var pfonSpecialAllw = 0; var pfonOA = 0; var pfonWA = 0; var pfonGunAllw = 0; var pfonFireAllw = 0; var pfonBonus = 0; var pfonGratuity = 0; var pfonLA = 0; var pfonOTRate = 0; var pfonESIPayrate = 0; var pfonPFPayrate = 0; var pfonCCA = 0; var pfonNFHS = 0; var pfonRC = 0; var pfonPLAmount = 0; var pfonTLAmount = 0; var pfonServicecharge = 0; var pfonNHSRate = 0; var pfonWoRate = 0; var pfonTravelAllw = 0; var pfonOTHrs = 0; var pfonPerformanceAllw = 0; var pfonMobileAllw = 0; var pfonUniformCharges = 0; var pfonServiceWeightage = 0;

                //end 

                //new variables for ESIon

                var ESIonBasic = 0; var ESIonDA = 0; var ESIonHRA = 0; var ESIonConv = 0; var ESIonMedicalAllw = 0; var ESIonFoodAllw = 0; var ESIonSiteAllw = 0; var ESIonSpecialAllw = 0; var ESIonOA = 0; var ESIonWA = 0; var ESIonGunAllw = 0; var ESIonFireAllw = 0; var ESIonBonus = 0; var ESIonGratuity = 0; var ESIonLA = 0; var ESIonOTRate = 0; var ESIonESIPayrate = 0; var ESIonPFPayrate = 0; var ESIonCCA = 0; var ESIonNFHS = 0; var ESIonRC = 0; var ESIonPLAmount = 0; var ESIonTLAmount = 0; var ESIonServicecharge = 0; var ESIonNHSRate = 0; var ESIonWoRate = 0; var ESIonTravelAllw = 0; var ESIonOTHrs = 0; var ESIonPerformanceAllw = 0; var ESIonMobileAllw = 0; var ESIonUniformCharges = 0; var ESIonServiceWeightage = 0;

                //end 


                var ESI = "0";
                var ESIFrom = 0;
                var ESIonOt = 0;

                var Pflimit = "0";
                var Esilimit = "0";
                var Bpf = 0;
                var Besi = 0;
                var RelChrg = 0;

                #endregion  End Code For V-D     : Ref PF To ESI On OT

                #region    Begin Code For V-D     : Ref    Servicharge  Yes To   Service Chagre Amount
                var ServiceChargeType = 0;
                var ServiceCharge = "0";
                var BillDates = 0;
                var ServiceDesc = "";
                var PaySheetDates = 0;
                var WageType = 0;
                var ProfTax = 0;
                var SProfTax = 0;
                #endregion   End  Code For V-D     : Ref    Servicharge  Yes To   Service Chagre Amount

                #region    Begin Code For V-D     : Ref    ServiceTaxType To   TL No

                var ServiceTaxType = 0;
                var IncludeST = 0;
                var ServiceTax75 = 0;
                var OTPersent = 0;
                var OWF = "";
                var AdminCharges = "";
                var OTAmounttype = 0;
                var Description = "";
                var ContractDescription = "";
                var otsalaryratecheck = 0;
                var otsalaryrat = "0";
                var ojt = 0;
                var PFEmpr = 0;
                var ESIEmpr = 0;
                var TL = 0;
                var TLNo = "0";

                #endregion   End  Code For V-D     : Ref       ServiceTaxType To   Description

                #region    Begin Code For V-D     : Ref    New Field add in Contract on 29/03/2014 by venkat

                var Tds = 0;
                var Pono = "";
                var PoDate = "";
                var ReceiptExpectedDate = 0;
                var Staxonservicecharge = 0;
                var SCOnPFESIEmpr = 0;
                var Lumpsumtext = "";
                var TDSon = 0;
                #endregion   End  Code For V-D     : Ref       New Field add in Contract on 29/03/2014 by venkat

                #region Begin Code for Esi branche adding as on 02/08/2014
                var Esibranch = "0";
                var pdfs = 0;
                var TypeForBillNo = 0;
                var WithRoundoff = 0;
                var BillFromPayhsheetDuties = 0;
                var WagesCalnOn = 0;
                var PTOn = 0;
                string IndNoOfOts = "0";
                var NoUniform = 0;
                var NoSalAdv = 0;
                var NoLWF = 0;
                var NoRegFee = 0;

                if (ddlIndNoOfOtsPaysheet.SelectedIndex == 0)
                {
                    IndNoOfOts = "99";
                }
                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 1)
                {
                    IndNoOfOts = "0";
                }
                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 2)
                {
                    IndNoOfOts = "1";
                }
                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 3)
                {
                    IndNoOfOts = "2";
                }
                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 4)
                {
                    IndNoOfOts = "3";
                }

                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 5)
                {
                    IndNoOfOts = "4";
                }

                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 6)
                {
                    IndNoOfOts = "5";
                }

                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 7)
                {
                    IndNoOfOts = "6";
                }

                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 8)
                {
                    IndNoOfOts = "7";
                }
                else if (ddlIndNoOfOtsPaysheet.SelectedIndex == 9)
                {
                    IndNoOfOts = "8";
                }
                else if (ddlIndNoOfOtsPaysheet.SelectedIndex > 9)
                {
                    IndNoOfOts = ddlIndNoOfOtsPaysheet.SelectedValue;
                }


                string IndNoOfDays = "0";

                if (ddlIndNoOfDays.SelectedIndex == 0)
                {
                    IndNoOfDays = "99";
                }
                else if (ddlIndNoOfDays.SelectedIndex == 1)
                {
                    IndNoOfDays = "0";
                }
                else if (ddlIndNoOfDays.SelectedIndex == 2)
                {
                    IndNoOfDays = "1";
                }
                else if (ddlIndNoOfDays.SelectedIndex == 3)
                {
                    IndNoOfDays = "2";
                }
                else if (ddlIndNoOfDays.SelectedIndex == 4)
                {
                    IndNoOfDays = "3";
                }

                else if (ddlIndNoOfDays.SelectedIndex == 5)
                {
                    IndNoOfDays = "4";
                }

                else if (ddlIndNoOfDays.SelectedIndex == 6)
                {
                    IndNoOfDays = "5";
                }

                else if (ddlIndNoOfDays.SelectedIndex == 7)
                {
                    IndNoOfDays = "6";
                }

                else if (ddlIndNoOfDays.SelectedIndex == 8)
                {
                    IndNoOfDays = "7";
                }
                else if (ddlIndNoOfDays.SelectedIndex == 9)
                {
                    IndNoOfDays = "8";
                }
                else if (ddlIndNoOfDays.SelectedIndex > 9)
                {
                    IndNoOfDays = ddlIndNoOfDays.SelectedValue;
                }

                #endregion
                #region Gst
                var CGST = 0;
                var SGST = 0;
                var IGST = 0;
                var Cess1 = 0;
                var Cess2 = 0;
                var GSTLineItem = 0;
                var GSTSplCheck = "N";
                var GSTSplPer = "0";
                #endregion Gst

                #region for inv headings

                var InvDescription = "";
                var InvNoOfEmps = "";
                var InvNoofDuties = "";
                var InvPayrate = "";
                var InvAmount = "";
                var InvMonthDays = "";
                var InvSacCode = "";
                var InvDescriptionVisibility = "N";
                var InvNoOfEmpsVisibility = "N";
                var InvNoofDutiesVisibility = "N";
                var InvPayrateVisibility = "N";
                var InvAmountVisibility = "N";

                var InvMonthDaysVisibility = "N";
                var InvSacCodeVisibility = "N";

                #endregion

                #region Begin code For Stored Procedure related Variables declaration as on [18-10-2013]
                Hashtable HtContracts = new Hashtable();
                string SPName = "";
                var IRecordStatus = 0;
                #endregion  End code For Stored Procedure related Variables declaration as on [18-10-2013]

                #endregion End  Code For Variable Declaration Which is Stored into Contracts Table as on [18-10-2013]

                #endregion End Code For Declaring Variables as on [18-10-2013]

                #region  Begin Code For Assign Values to The Variables as on [18-10-2013]

                #region Begin Code For A-V (1) to (5)  Ref : ClientId To ConractID
                ClientId = ddlclientid.SelectedValue;
                //ContractStartDate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                ContractStartDate = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                //ContractEndDate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                ContractEndDate = Timings.Instance.CheckDateFormat(txtEndingDate.Text);

                BGAmount = txtBGAmount.Text;
                ContractId = txtcontractid.Text;

                #endregion End Code For A-V (1) to (5)  Ref :ClientID To ContractID

                #region Begin Code For A-V (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month
                SecurityDeposit = txtSecurityDeposit.Text;


                TypeOfWork = ddltypeofwork.SelectedValue;




                MaterialCostPerMonth = txtMaterial.Text;
                if (txtValidityDate.Text.Trim().Length != 0)
                {
                    //ValidityDate = DateTime.Parse(txtValidityDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    ValidityDate = Timings.Instance.CheckDateFormat(txtValidityDate.Text);

                }

                MachinaryCostPerMonth = txtMachinary.Text;

                #endregion End Code For A-V (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month

                #region Begin Code For A-V (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                EMDValue = txtEMDValue.Text;
                if (RadioLumpsum.Checked)
                {
                    PaymentType = "1";
                    if (txtLumpsumtext.Text.Trim().Length > 0)
                    {
                        Lumpsumtext = txtLumpsumtext.Text;
                    }
                }

                WageAct = txtWAWA.Text;
                if (RadioLumpsum.Checked)
                    PayLumpsum = txtlampsum.Text.Trim();
                PerformanceGuaranty = txtPerGurante.Text;

                #endregion End Code For A-V (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                #region Begin Code For A-V   Ref : Ref PF To ESI On OT

                if (TxtPf.Text.Trim().Length > 0)
                {
                    PF = TxtPf.Text;
                }

                PFFrom = DdlPf.SelectedIndex;

                //if (checkPFonOT.Checked)
                //    PFonOT = 1;

                //new code for pt on check boxes 

                if (ChkpfonBasic.Checked)
                    pfonBasic = 1;

                if (ChkpfonDA.Checked)
                    pfonDA = 1;

                if (ChkpfonHRA.Checked)
                    pfonHRA = 1;

                if (ChkpfonConv.Checked)
                    pfonConv = 1;

                if (ChkpfonMedicalAllw.Checked)
                    pfonMedicalAllw = 1;



                if (ChkpfonFoodAllw.Checked)
                    pfonFoodAllw = 1;

                if (ChkpfonSiteAllw.Checked)
                    pfonSiteAllw = 1;

                if (ChkpfonSpecialAllw.Checked)
                    pfonSpecialAllw = 1;

                if (ChkpfonOA.Checked)
                    pfonOA = 1;

                if (ChkpfonWA.Checked)
                    pfonWA = 1;

                if (ChkpfonGunAllw.Checked)
                    pfonGunAllw = 1;

                if (ChkpfonFireAllw.Checked)
                    pfonFireAllw = 1;

                if (ChkpfonBonus.Checked)
                    pfonBonus = 1;

                if (ChkpfonGratuity.Checked)
                    pfonGratuity = 1;


                if (ChkpfonLA.Checked)
                    pfonLA = 1;

                if (ChkpfonOTRate.Checked)
                    pfonOTRate = 1;

                if (ChkpfonESIPayrate.Checked)
                    pfonESIPayrate = 1;

                if (ChkpfonPFPayrate.Checked)
                    pfonPFPayrate = 1;

                if (ChkpfonCCA.Checked)
                    pfonCCA = 1;

                //if (ChkpfonNFHS.Checked)
                //    pfonNFHS = 1;

                if (ChkpfonRC.Checked)
                    pfonRC = 1;

                if (ChkpfonPLAmount.Checked)
                    pfonPLAmount = 1;

                if (ChkpfonTLAmount.Checked)
                    pfonTLAmount = 1;

                if (ChkpfonServicecharge.Checked)
                    pfonServicecharge = 1;

                if (ChkpfonNHSRate.Checked)
                    pfonNHSRate = 1;

                if (ChkpfonWoRate.Checked)
                    pfonWoRate = 1;


                if (ChkpfonTravelAllw.Checked)
                    pfonTravelAllw = 1;

                if (ChkpfonOTHrs.Checked)
                    pfonOTHrs = 1;

                if (ChkpfonPerformanceAllw.Checked)
                    pfonPerformanceAllw = 1;

                if (ChkpfonMobileAllw.Checked)
                    pfonMobileAllw = 1;

                if (ChkpfonUniformCharges.Checked)
                    pfonUniformCharges = 1;

                if (ChkpfonServiceWeightage.Checked)
                    pfonServiceWeightage = 1;


                //new code End  for pt on check boxes 


                //new code for pt on check boxes 

                if (ChkESIonBasic.Checked)
                    ESIonBasic = 1;

                if (ChkESIonDA.Checked)
                    ESIonDA = 1;

                if (ChkESIonHRA.Checked)
                    ESIonHRA = 1;

                if (ChkESIonConv.Checked)
                    ESIonConv = 1;

                if (ChkESIonMedicalAllw.Checked)
                    ESIonMedicalAllw = 1;



                if (ChkESIonFoodAllw.Checked)
                    ESIonFoodAllw = 1;

                if (ChkESIonSiteAllw.Checked)
                    ESIonSiteAllw = 1;

                if (ChkESIonSpecialAllw.Checked)
                    ESIonSpecialAllw = 1;

                if (ChkESIonOA.Checked)
                    ESIonOA = 1;

                if (ChkESIonWA.Checked)
                    ESIonWA = 1;

                if (ChkESIonGunAllw.Checked)
                    ESIonGunAllw = 1;

                if (ChkESIonFireAllw.Checked)
                    ESIonFireAllw = 1;

                if (ChkESIonBonus.Checked)
                    ESIonBonus = 1;

                if (ChkESIonGratuity.Checked)
                    ESIonGratuity = 1;


                if (ChkESIonLA.Checked)
                    ESIonLA = 1;

                if (ChkESIonOTRate.Checked)
                    ESIonOTRate = 1;

                if (ChkESIonESIPayrate.Checked)
                    ESIonESIPayrate = 1;

                if (ChkESIonPFPayrate.Checked)
                    ESIonPFPayrate = 1;

                if (ChkESIonCCA.Checked)
                    ESIonCCA = 1;

                //if (ChkESIonNFHS.Checked)
                //    ESIonNFHS = 1;

                if (ChkESIonRC.Checked)
                    ESIonRC = 1;

                if (ChkESIonPLAmount.Checked)
                    ESIonPLAmount = 1;

                if (ChkESIonTLAmount.Checked)
                    ESIonTLAmount = 1;

                if (ChkESIonServicecharge.Checked)
                    ESIonServicecharge = 1;

                if (ChkESIonNHSRate.Checked)
                    ESIonNHSRate = 1;

                if (ChkpfonWoRate.Checked)
                    ESIonWoRate = 1;


                if (ChkESIonTravelAllw.Checked)
                    ESIonTravelAllw = 1;

                if (ChkESIonOTHrs.Checked)
                    ESIonOTHrs = 1;

                if (ChkESIonPerformanceAllw.Checked)
                    ESIonPerformanceAllw = 1;

                if (ChkESIonMobileAllw.Checked)
                    ESIonMobileAllw = 1;

                if (ChkESIonUniformCharges.Checked)
                    ESIonUniformCharges = 1;

                if (ChkESIonServiceWeightage.Checked)
                    ESIonServiceWeightage = 1;


                //new code End  for pt on check boxes 


                PFonOT = ddlpfon.SelectedIndex;
                if (TxtEsi.Text.Trim().Length > 0)
                {
                    ESI = TxtEsi.Text;
                }
                ESIFrom = DdlEsi.SelectedIndex;

                // if (checkESIonOT.Checked)
                //    ESIonOt = 1;
                ESIonOt = ddlesion.SelectedIndex;

                if (Chkpf.Checked)
                    Bpf = 1;

                if (ChkEsi.Checked)
                    Besi = 1;

                if (txtPfLimit.Text.Trim().Length > 0)
                    Pflimit = txtPfLimit.Text;
                if (txtEsiLimit.Text.Trim().Length > 0)
                    Esilimit = txtEsiLimit.Text;
                if (chkrc.Checked)
                    RelChrg = 1;


                #endregion End Code For A-V   Ref : Ref PF To ESI On OT

                #region Begin Code For A-V   Ref : Ref  ServiceChargeType  To SProfTax

                if (radioyes.Checked)
                {
                    if (RadioPercent.Checked)
                    {
                        ServiceCharge = txtservicecharge.Text.Trim();
                        ServiceChargeType = 0;
                        ServiceDesc = txtservicedesc.Text.Trim();
                    }
                    else
                    {
                        ServiceCharge = txtservicecharge.Text.Trim();
                        ServiceChargeType = 1;
                        ServiceDesc = txtservicedesc.Text.Trim();
                    }

                    if (chkStaxonservicecharge.Checked)
                    {
                        Staxonservicecharge = 1;
                    }

                    if (chksconpfesi.Checked)
                    {
                        SCOnPFESIEmpr = 1;

                    }

                }

                //if (RadioStartDate.Checked == true)
                //    BillDates = 1;
                BillDates = ddlbilldates.SelectedIndex;
                PaySheetDates = ddlPaySheetDates.SelectedIndex;

                if (RadioCompany.Checked)
                {
                    WageType = 0;
                }
                else if (RadioClient.Checked)
                {
                    WageType = 1;
                }
                else if (RadioSpecial.Checked)
                {
                    WageType = 2;
                }
                else if (RadioIndividual.Checked)
                {
                    WageType = 3;
                }
                else if (RadioBoth.Checked)
                {
                    WageType = 4;
                }

                if (chkProfTax.Checked)
                    ProfTax = 1;

                if (chkspt.Checked)
                    SProfTax = 1;

                #endregion End Code For A-V   Ref : Ref  ServiceChargeType  To SProfTax

                #region Begin Code For A-V   Ref : Ref  ServiceTaxType  To TL No

                if (RadioWithoutST.Checked)
                {
                    ServiceTaxType = 1;
                }

                if (CheckIncludeST.Checked)
                {
                    IncludeST = 1;
                }

                if (Check75ST.Checked)
                {
                    ServiceTax75 = 1;
                }

                if (DdlOt.SelectedIndex == 0)
                    OTPersent = 100;
                else
                    OTPersent = 200;

                OWF = txtOWF.Text;
                AdminCharges = txtadmincharges.Text;
                if (radiootspecial.Checked)
                {
                    OTAmounttype = 1;
                }
                Description = txtdescription.Text.Trim();
                ContractDescription = TxtContractDescription.Text;

                if (chkotsalaryrate.Checked)
                {
                    otsalaryratecheck = 1;
                    otsalaryrat = txtotsalaryrate.Text;
                }
                if (chkojt.Checked)
                {
                    ojt = 1;
                }
                if (ChkPFEmpr.Checked)
                {
                    PFEmpr = 1;
                }
                if (ChkESIEmpr.Checked)
                {
                    ESIEmpr = 1;
                }

                if (chktl.Checked)
                {
                    TL = 1;
                    TLNo = txttlamt.Text;
                }


                #endregion End Code For A-V   Ref : Ref  ServiceTaxType  To Description


                #region Begin Code For A-V   Ref : Ref  Tds  To Expect date on 29/03/2014 by venkat

                if (txtTds.Text.Trim().Length > 0)
                {
                    Tds = int.Parse(txtTds.Text);
                }

                if (txtPono.Text.Trim().Length > 0)
                {
                    Pono = txtPono.Text;
                }
                //PoDate = Timings.Instance.CheckDateFormat(txtPoDate.Text);
                if (txtPoDate.Text.Trim().Length > 0)
                {
                    PoDate = txtPoDate.Text;

                }

                if (txtExpectdateofreceipt.Text.Trim().Length > 0)
                {
                    ReceiptExpectedDate = int.Parse(txtExpectdateofreceipt.Text);
                }

                TDSon = ddlTDSon.SelectedIndex;

                #endregion End Code For A-V   Ref : Ref   Tds  To Expecte date

                #region Begin Code for Esi branche adding as on 02/08/2014

                if (ddlEsibranch.SelectedIndex > 0)
                {
                    Esibranch = ddlEsibranch.SelectedValue;
                }
                string PFBranch = "";
                if (ddlPFbranch.SelectedIndex > 0)
                {
                    PFBranch = ddlPFbranch.SelectedValue;
                }

                if (Chkpdfs.Checked)
                {

                    pdfs = 1;
                }
                if (ChkWithRoundoff.Checked)
                {

                    WithRoundoff = 1;
                }

                if (ChkbillfromPaysheetduties.Checked)
                {

                    BillFromPayhsheetDuties = 1;
                }

                if (chkNoUnif.Checked == true)
                {
                    NoUniform = 1;
                }
                else
                {
                    NoUniform = 0;
                }
                if (chkNoSal.Checked == true)
                {
                    NoSalAdv = 1;
                }
                else
                {
                    NoSalAdv = 0;
                }

                if (chkNoLWF.Checked == true)
                {
                    NoLWF = 1;
                }
                else
                {
                    NoLWF = 0;
                }

                if (chkNoRegFee.Checked == true)
                {
                    NoRegFee = 1;
                }
                else
                {
                    NoRegFee = 0;
                }

                WagesCalnOn = ddlWagesCalnOn.SelectedIndex;
                PTOn = ddlPTOn.SelectedIndex;
                #endregion

                #region for Gst


                if (RdbSGST.Checked)
                {
                    SGST = 1;
                    CGST = 1;
                }

                if (RdbIGST.Checked)
                {
                    IGST = 1;
                }

                if (chkCess1.Checked)
                {
                    Cess1 = 1;
                }

                if (chkCess2.Checked)
                {
                    Cess2 = 1;
                }

                if (chkGSTLineItem.Checked)
                {
                    GSTLineItem = 1;
                }


                if (rdbGSTSplYes.Checked == true)
                {
                    GSTSplCheck = "Y";
                    GSTSplPer = ddlGSTSplPer.SelectedValue;
                }
                else
                {
                    GSTSplCheck = "N";
                    GSTSplPer = "0";
                }



                #endregion For Gst


                #endregion   End  Code For Assign Values to The Variables as on [18-10-2013]

                #region for invoice headings

                InvDescription = TxtInvDescription.Text;
                InvNoOfEmps = txtInvNoofEmployees.Text;
                InvPayrate = txtInvPayrate.Text;
                InvNoofDuties = txtInvNoofDuties.Text;
                InvAmount = txtInvAmount.Text;

                InvMonthDays = txtInvmonthdays.Text;
                InvSacCode = txtInvsaccode.Text;

                if (chkInvDesc.Checked)
                {
                    InvDescriptionVisibility = "Y";
                }

                if (chkInvNoofemp.Checked)
                {
                    InvNoOfEmpsVisibility = "Y";
                }

                if (chkInvNoofduties.Checked)
                {
                    InvNoofDutiesVisibility = "Y";
                }

                if (chkInvPayrate.Checked)
                {
                    InvPayrateVisibility = "Y";
                }

                if (chkInvAmount.Checked)
                {
                    InvAmountVisibility = "Y";
                }

                if (chkInvSaccode.Checked)
                {
                    InvSacCodeVisibility = "Y";
                }

                if (chkInvMonthDays.Checked)
                {
                    InvMonthDaysVisibility = "Y";
                }
                #endregion for invoice headings

                #region for RCM and RCM Text
                if (chkrcm.Checked)
                    RCM = 1;
                RCMText = txtrcmtext.Text;

                #endregion for RCM

                #region Begin Code For Hash Table/Sp Parameters As on [18-10-2013]
                #region  Begin Code For H-S-Parameters   (1) to (5)  : Ref Clientid To ContractID

                HtContracts.Add("@ClientId", ClientId);
                HtContracts.Add("@ContractStartDate", ContractStartDate);
                HtContracts.Add("@ContractEndDate", ContractEndDate);
                HtContracts.Add("@BGAmount", BGAmount);
                HtContracts.Add("@ContractId", ContractId);

                #endregion  End Code For  H-S-Parameters   (1) to (5)  : Ref Clientid To ContractID

                #region  Begin Code For H-S-Parameters    (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                HtContracts.Add("@SecurityDeposit", SecurityDeposit);
                HtContracts.Add("@TypeOfWork", TypeOfWork);
                HtContracts.Add("@MaterialCostPerMonth", MaterialCostPerMonth);
                HtContracts.Add("@ValidityDate", ValidityDate);
                HtContracts.Add("@MachinaryCostPerMonth", MachinaryCostPerMonth);

                #endregion  End Code For H-S-Parameters    (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                #region  Begin Code For H-S-Parameters    (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                HtContracts.Add("@EMDValue", EMDValue);
                HtContracts.Add("@PaymentType", PaymentType);
                HtContracts.Add("@WageAct", WageAct);
                HtContracts.Add("@PayLumpsum", PayLumpsum);
                HtContracts.Add("@PerformanceGuaranty", PerformanceGuaranty);

                #endregion  End Code For H-S-Parameters    (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                #region  Begin Code For H-S-Parameters     : Ref PF To ESI On OT

                HtContracts.Add("@PF", PF);
                HtContracts.Add("@PFFrom", PFFrom);
                HtContracts.Add("@PFonOT", PFonOT);
                HtContracts.Add("@ESI", ESI);
                HtContracts.Add("@ESIFrom", ESIFrom);
                HtContracts.Add("@ESIonOt", ESIonOt);


                #endregion  End Code For H-S-Parameters      : Ref PF To ESI On OT

                #region For Code For H-S-Parameters      : Ref PFOn To ESI On

                HtContracts.Add("@pfonBasic", pfonBasic);
                HtContracts.Add("@pfonDA", pfonDA);
                HtContracts.Add("@pfonHRA", pfonHRA);
                HtContracts.Add("@pfonConv", pfonConv);
                HtContracts.Add("@pfonMedicalAllw", pfonMedicalAllw);
                HtContracts.Add("@pfonFoodAllw", pfonFoodAllw);
                HtContracts.Add("@pfonSiteAllw", pfonSiteAllw);
                HtContracts.Add("@pfonSpecialAllw", pfonSpecialAllw);
                HtContracts.Add("@pfonOA", pfonOA);
                HtContracts.Add("@pfonWA", pfonWA);
                HtContracts.Add("@pfonGunAllw", pfonGunAllw);
                HtContracts.Add("@pfonFireAllw", pfonFireAllw);
                HtContracts.Add("@pfonBonus", pfonBonus);
                HtContracts.Add("@pfonGratuity", pfonGratuity);
                HtContracts.Add("@pfonLA", pfonLA);
                HtContracts.Add("@pfonOTRate", pfonOTRate);
                HtContracts.Add("@pfonESIPayrate", pfonESIPayrate);
                HtContracts.Add("@pfonPFPayrate", pfonPFPayrate);
                HtContracts.Add("@pfonCCA", pfonCCA);
                HtContracts.Add("@pfonNFHS", pfonNFHS);
                HtContracts.Add("@pfonRC", pfonRC);
                HtContracts.Add("@pfonPLAmount", pfonPLAmount);
                HtContracts.Add("@pfonTLAmount", pfonTLAmount);
                HtContracts.Add("@pfonServicecharge", pfonServicecharge);
                HtContracts.Add("@pfonNHSRate", pfonNHSRate);
                HtContracts.Add("@pfonWoRate", pfonWoRate);
                HtContracts.Add("@pfonTravelAllw", pfonTravelAllw);
                HtContracts.Add("@pfonOTHrs", pfonOTHrs);
                HtContracts.Add("@pfonPerformanceAllw", pfonPerformanceAllw);
                HtContracts.Add("@pfonMobileAllw", pfonMobileAllw);
                HtContracts.Add("@pfonUniformCharges", pfonUniformCharges);
                HtContracts.Add("@pfonServiceWeightage", pfonServiceWeightage);


                HtContracts.Add("@ESIonBasic", ESIonBasic);
                HtContracts.Add("@ESIonDA", ESIonDA);
                HtContracts.Add("@ESIonHRA", ESIonHRA);
                HtContracts.Add("@ESIonConv", ESIonConv);
                HtContracts.Add("@ESIonMedicalAllw", ESIonMedicalAllw);
                HtContracts.Add("@ESIonFoodAllw", ESIonFoodAllw);
                HtContracts.Add("@ESIonSiteAllw", ESIonSiteAllw);
                HtContracts.Add("@ESIonSpecialAllw", ESIonSpecialAllw);
                HtContracts.Add("@ESIonOA", ESIonOA);
                HtContracts.Add("@ESIonWA", ESIonWA);
                HtContracts.Add("@ESIonGunAllw", ESIonGunAllw);
                HtContracts.Add("@ESIonFireAllw", ESIonFireAllw);
                HtContracts.Add("@ESIonBonus", ESIonBonus);
                HtContracts.Add("@ESIonGratuity", ESIonGratuity);
                HtContracts.Add("@ESIonLA", ESIonLA);
                HtContracts.Add("@ESIonOTRate", ESIonOTRate);
                HtContracts.Add("@ESIonESIPayrate", ESIonESIPayrate);
                HtContracts.Add("@ESIonPFPayrate", ESIonPFPayrate);
                HtContracts.Add("@ESIonCCA", ESIonCCA);
                HtContracts.Add("@ESIonNFHS", ESIonNFHS);
                HtContracts.Add("@ESIonRC", ESIonRC);
                HtContracts.Add("@ESIonPLAmount", ESIonPLAmount);
                HtContracts.Add("@ESIonTLAmount", ESIonTLAmount);
                HtContracts.Add("@ESIonServicecharge", ESIonServicecharge);
                HtContracts.Add("@ESIonNHSRate", ESIonNHSRate);
                HtContracts.Add("@ESIonWoRate", ESIonWoRate);
                HtContracts.Add("@ESIonTravelAllw", ESIonTravelAllw);
                HtContracts.Add("@ESIonOTHrs", ESIonOTHrs);
                HtContracts.Add("@ESIonPerformanceAllw", ESIonPerformanceAllw);
                HtContracts.Add("@ESIonMobileAllw", ESIonMobileAllw);
                HtContracts.Add("@ESIonUniformCharges", ESIonUniformCharges);
                HtContracts.Add("@ESIonServiceWeightage", ESIonServiceWeightage);

                #endregion For Code For H-S-Parameters      : Ref PFOn To ESI On

                #region    Begin Code For H-S-Parameters     : Ref    Servicharge  Yes To   Service Chagre Amount

                HtContracts.Add("@ServiceChargeType", ServiceChargeType);
                HtContracts.Add("@ServiceCharge", ServiceCharge);
                HtContracts.Add("@ServiceChargeText", ServiceDesc);
                HtContracts.Add("@BillDates", BillDates);
                HtContracts.Add("@PaySheetDate", PaySheetDates);
                HtContracts.Add("@WageType", WageType);
                HtContracts.Add("@ProfTax", ProfTax);
                HtContracts.Add("@SProfTax", SProfTax);

                #endregion   End  Code For   H-S-Parameters    : Ref    Servicharge  Yes To   Service Chagre Amount


                #region    Begin Code For H-S-Parameters    : Ref    ServiceTaxType To   TL No

                HtContracts.Add("@ServiceTaxType", ServiceTaxType);
                HtContracts.Add("@IncludeST", IncludeST);
                HtContracts.Add("@ServiceTax75", ServiceTax75);
                HtContracts.Add("@OTPersent", OTPersent);
                HtContracts.Add("@OWF", OWF);
                HtContracts.Add("@AdminCharges", AdminCharges);
                HtContracts.Add("@OTAmounttype", OTAmounttype);
                HtContracts.Add("@Description", Description);
                HtContracts.Add("@ContractDescription", ContractDescription);
                HtContracts.Add("@otsalaryratecheck", otsalaryratecheck);
                HtContracts.Add("@otsalaryrat", otsalaryrat);
                HtContracts.Add("@ojt", ojt);
                HtContracts.Add("@PFEmpr", PFEmpr);
                HtContracts.Add("@ESIEmpr", ESIEmpr);
                HtContracts.Add("@tl", TL);
                HtContracts.Add("@tlno", TLNo);
                HtContracts.Add("@PFLimit", Pflimit);
                HtContracts.Add("@ESILimit", Esilimit);
                HtContracts.Add("@Bpf", Bpf);
                HtContracts.Add("@Besi", Besi);
                HtContracts.Add("@RelChrg", RelChrg);

                #endregion   End  Code For H-S-Parameters    : Ref       ServiceTaxType To   Description


                #region    Begin Code For H-S-Parameters    : Ref    Tds To   Lumpsumtext on 29/03/2014 by venkat

                HtContracts.Add("@Tds", Tds);
                HtContracts.Add("@TDSon", TDSon);
                HtContracts.Add("@Pono", Pono);
                HtContracts.Add("@PoDate", PoDate);
                HtContracts.Add("@ReceiptExpectedDate", ReceiptExpectedDate);
                HtContracts.Add("@Staxonservicecharge", Staxonservicecharge);
                HtContracts.Add("@SCOnPFESIEmpr", SCOnPFESIEmpr);
                HtContracts.Add("@Lumpsumtext", Lumpsumtext);

                #endregion   End  Code For H-S-Parameters    : Ref        Tds To   Lumpsumtext on 29/03/2014 by venkat


                HtContracts.Add("@Esibranch", Esibranch);
                HtContracts.Add("@PFBranch", PFBranch);

                HtContracts.Add("@pdfs", pdfs);
                HtContracts.Add("@WithRoundoff", WithRoundoff);
                HtContracts.Add("@BillFromPayhsheetDuties", BillFromPayhsheetDuties);
                HtContracts.Add("@NoUniform", NoUniform);
                HtContracts.Add("@NoSalAdv", NoSalAdv);
                HtContracts.Add("@NoLWF", NoLWF);
                HtContracts.Add("@NoRegFee", NoRegFee);
                HtContracts.Add("@CGST", CGST);
                HtContracts.Add("@IGST", IGST);
                HtContracts.Add("@SGST", SGST);
                HtContracts.Add("@Cess1", Cess1);
                HtContracts.Add("@Cess2", Cess2);
                HtContracts.Add("@GSTLineItem", GSTLineItem);
                HtContracts.Add("@WagesCalnOn", WagesCalnOn);
                HtContracts.Add("@PTOn", PTOn);
                HtContracts.Add("@IndNoOfOts", IndNoOfOts);
                HtContracts.Add("@IndNoOfDays", IndNoOfDays);
                HtContracts.Add("@GSTSplCheck", GSTSplCheck);
                HtContracts.Add("@GSTSplPer", GSTSplPer);


                #endregion  End  Code For Hash Table/Sp Parameters As on [18-10-2013]


                #region for inv headings

                HtContracts.Add("@InvDescription", InvDescription);
                HtContracts.Add("@InvNoOfEmps", InvNoOfEmps);
                HtContracts.Add("@InvPayrate", InvPayrate);
                HtContracts.Add("@InvNoofDuties", InvNoofDuties);
                HtContracts.Add("@InvAmount", InvAmount);
                HtContracts.Add("@InvSacCode", InvSacCode);
                HtContracts.Add("@InvMonthDays", InvMonthDays);
                HtContracts.Add("@InvDescriptionVisibility", InvDescriptionVisibility);
                HtContracts.Add("@InvNoOfEmpsVisibility", InvNoOfEmpsVisibility);
                HtContracts.Add("@InvPayratevisibility", InvPayrateVisibility);
                HtContracts.Add("@InvNoofDutiesvisibility", InvNoofDutiesVisibility);
                HtContracts.Add("@InvAmountvisibility", InvAmountVisibility);
                HtContracts.Add("@InvSACCodevisibility", InvSacCodeVisibility);
                HtContracts.Add("@InvMonthDaysvisibility", InvMonthDaysVisibility);
                HtContracts.Add("@RCM", RCM);
                HtContracts.Add("@RCMText", RCMText);

                #endregion

                #region Begin Code For Calling Stored Procedure as on [18-10-2013]
                SPName = "AddorModifyContracts";
                IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                #endregion End Code For Calling Stored Procedure as on [18-10-2013]

                addInvoicedetails(ClientId, ContractId);

                calculatevalues();

                addSplWagesdetails(ClientId, ContractId);

                if (IRecordStatus != 0)
                {
                    contractidautogenrate();
                    clearcontractdetails();
                    lblSuc.Text = "Contract Added Successfully";
                    ClearDataFromThePage();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void contractidautogenrate()
        {
            txtcontractid.Text = GlobalData.Instance.LoadMaxContractId(ddlclientid.SelectedValue);
            DataTable DtContractIds = GlobalData.Instance.LoadAllContractIds(ddlclientid.SelectedValue);
            if (DtContractIds.Rows.Count > 0)
            {
                ddlContractids.DataValueField = "Contractid";
                ddlContractids.DataTextField = "Contractid";
                ddlContractids.DataSource = DtContractIds;
                ddlContractids.DataBind();
            }
            ddlContractids.Items.Insert(0, "-Select-");

        }

        //region for clone dropdown() 
        private void CloneContractidautogenrate()
        {
            txtclonecontractid.Text = GlobalData.Instance.LoadMaxContractId(ddlClientidNotincontract.SelectedValue);
            DataTable DtContractIds = GlobalData.Instance.LoadAllContractIds(ddlClientidNotincontract.SelectedValue);

            //if (DtContractIds.Rows.Count > 0)
            //{
            //    ddlContractids.DataValueField = "Contractid";
            //    ddlContractids.DataTextField = "Contractid";
            //    ddlContractids.DataSource = DtContractIds;
            //    ddlContractids.DataBind();
            //}


            //ddlContractids.Items.Insert(0, "-Select-");

        }

        //endregion for clone dropdown() 

        protected void gvdesignation_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
        }

        protected void RadioLumpsum_CheckedChanged1(object sender, EventArgs e)
        {
            if (RadioLumpsum.Checked == true)
            {
                txtlampsum.Visible = true;
                lbllampsum.Visible = true;
                txtLumpsumtext.Visible = true;
                lbllumpsumtext.Visible = true;

            }
            else
            {
                txtlampsum.Visible = false;
                txtLumpsumtext.Visible = false;
                lbllumpsumtext.Visible = false;
            }

        }

        protected void gvdesignation_RowCommand1(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvdesignation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void radioyes_CheckedChanged(object sender, EventArgs e)
        {
            if (radioyes.Checked == true)
            {
                //RadioPercent.Visible = true;
                //RadioAmount.Visible = true;
                //txtservicecharge.Visible = true;
                //chkStaxonservicecharge.Visible = true;
                chkStaxonservicecharge.Checked = true;
                chksconpfesi.Checked = true;

            }
            else
            {
                //RadioPercent.Visible = false;
                //RadioAmount.Visible = false;
                //txtservicecharge.Visible = false;
                //chkStaxonservicecharge.Visible = false;
                chkStaxonservicecharge.Checked = false;
                chksconpfesi.Checked = false;


            }
        }

        protected void clearcontractdetails()
        {
            ddlclientid.SelectedIndex = 0;
            ddlcname.SelectedIndex = 0;
            checkPFonOT.Checked = false;

            //new pfon 
            ChkpfonBasic.Checked = ChkpfonDA.Checked = ChkpfonHRA.Checked = ChkpfonConv.Checked = ChkpfonMedicalAllw.Checked = ChkpfonFoodAllw.Checked = ChkpfonSiteAllw.Checked = ChkpfonSpecialAllw.Checked = ChkpfonOA.Checked = ChkpfonWA.Checked = ChkpfonGunAllw.Checked = ChkpfonFireAllw.Checked = ChkpfonBonus.Checked = ChkpfonGratuity.Checked = ChkpfonLA.Checked = ChkpfonOTRate.Checked = ChkpfonESIPayrate.Checked = ChkpfonPFPayrate.Checked = ChkpfonCCA.Checked /*= ChkpfonNFHS.Checked*/ = ChkpfonRC.Checked = ChkpfonPLAmount.Checked = ChkpfonTLAmount.Checked = ChkpfonServicecharge.Checked = ChkpfonNHSRate.Checked = ChkpfonWoRate.Checked = ChkpfonTravelAllw.Checked = ChkpfonOTHrs.Checked = ChkpfonPerformanceAllw.Checked = ChkpfonMobileAllw.Checked = ChkpfonUniformCharges.Checked = ChkpfonServiceWeightage.Checked = false;

            //new ESIon

            ChkESIonBasic.Checked = ChkESIonDA.Checked = ChkESIonHRA.Checked = ChkESIonConv.Checked = ChkESIonMedicalAllw.Checked =
            ChkESIonFoodAllw.Checked = ChkESIonSiteAllw.Checked = ChkESIonSpecialAllw.Checked = ChkESIonOA.Checked = ChkESIonWA.Checked = ChkESIonGunAllw.Checked = ChkESIonFireAllw.Checked = ChkESIonBonus.Checked = ChkESIonGratuity.Checked = ChkESIonLA.Checked = ChkESIonOTRate.Checked = ChkESIonESIPayrate.Checked = ChkESIonPFPayrate.Checked = ChkESIonCCA.Checked /*= ChkESIonNFHS.Checked*/ = ChkESIonRC.Checked = ChkESIonPLAmount.Checked = ChkESIonTLAmount.Checked = ChkESIonServicecharge.Checked = ChkESIonNHSRate.Checked = ChkESIonWoRate.Checked = ChkESIonTravelAllw.Checked = ChkESIonOTHrs.Checked = ChkESIonPerformanceAllw.Checked = ChkESIonMobileAllw.Checked = ChkESIonUniformCharges.Checked = ChkESIonServiceWeightage.Checked = false;


            checkESIonOT.Checked = false;
            TxtPf.Text = "100";
            TxtEsi.Text = "100";
            RadioCompany.Checked = true;
            RadioWithST.Checked = true;
            radiootregular.Checked = true;
            txtTypeOfWork.Text = "";
            radiono.Checked = true;
            Radio1to1.Checked = true;
            ddlEsibranch.SelectedIndex = 0;
            ddlPFbranch.SelectedIndex = 0;
            chkCGST.Checked = true;
            RdbSGST.Checked = true;
            RdbIGST.Checked = false;
            chkCess1.Checked = false;
            chkCess2.Checked = false;
            chkGSTLineItem.Checked = false;

        }

        protected void GetGridData()
        {
            //GlobalData.Instance.AppendLog("In GetGridDate");

            try
            {
                Session["DataContractsAIndex"] = 0;
                Session["DataContractsAIndexsw"] = 0;

                ClearDataFromThePage();

                gvSWDesignations.Visible = true;
                gvdesignation.Visible = true;
                DateTime today = DateTime.Now.Date;

                #region Begin Old Code as on [19-10-2013]

                //string SqlQry = "Select * from contracts where Clientid='" + ddlclientid.SelectedValue + "' AND ContractStartDate <= '" +
                //    today.ToString("MM/dd/yyyy") + "' AND ContractEndDate>='" + today.ToString("MM/dd/yyyy") + "'";
                //DataTable DtContractsData = SqlHelper.Instance.GetTableByQuery(SqlQry);

                ////GlobalData.Instance.AppendLog("In GetGridDate- After quering for contracts");
                //if (DtContractsData.Rows.Count == 0)
                //{
                //    lblreslt.Text = "There is no valid Contract for the this Client";
                //    contractidautogenrate();
                //    Enable5Rows();
                //    return;
                //}
                #endregion End Old Code As on [19-10-2013]

                #region    Begin Code For Variable Declaration as on [19-10-2013]
                var SPName = "";
                var ClientID = "";
                Hashtable HtContracts = new Hashtable();
                #endregion  Begin Code For Variable Declaration as on [19-10-2013]

                #region    Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]
                SPName = "GetContracts";
                ClientID = ddlclientid.SelectedValue; ;
                #endregion  Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]

                #region Begin Code For Hash Table Parameters as on [19-10-2013]
                HtContracts.Add("@clientid", ClientID);
                HtContracts.Add("@Contractid", ddlContractids.SelectedValue);

                #endregion  End  Code For Hash Table Parameters as on [19-10-2013]

                #region  Begin Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]
                DataTable DtContractsData = config.ExecuteAdaptorAsyncWithParams(SPName, HtContracts).Result;
                #endregion End Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]

                #region  Begin Code Display the Resulted Messages As on [19-10-2013]
                if (DtContractsData.Rows.Count <= 0)
                {
                    lblMsg.Text = "There is no valid Contract for the this Client";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('There is no valid Contract for the this Client');", true);
                    contractidautogenrate();
                    return;
                }

                #endregion  End  Code Display the Resulted Messages As on [19-10-2013]

                #region  Begin Code For Assign Values to The Controls as on [18-10-2013]

                #region Begin Code For V-C (1) to (5)  Ref : ClientId To ConractID
                // ClientId = ddlclientid.SelectedValue;
                txtStartingDate.Text = DateTime.Parse(DtContractsData.Rows[0]["ContractStartDate"].ToString()).ToString("dd/MM/yyyy");
                txtEndingDate.Text = DateTime.Parse(DtContractsData.Rows[0]["ContractEndDate"].ToString()).ToString("dd/MM/yyyy");
                txtBGAmount.Text = DtContractsData.Rows[0]["BGAmount"].ToString();
                txtcontractid.Text = DtContractsData.Rows[0]["ContractId"].ToString();

                #endregion End Code For V-C (1) to (5)  Ref :ClientID To ContractID

                #region Begin Code For V-C (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month
                txtSecurityDeposit.Text = DtContractsData.Rows[0]["SecurityDeposit"].ToString();
                ddltypeofwork.SelectedValue = DtContractsData.Rows[0]["TypeOfWork"].ToString();


                txtMaterial.Text = DtContractsData.Rows[0]["MaterialCostPerMonth"].ToString();
                txtValidityDate.Text = DateTime.Parse(DtContractsData.Rows[0]["ValidityDate"].ToString()).ToString("dd/MM/yyyy");
                if (txtValidityDate.Text == "01/01/1900")
                {
                    txtValidityDate.Text = "";
                }

                txtMachinary.Text = DtContractsData.Rows[0]["MachinaryCostPerMonth"].ToString();

                #endregion End Code For V-C (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month

                #region Begin Code For V-C (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                txtEMDValue.Text = DtContractsData.Rows[0]["EMDValue"].ToString();

                bool PaymentType = bool.Parse(DtContractsData.Rows[0]["PaymentType"].ToString());
                if (PaymentType == false)
                {
                    RadioLumpsum.Checked = false;
                    RadioManPower.Checked = true;
                    txtlampsum.Visible = false;
                    txtLumpsumtext.Visible = false;
                    lbllumpsumtext.Visible = false;
                }
                else
                {
                    RadioManPower.Checked = false;
                    RadioLumpsum.Checked = true;
                    txtlampsum.Visible = true;
                    txtlampsum.Text = DtContractsData.Rows[0]["PayLumpsum"].ToString();
                    txtLumpsumtext.Visible = true;
                    lbllumpsumtext.Visible = true;
                    txtLumpsumtext.Text = DtContractsData.Rows[0]["Lumpsumtext"].ToString();
                }

                txtWAWA.Text = DtContractsData.Rows[0]["WageAct"].ToString();
                txtPerGurante.Text = DtContractsData.Rows[0]["PerformanceGuaranty"].ToString();

                #endregion End Code For V-C(11) to (15)  Ref : EMD Value To PerFormance Guarantee

                #region Begin Code For V-C  Ref : Ref PF To ESI On OT

                TxtPf.Text = DtContractsData.Rows[0]["PF"].ToString();
                DdlPf.SelectedIndex = int.Parse(DtContractsData.Rows[0]["PFfrom"].ToString());
                //checkPFonOT.Checked = bool.Parse(DtContractsData.Rows[0]["PFonOT"].ToString());



                ddlpfon.SelectedIndex = int.Parse(DtContractsData.Rows[0]["PFonOT"].ToString());

                TxtEsi.Text = DtContractsData.Rows[0]["ESI"].ToString();
                DdlEsi.SelectedIndex = int.Parse(DtContractsData.Rows[0]["ESIfrom"].ToString());
                //checkESIonOT.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonOT"].ToString());
                ddlesion.SelectedIndex = int.Parse(DtContractsData.Rows[0]["ESIonOT"].ToString());
                txtPfLimit.Text = DtContractsData.Rows[0]["Pflimit"].ToString();
                txtEsiLimit.Text = DtContractsData.Rows[0]["Esilimit"].ToString();
                Chkpf.Checked = bool.Parse(DtContractsData.Rows[0]["Bpf"].ToString());
                ChkEsi.Checked = bool.Parse(DtContractsData.Rows[0]["Besi"].ToString());
                chkrc.Checked = bool.Parse(DtContractsData.Rows[0]["RelChrg"].ToString());
                #endregion End Code For V-c   Ref : Ref PF To ESI On OT
                #region for RCM

                bool RCM = bool.Parse(DtContractsData.Rows[0]["RCM"].ToString());
                txtrcmtext.Text = DtContractsData.Rows[0]["RCMText"].ToString();
                if (RCM == true)
                {
                    chkrcm.Checked = true;
                    txtrcmtext.Visible = true;
                }
                else
                {
                    chkrcm.Checked = false;
                    txtrcmtext.Visible = false;
                }

                #endregion
                //new code for PFon and Esion

                ChkpfonBasic.Checked = bool.Parse(DtContractsData.Rows[0]["pfonBasic"].ToString());
                ChkpfonDA.Checked = bool.Parse(DtContractsData.Rows[0]["pfonDA"].ToString());
                ChkpfonHRA.Checked = bool.Parse(DtContractsData.Rows[0]["pfonHRA"].ToString());
                ChkpfonConv.Checked = bool.Parse(DtContractsData.Rows[0]["pfonConv"].ToString());
                ChkpfonMedicalAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonMedicalAllw"].ToString());
                ChkpfonFoodAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonFoodAllw"].ToString());
                ChkpfonSiteAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonSiteAllw"].ToString());
                ChkpfonSpecialAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonSpecialAllw"].ToString());
                ChkpfonOA.Checked = bool.Parse(DtContractsData.Rows[0]["pfonOA"].ToString());
                ChkpfonWA.Checked = bool.Parse(DtContractsData.Rows[0]["pfonWA"].ToString());
                ChkpfonGunAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonGunAllw"].ToString());
                ChkpfonFireAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonFireAllw"].ToString());
                ChkpfonBonus.Checked = bool.Parse(DtContractsData.Rows[0]["pfonBonus"].ToString());
                ChkpfonGratuity.Checked = bool.Parse(DtContractsData.Rows[0]["pfonGratuity"].ToString());
                ChkpfonLA.Checked = bool.Parse(DtContractsData.Rows[0]["pfonLA"].ToString());
                ChkpfonOTRate.Checked = bool.Parse(DtContractsData.Rows[0]["pfonOTRate"].ToString());
                ChkpfonESIPayrate.Checked = bool.Parse(DtContractsData.Rows[0]["pfonESIPayrate"].ToString());
                ChkpfonPFPayrate.Checked = bool.Parse(DtContractsData.Rows[0]["pfonPFPayrate"].ToString());
                ChkpfonCCA.Checked = bool.Parse(DtContractsData.Rows[0]["pfonCCA"].ToString());
                //ChkpfonNFHS.Checked = bool.Parse(DtContractsData.Rows[0]["pfonNFHS"].ToString());
                ChkpfonRC.Checked = bool.Parse(DtContractsData.Rows[0]["pfonRC"].ToString());
                ChkpfonPLAmount.Checked = bool.Parse(DtContractsData.Rows[0]["pfonPLAmount"].ToString());
                ChkpfonTLAmount.Checked = bool.Parse(DtContractsData.Rows[0]["pfonTLAmount"].ToString());
                ChkpfonServicecharge.Checked = bool.Parse(DtContractsData.Rows[0]["pfonServicecharge"].ToString());
                ChkpfonNHSRate.Checked = bool.Parse(DtContractsData.Rows[0]["pfonNHSRate"].ToString());
                ChkpfonWoRate.Checked = bool.Parse(DtContractsData.Rows[0]["pfonWoRate"].ToString());
                ChkpfonTravelAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonTravelAllw"].ToString());
                ChkpfonOTHrs.Checked = bool.Parse(DtContractsData.Rows[0]["pfonOTHrs"].ToString());
                ChkpfonPerformanceAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonPerformanceAllw"].ToString());
                ChkpfonMobileAllw.Checked = bool.Parse(DtContractsData.Rows[0]["pfonMobileAllw"].ToString());
                ChkpfonUniformCharges.Checked = bool.Parse(DtContractsData.Rows[0]["pfonUniformCharges"].ToString());
                ChkpfonServiceWeightage.Checked = bool.Parse(DtContractsData.Rows[0]["pfonServiceWeightage"].ToString());


                ChkESIonBasic.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonBasic"].ToString());
                ChkESIonDA.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonDA"].ToString());
                ChkESIonHRA.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonHRA"].ToString());
                ChkESIonConv.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonConv"].ToString());
                ChkESIonMedicalAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonMedicalAllw"].ToString());
                ChkESIonFoodAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonFoodAllw"].ToString());
                ChkESIonSiteAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonSiteAllw"].ToString());
                ChkESIonSpecialAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonSpecialAllw"].ToString());
                ChkESIonOA.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonOA"].ToString());
                ChkESIonWA.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonWA"].ToString());
                ChkESIonGunAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonGunAllw"].ToString());
                ChkESIonFireAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonFireAllw"].ToString());
                ChkESIonBonus.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonBonus"].ToString());
                ChkESIonGratuity.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonGratuity"].ToString());
                ChkESIonLA.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonLA"].ToString());
                ChkESIonOTRate.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonOTRate"].ToString());
                ChkESIonESIPayrate.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonESIPayrate"].ToString());
                ChkESIonPFPayrate.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonPFPayrate"].ToString());
                ChkESIonCCA.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonCCA"].ToString());
                //ChkESIonNFHS.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonNFHS"].ToString());
                ChkESIonRC.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonRC"].ToString());
                ChkESIonPLAmount.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonPLAmount"].ToString());
                ChkESIonTLAmount.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonTLAmount"].ToString());
                ChkESIonServicecharge.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonServicecharge"].ToString());
                ChkESIonNHSRate.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonNHSRate"].ToString());
                ChkESIonWoRate.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonWoRate"].ToString());
                ChkESIonTravelAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonTravelAllw"].ToString());
                ChkESIonOTHrs.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonOTHrs"].ToString());
                ChkESIonPerformanceAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonPerformanceAllw"].ToString());
                ChkESIonMobileAllw.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonMobileAllw"].ToString());
                ChkESIonUniformCharges.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonUniformCharges"].ToString());
                ChkESIonServiceWeightage.Checked = bool.Parse(DtContractsData.Rows[0]["ESIonServiceWeightage"].ToString());

                //end 

                #region Begin Code For V-C   Ref : Ref  ServiceChargeType  To SProfTax

                txtservicecharge.Text = DtContractsData.Rows[0]["ServiceCharge"].ToString();
                txtservicedesc.Text = DtContractsData.Rows[0]["ServiceChargeDesc"].ToString();
                if (float.Parse(txtservicecharge.Text.Trim().ToString()) == 0)
                {
                    radiono.Checked = true;
                    //RadioPercent.Visible = false;
                    //RadioAmount.Visible = false;
                    txtservicecharge.Text = "0";
                    //txtservicecharge.Visible = false;
                    //txtservicedesc.Visible = false;
                    //lbldesc.Visible = false;
                    // RadioPercent.Checked = true;
                }
                else
                {
                    radiono.Checked = false;
                    //RadioPercent.Visible = true;
                    //RadioAmount.Visible = true;
                    radioyes.Checked = true;
                    //txtservicecharge.Visible = true;
                    //txtservicedesc.Visible = true;
                    //lbldesc.Visible = true;
                    bool ServiceChargeType = false;
                    if (String.IsNullOrEmpty(DtContractsData.Rows[0]["serviceChargeType"].ToString()) != false)
                    {
                        string tempData = DtContractsData.Rows[0]["serviceChargeType"].ToString();
                        if (tempData.Trim().Length > 0)
                        {
                            ServiceChargeType = bool.Parse(tempData);
                        }
                    }
                    else
                    {
                        ServiceChargeType = bool.Parse(DtContractsData.Rows[0]["serviceChargeType"].ToString());
                    }

                    if (ServiceChargeType == true)
                    {
                        RadioAmount.Checked = true;
                    }
                    else
                    {
                        RadioPercent.Checked = true;
                    }
                    //chkStaxonservicecharge.Visible = true;

                    string strStaxonservicecharge = "";

                    if (String.IsNullOrEmpty(DtContractsData.Rows[0]["Staxonservicecharge"].ToString()) == false)
                    {
                        string tempData = DtContractsData.Rows[0]["Staxonservicecharge"].ToString();
                        if (tempData.Trim().Length > 0)
                        {
                            strStaxonservicecharge = tempData;
                        }
                    }
                    else
                    {
                        strStaxonservicecharge = DtContractsData.Rows[0]["serviceChargeType"].ToString();
                    }
                    if (strStaxonservicecharge == "True")
                    {
                        chkStaxonservicecharge.Checked = true;
                    }
                    if (strStaxonservicecharge == "False")
                    {
                        chkStaxonservicecharge.Checked = false;
                    }

                }

                chksconpfesi.Checked = Convert.ToBoolean(DtContractsData.Rows[0]["SCOnPFESIEmpr"].ToString());
                var strBillDate = int.Parse(DtContractsData.Rows[0]["BillDates"].ToString());


                ddlbilldates.SelectedIndex = strBillDate;

                var strPaySheetDates = int.Parse(DtContractsData.Rows[0]["PaySheetDates"].ToString());

                ddlPaySheetDates.SelectedIndex = strPaySheetDates;

                var ojt = bool.Parse(DtContractsData.Rows[0]["ojt"].ToString());
                if (ojt == true)
                {
                    chkojt.Checked = true;
                }

                var PFEmpr = bool.Parse(DtContractsData.Rows[0]["PFEmpr"].ToString());
                if (PFEmpr == true)
                {
                    ChkPFEmpr.Checked = true;
                }

                var ESIEmpr = bool.Parse(DtContractsData.Rows[0]["ESIEmpr"].ToString());
                if (ESIEmpr == true)
                {
                    ChkESIEmpr.Checked = true;
                }



                UInt16 WagesType = 0;
                if (String.IsNullOrEmpty(DtContractsData.Rows[0]["WageType"].ToString()) == false)
                {
                    string tempData = DtContractsData.Rows[0]["WageType"].ToString();
                    if (tempData.Trim().Length > 0)
                    {
                        //WagesType = bool.Parse(tempData);
                        WagesType = Convert.ToUInt16(tempData);
                    }
                }

                if (WagesType == 0)
                {
                    RadioSpecial.Checked = false;
                    RadioClient.Checked = false;
                    RadioIndividual.Checked = false;
                    RadioCompany.Checked = true;
                    SpecialWagesPanel.Visible = false;
                }
                else if (WagesType == 1)
                {
                    RadioSpecial.Checked = false;
                    RadioCompany.Checked = false;
                    RadioClient.Checked = true;
                    RadioIndividual.Checked = false;
                    SpecialWagesPanel.Visible = false;
                }
                else if (WagesType == 2)
                {
                    RadioSpecial.Checked = true;
                    RadioCompany.Checked = false;
                    RadioIndividual.Checked = false;
                    RadioClient.Checked = false;
                    SpecialWagesPanel.Visible = true;
                }
                else if (WagesType == 4)
                {
                    RadioBoth.Checked = true;
                    RadioCompany.Checked = false;
                    RadioIndividual.Checked = false;
                    RadioClient.Checked = false;
                    SpecialWagesPanel.Visible = true;
                }
                else
                {
                    RadioSpecial.Checked = false;
                    RadioCompany.Checked = false;
                    RadioClient.Checked = false;
                    RadioIndividual.Checked = true;
                    SpecialWagesPanel.Visible = false;
                    btnadddesgnsw.Visible = true;
                }
                if (DtContractsData.Rows[0]["ProfTax"].ToString().Length > 0)
                {
                    bool PTaxStatus = Convert.ToBoolean(DtContractsData.Rows[0]["ProfTax"].ToString());
                    chkProfTax.Checked = PTaxStatus;
                }


                if (DtContractsData.Rows[0]["SProfTax"].ToString().Length > 0)
                {
                    bool SPTaxStatus = Convert.ToBoolean(DtContractsData.Rows[0]["SProfTax"].ToString());
                    chkspt.Checked = SPTaxStatus;
                }
                #endregion End Code For V-C   Ref : Ref  ServiceChargeType  To SProfTax

                #region Begin Code For V-C   Ref : Ref  ServiceTaxType  To TL No

                string stType = DtContractsData.Rows[0]["ServiceTaxType"].ToString();
                if (stType.Length > 0)
                {
                    bool bSTType = Convert.ToBoolean(stType);
                    if (bSTType == true)
                    {
                        RadioWithoutST.Checked = true;
                        RadioWithST.Checked = false;
                    }
                    else
                    {
                        RadioWithST.Checked = true;
                        RadioWithoutST.Checked = false;
                    }
                }


                string SGST = DtContractsData.Rows[0]["SGST"].ToString();
                if (SGST == "False")
                {
                    RdbSGST.Checked = false;
                    chkCGST.Checked = false;
                }
                else
                {
                    RdbSGST.Checked = true;
                    chkCGST.Checked = true;
                }

                string IGST = DtContractsData.Rows[0]["IGST"].ToString();
                if (IGST == "False")
                {
                    RdbIGST.Checked = false;
                }
                else
                {
                    RdbIGST.Checked = true;
                }


                string Cess1 = DtContractsData.Rows[0]["Cess1"].ToString();
                if (Cess1 == "False")
                {
                    chkCess1.Checked = false;
                }
                else
                {
                    chkCess1.Checked = true;
                }


                string Cess2 = DtContractsData.Rows[0]["Cess2"].ToString();
                if (Cess2 == "False")
                {
                    chkCess2.Checked = false;
                }
                else
                {
                    chkCess2.Checked = true;
                }
                string GSTLineitem = DtContractsData.Rows[0]["GSTLineitem"].ToString();
                if (GSTLineitem == "False")
                {
                    chkGSTLineItem.Checked = false;
                }
                else
                {
                    chkGSTLineItem.Checked = true;
                }

                string GSTSplCheck = DtContractsData.Rows[0]["GSTSplCheck"].ToString();

                if (GSTSplCheck == "Y")
                {
                    rdbGSTSplYes.Checked = true;
                    rdbGSTSplNo.Checked = false;
                }
                else
                {
                    rdbGSTSplNo.Checked = true;
                    rdbGSTSplYes.Checked = false;
                }

                ddlGSTSplPer.SelectedValue = DtContractsData.Rows[0]["GSTSplper"].ToString();



                string includeST = DtContractsData.Rows[0]["IncludeST"].ToString();
                CheckIncludeST.Checked = false;
                if (includeST.Length > 0)
                {
                    bool bIncludeST = Convert.ToBoolean(includeST);
                    if (bIncludeST == true)
                    {
                        CheckIncludeST.Checked = true;
                    }
                }
                string ST75 = DtContractsData.Rows[0]["ServiceTax75"].ToString();
                Check75ST.Checked = false;
                if (ST75.Length > 0)
                {
                    bool bST75 = Convert.ToBoolean(ST75);
                    if (bST75 == true)
                    {
                        Check75ST.Checked = true;
                    }
                }

                string OtPercent = DtContractsData.Rows[0]["OTPersent"].ToString();
                if (OtPercent == "100")
                {
                    DdlOt.SelectedIndex = 0;
                }
                else
                {
                    DdlOt.SelectedIndex = 1;
                }

                txtOWF.Text = DtContractsData.Rows[0]["OWF"].ToString();
                txtadmincharges.Text = DtContractsData.Rows[0]["AdminCharges"].ToString();

                string OTAmounttype = DtContractsData.Rows[0]["OTAmounttype"].ToString();
                if (OTAmounttype.Length > 0)
                {
                    bool bSTType = Convert.ToBoolean(OTAmounttype);
                    if (bSTType == false)
                    {
                        radiootregular.Checked = true;
                    }
                    else
                    {
                        radiootspecial.Checked = true;
                    }
                }
                txtdescription.Text = DtContractsData.Rows[0]["Description"].ToString();
                TxtContractDescription.Text = DtContractsData.Rows[0]["ContractDescription"].ToString();

                chktl.Checked = bool.Parse(DtContractsData.Rows[0]["tl"].ToString());
                txttlamt.Text = DtContractsData.Rows[0]["tlno"].ToString();



                #endregion End Code For V-C  Ref : Ref  ServiceTaxType  To Description

                #region Begin Code For V-C   Ref : Ref  Tds  To Expecteddate as on 29/03/2014 by venkat

                txtTds.Text = DtContractsData.Rows[0]["Tds"].ToString();

                txtPoDate.Text = DtContractsData.Rows[0]["PoDate"].ToString();
                txtPono.Text = DtContractsData.Rows[0]["Pono"].ToString();
                // txtPoDate.Text = DateTime.Parse(DtContractsData.Rows[0]["PoDate"].ToString()).ToString("dd/MM/yyyy");

                txtExpectdateofreceipt.Text = DtContractsData.Rows[0]["ReceiptExpectedDate"].ToString();

                ddlTDSon.SelectedIndex = int.Parse(DtContractsData.Rows[0]["TDSon"].ToString());

                #endregion End Code For V-C  Ref : Ref  Tds  To Expecteddate as on 29/03/2014 by venkat

                #region Begin New code for Esibranche as on 02/08/2014 by venkat
                if (DtContractsData.Rows[0]["ESIBranch"].ToString() == "0")
                {
                    ddlEsibranch.SelectedIndex = 0;
                }
                else
                {
                    ddlEsibranch.SelectedValue = DtContractsData.Rows[0]["ESIBranch"].ToString();
                }

                if (DtContractsData.Rows[0]["PFBranch"].ToString() == "0")
                {
                    ddlPFbranch.SelectedIndex = 0;
                }
                else
                {
                    ddlPFbranch.SelectedValue = DtContractsData.Rows[0]["PFBranch"].ToString();
                }

                Chkpdfs.Checked = bool.Parse(DtContractsData.Rows[0]["pdfs"].ToString());

                ChkWithRoundoff.Checked = bool.Parse(DtContractsData.Rows[0]["WithRoundoff"].ToString());
                ChkbillfromPaysheetduties.Checked = bool.Parse(DtContractsData.Rows[0]["BillFromPaysheetDuties"].ToString());

                chkNoUnif.Checked = bool.Parse(DtContractsData.Rows[0]["NoUniform"].ToString());

                chkNoSal.Checked = bool.Parse(DtContractsData.Rows[0]["NoSalAdv"].ToString());
                chkNoLWF.Checked = bool.Parse(DtContractsData.Rows[0]["NoLWF"].ToString());
                chkNoRegFee.Checked = bool.Parse(DtContractsData.Rows[0]["NoRegFee"].ToString());
                ddlWagesCalnOn.SelectedIndex = int.Parse(DtContractsData.Rows[0]["WagesCalnOn"].ToString());

                ddlPTOn.SelectedIndex = int.Parse(DtContractsData.Rows[0]["PTOn"].ToString());

                string IndNoOfOts = "0";
                IndNoOfOts = DtContractsData.Rows[0]["IndNoOfOts"].ToString();
                if (IndNoOfOts == "99")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 0;
                }
                else if (IndNoOfOts == "0")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 1;
                }
                else if (IndNoOfOts == "1")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 2;
                }
                else if (IndNoOfOts == "2")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 3;
                }
                else if (IndNoOfOts == "3")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 4;
                }
                else if (IndNoOfOts == "4")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 5;
                }
                else if (IndNoOfOts == "5")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 6;
                }
                else if (IndNoOfOts == "6")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 7;
                }
                else if (IndNoOfOts == "7")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 8;
                }
                else if (IndNoOfOts == "8")
                {
                    ddlIndNoOfOtsPaysheet.SelectedIndex = 9;
                }
                else if (int.Parse(IndNoOfOts) > 9)
                {
                    ddlIndNoOfOtsPaysheet.SelectedValue = IndNoOfOts;
                }


                string IndNoOfDays = "0";
                IndNoOfDays = DtContractsData.Rows[0]["IndNoOfDays"].ToString();
                if (IndNoOfDays == "99")
                {
                    ddlIndNoOfDays.SelectedIndex = 0;
                }
                else if (IndNoOfDays == "0")
                {
                    ddlIndNoOfDays.SelectedIndex = 1;
                }
                else if (IndNoOfDays == "1")
                {
                    ddlIndNoOfDays.SelectedIndex = 2;
                }
                else if (IndNoOfDays == "2")
                {
                    ddlIndNoOfDays.SelectedIndex = 3;
                }
                else if (IndNoOfDays == "3")
                {
                    ddlIndNoOfDays.SelectedIndex = 4;
                }
                else if (IndNoOfDays == "4")
                {
                    ddlIndNoOfDays.SelectedIndex = 5;
                }
                else if (IndNoOfDays == "5")
                {
                    ddlIndNoOfDays.SelectedIndex = 6;
                }
                else if (IndNoOfDays == "6")
                {
                    ddlIndNoOfDays.SelectedIndex = 7;
                }
                else if (IndNoOfDays == "7")
                {
                    ddlIndNoOfDays.SelectedIndex = 8;
                }
                else if (IndNoOfDays == "8")
                {
                    ddlIndNoOfDays.SelectedIndex = 9;
                }
                else if (int.Parse(IndNoOfDays) > 9)
                {
                    ddlIndNoOfDays.SelectedValue = IndNoOfDays;
                }

                #endregion

                #region for inv headings

                TxtInvDescription.Text = DtContractsData.Rows[0]["InvDescription"].ToString();
                txtInvNoofEmployees.Text = DtContractsData.Rows[0]["InvNoofEmps"].ToString();
                txtInvNoofDuties.Text = DtContractsData.Rows[0]["InvNoofDuties"].ToString();
                txtInvPayrate.Text = DtContractsData.Rows[0]["InvPayrate"].ToString();
                txtInvAmount.Text = DtContractsData.Rows[0]["InvAmount"].ToString();

                txtInvsaccode.Text = DtContractsData.Rows[0]["InvSACCode"].ToString();
                txtInvmonthdays.Text = DtContractsData.Rows[0]["InvMonthDays"].ToString();


                if (DtContractsData.Rows[0]["InvSACCodeVisible"].ToString() == "Y")
                {
                    chkInvSaccode.Checked = true;
                }
                else
                {
                    chkInvSaccode.Checked = false;
                }

                if (DtContractsData.Rows[0]["InvMonthDaysVisible"].ToString() == "Y")
                {
                    chkInvMonthDays.Checked = true;
                }
                else
                {
                    chkInvMonthDays.Checked = false;
                }

                if (DtContractsData.Rows[0]["InvDescriptionVisible"].ToString() == "Y")
                {
                    chkInvDesc.Checked = true;
                }
                else
                {
                    chkInvDesc.Checked = false;
                }

                if (DtContractsData.Rows[0]["InvNoofEmpsVisible"].ToString() == "Y")
                {
                    chkInvNoofemp.Checked = true;
                }
                else
                {
                    chkInvNoofemp.Checked = false;
                }

                if (DtContractsData.Rows[0]["InvNoofDutiesVisible"].ToString() == "Y")
                {
                    chkInvNoofduties.Checked = true;
                }
                else
                {
                    chkInvNoofduties.Checked = false;
                }

                if (DtContractsData.Rows[0]["InvPayrateVisible"].ToString() == "Y")
                {
                    chkInvPayrate.Checked = true;
                }
                else
                {
                    chkInvPayrate.Checked = false;
                }

                if (DtContractsData.Rows[0]["InvAmountVisible"].ToString() == "Y")
                {
                    chkInvAmount.Checked = true;
                }
                else
                {
                    chkInvAmount.Checked = false;
                }


                #endregion

                #endregion   End  Code For Assign Values to The Controls as on [18-10-2013]

                #region  Begin Code For Client Contract Details Which are Used For The Billing As on [19-10-2013]
                HtContracts.Clear();
                #region    Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]
                SPName = "GetContractDetailForInvoice";
                #endregion  Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]

                #region Begin Code For Hash Table Parameters as on [19-10-2013]
                HtContracts.Add("@clientid", ClientID);
                HtContracts.Add("@Contractid", ddlContractids.SelectedValue);

                #endregion  End  Code For Hash Table Parameters as on [19-10-2013]

                #region  Begin Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]
                DataTable DtContractDetailsData = config.ExecuteAdaptorAsyncWithParams(SPName, HtContracts).Result;
                #endregion End Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]

                #endregion End  Code For Client Contract Details Which are Used For The Billing As on [19-10-2013]

                if (DtContractDetailsData.Rows.Count == 0)
                {
                    //lblmsgcontractdetails.Text = "There IS No Contract Details For This Client";
                    lblMsg.Text = "Contract Details Are Not Available . Which Are Generating The Invoice.";

                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Contract Details Are Not Available . Which Are Generating The Invoice.');", true);

                    if (RadioSpecial.Checked == false)
                    {
                        return;
                    }

                    else
                    {
                        Specialwagesdata();
                    }
                    // return;
                }
                else
                {
                    for (int i = 0; i < DtContractDetailsData.Rows.Count; i++)
                    {


                        gvdesignation.Rows[i].Visible = true;
                        //DefaultRowData(i);

                        DropDownList CDesgn = gvdesignation.Rows[i].FindControl("DdlDesign") as DropDownList;

                        if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["Designations"].ToString()) != false)
                        {
                            CDesgn.SelectedIndex = 0;

                        }
                        else
                        {
                            if (int.Parse(DtContractDetailsData.Rows[i]["Designations"].ToString()) != 0)
                            {
                                CDesgn.SelectedValue = DtContractDetailsData.Rows[i]["Designations"].ToString();
                            }
                            else
                            {
                                CDesgn.SelectedIndex = 0;
                            }
                        }

                        //For Employee Duty Type

                        DropDownList CDutytype = gvdesignation.Rows[i].FindControl("ddldutytype") as DropDownList;

                        if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["PayType"].ToString()) != false)
                        {
                            CDutytype.SelectedIndex = 0;
                        }
                        else
                        {
                            if (DtContractDetailsData.Rows[i]["PayType"].ToString().Trim().Length > 0)
                            {
                                CDutytype.SelectedIndex = Convert.ToInt32(DtContractDetailsData.Rows[i]["PayType"].ToString().Trim());
                            }
                        }

                        //No Of Days For Billing
                        DropDownList CNoOfDaysForBilling = gvdesignation.Rows[i].FindControl("ddlNoOfDaysBilling") as DropDownList;

                        if (CNoOfDaysForBilling != null)
                        {

                            if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["NoofDays"].ToString()) != false)
                            {
                                CNoOfDaysForBilling.SelectedIndex = 0;
                            }
                            else
                            {



                                if (DtContractDetailsData.Rows[i]["NoofDays"].ToString().Trim().Length > 0)
                                {
                                    float noofdays = float.Parse(DtContractDetailsData.Rows[i]["NoofDays"].ToString());
                                    if (noofdays == 0)
                                        CNoOfDaysForBilling.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        CNoOfDaysForBilling.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        CNoOfDaysForBilling.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        CNoOfDaysForBilling.SelectedIndex = 3;
                                    else
                                        CNoOfDaysForBilling.SelectedValue = DtContractDetailsData.Rows[i]["NoofDays"].ToString();
                                }
                            }

                        }


                        #region for GST

                        DropDownList ddlHSNNumber = gvdesignation.Rows[i].FindControl("ddlHSNNumber") as DropDownList;
                        ddlHSNNumber.SelectedValue = DtContractDetailsData.Rows[i]["HSNNumber"].ToString();

                        CheckBox chkcdCGST = gvdesignation.Rows[i].FindControl("chkcdCGST") as CheckBox;
                        chkcdCGST.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdCGST"].ToString());

                        CheckBox chkcdSGST = gvdesignation.Rows[i].FindControl("chkcdSGST") as CheckBox;
                        chkcdSGST.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdSGST"].ToString());

                        CheckBox chkcdIGST = gvdesignation.Rows[i].FindControl("chkcdIGST") as CheckBox;
                        chkcdIGST.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdIGST"].ToString());

                        #endregion for GST

                        #region PF & ESI
                        CheckBox chkcdPF = gvdesignation.Rows[i].FindControl("chkcdPF") as CheckBox;
                        chkcdPF.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdPF"].ToString());

                        CheckBox chkcdESI = gvdesignation.Rows[i].FindControl("chkcdESI") as CheckBox;
                        chkcdESI.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdESI"].ToString());

                        CheckBox chkcdSC = gvdesignation.Rows[i].FindControl("chkcdSC") as CheckBox;
                        chkcdSC.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdSC"].ToString());

                        CheckBox chkcdSCOnPFESI = gvdesignation.Rows[i].FindControl("chkcdSCOnPFESI") as CheckBox;
                        chkcdSCOnPFESI.Checked = bool.Parse(DtContractDetailsData.Rows[i]["cdSCOnPFESI"].ToString());

                        #endregion PF & ESI


                        DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[i].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                        if (ddlNoOfOtsPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["Nots"].ToString()) != false)
                            {
                                ddlNoOfOtsPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (DtContractDetailsData.Rows[i]["Nots"].ToString().Trim().Length > 0)
                                {
                                    float noofdays = float.Parse(DtContractDetailsData.Rows[i]["Nots"].ToString());
                                    if (noofdays == 0)
                                    {
                                        ddlNoOfOtsPaysheet.SelectedIndex = 0;
                                    }

                                    else if (noofdays == 1)
                                    {
                                        ddlNoOfOtsPaysheet.SelectedIndex = 1;
                                    }
                                    else if (noofdays == 2)
                                    {
                                        ddlNoOfOtsPaysheet.SelectedIndex = 2;
                                    }
                                    else if (noofdays == 3)
                                    {
                                        ddlNoOfOtsPaysheet.SelectedIndex = 3;
                                    }
                                    else if (noofdays == 4)
                                    {
                                        ddlNoOfOtsPaysheet.SelectedIndex = 4;
                                    }

                                    else
                                    {
                                        ddlNoOfOtsPaysheet.SelectedValue = DtContractDetailsData.Rows[i]["Nots"].ToString();
                                    }

                                }

                            }
                        }

                        DropDownList ddlType = gvdesignation.Rows[i].FindControl("ddlType") as DropDownList;
                        ddlType.SelectedValue = DtContractDetailsData.Rows[i]["Type"].ToString();

                        TextBox Cdutyhrs = (TextBox)gvdesignation.Rows[i].FindControl("txtdutyhrs");
                        Cdutyhrs.Text = DtContractDetailsData.Rows[i]["DutyHrs"].ToString();

                        TextBox CQuantity = (TextBox)gvdesignation.Rows[i].FindControl("txtquantity");
                        CQuantity.Text = DtContractDetailsData.Rows[i]["Quantity"].ToString();

                        TextBox Csummary = (TextBox)gvdesignation.Rows[i].FindControl("txtsummary");
                        Csummary.Text = DtContractDetailsData.Rows[i]["Summary"].ToString();

                        TextBox Cbasic = (TextBox)gvdesignation.Rows[i].FindControl("TxtBasic");
                        Cbasic.Text = DtContractDetailsData.Rows[i]["Basic"].ToString();

                        CheckBox ChkRCApplicable = (CheckBox)gvdesignation.Rows[i].FindControl("ChkRCApplicable");
                        if (DtContractDetailsData.Rows[i]["RCApplicable"].ToString() == "True")
                        {
                            ChkRCApplicable.Checked = true;
                        }
                        else
                        {
                            ChkRCApplicable.Checked = false;
                        }

                        TextBox Cda = (TextBox)gvdesignation.Rows[i].FindControl("txtda");
                        Cda.Text = DtContractDetailsData.Rows[i]["da"].ToString();

                        TextBox Chra = (TextBox)gvdesignation.Rows[i].FindControl("txthra");
                        Chra.Text = DtContractDetailsData.Rows[i]["hra"].ToString();

                        TextBox CConveyance = (TextBox)gvdesignation.Rows[i].FindControl("txtConveyance");
                        CConveyance.Text = DtContractDetailsData.Rows[i]["Conveyance"].ToString();

                        TextBox Caw = (TextBox)gvdesignation.Rows[i].FindControl("txtoa");
                        Caw.Text = DtContractDetailsData.Rows[i]["OtherAllowance"].ToString();

                        TextBox Cwashallowance = (TextBox)gvdesignation.Rows[i].FindControl("txtwa");
                        Cwashallowance.Text = DtContractDetailsData.Rows[i]["washallownce"].ToString();

                        TextBox Cca = (TextBox)gvdesignation.Rows[i].FindControl("txtcca");
                        Cca.Text = DtContractDetailsData.Rows[i]["cca"].ToString();

                        TextBox LeaveAmount = (TextBox)gvdesignation.Rows[i].FindControl("txtleaveamount");
                        LeaveAmount.Text = DtContractDetailsData.Rows[i]["LeaveAmount"].ToString();

                        TextBox Gratuty = (TextBox)gvdesignation.Rows[i].FindControl("txtgratuty");
                        Gratuty.Text = DtContractDetailsData.Rows[i]["Gratuity"].ToString();

                        TextBox Bonus = (TextBox)gvdesignation.Rows[i].FindControl("txtbonus");
                        Bonus.Text = DtContractDetailsData.Rows[i]["Bonus"].ToString();

                        TextBox CAttBonus = (TextBox)gvdesignation.Rows[i].FindControl("txtattbonus");
                        CAttBonus.Text = DtContractDetailsData.Rows[i]["attbonus"].ToString();

                        TextBox PayRate = (TextBox)gvdesignation.Rows[i].FindControl("txtPayRate");
                        PayRate.Text = DtContractDetailsData.Rows[i]["Amount"].ToString();


                        if (i < DtContractDetailsData.Rows.Count)
                        {
                            Session["DataContractsAIndex"] = i + 1;
                            NewDataRow();
                        }
                        TextBox Nfhs = (TextBox)gvdesignation.Rows[i].FindControl("txtNfhs");
                        Nfhs.Text = DtContractDetailsData.Rows[i]["NFhs"].ToString();


                        TextBox Txtrc = (TextBox)gvdesignation.Rows[i].FindControl("Txtrc");
                        Txtrc.Text = DtContractDetailsData.Rows[i]["rc"].ToString();
                        TextBox CTxtCs = (TextBox)gvdesignation.Rows[i].FindControl("TxtCs");
                        CTxtCs.Text = DtContractDetailsData.Rows[i]["cs"].ToString();
                        TextBox CTxtOTRate = (TextBox)gvdesignation.Rows[i].FindControl("TxtOTRate");
                        CTxtOTRate.Text = DtContractDetailsData.Rows[i]["OTRATE"].ToString();

                        TextBox CTxtScPer = (TextBox)gvdesignation.Rows[i].FindControl("TxtScPer");
                        CTxtScPer.Text = DtContractDetailsData.Rows[i]["ServicechargePer"].ToString();



                    }
                    Session["ContractsAIndex"] = DtContractDetailsData.Rows.Count + 1;

                    //Data For The Second Gridview 


                    if (RadioSpecial.Checked == false && RadioBoth.Checked == false)
                    {
                        return;
                    }
                    else
                    {
                        Specialwagesdata();
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Specialwagesdata()
        {
            try
            {


                #region    Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]
                var SPName = "GetContractDetailForPaysheet";
                Hashtable HtContractsForInvoice = new Hashtable();
                #endregion  Begin Code For Assign Values To  Variable Declaration as on [19-10-2013]


                #region Begin Code For Hash Table Parameters as on [19-10-2013]
                HtContractsForInvoice.Add("@clientid", ddlclientid.SelectedValue);
                HtContractsForInvoice.Add("@ContractId", ddlContractids.SelectedValue);


                #endregion  End  Code For Hash Table Parameters as on [19-10-2013]

                #region  Begin Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]
                DataTable dtspecialwage = config.ExecuteAdaptorAsyncWithParams(SPName, HtContractsForInvoice).Result;
                #endregion End Code For Calling Stored Procedure To Retrive Data from Contracts as on [19-10-2013]

                if (dtspecialwage.Rows.Count == 0)
                {
                    SetInitialRowForSW();
                    lblMsg.Text = "There Is No Special Wage details for The Selected Client.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('There Is No Special Wage details for The Selected Client.');", true);
                    return;

                }
                else
                {
                    btnadddesgnsw.Visible = true;
                    for (int i = 0; i < dtspecialwage.Rows.Count; i++)
                    {
                        gvSWDesignations.Rows[i].Visible = true;
                        DropDownList CDesgnsw = gvSWDesignations.Rows[i].FindControl("DdlDesign") as DropDownList;
                        if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["Designations"].ToString()) != false)
                        {
                            CDesgnsw.SelectedIndex = 0;
                        }
                        else
                        {
                            if (int.Parse(dtspecialwage.Rows[i]["Designations"].ToString()) != 0)
                            {
                                CDesgnsw.SelectedValue = dtspecialwage.Rows[i]["Designations"].ToString();
                            }
                            else
                            {
                                CDesgnsw.SelectedIndex = 0;

                            }
                        }

                        DropDownList ddlshift = gvSWDesignations.Rows[i].FindControl("ddlshift") as DropDownList;

                        if (ddlshift != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["Shift"].ToString()) != false)
                            {
                                ddlshift.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlshift.SelectedValue = dtspecialwage.Rows[i]["Shift"].ToString();
                            }

                        }


                        DropDownList Ccategorysw = gvSWDesignations.Rows[i].FindControl("DdlCategory") as DropDownList;
                        if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["MinWagesCategory"].ToString()) != false)
                        {
                            Ccategorysw.SelectedIndex = 0;
                        }
                        else
                        {
                            if (int.Parse(dtspecialwage.Rows[i]["MinWagesCategory"].ToString()) != 0)
                            {
                                Ccategorysw.SelectedValue = dtspecialwage.Rows[i]["MinWagesCategory"].ToString();
                            }
                            else
                            {
                                Ccategorysw.SelectedIndex = 0;
                            }
                        }
                        //No Of Days For  wages
                        DropDownList CNoOfDaysFoWages = gvSWDesignations.Rows[i].FindControl("ddlNoOfDaysWages") as DropDownList;

                        if (CNoOfDaysFoWages != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["NoofDays"].ToString()) != false)
                            {
                                CNoOfDaysFoWages.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dtspecialwage.Rows[i]["NoofDays"].ToString().Trim().Length > 0)
                                {

                                    int noofdays = int.Parse(dtspecialwage.Rows[i]["NoofDays"].ToString());
                                    if (noofdays == 0)
                                        CNoOfDaysFoWages.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        CNoOfDaysFoWages.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        CNoOfDaysFoWages.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        CNoOfDaysFoWages.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        CNoOfDaysFoWages.SelectedIndex = 4;
                                    else
                                                        if (noofdays == 5)
                                        CNoOfDaysFoWages.SelectedIndex = 5;
                                    else
                                                            if (noofdays == 6)
                                        CNoOfDaysFoWages.SelectedIndex = 6;
                                    else
                                                                if (noofdays == 7)
                                        CNoOfDaysFoWages.SelectedIndex = 7;
                                    else
                                                                    if (noofdays == 8)
                                        CNoOfDaysFoWages.SelectedIndex = 8;
                                    else
                                        CNoOfDaysFoWages.SelectedValue = dtspecialwage.Rows[i]["NoofDays"].ToString();

                                }
                            }
                        }


                        DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                        if (ddlNoOfOtsPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["Nots"].ToString()) != false)
                            {
                                ddlNoOfOtsPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dtspecialwage.Rows[i]["Nots"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dtspecialwage.Rows[i]["Nots"].ToString());
                                    if (noofdays == 0)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 4;
                                    else
                                                        if (noofdays == 5)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 5;
                                    else

                                                            if (noofdays == 6)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 6;
                                    else

                                                                if (noofdays == 7)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 7;
                                    else

                                                                    if (noofdays == 8)
                                        ddlNoOfOtsPaysheet.SelectedIndex = 8;
                                    else

                                        ddlNoOfOtsPaysheet.SelectedValue = dtspecialwage.Rows[i]["Nots"].ToString();
                                }
                            }

                        }


                        DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfNhsPaysheet") as DropDownList;

                        if (ddlNoOfNhsPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["NNhs"].ToString()) != false)
                            {
                                ddlNoOfNhsPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dtspecialwage.Rows[i]["NNhs"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dtspecialwage.Rows[i]["NNhs"].ToString());
                                    if (noofdays == 0)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlNoOfNhsPaysheet.SelectedIndex = 4;
                                    else

                                        ddlNoOfNhsPaysheet.SelectedValue = dtspecialwage.Rows[i]["NNhs"].ToString();
                                }
                            }

                        }


                        DropDownList ddlNoOfWosPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfWosPaysheet") as DropDownList;

                        if (ddlNoOfWosPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["NWos"].ToString()) != false)
                            {
                                ddlNoOfWosPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dtspecialwage.Rows[i]["NWos"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dtspecialwage.Rows[i]["NWos"].ToString());
                                    if (noofdays == 0)
                                        ddlNoOfWosPaysheet.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlNoOfWosPaysheet.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlNoOfWosPaysheet.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlNoOfWosPaysheet.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlNoOfWosPaysheet.SelectedIndex = 4;
                                    else

                                        ddlNoOfWosPaysheet.SelectedValue = dtspecialwage.Rows[i]["NWos"].ToString();
                                }
                            }

                        }

                        TextBox Cbasicsw = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtBasic");
                        Cbasicsw.Text = dtspecialwage.Rows[i]["Basic"].ToString();

                        TextBox Cgrosssw = (TextBox)gvSWDesignations.Rows[i].FindControl("Txtgross");
                        Cgrosssw.Text = dtspecialwage.Rows[i]["Gross"].ToString();

                        TextBox CNetPaysw = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtNetPay");
                        CNetPaysw.Text = dtspecialwage.Rows[i]["NetPay"].ToString();

                        #region pf days

                        DropDownList ddlNoOfPFDaysPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfPFDaysPaysheet") as DropDownList;

                        if (ddlNoOfPFDaysPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["PFDays"].ToString()) != false)
                            {
                                ddlNoOfPFDaysPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dtspecialwage.Rows[i]["PFDays"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dtspecialwage.Rows[i]["PFDays"].ToString());
                                    if (noofdays == 0)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlNoOfPFDaysPaysheet.SelectedIndex = 4;
                                    else

                                        ddlNoOfPFDaysPaysheet.SelectedValue = dtspecialwage.Rows[i]["PFDays"].ToString();
                                }
                            }

                        }

                        #endregion

                        #region esi days

                        DropDownList ddlNoOfESIDaysPaysheet = gvSWDesignations.Rows[i].FindControl("ddlNoOfESIDaysPaysheet") as DropDownList;

                        if (ddlNoOfESIDaysPaysheet != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["ESIDays"].ToString()) != false)
                            {
                                ddlNoOfESIDaysPaysheet.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dtspecialwage.Rows[i]["ESIDays"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dtspecialwage.Rows[i]["ESIDays"].ToString());
                                    if (noofdays == 0)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlNoOfESIDaysPaysheet.SelectedIndex = 4;
                                    else

                                        ddlNoOfESIDaysPaysheet.SelectedValue = dtspecialwage.Rows[i]["ESIDays"].ToString();
                                }
                            }

                        }


                        DropDownList ddlOTESIWagesDays = gvSWDesignations.Rows[i].FindControl("ddlOTESIWagesDays") as DropDownList;

                        if (ddlOTESIWagesDays != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["OTESIWagesDays"].ToString()) != false)
                            {
                                ddlOTESIWagesDays.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dtspecialwage.Rows[i]["OTESIWagesDays"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dtspecialwage.Rows[i]["OTESIWagesDays"].ToString());
                                    if (noofdays == 0)
                                        ddlOTESIWagesDays.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlOTESIWagesDays.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlOTESIWagesDays.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlOTESIWagesDays.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlOTESIWagesDays.SelectedIndex = 4;
                                    else
                                         if (noofdays == 5)
                                        ddlOTESIWagesDays.SelectedIndex = 5;
                                    else
                                         if (noofdays == 6)
                                        ddlOTESIWagesDays.SelectedIndex = 6;
                                    else
                                         if (noofdays == 7)
                                        ddlOTESIWagesDays.SelectedIndex = 7;
                                    else
                                         if (noofdays == 8)
                                        ddlOTESIWagesDays.SelectedIndex = 8;
                                    else

                                        ddlOTESIWagesDays.SelectedValue = dtspecialwage.Rows[i]["OTESIWagesDays"].ToString();
                                }
                            }

                        }

                        #endregion

                        CheckBox chkswPF = gvSWDesignations.Rows[i].FindControl("chkswPF") as CheckBox;
                        chkswPF.Checked = bool.Parse(dtspecialwage.Rows[i]["ChkPF"].ToString());

                        CheckBox chkswESI = gvSWDesignations.Rows[i].FindControl("chkswESI") as CheckBox;
                        chkswESI.Checked = bool.Parse(dtspecialwage.Rows[i]["ChkESI"].ToString());

                        #region spl days

                        DropDownList ddlSplAllwDays = gvSWDesignations.Rows[i].FindControl("ddlSplAllwDays") as DropDownList;

                        if (ddlSplAllwDays != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["NoOfSplDays"].ToString()) != false)
                            {
                                ddlSplAllwDays.SelectedIndex = 0;
                            }
                            else
                            {
                                if (dtspecialwage.Rows[i]["NoOfSplDays"].ToString().Trim().Length > 0)
                                {
                                    int noofdays = int.Parse(dtspecialwage.Rows[i]["NoOfSplDays"].ToString());
                                    if (noofdays == 0)
                                        ddlSplAllwDays.SelectedIndex = 0;
                                    else
                                        if (noofdays == 1)
                                        ddlSplAllwDays.SelectedIndex = 1;
                                    else
                                            if (noofdays == 2)
                                        ddlSplAllwDays.SelectedIndex = 2;
                                    else
                                                if (noofdays == 3)
                                        ddlSplAllwDays.SelectedIndex = 3;
                                    else
                                                    if (noofdays == 4)
                                        ddlSplAllwDays.SelectedIndex = 4;
                                    else

                                        ddlSplAllwDays.SelectedValue = dtspecialwage.Rows[i]["NoOfSplDays"].ToString();
                                }
                            }

                        }

                        #endregion

                        #region for Bonus,Gratuity and la type
                        DropDownList ddllatype = gvSWDesignations.Rows[i].FindControl("ddllatype") as DropDownList;

                        if (ddllatype != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["latype"].ToString()) != false)
                            {
                                ddllatype.SelectedIndex = 0;
                            }
                            else
                            {
                                ddllatype.SelectedIndex = int.Parse(dtspecialwage.Rows[i]["latype"].ToString());
                            }
                        }

                        DropDownList ddlgratuitytype = gvSWDesignations.Rows[i].FindControl("ddlgratuitytype") as DropDownList;

                        if (ddlgratuitytype != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["GratuityType"].ToString()) != false)
                            {
                                ddlgratuitytype.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlgratuitytype.SelectedIndex = int.Parse(dtspecialwage.Rows[i]["GratuityType"].ToString());
                            }
                        }

                        DropDownList ddlbonustype = gvSWDesignations.Rows[i].FindControl("ddlbonustype") as DropDownList;

                        if (ddlbonustype != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["BonusType"].ToString()) != false)
                            {
                                ddlbonustype.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlbonustype.SelectedIndex = int.Parse(dtspecialwage.Rows[i]["BonusType"].ToString());
                            }
                        }



                        DropDownList ddlAttbonustype = gvSWDesignations.Rows[i].FindControl("ddlAttbonustype") as DropDownList;

                        if (ddlAttbonustype != null)
                        {

                            if (String.IsNullOrEmpty(dtspecialwage.Rows[i]["AttBonusType"].ToString()) != false)
                            {
                                ddlAttbonustype.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlAttbonustype.SelectedIndex = int.Parse(dtspecialwage.Rows[i]["AttBonusType"].ToString());
                            }
                        }
                        #endregion

                        TextBox Cdasw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtda");
                        Cdasw.Text = dtspecialwage.Rows[i]["da"].ToString();

                        TextBox Chrasw = (TextBox)gvSWDesignations.Rows[i].FindControl("txthra");
                        Chrasw.Text = dtspecialwage.Rows[i]["hra"].ToString();

                        TextBox CConveyancesw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtConveyance");
                        CConveyancesw.Text = dtspecialwage.Rows[i]["Conveyance"].ToString();

                        TextBox Cawsw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtoa");
                        Cawsw.Text = dtspecialwage.Rows[i]["OtherAllowance"].ToString();

                        TextBox Cwashallowancesw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtwa");
                        Cwashallowancesw.Text = dtspecialwage.Rows[i]["washallownce"].ToString();

                        TextBox Ccasw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtcca");
                        Ccasw.Text = dtspecialwage.Rows[i]["cca"].ToString();

                        TextBox LeaveAmountsw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtleaveamount");
                        LeaveAmountsw.Text = dtspecialwage.Rows[i]["LeaveAmount"].ToString();

                        TextBox Gratutysw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtgratuty");
                        Gratutysw.Text = dtspecialwage.Rows[i]["Gratuity"].ToString();

                        TextBox Bonussw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtbonus");
                        Bonussw.Text = dtspecialwage.Rows[i]["Bonus"].ToString();

                        TextBox AttBonussw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtattbonus");
                        AttBonussw.Text = dtspecialwage.Rows[i]["attbonus"].ToString();


                        TextBox NFHsw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtNfhs1");
                        NFHsw.Text = dtspecialwage.Rows[i]["NFHs"].ToString();

                        TextBox TxtCSWPLDays = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtCSWPLDays");
                        TxtCSWPLDays.Text = dtspecialwage.Rows[i]["PLDays"].ToString();

                        TextBox TxtCSWPLAmount = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtCSWPLAmount");
                        TxtCSWPLAmount.Text = dtspecialwage.Rows[i]["PLAmount"].ToString();

                        TextBox TxtCSWTLDays = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtCSWTLDays");
                        TxtCSWTLDays.Text = dtspecialwage.Rows[i]["TLDays"].ToString();

                        TextBox TxtCSWTLAmount = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtCSWTLAmount");
                        TxtCSWTLAmount.Text = dtspecialwage.Rows[i]["TLAmount"].ToString();

                        TextBox TxtNHSRate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtNHSRate");
                        TxtNHSRate.Text = dtspecialwage.Rows[i]["NHSRate"].ToString();

                        TextBox TxtWORate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtWORate");
                        TxtWORate.Text = dtspecialwage.Rows[i]["WORate"].ToString();

                        TextBox TxtFoodAllowance = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtFoodAllowance");
                        TxtFoodAllowance.Text = dtspecialwage.Rows[i]["FoodAllowance"].ToString();

                        TextBox TxtMedicalAllowance = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtMedicalAllowance");
                        TxtMedicalAllowance.Text = dtspecialwage.Rows[i]["MedicalAllowance"].ToString();

                        TextBox TxtSplAllowance = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtSplAllowance");
                        TxtSplAllowance.Text = dtspecialwage.Rows[i]["SplAllowance"].ToString();

                        TextBox TxtTravelAllowance = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtTravelAllowance");
                        TxtTravelAllowance.Text = dtspecialwage.Rows[i]["TravelAllw"].ToString();

                        TextBox TxtPerformanceAllowance = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtPerformanceAllowance");
                        TxtPerformanceAllowance.Text = dtspecialwage.Rows[i]["PerformanceAllw"].ToString();

                        TextBox TxtMobileAllowance = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtMobileAllowance");
                        TxtMobileAllowance.Text = dtspecialwage.Rows[i]["MobileAllw"].ToString();



                        TextBox Txtrc = (TextBox)gvSWDesignations.Rows[i].FindControl("Txtrc");
                        Txtrc.Text = dtspecialwage.Rows[i]["rc"].ToString();


                        TextBox TxtCs = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtCs");
                        TxtCs.Text = dtspecialwage.Rows[i]["cs"].ToString();
                        TextBox TxtOTRate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtOTRate");
                        TxtOTRate.Text = dtspecialwage.Rows[i]["OTRate"].ToString();

                        TextBox TxtOTHrs = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtOThrs");
                        TxtOTHrs.Text = dtspecialwage.Rows[i]["OTHrs"].ToString();

                        TextBox TxtADVDed = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtADVDed");
                        TxtADVDed.Text = dtspecialwage.Rows[i]["ADVDed"].ToString();
                        TextBox TxtWCDed = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtWCDed");
                        TxtWCDed.Text = dtspecialwage.Rows[i]["WCDed"].ToString();
                        TextBox TxtUniformDed = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtUniformDed");
                        TxtUniformDed.Text = dtspecialwage.Rows[i]["UniformDed"].ToString();

                        TextBox TxtScPersw = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtScPer");
                        TxtScPersw.Text = dtspecialwage.Rows[i]["ServicechargePer"].ToString();

                        TextBox TxtPFRate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtPFRate");
                        TxtPFRate.Text = dtspecialwage.Rows[i]["PFRate"].ToString();
                        TextBox TxtESIRate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtESIRate");
                        TxtESIRate.Text = dtspecialwage.Rows[i]["ESIRate"].ToString();

                        TextBox txtserviceweightage = (TextBox)gvSWDesignations.Rows[i].FindControl("txtservicewt");
                        txtserviceweightage.Text = dtspecialwage.Rows[i]["Serviceweightage"].ToString();

                        TextBox txtHardshipAllw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtHardshipAllw");
                        txtHardshipAllw.Text = dtspecialwage.Rows[i]["HardshipAllw"].ToString();

                        TextBox txtRankAllw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtRankAllw");
                        txtRankAllw.Text = dtspecialwage.Rows[i]["RankAllw"].ToString();

                        TextBox txtuniformcharge = (TextBox)gvSWDesignations.Rows[i].FindControl("txtuniformcharge");
                        txtuniformcharge.Text = dtspecialwage.Rows[i]["uniformcharge"].ToString();

                        TextBox txtPFEmp = (TextBox)gvSWDesignations.Rows[i].FindControl("txtPFEmp");
                        txtPFEmp.Text = dtspecialwage.Rows[i]["PFemp"].ToString();
                        TextBox txtESIEmp = (TextBox)gvSWDesignations.Rows[i].FindControl("txtESIEmp");
                        txtESIEmp.Text = dtspecialwage.Rows[i]["ESIemp"].ToString();
                        TextBox txtPFEmpr = (TextBox)gvSWDesignations.Rows[i].FindControl("txtPFEmpr");
                        txtPFEmpr.Text = dtspecialwage.Rows[i]["PFempr"].ToString();
                        TextBox txtESIEmpr = (TextBox)gvSWDesignations.Rows[i].FindControl("txtESIEmpr");
                        txtESIEmpr.Text = dtspecialwage.Rows[i]["ESIempr"].ToString();
                        TextBox txtSCEmp = (TextBox)gvSWDesignations.Rows[i].FindControl("txtSCEmp");
                        txtSCEmp.Text = dtspecialwage.Rows[i]["SCemp"].ToString();
                        TextBox txtCTC = (TextBox)gvSWDesignations.Rows[i].FindControl("txtCTC");
                        txtCTC.Text = dtspecialwage.Rows[i]["CTC"].ToString();
                        TextBox TxtIncentiveRate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtIncentiveRate");
                        TxtIncentiveRate.Text = dtspecialwage.Rows[i]["IncentiveRate"].ToString();
                        TextBox TxtLCRate = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtLCRate");
                        TxtLCRate.Text = dtspecialwage.Rows[i]["LCRate"].ToString();



                        TextBox TxtsiteAllowance = (TextBox)gvSWDesignations.Rows[i].FindControl("TxtsiteAllowance");
                        TxtsiteAllowance.Text = dtspecialwage.Rows[i]["SITEALLOW"].ToString();

                        TextBox txtaddlhrallw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtaddlhrallw");
                        txtaddlhrallw.Text = dtspecialwage.Rows[i]["ADDL4HR"].ToString();

                        TextBox txtqtrallw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtqtrallw");
                        txtqtrallw.Text = dtspecialwage.Rows[i]["QTRALLOW"].ToString();

                        TextBox txtrelallw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtrelallw");
                        txtrelallw.Text = dtspecialwage.Rows[i]["RELALLOW"].ToString();

                        TextBox txtotesiwages = (TextBox)gvSWDesignations.Rows[i].FindControl("txtotesiwages");
                        txtotesiwages.Text = dtspecialwage.Rows[i]["OTESICWAGES"].ToString();

                        TextBox txtGunallw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtGunallw");
                        txtGunallw.Text = dtspecialwage.Rows[i]["GunAllw"].ToString();

                        DropDownList ddlGunAllwType = gvSWDesignations.Rows[i].FindControl("ddlGunAllwType") as DropDownList;
                        ddlGunAllwType.SelectedValue = dtspecialwage.Rows[i]["GunAllwType"].ToString();

                        TextBox txtFireallw = (TextBox)gvSWDesignations.Rows[i].FindControl("txtFireallw");
                        txtFireallw.Text = dtspecialwage.Rows[i]["FireAllw"].ToString();

                        if (i < dtspecialwage.Rows.Count)
                        {
                            Session["DataContractsAIndexsw"] = i + 1;
                            NewDataRowsw();
                        }

                    }
                }
                Session["ContractsAIndexsw"] = dtspecialwage.Rows.Count + 1;
            }
            catch (Exception ex)
            {

            }
        }

        protected void NewDataRow()
        {
            int designcount = (int)Session["DataContractsAIndex"];

            if (designcount < gvdesignation.Rows.Count)
            {
                gvdesignation.Rows[designcount].Visible = true;
                DefaultRowData(designcount);

                //string selectquery = "Select Design from designations ORDER BY Design ";
                //DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                DropDownList ddldrow = gvdesignation.Rows[designcount].FindControl("DdlDesign") as DropDownList;
                ddldrow.Items.Clear();

                // ddldrow.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    ddldrow.Items.Add(DtDesignation.Rows[i][0].ToString());
                //}


                if (DtDesignation.Rows.Count > 0)
                {
                    ddldrow.DataValueField = "Designid";
                    ddldrow.DataTextField = "Design";
                    ddldrow.DataSource = DtDesignation;
                    ddldrow.DataBind();

                }
                ddldrow.Items.Insert(0, "--Select--");
                ddldrow.SelectedIndex = 0;

            }
        }

        protected void NewDataRowsw()
        {
            int designcount = (int)Session["DataContractsAIndexsw"];
            if (designcount < gvSWDesignations.Rows.Count)
            {
                gvSWDesignations.Rows[designcount].Visible = true;
                DefaultRowDatasw(designcount);

                //string selectquery = "Select Design from designations ORDER BY Design";
                // DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                DropDownList ddldrowsw = gvSWDesignations.Rows[designcount].FindControl("DdlDesign") as DropDownList;
                ddldrowsw.Items.Clear();


                if (DtDesignation.Rows.Count > 0)
                {
                    ddldrowsw.DataValueField = "Designid";
                    ddldrowsw.DataTextField = "Design";
                    ddldrowsw.DataSource = DtDesignation;
                    ddldrowsw.DataBind();

                }
                ddldrowsw.Items.Insert(0, "--Select--");
                ddldrowsw.SelectedIndex = 0;


                DataTable DtCategory = GlobalData.Instance.LoadMinimumWagesCategories();
                DropDownList ddldrowsw1 = gvSWDesignations.Rows[designcount].FindControl("DdlCategory") as DropDownList;
                ddldrowsw1.Items.Clear();


                if (DtCategory.Rows.Count > 0)
                {
                    ddldrowsw1.DataValueField = "id";
                    ddldrowsw1.DataTextField = "name";
                    ddldrowsw1.DataSource = DtCategory;
                    ddldrowsw1.DataBind();

                }
                ddldrowsw1.Items.Insert(0, "--Select--");
                ddldrowsw1.SelectedIndex = 0;


                //ddldrowsw.Items.Add("--Select--");
                //for (int i = 0; i < DtDesignation.Rows.Count; i++)
                //{
                //    ddldrowsw.Items.Add(DtDesignation.Rows[i][0].ToString());
                //}
            }
        }

        protected void RadioManPower_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioManPower.Checked == true)
            {
                lbllampsum.Visible = false;
                txtlampsum.Visible = false;
            }
        }

        protected void RadioSpecial_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioSpecial.Checked == true || RadioBoth.Checked == true)
            {
                SpecialWagesPanel.Visible = true;
                btnadddesgnsw.Visible = true;
                Specialwagesdata();
            }
            else
            {
                SpecialWagesPanel.Visible = false;
                btnadddesgnsw.Visible = true;
            }
        }

        protected void btnadddesgnsw_Click(object sender, EventArgs e)
        {

            int columns = 1;

            if (txtnoofrowssw.Text == "")
            {
                columns = 1;
            }
            else
            {
                columns = int.Parse(txtnoofrowssw.Text);
            }

            for (int i = 0; i < columns; i++)
            {
                if (columns > 0)
                {
                    AddNewRowToGridForSW();
                }
                else
                {
                    lblmsgspecialwages.Text = "There is no more Designations";
                }
            }

            #region 
            //int designCount = Convert.ToInt16(Session["ContractsAIndexsw"]);
            //if (designCount < gvSWDesignations.Rows.Count)
            //{
            //    gvSWDesignations.Rows[designCount].Visible = true;
            //    DefaultRowDatasw(designCount);

            //    // string selectquery = "Select Design from designations ORDER BY Design";
            //    // DataTable DtDesignation = SqlHelper.Instance.GetTableByQuery(selectquery);

            //    DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
            //    DropDownList ddldrow = gvSWDesignations.Rows[designCount].FindControl("DdlDesign") as DropDownList;
            //    ddldrow.Items.Clear();
            //    //ddldrow.Items.Insert(0, "--Select--");
            //    if (DtDesignation.Rows.Count > 0)
            //    {
            //        ddldrow.DataValueField = "Designid";
            //        ddldrow.DataTextField = "Design";
            //        ddldrow.DataSource = DtDesignation;
            //        ddldrow.DataBind();

            //    }
            //    ddldrow.Items.Insert(0, "--Select--");
            //    ddldrow.SelectedIndex = 0;


            //    DataTable Dtcategory = GlobalData.Instance.LoadMinimumWagesCategories();
            //    DropDownList ddldrow1 = gvSWDesignations.Rows[designCount].FindControl("DdlCategory") as DropDownList;
            //    ddldrow1.Items.Clear();
            //    //ddldrow.Items.Insert(0, "--Select--");
            //    if (Dtcategory.Rows.Count > 0)
            //    {
            //        ddldrow1.DataValueField = "id";
            //        ddldrow1.DataTextField = "name";
            //        ddldrow1.DataSource = Dtcategory;
            //        ddldrow1.DataBind();

            //    }
            //    ddldrow1.Items.Insert(0, "--Select--");
            //    ddldrow1.SelectedIndex = 0;

            //    //for (int i = 0; i < DtDesignation.Rows.Count; i++)
            //    //{
            //    //    ddldrow.Items.Add(DtDesignation.Rows[i][0].ToString());
            //    //}
            //    designCount = designCount + 1;
            //    Session["ContractsAIndexsw"] = designCount;
            //    int check = int.Parse(Session["ContractsAIndexsw"].ToString());
            //}
            #endregion

        }

        protected void Btn_Renewal_Click(object sender, EventArgs e)
        {
            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select  Client ID/Name');", true);
                return;
            }
            else
            {
                txtcontractid.Text = GlobalData.Instance.LoadMaxContractId(ddlclientid.SelectedValue);
                ddlContractids.SelectedIndex = 0;
                txtStartingDate.Text = txtEndingDate.Text = "";
            }


        }

        protected void ddlContractids_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlContractids.SelectedIndex > 0)
                {
                    SetInitialRowForSW();
                    SetInitialRowForInvoice();
                    GetGridData();
                }
                else
                {
                    ddlclientid.SelectedIndex = 0;
                    SetInitialRowForInvoice();
                    ddlcname.SelectedIndex = 0;
                    ClearDataFromThePage();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlclientid_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblreslt.Text = "";
            SpecialWagesPanel.Visible = false;
            ClearDataFromThePage();
            if (ddlclientid.SelectedIndex > 0)
            {
                Fillcname();
                ddlContractids.Items.Clear();
                contractidautogenrate();
            }
            else
            {
                ddlcname.SelectedIndex = 0;

            }
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SpecialWagesPanel.Visible = false;
            ClearDataFromThePage();
            if (ddlcname.SelectedIndex > 0)
            {
                FillClientid();
                ddlContractids.Items.Clear();
                contractidautogenrate();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        #region Begin New code for Esibranches as on 02/08/2014

        protected void LoadEsibranches()
        {
            DataTable dtEsibranches = GlobalData.Instance.LoadEsibranches();
            if (dtEsibranches.Rows.Count > 0)
            {
                ddlEsibranch.DataValueField = "EsiBranchid";
                ddlEsibranch.DataTextField = "ESIBranchNo";
                ddlEsibranch.DataSource = dtEsibranches;
                ddlEsibranch.DataBind();
            }
            ddlEsibranch.Items.Insert(0, "-Select-");
        }

        protected void LoadPFbranches()
        {
            DataTable dtPFbranches = GlobalData.Instance.LoadPFbranches();
            if (dtPFbranches.Rows.Count > 0)
            {
                ddlPFbranch.DataValueField = "PFBranchid";
                ddlPFbranch.DataTextField = "PFBranchNo";
                ddlPFbranch.DataSource = dtPFbranches;
                ddlPFbranch.DataBind();
            }
            ddlPFbranch.Items.Insert(0, "-Select-");
        }

        #endregion

        protected void FillddlTakedata()
        {
            string SqlqryForClientIdAndName = string.Empty;
            DataTable dtForClientIdAndName = null;

            SqlqryForClientIdAndName = "select ClientName,Clientid from Clients where ClientId not in (select clientid from Contracts) order by ClientName";
            dtForClientIdAndName = config.ExecuteAdaptorAsyncWithQueryParams(SqlqryForClientIdAndName).Result;

            if (dtForClientIdAndName.Rows.Count > 0)
            {
                ddlClientidNotincontract.DataTextField = "Clientname";
                ddlClientidNotincontract.DataValueField = "Clientid";
                ddlClientidNotincontract.DataSource = dtForClientIdAndName;
                ddlClientidNotincontract.DataBind();
            }
            ddlClientidNotincontract.Items.Insert(0, "-Select-");
        }

        protected void btnClone_Click(object sender, EventArgs e)
        {



            try
            {
                var testDate = 0;

                //txtcontractid.Text = "";

                #region  Begin Code For Validations As on [18-10-2013]

                #region     Begin Code For Check The Client Id/Name Selected Or Not   as on [18-10-2013]
                if (ddlclientid.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select Clientid";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' please Select Clientid ');", true);
                    clearcontractdetails();
                    return;
                }
                #endregion  End Code For Check The Client Id/Name Selected Or Not  as on [18-10-2013]

                #region     Begin Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]
                if (txtStartingDate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill start date.');", true);
                    return;
                }

                if (txtStartingDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtStartingDate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "show alert", "alert('You Are Entered Invalid Contract Start Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                    else
                    {
                        string CheckSD = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                        //string CheckSD = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();

                        string CheckStartDate = "";

                        if (ddlContractids.SelectedIndex == 0)
                        {
                            CheckStartDate = " select clientid from contracts  where ContractEndDate>='" +
                                CheckSD + "'  and Clientid='" + ddlclientid.SelectedValue + "'";

                            DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(CheckStartDate).Result;
                            if (Dt.Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(),
                                  "show alert", "alert('You Are Entered Invalid Contract Start Date.Start Date Should Not Be Interval of the Previous Contracts Start and End Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                return;
                            }
                        }
                        else
                        {
                            if (ddlContractids.SelectedIndex > 1)
                            {
                                string CIDForCheck = (txtcontractid.Text).ToString().Substring((txtcontractid.Text).Length - 2);
                                CheckStartDate = " select clientid from contracts  where ContractEndDate>='" +
                                    CheckSD + "'  and Clientid='" + ddlclientid.SelectedValue + "'  and Right(contractid,2)<" + CIDForCheck;

                                DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(CheckStartDate).Result;
                                if (Dt.Rows.Count > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(),
                                      "show alert", "alert('You Are Entered Invalid Contract Start Date.Start Date Should Not Be Interval of the Previous Contracts Start and End Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                    return;
                                }
                            }
                        }

                    }


                }


                #endregion  End Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]

                #region     Begin Code For Check The Contract End Date Enetered Or Not  as on [18-10-2013]
                if (txtEndingDate.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please fill End date.";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill End date.');", true);
                    return;
                }

                if (txtEndingDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtEndingDate.Text);
                    if (testDate > 0)
                    {
                        lblMsg.Text = "You Are Entered Invalid Contract End Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        // ScriptManager.RegisterStartupScript(this, GetType(),
                        //"show alert", "alert('You Are Entered Invalid Contract End Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }

                }



                #endregion  End Code For Check The Contract End Date Enetered Or Not as on [18-10-2013]

                #region     Begin Code For Check The Selected Dates are Valid Or Not  as on [18-10-2013]
                if (txtStartingDate.Text.Trim().Length != 0 && txtEndingDate.Text.Trim().Length != 0)
                {
                    DateTime Dtstartdate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb"));
                    DateTime DtEnddate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb"));


                    if (Dtstartdate >= DtEnddate)
                    {
                        lblMsg.Text = "Invalid Contract End Date . Contract End Date Always Should Be Greater Than To Start Date.";
                        return;
                    }
                }
                #endregion  End Code For Check Selected Dates are Valid Or Not   as on [18-10-2013]

                #region     Begin Code For Check The Lampsum if Lampsum Selected  as on [18-10-2013]
                if (RadioLumpsum.Checked)
                {
                    if (txtlampsum.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please Enter The Lampsum Amount.";
                        return;
                    }
                }

                #endregion  End Code For Check The Lampsum if Lampsum Selected    as on [18-10-2013]

                #region     Begin Code For Check The Service Charge if Service Charge Yes Selected  as on [18-10-2013]
                if (radioyes.Checked)
                {
                    if (txtservicecharge.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please Enter The Service Charge.";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter The Service Charge.');", true);
                        return;
                    }
                }

                #endregion  End Code For Check The Service Charge if Service Charge Yes Selected    as on [18-10-2013]

                #endregion  Begin Code For Validations As on [18-10-2013]

                #region  Begin Code For Declaring Variables as on [18-10-2013]

                #region  Begin Code For Variable Declaration Which is Stored into Contracts Table as on [18-10-2013]

                #region  Begin Code For V-D   (1) to (5)  : Ref Clientid To ContractID

                var ClientId = "";
                var ContractStartDate = "01/01/1900";
                var ContractEndDate = "01/01/1900";
                var BGAmount = "0";
                var ContractId = "";

                #endregion  End Code For V-D   (1) to (5)  : Ref Clientid To ContractID

                #region  Begin Code For V-D   (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                var SecurityDeposit = "0";
                var TypeOfWork = "";
                var MaterialCostPerMonth = "0";
                var ValidityDate = "01/01/1900";
                var MachinaryCostPerMonth = "0";

                #endregion  End Code For V-D   (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                #region  Begin Code For V-D   (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                var EMDValue = "0";
                var PaymentType = "0";
                var WageAct = "0";
                var PayLumpsum = "0";
                var PerformanceGuaranty = "0";

                #endregion  End Code For V-D   (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                #region  Begin Code For V-D     : Ref PF To ESI On OT

                var PF = "0";
                var PFFrom = 0;
                var PFonOT = 0;
                var ESI = "0";
                var ESIFrom = 0;
                var ESIonOt = 0;

                var Pflimit = "0";
                var Esilimit = "0";
                var Bpf = 0;
                var Besi = 0;
                var RelChrg = 0;

                #endregion  End Code For V-D     : Ref PF To ESI On OT

                #region    Begin Code For V-D     : Ref    Servicharge  Yes To   Service Chagre Amount
                var ServiceChargeType = 0;
                var ServiceCharge = "0";
                var ServiceDesc = "Service Charge";
                var BillDates = 0;
                var PaySheetDates = 0;
                var WageType = 0;
                var ProfTax = 0;
                var SProfTax = 0;
                #endregion   End  Code For V-D     : Ref    Servicharge  Yes To   Service Chagre Amount

                #region    Begin Code For V-D     : Ref    ServiceTaxType To   TL No

                var ServiceTaxType = 0;
                var IncludeST = 0;
                var ServiceTax75 = 0;
                var OTPersent = 0;
                var OWF = "";
                var Admincharges = "";
                var OTAmounttype = 0;
                var Description = "";
                var ContractDescription = "";
                var otsalaryratecheck = 0;
                var otsalaryrat = "0";
                var ojt = 0;
                var TL = 0;
                var TLNo = "0";

                #endregion   End  Code For V-D     : Ref       ServiceTaxType To   Description

                #region    Begin Code For V-D     : Ref    New Field add in Contract on 29/03/2014 by venkat

                var Tds = 0;
                var Pono = "";
                var PoDate = "";
                var ReceiptExpectedDate = 0;
                var Staxonservicecharge = 0;
                var SCOnPFESIEmpr = 0;
                var Lumpsumtext = "";
                var TDSon = 0;
                #endregion   End  Code For V-D     : Ref       New Field add in Contract on 29/03/2014 by venkat

                #region Begin Code for Esi branche adding as on 02/08/2014
                var Esibranch = "0";
                var pdfs = 0;
                var WithRoundoff = 0;
                var WagesCalnOn = 0;
                var PTOn = 0;
                var NoUniform = 0;
                var NoSalAdv = 0;
                var NoLWF = 0;
                var NoRegFee = 0;
                #endregion

                #region for Gst
                var CGST = 0;
                var SGST = 0;
                var IGST = 0;
                var Cess1 = 0;
                var Cess2 = 0;
                var GSTLineItem = 0;
                #endregion for Gst


                #region for inv headings

                var InvDescription = "";
                var InvNoOfEmps = "";
                var InvNoofDuties = "";
                var InvPayrate = "";
                var InvAmount = "";
                var InvMonthDays = "";
                var InvSacCode = "";
                var InvDescriptionVisibility = "N";
                var InvNoOfEmpsVisibility = "N";
                var InvNoofDutiesVisibility = "N";
                var InvPayrateVisibility = "N";
                var InvAmountVisibility = "N";

                #endregion


                #region Begin code For Stored Procedure related Variables declaration as on [18-10-2013]
                Hashtable HtContracts = new Hashtable();
                string SPName = "";
                var IRecordStatus = 0;
                #endregion  End code For Stored Procedure related Variables declaration as on [18-10-2013]

                #endregion End  Code For Variable Declaration Which is Stored into Contracts Table as on [18-10-2013]

                #endregion End Code For Declaring Variables as on [18-10-2013]

                #region begin code for RCM  and RCMText
                var RCM = 0;
                var RCMText = string.Empty;
                #endregion
                #region  Begin Code For Assign Values to The Variables as on [18-10-2013]

                #region Begin Code For A-V (1) to (5)  Ref : ClientId To ConractID
                ClientId = ddlClientidNotincontract.SelectedValue;
                //ContractStartDate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                ContractStartDate = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                //ContractEndDate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                ContractEndDate = Timings.Instance.CheckDateFormat(txtEndingDate.Text);

                BGAmount = txtBGAmount.Text;

                if (ddlClientidNotincontract.SelectedIndex > 0)
                {
                    CloneContractidautogenrate();
                    ContractId = txtclonecontractid.Text;
                }

                #endregion End Code For A-V (1) to (5)  Ref :ClientID To ContractID

                #region Begin Code For A-V (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month
                SecurityDeposit = txtSecurityDeposit.Text;
                TypeOfWork = ddltypeofwork.SelectedValue;

                MaterialCostPerMonth = txtMaterial.Text;
                if (txtValidityDate.Text.Trim().Length != 0)
                {
                    //ValidityDate = DateTime.Parse(txtValidityDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    ValidityDate = Timings.Instance.CheckDateFormat(txtValidityDate.Text);

                }

                MachinaryCostPerMonth = txtMachinary.Text;

                #endregion End Code For A-V (6) to (10)  Ref : Security Deposit To Machinary Cose Per Month

                #region Begin Code For A-V (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                EMDValue = txtEMDValue.Text;
                if (RadioLumpsum.Checked)
                {
                    PaymentType = "1";
                    if (txtLumpsumtext.Text.Trim().Length > 0)
                    {
                        Lumpsumtext = txtLumpsumtext.Text;
                    }
                }

                WageAct = txtWAWA.Text;
                if (RadioLumpsum.Checked)
                    PayLumpsum = txtlampsum.Text.Trim();
                PerformanceGuaranty = txtPerGurante.Text;

                #endregion End Code For A-V (11) to (15)  Ref : EMD Value To PerFormance Guarantee

                #region Begin Code For A-V   Ref : Ref PF To ESI On OT

                if (TxtPf.Text.Trim().Length > 0)
                {
                    PF = TxtPf.Text;
                }

                PFFrom = DdlPf.SelectedIndex;

                //if (checkPFonOT.Checked)
                //    PFonOT = 1;
                PFonOT = ddlpfon.SelectedIndex;
                if (TxtEsi.Text.Trim().Length > 0)
                {
                    ESI = TxtEsi.Text;
                }
                ESIFrom = DdlEsi.SelectedIndex;

                // if (checkESIonOT.Checked)
                //    ESIonOt = 1;
                ESIonOt = ddlesion.SelectedIndex;

                if (Chkpf.Checked)
                    Bpf = 1;

                if (ChkEsi.Checked)
                    Besi = 1;

                if (txtPfLimit.Text.Trim().Length > 0)
                    Pflimit = txtPfLimit.Text;
                if (txtEsiLimit.Text.Trim().Length > 0)
                    Esilimit = txtEsiLimit.Text;
                if (chkrc.Checked)
                    RelChrg = 1;


                #endregion End Code For A-V   Ref : Ref PF To ESI On OT

                #region Begin Code For A-V   Ref : Ref  ServiceChargeType  To SProfTax

                if (radioyes.Checked)
                {
                    if (RadioPercent.Checked)
                    {
                        ServiceCharge = txtservicecharge.Text.Trim();
                        ServiceChargeType = 0;
                        ServiceDesc = txtservicedesc.Text;
                    }
                    else
                    {
                        ServiceCharge = txtservicecharge.Text.Trim();
                        ServiceChargeType = 1;
                        ServiceDesc = txtservicedesc.Text;
                    }

                    if (chkStaxonservicecharge.Checked)
                    {
                        Staxonservicecharge = 1;
                    }

                    if (chksconpfesi.Checked)
                    {
                        SCOnPFESIEmpr = 1;
                    }

                }

                //if (RadioStartDate.Checked == true)
                //    BillDates = 1;
                BillDates = ddlbilldates.SelectedIndex;
                PaySheetDates = ddlPaySheetDates.SelectedIndex;

                if (RadioCompany.Checked)
                {
                    WageType = 0;
                }
                else if (RadioClient.Checked)
                {
                    WageType = 1;
                }
                else if (RadioSpecial.Checked)
                {
                    WageType = 2;
                }
                else if (RadioIndividual.Checked)
                {
                    WageType = 3;
                }
                else if (RadioBoth.Checked)
                {
                    WageType = 4;
                }

                if (chkProfTax.Checked)
                    ProfTax = 1;

                if (chkspt.Checked)
                    SProfTax = 1;

                #endregion End Code For A-V   Ref : Ref  ServiceChargeType  To SProfTax

                #region Begin Code For A-V   Ref : Ref  ServiceTaxType  To TL No

                if (RadioWithoutST.Checked)
                {
                    ServiceTaxType = 1;
                }

                if (CheckIncludeST.Checked)
                {
                    IncludeST = 1;
                }

                if (Check75ST.Checked)
                {
                    ServiceTax75 = 1;
                }

                if (DdlOt.SelectedIndex == 0)
                    OTPersent = 100;
                else
                    OTPersent = 200;

                OWF = txtOWF.Text;
                Admincharges = txtadmincharges.Text;
                if (radiootspecial.Checked)
                {
                    OTAmounttype = 1;
                }
                Description = txtdescription.Text.Trim();
                ContractDescription = TxtContractDescription.Text;

                if (chkotsalaryrate.Checked)
                {
                    otsalaryratecheck = 1;
                    otsalaryrat = txtotsalaryrate.Text;
                }
                if (chkojt.Checked)
                {
                    ojt = 1;
                }

                if (chktl.Checked)
                {
                    TL = 1;
                    TLNo = txttlamt.Text;
                }


                #endregion End Code For A-V   Ref : Ref  ServiceTaxType  To Description


                #region Begin Code For A-V   Ref : Ref  Tds  To Expect date on 29/03/2014 by venkat

                if (txtTds.Text.Trim().Length > 0)
                {
                    Tds = int.Parse(txtTds.Text);
                }

                if (txtPono.Text.Trim().Length > 0)
                {
                    Pono = txtPono.Text;
                }
                //  PoDate = Timings.Instance.CheckDateFormat(txtPoDate.Text);

                if (txtPoDate.Text.Trim().Length > 0)
                {
                    PoDate = txtPoDate.Text;

                }
                if (txtExpectdateofreceipt.Text.Trim().Length > 0)
                {
                    ReceiptExpectedDate = int.Parse(txtExpectdateofreceipt.Text);
                }

                TDSon = ddlTDSon.SelectedIndex;
                #endregion End Code For A-V   Ref : Ref   Tds  To Expecte date

                #region Begin Code for Esi branche adding as on 02/08/2014

                if (ddlEsibranch.SelectedIndex > 0)
                {
                    Esibranch = ddlEsibranch.SelectedValue;
                }

                string PFBranch = "";
                if (ddlPFbranch.SelectedIndex > 0)
                {
                    PFBranch = ddlPFbranch.SelectedValue;
                }


                if (Chkpdfs.Checked)
                {

                    pdfs = 1;
                }
                if (ChkWithRoundoff.Checked)
                {

                    WithRoundoff = 1;
                }

                WagesCalnOn = ddlWagesCalnOn.SelectedIndex;
                PTOn = ddlPTOn.SelectedIndex;

                if (chkNoUnif.Checked == true)
                {
                    NoUniform = 1;
                }
                else
                {
                    NoUniform = 0;
                }
                if (chkNoSal.Checked == true)
                {
                    NoSalAdv = 1;
                }
                else
                {
                    NoSalAdv = 0;
                }

                if (chkNoLWF.Checked == true)
                {
                    NoLWF = 1;
                }
                else
                {
                    NoLWF = 0;
                }

                if (chkNoRegFee.Checked == true)
                {
                    NoRegFee = 1;
                }
                else
                {
                    NoRegFee = 0;
                }


                #endregion

                #region for GST on 20-6-2017 by swathi

                if (chkCGST.Checked)
                {
                    CGST = 1;
                }

                if (RdbSGST.Checked)
                {
                    SGST = 1;
                }

                if (RdbIGST.Checked)
                {
                    IGST = 1;
                }


                if (chkCess1.Checked)
                {
                    Cess1 = 1;
                }

                if (chkCess2.Checked)
                {
                    Cess2 = 1;
                }


                if (chkGSTLineItem.Checked)
                {
                    GSTLineItem = 1;
                }

                #endregion for GST on 20-6-2017 by swathi

                #region for invoice headings

                InvDescription = TxtInvDescription.Text;
                InvNoOfEmps = txtInvNoofEmployees.Text;
                InvPayrate = txtInvPayrate.Text;
                InvNoofDuties = txtInvNoofDuties.Text;
                InvAmount = txtInvAmount.Text;

                InvMonthDays = txtInvmonthdays.Text;
                InvSacCode = txtInvsaccode.Text;

                if (chkInvDesc.Checked)
                {
                    InvDescriptionVisibility = "Y";
                }

                if (chkInvNoofemp.Checked)
                {
                    InvNoOfEmpsVisibility = "Y";
                }

                if (chkInvNoofduties.Checked)
                {
                    InvNoofDutiesVisibility = "Y";
                }

                if (chkInvPayrate.Checked)
                {
                    InvPayrateVisibility = "Y";
                }

                if (chkInvAmount.Checked)
                {
                    InvAmountVisibility = "Y";
                }
                #endregion for invoice headings
                #region for RCM and RCM Text
                if (chkrcm.Checked)
                    RCM = 1;
                RCMText = txtrcmtext.Text;

                #endregion for RCM
                #endregion   End  Code For Assign Values to The Variables as on [18-10-2013]


                #region Begin Code For Hash Table/Sp Parameters As on [18-10-2013]
                #region  Begin Code For H-S-Parameters   (1) to (5)  : Ref Clientid To ContractID

                HtContracts.Add("@ClientId", ClientId);
                HtContracts.Add("@ContractStartDate", ContractStartDate);
                HtContracts.Add("@ContractEndDate", ContractEndDate);
                HtContracts.Add("@BGAmount", BGAmount);
                HtContracts.Add("@ContractId", ContractId);

                #endregion  End Code For  H-S-Parameters   (1) to (5)  : Ref Clientid To ContractID

                #region  Begin Code For H-S-Parameters    (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                HtContracts.Add("@SecurityDeposit", SecurityDeposit);
                HtContracts.Add("@TypeOfWork", TypeOfWork);
                HtContracts.Add("@MaterialCostPerMonth", MaterialCostPerMonth);
                HtContracts.Add("@ValidityDate", ValidityDate);
                HtContracts.Add("@MachinaryCostPerMonth", MachinaryCostPerMonth);

                #endregion  End Code For H-S-Parameters    (6) to (10)  : Ref Security Deposit To Machinary Cost Per Month

                #region  Begin Code For H-S-Parameters    (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                HtContracts.Add("@EMDValue", EMDValue);
                HtContracts.Add("@PaymentType", PaymentType);
                HtContracts.Add("@WageAct", WageAct);
                HtContracts.Add("@PayLumpsum", PayLumpsum);
                HtContracts.Add("@PerformanceGuaranty", PerformanceGuaranty);

                #endregion  End Code For H-S-Parameters    (11) to (15)  : Ref EMD Value To PerFormance Guarantee

                #region  Begin Code For H-S-Parameters     : Ref PF To ESI On OT

                HtContracts.Add("@PF", PF);
                HtContracts.Add("@PFFrom", PFFrom);
                HtContracts.Add("@PFonOT", PFonOT);
                HtContracts.Add("@ESI", ESI);
                HtContracts.Add("@ESIFrom", ESIFrom);
                HtContracts.Add("@ESIonOt", ESIonOt);


                #endregion  End Code For H-S-Parameters      : Ref PF To ESI On OT


                #region    Begin Code For H-S-Parameters     : Ref    Servicharge  Yes To   Service Chagre Amount

                HtContracts.Add("@ServiceChargeType", ServiceChargeType);
                HtContracts.Add("@ServiceCharge", ServiceCharge);
                HtContracts.Add("@ServiceChargeText", ServiceDesc);
                HtContracts.Add("@BillDates", BillDates);
                HtContracts.Add("@PaySheetDate", PaySheetDates);
                HtContracts.Add("@WageType", WageType);
                HtContracts.Add("@ProfTax", ProfTax);
                HtContracts.Add("@SProfTax", SProfTax);
                HtContracts.Add("@RCM", RCM);
                HtContracts.Add("@RCMText", RCMText);
                #endregion   End  Code For   H-S-Parameters    : Ref    Servicharge  Yes To   Service Chagre Amount


                #region    Begin Code For H-S-Parameters    : Ref    ServiceTaxType To   TL No

                HtContracts.Add("@ServiceTaxType", ServiceTaxType);
                HtContracts.Add("@IncludeST", IncludeST);
                HtContracts.Add("@ServiceTax75", ServiceTax75);
                HtContracts.Add("@OTPersent", OTPersent);
                HtContracts.Add("@OWF", OWF);
                HtContracts.Add("@Admincharges", Admincharges);

                HtContracts.Add("@OTAmounttype", OTAmounttype);
                HtContracts.Add("@Description", Description);
                HtContracts.Add("@ContractDescription", ContractDescription);
                HtContracts.Add("@otsalaryratecheck", otsalaryratecheck);
                HtContracts.Add("@otsalaryrat", otsalaryrat);
                HtContracts.Add("@ojt", ojt);
                HtContracts.Add("@tl", TL);
                HtContracts.Add("@tlno", TLNo);
                HtContracts.Add("@PFLimit", Pflimit);
                HtContracts.Add("@ESILimit", Esilimit);
                HtContracts.Add("@Bpf", Bpf);
                HtContracts.Add("@Besi", Besi);
                HtContracts.Add("@RelChrg", RelChrg);

                #endregion   End  Code For H-S-Parameters    : Ref       ServiceTaxType To   Description


                #region    Begin Code For H-S-Parameters    : Ref    Tds To   Lumpsumtext on 29/03/2014 by venkat

                HtContracts.Add("@Tds", Tds);
                HtContracts.Add("@Pono", Pono);
                HtContracts.Add("@PoDate", PoDate);
                HtContracts.Add("@ReceiptExpectedDate", ReceiptExpectedDate);
                HtContracts.Add("@Staxonservicecharge", Staxonservicecharge);
                HtContracts.Add("@SCOnPFESIEmpr", SCOnPFESIEmpr);

                HtContracts.Add("@Lumpsumtext", Lumpsumtext);
                HtContracts.Add("@TDSon", TDSon);
                #endregion   End  Code For H-S-Parameters    : Ref        Tds To   Lumpsumtext on 29/03/2014 by venkat


                HtContracts.Add("@Esibranch", Esibranch);
                HtContracts.Add("@PFBranch", PFBranch);
                HtContracts.Add("@pdfs", pdfs);
                HtContracts.Add("@WagesCalnOn", WagesCalnOn);
                HtContracts.Add("@PTOn", PTOn);
                HtContracts.Add("@WithRoundoff", WithRoundoff);
                HtContracts.Add("@NoUniform", NoUniform);
                HtContracts.Add("@NoSalAdv", NoSalAdv);
                HtContracts.Add("@NoLWF", NoLWF);
                HtContracts.Add("@NoRegFee", NoRegFee);
                HtContracts.Add("@CGST", CGST);
                HtContracts.Add("@IGST", IGST);
                HtContracts.Add("@SGST", SGST);
                HtContracts.Add("@Cess1", Cess1);
                HtContracts.Add("@Cess2", Cess2);
                HtContracts.Add("@GSTLineItem", GSTLineItem);
                #endregion  End  Code For Hash Table/Sp Parameters As on [18-10-2013]


                #region for inv headings

                HtContracts.Add("@InvDescription", InvDescription);
                HtContracts.Add("@InvNoOfEmps", InvNoOfEmps);
                HtContracts.Add("@InvPayrate", InvPayrate);
                HtContracts.Add("@InvSacCode", InvSacCode);
                HtContracts.Add("@InvMonthDays", InvMonthDays);
                HtContracts.Add("@InvNoofDuties", InvNoofDuties);
                HtContracts.Add("@InvAmount", InvAmount);
                HtContracts.Add("@InvDescriptionVisibility", InvDescriptionVisibility);
                HtContracts.Add("@InvNoOfEmpsVisibility", InvNoOfEmpsVisibility);
                HtContracts.Add("@InvPayratevisibility", InvPayrateVisibility);
                HtContracts.Add("@InvNoofDutiesvisibility", InvNoofDutiesVisibility);
                HtContracts.Add("@InvAmountvisibility", InvAmountVisibility);


                #endregion

                #region Begin Code For Calling Stored Procedure as on [18-10-2013]
                SPName = "AddorModifyContracts";
                IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                #endregion End Code For Calling Stored Procedure as on [18-10-2013]




                if (IRecordStatus > 0)
                {


                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Contract Added Successfully ');", true);
                    int j = 0;

                    DateTime today = DateTime.Now.Date;
                    int designCount = Convert.ToInt16(Session["ContractsAIndex"]);
                    //Store Data into Contract Details
                    int clientdesigncount = 0;
                    for (j = 0; j < designCount; j++)
                    {
                        if (j < gvdesignation.Rows.Count)
                        {

                            HtContracts.Clear();

                            string CSno = ((TextBox)gvdesignation.Rows[j].FindControl("lblCSlno")).Text;
                            if (CSno.Trim().Length == 0)
                            {
                                CSno = "0";
                            }


                            #region Clientdesignation
                            string Cddldesignation = ((DropDownList)gvdesignation.Rows[j].FindControl("DdlDesign")).SelectedValue;
                            DropDownList ddlindex = gvdesignation.Rows[j].FindControl("DdlDesign") as DropDownList;
                            if (ddlindex.SelectedIndex == 0)
                            {
                                break;
                            }
                            #endregion


                            #region   Duty Hrs
                            string Cdutyhrs = ((TextBox)gvdesignation.Rows[j].FindControl("txtdutyhrs")).Text;
                            if (Cdutyhrs.Trim().Length == 0)
                            {
                                Cdutyhrs = "";
                            }
                            #endregion

                            #region   Quantity
                            string Cquantity = ((TextBox)gvdesignation.Rows[j].FindControl("txtquantity")).Text;
                            if (Cquantity.Trim().Length == 0)
                            {
                                //lblmsgcontractdetails.Text = "Please enter No. of employee needed";
                                // break;
                                //Cquantity = "0";
                            }
                            else
                            {
                                float tempQty = Convert.ToSingle(Cquantity);
                                if (tempQty < 0)
                                {
                                    lblMsg.Text = "No. of employee needed can't be  negative";
                                    break;
                                }
                            }
                            #endregion
                            //string Cddldutytype = ((DropDownList)gvdesignation.Rows[j].FindControl("ddldutytype")).Text;
                            #region PayType
                            DropDownList ddlPayType = gvdesignation.Rows[j].FindControl("ddldutytype") as DropDownList;
                            int PayType = ddlPayType.SelectedIndex;
                            #endregion

                            #region  No Of Days For Billing
                            DropDownList ddlNoOfDaysForBilling = gvdesignation.Rows[j].FindControl("ddlNoOfDaysBilling") as DropDownList;
                            var NoOfDaysForBilling = "0";

                            if (ddlNoOfDaysForBilling.SelectedIndex == 0)
                            {
                                NoOfDaysForBilling = "0";
                            }

                            if (ddlNoOfDaysForBilling.SelectedIndex == 1)
                            {
                                NoOfDaysForBilling = "1";
                            }
                            if (ddlNoOfDaysForBilling.SelectedIndex == 2)
                            {
                                NoOfDaysForBilling = "2";
                            }
                            if (ddlNoOfDaysForBilling.SelectedIndex == 3)
                            {
                                NoOfDaysForBilling = "3";
                            }

                            if (ddlNoOfDaysForBilling.SelectedIndex > 3)
                            {
                                NoOfDaysForBilling = ddlNoOfDaysForBilling.SelectedValue.ToString();
                            }
                            #endregion

                            #region ClientType
                            string CddlType = ((DropDownList)gvdesignation.Rows[j].FindControl("ddlType")).SelectedValue;
                            DropDownList ddlType = gvdesignation.Rows[j].FindControl("ddlType") as DropDownList;
                            CddlType = ddlType.SelectedValue;
                            #endregion

                            #region  No Of Ots
                            string NoOfOts = "0";
                            DropDownList ddlNoOfOtsPaysheet = gvdesignation.Rows[j].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 0)
                            {
                                NoOfOts = "0";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 1)
                            {
                                NoOfOts = "1";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 2)
                            {
                                NoOfOts = "2";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 3)
                            {
                                NoOfOts = "3";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 4)
                            {
                                NoOfOts = "4";
                            }

                            if (ddlNoOfOtsPaysheet.SelectedIndex > 4)
                            {
                                NoOfOts = ddlNoOfOtsPaysheet.SelectedValue;
                            }
                            #endregion No Of Ots




                            #region Summary
                            string Csummary = ((TextBox)gvdesignation.Rows[j].FindControl("txtsummary")).Text;
                            //string Camount = ((TextBox)gvdesignation.Rows[j].FindControl("txtamount")).Text;
                            if (Csummary.Trim().Length == 0)
                            {
                                Csummary = "";
                            }
                            #endregion

                            #region  PayRate
                            string strPayRate = ((TextBox)gvdesignation.Rows[j].FindControl("txtPayRate")).Text;
                            if (RadioLumpsum.Checked == false)
                            {
                                if (strPayRate.Trim().Length == 0)
                                {
                                    lblMsg.Text = "Please enter Pay Rate for employee";
                                    break;
                                    //Cquantity = "0";
                                }
                                else
                                {
                                    float tempPay = Convert.ToSingle(strPayRate);
                                    if (tempPay <= 0)
                                    {
                                        lblMsg.Text = "Pay Rate of employee can't be 0 or negative";
                                        break;
                                    }
                                }
                            }
                            else
                                strPayRate = "0";

                            #endregion

                            #region RC Applicable
                            var CRCApplicable = 0;
                            CheckBox ChkRCApplicable = (CheckBox)gvdesignation.Rows[j].FindControl("ChkRCApplicable");
                            if (ChkRCApplicable.Checked)
                            {
                                CRCApplicable = 1;

                            }
                            #endregion

                            #region  Basic
                            string Cbasic = ((TextBox)gvdesignation.Rows[j].FindControl("TxtBasic")).Text;
                            //if (RadioLumpsum.Checked == false)
                            //{
                            //    if (Cbasic.Trim().Length == 0)
                            //    {
                            //        if (RadioClient.Checked)
                            //        {
                            //            lblMsg.Text = "Please enter basic pay for employee";
                            //            break;
                            //        }
                            //        Cbasic = "0";
                            //    }
                            //    else
                            //    {
                            //        if (RadioClient.Checked)
                            //        {
                            //            float tempBaic = Convert.ToSingle(Cbasic);
                            //            if (tempBaic <= 0)
                            //            {
                            //                lblMsg.Text = "Basic pay can't be 0 or negative";
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
                            //else
                            //    Cbasic = "0";

                            #endregion

                            #region Da
                            string Cda = ((TextBox)gvdesignation.Rows[j].FindControl("txtda")).Text;
                            if (Cda.Trim().Length == 0)
                            {
                                Cda = "0";
                            }
                            #endregion

                            #region Hra
                            string Chra = ((TextBox)gvdesignation.Rows[j].FindControl("txthra")).Text;
                            if (Chra.Trim().Length == 0)
                            {
                                Chra = "0";
                            }
                            #endregion

                            #region Conveyance
                            string CConveyance = ((TextBox)gvdesignation.Rows[j].FindControl("txtConveyance")).Text;
                            if (CConveyance.Trim().Length == 0)
                            {
                                CConveyance = "0";
                            }
                            #endregion

                            #region Other Allowance
                            string Caw = ((TextBox)gvdesignation.Rows[j].FindControl("txtoa")).Text;
                            if (Caw.Trim().Length == 0)
                            {
                                Caw = "0";
                            }
                            #endregion

                            #region Wash Allowance
                            string Cwashallowance = ((TextBox)gvdesignation.Rows[j].FindControl("txtwa")).Text;
                            if (Cwashallowance.Trim().Length == 0)
                            {
                                Cwashallowance = "0";
                            }
                            #endregion

                            #region CCA
                            string Cca = ((TextBox)gvdesignation.Rows[j].FindControl("txtcca")).Text;
                            if (Cca.Trim().Length == 0)
                            {
                                Cca = "0";
                            }
                            #endregion

                            #region LeaveAmount

                            string LeaveAmount = ((TextBox)gvdesignation.Rows[j].FindControl("txtleaveamount")).Text;
                            if (LeaveAmount.Trim().Length == 0)
                            {
                                LeaveAmount = "0";
                            }
                            #endregion

                            #region Gratituty
                            string Gratuty = ((TextBox)gvdesignation.Rows[j].FindControl("txtgratuty")).Text;
                            if (Gratuty.Trim().Length == 0)
                            {
                                Gratuty = "0";
                            }
                            #endregion

                            #region Bonus
                            string Bonus = ((TextBox)gvdesignation.Rows[j].FindControl("txtbonus")).Text;
                            if (Bonus.Trim().Length == 0)
                            {
                                Bonus = "0";
                            }

                            string AttBonus = ((TextBox)gvdesignation.Rows[j].FindControl("txtattbonus")).Text;
                            if (AttBonus.Trim().Length == 0)
                            {
                                AttBonus = "0";
                            }
                            #endregion

                            #region NFhs
                            string Nfhsw = ((TextBox)gvdesignation.Rows[j].FindControl("txtNfhs")).Text;
                            if (Nfhsw.Trim().Length == 0)
                            {
                                Nfhsw = "0";
                            }


                            #endregion


                            #region BEgin RC
                            string RC = ((TextBox)gvdesignation.Rows[j].FindControl("Txtrc")).Text;
                            if (RC.Trim().Length == 0)
                            {
                                RC = "0";
                            }

                            #endregion End Rc



                            #region Begin CS
                            string CS = ((TextBox)gvdesignation.Rows[j].FindControl("TxtCs")).Text;
                            if (CS.Trim().Length == 0)
                            {
                                CS = "0";
                            }

                            string CSper = ((TextBox)gvdesignation.Rows[j].FindControl("TxtScPer")).Text;
                            if (CSper.Trim().Length == 0)
                            {
                                CSper = "0";
                            }

                            #endregion End CS

                            #region Begin OT RATE
                            string OTRATE = ((TextBox)gvdesignation.Rows[j].FindControl("TxtOTRate")).Text;
                            if (OTRATE.Trim().Length == 0)
                            {
                                OTRATE = "0";
                            }
                            #endregion End OT RATE

                            double payRate = double.Parse(strPayRate);

                            #region Begin CGST

                            var cdCGST = 0;

                            CheckBox chkcdCGST = gvdesignation.Rows[j].FindControl("chkcdCGST") as CheckBox;
                            if (chkcdCGST.Checked)
                            {
                                cdCGST = 1;
                            }


                            var cdSGST = 0;

                            CheckBox chkcdSGST = gvdesignation.Rows[j].FindControl("chkcdSGST") as CheckBox;
                            if (chkcdSGST.Checked)
                            {
                                cdSGST = 1;
                            }


                            var cdIGST = 0;

                            CheckBox chkcdIGST = gvdesignation.Rows[j].FindControl("chkcdIGST") as CheckBox;
                            if (chkcdIGST.Checked)
                            {
                                cdIGST = 1;
                            }

                            string HSNNumber = ((DropDownList)gvdesignation.Rows[j].FindControl("ddlHSNNumber")).SelectedValue;


                            #endregion End GST

                            #region PF & ESI
                            var cdPF = 0;

                            CheckBox chkcdPF = gvdesignation.Rows[j].FindControl("chkcdPF") as CheckBox;
                            if (chkcdPF.Checked)
                            {
                                cdPF = 1;
                            }


                            var cdESI = 0;

                            CheckBox chkcdESI = gvdesignation.Rows[j].FindControl("chkcdESI") as CheckBox;
                            if (chkcdESI.Checked)
                            {
                                cdESI = 1;
                            }

                            var cdSC = 0;

                            CheckBox chkcdSC = gvdesignation.Rows[j].FindControl("chkcdSC") as CheckBox;
                            if (chkcdSC.Checked)
                            {
                                cdSC = 1;
                            }

                            var cdSCOnPFESI = 0;

                            CheckBox chkcdSCOnPFESI = gvdesignation.Rows[j].FindControl("chkcdSCOnPFESI") as CheckBox;
                            if (chkcdSCOnPFESI.Checked)
                            {
                                cdSCOnPFESI = 1;
                            }

                            #endregion PF & ESI

                            #region  STored Procedure Parameters , connection Strings
                            SPName = "GetContractDetails";
                            #region Parameters 1 -8
                            HtContracts.Add("@clientid", ClientId);
                            HtContracts.Add("@Contractid", ContractId);
                            HtContracts.Add("@Designations", Cddldesignation);
                            HtContracts.Add("@DutyHrs", Cdutyhrs);
                            HtContracts.Add("@RCApplicable", CRCApplicable);
                            HtContracts.Add("@Quantity", Cquantity);
                            HtContracts.Add("@basic", Cbasic);
                            HtContracts.Add("@da", Cda);
                            HtContracts.Add("@hra", Chra);
                            HtContracts.Add("@conveyance", CConveyance);

                            HtContracts.Add("@sno", CSno);

                            #endregion

                            #region Parameters 8-16
                            HtContracts.Add("@washallownce", Cwashallowance);
                            HtContracts.Add("@OtherAllowance", Caw);
                            HtContracts.Add("@Summary", Csummary);
                            HtContracts.Add("@Amount", payRate);
                            #endregion

                            #region Parameters 16-24
                            HtContracts.Add("@cca", Cca);
                            HtContracts.Add("@leaveamount", LeaveAmount);
                            HtContracts.Add("@bonus", Bonus);
                            HtContracts.Add("@gratuity", Gratuty);
                            HtContracts.Add("@PayType", PayType);
                            HtContracts.Add("@NoOfDays", NoOfDaysForBilling);
                            HtContracts.Add("@NFhs", Nfhsw);
                            HtContracts.Add("@testrecord", clientdesigncount);

                            HtContracts.Add("@Nots", NoOfOts);
                            HtContracts.Add("@RC", RC);
                            HtContracts.Add("@CS", CS);
                            HtContracts.Add("@OTRATE", OTRATE);
                            HtContracts.Add("@attbonus", AttBonus);
                            HtContracts.Add("@servicechargeper", CSper);
                            HtContracts.Add("@Type", CddlType);
                            HtContracts.Add("@cdCGST", cdCGST);
                            HtContracts.Add("@cdSGST", cdSGST);
                            HtContracts.Add("@cdIGST", cdIGST);
                            HtContracts.Add("@HSNNumber", HSNNumber);
                            HtContracts.Add("@cdPF", cdPF);
                            HtContracts.Add("@cdESI", cdESI);
                            HtContracts.Add("@cdSC", cdSC);
                            HtContracts.Add("@cdSCOnPFESI", cdSCOnPFESI);


                            #endregion

                            IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                            if (IRecordStatus != 0)
                            {
                                clientdesigncount++;
                            }
                            #endregion

                        }
                    }

                    #region   Contract Special Wise Designations

                    int designCountsw = Convert.ToInt16(Session["ContractsAIndexsw"]);
                    int specialdesigncount = 0;
                    for (j = 0; j < designCountsw; j++)
                    {
                        if (j < gvSWDesignations.Rows.Count)
                        {
                            HtContracts.Clear();
                            #region  Client Designations
                            string Cddldesignationsw = ((DropDownList)gvSWDesignations.Rows[j].FindControl("DdlDesign")).SelectedValue;
                            DropDownList ddlindexsw = gvSWDesignations.Rows[j].FindControl("DdlDesign") as DropDownList;
                            if (ddlindexsw.SelectedIndex == 0)
                            {
                                break;
                            }
                            #endregion

                            #region  Client Category
                            string Cddlcategoriessw = ((DropDownList)gvSWDesignations.Rows[j].FindControl("DdlCategory")).SelectedValue;
                            DropDownList ddlcategoriessw = gvSWDesignations.Rows[j].FindControl("DdlCategory") as DropDownList;
                            if (ddlcategoriessw.SelectedIndex == 0)
                            {
                                Cddlcategoriessw = "0";
                            }
                            #endregion

                            #region  Begin  No Of Days For Wages
                            DropDownList ddlNoOfDaysForWages = gvSWDesignations.Rows[j].FindControl("ddlNoOfDaysWages") as DropDownList;
                            string NoOfDaysForWages = "0";

                            if (ddlNoOfDaysForWages.SelectedIndex == 0)
                            {
                                NoOfDaysForWages = "0";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 1)
                            {
                                NoOfDaysForWages = "1";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 2)
                            {
                                NoOfDaysForWages = "2";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 3)
                            {
                                NoOfDaysForWages = "3";
                            }
                            if (ddlNoOfDaysForWages.SelectedIndex == 4)
                            {
                                NoOfDaysForWages = "4";
                            }

                            if (ddlNoOfDaysForWages.SelectedIndex > 4)
                            {
                                NoOfDaysForWages = ddlNoOfDaysForWages.SelectedValue;
                            }
                            #endregion  //End  No Of Days For Wages

                            #region  No Of Ots
                            string NoOfOts = "0";
                            DropDownList ddlNoOfOtsPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfOtsPaysheet") as DropDownList;

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 0)
                            {
                                NoOfOts = "0";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 1)
                            {
                                NoOfOts = "1";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 2)
                            {
                                NoOfOts = "2";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 3)
                            {
                                NoOfOts = "3";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex == 4)
                            {
                                NoOfOts = "4";
                            }

                            if (ddlNoOfOtsPaysheet.SelectedIndex == 5)
                            {
                                NoOfOts = "5";
                            }
                            if (ddlNoOfOtsPaysheet.SelectedIndex > 5)
                            {
                                NoOfOts = ddlNoOfOtsPaysheet.SelectedValue;
                            }
                            #endregion No Of Ots


                            #region No Of Nhs
                            string NoOfNhs = "0";
                            DropDownList ddlNoOfNhsPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfNhsPaysheet") as DropDownList;

                            if (ddlNoOfNhsPaysheet.SelectedIndex == 0)
                            {
                                NoOfNhs = "0";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 1)
                            {
                                NoOfNhs = "1";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 2)
                            {
                                NoOfNhs = "2";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 3)
                            {
                                NoOfNhs = "3";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex == 4)
                            {
                                NoOfNhs = "4";
                            }
                            if (ddlNoOfNhsPaysheet.SelectedIndex > 4)
                            {
                                NoOfNhs = ddlNoOfNhsPaysheet.SelectedValue;
                            }
                            #endregion of No Of Nhs

                            #region for NoOfWos
                            string NoOfWos = "0";
                            DropDownList ddlNoOfWosPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfWosPaysheet") as DropDownList;

                            if (ddlNoOfWosPaysheet.SelectedIndex == 0)
                            {
                                NoOfWos = "0";
                            }
                            if (ddlNoOfWosPaysheet.SelectedIndex == 1)
                            {
                                NoOfWos = "1";
                            }
                            if (ddlNoOfWosPaysheet.SelectedIndex == 2)
                            {
                                NoOfWos = "2";
                            }
                            if (ddlNoOfWosPaysheet.SelectedIndex == 3)
                            {
                                NoOfWos = "3";
                            }
                            if (ddlNoOfWosPaysheet.SelectedIndex == 4)
                            {
                                NoOfWos = "4";
                            }
                            if (ddlNoOfWosPaysheet.SelectedIndex > 4)
                            {
                                NoOfWos = ddlNoOfWosPaysheet.SelectedValue;
                            }
                            #endregion for NoOfWos


                            #region for NoOfpfdays
                            string NoOfPFDays = "0";
                            DropDownList ddlNoOfPFDaysPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfPFDaysPaysheet") as DropDownList;

                            if (ddlNoOfPFDaysPaysheet.SelectedIndex == 0)
                            {
                                NoOfPFDays = "0";
                            }
                            if (ddlNoOfPFDaysPaysheet.SelectedIndex == 1)
                            {
                                NoOfPFDays = "1";
                            }
                            if (ddlNoOfPFDaysPaysheet.SelectedIndex == 2)
                            {
                                NoOfPFDays = "2";
                            }
                            if (ddlNoOfPFDaysPaysheet.SelectedIndex == 3)
                            {
                                NoOfPFDays = "3";
                            }
                            if (ddlNoOfPFDaysPaysheet.SelectedIndex == 4)
                            {
                                NoOfPFDays = "4";
                            }
                            if (ddlNoOfPFDaysPaysheet.SelectedIndex > 4)
                            {
                                NoOfPFDays = ddlNoOfPFDaysPaysheet.SelectedValue;
                            }
                            #endregion for NoOfpfdays

                            #region for NoOfesidays
                            string NoOfESIDays = "0";
                            DropDownList ddlNoOfESIDaysPaysheet = gvSWDesignations.Rows[j].FindControl("ddlNoOfESIDaysPaysheet") as DropDownList;

                            if (ddlNoOfESIDaysPaysheet.SelectedIndex == 0)
                            {
                                NoOfESIDays = "0";
                            }
                            if (ddlNoOfESIDaysPaysheet.SelectedIndex == 1)
                            {
                                NoOfESIDays = "1";
                            }
                            if (ddlNoOfESIDaysPaysheet.SelectedIndex == 2)
                            {
                                NoOfESIDays = "2";
                            }
                            if (ddlNoOfESIDaysPaysheet.SelectedIndex == 3)
                            {
                                NoOfESIDays = "3";
                            }
                            if (ddlNoOfESIDaysPaysheet.SelectedIndex == 4)
                            {
                                NoOfESIDays = "4";
                            }
                            if (ddlNoOfESIDaysPaysheet.SelectedIndex > 4)
                            {
                                NoOfESIDays = ddlNoOfESIDaysPaysheet.SelectedValue;
                            }


                            string OTESIWagesDays = "0";
                            DropDownList ddlOTESIWagesDays = gvSWDesignations.Rows[j].FindControl("ddlOTESIWagesDays") as DropDownList;

                            if (ddlOTESIWagesDays.SelectedIndex == 0)
                            {
                                OTESIWagesDays = "0";
                            }
                            if (ddlOTESIWagesDays.SelectedIndex == 1)
                            {
                                OTESIWagesDays = "1";
                            }
                            if (ddlOTESIWagesDays.SelectedIndex == 2)
                            {
                                OTESIWagesDays = "2";
                            }
                            if (ddlOTESIWagesDays.SelectedIndex == 3)
                            {
                                OTESIWagesDays = "3";
                            }
                            if (ddlOTESIWagesDays.SelectedIndex == 4)
                            {
                                OTESIWagesDays = "4";
                            }
                            if (ddlOTESIWagesDays.SelectedIndex == 5)
                            {
                                OTESIWagesDays = "5";
                            }

                            if (ddlOTESIWagesDays.SelectedIndex == 6)
                            {
                                OTESIWagesDays = "6";
                            }

                            if (ddlOTESIWagesDays.SelectedIndex == 7)
                            {
                                OTESIWagesDays = "7";
                            }

                            if (ddlOTESIWagesDays.SelectedIndex == 8)
                            {
                                OTESIWagesDays = "8";
                            }
                            if (ddlOTESIWagesDays.SelectedIndex > 8)
                            {
                                OTESIWagesDays = ddlOTESIWagesDays.SelectedValue;
                            }

                            #endregion for NoOfesidays

                            #region  gross
                            string Cgrosssw = ((TextBox)gvSWDesignations.Rows[j].FindControl("Txtgross")).Text;
                            if (Cgrosssw.Trim().Length == 0)
                            {
                                Cgrosssw = "0";
                            }

                            #endregion

                            string CNetPaysw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtNetPay")).Text;
                            if (CNetPaysw.Trim().Length == 0)
                            {
                                CNetPaysw = "0";
                            }

                            #region for NoOfSplDays
                            string NoOfSplDays = "0";
                            DropDownList ddlSplAllwDays = gvSWDesignations.Rows[j].FindControl("ddlSplAllwDays") as DropDownList;

                            if (ddlSplAllwDays.SelectedIndex == 0)
                            {
                                NoOfSplDays = "0";
                            }
                            if (ddlSplAllwDays.SelectedIndex == 1)
                            {
                                NoOfSplDays = "1";
                            }
                            if (ddlSplAllwDays.SelectedIndex == 2)
                            {
                                NoOfSplDays = "2";
                            }
                            if (ddlSplAllwDays.SelectedIndex == 3)
                            {
                                NoOfSplDays = "3";
                            }
                            if (ddlSplAllwDays.SelectedIndex == 4)
                            {
                                NoOfSplDays = "4";
                            }
                            if (ddlSplAllwDays.SelectedIndex > 4)
                            {
                                NoOfSplDays = ddlSplAllwDays.SelectedValue;
                            }
                            #endregion for NoOfSplDays

                            #region for Bonus,Gratuity and LA Type
                            int BonusType = 0;
                            DropDownList ddlbonustype = gvSWDesignations.Rows[j].FindControl("ddlbonustype") as DropDownList;
                            BonusType = ddlbonustype.SelectedIndex;

                            int AttBonusType = 0;
                            DropDownList ddlAttbonustype = gvSWDesignations.Rows[j].FindControl("ddlAttbonustype") as DropDownList;
                            AttBonusType = ddlAttbonustype.SelectedIndex;

                            int GratuityType = 0;
                            DropDownList ddlgratuitytype = gvSWDesignations.Rows[j].FindControl("ddlgratuitytype") as DropDownList;
                            GratuityType = ddlgratuitytype.SelectedIndex;

                            int LAType = 0;
                            DropDownList ddllatype = gvSWDesignations.Rows[j].FindControl("ddllatype") as DropDownList;
                            LAType = ddllatype.SelectedIndex;

                            #endregion

                            #region  Basic
                            string Cbasicsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtBasic")).Text;
                            if (Cbasicsw.Trim().Length == 0)
                            {
                                Cbasicsw = "0";
                            }

                            #endregion

                            #region DA
                            string Cdasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtda")).Text;
                            if (Cdasw.Trim().Length == 0)
                            {
                                Cdasw = "0";
                            }
                            #endregion

                            #region  Hra
                            string Chrasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txthra")).Text;
                            if (Chrasw.Trim().Length == 0)
                            {
                                Chrasw = "0";
                            }

                            #endregion
                            #region Conveyance
                            string CConveyancesw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtConveyance")).Text;
                            if (CConveyancesw.Trim().Length == 0)
                            {
                                CConveyancesw = "0";
                            }
                            #endregion

                            #region Other Allowance
                            string Cawsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtoa")).Text;
                            if (Cawsw.Trim().Length == 0)
                            {
                                Cawsw = "0";
                            }
                            #endregion

                            #region Wash Allowance

                            string Cwashallowancesw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtwa")).Text;
                            if (Cwashallowancesw.Trim().Length == 0)
                            {
                                Cwashallowancesw = "0";
                            }

                            #endregion

                            #region CCa
                            string Ccasw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtcca")).Text;
                            if (Ccasw.Trim().Length == 0)
                            {
                                Ccasw = "0";
                            }
                            #endregion

                            #region Leave Amount
                            string LeaveAmountsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtleaveamount")).Text;
                            if (LeaveAmountsw.Trim().Length == 0)
                            {
                                LeaveAmountsw = "0";
                            }
                            #endregion

                            #region gratituty
                            string Gratutysw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtgratuty")).Text;
                            if (Gratutysw.Trim().Length == 0)
                            {
                                Gratutysw = "0";
                            }
                            #endregion

                            #region Bonus
                            string Bonussw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtbonus")).Text;
                            if (Bonussw.Trim().Length == 0)
                            {
                                Bonussw = "0";
                            }

                            string AttBonussw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtattbonus")).Text;
                            if (AttBonussw.Trim().Length == 0)
                            {
                                AttBonussw = "0";
                            }
                            #endregion
                            #region NFHs
                            String Nfhssw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtNfhs1")).Text;
                            if (Nfhssw.Trim().Length == 0)
                            {
                                Nfhssw = "0";
                            }



                            #endregion

                            #region BEgin RC
                            string RC = ((TextBox)gvSWDesignations.Rows[j].FindControl("Txtrc")).Text;
                            if (RC.Trim().Length == 0)
                            {
                                RC = "0";
                            }

                            #endregion End Rc



                            #region Begin CS
                            string CS = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtCs")).Text;
                            if (CS.Trim().Length == 0)
                            {
                                CS = "0";
                            }

                            string CSpersw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtScPer")).Text;
                            if (CSpersw.Trim().Length == 0)
                            {
                                CSpersw = "0";
                            }
                            #endregion End CS

                            #region for PLDays and PLAmount

                            string CSWPLDays = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtCSWPLDays")).Text;
                            if (CSWPLDays.Trim().Length == 0)
                            {
                                CSWPLDays = "0";
                            }


                            string CSWPLAmount = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtCSWPLAmount")).Text;
                            if (CSWPLAmount.Trim().Length == 0)
                            {
                                CSWPLAmount = "0";
                            }

                            #endregion for PLDays and PLAmount


                            #region for TLDays and TLAmount

                            string CSWTLDays = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtCSWTLDays")).Text;
                            if (CSWTLDays.Trim().Length == 0)
                            {
                                CSWTLDays = "0";
                            }


                            string CSWTLAmount = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtCSWTLAmount")).Text;
                            if (CSWTLAmount.Trim().Length == 0)
                            {
                                CSWTLAmount = "0";
                            }


                            #endregion for TLDays and TLAmount

                            #region for NHSRate and WORate
                            string CtxtTLnhsamt = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtNHSRate")).Text;
                            if (CtxtTLnhsamt.Trim().Length == 0)
                            {
                                CtxtTLnhsamt = "0";
                            }

                            string CtxtTLwoamt = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtWORate")).Text;
                            if (CtxtTLwoamt.Trim().Length == 0)
                            {
                                CtxtTLwoamt = "0";
                            }

                            #endregion for NHSRate and WORate

                            #region for FoodAloowance and Medical Allowance
                            string CtxtTLfoodamt = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtFoodAllowance")).Text;
                            if (CtxtTLfoodamt.Trim().Length == 0)
                            {
                                CtxtTLfoodamt = "0";
                            }

                            string CtxtTLmedicalamt = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtMedicalAllowance")).Text;
                            if (CtxtTLmedicalamt.Trim().Length == 0)
                            {
                                CtxtTLmedicalamt = "0";
                            }
                            string splallw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtSplAllowance")).Text;
                            if (splallw.Trim().Length == 0)
                            {
                                splallw = "0";
                            }

                            string travelallw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtTravelAllowance")).Text;
                            if (travelallw.Trim().Length == 0)
                            {
                                travelallw = "0";
                            }

                            string performanceallw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtPerformanceAllowance")).Text;
                            if (performanceallw.Trim().Length == 0)
                            {
                                performanceallw = "0";
                            }
                            string mobileallw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtMobileAllowance")).Text;
                            if (mobileallw.Trim().Length == 0)
                            {
                                mobileallw = "0";
                            }
                            #endregion for FoodAloowance and Medical Allowance

                            #region for ADVDed and WCDed
                            string CtxtTLadvded = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtADVDed")).Text;
                            if (CtxtTLadvded.Trim().Length == 0)
                            {
                                CtxtTLadvded = "0";
                            }

                            string CtxtTLwcded = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtWCDed")).Text;
                            if (CtxtTLwcded.Trim().Length == 0)
                            {
                                CtxtTLwcded = "0";
                            }
                            #endregion for ADVDed and WCDed

                            #region for uniformded
                            string CtxtTLunidedsw = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtUniformDed")).Text;
                            if (CtxtTLunidedsw.Trim().Length == 0)
                            {
                                CtxtTLunidedsw = "0";
                            }
                            #endregion for uniformded

                            #region Begin OT RATE
                            string OTRATE = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtOTRate")).Text;
                            if (OTRATE.Trim().Length == 0)
                            {
                                OTRATE = "0";
                            }
                            #endregion End OT RATE

                            #region for begin OTHrs

                            string OThrs = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtOThrs")).Text;
                            if (OThrs.Trim().Length == 0)
                            {
                                OThrs = "0";
                            }
                            #endregion

                            string PFRate = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtPFRate")).Text;
                            if (PFRate.Trim().Length == 0)
                            {
                                PFRate = "0";
                            }
                            string ESIRate = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtESIRate")).Text;
                            if (ESIRate.Trim().Length == 0)
                            {
                                ESIRate = "0";
                            }

                            string servicewightage = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtservicewt")).Text;
                            if (servicewightage.Trim().Length == 0)
                            {
                                servicewightage = "0";
                            }

                            string HardshipAllw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtHardshipAllw")).Text;
                            if (HardshipAllw.Trim().Length == 0)
                            {
                                HardshipAllw = "0";
                            }

                            string RankAllw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtRankAllw")).Text;
                            if (RankAllw.Trim().Length == 0)
                            {
                                RankAllw = "0";
                            }

                            string uniformcharge = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtuniformcharge")).Text;
                            if (uniformcharge.Trim().Length == 0)
                            {
                                uniformcharge = "0";
                            }

                            string SITEALLOW = ((TextBox)gvSWDesignations.Rows[j].FindControl("TxtsiteAllowance")).Text;
                            if (SITEALLOW.Trim().Length == 0)
                            {
                                SITEALLOW = "0";
                            }

                            string ADDL4HR = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtaddlhrallw")).Text;
                            if (ADDL4HR.Trim().Length == 0)
                            {
                                ADDL4HR = "0";
                            }

                            string QTRALLOW = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtqtrallw")).Text;
                            if (QTRALLOW.Trim().Length == 0)
                            {
                                QTRALLOW = "0";
                            }

                            string RELALLOW = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtrelallw")).Text;
                            if (RELALLOW.Trim().Length == 0)
                            {
                                RELALLOW = "0";
                            }

                            string OTESICWAGES = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtotesiwages")).Text;
                            if (OTESICWAGES.Trim().Length == 0)
                            {
                                OTESICWAGES = "0";
                            }

                            string GunAllw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtGunallw")).Text;
                            if (GunAllw.Trim().Length == 0)
                            {
                                GunAllw = "0";
                            }

                            string FireAllw = ((TextBox)gvSWDesignations.Rows[j].FindControl("txtFireallw")).Text;
                            if (FireAllw.Trim().Length == 0)
                            {
                                FireAllw = "0";
                            }

                            string CSno = ((TextBox)gvdesignation.Rows[j].FindControl("lblCSlno")).Text;
                            if (CSno.Trim().Length == 0)
                            {
                                CSno = "0";
                            }

                            //string lblCSlno = ((Label)gvSWDesignations.Rows[j].FindControl("lblCSlno")).Text;


                            #region  STored Procedure Parameters , connection Strings
                            SPName = "Insertcontractspecialdetails";

                            #region Parameters 1 -8
                            HtContracts.Add("@Sno", CSno);
                            HtContracts.Add("@clientid", ClientId);
                            HtContracts.Add("@ContractId", ContractId);
                            HtContracts.Add("@Designations", Cddldesignationsw);
                            HtContracts.Add("@MinWagesCategory", Cddlcategoriessw);
                            HtContracts.Add("@Netpay", CNetPaysw);
                            HtContracts.Add("@Gross", Cgrosssw);
                            HtContracts.Add("@Basic", Cbasicsw);
                            HtContracts.Add("@DA", Cdasw);
                            HtContracts.Add("@HRA", Chrasw);
                            HtContracts.Add("@Conveyance", CConveyancesw);
                            HtContracts.Add("@WashAllownce", Cwashallowancesw);

                            #endregion

                            #region Parameters 8-16

                            HtContracts.Add("@OtherAllowance", Cawsw);
                            HtContracts.Add("@CCA", Ccasw);
                            HtContracts.Add("@LeaveAmount", LeaveAmountsw);
                            HtContracts.Add("@Bonus", Bonussw);
                            HtContracts.Add("@gratuity", Gratutysw);
                            #endregion

                            #region Parameters 16-22

                            HtContracts.Add("@NoOfDays", NoOfDaysForWages);
                            HtContracts.Add("@NFHs", Nfhssw);
                            HtContracts.Add("@testrecord", specialdesigncount);
                            HtContracts.Add("@Nots", NoOfOts);
                            HtContracts.Add("@NNhs", NoOfNhs);
                            HtContracts.Add("@NWos", NoOfWos);
                            HtContracts.Add("@RC", RC);
                            HtContracts.Add("@CS", CS);
                            HtContracts.Add("@OTRATE", OTRATE);
                            HtContracts.Add("@OTHrs", OThrs);
                            HtContracts.Add("@attbonus", AttBonussw);
                            HtContracts.Add("@servicechargeper", CSpersw);
                            HtContracts.Add("@CSWPLDays", CSWPLDays);
                            HtContracts.Add("@CSWPLAmount", CSWPLAmount);
                            HtContracts.Add("@CSWTLDays", CSWTLDays);
                            HtContracts.Add("@CSWTLAmount", CSWTLAmount);
                            HtContracts.Add("@NHSRate", CtxtTLnhsamt);
                            HtContracts.Add("@WORate", CtxtTLwoamt);
                            HtContracts.Add("@FoodAllowance", CtxtTLfoodamt);
                            HtContracts.Add("@MedicalAllowance", CtxtTLmedicalamt);
                            HtContracts.Add("@SplAllowance", splallw);
                            HtContracts.Add("@TravelAllw", travelallw);
                            HtContracts.Add("@PerformanceAllw", performanceallw);
                            HtContracts.Add("@MobileAllw", mobileallw);
                            HtContracts.Add("@AdvDed", CtxtTLadvded);
                            HtContracts.Add("@WCDed", CtxtTLwcded);
                            HtContracts.Add("@Uniformded", CtxtTLunidedsw);
                            HtContracts.Add("@PFRATE", PFRate);
                            HtContracts.Add("@ESIRATE", ESIRate);
                            HtContracts.Add("@PFDays", NoOfPFDays);
                            HtContracts.Add("@ESIDays", NoOfESIDays);
                            HtContracts.Add("@NoofSplDays", NoOfSplDays);
                            HtContracts.Add("@LAType", LAType);
                            HtContracts.Add("@BonusType", BonusType);
                            HtContracts.Add("@GratuityType", GratuityType);
                            HtContracts.Add("@Serviceweightage", servicewightage);
                            HtContracts.Add("@HardshipAllw", HardshipAllw);
                            HtContracts.Add("@RankAllw", RankAllw);
                            HtContracts.Add("@uniformcharge", uniformcharge);
                            HtContracts.Add("@AttBonusType", AttBonusType);
                            HtContracts.Add("@SITEALLOW", SITEALLOW);
                            HtContracts.Add("@ADDL4HR", ADDL4HR);
                            HtContracts.Add("@QTRALLOW", QTRALLOW);
                            HtContracts.Add("@RELALLOW", RELALLOW);
                            HtContracts.Add("@OTESICWAGES", OTESICWAGES);
                            HtContracts.Add("@GunAllw", GunAllw);
                            HtContracts.Add("@FireAllw", FireAllw);
                            HtContracts.Add("@OTESIWagesDays", OTESIWagesDays);

                            #endregion

                            IRecordStatus = config.ExecuteNonQueryParamsAsync(SPName, HtContracts).Result;
                            if (IRecordStatus != 0)
                            {
                                specialdesigncount++;
                            }
                            #endregion

                        }
                    }

                    #endregion
                }
                if (IRecordStatus != 0)
                {
                    contractidautogenrate();
                    clearcontractdetails();
                    lblSuc.Text = "Contract Added Successfully";
                    Session["ContractsAIndex"] = 0;
                    Session["ContractsAIndexsw"] = 0;
                    ClearDataFromThePage();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }

            Session["ContractsAIndex"] = 0;
            Session["ContractsAIndexsw"] = 0;

            FillddlTakedata();

        }

        protected void RdbIGST_CheckedChanged(object sender, EventArgs e)
        {

            if (RdbIGST.Checked)
            {
                chkCGST.Checked = false;
                chkCGST.Enabled = false;
            }
            else
            {
                chkCGST.Checked = true;
                chkCGST.Enabled = true;
            }
        }

        protected void RdbSGST_CheckedChanged(object sender, EventArgs e)
        {

            if (RdbIGST.Checked)
            {
                chkCGST.Checked = false;
                chkCGST.Enabled = false;
            }
            else
            {
                chkCGST.Checked = true;
                chkCGST.Enabled = true;
            }
        }

        protected void chkcdSGST_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkcdIGST_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkcdIGST = sender as CheckBox;
            GridViewRow row = null;
            if (chkcdIGST == null)
                return;

            row = (GridViewRow)chkcdIGST.NamingContainer;
            if (row == null)
                return;


            CheckBox chkcdCGST = row.FindControl("chkcdCGST") as CheckBox;
            CheckBox chkcdSGST = row.FindControl("chkcdSGST") as CheckBox;


            if (chkcdIGST.Checked)
            {
                chkcdCGST.Checked = false;
                chkcdCGST.Enabled = false;

                chkcdSGST.Checked = false;
                chkcdSGST.Enabled = false;

            }
            else
            {
                chkcdCGST.Checked = true;
                chkcdCGST.Enabled = true;

                chkcdSGST.Checked = true;
                chkcdSGST.Enabled = true;

            }
        }

        protected void gvdesignation_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        public void calculatevalues()
        {
            //new variables for PFon

            var pfonBasic = 0; var pfonDA = 0; var pfonHRA = 0; var pfonConv = 0; var pfonMedicalAllw = 0; var pfonFoodAllw = 0; var pfonSiteAllw = 0; var pfonSpecialAllw = 0; var pfonOA = 0; var pfonWA = 0; var pfonGunAllw = 0; var pfonFireAllw = 0; var pfonBonus = 0; var pfonGratuity = 0; var pfonLA = 0; var pfonOTRate = 0; var pfonESIPayrate = 0; var pfonPFPayrate = 0; var pfonCCA = 0; var pfonNFHS = 0; var pfonRC = 0; var pfonPLAmount = 0; var pfonTLAmount = 0; var pfonServicecharge = 0; var pfonNHSRate = 0; var pfonWoRate = 0; var pfonTravelAllw = 0; var pfonOTHrs = 0; var pfonPerformanceAllw = 0; var pfonMobileAllw = 0; var pfonUniformCharges = 0; var pfonServiceWeightage = 0;

            //end 

            //new variables for ESIon

            var ESIonBasic = 0; var ESIonDA = 0; var ESIonHRA = 0; var ESIonConv = 0; var ESIonMedicalAllw = 0; var ESIonFoodAllw = 0; var ESIonSiteAllw = 0; var ESIonSpecialAllw = 0; var ESIonOA = 0; var ESIonWA = 0; var ESIonGunAllw = 0; var ESIonFireAllw = 0; var ESIonBonus = 0; var ESIonGratuity = 0; var ESIonLA = 0; var ESIonOTRate = 0; var ESIonESIPayrate = 0; var ESIonPFPayrate = 0; var ESIonCCA = 0; var ESIonNFHS = 0; var ESIonRC = 0; var ESIonPLAmount = 0; var ESIonTLAmount = 0; var ESIonServicecharge = 0; var ESIonNHSRate = 0; var ESIonWoRate = 0; var ESIonTravelAllw = 0; var ESIonOTHrs = 0; var ESIonPerformanceAllw = 0; var ESIonMobileAllw = 0; var ESIonUniformCharges = 0; var ESIonServiceWeightage = 0;

            //end 
            var servicecharge = txtservicecharge.Text;
            if (txtservicecharge.Text.Trim() == "")
            {
                servicecharge = "0";
            }


            if (ChkpfonBasic.Checked)
                pfonBasic = 1;

            if (ChkpfonDA.Checked)
                pfonDA = 1;

            if (ChkpfonHRA.Checked)
                pfonHRA = 1;

            if (ChkpfonConv.Checked)
                pfonConv = 1;

            if (ChkpfonMedicalAllw.Checked)
                pfonMedicalAllw = 1;



            if (ChkpfonFoodAllw.Checked)
                pfonFoodAllw = 1;

            if (ChkpfonSiteAllw.Checked)
                pfonSiteAllw = 1;

            if (ChkpfonSpecialAllw.Checked)
                pfonSpecialAllw = 1;

            if (ChkpfonOA.Checked)
                pfonOA = 1;

            if (ChkpfonWA.Checked)
                pfonWA = 1;

            if (ChkpfonGunAllw.Checked)
                pfonGunAllw = 1;

            if (ChkpfonFireAllw.Checked)
                pfonFireAllw = 1;

            if (ChkpfonBonus.Checked)
                pfonBonus = 1;

            if (ChkpfonGratuity.Checked)
                pfonGratuity = 1;


            if (ChkpfonLA.Checked)
                pfonLA = 1;

            if (ChkpfonOTRate.Checked)
                pfonOTRate = 1;

            if (ChkpfonESIPayrate.Checked)
                pfonESIPayrate = 1;

            if (ChkpfonPFPayrate.Checked)
                pfonPFPayrate = 1;

            if (ChkpfonCCA.Checked)
                pfonCCA = 1;

            //if (ChkpfonNFHS.Checked)
            //    pfonNFHS = 1;

            if (ChkpfonRC.Checked)
                pfonRC = 1;

            if (ChkpfonPLAmount.Checked)
                pfonPLAmount = 1;

            if (ChkpfonTLAmount.Checked)
                pfonTLAmount = 1;

            if (ChkpfonServicecharge.Checked)
                pfonServicecharge = 1;

            if (ChkpfonNHSRate.Checked)
                pfonNHSRate = 1;

            if (ChkpfonWoRate.Checked)
                pfonWoRate = 1;


            if (ChkpfonTravelAllw.Checked)
                pfonTravelAllw = 1;

            if (ChkpfonOTHrs.Checked)
                pfonOTHrs = 1;

            if (ChkpfonPerformanceAllw.Checked)
                pfonPerformanceAllw = 1;

            if (ChkpfonMobileAllw.Checked)
                pfonMobileAllw = 1;

            if (ChkpfonUniformCharges.Checked)
                pfonUniformCharges = 1;

            if (ChkpfonServiceWeightage.Checked)
                pfonServiceWeightage = 1;


            //new code End  for pt on check boxes 


            //new code for pt on check boxes 

            if (ChkESIonBasic.Checked)
                ESIonBasic = 1;

            if (ChkESIonDA.Checked)
                ESIonDA = 1;

            if (ChkESIonHRA.Checked)
                ESIonHRA = 1;

            if (ChkESIonConv.Checked)
                ESIonConv = 1;

            if (ChkESIonMedicalAllw.Checked)
                ESIonMedicalAllw = 1;



            if (ChkESIonFoodAllw.Checked)
                ESIonFoodAllw = 1;

            if (ChkESIonSiteAllw.Checked)
                ESIonSiteAllw = 1;

            if (ChkESIonSpecialAllw.Checked)
                ESIonSpecialAllw = 1;

            if (ChkESIonOA.Checked)
                ESIonOA = 1;

            if (ChkESIonWA.Checked)
                ESIonWA = 1;

            if (ChkESIonGunAllw.Checked)
                ESIonGunAllw = 1;

            if (ChkESIonFireAllw.Checked)
                ESIonFireAllw = 1;

            if (ChkESIonBonus.Checked)
                ESIonBonus = 1;

            if (ChkESIonGratuity.Checked)
                ESIonGratuity = 1;


            if (ChkESIonLA.Checked)
                ESIonLA = 1;

            if (ChkESIonOTRate.Checked)
                ESIonOTRate = 1;

            if (ChkESIonESIPayrate.Checked)
                ESIonESIPayrate = 1;

            if (ChkESIonPFPayrate.Checked)
                ESIonPFPayrate = 1;

            if (ChkESIonCCA.Checked)
                ESIonCCA = 1;

            //if (ChkESIonNFHS.Checked)
            //    ESIonNFHS = 1;

            if (ChkESIonRC.Checked)
                ESIonRC = 1;

            if (ChkESIonPLAmount.Checked)
                ESIonPLAmount = 1;

            if (ChkESIonTLAmount.Checked)
                ESIonTLAmount = 1;

            if (ChkESIonServicecharge.Checked)
                ESIonServicecharge = 1;

            if (ChkESIonNHSRate.Checked)
                ESIonNHSRate = 1;

            if (ChkpfonWoRate.Checked)
                ESIonWoRate = 1;


            if (ChkESIonTravelAllw.Checked)
                ESIonTravelAllw = 1;

            if (ChkESIonOTHrs.Checked)
                ESIonOTHrs = 1;

            if (ChkESIonPerformanceAllw.Checked)
                ESIonPerformanceAllw = 1;

            if (ChkESIonMobileAllw.Checked)
                ESIonMobileAllw = 1;

            if (ChkESIonUniformCharges.Checked)
                ESIonUniformCharges = 1;

            if (ChkESIonServiceWeightage.Checked)
                ESIonServiceWeightage = 1;

            for (int i = 0; i < gvSWDesignations.Rows.Count; i++)
            {
                DropDownList DdlDesign = gvSWDesignations.Rows[i].FindControl("DdlDesign") as DropDownList;
                if (DdlDesign.SelectedValue != "--Select--")
                {
                    TextBox TxtBasic = gvSWDesignations.Rows[i].FindControl("TxtBasic") as TextBox;
                    TextBox txtda = gvSWDesignations.Rows[i].FindControl("txtda") as TextBox;
                    TextBox txthra = gvSWDesignations.Rows[i].FindControl("txthra") as TextBox;
                    TextBox txtConveyance = gvSWDesignations.Rows[i].FindControl("txtConveyance") as TextBox;
                    TextBox txtcca = gvSWDesignations.Rows[i].FindControl("txtcca") as TextBox;
                    TextBox txtleaveamount = gvSWDesignations.Rows[i].FindControl("txtleaveamount") as TextBox;
                    DropDownList ddllatype = gvSWDesignations.Rows[i].FindControl("ddllatype") as DropDownList;
                    TextBox txtgratuty = gvSWDesignations.Rows[i].FindControl("txtgratuty") as TextBox;
                    DropDownList ddlgratuitytype = gvSWDesignations.Rows[i].FindControl("ddlgratuitytype") as DropDownList;
                    TextBox txtbonus = gvSWDesignations.Rows[i].FindControl("txtbonus") as TextBox;
                    DropDownList ddlbonustype = gvSWDesignations.Rows[i].FindControl("ddlbonustype") as DropDownList;
                    TextBox txtattbonus = gvSWDesignations.Rows[i].FindControl("txtattbonus") as TextBox;
                    TextBox txtwa = gvSWDesignations.Rows[i].FindControl("txtwa") as TextBox;
                    TextBox txtoa = gvSWDesignations.Rows[i].FindControl("txtoa") as TextBox;
                    TextBox txtNfhs1 = gvSWDesignations.Rows[i].FindControl("txtNfhs1") as TextBox;
                    TextBox Txtrc = gvSWDesignations.Rows[i].FindControl("Txtrc") as TextBox;
                    TextBox TxtFoodAllowance = gvSWDesignations.Rows[i].FindControl("TxtFoodAllowance") as TextBox;
                    TextBox TxtMedicalAllowance = gvSWDesignations.Rows[i].FindControl("TxtMedicalAllowance") as TextBox;
                    TextBox TxtSpecialAllowance = gvSWDesignations.Rows[i].FindControl("TxtSplAllowance") as TextBox;
                    TextBox txtplamount = gvSWDesignations.Rows[i].FindControl("TxtCSWPLAmount") as TextBox;
                    TextBox txttlamount = gvSWDesignations.Rows[i].FindControl("TxtCSWTLDays") as TextBox;
                    TextBox txttravelallw = gvSWDesignations.Rows[i].FindControl("TxtTravelAllowance") as TextBox;
                    TextBox txtperformanceallw = gvSWDesignations.Rows[i].FindControl("TxtPerformanceAllowance") as TextBox;
                    TextBox txtmobileallw = gvSWDesignations.Rows[i].FindControl("TxtMobileAllowance") as TextBox;
                    TextBox txtserviceweightage = gvSWDesignations.Rows[i].FindControl("txtservicewt") as TextBox;
                    TextBox txtHardshipAllw = gvSWDesignations.Rows[i].FindControl("txtHardshipAllw") as TextBox;
                    TextBox txtRankAllw = gvSWDesignations.Rows[i].FindControl("txtRankAllw") as TextBox;
                    TextBox TxtOTRate = gvSWDesignations.Rows[i].FindControl("TxtOTRate") as TextBox;
                    TextBox TxtNHSRate = gvSWDesignations.Rows[i].FindControl("TxtNHSRate") as TextBox;
                    TextBox TxtWORate = gvSWDesignations.Rows[i].FindControl("TxtWORate") as TextBox;



                    // TextBox txtuniformcharge = gvSWDesignations.Rows[i].FindControl("txtuniformcharge") as TextBox;


                    TextBox TxtsiteAllowance = gvSWDesignations.Rows[i].FindControl("TxtsiteAllowance") as TextBox;
                    TextBox txtaddlhrallw = gvSWDesignations.Rows[i].FindControl("txtaddlhrallw") as TextBox;
                    TextBox txtqtrallw = gvSWDesignations.Rows[i].FindControl("txtqtrallw") as TextBox;
                    TextBox txtrelallw = gvSWDesignations.Rows[i].FindControl("txtrelallw") as TextBox;

                    TextBox txtGunallw = gvSWDesignations.Rows[i].FindControl("txtGunallw") as TextBox;
                    TextBox txtFireallw = gvSWDesignations.Rows[i].FindControl("txtFireallw") as TextBox;

                    TextBox TxtPFRate = gvSWDesignations.Rows[i].FindControl("TxtPFRate") as TextBox;
                    TextBox TxtESIRate = gvSWDesignations.Rows[i].FindControl("TxtESIRate") as TextBox;

                    TextBox Txtgross = gvSWDesignations.Rows[i].FindControl("Txtgross") as TextBox;
                    TextBox TxtNetPay = gvSWDesignations.Rows[i].FindControl("TxtNetPay") as TextBox;

                    TextBox txtPFEmp = gvSWDesignations.Rows[i].FindControl("txtPFEmp") as TextBox;
                    TextBox txtESIEmp = gvSWDesignations.Rows[i].FindControl("txtESIEmp") as TextBox;

                    TextBox txtPFEmpr = gvSWDesignations.Rows[i].FindControl("txtPFEmpr") as TextBox;
                    TextBox txtESIEmpr = gvSWDesignations.Rows[i].FindControl("txtESIEmpr") as TextBox;

                    TextBox txtSCEmp = gvSWDesignations.Rows[i].FindControl("txtSCEmp") as TextBox;
                    TextBox txtCTC = gvSWDesignations.Rows[i].FindControl("txtCTC") as TextBox;

                    TextBox TxtIncentiveRate = gvSWDesignations.Rows[i].FindControl("TxtIncentiveRate") as TextBox;
                    TextBox TxtLCRate = gvSWDesignations.Rows[i].FindControl("TxtLCRate") as TextBox;



                    #region start values
                    if (TxtBasic.Text == "")
                    {
                        TxtBasic.Text = "0";
                    }
                    if (txtda.Text == "")
                    {
                        txtda.Text = "0";
                    }
                    if (txthra.Text == "")
                    {
                        txthra.Text = "0";
                    }
                    if (txtConveyance.Text == "")
                    {
                        txtConveyance.Text = "0";
                    }
                    if (txtcca.Text == "")
                    {
                        txtcca.Text = "0";
                    }
                    if (txtleaveamount.Text == "")
                    {
                        txtleaveamount.Text = "0";
                    }
                    if (txtgratuty.Text == "")
                    {
                        txtgratuty.Text = "0";
                    }
                    if (txtbonus.Text == "")
                    {
                        txtbonus.Text = "0";
                    }
                    if (txtattbonus.Text == "")
                    {
                        txtattbonus.Text = "0";
                    }
                    if (txtwa.Text == "")
                    {
                        txtwa.Text = "0";
                    }
                    if (txtoa.Text == "")
                    {
                        txtoa.Text = "0";
                    }
                    if (txtNfhs1.Text == "")
                    {
                        txtNfhs1.Text = "0";
                    }
                    if (Txtrc.Text == "")
                    {
                        Txtrc.Text = "0";
                    }
                    if (TxtFoodAllowance.Text == "")
                    {
                        TxtFoodAllowance.Text = "0";
                    }
                    if (TxtMedicalAllowance.Text == "")
                    {
                        TxtMedicalAllowance.Text = "0";
                    }
                    if (TxtSpecialAllowance.Text == "")
                    {
                        TxtSpecialAllowance.Text = "0";
                    }

                    if (txtplamount.Text == "")
                    {
                        txtplamount.Text = "0";
                    }
                    if (txttlamount.Text == "")
                    {
                        txttlamount.Text = "0";
                    }

                    if (txttravelallw.Text == "")
                    {
                        txttravelallw.Text = "0";
                    }

                    if (txtperformanceallw.Text == "")
                    {
                        txtperformanceallw.Text = "0";
                    }

                    if (txtmobileallw.Text == "")
                    {
                        txtmobileallw.Text = "0";
                    }

                    if (txtserviceweightage.Text == "")
                    {
                        txtserviceweightage.Text = "0";
                    }

                    if (txtHardshipAllw.Text == "")
                    {
                        txtHardshipAllw.Text = "0";
                    }

                    if (txtRankAllw.Text == "")
                    {
                        txtRankAllw.Text = "0";
                    }

                    //if (txtuniformcharge.Text == "")
                    //{
                    //    txtuniformcharge.Text = "0";
                    //}

                    if (TxtsiteAllowance.Text == "")
                    {
                        TxtsiteAllowance.Text = "0";
                    }
                    if (txtaddlhrallw.Text == "")
                    {
                        txtaddlhrallw.Text = "0";
                    }
                    if (txtqtrallw.Text == "")
                    {
                        txtqtrallw.Text = "0";
                    }
                    if (txtrelallw.Text == "")
                    {
                        txtrelallw.Text = "0";
                    }

                    #endregion

                    if (txtGunallw.Text == "")
                    {
                        txtGunallw.Text = "0";
                    }

                    if (txtFireallw.Text == "")
                    {
                        txtFireallw.Text = "0";
                    }



                    if (TxtPf.Text == "")
                    {
                        TxtPf.Text = "0";
                    }

                    if (TxtEsi.Text == "")
                    {
                        TxtEsi.Text = "0";
                    }

                    if (TxtPFRate.Text == "")
                    {
                        TxtPFRate.Text = "0";
                    }

                    if (TxtESIRate.Text == "")
                    {
                        TxtESIRate.Text = "0";
                    }

                    if (TxtOTRate.Text == "")
                    {
                        TxtOTRate.Text = "0";
                    }

                    if (TxtNHSRate.Text == "")
                    {
                        TxtNHSRate.Text = "0";
                    }

                    if (TxtWORate.Text == "")
                    {
                        TxtWORate.Text = "0";
                    }

                    string SPName = "CalculateValues";
                    Hashtable ht = new Hashtable();
                    ht.Add("@Clientid", ddlclientid.SelectedValue);
                    ht.Add("@Contractid", ddlContractids.SelectedValue);
                    ht.Add("@Basic", TxtBasic.Text.Trim());
                    ht.Add("@DA", txtda.Text.Trim());
                    ht.Add("@HRA", txthra.Text.Trim());
                    ht.Add("@CCA", txtcca.Text.Trim());
                    ht.Add("@Conveyance", txtConveyance.Text.Trim());
                    ht.Add("@LeaveAmount", txtleaveamount.Text.Trim());
                    ht.Add("@Gratuity", txtgratuty.Text.Trim());
                    ht.Add("@Bonus", txtbonus.Text.Trim());
                    ht.Add("@AttBonus", txtattbonus.Text.Trim());
                    ht.Add("@WA", txtwa.Text.Trim());
                    ht.Add("@OA", txtoa.Text.Trim());
                    ht.Add("@NFHs", txtNfhs1.Text.Trim());
                    ht.Add("@RC", Txtrc.Text.Trim());
                    ht.Add("@FoodAllowance", TxtFoodAllowance.Text.Trim());
                    ht.Add("@MedicalAllowance", TxtMedicalAllowance.Text.Trim());
                    ht.Add("@SpecialAllowance", TxtSpecialAllowance.Text.Trim());
                    ht.Add("@TravelAllowance", txttravelallw.Text.Trim());
                    ht.Add("@PerformanceAllowance", txtperformanceallw.Text.Trim());
                    ht.Add("@MobileAllowance", txtmobileallw.Text.Trim());
                    ht.Add("@HardshipAllw", txtHardshipAllw.Text.Trim());
                    ht.Add("@RankAllw", txtRankAllw.Text.Trim());
                    //ht.Add("@uniformcharge", txtuniformcharge.Text.Trim());
                    ht.Add("@PFFrom", DdlPf.SelectedIndex);
                    ht.Add("@ESIFrom", DdlEsi.SelectedIndex);
                    ht.Add("@Contractstartdate", Timings.Instance.CheckDateFormat(txtStartingDate.Text));
                    ht.Add("@Contractenddate", Timings.Instance.CheckDateFormat(txtEndingDate.Text));
                    ht.Add("@DdlDesign", DdlDesign.SelectedValue);
                    ht.Add("@CPFPercentage", TxtPf.Text.Trim());
                    ht.Add("@CESIPercentage", TxtEsi.Text.Trim());
                    ht.Add("@BonusType", ddlbonustype.SelectedIndex);
                    ht.Add("@LAType", ddllatype.SelectedIndex);
                    ht.Add("@GratuityType", ddlgratuitytype.SelectedIndex);

                    ht.Add("@SITEALLOW", TxtsiteAllowance.Text.Trim());
                    ht.Add("@ADDL4HR", txtaddlhrallw.Text.Trim());
                    ht.Add("@QTRALLOW", txtqtrallw.Text.Trim());
                    ht.Add("@RELALLOW", txtrelallw.Text.Trim());
                    ht.Add("@GunAllw", txtGunallw.Text.Trim());
                    ht.Add("@FireAllw", txtFireallw.Text.Trim());

                    ht.Add("@PFRate", TxtPFRate.Text.Trim());
                    ht.Add("@ESIRate", TxtESIRate.Text.Trim());
                    ht.Add("@OTAmtC", TxtOTRate.Text.Trim());
                    ht.Add("@NHSAmtC", TxtNHSRate.Text.Trim());
                    ht.Add("@WoAmtC", TxtWORate.Text.Trim());
                    //ht.Add("@IncentiveRate", TxtIncentiveRate.Text.Trim());
                    //ht.Add("@LCRate", TxtLCRate.Text.Trim());
                    ht.Add("@ServChrge", servicecharge);
                    



                    DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

                    if (dt.Rows.Count > 0)
                    {
                        if (DdlDesign.SelectedValue == dt.Rows[0]["design"].ToString())
                        {
                            Txtgross.Text = dt.Rows[0]["gross"].ToString();
                            TxtNetPay.Text = dt.Rows[0]["Netpay"].ToString();

                            txtPFEmp.Text = dt.Rows[0]["PFEmp"].ToString();
                            txtESIEmp.Text = dt.Rows[0]["ESIEmp"].ToString();

                            txtPFEmpr.Text = dt.Rows[0]["PFEmpr"].ToString();
                            txtESIEmpr.Text = dt.Rows[0]["ESIEmpr"].ToString();

                            txtSCEmp.Text = dt.Rows[0]["SCharge"].ToString();
                            txtCTC.Text = dt.Rows[0]["CTC"].ToString();

                        }
                    }
                }
            }

            ContractSpecialWages.Update();

        }

        protected void btncalculate_Click(object sender, EventArgs e)
        {

            calculatevalues();


        }

        protected void chkrcm_CheckedChanged(object sender, EventArgs e)
        {
            if (chkrcm.Checked)
            {
                txtrcmtext.Visible = true;
            }
            else
            {
                txtrcmtext.Visible = false;
            }
        }
    }
}