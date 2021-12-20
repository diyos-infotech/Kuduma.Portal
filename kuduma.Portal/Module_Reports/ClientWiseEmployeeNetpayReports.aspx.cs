using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KLTS.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ClientWiseEmployeeNetpayReports : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string Zone = "";
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
                    if (this.Master != null)
                    {
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli2");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }

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
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVClientsData.DataSource = null;
            GVClientsData.DataBind();
            var SPName = "";
            int options = 0;
            Hashtable HTPaysheet = new Hashtable();
            if (ddloptions.SelectedIndex == 0)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                    return;
                }

                else
                {
                    string date = string.Empty;

                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }
                    string monthnew = string.Empty;

                    string month = DateTime.Parse(date).Month.ToString();
                    string Year = DateTime.Parse(date).Year.ToString();

                    if (ddloptions.SelectedIndex == 0)
                    {
                        monthnew = month + Year.Substring(2, 2);
                        SPName = "GetClientList";
                        options = ddloptions.SelectedIndex;
                        HTPaysheet.Add("@month", monthnew);
                        HTPaysheet.Add("@Options", options);
                        HTPaysheet.Add("@CmpIDPrefix", CmpIDPrefix);
                    }

                }
            }

            else
            {
                if (txtfrom.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select From Month');", true);

                    return;
                }

                if (txtto.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select To Month');", true);

                    return;
                }

                else
                {
                    string Fromdate = string.Empty;
                    string ToMonth = string.Empty;

                    if (txtfrom.Text.Trim().Length > 0)
                    {
                        Fromdate = DateTime.Parse(txtfrom.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }
                    if (txtto.Text.Trim().Length > 0)
                    {
                        ToMonth = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }
                    SPName = "GetClientList";
                    options = ddloptions.SelectedIndex;
                    HTPaysheet.Add("@FromMonth", Fromdate);
                    HTPaysheet.Add("@TOMonth", ToMonth);
                    HTPaysheet.Add("@Options", options);
                    HTPaysheet.Add("@CmpIDPrefix", CmpIDPrefix);
                }


            }
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;
            if (dt.Rows.Count > 0)
            {
                GVClientsData.DataSource = dt;
                GVClientsData.DataBind();
                btngetdata.Visible = true;
                btn_Submit.Visible = false;

            }

            //DisplayData();
        }

        protected void Bindata(string Sqlqry)
        {
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Salary Details For The Selected client');", true);

            }
        }

        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            GVClientsData.DataSource = null;
            GVClientsData.DataBind();
            // lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            try
            {


                // gve.ExportGrid("WageSheetReport" + ".xls", hidGridView);

                string branchesi = "select * from companyinfo";
                string companyname = "";
                string line1 = "";
                string line2 = "";
                DataTable dtbranchesi = config.ExecuteReaderWithQueryAsync(branchesi).Result;
                string monthnew = string.Empty;

                if (ddloptions.SelectedIndex == 0)
                {
                    string date = string.Empty;
                }
                string sqlqry = string.Empty;

                //var list = new List<string>();
                var list = new List<Tuple<int, string>>();
                #region Declaring TotalVariables
                float totalActualamount = 0;
                float totalctcamount = 0;
                float totalDuties = 0;
                float totalOts = 0;
                float totalwo = 0;
                float totalnhs = 0;
                float totalnpots = 0;
                float totaltempgross = 0;
                float totalBasic = 0;
                float totalDA = 0;
                float totalHRA = 0;
                float totalCCA = 0;
                float totalConveyance = 0;
                float totalWA = 0;
                float totalOA = 0;
                float totalGrass = 0;
                float totalOTAmount = 0;
                float totalPF = 0;
                float totalESI = 0;
                float totalProfTax = 0;
                float totalSalAdv = 0;
                float totalUniformDed = 0;
                float totalAdvDed = 0;
                float totalWCDed = 0;
                float totalCanteenAdv = 0;
                float totalLeaveEncashAmt = 0;
                float totalGratuity = 0;
                float totalBonus = 0;
                float totalnfhs = 0;
                float totalDed = 0;
                float totalOtherDed = 0;
                float totalIncentivs = 0;
                float totalWoAmt = 0;
                float totalNhsAmt = 0;
                float totalNpotsAmt = 0;
                float totalPenalty = 0;
                float totalRC = 0;
                float totalCS = 0;
                float totalOWF = 0;
                float totalSecDepDed = 0;
                float totalloanded = 0;
                float totalGenDed = 0;
                float totalctc = 0;

                float totalAttBonus = 0;
                float totalTravelAllw = 0;
                float totalNightShiftAllw = 0;
                float totalFoodAllowance = 0;
                float totalmedicalallowance = 0;
                float totalUniformAllw = 0;

                float totalAdv4Ded = 0;
                float totalNightRoundDed = 0;
                float totalManpowerMobDed = 0;
                float totalMobileusageDed = 0;
                float totalMediClaimDed = 0;
                float totalCrisisDed = 0;
                float totalMobInstDed = 0;
                float totalTDSDed = 0;


                float totalSpecialAllowance = 0;
                float totalMobileAllowance = 0;
                float totalNPCl25Per = 0;
                float totalTransport6Per = 0;
                float totalTransport = 0;

                float totalRentDed = 0;
                float totalMedicalDed = 0;
                float totalMLWFDed = 0;
                float totalFoodDed = 0;
                float totalAddlAmount = 0;


                float totalElectricityDed = 0;
                float totalTransportDed = 0;
                float totalDccDed = 0;
                float totalLeaveDed = 0;
                float totalLicenseDed = 0;
                float totalpfempr = 0;
                float totalesiempr = 0;
                float totalDiv = 0;
                float totalArea = 0;
                float totalTelephoneBillDed = 0;
                #endregion Declaring TotalVariables

                // var monthlist = new List<string>();
                var options = 0;
                if (GVClientsData.Rows.Count > 0)
                {
                    for (int i = 0; i < GVClientsData.Rows.Count; i++)
                    {
                        CheckBox chkclientid = GVClientsData.Rows[i].FindControl("chkindividual") as CheckBox;
                        Label lblclientid = GVClientsData.Rows[i].FindControl("lblclientid") as Label;
                        Label lblmonth = GVClientsData.Rows[i].FindControl("lblmonth") as Label;

                        if (chkclientid.Checked == true)
                        {
                            list.Add(Tuple.Create(Convert.ToInt32(lblmonth.Text), lblclientid.Text));
                        }

                    }
                }

                // string Clientids = string.Join(",", list.ToArray());


                DataTable dtClientList = new DataTable();
                DataTable dtmonth = new DataTable();
                dtClientList.Columns.Add("Clientid");
                dtmonth.Columns.Add("month");
                if (list.Count != 0)
                {
                    //foreach (var s in list)
                    //{
                    //    DataRow row = dtClientList.NewRow();
                    //    row["Clientid"] = s;
                    //    dtClientList.Rows.Add(row);
                    //}

                    foreach (Tuple<int, string> tuple in list)
                    {
                        DataRow row = dtClientList.NewRow();
                        DataRow row1 = dtmonth.NewRow();
                        row["Clientid"] = tuple.Item2;
                        row1["month"] = tuple.Item1;
                        dtClientList.Rows.Add(row["Clientid"]);
                        dtmonth.Rows.Add(row1["month"]);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select Client IDs');", true);
                }

                var SPName = "";
                Hashtable HTPaysheet = new Hashtable();
                int countearnings = 0;
                int countdedutions = 0;
                int countot = 0;
                int countpfempr = 0;
                int countduties = 0;
                int countesiempr = 0;
                int countctc = 0;
                int countfixed = 0;
                int countNetpay = 1;

                string Fromdate = string.Empty;
                string ToMonth = string.Empty;

                if (txtfrom.Text.Trim().Length > 0)
                {
                    Fromdate = DateTime.Parse(txtfrom.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                if (txtto.Text.Trim().Length > 0)
                {
                    ToMonth = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                SPName = "NetPayReport";
                options = ddloptions.SelectedIndex;
                //  HTPaysheet.Add("@FromMonth", Fromdate);
                // HTPaysheet.Add("@TOMonth", ToMonth);
                HTPaysheet.Add("@ClientIDList", dtClientList);
                // HTPaysheet.Add("@Options", options);
                HTPaysheet.Add("@MonthList", dtmonth);


                DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;

                if (dt.Rows.Count > 0)
                {
                    GVListEmployees.DataSource = dt;
                    GVListEmployees.DataBind();
                    lbtn_Export.Visible = true;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // if (dt.Rows[i]["Monthname"].ToString() == "Sub Total")
                        {
                            float actAmount = 0;

                            if (actAmount >= 0)
                            {
                                #region

                                // Total Calculation 


                                //26-04-2019
                                string strCdBasic = dt.Rows[i]["CdBasic"].ToString();
                                if (strCdBasic.Trim().Length > 0)
                                {
                                    totalCdBasic += Convert.ToSingle(strCdBasic);
                                }
                                string strCdDA = dt.Rows[i]["CdDA"].ToString();
                                if (strCdDA.Trim().Length > 0)
                                {
                                    totalCdDA += Convert.ToSingle(strCdDA);
                                }
                                string strCdHRA = dt.Rows[i]["CdHRA"].ToString();
                                if (strCdHRA.Trim().Length > 0)
                                {
                                    totalCdHRA += Convert.ToSingle(strCdHRA);
                                }
                                string strCdCCA = dt.Rows[i]["CdCCA"].ToString();
                                if (strCdCCA.Trim().Length > 0)
                                {
                                    totalCdCCA += Convert.ToSingle(strCdCCA);
                                }
                                string strCdConv = dt.Rows[i]["CdConv"].ToString();
                                if (strCdConv.Trim().Length > 0)
                                {
                                    totalCdConveyance += Convert.ToSingle(strCdConv);
                                }

                                string strCdWA = dt.Rows[i]["CdWA"].ToString();
                                if (strCdWA.Trim().Length > 0)
                                {
                                    totalCdWA += Convert.ToSingle(strCdWA);
                                }
                                string strCdNfhs = dt.Rows[i]["CdNfhs"].ToString();
                                if (strCdNfhs.Trim().Length > 0)
                                {
                                    totalCdNfhs += Convert.ToSingle(strCdNfhs);
                                }
                                string strCdRC = dt.Rows[i]["CdRC"].ToString();
                                if (strCdRC.Trim().Length > 0)
                                {
                                    totalCdrc += Convert.ToSingle(strCdRC);
                                }
                                string strCdCS = dt.Rows[i]["CdCS"].ToString();
                                if (strCdCS.Trim().Length > 0)
                                {
                                    totalCdcs += Convert.ToSingle(strCdCS);
                                }
                                string strCdAddlAmount = dt.Rows[i]["CdAddlAmount"].ToString();
                                if (strCdAddlAmount.Trim().Length > 0)
                                {
                                    totalCdAddlAmount += Convert.ToSingle(strCdAddlAmount);
                                }
                                string strCdFoodAllw = dt.Rows[i]["CdFoodAllw"].ToString();
                                if (strCdFoodAllw.Trim().Length > 0)
                                {
                                    totalCdFoodAllw += Convert.ToSingle(strCdFoodAllw);
                                }
                                string strCdWOAmt = dt.Rows[i]["CdWOAmt"].ToString();
                                if (strCdWOAmt.Trim().Length > 0)
                                {
                                    totalCdWOAmt += Convert.ToSingle(strCdWOAmt);
                                }
                                string strCdNHsAmt = dt.Rows[i]["CdNHsAmt"].ToString();
                                if (strCdNHsAmt.Trim().Length > 0)
                                {
                                    totalCdNHsAmt += Convert.ToSingle(strCdNHsAmt);
                                }
                                string strCdMedicalReimbursement = dt.Rows[i]["CdMedicalReimbursement"].ToString();
                                if (strCdMedicalReimbursement.Trim().Length > 0)
                                {
                                    totalCdMedicalReimbursement += Convert.ToSingle(strCdMedicalReimbursement);
                                }
                                string strCdSpecialAllW = dt.Rows[i]["CdSpecialAllW"].ToString();
                                if (strCdSpecialAllW.Trim().Length > 0)
                                {
                                    totalCdSpecialAllW += Convert.ToSingle(strCdSpecialAllW);
                                }
                                string strCdTravelAllw = dt.Rows[i]["CdTravelAllw"].ToString();
                                if (strCdTravelAllw.Trim().Length > 0)
                                {
                                    totalCdTravelAllw += Convert.ToSingle(strCdTravelAllw);
                                }
                                string strCdMobileAllw = dt.Rows[i]["CdMobileAllw"].ToString();
                                if (strCdMobileAllw.Trim().Length > 0)
                                {
                                    totalCdMobileAllw += Convert.ToSingle(strCdMobileAllw);
                                }
                                string strCdPerformanceAllw = dt.Rows[i]["CdPerformanceAllw"].ToString();
                                if (strCdPerformanceAllw.Trim().Length > 0)
                                {
                                    totalCdPerformanceAllw += Convert.ToSingle(strCdPerformanceAllw);
                                }
                                string strCdLW = dt.Rows[i]["CdLW"].ToString();
                                if (strCdLW.Trim().Length > 0)
                                {
                                    totalCdLW += Convert.ToSingle(strCdLW);
                                }
                                string strCdNPOTsAmt = dt.Rows[i]["CdNPOTsAmt"].ToString();
                                if (strCdNPOTsAmt.Trim().Length > 0)
                                {
                                    totalCdNPOTsAmt += Convert.ToSingle(strCdNPOTsAmt);
                                }
                                string strCdIncentivs = dt.Rows[i]["CdIncentivs"].ToString();
                                if (strCdIncentivs.Trim().Length > 0)
                                {
                                    totalCdIncentivs += Convert.ToSingle(strCdIncentivs);
                                }
                                string strCdBonus = dt.Rows[i]["CdBonus"].ToString();
                                if (strCdBonus.Trim().Length > 0)
                                {
                                    totalCdBonus += Convert.ToSingle(strCdBonus);
                                }
                                string strCdGratuity = dt.Rows[i]["CdGratuity"].ToString();
                                if (strCdGratuity.Trim().Length > 0)
                                {
                                    totalCdGratuity += Convert.ToSingle(strCdGratuity);
                                }
                                string strCdOA = dt.Rows[i]["CdOA"].ToString();
                                if (strCdOA.Trim().Length > 0)
                                {
                                    totalCdOA += Convert.ToSingle(strCdOA);
                                }
                                string strCdOTAmt = dt.Rows[i]["CdOTAmt1"].ToString();
                                if (strCdOTAmt.Trim().Length > 0)
                                {
                                    totalcdOtRate += Convert.ToSingle(strCdOTAmt);
                                }
                                string strCdServiceWeightage = dt.Rows[i]["CdServiceWeightage"].ToString();
                                if (strCdServiceWeightage.Trim().Length > 0)
                                {
                                    totalCdServiceWeightage += Convert.ToSingle(strCdServiceWeightage);
                                }
                                string strCdArrears = dt.Rows[i]["CdArrears"].ToString();
                                if (strCdArrears.Trim().Length > 0)
                                {
                                    totalCdArrears += Convert.ToSingle(strCdArrears);
                                }
                                string strCdAttBonus = dt.Rows[i]["CdAttBonus"].ToString();
                                if (strCdAttBonus.Trim().Length > 0)
                                {
                                    totalCdAttBonus += Convert.ToSingle(strCdAttBonus);
                                }
                                string strCdNightAllw = dt.Rows[i]["CdNightAllw"].ToString();
                                if (strCdNightAllw.Trim().Length > 0)
                                {
                                    totalCdNightAllw += Convert.ToSingle(strCdNightAllw);
                                }
                                string strCdEmpty1 = dt.Rows[i]["CdEmpty1"].ToString();
                                if (strCdEmpty1.Trim().Length > 0)
                                {
                                    totalCdEmpty1 += Convert.ToSingle(strCdEmpty1);
                                }
                                string strCdEmpty2 = dt.Rows[i]["CdEmpty2"].ToString();
                                if (strCdEmpty2.Trim().Length > 0)
                                {
                                    totalCdEmpty2 += Convert.ToSingle(strCdEmpty2);
                                }


                                string duties = dt.Rows[i]["NoOfDuties"].ToString();
                                if (duties.Trim().Length > 0)
                                {
                                    totalDuties += Convert.ToSingle(duties);
                                }
                                string ots = dt.Rows[i]["OTs"].ToString();
                                if (ots.Trim().Length > 0)
                                {
                                    totalOts += Convert.ToSingle(ots);
                                }

                                string wos = dt.Rows[i]["wo"].ToString();
                                if (wos.Trim().Length > 0)
                                {
                                    totalwo += Convert.ToSingle(wos);
                                }
                                string nhs = dt.Rows[i]["nhs"].ToString();
                                if (nhs.Trim().Length > 0)
                                {
                                    totalnhs += Convert.ToSingle(nhs);
                                }
                                string npots = dt.Rows[i]["pldays"].ToString();
                                if (npots.Trim().Length > 0)
                                {
                                    totalnpots += Convert.ToSingle(npots);
                                }

                                string strBasic = dt.Rows[i]["Basic"].ToString();
                                if (strBasic.Trim().Length > 0)
                                {
                                    totalBasic += Convert.ToSingle(strBasic);
                                    // countearnings += 1;
                                }
                                string strDA = dt.Rows[i]["DA"].ToString();
                                if (strDA.Trim().Length > 0)
                                {
                                    totalDA += Convert.ToSingle(strDA);

                                }
                                string strhHRA = dt.Rows[i]["HRA"].ToString();
                                if (strhHRA.Trim().Length > 0)
                                {
                                    totalHRA += Convert.ToSingle(strhHRA);

                                }
                                string strCCA = dt.Rows[i]["CCA"].ToString();
                                if (strCCA.Trim().Length > 0)
                                {
                                    totalCCA += Convert.ToSingle(strCCA);

                                }
                                string strConveyance = dt.Rows[i]["Conveyance"].ToString();
                                if (strConveyance.Trim().Length > 0)
                                {
                                    totalConveyance += Convert.ToSingle(strConveyance);

                                }
                                string strWA = dt.Rows[i]["WashAllowance"].ToString();
                                if (strWA.Trim().Length > 0)
                                {
                                    totalWA += Convert.ToSingle(strWA);

                                }
                                string strNfhs = dt.Rows[i]["Nfhs"].ToString();
                                if (strNfhs.Trim().Length > 0)
                                {
                                    totalnfhs += Convert.ToSingle(strNfhs);

                                }
                                string strRC = dt.Rows[i]["RC"].ToString();
                                if (strRC.Trim().Length > 0)
                                {
                                    totalRC += Convert.ToSingle(strRC);

                                }

                                string strCS = dt.Rows[i]["CS"].ToString();
                                if (strCS.Trim().Length > 0)
                                {
                                    totalCS += Convert.ToSingle(strCS);
                                    //countearnings += 1;
                                }
                                string strAddlAmount = dt.Rows[i]["AddlAmount"].ToString();
                                if (strAddlAmount.Trim().Length > 0)
                                {
                                    totalAddlAmount += Convert.ToSingle(strAddlAmount);

                                }
                                string strFoodAllowance = dt.Rows[i]["FoodAllowance"].ToString();
                                if (strFoodAllowance.Trim().Length > 0)
                                {
                                    totalFoodAllowance += Convert.ToSingle(strFoodAllowance);

                                }
                                string strWoAmt = dt.Rows[i]["WOAmt"].ToString();
                                if (strWoAmt.Trim().Length > 0)
                                {
                                    totalWoAmt += Convert.ToSingle(strWoAmt);

                                }

                                string strNhsAmt = dt.Rows[i]["Nhsamt"].ToString();
                                if (strNhsAmt.Trim().Length > 0)
                                {
                                    totalNhsAmt += Convert.ToSingle(strNhsAmt);

                                }
                                string strmedicalallowance = dt.Rows[i]["medicalallowance"].ToString();
                                if (strmedicalallowance.Trim().Length > 0)
                                {
                                    totalmedicalallowance += Convert.ToSingle(strmedicalallowance);

                                }
                                string strSpecialAllowance = dt.Rows[i]["SpecialAllowance"].ToString();
                                if (strSpecialAllowance.Trim().Length > 0)
                                {
                                    totalSpecialAllowance += Convert.ToSingle(strSpecialAllowance);

                                }
                                string strTravelAllw = dt.Rows[i]["TravelAllw"].ToString();
                                if (strTravelAllw.Trim().Length > 0)
                                {
                                    totalTravelAllw += Convert.ToSingle(strTravelAllw);

                                }
                                string strMobileAllowance = dt.Rows[i]["MobileAllw"].ToString();
                                if (strMobileAllowance.Trim().Length > 0)
                                {
                                    totalMobileAllowance += Convert.ToSingle(strMobileAllowance);

                                }

                                string strPerformanceAllw = dt.Rows[i]["PerformanceAllw"].ToString();
                                if (strPerformanceAllw.Trim().Length > 0)
                                {
                                    totalPerformanceAllw += Convert.ToSingle(strPerformanceAllw);

                                }

                                string strLeaveEncashAmt = dt.Rows[i]["LeaveEncashAmt"].ToString();
                                if (strCCA.Trim().Length > 0)
                                {
                                    totalLeaveEncashAmt += Convert.ToSingle(strLeaveEncashAmt);

                                }
                                string strNpotsAmt = dt.Rows[i]["Npotsamt"].ToString();
                                if (strNpotsAmt.Trim().Length > 0)
                                {
                                    totalNpotsAmt += Convert.ToSingle(strNpotsAmt);

                                }
                                string strIncentivs = dt.Rows[i]["Incentivs"].ToString();
                                if (strIncentivs.Trim().Length > 0)
                                {
                                    totalIncentivs += Convert.ToSingle(strIncentivs);

                                }
                                string strBonus = dt.Rows[i]["Bonus"].ToString();
                                if (strBonus.Trim().Length > 0)
                                {
                                    totalBonus += Convert.ToSingle(strBonus);

                                }
                                string strGratuity = dt.Rows[i]["Gratuity"].ToString();
                                if (strGratuity.Trim().Length > 0)
                                {
                                    totalGratuity += Convert.ToSingle(strGratuity);

                                }

                                string strOA = dt.Rows[i]["OtherAllowance"].ToString();
                                if (strOA.Trim().Length > 0)
                                {
                                    totalOA += Convert.ToSingle(strOA);

                                }
                                string strOTAmount = dt.Rows[i]["OTAmt"].ToString();
                                if (strOTAmount.Trim().Length > 0)
                                {
                                    totalOTAmount += Convert.ToSingle(strOTAmount);
                                }

                                string strTransport6Per = dt.Rows[i]["ServiceWeightage"].ToString();
                                if (strTransport6Per.Trim().Length > 0)
                                {
                                    totalTransport6Per += Convert.ToSingle(strTransport6Per);

                                }
                                string strArrears = dt.Rows[i]["Arrears"].ToString();
                                if (strArrears.Trim().Length > 0)
                                {
                                    totalArrears += Convert.ToSingle(strArrears);

                                }
                                string strAttBonus = dt.Rows[i]["AttBonus"].ToString();
                                if (strAttBonus.Trim().Length > 0)
                                {
                                    totalAttBonus += Convert.ToSingle(strAttBonus);
                                    // countearnings += 1;
                                }
                                string strNightShiftAllw = dt.Rows[i]["NightAllw"].ToString();
                                if (strNightShiftAllw.Trim().Length > 0)
                                {
                                    totalNightShiftAllw += Convert.ToSingle(strNightShiftAllw);

                                }

                                string strEmpty = dt.Rows[i]["Empty"].ToString();
                                if (strEmpty.Trim().Length > 0)
                                {
                                    totalEmpty1 += Convert.ToSingle(strEmpty);
                                }
                                string strEmpty1 = dt.Rows[i]["Empty"].ToString();
                                if (strEmpty1.Trim().Length > 0)
                                {
                                    totalEmpty2 += Convert.ToSingle(strEmpty1);
                                }
                                string strEmpty2 = dt.Rows[i]["Empty"].ToString();
                                if (strEmpty2.Trim().Length > 0)
                                {
                                    totalEmpty3 += Convert.ToSingle(strEmpty2);
                                }
                                string strGross = dt.Rows[i]["Gross"].ToString();
                                if (strGross.Trim().Length > 0)
                                {
                                    totalGrass += Convert.ToSingle(strGross);

                                }
                                string strPF = dt.Rows[i]["PF"].ToString();
                                if (strPF.Trim().Length > 0)
                                {
                                    totalPF += Convert.ToSingle(strPF);

                                }
                                string strESI = dt.Rows[i]["ESI"].ToString();
                                if (strESI.Trim().Length > 0)
                                {
                                    totalESI += Convert.ToSingle(strESI);

                                }
                                string strProfTax = dt.Rows[i]["ProfTax"].ToString();
                                if (strProfTax.Trim().Length > 0)
                                {
                                    totalProfTax += Convert.ToSingle(strProfTax);

                                }

                                string strSalAdv = dt.Rows[i]["SalAdvDed"].ToString();
                                if (strSalAdv.Trim().Length > 0)
                                {
                                    totalSalAdv += Convert.ToSingle(strSalAdv);

                                }

                                string strAdvDed = dt.Rows[i]["ADVDed"].ToString();
                                if (strAdvDed.Trim().Length > 0)
                                {
                                    totalAdvDed += Convert.ToSingle(strAdvDed);

                                }

                                string strWCDed = dt.Rows[i]["WCDed"].ToString();
                                if (strWCDed.Trim().Length > 0)
                                {
                                    totalWCDed += Convert.ToSingle(strWCDed);

                                }

                                string strUniformDed = dt.Rows[i]["UniformDed"].ToString();
                                if (strUniformDed.Trim().Length > 0)
                                {
                                    totalUniformDed += Convert.ToSingle(strUniformDed);

                                }
                                string strOtherDed = dt.Rows[i]["OtherDed"].ToString();
                                if (strOtherDed.Trim().Length > 0)
                                {
                                    totalOtherDed += Convert.ToSingle(strOtherDed);

                                }

                                string strLoanDed = dt.Rows[i]["LoanDed"].ToString();
                                if (strLoanDed.Trim().Length > 0)
                                {
                                    totalloanded += Convert.ToSingle(strLoanDed);
                                }
                                string strCanteenAdv = dt.Rows[i]["CanteenAdv"].ToString();
                                if (strCanteenAdv.Trim().Length > 0)
                                {
                                    totalCanteenAdv += Convert.ToSingle(strCanteenAdv);

                                }
                                string strSecDep = dt.Rows[i]["SecurityDepDed"].ToString();
                                if (strSecDep.Trim().Length > 0)
                                {
                                    totalSecDepDed += Convert.ToSingle(strSecDep);
                                }


                                string strGeneralDed = dt.Rows[i]["GeneralDed"].ToString();
                                if (strGeneralDed.Trim().Length > 0)
                                {
                                    totalGenDed += Convert.ToSingle(strGeneralDed);
                                }
                                string strOWF = dt.Rows[i]["OWF"].ToString();
                                if (strOWF.Trim().Length > 0)
                                {
                                    totalOWF += Convert.ToSingle(strOWF);
                                }
                                string strPenalty = dt.Rows[i]["Penalty"].ToString();
                                if (strPenalty.Trim().Length > 0)
                                {
                                    totalPenalty += Convert.ToSingle(strPenalty);
                                }
                                string strRentDed = dt.Rows[i]["atmDed"].ToString();
                                if (strRentDed.Trim().Length > 0)
                                {
                                    totalRentDed += Convert.ToSingle(strRentDed);

                                }
                                string strMedicalDed = dt.Rows[i]["MedicalDed"].ToString();
                                if (strMedicalDed.Trim().Length > 0)
                                {
                                    totalMedicalDed += Convert.ToSingle(strMedicalDed);

                                }
                                string strMLWFDed = dt.Rows[i]["MLWFDed"].ToString();
                                if (strMLWFDed.Trim().Length > 0)
                                {
                                    totalMLWFDed += Convert.ToSingle(strMLWFDed);

                                }
                                string strFoodDed = dt.Rows[i]["FoodDed"].ToString();
                                if (strFoodDed.Trim().Length > 0)
                                {
                                    totalFoodDed += Convert.ToSingle(strFoodDed);

                                }

                                string strElectricityDed = dt.Rows[i]["IDCardDed"].ToString();
                                if (strElectricityDed.Trim().Length > 0)
                                {
                                    totalElectricityDed += Convert.ToSingle(strElectricityDed);

                                }


                                string strTransportDed = dt.Rows[i]["RentDed1"].ToString();
                                if (strTransportDed.Trim().Length > 0)
                                {
                                    totalTransportDed += Convert.ToSingle(strTransportDed);

                                }
                                string strDccDed = dt.Rows[i]["Finesded1"].ToString();
                                if (strDccDed.Trim().Length > 0)
                                {
                                    totalDccDed += Convert.ToSingle(strDccDed);

                                }
                                string strLeaveDed = dt.Rows[i]["PVCDed"].ToString();
                                if (strLeaveDed.Trim().Length > 0)
                                {
                                    totalLeaveDed += Convert.ToSingle(strLeaveDed);

                                }
                                string strLicenseDed = dt.Rows[i]["LicenseDed"].ToString();
                                if (strLicenseDed.Trim().Length > 0)
                                {
                                    totalLicenseDed += Convert.ToSingle(strLicenseDed);

                                }

                                //
                                string strAdv4Ded = dt.Rows[i]["Adv4Ded"].ToString();
                                if (strAdv4Ded.Trim().Length > 0)
                                {
                                    totalAdv4Ded += Convert.ToSingle(strAdv4Ded);

                                }
                                string strNightRoundDed = dt.Rows[i]["Extra"].ToString();
                                if (strNightRoundDed.Trim().Length > 0)
                                {
                                    totalNightRoundDed += Convert.ToSingle(strNightRoundDed);

                                }

                                string strManpowerMobDed = dt.Rows[i]["ManpowerMobDed"].ToString();
                                if (strManpowerMobDed.Trim().Length > 0)
                                {
                                    totalManpowerMobDed += Convert.ToSingle(strManpowerMobDed);

                                }

                                string strMobileusageDed = dt.Rows[i]["MobileusageDed"].ToString();
                                if (strMobileusageDed.Trim().Length > 0)
                                {
                                    totalMobileusageDed += Convert.ToSingle(strMobileusageDed);

                                }

                                string strMediClaimDed = dt.Rows[i]["MediClaimDed"].ToString();
                                if (strMediClaimDed.Trim().Length > 0)
                                {
                                    totalMediClaimDed += Convert.ToSingle(strMediClaimDed);

                                }

                                string strCrisisDed = dt.Rows[i]["CrisisDed"].ToString();
                                if (strCrisisDed.Trim().Length > 0)
                                {
                                    totalCrisisDed += Convert.ToSingle(strCrisisDed);

                                }
                                string strTelephoneBillDed = dt.Rows[i]["TelephoneBillDed"].ToString();
                                if (strTelephoneBillDed.Trim().Length > 0)
                                {
                                    totalTelephoneBillDed += Convert.ToSingle(strTelephoneBillDed);
                                }
                                string strDed = dt.Rows[i]["TotalDeductions"].ToString();
                                if (strDed.Trim().Length > 0)
                                {
                                    totalDed += Convert.ToSingle(strDed);
                                }
                                string actualAmount = dt.Rows[i]["ActualAmount"].ToString();
                                if (actualAmount.Trim().Length > 0)
                                {
                                    totalActualamount += Convert.ToSingle(actualAmount);
                                }

                                string strfixedADDL4HR = dt.Rows[i]["fixedADDL4HR"].ToString();
                                if (strfixedADDL4HR.Trim().Length > 0)
                                {
                                    totalfixedADDL4HR += Convert.ToSingle(strfixedADDL4HR);

                                }

                                string strfixedQTRALLOW = dt.Rows[i]["fixedQTRALLOW"].ToString();
                                if (strfixedQTRALLOW.Trim().Length > 0)
                                {
                                    totalfixedQTRALLOW += Convert.ToSingle(strfixedQTRALLOW);

                                }

                                string strfixedRELALLOW = dt.Rows[i]["fixedRELALLOW"].ToString();
                                if (strfixedRELALLOW.Trim().Length > 0)
                                {
                                    totalfixedRELALLOW += Convert.ToSingle(strfixedRELALLOW);

                                }

                                string strfixedSITEALLOW = dt.Rows[i]["fixedSITEALLOW"].ToString();
                                if (strfixedSITEALLOW.Trim().Length > 0)
                                {
                                    totalfixedSITEALLOW += Convert.ToSingle(strfixedSITEALLOW);

                                }

                                string strfixedGunAllw = dt.Rows[i]["fixedGunAllw"].ToString();
                                if (strfixedGunAllw.Trim().Length > 0)
                                {
                                    totalfixedGunAllw += Convert.ToSingle(strfixedGunAllw);

                                }

                                string strfixedFireAllw = dt.Rows[i]["fixedFireAllw"].ToString();
                                if (strfixedFireAllw.Trim().Length > 0)
                                {
                                    totalfixedFireAllw += Convert.ToSingle(strfixedFireAllw);

                                }

                                string strfixedTelephoneAllw = dt.Rows[i]["fixedTelephoneAllw"].ToString();
                                if (strfixedTelephoneAllw.Trim().Length > 0)
                                {
                                    totalfixedTelephoneAllw += Convert.ToSingle(strfixedTelephoneAllw);

                                }

                                string strfixedReimbursement = dt.Rows[i]["fixedReimbursement"].ToString();
                                if (strfixedReimbursement.Trim().Length > 0)
                                {
                                    totalfixedReimbursement += Convert.ToSingle(strfixedReimbursement);

                                }

                                string strfixedHardshipAllw = dt.Rows[i]["fixedHardshipAllw"].ToString();
                                if (strfixedHardshipAllw.Trim().Length > 0)
                                {
                                    totalfixedHardshipAllw += Convert.ToSingle(strfixedHardshipAllw);

                                }

                                string strfixedPaidHolidayAllw = dt.Rows[i]["fixedPaidHolidayAllw"].ToString();
                                if (strfixedPaidHolidayAllw.Trim().Length > 0)
                                {
                                    totalfixedPaidHolidayAllw += Convert.ToSingle(strfixedPaidHolidayAllw);

                                }

                                string strADDL4HR = dt.Rows[i]["ADDL4HR"].ToString();
                                if (strADDL4HR.Trim().Length > 0)
                                {
                                    totalADDL4HR += Convert.ToSingle(strADDL4HR);

                                }

                                string strQTRALLOW = dt.Rows[i]["QTRALLOW"].ToString();
                                if (strQTRALLOW.Trim().Length > 0)
                                {
                                    totalQTRALLOW += Convert.ToSingle(strQTRALLOW);

                                }

                                string strRELALLOW = dt.Rows[i]["RELALLOW"].ToString();
                                if (strRELALLOW.Trim().Length > 0)
                                {
                                    totalRELALLOW += Convert.ToSingle(strRELALLOW);

                                }

                                string strSITEALLOW = dt.Rows[i]["SITEALLOW"].ToString();
                                if (strSITEALLOW.Trim().Length > 0)
                                {
                                    totalSITEALLOW += Convert.ToSingle(strSITEALLOW);

                                }

                                string strGunAllw = dt.Rows[i]["GunAllw"].ToString();
                                if (strGunAllw.Trim().Length > 0)
                                {
                                    totalGunAllw += Convert.ToSingle(strGunAllw);

                                }

                                string strFireAllw = dt.Rows[i]["FireAllw"].ToString();
                                if (strFireAllw.Trim().Length > 0)
                                {
                                    totalFireAllw += Convert.ToSingle(strFireAllw);

                                }

                                string strTelephoneAllw = dt.Rows[i]["TelephoneAllw"].ToString();
                                if (strTelephoneAllw.Trim().Length > 0)
                                {
                                    totalTelephoneAllw += Convert.ToSingle(strTelephoneAllw);

                                }

                                string strReimbursement = dt.Rows[i]["Reimbursement"].ToString();
                                if (strReimbursement.Trim().Length > 0)
                                {
                                    totalReimbursement += Convert.ToSingle(strReimbursement);

                                }

                                string strHardshipAllw = dt.Rows[i]["HardshipAllw"].ToString();
                                if (strHardshipAllw.Trim().Length > 0)
                                {
                                    totalHardshipAllw += Convert.ToSingle(strHardshipAllw);

                                }

                                string strPaidHolidayAllw = dt.Rows[i]["PaidHolidayAllw"].ToString();
                                if (strPaidHolidayAllw.Trim().Length > 0)
                                {
                                    totalPaidHolidayAllw += Convert.ToSingle(strPaidHolidayAllw);

                                }

                                //string strUniformAllw = dt.Rows[i]["UniformAllw"].ToString();
                                //if (strUniformAllw.Trim().Length > 0)
                                //{
                                //    totalUniformAllw += Convert.ToSingle(strUniformAllw);

                                //}
                                //string strNPCl25Per = dt.Rows[i]["OTHrs"].ToString();
                                //if (strNPCl25Per.Trim().Length > 0)
                                //{
                                //    totalNPCl25Per += Convert.ToSingle(strNPCl25Per);
                                //}

                                //string Div = dt.Rows[i]["DivisionName"].ToString();
                                //if (Div.Trim().Length > 0)
                                //{
                                //    totalDiv += Convert.ToSingle(Div);
                                //}
                                //string Area = dt.Rows[i]["Location"].ToString();
                                //if (Area.Trim().Length > 0)
                                //{
                                //    totalArea += Convert.ToSingle(Area);
                                //}
                                //string ntempgross = dt.Rows[i]["tempgross"].ToString();
                                //if (ntempgross.Trim().Length > 0)
                                //{
                                //    totaltempgross += Convert.ToSingle(ntempgross);
                                //}
                                //string strcdOtAmt = dt.Rows[i]["cdOTAmt"].ToString();
                                //if (strcdOtAmt.Trim().Length > 0)
                                //{
                                //    totalcdOtAmt += Convert.ToSingle(strcdOtAmt);

                                //}
                                //string strTransport = dt.Rows[i]["OTHrsAmt"].ToString();
                                //if (strTransport.Trim().Length > 0)
                                //{
                                //    totalTransport += Convert.ToSingle(strTransport);
                                //}
                                //string strMobInstDed = dt.Rows[i]["MobInstDed"].ToString();
                                //if (strMobInstDed.Trim().Length > 0)
                                //{
                                //    totalMobInstDed += Convert.ToSingle(strMobInstDed);

                                //}

                                //string strTDSDed = dt.Rows[i]["TDSDed"].ToString();
                                //if (strTDSDed.Trim().Length > 0)
                                //{
                                //    totalTDSDed += Convert.ToSingle(strTDSDed);

                                //}
                                //string strNightAllw = dt.Rows[i]["NightAllw"].ToString();
                                //if (strNightAllw.Trim().Length > 0)
                                //{
                                //    totalNightAllw += Convert.ToSingle(strNightAllw);

                                //}
                                //string strtotalwed = dt.Rows[i]["WCDed"].ToString();
                                //if (strtotalwed.Trim().Length > 0)
                                //{
                                //    totalwed += Convert.ToSingle(strtotalwed);

                                //}

                                ////
                                //string strpfempr = dt.Rows[i]["pfempr"].ToString();
                                //if (strpfempr.Trim().Length > 0)
                                //{
                                //    totalpfempr += Convert.ToSingle(strpfempr);
                                //}

                                //string stresiempr = dt.Rows[i]["esiempr"].ToString();
                                //if (stresiempr.Trim().Length > 0)
                                //{
                                //    totalesiempr += Convert.ToSingle(stresiempr);
                                //}

                                //string strctc = dt.Rows[i]["CTC"].ToString();
                                //if (strctc.Trim().Length > 0)
                                //{
                                //    totalctc += Convert.ToSingle(strctc);
                                //}

                            }
                            #endregion
                        }
                    }
                    #region TotalCount

                    //Fixed Wages 
                    if (totalCdBasic > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdDA > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdHRA > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdCCA > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdConveyance > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdWA > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdNfhs > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdrc > 0)
                    {
                        countfixed += 1;
                    }
                    //if (totalCdrc > 0)
                    //{
                    //    countfixed += 1;
                    //}
                    if (totalCdcs > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdAddlAmount > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdFoodAllw > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdWOAmt > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdNHsAmt > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdMedicalReimbursement > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdSpecialAllW > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdTravelAllw > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdMobileAllw > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdPerformanceAllw > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdLW > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdNPOTsAmt > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdIncentivs > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdBonus > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdGratuity > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdOA > 0)
                    {
                        countfixed += 1;
                    }

                    if (totalcdOtRate > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdServiceWeightage > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdArrears > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdAttBonus > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalCdNightAllw > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalfixedADDL4HR > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalfixedQTRALLOW > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalfixedRELALLOW > 0)
                    {
                        countfixed += 1;
                    }

                    if (totalfixedSITEALLOW > 0)
                    {
                        countfixed += 1;
                    }

                    if (totalfixedGunAllw > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalfixedFireAllw > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalfixedTelephoneAllw > 0)
                    {
                        countfixed += 1;
                    }

                    if (totalfixedReimbursement > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalfixedHardshipAllw > 0)
                    {
                        countfixed += 1;
                    }
                    if (totalfixedPaidHolidayAllw > 0)
                    {
                        countfixed += 1;
                    }


                    //Duties
                    if (totalDuties > 0)
                    {
                        countduties += 1;
                    }

                    if (totalOts > 0)
                    {
                        countduties += 1;
                    }


                    if (totalwo > 0)
                    {
                        countduties += 1;
                    }


                    if (totalnhs > 0)
                    {
                        countduties += 1;
                    }


                    if (totalnpots > 0)
                    {
                        countduties += 1;
                    }


                    //Earnings
                    if (totalBasic > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalDA > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalHRA > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalCCA > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalConveyance > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalWA > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalnfhs > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalRC > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalAddlAmount > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalFoodAllowance > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalWoAmt > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalNhsAmt > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalmedicalallowance > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalSpecialAllowance > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalTravelAllw > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalMobileAllowance > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalPerformanceAllw > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalLeaveEncashAmt > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalNpotsAmt > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalIncentivs > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalBonus > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalGratuity > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalOA > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalOTAmount > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalServiceWeightage > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalArrears > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalAttBonus > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalNightShiftAllw > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalGrass > 0)
                    {
                        countearnings += 1;
                    }
                    //if (totalNPCl25Per>0)
                    //{
                    //    countearnings += 1;
                    //}

                    if (totalTransport6Per > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalCS > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalUniformAllw > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalTransport > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalADDL4HR > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalQTRALLOW > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalRELALLOW > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalSITEALLOW > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalGunAllw > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalFireAllw > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalTelephoneAllw > 0)
                    {
                        countearnings += 1;
                    }

                    if (totalReimbursement > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalHardshipAllw > 0)
                    {
                        countearnings += 1;
                    }
                    if (totalPaidHolidayAllw > 0)
                    {
                        countearnings += 1;
                    }

                    //Deductions
                    if (totalPF > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalESI > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalProfTax > 0)
                    {
                        countdedutions += 1;
                    }


                    if (totalSalAdv > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalAdvDed > 0)
                    {
                        countdedutions += 1;
                    }


                    if (totalWCDed > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalUniformDed > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalOtherDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalloanded > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalCanteenAdv > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalSecDepDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalGenDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalPenalty > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalOWF > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalRentDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalMedicalDed > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalMLWFDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalFoodDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalElectricityDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalTransportDed > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalLeaveDed > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalLicenseDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalAdv4Ded > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalNightRoundDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalManpowerMobDed > 0)
                    {
                        countdedutions += 1;
                    }


                    if (totalMobileusageDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalMediClaimDed > 0)
                    {
                        countdedutions += 1;
                    }

                    if (totalCrisisDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalTelephoneBillDed > 0)
                    {
                        countdedutions += 1;
                    }
                    if (totalDccDed > 0)
                    {
                        countdedutions += 1;
                    }


                    //if (totalMobInstDed > 0)
                    //{
                    //    countdedutions += 1;
                    //}


                    //if (totalTDSDed > 0)
                    //{
                    //    countdedutions += 1;
                    //}
                    //if (totalpfempr > 0)
                    //{
                    //    countpfempr += 1;
                    //}

                    //if (totalesiempr > 0)
                    //{
                    //    countpfempr += 1;
                    //}


                    //if (totalctc > 0)
                    //{
                    //    countpfempr += 1;
                    //}

                    // Fixed Count





                }
                #endregion TotalCount



                if (dtbranchesi.Rows.Count > 0)
                {
                    string date = "";
                    string Year = "";
                    int EmpDetialscount = 19;
                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                        Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                    }
                    string filename = "WageSheetReport " + ".xls";
                    //int count = GVListEmployees.Columns.Count;
                    int count = countduties + countfixed + countearnings + countdedutions + countNetpay + EmpDetialscount;
                    companyname = dtbranchesi.Rows[0]["CompanyName"].ToString();
                    string Form = "FORM XVII";
                    string wages = "REGISTER OF WAGES";
                    string Rule = "[Rule 78(1)(a)(i)]";
                    string ContractorName = "Name and address of contractor " + companyname;
                    string WorkLocation = "Nature and location of work";
                    string Address = "Name and address of establishment in/under which contract is carried on";
                    string PrincipalEmployeer = "Name and address of principal employer";
                    string Wageperiod = "Wage period: Monthly";
                    line2 = "Salary Statement for Month of " + GetMonthName() + "-" + Year;
                    //GVUtill.ExportGrid("Netpay-(" + txtmonth.Text + ")" + ".xls", hidGridView);
                    gve.ExportGridForWagesheetReport(filename, countduties, countfixed, countearnings, countdedutions, countpfempr, 0, countNetpay, EmpDetialscount, Form, wages, Rule, ContractorName, WorkLocation, count, hidGridView);

                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void GVListEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVListEmployees.PageIndex = e.NewPageIndex;
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            if (txtmonth.Text.Trim().Length > 0)
            {
                DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                monthname = mfi.GetMonthName(date.Month).ToString();
            }
            //payMonth = GetMonth(monthname);

            return monthname;

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

            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            month = Timings.Instance.GetIdForEnteredMOnth(date);
            return month;


            #endregion
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

        float totalActualamount = 0;
        float totalctcamount = 0;
        float totalDuties = 0;
        float totalOts = 0;
        float totalwo = 0;
        float totalnhs = 0;
        float totalnpots = 0;
        float totaltempgross = 0;
        float totalBasic = 0;
        float totalDA = 0;
        float totalHRA = 0;
        float totalCCA = 0;
        float totalConveyance = 0;
        float totalWA = 0;
        float totalOA = 0;
        float totalGrass = 0;
        float totalOTAmount = 0;
        float totalServiceWeightage = 0;
        float totalArrears = 0;
        float totalPF = 0;
        float totalESI = 0;
        float totalProfTax = 0;
        float totalSalAdv = 0;
        float totalUniformDed = 0;
        float totalAdvDed = 0;
        float totalwed = 0;
        float totalWCDed = 0;
        float totalCanteenAdv = 0;
        float totalSeepDed = 0;
        float totalLeaveEncashAmt = 0;
        float totalGratuity = 0;
        float totalBonus = 0;
        float totalnfhs = 0;
        float totalDed = 0;
        float totalOtherDed = 0;
        float totalIncentivs = 0;
        float totalWoAmt = 0;
        float totalNhsAmt = 0;
        float totalNpotsAmt = 0;
        float totalPenalty = 0;
        float totalRC = 0;
        float totalCS = 0;
        float totalOWF = 0;
        float totalSecDepDed = 0;
        float totalloanded = 0;
        float totalGenDed = 0;
        float totalctc = 0;

        float totalAttBonus = 0;
        float totalNightAllw = 0;
        float totalEmpty1 = 0;
        float totalEmpty2 = 0;
        float totalEmpty3 = 0;
        float totalTravelAllw = 0;
        float totalNightShiftAllw = 0;
        float totalFoodAllowance = 0;
        float totalmedicalallowance = 0;
        float totalUniformAllw = 0;

        float totalAdv4Ded = 0;
        float totalNightRoundDed = 0;
        float totalManpowerMobDed = 0;
        float totalMobileusageDed = 0;
        float totalMediClaimDed = 0;
        float totalCrisisDed = 0;
        float totalMobInstDed = 0;
        float totalTDSDed = 0;


        float totalSpecialAllowance = 0;
        float totalMobileAllowance = 0;
        float totalPerformanceAllw = 0;
        float totalNPCl25Per = 0;
        float totalTransport6Per = 0;
        float totalTransport = 0;

        float totalRentDed = 0;
        float totalMedicalDed = 0;
        float totalMLWFDed = 0;
        float totalFoodDed = 0;
        float totalAddlAmount = 0;


        float totalElectricityDed = 0;
        float totalTransportDed = 0;
        float totalDced = 0;
        float totalDccDed = 0;
        float totalLeaveDed = 0;
        float totalLicenseDed = 0;
        float totalpfempr = 0;
        float totalesiempr = 0;
        float totalDiv = 0;
        float totalArea = 0;
        float totalTelephoneBillDed = 0;
        float totalcdOtRate = 0;

        float totalCdBasic = 0;
        float totalCdDA = 0;
        float totalCdHRA = 0;
        float totalCdCCA = 0;
        float totalCdConveyance = 0;
        float totalCdWA = 0;
        float totalCdNfhs = 0;
        float totalCdrc = 0;
        float totalCdcs = 0;

        float totalCdAddlAmount = 0;
        float totalCdFoodAllw = 0;
        float totalCdWOAmt = 0;
        float totalCdNHsAmt = 0;
        float totalCdMedicalReimbursement = 0;
        float totalCdSpecialAllW = 0;
        float totalCdMobileAllw = 0;
        float totalCdTravelAllw = 0;
        float totalCdPerformanceAllw = 0;
        float totalCdLW = 0;
        float totalCdNPOTsAmt = 0;
        float totalCdOA = 0;
        float totalCdIncentivs = 0;
        float totalCdBonus = 0;
        float totalCdGratuity = 0;
        float totalCdServiceWeightage = 0;
        float totalCdArrears = 0;
        float totalCdAttBonus = 0;
        float totalCdNightAllw = 0;
        float totalCdEmpty1 = 0;
        float totalCdEmpty2 = 0;
        float totalCdEmpty3 = 0;
        float totalADDL4HR = 0;
        float totalQTRALLOW = 0;
        float totalRELALLOW = 0;
        float totalSITEALLOW = 0;
        float totalGunAllw = 0;
        float totalFireAllw = 0;
        float totalTelephoneAllw = 0;
        float totalReimbursement = 0;
        float totalHardshipAllw = 0;
        float totalPaidHolidayAllw = 0;
        float totalfixedADDL4HR = 0;
        float totalfixedQTRALLOW = 0;
        float totalfixedRELALLOW = 0;
        float totalfixedSITEALLOW = 0;
        float totalfixedGunAllw = 0;
        float totalfixedFireAllw = 0;
        float totalfixedTelephoneAllw = 0;
        float totalfixedReimbursement = 0;
        float totalfixedHardshipAllw = 0;
        float totalfixedPaidHolidayAllw = 0;


        protected void DisplayData()
        {
            // if (ddlclientid.SelectedIndex > 0)
            {
                try
                {
                    string monthnew = string.Empty;

                    string sqlqry = string.Empty;

                    //var list = new List<string>();
                    var list = new List<Tuple<int, string>>();
                    // var monthlist = new List<string>();
                    var options = 0;
                    if (GVClientsData.Rows.Count > 0)
                    {
                        for (int i = 0; i < GVClientsData.Rows.Count; i++)
                        {
                            CheckBox chkclientid = GVClientsData.Rows[i].FindControl("chkindividual") as CheckBox;
                            Label lblclientid = GVClientsData.Rows[i].FindControl("lblclientid") as Label;
                            Label lblmonth = GVClientsData.Rows[i].FindControl("lblmonth") as Label;

                            if (chkclientid.Checked == true)
                            {
                                list.Add(Tuple.Create(Convert.ToInt32(lblmonth.Text), lblclientid.Text));
                            }

                        }
                    }

                    // string Clientids = string.Join(",", list.ToArray());


                    DataTable dtClientList = new DataTable();
                    DataTable dtmonth = new DataTable();
                    dtClientList.Columns.Add("Clientid");
                    dtmonth.Columns.Add("month");
                    if (list.Count != 0)
                    {

                        foreach (Tuple<int, string> tuple in list)
                        {
                            DataRow row = dtClientList.NewRow();
                            DataRow row1 = dtmonth.NewRow();
                            row["Clientid"] = tuple.Item2;
                            row1["month"] = tuple.Item1;
                            dtClientList.Rows.Add(row["Clientid"]);
                            dtmonth.Rows.Add(row1["month"]);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select Client IDs');", true);
                    }

                    var SPName = "";
                    Hashtable HTPaysheet = new Hashtable();
                    int countearnings = 0;
                    int countdedutions = 0;
                    int countot = 0;
                    int countpfempr = 0;
                    int countesiempr = 0;
                    int countctc = 0;

                    string Fromdate = string.Empty;
                    string ToMonth = string.Empty;

                    if (txtfrom.Text.Trim().Length > 0)
                    {
                        Fromdate = DateTime.Parse(txtfrom.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }
                    if (txtto.Text.Trim().Length > 0)
                    {
                        ToMonth = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }
                    SPName = "NetPayReport";
                    options = ddloptions.SelectedIndex;
                    HTPaysheet.Add("@ClientIDList", dtClientList);
                    HTPaysheet.Add("@MonthList", dtmonth);


                    DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;

                    if (dt.Rows.Count > 0)
                    {
                        GVListEmployees.DataSource = dt;
                        GVListEmployees.DataBind();
                        lbtn_ExportNew.Visible = true;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            // if (dt.Rows[i]["Monthname"].ToString() == "Sub Total")
                            {
                                float actAmount = 0;

                                if (actAmount >= 0)
                                {
                                    #region

                                    // Total Calculation 

                                    //26-04-2019
                                    string strCdBasic = dt.Rows[i]["CdBasic"].ToString();
                                    if (strCdBasic.Trim().Length > 0)
                                    {
                                        totalCdBasic += Convert.ToSingle(strCdBasic);
                                    }
                                    string strCdDA = dt.Rows[i]["CdDA"].ToString();
                                    if (strCdDA.Trim().Length > 0)
                                    {
                                        totalCdDA += Convert.ToSingle(strCdDA);
                                    }
                                    string strCdHRA = dt.Rows[i]["CdHRA"].ToString();
                                    if (strCdHRA.Trim().Length > 0)
                                    {
                                        totalCdHRA += Convert.ToSingle(strCdHRA);
                                    }
                                    string strCdCCA = dt.Rows[i]["CdCCA"].ToString();
                                    if (strCdCCA.Trim().Length > 0)
                                    {
                                        totalCdCCA += Convert.ToSingle(strCdCCA);
                                    }
                                    string strCdConv = dt.Rows[i]["CdConv"].ToString();
                                    if (strCdConv.Trim().Length > 0)
                                    {
                                        totalCdConveyance += Convert.ToSingle(strCdConv);
                                    }

                                    string strCdWA = dt.Rows[i]["CdWA"].ToString();
                                    if (strCdWA.Trim().Length > 0)
                                    {
                                        totalCdWA += Convert.ToSingle(strCdWA);
                                    }
                                    string strCdNfhs = dt.Rows[i]["CdNfhs"].ToString();
                                    if (strCdNfhs.Trim().Length > 0)
                                    {
                                        totalCdNfhs += Convert.ToSingle(strCdNfhs);
                                    }
                                    string strCdRC = dt.Rows[i]["CdRC"].ToString();
                                    if (strCdRC.Trim().Length > 0)
                                    {
                                        totalCdrc += Convert.ToSingle(strCdRC);
                                    }
                                    string strCdCS = dt.Rows[i]["CdCS"].ToString();
                                    if (strCdCS.Trim().Length > 0)
                                    {
                                        totalCdcs += Convert.ToSingle(strCdCS);
                                    }
                                    string strCdAddlAmount = dt.Rows[i]["CdAddlAmount"].ToString();
                                    if (strCdAddlAmount.Trim().Length > 0)
                                    {
                                        totalCdAddlAmount += Convert.ToSingle(strCdAddlAmount);
                                    }
                                    string strCdFoodAllw = dt.Rows[i]["CdFoodAllw"].ToString();
                                    if (strCdFoodAllw.Trim().Length > 0)
                                    {
                                        totalCdFoodAllw += Convert.ToSingle(strCdFoodAllw);
                                    }
                                    string strCdWOAmt = dt.Rows[i]["CdWOAmt"].ToString();
                                    if (strCdWOAmt.Trim().Length > 0)
                                    {
                                        totalCdWOAmt += Convert.ToSingle(strCdWOAmt);
                                    }
                                    string strCdNHsAmt = dt.Rows[i]["CdNHsAmt"].ToString();
                                    if (strCdNHsAmt.Trim().Length > 0)
                                    {
                                        totalCdNHsAmt += Convert.ToSingle(strCdNHsAmt);
                                    }
                                    string strCdMedicalReimbursement = dt.Rows[i]["CdMedicalReimbursement"].ToString();
                                    if (strCdMedicalReimbursement.Trim().Length > 0)
                                    {
                                        totalCdMedicalReimbursement += Convert.ToSingle(strCdMedicalReimbursement);
                                    }
                                    string strCdSpecialAllW = dt.Rows[i]["CdSpecialAllW"].ToString();
                                    if (strCdSpecialAllW.Trim().Length > 0)
                                    {
                                        totalCdSpecialAllW += Convert.ToSingle(strCdSpecialAllW);
                                    }
                                    string strCdTravelAllw = dt.Rows[i]["CdTravelAllw"].ToString();
                                    if (strCdTravelAllw.Trim().Length > 0)
                                    {
                                        totalCdTravelAllw += Convert.ToSingle(strCdTravelAllw);
                                    }
                                    string strCdMobileAllw = dt.Rows[i]["CdMobileAllw"].ToString();
                                    if (strCdMobileAllw.Trim().Length > 0)
                                    {
                                        totalCdMobileAllw += Convert.ToSingle(strCdMobileAllw);
                                    }
                                    string strCdPerformanceAllw = dt.Rows[i]["CdPerformanceAllw"].ToString();
                                    if (strCdPerformanceAllw.Trim().Length > 0)
                                    {
                                        totalCdPerformanceAllw += Convert.ToSingle(strCdPerformanceAllw);
                                    }
                                    string strCdLW = dt.Rows[i]["CdLW"].ToString();
                                    if (strCdLW.Trim().Length > 0)
                                    {
                                        totalCdLW += Convert.ToSingle(strCdLW);
                                    }
                                    string strCdNPOTsAmt = dt.Rows[i]["CdNPOTsAmt"].ToString();
                                    if (strCdNPOTsAmt.Trim().Length > 0)
                                    {
                                        totalCdNPOTsAmt += Convert.ToSingle(strCdNPOTsAmt);
                                    }
                                    string strCdIncentivs = dt.Rows[i]["CdIncentivs"].ToString();
                                    if (strCdIncentivs.Trim().Length > 0)
                                    {
                                        totalCdIncentivs += Convert.ToSingle(strCdIncentivs);
                                    }
                                    string strCdBonus = dt.Rows[i]["CdBonus"].ToString();
                                    if (strCdBonus.Trim().Length > 0)
                                    {
                                        totalCdBonus += Convert.ToSingle(strCdBonus);
                                    }
                                    string strCdGratuity = dt.Rows[i]["CdGratuity"].ToString();
                                    if (strCdGratuity.Trim().Length > 0)
                                    {
                                        totalCdGratuity += Convert.ToSingle(strCdGratuity);
                                    }
                                    string strCdOA = dt.Rows[i]["CdOA"].ToString();
                                    if (strCdOA.Trim().Length > 0)
                                    {
                                        totalCdOA += Convert.ToSingle(strCdOA);
                                    }
                                    string strCdOTAmt = dt.Rows[i]["CdOtAmt1"].ToString();
                                    if (strCdOTAmt.Trim().Length > 0)
                                    {
                                        totalcdOtRate += Convert.ToSingle(strCdOTAmt);
                                    }
                                    string strCdServiceWeightage = dt.Rows[i]["CdServiceWeightage"].ToString();
                                    if (strCdServiceWeightage.Trim().Length > 0)
                                    {
                                        totalCdServiceWeightage += Convert.ToSingle(strCdServiceWeightage);
                                    }
                                    string strCdArrears = dt.Rows[i]["CdArrears"].ToString();
                                    if (strCdArrears.Trim().Length > 0)
                                    {
                                        totalCdArrears += Convert.ToSingle(strCdArrears);
                                    }
                                    string strCdAttBonus = dt.Rows[i]["CdAttBonus"].ToString();
                                    if (strCdAttBonus.Trim().Length > 0)
                                    {
                                        totalCdAttBonus += Convert.ToSingle(strCdAttBonus);
                                    }
                                    string strCdNightAllw = dt.Rows[i]["CdNightAllw"].ToString();
                                    if (strCdNightAllw.Trim().Length > 0)
                                    {
                                        totalCdNightAllw += Convert.ToSingle(strCdNightAllw);
                                    }
                                    string strCdEmpty1 = dt.Rows[i]["CdEmpty1"].ToString();
                                    if (strCdEmpty1.Trim().Length > 0)
                                    {
                                        totalCdEmpty1 += Convert.ToSingle(strCdEmpty1);
                                    }
                                    string strCdEmpty2 = dt.Rows[i]["CdEmpty2"].ToString();
                                    if (strCdEmpty2.Trim().Length > 0)
                                    {
                                        totalCdEmpty2 += Convert.ToSingle(strCdEmpty2);
                                    }


                                    string duties = dt.Rows[i]["NoOfDuties"].ToString();
                                    if (duties.Trim().Length > 0)
                                    {
                                        totalDuties += Convert.ToSingle(duties);
                                    }
                                    string ots = dt.Rows[i]["OTs"].ToString();
                                    if (ots.Trim().Length > 0)
                                    {
                                        totalOts += Convert.ToSingle(ots);
                                    }

                                    string wos = dt.Rows[i]["wo"].ToString();
                                    if (wos.Trim().Length > 0)
                                    {
                                        totalwo += Convert.ToSingle(wos);
                                    }
                                    string nhs = dt.Rows[i]["nhs"].ToString();
                                    if (nhs.Trim().Length > 0)
                                    {
                                        totalnhs += Convert.ToSingle(nhs);
                                    }
                                    string npots = dt.Rows[i]["pldays"].ToString();
                                    if (npots.Trim().Length > 0)
                                    {
                                        totalnpots += Convert.ToSingle(npots);
                                    }

                                    string strBasic = dt.Rows[i]["Basic"].ToString();
                                    if (strBasic.Trim().Length > 0)
                                    {
                                        totalBasic += Convert.ToSingle(strBasic);
                                        countearnings += 1;
                                    }
                                    string strDA = dt.Rows[i]["DA"].ToString();
                                    if (strDA.Trim().Length > 0)
                                    {
                                        totalDA += Convert.ToSingle(strDA);

                                    }
                                    string strhHRA = dt.Rows[i]["HRA"].ToString();
                                    if (strhHRA.Trim().Length > 0)
                                    {
                                        totalHRA += Convert.ToSingle(strhHRA);

                                    }
                                    string strCCA = dt.Rows[i]["CCA"].ToString();
                                    if (strCCA.Trim().Length > 0)
                                    {
                                        totalCCA += Convert.ToSingle(strCCA);

                                    }
                                    string strConveyance = dt.Rows[i]["Conveyance"].ToString();
                                    if (strConveyance.Trim().Length > 0)
                                    {
                                        totalConveyance += Convert.ToSingle(strConveyance);

                                    }
                                    string strWA = dt.Rows[i]["WashAllowance"].ToString();
                                    if (strWA.Trim().Length > 0)
                                    {
                                        totalWA += Convert.ToSingle(strWA);

                                    }
                                    string strNfhs = dt.Rows[i]["Nfhs"].ToString();
                                    if (strNfhs.Trim().Length > 0)
                                    {
                                        totalnfhs += Convert.ToSingle(strNfhs);

                                    }
                                    string strRC = dt.Rows[i]["RC"].ToString();
                                    if (strRC.Trim().Length > 0)
                                    {
                                        totalRC += Convert.ToSingle(strRC);

                                    }

                                    string strCS = dt.Rows[i]["CS"].ToString();
                                    if (strCS.Trim().Length > 0)
                                    {
                                        totalCS += Convert.ToSingle(strCS);
                                        countearnings += 1;
                                    }
                                    string strAddlAmount = dt.Rows[i]["AddlAmount"].ToString();
                                    if (strAddlAmount.Trim().Length > 0)
                                    {
                                        totalAddlAmount += Convert.ToSingle(strAddlAmount);

                                    }
                                    string strFoodAllowance = dt.Rows[i]["FoodAllowance"].ToString();
                                    if (strFoodAllowance.Trim().Length > 0)
                                    {
                                        totalFoodAllowance += Convert.ToSingle(strFoodAllowance);

                                    }
                                    string strWoAmt = dt.Rows[i]["WOAmt"].ToString();
                                    if (strWoAmt.Trim().Length > 0)
                                    {
                                        totalWoAmt += Convert.ToSingle(strWoAmt);

                                    }

                                    string strNhsAmt = dt.Rows[i]["Nhsamt"].ToString();
                                    if (strNhsAmt.Trim().Length > 0)
                                    {
                                        totalNhsAmt += Convert.ToSingle(strNhsAmt);

                                    }
                                    string strmedicalallowance = dt.Rows[i]["medicalallowance"].ToString();
                                    if (strmedicalallowance.Trim().Length > 0)
                                    {
                                        totalmedicalallowance += Convert.ToSingle(strmedicalallowance);

                                    }
                                    string strSpecialAllowance = dt.Rows[i]["SpecialAllowance"].ToString();
                                    if (strSpecialAllowance.Trim().Length > 0)
                                    {
                                        totalSpecialAllowance += Convert.ToSingle(strSpecialAllowance);

                                    }
                                    string strTravelAllw = dt.Rows[i]["TravelAllw"].ToString();
                                    if (strTravelAllw.Trim().Length > 0)
                                    {
                                        totalTravelAllw += Convert.ToSingle(strTravelAllw);

                                    }
                                    string strMobileAllowance = dt.Rows[i]["MobileAllw"].ToString();
                                    if (strMobileAllowance.Trim().Length > 0)
                                    {
                                        totalMobileAllowance += Convert.ToSingle(strMobileAllowance);

                                    }

                                    string strPerformanceAllw = dt.Rows[i]["PerformanceAllw"].ToString();
                                    if (strPerformanceAllw.Trim().Length > 0)
                                    {
                                        totalPerformanceAllw += Convert.ToSingle(strPerformanceAllw);

                                    }

                                    string strLeaveEncashAmt = dt.Rows[i]["LeaveEncashAmt"].ToString();
                                    if (strCCA.Trim().Length > 0)
                                    {
                                        totalLeaveEncashAmt += Convert.ToSingle(strLeaveEncashAmt);

                                    }
                                    string strNpotsAmt = dt.Rows[i]["Npotsamt"].ToString();
                                    if (strNpotsAmt.Trim().Length > 0)
                                    {
                                        totalNpotsAmt += Convert.ToSingle(strNpotsAmt);

                                    }
                                    string strIncentivs = dt.Rows[i]["Incentivs"].ToString();
                                    if (strIncentivs.Trim().Length > 0)
                                    {
                                        totalIncentivs += Convert.ToSingle(strIncentivs);

                                    }
                                    string strBonus = dt.Rows[i]["Bonus"].ToString();
                                    if (strBonus.Trim().Length > 0)
                                    {
                                        totalBonus += Convert.ToSingle(strBonus);

                                    }
                                    string strGratuity = dt.Rows[i]["Gratuity"].ToString();
                                    if (strGratuity.Trim().Length > 0)
                                    {
                                        totalGratuity += Convert.ToSingle(strGratuity);

                                    }

                                    string strOA = dt.Rows[i]["OtherAllowance"].ToString();
                                    if (strOA.Trim().Length > 0)
                                    {
                                        totalOA += Convert.ToSingle(strOA);

                                    }
                                    string strOTAmount = dt.Rows[i]["OTAmt"].ToString();
                                    if (strOTAmount.Trim().Length > 0)
                                    {
                                        totalOTAmount += Convert.ToSingle(strOTAmount);
                                    }

                                    string strTransport6Per = dt.Rows[i]["ServiceWeightage"].ToString();
                                    if (strTransport6Per.Trim().Length > 0)
                                    {
                                        totalTransport6Per += Convert.ToSingle(strTransport6Per);

                                    }
                                    string strArrears = dt.Rows[i]["Arrears"].ToString();
                                    if (strArrears.Trim().Length > 0)
                                    {
                                        totalArrears += Convert.ToSingle(strArrears);

                                    }
                                    string strAttBonus = dt.Rows[i]["AttBonus"].ToString();
                                    if (strAttBonus.Trim().Length > 0)
                                    {
                                        totalAttBonus += Convert.ToSingle(strAttBonus);
                                        countearnings += 1;
                                    }
                                    string strNightShiftAllw = dt.Rows[i]["NightAllw"].ToString();
                                    if (strNightShiftAllw.Trim().Length > 0)
                                    {
                                        totalNightShiftAllw += Convert.ToSingle(strNightShiftAllw);

                                    }

                                    string strEmpty = dt.Rows[i]["Empty"].ToString();
                                    if (strEmpty.Trim().Length > 0)
                                    {
                                        totalEmpty1 += Convert.ToSingle(strEmpty);
                                    }
                                    string strEmpty1 = dt.Rows[i]["Empty"].ToString();
                                    if (strEmpty1.Trim().Length > 0)
                                    {
                                        totalEmpty2 += Convert.ToSingle(strEmpty1);
                                    }
                                    string strEmpty2 = dt.Rows[i]["Empty"].ToString();
                                    if (strEmpty2.Trim().Length > 0)
                                    {
                                        totalEmpty3 += Convert.ToSingle(strEmpty2);
                                    }
                                    string strGross = dt.Rows[i]["Gross"].ToString();
                                    if (strGross.Trim().Length > 0)
                                    {
                                        totalGrass += Convert.ToSingle(strGross);

                                    }
                                    string strPF = dt.Rows[i]["PF"].ToString();
                                    if (strPF.Trim().Length > 0)
                                    {
                                        totalPF += Convert.ToSingle(strPF);

                                    }
                                    string strESI = dt.Rows[i]["ESI"].ToString();
                                    if (strESI.Trim().Length > 0)
                                    {
                                        totalESI += Convert.ToSingle(strESI);

                                    }
                                    string strProfTax = dt.Rows[i]["ProfTax"].ToString();
                                    if (strProfTax.Trim().Length > 0)
                                    {
                                        totalProfTax += Convert.ToSingle(strProfTax);

                                    }

                                    string strSalAdv = dt.Rows[i]["SalAdvDed"].ToString();
                                    if (strSalAdv.Trim().Length > 0)
                                    {
                                        totalSalAdv += Convert.ToSingle(strSalAdv);

                                    }

                                    string strAdvDed = dt.Rows[i]["ADVDed"].ToString();
                                    if (strAdvDed.Trim().Length > 0)
                                    {
                                        totalAdvDed += Convert.ToSingle(strAdvDed);

                                    }

                                    string strWCDed = dt.Rows[i]["WCDed"].ToString();
                                    if (strWCDed.Trim().Length > 0)
                                    {
                                        totalWCDed += Convert.ToSingle(strWCDed);

                                    }

                                    string strUniformDed = dt.Rows[i]["UniformDed"].ToString();
                                    if (strUniformDed.Trim().Length > 0)
                                    {
                                        totalUniformDed += Convert.ToSingle(strUniformDed);

                                    }
                                    string strOtherDed = dt.Rows[i]["OtherDed"].ToString();
                                    if (strOtherDed.Trim().Length > 0)
                                    {
                                        totalOtherDed += Convert.ToSingle(strOtherDed);

                                    }

                                    string strLoanDed = dt.Rows[i]["LoanDed"].ToString();
                                    if (strLoanDed.Trim().Length > 0)
                                    {
                                        totalloanded += Convert.ToSingle(strLoanDed);
                                    }
                                    string strCanteenAdv = dt.Rows[i]["CanteenAdv"].ToString();
                                    if (strCanteenAdv.Trim().Length > 0)
                                    {
                                        totalCanteenAdv += Convert.ToSingle(strCanteenAdv);

                                    }
                                    string strSecDep = dt.Rows[i]["SecurityDepDed"].ToString();
                                    if (strSecDep.Trim().Length > 0)
                                    {
                                        totalSecDepDed += Convert.ToSingle(strSecDep);
                                    }


                                    string strGeneralDed = dt.Rows[i]["GeneralDed"].ToString();
                                    if (strGeneralDed.Trim().Length > 0)
                                    {
                                        totalGenDed += Convert.ToSingle(strGeneralDed);
                                    }
                                    string strOWF = dt.Rows[i]["OWF"].ToString();
                                    if (strOWF.Trim().Length > 0)
                                    {
                                        totalOWF += Convert.ToSingle(strOWF);
                                    }
                                    string strPenalty = dt.Rows[i]["Penalty"].ToString();
                                    if (strPenalty.Trim().Length > 0)
                                    {
                                        totalPenalty += Convert.ToSingle(strPenalty);
                                    }
                                    string strRentDed = dt.Rows[i]["atmDed"].ToString();
                                    if (strRentDed.Trim().Length > 0)
                                    {
                                        totalRentDed += Convert.ToSingle(strRentDed);

                                    }
                                    string strMedicalDed = dt.Rows[i]["MedicalDed"].ToString();
                                    if (strMedicalDed.Trim().Length > 0)
                                    {
                                        totalMedicalDed += Convert.ToSingle(strMedicalDed);

                                    }
                                    string strMLWFDed = dt.Rows[i]["MLWFDed"].ToString();
                                    if (strMLWFDed.Trim().Length > 0)
                                    {
                                        totalMLWFDed += Convert.ToSingle(strMLWFDed);

                                    }
                                    string strFoodDed = dt.Rows[i]["FoodDed"].ToString();
                                    if (strFoodDed.Trim().Length > 0)
                                    {
                                        totalFoodDed += Convert.ToSingle(strFoodDed);

                                    }

                                    string strElectricityDed = dt.Rows[i]["IDCardDed"].ToString();
                                    if (strElectricityDed.Trim().Length > 0)
                                    {
                                        totalElectricityDed += Convert.ToSingle(strElectricityDed);

                                    }


                                    string strTransportDed = dt.Rows[i]["RentDed1"].ToString();
                                    if (strTransportDed.Trim().Length > 0)
                                    {
                                        totalTransportDed += Convert.ToSingle(strTransportDed);

                                    }
                                    string strDccDed = dt.Rows[i]["Finesded1"].ToString();
                                    if (strDccDed.Trim().Length > 0)
                                    {
                                        totalDccDed += Convert.ToSingle(strDccDed);

                                    }
                                    string strLeaveDed = dt.Rows[i]["PVCDed"].ToString();
                                    if (strLeaveDed.Trim().Length > 0)
                                    {
                                        totalLeaveDed += Convert.ToSingle(strLeaveDed);

                                    }
                                    string strLicenseDed = dt.Rows[i]["LicenseDed"].ToString();
                                    if (strLicenseDed.Trim().Length > 0)
                                    {
                                        totalLicenseDed += Convert.ToSingle(strLicenseDed);

                                    }

                                    //
                                    string strAdv4Ded = dt.Rows[i]["Adv4Ded"].ToString();
                                    if (strAdv4Ded.Trim().Length > 0)
                                    {
                                        totalAdv4Ded += Convert.ToSingle(strAdv4Ded);

                                    }
                                    string strNightRoundDed = dt.Rows[i]["Extra"].ToString();
                                    if (strNightRoundDed.Trim().Length > 0)
                                    {
                                        totalNightRoundDed += Convert.ToSingle(strNightRoundDed);

                                    }

                                    string strManpowerMobDed = dt.Rows[i]["ManpowerMobDed"].ToString();
                                    if (strManpowerMobDed.Trim().Length > 0)
                                    {
                                        totalManpowerMobDed += Convert.ToSingle(strManpowerMobDed);

                                    }

                                    string strMobileusageDed = dt.Rows[i]["MobileusageDed"].ToString();
                                    if (strMobileusageDed.Trim().Length > 0)
                                    {
                                        totalMobileusageDed += Convert.ToSingle(strMobileusageDed);

                                    }

                                    string strMediClaimDed = dt.Rows[i]["MediClaimDed"].ToString();
                                    if (strMediClaimDed.Trim().Length > 0)
                                    {
                                        totalMediClaimDed += Convert.ToSingle(strMediClaimDed);

                                    }

                                    string strCrisisDed = dt.Rows[i]["CrisisDed"].ToString();
                                    if (strCrisisDed.Trim().Length > 0)
                                    {
                                        totalCrisisDed += Convert.ToSingle(strCrisisDed);

                                    }
                                    string strTelephoneBillDed = dt.Rows[i]["TelephoneBillDed"].ToString();
                                    if (strTelephoneBillDed.Trim().Length > 0)
                                    {
                                        totalTelephoneBillDed += Convert.ToSingle(strTelephoneBillDed);
                                    }
                                    string strDed = dt.Rows[i]["TotalDeductions"].ToString();
                                    if (strDed.Trim().Length > 0)
                                    {
                                        totalDed += Convert.ToSingle(strDed);
                                    }
                                    string actualAmount = dt.Rows[i]["ActualAmount"].ToString();
                                    if (actualAmount.Trim().Length > 0)
                                    {
                                        totalActualamount += Convert.ToSingle(actualAmount);
                                    }

                                    string strfixedADDL4HR = dt.Rows[i]["fixedADDL4HR"].ToString();
                                    if (strfixedADDL4HR.Trim().Length > 0)
                                    {
                                        totalfixedADDL4HR += Convert.ToSingle(strfixedADDL4HR);

                                    }

                                    string strfixedQTRALLOW = dt.Rows[i]["fixedQTRALLOW"].ToString();
                                    if (strfixedQTRALLOW.Trim().Length > 0)
                                    {
                                        totalfixedQTRALLOW += Convert.ToSingle(strfixedQTRALLOW);

                                    }

                                    string strfixedRELALLOW = dt.Rows[i]["fixedRELALLOW"].ToString();
                                    if (strfixedRELALLOW.Trim().Length > 0)
                                    {
                                        totalfixedRELALLOW += Convert.ToSingle(strfixedRELALLOW);

                                    }

                                    string strfixedSITEALLOW = dt.Rows[i]["fixedSITEALLOW"].ToString();
                                    if (strfixedSITEALLOW.Trim().Length > 0)
                                    {
                                        totalfixedSITEALLOW += Convert.ToSingle(strfixedSITEALLOW);

                                    }

                                    string strfixedGunAllw = dt.Rows[i]["fixedGunAllw"].ToString();
                                    if (strfixedGunAllw.Trim().Length > 0)
                                    {
                                        totalfixedGunAllw += Convert.ToSingle(strfixedGunAllw);

                                    }

                                    string strfixedFireAllw = dt.Rows[i]["fixedFireAllw"].ToString();
                                    if (strfixedFireAllw.Trim().Length > 0)
                                    {
                                        totalfixedFireAllw += Convert.ToSingle(strfixedFireAllw);

                                    }

                                    string strfixedTelephoneAllw = dt.Rows[i]["fixedTelephoneAllw"].ToString();
                                    if (strfixedTelephoneAllw.Trim().Length > 0)
                                    {
                                        totalfixedTelephoneAllw += Convert.ToSingle(strfixedTelephoneAllw);

                                    }

                                    string strfixedReimbursement = dt.Rows[i]["fixedReimbursement"].ToString();
                                    if (strfixedReimbursement.Trim().Length > 0)
                                    {
                                        totalfixedReimbursement += Convert.ToSingle(strfixedReimbursement);

                                    }

                                    string strfixedHardshipAllw = dt.Rows[i]["fixedHardshipAllw"].ToString();
                                    if (strfixedHardshipAllw.Trim().Length > 0)
                                    {
                                        totalfixedHardshipAllw += Convert.ToSingle(strfixedHardshipAllw);

                                    }

                                    string strfixedPaidHolidayAllw = dt.Rows[i]["fixedPaidHolidayAllw"].ToString();
                                    if (strfixedPaidHolidayAllw.Trim().Length > 0)
                                    {
                                        totalfixedPaidHolidayAllw += Convert.ToSingle(strfixedPaidHolidayAllw);

                                    }

                                    string strADDL4HR = dt.Rows[i]["ADDL4HR"].ToString();
                                    if (strADDL4HR.Trim().Length > 0)
                                    {
                                        totalADDL4HR += Convert.ToSingle(strADDL4HR);

                                    }

                                    string strQTRALLOW = dt.Rows[i]["QTRALLOW"].ToString();
                                    if (strQTRALLOW.Trim().Length > 0)
                                    {
                                        totalQTRALLOW += Convert.ToSingle(strQTRALLOW);

                                    }

                                    string strRELALLOW = dt.Rows[i]["RELALLOW"].ToString();
                                    if (strRELALLOW.Trim().Length > 0)
                                    {
                                        totalRELALLOW += Convert.ToSingle(strRELALLOW);

                                    }

                                    string strSITEALLOW = dt.Rows[i]["SITEALLOW"].ToString();
                                    if (strSITEALLOW.Trim().Length > 0)
                                    {
                                        totalSITEALLOW += Convert.ToSingle(strSITEALLOW);

                                    }

                                    string strGunAllw = dt.Rows[i]["GunAllw"].ToString();
                                    if (strGunAllw.Trim().Length > 0)
                                    {
                                        totalGunAllw += Convert.ToSingle(strGunAllw);

                                    }

                                    string strFireAllw = dt.Rows[i]["FireAllw"].ToString();
                                    if (strFireAllw.Trim().Length > 0)
                                    {
                                        totalFireAllw += Convert.ToSingle(strFireAllw);

                                    }

                                    string strTelephoneAllw = dt.Rows[i]["TelephoneAllw"].ToString();
                                    if (strTelephoneAllw.Trim().Length > 0)
                                    {
                                        totalTelephoneAllw += Convert.ToSingle(strTelephoneAllw);

                                    }

                                    string strReimbursement = dt.Rows[i]["Reimbursement"].ToString();
                                    if (strReimbursement.Trim().Length > 0)
                                    {
                                        totalReimbursement += Convert.ToSingle(strReimbursement);

                                    }

                                    string strHardshipAllw = dt.Rows[i]["HardshipAllw"].ToString();
                                    if (strHardshipAllw.Trim().Length > 0)
                                    {
                                        totalHardshipAllw += Convert.ToSingle(strHardshipAllw);

                                    }

                                    string strPaidHolidayAllw = dt.Rows[i]["PaidHolidayAllw"].ToString();
                                    if (strPaidHolidayAllw.Trim().Length > 0)
                                    {
                                        totalPaidHolidayAllw += Convert.ToSingle(strPaidHolidayAllw);

                                    }

                                    //string strUniformAllw = dt.Rows[i]["UniformAllw"].ToString();
                                    //if (strUniformAllw.Trim().Length > 0)
                                    //{
                                    //    totalUniformAllw += Convert.ToSingle(strUniformAllw);

                                    //}
                                    //string strNPCl25Per = dt.Rows[i]["OTHrs"].ToString();
                                    //if (strNPCl25Per.Trim().Length > 0)
                                    //{
                                    //    totalNPCl25Per += Convert.ToSingle(strNPCl25Per);
                                    //}

                                    //string Div = dt.Rows[i]["DivisionName"].ToString();
                                    //if (Div.Trim().Length > 0)
                                    //{
                                    //    totalDiv += Convert.ToSingle(Div);
                                    //}
                                    //string Area = dt.Rows[i]["Location"].ToString();
                                    //if (Area.Trim().Length > 0)
                                    //{
                                    //    totalArea += Convert.ToSingle(Area);
                                    //}
                                    //string ntempgross = dt.Rows[i]["tempgross"].ToString();
                                    //if (ntempgross.Trim().Length > 0)
                                    //{
                                    //    totaltempgross += Convert.ToSingle(ntempgross);
                                    //}
                                    //string strcdOtAmt = dt.Rows[i]["cdOTAmt"].ToString();
                                    //if (strcdOtAmt.Trim().Length > 0)
                                    //{
                                    //    totalcdOtAmt += Convert.ToSingle(strcdOtAmt);

                                    //}
                                    //string strTransport = dt.Rows[i]["OTHrsAmt"].ToString();
                                    //if (strTransport.Trim().Length > 0)
                                    //{
                                    //    totalTransport += Convert.ToSingle(strTransport);
                                    //}
                                    //string strMobInstDed = dt.Rows[i]["MobInstDed"].ToString();
                                    //if (strMobInstDed.Trim().Length > 0)
                                    //{
                                    //    totalMobInstDed += Convert.ToSingle(strMobInstDed);

                                    //}

                                    //string strTDSDed = dt.Rows[i]["TDSDed"].ToString();
                                    //if (strTDSDed.Trim().Length > 0)
                                    //{
                                    //    totalTDSDed += Convert.ToSingle(strTDSDed);

                                    //}
                                    //string strNightAllw = dt.Rows[i]["NightAllw"].ToString();
                                    //if (strNightAllw.Trim().Length > 0)
                                    //{
                                    //    totalNightAllw += Convert.ToSingle(strNightAllw);

                                    //}
                                    //string strtotalwed = dt.Rows[i]["WCDed"].ToString();
                                    //if (strtotalwed.Trim().Length > 0)
                                    //{
                                    //    totalwed += Convert.ToSingle(strtotalwed);

                                    //}

                                    ////
                                    //string strpfempr = dt.Rows[i]["pfempr"].ToString();
                                    //if (strpfempr.Trim().Length > 0)
                                    //{
                                    //    totalpfempr += Convert.ToSingle(strpfempr);
                                    //}

                                    //string stresiempr = dt.Rows[i]["esiempr"].ToString();
                                    //if (stresiempr.Trim().Length > 0)
                                    //{
                                    //    totalesiempr += Convert.ToSingle(stresiempr);
                                    //}

                                    //string strctc = dt.Rows[i]["CTC"].ToString();
                                    //if (strctc.Trim().Length > 0)
                                    //{
                                    //    totalctc += Convert.ToSingle(strctc);
                                    //}

                                }
                                #endregion
                            }
                        }
                        #region for total


                        // Total Fixed Variables 

                        //26-04-2018

                        Label lblTotalCdBasic = GVListEmployees.FooterRow.FindControl("lblTotalCdBasic") as Label;
                        lblTotalCdBasic.Text = Math.Round(totalCdBasic).ToString();

                        if (totalCdBasic > 0)
                        {
                            GVListEmployees.Columns[19].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[19].Visible = false;

                        }
                        Label lblTotalCdDA = GVListEmployees.FooterRow.FindControl("lblTotalCdDA") as Label;
                        lblTotalCdDA.Text = Math.Round(totalCdDA).ToString();

                        if (totalCdDA > 0)
                        {
                            GVListEmployees.Columns[20].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[20].Visible = false;

                        }
                        Label lblTotalCdHRA = GVListEmployees.FooterRow.FindControl("lblTotalCdHRA") as Label;
                        lblTotalCdHRA.Text = Math.Round(totalCdHRA).ToString();

                        if (totalCdHRA > 0)
                        {
                            GVListEmployees.Columns[21].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[21].Visible = false;

                        }
                        Label lblTotalCdCCA = GVListEmployees.FooterRow.FindControl("lblTotalCdCCA") as Label;
                        lblTotalCdCCA.Text = Math.Round(totalCdCCA).ToString();
                        if (totalCdCCA > 0)
                        {
                            GVListEmployees.Columns[22].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[22].Visible = false;

                        }
                        Label lblTotalCdConveyance = GVListEmployees.FooterRow.FindControl("lblTotalCdConveyance") as Label;
                        lblTotalCdConveyance.Text = Math.Round(totalCdConveyance).ToString();
                        if (totalCdConveyance > 0)
                        {
                            GVListEmployees.Columns[23].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[23].Visible = false;

                        }
                        Label lblTotalcdWA = GVListEmployees.FooterRow.FindControl("lblTotalcdWA") as Label;
                        lblTotalcdWA.Text = Math.Round(totalCdWA).ToString();
                        if (totalCdWA > 0)
                        {
                            GVListEmployees.Columns[24].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[24].Visible = false;

                        }
                        Label lblTotalcdNfhs = GVListEmployees.FooterRow.FindControl("lblTotalcdNfhs") as Label;
                        lblTotalcdNfhs.Text = Math.Round(totalCdNfhs).ToString();
                        if (totalCdNfhs > 0)
                        {
                            GVListEmployees.Columns[25].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[25].Visible = false;

                        }
                        Label lblTotalcdrc = GVListEmployees.FooterRow.FindControl("lblTotalcdrc") as Label;
                        lblTotalcdrc.Text = Math.Round(totalCdrc).ToString();
                        if (totalCdrc > 0)
                        {
                            GVListEmployees.Columns[26].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[26].Visible = false;

                        }
                        Label lblTotalcdcs = GVListEmployees.FooterRow.FindControl("lblTotalcdcs") as Label;
                        lblTotalcdcs.Text = Math.Round(totalCdcs).ToString();
                        if (totalCdcs > 0)
                        {
                            GVListEmployees.Columns[27].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[27].Visible = false;

                        }
                        Label lbltTotalcdAddlAmount = GVListEmployees.FooterRow.FindControl("lbltTotalcdAddlAmount") as Label;
                        lbltTotalcdAddlAmount.Text = Math.Round(totalCdAddlAmount).ToString();
                        if (totalCdAddlAmount > 0)
                        {
                            GVListEmployees.Columns[28].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[28].Visible = false;

                        }
                        Label lblTotalcdFoodAllowance = GVListEmployees.FooterRow.FindControl("lblTotalcdFoodAllowance") as Label;
                        lblTotalcdFoodAllowance.Text = Math.Round(totalCdFoodAllw).ToString();
                        if (totalCdFoodAllw > 0)
                        {
                            GVListEmployees.Columns[29].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[29].Visible = false;

                        }
                        Label lblTotalcdWOAmount = GVListEmployees.FooterRow.FindControl("lblTotalcdWOAmount") as Label;
                        lblTotalcdWOAmount.Text = Math.Round(totalCdWOAmt).ToString();
                        if (totalCdWOAmt > 0)
                        {
                            GVListEmployees.Columns[30].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[30].Visible = false;

                        }
                        Label lblTotalcdNhsAmount = GVListEmployees.FooterRow.FindControl("lblTotalcdNhsAmount") as Label;
                        lblTotalcdNhsAmount.Text = Math.Round(totalCdNHsAmt).ToString();
                        if (totalCdNHsAmt > 0)
                        {
                            GVListEmployees.Columns[31].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[31].Visible = false;

                        }
                        Label lblTotalcdmedicalallowance = GVListEmployees.FooterRow.FindControl("lblTotalcdmedicalallowance") as Label;
                        lblTotalcdmedicalallowance.Text = Math.Round(totalCdMedicalReimbursement).ToString();
                        if (totalCdMedicalReimbursement > 0)
                        {
                            GVListEmployees.Columns[32].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[32].Visible = false;


                        }
                        Label lblTotalcdSpecialAllowance = GVListEmployees.FooterRow.FindControl("lblTotalcdSpecialAllowance") as Label;
                        lblTotalcdSpecialAllowance.Text = Math.Round(totalCdSpecialAllW).ToString();
                        if (totalCdSpecialAllW > 0)
                        {
                            GVListEmployees.Columns[33].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[33].Visible = false;

                        }
                        Label lblTotalcdTravelAllw = GVListEmployees.FooterRow.FindControl("lblTotalcdTravelAllw") as Label;
                        lblTotalcdTravelAllw.Text = Math.Round(totalCdTravelAllw).ToString();
                        if (totalCdTravelAllw > 0)
                        {
                            GVListEmployees.Columns[34].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[34].Visible = false;

                        }
                        Label lblTotalcdMobileAllowance = GVListEmployees.FooterRow.FindControl("lblTotalcdMobileAllowance") as Label;
                        lblTotalcdMobileAllowance.Text = Math.Round(totalCdMobileAllw).ToString();
                        if (totalCdMobileAllw > 0)
                        {
                            GVListEmployees.Columns[35].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[35].Visible = false;

                        }
                        Label lblTotalcdPerformanceAllw = GVListEmployees.FooterRow.FindControl("lblTotalcdPerformanceAllw") as Label;
                        lblTotalcdPerformanceAllw.Text = Math.Round(totalCdPerformanceAllw).ToString();
                        if (totalCdPerformanceAllw > 0)
                        {
                            GVListEmployees.Columns[36].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[36].Visible = false;

                        }
                        Label lblTotalcdLeaveEncashAmt = GVListEmployees.FooterRow.FindControl("lblTotalcdLeaveEncashAmt") as Label;
                        lblTotalcdLeaveEncashAmt.Text = Math.Round(totalCdLW).ToString();
                        if (totalCdLW > 0)
                        {
                            GVListEmployees.Columns[37].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[37].Visible = false;

                        }
                        Label lblTotalcdNpotsAmount = GVListEmployees.FooterRow.FindControl("lblTotalcdNpotsAmount") as Label;
                        lblTotalcdNpotsAmount.Text = Math.Round(totalCdNPOTsAmt).ToString();
                        if (totalCdNPOTsAmt > 0)
                        {
                            GVListEmployees.Columns[38].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[38].Visible = false;

                        }
                        Label lblTotalcdIncentivs = GVListEmployees.FooterRow.FindControl("lblTotalcdIncentivs") as Label;
                        lblTotalcdIncentivs.Text = Math.Round(totalCdIncentivs).ToString();
                        if (totalCdIncentivs > 0)
                        {
                            GVListEmployees.Columns[39].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[39].Visible = false;

                        }
                        Label lblTotalcdBonus = GVListEmployees.FooterRow.FindControl("lblTotalcdBonus") as Label;
                        lblTotalcdBonus.Text = Math.Round(totalCdBonus).ToString();
                        if (totalCdBonus > 0)
                        {
                            GVListEmployees.Columns[40].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[40].Visible = false;

                        }
                        Label lblTotalcdGratuity = GVListEmployees.FooterRow.FindControl("lblTotalcdGratuity") as Label;
                        lblTotalcdGratuity.Text = Math.Round(totalCdGratuity).ToString();
                        if (totalCdGratuity > 0)
                        {
                            GVListEmployees.Columns[41].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[41].Visible = false;

                        }
                        Label lblTotalcdOA = GVListEmployees.FooterRow.FindControl("lblTotalcdOA") as Label;
                        lblTotalcdOA.Text = Math.Round(totalCdOA).ToString();
                        if (totalCdOA > 0)
                        {
                            GVListEmployees.Columns[42].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[42].Visible = false;

                        }

                        Label lblTotalcdOTAmount = GVListEmployees.FooterRow.FindControl("lblTotalcdOTAmount") as Label;
                        lblTotalcdOTAmount.Text = Math.Round(totalcdOtRate).ToString();
                        if (totalcdOtRate > 0)
                        {
                            GVListEmployees.Columns[43].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[43].Visible = false;

                        }
                        Label lblTotalcdServiceWeightage = GVListEmployees.FooterRow.FindControl("lblTotalcdServiceWeightage") as Label;
                        lblTotalcdServiceWeightage.Text = Math.Round(totalCdServiceWeightage).ToString();
                        if (totalCdServiceWeightage > 0)
                        {
                            GVListEmployees.Columns[44].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[44].Visible = false;

                        }
                        Label lblTotalcdArrears = GVListEmployees.FooterRow.FindControl("lblTotalcdArrears") as Label;
                        lblTotalcdArrears.Text = Math.Round(totalCdArrears).ToString();
                        if (totalCdArrears > 0)
                        {
                            GVListEmployees.Columns[45].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[45].Visible = false;

                        }
                        Label lblTotalcdAttBonus = GVListEmployees.FooterRow.FindControl("lblTotalcdAttBonus") as Label;
                        lblTotalcdAttBonus.Text = Math.Round(totalCdAttBonus).ToString();
                        if (totalCdAttBonus > 0)
                        {
                            GVListEmployees.Columns[46].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[46].Visible = false;

                        }
                        Label lblTotalcdNightAllw = GVListEmployees.FooterRow.FindControl("lblTotalcdNightAllw") as Label;
                        lblTotalcdNightAllw.Text = Math.Round(totalCdNightAllw).ToString();
                        if (totalCdNightAllw > 0)
                        {
                            GVListEmployees.Columns[47].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[47].Visible = false;

                        }
                        Label lblTotalfixedADDL4HR = GVListEmployees.FooterRow.FindControl("lblTotalfixedADDL4HR") as Label;
                        lblTotalfixedADDL4HR.Text = Math.Round(totalfixedADDL4HR).ToString();
                        if (totalfixedADDL4HR > 0)
                        {
                            GVListEmployees.Columns[48].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[48].Visible = false;

                        }
                        Label lblTotalfixedQTRALLOW = GVListEmployees.FooterRow.FindControl("lblTotalfixedQTRALLOW") as Label;
                        lblTotalfixedQTRALLOW.Text = Math.Round(totalfixedQTRALLOW).ToString();
                        if (totalfixedQTRALLOW > 0)
                        {
                            GVListEmployees.Columns[49].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[49].Visible = false;

                        }
                        Label lblTotalfixedRELALLOW = GVListEmployees.FooterRow.FindControl("lblTotalfixedRELALLOW") as Label;
                        lblTotalfixedRELALLOW.Text = Math.Round(totalfixedRELALLOW).ToString();
                        if (totalfixedRELALLOW > 0)
                        {
                            GVListEmployees.Columns[50].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[50].Visible = false;

                        }
                        Label lblTotalfixedSITEALLOW = GVListEmployees.FooterRow.FindControl("lblTotalfixedSITEALLOW") as Label;
                        lblTotalfixedSITEALLOW.Text = Math.Round(totalfixedSITEALLOW).ToString();
                        if (totalfixedSITEALLOW > 0)
                        {
                            GVListEmployees.Columns[51].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[51].Visible = false;

                        }

                        Label lblTotalfixedGunAllw = GVListEmployees.FooterRow.FindControl("lblTotalfixedGunAllw") as Label;
                        lblTotalfixedGunAllw.Text = Math.Round(totalfixedGunAllw).ToString();
                        if (totalfixedGunAllw > 0)
                        {
                            GVListEmployees.Columns[52].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[52].Visible = false;

                        }
                        Label lblTotalfixedFireAllw = GVListEmployees.FooterRow.FindControl("lblTotalfixedFireAllw") as Label;
                        lblTotalfixedFireAllw.Text = Math.Round(totalfixedFireAllw).ToString();
                        if (totalfixedFireAllw > 0)
                        {
                            GVListEmployees.Columns[53].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[53].Visible = false;

                        }

                        Label lblTotalfixedTelephoneAllw = GVListEmployees.FooterRow.FindControl("lblTotalfixedTelephoneAllw") as Label;
                        lblTotalfixedTelephoneAllw.Text = Math.Round(totalfixedTelephoneAllw).ToString();
                        if (totalfixedTelephoneAllw > 0)
                        {
                            GVListEmployees.Columns[54].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[54].Visible = false;

                        }
                        Label lblTotalfixedReimbursement = GVListEmployees.FooterRow.FindControl("lblTotalfixedReimbursement") as Label;
                        lblTotalfixedReimbursement.Text = Math.Round(totalfixedReimbursement).ToString();
                        if (totalfixedReimbursement > 0)
                        {
                            GVListEmployees.Columns[55].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[55].Visible = false;

                        }

                        Label lblTotalfixedHardshipAllw = GVListEmployees.FooterRow.FindControl("lblTotalfixedHardshipAllw") as Label;
                        lblTotalfixedHardshipAllw.Text = Math.Round(totalfixedHardshipAllw).ToString();
                        if (totalfixedHardshipAllw > 0)
                        {
                            GVListEmployees.Columns[56].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[56].Visible = false;

                        }
                        Label lblTotalfixedPaidHolidayAllw = GVListEmployees.FooterRow.FindControl("lblTotalfixedPaidHolidayAllw") as Label;
                        lblTotalfixedPaidHolidayAllw.Text = Math.Round(totalfixedPaidHolidayAllw).ToString();
                        if (totalfixedPaidHolidayAllw > 0)
                        {
                            GVListEmployees.Columns[57].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[57].Visible = false;

                        }


                        Label lblTotalDuties = GVListEmployees.FooterRow.FindControl("lblTotalDuties") as Label;
                        lblTotalDuties.Text = Math.Round(totalDuties).ToString();
                        if (totalDuties > 0)
                        {
                            GVListEmployees.Columns[58].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[58].Visible = false;

                        }
                        Label lblTotalOts = GVListEmployees.FooterRow.FindControl("lblTotalOts") as Label;
                        lblTotalOts.Text = Math.Round(totalOts).ToString();
                        if (totalOts > 0)
                        {
                            GVListEmployees.Columns[59].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[59].Visible = false;

                        }


                        Label lblTotalwos = GVListEmployees.FooterRow.FindControl("lblTotalwos") as Label;
                        lblTotalwos.Text = Math.Round(totalwo).ToString();

                        if (totalwo > 0)
                        {
                            GVListEmployees.Columns[60].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[60].Visible = false;

                        }

                        Label lblTotalNhs = GVListEmployees.FooterRow.FindControl("lblTotalNhs") as Label;
                        lblTotalNhs.Text = Math.Round(totalnhs).ToString();

                        if (totalnhs > 0)
                        {
                            GVListEmployees.Columns[61].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[61].Visible = false;

                        }
                        Label lblTotalNpots = GVListEmployees.FooterRow.FindControl("lblTotalNpots") as Label;
                        lblTotalNpots.Text = Math.Round(totalnpots).ToString();

                        if (totalnpots > 0)
                        {
                            GVListEmployees.Columns[62].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[62].Visible = false;

                        }

                        Label lblTotalBasic = GVListEmployees.FooterRow.FindControl("lblTotalBasic") as Label;
                        lblTotalBasic.Text = Math.Round(totalBasic).ToString();
                        if (totalBasic > 0)
                        {
                            GVListEmployees.Columns[63].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[63].Visible = false;

                        }
                        Label lblTotalDA = GVListEmployees.FooterRow.FindControl("lblTotalDA") as Label;
                        lblTotalDA.Text = Math.Round(totalDA).ToString();

                        if (totalDA > 0)
                        {
                            GVListEmployees.Columns[64].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[64].Visible = false;

                        }

                        Label lblTotalHRA = GVListEmployees.FooterRow.FindControl("lblTotalHRA") as Label;
                        lblTotalHRA.Text = Math.Round(totalHRA).ToString();

                        if (totalHRA > 0)
                        {
                            GVListEmployees.Columns[65].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[65].Visible = false;

                        }

                        Label lblTotalCCA = GVListEmployees.FooterRow.FindControl("lblTotalCCA") as Label;
                        lblTotalCCA.Text = Math.Round(totalCCA).ToString();

                        if (totalCCA > 0)
                        {
                            GVListEmployees.Columns[66].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[66].Visible = false;

                        }

                        Label lblTotalConveyance = GVListEmployees.FooterRow.FindControl("lblTotalConveyance") as Label;
                        lblTotalConveyance.Text = Math.Round(totalConveyance).ToString();

                        if (totalConveyance > 0)
                        {
                            GVListEmployees.Columns[67].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[67].Visible = false;

                        }

                        Label lblTotalWA = GVListEmployees.FooterRow.FindControl("lblTotalWA") as Label;
                        lblTotalWA.Text = Math.Round(totalWA).ToString();

                        if (totalWA > 0)
                        {
                            GVListEmployees.Columns[68].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[68].Visible = false;

                        }

                        Label lblTotalNfhs = GVListEmployees.FooterRow.FindControl("lblTotalNfhs") as Label;
                        lblTotalNfhs.Text = Math.Round(totalnfhs).ToString();

                        if (totalnfhs > 0)
                        {
                            GVListEmployees.Columns[69].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[69].Visible = false;

                        }

                        Label lblTotalrc = GVListEmployees.FooterRow.FindControl("lblTotalrc") as Label;
                        lblTotalrc.Text = Math.Round(totalRC).ToString();

                        if (totalRC > 0)
                        {
                            GVListEmployees.Columns[70].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[70].Visible = false;

                        }

                        Label lblTotalcs = GVListEmployees.FooterRow.FindControl("lblTotalcs") as Label;
                        lblTotalcs.Text = Math.Round(totalCS).ToString();

                        if (totalCS > 0)
                        {
                            GVListEmployees.Columns[71].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[71].Visible = false;

                        }
                        Label lbltTotalAddlAmount = GVListEmployees.FooterRow.FindControl("lbltTotalAddlAmount") as Label;
                        lbltTotalAddlAmount.Text = Math.Round(totalAddlAmount).ToString();

                        if (totalAddlAmount > 0)
                        {
                            GVListEmployees.Columns[72].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[72].Visible = false;

                        }
                        Label lblTotalFoodAllowance = GVListEmployees.FooterRow.FindControl("lblTotalFoodAllowance") as Label;
                        lblTotalFoodAllowance.Text = Math.Round(totalFoodAllowance).ToString();

                        if (totalFoodAllowance > 0)
                        {
                            GVListEmployees.Columns[73].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[73].Visible = false;
                        }

                        Label lblTotalWOAmount = GVListEmployees.FooterRow.FindControl("lblTotalWOAmount") as Label;
                        lblTotalWOAmount.Text = Math.Round(totalWoAmt).ToString();

                        if (totalWoAmt > 0)
                        {
                            GVListEmployees.Columns[74].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[74].Visible = false;

                        }

                        Label lblTotalNhsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNhsAmount") as Label;
                        lblTotalNhsAmount.Text = Math.Round(totalNhsAmt).ToString();

                        if (totalNhsAmt > 0)
                        {
                            GVListEmployees.Columns[75].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[75].Visible = false;

                        }

                        Label lblTotalmedicalallowance = GVListEmployees.FooterRow.FindControl("lblTotalmedicalallowance") as Label;
                        lblTotalmedicalallowance.Text = Math.Round(totalmedicalallowance).ToString();

                        if (totalmedicalallowance > 0)
                        {
                            GVListEmployees.Columns[76].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[76].Visible = false;
                        }


                        Label lblTotalSpecialAllowance = GVListEmployees.FooterRow.FindControl("lblTotalSpecialAllowance") as Label;
                        lblTotalSpecialAllowance.Text = Math.Round(totalSpecialAllowance).ToString();

                        if (totalSpecialAllowance > 0)
                        {
                            GVListEmployees.Columns[77].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[77].Visible = false;

                        }

                        Label lblTotalTravelAllw = GVListEmployees.FooterRow.FindControl("lblTotalTravelAllw") as Label;
                        lblTotalTravelAllw.Text = Math.Round(totalTravelAllw).ToString();

                        if (totalTravelAllw > 0)
                        {
                            GVListEmployees.Columns[78].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[78].Visible = false;
                        }

                        Label lblTotalMobileAllowance = GVListEmployees.FooterRow.FindControl("lblTotalMobileAllowance") as Label;
                        lblTotalMobileAllowance.Text = Math.Round(totalMobileAllowance).ToString();

                        if (totalMobileAllowance > 0)
                        {
                            GVListEmployees.Columns[79].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[79].Visible = false;

                        }
                        Label lblTotalPerformanceAllw = GVListEmployees.FooterRow.FindControl("lblTotalPerformanceAllw") as Label;
                        lblTotalPerformanceAllw.Text = Math.Round(totalPerformanceAllw).ToString();
                        if (totalPerformanceAllw > 0)
                        {
                            GVListEmployees.Columns[80].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[80].Visible = false;

                        }

                        Label lblTotalLeaveEncashAmt = GVListEmployees.FooterRow.FindControl("lblTotalLeaveEncashAmt") as Label;
                        lblTotalLeaveEncashAmt.Text = Math.Round(totalLeaveEncashAmt).ToString();

                        if (totalLeaveEncashAmt > 0)
                        {
                            GVListEmployees.Columns[81].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[81].Visible = false;

                        }
                        Label lblTotalNpotsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNpotsAmount") as Label;
                        lblTotalNpotsAmount.Text = Math.Round(totalNpotsAmt).ToString();

                        if (totalNpotsAmt > 0)
                        {
                            GVListEmployees.Columns[82].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[82].Visible = false;

                        }


                        Label lblTotalIncentivs = GVListEmployees.FooterRow.FindControl("lblTotalIncentivs") as Label;
                        lblTotalIncentivs.Text = Math.Round(totalIncentivs).ToString();
                        if (totalIncentivs > 0)
                        {
                            GVListEmployees.Columns[83].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[83].Visible = false;

                        }
                        Label lblTotalBonus = GVListEmployees.FooterRow.FindControl("lblTotalBonus") as Label;
                        lblTotalBonus.Text = Math.Round(totalBonus).ToString();
                        if (totalBonus > 0)
                        {
                            GVListEmployees.Columns[84].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[84].Visible = false;

                        }

                        Label lblTotalGratuity = GVListEmployees.FooterRow.FindControl("lblTotalGratuity") as Label;
                        lblTotalGratuity.Text = Math.Round(totalGratuity).ToString();

                        if (totalGratuity > 0)
                        {
                            GVListEmployees.Columns[85].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[85].Visible = false;

                        }
                        Label lblTotalOA = GVListEmployees.FooterRow.FindControl("lblTotalOA") as Label;
                        lblTotalOA.Text = Math.Round(totalOA).ToString();

                        if (totalOA > 0)
                        {
                            GVListEmployees.Columns[86].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[86].Visible = false;

                        }
                        Label lblTotalOTAmount = GVListEmployees.FooterRow.FindControl("lblTotalOTAmount") as Label;
                        lblTotalOTAmount.Text = Math.Round(totalOTAmount).ToString();

                        if (totalOTAmount > 0)
                        {
                            GVListEmployees.Columns[87].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[87].Visible = false;

                        }
                        Label lblTotalServiceWeightage = GVListEmployees.FooterRow.FindControl("lblTotalServiceWeightage") as Label;
                        lblTotalServiceWeightage.Text = Math.Round(totalServiceWeightage).ToString();

                        if (totalServiceWeightage > 0)
                        {
                            GVListEmployees.Columns[88].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[88].Visible = false;

                        }
                        Label lblTotalArrears = GVListEmployees.FooterRow.FindControl("lblTotalArrears") as Label;
                        lblTotalArrears.Text = Math.Round(totalArrears).ToString();

                        if (totalArrears > 0)
                        {
                            GVListEmployees.Columns[89].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[89].Visible = false;

                        }

                        Label lblTotalAttBonus = GVListEmployees.FooterRow.FindControl("lblTotalAttBonus") as Label;
                        lblTotalAttBonus.Text = Math.Round(totalAttBonus).ToString();

                        if (totalAttBonus > 0)
                        {
                            GVListEmployees.Columns[90].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[90].Visible = false;

                        }
                        Label lblTotalNightAllw = GVListEmployees.FooterRow.FindControl("lblTotalNightAllw") as Label;
                        lblTotalNightAllw.Text = Math.Round(totalNightAllw).ToString();

                        if (totalNightAllw > 0)
                        {
                            GVListEmployees.Columns[91].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[91].Visible = false;

                        }
                        Label lblTotalADDL4HR = GVListEmployees.FooterRow.FindControl("lblTotalADDL4HR") as Label;
                        lblTotalADDL4HR.Text = Math.Round(totalADDL4HR).ToString();

                        if (totalADDL4HR > 0)
                        {
                            GVListEmployees.Columns[92].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[92].Visible = false;

                        }
                        Label lblTotalQTRALLOW = GVListEmployees.FooterRow.FindControl("lblTotalQTRALLOW") as Label;
                        lblTotalQTRALLOW.Text = Math.Round(totalQTRALLOW).ToString();

                        if (totalQTRALLOW > 0)
                        {
                            GVListEmployees.Columns[93].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[93].Visible = false;

                        }

                        Label lblTotalRELALLOW = GVListEmployees.FooterRow.FindControl("lblTotalRELALLOW") as Label;
                        lblTotalRELALLOW.Text = Math.Round(totalRELALLOW).ToString();
                        if (totalRELALLOW > 0)
                        {
                            GVListEmployees.Columns[94].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[94].Visible = false;

                        }
                        Label lblTotalSITEALLOW = GVListEmployees.FooterRow.FindControl("lblTotalSITEALLOW") as Label;
                        lblTotalSITEALLOW.Text = Math.Round(totalSITEALLOW).ToString();
                        if (totalSITEALLOW > 0)
                        {
                            GVListEmployees.Columns[95].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[95].Visible = false;

                        }

                        Label lblTotalGunAllw = GVListEmployees.FooterRow.FindControl("lblTotalGunAllw") as Label;
                        lblTotalGunAllw.Text = Math.Round(totalGunAllw).ToString();
                        if (totalGunAllw > 0)
                        {
                            GVListEmployees.Columns[96].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[96].Visible = false;

                        }
                        Label lblTotalFireAllw = GVListEmployees.FooterRow.FindControl("lblTotalFireAllw") as Label;
                        lblTotalFireAllw.Text = Math.Round(totalFireAllw).ToString();
                        if (totalFireAllw > 0)
                        {
                            GVListEmployees.Columns[97].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[97].Visible = false;

                        }

                        Label lblTotalTelephoneAllw = GVListEmployees.FooterRow.FindControl("lblTotalTelephoneAllw") as Label;
                        lblTotalTelephoneAllw.Text = Math.Round(totalTelephoneAllw).ToString();
                        if (totalTelephoneAllw > 0)
                        {
                            GVListEmployees.Columns[98].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[98].Visible = false;

                        }
                        Label lblTotalReimbursement = GVListEmployees.FooterRow.FindControl("lblTotalReimbursement") as Label;
                        lblTotalReimbursement.Text = Math.Round(totalReimbursement).ToString();
                        if (totalReimbursement > 0)
                        {
                            GVListEmployees.Columns[99].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[99].Visible = false;

                        }

                        Label lblTotalHardshipAllw = GVListEmployees.FooterRow.FindControl("lblTotalHardshipAllw") as Label;
                        lblTotalHardshipAllw.Text = Math.Round(totalHardshipAllw).ToString();
                        if (totalHardshipAllw > 0)
                        {
                            GVListEmployees.Columns[100].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[100].Visible = false;

                        }
                        Label lblTotalPaidHolidayAllw = GVListEmployees.FooterRow.FindControl("lblTotalPaidHolidayAllw") as Label;
                        lblTotalPaidHolidayAllw.Text = Math.Round(totalPaidHolidayAllw).ToString();
                        if (totalPaidHolidayAllw > 0)
                        {
                            GVListEmployees.Columns[101].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[101].Visible = false;

                        }

                        Label lblTotalEmpty3 = GVListEmployees.FooterRow.FindControl("lblTotalEmpty3") as Label;
                        lblTotalEmpty3.Text = Math.Round(totalEmpty3).ToString();

                        if (totalEmpty3 > 0)
                        {
                            GVListEmployees.Columns[102].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[102].Visible = false;

                        }

                        Label lblTotalGross = GVListEmployees.FooterRow.FindControl("lblTotalGross") as Label;
                        lblTotalGross.Text = Math.Round(totalGrass).ToString();
                        if (totalGrass > 0)
                        {
                            GVListEmployees.Columns[103].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[103].Visible = false;

                        }

                        Label lblTotalPF = GVListEmployees.FooterRow.FindControl("lblTotalPF") as Label;
                        lblTotalPF.Text = Math.Round(totalPF).ToString();
                        if (totalPF > 0)
                        {
                            GVListEmployees.Columns[104].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[104].Visible = false;

                        }

                        Label lblTotalESI = GVListEmployees.FooterRow.FindControl("lblTotalESI") as Label;
                        lblTotalESI.Text = Math.Round(totalESI).ToString();
                        if (totalESI > 0)
                        {
                            GVListEmployees.Columns[105].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[105].Visible = false;

                        }

                        Label lblTotalProfTax = GVListEmployees.FooterRow.FindControl("lblTotalProfTax") as Label;
                        lblTotalProfTax.Text = Math.Round(totalProfTax).ToString();
                        if (totalProfTax > 0)
                        {
                            GVListEmployees.Columns[106].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[106].Visible = false;

                        }


                        Label lblTotalsaladv = GVListEmployees.FooterRow.FindControl("lblTotalsaladv") as Label;
                        lblTotalsaladv.Text = Math.Round(totalSalAdv).ToString();

                        if (totalSalAdv > 0)
                        {
                            GVListEmployees.Columns[107].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[107].Visible = false;

                        }

                        Label lblTotaladvded = GVListEmployees.FooterRow.FindControl("lblTotaladvded") as Label;
                        lblTotaladvded.Text = Math.Round(totalAdvDed).ToString();
                        if (totalAdvDed > 0)
                        {
                            GVListEmployees.Columns[108].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[108].Visible = false;

                        }
                        Label lblTotalwed = GVListEmployees.FooterRow.FindControl("lblTotalwed") as Label;
                        lblTotalwed.Text = Math.Round(totalwed).ToString();
                        if (totalwed > 0)
                        {
                            GVListEmployees.Columns[109].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[109].Visible = false;

                        }

                        Label lblTotalUniformDed = GVListEmployees.FooterRow.FindControl("lblTotalUniformDed") as Label;
                        lblTotalUniformDed.Text = Math.Round(totalUniformDed).ToString();

                        if (totalUniformDed > 0)
                        {
                            GVListEmployees.Columns[110].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[110].Visible = false;

                        }

                        Label lblTotalOtherDed = GVListEmployees.FooterRow.FindControl("lblTotalOtherDed") as Label;
                        lblTotalOtherDed.Text = Math.Round(totalOtherDed).ToString();

                        if (totalOtherDed > 0)
                        {
                            GVListEmployees.Columns[111].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[111].Visible = false;

                        }

                        Label lblTotaltotalloanded = GVListEmployees.FooterRow.FindControl("lblTotaltotalloanded") as Label;
                        lblTotaltotalloanded.Text = Math.Round(totalloanded).ToString();

                        if (totalloanded > 0)
                        {
                            GVListEmployees.Columns[112].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[112].Visible = false;

                        }

                        Label lblTotalcantadv = GVListEmployees.FooterRow.FindControl("lblTotalcantadv") as Label;
                        lblTotalcantadv.Text = Math.Round(totalCanteenAdv).ToString();

                        if (totalCanteenAdv > 0)
                        {
                            GVListEmployees.Columns[113].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[113].Visible = false;

                        }
                        Label lblTotalSeepDed = GVListEmployees.FooterRow.FindControl("lblTotalSeepDed") as Label;
                        lblTotalSeepDed.Text = Math.Round(totalSeepDed).ToString();

                        if (totalSeepDed > 0)
                        {
                            GVListEmployees.Columns[114].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[114].Visible = false;

                        }


                        Label lblTotalGeneralDed = GVListEmployees.FooterRow.FindControl("lblTotalGeneralDed") as Label;
                        lblTotalGeneralDed.Text = Math.Round(totalGenDed).ToString();


                        if (totalGenDed > 0)
                        {
                            GVListEmployees.Columns[115].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[115].Visible = false;

                        }

                        Label lblTotalowf = GVListEmployees.FooterRow.FindControl("lblTotalowf") as Label;
                        lblTotalowf.Text = Math.Round(totalOWF).ToString();

                        if (totalOWF > 0)
                        {

                            GVListEmployees.Columns[116].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[116].Visible = false;

                        }

                        Label lblTotalPenalty = GVListEmployees.FooterRow.FindControl("lblTotalPenalty") as Label;
                        lblTotalPenalty.Text = Math.Round(totalPenalty).ToString();

                        if (totalPenalty > 0)
                        {
                            GVListEmployees.Columns[117].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[117].Visible = false;

                        }


                        Label lblTotalRentDed = GVListEmployees.FooterRow.FindControl("lblTotalRentDed") as Label;
                        lblTotalRentDed.Text = Math.Round(totalRentDed).ToString();

                        if (totalRentDed > 0)
                        {
                            GVListEmployees.Columns[118].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[118].Visible = false;

                        }


                        Label lblTotalMedicalDed = GVListEmployees.FooterRow.FindControl("lblTotalMedicalDed") as Label;
                        lblTotalMedicalDed.Text = Math.Round(totalMedicalDed).ToString();

                        if (totalMedicalDed > 0)
                        {
                            GVListEmployees.Columns[119].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[119].Visible = false;

                        }

                        Label lblTotalMLWFDed = GVListEmployees.FooterRow.FindControl("lblTotalMLWFDed") as Label;
                        lblTotalMLWFDed.Text = Math.Round(totalMLWFDed).ToString();

                        if (totalMLWFDed > 0)
                        {
                            GVListEmployees.Columns[120].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[120].Visible = false;

                        }

                        Label lblTotalFoodDed = GVListEmployees.FooterRow.FindControl("lblTotalFoodDed") as Label;
                        lblTotalFoodDed.Text = Math.Round(totalFoodDed).ToString();

                        if (totalFoodDed > 0)
                        {
                            GVListEmployees.Columns[121].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[121].Visible = false;

                        }

                        Label lblTotalElectricityDed = GVListEmployees.FooterRow.FindControl("lblTotalElectricityDed") as Label;
                        lblTotalElectricityDed.Text = Math.Round(totalElectricityDed).ToString();

                        if (totalElectricityDed > 0)
                        {
                            GVListEmployees.Columns[122].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[122].Visible = false;
                        }

                        Label lblTotalTransportDed = GVListEmployees.FooterRow.FindControl("lblTotalTransportDed") as Label;
                        lblTotalTransportDed.Text = Math.Round(totalTransportDed).ToString();

                        if (totalTransportDed > 0)
                        {
                            GVListEmployees.Columns[123].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[123].Visible = false;
                        }
                        Label lblTotalDced = GVListEmployees.FooterRow.FindControl("lblTotalDced") as Label;
                        lblTotalDced.Text = Math.Round(totalDced).ToString();

                        if (totalDced > 0)
                        {
                            GVListEmployees.Columns[124].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[124].Visible = false;
                        }

                        Label lblTotalLeaveDed = GVListEmployees.FooterRow.FindControl("lblTotalLeaveDed") as Label;
                        lblTotalLeaveDed.Text = Math.Round(totalLeaveDed).ToString();

                        if (totalLeaveDed > 0)
                        {
                            GVListEmployees.Columns[125].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[125].Visible = false;
                        }

                        Label lblTotalLicenseDed = GVListEmployees.FooterRow.FindControl("lblTotalLicenseDed") as Label;
                        lblTotalLicenseDed.Text = Math.Round(totalLicenseDed).ToString();

                        if (totalLicenseDed > 0)
                        {
                            GVListEmployees.Columns[126].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[126].Visible = false;
                        }


                        ///

                        Label lblTotalAdv4Ded = GVListEmployees.FooterRow.FindControl("lblTotalAdv4Ded") as Label;
                        lblTotalAdv4Ded.Text = Math.Round(totalAdv4Ded).ToString();
                        if (totalAdv4Ded > 0)
                        {
                            GVListEmployees.Columns[127].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[127].Visible = false;

                        }

                        Label lblTotalNightRoundDed = GVListEmployees.FooterRow.FindControl("lblTotalNightRoundDed") as Label;
                        lblTotalNightRoundDed.Text = Math.Round(totalNightRoundDed).ToString();
                        if (totalNightRoundDed > 0)
                        {
                            GVListEmployees.Columns[128].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[128].Visible = false;

                        }

                        Label lblTotalManpowerMobDed = GVListEmployees.FooterRow.FindControl("lblTotalManpowerMobDed") as Label;
                        lblTotalManpowerMobDed.Text = Math.Round(totalManpowerMobDed).ToString();
                        if (totalManpowerMobDed > 0)
                        {
                            GVListEmployees.Columns[129].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[129].Visible = false;

                        }


                        Label lblTotalMobileusageDed = GVListEmployees.FooterRow.FindControl("lblTotalMobileusageDed") as Label;
                        lblTotalMobileusageDed.Text = Math.Round(totalMobileusageDed).ToString();
                        if (totalMobileusageDed > 0)
                        {
                            GVListEmployees.Columns[130].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[130].Visible = false;

                        }

                        Label lblTotalMediClaimDed = GVListEmployees.FooterRow.FindControl("lblTotalMediClaimDed") as Label;
                        lblTotalMediClaimDed.Text = Math.Round(totalMediClaimDed).ToString();
                        if (totalMediClaimDed > 0)
                        {
                            GVListEmployees.Columns[131].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[131].Visible = false;

                        }


                        Label lblTotalCrisisDed = GVListEmployees.FooterRow.FindControl("lblTotalCrisisDed") as Label;
                        lblTotalCrisisDed.Text = Math.Round(totalCrisisDed).ToString();
                        if (totalCrisisDed > 0)
                        {
                            GVListEmployees.Columns[132].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[132].Visible = false;

                        }



                        Label lblTotalTelephoneBillDed = GVListEmployees.FooterRow.FindControl("lblTotalTelephoneBillDed") as Label;
                        lblTotalTelephoneBillDed.Text = Math.Round(totalTelephoneBillDed).ToString();
                        if (totalTelephoneBillDed > 0)
                        {
                            GVListEmployees.Columns[133].Visible = true;

                        }
                        else
                        {
                            GVListEmployees.Columns[133].Visible = false;

                        }

                        Label lblTotalDeductions = GVListEmployees.FooterRow.FindControl("lblTotalDeductions") as Label;
                        lblTotalDeductions.Text = Math.Round(totalDed).ToString();
                        if (totalDed > 0)
                        {
                            GVListEmployees.Columns[134].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[134].Visible = false;

                        }

                        Label lblTotalNetAmount = GVListEmployees.FooterRow.FindControl("lblTotalNetAmount") as Label;
                        lblTotalNetAmount.Text = Math.Round(totalActualamount).ToString();
                        if (totalActualamount > 0)
                        {
                            GVListEmployees.Columns[135].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[135].Visible = false;

                        }

                        //New code add as on 24/12/2013 by venkat

                        #endregion


                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Attributes.Add("class", "text");
                e.Row.Cells[7].Attributes.Add("class", "text");
                e.Row.Cells[8].Attributes.Add("class", "text");
                e.Row.Cells[9].Attributes.Add("class", "text");
                e.Row.Cells[10].Attributes.Add("class", "text");

            }
        }

        protected void ddloptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddloptions.SelectedIndex == 1)
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
                GVClientsData.DataSource = null;
                GVClientsData.DataBind();
                txtfrom.Visible = true;
                txtto.Visible = true;
                lblfrom.Visible = true;
                lblto.Visible = true;
                lblmonth.Visible = false;
                txtmonth.Visible = false;
                btngetdata.Visible = false;
                btn_Submit.Visible = true;
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
                GVClientsData.DataSource = null;
                GVClientsData.DataBind();
                txtfrom.Visible = false;
                txtto.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                btngetdata.Visible = false;
                btn_Submit.Visible = true;
            }
        }

        protected void btngetdata_Click(object sender, EventArgs e)
        {
            DisplayData();
        }

        protected void lbtn_ExportNew_Click(object sender, EventArgs e)
        {
            string filename = "NetpayReport " + ".xls";
            gve.ExportGrid(filename, hidGridView);
        }
    }
}
