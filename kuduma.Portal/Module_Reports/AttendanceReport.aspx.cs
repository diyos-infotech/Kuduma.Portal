using System;
using System.Web.UI;
using System.Data;
using KLTS.Data;
using System.Web.UI.HtmlControls;
using Kuduma.Portal.DAL;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Drawing;
using iTextSharp.text.pdf;
using OfficeOpenXml.Packaging;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text;
using System.Text;

namespace Kuduma.Portal.Module_Reports
{
    public partial class AttendanceReport : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string CmpIDPrefix = "";
        string BranchID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {

                    Loadclientids();
                    Loadclientnames();
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

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void Loadclientids()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable dtids = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (dtids.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dtids;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "--Select--");
            ddlclientid.Items.Insert(1, "All");
        }

        protected void ExcelNumbers()
        {
            string date = string.Empty;
            string month = "";
            string Year = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            month = DateTime.Parse(date).Month.ToString();
            Year = DateTime.Parse(date).Year.ToString();

            string newmonth = month + Year.Substring(2, 2);

            string strQry = "select distinct(ExcelNumber)from EmpAttendance where MONTH ='" + newmonth + "' and ExcelNumber is not null order by ExcelNumber Desc";
            DataTable dtids = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            if (dtids.Rows.Count > 0)
            {
                ddlexcelno.DataValueField = "ExcelNumber";
                ddlexcelno.DataTextField = "ExcelNumber";
                ddlexcelno.DataSource = dtids;
                ddlexcelno.DataBind();
            }
            ddlexcelno.Items.Insert(0, "--Select--");
        }

        protected void Textbind()
        {
            string date = string.Empty;
            string month = "";
            string Year = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            month = DateTime.Parse(date).Month.ToString();
            Year = DateTime.Parse(date).Year.ToString();

            string newmonth = month + Year.Substring(2, 2);

            string ExcelNo = ddlexcelno.SelectedValue;

            string strQry = "select top 1(DateCreated) from EmpAttendance where MONTH = '" + newmonth + "' and ExcelNumber = '" + ExcelNo + "'order by DateCreated Desc";
            DataTable dtids = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            string time = dtids.Rows[0]["DateCreated"].ToString();
            txttime.Text = time.Substring(0, 15);


        }

