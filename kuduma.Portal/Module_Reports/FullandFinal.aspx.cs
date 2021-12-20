using System;
using KLTS.Data;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class FullandFinal : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string Fontstyle = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                        
                       // FillEmployeesList();
                        //LoadNames();
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

            }

        }
        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }
     

        public void Cleardata()
        {


        }
        protected void btnSearchFullandFinal_Click(object sender, EventArgs e)
        {
            #region for validation
            if(txtemplyid.Text.Trim().Length==0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert()", "alert('Please select employee ID/Name')", true);
                return;
            }
#endregion
            #region Begin Code For Declaration

            DataTable dt = null;
            Hashtable HtFullandFinal = new Hashtable();
            var SPName = "";
            var Empid = "";

            #endregion Begin Code For Declaration

            #region Begin Code Assign Values To the variables

            SPName = "EmpFullandFinalSettlement";
            Empid = txtemplyid.Text; ;

            #endregion Begin Code Assign Values To the variables

            #region  Begin Code For HT Parameters

            HtFullandFinal.Add("@empid", Empid);

            #endregion End  Code For HT Parameters

            #region  Begin Code For Calling Stored Procedure

            dt =config.ExecuteAdaptorAsyncWithParams(SPName, HtFullandFinal).Result;

            #endregion End  Code For Calling Stored Procedure

            #region  Begin Code For Calling variables

            string emplname = "";
            string emplid = "";
            string Fathername = "";
            string DOJ = "";
            string DOL = "";
            string Designation = "";
            string loanno = "-";
            string LoandDt = "";
            string LoanDt = "-";
            decimal LoanAmt = 0;
            int NoofInstalments = 0;
            string TypeofLoan = "-";
            string Status = "-";
            decimal DueAmt = 0;

            #endregion  Begin Code For Calling variables

            MemoryStream ms = new MemoryStream();
            if (dt.Rows.Count > 0)
            {
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                document.AddTitle("FaMS");
                document.AddAuthor("DIYOS");
                document.AddSubject("Wage Slips");
                document.AddKeywords("Keyword1, keyword2, …");//
                string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                string companyName = "Your Company Name";
                string companyAddress = "Your Company Address";
                if (compInfo.Rows.Count > 0)
                {
                    companyName = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }
                float forConvert = 0;
                int slipsCount = 0;

                string monthname = "";



                strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid ";
                string pfNo = "";
                string esiNo = "";
                DataTable PfTable = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                if (PfTable.Rows.Count > 0)
                {
                    pfNo = PfTable.Rows[0]["EmpEpfNo"].ToString();
                    esiNo = PfTable.Rows[0]["EmpESINo"].ToString();
                }

                PdfPTable Temptable = new PdfPTable(7);
                Temptable.TotalWidth = 505f;
                Temptable.LockedWidth = true;
                float[] tempwidths = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f };
                Temptable.SetWidths(tempwidths);

                PdfPTable Maintable = new PdfPTable(7);
                Maintable.TotalWidth = 500f;
                Maintable.LockedWidth = true;
                float[] width = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f };
                Maintable.SetWidths(width);
                uint FONT_SIZE = 9;

                #region  Table Headings



                PdfPCell employerName = new PdfPCell(new Phrase(companyName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLACK)));
                employerName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                employerName.Colspan = 7;
                employerName.BorderColor = new BaseColor(204, 204, 204);
                employerName.PaddingTop = 3;
                employerName.PaddingBottom = 4;
                Maintable.AddCell(employerName);

                PdfPCell head2 = new PdfPCell(new Phrase("Full and Final Settlement", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK)));
                head2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                head2.Colspan = 7;
                head2.BorderColor = new BaseColor(204, 204, 204);
                head2.PaddingTop = 3;
                head2.PaddingBottom = 5;
                Maintable.AddCell(head2);



                //1
                PdfPCell EmpIdhead = new PdfPCell(new Phrase("Employee ID", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                EmpIdhead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //EmpIdhead.Border = 0;
                EmpIdhead.PaddingTop = 3;
                EmpIdhead.PaddingBottom = 3;
                EmpIdhead.BorderWidthRight = 0;
                EmpIdhead.BorderWidthLeft = 0.5f;
                EmpIdhead.BorderWidthBottom = 0.5f;
                EmpIdhead.BorderWidthTop = 0;
                EmpIdhead.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(EmpIdhead);


                emplid = dt.Rows[0]["empid"].ToString();
                PdfPCell EmpId = new PdfPCell(new Phrase(emplid, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                EmpId.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // EmpId.Border = 0;
                EmpId.PaddingTop = 3;
                EmpId.PaddingBottom = 3;
                EmpId.BorderWidthRight = 0;
                EmpId.BorderWidthLeft = 0;
                EmpId.BorderWidthBottom = 0.5f;
                EmpId.Colspan = 3;
                EmpId.BorderWidthTop = 0;
                EmpId.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(EmpId);

                PdfPCell fnamehead = new PdfPCell(new Phrase("Father Name", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                fnamehead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // fnamehead.Border = 0;
                fnamehead.PaddingTop = 3;
                fnamehead.PaddingBottom = 3;
                fnamehead.BorderWidthRight = 0;
                fnamehead.BorderWidthLeft = 0.5f;
                fnamehead.BorderWidthBottom = 0.5f;
                fnamehead.BorderWidthTop = 0;

                fnamehead.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(fnamehead);

                Fathername = dt.Rows[0]["EmpFatherName"].ToString();
                PdfPCell fname = new PdfPCell(new Phrase(Fathername.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                fname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // fname.Border = 0;
                fname.PaddingTop = 3;
                fname.BorderWidthRight = 0.5f;
                fname.BorderWidthLeft = 0;
                fname.BorderWidthBottom = 0.5f;
                fname.BorderWidthTop = 0;
                fname.Colspan = 2;
                fname.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(fname);


                //PdfPCell PfNohead = new PdfPCell(new Phrase("EPF NO.", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                //PfNohead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //PfNohead.PaddingTop = 3;
                //PfNohead.PaddingBottom = 3;
                //PfNohead.BorderWidthLeft = 0.5f;
                //PfNohead.BorderWidthRight = 0;
                //PfNohead.BorderWidthBottom = 0.5f;
                //PfNohead.BorderWidthTop = 0;
                //PfNohead.BorderColor = new BaseColor(204, 204, 204);
                //Maintable.AddCell(PfNohead);

                //PdfPCell PfNo = new PdfPCell(new Phrase(pfNo.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                //PfNo.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //// PfNo.Border = 0;
                //PfNo.BorderWidthRight = 0f;
                //PfNo.BorderWidthLeft = 0;
                //PfNo.BorderWidthBottom = 0.5f;
                //PfNo.BorderWidthTop = 0;
                //PfNo.BorderColor = new BaseColor(204, 204, 204);
                //PfNo.PaddingTop = 3;
                //Maintable.AddCell(PfNo);


                //2

                PdfPCell EmpNamehead = new PdfPCell(new Phrase("Emp Name", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                EmpNamehead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                EmpNamehead.BorderWidthLeft = 0.5f;
                EmpNamehead.BorderWidthRight = 0;
                EmpNamehead.BorderWidthBottom = 0.5f;
                EmpNamehead.BorderWidthTop = 0;
                EmpNamehead.PaddingTop = 3;
                EmpNamehead.PaddingBottom = 3;
                EmpNamehead.BorderColor = new BaseColor(204, 204, 204);
                EmpNamehead.PaddingBottom = 3;
                Maintable.AddCell(EmpNamehead);


                emplname = dt.Rows[0]["Fullname"].ToString();
                PdfPCell EmpName = new PdfPCell(new Phrase(emplname.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                EmpName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                EmpName.BorderWidthLeft = 0;
                EmpName.BorderWidthRight = 0f;
                EmpName.BorderWidthBottom = 0.5f;
                EmpName.BorderWidthTop = 0;
                EmpName.BorderColor = new BaseColor(204, 204, 204);
                EmpName.PaddingTop = 3;
                EmpName.PaddingBottom = 3;
                EmpName.Colspan = 3;
                Maintable.AddCell(EmpName);



                //PdfPCell ESINohead = new PdfPCell(new Phrase("ESI NO.", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                //ESINohead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //// ESINohead.Border = 0;
                //ESINohead.PaddingTop = 3;
                //ESINohead.PaddingBottom = 3;
                //ESINohead.BorderWidthRight = 0f;
                //ESINohead.BorderWidthLeft = 0.5f;
                //ESINohead.BorderWidthBottom = 0.5f;
                //ESINohead.BorderWidthTop = 0;
                //ESINohead.BorderColor = new BaseColor(204, 204, 204);
                ////Maintable.AddCell(ESINohead);


                //PdfPCell ESINo = new PdfPCell(new Phrase(esiNo.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                //ESINo.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //// ESINo.Border = 0;
                //ESINo.BorderWidthRight = 0;
                //ESINo.BorderWidthLeft = 0;
                //ESINo.BorderWidthBottom = 0.5f;
                //ESINo.BorderWidthTop = 0;
                //ESINo.BorderColor = new BaseColor(204, 204, 204);
                //ESINo.PaddingTop = 3;
                //ESINo.PaddingBottom = 3;
                //Maintable.AddCell(ESINo);


                PdfPCell CDOJHead = new PdfPCell(new Phrase("Dt of Joining ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CDOJHead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //CDOJHead.Border = 0;
                CDOJHead.BorderWidthLeft = 0.5f;
                CDOJHead.BorderWidthRight = 0;
                CDOJHead.BorderWidthBottom = 0.5f;
                CDOJHead.BorderWidthTop = 0;

                CDOJHead.BorderColor = new BaseColor(204, 204, 204);
                CDOJHead.PaddingTop = 3;
                Maintable.AddCell(CDOJHead);

                DOJ = dt.Rows[0]["EmpDtofJoining"].ToString();
                if (DOJ == "01/01/1900")
                {
                    DOJ = "";

                }

                PdfPCell CDOJ = new PdfPCell(new Phrase(DOJ, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                CDOJ.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // CDOJ.Border = 0;
                CDOJ.BorderWidthLeft = 0f;
                CDOJ.BorderWidthRight = 0.5f;
                CDOJ.BorderWidthBottom = 0.5f;
                CDOJ.BorderWidthTop = 0;
                CDOJ.BorderColor = new BaseColor(204, 204, 204);
                CDOJ.PaddingTop = 3;
                CDOJ.PaddingBottom = 3;
                CDOJ.Colspan = 2;
                Maintable.AddCell(CDOJ);



                //3

                PdfPCell designhead = new PdfPCell(new Phrase("Designation", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                designhead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // designhead.Border = 0;
                designhead.PaddingTop = 3;
                designhead.PaddingBottom = 3;
                designhead.BorderWidthRight = 0;
                designhead.BorderWidthLeft = 0.5f;
                designhead.BorderWidthBottom = 0.5f;
                designhead.BorderWidthTop = 0;
                designhead.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(designhead);

                Designation = dt.Rows[0]["Design"].ToString();
                PdfPCell design = new PdfPCell(new Phrase(Designation, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                design.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // design.Border = 0;
                design.PaddingTop = 3;
                design.BorderWidthRight = 0;
                design.BorderWidthLeft = 0;
                design.BorderWidthBottom = 0.5f;
                design.BorderWidthTop = 0;
                design.Colspan = 3;
                design.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(design);

                //PdfPCell AccountNohead = new PdfPCell(new Phrase("Bank Acc No.", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                //AccountNohead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                ////AccountNohead.Border = 0;
                //AccountNohead.PaddingTop = 3;
                //AccountNohead.PaddingBottom = 3;
                //AccountNohead.BorderWidthRight = 0;
                //AccountNohead.BorderWidthLeft = 0.5f;
                //AccountNohead.BorderWidthBottom = 0.5f;
                //AccountNohead.BorderWidthTop = 0;
                //AccountNohead.BorderColor = new BaseColor(204, 204, 204);
                //Maintable.AddCell(AccountNohead);


                //PdfPCell AccountNo = new PdfPCell(new Phrase(dt.Rows[i]["EmpBankAcNo"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                //AccountNo.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                ////AccountNo.Border = 0;
                //AccountNo.PaddingTop = 3;
                //AccountNo.PaddingBottom = 3;
                //AccountNo.BorderWidthRight = 0f;
                //AccountNo.BorderWidthLeft = 0;
                //AccountNo.BorderWidthBottom = 0.5f;
                //AccountNo.BorderWidthTop = 0;
                //AccountNo.BorderColor = new BaseColor(204, 204, 204);
                //Maintable.AddCell(AccountNo);


                PdfPCell CDOLHead = new PdfPCell(new Phrase("Dt of Leaving", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CDOLHead.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //CDOLHead.Border = 0;
                CDOLHead.PaddingTop = 3;
                CDOLHead.BorderWidthRight = 0;
                CDOLHead.BorderWidthLeft = 0.5f;
                CDOLHead.BorderWidthBottom = 0.5f;
                CDOLHead.BorderWidthTop = 0;

                CDOLHead.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(CDOLHead);

                DOL = dt.Rows[0]["EmpDtofLeaving"].ToString();
                if (DOL == "01/01/1900")
                {
                    DOL = "";

                }
                PdfPCell CDOL = new PdfPCell(new Phrase(DOL, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                CDOL.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //CDOL.Border = 0;
                CDOL.BorderWidthRight = 0.5f;
                CDOL.BorderWidthLeft = 0f;
                CDOL.BorderWidthBottom = 0.5f;
                CDOL.BorderWidthTop = 0;
                CDOL.BorderColor = new BaseColor(204, 204, 204);
                CDOL.Colspan = 2;
                CDOL.PaddingTop = 3;
                Maintable.AddCell(CDOL);

                PdfPCell Emoluments = new PdfPCell(new Phrase("Loans Taken ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Emoluments.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Emoluments.Colspan = 7;
                Emoluments.PaddingTop = 3;
                Emoluments.PaddingBottom = 3;
                Emoluments.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(Emoluments);


                PdfPCell CLoanno = new PdfPCell(new Phrase("Loan ID", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CLoanno.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CLoanno.PaddingTop = 3;
                CLoanno.PaddingBottom = 3;
                CLoanno.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(CLoanno);

                PdfPCell CLoanDt = new PdfPCell(new Phrase("Loan Dt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CLoanDt.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CLoanDt.PaddingTop = 3;
                CLoanDt.PaddingBottom = 3;
                CLoanDt.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(CLoanDt);

                PdfPCell CLoanAmt = new PdfPCell(new Phrase("Loan Amount", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CLoanAmt.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CLoanAmt.PaddingTop = 3;
                CLoanAmt.PaddingBottom = 3;
                CLoanAmt.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(CLoanAmt);

                PdfPCell CNoofInstalments = new PdfPCell(new Phrase("No.of Instalments", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CNoofInstalments.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CNoofInstalments.PaddingTop = 3;
                CNoofInstalments.PaddingBottom = 3;
                CNoofInstalments.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(CNoofInstalments);

                PdfPCell CTypeofLoan = new PdfPCell(new Phrase("Type of Loan", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CTypeofLoan.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CTypeofLoan.PaddingTop = 3;
                CTypeofLoan.PaddingBottom = 3;
                CTypeofLoan.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(CTypeofLoan);

                PdfPCell CStatus = new PdfPCell(new Phrase("Status", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CStatus.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CStatus.PaddingTop = 3;
                CStatus.PaddingBottom = 3;
                CStatus.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(CStatus);

                PdfPCell Amount = new PdfPCell(new Phrase("Due Amount", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Amount.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Amount.PaddingTop = 3;
                Amount.PaddingBottom = 3;
                Amount.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(Amount);

                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    loanno = dt.Rows[i]["LoanNo"].ToString();
                    PdfPCell CLoanno1 = new PdfPCell(new Phrase(loanno, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CLoanno1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CLoanno1.PaddingTop = 3;
                    CLoanno1.PaddingBottom = 3;
                    CLoanno1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CLoanno1);

                    LoanDt = dt.Rows[i]["LoanIssedDate"].ToString();
                    PdfPCell CLoanDt1 = new PdfPCell(new Phrase(LoanDt, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CLoanDt1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CLoanDt1.PaddingTop = 3;
                    CLoanDt1.PaddingBottom = 3;
                    CLoanDt1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CLoanDt1);

                    LoanAmt = Convert.ToDecimal(dt.Rows[i]["LoanAmount"].ToString());
                    PdfPCell CLoanAmt1 = new PdfPCell(new Phrase(LoanAmt.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CLoanAmt1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CLoanAmt1.PaddingTop = 3;
                    CLoanAmt1.PaddingBottom = 3;
                    CLoanAmt1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CLoanAmt1);

                    NoofInstalments = Convert.ToInt32(dt.Rows[i]["NoInstalments"].ToString());
                    PdfPCell CNoofInstalments1 = new PdfPCell(new Phrase(NoofInstalments.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CNoofInstalments1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CNoofInstalments1.PaddingTop = 3;
                    CNoofInstalments1.PaddingBottom = 3;
                    CNoofInstalments1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CNoofInstalments1);

                    TypeofLoan = dt.Rows[i]["TypeOfLoan"].ToString();
                    PdfPCell CTypeofLoan1 = new PdfPCell(new Phrase(TypeofLoan.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CTypeofLoan1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CTypeofLoan1.PaddingTop = 3;
                    CTypeofLoan1.PaddingBottom = 3;
                    CTypeofLoan1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CTypeofLoan1);

                    Status = dt.Rows[i]["LoanStatus"].ToString();
                    PdfPCell CStatus1 = new PdfPCell(new Phrase(Status.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CStatus1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CStatus1.PaddingTop = 3;
                    CStatus1.PaddingBottom = 3;
                    CStatus1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CStatus1);

                    DueAmt = Convert.ToDecimal(dt.Rows[i]["DueAmount"].ToString());
                    PdfPCell Amount1 = new PdfPCell(new Phrase(DueAmt.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    Amount1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Amount1.PaddingTop = 3;
                    Amount1.PaddingBottom = 3;
                    Amount1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(Amount1);


                    PdfPCell Tempcell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, BaseColor.BLACK)));
                    Tempcell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Tempcell.Colspan = 7;
                    Tempcell.Border = 0;
                    Tempcell.PaddingTop = 1;
                    //Tempcell.PaddingBottom = 4;
                    Maintable.AddCell(Tempcell);
                }




                string Data = "select top(1) month,eps.empid,d.Design,eps.ClientId,C.ClientName,eps.NoOfDuties,eps.ActualAmount from EmpPaySheet eps " +
                               "inner join Clients c on c.ClientId=eps.ClientId inner join Designations d on d.DesignId=eps.Desgn " +
                               "where EmpId='" + txtemplyid.Text+ "' order by Timings desc";
                DataTable dtlastworked = config.ExecuteAdaptorAsyncWithQueryParams(Data).Result;

                if (dtlastworked.Rows.Count > 0)
                {
                    PdfPCell CLastWorked = new PdfPCell(new Phrase("Last Worked at ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CLastWorked.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CLastWorked.Colspan = 7;
                    CLastWorked.PaddingTop = 3;
                    CLastWorked.PaddingBottom = 3;
                    CLastWorked.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CLastWorked);


                    PdfPCell CClientid = new PdfPCell(new Phrase("Client ID", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CClientid.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CClientid.PaddingTop = 3;
                    CClientid.PaddingBottom = 3;
                    CClientid.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CClientid);

                    PdfPCell CClientname = new PdfPCell(new Phrase("Client Name", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CClientname.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CClientname.PaddingTop = 3;
                    CClientname.PaddingBottom = 3;
                    CClientname.BorderColor = new BaseColor(204, 204, 204);
                    CClientname.Colspan = 2;
                    Maintable.AddCell(CClientname);

                    PdfPCell CMonth = new PdfPCell(new Phrase("Month", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CMonth.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CMonth.PaddingTop = 3;
                    CMonth.PaddingBottom = 3;
                    CMonth.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CMonth);

                    PdfPCell Cdesign = new PdfPCell(new Phrase("Design", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    Cdesign.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cdesign.PaddingTop = 3;
                    Cdesign.PaddingBottom = 3;
                    Cdesign.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(Cdesign);

                    PdfPCell CAttendance = new PdfPCell(new Phrase("Duties", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CAttendance.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CAttendance.PaddingTop = 3;
                    CAttendance.PaddingBottom = 3;
                    CAttendance.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CAttendance);

                    PdfPCell CAcAmount = new PdfPCell(new Phrase("Actual Amount", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CAcAmount.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CAcAmount.PaddingTop = 3;
                    CAcAmount.PaddingBottom = 3;
                    CAcAmount.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CAcAmount);

                    PdfPCell CClientid1 = new PdfPCell(new Phrase(dtlastworked.Rows[0]["clientid"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CClientid1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CClientid1.PaddingTop = 3;
                    CClientid1.PaddingBottom = 3;
                    CClientid1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CClientid1);

                    PdfPCell CClientname1 = new PdfPCell(new Phrase(dtlastworked.Rows[0]["clientname"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CClientname1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CClientname1.PaddingTop = 3;
                    CClientname1.PaddingBottom = 3;
                    CClientname1.BorderColor = new BaseColor(204, 204, 204);
                    CClientname1.Colspan = 2;
                    Maintable.AddCell(CClientname1);

                    #region for getting monthname and year

                    string month = (dtlastworked.Rows[0]["month"].ToString());
                    string year = (dtlastworked.Rows[0]["month"].ToString());

                    if (month.Length.ToString() == "4")
                    {
                        month = month.Substring(0, 2);
                        year = year.Substring(2, 2);
                    }
                    else if (month.Length.ToString() == "3")
                    {
                        month = month.Substring(0, 1);
                        year = year.Substring(1, 2);

                    }

                    if (month == "1")
                    {
                        monthname = "Jan";
                    }
                    if (month == "2")
                    {
                        monthname = "Feb";
                    }
                    if (month == "3")
                    {
                        monthname = "Mar";

                    }
                    if (month == "4")
                    {
                        monthname = "Apr";
                    }
                    if (month == "5")
                    {
                        monthname = "May";
                    }
                    if (month == "6")
                    {
                        monthname = "Jun";
                    }
                    if (month == "7")
                    {
                        monthname = "Jul";
                    }
                    if (month == "8")
                    {
                        monthname = "Aug";
                    }
                    if (month == "9")
                    {
                        monthname = "Sep";
                    }
                    if (month == "10")
                    {
                        monthname = "Oct";
                    }
                    if (month == "11")
                    {
                        monthname = "Nov";
                    }
                    if (month == "12")
                    {
                        monthname = "Dec";
                    }

                    #endregion for getting monthname and year

                    PdfPCell CMonth1 = new PdfPCell(new Phrase(monthname + " - " + year, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CMonth1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CMonth1.PaddingTop = 3;
                    CMonth1.PaddingBottom = 3;
                    CMonth1.BorderColor = new BaseColor(204, 204, 204);

                    Maintable.AddCell(CMonth1);

                    PdfPCell Cdesign1 = new PdfPCell(new Phrase(dtlastworked.Rows[0]["design"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    Cdesign1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Cdesign1.PaddingTop = 3;
                    Cdesign1.PaddingBottom = 3;
                    Cdesign1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(Cdesign1);

                    PdfPCell CAttendance1 = new PdfPCell(new Phrase(dtlastworked.Rows[0]["NoOfDuties"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CAttendance1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CAttendance1.PaddingTop = 3;
                    CAttendance1.PaddingBottom = 3;
                    CAttendance1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CAttendance1);

                    PdfPCell CAcAmount1 = new PdfPCell(new Phrase(dtlastworked.Rows[0]["ActualAmount"].ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    CAcAmount1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    CAcAmount1.PaddingTop = 3;
                    CAcAmount1.PaddingBottom = 3;
                    CAcAmount1.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CAcAmount1);

                    PdfPCell Tempcell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, BaseColor.BLACK)));
                    Tempcell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    Tempcell.Colspan = 7;
                    Tempcell.Border = 0;
                    Tempcell.PaddingTop = 1;
                    //Tempcell.PaddingBottom = 4;
                    Maintable.AddCell(Tempcell);
                }

                PdfPCell CRemarks = new PdfPCell(new Phrase("Remarks :", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CRemarks.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CRemarks.PaddingTop = 3;
                CRemarks.PaddingBottom = 3;
                CRemarks.Colspan = 7;
                CRemarks.BorderColor = new BaseColor(204, 204, 204);
                Maintable.AddCell(CRemarks);

                for (int j = 0; j < 5; j++)
                {
                    PdfPCell CRemarksdesc = new PdfPCell(new Phrase(" ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                    CRemarksdesc.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    CRemarksdesc.PaddingTop = 5;
                    CRemarksdesc.PaddingBottom = 5;
                    CRemarksdesc.Colspan = 7;
                    CRemarksdesc.BorderColor = new BaseColor(204, 204, 204);
                    Maintable.AddCell(CRemarksdesc);


                }

                PdfPCell Tempcell1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, BaseColor.BLACK)));
                Tempcell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Tempcell1.Colspan = 7;
                Tempcell1.Border = 0;
                Tempcell1.PaddingTop = 1;
                //Tempcell.PaddingBottom = 4;
                Maintable.AddCell(Tempcell1);

                PdfPCell childTable1 = new PdfPCell(Maintable);
                //childTable1.Border = 0;
                childTable1.Colspan = 7;
                childTable1.PaddingTop = 3;
                childTable1.BorderColor = new BaseColor(204, 204, 204);
                childTable1.HorizontalAlignment = 0;
                Temptable.AddCell(childTable1);




                PdfPCell CEmpty = new PdfPCell(new Phrase(" ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CEmpty.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CEmpty.Border = 0;
                CEmpty.Colspan = 7;
                Temptable.AddCell(CEmpty);

                document.Add(Temptable);

                #endregion





                document.Close();

                string filename = "F&F " + txtemplyid.Text + "-" + txtFname.Text + ".pdf";
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }


        }

        protected void txtemplyid_TextChanged(object sender, EventArgs e)
        {
            GetEmpName();
        }

        protected void txtFname_TextChanged(object sender, EventArgs e)
        {
            Getempid();
        }

        protected void Getempid()
        {

            string Sqlqry = "select   Empid +' - '+ OlEmpid empid from empdetails where empfname+' '+empmname+' '+emplname like '%" + txtFname.Text + "%' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtemplyid.Text = dt.Rows[0]["empid"].ToString();

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

        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname from empdetails where empid='" + txtemplyid.Text+ "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtFname.Text = dt.Rows[0]["empname"].ToString();

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

    }
}