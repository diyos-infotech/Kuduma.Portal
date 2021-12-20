using System;
using System.Web.UI;
using KLTS.Data;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class EmployeeForms : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string Fontstyle = "";
        string FontStyle = "Tahoma";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                       
                        //FillEmployeesList();
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

        protected void ddlForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlForms.SelectedIndex == 0)
            {
                Lblempid.Visible = false;
                lblempname.Visible = false;
                //ddlEmployee.Visible = false;
                //ddlempname.Visible = false;
                txtemplyid.Visible = false;
                txtFname.Visible = false;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
                TxtMonth.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblDOL.Visible = false;
                lblDOJ.Visible = false;
                txtEmpDtofLeaveing.Visible = false;
            }

            if (ddlForms.SelectedIndex == 1)
            {
                Lblempid.Visible = true;
                lblempname.Visible = true;
                //ddlEmployee.Visible = true;
                //ddlempname.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
                TxtMonth.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblDOL.Visible = false;
                lblDOJ.Visible = false;
                txtEmpDtofLeaveing.Visible = false;

            }

            if (ddlForms.SelectedIndex == 2)
            {
                Lblempid.Visible = true;
                lblempname.Visible = true;
                //ddlEmployee.Visible = true;
                //ddlempname.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                lblfrom.Visible = true;
                txtfrom.Visible = true;
                lblto.Visible = true;
                txtto.Visible = true;
                TxtMonth.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblDOL.Visible = false;
                lblDOJ.Visible = false;
                txtEmpDtofLeaveing.Visible = false;

            }

            if (ddlForms.SelectedIndex == 3)
            {
                Lblempid.Visible = true;
                lblempname.Visible = true;
                // ddlEmployee.Visible = true;
                // ddlempname.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
                TxtMonth.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblDOL.Visible = false;
                lblDOJ.Visible = false;
                txtEmpDtofLeaveing.Visible = false;

            }


            if (ddlForms.SelectedIndex == 4)
            {
                Lblempid.Visible = true;
                lblempname.Visible = true;
                // ddlEmployee.Visible = true;
                //ddlempname.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
                TxtMonth.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblDOL.Visible = false;
                txtEmpDtofLeaveing.Visible = false;

            }

            if (ddlForms.SelectedIndex == 5)
            {
                Lblempid.Visible = true;
                lblempname.Visible = true;
                //ddlEmployee.Visible = true;
                //ddlempname.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
                TxtMonth.Visible = true;
                lblmonth.Visible = true;
                lblDOJ.Visible = true;
                txtEmpDtofJoining.Visible = true;
                //lblDOL.Visible = true;
                //txtEmpDtofLeaveing.Visible = true;
            }
            if (ddlForms.SelectedIndex == 6)
            {
                Lblempid.Visible = true;
                lblempname.Visible = true;
                //ddlEmployee.Visible = true;
                //ddlempname.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
                TxtMonth.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblDOL.Visible = false;

                txtEmpDtofLeaveing.Visible = false;
            }
            if (ddlForms.SelectedIndex == 7)
            {
                Lblempid.Visible = true;
                lblempname.Visible = true;
                //ddlEmployee.Visible = true;
                //ddlempname.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
                TxtMonth.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblDOL.Visible = false;
                txtEmpDtofLeaveing.Visible = false;
            }

            if (ddlForms.SelectedIndex == 8)
            {
                Lblempid.Visible = true;
                lblempname.Visible = true;
                //ddlEmployee.Visible = true;
                //ddlempname.Visible = true;
                txtemplyid.Visible = true;
                txtFname.Visible = true;
                lblfrom.Visible = true;
                txtfrom.Visible = true;
                lblto.Visible = true;
                txtto.Visible = true;
                TxtMonth.Visible = false;
                lblmonth.Visible = false;
                txtEmpDtofJoining.Visible = false;
                lblDOL.Visible = false;
                txtEmpDtofLeaveing.Visible = false;
            }
        }

        protected void btnForms_Click(object sender, EventArgs e)
        {

            if (ddlForms.SelectedIndex == 1)
            {
                btnFormQ_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 2)
            {
                btnFormLeaveWages_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 3)
            {
                btngratuity_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 4)
            {
                btnFormA_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 5)
            {
                btnform5_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 6)
            {
                btnform13_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 7)
            {
                btndeclaration_Click(sender, e);
                return;
            }

            if (ddlForms.SelectedIndex == 8)
            {
                btnform3a_Click(sender, e);
                return;
            }
        }

        protected void btnform3a_Click(object sender, EventArgs e)
        {

            int Fontsize = 11;
            string fontsyle = "verdana";


            if (txtemplyid.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
                return;
            }

            //#region  Begin  New Code

            #region  Begin  New Code

            #region Begin Variable Declaration

            string SPName = "";
            Hashtable HTEmpBiodata = new Hashtable();
            string Empid = "";
            DataTable dtEmpdetails = null;

            #endregion end Variable Declaration


            #region Begin Assign Values to The Variables
            SPName = "EmployeeFormsPDF";
            Empid = txtemplyid.Text;

            string date = "";

            var DtofJoining = string.Empty;
            if (txtEmpDtofJoining.Text.Trim().Length != 0)
            {
                DtofJoining = Timings.Instance.CheckDateFormat(txtEmpDtofJoining.Text);

            }
            else
            {
                DtofJoining = "01/01/1900";
            }


            if (TxtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(TxtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = "";
            string Year = "";
            //string month = DateTime.Parse(date).Month.ToString();
            // string Year = DateTime.Parse(date).Year.ToString();


            #endregion End Assign values To the Variables

            #region Begin Pass values to the Hash table
            HTEmpBiodata.Add("@empid", Empid);
            // HTEmpBiodata.Add("@month", month + Year.Substring(2, 2));
            HTEmpBiodata.Add("@DtofJoining", DtofJoining);
            #endregion  end Pass values to the Hash table

            #region Begin  Call Stored Procedure
            dtEmpdetails = config.ExecuteAdaptorAsyncWithParams(SPName, HTEmpBiodata).Result;
            #endregion  End  Call Stored Procedure

            #endregion End New Code As on [31-05-2014]


            string name = "";
            string fathername = "";
            string Gender = "";
            string Companyname = "";
            string CompanyAddress = "";
            string remarks1 = "";
            string accno = "";
            string Empdateofbirth = "";
            string empdesign = "";
            int j = 1;
            if (dtEmpdetails.Rows.Count > 0)
            {

                name = dtEmpdetails.Rows[0]["EmployeeName"].ToString();
                fathername = dtEmpdetails.Rows[0]["EmpFatherName"].ToString();
                empdesign = dtEmpdetails.Rows[0]["Designation"].ToString();
                date = dtEmpdetails.Rows[0]["DtOfJoining"].ToString();
                Empdateofbirth = dtEmpdetails.Rows[0]["DtOfBirth"].ToString();
                Gender = dtEmpdetails.Rows[0]["gender"].ToString();
                Companyname = dtEmpdetails.Rows[0]["CompanyName"].ToString();
                CompanyAddress = dtEmpdetails.Rows[0]["CompanyAddress"].ToString();
                remarks1 = dtEmpdetails.Rows[0]["EmpRemarks"].ToString();
                accno = dtEmpdetails.Rows[0]["EmpBankAcNo"].ToString();
            }


            string strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + txtemplyid.Text + "'";
            string pfNo = "";
            DataTable PfTable = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            if (PfTable.Rows.Count > 0)
            {
                pfNo = PfTable.Rows[0]["EmpEpfNo"].ToString();
            }
            string cmpnypfno = "";
            string qry = "select PFNo from CompanyInfo ";
            DataTable dttblroptns = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dttblroptns.Rows.Count > 0)
            {
                cmpnypfno = dttblroptns.Rows[0]["PFNo"].ToString();

            }



            MemoryStream ms = new MemoryStream();


            Document document = new Document(PageSize.LEGAL);
            // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
            var writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            string imagepath1 = Server.MapPath("images");
            #region

            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            float[] width11 = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
            table.SetWidths(width11);

            PdfPCell heading1 = new PdfPCell(new Phrase("Form-3A (Revised)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            heading1.HorizontalAlignment = 1;
            heading1.Colspan = 8;
            heading1.Border = 0;
            table.AddCell(heading1);


            PdfPCell heading2 = new PdfPCell(new Phrase("Employees provident fund scheme 1952 (Para 35&42) & the employees pension scheme 1995 (Para 19) ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            heading2.HorizontalAlignment = 1;
            heading2.Colspan = 8;
            heading2.Border = 0;
            table.AddCell(heading2);

            PdfPCell heading3 = new PdfPCell(new Phrase("CONTRIBUTION CARD FOR CURRENCY PERIOD FROM", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            heading3.HorizontalAlignment = 1;
            heading3.Colspan = 8;
            heading3.Border = 0;
            table.AddCell(heading3);


            PdfPCell space1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            space1.HorizontalAlignment = 1;
            space1.Colspan = 8;
            space1.Border = 0;
            table.AddCell(space1);

            document.Add(table);


            PdfPTable tablesnewone1 = new PdfPTable(10);
            tablesnewone1.TotalWidth = 500f;
            tablesnewone1.LockedWidth = true;
            float[] width1 = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
            tablesnewone1.SetWidths(width1);


            PdfPTable childtable1 = new PdfPTable(5);
            childtable1.TotalWidth = 250f;
            childtable1.LockedWidth = true;
            float[] childwidth1 = new float[] { 0.7f, 3f, 3f, 3f, 3f };
            childtable1.SetWidths(childwidth1);

            PdfPCell slno1 = new PdfPCell(new Phrase("1.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno1.HorizontalAlignment = 0;
            slno1.Colspan = 1;
            slno1.Border = 0;
            childtable1.AddCell(slno1);

            PdfPCell accountno = new PdfPCell(new Phrase("Account No.:", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            accountno.HorizontalAlignment = 0;
            accountno.Colspan = 2;
            accountno.Border = 0;
            childtable1.AddCell(accountno);

            PdfPCell accountnovalue = new PdfPCell(new Phrase(pfNo, FontFactory.GetFont(FontStyle, Fontsize - 1, Font.BOLD, BaseColor.BLACK)));
            accountnovalue.HorizontalAlignment = 0;
            accountnovalue.Colspan = 2;
            accountnovalue.Border = 0;
            childtable1.AddCell(accountnovalue);

            PdfPCell slno2 = new PdfPCell(new Phrase("2.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno2.HorizontalAlignment = 0;
            slno2.Colspan = 1;
            slno2.Border = 0;
            childtable1.AddCell(slno2);

            PdfPCell employeename = new PdfPCell(new Phrase("Employee Name :", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            employeename.HorizontalAlignment = 0;
            employeename.Colspan = 2;
            employeename.Border = 0;
            childtable1.AddCell(employeename);

            PdfPCell employeename1 = new PdfPCell(new Phrase(name, FontFactory.GetFont(FontStyle, Fontsize - 1, Font.BOLD, BaseColor.BLACK)));
            employeename1.HorizontalAlignment = 0;
            employeename1.Colspan = 2;
            employeename1.Border = 0;
            childtable1.AddCell(employeename1);

            PdfPCell slno3 = new PdfPCell(new Phrase("3.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno3.HorizontalAlignment = 0;
            slno3.Colspan = 1;
            slno3.Border = 0;
            childtable1.AddCell(slno3);


            PdfPCell husbandname = new PdfPCell(new Phrase("Husband/Father Name :", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            husbandname.HorizontalAlignment = 0;
            husbandname.Colspan = 2;
            husbandname.Border = 0;
            childtable1.AddCell(husbandname);

            PdfPCell husbandname1 = new PdfPCell(new Phrase(fathername, FontFactory.GetFont(FontStyle, Fontsize - 1, Font.BOLD, BaseColor.BLACK)));
            husbandname1.HorizontalAlignment = 0;
            husbandname1.Colspan = 2;
            husbandname1.Border = 0;
            childtable1.AddCell(husbandname1);

            PdfPCell slno4 = new PdfPCell(new Phrase("4.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno4.HorizontalAlignment = 0;
            slno4.Colspan = 1;
            slno4.Border = 0;
            childtable1.AddCell(slno4);


            PdfPCell Address = new PdfPCell(new Phrase("Name & Address of the Factory /Establishment :", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            Address.HorizontalAlignment = 0;
            Address.Colspan = 2;
            Address.Border = 0;
            childtable1.AddCell(Address);

            PdfPCell Address1 = new PdfPCell(new Phrase(Companyname, FontFactory.GetFont(FontStyle, Fontsize - 1, Font.BOLD, BaseColor.BLACK)));
            Address1.HorizontalAlignment = 0;
            Address1.Colspan = 2;
            Address1.Border = 0;
            childtable1.AddCell(Address1);


            PdfPCell slno5 = new PdfPCell(new Phrase("5.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno5.HorizontalAlignment = 0;
            slno5.Colspan = 1;
            slno5.Border = 0;
            childtable1.AddCell(slno5);


            string qrytbl = "select PFEmployee from TblOptions ";
            DataTable dtpfoptions = config.ExecuteAdaptorAsyncWithQueryParams(qrytbl).Result;
            string pfemplyee = "";
            if (dtpfoptions.Rows.Count > 0)
            {
                pfemplyee = dtpfoptions.Rows[0]["PFEmployee"].ToString();

            }


            PdfPCell Statutory = new PdfPCell(new Phrase("Statutory Rate of  Contribution :", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            Statutory.HorizontalAlignment = 0;
            Statutory.Colspan = 2;
            Statutory.Border = 0;
            childtable1.AddCell(Statutory);

            PdfPCell Statutory1 = new PdfPCell(new Phrase(pfemplyee + " %", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.BOLD, BaseColor.BLACK)));
            Statutory1.HorizontalAlignment = 0;
            Statutory1.Colspan = 2;
            Statutory1.Border = 0;
            childtable1.AddCell(Statutory1);

            PdfPCell endchildTable1 = new PdfPCell(childtable1);
            endchildTable1.Border = 0;
            endchildTable1.Colspan = 5;
            endchildTable1.HorizontalAlignment = 0;
            tablesnewone1.AddCell(endchildTable1);


            PdfPTable childtable2 = new PdfPTable(5);
            childtable2.TotalWidth = 250f;
            childtable2.LockedWidth = true;
            float[] childwidth2 = new float[] { 0.7f, 3f, 3f, 3f, 3f };
            childtable2.SetWidths(childwidth2);

            PdfPCell slno6 = new PdfPCell(new Phrase("6.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno6.HorizontalAlignment = 0;
            slno6.Colspan = 1;
            slno6.Border = 0;
            childtable2.AddCell(slno6);

            PdfPCell Voluntary = new PdfPCell(new Phrase("Voluntary Higher Rate of Employee's Contribution if any", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            Voluntary.HorizontalAlignment = 0;
            Voluntary.Colspan = 2;
            Voluntary.Border = 0;
            childtable2.AddCell(Voluntary);

            PdfPCell Voluntary1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            Voluntary1.HorizontalAlignment = 0;
            Voluntary1.Colspan = 2;
            Voluntary1.Border = 0;
            childtable2.AddCell(Voluntary1);

            PdfPCell slno7 = new PdfPCell(new Phrase("7.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno7.HorizontalAlignment = 0;
            slno7.Colspan = 1;
            slno7.Border = 0;
            childtable2.AddCell(slno7);


            PdfPCell employee = new PdfPCell(new Phrase("Employee Contribution of higher wages to EPF Yes/No", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            employee.HorizontalAlignment = 0;
            employee.Colspan = 2;
            employee.Border = 0;
            childtable2.AddCell(employee);

            PdfPCell employee1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            employee1.HorizontalAlignment = 0;
            employee1.Colspan = 2;
            employee1.Border = 0;
            childtable2.AddCell(employee1);

            PdfPCell slno8 = new PdfPCell(new Phrase("8.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno8.HorizontalAlignment = 0;
            slno8.Colspan = 1;
            slno8.Border = 0;
            childtable2.AddCell(slno8);

            PdfPCell employeec = new PdfPCell(new Phrase("Employee Contribution to pension fund on heigher wages EPF Yes/No", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            employeec.HorizontalAlignment = 0;
            employeec.Colspan = 2;
            employeec.Border = 0;
            childtable2.AddCell(employeec);

            PdfPCell employeec1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            employeec1.HorizontalAlignment = 0;
            employeec1.Colspan = 2;
            employeec1.Border = 0;
            childtable2.AddCell(employeec1);


            PdfPCell slno9 = new PdfPCell(new Phrase("9.", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            slno9.HorizontalAlignment = 0;
            slno9.Colspan = 1;
            slno9.Border = 0;
            childtable2.AddCell(slno9);

            PdfPCell dareodf = new PdfPCell(new Phrase("Date on which 6500  wages started", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            dareodf.HorizontalAlignment = 0;
            dareodf.Colspan = 2;
            dareodf.Border = 0;
            childtable2.AddCell(dareodf);

            PdfPCell dareodf1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            dareodf1.HorizontalAlignment = 0;
            dareodf1.Colspan = 2;
            dareodf1.Border = 0;
            childtable2.AddCell(dareodf1);


            PdfPCell endchildTable2 = new PdfPCell(childtable2);
            endchildTable2.Border = 0;
            endchildTable2.HorizontalAlignment = 0;
            endchildTable2.Colspan = 5;
            tablesnewone1.AddCell(endchildTable2);

            document.Add(tablesnewone1);


            #endregion

            #region

            PdfPTable tablesnewone = new PdfPTable(8);
            tablesnewone.TotalWidth = 500f;
            tablesnewone.LockedWidth = true;
            float[] width = new float[] { 4.8f, 4f, 4f, 4f, 4f, 4f, 3f, 5.1f };
            tablesnewone.SetWidths(width);


            PdfPCell space = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            space.HorizontalAlignment = 1;
            space.Colspan = 8;
            space.Border = 0;
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);

            PdfPCell mt1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            mt1.HorizontalAlignment = 1;
            mt1.Colspan = 1;
            mt1.BorderWidthBottom = 0;
            mt1.BorderWidthLeft = .5f;
            mt1.BorderWidthRight = 0;
            mt1.BorderWidthTop = .5f;
            tablesnewone.AddCell(mt1);


            PdfPCell workershare = new PdfPCell(new Phrase("Worker Share", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            workershare.HorizontalAlignment = 1;
            workershare.Colspan = 2;
            tablesnewone.AddCell(workershare);

            PdfPCell employerrshare = new PdfPCell(new Phrase("Employerr Share", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            employerrshare.HorizontalAlignment = 1;
            employerrshare.Colspan = 2;
            tablesnewone.AddCell(employerrshare);

            PdfPCell mt2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            mt2.HorizontalAlignment = 1;
            mt2.Colspan = 1;
            mt2.BorderWidthBottom = 0;
            mt2.BorderWidthLeft = 0;
            mt2.BorderWidthRight = 0;
            mt2.BorderWidthTop = .5f;
            tablesnewone.AddCell(mt2);

            PdfPCell mt3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            mt3.HorizontalAlignment = 1;
            mt3.Colspan = 1;
            mt3.BorderWidthBottom = 0;
            mt3.BorderWidthLeft = .5f;
            mt3.BorderWidthRight = .5f;
            mt3.BorderWidthTop = .5f;
            tablesnewone.AddCell(mt3);

            PdfPCell mt4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            mt4.HorizontalAlignment = 1;
            mt4.Colspan = 1;
            mt4.BorderWidthBottom = 0;
            mt4.BorderWidthLeft = .5f;
            mt4.BorderWidthRight = .5f;
            mt4.BorderWidthTop = .5f;
            tablesnewone.AddCell(mt4);


            PdfPCell month1 = new PdfPCell(new Phrase("Month", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            month1.VerticalAlignment = 2;
            month1.Rowspan = 1;
            month1.Rotation = 90;
            month1.BorderWidthBottom = .5f;
            month1.BorderWidthLeft = .5f;
            month1.BorderWidthRight = 0;
            month1.BorderWidthTop = 0;
            tablesnewone.AddCell(month1);

            PdfPCell amount = new PdfPCell(new Phrase("Amount of wage", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            amount.VerticalAlignment = 1;
            amount.Rowspan = 1;
            amount.Rotation = 90;
            tablesnewone.AddCell(amount);

            PdfPCell epf = new PdfPCell(new Phrase("EPF", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            epf.VerticalAlignment = 1;
            epf.Rowspan = 1;
            epf.Rotation = 90;
            tablesnewone.AddCell(epf);

            PdfPCell epfcontribution = new PdfPCell(new Phrase("EPF contribution", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            epfcontribution.VerticalAlignment = 1;
            epfcontribution.Rowspan = 1;
            epfcontribution.Rotation = 90;
            tablesnewone.AddCell(epfcontribution);

            PdfPCell Pension = new PdfPCell(new Phrase("Pension fund \n contribution", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Pension.VerticalAlignment = 1;
            Pension.Rowspan = 1;
            Pension.Rotation = 90;
            tablesnewone.AddCell(Pension);

            PdfPCell Refund = new PdfPCell(new Phrase("Refund of advance", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Refund.VerticalAlignment = 1;
            Refund.Rowspan = 1;
            Refund.Rotation = 90;
            Refund.BorderWidthBottom = .5f;
            Refund.BorderWidthLeft = .5f;
            Refund.BorderWidthRight = .5f;
            Refund.BorderWidthTop = 0;
            tablesnewone.AddCell(Refund);

            PdfPCell noofdays = new PdfPCell(new Phrase("No of days/ period", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            noofdays.VerticalAlignment = 1;
            noofdays.Rowspan = 1;
            noofdays.Rotation = 90;
            noofdays.BorderWidthBottom = .5f;
            noofdays.BorderWidthLeft = .5f;
            noofdays.BorderWidthRight = .5f;
            noofdays.BorderWidthTop = 0;
            tablesnewone.AddCell(noofdays);

            PdfPCell remarks = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            remarks.VerticalAlignment = 1;
            remarks.Rowspan = 1;
            remarks.Rotation = 90;
            remarks.BorderWidthBottom = .5f;
            remarks.BorderWidthLeft = .5f;
            remarks.BorderWidthRight = .5f;
            remarks.BorderWidthTop = 0;
            tablesnewone.AddCell(remarks);

            PdfPCell April1 = new PdfPCell(new Phrase("March paid in April", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            April1.HorizontalAlignment = 0;
            April1.Colspan = 1;
            tablesnewone.AddCell(April1);

            PdfPCell Aprilamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Aprilamount.HorizontalAlignment = 1;
            Aprilamount.Colspan = 1;
            tablesnewone.AddCell(Aprilamount);

            PdfPCell Aprilepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Aprilepf.HorizontalAlignment = 1;
            Aprilepf.Colspan = 1;
            tablesnewone.AddCell(Aprilepf);

            PdfPCell Aprilepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Aprilepfcontributionvalue.HorizontalAlignment = 1;
            Aprilepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Aprilepfcontributionvalue);

            PdfPCell Aprilpensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Aprilpensionvalue.HorizontalAlignment = 1;
            Aprilpensionvalue.Colspan = 1;
            tablesnewone.AddCell(Aprilpensionvalue);

            PdfPCell AprilRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            AprilRefundvalue.HorizontalAlignment = 1;
            AprilRefundvalue.Colspan = 1;
            tablesnewone.AddCell(AprilRefundvalue);

            PdfPCell Aprilnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Aprilnoofdaysvalue.HorizontalAlignment = 0;
            Aprilnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Aprilnoofdaysvalue);

            PdfPCell Aprilremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Aprilremarkstext.HorizontalAlignment = 0;
            Aprilremarkstext.Colspan = 1;
            tablesnewone.AddCell(Aprilremarkstext);

            //may
            PdfPCell May1 = new PdfPCell(new Phrase("April paid in May", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            May1.HorizontalAlignment = 0;
            May1.Colspan = 1;
            tablesnewone.AddCell(May1);

            PdfPCell Mayamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Mayamount.HorizontalAlignment = 1;
            Mayamount.Colspan = 1;
            tablesnewone.AddCell(Mayamount);

            PdfPCell Mayepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Mayepf.HorizontalAlignment = 1;
            Mayepf.Colspan = 1;
            tablesnewone.AddCell(Mayepf);

            PdfPCell Mayepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Mayepfcontributionvalue.HorizontalAlignment = 1;
            Mayepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Mayepfcontributionvalue);

            PdfPCell Maypensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Maypensionvalue.HorizontalAlignment = 1;
            Maypensionvalue.Colspan = 1;
            tablesnewone.AddCell(Maypensionvalue);

            PdfPCell MayRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            MayRefundvalue.HorizontalAlignment = 1;
            MayRefundvalue.Colspan = 1;
            tablesnewone.AddCell(MayRefundvalue);

            PdfPCell Maynoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Maynoofdaysvalue.HorizontalAlignment = 0;
            Maynoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Maynoofdaysvalue);

            PdfPCell Mayremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Mayremarkstext.HorizontalAlignment = 0;
            Mayremarkstext.Colspan = 1;
            tablesnewone.AddCell(Mayremarkstext);


            //end may

            //june
            PdfPCell june1 = new PdfPCell(new Phrase("May paid in June", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            june1.HorizontalAlignment = 0;
            june1.Colspan = 1;
            tablesnewone.AddCell(june1);

            PdfPCell juneamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            juneamount.HorizontalAlignment = 1;
            juneamount.Colspan = 1;
            tablesnewone.AddCell(juneamount);

            PdfPCell juneepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            juneepf.HorizontalAlignment = 1;
            juneepf.Colspan = 1;
            tablesnewone.AddCell(juneepf);

            PdfPCell juneepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            juneepfcontributionvalue.HorizontalAlignment = 1;
            juneepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(juneepfcontributionvalue);

            PdfPCell junepensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            junepensionvalue.HorizontalAlignment = 1;
            junepensionvalue.Colspan = 1;
            tablesnewone.AddCell(junepensionvalue);

            PdfPCell juneRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            juneRefundvalue.HorizontalAlignment = 1;
            juneRefundvalue.Colspan = 1;
            tablesnewone.AddCell(juneRefundvalue);

            PdfPCell junenoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            junenoofdaysvalue.HorizontalAlignment = 0;
            junenoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(junenoofdaysvalue);

            PdfPCell juneremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            juneremarkstext.HorizontalAlignment = 0;
            juneremarkstext.Colspan = 1;
            tablesnewone.AddCell(juneremarkstext);


            // endjune

            //july
            PdfPCell july1 = new PdfPCell(new Phrase("June paid in July", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            july1.HorizontalAlignment = 0;
            july1.Colspan = 1;
            tablesnewone.AddCell(july1);

            PdfPCell julyamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            julyamount.HorizontalAlignment = 1;
            julyamount.Colspan = 1;
            tablesnewone.AddCell(julyamount);

            PdfPCell julyepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            julyepf.HorizontalAlignment = 1;
            julyepf.Colspan = 1;
            tablesnewone.AddCell(julyepf);

            PdfPCell julyepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            julyepfcontributionvalue.HorizontalAlignment = 1;
            julyepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(julyepfcontributionvalue);

            PdfPCell julypensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            julypensionvalue.HorizontalAlignment = 1;
            julypensionvalue.Colspan = 1;
            tablesnewone.AddCell(julypensionvalue);

            PdfPCell julyRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            julyRefundvalue.HorizontalAlignment = 1;
            julyRefundvalue.Colspan = 1;
            tablesnewone.AddCell(julyRefundvalue);

            PdfPCell julynoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            julynoofdaysvalue.HorizontalAlignment = 0;
            julynoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(julynoofdaysvalue);

            PdfPCell julyremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            julyremarkstext.HorizontalAlignment = 0;
            julyremarkstext.Colspan = 1;
            tablesnewone.AddCell(julyremarkstext);

            // end july
            //aug
            PdfPCell Aug1 = new PdfPCell(new Phrase("Juy paid in Aug", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Aug1.HorizontalAlignment = 0;
            Aug1.Colspan = 1;
            tablesnewone.AddCell(Aug1);

            PdfPCell Augamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Augamount.HorizontalAlignment = 1;
            Augamount.Colspan = 1;
            tablesnewone.AddCell(Augamount);

            PdfPCell Augepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Augepf.HorizontalAlignment = 1;
            Augepf.Colspan = 1;
            tablesnewone.AddCell(Augepf);

            PdfPCell Augepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Augepfcontributionvalue.HorizontalAlignment = 1;
            Augepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Augepfcontributionvalue);

            PdfPCell Augpensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Augpensionvalue.HorizontalAlignment = 1;
            Augpensionvalue.Colspan = 1;
            tablesnewone.AddCell(Augpensionvalue);

            PdfPCell AugRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            AugRefundvalue.HorizontalAlignment = 1;
            AugRefundvalue.Colspan = 1;
            tablesnewone.AddCell(AugRefundvalue);

            PdfPCell Augnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Augnoofdaysvalue.HorizontalAlignment = 0;
            Augnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Augnoofdaysvalue);

            PdfPCell Augremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Augremarkstext.HorizontalAlignment = 0;
            Augremarkstext.Colspan = 1;
            tablesnewone.AddCell(Augremarkstext);

            // end aug
            // sep
            PdfPCell Sep1 = new PdfPCell(new Phrase("Aug paid in Sep", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Sep1.HorizontalAlignment = 0;
            Sep1.Colspan = 1;
            tablesnewone.AddCell(Sep1);

            PdfPCell Sepamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Sepamount.HorizontalAlignment = 1;
            Sepamount.Colspan = 1;
            tablesnewone.AddCell(Sepamount);

            PdfPCell Sepepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Sepepf.HorizontalAlignment = 1;
            Sepepf.Colspan = 1;
            tablesnewone.AddCell(Sepepf);

            PdfPCell Sepepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Sepepfcontributionvalue.HorizontalAlignment = 1;
            Sepepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Sepepfcontributionvalue);

            PdfPCell Seppensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Seppensionvalue.HorizontalAlignment = 1;
            Seppensionvalue.Colspan = 1;
            tablesnewone.AddCell(Seppensionvalue);

            PdfPCell SepRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            SepRefundvalue.HorizontalAlignment = 1;
            SepRefundvalue.Colspan = 1;
            tablesnewone.AddCell(SepRefundvalue);

            PdfPCell Sepnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Sepnoofdaysvalue.HorizontalAlignment = 0;
            Sepnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Sepnoofdaysvalue);

            PdfPCell Sepremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Sepremarkstext.HorizontalAlignment = 0;
            Sepremarkstext.Colspan = 1;
            tablesnewone.AddCell(Sepremarkstext);

            // end sep
            // oct
            PdfPCell Oct1 = new PdfPCell(new Phrase("Sep paid in Oct", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Oct1.HorizontalAlignment = 0;
            Oct1.Colspan = 1;
            tablesnewone.AddCell(Oct1);

            PdfPCell Octamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Octamount.HorizontalAlignment = 1;
            Octamount.Colspan = 1;
            tablesnewone.AddCell(Octamount);

            PdfPCell Octepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Octepf.HorizontalAlignment = 1;
            Octepf.Colspan = 1;
            tablesnewone.AddCell(Octepf);

            PdfPCell Octepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Octepfcontributionvalue.HorizontalAlignment = 1;
            Octepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Octepfcontributionvalue);

            PdfPCell Octpensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Octpensionvalue.HorizontalAlignment = 1;
            Octpensionvalue.Colspan = 1;
            tablesnewone.AddCell(Octpensionvalue);

            PdfPCell OctRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            OctRefundvalue.HorizontalAlignment = 1;
            OctRefundvalue.Colspan = 1;
            tablesnewone.AddCell(OctRefundvalue);

            PdfPCell Octnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Octnoofdaysvalue.HorizontalAlignment = 0;
            Octnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Octnoofdaysvalue);

            PdfPCell Octremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Octremarkstext.HorizontalAlignment = 0;
            Octremarkstext.Colspan = 1;
            Octremarkstext.BorderWidthBottom = 0;
            Octremarkstext.BorderWidthLeft = .5f;
            Octremarkstext.BorderWidthRight = .5f;
            Octremarkstext.BorderWidthTop = .5f;
            tablesnewone.AddCell(Octremarkstext);


            //end oct

            // nov

            PdfPCell Nov1 = new PdfPCell(new Phrase("Oct paid in Nov", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Nov1.HorizontalAlignment = 0;
            Nov1.Colspan = 1;
            tablesnewone.AddCell(Nov1);

            PdfPCell Novamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Novamount.HorizontalAlignment = 1;
            Novamount.Colspan = 1;
            tablesnewone.AddCell(Novamount);

            PdfPCell Novepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Novepf.HorizontalAlignment = 1;
            Novepf.Colspan = 1;
            tablesnewone.AddCell(Novepf);

            PdfPCell Novepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Novepfcontributionvalue.HorizontalAlignment = 1;
            Novepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Novepfcontributionvalue);

            PdfPCell Novpensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Novpensionvalue.HorizontalAlignment = 1;
            Novpensionvalue.Colspan = 1;
            tablesnewone.AddCell(Novpensionvalue);

            PdfPCell NovRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            NovRefundvalue.HorizontalAlignment = 1;
            NovRefundvalue.Colspan = 1;
            tablesnewone.AddCell(NovRefundvalue);

            PdfPCell Novnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Novnoofdaysvalue.HorizontalAlignment = 0;
            Novnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Novnoofdaysvalue);

            PdfPCell Novremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Novremarkstext.HorizontalAlignment = 0;
            Novremarkstext.Colspan = 1;
            Novremarkstext.BorderWidthBottom = 0;
            Novremarkstext.BorderWidthLeft = .5f;
            Novremarkstext.BorderWidthRight = .5f;
            Novremarkstext.BorderWidthTop = 0;
            tablesnewone.AddCell(Novremarkstext);

            // end nov
            //dec
            PdfPCell Des1 = new PdfPCell(new Phrase("Nov paid in Dec", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Des1.HorizontalAlignment = 0;
            Des1.Colspan = 1;
            tablesnewone.AddCell(Des1);

            PdfPCell Desamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Desamount.HorizontalAlignment = 1;
            Desamount.Colspan = 1;
            tablesnewone.AddCell(Desamount);

            PdfPCell Desepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Desepf.HorizontalAlignment = 1;
            Desepf.Colspan = 1;
            tablesnewone.AddCell(Desepf);

            PdfPCell Desepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Desepfcontributionvalue.HorizontalAlignment = 1;
            Desepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Desepfcontributionvalue);

            PdfPCell Despensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Despensionvalue.HorizontalAlignment = 1;
            Despensionvalue.Colspan = 1;
            tablesnewone.AddCell(Despensionvalue);

            PdfPCell DesRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            DesRefundvalue.HorizontalAlignment = 1;
            DesRefundvalue.Colspan = 1;
            tablesnewone.AddCell(DesRefundvalue);

            PdfPCell Desnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Desnoofdaysvalue.HorizontalAlignment = 0;
            Desnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Desnoofdaysvalue);

            PdfPCell Desremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Desremarkstext.HorizontalAlignment = 0;
            Desremarkstext.Colspan = 1;
            Desremarkstext.BorderWidthBottom = 0;
            Desremarkstext.BorderWidthLeft = .5f;
            Desremarkstext.BorderWidthRight = .5f;
            Desremarkstext.BorderWidthTop = 0;
            tablesnewone.AddCell(Desremarkstext);

            //end des
            //jan
            PdfPCell Jan1 = new PdfPCell(new Phrase("Dec paid in Jan", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Jan1.HorizontalAlignment = 0;
            Jan1.Colspan = 1;
            tablesnewone.AddCell(Jan1);

            PdfPCell Janamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Janamount.HorizontalAlignment = 1;
            Janamount.Colspan = 1;
            tablesnewone.AddCell(Janamount);

            PdfPCell Janepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Janepf.HorizontalAlignment = 1;
            Janepf.Colspan = 1;
            tablesnewone.AddCell(Janepf);

            PdfPCell Janepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Janepfcontributionvalue.HorizontalAlignment = 1;
            Janepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Janepfcontributionvalue);

            PdfPCell Janpensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Janpensionvalue.HorizontalAlignment = 1;
            Janpensionvalue.Colspan = 1;
            tablesnewone.AddCell(Janpensionvalue);

            PdfPCell JanRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            JanRefundvalue.HorizontalAlignment = 1;
            JanRefundvalue.Colspan = 1;
            tablesnewone.AddCell(JanRefundvalue);

            PdfPCell Jannoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Jannoofdaysvalue.HorizontalAlignment = 0;
            Jannoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Jannoofdaysvalue);

            PdfPCell Janremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Janremarkstext.HorizontalAlignment = 0;
            Janremarkstext.Colspan = 1;
            Janremarkstext.BorderWidthBottom = .5f;
            Janremarkstext.BorderWidthLeft = .5f;
            Janremarkstext.BorderWidthRight = .5f;
            Janremarkstext.BorderWidthTop = 0;
            tablesnewone.AddCell(Janremarkstext);

            //end jan

            //feb

            PdfPCell Feb1 = new PdfPCell(new Phrase("Jan paid in Feb", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Feb1.HorizontalAlignment = 0;
            Feb1.Colspan = 1;
            tablesnewone.AddCell(Feb1);

            PdfPCell Febamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Febamount.HorizontalAlignment = 1;
            Febamount.Colspan = 1;
            tablesnewone.AddCell(Febamount);

            PdfPCell Febepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Febepf.HorizontalAlignment = 1;
            Febepf.Colspan = 1;
            tablesnewone.AddCell(Febepf);

            PdfPCell Febepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Febepfcontributionvalue.HorizontalAlignment = 1;
            Febepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Febepfcontributionvalue);

            PdfPCell Febpensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Febpensionvalue.HorizontalAlignment = 1;
            Febpensionvalue.Colspan = 1;
            tablesnewone.AddCell(Febpensionvalue);

            PdfPCell FebRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            FebRefundvalue.HorizontalAlignment = 1;
            FebRefundvalue.Colspan = 1;
            tablesnewone.AddCell(FebRefundvalue);

            PdfPCell Febnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Febnoofdaysvalue.HorizontalAlignment = 0;
            Febnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Febnoofdaysvalue);

            PdfPCell Febremarkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Febremarkstext.HorizontalAlignment = 0;
            Febremarkstext.Colspan = 1;
            tablesnewone.AddCell(Febremarkstext);

            //end feb

            //mar

            PdfPCell March1 = new PdfPCell(new Phrase("Fed paid in March", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            March1.HorizontalAlignment = 0;
            March1.Colspan = 1;
            tablesnewone.AddCell(March1);

            PdfPCell Marchamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Marchamount.HorizontalAlignment = 1;
            Marchamount.Colspan = 1;
            tablesnewone.AddCell(Marchamount);

            PdfPCell Marchepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Marchepf.HorizontalAlignment = 1;
            Marchepf.Colspan = 1;
            tablesnewone.AddCell(Marchepf);

            PdfPCell Marchepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Marchepfcontributionvalue.HorizontalAlignment = 1;
            Marchepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Marchepfcontributionvalue);

            PdfPCell Marchpensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Marchpensionvalue.HorizontalAlignment = 1;
            Marchpensionvalue.Colspan = 1;
            tablesnewone.AddCell(Marchpensionvalue);

            PdfPCell MarchRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            MarchRefundvalue.HorizontalAlignment = 1;
            MarchRefundvalue.Colspan = 1;
            tablesnewone.AddCell(MarchRefundvalue);

            PdfPCell Marchnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Marchnoofdaysvalue.HorizontalAlignment = 0;
            Marchnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Marchnoofdaysvalue);

            PdfPCell MarchreMarchkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            MarchreMarchkstext.HorizontalAlignment = 0;
            MarchreMarchkstext.Colspan = 1;
            tablesnewone.AddCell(MarchreMarchkstext);

            //end mar

            //any

            PdfPCell any1 = new PdfPCell(new Phrase("Supplementary if any", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            any1.HorizontalAlignment = 0;
            any1.Colspan = 1;
            tablesnewone.AddCell(any1);

            PdfPCell anyamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            anyamount.HorizontalAlignment = 1;
            anyamount.Colspan = 1;
            tablesnewone.AddCell(anyamount);

            PdfPCell anyepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            anyepf.HorizontalAlignment = 1;
            anyepf.Colspan = 1;
            tablesnewone.AddCell(anyepf);

            PdfPCell anyepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            anyepfcontributionvalue.HorizontalAlignment = 1;
            anyepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(anyepfcontributionvalue);

            PdfPCell anypensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            anypensionvalue.HorizontalAlignment = 1;
            anypensionvalue.Colspan = 1;
            tablesnewone.AddCell(anypensionvalue);

            PdfPCell anyRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            anyRefundvalue.HorizontalAlignment = 1;
            anyRefundvalue.Colspan = 1;
            tablesnewone.AddCell(anyRefundvalue);

            PdfPCell anynoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            anynoofdaysvalue.HorizontalAlignment = 0;
            anynoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(anynoofdaysvalue);

            PdfPCell anyreanykstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            anyreanykstext.HorizontalAlignment = 0;
            anyreanykstext.Colspan = 1;
            tablesnewone.AddCell(anyreanykstext);

            // end any

            //total
            PdfPCell Total1 = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Total1.HorizontalAlignment = 0;
            Total1.Colspan = 1;
            tablesnewone.AddCell(Total1);

            PdfPCell Totalamount = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Totalamount.HorizontalAlignment = 1;
            Totalamount.Colspan = 1;
            tablesnewone.AddCell(Totalamount);

            PdfPCell Totalepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Totalepf.HorizontalAlignment = 1;
            Totalepf.Colspan = 1;
            tablesnewone.AddCell(Totalepf);

            PdfPCell Totalepfcontributionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, 13, Font.NORMAL, BaseColor.BLACK)));
            Totalepfcontributionvalue.HorizontalAlignment = 1;
            Totalepfcontributionvalue.Colspan = 1;
            tablesnewone.AddCell(Totalepfcontributionvalue);

            PdfPCell Totalpensionvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Totalpensionvalue.HorizontalAlignment = 1;
            Totalpensionvalue.Colspan = 1;
            tablesnewone.AddCell(Totalpensionvalue);

            PdfPCell TotalRefundvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            TotalRefundvalue.HorizontalAlignment = 1;
            TotalRefundvalue.Colspan = 1;
            tablesnewone.AddCell(TotalRefundvalue);

            PdfPCell Totalnoofdaysvalue = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Totalnoofdaysvalue.HorizontalAlignment = 0;
            Totalnoofdaysvalue.Colspan = 1;
            tablesnewone.AddCell(Totalnoofdaysvalue);

            PdfPCell TotalreTotalkstext = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            TotalreTotalkstext.HorizontalAlignment = 0;
            TotalreTotalkstext.Colspan = 1;
            tablesnewone.AddCell(TotalreTotalkstext);

            //end total


            PdfPCell emp = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            emp.HorizontalAlignment = 0;
            emp.Colspan = 8;
            emp.Border = 0;
            tablesnewone.AddCell(emp);
            tablesnewone.AddCell(emp);



            PdfPCell fottertext1 = new PdfPCell(new Phrase("Certified that the total amount of contribution (both shares) indicated in this card i.e.             has  already been remitted  it full in E.P.F. A/c No. 1 and PensionFund A/c No. 10 Rs", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            fottertext1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            fottertext1.Colspan = 8;
            fottertext1.Border = 0;
            tablesnewone.AddCell(fottertext1);

            PdfPCell emp1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            emp1.HorizontalAlignment = 0;
            emp1.Colspan = 8;
            emp1.Border = 0;
            tablesnewone.AddCell(emp1);
            tablesnewone.AddCell(emp1);

            PdfPCell fottertext = new PdfPCell(new Phrase("Certified that the difference between the total of the contribution shown under Cols. 3 & 4a & 4b of the above table and that arrived at on the total wages shown in column 2 at the prescribed rate is solely due to rounding off the contributions to the nearest rupee under the rules", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            fottertext.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            fottertext.Colspan = 8;
            fottertext.Border = 0;
            tablesnewone.AddCell(fottertext);

            PdfPCell emp11 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            emp11.HorizontalAlignment = 0;
            emp11.Colspan = 8;
            emp11.Border = 0;
            tablesnewone.AddCell(emp11);
            tablesnewone.AddCell(emp11);
            tablesnewone.AddCell(emp11);
            tablesnewone.AddCell(emp11);


            PdfPCell celldate = new PdfPCell(new Phrase("Date: ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            celldate.HorizontalAlignment = 0;
            celldate.Colspan = 4;
            // celldate.PaddingBottom = -20;
            celldate.Border = 0;
            tablesnewone.AddCell(celldate);

            PdfPCell signature = new PdfPCell(new Phrase("Signature of the Employer(with office seal)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            signature.HorizontalAlignment = 2;
            signature.Colspan = 4;
            signature.Border = 0;
            // signature.PaddingBottom = -20;
            tablesnewone.AddCell(signature);

            document.Add(tablesnewone);

            #endregion Basic Information of the Employee

            document.NewPage();

            PdfPTable tblFingerprints = new PdfPTable(6);
            tblFingerprints.TotalWidth = 500f;
            tblFingerprints.LockedWidth = true;
            float[] widthfinger = new float[] { 2f, 1.5f, 2f, 2f, 1.5f, 2f };
            tblFingerprints.SetWidths(widthfinger);

            string filename = "formX.pdf";

            document.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            //}

            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Duration expired');", true);
            //    return;
            //}
        }

        protected void btndeclaration_Click(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string fontsyle = "verdana";



            if (txtemplyid.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
                return;
            }

            //#region  Begin  New Code

            #region  Begin  New Code

            #region Begin Variable Declaration

            string SPName = "";
            Hashtable HTEmpBiodata = new Hashtable();
            string Empid = "";
            DataTable dtEmpdetails = null;

            #endregion end Variable Declaration


            #region Begin Assign Values to The Variables
            SPName = "EmployeeFormsPDF";
            Empid = txtemplyid.Text;

            string date = "";

            var DtofJoining = string.Empty;
            if (txtEmpDtofJoining.Text.Trim().Length != 0)
            {
                DtofJoining = Timings.Instance.CheckDateFormat(txtEmpDtofJoining.Text);

            }
            else
            {
                DtofJoining = "01/01/1900";
            }


            if (TxtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(TxtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = "";
            string Year = "";
            //string month = DateTime.Parse(date).Month.ToString();
            // string Year = DateTime.Parse(date).Year.ToString();


            #endregion End Assign values To the Variables

            #region Begin Pass values to the Hash table
            HTEmpBiodata.Add("@empid", Empid);
            // HTEmpBiodata.Add("@month", month + Year.Substring(2, 2));
            HTEmpBiodata.Add("@DtofJoining", DtofJoining);
            #endregion  end Pass values to the Hash table

            #region Begin  Call Stored Procedure
            dtEmpdetails = config.ExecuteAdaptorAsyncWithParams(SPName, HTEmpBiodata).Result;
            #endregion  End  Call Stored Procedure

            #endregion End New Code As on [31-05-2014]


            string name = "";
            string fathername = "";
            string Gender = "";
            string Companyname = "";
            string CompanyAddress = "";
            string remarks1 = "";
            string accno = "";
            string Empdateofbirth = "";
            string empdesign = "";
            int j = 1;
            if (dtEmpdetails.Rows.Count > 0)
            {

                name = dtEmpdetails.Rows[0]["EmployeeName"].ToString();
                fathername = dtEmpdetails.Rows[0]["EmpFatherName"].ToString();
                empdesign = dtEmpdetails.Rows[0]["Designation"].ToString();
                date = dtEmpdetails.Rows[0]["DtOfJoining"].ToString();
                Empdateofbirth = dtEmpdetails.Rows[0]["DtOfBirth"].ToString();
                Gender = dtEmpdetails.Rows[0]["gender"].ToString();
                Companyname = dtEmpdetails.Rows[0]["CompanyName"].ToString();
                CompanyAddress = dtEmpdetails.Rows[0]["CompanyAddress"].ToString();
                remarks1 = dtEmpdetails.Rows[0]["EmpRemarks"].ToString();
                accno = dtEmpdetails.Rows[0]["EmpBankAcNo"].ToString();
            }


            string strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + txtemplyid.Text + "'";
            string pfNo = "";
            DataTable PfTable = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            if (PfTable.Rows.Count > 0)
            {
                pfNo = PfTable.Rows[0]["EmpEpfNo"].ToString();
            }
            string cmpnypfno = "";
            string qry = "select PFNo from CompanyInfo ";
            DataTable dttblroptns = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dttblroptns.Rows.Count > 0)
            {
                cmpnypfno = dttblroptns.Rows[0]["PFNo"].ToString();

            }



            MemoryStream ms = new MemoryStream();


            Document document = new Document(PageSize.LEGAL);
            // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
            var writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            string imagepath1 = Server.MapPath("images");
            #region

            PdfPTable tablesnewone = new PdfPTable(4);
            tablesnewone.TotalWidth = 500f;
            tablesnewone.LockedWidth = true;
            float[] width = new float[] { 2f, 2f, 2f, 2f };
            tablesnewone.SetWidths(width);

            PdfPCell space = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            space.HorizontalAlignment = 1;
            space.Colspan = 4;
            space.Border = 0;

            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);




            PdfPCell paragraph34 = new PdfPCell();
            Paragraph Pparagraph34 = new Paragraph();
            Pparagraph34.Add(new Chunk("                     THE EMPLOYEES’ PROVIDENT FUNDS SCHEME, 1952 ", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
            Pparagraph34.Add(new Chunk("(Paragraph 34)", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
            paragraph34.AddElement(Pparagraph34);
            paragraph34.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            paragraph34.Colspan = 4;
            paragraph34.Border = 0;// 15;
            tablesnewone.AddCell(paragraph34);
            tablesnewone.AddCell(space);

            PdfPCell unit = new PdfPCell(new Phrase("AND", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            unit.HorizontalAlignment = 1;
            unit.Colspan = 4;
            unit.Border = 0;
            tablesnewone.AddCell(unit);


            PdfPCell paragraph19 = new PdfPCell();
            Paragraph Pparagraph19 = new Paragraph();
            Pparagraph19.Add(new Chunk("                           THE EMPLOYEES’ FAMILY PENSION SCHEME, 1971 ", FontFactory.GetFont(FontStyle, Fontsize + 2, Font.BOLD, BaseColor.BLACK)));
            Pparagraph19.Add(new Chunk("(Paragraph 19)", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
            paragraph19.AddElement(Pparagraph19);
            paragraph19.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            paragraph19.Colspan = 4;
            paragraph19.Border = 0;// 15;
            tablesnewone.AddCell(paragraph19);
            tablesnewone.AddCell(space);






            PdfPCell Declaration = new PdfPCell(new Phrase("Declaration by a person taking up employment in an establishment in which the Employees’Provident Funds & Family Pension Fund Scheme enforce :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Declaration.HorizontalAlignment = 1;
            Declaration.Colspan = 4;
            Declaration.SetLeading(0.0f, 1.5f);
            Declaration.Border = 0;
            tablesnewone.AddCell(Declaration);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);




            PdfPCell names = new PdfPCell();
            Paragraph Nnames = new Paragraph();
            Nnames.Add(new Chunk(" I   ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Nnames.Add(new Chunk(name, FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
            Nnames.Add(new Chunk("  S/o. / W/o. / Daughter of  ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Nnames.Add(new Chunk(fathername, FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
            Nnames.Add(new Chunk("  do hereby solemnly declare that: -", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            names.AddElement(Nnames);
            names.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            names.Colspan = 4;
            names.Border = 0;// 15;
            tablesnewone.AddCell(names);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);



            PdfPCell Rank = new PdfPCell(new Phrase("    (a) I was employed in M/s. ..............................................(Name and full address of the establishment) And left service on ………………..   prior to that, I was employed in .............................................…...from..............................................to............................................", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Rank.HorizontalAlignment = 0;
            Rank.Colspan = 4;
            Rank.SetLeading(0.0f, 1.5f);
            Rank.Border = 0;
            tablesnewone.AddCell(Rank);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);


            PdfPCell Staff = new PdfPCell(new Phrase("    (b) I was member of ..................................…… Provident Fund and also / but not of the Pension Fund from.........………………… to................................and my account number(s) was / were....................................", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Staff.HorizontalAlignment = 0;
            Staff.Colspan = 4;
            Staff.SetLeading(0.0f, 1.5f);
            Staff.Border = 0;
            tablesnewone.AddCell(Staff);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);



            PdfPCell totalnum = new PdfPCell(new Phrase("    (c) I have/have not withdrawn the amount of my Provident Fund / Pension Fund.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            totalnum.HorizontalAlignment = 0;
            totalnum.Colspan = 4;
            totalnum.Border = 0;
            tablesnewone.AddCell(totalnum);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);


            PdfPCell cell3 = new PdfPCell(new Phrase("    (d) I have/have not drawn any superannuation benefits in respect of my past service from any employer.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell3.HorizontalAlignment = 0;
            cell3.Colspan = 4;
            cell3.Border = 0;
            tablesnewone.AddCell(cell3);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);


            PdfPCell cell4 = new PdfPCell(new Phrase("    (e) I have/have not never been a member of any Provident Fund and / or  Pension Fund.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell4.HorizontalAlignment = 0;
            cell4.Colspan = 4;
            cell4.Border = 0;
            tablesnewone.AddCell(cell4);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);


            PdfPCell cell5 = new PdfPCell(new Phrase("    (f) I am drawing/not drawing Pension under EPS '95.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell5.HorizontalAlignment = 0;
            cell5.Colspan = 4;
            cell5.Border = 0;
            tablesnewone.AddCell(cell5);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);

            PdfPCell cellmt = new PdfPCell(new Phrase("    (g) I am a holder / not holder of  Scheme Certificate.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellmt.HorizontalAlignment = 0;
            cellmt.Colspan = 4;
            cellmt.Border = 0;
            tablesnewone.AddCell(cellmt);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);



            PdfPCell cellmonth = new PdfPCell(new Phrase("    (h) Scheme certificate surrendered / not surrendered..", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellmonth.HorizontalAlignment = 0;
            cellmonth.Colspan = 4;
            cellmonth.Border = 0;
            tablesnewone.AddCell(cellmonth);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);

            PdfPCell Uniform = new PdfPCell(new Phrase("Dated : ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Uniform.HorizontalAlignment = 0;
            Uniform.Colspan = 2;
            Uniform.Border = 0;
            tablesnewone.AddCell(Uniform);

            PdfPCell Uniform1 = new PdfPCell(new Phrase("Signature or left hand thumb \n impression of the employee.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Uniform1.HorizontalAlignment = 2;
            Uniform1.Colspan = 2;
            Uniform1.Border = 0;
            tablesnewone.AddCell(Uniform1);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);



            PdfPCell Loan = new PdfPCell(new Phrase("(To be filled by the employer only when the person employed had not already been a member of the Employees’ Provident Fund)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Loan.HorizontalAlignment = 0;
            Loan.Colspan = 4;
            Loan.SetLeading(0.0f, 1.5f);
            Loan.Border = 0;
            tablesnewone.AddCell(Loan);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);




            PdfPCell names1 = new PdfPCell();
            Paragraph Nnames1 = new Paragraph();
            Nnames1.Add(new Chunk("Shri/Smt.  ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Nnames1.Add(new Chunk(name, FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
            Nnames1.Add(new Chunk("  (Name of Employee) is appointed as  ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Nnames1.Add(new Chunk(empdesign, FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
            Nnames1.Add(new Chunk(" (Designation)In M / s   ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Nnames1.Add(new Chunk(Companyname, FontFactory.GetFont(FontStyle, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
            Nnames1.Add(new Chunk(" (Name of the Factory / Establishment) with effect from ......................(Date of appointment)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            names1.AddElement(Nnames1);
            names1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            names1.Colspan = 4;
            names1.SetLeading(0.0f, 1.5f);
            names1.Border = 0;// 15;
            tablesnewone.AddCell(names1);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);

            PdfPCell idcards = new PdfPCell(new Phrase("Dated : ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            idcards.HorizontalAlignment = 0;
            idcards.Colspan = 2;
            idcards.Border = 0;
            tablesnewone.AddCell(idcards);


            PdfPCell noofdays = new PdfPCell(new Phrase("Signature of the Employer/Manager \n  or otherAuthorised Officer", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            noofdays.HorizontalAlignment = 0;
            noofdays.Colspan = 2;
            noofdays.Border = 0;
            tablesnewone.AddCell(noofdays);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);




            document.Add(tablesnewone);

            #endregion Basic Information of the Employee

            document.NewPage();

            PdfPTable tblFingerprints = new PdfPTable(6);
            tblFingerprints.TotalWidth = 500f;
            tblFingerprints.LockedWidth = true;
            float[] widthfinger = new float[] { 2f, 1.5f, 2f, 2f, 1.5f, 2f };
            tblFingerprints.SetWidths(widthfinger);

            string filename = "formX.pdf";

            document.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            //}

            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Duration expired');", true);
            //    return;
            //}
        }

        protected void btnform13_Click(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string fontsyle = "verdana";


            if (txtemplyid.Text.Trim().Length== 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
                return;
            }

            //#region  Begin  New Code

            #region  Begin  New Code

            #region Begin Variable Declaration

            string SPName = "";
            Hashtable HTEmpBiodata = new Hashtable();
            string Empid = "";
            DataTable dtEmpdetails = null;

            #endregion end Variable Declaration


            #region Begin Assign Values to The Variables
            SPName = "EmployeeFormsPDF";
            Empid = txtemplyid.Text;

            string date = "";

            var DtofJoining = string.Empty;
            if (txtEmpDtofJoining.Text.Trim().Length != 0)
            {
                DtofJoining = Timings.Instance.CheckDateFormat(txtEmpDtofJoining.Text);

            }
            else
            {
                DtofJoining = "01/01/1900";
            }


            if (TxtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(TxtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = "";
            string Year = "";
            //string month = DateTime.Parse(date).Month.ToString();
            // string Year = DateTime.Parse(date).Year.ToString();


            #endregion End Assign values To the Variables

            #region Begin Pass values to the Hash table
            HTEmpBiodata.Add("@empid", Empid);
            // HTEmpBiodata.Add("@month", month + Year.Substring(2, 2));
            HTEmpBiodata.Add("@DtofJoining", DtofJoining);
            #endregion  end Pass values to the Hash table

            #region Begin  Call Stored Procedure
            dtEmpdetails = config.ExecuteAdaptorAsyncWithParams(SPName, HTEmpBiodata).Result;
            #endregion  End  Call Stored Procedure

            #endregion End New Code As on [31-05-2014]


            string name = "";
            string fathername = "";
            string Gender = "";
            string Companyname = "";
            string CompanyAddress = "";
            string remarks = "";
            string accno = "";
            string Empdateofbirth = "";
            string contactno = "";
            string emailid = "";
            string empifsccode = "";
            int j = 1;
            if (dtEmpdetails.Rows.Count > 0)
            {

                name = dtEmpdetails.Rows[0]["EmployeeName"].ToString();
                fathername = dtEmpdetails.Rows[0]["EmpFatherName"].ToString();
                date = dtEmpdetails.Rows[0]["DtOfJoining"].ToString();
                Empdateofbirth = dtEmpdetails.Rows[0]["DtOfBirth"].ToString();
                Gender = dtEmpdetails.Rows[0]["gender"].ToString();
                Companyname = dtEmpdetails.Rows[0]["CompanyName"].ToString();
                CompanyAddress = dtEmpdetails.Rows[0]["CompanyAddress"].ToString();
                remarks = dtEmpdetails.Rows[0]["EmpRemarks"].ToString();
                accno = dtEmpdetails.Rows[0]["EmpBankAcNo"].ToString();
                contactno = dtEmpdetails.Rows[0]["EmpPhone"].ToString();
                emailid = dtEmpdetails.Rows[0]["Emailid"].ToString();
                empifsccode = dtEmpdetails.Rows[0]["EmpIFSCcode"].ToString();

            }
            string pfemplyee = "";
            string prevpfno = "";
            string prevcmpny = "";
            string prevdofresign = "";

            string prevqry = " select PFNo,YrOfExp,Designation,CompAddress,DateofResign from EmpPrevExperience where empid='" +txtemplyid.Text + "'";
            DataTable dtprev = config.ExecuteAdaptorAsyncWithQueryParams(prevqry).Result;
            if (dtprev.Rows.Count > 0)
            {

                pfemplyee = dtprev.Rows[0]["YrOfExp"].ToString();
                prevpfno = dtprev.Rows[0]["PFNo"].ToString();
                prevcmpny = dtprev.Rows[0]["CompAddress"].ToString();
                prevdofresign = dtprev.Rows[0]["DateofResign"].ToString();
            }

            string strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + txtemplyid.Text+ "'";
            string pfNo = "";
            DataTable PfTable = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            if (PfTable.Rows.Count > 0)
            {
                pfNo = PfTable.Rows[0]["EmpEpfNo"].ToString();
            }

            MemoryStream ms = new MemoryStream();


            Document document = new Document(PageSize.LEGAL);
            // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
            var writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            string imagepath1 = Server.MapPath("images");
            #region

            PdfPTable tablesnewone = new PdfPTable(4);
            tablesnewone.TotalWidth = 500f;
            tablesnewone.LockedWidth = true;
            float[] width = new float[] { 3f, 2f, 3f, 2f };
            tablesnewone.SetWidths(width);

            PdfPCell space = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            space.HorizontalAlignment = 1;
            space.Colspan = 5;
            space.Border = 0;

            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);




            PdfPCell TRANSFER = new PdfPCell(new Phrase("TRANSFER CLAIM FORM ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            TRANSFER.HorizontalAlignment = 0;
            TRANSFER.Colspan = 2;
            TRANSFER.Border = 0;
            tablesnewone.AddCell(TRANSFER);

            PdfPCell CLAIM = new PdfPCell(new Phrase("CLAIM ID", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            CLAIM.HorizontalAlignment = 2;
            CLAIM.Colspan = 2;
            CLAIM.Border = 0;
            tablesnewone.AddCell(CLAIM);

            PdfPCell FORM = new PdfPCell(new Phrase("FORM 13 (REVISED) ", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            FORM.HorizontalAlignment = 0;
            FORM.Colspan = 2;
            FORM.Border = 0;
            tablesnewone.AddCell(FORM);

            PdfPCell CLAIMid = new PdfPCell(new Phrase("(For EPFO Use only)", FontFactory.GetFont(FontStyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
            CLAIMid.HorizontalAlignment = 2;
            CLAIMid.Colspan = 2;
            CLAIMid.Border = 0;
            tablesnewone.AddCell(CLAIMid); tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);


            string imagepath = Server.MapPath("~/assets/BillLogo.png");
            if (File.Exists(imagepath))
            {
                iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(imagepath);

                gif2.Alignment = (iTextSharp.text.Image.ALIGN_LEFT | iTextSharp.text.Image.UNDERLYING);
                // gif2.SpacingBefore = 50;
                gif2.ScalePercent(40f);
                gif2.SetAbsolutePosition(60f, 870f);
                //document.Add(new Paragraph(" "));
                document.Add(gif2);
            }


            PdfPCell PROVIDENT = new PdfPCell(new Phrase("EMPLOYEE’S PROVIDENT FUND SCHEME, 1952 \n (PARA 57)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            PROVIDENT.HorizontalAlignment = 1;
            PROVIDENT.Colspan = 4;
            PROVIDENT.Border = 0;
            tablesnewone.AddCell(PROVIDENT);

            PdfPCell APPLICATION = new PdfPCell(new Phrase("[APPLICATION FOR THE TRANSFER OF EPF ACCOUNT FROM UNEXEMPTED ESTABLISHMENT TOEXEMPTED OR UNEXEMPTED ESTABLISHMENT]", FontFactory.GetFont(FontStyle, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
            APPLICATION.HorizontalAlignment = 1;
            APPLICATION.Colspan = 4;
            APPLICATION.Border = 0;
            tablesnewone.AddCell(APPLICATION);

            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);

            PdfPCell to1 = new PdfPCell(new Phrase("To,", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            to1.HorizontalAlignment = 0;
            to1.Colspan = 2;
            to1.Border = 0;
            tablesnewone.AddCell(to1);


            PdfPCell to2 = new PdfPCell(new Phrase("To,", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            to2.HorizontalAlignment = 0;
            to2.Colspan = 2;
            to2.Border = 0;
            tablesnewone.AddCell(to2);


            PdfPCell regional = new PdfPCell(new Phrase("The Regional PF Commissioner,", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            regional.HorizontalAlignment = 0;
            regional.Colspan = 2;
            regional.Border = 0;
            tablesnewone.AddCell(regional);


            PdfPCell trustname = new PdfPCell(new Phrase("Trust Name", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            trustname.HorizontalAlignment = 0;
            trustname.Colspan = 2;
            trustname.Border = 0;
            tablesnewone.AddCell(trustname);


            PdfPCell officename = new PdfPCell(new Phrase("Office Name:", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            officename.HorizontalAlignment = 0;
            officename.Colspan = 2;
            officename.Border = 0;
            tablesnewone.AddCell(officename);


            PdfPCell trustnameadd = new PdfPCell(new Phrase("Trust Address:", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            trustnameadd.HorizontalAlignment = 0;
            trustnameadd.Colspan = 2;
            trustnameadd.Border = 0;
            tablesnewone.AddCell(trustnameadd);


            PdfPCell addressoffice1 = new PdfPCell(new Phrase("Office Address :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            addressoffice1.HorizontalAlignment = 0;
            addressoffice1.Colspan = 2;
            addressoffice1.Border = 0;
            tablesnewone.AddCell(addressoffice1);


            PdfPCell addrsss1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            addrsss1.HorizontalAlignment = 0;
            addrsss1.Colspan = 2;
            addrsss1.Border = 0;
            tablesnewone.AddCell(addrsss1);


            PdfPCell addressoffice = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            addressoffice.HorizontalAlignment = 0;
            addressoffice.Colspan = 2;
            addressoffice.Border = 0;
            tablesnewone.AddCell(addressoffice);


            PdfPCell addrsss2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            addrsss2.HorizontalAlignment = 0;
            addrsss2.Colspan = 2;
            addrsss2.Border = 0;
            tablesnewone.AddCell(addrsss2);


            PdfPCell incase1 = new PdfPCell(new Phrase("(Please see instruction 3)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            incase1.HorizontalAlignment = 0;
            incase1.Colspan = 2;
            incase1.Border = 0;
            tablesnewone.AddCell(incase1);


            PdfPCell incase = new PdfPCell(new Phrase("(In case the PF A/C is with Exempted Establishment", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            incase.HorizontalAlignment = 0;
            incase.Colspan = 2;
            incase.Border = 0;
            tablesnewone.AddCell(incase);

            PdfPCell cellsir = new PdfPCell(new Phrase("Sir", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            cellsir.HorizontalAlignment = 0;
            cellsir.Colspan = 4;
            cellsir.Border = 0;
            tablesnewone.AddCell(cellsir);

            PdfPCell request = new PdfPCell(new Phrase("                 I request that my provident fund balance along with my pension service details may please be transferred to my present account under intimation to me. My details are as under:", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            request.HorizontalAlignment = 0;
            request.Colspan = 4;
            request.SetLeading(0f, 1.5f);
            request.Border = 0;
            tablesnewone.AddCell(request);

            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);

            PdfPCell Parta = new PdfPCell(new Phrase("PART A: PERSONAL INFORMATION", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Parta.HorizontalAlignment = 1;
            Parta.Colspan = 4;
            Parta.Border = 0;
            tablesnewone.AddCell(Parta);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);

            PdfPCell nameemp = new PdfPCell(new Phrase("1. *Name :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            nameemp.HorizontalAlignment = 0;
            nameemp.Colspan = 2;
            nameemp.Border = 0;
            tablesnewone.AddCell(nameemp);
            PdfPCell nameemp1 = new PdfPCell(new Phrase(name, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            nameemp1.HorizontalAlignment = 0;
            nameemp1.Colspan = 2;
            nameemp1.Border = 0;
            tablesnewone.AddCell(nameemp1);
            tablesnewone.AddCell(space);


            PdfPCell fathername1 = new PdfPCell(new Phrase("2. *Father’s/Husband’s name :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            fathername1.HorizontalAlignment = 0;
            fathername1.Colspan = 2;
            fathername1.Border = 0;
            tablesnewone.AddCell(fathername1);
            PdfPCell fathername12 = new PdfPCell(new Phrase(fathername, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            fathername12.HorizontalAlignment = 0;
            fathername12.Colspan = 2;
            fathername12.Border = 0;
            tablesnewone.AddCell(fathername12);
            tablesnewone.AddCell(space);


            PdfPCell mobileno = new PdfPCell(new Phrase("3. Mobile Number :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            mobileno.HorizontalAlignment = 0;
            mobileno.Colspan = 1;
            mobileno.Border = 0;
            tablesnewone.AddCell(mobileno);
            PdfPCell mobileno1 = new PdfPCell(new Phrase(contactno, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            mobileno1.HorizontalAlignment = 0;
            mobileno1.Colspan = 1;
            mobileno1.Border = 0;
            tablesnewone.AddCell(mobileno1);

            PdfPCell email = new PdfPCell(new Phrase("4. Email Id :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            email.HorizontalAlignment = 0;
            email.Colspan = 1;
            email.Border = 0;
            tablesnewone.AddCell(email);
            PdfPCell email1 = new PdfPCell(new Phrase(emailid, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            email.HorizontalAlignment = 0;
            email1.Colspan = 1;
            email1.Border = 0;
            tablesnewone.AddCell(email1);

            PdfPCell bankacno = new PdfPCell(new Phrase("5. Bank A/C Noumber :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            bankacno.HorizontalAlignment = 0;
            bankacno.Colspan = 1;
            bankacno.Border = 0;
            tablesnewone.AddCell(bankacno);
            PdfPCell bankacno1 = new PdfPCell(new Phrase(accno, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            bankacno1.HorizontalAlignment = 0;
            bankacno1.Colspan = 1;
            bankacno1.Border = 0;
            tablesnewone.AddCell(bankacno1);

            PdfPCell ifscode = new PdfPCell(new Phrase("6. IFS Code of Bank branch :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            ifscode.HorizontalAlignment = 0;
            ifscode.Colspan = 1;
            ifscode.Border = 0;
            tablesnewone.AddCell(ifscode);
            PdfPCell ifscode1 = new PdfPCell(new Phrase(empifsccode, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            ifscode1.HorizontalAlignment = 0;
            ifscode1.Colspan = 1;
            ifscode1.Border = 0;
            tablesnewone.AddCell(ifscode1);

            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);

            PdfPCell partb = new PdfPCell(new Phrase("PART B: DETAILS OF PREVIOUS ACCOUNT (WHICH IS TO BE TRANSFERRED)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            partb.HorizontalAlignment = 1;
            partb.Colspan = 4;
            partb.Border = 0;
            tablesnewone.AddCell(partb);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);


            PdfPCell pfacno = new PdfPCell();
            Paragraph Ppfacno = new Paragraph();
            Ppfacno.Add(new Chunk("1. *PF Account No. :    ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Ppfacno.Add(new Chunk(prevpfno, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            Ppfacno.Add(new Chunk("   In case the previous establishment is exempted under Employees’ Provident Fund Scheme, 1952 Pension Fund Account No. :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            pfacno.AddElement(Ppfacno);
            pfacno.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            pfacno.Colspan = 4;
            pfacno.Border = 0;// 15;
            tablesnewone.AddCell(pfacno);
            tablesnewone.AddCell(space);

            PdfPCell cellname1 = new PdfPCell(new Phrase("2. *Name and Address of the previous establishment:", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellname1.HorizontalAlignment = 0;
            cellname1.Colspan = 2;
            cellname1.Border = 0;
            tablesnewone.AddCell(cellname1);
            PdfPCell cellname11 = new PdfPCell(new Phrase(prevcmpny, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            cellname11.HorizontalAlignment = 0;
            cellname11.Colspan = 2;
            cellname11.Border = 0;
            tablesnewone.AddCell(cellname11);
            tablesnewone.AddCell(space);


            PdfPCell pfaccno = new PdfPCell(new Phrase("3. *PF Account is held by: (Name of EPF Office/ PF Trust) ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            pfaccno.HorizontalAlignment = 0;
            pfaccno.Colspan = 4;
            pfaccno.Border = 0;
            tablesnewone.AddCell(pfaccno);
            tablesnewone.AddCell(space);

            PdfPCell dob = new PdfPCell(new Phrase("4. *Date of Birth:(dd/mm/yyyy)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            dob.HorizontalAlignment = 0;
            dob.Colspan = 1;
            dob.Border = 0;
            tablesnewone.AddCell(dob);
            PdfPCell dob1 = new PdfPCell(new Phrase(Empdateofbirth, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            dob1.HorizontalAlignment = 0;
            dob1.Colspan = 1;
            dob1.Border = 0;
            tablesnewone.AddCell(dob1);

            PdfPCell datejoining = new PdfPCell(new Phrase(" 5. *Date of joining:(dd/mm/yyyy)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            datejoining.HorizontalAlignment = 0;
            datejoining.Colspan = 1;
            datejoining.Border = 0;
            tablesnewone.AddCell(datejoining);
            PdfPCell datejoining1 = new PdfPCell(new Phrase(date, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            datejoining1.HorizontalAlignment = 0;
            datejoining1.Colspan = 1;
            datejoining1.Border = 0;
            tablesnewone.AddCell(datejoining1);
            tablesnewone.AddCell(space);

            PdfPCell dateleaving = new PdfPCell(new Phrase("6. *Date of leaving:(dd/mm/yyyy)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            dateleaving.HorizontalAlignment = 0;
            dateleaving.Colspan = 2;
            dateleaving.Border = 0;
            tablesnewone.AddCell(dateleaving);
            PdfPCell dateleaving1 = new PdfPCell(new Phrase(prevdofresign, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            dateleaving1.HorizontalAlignment = 0;
            dateleaving1.Colspan = 2;
            dateleaving1.Border = 0;
            tablesnewone.AddCell(dateleaving1);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);




            PdfPCell partc = new PdfPCell(new Phrase("PART C: DETAILS OF PRESENT ACCOUNT", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            partc.HorizontalAlignment = 1;
            partc.Colspan = 4;
            partc.Border = 0;
            tablesnewone.AddCell(partc);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);


            PdfPCell pfacno1 = new PdfPCell();
            Paragraph Ppfacno1 = new Paragraph();
            Ppfacno1.Add(new Chunk("1. *PF Account No. :   ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            Ppfacno1.Add(new Chunk(pfNo, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            Ppfacno1.Add(new Chunk("   In case the previous establishment is exempted under Employees’ Provident Fund Scheme, 1952 Pension Fund Account No. :", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            pfacno1.AddElement(Ppfacno1);
            pfacno1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            pfacno1.Colspan = 4;
            pfacno1.Border = 0;// 15;
            tablesnewone.AddCell(pfacno1);
            tablesnewone.AddCell(space);


            PdfPCell cellname111 = new PdfPCell(new Phrase("2. *Name and Address of the present establishment:", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellname111.HorizontalAlignment = 0;
            cellname111.Colspan = 2;
            cellname111.Border = 0;
            tablesnewone.AddCell(cellname111);
            PdfPCell cellname1111 = new PdfPCell(new Phrase(Companyname + "," + CompanyAddress, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            cellname1111.HorizontalAlignment = 0;
            cellname1111.Colspan = 2;
            cellname1111.Border = 0;
            tablesnewone.AddCell(cellname1111);
            tablesnewone.AddCell(space);

            PdfPCell pfaccno1 = new PdfPCell(new Phrase("3. *Account is held by: (Name of EPF Office / PF Trust)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            pfaccno1.HorizontalAlignment = 0;
            pfaccno1.Colspan = 4;
            pfaccno1.Border = 0;
            tablesnewone.AddCell(pfaccno1);
            tablesnewone.AddCell(space);

            PdfPCell datejoining12 = new PdfPCell(new Phrase("4. *Date of joining:(dd/mm/yyyy)", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            datejoining12.HorizontalAlignment = 0;
            datejoining12.Colspan = 2;
            datejoining12.Border = 0;
            tablesnewone.AddCell(datejoining12);
            PdfPCell datejoining121 = new PdfPCell(new Phrase(date, FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            datejoining121.HorizontalAlignment = 0;
            datejoining121.Colspan = 2;
            datejoining121.Border = 0;
            tablesnewone.AddCell(datejoining121);
            tablesnewone.AddCell(space);

            PdfPCell nameoftrust = new PdfPCell(new Phrase("5. #Name of Trust (to whom funds are to be paid in case of present establishment being exempted under EPF Scheme, 1952): ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            nameoftrust.HorizontalAlignment = 0;
            nameoftrust.Colspan = 4;
            nameoftrust.SetLeading(0f, 1.5f);
            nameoftrust.Border = 0;
            tablesnewone.AddCell(nameoftrust);
            tablesnewone.AddCell(space);



            PdfPCell empcode = new PdfPCell(new Phrase("6. #Employee code under the Trust: ", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            empcode.HorizontalAlignment = 0;
            empcode.Colspan = 4;
            empcode.Border = 0;
            tablesnewone.AddCell(empcode);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);
            tablesnewone.AddCell(space);


            PdfPCell indicates = new PdfPCell(new Phrase("(* indicates mandatory fields)	", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            indicates.HorizontalAlignment = 0;
            indicates.Colspan = 2;
            indicates.Border = 0;
            tablesnewone.AddCell(indicates);

            PdfPCell strike = new PdfPCell(new Phrase("(# Strike off if not applicable)", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            strike.HorizontalAlignment = 1;
            strike.Colspan = 2;
            strike.Border = 0;
            tablesnewone.AddCell(strike);


            document.Add(tablesnewone);

            document.NewPage();
            Document document1 = new Document(PageSize.LEGAL);


            PdfPTable tablesnewone1 = new PdfPTable(4);
            tablesnewone1.TotalWidth = 500f;
            tablesnewone1.LockedWidth = true;
            float[] width1 = new float[] { 2f, 2f, 2f, 2f };
            tablesnewone1.SetWidths(width1);

            PdfPCell cellspace = new PdfPCell(new Phrase("", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellspace.HorizontalAlignment = 0;
            cellspace.Colspan = 4;
            cellspace.Border = 0;
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);




            PdfPCell Certify = new PdfPCell(new Phrase("I, Certify that all the information given above is true to the best of my knowledge and I have ensured the correctness of my present and previous account numbers.", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Certify.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Certify.Colspan = 4;
            Certify.SetLeading(0f, 1.5f);
            Certify.Border = 0;
            tablesnewone1.AddCell(Certify);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);

            PdfPCell Attested = new PdfPCell(new Phrase("Signature of the Member", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Attested.HorizontalAlignment = 2;
            Attested.Colspan = 4;
            Attested.Border = 0;
            tablesnewone1.AddCell(Attested);

            tablesnewone1.AddCell(cellspace);


            PdfPCell cell5 = new PdfPCell(new Phrase("Date:_____________________", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            cell5.HorizontalAlignment = 2;
            cell5.Colspan = 4;
            cell5.Border = 0;
            tablesnewone1.AddCell(cell5);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);

            PdfPCell IMPORTANT = new PdfPCell(new Phrase("IMPORTANT: Member has the option to get the claim form attested by present or previous employer.In case of attestation by the previous employer, time taken in settlement will be relatively less.", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            IMPORTANT.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            IMPORTANT.Colspan = 4;
            IMPORTANT.Border = 0;
            tablesnewone1.AddCell(IMPORTANT);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);


            PdfPCell fellname1 = new PdfPCell(new Phrase("Certified that I have verified the data in Part B in respect of the member mentioned in Part A of this form and the signature of the member.	", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            fellname1.HorizontalAlignment = 0;
            fellname1.Colspan = 4;
            fellname1.Border = 0;
            tablesnewone1.AddCell(fellname1);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);

            PdfPCell insno = new PdfPCell(new Phrase("Signature of Previous Employer", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            insno.HorizontalAlignment = 2;
            insno.Colspan = 4;
            insno.Border = 0;
            tablesnewone1.AddCell(insno); tablesnewone1.AddCell(cellspace);

            PdfPCell insno1 = new PdfPCell(new Phrase("Seal of the Establishment", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            insno1.HorizontalAlignment = 0;
            insno1.Colspan = 2;
            insno1.Border = 0;
            tablesnewone1.AddCell(insno1);


            PdfPCell namedesignation = new PdfPCell(new Phrase("Date:____________________", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            namedesignation.HorizontalAlignment = 2;
            namedesignation.Colspan = 2;
            namedesignation.Border = 0;
            tablesnewone1.AddCell(namedesignation);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);



            PdfPCell application = new PdfPCell(new Phrase("Certified that I have verified the data in Part C in respect of the member mentioned in Part A of this form.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            application.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            application.Colspan = 4;
            application.SetLeading(0.0f, 1.5f);
            application.Border = 0;
            tablesnewone1.AddCell(application);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);

            PdfPCell codeno = new PdfPCell(new Phrase("Signature of Present Employer", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            codeno.HorizontalAlignment = 2;
            codeno.Colspan = 4;
            codeno.Border = 0;
            tablesnewone1.AddCell(codeno);
            tablesnewone1.AddCell(cellspace);


            PdfPCell codeno1 = new PdfPCell(new Phrase("Seal of the Establishment", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            codeno1.HorizontalAlignment = 0;
            codeno1.Colspan = 2;
            codeno1.Border = 0;
            tablesnewone1.AddCell(codeno1);



            PdfPCell lono = new PdfPCell(new Phrase("Date:__________________", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            lono.HorizontalAlignment = 2;
            lono.Colspan = 2;
            lono.Border = 0;
            tablesnewone1.AddCell(lono);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);

            PdfPCell empty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            empty.HorizontalAlignment = 0;
            empty.Colspan = 4;
            empty.BorderWidthBottom = 0;
            empty.BorderWidthLeft = 0;
            empty.BorderWidthRight = 0;
            empty.BorderWidthTop = .7f;
            tablesnewone1.AddCell(empty);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);

            PdfPCell lono1 = new PdfPCell(new Phrase("INSTRUCTIONS AND GUIDELINES", FontFactory.GetFont(FontStyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
            lono1.HorizontalAlignment = 1;
            lono1.Colspan = 4;
            lono1.Border = 0;
            tablesnewone1.AddCell(lono1);
            tablesnewone1.AddCell(cellspace);
            tablesnewone1.AddCell(cellspace);


            PdfPCell no1 = new PdfPCell(new Phrase("1.	The Bank A/C details are for verification purpose even if the Fund is transferred to the EPFO Office/Trust maintaining the present account number.\n 2.In case the Previous Account was maintained by PF Trust of the exempted establishment, the member should submit a Transfer Claim Form { Form - 13(Revised) } to the Trust while sending another Transfer Claim Form { Form - 13(Revised)} to the PF Office for transferring the service details under the Pension Fund to the new account.\n 3.The Form should be submitted to that PF Office under which previous or the present account is maintained, depending upon as to which employer has attested the claim. (In case the claim is attested by the present employer, claim should be submitted with the PF Office under which the present account is maintained, and so on).\n 4.The mobile number(wherever provided) of the member would be used for sending an SMS alert informing him / her the processing of his / her claim and is non - mandatory for Physical form.", FontFactory.GetFont(FontStyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            no1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            no1.Colspan = 4;
            no1.Border = 0;
            no1.SetLeading(0f, 1.5f);
            tablesnewone1.AddCell(no1);






            document.Add(tablesnewone1);



            #endregion Basic Information of the Employee

            document.NewPage();

            PdfPTable tblFingerprints = new PdfPTable(6);
            tblFingerprints.TotalWidth = 500f;
            tblFingerprints.LockedWidth = true;
            float[] widthfinger = new float[] { 2f, 1.5f, 2f, 2f, 1.5f, 2f };
            tblFingerprints.SetWidths(widthfinger);

            string filename = "formX.pdf";

            document.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            //}

            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Duration expired');", true);
            //    return;
            //}
        }

        protected void btnform5_Click(object sender, EventArgs e)
        {


            int Fontsize = 13;
            string fontsyle = "verdana";

            #region Variable Declaration

            string contactno = "";
            string Idno = "";
            string date = "";
            string postappliedfor = "";
            string name = "";
            string fathername = "";
            string dateofbirth = "";
            string educationqualification = "";
            string TechnicalQualification = "";
            string previouspost = "";
            string nationality = "";
            string community = "";
            string maritalstatus = "";
            string height = "";
            string weight = "";
            string chest = "";
            string bloodgroup = "";
            string identificationmark1 = "";
            string identificationmark2 = "";

            string prdoorno = "";
            string prstreet = "";
            string prarea = "";
            string prcity = "";
            string prLmark = "";
            string prDistrict = "";
            string prPincode = "";
            string prState = "";


            string pedoor = "";
            string pestreet = "";
            string pearea = "";
            string pecity = "";
            string pelmark = "";
            string peDistrict = "";
            string pePincode = "";
            string peState = "";

            string refaddress1 = "";
            string refaddress2 = "";

            string sscschool = "";
            string sscbduniversity = "";
            string sscstdyear = "";

            string imschool = "";
            string imbduniversity = "";
            string imstdyear = "";

            string dgschool = "";
            string dgbduniversity = "";
            string dgstdyear = "";

            string pgschool = "";
            string pgbduniversity = "";
            string pgstdyear = "";
            string EmpCertfDet1 = "";

            float EmpsecurityDeposit = 0;
            string Referedby = "";
            string clientname = "";


            string relationName = "";
            string relationAge = "";
            string relationType = "";


            string EmpCertfDet2 = "";
            string EmpCertfDet3 = "";
            string EmpCertfDet4 = "";

            string Original1 = "";
            string Original2 = "";
            string Original3 = "";
            string Original4 = "";

            string Xerox1 = "";
            string Xerox2 = "";
            string Xerox3 = "";
            string Xerox4 = "";

            string Ref1Phone1 = "";
            string Ref1Phone2 = "";
            string Ref2Phone1 = "";
            string Ref2Phone2 = "";

            string ReplacementFor = "";
            string PlaceofBirth = "";
            string Haircolour = "";
            string eyecolour = "";
            string Complexion = "";
            string Languagesknown = "";
            string EmergencyPhone = "";
            string Fname = "";
            string Fage = "";
            string Mname = "";
            string Mage = "";
            string relationoccupation = "";
            string relationresidence = "";
            string relationplace = "";
            string Esino = "";
            string prphone = "";
            string pephone = "";

            #endregion



            if (txtemplyid.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
                return;
            }

            //if (txtEmpDtofJoining == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
            //    return;
            //}

            //#region  Begin  New Code

            #region  Begin  New Code

            #region Begin Variable Declaration

            string SPName = "";
            Hashtable HTEmpBiodata = new Hashtable();
            string Empid = "";
            DataTable dtEmpdetails = null;

            #endregion end Variable Declaration


            #region Begin Assign Values to The Variables
            SPName = "EmployeeFormsPDF";
            Empid = txtemplyid.Text;


            var DtofJoining = string.Empty;
            if (txtEmpDtofJoining.Text.Trim().Length != 0)
            {
                DtofJoining = Timings.Instance.CheckDateFormat(txtEmpDtofJoining.Text);

            }
            else
            {
                DtofJoining = "01/01/1900";
            }


            if (TxtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(TxtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = "";
            string Year = "";
            //string month = DateTime.Parse(date).Month.ToString();
            // string Year = DateTime.Parse(date).Year.ToString();


            #endregion End Assign values To the Variables

            #region Begin Pass values to the Hash table
            HTEmpBiodata.Add("@empid", Empid);
            // HTEmpBiodata.Add("@month", month + Year.Substring(2, 2));
            HTEmpBiodata.Add("@DtofJoining", DtofJoining);
            #endregion  end Pass values to the Hash table

            #region Begin  Call Stored Procedure
            dtEmpdetails = config.ExecuteAdaptorAsyncWithParams(SPName, HTEmpBiodata).Result;
            #endregion  End  Call Stored Procedure

            #endregion End New Code As on [31-05-2014]



            string Gender = "";
            string Companyname = "";
            string CompanyAddress = "";
            string remarks = "";
            string accno = "";
            string Empdateofbirth = "";
            int j = 1;
            if (dtEmpdetails.Rows.Count > 0)
            {

                name = dtEmpdetails.Rows[0]["EmployeeName"].ToString();
                fathername = dtEmpdetails.Rows[0]["EmpFatherName"].ToString();
                date = dtEmpdetails.Rows[0]["DtOfJoining"].ToString();
                Empdateofbirth = dtEmpdetails.Rows[0]["DtOfBirth"].ToString();

                Gender = dtEmpdetails.Rows[0]["gender"].ToString();
                Companyname = dtEmpdetails.Rows[0]["CompanyName"].ToString();
                CompanyAddress = dtEmpdetails.Rows[0]["CompanyAddress"].ToString();
                remarks = dtEmpdetails.Rows[0]["EmpRemarks"].ToString();
                accno = dtEmpdetails.Rows[0]["EmpBankAcNo"].ToString();




            }

            DateTime frmdate;
            string FromDate = "";
            string Frmonth = "";
            string FrYear = "";

            if (txtfrom.Text.Trim().Length > 0)
            {
                frmdate = DateTime.ParseExact(txtfrom.Text.Trim(), "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Frmonth = frmdate.ToString("MM");
                FrYear = frmdate.ToString("yy");
            }



            FromDate = FrYear + Frmonth;


            DateTime tdate;
            string ToDate = "";
            string Tomonth = "";
            string ToYear = "";

            if (txtto.Text.Trim().Length > 0)
            {
                tdate = DateTime.ParseExact(txtto.Text.Trim(), "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Tomonth = tdate.ToString("MM");
                ToYear = tdate.ToString("yy");

            }

            ToDate = ToYear + Tomonth;



            string dqry = "select eps.empid,(empfname+' '+empmname+' '+emplname) as empname,case convert(varchar(10),EmpDtofJoining,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofJoining,103) end EmpDtofJoining ,month from emppaysheet eps inner join empdetails e on e.empid=eps.empid where eps.Monthnew between '" + FromDate.ToString() + "' and '" + ToDate.ToString() + "' order by eps.Monthnew asc ";
            dtEmpdetails = config.ExecuteAdaptorAsyncWithQueryParams(dqry).Result;

            if (dtEmpdetails.Rows.Count > 0)
            {
                name = dtEmpdetails.Rows[0]["empname"].ToString();
                date = dtEmpdetails.Rows[0]["EmpDtofJoining"].ToString();

            }

            string strQry = "Select p.EmpEpfNo,e.EmpESINo from EMPESICodes AS e INNER JOIN EMPEPFCodes as p ON e.Empid = p.Empid AND e.Empid='" + txtemplyid.Text+ "'";
            string pfNo = "";
            DataTable PfTable = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            if (PfTable.Rows.Count > 0)
            {
                pfNo = PfTable.Rows[0]["EmpEpfNo"].ToString();
            }
            string cmpnypfno = "";
            string qry = "select PFNo from CompanyInfo ";
            DataTable dttblroptns = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dttblroptns.Rows.Count > 0)
            {
                cmpnypfno = dttblroptns.Rows[0]["PFNo"].ToString();
            }
            MemoryStream ms = new MemoryStream();

            Document document = new Document(PageSize.LEGAL.Rotate());

            // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
            var writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            string imagepath1 = Server.MapPath("images");
            #region

            PdfPTable tablenewc = new PdfPTable(9);
            tablenewc.TotalWidth = 900f;
            tablenewc.LockedWidth = true;
            float[] width = new float[] { 1.5f, 4f, 5f, 5f, 3f, 2f, 3f, 7f, 3.5f };
            tablenewc.SetWidths(width);

            PdfPCell cellspace = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
            cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cellspace.Colspan = 11;
            cellspace.Border = 0;
            cellspace.PaddingTop = 0;

            PdfPCell cellHead = new PdfPCell(new Phrase("Form-5 ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            cellHead.HorizontalAlignment = 1;
            cellHead.Colspan = 11;
            cellHead.Border = 0;
            tablenewc.AddCell(cellHead);

            PdfPCell provident = new PdfPCell(new Phrase("Employees' Provident Fund Organisation", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK)));
            provident.HorizontalAlignment = 1;
            provident.Colspan = 11;
            provident.Border = 0;
            tablenewc.AddCell(provident);

            PdfPCell cellemp = new PdfPCell(new Phrase("THE EMPLOYEES' PROVIDENT FUND SCHEME,1952[Para 36 (2) (a) ] AND THE EMPLOYEES' PENSION SCHEME,1995 [Para 20 (4) ] ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            cellemp.HorizontalAlignment = 1;
            cellemp.Colspan = 11;
            cellemp.Border = 0;
            tablenewc.AddCell(cellemp);

            PdfPCell cellreturn = new PdfPCell(new Phrase("Return of Employees' qualifying for membership of the Employees' Provident Fund , Employees' Pension Fund & Employees' Deposit Linked Insurance Fund for the first time ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
            cellreturn.HorizontalAlignment = 0;
            cellreturn.Colspan = 11;
            cellreturn.Border = 0;
            tablenewc.AddCell(cellreturn);

            PdfPCell cellmonth = new PdfPCell(new Phrase("during the month of  :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
            cellmonth.HorizontalAlignment = 0;
            cellmonth.Colspan = 11;
            cellmonth.Border = 0;
            tablenewc.AddCell(cellmonth);

            PdfPCell nameaddress = new PdfPCell(new Phrase("Name and address of the Factory / Estt.:" + CompanyAddress + CompanyAddress, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
            nameaddress.HorizontalAlignment = 0;
            nameaddress.Colspan = 11;
            nameaddress.Border = 0;
            tablenewc.AddCell(nameaddress);

            PdfPCell cellcode = new PdfPCell(new Phrase("Code No. of the Factory / Estt.:" + cmpnypfno, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK)));
            cellcode.HorizontalAlignment = 0;
            cellcode.Colspan = 11;
            cellcode.Border = 0;
            tablenewc.AddCell(cellcode);

            tablenewc.AddCell(cellspace);

            PdfPCell celltable10 = new PdfPCell(new Phrase("SL.No. ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            celltable10.HorizontalAlignment = 1;
            celltable10.Colspan = 0;
            tablenewc.AddCell(celltable10);

            PdfPCell Account = new PdfPCell(new Phrase("Account No.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Account.HorizontalAlignment = 1;
            Account.Colspan = 0;
            tablenewc.AddCell(Account);

            PdfPCell Name = new PdfPCell(new Phrase("Name Of the Member (In the  block capitals) ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Name.HorizontalAlignment = 1;
            Name.Colspan = 0;
            tablenewc.AddCell(Name);

            PdfPCell Father = new PdfPCell(new Phrase("Father Name (or husband's name in case of married women)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Father.HorizontalAlignment = 1;
            Father.Colspan = 0;
            tablenewc.AddCell(Father);

            PdfPCell birth = new PdfPCell(new Phrase("Date of birth ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            birth.HorizontalAlignment = 1;
            birth.Colspan = 0;
            tablenewc.AddCell(birth);

            PdfPCell Sex = new PdfPCell(new Phrase("Sex", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Sex.HorizontalAlignment = 1;
            Sex.Colspan = 0;
            tablenewc.AddCell(Sex);

            PdfPCell Date = new PdfPCell(new Phrase("Date of Joining the Fund", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Date.HorizontalAlignment = 1;
            Date.Colspan = 0;
            tablenewc.AddCell(Date);

            PdfPCell Total = new PdfPCell(new Phrase("Total period of previous service as on the date of joining the Fund (Enclose scheme Certificate if applicable)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Total.HorizontalAlignment = 1;
            Total.Colspan = 0;
            tablenewc.AddCell(Total);

            PdfPCell Remarks = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
            Remarks.HorizontalAlignment = 1;
            Remarks.Colspan = 0;
            tablenewc.AddCell(Remarks);

            PdfPCell cell1 = new PdfPCell(new Phrase("1 ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell1.HorizontalAlignment = 1;
            cell1.Colspan = 0;
            tablenewc.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell2.HorizontalAlignment = 1;
            cell2.Colspan = 0;
            tablenewc.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase("3 ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell3.HorizontalAlignment = 1;
            cell3.Colspan = 0;
            tablenewc.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase("4", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell4.HorizontalAlignment = 1;
            cell4.Colspan = 0;
            tablenewc.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase("5 ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell5.HorizontalAlignment = 1;
            cell5.Colspan = 0;
            tablenewc.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase("6", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell6.HorizontalAlignment = 1;
            cell6.Colspan = 0;
            tablenewc.AddCell(cell6);

            PdfPCell cell7 = new PdfPCell(new Phrase("7", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell7.HorizontalAlignment = 1;
            cell7.Colspan = 0;
            tablenewc.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase("8", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell8.HorizontalAlignment = 1;
            cell8.Colspan = 0;
            tablenewc.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase("9", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cell9.HorizontalAlignment = 1;
            cell9.Colspan = 0;
            tablenewc.AddCell(cell9);


            PdfPCell cellsno1 = new PdfPCell(new Phrase(j.ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellsno1.HorizontalAlignment = 1;
            cellsno1.Colspan = 0;
            tablenewc.AddCell(cellsno1);


            PdfPCell cellacc2 = new PdfPCell(new Phrase(pfNo, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellacc2.HorizontalAlignment = 1;
            cellacc2.Colspan = 0;
            tablenewc.AddCell(cellacc2);

            PdfPCell cellname3 = new PdfPCell(new Phrase(name, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellname3.HorizontalAlignment = 1;
            cellname3.Colspan = 0;
            tablenewc.AddCell(cellname3);

            PdfPCell cellfather4 = new PdfPCell(new Phrase(fathername, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellfather4.HorizontalAlignment = 1;
            cellfather4.Colspan = 0;
            tablenewc.AddCell(cellfather4);

            PdfPCell cellbirh5 = new PdfPCell(new Phrase(date, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellbirh5.HorizontalAlignment = 1;
            cellbirh5.Colspan = 0;
            tablenewc.AddCell(cellbirh5);

            PdfPCell cellsex6 = new PdfPCell(new Phrase(Gender, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellsex6.HorizontalAlignment = 1;
            cellsex6.Colspan = 0;
            tablenewc.AddCell(cellsex6);

            PdfPCell celldatejoin7 = new PdfPCell(new Phrase(date, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            celldatejoin7.HorizontalAlignment = 1;
            celldatejoin7.Colspan = 0;
            tablenewc.AddCell(celldatejoin7);

            PdfPCell celltotal8 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            celltotal8.HorizontalAlignment = 1;
            celltotal8.Colspan = 0;
            tablenewc.AddCell(celltotal8);

            PdfPCell cellremarks9 = new PdfPCell(new Phrase(remarks, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            cellremarks9.HorizontalAlignment = 1;
            cellremarks9.Colspan = 0;
            cellremarks9.FixedHeight = 25f;
            tablenewc.AddCell(cellremarks9);

            tablenewc.AddCell(cellspace);
            tablenewc.AddCell(cellspace);
            tablenewc.AddCell(cellspace);
            tablenewc.AddCell(cellspace);
            tablenewc.AddCell(cellspace);
            tablenewc.AddCell(cellspace);

            PdfPCell empty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK)));
            empty.HorizontalAlignment = 0;
            empty.Colspan = 11;
            empty.Border = 0;
            tablenewc.AddCell(empty);



            PdfPCell footerdate = new PdfPCell(new Phrase("Date :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            footerdate.HorizontalAlignment = 0;
            footerdate.Colspan = 2;
            footerdate.BorderWidthTop = .5f;
            footerdate.BorderWidthRight = 0;
            footerdate.BorderWidthLeft = 0;
            footerdate.BorderWidthBottom = 0;
            tablenewc.AddCell(footerdate);

            PdfPCell stamp = new PdfPCell(new Phrase("Stamp of the Factory / Est.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            stamp.HorizontalAlignment = 0;
            stamp.Colspan = 2;
            stamp.BorderWidthTop = .5f;
            stamp.BorderWidthRight = 0;
            stamp.BorderWidthLeft = 0;
            stamp.BorderWidthBottom = 0;
            tablenewc.AddCell(stamp);

            PdfPCell sig = new PdfPCell(new Phrase("Signature of the employer or other authorised officer of the Factory / Establishment", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
            sig.HorizontalAlignment = 2;
            sig.Colspan = 7;
            sig.BorderWidthTop = .5f;
            sig.BorderWidthRight = 0;
            sig.BorderWidthLeft = 0;
            sig.BorderWidthBottom = 0;
            tablenewc.AddCell(sig);



            document.Add(tablenewc);

            #endregion Basic Information of the Employee

            document.NewPage();

            PdfPTable tblFingerprints = new PdfPTable(6);
            tblFingerprints.TotalWidth = 500f;
            tblFingerprints.LockedWidth = true;
            float[] widthfinger = new float[] { 2f, 1.5f, 2f, 2f, 1.5f, 2f };
            tblFingerprints.SetWidths(widthfinger);


            string filename = "FormXXII.pdf";

            document.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();

            //}



            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Duration expired');", true);
            //    return;
            //}
        }

        //protected void btnform12_Click(object sender, EventArgs e)
        //{


        //    int Fontsize = 13;
        //    string fontsyle = "verdana";

        //    #region Variable Declaration

        //    #endregion


        //    if (ddlEmployee.SelectedIndex == 0)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
        //        return;
        //    }

        //    #region  Begin  New Code

        //    #region Begin Variable Declaration

        //    string SPName = "";
        //    Hashtable HTEmpBiodata = new Hashtable();
        //    string Empid = "";
        //    DataTable dtEmpdetails = null;

        //    #endregion end Variable Declaration

        //    #region Begin Assign Values to The Variables
        //    SPName = "EmployeeFormsPDF";
        //    Empid = ddlEmployee.SelectedValue;

        //    var DtofJoining = string.Empty;
        //    if (txtEmpDtofJoining.Text.Trim().Length != 0)
        //    {
        //        DtofJoining = Timings.Instance.CheckDateFormat(txtEmpDtofJoining.Text);

        //    }
        //    else
        //    {
        //        DtofJoining = "01/01/1900";
        //    }


        //    if (TxtMonth.Text.Trim().Length > 0)
        //    {
        //        date = DateTime.Parse(TxtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
        //    }


        //    string month = DateTime.Parse(date).Month.ToString();
        //    string Year = DateTime.Parse(date).Year.ToString();

        //    #endregion End Assign values To the Variables
        //    int forms = 0;
        //    forms = ddlForms.SelectedIndex;
        //    #region Begin Pass values to the Hash table
        //    HTEmpBiodata.Add("@empid", Empid);
        //    HTEmpBiodata.Add("@month", month + Year.Substring(2, 2));
        //    HTEmpBiodata.Add("@DtofJoining", DtofJoining);
        //    #endregion  end Pass values to the Hash table

        //    #region Begin  Call Stored Procedure
        //    dtEmpdetails = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTEmpBiodata);
        //    #endregion  End  Call Stored Procedure

        //    #endregion End New Code As on [31-05-2014]

        //    MemoryStream ms = new MemoryStream();

        //    Document document = new Document(PageSize.LEGAL.Rotate());

        //    // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
        //    var writer = PdfWriter.GetInstance(document, ms);
        //    document.Open();
        //    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    string imagepath1 = Server.MapPath("images");
        //    #region

        //    PdfPTable tablenewc = new PdfPTable(9);
        //    tablenewc.TotalWidth = 900f;
        //    tablenewc.LockedWidth = true;
        //    float[] width = new float[] { 4f, 2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
        //    tablenewc.SetWidths(width);

        //    PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
        //    cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //    cellspace.Colspan = 9;
        //    cellspace.Border = 0;
        //    cellspace.PaddingTop = 0;

        //    PdfPCell cellonlyun = new PdfPCell(new Phrase("(ONLY FOR UN-EXEMPTED ESTABLISHMENTS ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 2, Font.BOLD, BaseColor.BLACK)));
        //    cellonlyun.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //    cellonlyun.Colspan = 9;
        //    cellonlyun.Border = 0;
        //    tablenewc.AddCell(cellonlyun);

        //    PdfPCell provident = new PdfPCell(new Phrase("Employees' Provident Fund Organisation ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK)));
        //    provident.HorizontalAlignment = 1;
        //    provident.Colspan = 9;
        //    provident.Border = 0;
        //    tablenewc.AddCell(provident);

        //    PdfPCell cellemp = new PdfPCell(new Phrase("THE EMPLOYEES' PROVIDENT FUND  AND  MISC.PROVISIONS ACT,1952- EMPLOYEES' PENSION SCHEME [PARA 20 (4) ]  ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
        //    cellemp.HorizontalAlignment = 1;
        //    cellemp.Colspan = 9;
        //    cellemp.Border = 0;
        //    tablenewc.AddCell(cellemp);

        //    PdfPCell cellHead = new PdfPCell(new Phrase("Form 12-A ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
        //    cellHead.HorizontalAlignment = 1;
        //    cellHead.Colspan = 9;
        //    cellHead.Border = 0;
        //    tablenewc.AddCell(cellHead);

        //    PdfPCell cellmonth = new PdfPCell(new Phrase("Name and address of  the Factory / Establishment :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    cellmonth.HorizontalAlignment = 0;
        //    cellmonth.Colspan = 3;
        //    cellmonth.Border = 0;
        //    tablenewc.AddCell(cellmonth);

        //    PdfPCell nameaddress = new PdfPCell(new Phrase("Currency period from", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    nameaddress.HorizontalAlignment = 0;
        //    nameaddress.Colspan = 4;
        //    nameaddress.Border = 0;
        //    tablenewc.AddCell(nameaddress);

        //    PdfPCell cellcode = new PdfPCell(new Phrase(" ( To be filled by the EPFO)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    cellcode.HorizontalAlignment = 2;
        //    cellcode.Colspan = 2;
        //    cellcode.Border = 0;
        //    tablenewc.AddCell(cellcode);

        //    PdfPCell epty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    epty.HorizontalAlignment = 0;
        //    epty.Colspan = 3;
        //    epty.Border = 0;
        //    tablenewc.AddCell(epty);

        //    PdfPCell Statement = new PdfPCell(new Phrase("Statement of contribution for the month of", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    Statement.HorizontalAlignment = 0;
        //    Statement.Colspan = 4;
        //    Statement.Border = 0;
        //    tablenewc.AddCell(Statement);

        //    PdfPCell Group = new PdfPCell(new Phrase("Group code :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    Group.HorizontalAlignment = 2;
        //    Group.Colspan = 2;
        //    Group.Border = 0;
        //    tablenewc.AddCell(Group);

        //    PdfPCell Establishment = new PdfPCell(new Phrase("Establishment Status :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    Establishment.HorizontalAlignment = 2;
        //    Establishment.Colspan = 9;
        //    Establishment.Border = 0;
        //    tablenewc.AddCell(Establishment);

        //    PdfPCell code1 = new PdfPCell(new Phrase("Group code :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    code1.HorizontalAlignment = 0;
        //    code1.Colspan = 4;
        //    code1.Border = 0;
        //    tablenewc.AddCell(code1);

        //    PdfPCell Statutory = new PdfPCell(new Phrase("Statutory rate of contribution 12.00 % ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    Statutory.HorizontalAlignment = 0;
        //    Statutory.Colspan = 5;
        //    Statutory.Border = 0;
        //    tablenewc.AddCell(Statutory);



        //    #region
        //    PdfPCell empty6 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    empty6.HorizontalAlignment = 1;
        //    empty6.BorderWidthBottom = 0;
        //    empty6.BorderWidthLeft = .5f;
        //    empty6.BorderWidthRight = .5f;
        //    empty6.BorderWidthTop = .5f;
        //    tablenewc.AddCell(empty6);

        //    PdfPCell empty5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    empty5.HorizontalAlignment = 1;
        //    empty5.BorderWidthBottom = 0;
        //    empty5.BorderWidthLeft = .5f;
        //    empty5.BorderWidthRight = .5f;
        //    empty5.BorderWidthTop = .5f;
        //    tablenewc.AddCell(empty5);

        //    PdfPCell empty4 = new PdfPCell(new Phrase("Amount of Contribution \n (3) ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    empty4.HorizontalAlignment = 1;
        //    empty4.Colspan = 2;
        //    tablenewc.AddCell(empty4);

        //    PdfPCell empty3 = new PdfPCell(new Phrase("Amount of Contribution remitted \n (4)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    empty3.HorizontalAlignment = 1;
        //    empty3.Colspan = 2;
        //    tablenewc.AddCell(empty3);

        //    PdfPCell empty2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    empty2.HorizontalAlignment = 1;
        //    empty2.BorderWidthBottom = 0;
        //    empty2.BorderWidthLeft = .5f;
        //    empty2.BorderWidthRight = 0;
        //    empty2.BorderWidthTop = .5f;
        //    tablenewc.AddCell(empty2);

        //    PdfPCell empty1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    empty1.HorizontalAlignment = 1;
        //    empty1.BorderWidthBottom = 0;
        //    empty1.BorderWidthLeft = .5f;
        //    empty1.BorderWidthRight = .5f;
        //    empty1.BorderWidthTop = .5f;
        //    tablenewc.AddCell(empty1);

        //    PdfPCell empty = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    empty.HorizontalAlignment = 1;
        //    empty.BorderWidthBottom = 0;
        //    empty.BorderWidthLeft = .5f;
        //    empty.BorderWidthRight = .5f;
        //    empty.BorderWidthTop = .5f;
        //    tablenewc.AddCell(empty);



        //    #endregion

        //    PdfPCell celltable10 = new PdfPCell(new Phrase("Particulars \n (1)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    celltable10.HorizontalAlignment = 1;
        //    celltable10.Colspan = 0;
        //    celltable10.BorderWidthBottom = .5f;
        //    celltable10.BorderWidthLeft = .5f;
        //    celltable10.BorderWidthRight = .5f;
        //    celltable10.BorderWidthTop = 0;
        //    tablenewc.AddCell(celltable10);

        //    PdfPCell Account = new PdfPCell(new Phrase("Wages on  which contributions are payable \n (2)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    Account.HorizontalAlignment = 1;
        //    Account.Colspan = 0;
        //    Account.BorderWidthBottom = .5f;
        //    Account.BorderWidthLeft = .5f;
        //    Account.BorderWidthRight = .5f;
        //    Account.BorderWidthTop = 0;
        //    tablenewc.AddCell(Account);

        //    PdfPCell Name = new PdfPCell(new Phrase("Recovered from the Employees  ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    Name.HorizontalAlignment = 1;
        //    Name.Colspan = 0;
        //    tablenewc.AddCell(Name);

        //    PdfPCell Father = new PdfPCell(new Phrase("Payable from the Employeer", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    Father.HorizontalAlignment = 1;
        //    Father.Colspan = 0;
        //    tablenewc.AddCell(Father);

        //    PdfPCell Date = new PdfPCell(new Phrase("Employee'Share", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    Date.HorizontalAlignment = 1;
        //    Date.Colspan = 0;
        //    tablenewc.AddCell(Date);

        //    PdfPCell reson = new PdfPCell(new Phrase("Employer's Share", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    reson.HorizontalAlignment = 1;
        //    reson.Colspan = 0;
        //    tablenewc.AddCell(reson);

        //    PdfPCell Amount = new PdfPCell(new Phrase("Amount ofadministrative charges due \n (5)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    Amount.HorizontalAlignment = 1;
        //    Amount.Colspan = 0;
        //    Amount.BorderWidthBottom = .5f;
        //    Amount.BorderWidthLeft = .5f;
        //    Amount.BorderWidthRight = .5f;
        //    Amount.BorderWidthTop = 0;
        //    tablenewc.AddCell(Amount);

        //    PdfPCell Amount1 = new PdfPCell(new Phrase("Amount ofadministrative charges due remitted \n (6)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    Amount1.HorizontalAlignment = 1;
        //    Amount1.Colspan = 0;
        //    Amount1.BorderWidthBottom = .5f;
        //    Amount1.BorderWidthLeft = .5f;
        //    Amount1.BorderWidthRight = .5f;
        //    Amount1.BorderWidthTop = 0;
        //    tablenewc.AddCell(Amount1);

        //    PdfPCell Date1 = new PdfPCell(new Phrase("Date of remittance (enclose triplicate copy of challan/s) \n (7)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    Date1.HorizontalAlignment = 1;
        //    Date1.Colspan = 0;
        //    Date1.BorderWidthBottom = .5f;
        //    Date1.BorderWidthLeft = .5f;
        //    Date1.BorderWidthRight = .5f;
        //    Date1.BorderWidthTop = 0;
        //    tablenewc.AddCell(Date1);

        //    PdfPCell cellepf1 = new PdfPCell(new Phrase("E.P.F. A/c No.01", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf1.HorizontalAlignment = 0;
        //    cellepf1.Colspan = 0;
        //    tablenewc.AddCell(cellepf1);

        //    PdfPCell cellepf2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf2.HorizontalAlignment = 1;
        //    cellepf2.Colspan = 0;
        //    tablenewc.AddCell(cellepf2);

        //    PdfPCell cellepf3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf3.HorizontalAlignment = 1;
        //    cellepf3.Colspan = 0;
        //    tablenewc.AddCell(cellepf3);

        //    PdfPCell cellepf4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf4.HorizontalAlignment = 1;
        //    cellepf4.Colspan = 0;
        //    tablenewc.AddCell(cellepf4);

        //    PdfPCell cellepf5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf5.HorizontalAlignment = 1;
        //    cellepf5.Colspan = 0;
        //    tablenewc.AddCell(cellepf5);

        //    PdfPCell cellepf6 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf6.HorizontalAlignment = 1;
        //    cellepf6.Colspan = 0;
        //    tablenewc.AddCell(cellepf6);

        //    PdfPCell cellepf7 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf7.HorizontalAlignment = 1;
        //    cellepf7.Colspan = 0;
        //    tablenewc.AddCell(cellepf7);

        //    PdfPCell cellepf8 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf8.HorizontalAlignment = 1;
        //    cellepf8.Colspan = 0;
        //    tablenewc.AddCell(cellepf8);

        //    PdfPCell cellepf9 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellepf9.HorizontalAlignment = 1;
        //    cellepf9.Colspan = 0;
        //    tablenewc.AddCell(cellepf9);

        //    PdfPCell cell2 = new PdfPCell(new Phrase("Pension Fund A/c.No.10", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    cell2.HorizontalAlignment = 0;
        //    cell2.Colspan = 0;
        //    tablenewc.AddCell(cell2);

        //    PdfPCell pensionfund1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    pensionfund1.HorizontalAlignment = 1;
        //    pensionfund1.Colspan = 0;
        //    tablenewc.AddCell(pensionfund1);

        //    PdfPCell pensionfund2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    pensionfund2.HorizontalAlignment = 1;
        //    pensionfund2.Colspan = 0;
        //    tablenewc.AddCell(pensionfund2);

        //    PdfPCell pensionfund3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    pensionfund3.HorizontalAlignment = 1;
        //    pensionfund3.Colspan = 0;
        //    tablenewc.AddCell(pensionfund3);

        //    PdfPCell pensionfund4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    pensionfund4.HorizontalAlignment = 1;
        //    pensionfund4.Colspan = 0;
        //    tablenewc.AddCell(pensionfund4);

        //    PdfPCell pensionfund5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    pensionfund5.HorizontalAlignment = 1;
        //    pensionfund5.Colspan = 0;
        //    tablenewc.AddCell(pensionfund5);

        //    PdfPCell pensionfund6 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    pensionfund6.HorizontalAlignment = 1;
        //    pensionfund6.Colspan = 0;
        //    tablenewc.AddCell(pensionfund6);

        //    PdfPCell pensionfund7 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    pensionfund7.HorizontalAlignment = 1;
        //    pensionfund7.Colspan = 0;
        //    tablenewc.AddCell(pensionfund7);

        //    PdfPCell pensionfund8 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    pensionfund8.HorizontalAlignment = 1;
        //    pensionfund8.Colspan = 0;
        //    tablenewc.AddCell(pensionfund8);

        //    PdfPCell edlac1 = new PdfPCell(new Phrase("E.D.L.I.A/c.No.21", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize - 1, Font.NORMAL, BaseColor.BLACK)));
        //    edlac1.HorizontalAlignment = 0;
        //    edlac1.Colspan = 0;
        //    tablenewc.AddCell(edlac1);

        //    PdfPCell edlac2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    edlac2.HorizontalAlignment = 1;
        //    edlac2.Colspan = 0;
        //    tablenewc.AddCell(edlac2);

        //    PdfPCell edlac3 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    edlac3.HorizontalAlignment = 1;
        //    edlac3.Colspan = 0;
        //    tablenewc.AddCell(edlac3);

        //    PdfPCell edlac4 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    edlac4.HorizontalAlignment = 1;
        //    edlac4.Colspan = 0;
        //    tablenewc.AddCell(edlac4);

        //    PdfPCell edlac5 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    edlac5.HorizontalAlignment = 1;
        //    edlac5.Colspan = 0;
        //    tablenewc.AddCell(edlac5);

        //    PdfPCell edlac6 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    edlac6.HorizontalAlignment = 1;
        //    edlac6.Colspan = 0;
        //    tablenewc.AddCell(edlac6);

        //    PdfPCell edlac7 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    edlac7.HorizontalAlignment = 1;
        //    edlac7.Colspan = 0;
        //    tablenewc.AddCell(edlac7);

        //    PdfPCell edlac8 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    edlac8.HorizontalAlignment = 1;
        //    edlac8.Colspan = 0;
        //    tablenewc.AddCell(edlac8);

        //    PdfPCell edlac9 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    edlac9.HorizontalAlignment = 1;
        //    edlac9.Colspan = 0;
        //    tablenewc.AddCell(edlac9);
        //    tablenewc.AddCell(cellspace);

        //    PdfPCell cellsno1 = new PdfPCell(new Phrase("Total No. of Employees :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellsno1.HorizontalAlignment = 0;
        //    cellsno1.Colspan = 4;
        //    cellsno1.Border = 0;
        //    tablenewc.AddCell(cellsno1);

        //    PdfPCell cellacc2 = new PdfPCell(new Phrase("Name and address of the bank in which the amount is remitted :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellacc2.HorizontalAlignment = 0;
        //    cellacc2.Colspan = 5;
        //    cellacc2.Border = 0;
        //    tablenewc.AddCell(cellacc2);

        //    PdfPCell cellname3 = new PdfPCell(new Phrase("(a) Contract :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellname3.HorizontalAlignment = 0;
        //    cellname3.Colspan = 9;
        //    cellname3.Border = 0;
        //    tablenewc.AddCell(cellname3);

        //    PdfPCell cellfather4 = new PdfPCell(new Phrase("(b) Rest :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellfather4.HorizontalAlignment = 0;
        //    cellfather4.Colspan = 9;
        //    cellfather4.Border = 0;
        //    tablenewc.AddCell(cellfather4);

        //    PdfPCell cellbirh5 = new PdfPCell(new Phrase("(c) Total :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    cellbirh5.HorizontalAlignment = 0;
        //    cellbirh5.Colspan = 9;
        //    cellbirh5.Border = 0;
        //    tablenewc.AddCell(cellbirh5);
        //    tablenewc.AddCell(cellspace);

        //    document.Add(tablenewc);


        //    PdfPTable tempTable2 = new PdfPTable(8);
        //    tempTable2.TotalWidth = 900f;
        //    tempTable2.LockedWidth = true;
        //    float[] tempWidth2 = new float[] { 7f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };
        //    tempTable2.SetWidths(tempWidth2);

        //    PdfPTable childtable1 = new PdfPTable(4);
        //    childtable1.TotalWidth = 560f;
        //    childtable1.LockedWidth = true;
        //    float[] childtblewidth = new float[] { 8f, 1.5f, 2f, 2f };
        //    childtable1.SetWidths(childtblewidth);

        //    #region


        //    PdfPCell Details = new PdfPCell(new Phrase("Details of Subscriberss", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    Details.HorizontalAlignment = 0;
        //    Details.Colspan = 1;
        //    Details.BorderWidthBottom = .5f;
        //    Details.BorderWidthLeft = 0;
        //    Details.BorderWidthRight = .5f;
        //    Details.BorderWidthTop = .5f;
        //    childtable1.AddCell(Details);

        //    PdfPCell EPF = new PdfPCell(new Phrase("EPF", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    EPF.HorizontalAlignment = 0;
        //    EPF.Colspan = 1;
        //    childtable1.AddCell(EPF);

        //    PdfPCell EPS = new PdfPCell(new Phrase("EPS", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    EPS.HorizontalAlignment = 0;
        //    EPS.Colspan = 1;
        //    childtable1.AddCell(EPS);

        //    PdfPCell EDLI = new PdfPCell(new Phrase("EDLI", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    EDLI.HorizontalAlignment = 0;
        //    EDLI.Colspan = 1;
        //    childtable1.AddCell(EDLI);



        //    PdfPCell last = new PdfPCell(new Phrase(" No.of subscribers as per last month(vide Form 12 A)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    last.HorizontalAlignment = 0;
        //    last.Colspan = 1;
        //    last.BorderWidthBottom = 0;
        //    last.BorderWidthLeft = 0;
        //    last.BorderWidthRight = .5f;
        //    last.BorderWidthTop = .5f;
        //    childtable1.AddCell(last);

        //    PdfPCell noofEPF = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    noofEPF.HorizontalAlignment = 0;
        //    noofEPF.Colspan = 1;
        //    noofEPF.BorderWidthBottom = 0;
        //    noofEPF.BorderWidthLeft = .5f;
        //    noofEPF.BorderWidthRight = .5f;
        //    noofEPF.BorderWidthTop = .5f;
        //    childtable1.AddCell(noofEPF);

        //    PdfPCell noofEPS = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    noofEPS.HorizontalAlignment = 0;
        //    noofEPS.Colspan = 1;
        //    noofEPS.BorderWidthBottom = 0;
        //    noofEPS.BorderWidthLeft = 0;
        //    noofEPS.BorderWidthRight = .5f;
        //    noofEPS.BorderWidthTop = .5f;
        //    childtable1.AddCell(noofEPS);

        //    PdfPCell noofEDLI = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    noofEDLI.HorizontalAlignment = 0;
        //    noofEDLI.Colspan = 1;
        //    noofEDLI.BorderWidthBottom = 0;
        //    noofEDLI.BorderWidthLeft = 0;
        //    noofEDLI.BorderWidthRight = .5f;
        //    noofEDLI.BorderWidthTop = .5f;
        //    childtable1.AddCell(noofEDLI);


        //    PdfPCell newsub = new PdfPCell(new Phrase(" No. of New subscribers (Vide Form 5)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    newsub.HorizontalAlignment = 0;
        //    newsub.Colspan = 1;
        //    newsub.BorderWidthBottom = 0;
        //    newsub.BorderWidthLeft = 0;
        //    newsub.BorderWidthRight = .5f;
        //    newsub.BorderWidthTop = 0;
        //    childtable1.AddCell(newsub);

        //    PdfPCell newsubepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    newsubepf.HorizontalAlignment = 0;
        //    newsubepf.Colspan = 1;
        //    newsubepf.BorderWidthBottom = 0;
        //    newsubepf.BorderWidthLeft = .5f;
        //    newsubepf.BorderWidthRight = .5f;
        //    newsubepf.BorderWidthTop = 0;
        //    childtable1.AddCell(newsubepf);

        //    PdfPCell newsubEPS = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    newsubEPS.HorizontalAlignment = 0;
        //    newsubEPS.Colspan = 1;
        //    newsubEPS.BorderWidthBottom = 0;
        //    newsubEPS.BorderWidthLeft = .5f;
        //    newsubEPS.BorderWidthRight = .5f;
        //    newsubEPS.BorderWidthTop = 0;
        //    childtable1.AddCell(newsubEPS);

        //    PdfPCell newsubEDLI = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    newsubEDLI.HorizontalAlignment = 0;
        //    newsubEDLI.Colspan = 1;
        //    newsubEDLI.BorderWidthBottom = 0;
        //    newsubEDLI.BorderWidthLeft = .5f;
        //    newsubEDLI.BorderWidthRight = .5f;
        //    newsubEDLI.BorderWidthTop = 0;
        //    childtable1.AddCell(newsubEDLI);


        //    PdfPCell nofsubsub = new PdfPCell(new Phrase(" No.of subscribers left service (vide Form 10) ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    nofsubsub.HorizontalAlignment = 0;
        //    nofsubsub.Colspan = 1;
        //    nofsubsub.BorderWidthBottom = 0;
        //    nofsubsub.BorderWidthLeft = 0;
        //    nofsubsub.BorderWidthRight = .5f;
        //    nofsubsub.BorderWidthTop = 0;
        //    childtable1.AddCell(nofsubsub);

        //    PdfPCell nosubepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    nosubepf.HorizontalAlignment = 0;
        //    nosubepf.Colspan = 1;
        //    nosubepf.BorderWidthBottom = 0;
        //    nosubepf.BorderWidthLeft = .5f;
        //    nosubepf.BorderWidthRight = .5f;
        //    nosubepf.BorderWidthTop = 0;
        //    childtable1.AddCell(nosubepf);

        //    PdfPCell nosubEPS = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    nosubEPS.HorizontalAlignment = 0;
        //    nosubEPS.Colspan = 1;
        //    nosubEPS.BorderWidthBottom = 0;
        //    nosubEPS.BorderWidthLeft = .5f;
        //    nosubEPS.BorderWidthRight = .5f;
        //    nosubEPS.BorderWidthTop = 0;
        //    childtable1.AddCell(nosubEPS);

        //    PdfPCell nosubEDLI = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    nosubEDLI.HorizontalAlignment = 0;
        //    nosubEDLI.Colspan = 1;
        //    nosubEDLI.BorderWidthBottom = 0;
        //    nosubEDLI.BorderWidthLeft = .5f;
        //    nosubEDLI.BorderWidthRight = .5f;
        //    nosubEDLI.BorderWidthTop = 0;
        //    childtable1.AddCell(nosubEDLI);



        //    PdfPCell netsubsub = new PdfPCell(new Phrase(" Net. No. of subscribers ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    netsubsub.HorizontalAlignment = 0;
        //    netsubsub.Colspan = 1;
        //    netsubsub.BorderWidthBottom = .5f;
        //    netsubsub.BorderWidthLeft = 0;
        //    netsubsub.BorderWidthRight = .5f;
        //    netsubsub.BorderWidthTop = 0;
        //    childtable1.AddCell(netsubsub);

        //    PdfPCell netsubepf = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    netsubepf.HorizontalAlignment = 0;
        //    netsubepf.Colspan = 1;
        //    netsubepf.BorderWidthBottom = .5f;
        //    netsubepf.BorderWidthLeft = .5f;
        //    netsubepf.BorderWidthRight = .5f;
        //    netsubepf.BorderWidthTop = 0;
        //    childtable1.AddCell(netsubepf);

        //    PdfPCell netsubEPS = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    netsubEPS.HorizontalAlignment = 0;
        //    netsubEPS.Colspan = 1;
        //    netsubEPS.BorderWidthBottom = .5f;
        //    netsubEPS.BorderWidthLeft = .5f;
        //    netsubEPS.BorderWidthRight = .5f;
        //    netsubEPS.BorderWidthTop = 0;
        //    childtable1.AddCell(netsubEPS);

        //    PdfPCell netsubEDLI = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    netsubEDLI.HorizontalAlignment = 0;
        //    netsubEDLI.Colspan = 1;
        //    netsubEDLI.BorderWidthBottom = .5f;
        //    netsubEDLI.BorderWidthLeft = .5f;
        //    netsubEDLI.BorderWidthRight = .5f;
        //    netsubEDLI.BorderWidthTop = 0;
        //    childtable1.AddCell(netsubEDLI);
        //    #endregion

        //    PdfPCell endchildTable1 = new PdfPCell(childtable1);
        //    endchildTable1.HorizontalAlignment = 0;
        //    endchildTable1.Colspan = 4;
        //    tempTable2.AddCell(endchildTable1);

        //    PdfPTable childtable2 = new PdfPTable(4);
        //    childtable2.TotalWidth = 350f;
        //    childtable2.LockedWidth = true;
        //    float[] childtblewidth1 = new float[] { 2f, 2f, 2f, 2f };
        //    childtable2.SetWidths(childtblewidth1);


        //    PdfPCell sign = new PdfPCell(new Phrase(" Signature of the Employer(with office seal)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
        //    sign.HorizontalAlignment = 2;
        //    sign.Colspan = 4;
        //    sign.Border = 0;
        //    sign.PaddingTop = 70;
        //    childtable2.AddCell(sign);


        //    PdfPCell endchildTable2 = new PdfPCell(childtable2);
        //    endchildTable2.Border = 0;
        //    endchildTable2.Colspan = 4;
        //    endchildTable2.HorizontalAlignment = 0;
        //    tempTable2.AddCell(endchildTable2);

        //    document.Add(tempTable2);

        //    #endregion Basic Information of the Employee

        //    document.NewPage();

        //    PdfPTable tblFingerprints = new PdfPTable(6);
        //    tblFingerprints.TotalWidth = 500f;
        //    tblFingerprints.LockedWidth = true;
        //    float[] widthfinger = new float[] { 2f, 1.5f, 2f, 2f, 1.5f, 2f };
        //    tblFingerprints.SetWidths(widthfinger);


        //    string filename = "FormXXII.pdf";

        //    document.Close();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        //    Response.Buffer = true;
        //    Response.Clear();
        //    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        //    Response.OutputStream.Flush();
        //    Response.End();

        //    //}



        //    //else
        //    //{
        //    //    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Duration expired');", true);
        //    //    return;
        //    //}
        //}

        protected void btngratuity_Click(object sender, EventArgs e)
        {

            int Fontsize = 12;
            string fontstyle = "verdana";

            #region Variable Declaration

            string Idno = "";
            string date = "";
            string name = "";
            string fathername = "";
            string designation = "";
            string Gender = "";
            string MaritalStatus = "";
            string empReligion = "";
            string Companyname = "";
            string CompanyAddress = "";

            string peTaluka = "";
            string pestreet = "";
            string peTown = "";
            string pecity = "";
            string pelmark = "";

            string peDistrict = "";
            string pePincode = "";
            string peState = "";
            string pephone = "";

            #endregion


            if (txtemplyid.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
                return;
            }

            #region  Begin  New Code

            #region Begin Variable Declaration

            string SPName = "";
            Hashtable HTEmpBiodata = new Hashtable();
            string Empid = "";
            DataTable dtEmpdetails = null;

            #endregion end Variable Declaration


            #region Begin Assign Values to The Variables
            SPName = "EmployeeFormsPDF";
            Empid = txtemplyid.Text;
            #endregion End Assign values To the Variables

            #region Begin Pass values to the Hash table
            HTEmpBiodata.Add("@empid", Empid);
            #endregion  end Pass values to the Hash table

            #region Begin  Call Stored Procedure
            dtEmpdetails =config.ExecuteAdaptorAsyncWithParams(SPName, HTEmpBiodata).Result;
            #endregion  End  Call Stored Procedure

            #endregion End New Code As on [31-05-2014]


            if (dtEmpdetails.Rows.Count > 0)
            {

                #region Assining data to Variables

                Idno = dtEmpdetails.Rows[0]["EmpId"].ToString();
                designation = dtEmpdetails.Rows[0]["designation"].ToString();
                name = dtEmpdetails.Rows[0]["EmployeeName"].ToString();
                fathername = dtEmpdetails.Rows[0]["EmpFatherName"].ToString();
                date = dtEmpdetails.Rows[0]["DtOfJoining"].ToString();
                Gender = dtEmpdetails.Rows[0]["gender"].ToString();
                MaritalStatus = dtEmpdetails.Rows[0]["MaritalStatus"].ToString();
                empReligion = dtEmpdetails.Rows[0]["Religion"].ToString();
                Companyname = dtEmpdetails.Rows[0]["CompanyName"].ToString();
                CompanyAddress = dtEmpdetails.Rows[0]["CompanyAddress"].ToString();




                pelmark = dtEmpdetails.Rows[0]["pelmark"].ToString();
                peTaluka = dtEmpdetails.Rows[0]["peTaluka"].ToString();
                peTown = dtEmpdetails.Rows[0]["peTown"].ToString();
                peState = dtEmpdetails.Rows[0]["peState"].ToString();
                pecity = dtEmpdetails.Rows[0]["pecity"].ToString();
                pePincode = dtEmpdetails.Rows[0]["pePincode"].ToString();


                #endregion
                MemoryStream ms = new MemoryStream();


                Document document = new Document(PageSize.A4);
                // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");
                #region

                PdfPTable tablesnewone = new PdfPTable(4);
                tablesnewone.TotalWidth = 500f;
                tablesnewone.LockedWidth = true;
                float[] width = new float[] { 2f, 2f, 2f, 2f };
                tablesnewone.SetWidths(width);

                PdfPCell cellHead1 = new PdfPCell(new Phrase("The Karnataka payment of Gratuity Rules, 1972", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cellHead1.HorizontalAlignment = 1;
                cellHead1.Colspan = 4;
                cellHead1.Border = 0;
                tablesnewone.AddCell(cellHead1);

                PdfPCell cellHead = new PdfPCell(new Phrase("FORM-'F'  ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cellHead.HorizontalAlignment = 1;
                cellHead.Colspan = 4;
                cellHead.Border = 0;
                cellHead.PaddingBottom = 10;
                tablesnewone.AddCell(cellHead);



                PdfPCell cellRule32 = new PdfPCell(new Phrase("[See sub-Rule(1) of Rule 6]", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                cellRule32.HorizontalAlignment = 1;
                cellRule32.Colspan = 4;
                cellRule32.Border = 0;
                tablesnewone.AddCell(cellRule32);


                PdfPCell Nomination = new PdfPCell(new Phrase("Nomination", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize + 1, Font.BOLD, BaseColor.BLACK)));
                Nomination.HorizontalAlignment = 1;
                Nomination.Colspan = 4;
                Nomination.Border = 0;
                tablesnewone.AddCell(Nomination);


                PdfPCell to = new PdfPCell(new Phrase("To", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                to.HorizontalAlignment = 0;
                to.Colspan = 4;
                to.Border = 0;
                tablesnewone.AddCell(to);

                PdfPCell cmpnyname = new PdfPCell(new Phrase(Companyname, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cmpnyname.HorizontalAlignment = 0;
                cmpnyname.Colspan = 4;
                cmpnyname.Border = 0;
                tablesnewone.AddCell(cmpnyname);

                PdfPCell address1 = new PdfPCell(new Phrase(CompanyAddress, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                address1.HorizontalAlignment = 0;
                address1.Colspan = 2;
                address1.Border = 0;
                tablesnewone.AddCell(address1);

                PdfPCell addline1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                addline1.HorizontalAlignment = 0;
                addline1.Colspan = 2;
                addline1.Border = 0;
                tablesnewone.AddCell(addline1);

                PdfPCell addpincode = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                addpincode.HorizontalAlignment = 0;
                addpincode.Colspan = 4;
                addpincode.Border = 0;
                tablesnewone.AddCell(addpincode);

                PdfPCell nameshri = new PdfPCell(new Phrase("1. Shri / Shrimati / Kumari " + name + " (name in full here) whose particulars are given the statement  below, hereby nominate the person(s) mentioned below to recive the gratuity payable after my death as also the gratuity standing to my credit in the event of my death before that amount has become payable,or having become payable has not been paid and direct that the said amount of gratuity shall be paidin proporation indicated against the name(s) of the nominee(s).", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                nameshri.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                nameshri.Colspan = 4;
                nameshri.Border = 0;
                nameshri.SetLeading(0f, 1.3f);
                tablesnewone.AddCell(nameshri);

                PdfPCell space = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                space.HorizontalAlignment = 0;
                space.Colspan = 4;
                space.Border = 0;
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);


                PdfPCell cell2 = new PdfPCell(new Phrase("2. I here by certify that the person(s) mentioned is a /are member(s) of my family within the meaning of clause (h) of section 2 of the payment of Gratuity Act,1972 ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                cell2.Colspan = 4;
                cell2.Border = 0;
                cell2.SetLeading(0f, 1.3f);
                tablesnewone.AddCell(cell2);

                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);

                PdfPCell cell3 = new PdfPCell(new Phrase("3. I hereby declare that i have no family within the meaning of clause (h) of section2 of the said Act.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                cell3.Colspan = 4;
                cell3.Border = 0;
                tablesnewone.AddCell(cell3);

                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);

                PdfPCell cell4 = new PdfPCell(new Phrase("4. (a) My father/mother/parents is /are not dependent on me", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                cell4.Colspan = 4;
                cell4.Border = 0;
                tablesnewone.AddCell(cell4);

                PdfPCell cell4b = new PdfPCell(new Phrase("    (b) My husband's father/mother/parents is/are not dependent on my husband.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell4b.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                cell4b.Colspan = 4;
                cell4b.Border = 0;
                tablesnewone.AddCell(cell4b);

                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);

                PdfPCell cell5 = new PdfPCell(new Phrase("5. I have excluded my husband from my family by a notice dated the................................. to the Controlling Authority in terms of the proviso to clause (h) of section 2 of the said Act.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell5.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                cell5.Colspan = 4;
                cell5.Border = 0;
                cell5.SetLeading(0f, 1.3f);
                tablesnewone.AddCell(cell5);
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);

                PdfPCell cellmt = new PdfPCell(new Phrase("6. Nomination made  herein invalidates my previous nomination.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellmt.HorizontalAlignment = 0;
                cellmt.Colspan = 4;
                cellmt.Border = 0;
                tablesnewone.AddCell(cellmt);
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);
                tablesnewone.AddCell(space);



                PdfPCell nomindees = new PdfPCell(new Phrase("NOMINEE(S)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                nomindees.HorizontalAlignment = 1;
                nomindees.Colspan = 4;
                nomindees.Border = 0;
                tablesnewone.AddCell(nomindees);

                PdfPCell cellmonth = new PdfPCell(new Phrase("Name in full with address of nominee(s):", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellmonth.HorizontalAlignment = 1;
                cellmonth.Colspan = 1;
                tablesnewone.AddCell(cellmonth);

                PdfPCell cellnoofdaysemployed = new PdfPCell(new Phrase("Relationship with the employee:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellnoofdaysemployed.HorizontalAlignment = 1;
                cellnoofdaysemployed.Colspan = 1;
                tablesnewone.AddCell(cellnoofdaysemployed);

                PdfPCell cellnoodayslaid = new PdfPCell(new Phrase("Age of nominee:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellnoodayslaid.HorizontalAlignment = 1;
                cellnoodayslaid.Colspan = 1;
                tablesnewone.AddCell(cellnoodayslaid);

                PdfPCell cellnoofdaysnotemployed = new PdfPCell(new Phrase("Proportion by Which the gratuity will be shared:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellnoofdaysnotemployed.HorizontalAlignment = 1;
                cellnoofdaysnotemployed.Colspan = 1;
                tablesnewone.AddCell(cellnoofdaysnotemployed);


                PdfPCell month = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                month.HorizontalAlignment = 1;
                month.Colspan = 1;
                tablesnewone.AddCell(month);

                PdfPCell noofdays = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                noofdays.HorizontalAlignment = 1;
                noofdays.Colspan = 1;
                noofdays.FixedHeight = 20;
                tablesnewone.AddCell(noofdays);

                PdfPCell noofdayslaidof = new PdfPCell(new Phrase("3", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                noofdayslaidof.HorizontalAlignment = 1;
                noofdayslaidof.Colspan = 1;
                tablesnewone.AddCell(noofdayslaidof);

                PdfPCell noofdaysemployed = new PdfPCell(new Phrase("4", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                noofdaysemployed.HorizontalAlignment = 1;
                noofdaysemployed.Colspan = 1;
                tablesnewone.AddCell(noofdaysemployed);


                PdfPCell addressnominee2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                addressnominee2.HorizontalAlignment = 0;
                addressnominee2.Colspan = 1;
                addressnominee2.FixedHeight = 20;
                tablesnewone.AddCell(addressnominee2);

                PdfPCell reltionnominee2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                reltionnominee2.HorizontalAlignment = 0;
                reltionnominee2.Colspan = 1;
                reltionnominee2.FixedHeight = 20;
                tablesnewone.AddCell(reltionnominee2);


                PdfPCell agenominee2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                agenominee2.HorizontalAlignment = 0;
                agenominee2.Colspan = 1;
                tablesnewone.AddCell(agenominee2);

                PdfPCell gratuity2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                gratuity2.HorizontalAlignment = 0;
                gratuity2.Colspan = 1;
                tablesnewone.AddCell(gratuity2);

                document.Add(tablesnewone);
                document.NewPage();

                PdfPTable tablesnewone1 = new PdfPTable(4);
                tablesnewone1.TotalWidth = 500f;
                tablesnewone1.LockedWidth = true;
                float[] width1 = new float[] { 2f, 2f, 2f, 2f };
                tablesnewone1.SetWidths(width1);

                PdfPCell space1 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                space1.HorizontalAlignment = 0;
                space1.Colspan = 4;
                space1.Border = 0;
                tablesnewone1.AddCell(space1);



                PdfPCell Statement12 = new PdfPCell(new Phrase("Statement", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                Statement12.HorizontalAlignment = 1;
                Statement12.Colspan = 4;
                Statement12.Border = 0;
                tablesnewone1.AddCell(Statement12);




                PdfPCell nameofemp = new PdfPCell(new Phrase("Name of employee in full:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                nameofemp.HorizontalAlignment = 0;
                nameofemp.Colspan = 2;
                nameofemp.PaddingTop = 5;
                nameofemp.PaddingBottom = 5;
                tablesnewone1.AddCell(nameofemp);

                PdfPCell nameofemp1 = new PdfPCell(new Phrase(name, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                nameofemp1.HorizontalAlignment = 0;
                nameofemp1.Colspan = 2;
                nameofemp1.PaddingBottom = 5;
                nameofemp1.PaddingTop = 5;
                tablesnewone1.AddCell(nameofemp1);

                PdfPCell sex = new PdfPCell(new Phrase("Sex:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                sex.HorizontalAlignment = 0;
                sex.Colspan = 2;
                sex.PaddingBottom = 5;
                sex.PaddingTop = 5;
                tablesnewone1.AddCell(sex);

                PdfPCell sex1 = new PdfPCell(new Phrase(Gender, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                sex1.HorizontalAlignment = 0;
                sex1.Colspan = 2;
                sex1.PaddingBottom = 5;
                sex1.PaddingTop = 5;
                tablesnewone1.AddCell(sex1);


                PdfPCell Religion = new PdfPCell(new Phrase("Religion:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Religion.HorizontalAlignment = 0;
                Religion.Colspan = 2;
                Religion.PaddingBottom = 5;
                Religion.PaddingTop = 5;
                tablesnewone1.AddCell(Religion);

                PdfPCell Religion1 = new PdfPCell(new Phrase(empReligion, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Religion1.HorizontalAlignment = 0;
                Religion1.Colspan = 2;
                Religion1.PaddingTop = 5;
                Religion1.PaddingBottom = 5;
                tablesnewone1.AddCell(Religion1);


                PdfPCell married = new PdfPCell(new Phrase("Whether unmarried/married/window/widower:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                married.HorizontalAlignment = 0;
                married.Colspan = 2;
                married.PaddingTop = 5;
                married.PaddingBottom = 5;
                tablesnewone1.AddCell(married);

                PdfPCell married1 = new PdfPCell(new Phrase(MaritalStatus, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                married1.HorizontalAlignment = 0;
                married1.Colspan = 2;
                married1.PaddingTop = 5;
                married1.PaddingBottom = 5;
                tablesnewone1.AddCell(married1);





                PdfPCell branch = new PdfPCell(new Phrase("Department/Branch/Section where employed:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                branch.HorizontalAlignment = 0;
                branch.Colspan = 2;
                branch.PaddingBottom = 5;
                branch.PaddingTop = 5;
                tablesnewone1.AddCell(branch);

                PdfPCell branch1 = new PdfPCell(new Phrase(designation, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                branch1.HorizontalAlignment = 0;
                branch1.Colspan = 2;
                branch1.PaddingTop = 5;
                branch1.PaddingBottom = 5;
                tablesnewone1.AddCell(branch1);

                PdfPCell postal = new PdfPCell(new Phrase("Post held with ticket No. or Serial No. if any:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                postal.HorizontalAlignment = 0;
                postal.Colspan = 2;
                postal.PaddingBottom = 5;
                postal.PaddingTop = 5;
                tablesnewone1.AddCell(postal);

                PdfPCell posta = new PdfPCell(new Phrase(Idno, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                posta.HorizontalAlignment = 0;
                posta.Colspan = 2;
                posta.PaddingTop = 5;
                posta.PaddingBottom = 5;
                tablesnewone1.AddCell(posta);


                PdfPCell appointment = new PdfPCell(new Phrase("Date of appointment:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                appointment.HorizontalAlignment = 0;
                appointment.Colspan = 2;
                appointment.PaddingTop = 5;
                appointment.PaddingBottom = 5;
                tablesnewone1.AddCell(appointment);

                PdfPCell appointment1 = new PdfPCell(new Phrase(date, FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                appointment1.HorizontalAlignment = 0;
                appointment1.Colspan = 2;
                appointment1.PaddingTop = 5;
                appointment1.PaddingBottom = 5;
                tablesnewone1.AddCell(appointment1);


                PdfPCell Permanentadd = new PdfPCell(new Phrase("\nPermanent address", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Permanentadd.HorizontalAlignment = 0;
                Permanentadd.Colspan = 2;
                Permanentadd.FixedHeight = 40;
                tablesnewone1.AddCell(Permanentadd);

                #region Present address String array

                string[] PeAdress = new string[8];
                if (peTaluka.Length > 0)
                {
                    PeAdress[0] = peTaluka;
                }
                else
                {
                    PeAdress[0] = "";
                }
                if (pestreet.Length > 0)
                {
                    PeAdress[1] = pestreet;
                }
                else
                {
                    PeAdress[1] = "";
                }
                if (pelmark.Length > 0)
                {
                    PeAdress[2] = pelmark;
                }
                else
                {
                    PeAdress[2] = "";
                }
                if (peTown.Length > 0)
                {
                    PeAdress[3] = peTown;
                }
                else
                {
                    PeAdress[3] = "";
                }
                if (pecity.Length > 0)
                {
                    PeAdress[4] = pecity;
                }
                else
                {
                    PeAdress[4] = "";
                }
                if (peDistrict.Length > 0)
                {
                    PeAdress[5] = peDistrict;
                }
                else
                {
                    PeAdress[5] = "";
                }
                if (pePincode.Length > 0)
                {
                    PeAdress[6] = pePincode;
                }
                else
                {
                    PeAdress[6] = "";
                }
                if (peState.Length > 0)
                {
                    PeAdress[7] = peState;
                }
                else
                {
                    PeAdress[7] = "";
                }

                string Address2 = string.Empty;

                for (int i = 0; i < 8; i++)
                {
                    Address2 += "  " + PeAdress[i];
                }


                #endregion

                PdfPCell Permanentadd1 = new PdfPCell(new Phrase(Address2.Trim(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Permanentadd1.HorizontalAlignment = 0;
                Permanentadd1.Colspan = 2;
                tablesnewone1.AddCell(Permanentadd1);

                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);



                PdfPCell place = new PdfPCell(new Phrase("Place:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                place.HorizontalAlignment = 0;
                place.Colspan = 4;
                place.Border = 0;
                tablesnewone1.AddCell(place);

                PdfPCell date1 = new PdfPCell(new Phrase("Date:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                date1.HorizontalAlignment = 0;
                date1.Colspan = 2;
                date1.Border = 0;
                tablesnewone1.AddCell(date1);



                PdfPCell thumb = new PdfPCell(new Phrase("Signature/Thumb impression of the employee", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                thumb.HorizontalAlignment = 0;
                thumb.Colspan = 2;
                thumb.Border = 0;
                tablesnewone1.AddCell(thumb);

                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);

                PdfPCell Delaration = new PdfPCell(new Phrase("Delaration by witnesses", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                Delaration.HorizontalAlignment = 1;
                Delaration.Colspan = 4;
                Delaration.Border = 0;
                tablesnewone1.AddCell(Delaration);

                PdfPCell signed = new PdfPCell(new Phrase("Nomination signed/thumb impresses before me:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                signed.HorizontalAlignment = 0;
                signed.Colspan = 4;
                signed.Border = 0;
                tablesnewone1.AddCell(signed);
                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);

                PdfPCell nameaddress = new PdfPCell(new Phrase("Name in full and full address", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                nameaddress.HorizontalAlignment = 0;
                nameaddress.Colspan = 2;
                nameaddress.Border = 0;
                tablesnewone1.AddCell(nameaddress);

                PdfPCell signwitness = new PdfPCell(new Phrase("Signature of witnesses:Of witnesses:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                signwitness.HorizontalAlignment = 0;
                signwitness.Colspan = 2;
                signwitness.Border = 0;
                tablesnewone1.AddCell(signwitness);


                PdfPCell celladd1 = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                celladd1.HorizontalAlignment = 0;
                celladd1.Colspan = 2;
                celladd1.Border = 0;
                celladd1.MinimumHeight = 40;
                tablesnewone1.AddCell(celladd1);

                PdfPCell cellsign1 = new PdfPCell(new Phrase("1", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsign1.HorizontalAlignment = 0;
                cellsign1.Colspan = 2;
                cellsign1.Border = 0;
                cellsign1.MinimumHeight = 40;
                tablesnewone1.AddCell(cellsign1);


                PdfPCell celladd2 = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                celladd2.HorizontalAlignment = 0;
                celladd2.Colspan = 2;
                celladd2.Border = 0;
                celladd2.MinimumHeight = 40;
                tablesnewone1.AddCell(celladd2);

                PdfPCell cellsign2 = new PdfPCell(new Phrase("2", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellsign2.HorizontalAlignment = 0;
                cellsign2.Colspan = 2;
                cellsign2.Border = 0;
                cellsign2.MinimumHeight = 40;
                tablesnewone1.AddCell(cellsign2);



                PdfPCell place12 = new PdfPCell(new Phrase("Place:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                place12.HorizontalAlignment = 0;
                place12.Colspan = 4;
                place12.Border = 0;
                tablesnewone1.AddCell(place12);


                PdfPCell date12 = new PdfPCell(new Phrase("Date:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                date12.HorizontalAlignment = 0;
                date12.Colspan = 4;
                date12.Border = 0;
                tablesnewone1.AddCell(date12);

                PdfPCell cetificate = new PdfPCell(new Phrase("Certificate by the Employer", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cetificate.HorizontalAlignment = 1;
                cetificate.Colspan = 4;
                cetificate.BorderWidthBottom = .5f;
                cetificate.BorderWidthLeft = 0;
                cetificate.BorderWidthRight = 0;
                cetificate.BorderWidthTop = .5f;
                cetificate.PaddingBottom = 5;
                tablesnewone1.AddCell(cetificate);


                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);

                PdfPCell certified = new PdfPCell(new Phrase("Certified that the particulers of the above nomination have been verified and recorded in this establishment.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                certified.HorizontalAlignment = 0;
                certified.Colspan = 4;
                certified.Border = 0;
                tablesnewone1.AddCell(certified);

                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);

                PdfPCell reference = new PdfPCell(new Phrase("Employer's Reference No., if any", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                reference.HorizontalAlignment = 0;
                reference.Colspan = 2;
                reference.Border = 0;
                reference.MinimumHeight = 70;
                tablesnewone1.AddCell(reference);


                PdfPCell authorized = new PdfPCell(new Phrase("Signature of the employer / officer Authorized", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                authorized.HorizontalAlignment = 0;
                authorized.Colspan = 2;
                authorized.Border = 0;
                tablesnewone1.AddCell(authorized);


                PdfPCell date123 = new PdfPCell(new Phrase("Date:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                date123.HorizontalAlignment = 0;
                date123.Colspan = 1;
                date123.Border = 0;
                tablesnewone1.AddCell(date123);


                PdfPCell dateaddname = new PdfPCell(new Phrase("Designation \nName and address of the establishment\nor rubber-stamp thereof", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                dateaddname.HorizontalAlignment = 0;
                dateaddname.Colspan = 3;
                dateaddname.Border = 0;
                tablesnewone1.AddCell(dateaddname);

                PdfPCell acknowledgement = new PdfPCell(new Phrase("Acknowledgement by the Employee", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                acknowledgement.HorizontalAlignment = 1;
                acknowledgement.Colspan = 4;
                acknowledgement.BorderWidthBottom = .5f;
                acknowledgement.BorderWidthLeft = 0;
                acknowledgement.BorderWidthRight = 0;
                acknowledgement.BorderWidthTop = .5f;
                acknowledgement.PaddingBottom = 5;
                tablesnewone1.AddCell(acknowledgement);

                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);

                PdfPCell recived = new PdfPCell(new Phrase("Received the duplicate copy of nomination in Form 'F' filed by me and duly certified by the employer", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                recived.HorizontalAlignment = 0;
                recived.Colspan = 4;
                recived.Border = 0;
                tablesnewone1.AddCell(recived);

                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);
                tablesnewone1.AddCell(space1);


                PdfPCell datea = new PdfPCell(new Phrase("Date:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                datea.HorizontalAlignment = 0;
                datea.Colspan = 2;
                datea.Border = 0;
                tablesnewone1.AddCell(datea);


                PdfPCell signa = new PdfPCell(new Phrase("Signature of the employee", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                signa.HorizontalAlignment = 2;
                signa.Colspan = 2;
                signa.Border = 0;
                tablesnewone1.AddCell(signa);


                PdfPCell cnote = new PdfPCell(new Phrase("Note: Strike out the words / paragraphs not applicable", FontFactory.GetFont(FontFactory.TIMES_ROMAN, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cnote.HorizontalAlignment = 1;
                cnote.Colspan = 4;
                cnote.BorderWidthBottom = .5f;
                cnote.BorderWidthLeft = 0;
                cnote.BorderWidthRight = 0;
                cnote.BorderWidthTop = .5f;
                cnote.PaddingBottom = 5;
                tablesnewone1.AddCell(cnote);

                document.Add(tablesnewone1);

                #endregion Basic Information of the Employee



                string filename = txtFname.Text + "/FormF(Gratuity).pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }
        }

        protected void btnFormQ_Click(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string fontsyle = "verdana";

            #region Variable Declaration

            string contactno = "";
            string Idno = "";
            string date = "";
            string postappliedfor = "";
            string name = "";
            string fathername = "";
            string SpouseName = "";
            string dateofbirth = "";
            string RelationName = "";
            string Designation = "";

            string maritalstatus = "";


            string prdoorno = "";
            string prstreet = "";
            string prarea = "";
            string prcity = "";
            string prLmark = "";
            string prDistrict = "";
            string prPincode = "";
            string prState = "";
            string prphone = "";



            string prTaluka = "";
            string prTown = "";
            string peTaluka = "";
            string peTown = "";

            string pedoor = "";
            string pestreet = "";
            string pearea = "";
            string pecity = "";
            string pelmark = "";
            string peDistrict = "";
            string pePincode = "";
            string peState = "";
            string pephone = "";



            string PFno = "";
            string Esino = "";
            string Companyname = "";
            string CompanyAddress = "";
            string branch = "";


            #endregion


            if (txtemplyid.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
                return;
            }

            #region  Begin  New Code

            #region Begin Variable Declaration

            string SPName = "";
            Hashtable HTEmpBiodata = new Hashtable();
            string Empid = "";
            DataTable dtEmpdetails = null;

            #endregion end Variable Declaration


            #region Begin Assign Values to The Variables
            SPName = "EmployeeFormsPDF";
            Empid = txtemplyid.Text;
            #endregion End Assign values To the Variables

            #region Begin Pass values to the Hash table
            HTEmpBiodata.Add("@empid", Empid);
            #endregion  end Pass values to the Hash table

            #region Begin  Call Stored Procedure
            dtEmpdetails = config.ExecuteAdaptorAsyncWithParams(SPName, HTEmpBiodata).Result;
            #endregion  End  Call Stored Procedure

            #endregion End New Code As on [31-05-2014]



            if (dtEmpdetails.Rows.Count > 0)
            {

                #region Assining data to Variables


                Idno = dtEmpdetails.Rows[0]["EmpId"].ToString();
                name = dtEmpdetails.Rows[0]["EmployeeName"].ToString();
                fathername = dtEmpdetails.Rows[0]["EmpFatherName"].ToString();
                dateofbirth = dtEmpdetails.Rows[0]["DtOfBirth"].ToString();
                Designation = dtEmpdetails.Rows[0]["Designation"].ToString();
                date = dtEmpdetails.Rows[0]["DtOfJoining"].ToString();
                PFno = dtEmpdetails.Rows[0]["PFNo"].ToString();
                Esino = dtEmpdetails.Rows[0]["EsiNo"].ToString();



                prTaluka = dtEmpdetails.Rows[0]["prTaluka"].ToString();
                prTown = dtEmpdetails.Rows[0]["prTown"].ToString();
                prLmark = dtEmpdetails.Rows[0]["prLmark"].ToString();
                prState = dtEmpdetails.Rows[0]["prState"].ToString();
                prcity = dtEmpdetails.Rows[0]["prcity"].ToString();
                prPincode = dtEmpdetails.Rows[0]["prPincode"].ToString();

                pelmark = dtEmpdetails.Rows[0]["peLmark"].ToString();
                peTaluka = dtEmpdetails.Rows[0]["peTaluka"].ToString();
                peTown = dtEmpdetails.Rows[0]["peTown"].ToString();
                pecity = dtEmpdetails.Rows[0]["pecity"].ToString();
                pePincode = dtEmpdetails.Rows[0]["pePincode"].ToString();
                peState = dtEmpdetails.Rows[0]["peState"].ToString();

                Companyname = dtEmpdetails.Rows[0]["CompanyName"].ToString();
                CompanyAddress = dtEmpdetails.Rows[0]["CompanyAddress"].ToString();
                branch = dtEmpdetails.Rows[0]["CompanyInfo"].ToString();

                #endregion


                MemoryStream ms = new MemoryStream();

                Document document = new Document(PageSize.LEGAL);
                // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");


                PdfPTable table = new PdfPTable(6);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] width = new float[] { 1.5f, 2f, 2f, 2f, 1.5f, 2f };
                table.SetWidths(width);

                PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontFactory.GetFont(fontsyle, Fontsize - 2, Font.NORMAL, BaseColor.BLACK)));
                cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellspace.Colspan = 6;
                cellspace.Border = 0;
                cellspace.PaddingTop = -5;

                PdfPCell celllogoHead = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 4, Font.NORMAL, BaseColor.BLACK)));
                celllogoHead.HorizontalAlignment = 2;
                celllogoHead.Colspan = 3;
                celllogoHead.Border = 0;
                table.AddCell(celllogoHead);

                PdfPCell cellHead = new PdfPCell(new Phrase("FORM Q", FontFactory.GetFont(fontsyle, Fontsize + 4, Font.NORMAL, BaseColor.BLACK)));
                cellHead.HorizontalAlignment = 0;
                cellHead.Colspan = 3;
                cellHead.Border = 0;
                cellHead.PaddingLeft = -30;
                table.AddCell(cellHead);

                PdfPCell cellrule = new PdfPCell(new Phrase("(see rule 24(9A)) ", FontFactory.GetFont(fontsyle, 12, Font.NORMAL, BaseColor.BLACK)));
                cellrule.HorizontalAlignment = 0;
                cellrule.Colspan = 6;
                cellrule.Border = 0;
                cellrule.PaddingLeft = 198;
                table.AddCell(cellrule);


                PdfPCell cellHead4 = new PdfPCell(new Phrase("APPOINTMENT ORDER", FontFactory.GetFont(fontsyle, Fontsize + 3, Font.NORMAL, BaseColor.BLACK)));
                cellHead4.HorizontalAlignment = 1;
                cellHead4.Colspan = 6;
                cellHead4.Border = 0;
                table.AddCell(cellHead4);
                document.Add(table);


                PdfPTable table2 = new PdfPTable(3);
                table2.TotalWidth = 500f;
                table2.LockedWidth = true;
                float[] width1 = new float[] { 0.5f, 6f, 7f };
                table2.SetWidths(width1);

                PdfPCell cellspace3 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellspace3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellspace3.Colspan = 3;
                cellspace3.Border = 0;
                table2.AddCell(cellspace3);
                //table2.AddCell(cellspace3);

                PdfPCell cellSno1 = new PdfPCell(new Phrase("\n1. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno1);

                PdfPCell cellName2 = new PdfPCell(new Phrase("\nName and address of Establishment\n\n\n\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName2.HorizontalAlignment = 0;
                cellName2.Colspan = 0;
                cellName2.Border = 0;
                cellName2.BorderWidthLeft = 0.5f;
                cellName2.BorderWidthRight = 0.2f;
                cellName2.BorderWidthTop = 0.5f;
                cellName2.BorderWidthBottom = 0.5f;
                cellName2.PaddingLeft = 10;
                table2.AddCell(cellName2);


                PdfPCell cellName3 = new PdfPCell(new Phrase(Companyname + "\n" + CompanyAddress, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellName3.HorizontalAlignment = 0;
                cellName3.Colspan = 0;
                cellName3.Border = 0;
                cellName3.PaddingLeft = 10;
                cellName3.BorderWidthLeft = 0;
                cellName3.BorderWidthRight = 0.9f;
                cellName3.BorderWidthTop = 0.5f;
                cellName3.BorderWidthBottom = 0.5f;
                table2.AddCell(cellName3);

                PdfPCell cellSno2 = new PdfPCell(new Phrase("\n2. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno2);

                PdfPCell cellName11 = new PdfPCell(new Phrase("\nName and address of the Employer\n\n\n\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName11.HorizontalAlignment = 0;
                cellName11.Colspan = 0;
                cellName11.Border = 0;
                cellName11.BorderWidthLeft = 0.5f;
                cellName11.BorderWidthRight = 0.2f;
                cellName11.BorderWidthTop = 0;
                cellName11.BorderWidthBottom = 0.5f;
                cellName11.PaddingLeft = 10;
                table2.AddCell(cellName11);


                PdfPCell cellName10 = new PdfPCell(new Phrase(Companyname + "\n" + CompanyAddress, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellName10.HorizontalAlignment = 0;
                cellName10.Colspan = 0;
                cellName10.Border = 0;
                cellName10.BorderWidthLeft = 0;
                cellName10.BorderWidthRight = 0.9f;
                cellName10.BorderWidthTop = 0;
                cellName10.BorderWidthBottom = 0.5f;
                cellName10.PaddingLeft = 10;
                table2.AddCell(cellName10);


                PdfPCell cellSno3 = new PdfPCell(new Phrase("\n3. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno3);

                PdfPCell cellName16 = new PdfPCell(new Phrase("\nName of the Employee\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName16.HorizontalAlignment = 0;
                cellName16.Colspan = 0;
                cellName16.Border = 0;
                cellName16.BorderWidthLeft = 0.5f;
                cellName16.BorderWidthRight = 0.2f;
                cellName16.BorderWidthTop = 0;
                cellName16.BorderWidthBottom = 0.5f;
                cellName16.PaddingLeft = 10;
                table2.AddCell(cellName16);

                PdfPCell cellName17 = new PdfPCell(new Phrase("\n" + name, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellName17.HorizontalAlignment = 0;
                cellName17.Colspan = 0;
                cellName17.Border = 0;
                cellName17.BorderWidthLeft = 0;
                cellName17.BorderWidthRight = 0.9f;
                cellName17.BorderWidthTop = 0;
                cellName17.BorderWidthBottom = 0.5f;
                cellName17.PaddingLeft = 10;
                table2.AddCell(cellName17);


                PdfPCell cellSno4 = new PdfPCell(new Phrase("\n4. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno4);

                PdfPCell cellName19 = new PdfPCell(new Phrase("\nHis / Her Postal Address\n\n\n\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName19.HorizontalAlignment = 0;
                cellName19.Colspan = 0;
                cellName19.Border = 0;
                cellName19.BorderWidthLeft = 0.5f;
                cellName19.BorderWidthRight = 0.2f;
                cellName19.BorderWidthTop = 0;
                cellName19.BorderWidthBottom = 0.5f;
                cellName19.PaddingLeft = 10;
                table2.AddCell(cellName19);

                #region Present address String array

                string[] PrAdress = new string[8];
                if (prTaluka.Length > 0)
                {
                    PrAdress[0] = prTaluka;
                }
                else
                {
                    PrAdress[0] = "";
                }
                if (prTown.Length > 0)
                {
                    PrAdress[1] = prTown;
                }
                else
                {
                    PrAdress[1] = "";
                }
                if (prLmark.Length > 0)
                {
                    PrAdress[2] = prLmark;
                }
                else
                {
                    PrAdress[2] = "";
                }
                if (prarea.Length > 0)
                {
                    PrAdress[3] = prarea;
                }
                else
                {
                    PrAdress[3] = "";
                }
                if (prcity.Length > 0)
                {
                    PrAdress[4] = prcity;
                }
                else
                {
                    PrAdress[4] = "";
                }
                if (prDistrict.Length > 0)
                {
                    PrAdress[5] = prDistrict;
                }
                else
                {
                    PrAdress[5] = "";
                }
                if (prPincode.Length > 0)
                {
                    PrAdress[6] = prPincode;
                }
                else
                {
                    PrAdress[6] = "";
                }
                if (prState.Length > 0)
                {
                    PrAdress[7] = prState;
                }
                else
                {
                    PrAdress[7] = "";
                }

                string Address1 = string.Empty;

                for (int i = 0; i < 8; i++)
                {
                    Address1 += "  " + PrAdress[i];
                }


                #endregion


                PdfPCell cellName18 = new PdfPCell(new Phrase("\n" + Address1.Trim(), FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellName18.HorizontalAlignment = 0;
                cellName18.Colspan = 0;
                cellName18.Border = 0;
                cellName18.BorderWidthLeft = 0;
                cellName18.BorderWidthRight = 0.9f;
                cellName18.BorderWidthTop = 0;
                cellName18.BorderWidthBottom = 0.5f;
                cellName18.PaddingLeft = 10;
                table2.AddCell(cellName18);


                PdfPCell cellSno5 = new PdfPCell(new Phrase("\n5. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                table2.AddCell(cellSno5);

                #region Present address String array

                string[] PeAdress = new string[8];
                if (peTaluka.Length > 0)
                {
                    PeAdress[0] = peTaluka;
                }
                else
                {
                    PeAdress[0] = "";
                }
                if (peTown.Length > 0)
                {
                    PeAdress[1] = peTown;
                }
                else
                {
                    PeAdress[1] = "";
                }
                if (pelmark.Length > 0)
                {
                    PeAdress[2] = pelmark;
                }
                else
                {
                    PeAdress[2] = "";
                }
                if (pearea.Length > 0)
                {
                    PeAdress[3] = pearea;
                }
                else
                {
                    PeAdress[3] = "";
                }
                if (pecity.Length > 0)
                {
                    PeAdress[4] = pecity;
                }
                else
                {
                    PeAdress[4] = "";
                }
                if (peDistrict.Length > 0)
                {
                    PeAdress[5] = peDistrict;
                }
                else
                {
                    PeAdress[5] = "";
                }
                if (pePincode.Length > 0)
                {
                    PeAdress[6] = pePincode;
                }
                else
                {
                    PeAdress[6] = "";
                }
                if (peState.Length > 0)
                {
                    PeAdress[7] = peState;
                }
                else
                {
                    PeAdress[7] = "";
                }

                string Address2 = string.Empty;

                for (int i = 0; i < 8; i++)
                {
                    Address2 += "  " + PeAdress[i];
                }


                #endregion

                PdfPCell cellPermanentAdd = new PdfPCell(new Phrase("\nHis / Her Permanent Address\n\n\n\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellPermanentAdd.HorizontalAlignment = 0;
                cellPermanentAdd.Colspan = 0;
                cellPermanentAdd.Border = 0;
                cellPermanentAdd.BorderWidthLeft = 0.5f;
                cellPermanentAdd.BorderWidthRight = 0.2f;
                cellPermanentAdd.BorderWidthTop = 0;
                cellPermanentAdd.BorderWidthBottom = 0.5f;
                cellPermanentAdd.PaddingLeft = 10;
                table2.AddCell(cellPermanentAdd);


                PdfPCell cellpermaddval = new PdfPCell(new Phrase("\n" + Address2.Trim(), FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellpermaddval.HorizontalAlignment = 0;
                cellpermaddval.Colspan = 0;
                cellpermaddval.Border = 0;
                cellpermaddval.BorderWidthLeft = 0;
                cellpermaddval.BorderWidthRight = 0.9f;
                cellpermaddval.BorderWidthTop = 0;
                cellpermaddval.PaddingLeft = 10;
                cellpermaddval.BorderWidthBottom = 0.5f;
                table2.AddCell(cellpermaddval);

                PdfPCell cellSno6 = new PdfPCell(new Phrase("\n6. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno6);

                PdfPCell cellFatherHusband = new PdfPCell(new Phrase("\nFather's / Husband's Name\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellFatherHusband.HorizontalAlignment = 0;
                cellFatherHusband.Colspan = 0;
                cellFatherHusband.Border = 0;
                cellFatherHusband.BorderWidthLeft = 0.5f;
                cellFatherHusband.BorderWidthRight = 0.2f;
                cellFatherHusband.BorderWidthTop = 0;
                cellFatherHusband.BorderWidthBottom = 0.5f;
                cellFatherHusband.PaddingLeft = 10;
                table2.AddCell(cellFatherHusband);

                if (fathername.Length > 0)
                {
                    RelationName = fathername;
                }
                else
                {
                    RelationName = "";

                }


                PdfPCell cellFatherHusbandval = new PdfPCell(new Phrase("\n" + RelationName, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellFatherHusbandval.HorizontalAlignment = 0;
                cellFatherHusbandval.Colspan = 0;
                cellFatherHusbandval.Border = 0;
                cellFatherHusbandval.PaddingLeft = 10;
                cellFatherHusbandval.BorderWidthLeft = 0;
                cellFatherHusbandval.BorderWidthRight = 0.9f;
                cellFatherHusbandval.BorderWidthTop = 0;
                cellFatherHusbandval.BorderWidthBottom = 0.5f;
                table2.AddCell(cellFatherHusbandval);


                PdfPCell cellSno7 = new PdfPCell(new Phrase("\n7. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno7.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno7);

                PdfPCell cellDOB = new PdfPCell(new Phrase("\nDate of Birth / Age\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellDOB.HorizontalAlignment = 0;
                cellDOB.Colspan = 0;
                cellDOB.Border = 0;
                cellDOB.BorderWidthLeft = 0.5f;
                cellDOB.BorderWidthRight = 0.2f;
                cellDOB.BorderWidthTop = 0;
                cellDOB.BorderWidthBottom = 0.5f;
                cellDOB.PaddingLeft = 10;
                table2.AddCell(cellDOB);


                PdfPCell cellDOBval = new PdfPCell(new Phrase("\n" + dateofbirth, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellDOBval.HorizontalAlignment = 0;
                cellDOBval.Colspan = 0;
                cellDOBval.Border = 0;
                cellDOBval.PaddingLeft = 10;
                cellDOBval.BorderWidthLeft = 0;
                cellDOBval.BorderWidthRight = 0.9f;
                cellDOBval.BorderWidthTop = 0;
                cellDOBval.BorderWidthBottom = 0.5f;
                table2.AddCell(cellDOBval);


                PdfPCell cellSno8 = new PdfPCell(new Phrase("\n8. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno8.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno8);

                PdfPCell cellDOJ = new PdfPCell(new Phrase("\nDate of His / Her entry into employment\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellDOJ.HorizontalAlignment = 0;
                cellDOJ.Colspan = 0;
                cellDOJ.Border = 0;
                cellDOJ.BorderWidthLeft = 0.5f;
                cellDOJ.BorderWidthRight = 0.2f;
                cellDOJ.BorderWidthTop = 0;
                cellDOJ.BorderWidthBottom = 0.5f;
                cellDOJ.PaddingLeft = 10;
                table2.AddCell(cellDOJ);


                PdfPCell cellDOJVal = new PdfPCell(new Phrase("\n" + date, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellDOJVal.HorizontalAlignment = 0;
                cellDOJVal.Colspan = 0;
                cellDOJVal.Border = 0;
                cellDOJVal.BorderWidthLeft = 0;
                cellDOJVal.BorderWidthRight = 0.9f;
                cellDOJVal.BorderWidthTop = 0;
                cellDOJVal.BorderWidthBottom = 0.5f;
                cellDOJVal.PaddingLeft = 10;
                table2.AddCell(cellDOJVal);


                PdfPCell cellSno9 = new PdfPCell(new Phrase("\n9. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno9.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno9);


                PdfPCell cellName21 = new PdfPCell(new Phrase("\nDesignation\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName21.HorizontalAlignment = 0;
                cellName21.Colspan = 0;
                cellName21.Border = 0;
                cellName21.BorderWidthLeft = 0.5f;
                cellName21.BorderWidthRight = 0.2f;
                cellName21.BorderWidthTop = 0;
                cellName21.BorderWidthBottom = 0.5f;
                cellName21.PaddingLeft = 10;
                table2.AddCell(cellName21);


                PdfPCell cellName20 = new PdfPCell(new Phrase("\n" + Designation, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellName20.HorizontalAlignment = 0;
                cellName20.Colspan = 0;
                cellName20.Border = 0;
                cellName20.BorderWidthLeft = 0;
                cellName20.BorderWidthRight = 0.9f;
                cellName20.BorderWidthTop = 0;
                cellName20.PaddingLeft = 10;
                cellName20.BorderWidthBottom = 0.5f;
                table2.AddCell(cellName20);

                PdfPCell cellSno10 = new PdfPCell(new Phrase("\n10. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno10.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno10);


                PdfPCell cellNatureofwork = new PdfPCell(new Phrase("\nNature of entrusted to him\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNatureofwork.HorizontalAlignment = 0;
                cellNatureofwork.Colspan = 0;
                cellNatureofwork.Border = 0;
                cellNatureofwork.BorderWidthLeft = 0.5f;
                cellNatureofwork.BorderWidthRight = 0.2f;
                cellNatureofwork.BorderWidthTop = 0;
                cellNatureofwork.BorderWidthBottom = 0.5f;
                cellNatureofwork.PaddingLeft = 10;
                table2.AddCell(cellNatureofwork);


                PdfPCell cellNatureofworkVal = new PdfPCell(new Phrase("\nGuarding\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellNatureofworkVal.HorizontalAlignment = 0;
                cellNatureofworkVal.Colspan = 0;
                cellNatureofworkVal.Border = 0;
                cellNatureofworkVal.BorderWidthLeft = 0;
                cellNatureofworkVal.BorderWidthRight = 0.9f;
                cellNatureofworkVal.BorderWidthTop = 0;
                cellNatureofworkVal.BorderWidthBottom = 0.5f;
                cellNatureofworkVal.PaddingLeft = 10;
                table2.AddCell(cellNatureofworkVal);

                PdfPCell cellSno11 = new PdfPCell(new Phrase("\n11. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno11.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno11);



                PdfPCell cellName14 = new PdfPCell(new Phrase("\nHis / Her serial number in the Register of Employment\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName14.HorizontalAlignment = 0;
                cellName14.Colspan = 0;
                cellName14.Border = 0;
                cellName14.BorderWidthLeft = 0.5f;
                cellName14.BorderWidthRight = 0.2f;
                cellName14.BorderWidthTop = 0;
                cellName14.BorderWidthBottom = 0.5f;
                cellName14.PaddingLeft = 10;
                table2.AddCell(cellName14);


                PdfPCell cellName15 = new PdfPCell(new Phrase("\n" + Idno + "\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellName15.HorizontalAlignment = 0;
                cellName15.Colspan = 0;
                cellName15.Border = 0;
                cellName15.BorderWidthLeft = 0;
                cellName15.BorderWidthRight = 0.9f;
                cellName15.BorderWidthTop = 0;
                cellName15.BorderWidthBottom = 0.5f;
                cellName15.PaddingLeft = 10;
                table2.AddCell(cellName15);

                PdfPCell cellSno12 = new PdfPCell(new Phrase("\n12. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno12);


                PdfPCell cellName22 = new PdfPCell(new Phrase("\nRates of Wages payable to him/her as on   ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName22.HorizontalAlignment = 0;
                cellName22.Colspan = 0;
                cellName22.Border = 0;
                cellName22.BorderWidthLeft = 0.5f;
                cellName22.BorderWidthRight = 0.2f;
                cellName22.BorderWidthTop = 0;
                cellName22.BorderWidthBottom = 0.5f;
                cellName22.PaddingLeft = 10;
                table2.AddCell(cellName22);


                //PdfPCell cellName29 = new PdfPCell(new Phrase("a) Basic               :  \nb) VDA               :  \nc) Other Allowances if any   : \n  TOTAL          :", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                //cellName29.HorizontalAlignment = 0;
                //cellName29.Colspan = 0;
                //cellName29.Border = 0;
                //cellName29.PaddingTop = 20;
                //cellName29.PaddingBottom = 20;
                //cellName29.BorderWidthLeft = 0;
                //cellName29.BorderWidthRight = 0.9f;
                //cellName29.BorderWidthTop = 0;
                //cellName29.BorderWidthBottom = 0.5f;
                //table2.AddCell(cellName29);



                PdfPCell cellName29 = new PdfPCell(new Phrase("a)Basic                                 :  \nb)VDA                                  :  \nc)Other Allowances if any    : \n  TOTAL                               :", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName29.HorizontalAlignment = 0;
                cellName29.Colspan = 0;
                cellName29.Border = 0;
                cellName29.PaddingTop = 20;
                cellName29.PaddingBottom = 20;
                cellName29.BorderWidthLeft = 0;
                cellName29.BorderWidthRight = 0.9f;
                cellName29.BorderWidthTop = 0;
                cellName29.BorderWidthBottom = 0.5f;
                table2.AddCell(cellName29);

                PdfPCell cellSno13 = new PdfPCell(new Phrase("\n13. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno13.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno13);


                PdfPCell cellName24 = new PdfPCell(new Phrase("\nE.S.I. Number\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName24.HorizontalAlignment = 0;
                cellName24.Colspan = 0;
                cellName24.Border = 0;
                cellName24.BorderWidthLeft = 0.5f;
                cellName24.BorderWidthRight = 0.2f;
                cellName24.BorderWidthTop = 0;
                cellName24.BorderWidthBottom = 0.5f;
                cellName24.PaddingLeft = 10;
                table2.AddCell(cellName24);

                PdfPCell cellName25 = new PdfPCell(new Phrase("\n" + Esino, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellName25.HorizontalAlignment = 0;
                cellName25.Colspan = 0;
                cellName25.Border = 0;
                cellName25.BorderWidthLeft = 0;
                cellName25.BorderWidthRight = 0.9f;
                cellName25.BorderWidthTop = 0f;
                cellName25.BorderWidthBottom = 0.5f;
                cellName25.PaddingLeft = 10;
                table2.AddCell(cellName25);

                PdfPCell cellSno14 = new PdfPCell(new Phrase("\n14. ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellSno14.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cellSno14);

                PdfPCell cellName26 = new PdfPCell(new Phrase("\nP.F. Account Number\n\n ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName26.HorizontalAlignment = 0;
                cellName26.Colspan = 0;
                cellName26.Border = 0;
                cellName26.BorderWidthLeft = 0.5f;
                cellName26.BorderWidthRight = 0.2f;
                cellName26.BorderWidthTop = 0;
                cellName26.BorderWidthBottom = 0.5f;
                cellName26.PaddingLeft = 10;
                table2.AddCell(cellName26);


                PdfPCell cellName27 = new PdfPCell(new Phrase("\n" + PFno, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellName27.HorizontalAlignment = 0;
                cellName27.Colspan = 0;
                cellName27.Border = 0;
                cellName27.BorderWidthLeft = 0;
                cellName27.BorderWidthRight = 0.9f;
                cellName27.BorderWidthTop = 0;
                cellName27.BorderWidthBottom = 0.5f;
                cellName27.PaddingLeft = 10;
                table2.AddCell(cellName27);


                PdfPCell cellName291 = new PdfPCell(new Phrase("\nPlace: " + branch + "                                                Accepted by                                      Signature of Employer\n\n ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellName291.HorizontalAlignment = 0;
                cellName291.Colspan = 3;
                cellName291.Border = 0;
                cellName291.BorderWidthLeft = 0.5f;
                cellName291.BorderWidthRight = 0.5f;
                cellName291.BorderWidthTop = 0;
                table2.AddCell(cellName291);

                PdfPCell cellNames28 = new PdfPCell(new Phrase("Date:                                                            Signature of Employee\n\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellNames28.HorizontalAlignment = 0;
                cellNames28.Colspan = 3;
                cellNames28.Border = 0;
                cellNames28.BorderWidthRight = 0.5f;
                cellNames28.BorderWidthLeft = 0.5f;
                cellNames28.BorderWidthBottom = 0.5f;
                table2.AddCell(cellNames28);


                document.Add(table2);


                // #endregion Basic Information of the Employee

                document.NewPage();

                PdfPTable tblFingerprints = new PdfPTable(6);
                tblFingerprints.TotalWidth = 500f;
                tblFingerprints.LockedWidth = true;
                float[] widthfinger = new float[] { 2f, 1.5f, 2f, 2f, 1.5f, 2f };
                tblFingerprints.SetWidths(widthfinger);

                string filename = txtFname.Text + "/FormQ.pdf";


                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

                //}



                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Duration expired');", true);
                //    return;
                //}
            }
        }

        protected void btnFormA_Click(object sender, EventArgs e)
        {

            int Fontsize = 11;
            string fontsyle = "verdana";

            #region Variable Declaration


            string Idno = "";
            string date = "";
            string designation = "";
            string name = "";
            string fathername = "";

            #endregion


            if (txtemplyid.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
                return;
            }

            #region  Begin  New Code



            #region Begin Variable Declaration

            string SPName = "";
            Hashtable HTEmpBiodata = new Hashtable();
            string Empid = "";
            DataTable dtEmpdetails = null;

            #endregion end Variable Declaration


            #region Begin Assign Values to The Variables
            SPName = "EmployeeFormsPDF";
            Empid = txtemplyid.Text;
            #endregion End Assign values To the Variables

            #region Begin Pass values to the Hash table
            HTEmpBiodata.Add("@empid", Empid);
            #endregion  end Pass values to the Hash table

            #region Begin  Call Stored Procedure
            dtEmpdetails = config .ExecuteAdaptorAsyncWithParams(SPName, HTEmpBiodata).Result;
            #endregion  End  Call Stored Procedure

            #endregion End New Code As on [31-05-2014]

            string doj = "";
            string monthnew = "";
            string year = "";



            if (dtEmpdetails.Rows.Count > 0)
            {

                #region Assining data to Variables

                Idno = dtEmpdetails.Rows[0]["EmpId"].ToString();
                designation = dtEmpdetails.Rows[0]["designation"].ToString();
                name = dtEmpdetails.Rows[0]["EmployeeName"].ToString();
                fathername = dtEmpdetails.Rows[0]["EmpFatherName"].ToString();
                date = dtEmpdetails.Rows[0]["DtOfJoining"].ToString();

                if (date.Length > 0)
                {
                    doj = DateTime.Parse(date, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    monthnew = DateTime.Parse(doj).ToString("MMMM");
                    year = DateTime.Parse(doj).Year.ToString().Substring(2, 2);
                }



                #endregion


                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL);
                // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");
                #region

                PdfPTable tablesnewone = new PdfPTable(5);
                tablesnewone.TotalWidth = 500f;
                tablesnewone.LockedWidth = true;
                float[] width = new float[] { 2f, 2f, 2f, 2f, 2f };
                tablesnewone.SetWidths(width);

                PdfPCell cellHead1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellHead1.HorizontalAlignment = 1;
                cellHead1.Colspan = 5;
                cellHead1.Border = 0;
                cellHead1.PaddingBottom = 15;
                tablesnewone.AddCell(cellHead1);

                PdfPCell cellHead = new PdfPCell(new Phrase("FORM-A  ", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellHead.HorizontalAlignment = 1;
                cellHead.Colspan = 5;
                cellHead.Border = 0;
                tablesnewone.AddCell(cellHead);

                PdfPCell cellmuster = new PdfPCell(new Phrase("MUSTER-ROLL", FontFactory.GetFont(fontsyle, 13, Font.BOLD, BaseColor.BLACK)));
                cellmuster.HorizontalAlignment = 1;
                cellmuster.Colspan = 5;
                cellmuster.Border = 0;
                tablesnewone.AddCell(cellmuster);

                PdfPCell cellRule32 = new PdfPCell(new Phrase("[Rule 3]", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellRule32.HorizontalAlignment = 1;
                cellRule32.Colspan = 5;
                cellRule32.Border = 0;
                tablesnewone.AddCell(cellRule32);


                PdfPCell cellHead2 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellHead2.HorizontalAlignment = 1;
                cellHead2.Colspan = 5;
                cellHead2.Border = 0;
                cellHead2.PaddingBottom = 15;
                tablesnewone.AddCell(cellHead2);


                PdfPCell nameofmine = new PdfPCell(new Phrase("Name of mine or circus- ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                nameofmine.HorizontalAlignment = 0;
                nameofmine.Colspan = 3;
                nameofmine.Border = 0;
                nameofmine.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(nameofmine);

                PdfPCell namemine = new PdfPCell(new Phrase(name, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                namemine.HorizontalAlignment = 0;
                namemine.Colspan = 2;
                namemine.Border = 0;
                namemine.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(namemine);

                PdfPCell cell1 = new PdfPCell(new Phrase("1. Serial number - ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell1.HorizontalAlignment = 0;
                cell1.Colspan = 3;
                cell1.Border = 0;
                cell1.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell1);

                PdfPCell cellIDNO = new PdfPCell(new Phrase(Idno, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellIDNO.HorizontalAlignment = 0;
                cellIDNO.Colspan = 2;
                cellIDNO.Border = 0;
                cellIDNO.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cellIDNO);


                PdfPCell cell2 = new PdfPCell(new Phrase("2. Name of women and her father's(or ,if married, husband's)name- ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell2.HorizontalAlignment = 0;
                cell2.Colspan = 3;
                cell2.Border = 0;
                cell2.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell2);

                PdfPCell cellfathername = new PdfPCell(new Phrase(fathername, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellfathername.HorizontalAlignment = 0;
                cellfathername.Colspan = 2;
                cellfathername.Border = 0;
                cellfathername.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cellfathername);

                PdfPCell cell3 = new PdfPCell(new Phrase("3. Date of appointment - ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell3.HorizontalAlignment = 0;
                cell3.Colspan = 3;
                cell3.Border = 0;
                cell3.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell3);

                PdfPCell celldoj = new PdfPCell(new Phrase(date, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                celldoj.HorizontalAlignment = 0;
                celldoj.Colspan = 2;
                celldoj.Border = 0;
                celldoj.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(celldoj);

                PdfPCell cell4 = new PdfPCell(new Phrase("4. Nature of Work - ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell4.HorizontalAlignment = 0;
                cell4.Colspan = 3;
                cell4.Border = 0;
                cell4.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell4);

                PdfPCell cellnature = new PdfPCell(new Phrase(designation, FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                cellnature.HorizontalAlignment = 0;
                cellnature.Colspan = 2;
                cellnature.Border = 0;
                cellnature.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cellnature);

                PdfPCell cell5 = new PdfPCell(new Phrase("5. Dates with month and year in which she is employed, laid off and employed", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell5.HorizontalAlignment = 0;
                cell5.Colspan = 5;
                cell5.Border = 0;
                cell5.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell5);

                PdfPCell cellmt = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellmt.HorizontalAlignment = 0;
                cellmt.Colspan = 5;
                cellmt.Border = 0;
                tablesnewone.AddCell(cellmt);

                PdfPCell cellmonth = new PdfPCell(new Phrase("Month", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellmonth.HorizontalAlignment = 1;
                cellmonth.Colspan = 1;
                tablesnewone.AddCell(cellmonth);

                PdfPCell cellnoofdaysemployed = new PdfPCell(new Phrase("No. of days employed", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellnoofdaysemployed.HorizontalAlignment = 1;
                cellnoofdaysemployed.Colspan = 1;
                tablesnewone.AddCell(cellnoofdaysemployed);

                PdfPCell cellnoodayslaid = new PdfPCell(new Phrase("No. of days laid off", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellnoodayslaid.HorizontalAlignment = 1;
                cellnoodayslaid.Colspan = 1;
                tablesnewone.AddCell(cellnoodayslaid);

                PdfPCell cellnoofdaysnotemployed = new PdfPCell(new Phrase("No. of days not employed", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellnoofdaysnotemployed.HorizontalAlignment = 1;
                cellnoofdaysnotemployed.Colspan = 1;
                tablesnewone.AddCell(cellnoofdaysnotemployed);

                PdfPCell cellremarks = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellremarks.HorizontalAlignment = 0;
                cellremarks.Colspan = 1;
                tablesnewone.AddCell(cellremarks);

                PdfPCell month = new PdfPCell(new Phrase(monthnew + "-" + year, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                month.HorizontalAlignment = 1;
                month.Colspan = 1;
                tablesnewone.AddCell(month);

                PdfPCell noofdays = new PdfPCell(new Phrase("NIL", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                noofdays.HorizontalAlignment = 1;
                noofdays.Colspan = 1;
                noofdays.FixedHeight = 20;
                tablesnewone.AddCell(noofdays);

                PdfPCell noofdayslaidof = new PdfPCell(new Phrase("NIL", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                noofdayslaidof.HorizontalAlignment = 1;
                noofdayslaidof.Colspan = 1;
                tablesnewone.AddCell(noofdayslaidof);

                PdfPCell noofdaysemployed = new PdfPCell(new Phrase("NIL", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                noofdaysemployed.HorizontalAlignment = 1;
                noofdaysemployed.Colspan = 1;
                tablesnewone.AddCell(noofdaysemployed);

                PdfPCell remarks = new PdfPCell(new Phrase("NIL", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                remarks.HorizontalAlignment = 1;
                remarks.Colspan = 1;
                tablesnewone.AddCell(remarks);

                PdfPCell cell6 = new PdfPCell(new Phrase("6. Date on Which the Women gives gives notice under section 6=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell6.HorizontalAlignment = 0;
                cell6.Colspan = 5;
                cell6.Border = 0;
                cell6.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell6);

                PdfPCell cell7 = new PdfPCell(new Phrase("7. Date of discharge/dismissal,if any=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell7.HorizontalAlignment = 0;
                cell7.Colspan = 5;
                cell7.Border = 0;
                cell7.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell7);

                PdfPCell cell8 = new PdfPCell(new Phrase("8. Date of Production of proof of pregnancy under section 6=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell8.HorizontalAlignment = 0;
                cell8.Colspan = 5;
                cell8.Border = 0;
                cell8.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell8);

                PdfPCell cell9 = new PdfPCell(new Phrase("9. Date of birth of child=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell9.HorizontalAlignment = 0;
                cell9.Colspan = 5;
                cell9.Border = 0;
                cell9.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell9);

                PdfPCell cell10 = new PdfPCell(new Phrase("10. Date of production of proof of delivery/miscarriage/medical termination of pregnancy/tubectomy operation/deth=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell10.HorizontalAlignment = 0;
                cell10.Colspan = 5;
                cell10.Border = 0;
                cell10.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell10);

                PdfPCell cellPl1 = new PdfPCell(new Phrase("11. Date of production of proof of illness referred to in section 10=Na  ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cellPl1.HorizontalAlignment = 0;
                cellPl1.Colspan = 5;
                cellPl1.Border = 0;
                cellPl1.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cellPl1);

                PdfPCell cel1l2 = new PdfPCell(new Phrase("12. Date with the amount of maternity benefit paid in advance of ecpected salary=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cel1l2.HorizontalAlignment = 0;
                cel1l2.Colspan = 5;
                cel1l2.Border = 0;
                cel1l2.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cel1l2);

                PdfPCell cell13 = new PdfPCell(new Phrase("13. Date with the amount of subsequent payment of maternity benefit=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell13.HorizontalAlignment = 0;
                cell13.Colspan = 5;
                cell13.Border = 0;
                cell13.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell13);

                PdfPCell cell14 = new PdfPCell(new Phrase("14. Date with the amount of bonus,if paid,under section 8=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell14.HorizontalAlignment = 0;
                cell14.Colspan = 5;
                cell14.Border = 0;
                cell14.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell14);

                PdfPCell cell15 = new PdfPCell(new Phrase("15. Date with the amount of wages paid on account of leave under section 9=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell15.HorizontalAlignment = 0;
                cell15.Colspan = 5;
                cell15.Border = 0;
                cell15.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell15);



                PdfPCell cell15a = new PdfPCell(new Phrase("15 A Date with the amount of wages paid on account of leave under section 9A=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell15a.HorizontalAlignment = 0;
                cell15a.Colspan = 5;
                cell15a.Border = 0;
                cell15a.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell15a);

                PdfPCell cell16 = new PdfPCell(new Phrase("16. Date with the amount of wages paid on account of leave under section 10 and period of leave granted =Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell16.HorizontalAlignment = 0;
                cell16.Colspan = 5;
                cell16.Border = 0;
                cell16.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell16);

                PdfPCell cell17 = new PdfPCell(new Phrase("17. Name of the person nominated by the women under section 6=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell17.HorizontalAlignment = 0;
                cell17.Colspan = 5;
                cell17.Border = 0;
                cell17.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell17);

                PdfPCell cell18 = new PdfPCell(new Phrase("18. If the women dies,the date of her death, the name of the person to whom maternity benifit and/or other amount was paid the amount thereof, and the date of payment=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell18.HorizontalAlignment = 0;
                cell18.Colspan = 5;
                cell18.Border = 0;
                cell18.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell18);

                PdfPCell cell19 = new PdfPCell(new Phrase("19. if the women dies and the child survives,the name of the person to whom the amount of maternity bnfit was paid on behalf of the period for which it was paid=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell19.HorizontalAlignment = 0;
                cell19.Colspan = 5;
                cell19.Border = 0;
                cell19.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell19);

                PdfPCell cell20 = new PdfPCell(new Phrase("20. Signature of the employer of the mine or circus authenticating the entries in the muster roll=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell20.HorizontalAlignment = 0;
                cell20.Colspan = 5;
                cell20.Border = 0;
                cell20.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell20);

                PdfPCell cell21 = new PdfPCell(new Phrase("21. Remarks coloum for the use of the inspector=Na", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cell21.HorizontalAlignment = 0;
                cell21.Colspan = 5;
                cell21.Border = 0;
                cell21.SetLeading(0f, 1.5f);
                tablesnewone.AddCell(cell21);


                document.Add(tablesnewone);

                #endregion Basic Information of the Employee



                string filename =txtFname.Text + "/FormA.pdf";

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }


        }

        protected void btnFormLeaveWages_Click(object sender, EventArgs e)
        {

            int Fontsize = 10;
            string fontsyle = "verdana";

            #region Variable Declaration


            string Idno = "";
            string date = "";
            string noofduties = "";
            string name = "";
            string fathername = "";
            string LeaveWages = "";
            string paymonth = "";
            string month = "";
            string year = "";
            DateTime firstday;
            DateTime lastday;
            #endregion

            if (txtemplyid.Text.Trim().Length== 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please select Employee');", true);
                return;
            }
            DateTime frmdate;
            string FromDate = "";
            string Frmonth = "";
            string FrYear = "";

            if (txtfrom.Text.Trim().Length > 0)
            {
                frmdate = DateTime.ParseExact(txtfrom.Text.Trim(), "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Frmonth = frmdate.ToString("MM");
                FrYear = frmdate.ToString("yy");
            }



            FromDate = FrYear + Frmonth;


            DateTime tdate;
            string ToDate = "";
            string Tomonth = "";
            string ToYear = "";

            if (txtto.Text.Trim().Length > 0)
            {
                tdate = DateTime.ParseExact(txtto.Text.Trim(), "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Tomonth = tdate.ToString("MM");
                ToYear = tdate.ToString("yy");

            }

            ToDate = ToYear + Tomonth;



            #region Begin Variable Declaration

            DataTable dtEmpdetails = null;

            #endregion end Variable Declaration




            string qry = "select eps.empid,(empfname+' '+empmname+' '+emplname) as empname,case convert(varchar(10),EmpDtofJoining,103) when '01/01/1900' then '' else convert(varchar(10),EmpDtofJoining,103) end EmpDtofJoining ,e.empfathername,month,noofduties,isnull(LeaveEncashAmt,'') as LeaveWages  from emppaysheet eps inner join empdetails e on e.empid=eps.empid where eps.empid='" + txtemplyid.Text+ "' and  eps.Monthnew between '" + FromDate.ToString() + "' and '" + ToDate.ToString() + "' order by eps.Monthnew asc ";
            dtEmpdetails = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;


            if (dtEmpdetails.Rows.Count > 0)
            {



                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL.Rotate());
                // var output = new FileStream(fileheader2, FileMode., FileAccess.Write, FileShare.None);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");


                PdfPTable tablesnewone = new PdfPTable(19);
                tablesnewone.TotalWidth = 900f;
                tablesnewone.LockedWidth = true;
                float[] width = new float[] { 2f, 4f, 6f, 4f, 4f, 4f, 3f, 3f, 3f, 3f, 3f, 2.5f, 3f, 3f, 3.2f, 2.5f, 2.5f, 2.5f, 3.5f };
                tablesnewone.SetWidths(width);


                PdfPCell CForm = new PdfPCell(new Phrase("FORM F", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CForm.HorizontalAlignment = 1;
                CForm.Colspan = 19;
                CForm.Border = 0;
                tablesnewone.AddCell(CForm);

                PdfPCell Crule = new PdfPCell(new Phrase("(See Rule 9)", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                Crule.HorizontalAlignment = 1;
                Crule.Colspan = 19;
                Crule.Border = 0;
                tablesnewone.AddCell(Crule);

                PdfPCell CRLW = new PdfPCell(new Phrase("REGISTER OF LEAVE WITH WAGES", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                CRLW.HorizontalAlignment = 1;
                CRLW.Colspan = 19;
                CRLW.Border = 0;
                CRLW.PaddingBottom = 10;
                tablesnewone.AddCell(CRLW);


                PdfPCell heading = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                heading.HorizontalAlignment = 1;
                heading.Colspan = 1;
                heading.BorderWidthBottom = 0;
                heading.BorderWidthLeft = .5f;
                heading.BorderWidthRight = 0;
                heading.BorderWidthTop = .5f;
                tablesnewone.AddCell(heading);


                PdfPCell headingd = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                headingd.HorizontalAlignment = 1;
                headingd.Colspan = 1;
                headingd.BorderWidthBottom = 0;
                headingd.BorderWidthLeft = .5f;
                headingd.BorderWidthRight = 0;
                headingd.BorderWidthTop = .5f;
                tablesnewone.AddCell(headingd);

                PdfPCell headingb = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                headingb.HorizontalAlignment = 1;
                headingb.Colspan = 1;
                headingb.BorderWidthBottom = 0;
                headingb.BorderWidthLeft = .5f;
                headingb.BorderWidthRight = 0;
                headingb.BorderWidthTop = .5f;
                tablesnewone.AddCell(headingb);

                PdfPCell headinga = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                headinga.HorizontalAlignment = 1;
                headinga.Colspan = 1;
                headinga.BorderWidthBottom = 0;
                headinga.BorderWidthLeft = .5f;
                headinga.BorderWidthRight = 0;
                headinga.BorderWidthTop = .5f;
                tablesnewone.AddCell(headinga);



                PdfPCell heading1 = new PdfPCell(new Phrase("                             Part - I                                                  Earned Leave", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading1.HorizontalAlignment = 0;
                heading1.Colspan = 11;
                tablesnewone.AddCell(heading1);


                PdfPCell heading2 = new PdfPCell(new Phrase("Part - II       Sick / Acc ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading2.HorizontalAlignment = 1;
                heading2.Colspan = 4;
                tablesnewone.AddCell(heading2);



                PdfPCell heading3 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading3.HorizontalAlignment = 1;
                heading3.Colspan = 1;
                heading3.BorderWidthBottom = 0;
                heading3.BorderWidthLeft = .5f;
                heading3.BorderWidthRight = 0;
                heading3.BorderWidthTop = 0;
                tablesnewone.AddCell(heading3);

                PdfPCell heading3a = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading3a.HorizontalAlignment = 1;
                heading3a.Colspan = 1;
                heading3a.BorderWidthBottom = 0;
                heading3a.BorderWidthLeft = .5f;
                heading3a.BorderWidthRight = 0;
                heading3a.BorderWidthTop = 0;
                tablesnewone.AddCell(heading3a);

                PdfPCell heading3b = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading3b.HorizontalAlignment = 1;
                heading3b.Colspan = 1;
                heading3b.BorderWidthBottom = 0;
                heading3b.BorderWidthLeft = .5f;
                heading3b.BorderWidthRight = 0;
                heading3b.BorderWidthTop = 0;
                tablesnewone.AddCell(heading3b);

                PdfPCell heading3c = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading3c.HorizontalAlignment = 1;
                heading3c.Colspan = 1;
                heading3c.BorderWidthBottom = 0;
                heading3c.BorderWidthLeft = .5f;
                heading3c.BorderWidthRight = 0;
                heading3c.BorderWidthTop = 0;
                tablesnewone.AddCell(heading3c);



                PdfPCell heading4 = new PdfPCell(new Phrase("No Of days worked", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading4.HorizontalAlignment = 1;
                heading4.Colspan = 3;
                tablesnewone.AddCell(heading4);


                PdfPCell heading5 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading5.HorizontalAlignment = 1;
                heading5.Colspan = 1;
                heading5.BorderWidthBottom = 0;
                heading5.BorderWidthLeft = 0;
                heading5.BorderWidthRight = 0;
                heading5.BorderWidthTop = .5f;
                tablesnewone.AddCell(heading5);


                PdfPCell heading5a = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading5a.HorizontalAlignment = 1;
                heading5a.Colspan = 1;
                heading5a.BorderWidthBottom = 0;
                heading5a.BorderWidthLeft = .5f;
                heading5a.BorderWidthRight = 0;
                heading5a.BorderWidthTop = .5f;
                tablesnewone.AddCell(heading5a);


                PdfPCell heading6 = new PdfPCell(new Phrase("Leave Availed", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading6.HorizontalAlignment = 1;
                heading6.Colspan = 3;
                tablesnewone.AddCell(heading6);


                PdfPCell heading7 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                heading7.HorizontalAlignment = 1;
                heading7.Colspan = 1;
                heading7.BorderWidthBottom = .5f;
                heading7.BorderWidthLeft = 0;
                heading7.BorderWidthRight = 0;
                heading7.BorderWidthTop = .5f;
                tablesnewone.AddCell(heading7);


                PdfPCell heading7a = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                heading7a.HorizontalAlignment = 1;
                heading7a.Colspan = 1;
                heading7a.BorderWidthBottom = .5f;
                heading7a.BorderWidthLeft = 0;
                heading7a.BorderWidthRight = 0;
                heading7a.BorderWidthTop = .5f;
                tablesnewone.AddCell(heading7a);


                PdfPCell heading7b = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                heading7b.HorizontalAlignment = 1;
                heading7b.Colspan = 1;
                heading7b.BorderWidthBottom = 0;
                heading7b.BorderWidthLeft = .5f;
                heading7b.BorderWidthRight = 0;
                heading7b.BorderWidthTop = .5f;
                tablesnewone.AddCell(heading7b);

                PdfPCell heading8 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                heading8.HorizontalAlignment = 1;
                heading8.Colspan = 1;
                heading8.BorderWidthBottom = 0;
                heading8.BorderWidthLeft = .5f;
                heading8.BorderWidthRight = 0;
                heading8.BorderWidthTop = .5f;
                tablesnewone.AddCell(heading8);

                PdfPCell heading9 = new PdfPCell(new Phrase("Accident", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                heading9.HorizontalAlignment = 1;
                heading9.Colspan = 2;
                tablesnewone.AddCell(heading9);

                PdfPCell heading10 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.BOLD, BaseColor.BLACK)));
                heading10.HorizontalAlignment = 1;
                heading10.Colspan = 1;
                heading10.BorderWidthBottom = 0;
                heading10.BorderWidthLeft = 0;
                heading10.BorderWidthRight = .5f;
                heading10.BorderWidthTop = .5f;
                tablesnewone.AddCell(heading10);


                PdfPCell register = new PdfPCell(new Phrase("Register Of", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                register.HorizontalAlignment = 1;
                register.Rowspan = 1;
                register.Rotation = 90;
                register.BorderWidthBottom = .5f;
                register.BorderWidthLeft = .5f;
                register.BorderWidthRight = .5f;
                register.BorderWidthTop = 0;
                tablesnewone.AddCell(register);

                PdfPCell dateofentry = new PdfPCell(new Phrase("Date of entry into service  ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                dateofentry.HorizontalAlignment = 1;
                dateofentry.Colspan = 1;
                dateofentry.BorderWidthBottom = .5f;
                dateofentry.BorderWidthLeft = .5f;
                dateofentry.BorderWidthRight = .5f;
                dateofentry.BorderWidthTop = 0;
                tablesnewone.AddCell(dateofentry);

                PdfPCell nameofemp = new PdfPCell(new Phrase("Name of the Employee", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                nameofemp.HorizontalAlignment = 1;
                nameofemp.Colspan = 1;
                nameofemp.BorderWidthBottom = .5f;
                nameofemp.BorderWidthLeft = .5f;
                nameofemp.BorderWidthRight = 0;
                nameofemp.BorderWidthTop = 0;
                tablesnewone.AddCell(nameofemp);

                PdfPCell fathername1 = new PdfPCell(new Phrase("Father/Husband Name", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                fathername1.HorizontalAlignment = 1;
                fathername1.Colspan = 1;
                fathername1.BorderWidthBottom = .5f;
                fathername1.BorderWidthLeft = .5f;
                fathername1.BorderWidthRight = .5f;
                fathername1.BorderWidthTop = 0;
                tablesnewone.AddCell(fathername1);

                PdfPCell from = new PdfPCell(new Phrase("From", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                from.HorizontalAlignment = 1;
                from.Colspan = 1;
                tablesnewone.AddCell(from);


                PdfPCell to = new PdfPCell(new Phrase("To", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                to.HorizontalAlignment = 1;
                to.Colspan = 1;
                to.Border = 0;
                tablesnewone.AddCell(to);


                PdfPCell wo = new PdfPCell(new Phrase("al Days Worked", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                wo.HorizontalAlignment = 0;
                wo.Colspan = 1;
                tablesnewone.AddCell(wo);

                PdfPCell leavearned = new PdfPCell(new Phrase("Leave Earned", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                leavearned.HorizontalAlignment = 0;
                leavearned.Colspan = 1;
                leavearned.BorderWidthBottom = .5f;
                leavearned.BorderWidthLeft = .5f;
                leavearned.BorderWidthRight = 0f;
                leavearned.BorderWidthTop = 0;
                tablesnewone.AddCell(leavearned);

                PdfPCell leavecredit = new PdfPCell(new Phrase("Leave at Credit", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                leavecredit.HorizontalAlignment = 0;
                leavecredit.Colspan = 1;
                leavecredit.BorderWidthBottom = .5f;
                leavecredit.BorderWidthLeft = .5f;
                leavecredit.BorderWidthRight = .5f;
                leavecredit.BorderWidthTop = 0;
                tablesnewone.AddCell(leavecredit);

                PdfPCell froml = new PdfPCell(new Phrase("From", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                froml.HorizontalAlignment = 1;
                froml.Colspan = 1;
                tablesnewone.AddCell(froml);

                PdfPCell tol = new PdfPCell(new Phrase("To", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                tol.HorizontalAlignment = 1;
                tol.Colspan = 1;
                tablesnewone.AddCell(tol);

                PdfPCell noofdays = new PdfPCell(new Phrase("No. of Days", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                noofdays.HorizontalAlignment = 0;
                noofdays.Colspan = 1;
                tablesnewone.AddCell(noofdays);

                PdfPCell balance = new PdfPCell(new Phrase("Balance on return", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                balance.HorizontalAlignment = 0;
                balance.Colspan = 1;
                balance.BorderWidthBottom = .5f;
                balance.BorderWidthLeft = .5f;
                balance.BorderWidthRight = 0;
                balance.BorderWidthTop = 0;
                tablesnewone.AddCell(balance);

                PdfPCell wages = new PdfPCell(new Phrase("Date on which wages for", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                wages.HorizontalAlignment = 1;
                wages.Colspan = 1;
                wages.BorderWidthBottom = .5f;
                wages.BorderWidthLeft = .5f;
                wages.BorderWidthRight = 0;
                wages.BorderWidthTop = 0;
                tablesnewone.AddCell(wages);

                PdfPCell Remarks = new PdfPCell(new Phrase("Remarks", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Remarks.HorizontalAlignment = 1;
                Remarks.Colspan = 1;
                Remarks.BorderWidthBottom = .5f;
                Remarks.BorderWidthLeft = .5f;
                Remarks.BorderWidthRight = 0;
                Remarks.BorderWidthTop = 0;
                tablesnewone.AddCell(Remarks);

                PdfPCell Year = new PdfPCell(new Phrase("Year", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Year.HorizontalAlignment = 1;
                Year.Colspan = 1;
                Year.BorderWidthBottom = .5f;
                Year.BorderWidthLeft = .5f;
                Year.BorderWidthRight = .5f;
                Year.BorderWidthTop = 0;
                tablesnewone.AddCell(Year);

                PdfPCell cred = new PdfPCell(new Phrase("cred", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                cred.HorizontalAlignment = 1;
                cred.Colspan = 1;
                tablesnewone.AddCell(cred);

                PdfPCell Avile = new PdfPCell(new Phrase("Avail", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                Avile.HorizontalAlignment = 0;
                Avile.Colspan = 1;
                tablesnewone.AddCell(Avile);

                PdfPCell balanceat = new PdfPCell(new Phrase("Balance at the end of the ", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                balanceat.HorizontalAlignment = 0;
                balanceat.Colspan = 1;
                balanceat.BorderWidthBottom = .5f;
                balanceat.BorderWidthLeft = .5f;
                balanceat.BorderWidthRight = .5f;
                balanceat.BorderWidthTop = 0;
                tablesnewone.AddCell(balanceat);

                int k = 1;


                for (int i = 0; i < dtEmpdetails.Rows.Count; i++)
                {

                    #region Assining data to Variables


                    Idno = dtEmpdetails.Rows[i]["EmpId"].ToString();
                    name = dtEmpdetails.Rows[i]["empname"].ToString();
                    fathername = dtEmpdetails.Rows[i]["EmpFatherName"].ToString();
                    date = dtEmpdetails.Rows[i]["EmpDtofJoining"].ToString();
                    noofduties = dtEmpdetails.Rows[i]["noofduties"].ToString();
                    LeaveWages = dtEmpdetails.Rows[i]["LeaveWages"].ToString();
                    paymonth = dtEmpdetails.Rows[i]["month"].ToString();

                    if (paymonth.Length == 3)
                    {
                        month = paymonth.Substring(0, 1);
                        year = DateTime.Now.Year.ToString().Substring(0, 2) + paymonth.Substring(1, 2);
                    }
                    else if (paymonth.Length == 4)
                    {
                        month = paymonth.Substring(0, 2);
                        year = DateTime.Now.Year.ToString().Substring(0, 2) + paymonth.Substring(2, 2);

                    }

                    firstday = GlobalData.Instance.GetFirstDayMonth(int.Parse(year), int.Parse(month));
                    lastday = GlobalData.Instance.GetLastDayMonth(int.Parse(year), int.Parse(month));

                    PdfPCell register1 = new PdfPCell(new Phrase(k.ToString() + "\n", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    register1.HorizontalAlignment = 1;
                    register1.Colspan = 1;
                    register1.PaddingBottom = 5;
                    tablesnewone.AddCell(register1);

                    PdfPCell dateofentry1 = new PdfPCell(new Phrase(date, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    dateofentry1.HorizontalAlignment = 1;
                    dateofentry1.Colspan = 1;
                    dateofentry1.PaddingBottom = 5;

                    tablesnewone.AddCell(dateofentry1);

                    PdfPCell nameofemp1 = new PdfPCell(new Phrase(name, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    nameofemp1.HorizontalAlignment = 0;
                    nameofemp1.Colspan = 1;
                    tablesnewone.AddCell(nameofemp1);

                    PdfPCell fathername11 = new PdfPCell(new Phrase(fathername, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    fathername11.HorizontalAlignment = 1;
                    fathername11.Colspan = 1;
                    tablesnewone.AddCell(fathername11);

                    PdfPCell from1 = new PdfPCell(new Phrase(firstday.ToString("dd.MM.yyyy"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    from1.HorizontalAlignment = 1;
                    from1.Colspan = 1;
                    tablesnewone.AddCell(from1);


                    PdfPCell to1 = new PdfPCell(new Phrase(lastday.ToString("dd.MM.yyyy"), FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    to1.HorizontalAlignment = 1;
                    to1.Colspan = 1;
                    tablesnewone.AddCell(to1);


                    PdfPCell wo1 = new PdfPCell(new Phrase(noofduties, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    wo1.HorizontalAlignment = 1;
                    wo1.Colspan = 1;
                    tablesnewone.AddCell(wo1);

                    PdfPCell leavearned1 = new PdfPCell(new Phrase(LeaveWages, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    leavearned1.HorizontalAlignment = 1;
                    leavearned1.Colspan = 1;
                    tablesnewone.AddCell(leavearned1);

                    PdfPCell leavecredit1 = new PdfPCell(new Phrase("Nil", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    leavecredit1.HorizontalAlignment = 1;
                    leavecredit1.Colspan = 1;
                    tablesnewone.AddCell(leavecredit1);

                    PdfPCell froml1 = new PdfPCell(new Phrase("Nil", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    froml1.HorizontalAlignment = 1;
                    froml1.Colspan = 1;
                    tablesnewone.AddCell(froml1);

                    PdfPCell tol1 = new PdfPCell(new Phrase("Nil", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    tol1.HorizontalAlignment = 1;
                    tol1.Colspan = 1;
                    tablesnewone.AddCell(tol1);

                    PdfPCell noofdays1 = new PdfPCell(new Phrase("Nil", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    noofdays1.HorizontalAlignment = 1;
                    noofdays1.Colspan = 1;
                    tablesnewone.AddCell(noofdays1);

                    PdfPCell balance1 = new PdfPCell(new Phrase(LeaveWages, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    balance1.HorizontalAlignment = 1;
                    balance1.Colspan = 1;
                    tablesnewone.AddCell(balance1);

                    PdfPCell wages1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    wages1.HorizontalAlignment = 1;
                    wages1.Colspan = 1;
                    tablesnewone.AddCell(wages1);

                    PdfPCell Remarks1 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Remarks1.HorizontalAlignment = 1;
                    Remarks1.Colspan = 1;
                    tablesnewone.AddCell(Remarks1);

                    PdfPCell Year1 = new PdfPCell(new Phrase(year, FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Year1.HorizontalAlignment = 1;
                    Year1.Colspan = 1;
                    tablesnewone.AddCell(Year1);

                    PdfPCell cred1 = new PdfPCell(new Phrase("0", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    cred1.HorizontalAlignment = 1;
                    cred1.Colspan = 1;
                    tablesnewone.AddCell(cred1);

                    PdfPCell Avile1 = new PdfPCell(new Phrase("0", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    Avile1.HorizontalAlignment = 1;
                    Avile1.Colspan = 1;
                    tablesnewone.AddCell(Avile1);

                    PdfPCell balanceat1 = new PdfPCell(new Phrase("12", FontFactory.GetFont(fontsyle, Fontsize, Font.NORMAL, BaseColor.BLACK)));
                    balanceat1.HorizontalAlignment = 1;
                    balanceat1.Colspan = 1;
                    tablesnewone.AddCell(balanceat1);

                    k++;

                }

                document.Add(tablesnewone);

                    #endregion Basic Information of the Employees





                string filename = txtFname.Text + "FormF(LeaveWages).pdf";

                document.Close();
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

        protected void Getempid()
        {

            string Sqlqry = "select  empid+' - '+Oldempid  empid from empdetails where empfname+' '+empmname+' '+emplname like '%" + txtFname.Text + "%' ";
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

    }
}