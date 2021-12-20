using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ArrearWageSheetReport : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GvUtill = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string Fontstyle = "";
        string CFontstyle = "";
        string BranchID = "";
        string Branch = "";
        string Emp_id = "";
        DataTable dt;

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
                    LoadClientList();
                    LoadClientNames();

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
            BranchID = Session["BranchID"].ToString();

        }

        protected void LoadClientNames()
        {
            string qry = "select Clientid,Clientname from clients where clientid like '%" + CmpIDPrefix + "%' and  Paysheet=1  order by clientname";
            DataTable DtClientids = config.ExecuteReaderWithQueryAsync(qry).Result;
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");
            ddlcname.Items.Insert(1, "ALL");

        }

        protected void LoadClientList()
        {
            string qry = "select Clientid from clients where clientid like '%" + CmpIDPrefix + "%' and  Paysheet=1  order by clientid";
            DataTable DtClientNames = config.ExecuteReaderWithQueryAsync(qry).Result;
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
            ddlclientid.Items.Insert(1, "ALL");
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlclientid.SelectedValue = ddlcname.SelectedValue;

            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlclientid.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            if (ddlclientid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client Id/Name');", true);

                return;
            }

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            DisplayData();
        }

        protected void Bindata(string SPName, Hashtable ht)
        {
            try
            {


                DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
                if (dt.Rows.Count > 0)
                {
                    GVListEmployees.DataSource = dt;
                    GVListEmployees.DataBind();
                    // Fillpfandesidetails();
                    lbtn_Export.Visible = true;



                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        float actAmount = 0;
                        string actualAmount = dt.Rows[i]["ActualAmount"].ToString();
                        if (actualAmount.Trim().Length > 0)
                        {
                            actAmount = Convert.ToSingle(actualAmount);
                        }
                        if (actAmount != 0)
                        {
                            totalActualamount += actAmount;
                            string duties = dt.Rows[i]["NoOfDuties"].ToString();
                            if (duties.Trim().Length != 0)
                            {
                                totalDuties += Convert.ToSingle(duties);
                            }
                            string ots = dt.Rows[i]["OTs"].ToString();
                            if (ots.Trim().Length != 0)
                            {
                                totalOts += Convert.ToSingle(ots);
                            }

                            string wos = dt.Rows[i]["wo"].ToString();
                            if (wos.Trim().Length != 0)
                            {
                                totalwo += Convert.ToSingle(wos);
                            }
                            string nhs = dt.Rows[i]["nhs"].ToString();
                            if (nhs.Trim().Length != 0)
                            {
                                totalnhs += Convert.ToSingle(nhs);
                            }
                            string npots = dt.Rows[i]["npots"].ToString();
                            if (npots.Trim().Length != 0)
                            {
                                totalnpots += Convert.ToSingle(npots);
                            }
                            string ntempgross = dt.Rows[i]["tempgross"].ToString();
                            if (ntempgross.Trim().Length != 0)
                            {
                                totaltempgross += Convert.ToSingle(ntempgross);
                            }

                            string strBasic = dt.Rows[i]["Basic"].ToString();
                            if (strBasic.Trim().Length != 0)
                            {
                                totalBasic += Convert.ToSingle(strBasic);
                            }
                            string strDA = dt.Rows[i]["DA"].ToString();
                            if (strDA.Trim().Length != 0)
                            {
                                totalDA += Convert.ToSingle(strDA);
                            }
                            string strhHRA = dt.Rows[i]["HRA"].ToString();
                            if (strhHRA.Trim().Length != 0)
                            {
                                totalHRA += Convert.ToSingle(strhHRA);
                            }
                            string strCCA = dt.Rows[i]["CCA"].ToString();
                            if (strCCA.Trim().Length != 0)
                            {
                                totalCCA += Convert.ToSingle(strCCA);
                            }
                            string strConveyance = dt.Rows[i]["Conveyance"].ToString();
                            if (strConveyance.Trim().Length != 0)
                            {
                                totalConveyance += Convert.ToSingle(strConveyance);
                            }
                            string strWA = dt.Rows[i]["WashAllowance"].ToString();
                            if (strWA.Trim().Length != 0)
                            {
                                totalWA += Convert.ToSingle(strWA);
                            }
                            string strOA = dt.Rows[i]["OtherAllowance"].ToString();
                            if (strOA.Trim().Length != 0)
                            {
                                totalOA += Convert.ToSingle(strOA);
                            }
                            string strLeaveEncashAmt = dt.Rows[i]["LeaveEncashAmt"].ToString();
                            if (strCCA.Trim().Length != 0)
                            {
                                totalLeaveEncashAmt += Convert.ToSingle(strLeaveEncashAmt);
                            }
                            string strGratuity = dt.Rows[i]["Gratuity"].ToString();
                            if (strGratuity.Trim().Length != 0)
                            {
                                totalGratuity += Convert.ToSingle(strGratuity);
                            }
                            string strBonus = dt.Rows[i]["Bonus"].ToString();
                            if (strBonus.Trim().Length != 0)
                            {
                                totalBonus += Convert.ToSingle(strBonus);
                            }
                            string strAttBonus = dt.Rows[i]["AttBonus"].ToString();
                            if (strAttBonus.Trim().Length != 0)
                            {
                                totalAttBonus += Convert.ToSingle(strAttBonus);
                            }

                            string strProfAllowance = dt.Rows[i]["Profallowance"].ToString();
                            if (strProfAllowance.Trim().Length != 0)
                            {
                                totalProfAllowance += Convert.ToSingle(strProfAllowance);
                            }

                            string strUniformAllw = dt.Rows[i]["UniformAllw"].ToString();
                            if (strUniformAllw.Trim().Length != 0)
                            {
                                totalUniformAllw += Convert.ToSingle(strUniformAllw);
                            }

                           
                           
                            
                            string strNfhs = dt.Rows[i]["Nfhs"].ToString();
                            if (strNfhs.Trim().Length != 0)
                            {
                                totalnfhs += Convert.ToSingle(strNfhs);
                            }

                            string strGross = dt.Rows[i]["Gross"].ToString();
                            if (strGross.Trim().Length != 0)
                            {
                                totalGrass += Convert.ToSingle(strGross);
                            }


                            string strIncentivs = dt.Rows[i]["Incentivs"].ToString();
                            if (strIncentivs.Trim().Length != 0)
                            {
                                totalIncentivs += Convert.ToSingle(strIncentivs);
                            }

                            string strOTAmount = dt.Rows[i]["OTAmt"].ToString();
                            if (strOTAmount.Trim().Length != 0)
                            {
                                totalOTAmount += Convert.ToSingle(strOTAmount);
                            }
                            string strPF = dt.Rows[i]["PF"].ToString();
                            if (strPF.Trim().Length != 0)
                            {
                                totalPF += Convert.ToSingle(strPF);
                            }
                            string strESI = dt.Rows[i]["ESI"].ToString();
                            if (strESI.Trim().Length != 0)
                            {
                                totalESI += Convert.ToSingle(strESI);
                            }
                            string strPT = dt.Rows[i]["Proftax"].ToString();
                            if (strPT.Trim().Length != 0)
                            {
                                totalProfTax += Convert.ToSingle(strPT);
                            }

                            string strDed = dt.Rows[i]["TotalDeductions"].ToString();
                            if (strDed.Trim().Length != 0)
                            {
                                totalDed += Convert.ToSingle(strDed);
                            }


                            string strSplAllow = dt.Rows[i]["SpecialAllw"].ToString();
                            if (strSplAllow.Trim().Length != 0)
                            {
                                totalSplAllw += Convert.ToSingle(strSplAllow);
                            }

                            string strNhsAmt = dt.Rows[i]["Nhsamt"].ToString();
                            if (strNhsAmt.Trim().Length != 0)
                            {
                                totalNhsAmt += Convert.ToSingle(strNhsAmt);
                            }

                            string strRC = dt.Rows[i]["RC"].ToString();
                            if (strRC.Trim().Length != 0)
                            {
                                totalRC += Convert.ToSingle(strRC);
                            }

                            string strCS = dt.Rows[i]["CS"].ToString();
                            if (strCS.Trim().Length != 0)
                            {
                                totalCS += Convert.ToSingle(strCS);
                            }

                            string strTotalEarnings = dt.Rows[i]["TotalEarnings"].ToString();
                            if (strTotalEarnings.Trim().Length != 0)
                            {
                                totalTotalEarnings += Convert.ToSingle(strTotalEarnings);
                            }



                        }
                    }



                    Label tot = GVListEmployees.FooterRow.FindControl("lblTotalNetAmount") as Label;
                    tot.Text = Math.Round(totalActualamount).ToString();

                    Label lblTotalDuties = GVListEmployees.FooterRow.FindControl("lblTotalDuties") as Label;
                    lblTotalDuties.Text = Math.Round(totalDuties).ToString();



                    Label lblTotalOTs = GVListEmployees.FooterRow.FindControl("lblTotalOTs") as Label;
                    lblTotalOTs.Text = Math.Round(totalOts).ToString();

                    if (totalOts != 0)
                    {
                        GVListEmployees.Columns[8].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[8].Visible = false;

                    }

                    Label lblTotalwos = GVListEmployees.FooterRow.FindControl("lblTotalwos") as Label;
                    lblTotalwos.Text = Math.Round(totalwo).ToString();

                    if (totalwo != 0)
                    {
                        GVListEmployees.Columns[9].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[9].Visible = false;

                    }

                    Label lblTotalNhs = GVListEmployees.FooterRow.FindControl("lblTotalNhs") as Label;
                    lblTotalNhs.Text = Math.Round(totalnhs).ToString();

                    if (totalnhs != 0)
                    {
                        GVListEmployees.Columns[10].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[10].Visible = false;

                    }



                    Label lblTotalNpots = GVListEmployees.FooterRow.FindControl("lblTotalNpots") as Label;
                    lblTotalNpots.Text = Math.Round(totalnpots).ToString();

                    if (totalnpots != 0)
                    {
                        GVListEmployees.Columns[11].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[11].Visible = false;

                    }

                    Label lblTotaltempgross = GVListEmployees.FooterRow.FindControl("lblTotaltempgross") as Label;
                    lblTotaltempgross.Text = Math.Round(totaltempgross).ToString();

                    Label lblTotalBasic = GVListEmployees.FooterRow.FindControl("lblTotalBasic") as Label;
                    lblTotalBasic.Text = Math.Round(totalBasic).ToString();

                    Label lblTotalDA = GVListEmployees.FooterRow.FindControl("lblTotalDA") as Label;
                    lblTotalDA.Text = Math.Round(totalDA).ToString();

                    if (totalDA != 0)
                    {
                        GVListEmployees.Columns[14].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[14].Visible = false;

                    }

                    Label lblTotalHRA = GVListEmployees.FooterRow.FindControl("lblTotalHRA") as Label;
                    lblTotalHRA.Text = Math.Round(totalHRA).ToString();

                    if (totalHRA != 0)
                    {
                        GVListEmployees.Columns[15].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[15].Visible = false;

                    }

                    Label lblTotalCCA = GVListEmployees.FooterRow.FindControl("lblTotalCCA") as Label;
                    lblTotalCCA.Text = Math.Round(totalCCA).ToString();

                    if (totalCCA != 0)
                    {
                        GVListEmployees.Columns[16].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[16].Visible = false;

                    }

                    Label lblTotalConveyance = GVListEmployees.FooterRow.FindControl("lblTotalConveyance") as Label;
                    lblTotalConveyance.Text = Math.Round(totalConveyance).ToString();

                    if (totalConveyance != 0)
                    {
                        GVListEmployees.Columns[17].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[17].Visible = false;

                    }

                    Label lblTotalWA = GVListEmployees.FooterRow.FindControl("lblTotalWA") as Label;
                    lblTotalWA.Text = Math.Round(totalWA).ToString();

                    if (totalWA != 0)
                    {
                        GVListEmployees.Columns[18].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[18].Visible = false;

                    }

                    Label lblTotalOA = GVListEmployees.FooterRow.FindControl("lblTotalOA") as Label;
                    lblTotalOA.Text = Math.Round(totalOA).ToString();

                    if (totalOA != 0)
                    {
                        GVListEmployees.Columns[19].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[19].Visible = false;

                    }


                    Label lblTotalLeaveEncashAmt = GVListEmployees.FooterRow.FindControl("lblTotalLeaveEncashAmt") as Label;
                    lblTotalLeaveEncashAmt.Text = Math.Round(totalLeaveEncashAmt).ToString();

                    if (totalLeaveEncashAmt != 0)
                    {
                        GVListEmployees.Columns[20].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[20].Visible = false;

                    }
                    Label lblTotalGratuity = GVListEmployees.FooterRow.FindControl("lblTotalGratuity") as Label;
                    lblTotalGratuity.Text = Math.Round(totalGratuity).ToString();

                    if (totalGratuity != 0)
                    {
                        GVListEmployees.Columns[21].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[21].Visible = false;

                    }

                    Label lblTotalBonus = GVListEmployees.FooterRow.FindControl("lblTotalBonus") as Label;
                    lblTotalBonus.Text = Math.Round(totalBonus).ToString();


                    if (totalBonus != 0)
                    {
                        GVListEmployees.Columns[22].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[22].Visible = false;

                    }
                    Label lblTotalAttBonus = GVListEmployees.FooterRow.FindControl("lblTotalAttBonus") as Label;
                    lblTotalAttBonus.Text = Math.Round(totalAttBonus).ToString();

                    if (totalAttBonus != 0)
                    {
                        GVListEmployees.Columns[23].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[23].Visible = false;

                    }
                    Label lblTotalNfhs = GVListEmployees.FooterRow.FindControl("lblTotalNfhs") as Label;
                    lblTotalNfhs.Text = Math.Round(totalnfhs).ToString();

                    if (totalnfhs != 0)
                    {
                        GVListEmployees.Columns[24].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[24].Visible = false;

                    }
                    Label lblTotalrc = GVListEmployees.FooterRow.FindControl("lblTotalrc") as Label;
                    lblTotalrc.Text = Math.Round(totalRC).ToString();

                    if (totalRC != 0)
                    {
                        GVListEmployees.Columns[25].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[25].Visible = false;

                    }

                    Label lblTotalcs = GVListEmployees.FooterRow.FindControl("lblTotalcs") as Label;
                    lblTotalcs.Text = Math.Round(totalCS).ToString();

                    if (totalCS != 0)
                    {
                        GVListEmployees.Columns[26].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[26].Visible = false;

                    }

                    Label lblTotalSplAllw = GVListEmployees.FooterRow.FindControl("lblTotalSplAllw") as Label;
                    lblTotalSplAllw.Text = Math.Round(totalSplAllw).ToString();

                    if (totalSplAllw != 0)
                    {
                        GVListEmployees.Columns[27].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[27].Visible = false;

                    }


                    Label lblTotalGross = GVListEmployees.FooterRow.FindControl("lblTotalGross") as Label;
                    lblTotalGross.Text = Math.Round(totalGrass).ToString();

                    Label lblTotalProfAllw = GVListEmployees.FooterRow.FindControl("lblTotalProfAllw") as Label;
                    lblTotalProfAllw.Text = Math.Round(totalProfallowance).ToString();

                    if (totalProfallowance != 0)
                    {
                        GVListEmployees.Columns[28].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[28].Visible = false;
                    }
                    

                    Label lblTotalUniformAllw = GVListEmployees.FooterRow.FindControl("lblTotalUniformAllw") as Label;
                    lblTotalUniformAllw.Text = Math.Round(totalUniformAllw).ToString();

                    if (totalUniformAllw != 0)
                    {
                        GVListEmployees.Columns[32].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[32].Visible = false;
                    }

                   
                    
                    Label lblTotalNHsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNHsAmount") as Label;
                    lblTotalNHsAmount.Text = Math.Round(totalNhsAmt).ToString();

                    if (totalNhsAmt != 0)
                    {
                        GVListEmployees.Columns[30].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[30].Visible = false;

                    }

                    Label lblTotalOTAmount = GVListEmployees.FooterRow.FindControl("lblTotalOTAmount") as Label;
                    lblTotalOTAmount.Text = Math.Round(totalOTAmount).ToString();

                    if (totalOTAmount != 0)
                    {
                        GVListEmployees.Columns[29].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[29].Visible = false;

                    }


                    Label lblTotalTotalEarnings = GVListEmployees.FooterRow.FindControl("lblTotalTotalEarnings") as Label;
                    lblTotalTotalEarnings.Text = Math.Round(totalTotalEarnings).ToString();

                    if (totalTotalEarnings != 0)
                    {
                        GVListEmployees.Columns[33].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[33].Visible = false;

                    }

                    Label lblTotalPF = GVListEmployees.FooterRow.FindControl("lblTotalPF") as Label;
                    lblTotalPF.Text = Math.Round(totalPF).ToString();

                    Label lblTotalESI = GVListEmployees.FooterRow.FindControl("lblTotalESI") as Label;
                    lblTotalESI.Text = Math.Round(totalESI).ToString();

                    Label lblTotalPT = GVListEmployees.FooterRow.FindControl("lblTotalPT") as Label;
                    lblTotalPT.Text = Math.Round(totalProfTax).ToString();


                    Label lblTotalDeductions = GVListEmployees.FooterRow.FindControl("lblTotalDeductions") as Label;
                    lblTotalDeductions.Text = Math.Round(totalDed).ToString();

                    //New code add as on 24/12/2013 by venkat
                }
                else
                {
                    GVListEmployees.DataSource = null;
                    GVListEmployees.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Salary Details For The Selected client');", true);

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GvUtill.ExportGrid("WageSheetReport.xls", this.hidGridView);
        }


        float totalActualamount = 0;
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
        float totalCanteenAdv = 0;
        float totalLeaveEncashAmt = 0;
        float totalGratuity = 0;
        float totalBonus = 0;
        float totalAttBonus = 0;
        float totalMedicalAllowance = 0;
        float totalTravelAllw = 0;
        float totalPerformanceAllw = 0;
        float totalShift1Amt = 0;
        float totalShift2Amt = 0;
        float totalNightshiftAmt = 0;
        float totalAPA = 0;
        float totalnfhs = 0;
        float totalDed = 0;
        float totalOtherDed = 0;
        float totalIncentivs = 0;
        float totalWoAmt = 0;
        float totalNhsAmt = 0;
        float totalNpotsAmt = 0;
        float totalPenalty = 0;
        float totalSplAllw = 0;
        float totalConvAllw = 0;
        float totalMobileAllw = 0;
        float totalRC = 0;
        float totalCS = 0;
        float totalOWF = 0;
        float totalSecDepDed = 0;
        float totalRoomRentDed = 0;
        float totalGenDed = 0;
        float totalProfallowance = 0;
        float totalUniformAllw = 0;
        float totalProfAllowance = 0;
        float totalTotalEarnings = 0;

        protected void DisplayData()
        {
            if (ddlclientid.SelectedIndex > 0)
            {
                try
                {

                    string date = string.Empty;

                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }

                    string month = DateTime.Parse(date).Month.ToString();
                    string Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
                    string sqlqry = string.Empty;

                    string clientid = ddlclientid.SelectedValue;

                    if (ddlclientid.SelectedIndex == 1)
                    {
                        clientid = "%";
                    }

                    var SPName = "ArrearWagesheet";
                    Hashtable ht = new Hashtable();
                    ht.Add("@month", month + Year);
                    ht.Add("@clientid", clientid);
                    Bindata(SPName, ht);

                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}