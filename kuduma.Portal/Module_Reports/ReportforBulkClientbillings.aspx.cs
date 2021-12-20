using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class ReportforBulkClientbillings : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string BranchID = "";
        string FontStyle = "Tahoma";


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
                if (this.Master != null)
                {
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli2");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }

                //string strQry1 = "Select * from CompanyInfo ";
                //DataTable compInfo1 = config.ExecuteAdaptorAsyncWithQueryParams(strQry1).Result;
                //string Billprefix = compInfo1.Rows[0]["BillSeq"].ToString();
                //txtfrombillno.Text = "GDXF/" + Billprefix + "/";
                //txttobillno.Text = "GDXF/" + Billprefix + "/";

            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            DataTable DtClientList = null;
            Hashtable HtClientList = new Hashtable();

            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            gvFromtobills.DataSource = null;
            gvFromtobills.DataBind();

            int status = 0;
            var testDate = 0;

            var Month = "";
            var DateFrom = "";
            var DateTo = "";
            var SPName = "";

            string date = "";
            string month = "";
            string Year = "";

            string Fromdate = "";
            string Frommonth = "";
            string FromYear = "";

            string TOdate = "";
            string TOmonth = "";
            string TOYear = "";

            if (ddlMonthType.SelectedIndex == 0)
            {
                if (txtmonth.Text.Trim().Length == 0)
                {
                    LblResult.Text = "Please Select Month";
                    return;
                }


                if (txtmonth.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                    if (testDate > 0)
                    {
                        LblResult.Text = "You Are Entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }

                }
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString();

                Month = month + Year.Substring(2, 2);
            }

            if (ddlMonthType.SelectedIndex == 1)
            {
                if (txtfrom.Text.Trim().Length == 0)
                {
                    LblResult.Text = "Please Select From Month";
                    return;
                }

                if (txtfrom.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtfrom.Text);
                    if (testDate > 0)
                    {
                        LblResult.Text = "You Are Entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }

                }
                Fromdate = DateTime.Parse(txtfrom.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                Frommonth = DateTime.Parse(Fromdate).Month.ToString();
                if (Frommonth.Length == 1)
                {
                    Frommonth = "0" + Frommonth;
                }
                FromYear = DateTime.Parse(Fromdate).Year.ToString().Substring(2, 2);
                DateFrom = FromYear + Frommonth;

                if (txtto.Text.Trim().Length == 0)
                {
                    LblResult.Text = "Please Select To Month";
                    return;
                }
                if (txtto.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtto.Text);
                    if (testDate > 0)
                    {
                        LblResult.Text = "You Are Entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }

                }
                TOdate = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                TOmonth = DateTime.Parse(TOdate).Month.ToString();
                if (TOmonth.Length == 1)
                {
                    TOmonth = "0" + TOmonth;
                }
                TOYear = DateTime.Parse(TOdate).Year.ToString().Substring(2, 2);
                DateTo = TOYear + TOmonth;
            }
            status = ddlBillType.SelectedIndex;
            var Options = "Month";
            if (ddlMonthType.SelectedIndex == 1)
            {
                Options = "FromTo";
            }

            SPName = "GetClientsbulkbillprint";

            HtClientList.Add("@month", Month);
            HtClientList.Add("@DateFrom", DateFrom);
            HtClientList.Add("@DateTo", DateTo);
            HtClientList.Add("@status", status);
            HtClientList.Add("@Options", Options);

            DtClientList = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HtClientList);
            if (ddlMonthType.SelectedIndex == 0)
            {
                if (DtClientList.Rows.Count > 0)
                {
                    gvFromtobills.DataSource = null;
                    gvFromtobills.DataBind();
                    GVListEmployees.DataSource = DtClientList;
                    GVListEmployees.DataBind();


                    float TServiceTax = 0;
                    float TSBCESS = 0;
                    float TKKcess = 0;
                    float TCGST = 0;
                    float TSGST = 0;
                    float TIGST = 0;
                    for (int i = 0; i < DtClientList.Rows.Count; i++)
                    {
                        string ServiceTax = DtClientList.Rows[i]["ServiceTax"].ToString();
                        if (ServiceTax.Trim().Length > 0)
                        {
                            TServiceTax += Convert.ToSingle(ServiceTax);
                        }
                        string SBCESS = DtClientList.Rows[i]["SBCessAmt"].ToString();
                        if (SBCESS.Trim().Length > 0)
                        {
                            TSBCESS += Convert.ToSingle(SBCESS);
                        }
                        string KKcess = DtClientList.Rows[i]["kkcessamt"].ToString();
                        if (KKcess.Trim().Length > 0)
                        {
                            TKKcess += Convert.ToSingle(KKcess);
                        }

                        string CGST = DtClientList.Rows[i]["CGSTAmt"].ToString();
                        if (CGST.Trim().Length > 0)
                        {
                            TCGST += Convert.ToSingle(CGST);
                        }
                        string SGST = DtClientList.Rows[i]["CGSTAmt"].ToString();
                        if (SGST.Trim().Length > 0)
                        {
                            TSGST += Convert.ToSingle(SGST);
                        }
                        string IGST = DtClientList.Rows[i]["IGSTAmt"].ToString();
                        if (IGST.Trim().Length > 0)
                        {
                            TIGST += Convert.ToSingle(IGST);
                        }
                    }

                    Label lblTotalservicetax = GVListEmployees.FooterRow.FindControl("lblTotalservicetax") as Label;
                    lblTotalservicetax.Text = Math.Round(TServiceTax).ToString();
                    if (TServiceTax > 0)
                    {
                        GVListEmployees.Columns[4].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[4].Visible = false;

                    }

                    Label lblTotalSBCessAmt = GVListEmployees.FooterRow.FindControl("lblTotalSBCessAmt") as Label;
                    lblTotalSBCessAmt.Text = Math.Round(TSBCESS).ToString();
                    if (TSBCESS > 0)
                    {
                        GVListEmployees.Columns[5].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[5].Visible = false;

                    }

                    Label lblTotalkkcessamt = GVListEmployees.FooterRow.FindControl("lblTotalkkcessamt") as Label;
                    lblTotalkkcessamt.Text = Math.Round(TKKcess).ToString();
                    if (TKKcess > 0)
                    {
                        GVListEmployees.Columns[6].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[6].Visible = false;

                    }

                    Label lblTotalCGSTAmt = GVListEmployees.FooterRow.FindControl("lblTotalCGSTAmt") as Label;
                    lblTotalCGSTAmt.Text = Math.Round(TCGST).ToString();
                    if (TCGST > 0)
                    {
                        GVListEmployees.Columns[7].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[7].Visible = false;

                    }

                    Label lblTotalSGSTAmt = GVListEmployees.FooterRow.FindControl("lblTotalSGSTAmt") as Label;
                    lblTotalSGSTAmt.Text = Math.Round(TSGST).ToString();
                    if (TSGST > 0)
                    {
                        GVListEmployees.Columns[8].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[8].Visible = false;

                    }

                    Label lblTotalIGSTAmt = GVListEmployees.FooterRow.FindControl("lblTotalIGSTAmt") as Label;
                    lblTotalIGSTAmt.Text = Math.Round(TIGST).ToString();
                    if (TIGST > 0)
                    {
                        GVListEmployees.Columns[9].Visible = true;
                    }
                    else
                    {
                        GVListEmployees.Columns[9].Visible = false;

                    }
                }
            }
            else
            {
                if (DtClientList.Rows.Count > 0)
                {
                    GVListEmployees.DataSource = null;
                    GVListEmployees.DataBind();
                    gvFromtobills.DataSource = DtClientList;
                    gvFromtobills.DataBind();
                }
            }

        }

        protected void gvFromtobills_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var testDate = 0;

            var Month = "";
            var DateFrom = "";
            var DateTo = "";
            var SPName = "";

            string date = "";
            string month = "";
            string Year = "";

            string Fromdate = "";
            string Frommonth = "";
            string FromYear = "";

            string TOdate = "";
            string TOmonth = "";
            string TOYear = "";

            if (ddlMonthType.SelectedIndex == 1)
            {
                if (txtfrom.Text.Trim().Length == 0)
                {
                    LblResult.Text = "Please Select From Month";
                    return;
                }

                if (txtfrom.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtfrom.Text);
                    if (testDate > 0)
                    {
                        LblResult.Text = "You Are Entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }

                }
                Fromdate = DateTime.Parse(txtfrom.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                Frommonth = DateTime.Parse(Fromdate).Month.ToString();
                if (Frommonth.Length == 1)
                {
                    Frommonth = "0" + Frommonth;
                }
                FromYear = DateTime.Parse(Fromdate).Year.ToString().Substring(2, 2);
                DateFrom = FromYear + Frommonth;

                if (txtto.Text.Trim().Length == 0)
                {
                    LblResult.Text = "Please Select To Month";
                    return;
                }
                if (txtto.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtto.Text);
                    if (testDate > 0)
                    {
                        LblResult.Text = "You Are Entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                        return;
                    }

                }
                TOdate = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                TOmonth = DateTime.Parse(TOdate).Month.ToString();
                if (TOmonth.Length == 1)
                {
                    TOmonth = "0" + TOmonth;
                }
                TOYear = DateTime.Parse(TOdate).Year.ToString().Substring(2, 2);
                DateTo = TOYear + TOmonth;
            }
            string Qry = "";


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string clientid = gvFromtobills.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView gvnestedgrid = e.Row.FindControl("gvnestedgrid") as GridView;

                if (ddlBillType.SelectedIndex == 0)
                {
                    Qry = " select u.UnitId clientid,BillNo,Left(DATENAME(month,convert(DATETIME,case when Len(month)>3 then (LEFT(month,2)+'/'+Left(08,1)+'/'+Right(month,2)) else (LEFT(month,1)+'/'+Left(08,1)+'/'+Right(month,2)) end )),3)+ ' - '+Left(20,2)+RIGHT(month,2) Month,Month as MonthNew  from UnitBill u inner join Clients c on u.UnitId=c.ClientId where unitid='" + clientid + "' and  MONTHNew between '" + DateFrom + "' and '" + DateTo + "' order by u.MonthNew  ";
                }
                else
                {
                    Qry = " select u.UnitId clientid,BillNo,Left(DATENAME(month,convert(DATETIME,case when Len(month)>3 then (LEFT(month,2)+'/'+Left(08,1)+'/'+Right(month,2)) else (LEFT(month,1)+'/'+Left(08,1)+'/'+Right(month,2)) end )),3)+ ' - '+Left(20,2)+RIGHT(month,2) Month,Month as MonthNew from MUnitBill u inner join Clients c on u.UnitId=c.ClientId where unitid='" + clientid + "' and   MONTHNew between '" + DateFrom + "' and '" + DateTo + "' order by u.MonthNew ";

                }
                DataTable Dinested = SqlHelper.Instance.GetTableByQuery(Qry);

                gvnestedgrid.DataSource = Dinested;
                gvnestedgrid.DataBind();
            }
        }

        public string GetMonthOfYear()
        {
            string MonthYear = "";

            var Month = "";

            string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            Month = month + Year.Substring(2, 2);

            if (Month.ToString().Length == 4)
            {

                MonthYear = "20" + Month.ToString().Substring(2, 2);

            }
            if (Month.ToString().Length == 3)
            {

                MonthYear = "20" + Month.ToString().Substring(1, 2);

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
            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString();
            //payMonth = GetMonth(monthname);
            return monthname;
        }

        protected void ddlMonthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            gvFromtobills.DataSource = null;
            gvFromtobills.DataBind();
            if (ddlMonthType.SelectedIndex == 1)
            {
                lblmonth.Visible = false;
                txtmonth.Visible = false;

                lblfrom.Visible = true;
                txtfrom.Visible = true;
                lblto.Visible = true;
                txtto.Visible = true;
            }
            else
            {
                lblmonth.Visible = true;
                txtmonth.Visible = true;

                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (ddlMonthType.SelectedIndex == 0)
            {
                btnMonthWiseDownload_Click(sender, e);
                return;
            }

            if (ddlMonthType.SelectedIndex == 1)
            {
                btnfromtoWiseDownload_Click(sender, e);
                return;
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

        protected void btnMonthWiseDownload_Click(object sender, EventArgs e)
        {
            int fontsize = 10;
            int font = 10;
            var Month = "";

            if (txtmonth.Text.Trim().Length == 0)
            {
                LblResult.Text = "Please Select Month";
                return;
            }

            var testDate = 0;
            if (txtmonth.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                if (testDate > 0)
                {
                    LblResult.Text = "You have entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.26/01/1990";
                    return;
                }

            }

            if (ddlOptions.SelectedIndex == 0)
            {

                string date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                string month = DateTime.Parse(date).Month.ToString();
                string Year = DateTime.Parse(date).Year.ToString();
                Month = month + Year.Substring(2, 2);
                var list = new List<string>();

                MemoryStream ms = new MemoryStream();

                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                document.AddTitle("Webwonders");
                document.AddAuthor("DIYOS");
                document.AddSubject("Invoice");
                document.AddKeywords("Keyword1, keyword2, …");



                try
                {
                    if (GVListEmployees.Rows.Count > 0)
                    {
                        for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                        {
                            CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                            Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                            Label lblbillno = GVListEmployees.Rows[i].FindControl("lblbillno") as Label;

                            if (chkclientid.Checked == true)
                            {
                                #region for CompanyInfo
                                string strQry = "Select * from CompanyInfo ";
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
                                }

                                #endregion

                                var ContractID = "";

                                int monthval = 0;
                                int yearval = 0;

                                if (Month.ToString().Length == 3)
                                {
                                    monthval = int.Parse(Month.ToString().Substring(0, 1));
                                    yearval = int.Parse("20" + Month.ToString().Substring(1, 2));

                                }


                                if (Month.ToString().Length == 4)
                                {
                                    monthval = int.Parse(Month.ToString().Substring(0, 2));
                                    yearval = int.Parse("20" + Month.ToString().Substring(2, 2));

                                }

                                int Selectdays = System.DateTime.DaysInMonth(yearval, monthval);
                                string dateCheck = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();

                                DateTime LastDay = DateTime.Now;
                                LastDay = DateTime.Parse(dateCheck, CultureInfo.GetCultureInfo("en-gb"));

                                #region  Begin Get Contract Id Based on The Last Day



                                Hashtable HtGetContractID = new Hashtable();
                                var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                                HtGetContractID.Add("@clientid", lblclientid.Text);
                                HtGetContractID.Add("@LastDay", LastDay);
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

                                #region
                                string SqlQuryForServiCharge = "select ContractId,servicecharge,PODate, isnull(EBD.ESiNO,'') EsiBranchname,isnull(PBD.PFNo,'') PFBranchname,convert(nvarchar(20), ContractStartDate, 103) as ContractStartDate,ServiceChargeType,Description,IncludeST,ServiceTax75,Pono,typeofwork,'' billnotes,isnull(ServiceChargeDesc,'') as ServiceChargeDesc,GSTLineitem from contracts  C left join EsiBranchDetails EBD on EBD.EsiBranchid=isnull(C.Esibranch,0) left join PFBranchDetails PBD on PBD.PFBranchid=isnull(C.PFbranch,0)   where " +
                                    " clientid ='" + lblclientid.Text + "' and ContractId='" + ContractID + "'";
                                DataTable DtServicecharge = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuryForServiCharge).Result;
                                string Typeofwork = "";
                                string BillNotes = "";
                                string ServiceCharge = "0";
                                string strSCType = "";
                                string strDescription = "We are presenting our bill for the House Keeping Services Provided at your establishment. Kindly release the payment at the earliest";
                                bool bSCType = false;
                                bool GSTLineitem = false;
                                string strIncludeST = "";
                                string ContractStartDate = "";
                                string strST75 = "";
                                bool bIncludeST = false;
                                bool bST75 = false;
                                string POContent = "";
                                string PODate = "";
                                string CnPFNo = "";
                                string CnESINo = "";
                                string Location = "";
                                string ReversCharges = "";
                                string ServiceChargeDesc = "";
                                // string ServiceTaxCategory = "";
                                if (DtServicecharge.Rows.Count > 0)
                                {
                                    PODate = DtServicecharge.Rows[0]["PODate"].ToString();
                                    if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceCharge"].ToString()) == false)
                                    {
                                        ServiceCharge = DtServicecharge.Rows[0]["ServiceCharge"].ToString();
                                    }
                                    if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceChargeType"].ToString()) == false)
                                    {
                                        strSCType = DtServicecharge.Rows[0]["ServiceChargeType"].ToString();
                                    }
                                    string tempDescription = DtServicecharge.Rows[0]["Description"].ToString();
                                    if (tempDescription.Trim().Length > 0)
                                    {
                                        strDescription = tempDescription;
                                    }
                                    if (strSCType.Length > 0)
                                    {
                                        bSCType = Convert.ToBoolean(strSCType);
                                    }
                                    GSTLineitem = Convert.ToBoolean(DtServicecharge.Rows[0]["GSTLineitem"].ToString());
                                    PFNo = DtServicecharge.Rows[0]["PFBranchname"].ToString().Trim();
                                    Esino = DtServicecharge.Rows[0]["EsiBranchname"].ToString().Trim();

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
                                    BillNotes = DtServicecharge.Rows[0]["BillNotes"].ToString();
                                    // ServiceTaxCategory = DtServicecharge.Rows[0]["ServiceTaxCategory"].ToString();
                                    string tempServiceDesc = DtServicecharge.Rows[0]["ServiceChargeDesc"].ToString();
                                    if (tempServiceDesc.Trim().Length > 0)
                                    {
                                        ServiceChargeDesc = tempServiceDesc;
                                    }
                                }

                                #endregion

                                #region

                                string selectclientaddress = "select isnull(sg.segname,'') as segname,c.*, s.state as Statename,s.GSTStateCode,gst.gstno,gst.GSTAddress,s1.state as ShipState,s1.GSTStateCode as ShipToStateCode1 from clients c left join Segments sg on c.ClientSegment = sg.SegId  left join states s on s.stateid=c.state left join GSTMaster gst on gst.id=c.ourgstin left join states s1 on s1.stateid=c.ShipToState where clientid= '" + lblclientid.Text + "'";
                                DataTable dtclientaddress = config.ExecuteAdaptorAsyncWithQueryParams(selectclientaddress).Result;
                                string OurGSTIN = "";
                                string GSTIN = "";
                                string StateCode = "0";
                                string State = "";
                                string ShipToGSTIN = "";
                                string ShipToStateCode = "0";
                                string ShipToState = "";

                                if (dtclientaddress.Rows.Count > 0)
                                {
                                    OurGSTIN = dtclientaddress.Rows[0]["gstno"].ToString();
                                    StateCode = dtclientaddress.Rows[0]["GSTStateCode"].ToString();
                                    GSTIN = dtclientaddress.Rows[0]["GSTIN"].ToString();
                                    State = dtclientaddress.Rows[0]["Statename"].ToString();
                                    Location = dtclientaddress.Rows[0]["Location"].ToString();

                                    companyAddress = dtclientaddress.Rows[0]["GSTAddress"].ToString();
                                    ShipToStateCode = dtclientaddress.Rows[0]["ShipToStateCode1"].ToString();
                                    ShipToGSTIN = dtclientaddress.Rows[0]["ShipToGSTIN"].ToString();
                                    ShipToState = dtclientaddress.Rows[0]["ShipState"].ToString();
                                }

                                string SelectBillNo = string.Empty;
                                if (ddlBillType.SelectedIndex == 0)
                                {
                                    SelectBillNo = "Select RIGHT(billno,5) as DisplayBillNo,* from Unitbill where month='" + Month + "' and unitid='" + lblclientid.Text + "'";
                                }
                                else //if (ddlType.SelectedIndex == 1 || ddlType.SelectedIndex == 2)
                                {
                                    SelectBillNo = "Select RIGHT(billno,5) as DisplayBillNo,* from mUnitbill where month='" + Month + "' and unitid='" + lblclientid.Text + "' and billno = '" + lblbillno.Text + "'";
                                }
                                DataTable DtBilling = config.ExecuteAdaptorAsyncWithQueryParams(SelectBillNo).Result;
                                string BillNo = "";
                                string DisplayBillNo = "";
                                string area = "";
                                string ExtraRemarks = "";
                                string BilldateCheck = "";
                                if (dtclientaddress.Rows.Count > 0)
                                {
                                    area = dtclientaddress.Rows[0]["segname"].ToString();
                                    BilldateCheck = (DtBilling.Rows[0]["billdt"].ToString());
                                }

                                DateTime BillDate;
                                DateTime DueDate;


                                #region Variables for data Fields as on 11/03/2014 by venkat


                                decimal servicecharge = 0;
                                decimal servicetax = 0;
                                decimal cess = 0;
                                decimal sbcess = 0;
                                decimal kkcess = 0;


                                #region for GST on 17-6-2017 by swathi

                                decimal CGST = 0;
                                decimal SGST = 0;
                                decimal IGST = 0;
                                decimal Cess1 = 0;
                                decimal Cess2 = 0;
                                decimal CGSTPrc = 0;
                                decimal SGSTPrc = 0;
                                decimal IGSTPrc = 0;
                                decimal Cess1Prc = 0;
                                decimal Cess2Prc = 0;

                                #endregion for GST on 17-6-2017 by swathi


                                decimal shecess = 0;
                                decimal totalamount = 0;
                                decimal Grandtotal = 0;

                                decimal ServiceTax75 = 0;
                                decimal ServiceTax25 = 0;

                                decimal machinarycost = 0;
                                decimal materialcost = 0;
                                decimal maintenancecost = 0;
                                decimal extraonecost = 0;
                                decimal extratwocost = 0;
                                decimal discountone = 0;
                                decimal discounttwo = 0;

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

                                decimal staxamtonservicecharge = 0;
                                decimal RelChrgAmt = 0;
                                decimal PFAmt = 0;
                                decimal ESIAmt = 0;
                                decimal BpfPer = 0;
                                decimal BesiPer = 0;


                                #endregion

                                //DateTime dtn = DateTime.ParseExact(BilldateCheck, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                                string billdt = BilldateCheck;

                                string BQry = "select * from TblOptions  where '" + billdt + "' between fromdate and todate";
                                DataTable Bdt = config.ExecuteAdaptorAsyncWithQueryParams(BQry).Result;

                                string CGSTAlias = "";
                                string SGSTAlias = "";
                                string IGSTAlias = "";
                                string Cess1Alias = "";
                                string Cess2Alias = "";
                                string GSTINAlias = "";
                                string OurGSTINAlias = "";

                                string SqlQryForTaxes = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2,CGSTAlias,SGSTAlias,IGSTAlias,cess1Alias,cess2Alias,GSTINAlias,OurGSTINAlias from TblOptions where '" + billdt + "' between fromdate and todate ";
                                DataTable DtTaxes = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForTaxes).Result;

                                string SCPersent = "";
                                if (DtTaxes.Rows.Count > 0)
                                {
                                    SCPersent = DtTaxes.Rows[0]["ServiceTaxSeparate"].ToString();
                                    CGSTAlias = DtTaxes.Rows[0]["CGSTAlias"].ToString();
                                    SGSTAlias = DtTaxes.Rows[0]["SGSTAlias"].ToString();
                                    IGSTAlias = DtTaxes.Rows[0]["IGSTAlias"].ToString();
                                    Cess1Alias = DtTaxes.Rows[0]["Cess1Alias"].ToString();
                                    Cess2Alias = DtTaxes.Rows[0]["Cess2Alias"].ToString();
                                    GSTINAlias = DtTaxes.Rows[0]["GSTINAlias"].ToString();
                                    OurGSTINAlias = DtTaxes.Rows[0]["OurGSTINAlias"].ToString();
                                }


                                if (DtBilling.Rows.Count > 0)
                                {

                                    ExtraRemarks = DtBilling.Rows[0]["Remarks"].ToString();
                                    BillNo = DtBilling.Rows[0]["billno"].ToString();
                                    DisplayBillNo = DtBilling.Rows[0]["DisplayBillNo"].ToString();
                                    BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                                    OurGSTIN = DtBilling.Rows[0]["OURGSTNo"].ToString();
                                    StateCode = DtBilling.Rows[0]["BillToStateCode"].ToString();
                                    GSTIN = DtBilling.Rows[0]["BillToGSTNo"].ToString();
                                    State = DtBilling.Rows[0]["BillToState"].ToString();
                                    ShipToStateCode = DtBilling.Rows[0]["ShipToStateCode"].ToString();
                                    ShipToGSTIN = DtBilling.Rows[0]["ShipToGSTNo"].ToString();
                                    ShipToState = DtBilling.Rows[0]["ShipToState"].ToString();
                                    if (ddlBillType.SelectedIndex == 0)
                                    {
                                        DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());
                                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax75"].ToString()) == false)
                                        {
                                            ServiceTax75 = decimal.Parse(DtBilling.Rows[0]["ServiceTax75"].ToString());
                                        }

                                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax25"].ToString()) == false)
                                        {
                                            ServiceTax25 = decimal.Parse(DtBilling.Rows[0]["ServiceTax25"].ToString());
                                        }

                                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString()) == false)
                                        {
                                            servicecharge = decimal.Parse(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString());
                                        }

                                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["RelChrgAmt"].ToString()) == false)
                                        {
                                            RelChrgAmt = decimal.Parse(DtBilling.Rows[0]["RelChrgAmt"].ToString());
                                        }

                                        if (string.IsNullOrEmpty(DtBilling.Rows[0]["Bpfamt"].ToString()) == false)
                                        {
                                            PFAmt = decimal.Parse(DtBilling.Rows[0]["Bpfamt"].ToString());
                                        }


                                        if (string.IsNullOrEmpty(DtBilling.Rows[0]["Besiamt"].ToString()) == false)
                                        {
                                            ESIAmt = decimal.Parse(DtBilling.Rows[0]["Besiamt"].ToString());
                                        }

                                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["BpfPer"].ToString()) == false)
                                        {
                                            BpfPer = decimal.Parse(DtBilling.Rows[0]["BpfPer"].ToString());
                                        }

                                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["BesiPer"].ToString()) == false)
                                        {
                                            BesiPer = decimal.Parse(DtBilling.Rows[0]["BesiPer"].ToString());
                                        }

                                    }

                                    else
                                    {
                                        if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrg"].ToString()) == false)
                                        {
                                            servicecharge = decimal.Parse(DtBilling.Rows[0]["ServiceChrg"].ToString());
                                        }
                                    }



                                    #region Begin New code for values taken from database as on 11/03/2014 by venkat

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["dutiestotalamount"].ToString()) == false)
                                    {
                                        totalamount = decimal.Parse(DtBilling.Rows[0]["dutiestotalamount"].ToString());
                                    }




                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                                    {
                                        servicetax = decimal.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                                    {
                                        sbcess = decimal.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                                    {
                                        kkcess = decimal.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
                                    }

                                    #region for GST as on 17-6-2017 by swathi

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTAmt"].ToString()) == false)
                                    {
                                        CGST = decimal.Parse(DtBilling.Rows[0]["CGSTAmt"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTAmt"].ToString()) == false)
                                    {
                                        SGST = decimal.Parse(DtBilling.Rows[0]["SGSTAmt"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTAmt"].ToString()) == false)
                                    {
                                        IGST = decimal.Parse(DtBilling.Rows[0]["IGSTAmt"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Amt"].ToString()) == false)
                                    {
                                        Cess1 = decimal.Parse(DtBilling.Rows[0]["Cess1Amt"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Amt"].ToString()) == false)
                                    {
                                        Cess2 = decimal.Parse(DtBilling.Rows[0]["Cess2Amt"].ToString());
                                    }


                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTPrc"].ToString()) == false)
                                    {
                                        CGSTPrc = decimal.Parse(DtBilling.Rows[0]["CGSTPrc"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTPrc"].ToString()) == false)
                                    {
                                        SGSTPrc = decimal.Parse(DtBilling.Rows[0]["SGSTPrc"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTPrc"].ToString()) == false)
                                    {
                                        IGSTPrc = decimal.Parse(DtBilling.Rows[0]["IGSTPrc"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Prc"].ToString()) == false)
                                    {
                                        Cess1Prc = decimal.Parse(DtBilling.Rows[0]["Cess1Prc"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Prc"].ToString()) == false)
                                    {
                                        Cess2Prc = decimal.Parse(DtBilling.Rows[0]["Cess2Prc"].ToString());
                                    }

                                    #endregion for GST as on 17-6-2017 by swathi

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["CESS"].ToString()) == false)
                                    {
                                        cess = decimal.Parse(DtBilling.Rows[0]["CESS"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SHECess"].ToString()) == false)
                                    {
                                        shecess = decimal.Parse(DtBilling.Rows[0]["SHECess"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["GrandTotal"].ToString()) == false)
                                    {
                                        Grandtotal = decimal.Parse(DtBilling.Rows[0]["GrandTotal"].ToString());
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["MachinaryCost"].ToString()) == false)
                                    {
                                        machinarycost = decimal.Parse(DtBilling.Rows[0]["MachinaryCost"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["MaterialCost"].ToString()) == false)
                                    {
                                        materialcost = decimal.Parse(DtBilling.Rows[0]["MaterialCost"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ElectricalChrg"].ToString()) == false)
                                    {
                                        maintenancecost = decimal.Parse(DtBilling.Rows[0]["ElectricalChrg"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtone"].ToString()) == false)
                                    {
                                        extraonecost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtone"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtTwo"].ToString()) == false)
                                    {
                                        extratwocost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtTwo"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discount"].ToString()) == false)
                                    {
                                        discountone = decimal.Parse(DtBilling.Rows[0]["Discount"].ToString());
                                    }
                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discounttwo"].ToString()) == false)
                                    {
                                        discounttwo = decimal.Parse(DtBilling.Rows[0]["Discounttwo"].ToString());
                                    }

                                    machinarycosttitle = DtBilling.Rows[0]["Machinarycosttitle"].ToString();
                                    materialcosttitle = DtBilling.Rows[0]["Materialcosttitle"].ToString();
                                    maintenancecosttitle = DtBilling.Rows[0]["Maintanancecosttitle"].ToString();
                                    extraonecosttitle = DtBilling.Rows[0]["Extraonetitle"].ToString();
                                    extratwocosttitle = DtBilling.Rows[0]["Extratwotitle"].ToString();
                                    discountonetitle = DtBilling.Rows[0]["Discountonetitle"].ToString();
                                    discounttwotitle = DtBilling.Rows[0]["Discounttwotitle"].ToString();

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Extradatacheck"].ToString()) == false)
                                    {
                                        strExtradatacheck = DtBilling.Rows[0]["Extradatacheck"].ToString();
                                        if (strExtradatacheck == "True")
                                        {
                                            Extradatacheck = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraDataSTcheck"].ToString()) == false)
                                    {
                                        strExtrastcheck = DtBilling.Rows[0]["ExtraDataSTcheck"].ToString();
                                        if (strExtrastcheck == "True")
                                        {
                                            ExtraDataSTcheck = true;
                                        }
                                    }



                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMachinary"].ToString()) == false)
                                    {
                                        strSTMachinary = DtBilling.Rows[0]["STMachinary"].ToString();
                                        if (strSTMachinary == "True")
                                        {
                                            STMachinary = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaterial"].ToString()) == false)
                                    {
                                        strSTMaterial = DtBilling.Rows[0]["STMaterial"].ToString();
                                        if (strSTMaterial == "True")
                                        {
                                            STMaterial = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaintenance"].ToString()) == false)
                                    {
                                        strSTMaintenance = DtBilling.Rows[0]["STMaintenance"].ToString();
                                        if (strSTMaintenance == "True")
                                        {
                                            STMaintenance = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtraone"].ToString()) == false)
                                    {
                                        strSTExtraone = DtBilling.Rows[0]["STExtraone"].ToString();
                                        if (strSTExtraone == "True")
                                        {
                                            STExtraone = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtratwo"].ToString()) == false)
                                    {
                                        strSTExtratwo = DtBilling.Rows[0]["STExtratwo"].ToString();
                                        if (strSTExtratwo == "True")
                                        {
                                            STExtratwo = true;
                                        }
                                    }


                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMachinary"].ToString()) == false)
                                    {
                                        strSCMachinary = DtBilling.Rows[0]["SCMachinary"].ToString();
                                        if (strSCMachinary == "True")
                                        {
                                            SCMachinary = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaterial"].ToString()) == false)
                                    {
                                        strSCMaterial = DtBilling.Rows[0]["SCMaterial"].ToString();
                                        if (strSCMaterial == "True")
                                        {
                                            SCMaterial = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaintenance"].ToString()) == false)
                                    {
                                        strSCMaintenance = DtBilling.Rows[0]["SCMaintenance"].ToString();
                                        if (strSCMaintenance == "True")
                                        {
                                            SCMaintenance = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtraone"].ToString()) == false)
                                    {
                                        strSCExtraone = DtBilling.Rows[0]["SCExtraone"].ToString();
                                        if (strSCExtraone == "True")
                                        {
                                            SCExtraone = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtratwo"].ToString()) == false)
                                    {
                                        strSCExtratwo = DtBilling.Rows[0]["SCExtratwo"].ToString();
                                        if (strSCExtratwo == "True")
                                        {
                                            SCExtratwo = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                                    {
                                        strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                                        if (strSTDiscountone == "True")
                                        {
                                            STDiscountone = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                                    {
                                        strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                                        if (strSTDiscounttwo == "True")
                                        {
                                            STDiscounttwo = true;
                                        }
                                    }


                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                                    {
                                        strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                                        if (strSTDiscountone == "True")
                                        {
                                            STDiscountone = true;
                                        }
                                    }

                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                                    {
                                        strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                                        if (strSTDiscounttwo == "True")
                                        {
                                            STDiscounttwo = true;
                                        }
                                    }




                                    if (String.IsNullOrEmpty(DtBilling.Rows[0]["Staxonservicecharge"].ToString()) == false)
                                    {
                                        staxamtonservicecharge = decimal.Parse(DtBilling.Rows[0]["Staxonservicecharge"].ToString());
                                    }

                                    #endregion
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                                    return;
                                }

                                #endregion

                                //document.AddTitle(companyName);
                                //document.AddAuthor("DIYOS");
                                //document.AddSubject("Invoice");
                                //document.AddKeywords("Keyword1, keyword2, …");


                                string imagepath = Server.MapPath("~/assets/" + CmpIDPrefix + "BillLogo.png");
                                if (File.Exists(imagepath))
                                {
                                    iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);
                                    gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                                    gif2.ScalePercent(55f);
                                    gif2.SetAbsolutePosition(5f, 753f);
                                    document.Add(gif2);
                                }


                                PdfPTable tablelogo = new PdfPTable(2);
                                tablelogo.TotalWidth = 580f;
                                tablelogo.LockedWidth = true;
                                float[] widtlogo = new float[] { 0.4f, 2f };
                                tablelogo.SetWidths(widtlogo);


                                var FontColour = new BaseColor(178, 34, 34);
                                Font FontStyle1 = FontFactory.GetFont("Belwe-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 30, Font.NORMAL, FontColour);

                                PdfPCell CCompName1 = new PdfPCell(new Paragraph("" + companyName, FontFactory.GetFont(FontStyle, 15, Font.BOLD, BaseColor.BLACK)));
                                CCompName1.HorizontalAlignment = 1;
                                CCompName1.Colspan = 2;
                                CCompName1.PaddingTop = 25f;
                                CCompName1.Border = 0;
                                CCompName1.PaddingLeft = 80;
                                tablelogo.AddCell(CCompName1);

                                PdfPCell CCompName = new PdfPCell(new Paragraph("" + companyAddress, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                CCompName.HorizontalAlignment = 1;
                                CCompName.Colspan = 2;
                                CCompName.Border = 0;
                                CCompName.PaddingLeft = 80;
                                CCompName.SetLeading(0, 1.2f);
                                tablelogo.AddCell(CCompName);


                                if (emailid.Length > 0)
                                {
                                    PdfPCell CCompName2 = new PdfPCell(new Paragraph("Website :" + website + " | Email :" + emailid, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    CCompName2.HorizontalAlignment = 1;
                                    CCompName2.Colspan = 2;
                                    CCompName2.Border = 0;
                                    CCompName2.PaddingLeft = 40;
                                    tablelogo.AddCell(CCompName2);
                                }
                                if (phoneno.Length > 0)
                                {
                                    PdfPCell CCompName2 = new PdfPCell(new Paragraph("Phone :" + phoneno, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                    CCompName2.HorizontalAlignment = 1;
                                    CCompName2.Colspan = 2;
                                    CCompName2.Border = 0;
                                    CCompName2.PaddingBottom = 5;
                                    tablelogo.AddCell(CCompName2);
                                }

                                document.Add(tablelogo);

                                #region  for Client Details

                                PdfPTable address = new PdfPTable(5);
                                address.TotalWidth = 580f;
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
                                tempTable1.TotalWidth = 348f;
                                tempTable1.LockedWidth = true;
                                float[] tempWidth1 = new float[] { 0.8f, 2f, 2f };
                                tempTable1.SetWidths(tempWidth1);

                                string addressData = "";

                                addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString();

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
                                addressData = dtclientaddress.Rows[0]["ClientAddrStreet"].ToString();
                                if (addressData.Trim().Length > 0)
                                {
                                    PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    clientstreet.BorderWidthBottom = 0;
                                    clientstreet.BorderWidthTop = 0;
                                    clientstreet.Colspan = 3;
                                    clientstreet.BorderWidthLeft = .2f;
                                    clientstreet.BorderWidthRight = 0.2f;
                                    clientstreet.BorderColor = BaseColor.BLACK;
                                    //clientstreet.PaddingLeft = 20;
                                    tempTable1.AddCell(clientstreet);
                                }


                                addressData = dtclientaddress.Rows[0]["ClientAddrArea"].ToString();
                                if (addressData.Trim().Length > 0)
                                {
                                    PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    clientstreet.BorderWidthBottom = 0;
                                    clientstreet.BorderWidthTop = 0;
                                    clientstreet.Colspan = 3;
                                    clientstreet.BorderColor = BaseColor.BLACK;
                                    clientstreet.BorderWidthLeft = .2f;
                                    clientstreet.BorderWidthRight = 0.2f;
                                    // clientstreet.PaddingLeft = 20;
                                    tempTable1.AddCell(clientstreet);
                                }


                                var ClientAddrColony = dtclientaddress.Rows[0]["ClientAddrColony"].ToString();
                                var ClientAddrcity = dtclientaddress.Rows[0]["ClientAddrcity"].ToString();
                                var ClientAddrstate = dtclientaddress.Rows[0]["ClientAddrstate"].ToString();
                                var ClientAddrpin = dtclientaddress.Rows[0]["ClientAddrpin"].ToString();
                                addressData = (ClientAddrColony + "," + ClientAddrcity + "," + ClientAddrstate + "," + ClientAddrpin);
                                if (addressData.Trim().Length > 0)
                                {
                                    PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    clietnpin.Colspan = 3;
                                    clietnpin.BorderWidthBottom = 0;
                                    clietnpin.BorderWidthTop = 0;
                                    clietnpin.BorderWidthLeft = .2f;
                                    clietnpin.BorderWidthRight = 0.2f;
                                    clietnpin.BorderColor = BaseColor.BLACK;
                                    //  clietnpin.PaddingLeft = 20;
                                    tempTable1.AddCell(clietnpin);
                                }

                                if (Bdt.Rows.Count > 0)
                                {
                                    if (StateCode == "1" || StateCode == "2" || StateCode == "3" || StateCode == "4" || StateCode == "5" || StateCode == "6" || StateCode == "7" || StateCode == "8" || StateCode == "7")
                                    {
                                        StateCode = "0" + StateCode;
                                    }
                                    if (State.Length > 0)
                                    {
                                        PdfPCell clietnpin = new PdfPCell(new Paragraph("State", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        clietnpin.Colspan = 1;
                                        clietnpin.BorderWidthBottom = 0;
                                        clietnpin.BorderWidthTop = 0;
                                        clietnpin.BorderWidthLeft = .2f;
                                        clietnpin.BorderWidthRight = 0;
                                        clietnpin.BorderColor = BaseColor.BLACK;
                                        //  clietnpin.PaddingLeft = 20;
                                        tempTable1.AddCell(clietnpin);

                                        clietnpin = new PdfPCell(new Paragraph(" : " + State, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        clietnpin.Colspan = 1;
                                        clietnpin.BorderWidthBottom = 0;
                                        clietnpin.BorderWidthTop = 0;
                                        clietnpin.BorderWidthLeft = 0;
                                        clietnpin.BorderWidthRight = 0;
                                        clietnpin.BorderColor = BaseColor.BLACK;
                                        //  clietnpin.PaddingLeft = 20;
                                        tempTable1.AddCell(clietnpin);
                                    }
                                    if (StateCode.Length > 0)
                                    {
                                        PdfPCell clietnpin = new PdfPCell(new Paragraph("State Code : " + StateCode, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        clietnpin.Colspan = 1;
                                        clietnpin.BorderWidthBottom = 0;
                                        clietnpin.BorderWidthTop = 0;
                                        clietnpin.BorderWidthLeft = 0;
                                        clietnpin.BorderWidthRight = 0.2f;
                                        clietnpin.BorderColor = BaseColor.BLACK;
                                        //  clietnpin.PaddingLeft = 20;
                                        tempTable1.AddCell(clietnpin);
                                    }


                                }

                                if (GSTIN.Length > 0)
                                {
                                    PdfPCell clietnpin = new PdfPCell(new Paragraph("GSTIN ", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                                    clietnpin = new PdfPCell(new Paragraph(" : " + GSTIN, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                                PdfPCell cellemp1 = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cellemp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                cellemp1.Colspan = 3;
                                cellemp1.BorderWidthTop = 0;
                                cellemp1.BorderWidthBottom = 0;
                                cellemp1.BorderWidthLeft = .2f;
                                cellemp1.BorderWidthRight = 0.2f;
                                cellemp1.BorderColor = BaseColor.BLACK;
                                cellemp1.PaddingBottom = 15;
                                //tempTable1.AddCell(cellemp1);


                                PdfPCell childTable1 = new PdfPCell(tempTable1);
                                childTable1.Border = 0;
                                childTable1.Colspan = 3;
                                // childTable1.FixedHeight = 100;
                                childTable1.HorizontalAlignment = 0;

                                address.AddCell(childTable1);

                                PdfPTable tempTable2 = new PdfPTable(2);
                                tempTable2.TotalWidth = 232f;
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
                                phrase10.Add(new Chunk(": " + BillNo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                                phrase11v.Add(new Chunk(": " + BillDate.Day.ToString("00") + "/" + BillDate.ToString("MM") + "/" +
                                    BillDate.Year, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                                phrase10im.Add(new Chunk(": " + GetMonthName() + "'" + GetMonthOfYear(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                                string Fromdate = "";
                                string Todate = "";


                                var phrase10p = new Phrase();
                                phrase10p.Add(new Chunk(": " + Fromdate + " to " + Todate, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                                if (POContent.Length > 0)
                                {
                                    var phrasew = new Phrase();
                                    phrasew.Add(new Chunk("PO No", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell13 = new PdfPCell();
                                    cell13.AddElement(phrasew);
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

                                    var phrase10w = new Phrase();
                                    phrase10w.Add(new Chunk(": " + POContent, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    cell13v = new PdfPCell();
                                    cell13v.AddElement(phrase10w);
                                    cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    cell13v.BorderWidthBottom = 0;
                                    cell13v.BorderWidthTop = 0;
                                    cell13v.Colspan = 1;
                                    cell13v.BorderWidthLeft = 0;
                                    cell13v.BorderWidthRight = .2f;
                                    cell13v.PaddingTop = -5;
                                    cell13v.BorderColor = BaseColor.BLACK;
                                    tempTable2.AddCell(cell13v);

                                }

                                if (PODate.Length > 0)
                                {
                                    var phrasew = new Phrase();
                                    phrasew.Add(new Chunk("Work Order Date", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell13 = new PdfPCell();
                                    cell13.AddElement(phrasew);
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

                                    var phrase10w = new Phrase();
                                    phrase10w.Add(new Chunk(": " + PODate, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    cell13v = new PdfPCell();
                                    cell13v.AddElement(phrase10w);
                                    cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    cell13v.BorderWidthBottom = 0;
                                    cell13v.BorderWidthTop = 0;
                                    cell13v.Colspan = 1;
                                    cell13v.BorderWidthLeft = 0;
                                    cell13v.BorderWidthRight = .2f;
                                    cell13v.PaddingTop = -5;
                                    cell13v.BorderColor = BaseColor.BLACK;
                                    tempTable2.AddCell(cell13v);

                                }

                                if (Location.Length > 0)
                                {
                                    var phrasew = new Phrase();
                                    phrasew.Add(new Chunk("Place of Supply", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell13 = new PdfPCell();
                                    cell13.AddElement(phrasew);
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

                                    var phrase10w = new Phrase();
                                    phrase10w.Add(new Chunk(": " + Location, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    cell13v = new PdfPCell();
                                    cell13v.AddElement(phrase10w);
                                    cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    cell13v.BorderWidthBottom = 0;
                                    cell13v.BorderWidthTop = 0;
                                    cell13v.Colspan = 1;
                                    cell13v.BorderWidthLeft = 0;
                                    cell13v.BorderWidthRight = .2f;
                                    cell13v.PaddingTop = -5;
                                    cell13v.BorderColor = BaseColor.BLACK;
                                    tempTable2.AddCell(cell13v);

                                }

                                if (ReversCharges.Length > 0)
                                {
                                    var phrasew = new Phrase();
                                    phrasew.Add(new Chunk("Revers Charges", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell13 = new PdfPCell();
                                    cell13.AddElement(phrasew);
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
                                    if (ReversCharges == "True")
                                    {
                                        ReversCharges = "Yes";
                                    }
                                    else
                                    {
                                        ReversCharges = "No";
                                    }

                                    var phrase10w = new Phrase();
                                    phrase10w.Add(new Chunk(": " + ReversCharges, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    cell13v = new PdfPCell();
                                    cell13v.AddElement(phrase10w);
                                    cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    cell13v.BorderWidthBottom = 0;
                                    cell13v.BorderWidthTop = 0;
                                    cell13v.Colspan = 1;
                                    cell13v.BorderWidthLeft = 0;
                                    cell13v.BorderWidthRight = .2f;
                                    cell13v.PaddingTop = -5;
                                    cell13v.BorderColor = BaseColor.BLACK;
                                    tempTable2.AddCell(cell13v);

                                }


                                PdfPCell childTable2 = new PdfPCell(tempTable2);
                                childTable2.Border = 0;
                                childTable2.Colspan = 2;
                                childTable2.HorizontalAlignment = 0;
                                address.AddCell(childTable2);

                                document.Add(address);





                                #endregion

                                #region for breakupdata
                                int countGrid = 5;

                                DataTable dtheadings = null;
                                var SPNameD = "GetInvHeadings";
                                Hashtable htheadings = new Hashtable();
                                htheadings.Add("@clientid", lblclientid.Text);
                                // htheadings.Add("@LastDay", DtLastDay);
                                dtheadings = config.ExecuteAdaptorAsyncWithParams(SPNameD, htheadings).Result;

                                string InvDescription = "";
                                string InvNoOfEmps = "";
                                string InvNoofDuties = "";
                                string InvPayrate = "";
                                string InvAmount = "";
                                string InvSACCode = "";
                                string InvMonthDays = "";
                                string InvDescriptionVisible = "N";
                                string InvNoOfEmpsVisible = "N";
                                string InvNoofDutiesVisible = "N";
                                string InvPayrateVisible = "N";
                                string InvAmountVisible = "N";
                                string InvSACCodeVisible = "N";
                                string InvMonthDaysVisible = "N";
                                string HSNNo = "";
                                var ExDBRemarks = "";
                                if (dtheadings.Rows.Count > 0)
                                {
                                    InvDescription = dtheadings.Rows[0]["InvDescription"].ToString();
                                    InvNoOfEmps = dtheadings.Rows[0]["InvNoOfEmps"].ToString();
                                    InvNoofDuties = dtheadings.Rows[0]["InvNoofDuties"].ToString();
                                    InvPayrate = dtheadings.Rows[0]["InvPayrate"].ToString();
                                    InvAmount = dtheadings.Rows[0]["InvAmount"].ToString();
                                    InvMonthDays = dtheadings.Rows[0]["InvMonthDays"].ToString();
                                    InvSACCode = dtheadings.Rows[0]["InvSACCode"].ToString();
                                    InvDescriptionVisible = dtheadings.Rows[0]["InvDescriptionVisible"].ToString();
                                    InvNoOfEmpsVisible = dtheadings.Rows[0]["InvNoOfEmpsVisible"].ToString();
                                    InvNoofDutiesVisible = dtheadings.Rows[0]["InvNoofDutiesVisible"].ToString();
                                    InvPayrateVisible = dtheadings.Rows[0]["InvPayrateVisible"].ToString();
                                    InvAmountVisible = dtheadings.Rows[0]["InvAmountVisible"].ToString();
                                    InvSACCodeVisible = dtheadings.Rows[0]["InvSACCodeVisible"].ToString();
                                    InvMonthDaysVisible = dtheadings.Rows[0]["InvMonthDaysVisible"].ToString();
                                }




                                int colCount = 1;

                                if (InvDescriptionVisible == "Y")
                                {
                                    colCount += 1;
                                }

                                if (InvNoOfEmpsVisible == "Y")
                                {
                                    colCount += 1;
                                }

                                if (InvNoofDutiesVisible == "Y")
                                {
                                    colCount += 1;
                                }

                                if (InvPayrateVisible == "Y")
                                {
                                    colCount += 1;
                                }

                                if (InvAmountVisible == "Y")
                                {
                                    colCount += 1;
                                }



                                if (InvSACCodeVisible == "Y")
                                {
                                    colCount += 1;
                                }

                                if (InvMonthDaysVisible == "Y")
                                {
                                    colCount += 1;
                                }


                                PdfPTable table = new PdfPTable(colCount);
                                table.TotalWidth = 580f;
                                table.LockedWidth = true;
                                table.HorizontalAlignment = 1;
                                float[] colWidths = new float[] { };
                                if (colCount == 8)
                                {
                                    colWidths = new float[] { 1f, 6f, 2f, 2f, 2f, 2.2f, 2f, 2.7f };
                                }
                                if (colCount == 7)
                                {
                                    colWidths = new float[] { 1f, 6f, 2f, 2f, 2.2f, 2f, 2.7f };
                                }
                                if (colCount == 6)
                                {
                                    colWidths = new float[] { 1f, 6f, 2f, 2.2f, 2f, 2.7f };
                                }

                                if (colCount == 5)
                                {
                                    colWidths = new float[] { 1f, 6f, 2f, 2.2f, 2.7f };
                                }

                                if (colCount == 4)
                                {
                                    colWidths = new float[] { 1f, 6f, 2.2f, 2.7f };
                                }

                                if (colCount == 3)
                                {
                                    colWidths = new float[] { 1f, 6f, 2.7f };
                                }


                                table.SetWidths(colWidths);

                                string cellText;


                                PdfPCell cell = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0.2f;
                                cell.BorderWidthRight = 0f;
                                cell.Colspan = 1;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                if (InvDescriptionVisible == "Y")
                                {
                                    cell = new PdfPCell(new Phrase(InvDescription, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell.HorizontalAlignment = 1;
                                    cell.BorderWidthBottom = 0.2f;
                                    cell.BorderWidthLeft = 0.2f;
                                    cell.BorderWidthTop = 0.2f;
                                    cell.BorderWidthRight = 0f;
                                    //cell.Colspan = 1;
                                    cell.BorderColor = BaseColor.BLACK;
                                    table.AddCell(cell);
                                }
                                if (InvSACCodeVisible == "Y")
                                {
                                    cell = new PdfPCell(new Phrase(InvSACCode, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell.HorizontalAlignment = 1;
                                    cell.BorderWidthBottom = 0.2f;
                                    cell.BorderWidthLeft = 0.2f;
                                    cell.BorderWidthTop = 0.2f;
                                    cell.BorderWidthRight = 0f;
                                    //cell.Colspan = 1;
                                    cell.BorderColor = BaseColor.BLACK;
                                    table.AddCell(cell);
                                }
                                if (InvMonthDaysVisible == "Y")
                                {
                                    cell = new PdfPCell(new Phrase(InvMonthDays, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell.HorizontalAlignment = 1;
                                    cell.BorderWidthBottom = 0.2f;
                                    cell.BorderWidthLeft = 0.2f;
                                    cell.BorderWidthTop = 0.2f;
                                    cell.BorderWidthRight = 0f;
                                    //cell.Colspan = 1;
                                    cell.BorderColor = BaseColor.BLACK;
                                    table.AddCell(cell);
                                }
                                if (InvNoOfEmpsVisible == "Y")
                                {
                                    cell = new PdfPCell(new Phrase(InvNoOfEmps, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell.HorizontalAlignment = 1;
                                    cell.BorderWidthBottom = 0.2f;
                                    cell.BorderWidthLeft = 0.2f;
                                    cell.BorderWidthTop = 0.2f;
                                    cell.BorderWidthRight = 0f;
                                    //cell.Colspan = 1;
                                    cell.BorderColor = BaseColor.BLACK;
                                    table.AddCell(cell);
                                }

                                if (InvPayrateVisible == "Y")
                                {
                                    cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell.HorizontalAlignment = 1;
                                    cell.BorderWidthBottom = 0.2f;
                                    cell.BorderWidthLeft = 0.2f;
                                    cell.BorderWidthTop = 0.2f;
                                    cell.BorderWidthRight = 0f;
                                    cell.BorderColor = BaseColor.BLACK;
                                    table.AddCell(cell);
                                }

                                if (InvNoofDutiesVisible == "Y")
                                {
                                    cell = new PdfPCell(new Phrase(InvNoofDuties, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell.HorizontalAlignment = 1;
                                    cell.BorderWidthBottom = 0.2f;
                                    cell.BorderWidthLeft = 0.2f;
                                    cell.BorderWidthTop = 0.2f;
                                    cell.BorderWidthRight = 0f;
                                    cell.BorderColor = BaseColor.BLACK;
                                    table.AddCell(cell);
                                }

                                if (InvAmountVisible == "Y")
                                {
                                    cell = new PdfPCell(new Phrase(InvAmount, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    cell.HorizontalAlignment = 1;
                                    cell.BorderWidthBottom = 0.2f;
                                    cell.BorderWidthLeft = 0.2f;
                                    cell.BorderWidthTop = 0.2f;
                                    cell.BorderWidthRight = .2f;
                                    cell.BorderColor = BaseColor.BLACK;
                                    table.AddCell(cell);
                                }


                                string spUnitbillbreakup = "GetpdfforClientbillingfromunitbillbreakup";
                                Hashtable htunitbillbreakup = new Hashtable();
                                htunitbillbreakup.Add("@Clientid", lblclientid.Text);
                                htunitbillbreakup.Add("@month", Month);
                                htunitbillbreakup.Add("@status", ddlBillType.SelectedIndex);
                                htunitbillbreakup.Add("@munitibillno", lblbillno.Text);

                                DataTable dtunitbillbreakup = SqlHelper.Instance.ExecuteStoredProcedureWithParams(spUnitbillbreakup, htunitbillbreakup);

                                for (int rowIndex = 0; rowIndex < dtunitbillbreakup.Rows.Count; rowIndex++)
                                {

                                    int SNo = 0;
                                    string Designation = "";
                                    string SACCodes = "";
                                    decimal MonthDays = 0;
                                    decimal NoOfEmps = 0;
                                    decimal payrate = 0;
                                    decimal Duties = 0;
                                    decimal Dutiesamount = 0;

                                    SNo = int.Parse(dtunitbillbreakup.Rows[rowIndex]["SNo"].ToString());
                                    Designation = dtunitbillbreakup.Rows[rowIndex]["Designation"].ToString();
                                    SACCodes = dtunitbillbreakup.Rows[rowIndex]["SACCode"].ToString();
                                    MonthDays = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["MonthDays"].ToString());
                                    NoOfEmps = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["NoOfEmps"].ToString());
                                    payrate = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["payrate"].ToString());
                                    Duties = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["Duties"].ToString());
                                    Dutiesamount = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["Dutiesamount"].ToString());

                                    cell = new PdfPCell(new Phrase(SNo.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    cell.Colspan = 1;
                                    cell.BorderWidthRight = 0f;
                                    cell.BorderWidthLeft = .2f;
                                    cell.BorderWidthTop = 0;
                                    cell.MinimumHeight = 20;
                                    cell.HorizontalAlignment = 1;
                                    cell.BorderColor = BaseColor.BLACK;
                                    table.AddCell(cell);

                                    if (InvDescriptionVisible == "Y")
                                    {
                                        cell = new PdfPCell(new Phrase(Designation, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0.2f;
                                        cell.BorderWidthLeft = 0.2f;
                                        cell.BorderWidthTop = 0.2f;
                                        cell.BorderWidthRight = 0f;
                                        //cell.Colspan = 1;
                                        cell.BorderColor = BaseColor.BLACK;
                                        table.AddCell(cell);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        cell = new PdfPCell(new Phrase(SACCodes, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.2f;
                                        cell.BorderWidthLeft = 0.2f;
                                        cell.BorderWidthTop = 0.2f;
                                        cell.BorderWidthRight = 0f;
                                        //cell.Colspan = 1;
                                        cell.BorderColor = BaseColor.BLACK;
                                        table.AddCell(cell);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        cell = new PdfPCell(new Phrase(MonthDays.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.2f;
                                        cell.BorderWidthLeft = 0.2f;
                                        cell.BorderWidthTop = 0.2f;
                                        cell.BorderWidthRight = 0f;
                                        //cell.Colspan = 1;
                                        cell.BorderColor = BaseColor.BLACK;
                                        table.AddCell(cell);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        cell = new PdfPCell(new Phrase(NoOfEmps.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.2f;
                                        cell.BorderWidthLeft = 0.2f;
                                        cell.BorderWidthTop = 0.2f;
                                        cell.BorderWidthRight = 0f;
                                        //cell.Colspan = 1;
                                        cell.BorderColor = BaseColor.BLACK;
                                        table.AddCell(cell);
                                    }

                                    if (InvPayrateVisible == "Y")
                                    {
                                        cell = new PdfPCell(new Phrase(payrate.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.2f;
                                        cell.BorderWidthLeft = 0.2f;
                                        cell.BorderWidthTop = 0.2f;
                                        cell.BorderWidthRight = 0f;
                                        cell.BorderColor = BaseColor.BLACK;
                                        table.AddCell(cell);
                                    }

                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        cell = new PdfPCell(new Phrase(Duties.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.2f;
                                        cell.BorderWidthLeft = 0.2f;
                                        cell.BorderWidthTop = 0.2f;
                                        cell.BorderWidthRight = 0f;
                                        cell.BorderColor = BaseColor.BLACK;
                                        table.AddCell(cell);
                                    }

                                    if (InvAmountVisible == "Y")
                                    {
                                        cell = new PdfPCell(new Phrase(Dutiesamount.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.2f;
                                        cell.BorderWidthLeft = 0.2f;
                                        cell.BorderWidthTop = 0.2f;
                                        cell.BorderWidthRight = .2f;
                                        cell.BorderColor = BaseColor.BLACK;
                                        table.AddCell(cell);
                                    }

                                }


                                #region for space
                                PdfPCell Cellempty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellempty.HorizontalAlignment = 2;
                                Cellempty.Colspan = 1;
                                Cellempty.BorderWidthTop = 0;
                                Cellempty.BorderWidthRight = 0f;
                                Cellempty.BorderWidthLeft = .2f;
                                Cellempty.BorderWidthBottom = 0;
                                // Cellempty.MinimumHeight = 5;
                                Cellempty.BorderColor = BaseColor.BLACK;


                                PdfPCell Cellempty1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellempty1.HorizontalAlignment = 2;
                                Cellempty1.Colspan = 1;
                                Cellempty1.BorderWidthTop = 0;
                                Cellempty1.BorderWidthRight = 0f;
                                Cellempty1.BorderWidthLeft = 0.2f;
                                Cellempty1.BorderWidthBottom = 0;
                                Cellempty1.BorderColor = BaseColor.BLACK;


                                PdfPCell Cellempty6 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellempty6.HorizontalAlignment = 2;
                                Cellempty6.Colspan = 1;
                                Cellempty6.BorderWidthTop = 0;
                                Cellempty6.BorderWidthRight = 0f;
                                Cellempty6.BorderWidthLeft = .2f;
                                Cellempty6.BorderWidthBottom = 0;

                                Cellempty6.BorderColor = BaseColor.BLACK;

                                PdfPCell Cellempty7 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellempty7.HorizontalAlignment = 2;
                                Cellempty7.Colspan = 1;
                                Cellempty7.BorderWidthTop = 0;
                                Cellempty7.BorderWidthRight = 0.2f;
                                Cellempty7.BorderWidthLeft = 0.2f;
                                Cellempty7.BorderWidthBottom = 0;
                                Cellempty7.BorderColor = BaseColor.BLACK;

                                PdfPCell Cellempty2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellempty2.HorizontalAlignment = 2;
                                Cellempty2.Colspan = 1;
                                Cellempty2.BorderWidthTop = 0;
                                Cellempty2.BorderWidthRight = 0f;
                                Cellempty2.BorderWidthLeft = 0.2f;
                                Cellempty2.BorderWidthBottom = 0;
                                Cellempty2.BorderColor = BaseColor.BLACK;

                                PdfPCell Cellempty3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellempty3.HorizontalAlignment = 2;
                                Cellempty3.Colspan = 1;
                                Cellempty3.BorderWidthTop = 0;
                                Cellempty3.BorderWidthRight = 0f;
                                Cellempty3.BorderWidthLeft = 0.2f;
                                Cellempty3.BorderWidthBottom = 0;
                                Cellempty3.BorderColor = BaseColor.BLACK;

                                PdfPCell Cellempty4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellempty4.HorizontalAlignment = 2;
                                Cellempty4.Colspan = 1;
                                Cellempty4.BorderWidthTop = 0;
                                Cellempty4.BorderWidthRight = 0f;
                                Cellempty4.BorderWidthLeft = 0.2f;
                                Cellempty4.BorderWidthBottom = 0;
                                Cellempty4.BorderColor = BaseColor.BLACK;

                                PdfPCell Cellempty5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellempty5.HorizontalAlignment = 2;
                                Cellempty5.Colspan = 1;
                                Cellempty5.BorderWidthTop = 0;
                                Cellempty5.BorderWidthRight = 0.2f;
                                Cellempty5.BorderWidthLeft = 0.2f;
                                Cellempty5.BorderWidthBottom = 0;
                                Cellempty5.BorderColor = BaseColor.BLACK;



                                if (dtunitbillbreakup.Rows.Count == 1)
                                {
                                    #region For cell count

                                    if (!bIncludeST)
                                    {
                                        for (i = 0; i < 13; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                            //table.AddCell(Cellempty4);
                                            //table.AddCell(Cellempty5);
                                        }
                                    }
                                    else
                                    {
                                        for (i = 0; i < 10; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }



                                    #endregion
                                }
                                if (dtunitbillbreakup.Rows.Count == 2)
                                {
                                    #region For cell count

                                    if (!bIncludeST)
                                    {
                                        for (i = 0; i < 12; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }
                                    else
                                    {

                                        for (i = 0; i < 10; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }



                                    #endregion
                                }
                                if (dtunitbillbreakup.Rows.Count == 3)
                                {
                                    #region For cell count

                                    if (!bIncludeST)
                                    {
                                        for (i = 0; i < 11; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }
                                    else
                                    {

                                        for (i = 0; i < 9; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }

                                    #endregion
                                }
                                if (dtunitbillbreakup.Rows.Count == 4)
                                {
                                    #region For cell count

                                    if (!bIncludeST)
                                    {
                                        for (i = 0; i < 10; i++)
                                        {
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        for (i = 0; i < 8; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }


                                    #endregion
                                }
                                if (dtunitbillbreakup.Rows.Count == 5)
                                {
                                    #region For cell count

                                    if (!bIncludeST)
                                    {
                                        for (i = 0; i < 9; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        for (i = 0; i < 7; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }


                                    #endregion
                                }
                                if (dtunitbillbreakup.Rows.Count == 6)
                                {
                                    #region For cell count
                                    if (!bIncludeST)
                                    {
                                        for (i = 0; i < 8; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        for (i = 0; i < 6; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }


                                    #endregion
                                }
                                if (dtunitbillbreakup.Rows.Count == 7)
                                {
                                    #region For cell count
                                    if (!bIncludeST)
                                    {
                                        for (i = 0; i < 7; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        for (i = 0; i < 5; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }


                                    #endregion
                                }

                                if (dtunitbillbreakup.Rows.Count == 8)
                                {
                                    #region For cell count
                                    if (!bIncludeST)
                                    {
                                        for (i = 0; i < 6; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        for (i = 0; i < 4; i++)
                                        {
                                            //1
                                            table.AddCell(Cellempty);
                                            if (InvDescriptionVisible == "Y")
                                            {
                                                table.AddCell(Cellempty1);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                table.AddCell(Cellempty6);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                table.AddCell(Cellempty7);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                table.AddCell(Cellempty2);
                                            }
                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                table.AddCell(Cellempty3);
                                            }
                                            if (InvPayrateVisible == "Y")
                                            {
                                                table.AddCell(Cellempty4);
                                            }
                                            if (InvAmountVisible == "Y")
                                            {
                                                table.AddCell(Cellempty5);
                                            }

                                        }
                                    }


                                    #endregion
                                }




                                #endregion

                                document.Add(table);

                                #endregion

                                #region for Total Values

                                PdfPTable tempTable22 = new PdfPTable(colCount);
                                tempTable22.TotalWidth = 580f;
                                tempTable22.LockedWidth = true;
                                float[] tempWidth22 = new float[] { };
                                if (colCount == 8)
                                {
                                    tempWidth22 = new float[] { 1f, 6f, 2f, 2f, 2f, 2.2f, 2f, 2.7f };
                                }

                                if (colCount == 7)
                                {
                                    tempWidth22 = new float[] { 1f, 6f, 2f, 2f, 2.2f, 2f, 2.7f };
                                }

                                if (colCount == 6)
                                {
                                    tempWidth22 = new float[] { 1f, 6f, 2f, 2.2f, 2f, 2.7f };
                                }

                                if (colCount == 5)
                                {
                                    tempWidth22 = new float[] { 1f, 6f, 2f, 2.2f, 2.7f };
                                }

                                if (colCount == 4)
                                {
                                    tempWidth22 = new float[] { 1f, 6f, 2.2f, 2.7f };
                                }

                                if (colCount == 3)
                                {
                                    tempWidth22 = new float[] { 1f, 6f, 2.7f };
                                }
                                tempTable22.SetWidths(tempWidth22);

                                #region


                                if (RelChrgAmt > 0)
                                {

                                    PdfPCell celldz5 = new PdfPCell(new Phrase("1/6 Reliever Charges", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldz5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    celldz5.Colspan = colCount - 1;
                                    celldz5.BorderWidthBottom = 0;
                                    celldz5.BorderWidthLeft = .2f;
                                    celldz5.BorderWidthTop = 0;
                                    celldz5.BorderWidthRight = .2f;
                                    celldz5.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldz5);

                                    PdfPCell celldz6 = new PdfPCell(new Phrase(" " + RelChrgAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldz6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldz6.BorderWidthBottom = 0;
                                    celldz6.BorderWidthLeft = .2f;
                                    celldz6.BorderWidthTop = 0;
                                    celldz6.BorderWidthRight = .2f;
                                    celldz6.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldz6);
                                }

                                int Noofcolumns = 4;
                                int Noofcolumnsheading = 3;
                                if (colCount == 4)
                                {
                                    Noofcolumns = 2;
                                    Noofcolumnsheading = 1;
                                }

                                PdfPCell celldz1 = new PdfPCell(new Phrase(ExtraRemarks, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                celldz1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                celldz1.Colspan = colCount - Noofcolumns;
                                celldz1.BorderWidthBottom = 0;
                                celldz1.BorderWidthLeft = .2f;
                                celldz1.BorderWidthTop = 0.2f;
                                celldz1.BorderWidthRight = 0.2f;
                                celldz1.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(celldz1);

                                celldz1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldz1.Colspan = Noofcolumnsheading;
                                celldz1.BorderWidthBottom = 0;
                                celldz1.BorderWidthLeft = .2f;
                                celldz1.BorderWidthTop = .2f;
                                celldz1.BorderWidthRight = 0;
                                celldz1.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(celldz1);

                                PdfPCell celldz4 = new PdfPCell(new Phrase(" " + (totalamount - (PFAmt + ESIAmt)).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                celldz4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldz4.BorderWidthBottom = 0;
                                celldz4.BorderWidthLeft = 0.2f;
                                celldz4.BorderWidthTop = .2f;
                                celldz4.BorderWidthRight = .2f;
                                celldz4.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(celldz4);


                                if (PFAmt > 0)
                                {

                                    PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellCGST.Colspan = colCount - Noofcolumns;
                                    CellCGST.BorderWidthBottom = 0;
                                    CellCGST.BorderWidthLeft = .2f;
                                    CellCGST.BorderWidthTop = 0f;
                                    CellCGST.BorderWidthRight = 0.2f;
                                    // CellCGST.PaddingBottom = 5;
                                    // CellCGST.PaddingTop = 5;
                                    CellCGST.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellCGST);

                                    CellCGST = new PdfPCell(new Phrase("EPF Employer Share @ " + BpfPer + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                    CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellCGST.Colspan = Noofcolumnsheading;
                                    CellCGST.BorderWidthBottom = 0;
                                    CellCGST.BorderWidthLeft = .2f;
                                    CellCGST.BorderWidthTop = 0.2f;
                                    CellCGST.BorderWidthRight = 0f;
                                    // CellCGST.PaddingBottom = 5;
                                    // CellCGST.PaddingTop = 5;
                                    CellCGST.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellCGST);

                                    PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(PFAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellCGSTAmt.BorderWidthBottom = 0;
                                    CellCGSTAmt.BorderWidthLeft = 0.2f;
                                    CellCGSTAmt.BorderWidthTop = 0.2f;
                                    CellCGSTAmt.BorderWidthRight = .2f;
                                    CellCGSTAmt.BorderColor = BaseColor.BLACK;
                                    //CellCGSTAmt.PaddingBottom = 5;
                                    //CellCGSTAmt.PaddingTop = 5;
                                    tempTable22.AddCell(CellCGSTAmt);

                                }


                                if (ESIAmt > 0)
                                {

                                    PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellCGST.Colspan = colCount - Noofcolumns;
                                    CellCGST.BorderWidthBottom = 0;
                                    CellCGST.BorderWidthLeft = .2f;
                                    CellCGST.BorderWidthTop = 0f;
                                    CellCGST.BorderWidthRight = 0.2f;
                                    // CellCGST.PaddingBottom = 5;
                                    // CellCGST.PaddingTop = 5;
                                    CellCGST.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellCGST);

                                    CellCGST = new PdfPCell(new Phrase("ESI Employer Share @ " + BesiPer + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                    CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellCGST.Colspan = Noofcolumnsheading;
                                    CellCGST.BorderWidthBottom = 0;
                                    CellCGST.BorderWidthLeft = .2f;
                                    CellCGST.BorderWidthTop = 0.2f;
                                    CellCGST.BorderWidthRight = 0f;
                                    // CellCGST.PaddingBottom = 5;
                                    // CellCGST.PaddingTop = 5;
                                    CellCGST.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellCGST);

                                    PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(ESIAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellCGSTAmt.BorderWidthBottom = 0;
                                    CellCGSTAmt.BorderWidthLeft = 0.2f;
                                    CellCGSTAmt.BorderWidthTop = 0.2f;
                                    CellCGSTAmt.BorderWidthRight = .2f;
                                    CellCGSTAmt.BorderColor = BaseColor.BLACK;
                                    //CellCGSTAmt.PaddingBottom = 5;
                                    //CellCGSTAmt.PaddingTop = 5;
                                    tempTable22.AddCell(CellCGSTAmt);
                                }

                                #region When Extradata check is false and STcheck is false

                                if (Extradatacheck == true)
                                {
                                    if (machinarycost > 0)
                                    {
                                        if (STMachinary == true)
                                        {
                                            if (SCMachinary == true)
                                            {
                                                PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldzz.Colspan = colCount - Noofcolumns;
                                                celldzz.BorderWidthBottom = 0;
                                                celldzz.BorderWidthLeft = .2f;
                                                celldzz.BorderWidthTop = 0;
                                                celldzz.BorderWidthRight = 0.2f;
                                                celldzz.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldzz);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0;
                                                celldcst1.BorderWidthRight = .2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);


                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = .2f;
                                                celldcst2.BorderWidthTop = 0;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }
                                    if (materialcost > 0)
                                    {
                                        if (STMaterial == true)
                                        {
                                            if (SCMaterial == true)
                                            {
                                                PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldzz.Colspan = colCount - Noofcolumns;
                                                celldzz.BorderWidthBottom = 0;
                                                celldzz.BorderWidthLeft = .2f;
                                                celldzz.BorderWidthTop = 0;
                                                celldzz.BorderWidthRight = 0.2f;
                                                celldzz.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldzz);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0;
                                                celldcst1.BorderWidthRight = .2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = .2f;
                                                celldcst2.BorderWidthTop = 0;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }


                                    if (maintenancecost > 0)
                                    {
                                        if (STMaintenance == true)
                                        {
                                            if (SCMaintenance == true)
                                            {
                                                PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldzz.Colspan = colCount - Noofcolumns;
                                                celldzz.BorderWidthBottom = 0;
                                                celldzz.BorderWidthLeft = .2f;
                                                celldzz.BorderWidthTop = 0;
                                                celldzz.BorderWidthRight = 0.2f;
                                                celldzz.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldzz);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0;
                                                celldcst1.BorderWidthRight = .2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = .2f;
                                                celldcst2.BorderWidthTop = 0;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }

                                    if (extraonecost > 0)
                                    {
                                        if (STExtraone == true)
                                        {
                                            if (SCExtraone == true)
                                            {
                                                PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldzz.Colspan = colCount - Noofcolumns;
                                                celldzz.BorderWidthBottom = 0;
                                                celldzz.BorderWidthLeft = .2f;
                                                celldzz.BorderWidthTop = 0;
                                                celldzz.BorderWidthRight = 0.2f;
                                                celldzz.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldzz);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0;
                                                celldcst1.BorderWidthRight = .2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = .2f;
                                                celldcst2.BorderWidthTop = 0;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }
                                    if (extratwocost > 0)
                                    {
                                        if (STExtratwo == true)
                                        {
                                            if (SCExtratwo == true)
                                            {
                                                PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldzz.Colspan = colCount - Noofcolumns;
                                                celldzz.BorderWidthBottom = 0;
                                                celldzz.BorderWidthLeft = .2f;
                                                celldzz.BorderWidthTop = 0;
                                                celldzz.BorderWidthRight = 0.2f;
                                                celldzz.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldzz);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0;
                                                celldcst1.BorderWidthRight = .2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = .2f;
                                                celldcst2.BorderWidthTop = 0;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }

                                }

                                if (servicecharge > 0)//bSCType == true)
                                {
                                    decimal scharge = servicecharge;
                                    if (scharge > 0)
                                    {
                                        string SCharge = "";
                                        if (bSCType == false)
                                        {
                                            SCharge = ServiceCharge + "%";
                                        }
                                        else
                                        {
                                            SCharge = ServiceCharge;
                                        }


                                        PdfPCell Cellservice = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        Cellservice.Colspan = colCount - Noofcolumns;
                                        Cellservice.BorderWidthBottom = 0;
                                        Cellservice.BorderWidthLeft = .2f;
                                        Cellservice.BorderWidthTop = 0f;
                                        Cellservice.BorderWidthRight = 0.2f;
                                        Cellservice.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(Cellservice);

                                        Cellservice = new PdfPCell(new Phrase("Service Charges @ " + SCharge, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        Cellservice.Colspan = Noofcolumnsheading;
                                        Cellservice.BorderWidthBottom = 0;
                                        Cellservice.BorderWidthLeft = .2f;
                                        Cellservice.BorderWidthTop = 0;
                                        Cellservice.BorderWidthRight = 0f;
                                        Cellservice.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(Cellservice);

                                        Cellservice = new PdfPCell(new Phrase(servicecharge.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        Cellservice.BorderWidthBottom = 0;
                                        Cellservice.BorderWidthLeft = 0.2f;
                                        Cellservice.BorderWidthTop = 0;
                                        Cellservice.BorderWidthRight = .2f;
                                        Cellservice.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(Cellservice);
                                    }
                                }
                                #endregion

                                #region When Extra data is checked and STcheck is true and SCcheck is false

                                if (machinarycost > 0)
                                {
                                    if (STMachinary == true)
                                    {
                                        if (SCMachinary == false)
                                        {
                                            PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellIGST2.Colspan = colCount - Noofcolumns;
                                            CellIGST2.BorderWidthBottom = 0;
                                            CellIGST2.BorderWidthLeft = .2f;
                                            CellIGST2.BorderWidthTop = 0f;
                                            CellIGST2.BorderWidthRight = 0.2f;
                                            CellIGST2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellIGST2);

                                            PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst1.Colspan = Noofcolumnsheading;
                                            celldcst1.BorderWidthBottom = 0;
                                            celldcst1.BorderWidthLeft = .2f;
                                            celldcst1.BorderWidthTop = 0;
                                            celldcst1.BorderWidthRight = 0.2f;
                                            celldcst1.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst1);

                                            PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst2.BorderWidthBottom = 0;
                                            celldcst2.BorderWidthLeft = 0f;
                                            celldcst2.BorderWidthTop = 0;
                                            celldcst2.BorderWidthRight = .2f;
                                            celldcst2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst2);
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
                                        if (SCMaterial == false)
                                        {
                                            PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellIGST2.Colspan = colCount - Noofcolumns;
                                            CellIGST2.BorderWidthBottom = 0;
                                            CellIGST2.BorderWidthLeft = .2f;
                                            CellIGST2.BorderWidthTop = 0f;
                                            CellIGST2.BorderWidthRight = 0.2f;
                                            // CellCGST.PaddingBottom = 5;
                                            // CellCGST.PaddingTop = 5;
                                            CellIGST2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellIGST2);

                                            PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst1.Colspan = Noofcolumnsheading;
                                            celldcst1.BorderWidthBottom = 0;
                                            celldcst1.BorderWidthLeft = .2f;
                                            celldcst1.BorderWidthTop = 0;
                                            celldcst1.BorderWidthRight = 0.2f;
                                            celldcst1.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst1);

                                            PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst2.BorderWidthBottom = 0;
                                            celldcst2.BorderWidthLeft = 0f;
                                            celldcst2.BorderWidthTop = 0;
                                            celldcst2.BorderWidthRight = .2f;
                                            celldcst2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst2);
                                        }
                                    }
                                }
                                if (maintenancecost > 0)
                                {
                                    if (STMaintenance == true)
                                    {
                                        if (SCMaintenance == false)
                                        {
                                            PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellIGST2.Colspan = colCount - Noofcolumns;
                                            CellIGST2.BorderWidthBottom = 0;
                                            CellIGST2.BorderWidthLeft = .2f;
                                            CellIGST2.BorderWidthTop = 0f;
                                            CellIGST2.BorderWidthRight = 0.2f;
                                            // CellCGST.PaddingBottom = 5;
                                            // CellCGST.PaddingTop = 5;
                                            CellIGST2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellIGST2);

                                            PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst1.Colspan = Noofcolumnsheading;
                                            celldcst1.BorderWidthBottom = 0;
                                            celldcst1.BorderWidthLeft = .2f;
                                            celldcst1.BorderWidthTop = 0;
                                            celldcst1.BorderWidthRight = 0.2f;
                                            celldcst1.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst1);

                                            PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst2.BorderWidthBottom = 0;
                                            celldcst2.BorderWidthLeft = 0f;
                                            celldcst2.BorderWidthTop = 0;
                                            celldcst2.BorderWidthRight = .2f;
                                            celldcst2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst2);
                                        }
                                    }
                                }

                                if (extraonecost > 0)
                                {
                                    if (STExtraone == true)
                                    {
                                        if (SCExtraone == false)
                                        {
                                            PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellIGST2.Colspan = colCount - Noofcolumns;
                                            CellIGST2.BorderWidthBottom = 0;
                                            CellIGST2.BorderWidthLeft = .2f;
                                            CellIGST2.BorderWidthTop = 0f;
                                            CellIGST2.BorderWidthRight = 0.2f;
                                            // CellCGST.PaddingBottom = 5;
                                            // CellCGST.PaddingTop = 5;
                                            CellIGST2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellIGST2);

                                            PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst1.Colspan = Noofcolumnsheading;
                                            celldcst1.BorderWidthBottom = 0;
                                            celldcst1.BorderWidthLeft = .2f;
                                            celldcst1.BorderWidthTop = 0;
                                            celldcst1.BorderWidthRight = 0.2f;
                                            celldcst1.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst1);

                                            PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst2.BorderWidthBottom = 0;
                                            celldcst2.BorderWidthLeft = 0f;
                                            celldcst2.BorderWidthTop = 0;
                                            celldcst2.BorderWidthRight = .2f;
                                            celldcst2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst2);
                                        }
                                    }
                                }
                                if (extratwocost > 0)
                                {
                                    if (STExtratwo == true)
                                    {
                                        if (SCExtratwo == false)
                                        {
                                            PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellIGST2.Colspan = colCount - Noofcolumns;
                                            CellIGST2.BorderWidthBottom = 0;
                                            CellIGST2.BorderWidthLeft = .2f;
                                            CellIGST2.BorderWidthTop = 0f;
                                            CellIGST2.BorderWidthRight = 0.2f;
                                            // CellCGST.PaddingBottom = 5;
                                            // CellCGST.PaddingTop = 5;
                                            CellIGST2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellIGST2);

                                            PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst1.Colspan = Noofcolumnsheading;
                                            celldcst1.BorderWidthBottom = 0;
                                            celldcst1.BorderWidthLeft = .2f;
                                            celldcst1.BorderWidthTop = 0;
                                            celldcst1.BorderWidthRight = 0.2f;
                                            celldcst1.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst1);

                                            PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldcst2.BorderWidthBottom = 0;
                                            celldcst2.BorderWidthLeft = 0f;
                                            celldcst2.BorderWidthTop = 0;
                                            celldcst2.BorderWidthRight = .2f;
                                            celldcst2.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldcst2);
                                        }
                                    }
                                }

                                #endregion

                                #endregion



                                decimal GSTDiscounts = 0;

                                if (STDiscountone == true)
                                {
                                    if (discountone > 0)
                                    {

                                        PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellbbCGST.Colspan = colCount - Noofcolumns;
                                        CellbbCGST.BorderWidthBottom = 0;
                                        CellbbCGST.BorderWidthLeft = .2f;
                                        CellbbCGST.BorderWidthTop = 0f;
                                        CellbbCGST.BorderWidthRight = 0.2f;
                                        CellbbCGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellbbCGST);

                                        CellbbCGST = new PdfPCell(new Phrase(discountonetitle, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellbbCGST.Colspan = Noofcolumnsheading;
                                        CellbbCGST.BorderWidthBottom = 0;
                                        CellbbCGST.BorderWidthLeft = .2f;
                                        CellbbCGST.BorderWidthTop = 0.2f;
                                        CellbbCGST.BorderWidthRight = 0f;
                                        CellbbCGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellbbCGST);

                                        CellbbCGST = new PdfPCell(new Phrase(discountone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellbbCGST.BorderWidthBottom = 0;
                                        CellbbCGST.BorderWidthLeft = 0.2f;
                                        CellbbCGST.BorderWidthTop = 0.2f;
                                        CellbbCGST.BorderWidthRight = .2f;
                                        CellbbCGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellbbCGST);
                                        GSTDiscounts += discountone;



                                    }
                                }

                                if (STDiscounttwo == true)
                                {
                                    if (discounttwo > 0)
                                    {
                                        PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellbbCGST.Colspan = colCount - Noofcolumns;
                                        CellbbCGST.BorderWidthBottom = 0;
                                        CellbbCGST.BorderWidthLeft = .2f;
                                        CellbbCGST.BorderWidthTop = 0f;
                                        CellbbCGST.BorderWidthRight = 0.2f;
                                        CellbbCGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellbbCGST);

                                        CellbbCGST = new PdfPCell(new Phrase(discounttwotitle, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellbbCGST.Colspan = Noofcolumnsheading;
                                        CellbbCGST.BorderWidthBottom = 0;
                                        CellbbCGST.BorderWidthLeft = .2f;
                                        CellbbCGST.BorderWidthTop = 0.2f;
                                        CellbbCGST.BorderWidthRight = 0f;
                                        CellbbCGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellbbCGST);

                                        CellbbCGST = new PdfPCell(new Phrase(discounttwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellbbCGST.BorderWidthBottom = 0;
                                        CellbbCGST.BorderWidthLeft = 0.2f;
                                        CellbbCGST.BorderWidthTop = 0.2f;
                                        CellbbCGST.BorderWidthRight = .2f;
                                        CellbbCGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellbbCGST);
                                        GSTDiscounts += discounttwo;
                                    }
                                }

                                if (((Grandtotal - (CGST + SGST + IGST)) - totalamount) > 0)
                                {
                                    PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellbbCGST.Colspan = colCount - Noofcolumns;
                                    CellbbCGST.BorderWidthBottom = 0;
                                    CellbbCGST.BorderWidthLeft = .2f;
                                    CellbbCGST.BorderWidthTop = 0f;
                                    CellbbCGST.BorderWidthRight = 0.2f;
                                    CellbbCGST.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellbbCGST);

                                    CellbbCGST = new PdfPCell(new Phrase("Total Before Tax", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellbbCGST.Colspan = Noofcolumnsheading;
                                    CellbbCGST.BorderWidthBottom = 0;
                                    CellbbCGST.BorderWidthLeft = .2f;
                                    CellbbCGST.BorderWidthTop = 0.2f;
                                    CellbbCGST.BorderWidthRight = 0f;
                                    CellbbCGST.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellbbCGST);

                                    CellbbCGST = new PdfPCell(new Phrase(((Grandtotal - (CGST + SGST + IGST) - GSTDiscounts)).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                    CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellbbCGST.BorderWidthBottom = 0;
                                    CellbbCGST.BorderWidthLeft = 0.2f;
                                    CellbbCGST.BorderWidthTop = 0.2f;
                                    CellbbCGST.BorderWidthRight = .2f;
                                    CellbbCGST.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellbbCGST);
                                }



                                #region for taxes

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
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldd2 = new PdfPCell(new Phrase("Service Tax @ " + scpercent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldd2.Colspan = Noofcolumnsheading;
                                        celldd2.BorderWidthBottom = 0;
                                        celldd2.BorderWidthLeft = .2f;
                                        celldd2.BorderWidthTop = 0.2f;
                                        celldd2.BorderWidthRight = 0f;
                                        //celldd2.PaddingBottom = 5;
                                        //celldd2.PaddingTop = 5;
                                        celldd2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldd2);


                                        PdfPCell celldd4 = new PdfPCell(new Phrase(servicetax.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldd4.BorderWidthBottom = 0;
                                        celldd4.BorderWidthLeft = 0.2f;
                                        celldd4.BorderWidthTop = 0.2f;
                                        celldd4.BorderWidthRight = .2f;
                                        celldd4.BorderColor = BaseColor.BLACK;
                                        //celldd4.PaddingBottom = 5;
                                        //celldd4.PaddingTop = 5;
                                        tempTable22.AddCell(celldd4);

                                    }

                                    if (sbcess > 0)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        string SBCESSPresent = DtTaxes.Rows[0]["SBCess"].ToString();
                                        PdfPCell celldd2 = new PdfPCell(new Phrase("Swachh Bharat Cess @ " + SBCESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldd2.Colspan = Noofcolumnsheading;
                                        celldd2.BorderWidthBottom = 0;
                                        celldd2.BorderWidthLeft = .2f;
                                        celldd2.BorderWidthTop = 0.2f;
                                        celldd2.BorderWidthRight = 0f;
                                        celldd2.BorderColor = BaseColor.BLACK;
                                        // celldd2.PaddingBottom = 5;
                                        // celldd2.PaddingTop = 5;
                                        tempTable22.AddCell(celldd2);


                                        PdfPCell celldd4 = new PdfPCell(new Phrase(sbcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldd4.BorderWidthBottom = 0;
                                        celldd4.BorderWidthLeft = 0.2f;
                                        celldd4.BorderWidthTop = 0.2f;
                                        celldd4.BorderWidthRight = .2f;
                                        celldd4.BorderColor = BaseColor.BLACK;
                                        //celldd4.PaddingBottom = 5;
                                        //celldd4.PaddingTop = 5;
                                        tempTable22.AddCell(celldd4);

                                    }


                                    if (kkcess > 0)
                                    {

                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        string KKCESSPresent = DtTaxes.Rows[0]["KKCess"].ToString();
                                        PdfPCell Cellmtcesskk1 = new PdfPCell(new Phrase("Krishi Kalyan Cess @ " + KKCESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellmtcesskk1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        Cellmtcesskk1.Colspan = Noofcolumnsheading;
                                        Cellmtcesskk1.BorderWidthBottom = 0;
                                        Cellmtcesskk1.BorderWidthLeft = .2f;
                                        Cellmtcesskk1.BorderWidthTop = 0.2f;
                                        Cellmtcesskk1.BorderWidthRight = 0f;
                                        // celldd2.PaddingBottom = 5;
                                        // celldd2.PaddingTop = 5;
                                        Cellmtcesskk1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(Cellmtcesskk1);

                                        PdfPCell Cellmtcesskk2 = new PdfPCell(new Phrase(kkcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellmtcesskk2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        Cellmtcesskk2.BorderWidthBottom = 0;
                                        Cellmtcesskk2.BorderWidthLeft = 0.2f;
                                        Cellmtcesskk2.BorderWidthTop = 0.2f;
                                        Cellmtcesskk2.BorderWidthRight = .2f;
                                        Cellmtcesskk2.BorderColor = BaseColor.BLACK;
                                        //celldd4.PaddingBottom = 5;
                                        //celldd4.PaddingTop = 5;
                                        tempTable22.AddCell(Cellmtcesskk2);

                                    }

                                    #region for GST as on 17-6-2017

                                    if (CGST > 0)
                                    {
                                        PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellCGST.Colspan = colCount - Noofcolumns;
                                        CellCGST.BorderWidthBottom = 0;
                                        CellCGST.BorderWidthLeft = .2f;
                                        CellCGST.BorderWidthTop = 0f;
                                        CellCGST.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellCGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellCGST);

                                        CellCGST = new PdfPCell(new Phrase(CGSTAlias + " @ " + CGSTPrc + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                        CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellCGST.Colspan = Noofcolumnsheading;
                                        CellCGST.BorderWidthBottom = 0;
                                        CellCGST.BorderWidthLeft = .2f;
                                        CellCGST.BorderWidthTop = 0.2f;
                                        CellCGST.BorderWidthRight = 0f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellCGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellCGST);

                                        PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(CGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellCGSTAmt.BorderWidthBottom = 0;
                                        CellCGSTAmt.BorderWidthLeft = 0.2f;
                                        CellCGSTAmt.BorderWidthTop = 0.2f;
                                        CellCGSTAmt.BorderWidthRight = .2f;
                                        CellCGSTAmt.BorderColor = BaseColor.BLACK;
                                        //CellCGSTAmt.PaddingBottom = 5;
                                        //CellCGSTAmt.PaddingTop = 5;
                                        tempTable22.AddCell(CellCGSTAmt);

                                    }


                                    if (SGST > 0)
                                    {
                                        PdfPCell CellSGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellSGST.Colspan = colCount - Noofcolumns;
                                        CellSGST.BorderWidthBottom = 0;
                                        CellSGST.BorderWidthLeft = .2f;
                                        CellSGST.BorderWidthTop = 0f;
                                        CellSGST.BorderWidthRight = 0.2f;
                                        // CellSGST.PaddingBottom = 5;
                                        // CellSGST.PaddingTop = 5;
                                        CellSGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellSGST);

                                        CellSGST = new PdfPCell(new Phrase(SGSTAlias + " @ " + SGSTPrc + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                        CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellSGST.Colspan = Noofcolumnsheading;
                                        CellSGST.BorderWidthBottom = 0;
                                        CellSGST.BorderWidthLeft = .2f;
                                        CellSGST.BorderWidthTop = 0.2f;
                                        CellSGST.BorderWidthRight = 0f;
                                        // CellSGST.PaddingBottom = 5;
                                        // CellSGST.PaddingTop = 5;
                                        CellSGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellSGST);

                                        PdfPCell CellSGSTAmt = new PdfPCell(new Phrase(SGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellSGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellSGSTAmt.BorderWidthBottom = 0;
                                        CellSGSTAmt.BorderWidthLeft = 0.2f;
                                        CellSGSTAmt.BorderWidthTop = 0.2f;
                                        CellSGSTAmt.BorderWidthRight = .2f;
                                        CellSGSTAmt.BorderColor = BaseColor.BLACK;
                                        //CellSGSTAmt.PaddingBottom = 5;
                                        //CellSGSTAmt.PaddingTop = 5;
                                        tempTable22.AddCell(CellSGSTAmt);


                                    }

                                    if (IGST > 0)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell CellIGST = new PdfPCell(new Phrase(IGSTAlias + " @ " + IGSTPrc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST.Colspan = Noofcolumnsheading;
                                        CellIGST.BorderWidthBottom = 0;
                                        CellIGST.BorderWidthLeft = .2f;
                                        CellIGST.BorderWidthTop = 0.2f;
                                        CellIGST.BorderWidthRight = 0f;
                                        // CellIGST.PaddingBottom = 5;
                                        // CellIGST.PaddingTop = 5;
                                        CellIGST.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST);

                                        PdfPCell CellIGSTAmt = new PdfPCell(new Phrase(IGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGSTAmt.BorderWidthBottom = 0;
                                        CellIGSTAmt.BorderWidthLeft = 0.2f;
                                        CellIGSTAmt.BorderWidthTop = 0.2f;
                                        CellIGSTAmt.BorderWidthRight = .2f;
                                        CellIGSTAmt.BorderColor = BaseColor.BLACK;
                                        //CellIGSTAmt.PaddingBottom = 5;
                                        //CellIGSTAmt.PaddingTop = 5;
                                        tempTable22.AddCell(CellIGSTAmt);


                                    }

                                    if (Cess1 > 0)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell CellCess1 = new PdfPCell(new Phrase(Cess1Alias + " @ " + Cess1Prc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellCess1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellCess1.Colspan = Noofcolumnsheading;
                                        CellCess1.BorderWidthBottom = 0;
                                        CellCess1.BorderWidthLeft = .2f;
                                        CellCess1.BorderWidthTop = 0.2f;
                                        CellCess1.BorderWidthRight = 0f;
                                        // CellCess1.PaddingBottom = 5;
                                        // CellCess1.PaddingTop = 5;
                                        CellCess1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellCess1);

                                        PdfPCell CellCess1Amt = new PdfPCell(new Phrase(Cess1.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellCess1Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellCess1Amt.BorderWidthBottom = 0;
                                        CellCess1Amt.BorderWidthLeft = 0.2f;
                                        CellCess1Amt.BorderWidthTop = 0.2f;
                                        CellCess1Amt.BorderWidthRight = .2f;
                                        CellCess1Amt.BorderColor = BaseColor.BLACK;
                                        //CellCess1Amt.PaddingBottom = 5;
                                        //CellCess1Amt.PaddingTop = 5;
                                        tempTable22.AddCell(CellCess1Amt);

                                    }


                                    if (Cess2 > 0)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell CellCess2 = new PdfPCell(new Phrase(Cess2Alias + " @ " + Cess2Prc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellCess2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellCess2.Colspan = Noofcolumnsheading;
                                        CellCess2.BorderWidthBottom = 0;
                                        CellCess2.BorderWidthLeft = .2f;
                                        CellCess2.BorderWidthTop = 0.2f;
                                        CellCess2.BorderWidthRight = 0f;
                                        // CellCess2.PaddingBottom = 5;
                                        // CellCess2.PaddingTop = 5;
                                        CellCess2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellCess2);

                                        PdfPCell CellCess2Amt = new PdfPCell(new Phrase(Cess2.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellCess2Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellCess2Amt.BorderWidthBottom = 0;
                                        CellCess2Amt.BorderWidthLeft = 0.2f;
                                        CellCess2Amt.BorderWidthTop = 0.2f;
                                        CellCess2Amt.BorderWidthRight = .2f;
                                        CellCess2Amt.BorderColor = BaseColor.BLACK;
                                        //CellCess2Amt.PaddingBottom = 5;
                                        //CellCess2Amt.PaddingTop = 5;
                                        tempTable22.AddCell(CellCess2Amt);

                                    }

                                    #endregion for GST

                                    if (cess > 0)
                                    {

                                        string CESSPresent = DtTaxes.Rows[0]["Cess"].ToString();
                                        PdfPCell celldd2 = new PdfPCell(new Phrase("CESS @ " + CESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldd2.Colspan = colCount - 1;
                                        celldd2.BorderWidthBottom = 0;
                                        celldd2.BorderWidthLeft = .2f;
                                        celldd2.BorderWidthTop = 0.2f;
                                        celldd2.BorderWidthRight = 0f;
                                        celldd2.BorderColor = BaseColor.BLACK;
                                        //celldd2.PaddingBottom = 5;
                                        //celldd2.PaddingTop = 5;
                                        tempTable22.AddCell(celldd2);


                                        PdfPCell celldd4 = new PdfPCell(new Phrase(cess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldd4.BorderWidthBottom = 0;
                                        celldd4.BorderWidthLeft = 0.2f;
                                        celldd4.BorderWidthTop = 0.2f;
                                        celldd4.BorderWidthRight = .2f;
                                        celldd4.BorderColor = BaseColor.BLACK;
                                        //celldd4.PaddingBottom = 5;
                                        //celldd4.PaddingTop = 5;
                                        tempTable22.AddCell(celldd4);

                                    }

                                    if (shecess > 0)
                                    {


                                        string SHECESSPresent = DtTaxes.Rows[0]["shecess"].ToString();
                                        PdfPCell celldf2 = new PdfPCell(new Phrase("S&H Ed.CESS @ " + SHECESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldf2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldf2.Colspan = colCount - 1;
                                        celldf2.BorderWidthBottom = 0;
                                        celldf2.BorderWidthLeft = .2f;
                                        celldf2.BorderWidthTop = 0.2f;
                                        celldf2.BorderWidthRight = 0f;
                                        celldf2.BorderColor = BaseColor.BLACK;
                                        //celldf2.PaddingBottom = 5;
                                        //celldf2.PaddingTop = 5;
                                        tempTable22.AddCell(celldf2);


                                        PdfPCell celldf4 = new PdfPCell(new Phrase(shecess.ToString("0.00"),
                                            FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldf4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldf4.BorderWidthBottom = 0;
                                        celldf4.BorderWidthLeft = 0.2f;
                                        celldf4.BorderWidthTop = 0.2f;
                                        celldf4.BorderWidthRight = .2f;
                                        celldf4.BorderColor = BaseColor.BLACK;
                                        ////celldf4.PaddingBottom = 5;
                                        //celldf4.PaddingTop = 5;
                                        //celldf4.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldf4);
                                    }
                                    #region When Extra data is checked and STcheck is false and SCcheck is true

                                    if (machinarycost > 0)
                                    {
                                        if (STMachinary == false)
                                        {
                                            if (SCMachinary == true)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }


                                    }
                                    if (materialcost > 0)
                                    {
                                        if (STMaterial == false)
                                        {
                                            if (SCMaterial == true)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }
                                    if (maintenancecost > 0)
                                    {
                                        if (STMaintenance == false)
                                        {
                                            if (SCMaintenance == true)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }

                                    if (extraonecost > 0)
                                    {
                                        if (STExtraone == false)
                                        {
                                            if (SCExtraone == true)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }
                                    if (extratwocost > 0)
                                    {
                                        if (STExtratwo == false)
                                        {
                                            if (SCExtratwo == true)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }

                                    #endregion



                                    #region When Extra data is checked and STcheck is false and SCcheck is true

                                    if (machinarycost > 0)
                                    {
                                        if (STMachinary == false)
                                        {
                                            if (SCMachinary == false)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
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
                                            if (SCMaterial == false)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }
                                    if (maintenancecost > 0)
                                    {
                                        if (STMaintenance == false)
                                        {
                                            if (SCMaintenance == false)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }

                                    if (extraonecost > 0)
                                    {
                                        if (STExtraone == false)
                                        {
                                            if (SCExtraone == false)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }
                                    if (extratwocost > 0)
                                    {
                                        if (STExtratwo == false)
                                        {
                                            if (SCExtratwo == false)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst1.Colspan = Noofcolumnsheading;
                                                celldcst1.BorderWidthBottom = 0;
                                                celldcst1.BorderWidthLeft = .2f;
                                                celldcst1.BorderWidthTop = 0.2f;
                                                celldcst1.BorderWidthRight = 0.2f;
                                                celldcst1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst1);

                                                PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldcst2.BorderWidthBottom = 0;
                                                celldcst2.BorderWidthLeft = 0f;
                                                celldcst2.BorderWidthTop = 0.2f;
                                                celldcst2.BorderWidthRight = .2f;
                                                celldcst2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldcst2);
                                            }
                                        }
                                    }

                                    #endregion

                                    decimal totaltaxamt = 0;
                                    totaltaxamt = CGST + SGST + IGST;
                                    if (totaltaxamt > 0)
                                    {
                                        PdfPCell celldfTax2 = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldfTax2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldfTax2.Colspan = colCount - 1;
                                        celldfTax2.BorderWidthBottom = 0;
                                        celldfTax2.BorderWidthLeft = .2f;
                                        celldfTax2.BorderWidthTop = 0;
                                        celldfTax2.BorderWidthRight = 0f;
                                        celldfTax2.BorderColor = BaseColor.BLACK;
                                        //celldfTax2.PaddingBottom = 5;
                                        //celldfTax2.PaddingTop = 5;
                                        //tempTable22.AddCell(celldfTax2);

                                        PdfPCell celldf4tax = new PdfPCell(new Phrase(totaltaxamt.ToString("#,##0.00"),
                                            FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldf4tax.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldf4tax.BorderWidthBottom = 0;
                                        celldf4tax.BorderWidthLeft = 0.2f;
                                        celldf4tax.BorderWidthTop = 0;
                                        celldf4tax.BorderWidthRight = .2f;
                                        celldf4tax.BorderColor = BaseColor.BLACK;
                                        ////celldf4tax.PaddingBottom = 5;
                                        //celldf4tax.PaddingTop = 5;
                                        //celldf4tax.BorderColor = BaseColor.BLACK;
                                        //tempTable22.AddCell(celldf4tax);
                                    }
                                }


                                #endregion for taxes

                                decimal GrandTotalVal = Grandtotal;

                                decimal GrandtotalRoundOff = Math.Round(GrandTotalVal, 0);
                                decimal RoundOff = (GrandtotalRoundOff - GrandTotalVal);
                                decimal GrandtotalValue = (GrandTotalVal + RoundOff);

                                PdfPCell cellgrandto = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                cellgrandto.Colspan = colCount - Noofcolumns;
                                cellgrandto.BorderWidthBottom = 0;
                                cellgrandto.BorderWidthLeft = .2f;
                                cellgrandto.BorderWidthTop = 0f;
                                cellgrandto.BorderWidthRight = 0.2f;
                                cellgrandto.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(cellgrandto);

                                cellgrandto = new PdfPCell(new Phrase("Round off", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                cellgrandto.Colspan = Noofcolumnsheading;
                                cellgrandto.BorderWidthBottom = 0;
                                cellgrandto.BorderWidthLeft = .2f;
                                cellgrandto.BorderWidthTop = 0.2f;
                                cellgrandto.BorderWidthRight = 0f;
                                cellgrandto.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(cellgrandto);

                                cellgrandto = new PdfPCell(new Phrase(RoundOff.ToString("N2"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                cellgrandto.BorderWidthBottom = 0;
                                cellgrandto.BorderWidthLeft = 0.2f;
                                cellgrandto.BorderWidthTop = 0.2f;
                                cellgrandto.BorderWidthRight = .2f;
                                cellgrandto.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(cellgrandto);

                                cellgrandto = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                cellgrandto.Colspan = colCount - Noofcolumns;
                                cellgrandto.BorderWidthBottom = 0;
                                cellgrandto.BorderWidthLeft = .2f;
                                cellgrandto.BorderWidthTop = 0f;
                                cellgrandto.BorderWidthRight = 0.2f;
                                cellgrandto.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(cellgrandto);

                                cellgrandto = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                cellgrandto.Colspan = Noofcolumnsheading;
                                cellgrandto.BorderWidthBottom = 0;
                                cellgrandto.BorderWidthLeft = .2f;
                                cellgrandto.BorderWidthTop = 0.2f;
                                cellgrandto.BorderWidthRight = 0f;
                                cellgrandto.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(cellgrandto);

                                cellgrandto = new PdfPCell(new Phrase(GrandtotalValue.ToString("N2"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                cellgrandto.BorderWidthBottom = 0;
                                cellgrandto.BorderWidthLeft = 0.2f;
                                cellgrandto.BorderWidthTop = 0.2f;
                                cellgrandto.BorderWidthRight = .2f;
                                cellgrandto.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(cellgrandto);


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

                                if (GSTLineitem == true)
                                {
                                    #region FooterTable
                                    PdfPTable tablev = new PdfPTable(7);
                                    tablev.TotalWidth = 580f;
                                    tablev.LockedWidth = true;
                                    float[] widthss = new float[] { 1.5f, 2.5f, 2f, 2f, 2f, 2f, 3f };
                                    tablev.SetWidths(widthss);

                                    if ((CGST + SGST) > 0)
                                    {
                                        cell = new PdfPCell(new Phrase("HSN Code", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthLeft = .2f;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Taxable Value", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Central Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 2;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("State Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 2;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);
                                    }
                                    else
                                    {

                                        cell = new PdfPCell(new Phrase("HSN Code", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthLeft = .2f;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Taxable Value", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 2;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Integrated Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 1.5f;
                                        cell.Colspan = 2;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 1.5f;
                                        cell.Colspan = 2;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);
                                    }





                                    //2ndrow
                                    if ((CGST + SGST) > 0)
                                    {

                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = .2f;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);


                                        cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);
                                    }
                                    else
                                    {
                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = .2f;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 2;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 1.5f;
                                        cell.PaddingBottom = 3;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 1.5f;
                                        cell.PaddingBottom = 3;
                                        cell.Colspan = 2;
                                        tablev.AddCell(cell);
                                    }


                                    DataTable DtDesgnWise = null;
                                    var SPDesgnWise = "DesginWiseGSTAmounts";
                                    Hashtable htDesgnWise = new Hashtable();
                                    htDesgnWise.Add("@ClientID", lblclientid.Text);
                                    htDesgnWise.Add("@Month", month);
                                    htDesgnWise.Add("@BillType", ddlBillType.SelectedIndex);
                                    htDesgnWise.Add("@BillNo", lblbillno.Text);

                                    DtDesgnWise = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPDesgnWise, htDesgnWise);


                                    decimal TotalTaxableval = 0;
                                    decimal CGSTAmt = 0;
                                    decimal SGSTAmt = 0;
                                    decimal IGSTAmt = 0;
                                    decimal GrandTotalTaxableval = 0;
                                    decimal GrandCGSTAmt = 0;
                                    decimal GrandSGSTAmt = 0;
                                    decimal GrandIGSTAmt = 0;
                                    decimal CGSTRate = 0;
                                    decimal SGSTRate = 0;
                                    decimal IGSTRate = 0;
                                    string HSNCode = "";

                                    if (DtDesgnWise.Rows.Count > 0)
                                    {
                                        for (int k = 0; k < DtDesgnWise.Rows.Count; k++)
                                        {
                                            TotalTaxableval = decimal.Parse(DtDesgnWise.Rows[k]["TotalTaxableval"].ToString());
                                            CGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["CGSTAmt"].ToString());
                                            SGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["SGSTAmt"].ToString());
                                            IGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["IGSTAmt"].ToString());
                                            CGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["CGSTPrc"].ToString());
                                            SGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["SGSTPrc"].ToString());
                                            IGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["IGSTPrc"].ToString());
                                            HSNCode = DtDesgnWise.Rows[k]["HSNCode"].ToString();

                                            if ((CGST + SGST) > 0)
                                            {
                                                cell = new PdfPCell(new Phrase(HSNCode, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = .2f;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase(TotalTaxableval.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);
                                                GrandTotalTaxableval += (TotalTaxableval);

                                                cell = new PdfPCell(new Phrase(CGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase(CGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);
                                                GrandCGSTAmt += CGSTAmt;


                                                cell = new PdfPCell(new Phrase(SGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase(SGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);
                                                GrandSGSTAmt += SGSTAmt;

                                                cell = new PdfPCell(new Phrase((GrandCGSTAmt + GrandSGSTAmt).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);
                                            }
                                            else
                                            {
                                                cell = new PdfPCell(new Phrase(HSNCode, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = .2f;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase(TotalTaxableval.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 2;
                                                tablev.AddCell(cell);
                                                GrandTotalTaxableval += (TotalTaxableval);


                                                cell = new PdfPCell(new Phrase(IGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase(IGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.PaddingBottom = 3;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);
                                                GrandIGSTAmt += IGSTAmt;

                                                cell = new PdfPCell(new Phrase(GrandIGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.PaddingBottom = 3;
                                                cell.Colspan = 2;
                                                tablev.AddCell(cell);
                                            }



                                        }


                                        decimal TotalAmount = GrandCGSTAmt + GrandIGSTAmt + GrandSGSTAmt;

                                        string GTotal = TotalAmount.ToString("0.00");
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
                                            inwords = " Rupees " + rupee + "" + paise + " Paise Only /-";

                                        }
                                        else
                                        {
                                            rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                                            inwords = " Rupees " + rupee + " Only /-";
                                        }


                                        cell = new PdfPCell(new Phrase("Total Tax Amount (in words) :" + inwords, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = .2f;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = .2f;
                                        cell.PaddingBottom = 3;
                                        cell.Colspan = 7;
                                        tablev.AddCell(cell);


                                    }


                                    document.Add(tablev);

                                    #endregion
                                }

                                #endregion

                                #region footer

                                PdfPTable Addterms = new PdfPTable(6);
                                Addterms.TotalWidth = 580f;
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
                                Childterms.TotalWidth = 335f;
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

                                if (Bdt.Rows.Count > 0)
                                {
                                    if (OurGSTIN.Length > 0)
                                    {


                                        cell = new PdfPCell(new Phrase(OurGSTINAlias, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0f;
                                        cell.BorderWidthLeft = .2f;
                                        cell.Colspan = 1;
                                        cell.BorderColor = BaseColor.BLACK;
                                        Childterms.AddCell(cell);


                                        cell = new PdfPCell(new Phrase(": " + OurGSTIN, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                                chilk.TotalWidth = 245f;
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

                                cell = new PdfPCell(new Phrase("\n\n\n Authorised Signatory", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
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
                                //chilk.AddCell(cell);


                                PdfPCell Chid4 = new PdfPCell(chilk);
                                Chid4.Border = 0;
                                Chid4.Colspan = 3;
                                Chid4.HorizontalAlignment = 0;
                                Addterms.AddCell(Chid4);


                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0;
                                cell.BorderWidthLeft = 0;
                                cell.Colspan = 6;
                                // Addterms.AddCell(cell);

                                document.Add(Addterms);


                                #endregion

                                document.NewPage();

                            }

                            //document.NewPage();
                        }
                    }

                    document.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();

                }
                catch (Exception ex)
                {

                }


            }
            else
            {

                return;
            }
        }

        protected void btnfromtoWiseDownload_Click(object sender, EventArgs e)
        {
            int fontsize = 10;
            int font = 10;

            var Month = "";

            if (txtfrom.Text.Trim().Length == 0)
            {
                LblResult.Text = "Please Select From Month";
                return;
            }

            if (txtto.Text.Trim().Length == 0)
            {
                LblResult.Text = "Please Select To Month";
                return;
            }

            var testDate = 0;
            if (txtfrom.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtfrom.Text);
                if (testDate > 0)
                {
                    LblResult.Text = "You have entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.26/01/1990";
                    return;
                }

            }
            if (txtto.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtto.Text);
                if (testDate > 0)
                {
                    LblResult.Text = "You have entered Invalid Date.Date Format Should be [DD/MM/YYYY].Ex.26/01/1990";
                    return;
                }

            }

            if (ddlOptions.SelectedIndex == 0)
            {


                var list = new List<string>();

                MemoryStream ms = new MemoryStream();

                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                document.AddTitle("Webwonders");
                document.AddAuthor("DIYOS");
                document.AddSubject("Invoice");
                document.AddKeywords("Keyword1, keyword2, …");

                try
                {
                    if (gvFromtobills.Rows.Count > 0)
                    {

                        for (int m = 0; m < gvFromtobills.Rows.Count; m++)
                        {
                            CheckBox chkselect = gvFromtobills.Rows[m].FindControl("chkselect") as CheckBox;
                            CheckBox clientid = gvFromtobills.Rows[m].FindControl("lblclientid") as CheckBox;

                            if (chkselect.Checked == true)
                            {
                                GridView childgrid = (GridView)gvFromtobills.Rows[m].FindControl("gvnestedgrid");
                                for (int i = 0; i < childgrid.Rows.Count; i++)
                                {
                                    CheckBox chkclientid = childgrid.Rows[i].FindControl("chkindividual") as CheckBox;
                                    Label lblclientid = childgrid.Rows[i].FindControl("lblclientid") as Label;
                                    Label lblbillno = childgrid.Rows[i].FindControl("lblbillno") as Label;
                                    Label lblMonthNew = childgrid.Rows[i].FindControl("lblMonthNew") as Label;
                                    Label lblMonthName = childgrid.Rows[i].FindControl("lblMonthName") as Label;
                                    Month = lblMonthNew.Text;
                                    if (chkclientid.Checked == true)
                                    {
                                        #region for CompanyInfo
                                        string strQry = "Select * from CompanyInfo ";
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
                                        }

                                        #endregion

                                        var ContractID = "";

                                        int monthval = 0;
                                        int yearval = 0;

                                        if (lblMonthNew.Text.Length == 3)
                                        {
                                            monthval = int.Parse(lblMonthNew.Text.ToString().Substring(0, 1));
                                            yearval = int.Parse("20" + lblMonthNew.Text.ToString().Substring(1, 2));

                                        }


                                        if (lblMonthNew.Text.Length == 4)
                                        {
                                            monthval = int.Parse(lblMonthNew.Text.ToString().Substring(0, 2));
                                            yearval = int.Parse("20" + lblMonthNew.Text.ToString().Substring(2, 2));

                                        }

                                        int Selectdays = System.DateTime.DaysInMonth(yearval, monthval);
                                        string dateCheck = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();

                                        DateTime LastDay = DateTime.Now;
                                        LastDay = DateTime.Parse(dateCheck, CultureInfo.GetCultureInfo("en-gb"));

                                        #region  Begin Get Contract Id Based on The Last Day



                                        Hashtable HtGetContractID = new Hashtable();
                                        var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                                        HtGetContractID.Add("@clientid", lblclientid.Text);
                                        HtGetContractID.Add("@LastDay", LastDay);
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

                                        #region
                                        string SqlQuryForServiCharge = "select ContractId,servicecharge,PODate, isnull(EBD.ESiNO,'') EsiBranchname,isnull(PBD.PFNo,'') PFBranchname,convert(nvarchar(20), ContractStartDate, 103) as ContractStartDate,ServiceChargeType,Description,IncludeST,ServiceTax75,Pono,typeofwork,'' billnotes,isnull(ServiceChargeDesc,'') as ServiceChargeDesc,GSTLineitem from contracts  C left join EsiBranchDetails EBD on EBD.EsiBranchid=isnull(C.Esibranch,0) left join PFBranchDetails PBD on PBD.PFBranchid=isnull(C.PFbranch,0)   where " +
                                            " clientid ='" + lblclientid.Text + "' and ContractId='" + ContractID + "'";
                                        DataTable DtServicecharge = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuryForServiCharge).Result;
                                        string Typeofwork = "";
                                        string BillNotes = "";
                                        string ServiceCharge = "0";
                                        string strSCType = "";
                                        string strDescription = "We are presenting our bill for the House Keeping Services Provided at your establishment. Kindly release the payment at the earliest";
                                        bool bSCType = false;
                                        bool GSTLineitem = false;
                                        string strIncludeST = "";
                                        string ContractStartDate = "";
                                        string strST75 = "";
                                        bool bIncludeST = false;
                                        bool bST75 = false;
                                        string POContent = "";
                                        string PODate = "";
                                        string CnPFNo = "";
                                        string CnESINo = "";
                                        string Location = "";
                                        string ReversCharges = "";
                                        string ServiceChargeDesc = "";
                                        // string ServiceTaxCategory = "";
                                        if (DtServicecharge.Rows.Count > 0)
                                        {
                                            PODate = DtServicecharge.Rows[0]["PODate"].ToString();
                                            if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceCharge"].ToString()) == false)
                                            {
                                                ServiceCharge = DtServicecharge.Rows[0]["ServiceCharge"].ToString();
                                            }
                                            if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceChargeType"].ToString()) == false)
                                            {
                                                strSCType = DtServicecharge.Rows[0]["ServiceChargeType"].ToString();
                                            }
                                            string tempDescription = DtServicecharge.Rows[0]["Description"].ToString();
                                            if (tempDescription.Trim().Length > 0)
                                            {
                                                strDescription = tempDescription;
                                            }
                                            if (strSCType.Length > 0)
                                            {
                                                bSCType = Convert.ToBoolean(strSCType);
                                            }
                                            GSTLineitem = Convert.ToBoolean(DtServicecharge.Rows[0]["GSTLineitem"].ToString());
                                            PFNo = DtServicecharge.Rows[0]["PFBranchname"].ToString().Trim();
                                            Esino = DtServicecharge.Rows[0]["EsiBranchname"].ToString().Trim();

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
                                            BillNotes = DtServicecharge.Rows[0]["BillNotes"].ToString();
                                            // ServiceTaxCategory = DtServicecharge.Rows[0]["ServiceTaxCategory"].ToString();
                                            string tempServiceDesc = DtServicecharge.Rows[0]["ServiceChargeDesc"].ToString();
                                            if (tempServiceDesc.Trim().Length > 0)
                                            {
                                                ServiceChargeDesc = tempServiceDesc;
                                            }
                                        }

                                        #endregion

                                        #region

                                        string selectclientaddress = "select isnull(sg.segname,'') as segname,c.*, s.state as Statename,s.GSTStateCode,gst.gstno,gst.GSTAddress,s1.state as ShipState,s1.GSTStateCode as ShipToStateCode1 from clients c left join Segments sg on c.ClientSegment = sg.SegId  left join states s on s.stateid=c.state left join GSTMaster gst on gst.id=c.ourgstin left join states s1 on s1.stateid=c.ShipToState where clientid= '" + lblclientid.Text + "'";
                                        DataTable dtclientaddress = config.ExecuteAdaptorAsyncWithQueryParams(selectclientaddress).Result;
                                        string OurGSTIN = "";
                                        string GSTIN = "";
                                        string StateCode = "0";
                                        string State = "";
                                        string ShipToGSTIN = "";
                                        string ShipToStateCode = "0";
                                        string ShipToState = "";

                                        if (dtclientaddress.Rows.Count > 0)
                                        {
                                            OurGSTIN = dtclientaddress.Rows[0]["gstno"].ToString();
                                            StateCode = dtclientaddress.Rows[0]["GSTStateCode"].ToString();
                                            GSTIN = dtclientaddress.Rows[0]["GSTIN"].ToString();
                                            State = dtclientaddress.Rows[0]["Statename"].ToString();
                                            Location = dtclientaddress.Rows[0]["Location"].ToString();

                                            companyAddress = dtclientaddress.Rows[0]["GSTAddress"].ToString();
                                            ShipToStateCode = dtclientaddress.Rows[0]["ShipToStateCode1"].ToString();
                                            ShipToGSTIN = dtclientaddress.Rows[0]["ShipToGSTIN"].ToString();
                                            ShipToState = dtclientaddress.Rows[0]["ShipState"].ToString();
                                        }

                                        string SelectBillNo = string.Empty;
                                        if (ddlBillType.SelectedIndex == 0)
                                        {
                                            SelectBillNo = "Select RIGHT(billno,5) as DisplayBillNo,* from Unitbill where month='" + Month + "' and unitid='" + lblclientid.Text + "'";
                                        }
                                        else
                                        {
                                            SelectBillNo = "Select RIGHT(billno,5) as DisplayBillNo,* from MUnitbill where month='" + Month + "' and unitid='" + lblclientid.Text + "' and billno = '" + lblbillno.Text + "'";
                                        }
                                        DataTable DtBilling = config.ExecuteAdaptorAsyncWithQueryParams(SelectBillNo).Result;
                                        string BillNo = "";
                                        string DisplayBillNo = "";
                                        string area = "";
                                        string ExtraRemarks = "";
                                        string BilldateCheck = "";
                                        if (dtclientaddress.Rows.Count > 0)
                                        {
                                            area = dtclientaddress.Rows[0]["segname"].ToString();
                                            BilldateCheck = (DtBilling.Rows[0]["billdt"].ToString());
                                        }

                                        DateTime BillDate;
                                        DateTime DueDate;


                                        #region Variables for data Fields as on 11/03/2014 by venkat


                                        decimal servicecharge = 0;
                                        decimal servicetax = 0;
                                        decimal cess = 0;
                                        decimal sbcess = 0;
                                        decimal kkcess = 0;


                                        #region for GST on 17-6-2017 by swathi

                                        decimal CGST = 0;
                                        decimal SGST = 0;
                                        decimal IGST = 0;
                                        decimal Cess1 = 0;
                                        decimal Cess2 = 0;
                                        decimal CGSTPrc = 0;
                                        decimal SGSTPrc = 0;
                                        decimal IGSTPrc = 0;
                                        decimal Cess1Prc = 0;
                                        decimal Cess2Prc = 0;

                                        #endregion for GST on 17-6-2017 by swathi


                                        decimal shecess = 0;
                                        decimal totalamount = 0;
                                        decimal Grandtotal = 0;

                                        decimal ServiceTax75 = 0;
                                        decimal ServiceTax25 = 0;

                                        decimal machinarycost = 0;
                                        decimal materialcost = 0;
                                        decimal maintenancecost = 0;
                                        decimal extraonecost = 0;
                                        decimal extratwocost = 0;
                                        decimal discountone = 0;
                                        decimal discounttwo = 0;

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

                                        decimal staxamtonservicecharge = 0;
                                        decimal RelChrgAmt = 0;
                                        decimal PFAmt = 0;
                                        decimal ESIAmt = 0;
                                        decimal BpfPer = 0;
                                        decimal BesiPer = 0;


                                        #endregion

                                        //DateTime dtn = DateTime.ParseExact(BilldateCheck, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                                        string billdt = BilldateCheck;

                                        string BQry = "select * from TblOptions  where '" + billdt + "' between fromdate and todate";
                                        DataTable Bdt = config.ExecuteAdaptorAsyncWithQueryParams(BQry).Result;

                                        string CGSTAlias = "";
                                        string SGSTAlias = "";
                                        string IGSTAlias = "";
                                        string Cess1Alias = "";
                                        string Cess2Alias = "";
                                        string GSTINAlias = "";
                                        string OurGSTINAlias = "";

                                        string SqlQryForTaxes = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2,CGSTAlias,SGSTAlias,IGSTAlias,cess1Alias,cess2Alias,GSTINAlias,OurGSTINAlias from TblOptions where '" + billdt + "' between fromdate and todate ";
                                        DataTable DtTaxes = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForTaxes).Result;

                                        string SCPersent = "";
                                        if (DtTaxes.Rows.Count > 0)
                                        {
                                            SCPersent = DtTaxes.Rows[0]["ServiceTaxSeparate"].ToString();
                                            CGSTAlias = DtTaxes.Rows[0]["CGSTAlias"].ToString();
                                            SGSTAlias = DtTaxes.Rows[0]["SGSTAlias"].ToString();
                                            IGSTAlias = DtTaxes.Rows[0]["IGSTAlias"].ToString();
                                            Cess1Alias = DtTaxes.Rows[0]["Cess1Alias"].ToString();
                                            Cess2Alias = DtTaxes.Rows[0]["Cess2Alias"].ToString();
                                            GSTINAlias = DtTaxes.Rows[0]["GSTINAlias"].ToString();
                                            OurGSTINAlias = DtTaxes.Rows[0]["OurGSTINAlias"].ToString();
                                        }


                                        if (DtBilling.Rows.Count > 0)
                                        {

                                            ExtraRemarks = DtBilling.Rows[0]["Remarks"].ToString();
                                            BillNo = DtBilling.Rows[0]["billno"].ToString();
                                            DisplayBillNo = DtBilling.Rows[0]["DisplayBillNo"].ToString();
                                            BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                                            OurGSTIN = DtBilling.Rows[0]["OURGSTNo"].ToString();
                                            StateCode = DtBilling.Rows[0]["BillToStateCode"].ToString();
                                            GSTIN = DtBilling.Rows[0]["BillToGSTNo"].ToString();
                                            State = DtBilling.Rows[0]["BillToState"].ToString();
                                            ShipToStateCode = DtBilling.Rows[0]["ShipToStateCode"].ToString();
                                            ShipToGSTIN = DtBilling.Rows[0]["ShipToGSTNo"].ToString();
                                            ShipToState = DtBilling.Rows[0]["ShipToState"].ToString();
                                            if (ddlBillType.SelectedIndex == 0)
                                            {
                                                DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());
                                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax75"].ToString()) == false)
                                                {
                                                    ServiceTax75 = decimal.Parse(DtBilling.Rows[0]["ServiceTax75"].ToString());
                                                }

                                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax25"].ToString()) == false)
                                                {
                                                    ServiceTax25 = decimal.Parse(DtBilling.Rows[0]["ServiceTax25"].ToString());
                                                }

                                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString()) == false)
                                                {
                                                    servicecharge = decimal.Parse(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString());
                                                }

                                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["RelChrgAmt"].ToString()) == false)
                                                {
                                                    RelChrgAmt = decimal.Parse(DtBilling.Rows[0]["RelChrgAmt"].ToString());
                                                }

                                                if (string.IsNullOrEmpty(DtBilling.Rows[0]["Bpfamt"].ToString()) == false)
                                                {
                                                    PFAmt = decimal.Parse(DtBilling.Rows[0]["Bpfamt"].ToString());
                                                }


                                                if (string.IsNullOrEmpty(DtBilling.Rows[0]["Besiamt"].ToString()) == false)
                                                {
                                                    ESIAmt = decimal.Parse(DtBilling.Rows[0]["Besiamt"].ToString());
                                                }

                                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["BpfPer"].ToString()) == false)
                                                {
                                                    BpfPer = decimal.Parse(DtBilling.Rows[0]["BpfPer"].ToString());
                                                }

                                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["BesiPer"].ToString()) == false)
                                                {
                                                    BesiPer = decimal.Parse(DtBilling.Rows[0]["BesiPer"].ToString());
                                                }

                                            }

                                            else
                                            {
                                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrg"].ToString()) == false)
                                                {
                                                    servicecharge = decimal.Parse(DtBilling.Rows[0]["ServiceChrg"].ToString());
                                                }
                                            }



                                            #region Begin New code for values taken from database as on 11/03/2014 by venkat

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["dutiestotalamount"].ToString()) == false)
                                            {
                                                totalamount = decimal.Parse(DtBilling.Rows[0]["dutiestotalamount"].ToString());
                                            }




                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                                            {
                                                servicetax = decimal.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                                            {
                                                sbcess = decimal.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                                            {
                                                kkcess = decimal.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
                                            }

                                            #region for GST as on 17-6-2017 by swathi

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTAmt"].ToString()) == false)
                                            {
                                                CGST = decimal.Parse(DtBilling.Rows[0]["CGSTAmt"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTAmt"].ToString()) == false)
                                            {
                                                SGST = decimal.Parse(DtBilling.Rows[0]["SGSTAmt"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTAmt"].ToString()) == false)
                                            {
                                                IGST = decimal.Parse(DtBilling.Rows[0]["IGSTAmt"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Amt"].ToString()) == false)
                                            {
                                                Cess1 = decimal.Parse(DtBilling.Rows[0]["Cess1Amt"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Amt"].ToString()) == false)
                                            {
                                                Cess2 = decimal.Parse(DtBilling.Rows[0]["Cess2Amt"].ToString());
                                            }


                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTPrc"].ToString()) == false)
                                            {
                                                CGSTPrc = decimal.Parse(DtBilling.Rows[0]["CGSTPrc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTPrc"].ToString()) == false)
                                            {
                                                SGSTPrc = decimal.Parse(DtBilling.Rows[0]["SGSTPrc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTPrc"].ToString()) == false)
                                            {
                                                IGSTPrc = decimal.Parse(DtBilling.Rows[0]["IGSTPrc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Prc"].ToString()) == false)
                                            {
                                                Cess1Prc = decimal.Parse(DtBilling.Rows[0]["Cess1Prc"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Prc"].ToString()) == false)
                                            {
                                                Cess2Prc = decimal.Parse(DtBilling.Rows[0]["Cess2Prc"].ToString());
                                            }

                                            #endregion for GST as on 17-6-2017 by swathi

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CESS"].ToString()) == false)
                                            {
                                                cess = decimal.Parse(DtBilling.Rows[0]["CESS"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SHECess"].ToString()) == false)
                                            {
                                                shecess = decimal.Parse(DtBilling.Rows[0]["SHECess"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["GrandTotal"].ToString()) == false)
                                            {
                                                Grandtotal = decimal.Parse(DtBilling.Rows[0]["GrandTotal"].ToString());
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["MachinaryCost"].ToString()) == false)
                                            {
                                                machinarycost = decimal.Parse(DtBilling.Rows[0]["MachinaryCost"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["MaterialCost"].ToString()) == false)
                                            {
                                                materialcost = decimal.Parse(DtBilling.Rows[0]["MaterialCost"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ElectricalChrg"].ToString()) == false)
                                            {
                                                maintenancecost = decimal.Parse(DtBilling.Rows[0]["ElectricalChrg"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtone"].ToString()) == false)
                                            {
                                                extraonecost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtone"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtTwo"].ToString()) == false)
                                            {
                                                extratwocost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtTwo"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discount"].ToString()) == false)
                                            {
                                                discountone = decimal.Parse(DtBilling.Rows[0]["Discount"].ToString());
                                            }
                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discounttwo"].ToString()) == false)
                                            {
                                                discounttwo = decimal.Parse(DtBilling.Rows[0]["Discounttwo"].ToString());
                                            }

                                            machinarycosttitle = DtBilling.Rows[0]["Machinarycosttitle"].ToString();
                                            materialcosttitle = DtBilling.Rows[0]["Materialcosttitle"].ToString();
                                            maintenancecosttitle = DtBilling.Rows[0]["Maintanancecosttitle"].ToString();
                                            extraonecosttitle = DtBilling.Rows[0]["Extraonetitle"].ToString();
                                            extratwocosttitle = DtBilling.Rows[0]["Extratwotitle"].ToString();
                                            discountonetitle = DtBilling.Rows[0]["Discountonetitle"].ToString();
                                            discounttwotitle = DtBilling.Rows[0]["Discounttwotitle"].ToString();

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Extradatacheck"].ToString()) == false)
                                            {
                                                strExtradatacheck = DtBilling.Rows[0]["Extradatacheck"].ToString();
                                                if (strExtradatacheck == "True")
                                                {
                                                    Extradatacheck = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraDataSTcheck"].ToString()) == false)
                                            {
                                                strExtrastcheck = DtBilling.Rows[0]["ExtraDataSTcheck"].ToString();
                                                if (strExtrastcheck == "True")
                                                {
                                                    ExtraDataSTcheck = true;
                                                }
                                            }



                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMachinary"].ToString()) == false)
                                            {
                                                strSTMachinary = DtBilling.Rows[0]["STMachinary"].ToString();
                                                if (strSTMachinary == "True")
                                                {
                                                    STMachinary = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaterial"].ToString()) == false)
                                            {
                                                strSTMaterial = DtBilling.Rows[0]["STMaterial"].ToString();
                                                if (strSTMaterial == "True")
                                                {
                                                    STMaterial = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaintenance"].ToString()) == false)
                                            {
                                                strSTMaintenance = DtBilling.Rows[0]["STMaintenance"].ToString();
                                                if (strSTMaintenance == "True")
                                                {
                                                    STMaintenance = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtraone"].ToString()) == false)
                                            {
                                                strSTExtraone = DtBilling.Rows[0]["STExtraone"].ToString();
                                                if (strSTExtraone == "True")
                                                {
                                                    STExtraone = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtratwo"].ToString()) == false)
                                            {
                                                strSTExtratwo = DtBilling.Rows[0]["STExtratwo"].ToString();
                                                if (strSTExtratwo == "True")
                                                {
                                                    STExtratwo = true;
                                                }
                                            }


                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMachinary"].ToString()) == false)
                                            {
                                                strSCMachinary = DtBilling.Rows[0]["SCMachinary"].ToString();
                                                if (strSCMachinary == "True")
                                                {
                                                    SCMachinary = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaterial"].ToString()) == false)
                                            {
                                                strSCMaterial = DtBilling.Rows[0]["SCMaterial"].ToString();
                                                if (strSCMaterial == "True")
                                                {
                                                    SCMaterial = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaintenance"].ToString()) == false)
                                            {
                                                strSCMaintenance = DtBilling.Rows[0]["SCMaintenance"].ToString();
                                                if (strSCMaintenance == "True")
                                                {
                                                    SCMaintenance = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtraone"].ToString()) == false)
                                            {
                                                strSCExtraone = DtBilling.Rows[0]["SCExtraone"].ToString();
                                                if (strSCExtraone == "True")
                                                {
                                                    SCExtraone = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtratwo"].ToString()) == false)
                                            {
                                                strSCExtratwo = DtBilling.Rows[0]["SCExtratwo"].ToString();
                                                if (strSCExtratwo == "True")
                                                {
                                                    SCExtratwo = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                                            {
                                                strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                                                if (strSTDiscountone == "True")
                                                {
                                                    STDiscountone = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                                            {
                                                strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                                                if (strSTDiscounttwo == "True")
                                                {
                                                    STDiscounttwo = true;
                                                }
                                            }


                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                                            {
                                                strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                                                if (strSTDiscountone == "True")
                                                {
                                                    STDiscountone = true;
                                                }
                                            }

                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                                            {
                                                strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                                                if (strSTDiscounttwo == "True")
                                                {
                                                    STDiscounttwo = true;
                                                }
                                            }




                                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Staxonservicecharge"].ToString()) == false)
                                            {
                                                staxamtonservicecharge = decimal.Parse(DtBilling.Rows[0]["Staxonservicecharge"].ToString());
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                                            return;
                                        }

                                        #endregion

                                        document.AddTitle(companyName);
                                        document.AddAuthor("DIYOS");
                                        document.AddSubject("Invoice");
                                        document.AddKeywords("Keyword1, keyword2, …");


                                        string imagepath = Server.MapPath("~/assets/" + CmpIDPrefix + "BillLogo.png");
                                        if (File.Exists(imagepath))
                                        {
                                            iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);
                                            gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                                            gif2.ScalePercent(55f);
                                            gif2.SetAbsolutePosition(5f, 753f);
                                            document.Add(gif2);
                                        }


                                        PdfPTable tablelogo = new PdfPTable(2);
                                        tablelogo.TotalWidth = 580f;
                                        tablelogo.LockedWidth = true;
                                        float[] widtlogo = new float[] { 0.4f, 2f };
                                        tablelogo.SetWidths(widtlogo);


                                        var FontColour = new BaseColor(178, 34, 34);
                                        Font FontStyle1 = FontFactory.GetFont("Belwe-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 30, Font.NORMAL, FontColour);

                                        PdfPCell CCompName1 = new PdfPCell(new Paragraph("" + companyName, FontFactory.GetFont(FontStyle, 15, Font.BOLD, BaseColor.BLACK)));
                                        CCompName1.HorizontalAlignment = 1;
                                        CCompName1.Colspan = 2;
                                        CCompName1.PaddingTop = 25f;
                                        CCompName1.Border = 0;
                                        CCompName1.PaddingLeft = 80;
                                        tablelogo.AddCell(CCompName1);

                                        PdfPCell CCompName = new PdfPCell(new Paragraph("" + companyAddress, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        CCompName.HorizontalAlignment = 1;
                                        CCompName.Colspan = 2;
                                        CCompName.Border = 0;
                                        CCompName.PaddingLeft = 80;
                                        CCompName.SetLeading(0, 1.2f);
                                        tablelogo.AddCell(CCompName);


                                        if (emailid.Length > 0)
                                        {
                                            PdfPCell CCompName2 = new PdfPCell(new Paragraph("Website :" + website + " | Email :" + emailid, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            CCompName2.HorizontalAlignment = 1;
                                            CCompName2.Colspan = 2;
                                            CCompName2.Border = 0;
                                            CCompName2.PaddingLeft = 40;
                                            tablelogo.AddCell(CCompName2);
                                        }
                                        if (phoneno.Length > 0)
                                        {
                                            PdfPCell CCompName2 = new PdfPCell(new Paragraph("Phone :" + phoneno, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                            CCompName2.HorizontalAlignment = 1;
                                            CCompName2.Colspan = 2;
                                            CCompName2.Border = 0;
                                            CCompName2.PaddingBottom = 5;
                                            tablelogo.AddCell(CCompName2);
                                        }

                                        document.Add(tablelogo);

                                        #region  for Client Details

                                        PdfPTable address = new PdfPTable(5);
                                        address.TotalWidth = 580f;
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
                                        tempTable1.TotalWidth = 348f;
                                        tempTable1.LockedWidth = true;
                                        float[] tempWidth1 = new float[] { 0.8f, 2f, 2f };
                                        tempTable1.SetWidths(tempWidth1);

                                        string addressData = "";

                                        addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString();

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
                                        addressData = dtclientaddress.Rows[0]["ClientAddrStreet"].ToString();
                                        if (addressData.Trim().Length > 0)
                                        {
                                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            clientstreet.BorderWidthBottom = 0;
                                            clientstreet.BorderWidthTop = 0;
                                            clientstreet.Colspan = 3;
                                            clientstreet.BorderWidthLeft = .2f;
                                            clientstreet.BorderWidthRight = 0.2f;
                                            clientstreet.BorderColor = BaseColor.BLACK;
                                            //clientstreet.PaddingLeft = 20;
                                            tempTable1.AddCell(clientstreet);
                                        }


                                        addressData = dtclientaddress.Rows[0]["ClientAddrArea"].ToString();
                                        if (addressData.Trim().Length > 0)
                                        {
                                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            clientstreet.BorderWidthBottom = 0;
                                            clientstreet.BorderWidthTop = 0;
                                            clientstreet.Colspan = 3;
                                            clientstreet.BorderColor = BaseColor.BLACK;
                                            clientstreet.BorderWidthLeft = .2f;
                                            clientstreet.BorderWidthRight = 0.2f;
                                            // clientstreet.PaddingLeft = 20;
                                            tempTable1.AddCell(clientstreet);
                                        }


                                        var ClientAddrColony = dtclientaddress.Rows[0]["ClientAddrColony"].ToString();
                                        var ClientAddrcity = dtclientaddress.Rows[0]["ClientAddrcity"].ToString();
                                        var ClientAddrstate = dtclientaddress.Rows[0]["ClientAddrstate"].ToString();
                                        var ClientAddrpin = dtclientaddress.Rows[0]["ClientAddrpin"].ToString();
                                        addressData = (ClientAddrColony + "," + ClientAddrcity + "," + ClientAddrstate + "," + ClientAddrpin);
                                        if (addressData.Trim().Length > 0)
                                        {
                                            PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            clietnpin.Colspan = 3;
                                            clietnpin.BorderWidthBottom = 0;
                                            clietnpin.BorderWidthTop = 0;
                                            clietnpin.BorderWidthLeft = .2f;
                                            clietnpin.BorderWidthRight = 0.2f;
                                            clietnpin.BorderColor = BaseColor.BLACK;
                                            //  clietnpin.PaddingLeft = 20;
                                            tempTable1.AddCell(clietnpin);
                                        }

                                        if (Bdt.Rows.Count > 0)
                                        {
                                            if (StateCode == "1" || StateCode == "2" || StateCode == "3" || StateCode == "4" || StateCode == "5" || StateCode == "6" || StateCode == "7" || StateCode == "8" || StateCode == "7")
                                            {
                                                StateCode = "0" + StateCode;
                                            }
                                            if (State.Length > 0)
                                            {
                                                PdfPCell clietnpin = new PdfPCell(new Paragraph("State", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                clietnpin.Colspan = 1;
                                                clietnpin.BorderWidthBottom = 0;
                                                clietnpin.BorderWidthTop = 0;
                                                clietnpin.BorderWidthLeft = .2f;
                                                clietnpin.BorderWidthRight = 0;
                                                clietnpin.BorderColor = BaseColor.BLACK;
                                                //  clietnpin.PaddingLeft = 20;
                                                tempTable1.AddCell(clietnpin);

                                                clietnpin = new PdfPCell(new Paragraph(" : " + State, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                clietnpin.Colspan = 1;
                                                clietnpin.BorderWidthBottom = 0;
                                                clietnpin.BorderWidthTop = 0;
                                                clietnpin.BorderWidthLeft = 0;
                                                clietnpin.BorderWidthRight = 0;
                                                clietnpin.BorderColor = BaseColor.BLACK;
                                                //  clietnpin.PaddingLeft = 20;
                                                tempTable1.AddCell(clietnpin);
                                            }
                                            if (StateCode.Length > 0)
                                            {
                                                PdfPCell clietnpin = new PdfPCell(new Paragraph("State Code : " + StateCode, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                clietnpin.Colspan = 1;
                                                clietnpin.BorderWidthBottom = 0;
                                                clietnpin.BorderWidthTop = 0;
                                                clietnpin.BorderWidthLeft = 0;
                                                clietnpin.BorderWidthRight = 0.2f;
                                                clietnpin.BorderColor = BaseColor.BLACK;
                                                //  clietnpin.PaddingLeft = 20;
                                                tempTable1.AddCell(clietnpin);
                                            }


                                        }

                                        if (GSTIN.Length > 0)
                                        {
                                            PdfPCell clietnpin = new PdfPCell(new Paragraph("GSTIN ", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                                            clietnpin = new PdfPCell(new Paragraph(" : " + GSTIN, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                                        PdfPCell cellemp1 = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cellemp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        cellemp1.Colspan = 3;
                                        cellemp1.BorderWidthTop = 0;
                                        cellemp1.BorderWidthBottom = 0;
                                        cellemp1.BorderWidthLeft = .2f;
                                        cellemp1.BorderWidthRight = 0.2f;
                                        cellemp1.BorderColor = BaseColor.BLACK;
                                        cellemp1.PaddingBottom = 15;
                                        //tempTable1.AddCell(cellemp1);


                                        PdfPCell childTable1 = new PdfPCell(tempTable1);
                                        childTable1.Border = 0;
                                        childTable1.Colspan = 3;
                                        // childTable1.FixedHeight = 100;
                                        childTable1.HorizontalAlignment = 0;

                                        address.AddCell(childTable1);

                                        PdfPTable tempTable2 = new PdfPTable(2);
                                        tempTable2.TotalWidth = 232f;
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
                                        phrase10.Add(new Chunk(": " + BillNo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                                        phrase11v.Add(new Chunk(": " + BillDate.Day.ToString("00") + "/" + BillDate.ToString("MM") + "/" +
                                            BillDate.Year, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                                        phrase10im.Add(new Chunk(": " + GetMonthName() + "'" + GetMonthOfYear(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                                        string Fromdate = "";
                                        string Todate = "";


                                        var phrase10p = new Phrase();
                                        phrase10p.Add(new Chunk(": " + Fromdate + " to " + Todate, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                                        if (POContent.Length > 0)
                                        {
                                            var phrasew = new Phrase();
                                            phrasew.Add(new Chunk("PO No", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell13 = new PdfPCell();
                                            cell13.AddElement(phrasew);
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

                                            var phrase10w = new Phrase();
                                            phrase10w.Add(new Chunk(": " + POContent, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            cell13v = new PdfPCell();
                                            cell13v.AddElement(phrase10w);
                                            cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            cell13v.BorderWidthBottom = 0;
                                            cell13v.BorderWidthTop = 0;
                                            cell13v.Colspan = 1;
                                            cell13v.BorderWidthLeft = 0;
                                            cell13v.BorderWidthRight = .2f;
                                            cell13v.PaddingTop = -5;
                                            cell13v.BorderColor = BaseColor.BLACK;
                                            tempTable2.AddCell(cell13v);

                                        }

                                        if (PODate.Length > 0)
                                        {
                                            var phrasew = new Phrase();
                                            phrasew.Add(new Chunk("Work Order Date", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell13 = new PdfPCell();
                                            cell13.AddElement(phrasew);
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

                                            var phrase10w = new Phrase();
                                            phrase10w.Add(new Chunk(": " + PODate, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            cell13v = new PdfPCell();
                                            cell13v.AddElement(phrase10w);
                                            cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            cell13v.BorderWidthBottom = 0;
                                            cell13v.BorderWidthTop = 0;
                                            cell13v.Colspan = 1;
                                            cell13v.BorderWidthLeft = 0;
                                            cell13v.BorderWidthRight = .2f;
                                            cell13v.PaddingTop = -5;
                                            cell13v.BorderColor = BaseColor.BLACK;
                                            tempTable2.AddCell(cell13v);

                                        }

                                        if (Location.Length > 0)
                                        {
                                            var phrasew = new Phrase();
                                            phrasew.Add(new Chunk("Place of Supply", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell13 = new PdfPCell();
                                            cell13.AddElement(phrasew);
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

                                            var phrase10w = new Phrase();
                                            phrase10w.Add(new Chunk(": " + Location, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            cell13v = new PdfPCell();
                                            cell13v.AddElement(phrase10w);
                                            cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            cell13v.BorderWidthBottom = 0;
                                            cell13v.BorderWidthTop = 0;
                                            cell13v.Colspan = 1;
                                            cell13v.BorderWidthLeft = 0;
                                            cell13v.BorderWidthRight = .2f;
                                            cell13v.PaddingTop = -5;
                                            cell13v.BorderColor = BaseColor.BLACK;
                                            tempTable2.AddCell(cell13v);

                                        }

                                        if (ReversCharges.Length > 0)
                                        {
                                            var phrasew = new Phrase();
                                            phrasew.Add(new Chunk("Revers Charges", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell13 = new PdfPCell();
                                            cell13.AddElement(phrasew);
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
                                            if (ReversCharges == "True")
                                            {
                                                ReversCharges = "Yes";
                                            }
                                            else
                                            {
                                                ReversCharges = "No";
                                            }

                                            var phrase10w = new Phrase();
                                            phrase10w.Add(new Chunk(": " + ReversCharges, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            cell13v = new PdfPCell();
                                            cell13v.AddElement(phrase10w);
                                            cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            cell13v.BorderWidthBottom = 0;
                                            cell13v.BorderWidthTop = 0;
                                            cell13v.Colspan = 1;
                                            cell13v.BorderWidthLeft = 0;
                                            cell13v.BorderWidthRight = .2f;
                                            cell13v.PaddingTop = -5;
                                            cell13v.BorderColor = BaseColor.BLACK;
                                            tempTable2.AddCell(cell13v);

                                        }


                                        PdfPCell childTable2 = new PdfPCell(tempTable2);
                                        childTable2.Border = 0;
                                        childTable2.Colspan = 2;
                                        childTable2.HorizontalAlignment = 0;
                                        address.AddCell(childTable2);

                                        document.Add(address);





                                        #endregion



                                        #region for breakupdata
                                        int countGrid = 5;

                                        DataTable dtheadings = null;
                                        var SPNameD = "GetInvHeadings";
                                        Hashtable htheadings = new Hashtable();
                                        htheadings.Add("@clientid", lblclientid.Text);
                                        // htheadings.Add("@LastDay", DtLastDay);
                                        dtheadings = config.ExecuteAdaptorAsyncWithParams(SPNameD, htheadings).Result;

                                        string InvDescription = "";
                                        string InvNoOfEmps = "";
                                        string InvNoofDuties = "";
                                        string InvPayrate = "";
                                        string InvAmount = "";
                                        string InvSACCode = "";
                                        string InvMonthDays = "";
                                        string InvDescriptionVisible = "N";
                                        string InvNoOfEmpsVisible = "N";
                                        string InvNoofDutiesVisible = "N";
                                        string InvPayrateVisible = "N";
                                        string InvAmountVisible = "N";
                                        string InvSACCodeVisible = "N";
                                        string InvMonthDaysVisible = "N";
                                        string HSNNo = "";
                                        var ExDBRemarks = "";
                                        if (dtheadings.Rows.Count > 0)
                                        {
                                            InvDescription = dtheadings.Rows[0]["InvDescription"].ToString();
                                            InvNoOfEmps = dtheadings.Rows[0]["InvNoOfEmps"].ToString();
                                            InvNoofDuties = dtheadings.Rows[0]["InvNoofDuties"].ToString();
                                            InvPayrate = dtheadings.Rows[0]["InvPayrate"].ToString();
                                            InvAmount = dtheadings.Rows[0]["InvAmount"].ToString();
                                            InvMonthDays = dtheadings.Rows[0]["InvMonthDays"].ToString();
                                            InvSACCode = dtheadings.Rows[0]["InvSACCode"].ToString();
                                            InvDescriptionVisible = dtheadings.Rows[0]["InvDescriptionVisible"].ToString();
                                            InvNoOfEmpsVisible = dtheadings.Rows[0]["InvNoOfEmpsVisible"].ToString();
                                            InvNoofDutiesVisible = dtheadings.Rows[0]["InvNoofDutiesVisible"].ToString();
                                            InvPayrateVisible = dtheadings.Rows[0]["InvPayrateVisible"].ToString();
                                            InvAmountVisible = dtheadings.Rows[0]["InvAmountVisible"].ToString();
                                            InvSACCodeVisible = dtheadings.Rows[0]["InvSACCodeVisible"].ToString();
                                            InvMonthDaysVisible = dtheadings.Rows[0]["InvMonthDaysVisible"].ToString();
                                        }




                                        int colCount = 1;

                                        if (InvDescriptionVisible == "Y")
                                        {
                                            colCount += 1;
                                        }

                                        if (InvNoOfEmpsVisible == "Y")
                                        {
                                            colCount += 1;
                                        }

                                        if (InvNoofDutiesVisible == "Y")
                                        {
                                            colCount += 1;
                                        }

                                        if (InvPayrateVisible == "Y")
                                        {
                                            colCount += 1;
                                        }

                                        if (InvAmountVisible == "Y")
                                        {
                                            colCount += 1;
                                        }



                                        if (InvSACCodeVisible == "Y")
                                        {
                                            colCount += 1;
                                        }

                                        if (InvMonthDaysVisible == "Y")
                                        {
                                            colCount += 1;
                                        }


                                        PdfPTable table = new PdfPTable(colCount);
                                        table.TotalWidth = 580f;
                                        table.LockedWidth = true;
                                        table.HorizontalAlignment = 1;
                                        float[] colWidths = new float[] { };
                                        if (colCount == 8)
                                        {
                                            colWidths = new float[] { 1f, 6f, 2f, 2f, 2f, 2.2f, 2f, 2.7f };
                                        }
                                        if (colCount == 7)
                                        {
                                            colWidths = new float[] { 1f, 6f, 2f, 2f, 2.2f, 2f, 2.7f };
                                        }
                                        if (colCount == 6)
                                        {
                                            colWidths = new float[] { 1f, 6f, 2f, 2.2f, 2f, 2.7f };
                                        }

                                        if (colCount == 5)
                                        {
                                            colWidths = new float[] { 1f, 6f, 2f, 2.2f, 2.7f };
                                        }

                                        if (colCount == 4)
                                        {
                                            colWidths = new float[] { 1f, 6f, 2.2f, 2.7f };
                                        }

                                        if (colCount == 3)
                                        {
                                            colWidths = new float[] { 1f, 6f, 2.7f };
                                        }


                                        table.SetWidths(colWidths);

                                        string cellText;


                                        PdfPCell cell = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0.2f;
                                        cell.BorderWidthLeft = .2f;
                                        cell.BorderWidthTop = 0.2f;
                                        cell.BorderWidthRight = 0f;
                                        cell.Colspan = 1;
                                        cell.BorderColor = BaseColor.BLACK;
                                        table.AddCell(cell);

                                        if (InvDescriptionVisible == "Y")
                                        {
                                            cell = new PdfPCell(new Phrase(InvDescription, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderWidthBottom = 0.2f;
                                            cell.BorderWidthLeft = 0.2f;
                                            cell.BorderWidthTop = 0.2f;
                                            cell.BorderWidthRight = 0f;
                                            //cell.Colspan = 1;
                                            cell.BorderColor = BaseColor.BLACK;
                                            table.AddCell(cell);
                                        }
                                        if (InvSACCodeVisible == "Y")
                                        {
                                            cell = new PdfPCell(new Phrase(InvSACCode, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderWidthBottom = 0.2f;
                                            cell.BorderWidthLeft = 0.2f;
                                            cell.BorderWidthTop = 0.2f;
                                            cell.BorderWidthRight = 0f;
                                            //cell.Colspan = 1;
                                            cell.BorderColor = BaseColor.BLACK;
                                            table.AddCell(cell);
                                        }
                                        if (InvMonthDaysVisible == "Y")
                                        {
                                            cell = new PdfPCell(new Phrase(InvMonthDays, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderWidthBottom = 0.2f;
                                            cell.BorderWidthLeft = 0.2f;
                                            cell.BorderWidthTop = 0.2f;
                                            cell.BorderWidthRight = 0f;
                                            //cell.Colspan = 1;
                                            cell.BorderColor = BaseColor.BLACK;
                                            table.AddCell(cell);
                                        }
                                        if (InvNoOfEmpsVisible == "Y")
                                        {
                                            cell = new PdfPCell(new Phrase(InvNoOfEmps, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderWidthBottom = 0.2f;
                                            cell.BorderWidthLeft = 0.2f;
                                            cell.BorderWidthTop = 0.2f;
                                            cell.BorderWidthRight = 0f;
                                            //cell.Colspan = 1;
                                            cell.BorderColor = BaseColor.BLACK;
                                            table.AddCell(cell);
                                        }

                                        if (InvPayrateVisible == "Y")
                                        {
                                            cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderWidthBottom = 0.2f;
                                            cell.BorderWidthLeft = 0.2f;
                                            cell.BorderWidthTop = 0.2f;
                                            cell.BorderWidthRight = 0f;
                                            cell.BorderColor = BaseColor.BLACK;
                                            table.AddCell(cell);
                                        }

                                        if (InvNoofDutiesVisible == "Y")
                                        {
                                            cell = new PdfPCell(new Phrase(InvNoofDuties, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderWidthBottom = 0.2f;
                                            cell.BorderWidthLeft = 0.2f;
                                            cell.BorderWidthTop = 0.2f;
                                            cell.BorderWidthRight = 0f;
                                            cell.BorderColor = BaseColor.BLACK;
                                            table.AddCell(cell);
                                        }

                                        if (InvAmountVisible == "Y")
                                        {
                                            cell = new PdfPCell(new Phrase(InvAmount, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderWidthBottom = 0.2f;
                                            cell.BorderWidthLeft = 0.2f;
                                            cell.BorderWidthTop = 0.2f;
                                            cell.BorderWidthRight = .2f;
                                            cell.BorderColor = BaseColor.BLACK;
                                            table.AddCell(cell);
                                        }


                                        string spUnitbillbreakup = "GetpdfforClientbillingfromunitbillbreakup";
                                        Hashtable htunitbillbreakup = new Hashtable();
                                        htunitbillbreakup.Add("@Clientid", lblclientid.Text);
                                        htunitbillbreakup.Add("@month", Month);
                                        htunitbillbreakup.Add("@status", ddlBillType.SelectedIndex);
                                        htunitbillbreakup.Add("@munitibillno", lblbillno.Text);

                                        DataTable dtunitbillbreakup = SqlHelper.Instance.ExecuteStoredProcedureWithParams(spUnitbillbreakup, htunitbillbreakup);

                                        for (int rowIndex = 0; rowIndex < dtunitbillbreakup.Rows.Count; rowIndex++)
                                        {

                                            int SNo = 0;
                                            string Designation = "";
                                            string SACCodes = "";
                                            decimal MonthDays = 0;
                                            decimal NoOfEmps = 0;
                                            decimal payrate = 0;
                                            decimal Duties = 0;
                                            decimal Dutiesamount = 0;

                                            SNo = int.Parse(dtunitbillbreakup.Rows[rowIndex]["SNo"].ToString());
                                            Designation = dtunitbillbreakup.Rows[rowIndex]["Designation"].ToString();
                                            SACCodes = dtunitbillbreakup.Rows[rowIndex]["SACCode"].ToString();
                                            MonthDays = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["MonthDays"].ToString());
                                            NoOfEmps = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["NoOfEmps"].ToString());
                                            payrate = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["payrate"].ToString());
                                            Duties = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["Duties"].ToString());
                                            Dutiesamount = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["Dutiesamount"].ToString());

                                            cell = new PdfPCell(new Phrase(SNo.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            cell.Colspan = 1;
                                            cell.BorderWidthRight = 0f;
                                            cell.BorderWidthLeft = .2f;
                                            cell.BorderWidthTop = 0;
                                            cell.MinimumHeight = 20;
                                            cell.HorizontalAlignment = 1;
                                            cell.BorderColor = BaseColor.BLACK;
                                            table.AddCell(cell);

                                            if (InvDescriptionVisible == "Y")
                                            {
                                                cell = new PdfPCell(new Phrase(Designation, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0.2f;
                                                cell.BorderWidthLeft = 0.2f;
                                                cell.BorderWidthTop = 0.2f;
                                                cell.BorderWidthRight = 0f;
                                                //cell.Colspan = 1;
                                                cell.BorderColor = BaseColor.BLACK;
                                                table.AddCell(cell);
                                            }
                                            if (InvSACCodeVisible == "Y")
                                            {
                                                cell = new PdfPCell(new Phrase(SACCodes, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.2f;
                                                cell.BorderWidthLeft = 0.2f;
                                                cell.BorderWidthTop = 0.2f;
                                                cell.BorderWidthRight = 0f;
                                                //cell.Colspan = 1;
                                                cell.BorderColor = BaseColor.BLACK;
                                                table.AddCell(cell);
                                            }
                                            if (InvMonthDaysVisible == "Y")
                                            {
                                                cell = new PdfPCell(new Phrase(MonthDays.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.2f;
                                                cell.BorderWidthLeft = 0.2f;
                                                cell.BorderWidthTop = 0.2f;
                                                cell.BorderWidthRight = 0f;
                                                //cell.Colspan = 1;
                                                cell.BorderColor = BaseColor.BLACK;
                                                table.AddCell(cell);
                                            }
                                            if (InvNoOfEmpsVisible == "Y")
                                            {
                                                cell = new PdfPCell(new Phrase(NoOfEmps.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.2f;
                                                cell.BorderWidthLeft = 0.2f;
                                                cell.BorderWidthTop = 0.2f;
                                                cell.BorderWidthRight = 0f;
                                                //cell.Colspan = 1;
                                                cell.BorderColor = BaseColor.BLACK;
                                                table.AddCell(cell);
                                            }

                                            if (InvPayrateVisible == "Y")
                                            {
                                                cell = new PdfPCell(new Phrase(payrate.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.2f;
                                                cell.BorderWidthLeft = 0.2f;
                                                cell.BorderWidthTop = 0.2f;
                                                cell.BorderWidthRight = 0f;
                                                cell.BorderColor = BaseColor.BLACK;
                                                table.AddCell(cell);
                                            }

                                            if (InvNoofDutiesVisible == "Y")
                                            {
                                                cell = new PdfPCell(new Phrase(Duties.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.2f;
                                                cell.BorderWidthLeft = 0.2f;
                                                cell.BorderWidthTop = 0.2f;
                                                cell.BorderWidthRight = 0f;
                                                cell.BorderColor = BaseColor.BLACK;
                                                table.AddCell(cell);
                                            }

                                            if (InvAmountVisible == "Y")
                                            {
                                                cell = new PdfPCell(new Phrase(Dutiesamount.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 2;
                                                cell.BorderWidthBottom = 0.2f;
                                                cell.BorderWidthLeft = 0.2f;
                                                cell.BorderWidthTop = 0.2f;
                                                cell.BorderWidthRight = .2f;
                                                cell.BorderColor = BaseColor.BLACK;
                                                table.AddCell(cell);
                                            }

                                        }


                                        #region for space
                                        PdfPCell Cellempty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty.HorizontalAlignment = 2;
                                        Cellempty.Colspan = 1;
                                        Cellempty.BorderWidthTop = 0;
                                        Cellempty.BorderWidthRight = 0f;
                                        Cellempty.BorderWidthLeft = .2f;
                                        Cellempty.BorderWidthBottom = 0;
                                        // Cellempty.MinimumHeight = 5;
                                        Cellempty.BorderColor = BaseColor.BLACK;


                                        PdfPCell Cellempty1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty1.HorizontalAlignment = 2;
                                        Cellempty1.Colspan = 1;
                                        Cellempty1.BorderWidthTop = 0;
                                        Cellempty1.BorderWidthRight = 0f;
                                        Cellempty1.BorderWidthLeft = 0.2f;
                                        Cellempty1.BorderWidthBottom = 0;
                                        Cellempty1.BorderColor = BaseColor.BLACK;


                                        PdfPCell Cellempty6 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty6.HorizontalAlignment = 2;
                                        Cellempty6.Colspan = 1;
                                        Cellempty6.BorderWidthTop = 0;
                                        Cellempty6.BorderWidthRight = 0f;
                                        Cellempty6.BorderWidthLeft = .2f;
                                        Cellempty6.BorderWidthBottom = 0;

                                        Cellempty6.BorderColor = BaseColor.BLACK;

                                        PdfPCell Cellempty7 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty7.HorizontalAlignment = 2;
                                        Cellempty7.Colspan = 1;
                                        Cellempty7.BorderWidthTop = 0;
                                        Cellempty7.BorderWidthRight = 0.2f;
                                        Cellempty7.BorderWidthLeft = 0.2f;
                                        Cellempty7.BorderWidthBottom = 0;
                                        Cellempty7.BorderColor = BaseColor.BLACK;

                                        PdfPCell Cellempty2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty2.HorizontalAlignment = 2;
                                        Cellempty2.Colspan = 1;
                                        Cellempty2.BorderWidthTop = 0;
                                        Cellempty2.BorderWidthRight = 0f;
                                        Cellempty2.BorderWidthLeft = 0.2f;
                                        Cellempty2.BorderWidthBottom = 0;
                                        Cellempty2.BorderColor = BaseColor.BLACK;

                                        PdfPCell Cellempty3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty3.HorizontalAlignment = 2;
                                        Cellempty3.Colspan = 1;
                                        Cellempty3.BorderWidthTop = 0;
                                        Cellempty3.BorderWidthRight = 0f;
                                        Cellempty3.BorderWidthLeft = 0.2f;
                                        Cellempty3.BorderWidthBottom = 0;
                                        Cellempty3.BorderColor = BaseColor.BLACK;

                                        PdfPCell Cellempty4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty4.HorizontalAlignment = 2;
                                        Cellempty4.Colspan = 1;
                                        Cellempty4.BorderWidthTop = 0;
                                        Cellempty4.BorderWidthRight = 0f;
                                        Cellempty4.BorderWidthLeft = 0.2f;
                                        Cellempty4.BorderWidthBottom = 0;
                                        Cellempty4.BorderColor = BaseColor.BLACK;

                                        PdfPCell Cellempty5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        Cellempty5.HorizontalAlignment = 2;
                                        Cellempty5.Colspan = 1;
                                        Cellempty5.BorderWidthTop = 0;
                                        Cellempty5.BorderWidthRight = 0.2f;
                                        Cellempty5.BorderWidthLeft = 0.2f;
                                        Cellempty5.BorderWidthBottom = 0;
                                        Cellempty5.BorderColor = BaseColor.BLACK;



                                        if (dtunitbillbreakup.Rows.Count == 1)
                                        {
                                            #region For cell count

                                            if (!bIncludeST)
                                            {
                                                for (i = 0; i < 13; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                    //table.AddCell(Cellempty4);
                                                    //table.AddCell(Cellempty5);
                                                }
                                            }
                                            else
                                            {
                                                for (i = 0; i < 10; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }



                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 2)
                                        {
                                            #region For cell count

                                            if (!bIncludeST)
                                            {
                                                for (i = 0; i < 12; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }
                                            else
                                            {

                                                for (i = 0; i < 10; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }



                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 3)
                                        {
                                            #region For cell count

                                            if (!bIncludeST)
                                            {
                                                for (i = 0; i < 11; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }
                                            else
                                            {

                                                for (i = 0; i < 9; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }

                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 4)
                                        {
                                            #region For cell count

                                            if (!bIncludeST)
                                            {
                                                for (i = 0; i < 10; i++)
                                                {
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                for (i = 0; i < 8; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }


                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 5)
                                        {
                                            #region For cell count

                                            if (!bIncludeST)
                                            {
                                                for (i = 0; i < 9; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                for (i = 0; i < 7; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }


                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 6)
                                        {
                                            #region For cell count
                                            if (!bIncludeST)
                                            {
                                                for (i = 0; i < 8; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                for (i = 0; i < 6; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }


                                            #endregion
                                        }
                                        if (dtunitbillbreakup.Rows.Count == 7)
                                        {
                                            #region For cell count
                                            if (!bIncludeST)
                                            {
                                                for (i = 0; i < 7; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                for (i = 0; i < 5; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }


                                            #endregion
                                        }

                                        if (dtunitbillbreakup.Rows.Count == 8)
                                        {
                                            #region For cell count
                                            if (!bIncludeST)
                                            {
                                                for (i = 0; i < 6; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                for (i = 0; i < 4; i++)
                                                {
                                                    //1
                                                    table.AddCell(Cellempty);
                                                    if (InvDescriptionVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty1);
                                                    }
                                                    if (InvSACCodeVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty6);
                                                    }
                                                    if (InvMonthDaysVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty7);
                                                    }
                                                    if (InvNoOfEmpsVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty2);
                                                    }
                                                    if (InvNoofDutiesVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty3);
                                                    }
                                                    if (InvPayrateVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty4);
                                                    }
                                                    if (InvAmountVisible == "Y")
                                                    {
                                                        table.AddCell(Cellempty5);
                                                    }

                                                }
                                            }


                                            #endregion
                                        }




                                        #endregion

                                        document.Add(table);

                                        #endregion

                                        #region for Total Values

                                        PdfPTable tempTable22 = new PdfPTable(colCount);
                                        tempTable22.TotalWidth = 580f;
                                        tempTable22.LockedWidth = true;
                                        float[] tempWidth22 = new float[] { };
                                        if (colCount == 8)
                                        {
                                            tempWidth22 = new float[] { 1f, 6f, 2f, 2f, 2f, 2.2f, 2f, 2.7f };
                                        }

                                        if (colCount == 7)
                                        {
                                            tempWidth22 = new float[] { 1f, 6f, 2f, 2f, 2.2f, 2f, 2.7f };
                                        }

                                        if (colCount == 6)
                                        {
                                            tempWidth22 = new float[] { 1f, 6f, 2f, 2.2f, 2f, 2.7f };
                                        }

                                        if (colCount == 5)
                                        {
                                            tempWidth22 = new float[] { 1f, 6f, 2f, 2.2f, 2.7f };
                                        }

                                        if (colCount == 4)
                                        {
                                            tempWidth22 = new float[] { 1f, 6f, 2.2f, 2.7f };
                                        }

                                        if (colCount == 3)
                                        {
                                            tempWidth22 = new float[] { 1f, 6f, 2.7f };
                                        }
                                        tempTable22.SetWidths(tempWidth22);

                                        #region


                                        if (RelChrgAmt > 0)
                                        {

                                            PdfPCell celldz5 = new PdfPCell(new Phrase("1/6 Reliever Charges", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldz5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                            celldz5.Colspan = colCount - 1;
                                            celldz5.BorderWidthBottom = 0;
                                            celldz5.BorderWidthLeft = .2f;
                                            celldz5.BorderWidthTop = 0;
                                            celldz5.BorderWidthRight = .2f;
                                            celldz5.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldz5);

                                            PdfPCell celldz6 = new PdfPCell(new Phrase(" " + RelChrgAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            celldz6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            celldz6.BorderWidthBottom = 0;
                                            celldz6.BorderWidthLeft = .2f;
                                            celldz6.BorderWidthTop = 0;
                                            celldz6.BorderWidthRight = .2f;
                                            celldz6.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(celldz6);
                                        }

                                        int Noofcolumns = 4;
                                        int Noofcolumnsheading = 3;
                                        if (colCount == 4)
                                        {
                                            Noofcolumns = 2;
                                            Noofcolumnsheading = 1;
                                        }

                                        PdfPCell celldz1 = new PdfPCell(new Phrase(ExtraRemarks, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldz1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        celldz1.Colspan = colCount - Noofcolumns;
                                        celldz1.BorderWidthBottom = 0;
                                        celldz1.BorderWidthLeft = .2f;
                                        celldz1.BorderWidthTop = 0.2f;
                                        celldz1.BorderWidthRight = 0.2f;
                                        celldz1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldz1);

                                        celldz1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldz1.Colspan = Noofcolumnsheading;
                                        celldz1.BorderWidthBottom = 0;
                                        celldz1.BorderWidthLeft = .2f;
                                        celldz1.BorderWidthTop = .2f;
                                        celldz1.BorderWidthRight = 0;
                                        celldz1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldz1);

                                        PdfPCell celldz4 = new PdfPCell(new Phrase(" " + (totalamount - (PFAmt + ESIAmt)).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldz4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldz4.BorderWidthBottom = 0;
                                        celldz4.BorderWidthLeft = 0.2f;
                                        celldz4.BorderWidthTop = .2f;
                                        celldz4.BorderWidthRight = .2f;
                                        celldz4.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldz4);


                                        if (PFAmt > 0)
                                        {

                                            PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellCGST.Colspan = colCount - Noofcolumns;
                                            CellCGST.BorderWidthBottom = 0;
                                            CellCGST.BorderWidthLeft = .2f;
                                            CellCGST.BorderWidthTop = 0f;
                                            CellCGST.BorderWidthRight = 0.2f;
                                            // CellCGST.PaddingBottom = 5;
                                            // CellCGST.PaddingTop = 5;
                                            CellCGST.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellCGST);

                                            CellCGST = new PdfPCell(new Phrase("EPF Employer Share @ " + BpfPer + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellCGST.Colspan = Noofcolumnsheading;
                                            CellCGST.BorderWidthBottom = 0;
                                            CellCGST.BorderWidthLeft = .2f;
                                            CellCGST.BorderWidthTop = 0.2f;
                                            CellCGST.BorderWidthRight = 0f;
                                            // CellCGST.PaddingBottom = 5;
                                            // CellCGST.PaddingTop = 5;
                                            CellCGST.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellCGST);

                                            PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(PFAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellCGSTAmt.BorderWidthBottom = 0;
                                            CellCGSTAmt.BorderWidthLeft = 0.2f;
                                            CellCGSTAmt.BorderWidthTop = 0.2f;
                                            CellCGSTAmt.BorderWidthRight = .2f;
                                            CellCGSTAmt.BorderColor = BaseColor.BLACK;
                                            //CellCGSTAmt.PaddingBottom = 5;
                                            //CellCGSTAmt.PaddingTop = 5;
                                            tempTable22.AddCell(CellCGSTAmt);

                                        }


                                        if (ESIAmt > 0)
                                        {

                                            PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellCGST.Colspan = colCount - Noofcolumns;
                                            CellCGST.BorderWidthBottom = 0;
                                            CellCGST.BorderWidthLeft = .2f;
                                            CellCGST.BorderWidthTop = 0f;
                                            CellCGST.BorderWidthRight = 0.2f;
                                            // CellCGST.PaddingBottom = 5;
                                            // CellCGST.PaddingTop = 5;
                                            CellCGST.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellCGST);

                                            CellCGST = new PdfPCell(new Phrase("ESI Employer Share @ " + BesiPer + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellCGST.Colspan = Noofcolumnsheading;
                                            CellCGST.BorderWidthBottom = 0;
                                            CellCGST.BorderWidthLeft = .2f;
                                            CellCGST.BorderWidthTop = 0.2f;
                                            CellCGST.BorderWidthRight = 0f;
                                            // CellCGST.PaddingBottom = 5;
                                            // CellCGST.PaddingTop = 5;
                                            CellCGST.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellCGST);

                                            PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(ESIAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellCGSTAmt.BorderWidthBottom = 0;
                                            CellCGSTAmt.BorderWidthLeft = 0.2f;
                                            CellCGSTAmt.BorderWidthTop = 0.2f;
                                            CellCGSTAmt.BorderWidthRight = .2f;
                                            CellCGSTAmt.BorderColor = BaseColor.BLACK;
                                            //CellCGSTAmt.PaddingBottom = 5;
                                            //CellCGSTAmt.PaddingTop = 5;
                                            tempTable22.AddCell(CellCGSTAmt);
                                        }

                                        #region When Extradata check is false and STcheck is false

                                        if (Extradatacheck == true)
                                        {
                                            if (machinarycost > 0)
                                            {
                                                if (STMachinary == true)
                                                {
                                                    if (SCMachinary == true)
                                                    {
                                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldzz.Colspan = colCount - Noofcolumns;
                                                        celldzz.BorderWidthBottom = 0;
                                                        celldzz.BorderWidthLeft = .2f;
                                                        celldzz.BorderWidthTop = 0;
                                                        celldzz.BorderWidthRight = 0.2f;
                                                        celldzz.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldzz);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0;
                                                        celldcst1.BorderWidthRight = .2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);


                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = .2f;
                                                        celldcst2.BorderWidthTop = 0;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }
                                            if (materialcost > 0)
                                            {
                                                if (STMaterial == true)
                                                {
                                                    if (SCMaterial == true)
                                                    {
                                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldzz.Colspan = colCount - Noofcolumns;
                                                        celldzz.BorderWidthBottom = 0;
                                                        celldzz.BorderWidthLeft = .2f;
                                                        celldzz.BorderWidthTop = 0;
                                                        celldzz.BorderWidthRight = 0.2f;
                                                        celldzz.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldzz);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0;
                                                        celldcst1.BorderWidthRight = .2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = .2f;
                                                        celldcst2.BorderWidthTop = 0;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }


                                            if (maintenancecost > 0)
                                            {
                                                if (STMaintenance == true)
                                                {
                                                    if (SCMaintenance == true)
                                                    {
                                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldzz.Colspan = colCount - Noofcolumns;
                                                        celldzz.BorderWidthBottom = 0;
                                                        celldzz.BorderWidthLeft = .2f;
                                                        celldzz.BorderWidthTop = 0;
                                                        celldzz.BorderWidthRight = 0.2f;
                                                        celldzz.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldzz);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0;
                                                        celldcst1.BorderWidthRight = .2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = .2f;
                                                        celldcst2.BorderWidthTop = 0;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }

                                            if (extraonecost > 0)
                                            {
                                                if (STExtraone == true)
                                                {
                                                    if (SCExtraone == true)
                                                    {
                                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldzz.Colspan = colCount - Noofcolumns;
                                                        celldzz.BorderWidthBottom = 0;
                                                        celldzz.BorderWidthLeft = .2f;
                                                        celldzz.BorderWidthTop = 0;
                                                        celldzz.BorderWidthRight = 0.2f;
                                                        celldzz.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldzz);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0;
                                                        celldcst1.BorderWidthRight = .2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = .2f;
                                                        celldcst2.BorderWidthTop = 0;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }
                                            if (extratwocost > 0)
                                            {
                                                if (STExtratwo == true)
                                                {
                                                    if (SCExtratwo == true)
                                                    {
                                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldzz.Colspan = colCount - Noofcolumns;
                                                        celldzz.BorderWidthBottom = 0;
                                                        celldzz.BorderWidthLeft = .2f;
                                                        celldzz.BorderWidthTop = 0;
                                                        celldzz.BorderWidthRight = 0.2f;
                                                        celldzz.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldzz);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0;
                                                        celldcst1.BorderWidthRight = .2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = .2f;
                                                        celldcst2.BorderWidthTop = 0;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }

                                        }

                                        if (servicecharge > 0)//bSCType == true)
                                        {
                                            decimal scharge = servicecharge;
                                            if (scharge > 0)
                                            {
                                                string SCharge = "";
                                                if (bSCType == false)
                                                {
                                                    SCharge = ServiceCharge + "%";
                                                }
                                                else
                                                {
                                                    SCharge = ServiceCharge;
                                                }


                                                PdfPCell Cellservice = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                Cellservice.Colspan = colCount - Noofcolumns;
                                                Cellservice.BorderWidthBottom = 0;
                                                Cellservice.BorderWidthLeft = .2f;
                                                Cellservice.BorderWidthTop = 0f;
                                                Cellservice.BorderWidthRight = 0.2f;
                                                Cellservice.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(Cellservice);

                                                Cellservice = new PdfPCell(new Phrase("Service Charges @ " + SCharge, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                Cellservice.Colspan = Noofcolumnsheading;
                                                Cellservice.BorderWidthBottom = 0;
                                                Cellservice.BorderWidthLeft = .2f;
                                                Cellservice.BorderWidthTop = 0;
                                                Cellservice.BorderWidthRight = 0f;
                                                Cellservice.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(Cellservice);

                                                Cellservice = new PdfPCell(new Phrase(servicecharge.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                Cellservice.BorderWidthBottom = 0;
                                                Cellservice.BorderWidthLeft = 0.2f;
                                                Cellservice.BorderWidthTop = 0;
                                                Cellservice.BorderWidthRight = .2f;
                                                Cellservice.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(Cellservice);
                                            }
                                        }
                                        #endregion

                                        #region When Extra data is checked and STcheck is true and SCcheck is false

                                        if (machinarycost > 0)
                                        {
                                            if (STMachinary == true)
                                            {
                                                if (SCMachinary == false)
                                                {
                                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                                    CellIGST2.BorderWidthBottom = 0;
                                                    CellIGST2.BorderWidthLeft = .2f;
                                                    CellIGST2.BorderWidthTop = 0f;
                                                    CellIGST2.BorderWidthRight = 0.2f;
                                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(CellIGST2);

                                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst1.Colspan = Noofcolumnsheading;
                                                    celldcst1.BorderWidthBottom = 0;
                                                    celldcst1.BorderWidthLeft = .2f;
                                                    celldcst1.BorderWidthTop = 0;
                                                    celldcst1.BorderWidthRight = 0.2f;
                                                    celldcst1.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst1);

                                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst2.BorderWidthBottom = 0;
                                                    celldcst2.BorderWidthLeft = 0f;
                                                    celldcst2.BorderWidthTop = 0;
                                                    celldcst2.BorderWidthRight = .2f;
                                                    celldcst2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst2);
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
                                                if (SCMaterial == false)
                                                {
                                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                                    CellIGST2.BorderWidthBottom = 0;
                                                    CellIGST2.BorderWidthLeft = .2f;
                                                    CellIGST2.BorderWidthTop = 0f;
                                                    CellIGST2.BorderWidthRight = 0.2f;
                                                    // CellCGST.PaddingBottom = 5;
                                                    // CellCGST.PaddingTop = 5;
                                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(CellIGST2);

                                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst1.Colspan = Noofcolumnsheading;
                                                    celldcst1.BorderWidthBottom = 0;
                                                    celldcst1.BorderWidthLeft = .2f;
                                                    celldcst1.BorderWidthTop = 0;
                                                    celldcst1.BorderWidthRight = 0.2f;
                                                    celldcst1.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst1);

                                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst2.BorderWidthBottom = 0;
                                                    celldcst2.BorderWidthLeft = 0f;
                                                    celldcst2.BorderWidthTop = 0;
                                                    celldcst2.BorderWidthRight = .2f;
                                                    celldcst2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst2);
                                                }
                                            }
                                        }
                                        if (maintenancecost > 0)
                                        {
                                            if (STMaintenance == true)
                                            {
                                                if (SCMaintenance == false)
                                                {
                                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                                    CellIGST2.BorderWidthBottom = 0;
                                                    CellIGST2.BorderWidthLeft = .2f;
                                                    CellIGST2.BorderWidthTop = 0f;
                                                    CellIGST2.BorderWidthRight = 0.2f;
                                                    // CellCGST.PaddingBottom = 5;
                                                    // CellCGST.PaddingTop = 5;
                                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(CellIGST2);

                                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst1.Colspan = Noofcolumnsheading;
                                                    celldcst1.BorderWidthBottom = 0;
                                                    celldcst1.BorderWidthLeft = .2f;
                                                    celldcst1.BorderWidthTop = 0;
                                                    celldcst1.BorderWidthRight = 0.2f;
                                                    celldcst1.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst1);

                                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst2.BorderWidthBottom = 0;
                                                    celldcst2.BorderWidthLeft = 0f;
                                                    celldcst2.BorderWidthTop = 0;
                                                    celldcst2.BorderWidthRight = .2f;
                                                    celldcst2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst2);
                                                }
                                            }
                                        }

                                        if (extraonecost > 0)
                                        {
                                            if (STExtraone == true)
                                            {
                                                if (SCExtraone == false)
                                                {
                                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                                    CellIGST2.BorderWidthBottom = 0;
                                                    CellIGST2.BorderWidthLeft = .2f;
                                                    CellIGST2.BorderWidthTop = 0f;
                                                    CellIGST2.BorderWidthRight = 0.2f;
                                                    // CellCGST.PaddingBottom = 5;
                                                    // CellCGST.PaddingTop = 5;
                                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(CellIGST2);

                                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst1.Colspan = Noofcolumnsheading;
                                                    celldcst1.BorderWidthBottom = 0;
                                                    celldcst1.BorderWidthLeft = .2f;
                                                    celldcst1.BorderWidthTop = 0;
                                                    celldcst1.BorderWidthRight = 0.2f;
                                                    celldcst1.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst1);

                                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst2.BorderWidthBottom = 0;
                                                    celldcst2.BorderWidthLeft = 0f;
                                                    celldcst2.BorderWidthTop = 0;
                                                    celldcst2.BorderWidthRight = .2f;
                                                    celldcst2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst2);
                                                }
                                            }
                                        }
                                        if (extratwocost > 0)
                                        {
                                            if (STExtratwo == true)
                                            {
                                                if (SCExtratwo == false)
                                                {
                                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                                    CellIGST2.BorderWidthBottom = 0;
                                                    CellIGST2.BorderWidthLeft = .2f;
                                                    CellIGST2.BorderWidthTop = 0f;
                                                    CellIGST2.BorderWidthRight = 0.2f;
                                                    // CellCGST.PaddingBottom = 5;
                                                    // CellCGST.PaddingTop = 5;
                                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(CellIGST2);

                                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst1.Colspan = Noofcolumnsheading;
                                                    celldcst1.BorderWidthBottom = 0;
                                                    celldcst1.BorderWidthLeft = .2f;
                                                    celldcst1.BorderWidthTop = 0;
                                                    celldcst1.BorderWidthRight = 0.2f;
                                                    celldcst1.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst1);

                                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                    celldcst2.BorderWidthBottom = 0;
                                                    celldcst2.BorderWidthLeft = 0f;
                                                    celldcst2.BorderWidthTop = 0;
                                                    celldcst2.BorderWidthRight = .2f;
                                                    celldcst2.BorderColor = BaseColor.BLACK;
                                                    tempTable22.AddCell(celldcst2);
                                                }
                                            }
                                        }

                                        #endregion

                                        #endregion



                                        decimal GSTDiscounts = 0;

                                        if (STDiscountone == true)
                                        {
                                            if (discountone > 0)
                                            {

                                                PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellbbCGST.Colspan = colCount - Noofcolumns;
                                                CellbbCGST.BorderWidthBottom = 0;
                                                CellbbCGST.BorderWidthLeft = .2f;
                                                CellbbCGST.BorderWidthTop = 0f;
                                                CellbbCGST.BorderWidthRight = 0.2f;
                                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellbbCGST);

                                                CellbbCGST = new PdfPCell(new Phrase(discountonetitle, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellbbCGST.Colspan = Noofcolumnsheading;
                                                CellbbCGST.BorderWidthBottom = 0;
                                                CellbbCGST.BorderWidthLeft = .2f;
                                                CellbbCGST.BorderWidthTop = 0.2f;
                                                CellbbCGST.BorderWidthRight = 0f;
                                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellbbCGST);

                                                CellbbCGST = new PdfPCell(new Phrase(discountone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellbbCGST.BorderWidthBottom = 0;
                                                CellbbCGST.BorderWidthLeft = 0.2f;
                                                CellbbCGST.BorderWidthTop = 0.2f;
                                                CellbbCGST.BorderWidthRight = .2f;
                                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellbbCGST);
                                                GSTDiscounts += discountone;



                                            }
                                        }

                                        if (STDiscounttwo == true)
                                        {
                                            if (discounttwo > 0)
                                            {
                                                PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellbbCGST.Colspan = colCount - Noofcolumns;
                                                CellbbCGST.BorderWidthBottom = 0;
                                                CellbbCGST.BorderWidthLeft = .2f;
                                                CellbbCGST.BorderWidthTop = 0f;
                                                CellbbCGST.BorderWidthRight = 0.2f;
                                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellbbCGST);

                                                CellbbCGST = new PdfPCell(new Phrase(discounttwotitle, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellbbCGST.Colspan = Noofcolumnsheading;
                                                CellbbCGST.BorderWidthBottom = 0;
                                                CellbbCGST.BorderWidthLeft = .2f;
                                                CellbbCGST.BorderWidthTop = 0.2f;
                                                CellbbCGST.BorderWidthRight = 0f;
                                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellbbCGST);

                                                CellbbCGST = new PdfPCell(new Phrase(discounttwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellbbCGST.BorderWidthBottom = 0;
                                                CellbbCGST.BorderWidthLeft = 0.2f;
                                                CellbbCGST.BorderWidthTop = 0.2f;
                                                CellbbCGST.BorderWidthRight = .2f;
                                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellbbCGST);
                                                GSTDiscounts += discounttwo;
                                            }
                                        }

                                        if (((Grandtotal - (CGST + SGST + IGST)) - totalamount) > 0)
                                        {
                                            PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                            CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellbbCGST.Colspan = colCount - Noofcolumns;
                                            CellbbCGST.BorderWidthBottom = 0;
                                            CellbbCGST.BorderWidthLeft = .2f;
                                            CellbbCGST.BorderWidthTop = 0f;
                                            CellbbCGST.BorderWidthRight = 0.2f;
                                            CellbbCGST.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellbbCGST);

                                            CellbbCGST = new PdfPCell(new Phrase("Total Before Tax", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellbbCGST.Colspan = Noofcolumnsheading;
                                            CellbbCGST.BorderWidthBottom = 0;
                                            CellbbCGST.BorderWidthLeft = .2f;
                                            CellbbCGST.BorderWidthTop = 0.2f;
                                            CellbbCGST.BorderWidthRight = 0f;
                                            CellbbCGST.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellbbCGST);

                                            CellbbCGST = new PdfPCell(new Phrase(((Grandtotal - (CGST + SGST + IGST) - GSTDiscounts)).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                            CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                            CellbbCGST.BorderWidthBottom = 0;
                                            CellbbCGST.BorderWidthLeft = 0.2f;
                                            CellbbCGST.BorderWidthTop = 0.2f;
                                            CellbbCGST.BorderWidthRight = .2f;
                                            CellbbCGST.BorderColor = BaseColor.BLACK;
                                            tempTable22.AddCell(CellbbCGST);
                                        }



                                        #region for taxes

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
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell celldd2 = new PdfPCell(new Phrase("Service Tax @ " + scpercent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd2.Colspan = Noofcolumnsheading;
                                                celldd2.BorderWidthBottom = 0;
                                                celldd2.BorderWidthLeft = .2f;
                                                celldd2.BorderWidthTop = 0.2f;
                                                celldd2.BorderWidthRight = 0f;
                                                //celldd2.PaddingBottom = 5;
                                                //celldd2.PaddingTop = 5;
                                                celldd2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldd2);


                                                PdfPCell celldd4 = new PdfPCell(new Phrase(servicetax.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd4.BorderWidthBottom = 0;
                                                celldd4.BorderWidthLeft = 0.2f;
                                                celldd4.BorderWidthTop = 0.2f;
                                                celldd4.BorderWidthRight = .2f;
                                                celldd4.BorderColor = BaseColor.BLACK;
                                                //celldd4.PaddingBottom = 5;
                                                //celldd4.PaddingTop = 5;
                                                tempTable22.AddCell(celldd4);

                                            }

                                            if (sbcess > 0)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                string SBCESSPresent = DtTaxes.Rows[0]["SBCess"].ToString();
                                                PdfPCell celldd2 = new PdfPCell(new Phrase("Swachh Bharat Cess @ " + SBCESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd2.Colspan = Noofcolumnsheading;
                                                celldd2.BorderWidthBottom = 0;
                                                celldd2.BorderWidthLeft = .2f;
                                                celldd2.BorderWidthTop = 0.2f;
                                                celldd2.BorderWidthRight = 0f;
                                                celldd2.BorderColor = BaseColor.BLACK;
                                                // celldd2.PaddingBottom = 5;
                                                // celldd2.PaddingTop = 5;
                                                tempTable22.AddCell(celldd2);


                                                PdfPCell celldd4 = new PdfPCell(new Phrase(sbcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd4.BorderWidthBottom = 0;
                                                celldd4.BorderWidthLeft = 0.2f;
                                                celldd4.BorderWidthTop = 0.2f;
                                                celldd4.BorderWidthRight = .2f;
                                                celldd4.BorderColor = BaseColor.BLACK;
                                                //celldd4.PaddingBottom = 5;
                                                //celldd4.PaddingTop = 5;
                                                tempTable22.AddCell(celldd4);

                                            }


                                            if (kkcess > 0)
                                            {

                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                string KKCESSPresent = DtTaxes.Rows[0]["KKCess"].ToString();
                                                PdfPCell Cellmtcesskk1 = new PdfPCell(new Phrase("Krishi Kalyan Cess @ " + KKCESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                Cellmtcesskk1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                Cellmtcesskk1.Colspan = Noofcolumnsheading;
                                                Cellmtcesskk1.BorderWidthBottom = 0;
                                                Cellmtcesskk1.BorderWidthLeft = .2f;
                                                Cellmtcesskk1.BorderWidthTop = 0.2f;
                                                Cellmtcesskk1.BorderWidthRight = 0f;
                                                // celldd2.PaddingBottom = 5;
                                                // celldd2.PaddingTop = 5;
                                                Cellmtcesskk1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(Cellmtcesskk1);

                                                PdfPCell Cellmtcesskk2 = new PdfPCell(new Phrase(kkcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                Cellmtcesskk2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                Cellmtcesskk2.BorderWidthBottom = 0;
                                                Cellmtcesskk2.BorderWidthLeft = 0.2f;
                                                Cellmtcesskk2.BorderWidthTop = 0.2f;
                                                Cellmtcesskk2.BorderWidthRight = .2f;
                                                Cellmtcesskk2.BorderColor = BaseColor.BLACK;
                                                //celldd4.PaddingBottom = 5;
                                                //celldd4.PaddingTop = 5;
                                                tempTable22.AddCell(Cellmtcesskk2);

                                            }

                                            #region for GST as on 17-6-2017

                                            if (CGST > 0)
                                            {
                                                PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCGST.Colspan = colCount - Noofcolumns;
                                                CellCGST.BorderWidthBottom = 0;
                                                CellCGST.BorderWidthLeft = .2f;
                                                CellCGST.BorderWidthTop = 0f;
                                                CellCGST.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellCGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellCGST);

                                                CellCGST = new PdfPCell(new Phrase(CGSTAlias + " @ " + CGSTPrc + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                                CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCGST.Colspan = Noofcolumnsheading;
                                                CellCGST.BorderWidthBottom = 0;
                                                CellCGST.BorderWidthLeft = .2f;
                                                CellCGST.BorderWidthTop = 0.2f;
                                                CellCGST.BorderWidthRight = 0f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellCGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellCGST);

                                                PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(CGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCGSTAmt.BorderWidthBottom = 0;
                                                CellCGSTAmt.BorderWidthLeft = 0.2f;
                                                CellCGSTAmt.BorderWidthTop = 0.2f;
                                                CellCGSTAmt.BorderWidthRight = .2f;
                                                CellCGSTAmt.BorderColor = BaseColor.BLACK;
                                                //CellCGSTAmt.PaddingBottom = 5;
                                                //CellCGSTAmt.PaddingTop = 5;
                                                tempTable22.AddCell(CellCGSTAmt);

                                            }


                                            if (SGST > 0)
                                            {
                                                PdfPCell CellSGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellSGST.Colspan = colCount - Noofcolumns;
                                                CellSGST.BorderWidthBottom = 0;
                                                CellSGST.BorderWidthLeft = .2f;
                                                CellSGST.BorderWidthTop = 0f;
                                                CellSGST.BorderWidthRight = 0.2f;
                                                // CellSGST.PaddingBottom = 5;
                                                // CellSGST.PaddingTop = 5;
                                                CellSGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellSGST);

                                                CellSGST = new PdfPCell(new Phrase(SGSTAlias + " @ " + SGSTPrc + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                                CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellSGST.Colspan = Noofcolumnsheading;
                                                CellSGST.BorderWidthBottom = 0;
                                                CellSGST.BorderWidthLeft = .2f;
                                                CellSGST.BorderWidthTop = 0.2f;
                                                CellSGST.BorderWidthRight = 0f;
                                                // CellSGST.PaddingBottom = 5;
                                                // CellSGST.PaddingTop = 5;
                                                CellSGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellSGST);

                                                PdfPCell CellSGSTAmt = new PdfPCell(new Phrase(SGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellSGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellSGSTAmt.BorderWidthBottom = 0;
                                                CellSGSTAmt.BorderWidthLeft = 0.2f;
                                                CellSGSTAmt.BorderWidthTop = 0.2f;
                                                CellSGSTAmt.BorderWidthRight = .2f;
                                                CellSGSTAmt.BorderColor = BaseColor.BLACK;
                                                //CellSGSTAmt.PaddingBottom = 5;
                                                //CellSGSTAmt.PaddingTop = 5;
                                                tempTable22.AddCell(CellSGSTAmt);


                                            }

                                            if (IGST > 0)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell CellIGST = new PdfPCell(new Phrase(IGSTAlias + " @ " + IGSTPrc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST.Colspan = Noofcolumnsheading;
                                                CellIGST.BorderWidthBottom = 0;
                                                CellIGST.BorderWidthLeft = .2f;
                                                CellIGST.BorderWidthTop = 0.2f;
                                                CellIGST.BorderWidthRight = 0f;
                                                // CellIGST.PaddingBottom = 5;
                                                // CellIGST.PaddingTop = 5;
                                                CellIGST.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST);

                                                PdfPCell CellIGSTAmt = new PdfPCell(new Phrase(IGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGSTAmt.BorderWidthBottom = 0;
                                                CellIGSTAmt.BorderWidthLeft = 0.2f;
                                                CellIGSTAmt.BorderWidthTop = 0.2f;
                                                CellIGSTAmt.BorderWidthRight = .2f;
                                                CellIGSTAmt.BorderColor = BaseColor.BLACK;
                                                //CellIGSTAmt.PaddingBottom = 5;
                                                //CellIGSTAmt.PaddingTop = 5;
                                                tempTable22.AddCell(CellIGSTAmt);


                                            }

                                            if (Cess1 > 0)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell CellCess1 = new PdfPCell(new Phrase(Cess1Alias + " @ " + Cess1Prc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellCess1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCess1.Colspan = Noofcolumnsheading;
                                                CellCess1.BorderWidthBottom = 0;
                                                CellCess1.BorderWidthLeft = .2f;
                                                CellCess1.BorderWidthTop = 0.2f;
                                                CellCess1.BorderWidthRight = 0f;
                                                // CellCess1.PaddingBottom = 5;
                                                // CellCess1.PaddingTop = 5;
                                                CellCess1.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellCess1);

                                                PdfPCell CellCess1Amt = new PdfPCell(new Phrase(Cess1.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellCess1Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCess1Amt.BorderWidthBottom = 0;
                                                CellCess1Amt.BorderWidthLeft = 0.2f;
                                                CellCess1Amt.BorderWidthTop = 0.2f;
                                                CellCess1Amt.BorderWidthRight = .2f;
                                                CellCess1Amt.BorderColor = BaseColor.BLACK;
                                                //CellCess1Amt.PaddingBottom = 5;
                                                //CellCess1Amt.PaddingTop = 5;
                                                tempTable22.AddCell(CellCess1Amt);

                                            }


                                            if (Cess2 > 0)
                                            {
                                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellIGST2.Colspan = colCount - Noofcolumns;
                                                CellIGST2.BorderWidthBottom = 0;
                                                CellIGST2.BorderWidthLeft = .2f;
                                                CellIGST2.BorderWidthTop = 0f;
                                                CellIGST2.BorderWidthRight = 0.2f;
                                                // CellCGST.PaddingBottom = 5;
                                                // CellCGST.PaddingTop = 5;
                                                CellIGST2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellIGST2);

                                                PdfPCell CellCess2 = new PdfPCell(new Phrase(Cess2Alias + " @ " + Cess2Prc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellCess2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCess2.Colspan = Noofcolumnsheading;
                                                CellCess2.BorderWidthBottom = 0;
                                                CellCess2.BorderWidthLeft = .2f;
                                                CellCess2.BorderWidthTop = 0.2f;
                                                CellCess2.BorderWidthRight = 0f;
                                                // CellCess2.PaddingBottom = 5;
                                                // CellCess2.PaddingTop = 5;
                                                CellCess2.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(CellCess2);

                                                PdfPCell CellCess2Amt = new PdfPCell(new Phrase(Cess2.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                CellCess2Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                CellCess2Amt.BorderWidthBottom = 0;
                                                CellCess2Amt.BorderWidthLeft = 0.2f;
                                                CellCess2Amt.BorderWidthTop = 0.2f;
                                                CellCess2Amt.BorderWidthRight = .2f;
                                                CellCess2Amt.BorderColor = BaseColor.BLACK;
                                                //CellCess2Amt.PaddingBottom = 5;
                                                //CellCess2Amt.PaddingTop = 5;
                                                tempTable22.AddCell(CellCess2Amt);

                                            }

                                            #endregion for GST

                                            if (cess > 0)
                                            {

                                                string CESSPresent = DtTaxes.Rows[0]["Cess"].ToString();
                                                PdfPCell celldd2 = new PdfPCell(new Phrase("CESS @ " + CESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd2.Colspan = colCount - 1;
                                                celldd2.BorderWidthBottom = 0;
                                                celldd2.BorderWidthLeft = .2f;
                                                celldd2.BorderWidthTop = 0.2f;
                                                celldd2.BorderWidthRight = 0f;
                                                celldd2.BorderColor = BaseColor.BLACK;
                                                //celldd2.PaddingBottom = 5;
                                                //celldd2.PaddingTop = 5;
                                                tempTable22.AddCell(celldd2);


                                                PdfPCell celldd4 = new PdfPCell(new Phrase(cess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldd4.BorderWidthBottom = 0;
                                                celldd4.BorderWidthLeft = 0.2f;
                                                celldd4.BorderWidthTop = 0.2f;
                                                celldd4.BorderWidthRight = .2f;
                                                celldd4.BorderColor = BaseColor.BLACK;
                                                //celldd4.PaddingBottom = 5;
                                                //celldd4.PaddingTop = 5;
                                                tempTable22.AddCell(celldd4);

                                            }

                                            if (shecess > 0)
                                            {


                                                string SHECESSPresent = DtTaxes.Rows[0]["shecess"].ToString();
                                                PdfPCell celldf2 = new PdfPCell(new Phrase("S&H Ed.CESS @ " + SHECESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldf2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldf2.Colspan = colCount - 1;
                                                celldf2.BorderWidthBottom = 0;
                                                celldf2.BorderWidthLeft = .2f;
                                                celldf2.BorderWidthTop = 0.2f;
                                                celldf2.BorderWidthRight = 0f;
                                                celldf2.BorderColor = BaseColor.BLACK;
                                                //celldf2.PaddingBottom = 5;
                                                //celldf2.PaddingTop = 5;
                                                tempTable22.AddCell(celldf2);


                                                PdfPCell celldf4 = new PdfPCell(new Phrase(shecess.ToString("0.00"),
                                                    FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                celldf4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldf4.BorderWidthBottom = 0;
                                                celldf4.BorderWidthLeft = 0.2f;
                                                celldf4.BorderWidthTop = 0.2f;
                                                celldf4.BorderWidthRight = .2f;
                                                celldf4.BorderColor = BaseColor.BLACK;
                                                ////celldf4.PaddingBottom = 5;
                                                //celldf4.PaddingTop = 5;
                                                //celldf4.BorderColor = BaseColor.BLACK;
                                                tempTable22.AddCell(celldf4);
                                            }
                                            #region When Extra data is checked and STcheck is false and SCcheck is true

                                            if (machinarycost > 0)
                                            {
                                                if (STMachinary == false)
                                                {
                                                    if (SCMachinary == true)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }


                                            }
                                            if (materialcost > 0)
                                            {
                                                if (STMaterial == false)
                                                {
                                                    if (SCMaterial == true)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }
                                            if (maintenancecost > 0)
                                            {
                                                if (STMaintenance == false)
                                                {
                                                    if (SCMaintenance == true)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }

                                            if (extraonecost > 0)
                                            {
                                                if (STExtraone == false)
                                                {
                                                    if (SCExtraone == true)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }
                                            if (extratwocost > 0)
                                            {
                                                if (STExtratwo == false)
                                                {
                                                    if (SCExtratwo == true)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }

                                            #endregion



                                            #region When Extra data is checked and STcheck is false and SCcheck is true

                                            if (machinarycost > 0)
                                            {
                                                if (STMachinary == false)
                                                {
                                                    if (SCMachinary == false)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
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
                                                    if (SCMaterial == false)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }
                                            if (maintenancecost > 0)
                                            {
                                                if (STMaintenance == false)
                                                {
                                                    if (SCMaintenance == false)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }

                                            if (extraonecost > 0)
                                            {
                                                if (STExtraone == false)
                                                {
                                                    if (SCExtraone == false)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }
                                            if (extratwocost > 0)
                                            {
                                                if (STExtratwo == false)
                                                {
                                                    if (SCExtratwo == false)
                                                    {
                                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                                        CellIGST2.BorderWidthBottom = 0;
                                                        CellIGST2.BorderWidthLeft = .2f;
                                                        CellIGST2.BorderWidthTop = 0f;
                                                        CellIGST2.BorderWidthRight = 0.2f;
                                                        // CellCGST.PaddingBottom = 5;
                                                        // CellCGST.PaddingTop = 5;
                                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(CellIGST2);

                                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst1.Colspan = Noofcolumnsheading;
                                                        celldcst1.BorderWidthBottom = 0;
                                                        celldcst1.BorderWidthLeft = .2f;
                                                        celldcst1.BorderWidthTop = 0.2f;
                                                        celldcst1.BorderWidthRight = 0.2f;
                                                        celldcst1.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst1);

                                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                        celldcst2.BorderWidthBottom = 0;
                                                        celldcst2.BorderWidthLeft = 0f;
                                                        celldcst2.BorderWidthTop = 0.2f;
                                                        celldcst2.BorderWidthRight = .2f;
                                                        celldcst2.BorderColor = BaseColor.BLACK;
                                                        tempTable22.AddCell(celldcst2);
                                                    }
                                                }
                                            }

                                            #endregion

                                            decimal totaltaxamt = 0;
                                            totaltaxamt = CGST + SGST + IGST;
                                            if (totaltaxamt > 0)
                                            {
                                                PdfPCell celldfTax2 = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                celldfTax2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldfTax2.Colspan = colCount - 1;
                                                celldfTax2.BorderWidthBottom = 0;
                                                celldfTax2.BorderWidthLeft = .2f;
                                                celldfTax2.BorderWidthTop = 0;
                                                celldfTax2.BorderWidthRight = 0f;
                                                celldfTax2.BorderColor = BaseColor.BLACK;
                                                //celldfTax2.PaddingBottom = 5;
                                                //celldfTax2.PaddingTop = 5;
                                                //tempTable22.AddCell(celldfTax2);

                                                PdfPCell celldf4tax = new PdfPCell(new Phrase(totaltaxamt.ToString("#,##0.00"),
                                                    FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                celldf4tax.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                                celldf4tax.BorderWidthBottom = 0;
                                                celldf4tax.BorderWidthLeft = 0.2f;
                                                celldf4tax.BorderWidthTop = 0;
                                                celldf4tax.BorderWidthRight = .2f;
                                                celldf4tax.BorderColor = BaseColor.BLACK;
                                                ////celldf4tax.PaddingBottom = 5;
                                                //celldf4tax.PaddingTop = 5;
                                                //celldf4tax.BorderColor = BaseColor.BLACK;
                                                //tempTable22.AddCell(celldf4tax);
                                            }
                                        }


                                        #endregion for taxes

                                        decimal GrandTotalVal = Grandtotal;

                                        decimal GrandtotalRoundOff = Math.Round(GrandTotalVal, 0);
                                        decimal RoundOff = (GrandtotalRoundOff - GrandTotalVal);
                                        decimal GrandtotalValue = (GrandTotalVal + RoundOff);

                                        PdfPCell cellgrandto = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        cellgrandto.Colspan = colCount - Noofcolumns;
                                        cellgrandto.BorderWidthBottom = 0;
                                        cellgrandto.BorderWidthLeft = .2f;
                                        cellgrandto.BorderWidthTop = 0f;
                                        cellgrandto.BorderWidthRight = 0.2f;
                                        cellgrandto.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(cellgrandto);

                                        cellgrandto = new PdfPCell(new Phrase("Round off", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        cellgrandto.Colspan = Noofcolumnsheading;
                                        cellgrandto.BorderWidthBottom = 0;
                                        cellgrandto.BorderWidthLeft = .2f;
                                        cellgrandto.BorderWidthTop = 0.2f;
                                        cellgrandto.BorderWidthRight = 0f;
                                        cellgrandto.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(cellgrandto);

                                        cellgrandto = new PdfPCell(new Phrase(RoundOff.ToString("N2"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        cellgrandto.BorderWidthBottom = 0;
                                        cellgrandto.BorderWidthLeft = 0.2f;
                                        cellgrandto.BorderWidthTop = 0.2f;
                                        cellgrandto.BorderWidthRight = .2f;
                                        cellgrandto.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(cellgrandto);

                                        cellgrandto = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        cellgrandto.Colspan = colCount - Noofcolumns;
                                        cellgrandto.BorderWidthBottom = 0;
                                        cellgrandto.BorderWidthLeft = .2f;
                                        cellgrandto.BorderWidthTop = 0f;
                                        cellgrandto.BorderWidthRight = 0.2f;
                                        cellgrandto.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(cellgrandto);

                                        cellgrandto = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        cellgrandto.Colspan = Noofcolumnsheading;
                                        cellgrandto.BorderWidthBottom = 0;
                                        cellgrandto.BorderWidthLeft = .2f;
                                        cellgrandto.BorderWidthTop = 0.2f;
                                        cellgrandto.BorderWidthRight = 0f;
                                        cellgrandto.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(cellgrandto);

                                        cellgrandto = new PdfPCell(new Phrase(GrandtotalValue.ToString("N2"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        cellgrandto.BorderWidthBottom = 0;
                                        cellgrandto.BorderWidthLeft = 0.2f;
                                        cellgrandto.BorderWidthTop = 0.2f;
                                        cellgrandto.BorderWidthRight = .2f;
                                        cellgrandto.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(cellgrandto);


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

                                        if (GSTLineitem == true)
                                        {
                                            #region FooterTable
                                            PdfPTable tablev = new PdfPTable(7);
                                            tablev.TotalWidth = 580f;
                                            tablev.LockedWidth = true;
                                            float[] widthss = new float[] { 1.5f, 2.5f, 2f, 2f, 2f, 2f, 3f };
                                            tablev.SetWidths(widthss);

                                            if ((CGST + SGST) > 0)
                                            {
                                                cell = new PdfPCell(new Phrase("HSN Code", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0;
                                                cell.BorderWidthLeft = .2f;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Taxable Value", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Central Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 2;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("State Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 2;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);
                                            }
                                            else
                                            {

                                                cell = new PdfPCell(new Phrase("HSN Code", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0;
                                                cell.BorderWidthLeft = .2f;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Taxable Value", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 2;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Integrated Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.Colspan = 2;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.Colspan = 2;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);
                                            }





                                            //2ndrow
                                            if ((CGST + SGST) > 0)
                                            {

                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = .2f;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);


                                                cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);
                                            }
                                            else
                                            {
                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = .2f;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 2;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0.5f;
                                                cell.Colspan = 1;
                                                cell.PaddingBottom = 3;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.PaddingBottom = 3;
                                                cell.Colspan = 1;
                                                tablev.AddCell(cell);

                                                cell = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 1;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 1.5f;
                                                cell.PaddingBottom = 3;
                                                cell.Colspan = 2;
                                                tablev.AddCell(cell);
                                            }


                                            DataTable DtDesgnWise = null;
                                            var SPDesgnWise = "DesginWiseGSTAmounts";
                                            Hashtable htDesgnWise = new Hashtable();
                                            htDesgnWise.Add("@ClientID", lblclientid.Text);
                                            htDesgnWise.Add("@Month", Month);
                                            htDesgnWise.Add("@BillType", ddlBillType.SelectedIndex);
                                            htDesgnWise.Add("@BillNo", lblbillno.Text);

                                            DtDesgnWise = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPDesgnWise, htDesgnWise);


                                            decimal TotalTaxableval = 0;
                                            decimal CGSTAmt = 0;
                                            decimal SGSTAmt = 0;
                                            decimal IGSTAmt = 0;
                                            decimal GrandTotalTaxableval = 0;
                                            decimal GrandCGSTAmt = 0;
                                            decimal GrandSGSTAmt = 0;
                                            decimal GrandIGSTAmt = 0;
                                            decimal CGSTRate = 0;
                                            decimal SGSTRate = 0;
                                            decimal IGSTRate = 0;
                                            string HSNCode = "";

                                            if (DtDesgnWise.Rows.Count > 0)
                                            {
                                                for (int k = 0; k < DtDesgnWise.Rows.Count; k++)
                                                {
                                                    TotalTaxableval = decimal.Parse(DtDesgnWise.Rows[k]["TotalTaxableval"].ToString());
                                                    CGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["CGSTAmt"].ToString());
                                                    SGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["SGSTAmt"].ToString());
                                                    IGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["IGSTAmt"].ToString());
                                                    CGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["CGSTPrc"].ToString());
                                                    SGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["SGSTPrc"].ToString());
                                                    IGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["IGSTPrc"].ToString());
                                                    HSNCode = DtDesgnWise.Rows[k]["HSNCode"].ToString();

                                                    if ((CGST + SGST) > 0)
                                                    {
                                                        cell = new PdfPCell(new Phrase(HSNCode, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 0;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = .2f;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        tablev.AddCell(cell);

                                                        cell = new PdfPCell(new Phrase(TotalTaxableval.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        tablev.AddCell(cell);
                                                        GrandTotalTaxableval += (TotalTaxableval);

                                                        cell = new PdfPCell(new Phrase(CGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        cell.PaddingBottom = 3;
                                                        tablev.AddCell(cell);

                                                        cell = new PdfPCell(new Phrase(CGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        cell.PaddingBottom = 3;
                                                        tablev.AddCell(cell);
                                                        GrandCGSTAmt += CGSTAmt;


                                                        cell = new PdfPCell(new Phrase(SGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        cell.PaddingBottom = 3;
                                                        tablev.AddCell(cell);

                                                        cell = new PdfPCell(new Phrase(SGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        cell.PaddingBottom = 3;
                                                        tablev.AddCell(cell);
                                                        GrandSGSTAmt += SGSTAmt;

                                                        cell = new PdfPCell(new Phrase((GrandCGSTAmt + GrandSGSTAmt).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        cell.PaddingBottom = 3;
                                                        tablev.AddCell(cell);
                                                    }
                                                    else
                                                    {
                                                        cell = new PdfPCell(new Phrase(HSNCode, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 0;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = .2f;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        tablev.AddCell(cell);

                                                        cell = new PdfPCell(new Phrase(TotalTaxableval.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 2;
                                                        tablev.AddCell(cell);
                                                        GrandTotalTaxableval += (TotalTaxableval);


                                                        cell = new PdfPCell(new Phrase(IGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 0.5f;
                                                        cell.Colspan = 1;
                                                        cell.PaddingBottom = 3;
                                                        tablev.AddCell(cell);

                                                        cell = new PdfPCell(new Phrase(IGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 1.5f;
                                                        cell.PaddingBottom = 3;
                                                        cell.Colspan = 1;
                                                        tablev.AddCell(cell);
                                                        GrandIGSTAmt += IGSTAmt;

                                                        cell = new PdfPCell(new Phrase(GrandIGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                        cell.HorizontalAlignment = 2;
                                                        cell.BorderWidthBottom = 0.5f;
                                                        cell.BorderWidthLeft = 0;
                                                        cell.BorderWidthTop = 0;
                                                        cell.BorderWidthRight = 1.5f;
                                                        cell.PaddingBottom = 3;
                                                        cell.Colspan = 2;
                                                        tablev.AddCell(cell);
                                                    }



                                                }


                                                decimal TotalAmount = GrandCGSTAmt + GrandIGSTAmt + GrandSGSTAmt;

                                                string GTotal = TotalAmount.ToString("0.00");
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
                                                    inwords = " Rupees " + rupee + "" + paise + " Paise Only /-";

                                                }
                                                else
                                                {
                                                    rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                                                    inwords = " Rupees " + rupee + " Only /-";
                                                }


                                                cell = new PdfPCell(new Phrase("Total Tax Amount (in words) :" + inwords, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0.5f;
                                                cell.BorderWidthLeft = .2f;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = .2f;
                                                cell.PaddingBottom = 3;
                                                cell.Colspan = 7;
                                                tablev.AddCell(cell);


                                            }


                                            document.Add(tablev);

                                            #endregion
                                        }

                                        #endregion

                                        #region footer

                                        PdfPTable Addterms = new PdfPTable(6);
                                        Addterms.TotalWidth = 580f;
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
                                        Childterms.TotalWidth = 335f;
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

                                        if (Bdt.Rows.Count > 0)
                                        {
                                            if (OurGSTIN.Length > 0)
                                            {


                                                cell = new PdfPCell(new Phrase(OurGSTINAlias, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                                cell.HorizontalAlignment = 0;
                                                cell.BorderWidthBottom = 0;
                                                cell.BorderWidthTop = 0;
                                                cell.BorderWidthRight = 0f;
                                                cell.BorderWidthLeft = .2f;
                                                cell.Colspan = 1;
                                                cell.BorderColor = BaseColor.BLACK;
                                                Childterms.AddCell(cell);


                                                cell = new PdfPCell(new Phrase(": " + OurGSTIN, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                                        chilk.TotalWidth = 245f;
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

                                        cell = new PdfPCell(new Phrase("\n\n\n Authorised Signatory", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
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
                                        //chilk.AddCell(cell);


                                        PdfPCell Chid4 = new PdfPCell(chilk);
                                        Chid4.Border = 0;
                                        Chid4.Colspan = 3;
                                        Chid4.HorizontalAlignment = 0;
                                        Addterms.AddCell(Chid4);


                                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 1;
                                        cell.BorderWidthBottom = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0;
                                        cell.BorderWidthLeft = 0;
                                        cell.Colspan = 6;
                                        // Addterms.AddCell(cell);

                                        document.Add(Addterms);


                                        #endregion

                                        document.NewPage();
                                        document.NewPage();

                                    }
                                }
                            }
                        }
                        document.Close();
                    }
                }
                catch (Exception ex)
                {

                }

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
            else
            {
                return;
            }

        }

        protected void btndownloadforbills_Click(object sender, EventArgs e)
        {
            int fontsize = 10;
            int font = 10;
            var ClientId = "";
            var BillType = "";
            int Month = 0;

            MemoryStream ms = new MemoryStream();

            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            document.AddTitle("Webwonders");
            document.AddAuthor("DIYOS");
            document.AddSubject("Invoice");
            document.AddKeywords("Keyword1, keyword2, …");

            try
            {

                string QueryforBillNo = "select BillNo,UnitId,case convert(varchar(10),BillDt,103) when '01/01/1900' then '' else convert(nvarchar(20),BillDt,103) end BillDt,'N' as Type,month from UnitBill where BillNo between '" + txtfrombillno.Text + "' and '" + txttobillno.Text + "' union select BillNo,UnitId,case convert(varchar(10),BillDt,103) when '01/01/1900' then '' else convert(nvarchar(20),BillDt,103) end BillDt,BillType,month as Type from MUnitBill where BillNo between '" + txtfrombillno.Text + "' and '" + txttobillno.Text + "' order by BillNo asc";
                DataTable dtforbillno = config.ExecuteAdaptorAsyncWithQueryParams(QueryforBillNo).Result;
                if (dtforbillno.Rows.Count > 0)
                {
                    for (int M = 0; M < dtforbillno.Rows.Count; M++)
                    {

                        #region for CompanyInfo
                        string strQry = "Select * from CompanyInfo ";
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
                        if (compInfo.Rows.Count > 0)
                        {
                            companyName = compInfo.Rows[0]["CompanyName"].ToString();
                            companyAddress = compInfo.Rows[0]["Address"].ToString();
                            companyaddressline = compInfo.Rows[0]["Addresslineone"].ToString();
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
                        }

                        #endregion

                        var ContractID = "";
                        int monthval = 0;
                        int yearval = 0;

                        ClientId = dtforbillno.Rows[M]["UnitId"].ToString();
                        Month = Convert.ToInt32(dtforbillno.Rows[M]["Month"]);
                        BillType = dtforbillno.Rows[M]["Type"].ToString();

                        #region  Begin Get Contract Id Based on The Last Day

                        DateTime LastDay = DateTime.Now;
                        LastDay = DateTime.Parse(dtforbillno.Rows[M]["BillDt"].ToString(), CultureInfo.GetCultureInfo("en-gb"));

                        Hashtable HtGetContractID = new Hashtable();
                        var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                        HtGetContractID.Add("@clientid", ClientId);
                        HtGetContractID.Add("@LastDay", LastDay);
                        DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                        if (DTContractID.Rows.Count > 0)
                        {
                            ContractID = DTContractID.Rows[0]["contractid"].ToString();
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Contract Details Are Not  Avaialable For This Client.');", true);
                            //return;
                        }


                        #endregion  End Get Contract Id Based on The Last Day

                        #region
                        string SqlQuryForServiCharge = "select ContractId,servicecharge,PODate, isnull(EBD.ESiNO,'') EsiBranchname,isnull(PBD.PFNo,'') PFBranchname,convert(nvarchar(20), ContractStartDate, 103) as ContractStartDate,ServiceChargeType,Description,IncludeST,ServiceTax75,Pono,typeofwork,'' billnotes,isnull(ServiceChargeDesc,'') as ServiceChargeDesc,GSTLineitem from contracts  C left join EsiBranchDetails EBD on EBD.EsiBranchid=isnull(C.Esibranch,0) left join PFBranchDetails PBD on PBD.PFBranchid=isnull(C.PFbranch,0)   where " +
                            " clientid ='" + ClientId + "' and ContractId='" + ContractID + "'";
                        DataTable DtServicecharge = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuryForServiCharge).Result;
                        string Typeofwork = "";
                        string BillNotes = "";
                        string ServiceCharge = "0";
                        string strSCType = "";
                        string strDescription = "We are presenting our bill for the House Keeping Services Provided at your establishment. Kindly release the payment at the earliest";
                        bool bSCType = false;
                        bool GSTLineitem = false;
                        string strIncludeST = "";
                        string ContractStartDate = "";
                        string strST75 = "";
                        bool bIncludeST = false;
                        bool bST75 = false;
                        string POContent = "";
                        string PODate = "";
                        string CnPFNo = "";
                        string CnESINo = "";
                        string Location = "";
                        string ReversCharges = "";
                        string ServiceChargeDesc = "";
                        // string ServiceTaxCategory = "";
                        if (DtServicecharge.Rows.Count > 0)
                        {
                            PODate = DtServicecharge.Rows[0]["PODate"].ToString();
                            if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceCharge"].ToString()) == false)
                            {
                                ServiceCharge = DtServicecharge.Rows[0]["ServiceCharge"].ToString();
                            }
                            if (String.IsNullOrEmpty(DtServicecharge.Rows[0]["ServiceChargeType"].ToString()) == false)
                            {
                                strSCType = DtServicecharge.Rows[0]["ServiceChargeType"].ToString();
                            }
                            string tempDescription = DtServicecharge.Rows[0]["Description"].ToString();
                            if (tempDescription.Trim().Length > 0)
                            {
                                strDescription = tempDescription;
                            }
                            if (strSCType.Length > 0)
                            {
                                bSCType = Convert.ToBoolean(strSCType);
                            }
                            GSTLineitem = Convert.ToBoolean(DtServicecharge.Rows[0]["GSTLineitem"].ToString());
                            PFNo = DtServicecharge.Rows[0]["PFBranchname"].ToString().Trim();
                            Esino = DtServicecharge.Rows[0]["EsiBranchname"].ToString().Trim();

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
                            BillNotes = DtServicecharge.Rows[0]["BillNotes"].ToString();
                            // ServiceTaxCategory = DtServicecharge.Rows[0]["ServiceTaxCategory"].ToString();
                            string tempServiceDesc = DtServicecharge.Rows[0]["ServiceChargeDesc"].ToString();
                            if (tempServiceDesc.Trim().Length > 0)
                            {
                                ServiceChargeDesc = tempServiceDesc;
                            }
                        }

                        #endregion

                        #region

                        string selectclientaddress = "select isnull(sg.segname,'') as segname,c.*, s.state as Statename,s.GSTStateCode,gst.gstno,gst.GSTAddress,s1.state as ShipState,s1.GSTStateCode as ShipToStateCode1 from clients c left join Segments sg on c.ClientSegment = sg.SegId  left join states s on s.stateid=c.state left join GSTMaster gst on gst.id=c.ourgstin left join states s1 on s1.stateid=c.ShipToState where clientid= '" + ClientId + "'";
                        DataTable dtclientaddress = config.ExecuteAdaptorAsyncWithQueryParams(selectclientaddress).Result;
                        string OurGSTIN = "";
                        string GSTIN = "";
                        string StateCode = "0";
                        string State = "";
                        string ShipToGSTIN = "";
                        string ShipToStateCode = "0";
                        string ShipToState = "";

                        if (dtclientaddress.Rows.Count > 0)
                        {
                            OurGSTIN = dtclientaddress.Rows[0]["gstno"].ToString();
                            StateCode = dtclientaddress.Rows[0]["GSTStateCode"].ToString();
                            GSTIN = dtclientaddress.Rows[0]["GSTIN"].ToString();
                            State = dtclientaddress.Rows[0]["Statename"].ToString();
                            Location = dtclientaddress.Rows[0]["Location"].ToString();
                            companyAddress = dtclientaddress.Rows[0]["GSTAddress"].ToString();

                            ShipToStateCode = dtclientaddress.Rows[0]["ShipToStateCode1"].ToString();
                            ShipToGSTIN = dtclientaddress.Rows[0]["ShipToGSTIN"].ToString();
                            ShipToState = dtclientaddress.Rows[0]["ShipState"].ToString();
                        }

                        string SelectBillNo = string.Empty;
                        string BillNo = dtforbillno.Rows[M]["BillNo"].ToString();
                        if (dtforbillno.Rows[M]["Type"].ToString() == "N")
                        {

                            SelectBillNo = "Select RIGHT(billno,5) as DisplayBillNo,DATENAME(month,convert(DATETIME,case when Len(Month)>3 then(LEFT(Month, 2) + '/' + Left(08, 1) + '/' + Right(Month, 2))else (LEFT(Month, 1) + '/' + Left(08, 1) + '/' + Right(Month, 2)) end )) +'-' + Left(20, 2) + RIGHT(Month, 2) as Monthname,* from Unitbill where BillNo='" + BillNo + "'";
                        }
                        else
                        {

                            SelectBillNo = "Select RIGHT(billno,5) as DisplayBillNo,DATENAME(month,convert(DATETIME,case when Len(Month)>3 then(LEFT(Month, 2) + '/' + Left(08, 1) + '/' + Right(Month, 2))else (LEFT(Month, 1) + '/' + Left(08, 1) + '/' + Right(Month, 2)) end )) +'-' + Left(20, 2) + RIGHT(Month, 2) as Monthname,* from mUnitbill where billno = '" + BillNo + "'";
                        }
                        DataTable DtBilling = config.ExecuteAdaptorAsyncWithQueryParams(SelectBillNo).Result;
                        string DisplayBillNo = "";
                        string area = "";
                        string ExtraRemarks = "";
                        string BilldateCheck = "";
                        if (dtclientaddress.Rows.Count > 0)
                        {
                            area = dtclientaddress.Rows[0]["segname"].ToString();
                            BilldateCheck = (DtBilling.Rows[0]["billdt"].ToString());
                        }

                        DateTime BillDate;
                        DateTime DueDate;
                        string MonthName = string.Empty;
                        DateTime Fromdate;
                        DateTime Todate;

                        #region Variables for data Fields as on 11/03/2014 by venkat


                        decimal servicecharge = 0;
                        decimal servicetax = 0;
                        decimal cess = 0;
                        decimal sbcess = 0;
                        decimal kkcess = 0;


                        #region for GST on 17-6-2017 by swathi

                        decimal CGST = 0;
                        decimal SGST = 0;
                        decimal IGST = 0;
                        decimal Cess1 = 0;
                        decimal Cess2 = 0;
                        decimal CGSTPrc = 0;
                        decimal SGSTPrc = 0;
                        decimal IGSTPrc = 0;
                        decimal Cess1Prc = 0;
                        decimal Cess2Prc = 0;

                        #endregion for GST on 17-6-2017 by swathi


                        decimal shecess = 0;
                        decimal totalamount = 0;
                        decimal Grandtotal = 0;

                        decimal ServiceTax75 = 0;
                        decimal ServiceTax25 = 0;

                        decimal machinarycost = 0;
                        decimal materialcost = 0;
                        decimal maintenancecost = 0;
                        decimal extraonecost = 0;
                        decimal extratwocost = 0;
                        decimal discountone = 0;
                        decimal discounttwo = 0;

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

                        decimal staxamtonservicecharge = 0;
                        decimal RelChrgAmt = 0;
                        decimal PFAmt = 0;
                        decimal ESIAmt = 0;
                        decimal BpfPer = 0;
                        decimal BesiPer = 0;


                        #endregion

                        //DateTime dtn = DateTime.ParseExact(BilldateCheck, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        string billdt = BilldateCheck;

                        string BQry = "select * from TblOptions  where '" + billdt + "' between fromdate and todate";
                        DataTable Bdt = config.ExecuteAdaptorAsyncWithQueryParams(BQry).Result;

                        string CGSTAlias = "";
                        string SGSTAlias = "";
                        string IGSTAlias = "";
                        string Cess1Alias = "";
                        string Cess2Alias = "";
                        string GSTINAlias = "";
                        string OurGSTINAlias = "";

                        string SqlQryForTaxes = @"select ServiceTaxSeparate,Cess,SHECess,SBCess,KKCess,CGST,SGST,IGST,cess1,cess2,CGSTAlias,SGSTAlias,IGSTAlias,cess1Alias,cess2Alias,GSTINAlias,OurGSTINAlias from TblOptions where '" + billdt + "' between fromdate and todate ";
                        DataTable DtTaxes = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForTaxes).Result;

                        string SCPersent = "";
                        if (DtTaxes.Rows.Count > 0)
                        {
                            SCPersent = DtTaxes.Rows[0]["ServiceTaxSeparate"].ToString();
                            CGSTAlias = DtTaxes.Rows[0]["CGSTAlias"].ToString();
                            SGSTAlias = DtTaxes.Rows[0]["SGSTAlias"].ToString();
                            IGSTAlias = DtTaxes.Rows[0]["IGSTAlias"].ToString();
                            Cess1Alias = DtTaxes.Rows[0]["Cess1Alias"].ToString();
                            Cess2Alias = DtTaxes.Rows[0]["Cess2Alias"].ToString();
                            GSTINAlias = DtTaxes.Rows[0]["GSTINAlias"].ToString();
                            OurGSTINAlias = DtTaxes.Rows[0]["OurGSTINAlias"].ToString();
                        }


                        if (DtBilling.Rows.Count > 0)
                        {

                            ExtraRemarks = DtBilling.Rows[0]["Remarks"].ToString();
                            BillNo = DtBilling.Rows[0]["billno"].ToString();
                            DisplayBillNo = DtBilling.Rows[0]["DisplayBillNo"].ToString();
                            BillDate = Convert.ToDateTime(DtBilling.Rows[0]["billdt"].ToString());
                            MonthName = DtBilling.Rows[0]["Monthname"].ToString();
                            Fromdate = Convert.ToDateTime(DtBilling.Rows[0]["Fromdt"].ToString());
                            Todate = Convert.ToDateTime(DtBilling.Rows[0]["Todt"].ToString());
                            OurGSTIN = DtBilling.Rows[0]["OURGSTNo"].ToString();
                            StateCode = DtBilling.Rows[0]["BillToStateCode"].ToString();
                            GSTIN = DtBilling.Rows[0]["BillToGSTNo"].ToString();
                            State = DtBilling.Rows[0]["BillToState"].ToString();
                            ShipToStateCode = DtBilling.Rows[0]["ShipToStateCode"].ToString();
                            ShipToGSTIN = DtBilling.Rows[0]["ShipToGSTNo"].ToString();
                            ShipToState = DtBilling.Rows[0]["ShipToState"].ToString();
                            if (BillType == "N")
                            {
                                DueDate = Convert.ToDateTime(DtBilling.Rows[0]["duedt"].ToString());
                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax75"].ToString()) == false)
                                {
                                    ServiceTax75 = decimal.Parse(DtBilling.Rows[0]["ServiceTax75"].ToString());
                                }

                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax25"].ToString()) == false)
                                {
                                    ServiceTax25 = decimal.Parse(DtBilling.Rows[0]["ServiceTax25"].ToString());
                                }

                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString()) == false)
                                {
                                    servicecharge = decimal.Parse(DtBilling.Rows[0]["TotalServiceChargeAmt"].ToString());
                                }

                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["RelChrgAmt"].ToString()) == false)
                                {
                                    RelChrgAmt = decimal.Parse(DtBilling.Rows[0]["RelChrgAmt"].ToString());
                                }

                                if (string.IsNullOrEmpty(DtBilling.Rows[0]["Bpfamt"].ToString()) == false)
                                {
                                    PFAmt = decimal.Parse(DtBilling.Rows[0]["Bpfamt"].ToString());
                                }


                                if (string.IsNullOrEmpty(DtBilling.Rows[0]["Besiamt"].ToString()) == false)
                                {
                                    ESIAmt = decimal.Parse(DtBilling.Rows[0]["Besiamt"].ToString());
                                }

                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["BpfPer"].ToString()) == false)
                                {
                                    BpfPer = decimal.Parse(DtBilling.Rows[0]["BpfPer"].ToString());
                                }

                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["BesiPer"].ToString()) == false)
                                {
                                    BesiPer = decimal.Parse(DtBilling.Rows[0]["BesiPer"].ToString());
                                }

                            }

                            else
                            {
                                if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceChrg"].ToString()) == false)
                                {
                                    servicecharge = decimal.Parse(DtBilling.Rows[0]["ServiceChrg"].ToString());
                                }
                            }



                            #region Begin New code for values taken from database as on 11/03/2014 by venkat

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["dutiestotalamount"].ToString()) == false)
                            {
                                totalamount = decimal.Parse(DtBilling.Rows[0]["dutiestotalamount"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ServiceTax"].ToString()) == false)
                            {
                                servicetax = decimal.Parse(DtBilling.Rows[0]["ServiceTax"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SBCessAmt"].ToString()) == false)
                            {
                                sbcess = decimal.Parse(DtBilling.Rows[0]["SBCessAmt"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["KKCessAmt"].ToString()) == false)
                            {
                                kkcess = decimal.Parse(DtBilling.Rows[0]["KKCessAmt"].ToString());
                            }

                            #region for GST as on 17-6-2017 by swathi

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTAmt"].ToString()) == false)
                            {
                                CGST = decimal.Parse(DtBilling.Rows[0]["CGSTAmt"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTAmt"].ToString()) == false)
                            {
                                SGST = decimal.Parse(DtBilling.Rows[0]["SGSTAmt"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTAmt"].ToString()) == false)
                            {
                                IGST = decimal.Parse(DtBilling.Rows[0]["IGSTAmt"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Amt"].ToString()) == false)
                            {
                                Cess1 = decimal.Parse(DtBilling.Rows[0]["Cess1Amt"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Amt"].ToString()) == false)
                            {
                                Cess2 = decimal.Parse(DtBilling.Rows[0]["Cess2Amt"].ToString());
                            }


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CGSTPrc"].ToString()) == false)
                            {
                                CGSTPrc = decimal.Parse(DtBilling.Rows[0]["CGSTPrc"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SGSTPrc"].ToString()) == false)
                            {
                                SGSTPrc = decimal.Parse(DtBilling.Rows[0]["SGSTPrc"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["IGSTPrc"].ToString()) == false)
                            {
                                IGSTPrc = decimal.Parse(DtBilling.Rows[0]["IGSTPrc"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess1Prc"].ToString()) == false)
                            {
                                Cess1Prc = decimal.Parse(DtBilling.Rows[0]["Cess1Prc"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Cess2Prc"].ToString()) == false)
                            {
                                Cess2Prc = decimal.Parse(DtBilling.Rows[0]["Cess2Prc"].ToString());
                            }

                            #endregion for GST as on 17-6-2017 by swathi

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["CESS"].ToString()) == false)
                            {
                                cess = decimal.Parse(DtBilling.Rows[0]["CESS"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SHECess"].ToString()) == false)
                            {
                                shecess = decimal.Parse(DtBilling.Rows[0]["SHECess"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["GrandTotal"].ToString()) == false)
                            {
                                Grandtotal = decimal.Parse(DtBilling.Rows[0]["GrandTotal"].ToString());
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["MachinaryCost"].ToString()) == false)
                            {
                                machinarycost = decimal.Parse(DtBilling.Rows[0]["MachinaryCost"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["MaterialCost"].ToString()) == false)
                            {
                                materialcost = decimal.Parse(DtBilling.Rows[0]["MaterialCost"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ElectricalChrg"].ToString()) == false)
                            {
                                maintenancecost = decimal.Parse(DtBilling.Rows[0]["ElectricalChrg"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtone"].ToString()) == false)
                            {
                                extraonecost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtone"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraAmtTwo"].ToString()) == false)
                            {
                                extratwocost = decimal.Parse(DtBilling.Rows[0]["ExtraAmtTwo"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discount"].ToString()) == false)
                            {
                                discountone = decimal.Parse(DtBilling.Rows[0]["Discount"].ToString());
                            }
                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Discounttwo"].ToString()) == false)
                            {
                                discounttwo = decimal.Parse(DtBilling.Rows[0]["Discounttwo"].ToString());
                            }

                            machinarycosttitle = DtBilling.Rows[0]["Machinarycosttitle"].ToString();
                            materialcosttitle = DtBilling.Rows[0]["Materialcosttitle"].ToString();
                            maintenancecosttitle = DtBilling.Rows[0]["Maintanancecosttitle"].ToString();
                            extraonecosttitle = DtBilling.Rows[0]["Extraonetitle"].ToString();
                            extratwocosttitle = DtBilling.Rows[0]["Extratwotitle"].ToString();
                            discountonetitle = DtBilling.Rows[0]["Discountonetitle"].ToString();
                            discounttwotitle = DtBilling.Rows[0]["Discounttwotitle"].ToString();

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Extradatacheck"].ToString()) == false)
                            {
                                strExtradatacheck = DtBilling.Rows[0]["Extradatacheck"].ToString();
                                if (strExtradatacheck == "True")
                                {
                                    Extradatacheck = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["ExtraDataSTcheck"].ToString()) == false)
                            {
                                strExtrastcheck = DtBilling.Rows[0]["ExtraDataSTcheck"].ToString();
                                if (strExtrastcheck == "True")
                                {
                                    ExtraDataSTcheck = true;
                                }
                            }



                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMachinary"].ToString()) == false)
                            {
                                strSTMachinary = DtBilling.Rows[0]["STMachinary"].ToString();
                                if (strSTMachinary == "True")
                                {
                                    STMachinary = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaterial"].ToString()) == false)
                            {
                                strSTMaterial = DtBilling.Rows[0]["STMaterial"].ToString();
                                if (strSTMaterial == "True")
                                {
                                    STMaterial = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STMaintenance"].ToString()) == false)
                            {
                                strSTMaintenance = DtBilling.Rows[0]["STMaintenance"].ToString();
                                if (strSTMaintenance == "True")
                                {
                                    STMaintenance = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtraone"].ToString()) == false)
                            {
                                strSTExtraone = DtBilling.Rows[0]["STExtraone"].ToString();
                                if (strSTExtraone == "True")
                                {
                                    STExtraone = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STExtratwo"].ToString()) == false)
                            {
                                strSTExtratwo = DtBilling.Rows[0]["STExtratwo"].ToString();
                                if (strSTExtratwo == "True")
                                {
                                    STExtratwo = true;
                                }
                            }


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMachinary"].ToString()) == false)
                            {
                                strSCMachinary = DtBilling.Rows[0]["SCMachinary"].ToString();
                                if (strSCMachinary == "True")
                                {
                                    SCMachinary = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaterial"].ToString()) == false)
                            {
                                strSCMaterial = DtBilling.Rows[0]["SCMaterial"].ToString();
                                if (strSCMaterial == "True")
                                {
                                    SCMaterial = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCMaintenance"].ToString()) == false)
                            {
                                strSCMaintenance = DtBilling.Rows[0]["SCMaintenance"].ToString();
                                if (strSCMaintenance == "True")
                                {
                                    SCMaintenance = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtraone"].ToString()) == false)
                            {
                                strSCExtraone = DtBilling.Rows[0]["SCExtraone"].ToString();
                                if (strSCExtraone == "True")
                                {
                                    SCExtraone = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["SCExtratwo"].ToString()) == false)
                            {
                                strSCExtratwo = DtBilling.Rows[0]["SCExtratwo"].ToString();
                                if (strSCExtratwo == "True")
                                {
                                    SCExtratwo = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                            {
                                strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                                if (strSTDiscountone == "True")
                                {
                                    STDiscountone = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                            {
                                strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                                if (strSTDiscounttwo == "True")
                                {
                                    STDiscounttwo = true;
                                }
                            }


                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscountone"].ToString()) == false)
                            {
                                strSTDiscountone = DtBilling.Rows[0]["STDiscountone"].ToString();
                                if (strSTDiscountone == "True")
                                {
                                    STDiscountone = true;
                                }
                            }

                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["STDiscounttwo"].ToString()) == false)
                            {
                                strSTDiscounttwo = DtBilling.Rows[0]["STDiscounttwo"].ToString();
                                if (strSTDiscounttwo == "True")
                                {
                                    STDiscounttwo = true;
                                }
                            }




                            if (String.IsNullOrEmpty(DtBilling.Rows[0]["Staxonservicecharge"].ToString()) == false)
                            {
                                staxamtonservicecharge = decimal.Parse(DtBilling.Rows[0]["Staxonservicecharge"].ToString());
                            }

                            #endregion
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Generate The Bill Once Again');", true);
                            return;
                        }

                        #endregion

                        //document.AddTitle(companyName);
                        //document.AddAuthor("DIYOS");
                        //document.AddSubject("Invoice");
                        //document.AddKeywords("Keyword1, keyword2, …");


                        string imagepath = Server.MapPath("~/assets/" + CmpIDPrefix + "BillLogo.png");
                        if (File.Exists(imagepath))
                        {
                            iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);
                            gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                            gif2.ScalePercent(55f);
                            gif2.SetAbsolutePosition(5f, 753f);
                            document.Add(gif2);
                        }


                        PdfPTable tablelogo = new PdfPTable(2);
                        tablelogo.TotalWidth = 580f;
                        tablelogo.LockedWidth = true;
                        float[] widtlogo = new float[] { 0.4f, 2f };
                        tablelogo.SetWidths(widtlogo);


                        var FontColour = new BaseColor(178, 34, 34);
                        Font FontStyle1 = FontFactory.GetFont("Belwe-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 30, Font.NORMAL, FontColour);

                        PdfPCell CCompName1 = new PdfPCell(new Paragraph("" + companyName, FontFactory.GetFont(FontStyle, 15, Font.BOLD, BaseColor.BLACK)));
                        CCompName1.HorizontalAlignment = 1;
                        CCompName1.Colspan = 2;
                        // CCompName1.PaddingTop = 25f;
                        CCompName1.Border = 0;
                        CCompName1.PaddingLeft = 80;
                        tablelogo.AddCell(CCompName1);

                        PdfPCell CCompName = new PdfPCell(new Paragraph("" + companyAddress, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                        CCompName.HorizontalAlignment = 1;
                        CCompName.Colspan = 2;
                        CCompName.Border = 0;
                        CCompName.PaddingLeft = 80;
                        CCompName.SetLeading(0, 1.2f);
                        tablelogo.AddCell(CCompName);


                        if (emailid.Length > 0)
                        {
                            PdfPCell CCompName2 = new PdfPCell(new Paragraph("Website :" + website + " | Email :" + emailid, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            CCompName2.HorizontalAlignment = 1;
                            CCompName2.Colspan = 2;
                            CCompName2.Border = 0;
                            CCompName2.PaddingLeft = 40;
                            tablelogo.AddCell(CCompName2);
                        }
                        if (phoneno.Length > 0)
                        {
                            PdfPCell CCompName2 = new PdfPCell(new Paragraph("Phone :" + phoneno, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                            CCompName2.HorizontalAlignment = 1;
                            CCompName2.Colspan = 2;
                            CCompName2.Border = 0;
                            CCompName2.PaddingBottom = 5;
                            tablelogo.AddCell(CCompName2);
                        }

                        document.Add(tablelogo);

                        #region  for Client Details

                        PdfPTable address = new PdfPTable(5);
                        address.TotalWidth = 580f;
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
                        tempTable1.TotalWidth = 348f;
                        tempTable1.LockedWidth = true;
                        float[] tempWidth1 = new float[] { 0.8f, 2f, 2f };
                        tempTable1.SetWidths(tempWidth1);

                        string addressData = "";

                        addressData = dtclientaddress.Rows[0]["ClientAddrHno"].ToString();

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
                        addressData = dtclientaddress.Rows[0]["ClientAddrStreet"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientstreet.BorderWidthBottom = 0;
                            clientstreet.BorderWidthTop = 0;
                            clientstreet.Colspan = 3;
                            clientstreet.BorderWidthLeft = .2f;
                            clientstreet.BorderWidthRight = 0.2f;
                            clientstreet.BorderColor = BaseColor.BLACK;
                            //clientstreet.PaddingLeft = 20;
                            tempTable1.AddCell(clientstreet);
                        }


                        addressData = dtclientaddress.Rows[0]["ClientAddrArea"].ToString();
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clientstreet = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            clientstreet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clientstreet.BorderWidthBottom = 0;
                            clientstreet.BorderWidthTop = 0;
                            clientstreet.Colspan = 3;
                            clientstreet.BorderColor = BaseColor.BLACK;
                            clientstreet.BorderWidthLeft = .2f;
                            clientstreet.BorderWidthRight = 0.2f;
                            // clientstreet.PaddingLeft = 20;
                            tempTable1.AddCell(clientstreet);
                        }


                        var ClientAddrColony = dtclientaddress.Rows[0]["ClientAddrColony"].ToString();
                        var ClientAddrcity = dtclientaddress.Rows[0]["ClientAddrcity"].ToString();
                        var ClientAddrstate = dtclientaddress.Rows[0]["ClientAddrstate"].ToString();
                        var ClientAddrpin = dtclientaddress.Rows[0]["ClientAddrpin"].ToString();
                        addressData = (ClientAddrColony + "," + ClientAddrcity + "," + ClientAddrstate + "," + ClientAddrpin);
                        if (addressData.Trim().Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph(addressData, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            clietnpin.Colspan = 3;
                            clietnpin.BorderWidthBottom = 0;
                            clietnpin.BorderWidthTop = 0;
                            clietnpin.BorderWidthLeft = .2f;
                            clietnpin.BorderWidthRight = 0.2f;
                            clietnpin.BorderColor = BaseColor.BLACK;
                            //  clietnpin.PaddingLeft = 20;
                            tempTable1.AddCell(clietnpin);
                        }

                        if (Bdt.Rows.Count > 0)
                        {
                            if (StateCode == "1" || StateCode == "2" || StateCode == "3" || StateCode == "4" || StateCode == "5" || StateCode == "6" || StateCode == "7" || StateCode == "8" || StateCode == "7")
                            {
                                StateCode = "0" + StateCode;
                            }
                            if (State.Length > 0)
                            {
                                PdfPCell clietnpin = new PdfPCell(new Paragraph("State", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 1;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = .2f;
                                clietnpin.BorderWidthRight = 0;
                                clietnpin.BorderColor = BaseColor.BLACK;
                                //  clietnpin.PaddingLeft = 20;
                                tempTable1.AddCell(clietnpin);

                                clietnpin = new PdfPCell(new Paragraph(" : " + State, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 1;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = 0;
                                clietnpin.BorderColor = BaseColor.BLACK;
                                //  clietnpin.PaddingLeft = 20;
                                tempTable1.AddCell(clietnpin);
                            }
                            if (StateCode.Length > 0)
                            {
                                PdfPCell clietnpin = new PdfPCell(new Paragraph("State Code : " + StateCode, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                clietnpin.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                clietnpin.Colspan = 1;
                                clietnpin.BorderWidthBottom = 0;
                                clietnpin.BorderWidthTop = 0;
                                clietnpin.BorderWidthLeft = 0;
                                clietnpin.BorderWidthRight = 0.2f;
                                clietnpin.BorderColor = BaseColor.BLACK;
                                //  clietnpin.PaddingLeft = 20;
                                tempTable1.AddCell(clietnpin);
                            }


                        }

                        if (GSTIN.Length > 0)
                        {
                            PdfPCell clietnpin = new PdfPCell(new Paragraph("GSTIN ", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                            clietnpin = new PdfPCell(new Paragraph(" : " + GSTIN, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                        PdfPCell cellemp1 = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        cellemp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cellemp1.Colspan = 3;
                        cellemp1.BorderWidthTop = 0;
                        cellemp1.BorderWidthBottom = 0;
                        cellemp1.BorderWidthLeft = .2f;
                        cellemp1.BorderWidthRight = 0.2f;
                        cellemp1.BorderColor = BaseColor.BLACK;
                        cellemp1.PaddingBottom = 15;
                        //tempTable1.AddCell(cellemp1);


                        PdfPCell childTable1 = new PdfPCell(tempTable1);
                        childTable1.Border = 0;
                        childTable1.Colspan = 3;
                        // childTable1.FixedHeight = 100;
                        childTable1.HorizontalAlignment = 0;

                        address.AddCell(childTable1);

                        PdfPTable tempTable2 = new PdfPTable(2);
                        tempTable2.TotalWidth = 232f;
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
                        phrase10.Add(new Chunk(": " + BillNo, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                        phrase11v.Add(new Chunk(": " + BillDate.Day.ToString("00") + "/" + BillDate.ToString("MM") + "/" +
                            BillDate.Year, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                        phrase10p.Add(new Chunk(": " + Fromdate.Day.ToString("00") + "/" + Fromdate.ToString("MM") + "/" +
                            Fromdate.Year + " to " + Todate.Day.ToString("00") + "/" + Todate.ToString("MM") + "/" +
                            Todate.Year, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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

                        if (POContent.Length > 0)
                        {
                            var phrasew = new Phrase();
                            phrasew.Add(new Chunk("PO No", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell13 = new PdfPCell();
                            cell13.AddElement(phrasew);
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

                            var phrase10w = new Phrase();
                            phrase10w.Add(new Chunk(": " + POContent, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell13v = new PdfPCell();
                            cell13v.AddElement(phrase10w);
                            cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell13v.BorderWidthBottom = 0;
                            cell13v.BorderWidthTop = 0;
                            cell13v.Colspan = 1;
                            cell13v.BorderWidthLeft = 0;
                            cell13v.BorderWidthRight = .2f;
                            cell13v.PaddingTop = -5;
                            cell13v.BorderColor = BaseColor.BLACK;
                            tempTable2.AddCell(cell13v);

                        }

                        if (PODate.Length > 0)
                        {
                            var phrasew = new Phrase();
                            phrasew.Add(new Chunk("Work Order Date", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell13 = new PdfPCell();
                            cell13.AddElement(phrasew);
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

                            var phrase10w = new Phrase();
                            phrase10w.Add(new Chunk(": " + PODate, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell13v = new PdfPCell();
                            cell13v.AddElement(phrase10w);
                            cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell13v.BorderWidthBottom = 0;
                            cell13v.BorderWidthTop = 0;
                            cell13v.Colspan = 1;
                            cell13v.BorderWidthLeft = 0;
                            cell13v.BorderWidthRight = .2f;
                            cell13v.PaddingTop = -5;
                            cell13v.BorderColor = BaseColor.BLACK;
                            tempTable2.AddCell(cell13v);

                        }

                        if (Location.Length > 0)
                        {
                            var phrasew = new Phrase();
                            phrasew.Add(new Chunk("Place of Supply", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell13 = new PdfPCell();
                            cell13.AddElement(phrasew);
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

                            var phrase10w = new Phrase();
                            phrase10w.Add(new Chunk(": " + Location, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell13v = new PdfPCell();
                            cell13v.AddElement(phrase10w);
                            cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell13v.BorderWidthBottom = 0;
                            cell13v.BorderWidthTop = 0;
                            cell13v.Colspan = 1;
                            cell13v.BorderWidthLeft = 0;
                            cell13v.BorderWidthRight = .2f;
                            cell13v.PaddingTop = -5;
                            cell13v.BorderColor = BaseColor.BLACK;
                            tempTable2.AddCell(cell13v);

                        }

                        if (ReversCharges.Length > 0)
                        {
                            var phrasew = new Phrase();
                            phrasew.Add(new Chunk("Revers Charges", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell13 = new PdfPCell();
                            cell13.AddElement(phrasew);
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
                            if (ReversCharges == "True")
                            {
                                ReversCharges = "Yes";
                            }
                            else
                            {
                                ReversCharges = "No";
                            }

                            var phrase10w = new Phrase();
                            phrase10w.Add(new Chunk(": " + ReversCharges, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell13v = new PdfPCell();
                            cell13v.AddElement(phrase10w);
                            cell13v.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cell13v.BorderWidthBottom = 0;
                            cell13v.BorderWidthTop = 0;
                            cell13v.Colspan = 1;
                            cell13v.BorderWidthLeft = 0;
                            cell13v.BorderWidthRight = .2f;
                            cell13v.PaddingTop = -5;
                            cell13v.BorderColor = BaseColor.BLACK;
                            tempTable2.AddCell(cell13v);

                        }


                        PdfPCell childTable2 = new PdfPCell(tempTable2);
                        childTable2.Border = 0;
                        childTable2.Colspan = 2;
                        childTable2.HorizontalAlignment = 0;
                        address.AddCell(childTable2);

                        document.Add(address);





                        #endregion

                        #region for breakupdata
                        int countGrid = 5;

                        DataTable dtheadings = null;
                        var SPNameD = "GetInvHeadings";
                        Hashtable htheadings = new Hashtable();
                        htheadings.Add("@clientid", ClientId);
                        // htheadings.Add("@LastDay", DtLastDay);
                        dtheadings = config.ExecuteAdaptorAsyncWithParams(SPNameD, htheadings).Result;

                        string InvDescription = "";
                        string InvNoOfEmps = "";
                        string InvNoofDuties = "";
                        string InvPayrate = "";
                        string InvAmount = "";
                        string InvSACCode = "";
                        string InvMonthDays = "";
                        string InvDescriptionVisible = "N";
                        string InvNoOfEmpsVisible = "N";
                        string InvNoofDutiesVisible = "N";
                        string InvPayrateVisible = "N";
                        string InvAmountVisible = "N";
                        string InvSACCodeVisible = "N";
                        string InvMonthDaysVisible = "N";
                        string HSNNo = "";
                        var ExDBRemarks = "";
                        if (dtheadings.Rows.Count > 0)
                        {
                            InvDescription = dtheadings.Rows[0]["InvDescription"].ToString();
                            InvNoOfEmps = dtheadings.Rows[0]["InvNoOfEmps"].ToString();
                            InvNoofDuties = dtheadings.Rows[0]["InvNoofDuties"].ToString();
                            InvPayrate = dtheadings.Rows[0]["InvPayrate"].ToString();
                            InvAmount = dtheadings.Rows[0]["InvAmount"].ToString();
                            InvMonthDays = dtheadings.Rows[0]["InvMonthDays"].ToString();
                            InvSACCode = dtheadings.Rows[0]["InvSACCode"].ToString();
                            InvDescriptionVisible = dtheadings.Rows[0]["InvDescriptionVisible"].ToString();
                            InvNoOfEmpsVisible = dtheadings.Rows[0]["InvNoOfEmpsVisible"].ToString();
                            InvNoofDutiesVisible = dtheadings.Rows[0]["InvNoofDutiesVisible"].ToString();
                            InvPayrateVisible = dtheadings.Rows[0]["InvPayrateVisible"].ToString();
                            InvAmountVisible = dtheadings.Rows[0]["InvAmountVisible"].ToString();
                            InvSACCodeVisible = dtheadings.Rows[0]["InvSACCodeVisible"].ToString();
                            InvMonthDaysVisible = dtheadings.Rows[0]["InvMonthDaysVisible"].ToString();
                        }

                        int colCount = 1;

                        if (InvDescriptionVisible == "Y")
                        {
                            colCount += 1;
                        }

                        if (InvNoOfEmpsVisible == "Y")
                        {
                            colCount += 1;
                        }

                        if (InvNoofDutiesVisible == "Y")
                        {
                            colCount += 1;
                        }

                        if (InvPayrateVisible == "Y")
                        {
                            colCount += 1;
                        }

                        if (InvAmountVisible == "Y")
                        {
                            colCount += 1;
                        }



                        if (InvSACCodeVisible == "Y")
                        {
                            colCount += 1;
                        }

                        if (InvMonthDaysVisible == "Y")
                        {
                            colCount += 1;
                        }


                        PdfPTable table = new PdfPTable(colCount);
                        table.TotalWidth = 580f;
                        table.LockedWidth = true;
                        table.HorizontalAlignment = 1;
                        float[] colWidths = new float[] { };
                        if (colCount == 8)
                        {
                            colWidths = new float[] { 1f, 6f, 2f, 2f, 2f, 2.2f, 2f, 2.7f };
                        }
                        if (colCount == 7)
                        {
                            colWidths = new float[] { 1f, 6f, 2f, 2f, 2.2f, 2f, 2.7f };
                        }
                        if (colCount == 6)
                        {
                            colWidths = new float[] { 1f, 6f, 2f, 2.2f, 2f, 2.7f };
                        }

                        if (colCount == 5)
                        {
                            colWidths = new float[] { 1f, 6f, 2f, 2.2f, 2.7f };
                        }

                        if (colCount == 4)
                        {
                            colWidths = new float[] { 1f, 6f, 2.2f, 2.7f };
                        }

                        if (colCount == 3)
                        {
                            colWidths = new float[] { 1f, 6f, 2.7f };
                        }


                        table.SetWidths(colWidths);

                        string cellText;


                        PdfPCell cell = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0.2f;
                        cell.BorderWidthLeft = .2f;
                        cell.BorderWidthTop = 0.2f;
                        cell.BorderWidthRight = 0f;
                        cell.Colspan = 1;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        if (InvDescriptionVisible == "Y")
                        {
                            cell = new PdfPCell(new Phrase(InvDescription, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.2f;
                            cell.BorderWidthLeft = 0.2f;
                            cell.BorderWidthTop = 0.2f;
                            cell.BorderWidthRight = 0f;
                            //cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            table.AddCell(cell);
                        }
                        if (InvSACCodeVisible == "Y")
                        {
                            cell = new PdfPCell(new Phrase(InvSACCode, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.2f;
                            cell.BorderWidthLeft = 0.2f;
                            cell.BorderWidthTop = 0.2f;
                            cell.BorderWidthRight = 0f;
                            //cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            table.AddCell(cell);
                        }
                        if (InvMonthDaysVisible == "Y")
                        {
                            cell = new PdfPCell(new Phrase(InvMonthDays, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.2f;
                            cell.BorderWidthLeft = 0.2f;
                            cell.BorderWidthTop = 0.2f;
                            cell.BorderWidthRight = 0f;
                            //cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            table.AddCell(cell);
                        }
                        if (InvNoOfEmpsVisible == "Y")
                        {
                            cell = new PdfPCell(new Phrase(InvNoOfEmps, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.2f;
                            cell.BorderWidthLeft = 0.2f;
                            cell.BorderWidthTop = 0.2f;
                            cell.BorderWidthRight = 0f;
                            //cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            table.AddCell(cell);
                        }

                        if (InvPayrateVisible == "Y")
                        {
                            cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.2f;
                            cell.BorderWidthLeft = 0.2f;
                            cell.BorderWidthTop = 0.2f;
                            cell.BorderWidthRight = 0f;
                            cell.BorderColor = BaseColor.BLACK;
                            table.AddCell(cell);
                        }

                        if (InvNoofDutiesVisible == "Y")
                        {
                            cell = new PdfPCell(new Phrase(InvNoofDuties, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.2f;
                            cell.BorderWidthLeft = 0.2f;
                            cell.BorderWidthTop = 0.2f;
                            cell.BorderWidthRight = 0f;
                            cell.BorderColor = BaseColor.BLACK;
                            table.AddCell(cell);
                        }

                        if (InvAmountVisible == "Y")
                        {
                            cell = new PdfPCell(new Phrase(InvAmount, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            cell.HorizontalAlignment = 1;
                            cell.BorderWidthBottom = 0.2f;
                            cell.BorderWidthLeft = 0.2f;
                            cell.BorderWidthTop = 0.2f;
                            cell.BorderWidthRight = .2f;
                            cell.BorderColor = BaseColor.BLACK;
                            table.AddCell(cell);
                        }
                        int Status = 0;
                        if (BillType == "N")
                        {
                            Status = 0;
                        }
                        else
                            Status = 1;
                        string spUnitbillbreakup = "GetpdfforClientbillingfromunitbillbreakup";
                        Hashtable htunitbillbreakup = new Hashtable();
                        htunitbillbreakup.Add("@Clientid", ClientId);
                        htunitbillbreakup.Add("@month", Month);
                        htunitbillbreakup.Add("@status", Status);
                        htunitbillbreakup.Add("@munitibillno", BillNo);

                        DataTable dtunitbillbreakup = SqlHelper.Instance.ExecuteStoredProcedureWithParams(spUnitbillbreakup, htunitbillbreakup);

                        for (int rowIndex = 0; rowIndex < dtunitbillbreakup.Rows.Count; rowIndex++)
                        {

                            int SNo = 0;
                            string Designation = "";
                            string SACCodes = "";
                            decimal MonthDays = 0;
                            decimal NoOfEmps = 0;
                            decimal payrate = 0;
                            decimal Duties = 0;
                            decimal Dutiesamount = 0;

                            SNo = int.Parse(dtunitbillbreakup.Rows[rowIndex]["SNo"].ToString());
                            Designation = dtunitbillbreakup.Rows[rowIndex]["Designation"].ToString();
                            SACCodes = dtunitbillbreakup.Rows[rowIndex]["SACCode"].ToString();
                            MonthDays = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["MonthDays"].ToString());
                            NoOfEmps = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["NoOfEmps"].ToString());
                            payrate = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["payrate"].ToString());
                            Duties = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["Duties"].ToString());
                            Dutiesamount = decimal.Parse(dtunitbillbreakup.Rows[rowIndex]["Dutiesamount"].ToString());

                            cell = new PdfPCell(new Phrase(SNo.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            cell.Colspan = 1;
                            cell.BorderWidthRight = 0f;
                            cell.BorderWidthLeft = .2f;
                            cell.BorderWidthTop = 0;
                            cell.MinimumHeight = 20;
                            cell.HorizontalAlignment = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            table.AddCell(cell);

                            if (InvDescriptionVisible == "Y")
                            {
                                cell = new PdfPCell(new Phrase(Designation, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0.2f;
                                cell.BorderWidthRight = 0f;
                                //cell.Colspan = 1;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);
                            }
                            if (InvSACCodeVisible == "Y")
                            {
                                cell = new PdfPCell(new Phrase(SACCodes, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0.2f;
                                cell.BorderWidthRight = 0f;
                                //cell.Colspan = 1;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);
                            }
                            if (InvMonthDaysVisible == "Y")
                            {
                                cell = new PdfPCell(new Phrase(MonthDays.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0.2f;
                                cell.BorderWidthRight = 0f;
                                //cell.Colspan = 1;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);
                            }
                            if (InvNoOfEmpsVisible == "Y")
                            {
                                cell = new PdfPCell(new Phrase(NoOfEmps.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0.2f;
                                cell.BorderWidthRight = 0f;
                                //cell.Colspan = 1;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);
                            }

                            if (InvPayrateVisible == "Y")
                            {
                                cell = new PdfPCell(new Phrase(payrate.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0.2f;
                                cell.BorderWidthRight = 0f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);
                            }

                            if (InvNoofDutiesVisible == "Y")
                            {
                                cell = new PdfPCell(new Phrase(Duties.ToString(), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0.2f;
                                cell.BorderWidthRight = 0f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);
                            }

                            if (InvAmountVisible == "Y")
                            {
                                cell = new PdfPCell(new Phrase(Dutiesamount.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 2;
                                cell.BorderWidthBottom = 0.2f;
                                cell.BorderWidthLeft = 0.2f;
                                cell.BorderWidthTop = 0.2f;
                                cell.BorderWidthRight = .2f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);
                            }

                        }


                        #region for space
                        PdfPCell Cellempty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        Cellempty.HorizontalAlignment = 2;
                        Cellempty.Colspan = 1;
                        Cellempty.BorderWidthTop = 0;
                        Cellempty.BorderWidthRight = 0f;
                        Cellempty.BorderWidthLeft = .2f;
                        Cellempty.BorderWidthBottom = 0;
                        // Cellempty.MinimumHeight = 5;
                        Cellempty.BorderColor = BaseColor.BLACK;


                        PdfPCell Cellempty1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        Cellempty1.HorizontalAlignment = 2;
                        Cellempty1.Colspan = 1;
                        Cellempty1.BorderWidthTop = 0;
                        Cellempty1.BorderWidthRight = 0f;
                        Cellempty1.BorderWidthLeft = 0.2f;
                        Cellempty1.BorderWidthBottom = 0;
                        Cellempty1.BorderColor = BaseColor.BLACK;


                        PdfPCell Cellempty6 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        Cellempty6.HorizontalAlignment = 2;
                        Cellempty6.Colspan = 1;
                        Cellempty6.BorderWidthTop = 0;
                        Cellempty6.BorderWidthRight = 0f;
                        Cellempty6.BorderWidthLeft = .2f;
                        Cellempty6.BorderWidthBottom = 0;

                        Cellempty6.BorderColor = BaseColor.BLACK;

                        PdfPCell Cellempty7 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        Cellempty7.HorizontalAlignment = 2;
                        Cellempty7.Colspan = 1;
                        Cellempty7.BorderWidthTop = 0;
                        Cellempty7.BorderWidthRight = 0.2f;
                        Cellempty7.BorderWidthLeft = 0.2f;
                        Cellempty7.BorderWidthBottom = 0;
                        Cellempty7.BorderColor = BaseColor.BLACK;

                        PdfPCell Cellempty2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        Cellempty2.HorizontalAlignment = 2;
                        Cellempty2.Colspan = 1;
                        Cellempty2.BorderWidthTop = 0;
                        Cellempty2.BorderWidthRight = 0f;
                        Cellempty2.BorderWidthLeft = 0.2f;
                        Cellempty2.BorderWidthBottom = 0;
                        Cellempty2.BorderColor = BaseColor.BLACK;

                        PdfPCell Cellempty3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        Cellempty3.HorizontalAlignment = 2;
                        Cellempty3.Colspan = 1;
                        Cellempty3.BorderWidthTop = 0;
                        Cellempty3.BorderWidthRight = 0f;
                        Cellempty3.BorderWidthLeft = 0.2f;
                        Cellempty3.BorderWidthBottom = 0;
                        Cellempty3.BorderColor = BaseColor.BLACK;

                        PdfPCell Cellempty4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        Cellempty4.HorizontalAlignment = 2;
                        Cellempty4.Colspan = 1;
                        Cellempty4.BorderWidthTop = 0;
                        Cellempty4.BorderWidthRight = 0f;
                        Cellempty4.BorderWidthLeft = 0.2f;
                        Cellempty4.BorderWidthBottom = 0;
                        Cellempty4.BorderColor = BaseColor.BLACK;

                        PdfPCell Cellempty5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        Cellempty5.HorizontalAlignment = 2;
                        Cellempty5.Colspan = 1;
                        Cellempty5.BorderWidthTop = 0;
                        Cellempty5.BorderWidthRight = 0.2f;
                        Cellempty5.BorderWidthLeft = 0.2f;
                        Cellempty5.BorderWidthBottom = 0;
                        Cellempty5.BorderColor = BaseColor.BLACK;

                        int i;

                        if (dtunitbillbreakup.Rows.Count == 1)
                        {
                            #region For cell count

                            if (!bIncludeST)
                            {
                                for (i = 0; i < 13; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                    //table.AddCell(Cellempty4);
                                    //table.AddCell(Cellempty5);
                                }
                            }
                            else
                            {
                                for (i = 0; i < 10; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }



                            #endregion
                        }
                        if (dtunitbillbreakup.Rows.Count == 2)
                        {
                            #region For cell count

                            if (!bIncludeST)
                            {
                                for (i = 0; i < 12; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }
                            else
                            {

                                for (i = 0; i < 10; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }



                            #endregion
                        }
                        if (dtunitbillbreakup.Rows.Count == 3)
                        {
                            #region For cell count

                            if (!bIncludeST)
                            {
                                for (i = 0; i < 11; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }
                            else
                            {

                                for (i = 0; i < 9; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }

                            #endregion
                        }
                        if (dtunitbillbreakup.Rows.Count == 4)
                        {
                            #region For cell count

                            if (!bIncludeST)
                            {
                                for (i = 0; i < 10; i++)
                                {
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }
                            else
                            {
                                for (i = 0; i < 8; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }


                            #endregion
                        }
                        if (dtunitbillbreakup.Rows.Count == 5)
                        {
                            #region For cell count

                            if (!bIncludeST)
                            {
                                for (i = 0; i < 9; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }
                            else
                            {
                                for (i = 0; i < 7; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }


                            #endregion
                        }
                        if (dtunitbillbreakup.Rows.Count == 6)
                        {
                            #region For cell count
                            if (!bIncludeST)
                            {
                                for (i = 0; i < 8; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }
                            else
                            {
                                for (i = 0; i < 6; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }


                            #endregion
                        }
                        if (dtunitbillbreakup.Rows.Count == 7)
                        {
                            #region For cell count
                            if (!bIncludeST)
                            {
                                for (i = 0; i < 7; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }
                            else
                            {
                                for (i = 0; i < 5; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }


                            #endregion
                        }

                        if (dtunitbillbreakup.Rows.Count == 8)
                        {
                            #region For cell count
                            if (!bIncludeST)
                            {
                                for (i = 0; i < 6; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }
                            else
                            {
                                for (i = 0; i < 4; i++)
                                {
                                    //1
                                    table.AddCell(Cellempty);
                                    if (InvDescriptionVisible == "Y")
                                    {
                                        table.AddCell(Cellempty1);
                                    }
                                    if (InvSACCodeVisible == "Y")
                                    {
                                        table.AddCell(Cellempty6);
                                    }
                                    if (InvMonthDaysVisible == "Y")
                                    {
                                        table.AddCell(Cellempty7);
                                    }
                                    if (InvNoOfEmpsVisible == "Y")
                                    {
                                        table.AddCell(Cellempty2);
                                    }
                                    if (InvNoofDutiesVisible == "Y")
                                    {
                                        table.AddCell(Cellempty3);
                                    }
                                    if (InvPayrateVisible == "Y")
                                    {
                                        table.AddCell(Cellempty4);
                                    }
                                    if (InvAmountVisible == "Y")
                                    {
                                        table.AddCell(Cellempty5);
                                    }

                                }
                            }


                            #endregion
                        }




                        #endregion

                        document.Add(table);

                        #endregion

                        #region for Total Values

                        PdfPTable tempTable22 = new PdfPTable(colCount);
                        tempTable22.TotalWidth = 580f;
                        tempTable22.LockedWidth = true;
                        float[] tempWidth22 = new float[] { };
                        if (colCount == 8)
                        {
                            tempWidth22 = new float[] { 1f, 6f, 2f, 2f, 2f, 2.2f, 2f, 2.7f };
                        }

                        if (colCount == 7)
                        {
                            tempWidth22 = new float[] { 1f, 6f, 2f, 2f, 2.2f, 2f, 2.7f };
                        }

                        if (colCount == 6)
                        {
                            tempWidth22 = new float[] { 1f, 6f, 2f, 2.2f, 2f, 2.7f };
                        }

                        if (colCount == 5)
                        {
                            tempWidth22 = new float[] { 1f, 6f, 2f, 2.2f, 2.7f };
                        }

                        if (colCount == 4)
                        {
                            tempWidth22 = new float[] { 1f, 6f, 2.2f, 2.7f };
                        }

                        if (colCount == 3)
                        {
                            tempWidth22 = new float[] { 1f, 6f, 2.7f };
                        }
                        tempTable22.SetWidths(tempWidth22);

                        #region


                        if (RelChrgAmt > 0)
                        {

                            PdfPCell celldz5 = new PdfPCell(new Phrase("1/6 Reliever Charges", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            celldz5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            celldz5.Colspan = colCount - 1;
                            celldz5.BorderWidthBottom = 0;
                            celldz5.BorderWidthLeft = .2f;
                            celldz5.BorderWidthTop = 0;
                            celldz5.BorderWidthRight = .2f;
                            celldz5.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(celldz5);

                            PdfPCell celldz6 = new PdfPCell(new Phrase(" " + RelChrgAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            celldz6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            celldz6.BorderWidthBottom = 0;
                            celldz6.BorderWidthLeft = .2f;
                            celldz6.BorderWidthTop = 0;
                            celldz6.BorderWidthRight = .2f;
                            celldz6.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(celldz6);
                        }

                        int Noofcolumns = 4;
                        int Noofcolumnsheading = 3;
                        if (colCount == 4)
                        {
                            Noofcolumns = 2;
                            Noofcolumnsheading = 1;
                        }

                        PdfPCell celldz1 = new PdfPCell(new Phrase(ExtraRemarks, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        celldz1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        celldz1.Colspan = colCount - Noofcolumns;
                        celldz1.BorderWidthBottom = 0;
                        celldz1.BorderWidthLeft = .2f;
                        celldz1.BorderWidthTop = 0.2f;
                        celldz1.BorderWidthRight = 0.2f;
                        celldz1.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(celldz1);

                        celldz1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        celldz1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldz1.Colspan = Noofcolumnsheading;
                        celldz1.BorderWidthBottom = 0;
                        celldz1.BorderWidthLeft = .2f;
                        celldz1.BorderWidthTop = .2f;
                        celldz1.BorderWidthRight = 0;
                        celldz1.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(celldz1);

                        PdfPCell celldz4 = new PdfPCell(new Phrase(" " + (totalamount - (PFAmt + ESIAmt)).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        celldz4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        celldz4.BorderWidthBottom = 0;
                        celldz4.BorderWidthLeft = 0.2f;
                        celldz4.BorderWidthTop = .2f;
                        celldz4.BorderWidthRight = .2f;
                        celldz4.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(celldz4);


                        if (PFAmt > 0)
                        {

                            PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGST.Colspan = colCount - Noofcolumns;
                            CellCGST.BorderWidthBottom = 0;
                            CellCGST.BorderWidthLeft = .2f;
                            CellCGST.BorderWidthTop = 0f;
                            CellCGST.BorderWidthRight = 0.2f;
                            // CellCGST.PaddingBottom = 5;
                            // CellCGST.PaddingTop = 5;
                            CellCGST.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(CellCGST);

                            CellCGST = new PdfPCell(new Phrase("EPF Employer Share @ " + BpfPer + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGST.Colspan = Noofcolumnsheading;
                            CellCGST.BorderWidthBottom = 0;
                            CellCGST.BorderWidthLeft = .2f;
                            CellCGST.BorderWidthTop = 0.2f;
                            CellCGST.BorderWidthRight = 0f;
                            // CellCGST.PaddingBottom = 5;
                            // CellCGST.PaddingTop = 5;
                            CellCGST.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(CellCGST);

                            PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(PFAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGSTAmt.BorderWidthBottom = 0;
                            CellCGSTAmt.BorderWidthLeft = 0.2f;
                            CellCGSTAmt.BorderWidthTop = 0.2f;
                            CellCGSTAmt.BorderWidthRight = .2f;
                            CellCGSTAmt.BorderColor = BaseColor.BLACK;
                            //CellCGSTAmt.PaddingBottom = 5;
                            //CellCGSTAmt.PaddingTop = 5;
                            tempTable22.AddCell(CellCGSTAmt);

                        }


                        if (ESIAmt > 0)
                        {

                            PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGST.Colspan = colCount - Noofcolumns;
                            CellCGST.BorderWidthBottom = 0;
                            CellCGST.BorderWidthLeft = .2f;
                            CellCGST.BorderWidthTop = 0f;
                            CellCGST.BorderWidthRight = 0.2f;
                            // CellCGST.PaddingBottom = 5;
                            // CellCGST.PaddingTop = 5;
                            CellCGST.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(CellCGST);

                            CellCGST = new PdfPCell(new Phrase("ESI Employer Share @ " + BesiPer + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                            CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGST.Colspan = Noofcolumnsheading;
                            CellCGST.BorderWidthBottom = 0;
                            CellCGST.BorderWidthLeft = .2f;
                            CellCGST.BorderWidthTop = 0.2f;
                            CellCGST.BorderWidthRight = 0f;
                            // CellCGST.PaddingBottom = 5;
                            // CellCGST.PaddingTop = 5;
                            CellCGST.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(CellCGST);

                            PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(ESIAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellCGSTAmt.BorderWidthBottom = 0;
                            CellCGSTAmt.BorderWidthLeft = 0.2f;
                            CellCGSTAmt.BorderWidthTop = 0.2f;
                            CellCGSTAmt.BorderWidthRight = .2f;
                            CellCGSTAmt.BorderColor = BaseColor.BLACK;
                            //CellCGSTAmt.PaddingBottom = 5;
                            //CellCGSTAmt.PaddingTop = 5;
                            tempTable22.AddCell(CellCGSTAmt);
                        }

                        #region When Extradata check is false and STcheck is false

                        if (Extradatacheck == true)
                        {
                            if (machinarycost > 0)
                            {
                                if (STMachinary == true)
                                {
                                    if (SCMachinary == true)
                                    {
                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldzz.Colspan = colCount - Noofcolumns;
                                        celldzz.BorderWidthBottom = 0;
                                        celldzz.BorderWidthLeft = .2f;
                                        celldzz.BorderWidthTop = 0;
                                        celldzz.BorderWidthRight = 0.2f;
                                        celldzz.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldzz);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0;
                                        celldcst1.BorderWidthRight = .2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);


                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = .2f;
                                        celldcst2.BorderWidthTop = 0;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }
                            if (materialcost > 0)
                            {
                                if (STMaterial == true)
                                {
                                    if (SCMaterial == true)
                                    {
                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldzz.Colspan = colCount - Noofcolumns;
                                        celldzz.BorderWidthBottom = 0;
                                        celldzz.BorderWidthLeft = .2f;
                                        celldzz.BorderWidthTop = 0;
                                        celldzz.BorderWidthRight = 0.2f;
                                        celldzz.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldzz);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0;
                                        celldcst1.BorderWidthRight = .2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = .2f;
                                        celldcst2.BorderWidthTop = 0;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }


                            if (maintenancecost > 0)
                            {
                                if (STMaintenance == true)
                                {
                                    if (SCMaintenance == true)
                                    {
                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldzz.Colspan = colCount - Noofcolumns;
                                        celldzz.BorderWidthBottom = 0;
                                        celldzz.BorderWidthLeft = .2f;
                                        celldzz.BorderWidthTop = 0;
                                        celldzz.BorderWidthRight = 0.2f;
                                        celldzz.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldzz);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0;
                                        celldcst1.BorderWidthRight = .2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = .2f;
                                        celldcst2.BorderWidthTop = 0;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }

                            if (extraonecost > 0)
                            {
                                if (STExtraone == true)
                                {
                                    if (SCExtraone == true)
                                    {
                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldzz.Colspan = colCount - Noofcolumns;
                                        celldzz.BorderWidthBottom = 0;
                                        celldzz.BorderWidthLeft = .2f;
                                        celldzz.BorderWidthTop = 0;
                                        celldzz.BorderWidthRight = 0.2f;
                                        celldzz.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldzz);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0;
                                        celldcst1.BorderWidthRight = .2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = .2f;
                                        celldcst2.BorderWidthTop = 0;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }
                            if (extratwocost > 0)
                            {
                                if (STExtratwo == true)
                                {
                                    if (SCExtratwo == true)
                                    {
                                        PdfPCell celldzz = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                        celldzz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldzz.Colspan = colCount - Noofcolumns;
                                        celldzz.BorderWidthBottom = 0;
                                        celldzz.BorderWidthLeft = .2f;
                                        celldzz.BorderWidthTop = 0;
                                        celldzz.BorderWidthRight = 0.2f;
                                        celldzz.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldzz);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0;
                                        celldcst1.BorderWidthRight = .2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = .2f;
                                        celldcst2.BorderWidthTop = 0;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }

                        }

                        if (servicecharge > 0)//bSCType == true)
                        {
                            decimal scharge = servicecharge;
                            if (scharge > 0)
                            {
                                string SCharge = "";
                                if (bSCType == false)
                                {
                                    SCharge = ServiceCharge + "%";
                                }
                                else
                                {
                                    SCharge = ServiceCharge;
                                }


                                PdfPCell Cellservice = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cellservice.Colspan = colCount - Noofcolumns;
                                Cellservice.BorderWidthBottom = 0;
                                Cellservice.BorderWidthLeft = .2f;
                                Cellservice.BorderWidthTop = 0f;
                                Cellservice.BorderWidthRight = 0.2f;
                                Cellservice.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(Cellservice);

                                Cellservice = new PdfPCell(new Phrase("Service Charges @ " + SCharge, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cellservice.Colspan = Noofcolumnsheading;
                                Cellservice.BorderWidthBottom = 0;
                                Cellservice.BorderWidthLeft = .2f;
                                Cellservice.BorderWidthTop = 0;
                                Cellservice.BorderWidthRight = 0f;
                                Cellservice.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(Cellservice);

                                Cellservice = new PdfPCell(new Phrase(servicecharge.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellservice.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cellservice.BorderWidthBottom = 0;
                                Cellservice.BorderWidthLeft = 0.2f;
                                Cellservice.BorderWidthTop = 0;
                                Cellservice.BorderWidthRight = .2f;
                                Cellservice.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(Cellservice);
                            }
                        }
                        #endregion

                        #region When Extra data is checked and STcheck is true and SCcheck is false

                        if (machinarycost > 0)
                        {
                            if (STMachinary == true)
                            {
                                if (SCMachinary == false)
                                {
                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                    CellIGST2.BorderWidthBottom = 0;
                                    CellIGST2.BorderWidthLeft = .2f;
                                    CellIGST2.BorderWidthTop = 0f;
                                    CellIGST2.BorderWidthRight = 0.2f;
                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellIGST2);

                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = Noofcolumnsheading;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .2f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = 0.2f;
                                    celldcst1.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = .2f;
                                    celldcst2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst2);
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
                                if (SCMaterial == false)
                                {
                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                    CellIGST2.BorderWidthBottom = 0;
                                    CellIGST2.BorderWidthLeft = .2f;
                                    CellIGST2.BorderWidthTop = 0f;
                                    CellIGST2.BorderWidthRight = 0.2f;
                                    // CellCGST.PaddingBottom = 5;
                                    // CellCGST.PaddingTop = 5;
                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellIGST2);

                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = Noofcolumnsheading;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .2f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = 0.2f;
                                    celldcst1.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = .2f;
                                    celldcst2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst2);
                                }
                            }
                        }
                        if (maintenancecost > 0)
                        {
                            if (STMaintenance == true)
                            {
                                if (SCMaintenance == false)
                                {
                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                    CellIGST2.BorderWidthBottom = 0;
                                    CellIGST2.BorderWidthLeft = .2f;
                                    CellIGST2.BorderWidthTop = 0f;
                                    CellIGST2.BorderWidthRight = 0.2f;
                                    // CellCGST.PaddingBottom = 5;
                                    // CellCGST.PaddingTop = 5;
                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellIGST2);

                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = Noofcolumnsheading;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .2f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = 0.2f;
                                    celldcst1.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = .2f;
                                    celldcst2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst2);
                                }
                            }
                        }

                        if (extraonecost > 0)
                        {
                            if (STExtraone == true)
                            {
                                if (SCExtraone == false)
                                {
                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                    CellIGST2.BorderWidthBottom = 0;
                                    CellIGST2.BorderWidthLeft = .2f;
                                    CellIGST2.BorderWidthTop = 0f;
                                    CellIGST2.BorderWidthRight = 0.2f;
                                    // CellCGST.PaddingBottom = 5;
                                    // CellCGST.PaddingTop = 5;
                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellIGST2);

                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = Noofcolumnsheading;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .2f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = 0.2f;
                                    celldcst1.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = .2f;
                                    celldcst2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst2);
                                }
                            }
                        }
                        if (extratwocost > 0)
                        {
                            if (STExtratwo == true)
                            {
                                if (SCExtratwo == false)
                                {
                                    PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    CellIGST2.Colspan = colCount - Noofcolumns;
                                    CellIGST2.BorderWidthBottom = 0;
                                    CellIGST2.BorderWidthLeft = .2f;
                                    CellIGST2.BorderWidthTop = 0f;
                                    CellIGST2.BorderWidthRight = 0.2f;
                                    // CellCGST.PaddingBottom = 5;
                                    // CellCGST.PaddingTop = 5;
                                    CellIGST2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(CellIGST2);

                                    PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst1.Colspan = Noofcolumnsheading;
                                    celldcst1.BorderWidthBottom = 0;
                                    celldcst1.BorderWidthLeft = .2f;
                                    celldcst1.BorderWidthTop = 0;
                                    celldcst1.BorderWidthRight = 0.2f;
                                    celldcst1.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst1);

                                    PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                    celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    celldcst2.BorderWidthBottom = 0;
                                    celldcst2.BorderWidthLeft = 0f;
                                    celldcst2.BorderWidthTop = 0;
                                    celldcst2.BorderWidthRight = .2f;
                                    celldcst2.BorderColor = BaseColor.BLACK;
                                    tempTable22.AddCell(celldcst2);
                                }
                            }
                        }

                        #endregion

                        #endregion



                        decimal GSTDiscounts = 0;

                        if (STDiscountone == true)
                        {
                            if (discountone > 0)
                            {

                                PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellbbCGST.Colspan = colCount - Noofcolumns;
                                CellbbCGST.BorderWidthBottom = 0;
                                CellbbCGST.BorderWidthLeft = .2f;
                                CellbbCGST.BorderWidthTop = 0f;
                                CellbbCGST.BorderWidthRight = 0.2f;
                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellbbCGST);

                                CellbbCGST = new PdfPCell(new Phrase(discountonetitle, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellbbCGST.Colspan = Noofcolumnsheading;
                                CellbbCGST.BorderWidthBottom = 0;
                                CellbbCGST.BorderWidthLeft = .2f;
                                CellbbCGST.BorderWidthTop = 0.2f;
                                CellbbCGST.BorderWidthRight = 0f;
                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellbbCGST);

                                CellbbCGST = new PdfPCell(new Phrase(discountone.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellbbCGST.BorderWidthBottom = 0;
                                CellbbCGST.BorderWidthLeft = 0.2f;
                                CellbbCGST.BorderWidthTop = 0.2f;
                                CellbbCGST.BorderWidthRight = .2f;
                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellbbCGST);
                                GSTDiscounts += discountone;



                            }
                        }

                        if (STDiscounttwo == true)
                        {
                            if (discounttwo > 0)
                            {
                                PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellbbCGST.Colspan = colCount - Noofcolumns;
                                CellbbCGST.BorderWidthBottom = 0;
                                CellbbCGST.BorderWidthLeft = .2f;
                                CellbbCGST.BorderWidthTop = 0f;
                                CellbbCGST.BorderWidthRight = 0.2f;
                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellbbCGST);

                                CellbbCGST = new PdfPCell(new Phrase(discounttwotitle, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellbbCGST.Colspan = Noofcolumnsheading;
                                CellbbCGST.BorderWidthBottom = 0;
                                CellbbCGST.BorderWidthLeft = .2f;
                                CellbbCGST.BorderWidthTop = 0.2f;
                                CellbbCGST.BorderWidthRight = 0f;
                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellbbCGST);

                                CellbbCGST = new PdfPCell(new Phrase(discounttwo.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellbbCGST.BorderWidthBottom = 0;
                                CellbbCGST.BorderWidthLeft = 0.2f;
                                CellbbCGST.BorderWidthTop = 0.2f;
                                CellbbCGST.BorderWidthRight = .2f;
                                CellbbCGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellbbCGST);
                                GSTDiscounts += discounttwo;
                            }
                        }

                        if (((Grandtotal - (CGST + SGST + IGST)) - totalamount) > 0)
                        {
                            PdfPCell CellbbCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                            CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellbbCGST.Colspan = colCount - Noofcolumns;
                            CellbbCGST.BorderWidthBottom = 0;
                            CellbbCGST.BorderWidthLeft = .2f;
                            CellbbCGST.BorderWidthTop = 0f;
                            CellbbCGST.BorderWidthRight = 0.2f;
                            CellbbCGST.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(CellbbCGST);

                            CellbbCGST = new PdfPCell(new Phrase("Total Before Tax", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellbbCGST.Colspan = Noofcolumnsheading;
                            CellbbCGST.BorderWidthBottom = 0;
                            CellbbCGST.BorderWidthLeft = .2f;
                            CellbbCGST.BorderWidthTop = 0.2f;
                            CellbbCGST.BorderWidthRight = 0f;
                            CellbbCGST.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(CellbbCGST);

                            CellbbCGST = new PdfPCell(new Phrase(((Grandtotal - (CGST + SGST + IGST) - GSTDiscounts)).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                            CellbbCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            CellbbCGST.BorderWidthBottom = 0;
                            CellbbCGST.BorderWidthLeft = 0.2f;
                            CellbbCGST.BorderWidthTop = 0.2f;
                            CellbbCGST.BorderWidthRight = .2f;
                            CellbbCGST.BorderColor = BaseColor.BLACK;
                            tempTable22.AddCell(CellbbCGST);
                        }



                        #region for taxes

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
                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGST2.Colspan = colCount - Noofcolumns;
                                CellIGST2.BorderWidthBottom = 0;
                                CellIGST2.BorderWidthLeft = .2f;
                                CellIGST2.BorderWidthTop = 0f;
                                CellIGST2.BorderWidthRight = 0.2f;
                                // CellCGST.PaddingBottom = 5;
                                // CellCGST.PaddingTop = 5;
                                CellIGST2.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellIGST2);

                                PdfPCell celldd2 = new PdfPCell(new Phrase("Service Tax @ " + scpercent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldd2.Colspan = Noofcolumnsheading;
                                celldd2.BorderWidthBottom = 0;
                                celldd2.BorderWidthLeft = .2f;
                                celldd2.BorderWidthTop = 0.2f;
                                celldd2.BorderWidthRight = 0f;
                                //celldd2.PaddingBottom = 5;
                                //celldd2.PaddingTop = 5;
                                celldd2.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(celldd2);


                                PdfPCell celldd4 = new PdfPCell(new Phrase(servicetax.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldd4.BorderWidthBottom = 0;
                                celldd4.BorderWidthLeft = 0.2f;
                                celldd4.BorderWidthTop = 0.2f;
                                celldd4.BorderWidthRight = .2f;
                                celldd4.BorderColor = BaseColor.BLACK;
                                //celldd4.PaddingBottom = 5;
                                //celldd4.PaddingTop = 5;
                                tempTable22.AddCell(celldd4);

                            }

                            if (sbcess > 0)
                            {
                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGST2.Colspan = colCount - Noofcolumns;
                                CellIGST2.BorderWidthBottom = 0;
                                CellIGST2.BorderWidthLeft = .2f;
                                CellIGST2.BorderWidthTop = 0f;
                                CellIGST2.BorderWidthRight = 0.2f;
                                // CellCGST.PaddingBottom = 5;
                                // CellCGST.PaddingTop = 5;
                                CellIGST2.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellIGST2);

                                string SBCESSPresent = DtTaxes.Rows[0]["SBCess"].ToString();
                                PdfPCell celldd2 = new PdfPCell(new Phrase("Swachh Bharat Cess @ " + SBCESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldd2.Colspan = Noofcolumnsheading;
                                celldd2.BorderWidthBottom = 0;
                                celldd2.BorderWidthLeft = .2f;
                                celldd2.BorderWidthTop = 0.2f;
                                celldd2.BorderWidthRight = 0f;
                                celldd2.BorderColor = BaseColor.BLACK;
                                // celldd2.PaddingBottom = 5;
                                // celldd2.PaddingTop = 5;
                                tempTable22.AddCell(celldd2);


                                PdfPCell celldd4 = new PdfPCell(new Phrase(sbcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldd4.BorderWidthBottom = 0;
                                celldd4.BorderWidthLeft = 0.2f;
                                celldd4.BorderWidthTop = 0.2f;
                                celldd4.BorderWidthRight = .2f;
                                celldd4.BorderColor = BaseColor.BLACK;
                                //celldd4.PaddingBottom = 5;
                                //celldd4.PaddingTop = 5;
                                tempTable22.AddCell(celldd4);

                            }


                            if (kkcess > 0)
                            {

                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGST2.Colspan = colCount - Noofcolumns;
                                CellIGST2.BorderWidthBottom = 0;
                                CellIGST2.BorderWidthLeft = .2f;
                                CellIGST2.BorderWidthTop = 0f;
                                CellIGST2.BorderWidthRight = 0.2f;
                                // CellCGST.PaddingBottom = 5;
                                // CellCGST.PaddingTop = 5;
                                CellIGST2.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellIGST2);

                                string KKCESSPresent = DtTaxes.Rows[0]["KKCess"].ToString();
                                PdfPCell Cellmtcesskk1 = new PdfPCell(new Phrase("Krishi Kalyan Cess @ " + KKCESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellmtcesskk1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cellmtcesskk1.Colspan = Noofcolumnsheading;
                                Cellmtcesskk1.BorderWidthBottom = 0;
                                Cellmtcesskk1.BorderWidthLeft = .2f;
                                Cellmtcesskk1.BorderWidthTop = 0.2f;
                                Cellmtcesskk1.BorderWidthRight = 0f;
                                // celldd2.PaddingBottom = 5;
                                // celldd2.PaddingTop = 5;
                                Cellmtcesskk1.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(Cellmtcesskk1);

                                PdfPCell Cellmtcesskk2 = new PdfPCell(new Phrase(kkcess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                Cellmtcesskk2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                Cellmtcesskk2.BorderWidthBottom = 0;
                                Cellmtcesskk2.BorderWidthLeft = 0.2f;
                                Cellmtcesskk2.BorderWidthTop = 0.2f;
                                Cellmtcesskk2.BorderWidthRight = .2f;
                                Cellmtcesskk2.BorderColor = BaseColor.BLACK;
                                //celldd4.PaddingBottom = 5;
                                //celldd4.PaddingTop = 5;
                                tempTable22.AddCell(Cellmtcesskk2);

                            }

                            #region for GST as on 17-6-2017

                            if (CGST > 0)
                            {
                                PdfPCell CellCGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCGST.Colspan = colCount - Noofcolumns;
                                CellCGST.BorderWidthBottom = 0;
                                CellCGST.BorderWidthLeft = .2f;
                                CellCGST.BorderWidthTop = 0f;
                                CellCGST.BorderWidthRight = 0.2f;
                                // CellCGST.PaddingBottom = 5;
                                // CellCGST.PaddingTop = 5;
                                CellCGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellCGST);

                                CellCGST = new PdfPCell(new Phrase(CGSTAlias + " @ " + CGSTPrc + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                CellCGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCGST.Colspan = Noofcolumnsheading;
                                CellCGST.BorderWidthBottom = 0;
                                CellCGST.BorderWidthLeft = .2f;
                                CellCGST.BorderWidthTop = 0.2f;
                                CellCGST.BorderWidthRight = 0f;
                                // CellCGST.PaddingBottom = 5;
                                // CellCGST.PaddingTop = 5;
                                CellCGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellCGST);

                                PdfPCell CellCGSTAmt = new PdfPCell(new Phrase(CGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellCGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCGSTAmt.BorderWidthBottom = 0;
                                CellCGSTAmt.BorderWidthLeft = 0.2f;
                                CellCGSTAmt.BorderWidthTop = 0.2f;
                                CellCGSTAmt.BorderWidthRight = .2f;
                                CellCGSTAmt.BorderColor = BaseColor.BLACK;
                                //CellCGSTAmt.PaddingBottom = 5;
                                //CellCGSTAmt.PaddingTop = 5;
                                tempTable22.AddCell(CellCGSTAmt);

                            }


                            if (SGST > 0)
                            {
                                PdfPCell CellSGST = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellSGST.Colspan = colCount - Noofcolumns;
                                CellSGST.BorderWidthBottom = 0;
                                CellSGST.BorderWidthLeft = .2f;
                                CellSGST.BorderWidthTop = 0f;
                                CellSGST.BorderWidthRight = 0.2f;
                                // CellSGST.PaddingBottom = 5;
                                // CellSGST.PaddingTop = 5;
                                CellSGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellSGST);

                                CellSGST = new PdfPCell(new Phrase(SGSTAlias + " @ " + SGSTPrc + "%", FontFactory.GetFont(FontStyle, font - 1, Font.NORMAL, BaseColor.BLACK)));
                                CellSGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellSGST.Colspan = Noofcolumnsheading;
                                CellSGST.BorderWidthBottom = 0;
                                CellSGST.BorderWidthLeft = .2f;
                                CellSGST.BorderWidthTop = 0.2f;
                                CellSGST.BorderWidthRight = 0f;
                                // CellSGST.PaddingBottom = 5;
                                // CellSGST.PaddingTop = 5;
                                CellSGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellSGST);

                                PdfPCell CellSGSTAmt = new PdfPCell(new Phrase(SGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellSGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellSGSTAmt.BorderWidthBottom = 0;
                                CellSGSTAmt.BorderWidthLeft = 0.2f;
                                CellSGSTAmt.BorderWidthTop = 0.2f;
                                CellSGSTAmt.BorderWidthRight = .2f;
                                CellSGSTAmt.BorderColor = BaseColor.BLACK;
                                //CellSGSTAmt.PaddingBottom = 5;
                                //CellSGSTAmt.PaddingTop = 5;
                                tempTable22.AddCell(CellSGSTAmt);


                            }

                            if (IGST > 0)
                            {
                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGST2.Colspan = colCount - Noofcolumns;
                                CellIGST2.BorderWidthBottom = 0;
                                CellIGST2.BorderWidthLeft = .2f;
                                CellIGST2.BorderWidthTop = 0f;
                                CellIGST2.BorderWidthRight = 0.2f;
                                // CellCGST.PaddingBottom = 5;
                                // CellCGST.PaddingTop = 5;
                                CellIGST2.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellIGST2);

                                PdfPCell CellIGST = new PdfPCell(new Phrase(IGSTAlias + " @ " + IGSTPrc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellIGST.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGST.Colspan = Noofcolumnsheading;
                                CellIGST.BorderWidthBottom = 0;
                                CellIGST.BorderWidthLeft = .2f;
                                CellIGST.BorderWidthTop = 0.2f;
                                CellIGST.BorderWidthRight = 0f;
                                // CellIGST.PaddingBottom = 5;
                                // CellIGST.PaddingTop = 5;
                                CellIGST.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellIGST);

                                PdfPCell CellIGSTAmt = new PdfPCell(new Phrase(IGST.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellIGSTAmt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGSTAmt.BorderWidthBottom = 0;
                                CellIGSTAmt.BorderWidthLeft = 0.2f;
                                CellIGSTAmt.BorderWidthTop = 0.2f;
                                CellIGSTAmt.BorderWidthRight = .2f;
                                CellIGSTAmt.BorderColor = BaseColor.BLACK;
                                //CellIGSTAmt.PaddingBottom = 5;
                                //CellIGSTAmt.PaddingTop = 5;
                                tempTable22.AddCell(CellIGSTAmt);


                            }

                            if (Cess1 > 0)
                            {
                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGST2.Colspan = colCount - Noofcolumns;
                                CellIGST2.BorderWidthBottom = 0;
                                CellIGST2.BorderWidthLeft = .2f;
                                CellIGST2.BorderWidthTop = 0f;
                                CellIGST2.BorderWidthRight = 0.2f;
                                // CellCGST.PaddingBottom = 5;
                                // CellCGST.PaddingTop = 5;
                                CellIGST2.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellIGST2);

                                PdfPCell CellCess1 = new PdfPCell(new Phrase(Cess1Alias + " @ " + Cess1Prc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellCess1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCess1.Colspan = Noofcolumnsheading;
                                CellCess1.BorderWidthBottom = 0;
                                CellCess1.BorderWidthLeft = .2f;
                                CellCess1.BorderWidthTop = 0.2f;
                                CellCess1.BorderWidthRight = 0f;
                                // CellCess1.PaddingBottom = 5;
                                // CellCess1.PaddingTop = 5;
                                CellCess1.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellCess1);

                                PdfPCell CellCess1Amt = new PdfPCell(new Phrase(Cess1.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellCess1Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCess1Amt.BorderWidthBottom = 0;
                                CellCess1Amt.BorderWidthLeft = 0.2f;
                                CellCess1Amt.BorderWidthTop = 0.2f;
                                CellCess1Amt.BorderWidthRight = .2f;
                                CellCess1Amt.BorderColor = BaseColor.BLACK;
                                //CellCess1Amt.PaddingBottom = 5;
                                //CellCess1Amt.PaddingTop = 5;
                                tempTable22.AddCell(CellCess1Amt);

                            }


                            if (Cess2 > 0)
                            {
                                PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellIGST2.Colspan = colCount - Noofcolumns;
                                CellIGST2.BorderWidthBottom = 0;
                                CellIGST2.BorderWidthLeft = .2f;
                                CellIGST2.BorderWidthTop = 0f;
                                CellIGST2.BorderWidthRight = 0.2f;
                                // CellCGST.PaddingBottom = 5;
                                // CellCGST.PaddingTop = 5;
                                CellIGST2.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellIGST2);

                                PdfPCell CellCess2 = new PdfPCell(new Phrase(Cess2Alias + " @ " + Cess2Prc + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellCess2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCess2.Colspan = Noofcolumnsheading;
                                CellCess2.BorderWidthBottom = 0;
                                CellCess2.BorderWidthLeft = .2f;
                                CellCess2.BorderWidthTop = 0.2f;
                                CellCess2.BorderWidthRight = 0f;
                                // CellCess2.PaddingBottom = 5;
                                // CellCess2.PaddingTop = 5;
                                CellCess2.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(CellCess2);

                                PdfPCell CellCess2Amt = new PdfPCell(new Phrase(Cess2.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                CellCess2Amt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                CellCess2Amt.BorderWidthBottom = 0;
                                CellCess2Amt.BorderWidthLeft = 0.2f;
                                CellCess2Amt.BorderWidthTop = 0.2f;
                                CellCess2Amt.BorderWidthRight = .2f;
                                CellCess2Amt.BorderColor = BaseColor.BLACK;
                                //CellCess2Amt.PaddingBottom = 5;
                                //CellCess2Amt.PaddingTop = 5;
                                tempTable22.AddCell(CellCess2Amt);

                            }

                            #endregion for GST

                            if (cess > 0)
                            {

                                string CESSPresent = DtTaxes.Rows[0]["Cess"].ToString();
                                PdfPCell celldd2 = new PdfPCell(new Phrase("CESS @ " + CESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                celldd2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldd2.Colspan = colCount - 1;
                                celldd2.BorderWidthBottom = 0;
                                celldd2.BorderWidthLeft = .2f;
                                celldd2.BorderWidthTop = 0.2f;
                                celldd2.BorderWidthRight = 0f;
                                celldd2.BorderColor = BaseColor.BLACK;
                                //celldd2.PaddingBottom = 5;
                                //celldd2.PaddingTop = 5;
                                tempTable22.AddCell(celldd2);


                                PdfPCell celldd4 = new PdfPCell(new Phrase(cess.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                celldd4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldd4.BorderWidthBottom = 0;
                                celldd4.BorderWidthLeft = 0.2f;
                                celldd4.BorderWidthTop = 0.2f;
                                celldd4.BorderWidthRight = .2f;
                                celldd4.BorderColor = BaseColor.BLACK;
                                //celldd4.PaddingBottom = 5;
                                //celldd4.PaddingTop = 5;
                                tempTable22.AddCell(celldd4);

                            }

                            if (shecess > 0)
                            {


                                string SHECESSPresent = DtTaxes.Rows[0]["shecess"].ToString();
                                PdfPCell celldf2 = new PdfPCell(new Phrase("S&H Ed.CESS @ " + SHECESSPresent + "%", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                celldf2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldf2.Colspan = colCount - 1;
                                celldf2.BorderWidthBottom = 0;
                                celldf2.BorderWidthLeft = .2f;
                                celldf2.BorderWidthTop = 0.2f;
                                celldf2.BorderWidthRight = 0f;
                                celldf2.BorderColor = BaseColor.BLACK;
                                //celldf2.PaddingBottom = 5;
                                //celldf2.PaddingTop = 5;
                                tempTable22.AddCell(celldf2);


                                PdfPCell celldf4 = new PdfPCell(new Phrase(shecess.ToString("0.00"),
                                    FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                celldf4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldf4.BorderWidthBottom = 0;
                                celldf4.BorderWidthLeft = 0.2f;
                                celldf4.BorderWidthTop = 0.2f;
                                celldf4.BorderWidthRight = .2f;
                                celldf4.BorderColor = BaseColor.BLACK;
                                ////celldf4.PaddingBottom = 5;
                                //celldf4.PaddingTop = 5;
                                //celldf4.BorderColor = BaseColor.BLACK;
                                tempTable22.AddCell(celldf4);
                            }
                            #region When Extra data is checked and STcheck is false and SCcheck is true

                            if (machinarycost > 0)
                            {
                                if (STMachinary == false)
                                {
                                    if (SCMachinary == true)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }


                            }
                            if (materialcost > 0)
                            {
                                if (STMaterial == false)
                                {
                                    if (SCMaterial == true)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }
                            if (maintenancecost > 0)
                            {
                                if (STMaintenance == false)
                                {
                                    if (SCMaintenance == true)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }

                            if (extraonecost > 0)
                            {
                                if (STExtraone == false)
                                {
                                    if (SCExtraone == true)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }
                            if (extratwocost > 0)
                            {
                                if (STExtratwo == false)
                                {
                                    if (SCExtratwo == true)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }

                            #endregion



                            #region When Extra data is checked and STcheck is false and SCcheck is true

                            if (machinarycost > 0)
                            {
                                if (STMachinary == false)
                                {
                                    if (SCMachinary == false)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(machinarycosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(machinarycost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
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
                                    if (SCMaterial == false)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(materialcosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(materialcost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }
                            if (maintenancecost > 0)
                            {
                                if (STMaintenance == false)
                                {
                                    if (SCMaintenance == false)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(maintenancecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(maintenancecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }

                            if (extraonecost > 0)
                            {
                                if (STExtraone == false)
                                {
                                    if (SCExtraone == false)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extraonecosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extraonecost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }
                            if (extratwocost > 0)
                            {
                                if (STExtratwo == false)
                                {
                                    if (SCExtratwo == false)
                                    {
                                        PdfPCell CellIGST2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        CellIGST2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        CellIGST2.Colspan = colCount - Noofcolumns;
                                        CellIGST2.BorderWidthBottom = 0;
                                        CellIGST2.BorderWidthLeft = .2f;
                                        CellIGST2.BorderWidthTop = 0f;
                                        CellIGST2.BorderWidthRight = 0.2f;
                                        // CellCGST.PaddingBottom = 5;
                                        // CellCGST.PaddingTop = 5;
                                        CellIGST2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(CellIGST2);

                                        PdfPCell celldcst1 = new PdfPCell(new Phrase(extratwocosttitle, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst1.Colspan = Noofcolumnsheading;
                                        celldcst1.BorderWidthBottom = 0;
                                        celldcst1.BorderWidthLeft = .2f;
                                        celldcst1.BorderWidthTop = 0.2f;
                                        celldcst1.BorderWidthRight = 0.2f;
                                        celldcst1.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst1);

                                        PdfPCell celldcst2 = new PdfPCell(new Phrase(extratwocost.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                                        celldcst2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                        celldcst2.BorderWidthBottom = 0;
                                        celldcst2.BorderWidthLeft = 0f;
                                        celldcst2.BorderWidthTop = 0.2f;
                                        celldcst2.BorderWidthRight = .2f;
                                        celldcst2.BorderColor = BaseColor.BLACK;
                                        tempTable22.AddCell(celldcst2);
                                    }
                                }
                            }

                            #endregion

                            decimal totaltaxamt = 0;
                            totaltaxamt = CGST + SGST + IGST;
                            if (totaltaxamt > 0)
                            {
                                PdfPCell celldfTax2 = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                celldfTax2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldfTax2.Colspan = colCount - 1;
                                celldfTax2.BorderWidthBottom = 0;
                                celldfTax2.BorderWidthLeft = .2f;
                                celldfTax2.BorderWidthTop = 0;
                                celldfTax2.BorderWidthRight = 0f;
                                celldfTax2.BorderColor = BaseColor.BLACK;
                                //celldfTax2.PaddingBottom = 5;
                                //celldfTax2.PaddingTop = 5;
                                //tempTable22.AddCell(celldfTax2);

                                PdfPCell celldf4tax = new PdfPCell(new Phrase(totaltaxamt.ToString("#,##0.00"),
                                    FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                celldf4tax.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                celldf4tax.BorderWidthBottom = 0;
                                celldf4tax.BorderWidthLeft = 0.2f;
                                celldf4tax.BorderWidthTop = 0;
                                celldf4tax.BorderWidthRight = .2f;
                                celldf4tax.BorderColor = BaseColor.BLACK;
                                ////celldf4tax.PaddingBottom = 5;
                                //celldf4tax.PaddingTop = 5;
                                //celldf4tax.BorderColor = BaseColor.BLACK;
                                //tempTable22.AddCell(celldf4tax);
                            }
                        }


                        #endregion for taxes

                        decimal GrandTotalVal = Grandtotal;

                        decimal GrandtotalRoundOff = Math.Round(GrandTotalVal, 0);
                        decimal RoundOff = (GrandtotalRoundOff - GrandTotalVal);
                        decimal GrandtotalValue = (GrandTotalVal + RoundOff);

                        PdfPCell cellgrandto = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellgrandto.Colspan = colCount - Noofcolumns;
                        cellgrandto.BorderWidthBottom = 0;
                        cellgrandto.BorderWidthLeft = .2f;
                        cellgrandto.BorderWidthTop = 0f;
                        cellgrandto.BorderWidthRight = 0.2f;
                        cellgrandto.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(cellgrandto);

                        cellgrandto = new PdfPCell(new Phrase("Round off", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellgrandto.Colspan = Noofcolumnsheading;
                        cellgrandto.BorderWidthBottom = 0;
                        cellgrandto.BorderWidthLeft = .2f;
                        cellgrandto.BorderWidthTop = 0.2f;
                        cellgrandto.BorderWidthRight = 0f;
                        cellgrandto.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(cellgrandto);

                        cellgrandto = new PdfPCell(new Phrase(RoundOff.ToString("N2"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellgrandto.BorderWidthBottom = 0;
                        cellgrandto.BorderWidthLeft = 0.2f;
                        cellgrandto.BorderWidthTop = 0.2f;
                        cellgrandto.BorderWidthRight = .2f;
                        cellgrandto.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(cellgrandto);

                        cellgrandto = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellgrandto.Colspan = colCount - Noofcolumns;
                        cellgrandto.BorderWidthBottom = 0;
                        cellgrandto.BorderWidthLeft = .2f;
                        cellgrandto.BorderWidthTop = 0f;
                        cellgrandto.BorderWidthRight = 0.2f;
                        cellgrandto.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(cellgrandto);

                        cellgrandto = new PdfPCell(new Phrase("Grand Total", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellgrandto.Colspan = Noofcolumnsheading;
                        cellgrandto.BorderWidthBottom = 0;
                        cellgrandto.BorderWidthLeft = .2f;
                        cellgrandto.BorderWidthTop = 0.2f;
                        cellgrandto.BorderWidthRight = 0f;
                        cellgrandto.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(cellgrandto);

                        cellgrandto = new PdfPCell(new Phrase(GrandtotalValue.ToString("N2"), FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cellgrandto.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cellgrandto.BorderWidthBottom = 0;
                        cellgrandto.BorderWidthLeft = 0.2f;
                        cellgrandto.BorderWidthTop = 0.2f;
                        cellgrandto.BorderWidthRight = .2f;
                        cellgrandto.BorderColor = BaseColor.BLACK;
                        tempTable22.AddCell(cellgrandto);


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

                        if (GSTLineitem == true)
                        {
                            #region FooterTable
                            PdfPTable tablev = new PdfPTable(7);
                            tablev.TotalWidth = 580f;
                            tablev.LockedWidth = true;
                            float[] widthss = new float[] { 1.5f, 2.5f, 2f, 2f, 2f, 2f, 3f };
                            tablev.SetWidths(widthss);

                            if ((CGST + SGST) > 0)
                            {
                                cell = new PdfPCell(new Phrase("HSN Code", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Taxable Value", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Central Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 2;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("State Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 2;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);
                            }
                            else
                            {

                                cell = new PdfPCell(new Phrase("HSN Code", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Taxable Value", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 2;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Integrated Tax", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 1.5f;
                                cell.Colspan = 2;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 1.5f;
                                cell.Colspan = 2;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);
                            }





                            //2ndrow
                            if ((CGST + SGST) > 0)
                            {

                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);


                                cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 2;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Rate", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0.5f;
                                cell.Colspan = 1;
                                cell.PaddingBottom = 3;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 1.5f;
                                cell.PaddingBottom = 3;
                                cell.Colspan = 1;
                                tablev.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Total Tax Amount", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 1;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 1.5f;
                                cell.PaddingBottom = 3;
                                cell.Colspan = 2;
                                tablev.AddCell(cell);
                            }


                            DataTable DtDesgnWise = null;
                            var SPDesgnWise = "DesginWiseGSTAmounts";
                            Hashtable htDesgnWise = new Hashtable();
                            htDesgnWise.Add("@ClientID", ClientId);
                            htDesgnWise.Add("@Month", Month);
                            htDesgnWise.Add("@BillType", Status);
                            htDesgnWise.Add("@BillNo", BillNo);

                            DtDesgnWise = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPDesgnWise, htDesgnWise);


                            decimal TotalTaxableval = 0;
                            decimal CGSTAmt = 0;
                            decimal SGSTAmt = 0;
                            decimal IGSTAmt = 0;
                            decimal GrandTotalTaxableval = 0;
                            decimal GrandCGSTAmt = 0;
                            decimal GrandSGSTAmt = 0;
                            decimal GrandIGSTAmt = 0;
                            decimal CGSTRate = 0;
                            decimal SGSTRate = 0;
                            decimal IGSTRate = 0;
                            string HSNCode = "";

                            if (DtDesgnWise.Rows.Count > 0)
                            {
                                for (int k = 0; k < DtDesgnWise.Rows.Count; k++)
                                {
                                    TotalTaxableval = decimal.Parse(DtDesgnWise.Rows[k]["TotalTaxableval"].ToString());
                                    CGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["CGSTAmt"].ToString());
                                    SGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["SGSTAmt"].ToString());
                                    IGSTAmt = decimal.Parse(DtDesgnWise.Rows[k]["IGSTAmt"].ToString());
                                    CGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["CGSTPrc"].ToString());
                                    SGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["SGSTPrc"].ToString());
                                    IGSTRate = decimal.Parse(DtDesgnWise.Rows[k]["IGSTPrc"].ToString());
                                    HSNCode = DtDesgnWise.Rows[k]["HSNCode"].ToString();

                                    if ((CGST + SGST) > 0)
                                    {
                                        cell = new PdfPCell(new Phrase(HSNCode, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = .2f;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(TotalTaxableval.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);
                                        GrandTotalTaxableval += (TotalTaxableval);

                                        cell = new PdfPCell(new Phrase(CGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(CGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);
                                        GrandCGSTAmt += CGSTAmt;


                                        cell = new PdfPCell(new Phrase(SGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(SGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);
                                        GrandSGSTAmt += SGSTAmt;

                                        cell = new PdfPCell(new Phrase((GrandCGSTAmt + GrandSGSTAmt).ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);
                                    }
                                    else
                                    {
                                        cell = new PdfPCell(new Phrase(HSNCode, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 0;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = .2f;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(TotalTaxableval.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 2;
                                        tablev.AddCell(cell);
                                        GrandTotalTaxableval += (TotalTaxableval);


                                        cell = new PdfPCell(new Phrase(IGSTRate.ToString() + "%", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 0.5f;
                                        cell.Colspan = 1;
                                        cell.PaddingBottom = 3;
                                        tablev.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(IGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 1.5f;
                                        cell.PaddingBottom = 3;
                                        cell.Colspan = 1;
                                        tablev.AddCell(cell);
                                        GrandIGSTAmt += IGSTAmt;

                                        cell = new PdfPCell(new Phrase(GrandIGSTAmt.ToString("#,##0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                        cell.HorizontalAlignment = 2;
                                        cell.BorderWidthBottom = 0.5f;
                                        cell.BorderWidthLeft = 0;
                                        cell.BorderWidthTop = 0;
                                        cell.BorderWidthRight = 1.5f;
                                        cell.PaddingBottom = 3;
                                        cell.Colspan = 2;
                                        tablev.AddCell(cell);
                                    }



                                }


                                decimal TotalAmount = GrandCGSTAmt + GrandIGSTAmt + GrandSGSTAmt;

                                string GTotal = TotalAmount.ToString("0.00");
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
                                    inwords = " Rupees " + rupee + "" + paise + " Paise Only /-";

                                }
                                else
                                {
                                    rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(arr[0]), true);
                                    inwords = " Rupees " + rupee + " Only /-";
                                }


                                cell = new PdfPCell(new Phrase("Total Tax Amount (in words) :" + inwords, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0.5f;
                                cell.BorderWidthLeft = .2f;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = .2f;
                                cell.PaddingBottom = 3;
                                cell.Colspan = 7;
                                tablev.AddCell(cell);


                            }


                            document.Add(tablev);

                            #endregion
                        }

                        #endregion

                        #region footer

                        PdfPTable Addterms = new PdfPTable(6);
                        Addterms.TotalWidth = 580f;
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
                        Childterms.TotalWidth = 335f;
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

                        if (Bdt.Rows.Count > 0)
                        {
                            if (OurGSTIN.Length > 0)
                            {


                                cell = new PdfPCell(new Phrase(OurGSTINAlias, FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidthBottom = 0;
                                cell.BorderWidthTop = 0;
                                cell.BorderWidthRight = 0f;
                                cell.BorderWidthLeft = .2f;
                                cell.Colspan = 1;
                                cell.BorderColor = BaseColor.BLACK;
                                Childterms.AddCell(cell);


                                cell = new PdfPCell(new Phrase(": " + OurGSTIN, FontFactory.GetFont(FontStyle, font, Font.NORMAL, BaseColor.BLACK)));
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
                        chilk.TotalWidth = 245f;
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

                        cell = new PdfPCell(new Phrase("\n\n\n Authorised Signatory", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
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
                        //chilk.AddCell(cell);


                        PdfPCell Chid4 = new PdfPCell(chilk);
                        Chid4.Border = 0;
                        Chid4.Colspan = 3;
                        Chid4.HorizontalAlignment = 0;
                        Addterms.AddCell(Chid4);


                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, font, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthRight = 0;
                        cell.BorderWidthLeft = 0;
                        cell.Colspan = 6;
                        // Addterms.AddCell(cell);

                        document.Add(Addterms);
                        document.NewPage();

                        #endregion
                    }

                }

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }
            catch (Exception ex)
            {

            }
        }
    }
}