using System;
using System.Collections;
using System.Data;
using KLTS.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class POPrint : System.Web.UI.Page
    {
        AppConfiguration config = new  AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string GRVPrefix = "";
        string CmpIDPrefix = "";
        string EmpIDPrefix = "";
        string UserID = "";
        string BranchID = "";
        string FontStyle = "TimesNewRoman";

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

                GetPONos();
                GRVIDAuto();


            }

        }

        
        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            UserID = Session["UserId"].ToString();
            GRVPrefix = Session["GRVPrefix"].ToString();

        }


        public void GRVIDAuto()
        {


            int GRVID = 1;
            string selectqueryclientid = "select (max(right(InflowID,4))) as GRVID from InvInflowMaster  where InflowID like '" + GRVPrefix + "%'";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;
            string invPrefix = string.Empty;

            if (dt.Rows.Count > 0)
            {

                if (String.IsNullOrEmpty(dt.Rows[0]["GRVID"].ToString()) == false)
                {
                    GRVID = Convert.ToInt32(dt.Rows[0]["GRVID"].ToString()) + 1;
                }
                else
                {
                    GRVID = int.Parse("1");
                }
            }






        }

        protected void GetPONos()
        {
            string sqlqry = "Select distinct pono from InvpoMaster  ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlPONo.DataTextField = "pono";
                ddlPONo.DataValueField = "pono";
                ddlPONo.DataSource = dt;
                ddlPONo.DataBind();

            }

            ddlPONo.Items.Insert(0, "-Select-");

        }




        protected void BtnSave_Click(object sender, EventArgs e)
        {

            try
            {

                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A4);
                Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();



                string strQry1 = "select sum(totalbuyingprice) as tottal,sum(isnull(VATCmp1,0)) as VATCmp1,sum(isnull(VATCmp2,0)) as VATCmp2,sum(isnull(VATCmp3,0)) as VATCmp3,sum(isnull(VATCmp4,0)) as VATCmp4,sum(isnull(VATCmp5,0)) as VATCmp5 from InvPOMaster where PONo='" + ddlPONo.SelectedValue + "'";
                DataTable dttotals = config.ExecuteAdaptorAsyncWithQueryParams(strQry1).Result;

                float tottal = 0;
                float VATCmp1per = 0;
                float VATCmp2per = 0;
                float VATCmp3per = 0;
                float VATCmp4per = 0;
                float VATCmp5per = 0;


                if (dttotals.Rows.Count > 0)
                {
                    tottal = float.Parse(dttotals.Rows[0]["tottal"].ToString());
                    VATCmp1per = float.Parse(dttotals.Rows[0]["VATCmp1"].ToString());
                    VATCmp2per = float.Parse(dttotals.Rows[0]["VATCmp2"].ToString());
                    VATCmp3per = float.Parse(dttotals.Rows[0]["VATCmp3"].ToString());
                    VATCmp4per = float.Parse(dttotals.Rows[0]["VATCmp4"].ToString());
                    VATCmp5per = float.Parse(dttotals.Rows[0]["VATCmp5"].ToString());

                }

                #region for Companyinfo Qry
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string strQry = "Select * from CompanyInfo";
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

                #endregion

                #region  Begin Get PO Print Based PONO
                Hashtable HtGetPONo = new Hashtable();
                var SPNameForPOPrint = "POPrint";
                HtGetPONo.Add("@PONo", ddlPONo.SelectedValue);
                HtGetPONo.Add("@BranchID", BranchID);
                DataTable DTPOPrint =config.ExecuteAdaptorAsyncWithParams(SPNameForPOPrint, HtGetPONo).Result;

                #endregion

                string PONo = "";
                string PODate = "";
                string contactperson = "";
                string contactno = "";
                string emialid = "";
                string Address = "";
                string itemdescription = "";
                string unitmeasure = "";
                string qty = "";
                string buyingprice = "";
                string total = "";
                string DeliveryAddress = "";
                string DelContactPerson = "";
                string DelContactNo = "";
                string DelEmailId = "";
                string suppliername = "";

                if (DTPOPrint.Rows.Count > 0)
                {

                    contactperson = DTPOPrint.Rows[0]["ContactPerson"].ToString();
                    contactno = DTPOPrint.Rows[0]["ContactNo"].ToString();
                    Address = DTPOPrint.Rows[0]["Address"].ToString();
                    emialid = DTPOPrint.Rows[0]["EmailId"].ToString();
                    PODate = DTPOPrint.Rows[0]["PODate"].ToString();
                    PONo = DTPOPrint.Rows[0]["PONo"].ToString();
                    DeliveryAddress = DTPOPrint.Rows[0]["DeliveryAddress"].ToString();
                    DelContactPerson = DTPOPrint.Rows[0]["DelContactPerson"].ToString();
                    DelContactNo = DTPOPrint.Rows[0]["DelContactNo"].ToString();
                    DelEmailId = DTPOPrint.Rows[0]["DelEmailId"].ToString();
                    suppliername = DTPOPrint.Rows[0]["VENDORNAME"].ToString();

                }

                // document.AddTitle(companyName);
                document.AddAuthor("DIYOS");
                document.AddSubject("Invoice");
                document.AddKeywords("Keyword1, keyword2, …");
                string FontStyle = "Tahoma";

                PdfPCell cell;

                PdfPTable Maintables = new PdfPTable(4);
                Maintables.TotalWidth = 560f;
                Maintables.LockedWidth = true;
                float[] Mainwidths = new float[] { 2f, 2f, 2f, 2f };
                Maintables.SetWidths(Mainwidths);

                string imagepath = Server.MapPath("~/assets/gs-logo.png");
                string imagepath1 = Server.MapPath("~/assets/BillLogo1.png");

                if (File.Exists(imagepath))
                {
                    iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);

                    gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                    gif2.SpacingBefore = 50;
                    gif2.ScalePercent(90f);
                    gif2.SetAbsolutePosition(27f, 720f);
                    //document.Add(new Paragraph(" "));
                    document.Add(gif2);
                }


                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.Border = 0;
                cell.PaddingTop = 40;
                cell.Colspan = 4;
                Maintables.AddCell(cell);
                cell = new PdfPCell(new Paragraph("PURCHASE ORDER", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 1.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 4;
                Maintables.AddCell(cell);
                cell = new PdfPCell(new Paragraph(companyName, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 4;
                Maintables.AddCell(cell);
                cell = new PdfPCell(new Paragraph(companyAddress, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 1.5f;
                cell.PaddingLeft = 40;
                cell.Colspan = 4;
                Maintables.AddCell(cell);

                document.Add(Maintables);
                PdfPTable address = new PdfPTable(4);
                address.TotalWidth = 560f;
                address.LockedWidth = true;
                float[] addreslogo = new float[] { 2f, 2f, 2f, 2f };
                address.SetWidths(addreslogo);

                #region for Child table One By Anil On  20-09-2016

                PdfPTable tempTable1 = new PdfPTable(2);
                tempTable1.TotalWidth = 280f;
                tempTable1.LockedWidth = true;
                float[] tempWidth1 = new float[] { 2f, 2f };
                tempTable1.SetWidths(tempWidth1);

                cell = new PdfPCell(new Paragraph("Purchase Order No", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                tempTable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph(PONo, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                cell.Colspan = 1;
                tempTable1.AddCell(cell);

                cell = new PdfPCell(new Paragraph("Purchase Order Date", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                tempTable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph(PODate, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                cell.Colspan = 1;
                tempTable1.AddCell(cell);

                cell = new PdfPCell(new Paragraph("Suppliers Name", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                tempTable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph(suppliername, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                cell.Colspan = 1;
                tempTable1.AddCell(cell);


                cell = new PdfPCell(new Paragraph("\n\nSuppliers Address", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                cell.FixedHeight = 60;
                tempTable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph(Address, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                cell.Colspan = 1;
                cell.FixedHeight = 50;
                tempTable1.AddCell(cell);


                PdfPCell childTable1 = new PdfPCell(tempTable1);
                childTable1.Border = 0;
                childTable1.Colspan = 2;
                childTable1.HorizontalAlignment = 0;
                address.AddCell(childTable1);

                PdfPTable tempTable2 = new PdfPTable(2);
                tempTable2.TotalWidth = 280f;
                tempTable2.LockedWidth = true;
                float[] tempWidth2 = new float[] { 2f, 2f };
                tempTable2.SetWidths(tempWidth2);

                cell = new PdfPCell(new Paragraph("Delivery Address", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 2;
                tempTable2.AddCell(cell);

                cell = new PdfPCell(new Paragraph(DeliveryAddress, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 2;
                tempTable2.AddCell(cell);


                PdfPCell childTable2 = new PdfPCell(tempTable2);
                childTable2.Border = 0;
                childTable2.Colspan = 2;
                childTable2.HorizontalAlignment = 0;
                address.AddCell(childTable2);
                #endregion

                document.Add(address);

                PdfPTable Maintable = new PdfPTable(4);
                Maintable.TotalWidth = 560f;
                Maintable.LockedWidth = true;
                float[] Mainwidth = new float[] { 2f, 2f, 2f, 2f };
                Maintable.SetWidths(Mainwidth);



                #region for Maintable (End column:Email Id)
                cell = new PdfPCell(new Paragraph("Contact Person ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(contactperson, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph("Contact Person ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(DelContactPerson, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);


                cell = new PdfPCell(new Paragraph("Contact No. ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(contactno, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph("Contact No. ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(DelContactNo, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);


                cell = new PdfPCell(new Paragraph("Email ID : ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(emialid, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph("Email ID :", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph(DelEmailId, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                Maintable.AddCell(cell);



                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                cell.Colspan = 1;
                Maintable.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                cell.FixedHeight = 15;
                Maintable.AddCell(cell);
                #endregion

                document.Add(Maintable);


                PdfPTable Maintable1 = new PdfPTable(6);
                Maintable1.TotalWidth = 560f;
                Maintable1.LockedWidth = true;
                float[] Mainwidth1 = new float[] { 0.7f, 3f, 1.7f, 1.7f, 1.9f, 2.8f };
                Maintable1.SetWidths(Mainwidth1);

                #region for Maintable1 (Grid values)

                cell = new PdfPCell(new Paragraph("S.No", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph("ITEM DESCRIPTION", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph("UNIT", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph("QTY", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph("PRICE", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable1.AddCell(cell);
                cell = new PdfPCell(new Paragraph("AMOUNT", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                Maintable1.AddCell(cell);

                int k = 1;
                for (int i = 0; i < DTPOPrint.Rows.Count; i++)
                {
                    itemdescription = DTPOPrint.Rows[i]["ItemName"].ToString();
                    unitmeasure = DTPOPrint.Rows[i]["UnitMeasure"].ToString();
                    qty = DTPOPrint.Rows[i]["Qty"].ToString();
                    buyingprice = DTPOPrint.Rows[i]["BuyingPrice"].ToString();
                    total = DTPOPrint.Rows[i]["Total"].ToString();

                    cell = new PdfPCell(new Paragraph(k.ToString(), FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthLeft = 1.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(itemdescription, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(unitmeasure, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(qty, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(buyingprice, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable1.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(total, FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1.5f;
                    cell.Colspan = 1;
                    //cell.FixedHeight = 25;
                    Maintable1.AddCell(cell);
                    k++;
                }

                #endregion

                document.Add(Maintable1);


                PdfPTable Maintable2 = new PdfPTable(6);
                Maintable2.TotalWidth = 560f;
                Maintable2.LockedWidth = true;
                float[] Mainwidth2 = new float[] { 0.7f, 3f, 1.7f, 1.7f, 1.9f, 2.8f };
                Maintable2.SetWidths(Mainwidth2);

                #region for Totals

                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("Total", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 2;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph(tottal.ToString("0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0.5f;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);

                DataTable dt = GlobalData.Instance.LoadTaxComponents();

                string Cmp1Name = "";
                string Cmp2Name = "";
                string Cmp3Name = "";
                string Cmp4Name = "";
                string Cmp5Name = "";

                if (dt.Rows.Count > 0)
                {
                    Cmp1Name = dt.Rows[10]["TaxCmpName"].ToString();
                    Cmp2Name = dt.Rows[11]["TaxCmpName"].ToString();
                    Cmp3Name = dt.Rows[12]["TaxCmpName"].ToString();
                    Cmp4Name = dt.Rows[13]["TaxCmpName"].ToString();
                    Cmp5Name = dt.Rows[14]["TaxCmpName"].ToString();

                }



                if (VATCmp1per > 0)
                {



                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);

                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);

                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);

                    cell = new PdfPCell(new Paragraph(Cmp1Name, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 2;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(VATCmp1per.ToString("0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                }



                if (VATCmp2per > 0)
                {
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(Cmp2Name, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 2;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(VATCmp2per.ToString("0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                }


                if (VATCmp3per > 0)
                {
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(Cmp3Name, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 2;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(VATCmp3per.ToString("0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                }


                if (VATCmp4per > 0)
                {
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(Cmp4Name, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 2;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(VATCmp4per.ToString("0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                }

                if (VATCmp5per > 0)
                {
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(Cmp5Name, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0.5f;
                    cell.Colspan = 2;
                    Maintable2.AddCell(cell);
                    cell = new PdfPCell(new Paragraph(VATCmp5per.ToString("0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1.5f;
                    cell.Colspan = 1;
                    Maintable2.AddCell(cell);
                }


                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);

                float totalamt = 0;
                totalamt = Convert.ToSingle(tottal) + Convert.ToSingle(VATCmp1per) + Convert.ToSingle(VATCmp2per) + Convert.ToSingle(VATCmp3per) + Convert.ToSingle(VATCmp4per) + Convert.ToSingle(VATCmp5per);



                cell = new PdfPCell(new Paragraph("FINAL VALUE", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 2;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph(totalamt.ToString("0.00"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);


                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("ROUND OFF", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 2;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph(totalamt.ToString("#"), FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                Maintable2.AddCell(cell);


                //

                string GTotal = totalamt.ToString("#");
                string inwords = "";
                string rupee = "";
                string paise = "";

                if (GTotal == "") ;
                {
                    GTotal = "0";
                }

                rupee = NumberToEnglish.Instance.NumbersToWords(Convert.ToInt64(GTotal), false);
                inwords = "(In Rupees) Rs. " + rupee + " Only /-";



                //lblamtinwords.Text = NumberToEnglish.Instance.changeNumericToWords(GrandTotal.ToString("0.00"));

                cell = new PdfPCell(new Paragraph("Amount in Words" + inwords, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 6;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("Terms & Conditions :", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 6;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("Payment will be made within 30 days from reciept of the materials,Please submit the Original invoice with the acknowledged DC at the office within 3 days from delivery,Payment will be made by cheque of RTGS", FontFactory.GetFont(FontStyle, 10, Font.NORMAL, BaseColor.BLACK)));
                cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 6;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("For " + companyName, FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 6;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                Maintable2.AddCell(cell);



                //

                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 2;
                cell.FixedHeight = 25;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 2;
                cell.FixedHeight = 25;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                cell.FixedHeight = 25;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 0.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                cell.FixedHeight = 25;
                Maintable2.AddCell(cell);


                cell = new PdfPCell(new Paragraph("Authorized Signature", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 1.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1.5f;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 2;
                cell.FixedHeight = 25;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("Receiver's\nSignature", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 1.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 2;
                cell.FixedHeight = 25;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("Prepared By", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 1.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0.5f;
                cell.Colspan = 1;
                cell.FixedHeight = 25;
                Maintable2.AddCell(cell);
                cell = new PdfPCell(new Paragraph("PLACE : ", FontFactory.GetFont(FontStyle, 10, Font.BOLD, BaseColor.BLACK)));
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BorderWidthBottom = 1.5f;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1.5f;
                cell.Colspan = 1;
                cell.FixedHeight = 25;
                Maintable2.AddCell(cell);
                #endregion


                document.Add(Maintable2);

                string filename = ddlPONo.SelectedValue + " PO.pdf";


                // document.Add(tablecon);

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                lblresult.Text = ex.Message;

            }



        }

    }
}