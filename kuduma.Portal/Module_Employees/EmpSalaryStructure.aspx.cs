using System;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.Globalization;
using System.Collections;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class EmpSalaryStructure : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";

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

                }
            }
            catch (Exception ex)
            {
                GoToLoginPage();
            }


        }

        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            GetEmpName();
            LoadIDList();

            // Getdata();

        }

        protected void ClearDataFromPersonalInfoTabFields()
        {
            txtid.Text = txtStartingDate.Text = txtEndingDate.Text = txtEmpid.Text = txtName.Text = string.Empty;
            txtgratuty.Text = txtnightshiftRate.Text = TxtBasic.Text = txthra.Text = txtcca.Text = txtConveyance.Text = txtMobileAllowance.Text = txtMedicalAllw.Text = txtSplAllw.Text = txtbonus.Text = txtleaveamount.Text = txtoa.Text = txtunicharges.Text = txtwmc.Text = txtoutstationCharges.Text = txtmedicalpolicy.Text = txtlwf.Text =
           TxtPFVoluntary.Text = TxtCs.Text = txtSplAllw.Text = txtda.Text = txtEducationAllowance.Text = txtwa.Text = txtNfhs1.Text = Txtrc.Text = TxtPFPayRate.Text = TxtESIPayRate.Text = TxtOTRate.Text = txtPerformanceAllowance.Text = txtTravellingAllowance.Text = txtpvc.Text = txtbgv.Text =
           txtshift1Rate.Text = txtshift2Rate.Text = txtlateloginRate.Text = txtExtraOne.Text = txtServiceWeightage.Text = txtRankAllowance.Text = txtInsurance.Text = txtMobileRecharge.Text = txtExta1.Text = txtExtra2.Text = txtExra3.Text = TxtNHSRate.Text = txtADDL4HR.Text = txtQTRALLOW.Text = txtRELALLOW.Text = txtOTESICWAGES.Text = txtSITEALLOW.Text = txtGunAllw.Text = txtFireAllw.Text = txtTelephoneAllw.Text =
     txtFoodAllw.Text = txtReimbursement.Text = txtHardshipAllw.Text = txtPaidHolidayAllw.Text = txtServiceCharge.Text = "0";
            ddlID.SelectedIndex=ddlGunAllwType.SelectedIndex = ddlNoOfDaysWages.SelectedIndex = ddl1shiftdays.SelectedIndex = ddlPFNoOfDaysForWages.SelectedIndex = ddlNoOfOtsPaysheet.SelectedIndex = ddlID.SelectedIndex = ddlnbonus.SelectedIndex = ddlNoOfDaysWages.SelectedIndex = ddlNightshiftdays.SelectedIndex = ddl2shiftdays.SelectedIndex = ddlnotsspl.SelectedIndex = ddlLateLogindays.SelectedIndex = ddlNunichrges.SelectedIndex = ddlNoOfNhsPaysheet.SelectedIndex=ddlShift.SelectedIndex = 0;

        }

        protected void GetEmpName()
        {
            if (txtEmpid.Text.Length > 0)
            {
                // and empid like '%" + EmpIDPrefix + "'
                string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where Branch in (" + BranchID + ") and empid='" + txtEmpid.Text + "'  ";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        txtName.Text = dt.Rows[0]["empname"].ToString();
                        txtdesignation.Text = dt.Rows[0]["EmpDesgn"].ToString();
                    }
                    catch (Exception ex)
                    {
                        // MessageLabel.Text = ex.Message;
                    }
                }
                else
                {
                    txtEmpid.Text = "";
                    txtName.Text = "";
                }
            }

        }

        protected void GetEmpid()
        {

            #region  Old Code
            string Sqlqry = "select  empid+' - '+oldempid Empid,EmpDesgn from empdetails where Branch in (" + BranchID + ") and (empfname+' '+empmname+' '+emplname)  like '" + txtName.Text + "%' and  empid like '" + EmpIDPrefix + "%' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["Empid"].ToString();
                    txtdesignation.Text = dt.Rows[0]["EmpDesgn"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                txtEmpid.Text = "";
                txtName.Text = "";
            }
            #endregion // End Old Code


        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            GetEmpid();
            LoadIDList();
            // Getdata();
        }

        public void Getdata()
        {
            #region for Salary Structure

            try
            {
                var EmpID = txtEmpid.Text;
                var ID = ddlID.SelectedValue;
                string Spname = "GetEmpSalaryStructure";
                Hashtable ht = new Hashtable();
                ht.Add("@ID", ID);
                ht.Add("@EmpID", EmpID);
                DataTable dtSalStructure = config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;
                if (dtSalStructure.Rows.Count > 0)
                {
                    ddlempsalstatus.SelectedValue = dtSalStructure.Rows[0]["Status"].ToString();
                    txtStartingDate.Text = DateTime.Parse(dtSalStructure.Rows[0]["FromDate"].ToString()).ToString("dd/MM/yyyy");
                    txtEndingDate.Text = DateTime.Parse(dtSalStructure.Rows[0]["ToDate"].ToString()).ToString("dd/MM/yyyy");
                    TxtBasic.Text = dtSalStructure.Rows[0]["Basic"].ToString();
                    txtda.Text = dtSalStructure.Rows[0]["DA"].ToString();
                    txthra.Text = dtSalStructure.Rows[0]["HRA"].ToString();
                    txtConveyance.Text = dtSalStructure.Rows[0]["Conveyance"].ToString();
                    txtcca.Text = dtSalStructure.Rows[0]["CCA"].ToString();
                    txtbonus.Text = dtSalStructure.Rows[0]["Bonus"].ToString();
                    txtgratuty.Text = dtSalStructure.Rows[0]["Gratuity"].ToString();
                    txtwa.Text = dtSalStructure.Rows[0]["WashAllowance"].ToString();
                    txtNfhs1.Text = dtSalStructure.Rows[0]["NFHS"].ToString();
                    txtMedicalAllw.Text = dtSalStructure.Rows[0]["MedicalAllowance"].ToString();
                    txtMobileAllowance.Text = dtSalStructure.Rows[0]["MobileAllowance"].ToString();
                    txtSplAllw.Text = dtSalStructure.Rows[0]["SplAllowance"].ToString();
                    txtleaveamount.Text = dtSalStructure.Rows[0]["LeaveAmount"].ToString();
                    txtoa.Text = dtSalStructure.Rows[0]["OtherAllowance"].ToString();
                    Txtrc.Text = dtSalStructure.Rows[0]["RC"].ToString();
                    TxtCs.Text = dtSalStructure.Rows[0]["CS"].ToString();
                    TxtOTRate.Text = dtSalStructure.Rows[0]["OTRate"].ToString();
                    txtEducationAllowance.Text = dtSalStructure.Rows[0]["EducationAllowance"].ToString();
                    txtExta1.Text = dtSalStructure.Rows[0]["Extra1"].ToString();
                    txtExtra2.Text = dtSalStructure.Rows[0]["Extra2"].ToString();
                    txtExra3.Text = dtSalStructure.Rows[0]["Extra3"].ToString();
                    txtlwf.Text = dtSalStructure.Rows[0]["LWF"].ToString();
                    TxtNHSRate.Text = dtSalStructure.Rows[0]["NHSrate"].ToString();
                    ddlnbonus.SelectedIndex = int.Parse(dtSalStructure.Rows[0]["BonusType"].ToString());
                    txtServiceWeightage.Text = dtSalStructure.Rows[0]["ServiceWeightage"].ToString();
                    txtpvc.Text = dtSalStructure.Rows[0]["PVCAmt"].ToString();
                    txtbgv.Text = dtSalStructure.Rows[0]["BGVAmt"].ToString();
                    txtTravellingAllowance.Text = dtSalStructure.Rows[0]["TravellingAllowance"].ToString();
                    txtPerformanceAllowance.Text = dtSalStructure.Rows[0]["PerformanceAllowance"].ToString();

                    txtADDL4HR.Text = dtSalStructure.Rows[0]["ADDL4HR"].ToString();
                    txtQTRALLOW.Text = dtSalStructure.Rows[0]["QTRALLOW"].ToString();
                    txtRELALLOW.Text = dtSalStructure.Rows[0]["RELALLOW"].ToString();
                    txtOTESICWAGES.Text = dtSalStructure.Rows[0]["OTESICWAGES"].ToString();
                    txtSITEALLOW.Text = dtSalStructure.Rows[0]["SITEALLOW"].ToString();
                    txtGunAllw.Text = dtSalStructure.Rows[0]["GunAllw"].ToString();
                    txtFireAllw.Text = dtSalStructure.Rows[0]["FireAllw"].ToString();
                    txtTelephoneAllw.Text = dtSalStructure.Rows[0]["TelephoneAllw"].ToString();
                    txtFoodAllw.Text = dtSalStructure.Rows[0]["FoodAllw"].ToString();
                    txtReimbursement.Text = dtSalStructure.Rows[0]["Reimbursement"].ToString();
                    txtHardshipAllw.Text = dtSalStructure.Rows[0]["HardshipAllw"].ToString();
                    txtPaidHolidayAllw.Text = dtSalStructure.Rows[0]["PaidHolidayAllw"].ToString();
                    txtServiceCharge.Text = dtSalStructure.Rows[0]["ServiceCharge"].ToString();
                    txtRankAllowance.Text = dtSalStructure.Rows[0]["RankAllw"].ToString();

                    TxtPFPayRate.Text = dtSalStructure.Rows[0]["Pfpayrate"].ToString();
                    TxtESIPayRate.Text = dtSalStructure.Rows[0]["ESIpayrate"].ToString();


                    if (String.IsNullOrEmpty(dtSalStructure.Rows[0]["PFNoOfDays"].ToString()) != false)
                    {
                        ddlPFNoOfDaysForWages.SelectedIndex = 0;
                    }
                    else
                    {
                        if (dtSalStructure.Rows[0]["PFNoOfDays"].ToString().Trim().Length > 0)
                        {
                            float noofdays = float.Parse(dtSalStructure.Rows[0]["PFNoOfDays"].ToString());
                            if (noofdays == 0)
                            {
                                ddlPFNoOfDaysForWages.SelectedIndex = 0;
                            }

                            else if (noofdays == 1)
                            {
                                ddlPFNoOfDaysForWages.SelectedIndex = 1;
                            }
                            else if (noofdays == 2)
                            {
                                ddlPFNoOfDaysForWages.SelectedIndex = 2;
                            }
                            else if (noofdays == 3)
                            {
                                ddlPFNoOfDaysForWages.SelectedIndex = 3;
                            }
                            else if (noofdays == 4)
                            {
                                ddlPFNoOfDaysForWages.SelectedIndex = 4;
                            }
                            else if (noofdays == 5)
                            {
                                ddlPFNoOfDaysForWages.SelectedIndex = 5;
                            }
                            else
                            {
                                ddlPFNoOfDaysForWages.SelectedValue = dtSalStructure.Rows[0]["PFNoOfDays"].ToString();
                            }
                        }

                    }




                    if (String.IsNullOrEmpty(dtSalStructure.Rows[0]["Nots"].ToString()) != false)
                    {
                        ddlNoOfOtsPaysheet.SelectedIndex = 0;
                    }
                    else
                    {
                        if (dtSalStructure.Rows[0]["Nots"].ToString().Trim().Length > 0)
                        {
                            int noofdays = int.Parse(dtSalStructure.Rows[0]["Nots"].ToString());
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
                            else if (noofdays == 5)
                            {
                                ddlNoOfOtsPaysheet.SelectedIndex = 5;
                            }
                            else if (noofdays == 6)
                            {
                                ddlNoOfOtsPaysheet.SelectedIndex = 6;
                            }
                            else if (noofdays == 7)
                            {
                                ddlNoOfOtsPaysheet.SelectedIndex = 7;
                            }
                            else if (noofdays == 8)
                            {
                                ddlNoOfOtsPaysheet.SelectedIndex = 8;
                            }

                            else
                            {
                                ddlNoOfOtsPaysheet.SelectedValue = dtSalStructure.Rows[0]["Nots"].ToString();
                            }
                        }

                    }

                    if (String.IsNullOrEmpty(dtSalStructure.Rows[0]["otesiwagesdays"].ToString()) != false)
                    {
                        ddlotesidays.SelectedIndex = 0;
                    }
                    else
                    {
                        if (dtSalStructure.Rows[0]["otesiwagesdays"].ToString().Trim().Length > 0)
                        {
                            int noofdays = int.Parse(dtSalStructure.Rows[0]["otesiwagesdays"].ToString());
                            if (noofdays == 0)
                            {
                                ddlotesidays.SelectedIndex = 0;
                            }

                            else if (noofdays == 1)
                            {
                                ddlotesidays.SelectedIndex = 1;
                            }
                            else if (noofdays == 2)
                            {
                                ddlotesidays.SelectedIndex = 2;
                            }
                            else if (noofdays == 3)
                            {
                                ddlotesidays.SelectedIndex = 3;
                            }
                            else if (noofdays == 4)
                            {
                                ddlotesidays.SelectedIndex = 4;
                            }
                            else if (noofdays == 5)
                            {
                                ddlotesidays.SelectedIndex = 5;
                            }
                            else if (noofdays == 6)
                            {
                                ddlotesidays.SelectedIndex = 6;
                            }
                            else if (noofdays == 7)
                            {
                                ddlotesidays.SelectedIndex = 7;
                            }
                            else if (noofdays == 8)
                            {
                                ddlotesidays.SelectedIndex = 8;
                            }

                            else
                            {
                                ddlotesidays.SelectedValue = dtSalStructure.Rows[0]["otesiwagesdays"].ToString();
                            }
                        }

                    }



                    if (String.IsNullOrEmpty(dtSalStructure.Rows[0]["Nnhs"].ToString()) != false)
                    {
                        ddlNoOfNhsPaysheet.SelectedIndex = 0;
                    }
                    else
                    {
                        if (dtSalStructure.Rows[0]["Nnhs"].ToString().Trim().Length > 0)
                        {
                            float noofdays = float.Parse(dtSalStructure.Rows[0]["Nnhs"].ToString());
                            if (noofdays == 0)
                            {
                                ddlNoOfNhsPaysheet.SelectedIndex = 0;
                            }

                            else if (noofdays == 1)
                            {
                                ddlNoOfNhsPaysheet.SelectedIndex = 1;
                            }
                            else if (noofdays == 2)
                            {
                                ddlNoOfNhsPaysheet.SelectedIndex = 2;
                            }
                            else if (noofdays == 3)
                            {
                                ddlNoOfNhsPaysheet.SelectedIndex = 3;
                            }
                            else if (noofdays == 4)
                            {
                                ddlNoOfNhsPaysheet.SelectedIndex = 4;
                            }
                            else if (noofdays == 5)
                            {
                                ddlNoOfNhsPaysheet.SelectedIndex = 5;
                            }
                            else
                            {
                                ddlNoOfNhsPaysheet.SelectedValue = dtSalStructure.Rows[0]["Nnhs"].ToString();
                            }
                        }

                    }


                    if (String.IsNullOrEmpty(dtSalStructure.Rows[0]["NoOfDays"].ToString()) != false)
                    {
                        ddlNoOfDaysWages.SelectedIndex = 0;
                    }
                    else
                    {
                        if (dtSalStructure.Rows[0]["NoOfDays"].ToString().Trim().Length > 0)
                        {
                            float noofdays = float.Parse(dtSalStructure.Rows[0]["NoOfDays"].ToString());
                            if (noofdays == 0)
                            {
                                ddlNoOfDaysWages.SelectedIndex = 0;
                            }

                            else if (noofdays == 1)
                            {
                                ddlNoOfDaysWages.SelectedIndex = 1;
                            }
                            else if (noofdays == 2)
                            {
                                ddlNoOfDaysWages.SelectedIndex = 2;
                            }
                            else if (noofdays == 3)
                            {
                                ddlNoOfDaysWages.SelectedIndex = 3;
                            }
                            else if (noofdays == 4)
                            {
                                ddlNoOfDaysWages.SelectedIndex = 4;
                            }
                            else
                            {
                                ddlNoOfDaysWages.SelectedValue = dtSalStructure.Rows[0]["NoOfDays"].ToString();
                            }
                        }
                    }

                    #endregion for Salary Structure
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void LoadIDList()
        {
            string Empid = "";
            if (txtEmpid.Text.Length > 0)
            {
                Empid = txtEmpid.Text;
            }
            string qry = "select distinct id from EmpSalaryStructure where empid='" + Empid + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlID.DataValueField = "ID";
                ddlID.DataTextField = "ID";
                ddlID.DataSource = dt;
                ddlID.DataBind();
            }
            ddlID.Items.Insert(0, "-Select-");
        }

        protected void ddlID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Getdata();
            // LoadIDList();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (txtStartingDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill From date.');", true);
                return;
            }

            if (txtEndingDate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill to date.');", true);
                return;
            }

            if (txtStartingDate.Text.Trim().Length != 0 && txtEndingDate.Text.Trim().Length != 0)
            {
                DateTime Dtstartdate = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb"));
                DateTime DtEnddate = DateTime.Parse(txtEndingDate.Text, CultureInfo.GetCultureInfo("en-gb"));


                if (Dtstartdate >= DtEnddate)
                {
                    lblMsg.Text = "Invalid To Date . To Date Always Should Be Greater Than To From Date.";
                    return;
                }
            }
            var testDate = 0;

            #region     Begin Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]


            if (txtStartingDate.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtStartingDate.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "show alert", "alert('You Are Entered Invalid  from Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }
                else
                {
                    string CheckSD = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
                    string CheckED = Timings.Instance.CheckDateFormat(txtEndingDate.Text);

                    //string CheckSD = DateTime.Parse(txtStartingDate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();

                    string CheckStartDate = "";

                    if (ddlID.SelectedIndex == 0)
                    {
                        CheckStartDate = " select empid from EmpSalaryStructure  where todate>='" +
                            CheckSD + "'  and empid='" + txtEmpid.Text+ "'";

                        DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(CheckStartDate).Result;
                        if (Dt.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                              "show alert", "alert('You Are Entered Invalid  from Date.from Date Should Not Be Interval of the Previous from and To Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ddlID.SelectedIndex > 0)
                        {

                            string CIDForCheck = (txtid.Text).ToString();
                            if (CIDForCheck != "")
                            {
                                CheckStartDate = " select empid from EmpSalaryStructure  where todate>='" +
                                    CheckSD + "'  and empid='" + txtEmpid.Text+ "'  and id<" + CIDForCheck;

                                DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(CheckStartDate).Result;
                                if (Dt.Rows.Count > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(),
                                      "show alert", "alert('You Are Entered Invalid From Date.From Date Should Not Be Interval of the Previous From and To Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                    return;
                                }
                            }
                            else
                            {
                                CheckStartDate = " select empid from EmpSalaryStructure  where '" + CheckSD + "' between FromDate and todate  and empid='" + txtEmpid.Text+ "'  and id!=" + ddlID.SelectedValue;

                                DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(CheckStartDate).Result;
                                if (Dt.Rows.Count > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(),
                                      "show alert", "alert('You Are Entered Invalid From Date.From Date Should Not Be Interval of the Previous From and To Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                    return;
                                }

                                CheckStartDate = " select empid from EmpSalaryStructure  where '" + CheckED + "' between FromDate and todate  and empid='" + txtEmpid.Text+ "'  and id!=" + ddlID.SelectedValue;

                                DataTable Dts = config.ExecuteAdaptorAsyncWithQueryParams(CheckStartDate).Result;
                                if (Dts.Rows.Count > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(),
                                      "show alert", "alert('You Are Entered Invalid From Date.From Date Should Not Be Interval of the Previous From and To Dates  Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                    return;
                                }

                            }
                        }
                    }

                }


            }


            #endregion  End Code For Check The Contract Start Date Entered Or Not  as on [18-10-2013]

            #region for New Fields fo Salary Structure
            var Status = "0";
            var Basic = "0";
            var DA = "0";
            var HRA = "0";
            var Conveyance = "0";
            var CCA = "0";
            var Bonus = "0";
            var Gratuity = "0";
            var WA = "0";
            var NFHs = "0";
            var MedicalAllw = "0";
            var MobileAllw = "0";
            var SplAllowance = "0";
            var LA = "0";
            var OA = "0";
            var RC = "0";
            var CS = "0";
            var NoofDays = "0";
            var NoOfOts = "0";
            var NoOfOTESIDays = "0";
            var OTRate = "0";
            var TravellingAllowance = "0";
            var PerformanceAllowance = "0";
            var PFPayrate = "0";
            var ESIPayrate = "0";
            var EducationAllw = "0";
            var PFVoluntary = "0";

            var shift1Rate = "0";
            var shift2Rate = "0";
            var nightshiftRate = "0";
            var lateloginRate = "0";
            var ExtraOne = "0";
            var ExtraTwo = "0";
            var NoOfshift1 = "0";
            var NoOfshift2 = "0";
            var NoOfnightshift = "0";
            var NoOfLateLogin = "0";
            var Id = "";
            var NotsSpl = "0";

            var ServiceWeightage = "0";
            var RankAllowance = "0";
            var Insurance = "0";
            var MobileRecharge = "0";
            var Extra1 = "0";
            var Extra2 = "0";
            var Extra3 = "0";

            var UniformCharges = "0";
            var NUniformCharges = 0;
            var WMC = "0";
            var MedicalPolicy = "0";
            var LWF = "0";
            var OutstationCharges = "0";
            var BillLeaveWages = "0";
            var BillLeaveWagesType = 0;
            var BillBonus = "0";
            var BillBonusType = 0;
            var NHSRate = "0";
            var NNhs = "0";
            var PVCAmount = "0";
            var BGVAmount = "0";
            var GunAllwType = "0";
            var ADDL4HR = "0"; var QTRALLOW = "0"; var RELALLOW = "0"; var OTESICWAGES = "0"; var SITEALLOW = "0"; var GunAllw = "0"; var FireAllw = "0";
            var TelephoneAllw = "0"; var FoodAllw = "0"; var Reimbursement = "0"; var HardshipAllw = "0"; var PaidHolidayAllw = "0"; var ServiceCharge = "0";

            ADDL4HR = txtADDL4HR.Text;
            QTRALLOW = txtQTRALLOW.Text;
            RELALLOW = txtRELALLOW.Text;
            OTESICWAGES = txtOTESICWAGES.Text;
            SITEALLOW = txtSITEALLOW.Text;
            GunAllw = txtGunAllw.Text;
            FireAllw = txtFireAllw.Text;
            TelephoneAllw = txtTelephoneAllw.Text;
            FoodAllw = txtFoodAllw.Text;
            Reimbursement = txtReimbursement.Text;
            HardshipAllw = txtHardshipAllw.Text;
            PaidHolidayAllw = txtPaidHolidayAllw.Text;
            ServiceCharge = txtServiceCharge.Text;

            shift1Rate = txtshift1Rate.Text;
            shift2Rate = txtshift2Rate.Text;
            nightshiftRate = txtnightshiftRate.Text;
            lateloginRate = txtlateloginRate.Text;
            ExtraOne = txtExtraOne.Text;
            ExtraTwo = txtExtraTwo.Text;
            Id = txtid.Text;

            UniformCharges = txtunicharges.Text;
            NUniformCharges = ddlNunichrges.SelectedIndex;
            WMC = txtwmc.Text;
            MedicalPolicy = txtmedicalpolicy.Text;
            LWF = txtlwf.Text;
            OutstationCharges = txtoutstationCharges.Text;

            ServiceWeightage = txtServiceWeightage.Text;
            RankAllowance = txtRankAllowance.Text;
            Insurance = txtInsurance.Text;
            MobileRecharge = txtMobileRecharge.Text;
            Extra1 = txtExta1.Text;
            Extra2 = txtExtra2.Text;
            Extra3 = txtExra3.Text;
            NHSRate = TxtNHSRate.Text;
            PVCAmount = txtpvc.Text;
            BGVAmount = txtbgv.Text;
            GunAllwType = ddlGunAllwType.SelectedValue;
            if (ddl1shiftdays.SelectedIndex == 0)
            {
                NoOfshift1 = "0";
            }
            if (ddl1shiftdays.SelectedIndex == 1)
            {
                NoOfshift1 = "1";
            }
            if (ddl1shiftdays.SelectedIndex == 2)
            {
                NoOfshift1 = "2";
            }
            if (ddl1shiftdays.SelectedIndex == 3)
            {
                NoOfshift1 = "3";
            }
            if (ddl1shiftdays.SelectedIndex == 4)
            {
                NoOfshift1 = "4";
            }

            if (ddl1shiftdays.SelectedIndex > 4)
            {
                NoOfshift1 = ddl1shiftdays.SelectedValue;
            }

            if (ddl2shiftdays.SelectedIndex == 0)
            {
                NoOfshift2 = "0";
            }
            if (ddl2shiftdays.SelectedIndex == 1)
            {
                NoOfshift2 = "1";
            }
            if (ddl2shiftdays.SelectedIndex == 2)
            {
                NoOfshift2 = "2";
            }
            if (ddl2shiftdays.SelectedIndex == 3)
            {
                NoOfshift2 = "3";
            }
            if (ddl2shiftdays.SelectedIndex == 4)
            {
                NoOfshift2 = "4";
            }

            if (ddl2shiftdays.SelectedIndex > 4)
            {
                NoOfshift2 = ddl2shiftdays.SelectedValue;
            }

            if (ddlNightshiftdays.SelectedIndex == 0)
            {
                NoOfnightshift = "0";
            }
            if (ddlNightshiftdays.SelectedIndex == 1)
            {
                NoOfnightshift = "1";
            }
            if (ddlNightshiftdays.SelectedIndex == 2)
            {
                NoOfnightshift = "2";
            }
            if (ddlNightshiftdays.SelectedIndex == 3)
            {
                NoOfnightshift = "3";
            }
            if (ddlNightshiftdays.SelectedIndex == 4)
            {
                NoOfnightshift = "4";
            }

            if (ddlNightshiftdays.SelectedIndex > 4)
            {
                NoOfnightshift = ddlNightshiftdays.SelectedValue;
            }

            NotsSpl = ddlnotsspl.SelectedValue;

            if (ddlLateLogindays.SelectedIndex == 0)
            {
                NoOfLateLogin = "0";
            }
            if (ddlLateLogindays.SelectedIndex == 1)
            {
                NoOfLateLogin = "1";
            }
            if (ddlLateLogindays.SelectedIndex == 2)
            {
                NoOfLateLogin = "2";
            }
            if (ddlLateLogindays.SelectedIndex == 3)
            {
                NoOfLateLogin = "3";
            }
            if (ddlLateLogindays.SelectedIndex == 4)
            {
                NoOfLateLogin = "4";
            }

            if (ddlLateLogindays.SelectedIndex > 4)
            {
                NoOfLateLogin = ddlLateLogindays.SelectedValue;
            }

            Status = ddlempsalstatus.SelectedValue;
            Basic = TxtBasic.Text;
            DA = txtda.Text;
            HRA = txthra.Text;
            Conveyance = txtConveyance.Text;
            CCA = txtcca.Text;
            Bonus = txtbonus.Text;
            Gratuity = txtgratuty.Text;
            WA = txtwa.Text;
            NFHs = txtNfhs1.Text;
            MedicalAllw = txtMedicalAllw.Text;
            MobileAllw = txtMobileAllowance.Text;
            SplAllowance = txtSplAllw.Text;
            LA = txtleaveamount.Text;
            OA = txtoa.Text;
            RC = Txtrc.Text;
            CS = TxtCs.Text;
            OTRate = TxtOTRate.Text;
            TravellingAllowance = txtTravellingAllowance.Text;
            PerformanceAllowance = txtPerformanceAllowance.Text;
            PFPayrate = TxtPFPayRate.Text;
            ESIPayrate = TxtESIPayRate.Text;
            EducationAllw = txtEducationAllowance.Text;
            PFVoluntary = TxtPFVoluntary.Text;
            BillLeaveWages = txtBillLeaveWages.Text;
            BillBonus = txtBillBonus.Text;

            if (ddlNoOfDaysWages.SelectedIndex == 0)
            {
                NoofDays = "0";
            }
            if (ddlNoOfDaysWages.SelectedIndex == 1)
            {
                NoofDays = "1";
            }
            if (ddlNoOfDaysWages.SelectedIndex == 2)
            {
                NoofDays = "2";
            }
            if (ddlNoOfDaysWages.SelectedIndex == 3)
            {
                NoofDays = "3";
            }
            if (ddlNoOfDaysWages.SelectedIndex == 4)
            {
                NoofDays = "4";
            }

            if (ddlNoOfDaysWages.SelectedIndex > 4)
            {
                NoofDays = ddlNoOfDaysWages.SelectedValue;
            }

            string Shift = "0";

                Shift = ddlShift.SelectedValue;
           

            var Nbonus = 0;
            var InsuranceType = 0;
            var UniformChargesType = 0;
            var MobileRechargeType = 0;
            var OutStationChargesType = 0;
            var MobileRechargeCaln = 0;
            var OutsationChargeCaln = 0;

            Nbonus = ddlnbonus.SelectedIndex;

            InsuranceType = ddlInsuranceType.SelectedIndex;
            UniformChargesType = ddlUniformChargesType.SelectedIndex;
            MobileRechargeType = ddlMobileRechargeType.SelectedIndex;
            OutStationChargesType = ddlOutStationChargesType.SelectedIndex;
            BillLeaveWagesType = ddlBillLeaveWagesType.SelectedIndex;
            BillBonusType = ddlBillBonusType.SelectedIndex;
            MobileRechargeCaln = ddlMobileChrgCaln.SelectedIndex;
            OutsationChargeCaln = ddlOutstationChrgCaln.SelectedIndex;

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

            if (ddlotesidays.SelectedIndex == 0)
            {
                NoOfOTESIDays = "0";
            }
            if (ddlotesidays.SelectedIndex == 1)
            {
                NoOfOTESIDays = "1";
            }
            if (ddlotesidays.SelectedIndex == 2)
            {
                NoOfOTESIDays = "2";
            }
            if (ddlotesidays.SelectedIndex == 3)
            {
                NoOfOTESIDays = "3";
            }
            if (ddlotesidays.SelectedIndex == 4)
            {
                NoOfOTESIDays = "4";
            }
            if (ddlotesidays.SelectedIndex == 5)
            {
                NoOfOTESIDays = "5";
            }
            if (ddlotesidays.SelectedIndex == 6)
            {
                NoOfOTESIDays = "6";
            }
            if (ddlotesidays.SelectedIndex == 7)
            {
                NoOfOTESIDays = "7";
            }
            if (ddlotesidays.SelectedIndex == 8)
            {
                NoOfOTESIDays = "8";
            }
            if (ddlotesidays.SelectedIndex > 8)
            {
                NoOfOTESIDays = ddlotesidays.SelectedValue;
            }

            if (ddlNoOfNhsPaysheet.SelectedIndex == 0)
            {
                NNhs = "0";
            }
            if (ddlNoOfNhsPaysheet.SelectedIndex == 1)
            {
                NNhs = "1";
            }
            if (ddlNoOfNhsPaysheet.SelectedIndex == 2)
            {
                NNhs = "2";
            }
            if (ddlNoOfNhsPaysheet.SelectedIndex == 3)
            {
                NNhs = "3";
            }
            if (ddlNoOfNhsPaysheet.SelectedIndex == 4)
            {
                NNhs = "4";
            }
            if (ddlNoOfNhsPaysheet.SelectedIndex > 4)
            {
                NNhs = ddlNoOfNhsPaysheet.SelectedValue;
            }


            string pfnods = "0";

            if (ddlPFNoOfDaysForWages.SelectedIndex == 0)
            {
                pfnods = "0";
            }
            if (ddlPFNoOfDaysForWages.SelectedIndex == 1)
            {
                pfnods = "1";
            }
            if (ddlPFNoOfDaysForWages.SelectedIndex == 2)
            {
                pfnods = "2";
            }
            if (ddlPFNoOfDaysForWages.SelectedIndex == 3)
            {
                pfnods = "3";
            }
            if (ddlPFNoOfDaysForWages.SelectedIndex == 4)
            {
                pfnods = "4";
            }

            if (ddlPFNoOfDaysForWages.SelectedIndex > 4)
            {
                pfnods = ddlPFNoOfDaysForWages.SelectedValue;
            }

            var ContractStartDate = "";
            var ContractEndDate = "";
            ContractStartDate = Timings.Instance.CheckDateFormat(txtStartingDate.Text);
            ContractEndDate = Timings.Instance.CheckDateFormat(txtEndingDate.Text);



            string ID = "";


            string Checkempid = " select empid from EmpSalaryStructure  where empid='" + txtEmpid.Text+ "'";
            DataTable dtCheckempid = config.ExecuteAdaptorAsyncWithQueryParams(Checkempid).Result;
            string ChkEMpid = "";
            if (dtCheckempid.Rows.Count > 0)
            {
                if (txtid.Text == " " || txtid.Text == "-Select-")
                {
                    ID = "";
                }
                else
                {
                    ID = txtid.Text;
                }
            }
            else
            {
                ID = "1";
            }


            int status = 0;
            if (ID.Length > 0)
            {
                string qryinsert = "insert into  EmpSalaryStructure(empid,Designations,id,NoOfDays,Basic,HRA,DA,Conveyance,CCA,WashAllowance,OtherAllowance,LeaveAmount,Bonus,Gratuity,NFhs,RC,CS,SplAllowance," +
               " MedicalAllowance,MobileAllowance,OTRate,Nots," +
              " EducationAllowance,BonusType,fromdate,todate," +
              "Extra1,Extra2,Extra3,Extra4,Extra5,LWF,nhsrate,nnhs,ServiceWeightage,BGVAmt,PVCAmt,ADDL4HR,QTRALLOW,RELALLOW,OTESICWAGES,SITEALLOW,GunAllw,FireAllw, " +
"TelephoneAllw,FoodAllw ,Reimbursement,HardshipAllw,PaidHolidayAllw,ServiceCharge,Pfpayrate,ESIpayrate,PFNoOfDays,PerformanceAllowance,TravellingAllowance,Status,otesiwagesdays,GunAllwType,RankAllw,Shift)" +
              "values ('" + txtEmpid.Text+ "','" + txtdesignation.Text + "','" + ID + "','" + NoofDays + "','" + Basic + "','" + HRA + "','" + DA + "','" + Conveyance + "','" + CCA + "','" + WA + "','" + OA + "','" + LA + "','" + Bonus + "','" + Gratuity + "','" + NFHs + "','" + RC + "','" + CS + "','" + SplAllowance + "','" + MedicalAllw + "','" + MobileAllw + "'," +
              "  '" + OTRate + "','" + NoOfOts + "'," +
               " '" + EducationAllw + "'," +
              "  '" + Nbonus + "', '" + ContractStartDate + "', '" + ContractEndDate + "', " +
              " '" + Extra1 + "', '" + Extra2 + "', '" + Extra3 + "', '" + Extra3 + "', '" + Extra3 + "', '" + LWF + "','" + NHSRate + "','" + NNhs + "','" + ServiceWeightage + "','" + BGVAmount + "','" + PVCAmount + "','" + ADDL4HR + "','" + QTRALLOW + "','" + RELALLOW + "','" + OTESICWAGES + "','" + SITEALLOW + "','" + GunAllw + "','" + FireAllw + "','" + TelephoneAllw + "','" + FoodAllw + "','" + Reimbursement + "','" + HardshipAllw + "','" + PaidHolidayAllw + "','" + ServiceCharge + "','" + PFPayrate + "','" + ESIPayrate + "','" + pfnods + "','" + PerformanceAllowance + "','" + TravellingAllowance + "','" + Status + "','" + NoOfOTESIDays + "','"+ GunAllwType + "','"+ RankAllowance+ "','" + Shift + "')";
                status = config.ExecuteNonQueryWithQueryAsync(qryinsert).Result;
                if (status > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Details inserted successfully...');", true);

                }
            }
            else
            {
                string qry = "  update EmpSalaryStructure set NoOfDays='" + NoofDays + "',Basic='" + Basic + "',HRA='" + HRA + "' ,DA='" + DA + "',Conveyance='" + Conveyance + "',CCA='" + CCA + "',WashAllowance='" + WA + "',OtherAllowance='" + OA + "',LeaveAmount='" + LA + "',Bonus='" + Bonus + "',Gratuity='" + Gratuity + "',NFhs='" + NFHs + "',RC='" + RC + "',CS='" + CS + "',SplAllowance='" + SplAllowance + "',MedicalAllowance='" + MedicalAllw + "',MobileAllowance='" + MobileAllw + "',ServiceWeightage='" + ServiceWeightage + "',BGVAmt='" + BGVAmount + "',PVCAmt='" + PVCAmount + "'," +
                                "OTRate='" + OTRate + "',Nots='" + NoOfOts + "' ," +
                                "BonusType ='" + Nbonus + "',fromdate='" + ContractStartDate + "',todate='" + ContractEndDate + "',Extra1='" + Extra1 + "',Extra2='" + Extra2 + "',Extra3='" + Extra3 + "'," +
                                "LWF='" + LWF + "',nhsrate='" + NHSRate + "',nnhs='" + NNhs + "',ADDL4HR='" + ADDL4HR + "',QTRALLOW='" + QTRALLOW + "',RELALLOW='" + RELALLOW + "',OTESICWAGES='" + OTESICWAGES + "',SITEALLOW='" + SITEALLOW + "',GunAllw='" + GunAllw + "',FireAllw='" + FireAllw + "',TelephoneAllw='" + TelephoneAllw + "',FoodAllw='" + FoodAllw + "',Reimbursement='" + Reimbursement + "',HardshipAllw='" + HardshipAllw + "',PaidHolidayAllw='" + PaidHolidayAllw + "',ServiceCharge='" + ServiceCharge + "',Pfpayrate='" + PFPayrate + "',ESIpayrate='" + ESIPayrate + "',PFNoOfDays='" + pfnods + "',PerformanceAllowance='" + PerformanceAllowance + "',TravellingAllowance='" + TravellingAllowance + "',Status='" + Status + "',otesiwagesdays='" + NoOfOTESIDays + "',GunAllwType='"+ GunAllwType + "',RankAllw='" + RankAllowance + "',Shift='" + Shift + "' where empid='" + txtEmpid.Text+ "' and id='" + ddlID.SelectedValue + "'";
                status = config.ExecuteNonQueryWithQueryAsync(qry).Result;
                if (status > 0)
                {
                    ClearDataFromPersonalInfoTabFields();
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Details Updated successfully...');", true);
                }
            }
            ClearData();
            LoadIDList();
            #endregion for New Fields fo Salary Structure
        }

        public void ClearData()
        {
            txtid.Text = txtStartingDate.Text = txtEndingDate.Text = txtEmpid.Text = txtName.Text = string.Empty;
            txtshift1Rate.Text = txtshift2Rate.Text = txtnightshiftRate.Text = txtlateloginRate.Text = txtExtraOne.Text = txtExtraTwo.Text =
                TxtBasic.Text = txtda.Text =
                txthra.Text = txtConveyance.Text = txtcca.Text = txtbonus.Text = txtgratuty.Text = txtwa.Text = txtNfhs1.Text = txtMedicalAllw.Text =
    txtMobileAllowance.Text = txtSplAllw.Text = txtleaveamount.Text = txtoa.Text = Txtrc.Text = TxtCs.Text = TxtOTRate.Text = txtTravellingAllowance.Text =
    txtPerformanceAllowance.Text = TxtPFPayRate.Text = TxtPFPayRate.Text = TxtESIPayRate.Text = txtEducationAllowance.Text = TxtPFVoluntary.Text =
 txtBillBonus.Text = txtBillLeaveWages.Text =
      txtADDL4HR.Text = txtQTRALLOW.Text = txtRELALLOW.Text = txtOTESICWAGES.Text = txtSITEALLOW.Text = txtGunAllw.Text = txtFireAllw.Text = txtTelephoneAllw.Text =
      txtFoodAllw.Text = txtReimbursement.Text = txtHardshipAllw.Text = txtPaidHolidayAllw.Text = txtServiceCharge.Text = "0";

            ddlID.SelectedIndex = ddl1shiftdays.SelectedIndex = ddl2shiftdays.SelectedIndex = ddlNightshiftdays.SelectedIndex = ddlLateLogindays.SelectedIndex = ddlNoOfDaysWages.SelectedIndex
            = ddlnbonus.SelectedIndex = ddlInsuranceType.SelectedIndex = ddlBillLeaveWagesType.SelectedIndex = ddlUniformChargesType.SelectedIndex = ddlMobileRechargeType.SelectedIndex = ddlOutStationChargesType.SelectedIndex = ddlNoOfOtsPaysheet.SelectedIndex = ddlPFNoOfDaysForWages.SelectedIndex=ddlShift.SelectedIndex = 0;

        }

        protected void Btn_Renewal_Click(object sender, EventArgs e)
        {
            if (txtEmpid.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select  Emp ID/Name');", true);
                return;
            }
            else
            {
                txtid.Text = GlobalData.Instance.LoadMaxId(txtEmpid.Text);
                ddlID.SelectedIndex = 0;
                txtStartingDate.Text = txtEndingDate.Text = "";
            }

        }

        protected void ChkSum_CheckedChanged(object sender, EventArgs e)
        {

            if (ChkSum.Checked == true)
            {


                decimal Basic = 0;
                decimal DA = 0;
                decimal HRA = 0;
                decimal Conveyance = 0;
                decimal CCA = 0;
                decimal Bonus = 0;
                decimal Gratuity = 0;
                decimal WA = 0;
                decimal NFHs = 0;
                decimal MedicalAllw = 0;
                decimal MobileAllw = 0;
                decimal SplAllowance = 0;
                decimal LA = 0;
                decimal OA = 0;
                decimal RC = 0;
                decimal CS = 0;
                decimal NoofDays = 0;
                decimal NoOfOts = 0;
                decimal OTRate = 0;
                decimal TravellingAllowance = 0;
                decimal PerformanceAllowance = 0;
                decimal PFPayrate = 0;
                decimal ESIPayrate = 0;
                decimal EducationAllw = 0;
                decimal PFVoluntary = 0;

                decimal shift1Rate = 0;
                decimal shift2Rate = 0;
                decimal nightshiftRate = 0;
                decimal lateloginRate = 0;
                decimal ExtraOne = 0;
                decimal ExtraTwo = 0;
                decimal NoOfshift1 = 0;
                decimal NoOfshift2 = 0;
                decimal NoOfnightshift = 0;
                decimal NoOfLateLogin = 0;
                var Id = "";
                decimal NotsSpl = 0;

                decimal ServiceWeightage = 0;
                decimal RankAllowance = 0;
                decimal Insurance = 0;
                decimal MobileRecharge = 0;
                decimal Extra1 = 0;
                decimal Extra2 = 0;
                decimal Extra3 = 0;

                decimal UniformCharges = 0;
                decimal NUniformCharges = 0;
                decimal WMC = 0;
                decimal MedicalPolicy = 0;
                decimal LWF = 0;
                decimal OutstationCharges = 0;
                decimal BillLeaveWages = 0;
                decimal BillLeaveWagesType = 0;
                decimal BillBonus = 0;
                decimal BillBonusType = 0;
                decimal NHSRate = 0;
                decimal NNhs = 0;

                decimal ADDL4HR = 0; decimal QTRALLOW = 0; decimal RELALLOW = 0; decimal OTESICWAGES = 0; decimal SITEALLOW = 0; decimal GunAllw = 0; decimal FireAllw = 0;
                decimal TelephoneAllw = 0; decimal FoodAllw = 0; decimal Reimbursement = 0; decimal HardshipAllw = 0; decimal PaidHolidayAllw = 0; decimal ServiceCharge = 0;

                if (txtADDL4HR.Text.Length > 0)
                {
                    ADDL4HR = decimal.Parse(txtADDL4HR.Text);
                }
                if (txtQTRALLOW.Text.Length > 0)
                {
                    QTRALLOW = decimal.Parse(txtQTRALLOW.Text);
                }
                if (txtRELALLOW.Text.Length > 0)
                {
                    RELALLOW = decimal.Parse(txtRELALLOW.Text);
                }
                if (txtOTESICWAGES.Text.Length > 0)
                {
                    OTESICWAGES = decimal.Parse(txtOTESICWAGES.Text);
                }
                if (txtSITEALLOW.Text.Length > 0)
                {
                    SITEALLOW = decimal.Parse(txtSITEALLOW.Text);
                }
                if (txtGunAllw.Text.Length > 0)
                {
                    GunAllw = decimal.Parse(txtGunAllw.Text);
                }
                if (txtFireAllw.Text.Length > 0)
                {
                    FireAllw = decimal.Parse(txtFireAllw.Text);
                }
                if (txtTelephoneAllw.Text.Length > 0)
                {
                    TelephoneAllw = decimal.Parse(txtTelephoneAllw.Text);
                }
                if (txtFoodAllw.Text.Length > 0)
                {
                    FoodAllw = decimal.Parse(txtFoodAllw.Text);
                }
                if (txtReimbursement.Text.Length > 0)
                {
                    Reimbursement = decimal.Parse(txtReimbursement.Text);
                }
                if (txtHardshipAllw.Text.Length > 0)
                {
                    HardshipAllw = decimal.Parse(txtHardshipAllw.Text);
                }
                if (txtPaidHolidayAllw.Text.Length > 0)
                {
                    PaidHolidayAllw = decimal.Parse(txtPaidHolidayAllw.Text);
                }
                if (txtServiceCharge.Text.Length > 0)
                {
                    ServiceCharge = decimal.Parse(txtServiceCharge.Text);
                }

                if (txtPerformanceAllowance.Text.Length > 0)
                {
                    PerformanceAllowance = decimal.Parse(txtPerformanceAllowance.Text);
                }
                if (txtTravellingAllowance.Text.Length > 0)
                {
                    TravellingAllowance = decimal.Parse(txtTravellingAllowance.Text);
                }

                if (TxtBasic.Text.Length > 0)
                {
                    Basic = decimal.Parse(TxtBasic.Text);
                }
                if (txtda.Text.Length > 0)
                {
                    DA = decimal.Parse(txtda.Text);
                }
                if (txtConveyance.Text.Length > 0)
                {
                    Conveyance = decimal.Parse(txtConveyance.Text);
                }
                if (txtcca.Text.Length > 0)
                {
                    CCA = decimal.Parse(txtcca.Text);
                }

                if (txthra.Text.Length > 0)
                {
                    HRA = decimal.Parse(txthra.Text);
                }

                if (txtbonus.Text.Length > 0)
                {
                    Bonus = decimal.Parse(txtbonus.Text);
                }
                if (txtgratuty.Text.Length > 0)
                {
                    Gratuity = decimal.Parse(txtgratuty.Text);
                }
                if (txtwa.Text.Length > 0)
                {
                    WA = decimal.Parse(txtwa.Text);
                }
                if (txtNfhs1.Text.Length > 0)
                {
                    NFHs = decimal.Parse(txtNfhs1.Text);
                }

                if (txtMedicalAllw.Text.Length > 0)
                {
                    MedicalAllw = decimal.Parse(txtMedicalAllw.Text);
                }
                if (txtMobileAllowance.Text.Length > 0)
                {
                    MobileAllw = decimal.Parse(txtMobileAllowance.Text);
                }
                if (txtSplAllw.Text.Length > 0)
                {
                    SplAllowance = decimal.Parse(txtSplAllw.Text);
                }
                if (txtleaveamount.Text.Length > 0)
                {
                    LA = decimal.Parse(txtleaveamount.Text);
                }


                if (txtoa.Text.Length > 0)
                {
                    OA = decimal.Parse(txtoa.Text);
                }
                if (Txtrc.Text.Length > 0)
                {
                    RC = decimal.Parse(Txtrc.Text);
                }
                if (TxtCs.Text.Length > 0)
                {
                    CS = decimal.Parse(TxtCs.Text);
                }
                if (txtRankAllowance.Text.Length > 0)
                {
                    RankAllowance = decimal.Parse(txtRankAllowance.Text);
                }


                decimal totalsum = 0;

                if (ChkSum.Checked == true)
                {
                    totalsum = (Basic + HRA + CCA + Gratuity + WA + RC + DA + Conveyance + LA + Bonus + OA + CS + MedicalAllw + MobileAllw + SplAllowance +
                        ADDL4HR + QTRALLOW + RELALLOW + SITEALLOW + GunAllw + FireAllw +
TelephoneAllw + FoodAllw + Reimbursement + HardshipAllw + PaidHolidayAllw + ServiceCharge + TravellingAllowance + PerformanceAllowance + RankAllowance);
                    txtsum.Text = totalsum.ToString();
                }

            }
        }
    }
}