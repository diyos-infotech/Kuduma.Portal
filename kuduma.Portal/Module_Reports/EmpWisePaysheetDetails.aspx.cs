using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class EmpWisePaysheetDetails : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
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

                    if (this.Master != null)
                    {
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli1");
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
            BranchID = Session["BranchID"].ToString();
        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            GetEmpName();
        }


        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where empid='" + txtEmpid.Text + "' and empid like '%" + EmpIDPrefix + "%' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtName.Text = dt.Rows[0]["empname"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }


        }


        protected void GetEmpid()
        {

            #region  Old Code
            string Sqlqry = "select Empid from empdetails where (empfname+' '+empmname+' '+emplname)  like '" + txtName.Text + "' and empid like '%" + EmpIDPrefix + "%' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["Empid"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }
            #endregion // End Old Code


        }


        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            GetEmpid();
        }



        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            DisplayData();
        }



        protected void Bindata(string Sqlqry)
        {
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
                // Fillpfandesidetails();
                //LinkButton1.Visible = true;            lbtn_Export.Visible = true;

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
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            //if (ddlclientid.SelectedIndex == 1)
            {
                gve.ExportGrid("Netpay-(" + txtmonth.Text + ")" + ".xls", hidGridView);
            }
            //// else
            // {
            //     GridViewExportUtil.ExportGrid(ddlcname.SelectedItem.Text + "(" + ddlclientid.SelectedValue + ")" + ".xls", hidGridView);
            // }
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


            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString();
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
        #region variables
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
        #endregion

        protected void DisplayData()
        {
            if (txtEmpid.Text.Length > 0)
            {
                try
                {

                    string date = string.Empty;

                    if (txtmonth.Text.Trim().Length > 0)
                    {
                        date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                    }

                    string month = DateTime.Parse(date).Month.ToString();
                    string Year = DateTime.Parse(date).Year.ToString();
                    string sqlqry = string.Empty;

                    sqlqry = "select  Eps.EmpId,D.Design as Desgn,eps.clientid,c.clientname,ISNULL( Eps.NoOfDuties,0) as NoOfDuties,ISNULL( Eps.ots,0) as ots,ISNULL( Eps.WO,0) as WO,  " +
                                "ISNULL( Eps.TempGross,0) as TempGross, ISNULL( Eps.NHS,0) as NHS,ISNULL( Eps.npots,0) as npots,ISNULL( Eps.Basic,0) as Basic,   " +
                                "ISNULL( Eps.DA,0) as DA,ISNULL( Eps.HRA,0) as HRA,ISNULL( Eps.CCA,0) as CCA,ISNULL( Eps.Conveyance,0) as Conveyance,ISNULL(Eps.TempGross, 0) as TempGross,ISNULL( Eps.WashAllowance,0) as WashAllowance, " +
                                "ISNULL( Eps.OtherAllowance,0) as OtherAllowance, ISNULL( Eps.PF,0) as PF,ISNULL( Eps.ESI,0) as ESI,ISNULL( Eps.Incentivs,0) as Incentivs, " +
                                "ISNULL( Eps.OTAmt,0) as OTAmt,ISNULL( Eps.ProfTax,0) as ProfTax,ISNULL( Eps.LeaveEncashAmt,0) as LeaveEncashAmt,ISNULL( Eps.Npotsamt,0) as Npotsamt,  " +
                                "ISNULL( Eps.Gratuity,0) as Gratuity,ISNULL( Eps.Bonus,0) as Bonus,ISNULL( Eps.NFHS,0) as NFHS,  " +
                                "ISNULL( Eps.RC,0) as RC,ISNULL( Eps.cs,0) as cs,ISNULL( Eps.SalAdvDed,0) as SalAdvDed,ISNULL( Eps.ADVDed,0) as ADVDed,ISNULL( Eps.WCDed,0) as WCDed,ISNULL( Eps.CanteenAdv,0) as CanteenAdv,ISNULL(Eps.LoanDed,0) as LoanDed ,ISNULL( Eps.UniformDed,0) as UniformDed, ISNULL( Eps.SecurityDepDed,0) as SecurityDepDed, ISNULL( Eps.LoanDed,0) as LoanDed," +
                                "ISNULL( Eps.GeneralDed,0) as GeneralDed,  ISNULL( Eps.Penalty,0) as Penalty,ISNULL( Eps.OtherDed,0) as OtherDed,ISNULL( Eps.Gross,0) as Gross,   " +
                                "(EPS.TotalDeductions) as TotalDeductions,eps.actualamount as ActualAmount,ISNULL( Eps.OWF,0) as OWF,   " +
                                "ISNULL( Eps.OWFAmt,0) as OWFAmt,ISNULL( Eps.WOAmt,0) as WOAmt, ISNULL( Eps.Nhsamt,0) as Nhsamt, ISNULL( Eps.Npotsamt,0) as Npotsamt, " +
                                "isnull(empdetails.EmpfName,'')+' ' + isnull(empdetails.EmpmName,'')+' '+isnull(empdetails.EmplName,'')  as EmpmName,EmpDetails.EmpIFSCcode,( rtrim(ltrim(EmpDetails.Empbankacno)) ) as Empbankacno,EmpDetails.EmpBankCardRef," +
                                "(select b.Bankname  from BankNames b where b.BankId=EmpDetails.Empbankname) as empbankname from  emppaysheet EPs" +
                                  " inner join Clients C on  C.Clientid = EPs.Clientid " +
                                  " inner join  empdetails  on empdetails.Empid=EPs.Empid   " +
                                  " inner join Designations d on EPs.Desgn=d.DesignId " +
                                  "  and  (EPs.noofduties+eps.ots+eps.wo+eps.nhs+eps.npots)<>0  and  EPs.empid='" + txtEmpid.Text + "'  and EPs.month='" + month + Year.Substring(2, 2) + " ' order By   Right(EPs.Clientid,4),   EPs.empid ";

                    DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
                    if (dt.Rows.Count > 0)
                    {
                        GVListEmployees.DataSource = dt;
                        GVListEmployees.DataBind();
                        lbtn_Export.Visible = true;




                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            float actAmount = 0;
                            string actualAmount = dt.Rows[i]["ActualAmount"].ToString();
                            if (actualAmount.Trim().Length > 0)
                            {
                                actAmount = Convert.ToSingle(actualAmount);
                            }
                            if (actAmount >= 0)
                            {
                                totalActualamount += actAmount;
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
                                string npots = dt.Rows[i]["npots"].ToString();
                                if (npots.Trim().Length > 0)
                                {
                                    totalnpots += Convert.ToSingle(npots);
                                }
                                string ntempgross = dt.Rows[i]["tempgross"].ToString();
                                if (ntempgross.Trim().Length > 0)
                                {
                                    totaltempgross += Convert.ToSingle(ntempgross);
                                }

                                string strBasic = dt.Rows[i]["Basic"].ToString();
                                if (strBasic.Trim().Length > 0)
                                {
                                    totalBasic += Convert.ToSingle(strBasic);
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
                                string strOA = dt.Rows[i]["OtherAllowance"].ToString();
                                if (strOA.Trim().Length > 0)
                                {
                                    totalOA += Convert.ToSingle(strOA);
                                }


                                string strLeaveEncashAmt = dt.Rows[i]["LeaveEncashAmt"].ToString();
                                if (strCCA.Trim().Length > 0)
                                {
                                    totalLeaveEncashAmt += Convert.ToSingle(strLeaveEncashAmt);
                                }
                                string strGratuity = dt.Rows[i]["Gratuity"].ToString();
                                if (strGratuity.Trim().Length > 0)
                                {
                                    totalGratuity += Convert.ToSingle(strGratuity);
                                }
                                string strBonus = dt.Rows[i]["Bonus"].ToString();
                                if (strBonus.Trim().Length > 0)
                                {
                                    totalBonus += Convert.ToSingle(strBonus);
                                }
                                string strNfhs = dt.Rows[i]["Nfhs"].ToString();
                                if (strNfhs.Trim().Length > 0)
                                {
                                    totalnfhs += Convert.ToSingle(strNfhs);
                                }

                                string strGross = dt.Rows[i]["Gross"].ToString();
                                if (strGross.Trim().Length > 0)
                                {
                                    totalGrass += Convert.ToSingle(strGross);
                                }


                                string strIncentivs = dt.Rows[i]["Incentivs"].ToString();
                                if (strIncentivs.Trim().Length > 0)
                                {
                                    totalIncentivs += Convert.ToSingle(strIncentivs);
                                }

                                string strOTAmount = dt.Rows[i]["OTAmt"].ToString();
                                if (strOTAmount.Trim().Length > 0)
                                {
                                    totalOTAmount += Convert.ToSingle(strOTAmount);
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
                                string strCanteenAdv = dt.Rows[i]["CanteenAdv"].ToString();
                                if (strCanteenAdv.Trim().Length > 0)
                                {
                                    totalCanteenAdv += Convert.ToSingle(strCanteenAdv);
                                }

                                string strDed = dt.Rows[i]["TotalDeductions"].ToString();
                                if (strDed.Trim().Length > 0)
                                {
                                    totalDed += Convert.ToSingle(strDed);
                                }


                                //New code add as on 24/12/2013 by venkat

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

                                string strNpotsAmt = dt.Rows[i]["Npotsamt"].ToString();
                                if (strNpotsAmt.Trim().Length > 0)
                                {
                                    totalNpotsAmt += Convert.ToSingle(strNpotsAmt);
                                }

                                string strPenalty = dt.Rows[i]["Penalty"].ToString();
                                if (strPenalty.Trim().Length > 0)
                                {
                                    totalPenalty += Convert.ToSingle(strPenalty);
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
                                }

                                string strOWF = dt.Rows[i]["OWF"].ToString();
                                if (strOWF.Trim().Length > 0)
                                {
                                    totalOWF += Convert.ToSingle(strOWF);
                                }

                                string strSecDep = dt.Rows[i]["SecurityDepDed"].ToString();
                                if (strSecDep.Trim().Length > 0)
                                {
                                    totalSecDepDed += Convert.ToSingle(strSecDep);
                                }

                                string strLoanDed = dt.Rows[i]["LoanDed"].ToString();
                                if (strLoanDed.Trim().Length > 0)
                                {
                                    totalloanded += Convert.ToSingle(strLoanDed);
                                }

                                string strGeneralDed = dt.Rows[i]["GeneralDed"].ToString();
                                if (strGeneralDed.Trim().Length > 0)
                                {
                                    totalGenDed += Convert.ToSingle(strGeneralDed);
                                }

                            }
                        }



                        Label tot = GVListEmployees.FooterRow.FindControl("lblTotalNetAmount") as Label;
                        tot.Text = Math.Round(totalActualamount).ToString();

                        Label lblTotalDuties = GVListEmployees.FooterRow.FindControl("lblTotalDuties") as Label;
                        lblTotalDuties.Text = Math.Round(totalDuties).ToString();



                        Label lblTotalOTs = GVListEmployees.FooterRow.FindControl("lblTotalOTs") as Label;
                        lblTotalOTs.Text = Math.Round(totalOts).ToString();

                        if (totalOts > 0)
                        {
                            GVListEmployees.Columns[8].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[8].Visible = false;

                        }
                        Label lblTotalwos = GVListEmployees.FooterRow.FindControl("lblTotalwos") as Label;

                        lblTotalwos.Text = Math.Round(totalwo).ToString();

                        if (totalwo > 0)
                        {
                            GVListEmployees.Columns[9].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[9].Visible = false;

                        }

                        Label lblTotalNhs = GVListEmployees.FooterRow.FindControl("lblTotalNhs") as Label;
                        lblTotalNhs.Text = Math.Round(totalnhs).ToString();

                        if (totalnhs > 0)
                        {
                            GVListEmployees.Columns[10].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[10].Visible = false;

                        }



                        Label lblTotalNpots = GVListEmployees.FooterRow.FindControl("lblTotalNpots") as Label;
                        lblTotalNpots.Text = Math.Round(totalnpots).ToString();

                        if (totalnpots > 0)
                        {
                            GVListEmployees.Columns[11].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[11].Visible = false;

                        }

                        Label lblTotaltempgross = GVListEmployees.FooterRow.FindControl("lblTotaltempgross") as Label;
                        lblTotaltempgross.Text = Math.Round(totaltempgross).ToString();
                        if (totaltempgross > 0)
                        {
                            GVListEmployees.Columns[12].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[12].Visible = false;

                        }
                        Label lblTotalBasic = GVListEmployees.FooterRow.FindControl("lblTotalBasic") as Label;
                        lblTotalBasic.Text = Math.Round(totalBasic).ToString();
                        if (totalBasic > 0)
                        {
                            GVListEmployees.Columns[13].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[13].Visible = false;

                        }
                        Label lblTotalDA = GVListEmployees.FooterRow.FindControl("lblTotalDA") as Label;
                        lblTotalDA.Text = Math.Round(totalDA).ToString();

                        if (totalDA > 0)
                        {
                            GVListEmployees.Columns[14].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[14].Visible = false;

                        }

                        Label lblTotalHRA = GVListEmployees.FooterRow.FindControl("lblTotalHRA") as Label;
                        lblTotalHRA.Text = Math.Round(totalHRA).ToString();

                        if (totalHRA > 0)
                        {
                            GVListEmployees.Columns[15].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[15].Visible = false;

                        }

                        Label lblTotalCCA = GVListEmployees.FooterRow.FindControl("lblTotalCCA") as Label;
                        lblTotalCCA.Text = Math.Round(totalCCA).ToString();

                        if (totalCCA > 0)
                        {
                            GVListEmployees.Columns[16].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[16].Visible = false;

                        }

                        Label lblTotalConveyance = GVListEmployees.FooterRow.FindControl("lblTotalConveyance") as Label;
                        lblTotalConveyance.Text = Math.Round(totalConveyance).ToString();

                        if (totalConveyance > 0)
                        {
                            GVListEmployees.Columns[17].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[17].Visible = false;

                        }

                        Label lblTotalWA = GVListEmployees.FooterRow.FindControl("lblTotalWA") as Label;
                        lblTotalWA.Text = Math.Round(totalWA).ToString();

                        if (totalWA > 0)
                        {
                            GVListEmployees.Columns[18].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[18].Visible = false;

                        }

                        Label lblTotalOA = GVListEmployees.FooterRow.FindControl("lblTotalOA") as Label;
                        lblTotalOA.Text = Math.Round(totalOA).ToString();

                        if (totalOA > 0)
                        {
                            GVListEmployees.Columns[19].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[19].Visible = false;

                        }


                        Label lblTotalLeaveEncashAmt = GVListEmployees.FooterRow.FindControl("lblTotalLeaveEncashAmt") as Label;
                        lblTotalLeaveEncashAmt.Text = Math.Round(totalLeaveEncashAmt).ToString();

                        if (totalLeaveEncashAmt > 0)
                        {
                            GVListEmployees.Columns[20].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[20].Visible = false;

                        }
                        Label lblTotalGratuity = GVListEmployees.FooterRow.FindControl("lblTotalGratuity") as Label;
                        lblTotalGratuity.Text = Math.Round(totalGratuity).ToString();

                        if (totalGratuity > 0)
                        {
                            GVListEmployees.Columns[21].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[21].Visible = false;

                        }

                        Label lblTotalBonus = GVListEmployees.FooterRow.FindControl("lblTotalBonus") as Label;
                        lblTotalBonus.Text = Math.Round(totalBonus).ToString();


                        if (totalBonus > 0)
                        {
                            GVListEmployees.Columns[22].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[22].Visible = false;

                        }

                        Label lblTotalNfhs = GVListEmployees.FooterRow.FindControl("lblTotalNfhs") as Label;
                        lblTotalNfhs.Text = Math.Round(totalnfhs).ToString();

                        if (totalnfhs > 0)
                        {
                            GVListEmployees.Columns[23].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[23].Visible = false;

                        }

                        Label lblTotalrc = GVListEmployees.FooterRow.FindControl("lblTotalrc") as Label;
                        lblTotalrc.Text = Math.Round(totalRC).ToString();

                        if (totalRC > 0)
                        {
                            GVListEmployees.Columns[24].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[24].Visible = false;

                        }

                        Label lblTotalcs = GVListEmployees.FooterRow.FindControl("lblTotalcs") as Label;
                        lblTotalcs.Text = Math.Round(totalCS).ToString();

                        if (totalCS > 0)
                        {
                            GVListEmployees.Columns[25].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[25].Visible = false;

                        }


                        Label lblTotalIncentivs = GVListEmployees.FooterRow.FindControl("lblTotalIncentivs") as Label;
                        lblTotalIncentivs.Text = Math.Round(totalIncentivs).ToString();
                        if (totalIncentivs > 0)
                        {
                            GVListEmployees.Columns[26].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[26].Visible = false;

                        }
                        Label lblTotalWOAmount = GVListEmployees.FooterRow.FindControl("lblTotalWOAmount") as Label;
                        lblTotalWOAmount.Text = Math.Round(totalWoAmt).ToString();

                        if (totalWoAmt > 0)
                        {
                            GVListEmployees.Columns[27].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[27].Visible = false;

                        }

                        Label lblTotalNhsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNhsAmount") as Label;
                        lblTotalNhsAmount.Text = Math.Round(totalNhsAmt).ToString();

                        if (totalNhsAmt > 0)
                        {
                            GVListEmployees.Columns[28].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[28].Visible = false;

                        }


                        Label lblTotalNpotsAmount = GVListEmployees.FooterRow.FindControl("lblTotalNpotsAmount") as Label;
                        lblTotalNpotsAmount.Text = Math.Round(totalNpotsAmt).ToString();

                        if (totalNpotsAmt > 0)
                        {
                            GVListEmployees.Columns[29].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[29].Visible = false;

                        }

                        Label lblTotalGross = GVListEmployees.FooterRow.FindControl("lblTotalGross") as Label;
                        lblTotalGross.Text = Math.Round(totalGrass).ToString();
                        if (totalGrass > 0)
                        {
                            GVListEmployees.Columns[30].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[30].Visible = false;

                        }

                        Label lblTotalOTAmount = GVListEmployees.FooterRow.FindControl("lblTotalOTAmount") as Label;
                        lblTotalOTAmount.Text = Math.Round(totalOTAmount).ToString();

                        if (totalOTAmount > 0)
                        {
                            GVListEmployees.Columns[31].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[31].Visible = false;

                        }


                        Label lblTotalPF = GVListEmployees.FooterRow.FindControl("lblTotalPF") as Label;
                        lblTotalPF.Text = Math.Round(totalPF).ToString();



                        Label lblTotalESI = GVListEmployees.FooterRow.FindControl("lblTotalESI") as Label;
                        lblTotalESI.Text = Math.Round(totalESI).ToString();

                        Label lblTotalProfTax = GVListEmployees.FooterRow.FindControl("lblTotalProfTax") as Label;
                        lblTotalProfTax.Text = Math.Round(totalProfTax).ToString();
                        if (totalProfTax > 0)
                        {
                            GVListEmployees.Columns[34].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[34].Visible = false;

                        }


                        Label lblTotalsaladv = GVListEmployees.FooterRow.FindControl("lblTotalsaladv") as Label;
                        lblTotalsaladv.Text = Math.Round(totalSalAdv).ToString();

                        if (totalSalAdv > 0)
                        {
                            GVListEmployees.Columns[35].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[35].Visible = false;

                        }

                        Label lblTotaladvded = GVListEmployees.FooterRow.FindControl("lblTotaladvded") as Label;
                        lblTotaladvded.Text = Math.Round(totalAdvDed).ToString();
                        if (totalAdvDed > 0)
                        {
                            GVListEmployees.Columns[36].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[36].Visible = false;

                        }

                        Label lblTotalwcded = GVListEmployees.FooterRow.FindControl("lblTotalwcded") as Label;
                        lblTotalwcded.Text = Math.Round(totalWCDed).ToString();
                        if (totalWCDed > 0)
                        {
                            GVListEmployees.Columns[37].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[37].Visible = false;

                        }

                        Label lblTotalUniformDed = GVListEmployees.FooterRow.FindControl("lblTotalUniformDed") as Label;
                        lblTotalUniformDed.Text = Math.Round(totalUniformDed).ToString();

                        if (totalUniformDed > 0)
                        {
                            GVListEmployees.Columns[38].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[38].Visible = false;

                        }


                        Label lblTotalOtherDed = GVListEmployees.FooterRow.FindControl("lblTotalOtherDed") as Label;
                        lblTotalOtherDed.Text = Math.Round(totalOtherDed).ToString();

                        if (totalOtherDed > 0)
                        {
                            GVListEmployees.Columns[39].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[39].Visible = false;

                        }

                        Label lbltotalloanded = GVListEmployees.FooterRow.FindControl("lblTotaltotalloanded") as Label;
                        lbltotalloanded.Text = Math.Round(totalloanded).ToString();

                        if (totalloanded > 0)
                        {
                            GVListEmployees.Columns[40].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[40].Visible = false;

                        }

                        Label lblTotalCanteenAdv = GVListEmployees.FooterRow.FindControl("lblTotalcantadv") as Label;
                        lblTotalCanteenAdv.Text = Math.Round(totalCanteenAdv).ToString();

                        if (totalCanteenAdv > 0)
                        {
                            GVListEmployees.Columns[41].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[41].Visible = false;

                        }


                        Label lblTotalSecDepDed = GVListEmployees.FooterRow.FindControl("lblTotalSecDepDed") as Label;
                        lblTotalSecDepDed.Text = Math.Round(totalSecDepDed).ToString();


                        if (totalSecDepDed > 0)
                        {
                            GVListEmployees.Columns[42].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[42].Visible = false;

                        }



                        Label lblTotalGeneralDed = GVListEmployees.FooterRow.FindControl("lblTotalGeneralDed") as Label;
                        lblTotalGeneralDed.Text = Math.Round(totalGenDed).ToString();


                        if (totalGenDed > 0)
                        {
                            GVListEmployees.Columns[43].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[43].Visible = false;

                        }

                        Label lblTotalowf = GVListEmployees.FooterRow.FindControl("lblTotalowf") as Label;
                        lblTotalowf.Text = Math.Round(totalOWF).ToString();

                        if (totalOWF > 0)
                        {
                            GVListEmployees.Columns[44].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[44].Visible = false;

                        }

                        Label lblTotalPenalty = GVListEmployees.FooterRow.FindControl("lblTotalPenalty") as Label;
                        lblTotalPenalty.Text = Math.Round(totalPenalty).ToString();

                        if (totalPenalty > 0)
                        {
                            GVListEmployees.Columns[45].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[45].Visible = false;

                        }

                        Label lblTotalDeductions = GVListEmployees.FooterRow.FindControl("lblTotalDeductions") as Label;
                        lblTotalDeductions.Text = Math.Round(totalDed).ToString();
                        if (totalDed > 0)
                        {
                            GVListEmployees.Columns[46].Visible = true;
                        }
                        else
                        {
                            GVListEmployees.Columns[46].Visible = false;

                        }

                        //New code add as on 24/12/2013 by venkat




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

                e.Row.Cells[48].Attributes.Add("class", "text");



                ((Label)e.Row.FindControl("lblmonth")).Text = txtmonth.Text;
            }
        }




    }
}