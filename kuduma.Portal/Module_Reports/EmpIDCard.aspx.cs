using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Text;
namespace Kuduma.Portal
{
    public partial class EmpIDCard : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string fontsyle = "verdana";
        string CmpIDPrefix = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {

                        BindData();
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
                GoToLoginPage();
            }


        }

        protected void BindData()
        {

            string Qry = "select empid,(empid+' - '+empfname+' '+empmname+' '+emplname) as empname from empdetails";
            DataTable dt = SqlHelper.Instance.GetTableByQuery(Qry);

            if (dt.Rows.Count > 0)
            {
                lstEmpIdName.DataSource = dt;
                lstEmpIdName.DataTextField = "empname";
                lstEmpIdName.DataValueField = "empid";
                lstEmpIdName.DataBind();
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
        }

        protected void GvSearchEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvSearchEmp.PageIndex = e.NewPageIndex;

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int Fontsize = 10;
            int Fontsize2 = 10;
            string fontstyle = "Calibri";

            List<String> EmpId_list = new List<string>();

            int totalfonts = FontFactory.RegisterDirectory("c:\\WINDOWS\\fonts");
            StringBuilder sa = new StringBuilder();
            foreach (string fontname in FontFactory.RegisteredFonts)
            {
                sa.Append(fontname + "\n");
            }


            Font FontStyle1 = FontFactory.GetFont("Perpetua Titling MT", BaseFont.CP1252, BaseFont.EMBEDDED, Fontsize - 3, Font.BOLD, BaseColor.BLACK);


            var list = new List<string>();

            for (int i = 0; i < lstEmpIdName.Items.Count; i++)
            {


                if (lstEmpIdName.Items[i].Selected == true)
                {
                    list.Add("'" + lstEmpIdName.Items[i].Value + "'");
                }
            }


            string empids = string.Join(",", list.ToArray());



            #region for Variable Declaration

            string Title = "";
            string Empid = "";
            string Name = "";
            string Designation = "";
            string IDcardIssued = "";
            string IDcardvalid = "";
            string BloodGroup = "";
            string prTown = "";
            string prPostOffice = "";
            string prTaluka = "";
            string statessndcity = "";
            string prPoliceStation = "";
            string prcity = "";
            string prphone = "";
            string prlmark = "";
            string prLmark = "";
            string prPincode = "";
            string prState = "";
            string State = "";
            string address1 = "";
            string Image = "";
            string EmpSign = "";
            string empdob = "";
            string empdoj = "";

            #endregion for Variable Declaration


            string QueryCompanyInfo = "select * from companyinfo";
            DataTable DtCompanyInfo = SqlHelper.Instance.GetTableByQuery(QueryCompanyInfo);

            string CompanyName = "";
            string Address = "";
            string address = "";
            string companyinfo = "";
            string EmpDtofLeaving = "";
            string IDCardValid = "";
            string peTaluka = "";
            string peTown = "";
            string peLmark = "";
            string pearea = "";
            string pecity = "";
            string peDistrict = "";
            string pePincode = "";
            string addres1 = "";
            string peState = "";
            string pelmark = "";
            string branch = "";
            string pestreet = "";
            string pePostOffice = "";
            string pephone = "";
            string pePoliceStation = "";
            string Emailid = "";
            string Website = "";
            string comphone = "";
            string empsex = "";
            if (DtCompanyInfo.Rows.Count > 0)
            {
                CompanyName = DtCompanyInfo.Rows[0]["CompanyName"].ToString();
                Address = DtCompanyInfo.Rows[0]["Address"].ToString();
                companyinfo = DtCompanyInfo.Rows[0]["CompanyInfo"].ToString();
                Emailid = DtCompanyInfo.Rows[0]["Emailid"].ToString();
                Website = DtCompanyInfo.Rows[0]["Website"].ToString();
                comphone = DtCompanyInfo.Rows[0]["Phoneno"].ToString();

            }

            string query = "";
            DataTable dt = new DataTable();




            query = "select Empid,(EmpFName+' '+EmpMName+''+EmpLName) as Fullname,D.Design as EmpDesgn,empsex,prPostOffice,prPincode,(States.State+Cities.City) as statessndcity,(prTaluka+prPostOffice) as address1,EmpDetails.prLmark,prphone,prState,prcity,EmpDetails.prTaluka,EmpDetails.prTown,States.State,Cities.City,EmpDetails.prPincode,EmpPermanentAddress,(EmpDetails.prcity+EmpDetails.prLmark+EmpDetails.prTaluka+EmpDetails.prTown+States.State+Cities.City+EmpDetails.prPincode+EmpDetails.EmpPresentAddress) as address ," +
                "case convert(varchar(10),EmpDtofBirth,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofBirth,103) end EmpDtofBirth ," +
                "case convert(varchar(10),EmpDtofJoining,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofJoining,103) end EmpDtofJoining ," +
                "case convert(varchar(10),EmpDtofLeaving,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofLeaving,103) end EmpDtofLeaving ," +
                "case convert(varchar(10),IDCardIssued,103) when '01/01/1900' then '' else convert(varchar(10),IDCardIssued,103) end IDCardIssued ," +
                "case convert(varchar(10),IDCardValid,103) when '01/01/1900' then '' else convert(varchar(10),IDCardValid,103) end IDCardValid ," +
                "Image,EmpSign,BN.BloodGroupName as EmpBloodGroup from EmpDetails " +
                         " inner join designations D on D.Designid=EmpDetails.EmpDesgn " +
                         " left join BloodGroupNames BN on BN.BloodGroupId=EmpDetails.EmpBloodGroup left join Cities on  Cities.CityID= EmpDetails.prCity       LEFT JOIN States on States.StateID=EmpDetails.prState      " +
                         "left join branch b on b.branchid=empdetails.branch" +
                         " where empid  in (" + empids + ")  order by empid";
            dt = SqlHelper.Instance.GetTableByQuery(query);

            if (dt.Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream();

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("~/assets/EmpPhotos/");
                string imagepath2 = Server.MapPath("~/assets/Images/sign.jpg");
                string imagepath5 = Server.MapPath("~/assets/" + CmpIDPrefix + "Billlogo.png");

                string imagepath6 = Server.MapPath("~/assets/Empsign/");

                Document document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                #region for range ID Card Display


                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    prlmark = "";
                    prTaluka = "";
                    prTown = "";
                    prphone = "";
                    prcity = "";
                    prPincode = "";
                    peState = "";
                    prPostOffice = "";

                    Empid = dt.Rows[k]["Empid"].ToString();
                    Name = dt.Rows[k]["Fullname"].ToString();
                    empsex = dt.Rows[k]["empsex"].ToString();
                    if (empsex == "M")
                    {
                        Title = "Mr";
                    }
                    else
                    {
                        Title = "Ms";
                    }

                    Designation = dt.Rows[k]["EmpDesgn"].ToString();
                    IDcardIssued = dt.Rows[k]["IDCardIssued"].ToString();
                    IDcardvalid = dt.Rows[k]["IDCardValid"].ToString();
                    BloodGroup = dt.Rows[k]["EmpBloodGroup"].ToString();
                    Image = dt.Rows[k]["Image"].ToString();
                    EmpSign = dt.Rows[k]["EmpSign"].ToString();
                    empdob = dt.Rows[k]["EmpDtofBirth"].ToString();
                    empdoj = dt.Rows[k]["EmpDtofJoining"].ToString();
                    address = dt.Rows[k]["address"].ToString();
                    prlmark = dt.Rows[k]["prLmark"].ToString();
                    prTaluka = dt.Rows[k]["prTaluka"].ToString();
                    prTown = dt.Rows[k]["prTown"].ToString();
                    prphone = dt.Rows[k]["prphone"].ToString();
                    prcity = dt.Rows[k]["City"].ToString();
                    prPincode = dt.Rows[k]["prPincode"].ToString();
                    peState = dt.Rows[k]["State"].ToString();
                    prPostOffice = dt.Rows[k]["prPostOffice"].ToString();
                    Emailid = DtCompanyInfo.Rows[0]["Emailid"].ToString();
                    Website = DtCompanyInfo.Rows[0]["Website"].ToString();
                    address1 = dt.Rows[k]["address1"].ToString();
                    State = dt.Rows[k]["State"].ToString();
                    prPincode = dt.Rows[k]["prPincode"].ToString();
                    EmpDtofLeaving = dt.Rows[k]["EmpDtofLeaving"].ToString();



                    PdfPTable IDCarddetails = new PdfPTable(10);
                    IDCarddetails.TotalWidth = 380f;
                    IDCarddetails.LockedWidth = true;
                    float[] width = new float[] { 5f, 2f, 2f, 2f, 0.2f, 5f, 2f, 2f, 2f, 2.4f };
                    IDCarddetails.SetWidths(width);

                    #region for sub table

                    PdfPTable IDCardTemptable1 = new PdfPTable(4);
                    IDCardTemptable1.TotalWidth = 180f;
                    IDCardTemptable1.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    IDCardTemptable1.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
                    //IDCardTemptable1.LockedWidth = true;
                    float[] width1 = new float[] { 2.4f, 2.4f, 2.4f, 2.4f };

                    //float[] width1 = new float[] { 5f, 2f, 2f, 2f };
                    IDCardTemptable1.SetWidths(width1);
                    #region For 1st


                    BaseColor color = new BaseColor(0, 0, 0);



                    if (File.Exists(imagepath5))
                    {
                        iTextSharp.text.Image srflogo = iTextSharp.text.Image.GetInstance(imagepath5);
                        srflogo.ScaleAbsolute(80f, 50f);
                        PdfPCell companylogo = new PdfPCell();
                        Paragraph cmplogo = new Paragraph();
                        cmplogo.Add(new Chunk(srflogo, 50f, 0f));
                        companylogo.AddElement(cmplogo);
                        companylogo.HorizontalAlignment = 0;
                        companylogo.Colspan = 4;
                        companylogo.PaddingLeft = -15;
                        companylogo.Border = 0;
                        companylogo.PaddingTop = 20;
                        IDCardTemptable1.AddCell(companylogo);
                    }
                    else
                    {
                        PdfPCell companylogo = new PdfPCell();
                        companylogo.HorizontalAlignment = 0;
                        companylogo.Colspan = 4;
                        companylogo.Border = 0;
                        companylogo.FixedHeight = 45;
                        IDCardTemptable1.AddCell(companylogo);
                    }
                    #region commented code for address

                    Font FontStyle2 = FontFactory.GetFont("Perpetua Titling MT", BaseFont.CP1252, BaseFont.EMBEDDED, Fontsize - 3, Font.BOLD, BaseColor.BLACK);

                    PdfPCell cellCertification = new PdfPCell(new Phrase(CompanyName, FontFactory.GetFont(fontstyle, Fontsize - 1, Font.BOLD, color)));
                    cellCertification.HorizontalAlignment = 1;
                    cellCertification.Border = 0;
                    cellCertification.Colspan = 4;
                    cellCertification.PaddingLeft = -5f;
                    IDCardTemptable1.AddCell(cellCertification);





                    #endregion commented code for address

                    if (Image.Length > 0)
                    {
                        iTextSharp.text.Image Empphoto = iTextSharp.text.Image.GetInstance(imagepath1 + Image);
                        //Empphoto.ScalePercent(25f);
                        Empphoto.ScaleAbsolute(70f, 80f);
                        PdfPCell EmpImage = new PdfPCell();
                        Paragraph Emplogo = new Paragraph();
                        Emplogo.Add(new Chunk(Empphoto, 50f, 0));
                        EmpImage.AddElement(Emplogo);
                        EmpImage.HorizontalAlignment = 1;
                        EmpImage.Colspan = 4;
                        EmpImage.Border = 0;
                        EmpImage.PaddingLeft = -15;
                        IDCardTemptable1.AddCell(EmpImage);
                    }
                    else
                    {
                        PdfPCell EmpImage = new PdfPCell();
                        EmpImage.HorizontalAlignment = 2;
                        EmpImage.Colspan = 4;
                        EmpImage.Border = 0;
                        EmpImage.FixedHeight = 68;
                        IDCardTemptable1.AddCell(EmpImage);

                    }

                    PdfPCell cellEmpNameval = new PdfPCell(new Phrase(Title + ". " + Name, FontFactory.GetFont(fontstyle, Fontsize2 + 2, Font.BOLD, color)));
                    cellEmpNameval.HorizontalAlignment = 1;
                    cellEmpNameval.Border = 0;
                    cellEmpNameval.Colspan = 4;
                    // cellEmpNameval.PaddingLeft = -25f;
                    IDCardTemptable1.AddCell(cellEmpNameval);

                    PdfPCell cellempcode = new PdfPCell(new Phrase("ID No	          ", FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    cellempcode.HorizontalAlignment = 0;
                    cellempcode.Border = 0;
                    cellempcode.Colspan = 2;
                    IDCardTemptable1.AddCell(cellempcode);

                    PdfPCell cellempcodeval = new PdfPCell(new Phrase(": " + Empid, FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    cellempcodeval.HorizontalAlignment = 0;
                    cellempcodeval.Border = 0;
                    cellempcodeval.Colspan = 2;
                    cellempcodeval.PaddingLeft = -20f;
                    IDCardTemptable1.AddCell(cellempcodeval);

                    PdfPCell celldesgn = new PdfPCell(new Phrase("Designation  ", FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    celldesgn.HorizontalAlignment = 0;
                    celldesgn.Border = 0;
                    celldesgn.Colspan = 2;
                    IDCardTemptable1.AddCell(celldesgn);

                    PdfPCell celldesgnval = new PdfPCell(new Phrase(": " + Designation, FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    celldesgnval.HorizontalAlignment = 0;
                    celldesgnval.Border = 0;
                    celldesgnval.Colspan = 2;
                    celldesgnval.PaddingLeft = -20f;
                    IDCardTemptable1.AddCell(celldesgnval);

                    PdfPCell dobv = new PdfPCell(new Phrase("DOB	               ", FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    dobv.HorizontalAlignment = 0;
                    dobv.Border = 0;
                    dobv.Colspan = 2;
                    IDCardTemptable1.AddCell(dobv);

                    PdfPCell dobval = new PdfPCell(new Phrase(": " + empdob, FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    dobval.HorizontalAlignment = 0;
                    dobval.Border = 0;
                    dobval.Colspan = 2;
                    dobval.PaddingLeft = -20f;
                    IDCardTemptable1.AddCell(dobval);

                    PdfPCell space = new PdfPCell(new Phrase("IDCard From	               ", FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    space.HorizontalAlignment = 0;
                    space.Border = 0;
                    space.Colspan = 2;
                    IDCardTemptable1.AddCell(space);

                    PdfPCell spaceval = new PdfPCell(new Phrase(": " + IDcardIssued, FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    spaceval.HorizontalAlignment = 0;
                    spaceval.Border = 0;
                    spaceval.Colspan = 2;
                    spaceval.PaddingLeft = -20f;
                    IDCardTemptable1.AddCell(spaceval);

                    PdfPCell cellBloodGroup = new PdfPCell(new Phrase("IDCard To	              ", FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    cellBloodGroup.HorizontalAlignment = 0;
                    cellBloodGroup.Border = 0;
                    cellBloodGroup.Colspan = 2;
                    IDCardTemptable1.AddCell(cellBloodGroup);

                    PdfPCell cellBloodGroupval = new PdfPCell(new Phrase(": " + IDcardvalid, FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                    cellBloodGroupval.HorizontalAlignment = 0;
                    cellBloodGroupval.Border = 0;
                    cellBloodGroupval.Colspan = 2;
                    cellBloodGroupval.PaddingLeft = -20f;
                    IDCardTemptable1.AddCell(cellBloodGroupval);

                    //if (EmpSign.Length > 0)
                    //{
                    //    iTextSharp.text.Image Sign = iTextSharp.text.Image.GetInstance(imagepath6 + EmpSign);
                    //    Sign.ScalePercent(8f);
                    //    Sign.ScaleAbsolute(60f, 15f);
                    //    PdfPCell Signature = new PdfPCell();
                    //    Paragraph signlogo = new Paragraph();
                    //    signlogo.Add(new Chunk(Sign, 23f, -7f));
                    //    Signature.AddElement(signlogo);
                    //    Signature.HorizontalAlignment = 0;
                    //    Signature.Colspan = 2;
                    //    Signature.PaddingTop = 5;
                    //    Signature.PaddingLeft = -16f;
                    //    Signature.Border = 0;
                    //    IDCardTemptable1.AddCell(Signature);
                    //}
                    //else
                    //{

                    //    PdfPCell Signature = new PdfPCell();
                    //    Signature.HorizontalAlignment = 0;
                    //    Signature.Colspan = 2;
                    //    Signature.PaddingTop = 5;
                    //    Signature.Border = 0;
                    //    Signature.PaddingLeft = -10f;
                    //    Signature.FixedHeight = 20;
                    //    IDCardTemptable1.AddCell(Signature);

                    //}

                    iTextSharp.text.Image IssuingAuth = iTextSharp.text.Image.GetInstance(imagepath2);
                    // IssuingAuth.ScalePercent(50f);
                    IssuingAuth.ScaleAbsolute(40f, 20f);
                    PdfPCell Authority = new PdfPCell();
                    Paragraph Authoritylogo = new Paragraph();
                    Authoritylogo.Add(new Chunk(IssuingAuth, 45f, -4f));
                    Authority.AddElement(Authoritylogo);
                    //Authority.HorizontalAlignment = 1;
                    Authority.HorizontalAlignment = Element.ALIGN_CENTER;
                    Authority.Colspan = 4;
                    Authority.Border = 0;
                    Authority.PaddingLeft = 0;
                    //Authority.PaddingTop = -12;
                    IDCardTemptable1.AddCell(Authority);

                    // PdfPCell cellempsignature = new PdfPCell(new Phrase("Employee Signature ", FontFactory.GetFont(fontstyle, Fontsize -2, Font.NORMAL, color)));
                    // cellempsignature.HorizontalAlignment = 0;
                    // cellempsignature.Border = 0;
                    //// cellempsignature.PaddingTop = 5;
                    // //cellempsignature.PaddingLeft = -7f;
                    // cellempsignature.Colspan = 2;
                    // IDCardTemptable1.AddCell(cellempsignature);

                    PdfPCell cellAuthority = new PdfPCell(new Phrase("Authorised Sign", FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, color)));
                    // cellAuthority.HorizontalAlignment = 1;
                    cellAuthority.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellAuthority.Border = 0;
                    cellAuthority.Colspan = 4;
                    cellAuthority.PaddingLeft = 0;
                    // cellAuthority.PaddingLeft = 70;
                    IDCardTemptable1.AddCell(cellAuthority);

                    #endregion
                    PdfPCell childTable1 = new PdfPCell(IDCardTemptable1);
                    childTable1.HorizontalAlignment = 0;
                    childTable1.Colspan = 4;
                    childTable1.PaddingLeft = 10;
                    IDCarddetails.AddCell(childTable1);
                    #endregion
                    PdfPTable IDCardTemptable41 = new PdfPTable(1);
                    IDCardTemptable41.TotalWidth = 1f;
                    IDCardTemptable41.LockedWidth = true;
                    float[] width41 = new float[] { 0.5f };
                    IDCardTemptable41.SetWidths(width41);
                    #region subtable2

                    PdfPCell cellempcell1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempcell1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellempcell1.Border = 0;
                    cellempcell1.Colspan = 1;
                    IDCardTemptable41.AddCell(cellempcell1);
                    #endregion
                    PdfPCell childTable4 = new PdfPCell(IDCardTemptable41);
                    childTable4.HorizontalAlignment = 0;
                    childTable4.Colspan = 1;
                    childTable4.Border = 0;
                    IDCarddetails.AddCell(childTable4);

                    PdfPTable IDCardTemptable2 = new PdfPTable(4);
                    IDCardTemptable2.TotalWidth = 190f;
                    //IDCardTemptable2.LockedWidth = true;
                    IDCardTemptable2.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    IDCardTemptable2.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
                    float[] width2 = new float[] { 2.3f, 2.3f, 2.3f, 2.3f };
                    IDCardTemptable2.SetWidths(width2);

                    #region for subtable3


                    #region Present address String array

                    string[] PrAdress = new string[8];

                    if (prlmark.Length > 0)
                    {
                        PrAdress[0] = prlmark + ", ";
                    }
                    else
                    {
                        PrAdress[0] = "";
                    }
                    if (prTown.Length > 0)
                    {
                        PrAdress[1] = prTown + ", ";
                    }
                    else
                    {
                        PrAdress[1] = "";
                    }

                    if (prPostOffice.Length > 0)
                    {
                        PrAdress[2] = prPostOffice + ", ";
                    }
                    else
                    {
                        PrAdress[2] = "";
                    }
                    if (prTaluka.Length > 0)
                    {
                        PrAdress[3] = prTaluka + ", ";
                    }
                    else
                    {
                        PrAdress[3] = " ";
                    }
                    if (prPoliceStation.Length > 0)
                    {
                        PrAdress[4] = prPoliceStation + ", ";
                    }
                    else
                    {
                        PrAdress[4] = " ";
                    }
                    if (prcity.Length > 0)
                    {
                        PrAdress[5] = prcity + ", ";
                    }
                    else
                    {
                        PrAdress[5] = " ";
                    }

                    if (prState.Length > 0)
                    {
                        PrAdress[6] = prState + " ";
                    }
                    else
                    {
                        PrAdress[6] = ".";
                    }


                    if (prPincode.Length > 0)
                    {
                        PrAdress[7] = prPincode + ".";
                    }
                    else
                    {
                        PrAdress[7] = "";
                    }

                    string Address2 = string.Empty;

                    for (int i = 0; i < 8; i++)
                    {
                        address += PrAdress[i];
                    }


                    #endregion

                    PdfPCell Instructions = new PdfPCell(new Phrase("INSTRUCTIONS:", FontFactory.GetFont(fontstyle, Fontsize, Font.UNDERLINE | Font.BOLD, color)));
                    Instructions.HorizontalAlignment = 1;
                    Instructions.Border = 0;
                    Instructions.Colspan = 4;
                    Instructions.PaddingLeft = -20;
                    Instructions.PaddingTop = 20;
                    IDCardTemptable2.AddCell(Instructions);

                    PdfPCell Instructions1 = new PdfPCell(new Phrase("1) This card is a property of Blue i\n    Enterprises\n" +
                    "2) This card cannot be transferred\n" +
                    "3) Misuse of this card is prohibited\n" +
                    "4) If card found please contact the\n     Below address:", FontFactory.GetFont(fontstyle, Fontsize - 1, Font.NORMAL, color)));
                    Instructions1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    Instructions1.Border = 0;
                    Instructions1.Colspan = 4;
                    Instructions1.PaddingLeft = -15f;
                    Instructions1.SetLeading(0f, 1.2f);
                    IDCardTemptable2.AddCell(Instructions1);



                    PdfPCell cellbloodgrp = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize, Font.BOLD, color)));
                    cellbloodgrp.HorizontalAlignment = 0;
                    cellbloodgrp.Border = 0;
                    cellbloodgrp.Colspan = 4;
                    cellbloodgrp.FixedHeight = 115f;
                    IDCardTemptable2.AddCell(cellbloodgrp);

                    PdfPCell cellcccomp = new PdfPCell(new Phrase(CompanyName, FontFactory.GetFont(fontstyle, Fontsize - 1, Font.BOLD, color)));
                    cellcccomp.HorizontalAlignment = 1;
                    cellcccomp.Border = 0;
                    cellcccomp.Colspan = 4;
                    cellcccomp.PaddingLeft = -15f;
                    IDCardTemptable2.AddCell(cellcccomp);

                    PdfPCell cellcccompadd = new PdfPCell(new Phrase(Address, FontFactory.GetFont(fontstyle, Fontsize - 3, Font.NORMAL, color)));
                    cellcccompadd.HorizontalAlignment = 1;
                    cellcccompadd.Border = 0;
                    cellcccompadd.Colspan = 4;
                    cellcccompadd.PaddingLeft = -21f;
                    IDCardTemptable2.AddCell(cellcccompadd);

                    //PdfPCell cellDtIssued = new PdfPCell(new Phrase(Emailid, FontFactory.GetFont(fontstyle, Fontsize - 1, Font.NORMAL, color)));
                    //cellDtIssued.HorizontalAlignment = 1;
                    //cellDtIssued.Border = 0;
                    //cellDtIssued.Colspan = 4;
                    ////cellDtIssued.PaddingTop = 5;
                    //cellDtIssued.PaddingLeft = -15f;
                    //IDCardTemptable2.AddCell(cellDtIssued);

                    PdfPCell cellDtIssuedval = new PdfPCell(new Phrase(Website, FontFactory.GetFont(fontstyle, Fontsize - 1, Font.NORMAL, color)));
                    cellDtIssuedval.HorizontalAlignment = 1;
                    cellDtIssuedval.Border = 0;
                    cellDtIssuedval.Colspan = 4;
                    //cellDtIssuedval.PaddingTop = 10;
                    cellDtIssuedval.PaddingLeft = -15f;
                    IDCardTemptable2.AddCell(cellDtIssuedval);


                    #endregion for sub table

                    PdfPCell childTable2 = new PdfPCell(IDCardTemptable2);
                    childTable2.HorizontalAlignment = 0;
                    childTable2.Colspan = 4;
                    childTable2.PaddingLeft = 20;
                    IDCarddetails.AddCell(childTable2);


                    PdfPTable IDCardTemptable31 = new PdfPTable(1);
                    IDCardTemptable31.TotalWidth = 2f;
                    IDCardTemptable31.LockedWidth = true;
                    float[] width31 = new float[] { 1f };
                    IDCardTemptable31.SetWidths(width31);

                    PdfPCell cellempcell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cellempcell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellempcell.Border = 0;
                    cellempcell.Colspan = 1;
                    cellempcell.PaddingBottom = 80;
                    IDCardTemptable31.AddCell(cellempcell);

                    PdfPCell childTable3 = new PdfPCell(IDCardTemptable31);
                    childTable3.HorizontalAlignment = 0;
                    childTable3.Colspan = 1;
                    childTable3.Border = 0;
                    childTable3.PaddingBottom = 30;
                    IDCarddetails.AddCell(childTable3);

                    PdfPCell childTable6 = new PdfPCell();
                    childTable6.HorizontalAlignment = 0;
                    childTable6.Colspan = 10;
                    childTable6.Border = 0;
                    childTable3.PaddingBottom = 10;
                    //childTable6.PaddingBottom = 10;
                    IDCarddetails.AddCell(childTable6);


                    PdfPCell empcellnew1 = new PdfPCell();
                    empcellnew1.HorizontalAlignment = 0;
                    empcellnew1.Colspan = 10;
                    empcellnew1.Border = 0;
                    IDCarddetails.AddCell(empcellnew1);



                    #endregion for range ID Card Display



                    document.Add(IDCarddetails);


                }

                document.Close();

                //document.Add(MainIDCarddetails);
                //document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=IDCard.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }

        }

        protected void BtnIDCard_Click(object sender, EventArgs e)
        {
            try
            {


                int Fontsize = 10;
                int Fontsize2 = 10;
                string fontstyle = "TimesNewRoman";
                string fontstyle1 = "Arial";

                List<String> EmpId_list = new List<string>();

                int totalfonts = FontFactory.RegisterDirectory("c:\\WINDOWS\\fonts");
                StringBuilder sa = new StringBuilder();
                foreach (string fontname in FontFactory.RegisteredFonts)
                {
                    sa.Append(fontname + "\n");
                }


                Font FontStyle1 = FontFactory.GetFont("Perpetua Titling MT", BaseFont.CP1252, BaseFont.EMBEDDED, Fontsize - 3, Font.BOLD, BaseColor.BLACK);


                var list = new List<string>();

                for (int i = 0; i < lstEmpIdName.Items.Count; i++)
                {


                    if (lstEmpIdName.Items[i].Selected == true)
                    {
                        list.Add("'" + lstEmpIdName.Items[i].Value + "'");
                    }
                }


                string empids = string.Join(",", list.ToArray());



                #region for Variable Declaration

                string Title = "";
                string Empid = "";
                string Name = "";
                string Designation = "";
                string IDcardIssued = "";
                string IDcardvalid = "";
                string BloodGroup = "";
                string prTown = "";
                string prPostOffice = "";
                string prTaluka = "";
                string statessndcity = "";
                string prPoliceStation = "";
                string prcity = "";
                string prphone = "";
                string prlmark = "";
                string prLmark = "";
                string prPincode = "";
                string prState = "";
                string State = "";
                string address1 = "";
                string Image = "";
                string EmpSign = "";
                string empdob = "";
                string empdoj = "";

                #endregion for Variable Declaration


                string QueryCompanyInfo = "select * from companyinfo";
                DataTable DtCompanyInfo = SqlHelper.Instance.GetTableByQuery(QueryCompanyInfo);

                string CompanyName = "";
                string Address = "";
                string address = "";
                string companyinfo = "";
                string EmpDtofLeaving = "";
                string peState = "";
                string Emailid = "";
                string Website = "";
                string comphone = "";
                string empsex = "";
                if (DtCompanyInfo.Rows.Count > 0)
                {
                    CompanyName = DtCompanyInfo.Rows[0]["CompanyName"].ToString();
                    Address = DtCompanyInfo.Rows[0]["Address"].ToString();
                    companyinfo = DtCompanyInfo.Rows[0]["CompanyInfo"].ToString();
                    Emailid = DtCompanyInfo.Rows[0]["Emailid"].ToString();
                    Website = DtCompanyInfo.Rows[0]["Website"].ToString();
                    comphone = DtCompanyInfo.Rows[0]["Phoneno"].ToString();
                    Emailid = DtCompanyInfo.Rows[0]["Emailid"].ToString();
                    Website = DtCompanyInfo.Rows[0]["Website"].ToString();
                }

                string query = "";
                DataTable dt = new DataTable();




                query = "select Empid,(EmpFName) as Fullname,D.Design as EmpDesgn,empsex,prPostOffice,prPincode,(States.State+Cities.City) as statessndcity,(prTaluka+prPostOffice) as address1,EmpDetails.prLmark,prphone,prState,prcity,EmpDetails.prTaluka,EmpDetails.prTown,States.State,Cities.City,EmpDetails.prPincode,EmpPermanentAddress,(EmpDetails.prcity+EmpDetails.prLmark+EmpDetails.prTaluka+EmpDetails.prTown+States.State+Cities.City+EmpDetails.prPincode+EmpDetails.EmpPresentAddress) as address ," +
                    "case convert(varchar(10),EmpDtofBirth,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofBirth,103) end EmpDtofBirth ," +
                    "case convert(varchar(10),EmpDtofJoining,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofJoining,103) end EmpDtofJoining ," +
                    "case convert(varchar(10),EmpDtofLeaving,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofLeaving,103) end EmpDtofLeaving ," +
                    "case convert(varchar(10),IDCardIssued,103) when '01/01/1900' then '' else convert(varchar(10),IDCardIssued,103) end IDCardIssued ," +
                    "case convert(varchar(10),IDCardValid,103) when '01/01/1900' then '' else convert(varchar(10),IDCardValid,103) end IDCardValid ," +
                    "Image,EmpSign,BN.BloodGroupName as EmpBloodGroup from EmpDetails " +
                             " inner join designations D on D.Designid=EmpDetails.EmpDesgn " +
                             " left join BloodGroupNames BN on BN.BloodGroupId=EmpDetails.EmpBloodGroup left join Cities on  Cities.CityID= EmpDetails.prCity       LEFT JOIN States on States.StateID=EmpDetails.prState      " +
                             "left join branch b on b.branchid=empdetails.branch" +
                             " where empid  in (" + empids + ")  order by empid";
                dt = SqlHelper.Instance.GetTableByQuery(query);

                if (dt.Rows.Count > 0)
                {
                    MemoryStream ms = new MemoryStream();

                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    string imagepath1 = Server.MapPath("~/assets/EmpPhotos/");
                    string imagepath2 = Server.MapPath("~/assets/Images/sign.jpg");
                    string imagepath5 = Server.MapPath("~/assets/" + CmpIDPrefix + "Billlogo.png");

                    string imagepath6 = Server.MapPath("~/assets/Stamp.png");
                    string imagepath7 = Server.MapPath("~/assets/NoPhoto.jpg");

                    Document document = new Document(PageSize.A4.Rotate());
                    var writer = PdfWriter.GetInstance(document, ms);
                    document.Open();



                    PdfPTable IDCarddetails = new PdfPTable(25);
                    IDCarddetails.TotalWidth = 830f;
                    IDCarddetails.LockedWidth = true;
                    float[] width = new float[] { 5f, 2f, 2f, 2f, 0.7f, 5f, 2f, 2f, 2f, 0.7f, 5f, 2f, 2f, 2f, 0.7f, 5f, 2f, 2f, 2f, 0.7f, 5f, 2f, 2f, 2f, 0.2f };
                    IDCarddetails.SetWidths(width);


                    PdfPTable IDCarddetails2 = new PdfPTable(25);
                    IDCarddetails2.TotalWidth = 830f;
                    IDCarddetails2.LockedWidth = true;
                    float[] widths = new float[] { 5f, 2f, 2f, 2f, 0.7f, 5f, 2f, 2f, 2f, 0.7f, 5f, 2f, 2f, 2f, 0.7f, 5f, 2f, 2f, 2f, 0.7f, 5f, 2f, 2f, 2f, 0.2f };
                    IDCarddetails2.SetWidths(widths);

                    BaseColor color = new BaseColor(0, 0, 0);

                    #region for first part

                    for (int k = 0; k < dt.Rows.Count; k++)
                    {

                        if (k % 5 == 0)
                        {
                            PdfPCell empcellnew11 = new PdfPCell();
                            empcellnew11.HorizontalAlignment = 0;
                            empcellnew11.Colspan = 25;
                            empcellnew11.FixedHeight = 7;
                            empcellnew11.Border = 0;
                            IDCarddetails.AddCell(empcellnew11);
                        }



                        prlmark = "";
                        prTaluka = "";
                        prTown = "";
                        prphone = "";
                        prcity = "";
                        prPincode = "";
                        peState = "";
                        prPostOffice = "";

                        Empid = dt.Rows[k]["Empid"].ToString();
                        Name = dt.Rows[k]["Fullname"].ToString();
                        empsex = dt.Rows[k]["empsex"].ToString();
                        if (empsex == "M")
                        {
                            Title = "Mr";
                        }
                        else
                        {
                            Title = "Ms";
                        }

                        Designation = dt.Rows[k]["EmpDesgn"].ToString().Substring(0, 1) + dt.Rows[k]["EmpDesgn"].ToString().Substring(1).ToLower();
                        IDcardIssued = dt.Rows[k]["IDCardIssued"].ToString();
                        IDcardvalid = dt.Rows[k]["IDCardValid"].ToString();
                        BloodGroup = dt.Rows[k]["EmpBloodGroup"].ToString();
                        Image = dt.Rows[k]["Image"].ToString();
                        EmpSign = dt.Rows[k]["EmpSign"].ToString();
                        empdob = dt.Rows[k]["EmpDtofBirth"].ToString();
                        empdoj = dt.Rows[k]["EmpDtofJoining"].ToString();
                        address = dt.Rows[k]["address"].ToString();
                        prlmark = dt.Rows[k]["prLmark"].ToString();
                        prTaluka = dt.Rows[k]["prTaluka"].ToString();
                        prTown = dt.Rows[k]["prTown"].ToString();
                        prphone = dt.Rows[k]["prphone"].ToString();
                        prcity = dt.Rows[0]["City"].ToString();
                        prPincode = dt.Rows[0]["prPincode"].ToString();
                        peState = dt.Rows[k]["State"].ToString();
                        prPostOffice = dt.Rows[k]["prPostOffice"].ToString();

                        address1 = dt.Rows[k]["address1"].ToString();
                        State = dt.Rows[k]["State"].ToString();
                        prPincode = dt.Rows[k]["prPincode"].ToString();
                        EmpDtofLeaving = dt.Rows[k]["EmpDtofLeaving"].ToString();

                        PdfPTable IDCardTemptable1 = new PdfPTable(4);
                        IDCardTemptable1.TotalWidth = 212f;
                        IDCardTemptable1.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        IDCardTemptable1.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
                        float[] width1 = new float[] { 5.6f, 2.4f, 2.4f, 2.4f };
                        IDCardTemptable1.SetWidths(width1);

                        if (File.Exists(imagepath5))
                        {
                            iTextSharp.text.Image srflogo = iTextSharp.text.Image.GetInstance(imagepath5);
                            srflogo.ScaleAbsolute(85f, 70f);
                            PdfPCell companylogo = new PdfPCell();
                            Paragraph cmplogo = new Paragraph();
                            cmplogo.Add(new Chunk(srflogo, 50f, 0f));
                            companylogo.AddElement(cmplogo);
                            companylogo.HorizontalAlignment = 0;
                            companylogo.Colspan = 4;
                            companylogo.PaddingLeft = -25;
                            companylogo.Border = 0;
                            IDCardTemptable1.AddCell(companylogo);
                        }
                        else
                        {
                            PdfPCell companylogo = new PdfPCell();
                            companylogo.HorizontalAlignment = 0;
                            companylogo.Colspan = 4;
                            companylogo.Border = 0;
                            companylogo.FixedHeight = 45;
                            IDCardTemptable1.AddCell(companylogo);
                        }

                        #region commented code for address

                        Font FontStyle2 = FontFactory.GetFont("Perpetua Titling MT", BaseFont.CP1252, BaseFont.EMBEDDED, Fontsize - 3, Font.BOLD, BaseColor.BLACK);

                        PdfPCell cellCertification = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle1, Fontsize - 2, Font.BOLD, color)));
                        cellCertification.HorizontalAlignment = 1;
                        cellCertification.Border = 0;
                        cellCertification.Colspan = 4;
                        cellCertification.PaddingLeft = -18f;
                        //IDCardTemptable1.AddCell(cellCertification);


                        #endregion commented code for address

                        if (Image.Length > 0)
                        {
                            iTextSharp.text.Image Empphoto = iTextSharp.text.Image.GetInstance(imagepath1 + Image);
                            //Empphoto.ScalePercent(25f);
                            Empphoto.ScaleAbsolute(50f, 60f);
                            PdfPCell EmpImage = new PdfPCell();
                            Paragraph Emplogo = new Paragraph();
                            Emplogo.Add(new Chunk(Empphoto, 60f, 0));
                            EmpImage.AddElement(Emplogo);
                            EmpImage.HorizontalAlignment = 1;
                            EmpImage.Colspan = 4;
                            EmpImage.Border = 0;
                            EmpImage.PaddingLeft = -20;
                            IDCardTemptable1.AddCell(EmpImage);
                        }
                        else
                        {
                            iTextSharp.text.Image Empphoto = iTextSharp.text.Image.GetInstance(imagepath7);
                            //Empphoto.ScalePercent(25f);
                            Empphoto.ScaleAbsolute(50f, 60f);
                            PdfPCell EmpImage = new PdfPCell();
                            Paragraph Emplogo = new Paragraph();
                            Emplogo.Add(new Chunk(Empphoto, 60f, 0));
                            EmpImage.AddElement(Emplogo);
                            EmpImage.HorizontalAlignment = 1;
                            EmpImage.Colspan = 4;
                            EmpImage.Border = 0;
                            EmpImage.PaddingLeft = -20;
                            EmpImage.PaddingTop = 2;
                            IDCardTemptable1.AddCell(EmpImage);

                        }

                        PdfPCell cellEmpNameval = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize2 - 1, Font.BOLD, color)));
                        cellEmpNameval.HorizontalAlignment = 1;
                        cellEmpNameval.Border = 0;
                        cellEmpNameval.Colspan = 4;
                        cellEmpNameval.MinimumHeight = 5;
                        IDCardTemptable1.AddCell(cellEmpNameval);

                        cellEmpNameval = new PdfPCell(new Phrase(Name, FontFactory.GetFont(fontstyle, Fontsize2 - 1, Font.BOLD, color)));
                        cellEmpNameval.HorizontalAlignment = 1;
                        cellEmpNameval.Border = 0;
                        cellEmpNameval.Colspan = 4;
                        IDCardTemptable1.AddCell(cellEmpNameval);

                        PdfPCell cellempcode = new PdfPCell(new Phrase("ID No           ", FontFactory.GetFont(fontstyle, Fontsize2 - 1, Font.NORMAL, color)));
                        cellempcode.HorizontalAlignment = 0;
                        cellempcode.Border = 0;
                        cellempcode.Colspan = 2;
                        IDCardTemptable1.AddCell(cellempcode);

                        PdfPCell cellempcodeval = new PdfPCell(new Phrase(": " + Empid, FontFactory.GetFont(fontstyle, Fontsize2 - 1, Font.NORMAL, color)));
                        cellempcodeval.HorizontalAlignment = 0;
                        cellempcodeval.Border = 0;
                        cellempcodeval.Colspan = 2;
                        cellempcodeval.PaddingLeft = -40f;
                        IDCardTemptable1.AddCell(cellempcodeval);

                        PdfPCell celldesgn = new PdfPCell(new Phrase("Design  ", FontFactory.GetFont(fontstyle, Fontsize2 - 1, Font.NORMAL, color)));
                        celldesgn.HorizontalAlignment = 0;
                        celldesgn.Border = 0;
                        celldesgn.Colspan = 2;
                        IDCardTemptable1.AddCell(celldesgn);

                        PdfPCell celldesgnval = new PdfPCell(new Phrase(": " + Designation, FontFactory.GetFont(fontstyle, Fontsize2 - 1, Font.NORMAL, color)));
                        celldesgnval.HorizontalAlignment = 0;
                        celldesgnval.Border = 0;
                        celldesgnval.Colspan = 2;
                        celldesgnval.PaddingLeft = -40f;
                        IDCardTemptable1.AddCell(celldesgnval);

                        PdfPCell dobv = new PdfPCell(new Phrase("Blood Group	               ", FontFactory.GetFont(fontstyle, Fontsize2 - 1, Font.NORMAL, color)));
                        dobv.HorizontalAlignment = 0;
                        dobv.Border = 0;
                        dobv.Colspan = 2;
                        IDCardTemptable1.AddCell(dobv);

                        PdfPCell dobval = new PdfPCell(new Phrase(": " + BloodGroup, FontFactory.GetFont(fontstyle, Fontsize2 - 1, Font.NORMAL, color)));
                        dobval.HorizontalAlignment = 0;
                        dobval.Border = 0;
                        dobval.Colspan = 2;
                        dobval.PaddingLeft = -40f;
                        IDCardTemptable1.AddCell(dobval);

                        PdfPCell spaceSS = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize2, Font.NORMAL, color)));
                        spaceSS.HorizontalAlignment = 0;
                        spaceSS.Border = 0;
                        spaceSS.MinimumHeight = 10;
                        spaceSS.Colspan = 2;
                        IDCardTemptable1.AddCell(spaceSS);

                        iTextSharp.text.Image IssuingAuth = iTextSharp.text.Image.GetInstance(imagepath2);
                        IssuingAuth.ScalePercent(40f);
                        PdfPCell Authority = new PdfPCell();
                        Paragraph Authoritylogo = new Paragraph();
                        Authoritylogo.Add(new Chunk(IssuingAuth, 0f, 5f));
                        Authority.AddElement(Authoritylogo);
                        //Authority.HorizontalAlignment = 1;
                        Authority.HorizontalAlignment = 1;
                        Authority.Colspan = 2;
                        Authority.Border = 0;
                        Authority.PaddingLeft = 0;
                        IDCardTemptable1.AddCell(Authority);

                        PdfPCell cellAuthority = new PdfPCell(new Phrase("Authorised Sign", FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, color)));
                        cellAuthority.HorizontalAlignment = 2;
                        cellAuthority.Border = 0;
                        cellAuthority.Colspan = 4;
                        cellAuthority.PaddingLeft = 0;
                        cellAuthority.PaddingTop = -10;
                        cellAuthority.PaddingLeft = 25;
                        IDCardTemptable1.AddCell(cellAuthority);

                        cellAuthority = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, color)));
                        cellAuthority.HorizontalAlignment = 1;
                        cellAuthority.Border = 0;
                        cellAuthority.Colspan = 4;
                        cellAuthority.MinimumHeight = 10f;
                        cellAuthority.PaddingLeft = 25;
                        IDCardTemptable1.AddCell(cellAuthority);


                        PdfPCell childTable1 = new PdfPCell(IDCardTemptable1);
                        childTable1.HorizontalAlignment = 0;
                        childTable1.Colspan = 4;
                        childTable1.PaddingLeft = 10;
                        IDCarddetails.AddCell(childTable1);



                        #region subtable2

                        PdfPTable IDCardTemptable41 = new PdfPTable(1);
                        IDCardTemptable41.TotalWidth = 2f;
                        IDCardTemptable41.LockedWidth = true;
                        float[] width41 = new float[] { 1f };
                        IDCardTemptable41.SetWidths(width41);


                        PdfPCell cellempcell1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellempcell1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        cellempcell1.Border = 0;
                        cellempcell1.Colspan = 1;
                        IDCardTemptable41.AddCell(cellempcell1);

                        PdfPCell childTable4 = new PdfPCell(IDCardTemptable41);
                        childTable4.HorizontalAlignment = 0;
                        childTable4.Colspan = 1;
                        childTable4.Border = 0;
                        IDCarddetails.AddCell(childTable4);

                        #endregion


                    }


                    PdfPCell empcellnew = new PdfPCell();
                    empcellnew.HorizontalAlignment = 0;
                    empcellnew.Colspan = 5;
                    empcellnew.Border = 0;
                    IDCarddetails.AddCell(empcellnew);

                    empcellnew = new PdfPCell();
                    empcellnew.HorizontalAlignment = 0;
                    empcellnew.Colspan = 5;
                    empcellnew.Border = 0;
                    IDCarddetails.AddCell(empcellnew);

                    IDCarddetails.AddCell(empcellnew);
                    IDCarddetails.AddCell(empcellnew);

                    #endregion for first part


                    document.Add(IDCarddetails);


                    //document.NewPage();

                    for (int k = 0; k < dt.Rows.Count; k++)
                    {

                        PdfPTable IDCardTemptable2 = new PdfPTable(4);
                        IDCardTemptable2.HorizontalAlignment = Element.ALIGN_LEFT;
                        IDCardTemptable2.TotalWidth = 192f;
                        //IDCardTemptable2.LockedWidth = true;
                        float[] width2 = new float[] { 2.3f, 2.3f, 2.3f, 2.3f };
                        IDCardTemptable2.SetWidths(width2);

                        if (k % 5 == 0)
                        {

                            PdfPCell empcellnew11 = new PdfPCell();
                            empcellnew11.HorizontalAlignment = 0;
                            empcellnew11.Colspan = 25;
                            empcellnew11.FixedHeight = 7;
                            empcellnew11.Border = 0;
                            IDCarddetails2.AddCell(empcellnew11);
                        }

                        #region for subtable3


                        PdfPCell Instructions = new PdfPCell(new Phrase("INSTRUCTIONS:", FontFactory.GetFont(fontstyle, Fontsize, Font.BOLD, color)));
                        Instructions.HorizontalAlignment = 1;
                        Instructions.Border = 0;
                        Instructions.Colspan = 4;
                        Instructions.PaddingLeft = -20;
                        Instructions.PaddingTop = 20;
                        IDCardTemptable2.AddCell(Instructions);

                        PdfPCell Instructions1 = new PdfPCell(new Phrase("1) This card must be worn by the holder at all time\n" +
                        "2) Loss or Theft of this ID Card has to be reported to Head Office Immediately", FontFactory.GetFont(fontstyle, Fontsize - 3, Font.NORMAL, color)));
                        Instructions1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        Instructions1.Border = 0;
                        Instructions1.Colspan = 4;
                        Instructions1.PaddingLeft = -10f;
                        Instructions1.SetLeading(0f, 1.2f);
                        IDCardTemptable2.AddCell(Instructions1);



                        PdfPCell cellbloodgrp = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize, Font.BOLD, color)));
                        cellbloodgrp.HorizontalAlignment = 0;
                        cellbloodgrp.Border = 0;
                        cellbloodgrp.Colspan = 4;
                        cellbloodgrp.FixedHeight = 75f;
                        IDCardTemptable2.AddCell(cellbloodgrp);

                        PdfPCell cellcccompaddase = new PdfPCell(new Phrase("In case anyone finds this card,please return to :", FontFactory.GetFont(fontstyle, Fontsize - 3, Font.NORMAL, color)));
                        cellcccompaddase.HorizontalAlignment = 1;
                        cellcccompaddase.Border = 0;
                        cellcccompaddase.Colspan = 4;
                        cellcccompaddase.PaddingLeft = -15f;
                        IDCardTemptable2.AddCell(cellcccompaddase);

                        PdfPCell cellcccomp = new PdfPCell(new Phrase(CompanyName, FontFactory.GetFont(fontstyle1, Fontsize - 2, Font.BOLD, color)));
                        cellcccomp.HorizontalAlignment = 1;
                        cellcccomp.Border = 0;
                        cellcccomp.Colspan = 4;
                        cellcccomp.PaddingLeft = -15f;
                        IDCardTemptable2.AddCell(cellcccomp);



                        PdfPCell cellcccompadd = new PdfPCell(new Phrase(Address, FontFactory.GetFont(fontstyle, Fontsize - 3, Font.NORMAL, color)));
                        cellcccompadd.HorizontalAlignment = 1;
                        cellcccompadd.Border = 0;
                        cellcccompadd.Colspan = 4;
                        cellcccompadd.PaddingLeft = -21f;
                        IDCardTemptable2.AddCell(cellcccompadd);




                        PdfPCell cellDtIssuedval = new PdfPCell(new Phrase("E-mail : " + Emailid, FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, color)));
                        cellDtIssuedval.HorizontalAlignment = 1;
                        cellDtIssuedval.Border = 0;
                        cellDtIssuedval.Colspan = 4;
                        //cellDtIssuedval.PaddingTop = 10;
                        cellDtIssuedval.PaddingLeft = -15f;
                        cellDtIssuedval.PaddingBottom = 12;
                        IDCardTemptable2.AddCell(cellDtIssuedval);

                        PdfPCell cellDtIssuedvalp = new PdfPCell(new Phrase("Ph : " + comphone, FontFactory.GetFont(fontstyle, Fontsize - 2, Font.NORMAL, color)));
                        cellDtIssuedvalp.HorizontalAlignment = 1;
                        cellDtIssuedvalp.Border = 0;
                        cellDtIssuedvalp.Colspan = 4;
                        //cellDtIssuedval.PaddingTop = 10;
                        cellDtIssuedvalp.PaddingLeft = -15f;
                        cellDtIssuedvalp.PaddingBottom = 12;
                        IDCardTemptable2.AddCell(cellDtIssuedvalp);


                        #endregion for sub table

                        PdfPCell childTable2 = new PdfPCell(IDCardTemptable2);
                        childTable2.HorizontalAlignment = 0;
                        childTable2.Colspan = 4;
                        childTable2.PaddingLeft = 20;
                        IDCarddetails2.AddCell(childTable2);


                        PdfPTable IDCardTemptable31 = new PdfPTable(1);
                        IDCardTemptable31.TotalWidth = 2f;
                        float[] width31 = new float[] { 1f };
                        IDCardTemptable31.SetWidths(width31);

                        PdfPCell cellempcell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontstyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                        cellempcell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        cellempcell.Border = 0;
                        cellempcell.Colspan = 1;
                        cellempcell.PaddingBottom = 80;
                        IDCardTemptable31.AddCell(cellempcell);

                        PdfPCell childTable3 = new PdfPCell(IDCardTemptable31);
                        childTable3.HorizontalAlignment = 0;
                        childTable3.Colspan = 1;
                        childTable3.Border = 0;
                        childTable3.PaddingBottom = 30;
                        IDCarddetails2.AddCell(childTable3);


                    }


                    PdfPCell empcellnew1 = new PdfPCell();
                    empcellnew1.HorizontalAlignment = 0;
                    empcellnew1.Colspan = 5;
                    empcellnew1.Border = 0;
                    IDCarddetails2.AddCell(empcellnew1);

                    empcellnew1 = new PdfPCell();
                    empcellnew1.HorizontalAlignment = 0;
                    empcellnew1.Colspan = 5;
                    empcellnew1.Border = 0;
                    IDCarddetails2.AddCell(empcellnew1);
                    IDCarddetails2.AddCell(empcellnew1);
                    IDCarddetails2.AddCell(empcellnew1);

                    document.Add(IDCarddetails2);

                    document.Close();

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=IDCard.pdf");
                    Response.Buffer = true;
                    Response.Clear();
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.End();
                }


            }
            catch (Exception ex)
            {


            }
        }
    }
}