        protected void Loadclientnames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable dtnames = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (dtnames.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = dtnames;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "--Select--");
            ddlcname.Items.Insert(1, "All");
        }

        protected void Fillclientname()
        {
            if (ddlclientid.SelectedIndex > 1)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            if (ddlclientid.SelectedIndex == 1)
            {
                ddlcname.SelectedIndex = 1;
            }

            if (ddlclientid.SelectedIndex == 0)
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void Fillclientid()
        {
            if (ddlcname.SelectedIndex > 1)
            {
                ddlclientid.SelectedValue = ddlcname.SelectedValue;
            }
            if (ddlcname.SelectedIndex == 1)
            {
                ddlclientid.SelectedIndex = 1;
            }

            if (ddlcname.SelectedIndex == 0)
            {
                ddlclientid.SelectedIndex = 0;
            }

        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVAttendance.DataSource = null;
            GVAttendance.DataBind();
            if (ddlclientid.SelectedIndex > 0)
            {
                Fillclientname();

            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }

        }

        protected void ddlclientname_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVAttendance.DataSource = null;
            GVAttendance.DataBind();
            if (ddlcname.SelectedIndex > 0)
            {
                Fillclientid();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select The Month')", true);
                return;
            }


            string date = string.Empty;
            string month = "";
            string Year = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            DataTable dt = null;

            GVAttendance.DataSource = null;
            GVAttendance.DataBind();


            string Clientid = ddlclientid.SelectedValue;

            if (ddlclientid.SelectedIndex == 1)
            {
                Clientid = "%";
            }

            month = DateTime.Parse(date).Month.ToString();
            Year = DateTime.Parse(date).Year.ToString();

            string SPName = "AttendanceReport";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientId", Clientid);
            ht.Add("@month", month + Year.Substring(2, 2));


            dt = config.ExecuteAdaptorWithParams(SPName, ht).Result;

            if (dt.Rows.Count > 0)
            {
                GVAttendance.DataSource = dt;
                GVAttendance.DataBind();
            }
            else
            {
                GVAttendance.DataSource = null;
                GVAttendance.DataBind();
            }

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select The Month')", true);
                return;
            }


            string date = string.Empty;
            string month = "";
            string Year = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            DataTable dt = null;

            GVAttendance.DataSource = null;
            GVAttendance.DataBind();


            string Clientid = ddlclientid.SelectedValue;

            if (ddlclientid.SelectedIndex == 1)
            {
                Clientid = "%";
            }

            month = DateTime.Parse(date).Month.ToString();
            Year = DateTime.Parse(date).Year.ToString();
            string SPName = "";
            Hashtable ht = new Hashtable();

            if (rbexcel.Checked == true)
            {
                SPName = "AttendanceReportExcelnumbers";
                ht.Add("@ExcelNumber", ddlexcelno.SelectedValue);
                ht.Add("@month", month + Year.Substring(2, 2));
            }
            else
            {
                SPName = "AttendanceReport";
                ht.Add("@ClientId", Clientid);
                ht.Add("@month", month + Year.Substring(2, 2));
            }

            dt = config.ExecuteAdaptorWithParams(SPName, ht).Result;

            if (dt.Rows.Count > 0)
            {
                GVAttendance.Visible = false;
                GVAttendance.DataSource = dt;
                GVAttendance.DataBind();
                if (rbexcel.Checked == true)
                {
                    DataSet ds = getDataSetExportToExcel();

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var style = XLWorkbook.DefaultStyle;

                        foreach (DataTable dts in ds.Tables)
                        {
                            wb.Worksheets.Add(dts);
                        }
                        wb.Style.Fill.BackgroundColor = XLColor.Black;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename= Attendence Report Excel No Wise.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    gve.Export("AttendanceReport.xls", this.GVAttendance);
                }

            }
            else
            {
                GVAttendance.DataSource = null;
                GVAttendance.DataBind();
            }
        }

        protected void GVAttendance_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text == "99999")
                {
                    e.Row.Cells[0].Text = "";
                    e.Row.Font.Bold = true;
                }
            }
        }

        public class PageEventHelperPageNo : PdfPageEventHelper
        {
            PdfContentByte cb;
            PdfTemplate template;

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                int pageN = writer.PageNumber;
                String text = "Page " + pageN.ToString() + " of ";
                float len = bf.GetWidthPoint(text, 8);


                iTextSharp.text.Rectangle pageSize = document.PageSize;

                cb.SetRGBColorFill(100, 100, 100);

                cb.BeginText();
                cb.SetFontAndSize(bf, 8);

                cb.SetTextMatrix(document.RightMargin + 25, pageSize.GetBottom(document.BottomMargin - 10));
                cb.ShowText(text);

                cb.EndText();

                cb.AddTemplate(template, document.RightMargin + len, pageSize.GetBottom(document.BottomMargin - 10));
            }

            int totalpgcount = 0;
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {

                base.OnCloseDocument(writer, document);

                template.BeginText();
                template.SetFontAndSize(bf, 8);
                template.SetTextMatrix(25, 0);
                template.SetGrayStroke(11);
                totalpgcount = writer.PageNumber;
                totalpgcount = totalpgcount - 1;
                template.ShowText("" + totalpgcount);
                template.EndText();

            }
        }

        protected void btndownloadpdf_Click(object sender, EventArgs e)
        {

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select The Month')", true);
                return;
            }


            string date = string.Empty;
            string month = "";
            string Year = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            DataTable dt = null;

            GVAttendance.DataSource = null;
            GVAttendance.DataBind();
            string Clientname = "";

            string Clientid = ddlclientid.SelectedValue;

            if (ddlclientid.SelectedIndex == 1)
            {
                Clientid = "%";
            }

            month = DateTime.Parse(date).Month.ToString();
            Year = DateTime.Parse(date).Year.ToString();

            string SPName = "";
            Hashtable ht = new Hashtable();

            if (rbclient.Checked == true)
            {
                SPName = "AttendanceReport";
                ht.Add("@ClientId", Clientid);
                ht.Add("@month", month + Year.Substring(2, 2));
            }
            else
            {
                SPName = "AttendanceReportExcelnumbers";
                ht.Add("@ExcelNumber", ddlexcelno.SelectedValue);
                ht.Add("@month", month + Year.Substring(2, 2));
            }


            dt = config.ExecuteAdaptorWithParams(SPName, ht).Result;
            DataTable DtGroup = dt.DefaultView.ToTable(true, "Client Id", "Client Name");

            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }


            MemoryStream ms = new MemoryStream();
            string fontsyle = "Courier New";

            int totalfonts = FontFactory.RegisterDirectory("c:\\WINDOWS\\fonts");
            StringBuilder sa = new StringBuilder();
            foreach (string fontname in FontFactory.RegisteredFonts)
            {
                sa.Append(fontname + "\n");
            }
            int Fontsize = 12;
            if (dt.Rows.Count > 0)
            {
                Document document = new Document(PageSize.A4.Rotate());
                var writer = PdfWriter.GetInstance(document, ms);
                PageEventHelperPageNo pageEventHelper = new PageEventHelperPageNo();
                writer.PageEvent = pageEventHelper;
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                for (int K = 0; K < DtGroup.Rows.Count; K++)
                {
                    if (DtGroup.Rows[K]["Client Id"].ToString().Trim() != "")
                    {
                        document.NewPage();

                        string Type = DtGroup.Rows[K]["Client Id"].ToString();

                        PdfPTable Maintable = new PdfPTable(10);
                        Maintable.TotalWidth = 790;
                        Maintable.HeaderRows = 5;
                        Maintable.LockedWidth = true;
                        float[] width = new float[] { 1f, 1.5f, 1.5f, 1f, 2f, 3f, 1f, 1f, 1f, 1f };
                        Maintable.SetWidths(width);

                        PdfPCell cell = new PdfPCell(new Phrase(companyName, FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell.Colspan = 10;
                        cell.Border = 0;
                        cell.FixedHeight = 22;
                        Maintable.AddCell(cell);

                        PdfPCell celldata = new PdfPCell(new Phrase("Data Checklist", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        celldata.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        celldata.Colspan = 10;
                        celldata.Border = 0;
                        celldata.FixedHeight = 22;
                        //celldata.PaddingTop = -10;
                        Maintable.AddCell(celldata);

                        Clientname = DtGroup.Rows[K]["Client Name"].ToString() + " - " + DtGroup.Rows[K]["Client ID"].ToString();

                        PdfPCell cell2 = new PdfPCell(new Phrase(Clientname, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell2.Colspan = 5;
                        cell2.Border = 0;
                        //cell2.PaddingTop = -5;
                        Maintable.AddCell(cell2);

                        PdfPCell cell3 = new PdfPCell(new Phrase("Salary Month", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cell3.Colspan = 5;
                        cell3.Border = 0;
                        //cell3.PaddingTop = -5;
                        Maintable.AddCell(cell3);

                        PdfPCell cell4 = new PdfPCell(new Phrase("93 PF1/ES1/NOIDA/GZB/NCR/SEC/", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell4.Colspan = 5;
                        cell4.Border = 0;
                        //cell4.PaddingTop = -5;
                        Maintable.AddCell(cell4);

                        PdfPCell cell5 = new PdfPCell(new Phrase(GetMonthName() + " " + Year, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell5.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        cell5.Colspan = 5;
                        cell5.Border = 0;
                        //cell5.PaddingTop = -5;
                        Maintable.AddCell(cell5);

                        PdfPCell cell6 = new PdfPCell(new Phrase("Slno", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell6.Colspan = 1;
                        cell6.BorderWidthTop = 0.2f;
                        cell6.BorderWidthBottom = 0.2f;
                        cell6.BorderWidthLeft = 0;
                        cell6.BorderWidthRight = 0;
                        Maintable.AddCell(cell6);

                        PdfPCell cell7 = new PdfPCell(new Phrase("Code", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell7.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell7.Colspan = 2;
                        cell7.BorderWidthTop = 0.2f;
                        cell7.BorderWidthBottom = 0.2f;
                        cell7.BorderWidthLeft = 0;
                        cell7.BorderWidthRight = 0;
                        Maintable.AddCell(cell7);

                        PdfPCell cell8 = new PdfPCell(new Phrase("Name", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell8.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell8.Colspan = 2;
                        cell8.BorderWidthTop = 0.2f;
                        cell8.BorderWidthBottom = 0.2f;
                        cell8.BorderWidthLeft = 0;
                        cell8.BorderWidthRight = 0;
                        Maintable.AddCell(cell8);

                        PdfPCell cell9 = new PdfPCell(new Phrase("Designation", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell9.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell9.Colspan = 1;
                        cell9.BorderWidthTop = 0.2f;
                        cell9.BorderWidthBottom = 0.2f;
                        cell9.BorderWidthLeft = 0;
                        cell9.BorderWidthRight = 0;
                        Maintable.AddCell(cell9);

                        PdfPCell cell10 = new PdfPCell(new Phrase("W/D", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell10.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell10.Colspan = 1;
                        cell10.BorderWidthTop = 0.2f;
                        cell10.BorderWidthBottom = 0.2f;
                        cell10.BorderWidthLeft = 0;
                        cell10.BorderWidthRight = 0;
                        Maintable.AddCell(cell10);

                        PdfPCell cell11 = new PdfPCell(new Phrase("OT", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell11.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell11.Colspan = 1;
                        cell11.BorderWidthTop = 0.2f;
                        cell11.BorderWidthBottom = 0.2f;
                        cell11.BorderWidthLeft = 0;
                        cell11.BorderWidthRight = 0;
                        Maintable.AddCell(cell11);

                        PdfPCell cell12 = new PdfPCell(new Phrase("TOT", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell12.Colspan = 1;
                        cell12.BorderWidthTop = 0.2f;
                        cell12.BorderWidthBottom = 0.2f;
                        cell12.BorderWidthLeft = 0;
                        cell12.BorderWidthRight = 0;
                        Maintable.AddCell(cell12);

                        PdfPCell cell13 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell13.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell13.Colspan = 1;
                        cell13.BorderWidthTop = 0.2f;
                        cell13.BorderWidthBottom = 0.2f;
                        cell13.BorderWidthLeft = 0;
                        cell13.BorderWidthRight = 0;
                        Maintable.AddCell(cell13);

                        int pagecount = 0;
                        string Sno = "";
                        string ClientID = "";
                        string Empid = "";
                        string Empname = "";
                        string Desgn = "";
                        float Noofduties = 0;
                        float Ots = 0;
                        float Tot = 0;
                        float Wo = 0;
                        float Leave = 0;
                        float PH = 0;
                        float Advance = 0;
                        float CanteenAdv = 0;
                        float Uniform = 0;
                        float TotalDeds = 0;
                        float OtherDed = 0;

                        float TotalNoofduties = 0;
                        float TotalOts = 0;
                        float TotalTot = 0;
                        float TotalWo = 0;
                        float TotalLeave = 0;
                        float TotalPH = 0;
                        float TotalAdvance = 0;
                        float TotalCanteenAdv = 0;
                        float TotalUniform = 0;
                        float TotalTotalDeds = 0;
                        float TotalOtherDed = 0;

                        int PageSno = 1;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            if (DtGroup.Rows[K]["Client Id"].ToString() == dt.Rows[i]["Client Id"].ToString())
                            {
                                #region
                                Sno = dt.Rows[i]["Sno"].ToString();
                                ClientID = dt.Rows[i]["Client Id"].ToString();
                                Empid = dt.Rows[i]["Emp Id"].ToString();
                                Empname = dt.Rows[i]["Name"].ToString();
                                Desgn = dt.Rows[i]["Designation"].ToString();
                                Noofduties = Convert.ToSingle(dt.Rows[i]["Duties"].ToString());
                                Ots = Convert.ToSingle(dt.Rows[i]["OT"].ToString());
                                Tot = Convert.ToSingle(dt.Rows[i]["TotalDuties"].ToString());
                                Wo = Convert.ToSingle(dt.Rows[i]["WO"].ToString());
                                Leave = Convert.ToSingle(dt.Rows[i]["Leave"].ToString());
                                PH = Convert.ToSingle(dt.Rows[i]["PH"].ToString());
                                Advance = Convert.ToSingle(dt.Rows[i]["Penalty"].ToString());
                                Uniform = Convert.ToSingle(dt.Rows[i]["Uniform Ded"].ToString());
                                OtherDed = Convert.ToSingle(dt.Rows[i]["Other Ded"].ToString());
                                CanteenAdv = Convert.ToSingle(dt.Rows[i]["Canteen Adv"].ToString());
                                TotalDeds = Convert.ToSingle(dt.Rows[i]["Total Amount"].ToString());

                                PdfPCell cell15 = new PdfPCell(new Phrase(PageSno++.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell15.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell15.Colspan = 1;
                                cell15.Border = 0;
                                Maintable.AddCell(cell15);

                                PdfPCell cell17 = new PdfPCell(new Phrase(Empid, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                                cell17.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell17.Colspan = 2;
                                cell17.Border = 0;
                                Maintable.AddCell(cell17);

                                PdfPCell cell18 = new PdfPCell(new Phrase(Empname, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                                cell18.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                cell18.Colspan = 2;
                                cell18.Border = 0;
                                Maintable.AddCell(cell18);

                                PdfPCell cell19 = new PdfPCell(new Phrase(Desgn, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell19.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                cell19.Colspan = 1;
                                cell19.Border = 0;
                                Maintable.AddCell(cell19);

                                PdfPCell cell20 = new PdfPCell(new Phrase(Noofduties.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell20.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell20.Colspan = 1;
                                cell20.Border = 0;
                                Maintable.AddCell(cell20);
                                TotalNoofduties += Noofduties;

                                PdfPCell cell21 = new PdfPCell(new Phrase(Ots.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell21.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell21.Colspan = 1;
                                cell21.Border = 0;
                                Maintable.AddCell(cell21);
                                TotalOts += Ots;

                                PdfPCell cell22 = new PdfPCell(new Phrase(Tot.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell22.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell22.Colspan = 1;
                                cell22.Border = 0;
                                Maintable.AddCell(cell22);
                                TotalTot += Tot;

                                PdfPCell cell23 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell23.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell23.Colspan = 1;
                                cell23.Border = 0;
                                Maintable.AddCell(cell23);

                                PdfPCell cell24 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell24.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell24.Colspan = 1;
                                cell24.Border = 0;
                                Maintable.AddCell(cell24);

                                PdfPCell cell25 = new PdfPCell(new Phrase("Advance", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell25.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell25.Colspan = 1;
                                cell25.Border = 0;
                                Maintable.AddCell(cell25);

                                PdfPCell cell26 = new PdfPCell(new Phrase("Uniform", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell26.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell26.Colspan = 1;
                                cell26.Border = 0;
                                Maintable.AddCell(cell26);

                                PdfPCell cell27 = new PdfPCell(new Phrase("Other", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell27.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell27.Colspan = 1;
                                cell27.Border = 0;
                                Maintable.AddCell(cell27);

                                PdfPCell cell28 = new PdfPCell(new Phrase("Misc", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell28.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell28.Colspan = 1;
                                cell28.Border = 0;
                                Maintable.AddCell(cell28);

                                PdfPCell cell29 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell29.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell29.Colspan = 1;
                                cell29.Border = 0;
                                Maintable.AddCell(cell29);

                                PdfPCell cell30 = new PdfPCell(new Phrase("WO", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell30.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell30.Colspan = 1;
                                cell30.Border = 0;
                                Maintable.AddCell(cell30);

                                PdfPCell cell31 = new PdfPCell(new Phrase("Leave", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell31.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell31.Colspan = 1;
                                cell31.Border = 0;
                                Maintable.AddCell(cell31);

                                PdfPCell cell32 = new PdfPCell(new Phrase("PH", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell32.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell32.Colspan = 1;
                                cell32.Border = 0;
                                Maintable.AddCell(cell32);

                                PdfPCell cell33 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell33.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell33.Colspan = 1;
                                cell33.Border = 0;
                                Maintable.AddCell(cell33);

                                PdfPCell cell34 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell34.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell34.Colspan = 1;
                                cell34.BorderWidthTop = 0;
                                cell34.BorderWidthBottom = 0.2f;
                                cell34.BorderWidthLeft = 0;
                                cell34.BorderWidthRight = 0;
                                Maintable.AddCell(cell34);

                                PdfPCell cell35 = new PdfPCell(new Phrase(Advance.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell35.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell35.Colspan = 1;
                                cell35.BorderWidthTop = 0;
                                cell35.BorderWidthBottom = 0.2f;
                                cell35.BorderWidthLeft = 0;
                                cell35.BorderWidthRight = 0;
                                Maintable.AddCell(cell35);
                                TotalAdvance += Advance;

                                PdfPCell cell36 = new PdfPCell(new Phrase(Uniform.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell36.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell36.Colspan = 1;
                                cell36.BorderWidthTop = 0;
                                cell36.BorderWidthBottom = 0.2f;
                                cell36.BorderWidthLeft = 0;
                                cell36.BorderWidthRight = 0;
                                Maintable.AddCell(cell36);
                                TotalUniform += Uniform;

                                PdfPCell cell37 = new PdfPCell(new Phrase(OtherDed.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell37.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell37.Colspan = 1;
                                cell37.BorderWidthTop = 0;
                                cell37.BorderWidthBottom = 0.2f;
                                cell37.BorderWidthLeft = 0;
                                cell37.BorderWidthRight = 0;
                                Maintable.AddCell(cell37);
                                TotalOtherDed += OtherDed;

                                PdfPCell cell38 = new PdfPCell(new Phrase(CanteenAdv.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell38.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell38.Colspan = 1;
                                cell38.BorderWidthTop = 0;
                                cell38.BorderWidthBottom = 0.2f;
                                cell38.BorderWidthLeft = 0;
                                cell38.BorderWidthRight = 0;
                                Maintable.AddCell(cell38);
                                TotalCanteenAdv += CanteenAdv;

                                PdfPCell cell39 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell39.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell39.Colspan = 1;
                                cell39.BorderWidthTop = 0;
                                cell39.BorderWidthBottom = 0.2f;
                                cell39.BorderWidthLeft = 0;
                                cell39.BorderWidthRight = 0;
                                Maintable.AddCell(cell39);

                                PdfPCell cell40 = new PdfPCell(new Phrase(Wo.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell40.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell40.Colspan = 1;
                                cell40.BorderWidthTop = 0;
                                cell40.BorderWidthBottom = 0.2f;
                                cell40.BorderWidthLeft = 0;
                                cell40.BorderWidthRight = 0;
                                Maintable.AddCell(cell40);
                                TotalWo += Wo;

                                PdfPCell cell41 = new PdfPCell(new Phrase(Leave.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell41.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell41.Colspan = 1;
                                cell41.BorderWidthTop = 0;
                                cell41.BorderWidthBottom = 0.2f;
                                cell41.BorderWidthLeft = 0;
                                cell41.BorderWidthRight = 0;
                                Maintable.AddCell(cell41);
                                TotalLeave += Leave;

                                PdfPCell cell42 = new PdfPCell(new Phrase(PH.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                cell42.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell42.Colspan = 1;
                                cell42.BorderWidthTop = 0;
                                cell42.BorderWidthBottom = 0.2f;
                                cell42.BorderWidthLeft = 0;
                                cell42.BorderWidthRight = 0;
                                Maintable.AddCell(cell42);
                                TotalPH += PH;

                                PdfPCell cell43 = new PdfPCell(new Phrase(TotalDeds.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                                cell43.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell43.Colspan = 1;
                                cell43.BorderWidthTop = 0;
                                cell43.BorderWidthBottom = 0.2f;
                                cell43.BorderWidthLeft = 0;
                                cell43.BorderWidthRight = 0;
                                Maintable.AddCell(cell43);
                                TotalTotalDeds += TotalDeds;

                                #endregion
                            }

                        }

                        #region for Totals

                        PdfPCell cell191 = new PdfPCell(new Phrase("Total Duties", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell191.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell191.Colspan = 6;
                        cell191.Border = 0;
                        Maintable.AddCell(cell191);

                        PdfPCell cell201 = new PdfPCell(new Phrase(TotalNoofduties.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell201.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell201.Colspan = 1;
                        cell201.Border = 0;
                        Maintable.AddCell(cell201);

                        PdfPCell cell211 = new PdfPCell(new Phrase(TotalOts.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell211.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell211.Colspan = 1;
                        cell211.Border = 0;
                        Maintable.AddCell(cell211);

                        PdfPCell cell221 = new PdfPCell(new Phrase(TotalTot.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell221.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell221.Colspan = 1;
                        cell221.Border = 0;
                        Maintable.AddCell(cell221);

                        PdfPCell cell231 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell231.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell231.Colspan = 1;
                        cell231.Border = 0;
                        Maintable.AddCell(cell231);

                        PdfPCell cell241 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell241.Colspan = 1;
                        cell241.Border = 0;
                        Maintable.AddCell(cell241);

                        PdfPCell cell251 = new PdfPCell(new Phrase("Advance", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell251.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell251.Colspan = 1;
                        cell251.Border = 0;
                        Maintable.AddCell(cell251);

                        PdfPCell cell261 = new PdfPCell(new Phrase("Uniform", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell261.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell261.Colspan = 1;
                        cell261.Border = 0;
                        Maintable.AddCell(cell261);

                        PdfPCell cell271 = new PdfPCell(new Phrase("Other", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell271.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell271.Colspan = 1;
                        cell271.Border = 0;
                        Maintable.AddCell(cell271);

                        PdfPCell cell281 = new PdfPCell(new Phrase("Misc", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell281.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell281.Colspan = 1;
                        cell281.Border = 0;
                        Maintable.AddCell(cell281);

                        PdfPCell cell291 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell291.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell291.Colspan = 1;
                        cell291.Border = 0;
                        Maintable.AddCell(cell291);

                        PdfPCell cell301 = new PdfPCell(new Phrase("WO", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell301.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell301.Colspan = 1;
                        cell301.Border = 0;
                        Maintable.AddCell(cell301);

                        PdfPCell cell311 = new PdfPCell(new Phrase("Leave", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell311.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell311.Colspan = 1;
                        cell311.Border = 0;
                        Maintable.AddCell(cell311);

                        PdfPCell cell321 = new PdfPCell(new Phrase("PH", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell321.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell321.Colspan = 1;
                        cell321.Border = 0;
                        Maintable.AddCell(cell321);

                        PdfPCell cell331 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell331.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell331.Colspan = 1;
                        cell331.Border = 0;
                        Maintable.AddCell(cell331);

                        PdfPCell cell341 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell341.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell341.Colspan = 1;
                        cell341.BorderWidthTop = 0;
                        cell341.BorderWidthBottom = 0.2f;
                        cell341.BorderWidthLeft = 0;
                        cell341.BorderWidthRight = 0;
                        Maintable.AddCell(cell341);

                        PdfPCell cell351 = new PdfPCell(new Phrase(TotalAdvance.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell351.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell351.Colspan = 1;
                        cell351.BorderWidthTop = 0;
                        cell351.BorderWidthBottom = 0.2f;
                        cell351.BorderWidthLeft = 0;
                        cell351.BorderWidthRight = 0;
                        Maintable.AddCell(cell351);

                        PdfPCell cell361 = new PdfPCell(new Phrase(TotalUniform.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell361.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell361.Colspan = 1;
                        cell361.BorderWidthTop = 0;
                        cell361.BorderWidthBottom = 0.2f;
                        cell361.BorderWidthLeft = 0;
                        cell361.BorderWidthRight = 0;
                        Maintable.AddCell(cell361);

                        PdfPCell cell371 = new PdfPCell(new Phrase(TotalOtherDed.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell371.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell371.Colspan = 1;
                        cell371.BorderWidthTop = 0;
                        cell371.BorderWidthBottom = 0.2f;
                        cell371.BorderWidthLeft = 0;
                        cell371.BorderWidthRight = 0;
                        Maintable.AddCell(cell371);

                        PdfPCell cell381 = new PdfPCell(new Phrase(TotalCanteenAdv.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell381.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell381.Colspan = 1;
                        cell381.BorderWidthTop = 0;
                        cell381.BorderWidthBottom = 0.2f;
                        cell381.BorderWidthLeft = 0;
                        cell381.BorderWidthRight = 0;
                        Maintable.AddCell(cell381);

                        PdfPCell cell391 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell391.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell391.Colspan = 1;
                        cell391.BorderWidthTop = 0;
                        cell391.BorderWidthBottom = 0.2f;
                        cell391.BorderWidthLeft = 0;
                        cell391.BorderWidthRight = 0;
                        Maintable.AddCell(cell391);

                        PdfPCell cell401 = new PdfPCell(new Phrase(TotalWo.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell401.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell401.Colspan = 1;
                        cell401.BorderWidthTop = 0;
                        cell401.BorderWidthBottom = 0.2f;
                        cell401.BorderWidthLeft = 0;
                        cell401.BorderWidthRight = 0;
                        Maintable.AddCell(cell401);

                        PdfPCell cell411 = new PdfPCell(new Phrase(TotalLeave.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell411.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell411.Colspan = 1;
                        cell411.BorderWidthTop = 0;
                        cell411.BorderWidthBottom = 0.2f;
                        cell411.BorderWidthLeft = 0;
                        cell411.BorderWidthRight = 0;
                        Maintable.AddCell(cell411);

                        PdfPCell cell421 = new PdfPCell(new Phrase(TotalPH.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell421.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell421.Colspan = 1;
                        cell421.BorderWidthTop = 0;
                        cell421.BorderWidthBottom = 0.2f;
                        cell421.BorderWidthLeft = 0;
                        cell421.BorderWidthRight = 0;
                        Maintable.AddCell(cell421);

                        PdfPCell cell431 = new PdfPCell(new Phrase(TotalTotalDeds.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell431.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell431.Colspan = 1;
                        cell431.BorderWidthTop = 0;
                        cell431.BorderWidthBottom = 0.2f;
                        cell431.BorderWidthLeft = 0;
                        cell431.BorderWidthRight = 0;
                        Maintable.AddCell(cell431);

                        #endregion

                        document.Add(Maintable);

                        #region chksummary
                        if (chksummary.Checked == true)
                        {
                            document.NewPage();
                            string strDesgnwise = "select SUM(isnull(ea.NoOfDuties,0)) as Duties,sum(isnull(ea.OT,0)) as OT,SUM(isnull(ea.NoOfDuties,0)+isnull(ea.OT,0)) as Total,ISNULL(d.Design,'') as Desgn  from EmpAttendance ea inner join Designations d on d.DesignId = ea.Design where ClientId = '" + Clientid + "' and MONTH = " + month + Year.Substring(2, 2) + " group by d.Design";
                            DataTable dtdesgn = config.ExecuteAdaptorAsyncWithQueryParams(strDesgnwise).Result;

                            PdfPTable tablesummary = new PdfPTable(4);
                            tablesummary.TotalWidth = 580;
                            //tablesummary.HeaderRows = 5;
                            tablesummary.LockedWidth = true;
                            float[] width2 = new float[] { 1f, 1f, 1f, 1f };
                            tablesummary.SetWidths(width2);

                            PdfPCell cellsumary;

                            cellsumary = new PdfPCell(new Phrase("DESIGNATION SUMMARY", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 4;
                            cellsumary.Border = 0;
                            // cellsumary.PaddingTop = -30;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase(Clientname, FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 4;
                            cellsumary.Border = 0;
                            //cellsumary.PaddingTop = -10;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase("DESIGNATION", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 1;
                            cellsumary.BorderWidthBottom = 0.2f;
                            cellsumary.BorderWidthTop = 0;
                            cellsumary.BorderWidthLeft = 0;
                            cellsumary.BorderWidthRight = 0;
                            //cellsumary.PaddingTop = -10;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase("NORMAL", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 1;
                            cellsumary.BorderWidthBottom = 0.2f;
                            cellsumary.BorderWidthTop = 0;
                            cellsumary.BorderWidthLeft = 0;
                            cellsumary.BorderWidthRight = 0;
                            //cellsumary.PaddingTop = -10;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase("OVER TIME", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 1;
                            cellsumary.BorderWidthBottom = 0.2f;
                            cellsumary.BorderWidthTop = 0;
                            cellsumary.BorderWidthLeft = 0;
                            cellsumary.BorderWidthRight = 0;
                            //cellsumary.PaddingTop = -10;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase("Total", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 2;
                            cellsumary.BorderWidthBottom = 0.2f;
                            cellsumary.BorderWidthTop = 0;
                            cellsumary.BorderWidthLeft = 0;
                            cellsumary.BorderWidthRight = 0;
                            //cellsumary.PaddingTop = -10;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 1;
                            cellsumary.Border = 0;
                            cellsumary.PaddingBottom = 10;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase("0.00", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 1;
                            cellsumary.Border = 0;
                            cellsumary.PaddingBottom = 10;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase("0.00", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 1;
                            cellsumary.Border = 0;
                            cellsumary.PaddingBottom = 10;
                            tablesummary.AddCell(cellsumary);

                            cellsumary = new PdfPCell(new Phrase("0.00", FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            cellsumary.Colspan = 1;
                            cellsumary.Border = 0;
                            cellsumary.PaddingBottom = 10;
                            tablesummary.AddCell(cellsumary);

                            float TotalDuties = 0;
                            float TotalOtss = 0;
                            float TotalDays = 0;
                            string Designation = "";
                            if (dtdesgn.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtdesgn.Rows.Count; i++)
                                {
                                    Designation = dtdesgn.Rows[i]["Desgn"].ToString();
                                    TotalDuties = Convert.ToSingle(dtdesgn.Rows[i]["Duties"].ToString());
                                    TotalOtss = Convert.ToSingle(dtdesgn.Rows[i]["OT"].ToString());
                                    TotalDays = Convert.ToSingle(dtdesgn.Rows[i]["Total"].ToString());


                                    cellsumary = new PdfPCell(new Phrase(Designation, FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                    cellsumary.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    cellsumary.Colspan = 1;
                                    cellsumary.Border = 0;
                                    cellsumary.PaddingBottom = 10;
                                    tablesummary.AddCell(cellsumary);

                                    cellsumary = new PdfPCell(new Phrase(TotalDuties.ToString(), FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                    cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    cellsumary.Colspan = 1;
                                    cellsumary.Border = 0;
                                    cellsumary.PaddingBottom = 10;
                                    tablesummary.AddCell(cellsumary);

                                    cellsumary = new PdfPCell(new Phrase(TotalOtss.ToString(), FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                    cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    cellsumary.Colspan = 1;
                                    cellsumary.Border = 0;
                                    cellsumary.PaddingBottom = 10;
                                    tablesummary.AddCell(cellsumary);

                                    cellsumary = new PdfPCell(new Phrase(TotalDays.ToString(), FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                    cellsumary.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                    cellsumary.Colspan = 1;
                                    cellsumary.Border = 0;
                                    cellsumary.PaddingBottom = 10;
                                    tablesummary.AddCell(cellsumary);

                                }
                                document.Add(tablesummary);


                            }
                        }

                        #endregion

                    }
                }

                string filename = "SalaryCheckList" + ".pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }

        }

        public string GetMonthName()
        {
            string monthname = string.Empty;

            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString();

            return monthname;
        }

        protected void rbclient_CheckedChanged(object sender, EventArgs e)
        {
            if (rbclient.Checked == true)
            {
                ddlclientid.Visible = true;
                ddlcname.Visible = true;
                txtmonth.Visible = true;
                chksummary.Visible = true;
                lblmonth.Visible = true;
                lblclinetid.Visible = true;
                lblcname.Visible = true;
                rbexcel.Checked = false;
                txtmonth.Text = "";
                ddlclientid.Items.Clear();
                ddlcname.Items.Clear();
                GVAttendance.DataSource = null;
                GVAttendance.DataBind();
                Loadclientids();
                Loadclientnames();
                ddlexcelno.Visible = false;
                txttime.Visible = false;
                lblexcelno.Visible = false;
                lbltime.Visible = false;
            }
            else
            {
                ddlexcelno.Visible = true;
                txttime.Visible = true;
                lblexcelno.Visible = true;
                lbltime.Visible = true;
                lblmonth.Visible = true;
                txtmonth.Visible = true;
                rbclient.Checked = false;
                txtmonth.Text = "";
                txttime.Text = "";
                ddlexcelno.Items.Clear();
                ddlclientid.Visible = false;
                ddlcname.Visible = false;
                chksummary.Visible = false;
                lblclinetid.Visible = false;
                lblcname.Visible = false;
            }
        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {
            ExcelNumbers();
        }

        protected void ddlexcelno_SelectedIndexChanged(object sender, EventArgs e)
        {
            Textbind();
        }

        public DataSet getDataSetExportToExcel()
        {
            string date = string.Empty;
            string month = "";
            string Year = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            DataTable dt = null;

            GVAttendance.DataSource = null;
            GVAttendance.DataBind();


            string Clientid = ddlclientid.SelectedValue;

            if (ddlclientid.SelectedIndex == 1)
            {
                Clientid = "%";
            }

            month = DateTime.Parse(date).Month.ToString();
            Year = DateTime.Parse(date).Year.ToString();
            string MonthNew = month + Year.Substring(2, 2);
            string SPName = "";



            DataSet ds = new DataSet();
            string QryCount = "select distinct ClientId,ExcelNumber from EmpAttendance  where ExcelNumber like '" + ddlexcelno.SelectedValue + "'+'%' and MONTH='" + MonthNew + "'";
            DataTable ClientidCount = config.ExecuteAdaptorAsyncWithQueryParams(QryCount).Result;
            if (ClientidCount.Rows.Count > 0)
            {
                for (int k = 0; k < ClientidCount.Rows.Count; k++)
                {
                    string Clientids = "";
                    string DesdinID = "";
                    Clientids = ClientidCount.Rows[k]["ClientId"].ToString();
                    DesdinID = ClientidCount.Rows[k]["ExcelNumber"].ToString();

                    SPName = "AttendanceReportExcelnumbers";
                    Hashtable ht = new Hashtable();
                    ht.Add("@ExcelNumber", ddlexcelno.SelectedValue);
                    ht.Add("@month", month + Year.Substring(2, 2));
                    ht.Add("@ClientId", Clientids);

                    DataTable dts = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

                    DataTable dtclients = new DataTable(Clientids);
                    dtclients = dts;

                    ds.Tables.Add(dtclients);

                }
            }
            return ds;
        }

    }
}