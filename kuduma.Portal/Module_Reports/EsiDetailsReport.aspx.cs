using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using KLTS.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Globalization;
using OfficeOpenXml;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class EsiDetailsReport : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            LblResult.Visible = true;
            LblResult.Text = "";
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
              
                LoadEsibranches();
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void ClearGridData()
        {
            LblResult.Text = "";
            GVListOfClients.DataSource = null;
            GVListOfClients.DataBind();
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

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
           
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVListOfClients.DataSource = null;
            GVListOfClients.DataBind();

            LblResult.Text = "";
            DataTable dtone = null;
            string date = "";


            string SelectDate = string.Empty;
            if (txtmonth.Text.Trim().Length > 0)
            {
                SelectDate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(SelectDate).Month.ToString();
            string Year = DateTime.Parse(SelectDate).Year.ToString();
            string esibranch = "";

            if (ddlEsibranch.SelectedIndex == 0)
            {
                esibranch = "%";
            }
            else
            {
                esibranch = ddlEsibranch.SelectedValue;
            }

            int monthdays = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(month));
            string Type = "ESI";

            string SPName = "PFESIDetailsReport";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month + Year.Substring(2, 2));
            ht.Add("@ESIBranch", esibranch);
            ht.Add("@MonthDays", monthdays);
            ht.Add("@Type", Type);
            ht.Add("@CurrentMonth", month);
            ht.Add("@CurrentYear", Year);


            dtone = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dtone.Rows.Count > 0)
            {
                GVListOfClients.DataSource = dtone;
                GVListOfClients.DataBind();
            }
            gve.Export("ESIReport.xls", this.GVListOfClients);
        }

        protected void lbtn_Export_PDF_Click(object sender, EventArgs e)
        {
            uint Fontsize = 10;
            string FontStyle = "Tahoma";

            DataTable dt = null;
            String Sqlqry = "";

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);
                return;
            }


            string date = "";


            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string strQry = "Select * from CompanyInfo   where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            string companyName1 = "Your Company Name";
            string companyAddress = "Your Company Address";
            string ESINO = "";

            if (compInfo.Rows.Count > 0)
            {
                companyName1 = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
                ESINO = compInfo.Rows[0]["ESINo"].ToString();
            }

            Sqlqry = "select ed.empid,ISNULL(ed.EmpFName,'')+' '+ISNULL(ed.empmname,'')+'  '+ISNULL(ed.emplname,'') as Fullname," +
                        "EmpESINo=case esi.EmpESINo when '0' then 'NA' else esi.EmpESINo end,EmpDtofjoining=case CONVERT(VARCHAR(10), " +
                        " ed.EmpDtofJoining, 103)  when '01/01/1900' then ' ' else CONVERT(VARCHAR(10), ed.EmpDtofJoining, 103)  end, " +
                        " round(sum((eps.NoOfDuties+eps.wo+eps.nhs)),0) as NoOfDuties,round(sum((eps.ots)),0) as ots," +
                        " (round(sum(isnull(eps.gross,0)),0)-round(sum(isnull(eps.WashAllowance,0)),0)) as grossamt," +
                        "round(sum(isnull(eps.OTAmt,0)),0) as Otamt,round(sum(eps.ESIWAGES),0) as ESIWAGES,round(sum(isnull(eps.ESIEmpr,0)),0) as Esiempr," +
                        " round(sum(isnull(eps.ESI,0)),0) as esi,(round(sum(isnull(eps.ESIEmpr,0)),0)+round(sum(isnull(eps.ESI,0)),0)) as TotalMonthlywages " +
                         " from EmpPaySheet Eps  inner join EmpDetails  Ed on ed.EmpId=eps.EmpId inner join EMPESICodes Esi " +
                         "on ed.EmpId=esi.Empid  " +
               " where  eps.month='" + month + Year.Substring(2, 2) + "' and eps.esi>0  group by esi.EmpESINo, " +
               " ed.EmpFName,ed.EmpMName,ed.EmpLName,ed.EmpId,ed.EmpDtofJoining  order by ed.empid";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;

            int j = 1;
            string esino = "";
            string Name = "";
            float esiwages = 0;
            float esi = 0;
            float esiempr = 0;
            float diff = 0;
            float pension = 0;
            float Totalesiwages = 0;
            float Totalesi = 0;
            float Totaldiff = 0;
            float Totalpension = 0;
            float epswages = 0;
            float Totalepswages = 0;
            float daysWkd = 0;
            float Totalesimpr = 0;

            if (dt.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A4);

                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");


                PdfPTable table = new PdfPTable(6);
                table.TotalWidth = 500f;
                table.HeaderRows = 4;
                table.LockedWidth = true;
                float[] width = new float[] { 1f, 3f, 5f, 2f, 3f, 4f };
                table.SetWidths(width);

                PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellspace.Colspan = 6;
                cellspace.Border = 0;
                cellspace.PaddingTop = 0;


                PdfPCell cellcompanyname = new PdfPCell(new Phrase(companyName1, FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                cellcompanyname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cellcompanyname.Colspan = 3;
                cellcompanyname.Border = 0;
                cellcompanyname.PaddingTop = 0;
                table.AddCell(cellcompanyname);

                PdfPCell EmplPF = new PdfPCell(new Phrase("Employer ESI No.: " + ESINO, FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                EmplPF.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                EmplPF.Colspan = 3;
                EmplPF.Border = 0;
                EmplPF.PaddingTop = 0;
                table.AddCell(EmplPF);


                PdfPCell PFReport = new PdfPCell(new Phrase("Monthly ESI report for the month of " + GetMonthName() + "/" + GetMonthOfYear(), FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
                PFReport.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                PFReport.Colspan = 6;
                PFReport.Border = 0;
                PFReport.PaddingTop = 20;
                PFReport.PaddingBottom = 20;
                table.AddCell(PFReport);




                /////

                PdfPCell CellSlNo = new PdfPCell(new Phrase("Sl.No.", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellSlNo.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CellSlNo.BorderWidthTop = 0.5f;
                CellSlNo.BorderWidthLeft = 0;
                CellSlNo.BorderWidthRight = 0;
                CellSlNo.BorderWidthBottom = 0;
                CellSlNo.PaddingTop = 0;
                table.AddCell(CellSlNo);


                PdfPCell CellPFNo = new PdfPCell(new Phrase("ESI No.", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellPFNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellPFNo.BorderWidthTop = 0.5f;
                CellPFNo.BorderWidthLeft = 0;
                CellPFNo.BorderWidthRight = 0;
                CellPFNo.BorderWidthBottom = 0;
                CellPFNo.PaddingTop = 0;
                table.AddCell(CellPFNo);

                PdfPCell CellName = new PdfPCell(new Phrase("Name of Member", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CellName.BorderWidthTop = 0.5f;
                CellName.PaddingTop = 0;
                CellName.BorderWidthLeft = 0;
                CellName.BorderWidthRight = 0;
                CellName.BorderWidthBottom = 0;
                table.AddCell(CellName);


                PdfPCell CellDaysWkd = new PdfPCell(new Phrase("Days Worked", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellDaysWkd.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CellDaysWkd.BorderWidthTop = 0.5f;
                CellDaysWkd.BorderWidthLeft = 0;
                CellDaysWkd.BorderWidthRight = 0;
                CellDaysWkd.BorderWidthBottom = 0;
                CellDaysWkd.PaddingTop = 0;
                table.AddCell(CellDaysWkd);



                PdfPCell CellESIEarnings = new PdfPCell(new Phrase("ESI Earnings", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellESIEarnings.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CellESIEarnings.Border = 0;
                CellESIEarnings.BorderWidthTop = 0.5f;
                CellESIEarnings.BorderWidthLeft = 0;
                CellESIEarnings.BorderWidthRight = 0;
                CellESIEarnings.BorderWidthBottom = 0;
                CellESIEarnings.PaddingTop = 0;
                table.AddCell(CellESIEarnings);


                PdfPCell CellESIContribution = new PdfPCell(new Phrase("ESI Contribution", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellESIContribution.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CellESIContribution.Border = 0;
                CellESIContribution.BorderWidthTop = 0.5f;
                CellESIContribution.PaddingTop = 0;
                table.AddCell(CellESIContribution);




                //////


                PdfPCell CellSlNo2 = new PdfPCell(new Phrase("(1)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellSlNo2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellSlNo2.BorderWidthLeft = 0;
                CellSlNo2.BorderWidthRight = 0;
                CellSlNo2.BorderWidthTop = 0;
                CellSlNo2.PaddingTop = 0;
                table.AddCell(CellSlNo2);


                PdfPCell CellPFNo2 = new PdfPCell(new Phrase("(2)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellPFNo2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellPFNo2.BorderWidthLeft = 0;
                CellPFNo2.BorderWidthRight = 0;
                CellPFNo2.BorderWidthTop = 0;
                CellPFNo2.PaddingTop = 0;
                table.AddCell(CellPFNo2);

                PdfPCell CellName2 = new PdfPCell(new Phrase("(3)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellName2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellName2.BorderWidthLeft = 0;
                CellName2.BorderWidthRight = 0;
                CellName2.BorderWidthTop = 0;
                CellName2.PaddingTop = 0;
                table.AddCell(CellName2);

                PdfPCell CellDaysWkd2 = new PdfPCell(new Phrase("(4)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellDaysWkd2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellDaysWkd2.BorderWidthLeft = 0;
                CellDaysWkd2.BorderWidthRight = 0;
                CellDaysWkd2.BorderWidthTop = 0;
                CellDaysWkd2.PaddingTop = 0;
                table.AddCell(CellDaysWkd2);


                PdfPCell CellESIEarnings2 = new PdfPCell(new Phrase("(5)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellESIEarnings2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellESIEarnings2.BorderWidthLeft = 0;
                CellESIEarnings2.BorderWidthRight = 0;
                CellESIEarnings2.BorderWidthTop = 0;
                CellESIEarnings2.PaddingTop = 0;
                table.AddCell(CellESIEarnings2);


                PdfPCell CellESIContribution2 = new PdfPCell(new Phrase("(6)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellESIContribution2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellESIContribution2.BorderWidthLeft = 0;
                CellESIContribution2.BorderWidthRight = 0;
                CellESIContribution2.PaddingTop = 0;
                CellESIContribution2.BorderWidthTop = 0;
                table.AddCell(CellESIContribution2);



                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    daysWkd = Convert.ToSingle(dt.Rows[k]["NoOfDuties"].ToString());
                    esino = dt.Rows[k]["EmpESINo"].ToString();
                    Name = dt.Rows[k]["Fullname"].ToString();
                    esiwages = Convert.ToSingle(dt.Rows[k]["esiwages"].ToString());
                    esi = Convert.ToSingle(dt.Rows[k]["esi"].ToString());
                    esiempr = Convert.ToSingle(dt.Rows[k]["esiempr"].ToString());

                    PdfPCell CellSlNo3 = new PdfPCell(new Phrase(j.ToString(), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CellSlNo3.HorizontalAlignment = 1; //0=Left, 1=Centre, 3=Right
                    CellSlNo3.Border = 0;
                    CellSlNo3.PaddingTop = 0;
                    table.AddCell(CellSlNo3);


                    PdfPCell CellPFNo3 = new PdfPCell(new Phrase(esino, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CellPFNo3.HorizontalAlignment = 1; //0=Left, 1=Centre, 3=Right
                    CellPFNo3.Border = 0;
                    CellPFNo3.PaddingTop = 0;
                    table.AddCell(CellPFNo3);

                    PdfPCell CellName3 = new PdfPCell(new Phrase(Name, FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CellName3.HorizontalAlignment = 0; //0=Left, 1=Centre, 3=Right
                    CellName3.Border = 0;
                    CellName3.BorderWidthRight = 0;
                    CellName3.PaddingTop = 0;
                    table.AddCell(CellName3);


                    PdfPCell CellDaysWkd3 = new PdfPCell(new Phrase(daysWkd.ToString("0.00"), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CellDaysWkd3.HorizontalAlignment = 2; //0=Left, 1=Centre, 3=Right
                    CellDaysWkd3.Border = 0;
                    CellDaysWkd3.PaddingTop = 0;
                    table.AddCell(CellDaysWkd3);


                    PdfPCell CellPFEarnings3 = new PdfPCell(new Phrase(esiwages.ToString("0.00"), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CellPFEarnings3.HorizontalAlignment = 2; //0=Left, 1=Centre, 3=Right
                    CellPFEarnings3.Border = 0;
                    CellPFEarnings3.PaddingTop = 0;
                    table.AddCell(CellPFEarnings3);
                    Totalesiwages += esiwages;

                    PdfPCell CellPFContribution3 = new PdfPCell(new Phrase(esi.ToString("0.00"), FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    CellPFContribution3.HorizontalAlignment = 2; //0=Left, 1=Centre, 3=Right
                    CellPFContribution3.Border = 0;
                    CellPFContribution3.PaddingTop = 0;
                    table.AddCell(CellPFContribution3);
                    Totalesi += esi;

                    Totalesimpr += esiempr;



                    j++;
                }



                PdfPCell CellSlNo4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellSlNo4.HorizontalAlignment = 1; //0=Left, 1=Centre, 4=Right
                CellSlNo4.BorderWidthLeft = 0;
                CellSlNo4.BorderWidthRight = 0;
                CellSlNo4.PaddingTop = 0;
                table.AddCell(CellSlNo4);


                PdfPCell CellPFNo4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellPFNo4.HorizontalAlignment = 1; //0=Left, 1=Centre, 4=Right
                CellPFNo4.BorderWidthLeft = 0;
                CellPFNo4.BorderWidthRight = 0;
                CellPFNo4.PaddingTop = 0;
                table.AddCell(CellPFNo4);


                PdfPCell CellName4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellName4.HorizontalAlignment = 2; //0=Left, 1=Centre, 4=Right
                CellName4.BorderWidthLeft = 0;
                CellName4.BorderWidthRight = 0;
                CellName4.PaddingTop = 0;
                table.AddCell(CellName4);

                PdfPCell CellDaysWorked4 = new PdfPCell(new Phrase("TOTAL\n\n", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellDaysWorked4.HorizontalAlignment = 2; //0=Left, 1=Centre, 4=Right
                CellDaysWorked4.BorderWidthLeft = 0;
                CellDaysWorked4.BorderWidthRight = 0;
                CellDaysWorked4.PaddingTop = 0;
                table.AddCell(CellDaysWorked4);

                PdfPCell CellPFEarnings4 = new PdfPCell(new Phrase(Totalesiwages.ToString("0.00"), FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellPFEarnings4.HorizontalAlignment = 2; //0=Left, 1=Centre, 4=Right
                CellPFEarnings4.BorderWidthLeft = 0;
                CellPFEarnings4.BorderWidthRight = 0;
                CellPFEarnings4.PaddingTop = 0;
                table.AddCell(CellPFEarnings4);


                PdfPCell CellPFContribution4 = new PdfPCell(new Phrase(Totalesi.ToString("0.00"), FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellPFContribution4.HorizontalAlignment = 2; //0=Left, 1=Centre, 4=Right
                CellPFContribution4.BorderWidthLeft = 0;
                CellPFContribution4.BorderWidthRight = 0;
                CellPFContribution4.PaddingTop = 0;
                table.AddCell(CellPFContribution4);




                //////////
                document.Add(table);

                PdfPTable table1 = new PdfPTable(5);
                table1.TotalWidth = 500f;
                table1.LockedWidth = true;
                float[] width1 = new float[] { 3f, 3f, 1f, 3f, 3f };
                table1.SetWidths(width1);


                PdfPCell CellPFEarnings5 = new PdfPCell(new Phrase("Employee Contribution", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellPFEarnings5.HorizontalAlignment = 2; //0=Left, 1=Centre, 5=Right
                CellPFEarnings5.Border = 0;
                CellPFEarnings5.BorderWidthRight = 0;
                CellPFEarnings5.PaddingTop = 0;
                CellPFEarnings5.Colspan = 2;
                table1.AddCell(CellPFEarnings5);

                PdfPCell CellEmpty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellEmpty.HorizontalAlignment = 0; //0=Left, 1=Centre, 6=Right
                CellEmpty.Border = 0;
                CellEmpty.BorderWidthRight = 0;
                CellEmpty.PaddingTop = 0;
                CellEmpty.Colspan = 1;
                table1.AddCell(CellEmpty);


                PdfPCell CellPFContribution5 = new PdfPCell(new Phrase(Totalesi.ToString("0.00"), FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellPFContribution5.HorizontalAlignment = 0; //0=Left, 1=Centre, 5=Right
                CellPFContribution5.Border = 0;
                CellPFContribution5.BorderWidthRight = 0;
                CellPFContribution5.PaddingTop = 0;
                CellPFContribution5.Colspan = 2;
                table1.AddCell(CellPFContribution5);


                ///////




                PdfPCell CellESIEarnings6 = new PdfPCell(new Phrase("Employer Contribution", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellESIEarnings6.HorizontalAlignment = 2; //0=Left, 1=Centre, 6=Right
                CellESIEarnings6.Border = 0;
                CellESIEarnings6.BorderWidthRight = 0;
                CellESIEarnings6.PaddingTop = 0;
                CellESIEarnings6.Colspan = 2;
                table1.AddCell(CellESIEarnings6);


                table1.AddCell(CellEmpty);


                PdfPCell CellESIContribution6 = new PdfPCell(new Phrase((Totalesiwages * 4.75 / 100).ToString("#") + ".00", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellESIContribution6.HorizontalAlignment = 0; //0=Left, 1=Centre, 6=Right
                CellESIContribution6.Border = 0;
                CellESIContribution6.BorderWidthRight = 0;
                CellESIContribution6.PaddingTop = 0;
                CellESIContribution6.Colspan = 2;
                table1.AddCell(CellESIContribution6);


                PdfPCell CellESITotal6 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellESITotal6.HorizontalAlignment = 2; //0=Left, 1=Centre, 6=Right
                CellESITotal6.Border = 0;
                CellESITotal6.BorderWidthRight = 0;
                CellESITotal6.BorderWidthTop = 0.5f;
                CellESITotal6.BorderWidthLeft = 0;
                CellESITotal6.BorderWidthBottom = 0.5f;
                CellESITotal6.PaddingTop = 0;
                CellESITotal6.Colspan = 2;
                table1.AddCell(CellESITotal6);

                PdfPCell CellEmpty6 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellEmpty6.HorizontalAlignment = 2; //0=Left, 1=Centre, 6=Right
                CellEmpty6.Border = 0;
                CellEmpty6.BorderWidthRight = 0;
                CellEmpty6.BorderWidthTop = 0.5f;
                CellEmpty6.BorderWidthLeft = 0;
                CellEmpty6.BorderWidthBottom = 0.5f;
                CellEmpty6.PaddingTop = 0;
                CellEmpty6.Colspan = 1;
                table1.AddCell(CellEmpty6);


                PdfPCell CellESIValue6 = new PdfPCell(new Phrase((Totalesi + (Totalesiwages * 4.75 / 100)).ToString("#") + ".00", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CellESIValue6.HorizontalAlignment = 0; //0=Left, 1=Centre, 6=Right
                CellESIValue6.Border = 0;
                CellESIValue6.BorderWidthRight = 0;
                CellESIValue6.BorderWidthTop = 0.5f;
                CellESIValue6.BorderWidthLeft = 0;
                CellESIValue6.BorderWidthBottom = 0.5f;
                CellESIValue6.PaddingTop = 0;
                CellESIValue6.Colspan = 2;
                table1.AddCell(CellESIValue6);

                document.Add(table1);


                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Consolidated.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }

        }

        protected void LoadEsibranches()
        {
            DataTable dtEsibranches = GlobalData.Instance.LoadEsibranches();
            if (dtEsibranches.Rows.Count > 0)
            {
                ddlEsibranch.DataValueField = "EsiBranchid";
                ddlEsibranch.DataTextField = "EsiBranchname";
                ddlEsibranch.DataSource = dtEsibranches;
                ddlEsibranch.DataBind();
            }
            ddlEsibranch.Items.Insert(0, "All");
        }

        protected void GVListOfClients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("class", "text");
            }
        }

        protected void lbtn_Export_esiregister_Click(object sender, EventArgs e)
        {
            GVListOfClients.DataSource = null;
            GVListOfClients.DataBind();

            LblResult.Text = "";
            DataTable dtone = null;
            string date = "";


            string SelectDate = string.Empty;
            if (txtmonth.Text.Trim().Length > 0)
            {
                SelectDate = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(SelectDate).Month.ToString();
            string Year = DateTime.Parse(SelectDate).Year.ToString();
            string esibranch = "";

            if (ddlEsibranch.SelectedIndex == 0)
            {
                esibranch = "%";
            }
            else
            {
                esibranch = ddlEsibranch.SelectedValue;
            }

            int monthdays = System.DateTime.DaysInMonth(int.Parse(Year), int.Parse(month));
            string Type = "ESIRegister";

            string SPName = "PFESIDetailsReport";
            Hashtable ht = new Hashtable();
            ht.Add("@month", month + Year.Substring(2, 2));
            ht.Add("@ESIBranch", esibranch);
            ht.Add("@MonthDays", monthdays);
            ht.Add("@Type", Type);
            ht.Add("@CurrentMonth", month);
            ht.Add("@CurrentYear", Year);


            dtone = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

            if (dtone.Rows.Count > 0)
            {
                string filename = "ESIRegister.xls";

                var products = dtone;
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("ESIRegister");
                var totalCols = products.Columns.Count;
                var totalRows = products.Rows.Count;

                for (var col = 1; col <= totalCols; col++)
                {
                    workSheet.Cells[1, col].Value = products.Columns[col - 1].ColumnName;
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                }
                for (var row = 1; row <= totalRows; row++)
                {
                    for (var col = 0; col < totalCols; col++)
                    {
                        workSheet.Cells[row + 1, col + 1].Value = products.Rows[row - 1][col];
                    }

                }
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment ;filename=\"" + filename + "\"");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}