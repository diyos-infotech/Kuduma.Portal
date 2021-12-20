using KLTS.Data;
using System;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Text;
using Kuduma.Portal.DAL;
using System.Web.UI;


namespace Kuduma.Portal
{
    public partial class ClientAttendancePage : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();

        private static string EmpIDPrefix = string.Empty;

        private static string CmpIDPrefix = string.Empty;

        private static string BranchID = string.Empty;

        private static DataTable _dtEmployees = new DataTable();

        public const string _trString = @"<tr class='tr-emp-att' data-emp-id='##EMPID##' data-emp-desg='##EMPDESG##' data-emp-jdate='##EMPJDATE##' data-emp-rdate='##EMPRDATE##' data-emp-pf='##EMPPF##' data-emp-pt='##EMPPT##' data-emp-esi='##EMPESI##' >
                                <td>##EMPID##</td><td>##EMPNAME##</td><td>##EMPDESGNAME##</td>
                                 <td><input type='text' class='form-control num-txt txt-nod' value='##NOD##'></td>
                                 <td><input type='text' class='form-control num-txt txt-ot' value='##OT##'></td>
                                 <td><input type='text' class='form-control num-txt txt-wo' value='##WO##'></td>
                                 <td><input type='text' class='form-control num-txt txt-nhs' value='##NHS##'></td>
                                 <td><input type='text' class='form-control num-txt txt-nposts' value='##NPOSTS##'></td>
                                 <td><input type='text' class='form-control num-txt txt-candav' value='##CANADV##'></td>
                                 <td><input type='text' class='form-control num-txt txt-pen' value='##PEN##'></td>
                                 <td><input type='text' class='form-control num-txt txt-inctvs' value='##INCTVS##'></td> 
                                 <td><input type='text' class='form-control num-txt txt-Arrears' value='##ARREARS##'></td>           
                                 <td><label class='txt-linetotal'/></td>           
                                 <td><button type='button' class='btn btn-danger' onclick='DeleteRow(this); return false;'><i class='glyphicon glyphicon-trash'></i></button></td>
                                </tr>";

        private const string _attendanceQuery = @"select ROW_NUMBER() over(order by EA.EmpId) as sno,EA.EmpId,
			                   ISNULL(EmpFName,'')+' '+ISNULL(EmpMName,'')+' '+ISNULL(EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
			                   EA.NoOfDuties as NOD ,
			                   EA.Ot as OT ,
			                   EA.WO as WO,
			                   EA.NHS as NHS,
			                   EA.Npots as NPots,
			                   EA.CanteenAdv as CanAdv,
			                   EA.Penalty as Pen,
			                   EA.Incentivs as Inctvs ,
                               EA.Arrears as Arrears 
		                from EmpAttendance EA join EmpDetails ED on Ed.EmpId=EA.EmpId join Designations D on D.DesignId=EA.Design 
		                and EA.ClientId='##CLIENTID##' and EA.Month=##MONTH## and EA.ContractId='##CONTRACTID##'
		                union all
		                select ROW_NUMBER() over(order by ep.EmpId) as sno,ep.EmpId,
			                   ISNULL(ed.EmpFName,'')+' '+ISNULL(ed.EmpMName,'')+' '+ISNULL(ed.EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
			                   0 as NOD,
			                   0 as OT,
			                   0 as WO,
			                   0 as NHS,
			                   0 as NPots,
			                   0 as CanAdv,
			                   0 as Pen,
			                   0 as Inctvs ,
			                   0 as Arrears 
		                from EmpPostingOrder ep
		                inner join EmpDetails ed on ep.EmpId = ed.EmpId
		                inner join Designations d on ep.Desgn = d.DesignId
		                where ToUnitId = '##CLIENTID##' and (relievemonth is null or  relievemonth  <>  ##MONTH##)
		                and ep.EmpId not in (select EmpId from EmpAttendance where ClientId = '##CLIENTID##' and month = ##MONTH## and ContractId='##CONTRACTID##')";

        private const string _attendanceSummaryquery = @"select d.Design DesName,
	                                                           cast(sum(ea.NoOfDuties)as nvarchar) NODTotal,
	                                                           cast(sum(ea.OT)as nvarchar) OTTotal,
	                                                           cast(sum(ea.WO)as nvarchar) WOTotal,
	                                                           cast(sum(ea.NHS)as nvarchar) NHSTotal,
	                                                           cast(sum(ea.Npots)as nvarchar) NpotsTotal,
	                                                           cast(sum(ea.Penalty)as nvarchar) PenTotal,
	                                                           cast(sum(ea.Incentivs)as nvarchar) InctvsTotal,
	                                                           cast(sum(ea.CanteenAdv)as nvarchar) CanAdvTotal,
	                                                           cast(sum(ea.Arrears)as nvarchar) ArrearsTotal
                                                        from EmpAttendance ea 
                                                        inner join Designations d on d.DesignId = ea.Design
                                                        inner join EmpPostingOrder ep on ea.EmpId = ep.EmpId
                                                        where ea.ClientId = '##CLIENTID##' and ea.[MONTH]= ##MONTH##
                                                        and ep.RelieveMonth is null
                                                        group by ea.Design,d.Design";

        public static DataTable EmployeesDataTable
        {
            get
            {
                EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();
                BranchID = HttpContext.Current.Session["BranchID"].ToString();
                DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

                if (_dtEmployees.Rows.Count < 1)
                {
                    var dtEmployees = GlobalData.Instance.LoadEmpNames(EmpIDPrefix, dtBranch);
                    _dtEmployees = dtEmployees;
                }
                return _dtEmployees;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            if (!IsPostBack)
            {
                if (this.Master != null)
                {
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("s2");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                FillMonthList();
                //BindEmpddls();
                GetClientsData();

            }
        }

        protected void FillMonthList()
        {
            //month
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            string month = monthName[DateTime.Now.Month - 1];
            string LastMonth = "";
            ddlMonth.Items.Add(month);
            try
            {
                month = monthName[DateTime.Now.Month - 2];
            }
            catch (IndexOutOfRangeException ex)
            {
                month = monthName[11];
            }
            try
            {
                LastMonth = monthName[DateTime.Now.Month - 3];
            }
            catch (IndexOutOfRangeException ex)
            {
                LastMonth = monthName[12 - (3 - DateTime.Now.Month)];
            }

            ddlMonth.Items.Add(month);
            ddlMonth.Items.Add(LastMonth);

            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Value = "0", Text = "--Select--" });

        }

        private void BindEmpddls()
        {
            DataTable DtDesignations = GlobalData.Instance.LoadDesigns();
            if (DtDesignations.Rows.Count > 0)
            {
                ddlEmpDesg.DataValueField = "Designid";
                ddlEmpDesg.DataTextField = "Design";
                ddlEmpDesg.DataSource = DtDesignations;
                ddlEmpDesg.DataBind();
            }
            ddlEmpDesg.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Value = "0", Text = "--Select--" });
        }

        private void GetClientsData()
        {
            BranchID = HttpContext.Current.Session["BranchID"].ToString();

            var result = "";
            string query = "select clientid,clientname,clientphonenumbers,ourcontactpersonid from clients where ClientId like '" + CmpIDPrefix + "%' and ClientStatus=1 and BranchID in (" + BranchID + ") Order By  Clientname";
            var dtAllClients = SqlHelper.Instance.GetTableByQuery(query);
            if (dtAllClients.Rows.Count > 0)
            {
                var obj = (from row in dtAllClients.AsEnumerable()
                           select new
                           {
                               ClientId = row.Field<string>("clientid"),
                               ClientName = row.Field<string>("clientname"),
                               PhoneNumber = row.Field<string>("clientphonenumbers"),
                               ContactPerson = row.Field<string>("ourcontactpersonid")
                           }).ToList();
                result = new JavaScriptSerializer().Serialize(obj);
                hdClientData.Value = result;
            }
        }

        [WebMethod]
        public static string GetEmployessData(string strid)
        {
            var result = string.Empty;
            try
            {
                if (EmployeesDataTable.Rows.Count > 0)
                {
                    var obj = (from row in EmployeesDataTable.AsEnumerable()
                               select new
                               {
                                   EmpId = row.Field<string>("Empid"),
                                   EmpName = row.Field<string>("FullName"),
                                   EmpDesg = row.Field<string>("Designation")
                               }).ToList();

                    obj = obj.Where(o => o.EmpId.Contains(strid.Trim())).ToList();
                    result = new JavaScriptSerializer().Serialize(obj);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        [WebMethod]
        public static string GetEmployessNameData(string strname)
        {
            var result = string.Empty;
            try
            {
                if (EmployeesDataTable.Rows.Count > 0)
                {
                    var obj = (from row in EmployeesDataTable.AsEnumerable()
                               select new
                               {
                                   EmpId = row.Field<string>("Empid"),
                                   EmpName = row.Field<string>("FullName"),
                                   EmpDesg = row.Field<string>("Designation")
                               }).ToList();

                    obj = obj.Where(o => o.EmpName.Contains(strname.Trim())).ToList();
                    result = new JavaScriptSerializer().Serialize(obj);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        [WebMethod]
        public static string GetAttendanceGrid(string clientId, string month)
        {
            var result = string.Empty;
            var resultobj = string.Empty;
            try
            {
                var empDt = GetAttendanceMainDatatable(clientId, month);
                if (empDt.Rows.Count > 0)
                {
                    var obj = (from row in empDt.AsEnumerable()
                               select new
                               {
                                   EmpId = row.Field<string>("EmpId"),
                                   EmpName = row.Field<string>("EmpName"),
                                   DesgId = row.Field<int>("DesId"),
                                   DesgName = row.Field<string>("DesName"),
                                   NoOfDuties = row.Field<float>("NOD"),
                                   OT = row.Field<float>("OT"),
                                   WO = row.Field<float>("WO"),
                                   NHS = row.Field<float>("NHS"),
                                   NPosts = row.Field<float>("NPots"),
                                   CanteenAdv = row.Field<float>("CanAdv"),
                                   Penalty = row.Field<float>("Pen"),
                                   Incentivs = row.Field<float>("Inctvs"),
                                   Arrears = row.Field<float>("Arrears")
                               }).ToList();

                    resultobj = new JavaScriptSerializer().Serialize(obj);
                    result = "success";
                }
                else
                {
                    result = "nodata";
                    resultobj = "Attendance Not Avaialable for  this month of the Selected client";
                }

            }
            catch (Exception ex)
            {
                result = "fail";
            }
            var res = new { msg = result, Obj = resultobj };
            resultobj = new JavaScriptSerializer().Serialize(res);
            return resultobj;
        }

        public static DataTable GetAttendanceMainDatatable(string clientId, string month)
        {
            var LastDate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");
            var result = new DataTable();
            var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            var strquery = "select contractid from contracts where clientid= '" + clientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
            var contractdata = SqlHelper.Instance.GetTableByQuery(strquery);
            var contractId = string.Empty;
            if (contractdata.Rows.Count > 0)
            {
                contractId = contractdata.Rows[0]["contractid"].ToString();
            }
            if (!string.IsNullOrEmpty(contractId))
            {
                var sx = _attendanceQuery.Replace("##MONTH##", Month.ToString())
                                 .Replace("##CLIENTID##", clientId)
                                 .Replace("##CONTRACTID##", contractId);
                var attData = SqlHelper.Instance.GetTableByQuery(sx);
                if (attData.Rows.Count > 0)
                {
                    result = attData;
                }
            }
            return result;

        }

        [WebMethod]
        public static string GetAttendanceSummary(string clientId, string month)
        {
            var result = string.Empty;
            var resultobj = string.Empty;
            try
            {
                var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
                var sx = _attendanceSummaryquery.Replace("##MONTH##", Month.ToString())
                                         .Replace("##CLIENTID##", clientId);
                var attData = SqlHelper.Instance.GetTableByQuery(sx);
                if (attData.Rows.Count > 0)
                {
                    var obj = (from row in attData.AsEnumerable()
                               select new
                               {
                                   DesgName = row.Field<string>("DesName"),
                                   NODTotal = row.Field<string>("NODTotal"),
                                   OTTotal = row.Field<string>("OTTotal"),
                                   WOTotal = row.Field<string>("WOTotal"),
                                   NHSTotal = row.Field<string>("NHSTotal"),
                                   NpotsTotal = row.Field<string>("NpotsTotal"),
                                   PenTotal = row.Field<string>("PenTotal"),
                                   InctvsTotal = row.Field<string>("InctvsTotal"),
                                   CanAdvTotal = row.Field<string>("CanAdvTotal"),
                                   ArrearsTotal = row.Field<string>("ArrearsTotal")
                               }).ToList();
                    resultobj = new JavaScriptSerializer().Serialize(obj);
                    result = "success";
                }
                else
                {
                    result = "nodata";
                }
            }
            catch (Exception ex)
            {
                result = "fail";
            }
            var res = new { msg = result, Obj = resultobj };
            resultobj = new JavaScriptSerializer().Serialize(res);
            return resultobj;
        }

        [WebMethod]
        public static string SaveAttendance(List<EmpAttendance> lst)
        {
            string OrderedDAte = DateTime.Now.Date.ToString();
            var LastDate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");

            try
            {
                foreach (var item in lst)
                {
                    var attendancetotal = item.NOD + item.OT + item.WO + item.NHS + item.Nposts;
                    if (attendancetotal > 0)
                    {
                        if (item.NewAdd) EmpTransfer(item);
                        var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(item.MonthIndex));
                        var strquery = "select contractid from contracts where clientid= '" + item.ClientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
                        var contractdata = SqlHelper.Instance.GetTableByQuery(strquery);
                        var contractId = string.Empty;
                        if (contractdata.Rows.Count > 0)
                        {
                            contractId = contractdata.Rows[0]["contractid"].ToString();
                        }
                        if (!string.IsNullOrEmpty(contractId))
                        {
                            var empquery = "select COUNT(*) as empcount from  Empattendance Where Empid = '" + item.EmpId +
                                        "' and [month]= " + Month + " and ClientId = '" + item.ClientId +
                                        "'  and  Design = " + item.EmpDesg + " and   contractid= '" + contractId + "'";
                            var empdata = SqlHelper.Instance.GetTableByQuery(empquery);
                            var empcount = string.Empty;
                            if (empdata.Rows.Count > 0)
                            {
                                empcount = empdata.Rows[0]["empcount"].ToString();
                            }
                            var query = string.Empty;


                            if (Convert.ToInt32(empcount) > 0)
                            {
                                query = "update EmpAttendance set NoofDuties=" + item.NOD
                                                    + ",OT=" + item.OT
                                                    + ",Penalty=" + item.Penality
                                                    + ",CanteenAdv=" + item.CanAdv
                                                    + ",Incentivs=" + item.Incentives
                                                     + ",Arrears=" + item.Arrears
                                                    + ",Design='" + item.EmpDesg
                                                    + "',WO=" + item.WO
                                                    + ",NHS=" + item.NHS
                                                    + ",NPOTS=" + item.Nposts
                                                    + " Where empid='" + item.EmpId
                                                    + "' and ClientId='" + item.ClientId
                                                    + "' and [Month]=" + Month
                                                    + " and  Design='" + item.EmpDesg
                                                    + "' and contractid= '" + contractId + "'";
                            }
                            else
                            {
                                query = "insert  EmpAttendance(clientid,empid,[month],Design,contractId,NoofDuties,OT,Penalty,CanteenAdv,WO,NHS,NPOTS,Incentivs,Arrears,DateCreated)" +
                                "values('" + item.ClientId + "','" + item.EmpId + "'," + Month + ",'" + item.EmpDesg + "','" + contractId + "'," + item.NOD + "," + item.OT + "," + item.Penality + "," + item.CanAdv + "," + item.WO + "," + item.NHS + "," + item.Nposts + "," + item.Incentives + "," + item.Arrears + ",GETDATE() )";
                            }
                            var res = SqlHelper.Instance.ExecuteDMLQry(query);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "success";
        }

        private static void EmpTransfer(EmpAttendance emp)
        {
            try
            {
                var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(emp.MonthIndex));
                var ReaddMonth = Timings.Instance.GetReverseIdForSelectedMonth(Convert.ToInt32(emp.MonthIndex));
                var jdate = DateTime.Parse(emp.JoiningDate, CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy-MM-dd hh:mm:ss");
                var rdate = DateTime.Parse(emp.JoiningDate, CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy-MM-dd hh:mm:ss");
                var odate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");
                var query = string.Empty;
                var ordermax = "select max(cast(OrderId as int))+ 1 as ordercount from EmpPostingOrder";
                var orderdata = SqlHelper.Instance.GetTableByQuery(ordermax);
                var orderId = string.Empty;
                if (orderdata.Rows.Count > 0)
                {
                    orderId = orderdata.Rows[0]["ordercount"].ToString();
                }
                query = " Insert into EmpPostingOrder(EmpId,OrderId,OrderDt,JoiningDt,Desgn,TransferType,PF,ESI,PT,tounitId)" +
                "values('" + emp.EmpId + "','" + orderId + "','" + odate + "','" + jdate + "','" + emp.EmpDesg + "'," + emp.TransferType +
                "," + (emp.PF ? 1 : 0) + "," + (emp.ESI ? 1 : 0) + "," + (emp.PT ? 1 : 0) + ",'" + emp.ClientId + "')";
                var res = SqlHelper.Instance.ExecuteDMLQry(query);
            }
            catch
            { }
        }

        [WebMethod]
        public static string DeleteAttendance(string empId, string empDesgId, string clientId, string month)
        {
            string JoiningDate = DateTime.Now.Date.ToString();
            string OrderedDAte = DateTime.Now.Date.ToString();
            string RelievingDate = DateTime.Now.Date.ToString();
            var LastDate = DateTime.Now.Date;
            var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            //var RelMonth = Timings.Instance.GetReverseIdForSelectedMonth(Convert.ToInt32(month));
            try
            {
                var deletequery = "delete from EmpAttendance where [MONTH] = " + Month + " and EmpId = '" + empId + "' and ClientId = '" + clientId + "' and Design = '" + empDesgId + "'";
                var updatequery = "update EmpPostingOrder set RelieveMonth = " + Month + " where EmpId = '" + empId + "' and ToUnitId = '" + clientId + "' and Desgn = '" + empDesgId + "'";
                var res = SqlHelper.Instance.ExecuteDMLQry(deletequery);
                var res1 = SqlHelper.Instance.ExecuteDMLQry(updatequery);
            }
            catch (Exception ex)
            {
            }
            return "";
        }


        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";

            #region Validation

            if (txtmonth.Value.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Value);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Value, CultureInfo.GetCultureInfo("en-gb")).ToString();
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
            if (chkold.Checked == false)
            {
                month = Timings.Instance.GetIdForSelectedMonth(ddlMonth.SelectedIndex);
                //return month;
            }
            if (chkold.Checked == true)
            {
                DateTime date = DateTime.Parse(txtmonth.Value, CultureInfo.GetCultureInfo("en-gb"));
                month = Timings.Instance.GetIdForEnteredMOnth(date);
                return month;
            }
            return month;

            #endregion
        }

        protected void btndownloadpdf_Click(object sender, EventArgs e)
        {
            int month = GetMonthBasedOnSelectionDateorMonth();

            string sText = HiddenField1.Value;

            string SPName = "WageSheetSummary";
            Hashtable ht = new Hashtable();
            ht.Add("@ClientIDList", sText);
            ht.Add("@MonthList", month);

            DataTable dt = null;

            dt = config.ExecuteAdaptorWithParams(SPName, ht).Result;
            DataTable dtgroup = dt.DefaultView.ToTable(true, "ClientID");


            string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
            DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string PFNOForms = "";
            string TotalPFNOForms = "";
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
                //PFNOForms = compInfo.Rows[0]["PFNoForms"].ToString();
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
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                for (int j = 0; j < dtgroup.Rows.Count; j++)
                {
                    document.NewPage();


                    PdfPTable Maintable = new PdfPTable(14);
                    Maintable.TotalWidth = 790;
                    Maintable.HeaderRows = 3;
                    Maintable.LockedWidth = true;
                    float[] width = new float[] { 1f, 1.5f, 1.5f, 1f, 1f, 1f, 1f, 1.2f, 1f, 1f, 1f, 1f, 1f, 1f };
                    Maintable.SetWidths(width);



                    PdfPCell cell = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 7;
                    cell.Border = 0;
                    //Maintable.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Zone", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 1;
                    cell.Border = 0;
                    //Maintable.AddCell(cell);

                    cell = new PdfPCell(new Phrase("PF1/WB1/KOLKATA/WEST/SEC", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Colspan = 6;
                    cell.Border = 0;
                    // Maintable.AddCell(cell);

                    PdfPCell cellunit = new PdfPCell(new Phrase("UNIT SUMMARY OF", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellunit.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellunit.Colspan = 3;
                    cellunit.Border = 0;
                    Maintable.AddCell(cellunit);

                    PdfPCell cell33 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell33.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell33.Colspan = 2;
                    cell33.Border = 0;
                    Maintable.AddCell(cell33);

                    PdfPCell cell52 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell52.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell52.Colspan = 2;
                    cell52.Border = 0;
                    Maintable.AddCell(cell52);

                    cell52 = new PdfPCell(new Phrase("Unit Code", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell52.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell52.Colspan = 2;
                    cell52.Border = 0;
                    Maintable.AddCell(cell52);

                    string Clientid = dtgroup.Rows[j]["Clientid"].ToString();
                    PdfPCell cell22 = new PdfPCell(new Phrase(Clientid, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell22.HorizontalAlignment = 3; //0=Left, 1=Centre, 2=Right
                    cell22.Colspan = 5;
                    cell22.Border = 0;
                    Maintable.AddCell(cell22);

                    PdfPCell celldata2 = new PdfPCell(new Phrase("EMPLOYEE", FontFactory.GetFont(fontsyle, Fontsize + 1, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    celldata2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    celldata2.Colspan = 3;
                    celldata2.Border = 0;
                    Maintable.AddCell(celldata2);

                    PdfPCell cell3 = new PdfPCell(new Phrase("Salary Month", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell3.Colspan = 2;
                    cell3.Border = 0;
                    Maintable.AddCell(cell3);

                    PdfPCell cell5 = new PdfPCell(new Phrase(dt.Rows[0]["MOnthName"].ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell5.Colspan = 2;
                    cell5.Border = 0;
                    Maintable.AddCell(cell5);

                    cell5 = new PdfPCell(new Phrase("Unit", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell5.Colspan = 1;
                    cell5.Border = 0;
                    Maintable.AddCell(cell5);

                    string strClientname = "select ClientName from Clients where clientid='" + Clientid + "'";
                    DataTable dtclientname = config.ExecuteAdaptorAsyncWithQueryParams(strClientname).Result;
                    string Clientname = "";
                    if (dtclientname.Rows.Count > 0)
                    {
                        Clientname = dtclientname.Rows[0]["ClientName"].ToString();
                    }

                    PdfPCell cell2 = new PdfPCell(new Phrase(Clientname, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell2.Colspan = 6;
                    cell2.Border = 0;
                    Maintable.AddCell(cell2);



                    PdfPCell cell6 = new PdfPCell(new Phrase("Slno", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell6.Colspan = 1;
                    cell6.BorderWidthTop = 0.2f;
                    cell6.BorderWidthBottom = 0.2f;
                    cell6.BorderWidthLeft = 0;
                    cell6.BorderWidthRight = 0;
                    Maintable.AddCell(cell6);

                    PdfPCell cell7 = new PdfPCell(new Phrase("Emp No\n" + "Employee Name", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell7.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell7.Colspan = 2;
                    cell7.BorderWidthTop = 0.2f;
                    cell7.BorderWidthBottom = 0.2f;
                    cell7.BorderWidthLeft = 0;
                    cell7.BorderWidthRight = 0;
                    Maintable.AddCell(cell7);

                    PdfPCell cell8 = new PdfPCell(new Phrase("DUTY\n" + "OT DUTY", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell8.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell8.Colspan = 1;
                    cell8.BorderWidthTop = 0.2f;
                    cell8.BorderWidthBottom = 0.2f;
                    cell8.BorderWidthLeft = 0;
                    cell8.BorderWidthRight = 0;
                    Maintable.AddCell(cell8);

                    PdfPCell cell9 = new PdfPCell(new Phrase("Gross", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell9.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell9.Colspan = 1;
                    cell9.BorderWidthTop = 0.2f;
                    cell9.BorderWidthBottom = 0.2f;
                    cell9.BorderWidthLeft = 0;
                    cell9.BorderWidthRight = 0;
                    Maintable.AddCell(cell9);

                    PdfPCell cell11 = new PdfPCell(new Phrase("PF\n" + "PT", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell11.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell11.Colspan = 1;
                    cell11.BorderWidthTop = 0.2f;
                    cell11.BorderWidthBottom = 0.2f;
                    cell11.BorderWidthLeft = 0;
                    cell11.BorderWidthRight = 0;
                    Maintable.AddCell(cell11);

                    PdfPCell cell12 = new PdfPCell(new Phrase(" ESI\n" + "LWF\n" + "ADM", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell12.Colspan = 1;
                    cell12.BorderWidthTop = 0.2f;
                    cell12.BorderWidthBottom = 0.2f;
                    cell12.BorderWidthLeft = 0;
                    cell12.BorderWidthRight = 0;
                    Maintable.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase("Adv\n" + "Uni\n" + "Misd", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell13.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell13.Colspan = 1;
                    cell13.BorderWidthTop = 0.2f;
                    cell13.BorderWidthBottom = 0.2f;
                    cell13.BorderWidthLeft = 0;
                    cell13.BorderWidthRight = 0;
                    Maintable.AddCell(cell13);

                    PdfPCell cellFine = new PdfPCell(new Phrase("Fine\n" + "Misc", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellFine.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellFine.Colspan = 1;
                    cellFine.BorderWidthTop = 0.2f;
                    cellFine.BorderWidthBottom = 0.2f;
                    cellFine.BorderWidthLeft = 0;
                    cellFine.BorderWidthRight = 0;
                    Maintable.AddCell(cellFine);

                    PdfPCell cellDed = new PdfPCell(new Phrase("TotalDeduction", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellDed.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellDed.Colspan = 1;
                    cellDed.BorderWidthTop = 0.2f;
                    cellDed.BorderWidthBottom = 0.2f;
                    cellDed.BorderWidthLeft = 0;
                    cellDed.BorderWidthRight = 0;
                    Maintable.AddCell(cellDed);

                    PdfPCell cellnetpay = new PdfPCell(new Phrase("NetPay\nOthers", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellnetpay.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellnetpay.Colspan = 1;
                    cellnetpay.BorderWidthTop = 0.2f;
                    cellnetpay.BorderWidthBottom = 0.2f;
                    cellnetpay.BorderWidthLeft = 0;
                    cellnetpay.BorderWidthRight = 0;
                    Maintable.AddCell(cellnetpay);

                    PdfPCell cellOtpay = new PdfPCell(new Phrase("OT PAY\n" + "ESI\n" + "OT NET", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellOtpay.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellOtpay.Colspan = 1;
                    cellOtpay.BorderWidthTop = 0.2f;
                    cellOtpay.BorderWidthBottom = 0.2f;
                    cellOtpay.BorderWidthLeft = 0;
                    cellOtpay.BorderWidthRight = 0;
                    Maintable.AddCell(cellOtpay);

                    PdfPCell cellGrand = new PdfPCell(new Phrase("GRAND TOTAL", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellGrand.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellGrand.Colspan = 1;
                    cellGrand.BorderWidthTop = 0.2f;
                    cellGrand.BorderWidthBottom = 0.2f;
                    cellGrand.BorderWidthLeft = 0;
                    cellGrand.BorderWidthRight = 0;
                    Maintable.AddCell(cellGrand);

                    PdfPCell cellSign = new PdfPCell(new Phrase("SIGN", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellSign.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellSign.Colspan = 1;
                    cellSign.BorderWidthTop = 0.2f;
                    cellSign.BorderWidthBottom = 0.2f;
                    cellSign.BorderWidthLeft = 0;
                    cellSign.BorderWidthRight = 0;
                    Maintable.AddCell(cellSign);

                    int pagecount = 0;
                    string Sno = "";
                    string ClientID = "";
                    //string Clientname = "";
                    string Empid = "";
                    string Empname = "";
                    string Desgn = "";
                    float Noofduties = 0;
                    float Ots = 0;
                    float Tot = 0;
                    float Wo = 0;
                    float Leave = 0;
                    float PH = 0;
                    float CanteenAdv = 0;
                    float Uniform = 0;
                    float Penalty = 0;
                    float ATMDed = 0;
                    float TotalDeds = 0;
                    float Gross = 0;
                    float PF = 0;
                    float ESI = 0;
                    float ProfTax = 0;
                    float OWF = 0;
                    float AdminCharges = 0;
                    float Misd = 0;
                    float TotalDeductions = 0;
                    float Netpay = 0;
                    float OTAmt = 0;
                    float ESIONOT = 0;
                    float OTNet = 0;
                    float ActualAmount = 0;
                    string BankName = "";
                    string AcNo = "";
                    string RefNo = "";
                    string IFSCCode = "";
                    float Others = 0;
                    string Remarks = "";

                    float TotalNoofduties = 0;
                    float TotalOts = 0;
                    float TotalTot = 0;
                    float TotalWo = 0;
                    float TotalLeave = 0;
                    float TotalPH = 0;
                    float TotalCanteenAdv = 0;
                    float TotalUniform = 0;
                    float TotalPenalty = 0;
                    float TotalATMDed = 0;
                    float TotalTotalDeds = 0;
                    float TotalGross = 0;
                    float TotalPF = 0;
                    float TotalESI = 0;
                    float TotalProfTax = 0;
                    float TotalOWF = 0;
                    float TotalAdminCharges = 0;
                    float TotalMisd = 0;
                    float TotalTotalDeductions = 0;
                    float TotalNetpay = 0;
                    float TotalOTAmt = 0;
                    float TotalESIONOT = 0;
                    float TotalOTNet = 0;
                    float TotalActualAmount = 0;
                    float TotalOthers = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dtgroup.Rows[j]["Clientid"].ToString() == dt.Rows[i]["Clientid"].ToString())
                        {

                            Sno = dt.Rows[i]["Sno"].ToString();
                            ClientID = dt.Rows[i]["Clientid"].ToString();
                            // Clientname = dt.Rows[i]["Client Name"].ToString();
                            Empid = dt.Rows[i]["EmpId"].ToString();
                            Empname = dt.Rows[i]["Name"].ToString();
                            Noofduties = Convert.ToSingle(dt.Rows[i]["NoOfDuties"].ToString());
                            Ots = Convert.ToSingle(dt.Rows[i]["ots"].ToString());
                            CanteenAdv = Convert.ToSingle(dt.Rows[i]["CanteenAdv"].ToString());
                            Uniform = Convert.ToSingle(dt.Rows[i]["UniformDed"].ToString());
                            Penalty = Convert.ToSingle(dt.Rows[i]["penalty"].ToString());
                            ATMDed = Convert.ToSingle(dt.Rows[i]["ATMDed"].ToString());
                            Gross = Convert.ToSingle(dt.Rows[i]["Gross"].ToString());
                            PF = Convert.ToSingle(dt.Rows[i]["PF"].ToString());
                            ESI = Convert.ToSingle(dt.Rows[i]["ESI"].ToString());
                            ProfTax = Convert.ToSingle(dt.Rows[i]["ProfTax"].ToString());
                            OWF = Convert.ToSingle(dt.Rows[i]["OWF"].ToString());
                            AdminCharges = Convert.ToSingle(dt.Rows[i]["AdminCharges"].ToString());
                            Misd = Convert.ToSingle(dt.Rows[i]["Misd"].ToString());
                            TotalDeductions = Convert.ToSingle(dt.Rows[i]["TotalDeductions"].ToString());
                            Netpay = Convert.ToSingle(dt.Rows[i]["Netpay"].ToString());
                            OTAmt = Convert.ToSingle(dt.Rows[i]["OTAmt"].ToString());
                            ESIONOT = Convert.ToSingle(dt.Rows[i]["ESIONOT"].ToString());
                            OTNet = Convert.ToSingle(dt.Rows[i]["OTNet"].ToString());
                            Others = Convert.ToSingle(dt.Rows[i]["OthersAllw"].ToString());
                            ActualAmount = Convert.ToSingle(dt.Rows[i]["ActualAmount"].ToString());
                            BankName = dt.Rows[i]["Bank"].ToString();
                            AcNo = dt.Rows[i]["ACNo"].ToString();
                            RefNo = dt.Rows[i]["RefNo"].ToString();
                            IFSCCode = dt.Rows[i]["IFSCCode"].ToString();
                            Remarks = dt.Rows[i]["Remarks"].ToString();

                            PdfPCell cellsno = new PdfPCell(new Phrase(Sno, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellsno.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellsno.Colspan = 1;
                            cellsno.BorderWidthTop = 0.2f;
                            cellsno.BorderWidthBottom = 0;
                            cellsno.BorderWidthLeft = 0;
                            cellsno.BorderWidthRight = 0;
                            Maintable.AddCell(cellsno);

                            PdfPCell cellempname = new PdfPCell(new Phrase(Empid + "\n" + Empname, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellempname.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellempname.Colspan = 2;
                            cellempname.BorderWidthTop = 0.2f;
                            cellempname.BorderWidthBottom = 0;
                            cellempname.BorderWidthLeft = 0;
                            cellempname.BorderWidthRight = 0;
                            Maintable.AddCell(cellempname);

                            PdfPCell cellduty = new PdfPCell(new Phrase(Noofduties.ToString() + "\n" + Ots.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellduty.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellduty.Colspan = 1;
                            cellduty.BorderWidthTop = 0.2f;
                            cellduty.BorderWidthBottom = 0;
                            cellduty.BorderWidthLeft = 0;
                            cellduty.BorderWidthRight = 0;
                            Maintable.AddCell(cellduty);
                            TotalNoofduties += Noofduties;
                            TotalOts += Ots;

                            PdfPCell cellGross = new PdfPCell(new Phrase(Gross.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellGross.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellGross.Colspan = 1;
                            cellGross.BorderWidthTop = 0.2f;
                            cellGross.BorderWidthBottom = 0;
                            cellGross.BorderWidthLeft = 0;
                            cellGross.BorderWidthRight = 0;
                            Maintable.AddCell(cellGross);
                            TotalGross += Gross;

                            PdfPCell cellPF = new PdfPCell(new Phrase(PF.ToString() + "\n" + ProfTax.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellPF.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellPF.Colspan = 1;
                            cellPF.BorderWidthTop = 0.2f;
                            cellPF.BorderWidthBottom = 0;
                            cellPF.BorderWidthLeft = 0;
                            cellPF.BorderWidthRight = 0;
                            Maintable.AddCell(cellPF);
                            TotalPF += PF;
                            TotalProfTax += ProfTax;

                            PdfPCell cellESI = new PdfPCell(new Phrase(ESI + "\n" + OWF + "\n" + AdminCharges, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellESI.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellESI.Colspan = 1;
                            cellESI.BorderWidthTop = 0.2f;
                            cellESI.BorderWidthBottom = 0;
                            cellESI.BorderWidthLeft = 0;
                            cellESI.BorderWidthRight = 0;
                            Maintable.AddCell(cellESI);
                            TotalESI += ESI;
                            TotalOWF += OWF;
                            TotalAdminCharges += AdminCharges;

                            PdfPCell celladv = new PdfPCell(new Phrase(CanteenAdv + "\n" + Uniform + "\n" + Misd, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            celladv.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            celladv.Colspan = 1;
                            celladv.BorderWidthTop = 0.2f;
                            celladv.BorderWidthBottom = 0;
                            celladv.BorderWidthLeft = 0;
                            celladv.BorderWidthRight = 0;
                            Maintable.AddCell(celladv);
                            TotalCanteenAdv += CanteenAdv;
                            TotalUniform += Uniform;
                            TotalMisd += Misd;

                            PdfPCell cellFine2 = new PdfPCell(new Phrase(Penalty + "\n" + ATMDed, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellFine2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellFine2.Colspan = 1;
                            cellFine2.BorderWidthTop = 0.2f;
                            cellFine2.BorderWidthBottom = 0;
                            cellFine2.BorderWidthLeft = 0;
                            cellFine2.BorderWidthRight = 0;
                            Maintable.AddCell(cellFine2);
                            TotalPenalty += Penalty;
                            TotalATMDed += ATMDed;

                            PdfPCell cellDed2 = new PdfPCell(new Phrase(TotalDeductions.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellDed2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellDed2.Colspan = 1;
                            cellDed2.BorderWidthTop = 0.2f;
                            cellDed2.BorderWidthBottom = 0;
                            cellDed2.BorderWidthLeft = 0;
                            cellDed2.BorderWidthRight = 0;
                            Maintable.AddCell(cellDed2);
                            TotalTotalDeductions += TotalDeductions;

                            PdfPCell cellnetpay2 = new PdfPCell(new Phrase(Netpay.ToString() + "\n" + Others.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellnetpay2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellnetpay2.Colspan = 1;
                            cellnetpay2.BorderWidthTop = 0.2f;
                            cellnetpay2.BorderWidthBottom = 0;
                            cellnetpay2.BorderWidthLeft = 0;
                            cellnetpay2.BorderWidthRight = 0;
                            Maintable.AddCell(cellnetpay2);
                            TotalNetpay += Netpay;
                            TotalOthers += Others;

                            PdfPCell cellOtpay2 = new PdfPCell(new Phrase(OTAmt + "\n" + ESIONOT + "\n" + OTNet, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellOtpay2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellOtpay2.Colspan = 1;
                            cellOtpay2.BorderWidthTop = 0.2f;
                            cellOtpay2.BorderWidthBottom = 0;
                            cellOtpay2.BorderWidthLeft = 0;
                            cellOtpay2.BorderWidthRight = 0;
                            Maintable.AddCell(cellOtpay2);
                            TotalOTAmt += OTAmt;
                            TotalESIONOT += ESIONOT;
                            TotalOTNet += OTNet;

                            PdfPCell cellGrand2 = new PdfPCell(new Phrase(ActualAmount.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellGrand2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellGrand2.Colspan = 1;
                            cellGrand2.BorderWidthTop = 0.2f;
                            cellGrand2.BorderWidthBottom = 0;
                            cellGrand2.BorderWidthLeft = 0;
                            cellGrand2.BorderWidthRight = 0;
                            Maintable.AddCell(cellGrand2);
                            TotalActualAmount += ActualAmount;

                            PdfPCell cellSign2 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellSign2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cellSign2.Colspan = 1;
                            cellSign2.BorderWidthTop = 0.2f;
                            cellSign2.BorderWidthBottom = 0;
                            cellSign2.BorderWidthLeft = 0;
                            cellSign2.BorderWidthRight = 0;
                            Maintable.AddCell(cellSign2);

                            PdfPCell cellSno2 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellSno2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellSno2.Colspan = 1;
                            cellSno2.BorderWidthTop = 0;
                            cellSno2.BorderWidthBottom = 0.2f;
                            cellSno2.BorderWidthLeft = 0;
                            cellSno2.BorderWidthRight = 0;
                            Maintable.AddCell(cellSno2);

                            PdfPCell cellBank = new PdfPCell(new Phrase("Bank : " + BankName, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellBank.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellBank.Colspan = 2;
                            cellBank.BorderWidthTop = 0;
                            cellBank.BorderWidthBottom = 0.2f;
                            cellBank.BorderWidthLeft = 0;
                            cellBank.BorderWidthRight = 0;
                            Maintable.AddCell(cellBank);

                            PdfPCell cellacno = new PdfPCell(new Phrase("AC No " + AcNo, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellacno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellacno.Colspan = 4;
                            cellacno.BorderWidthTop = 0;
                            cellacno.BorderWidthBottom = 0.2f;
                            cellacno.BorderWidthLeft = 0;
                            cellacno.BorderWidthRight = 0;
                            Maintable.AddCell(cellacno);

                            PdfPCell cellrefno = new PdfPCell(new Phrase("Refno " + RefNo, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellrefno.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellrefno.Colspan = 3;
                            cellrefno.BorderWidthTop = 0;
                            cellrefno.BorderWidthBottom = 0.2f;
                            cellrefno.BorderWidthLeft = 0;
                            cellrefno.BorderWidthRight = 0;
                            Maintable.AddCell(cellrefno);

                            PdfPCell cellifsccode = new PdfPCell(new Phrase("IFSC " + IFSCCode, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellifsccode.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellifsccode.Colspan = 2;
                            cellifsccode.BorderWidthTop = 0;
                            cellifsccode.BorderWidthBottom = 0.2f;
                            cellifsccode.BorderWidthLeft = 0;
                            cellifsccode.BorderWidthRight = 0;
                            Maintable.AddCell(cellifsccode);

                            PdfPCell cellspace = new PdfPCell(new Phrase(Remarks, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellspace.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            cellspace.Colspan = 2;
                            cellspace.BorderWidthTop = 0;
                            cellspace.BorderWidthBottom = 0.2f;
                            cellspace.BorderWidthLeft = 0;
                            cellspace.BorderWidthRight = 0;
                            Maintable.AddCell(cellspace);
                        }

                    }

                    PdfPCell cellsnototal = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellsnototal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellsnototal.Colspan = 1;
                    cellsnototal.BorderWidthTop = 0.2f;
                    cellsnototal.BorderWidthBottom = 0.2f;
                    cellsnototal.BorderWidthLeft = 0;
                    cellsnototal.BorderWidthRight = 0;
                    Maintable.AddCell(cellsnototal);

                    PdfPCell cellempnametotal = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellempnametotal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellempnametotal.Colspan = 2;
                    cellempnametotal.BorderWidthTop = 0.2f;
                    cellempnametotal.BorderWidthBottom = 0.2f;
                    cellempnametotal.BorderWidthLeft = 0;
                    cellempnametotal.BorderWidthRight = 0;
                    Maintable.AddCell(cellempnametotal);

                    PdfPCell celldutytotal = new PdfPCell(new Phrase(TotalNoofduties.ToString() + "\n" + TotalOts.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    celldutytotal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    celldutytotal.Colspan = 1;
                    celldutytotal.BorderWidthTop = 0.2f;
                    celldutytotal.BorderWidthBottom = 0.2f;
                    celldutytotal.BorderWidthLeft = 0;
                    celldutytotal.BorderWidthRight = 0;
                    Maintable.AddCell(celldutytotal);

                    PdfPCell cellGrosstotal = new PdfPCell(new Phrase(TotalGross.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellGrosstotal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellGrosstotal.Colspan = 1;
                    cellGrosstotal.BorderWidthTop = 0.2f;
                    cellGrosstotal.BorderWidthBottom = 0.2f;
                    cellGrosstotal.BorderWidthLeft = 0;
                    cellGrosstotal.BorderWidthRight = 0;
                    Maintable.AddCell(cellGrosstotal);

                    PdfPCell cellPFtotal = new PdfPCell(new Phrase(TotalPF.ToString() + "\n" + TotalProfTax.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellPFtotal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellPFtotal.Colspan = 1;
                    cellPFtotal.BorderWidthTop = 0.2f;
                    cellPFtotal.BorderWidthBottom = 0.2f;
                    cellPFtotal.BorderWidthLeft = 0;
                    cellPFtotal.BorderWidthRight = 0;
                    Maintable.AddCell(cellPFtotal);

                    PdfPCell cellESItotal = new PdfPCell(new Phrase(TotalESI + "\n" + TotalOWF + "\n" + TotalAdminCharges, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellESItotal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellESItotal.Colspan = 1;
                    cellESItotal.BorderWidthTop = 0.2f;
                    cellESItotal.BorderWidthBottom = 0.2f;
                    cellESItotal.BorderWidthLeft = 0;
                    cellESItotal.BorderWidthRight = 0;
                    Maintable.AddCell(cellESItotal);

                    PdfPCell celladvtotal = new PdfPCell(new Phrase(TotalCanteenAdv + "\n" + TotalUniform + "\n" + TotalMisd, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    celladvtotal.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    celladvtotal.Colspan = 1;
                    celladvtotal.BorderWidthTop = 0.2f;
                    celladvtotal.BorderWidthBottom = 0.2f;
                    celladvtotal.BorderWidthLeft = 0;
                    celladvtotal.BorderWidthRight = 0;
                    Maintable.AddCell(celladvtotal);

                    PdfPCell cellFine2total = new PdfPCell(new Phrase(TotalPenalty + "\n" + TotalATMDed, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellFine2total.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellFine2total.Colspan = 1;
                    cellFine2total.BorderWidthTop = 0.2f;
                    cellFine2total.BorderWidthBottom = 0.2f;
                    cellFine2total.BorderWidthLeft = 0;
                    cellFine2total.BorderWidthRight = 0;
                    Maintable.AddCell(cellFine2total);

                    PdfPCell cellDed2total = new PdfPCell(new Phrase(TotalTotalDeductions.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellDed2total.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellDed2total.Colspan = 1;
                    cellDed2total.BorderWidthTop = 0.2f;
                    cellDed2total.BorderWidthBottom = 0.2f;
                    cellDed2total.BorderWidthLeft = 0;
                    cellDed2total.BorderWidthRight = 0;
                    Maintable.AddCell(cellDed2total);


                    PdfPCell cellnetpay2total = new PdfPCell(new Phrase(TotalNetpay.ToString() + "\n" + TotalOthers.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellnetpay2total.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellnetpay2total.Colspan = 1;
                    cellnetpay2total.BorderWidthTop = 0.2f;
                    cellnetpay2total.BorderWidthBottom = 0.2f;
                    cellnetpay2total.BorderWidthLeft = 0;
                    cellnetpay2total.BorderWidthRight = 0;
                    Maintable.AddCell(cellnetpay2total);


                    PdfPCell cellOtpay2total = new PdfPCell(new Phrase(TotalOTAmt + "\n" + TotalESIONOT + "\n" + TotalOTNet, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellOtpay2total.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellOtpay2total.Colspan = 1;
                    cellOtpay2total.BorderWidthTop = 0.2f;
                    cellOtpay2total.BorderWidthBottom = 0.2f;
                    cellOtpay2total.BorderWidthLeft = 0;
                    cellOtpay2total.BorderWidthRight = 0;
                    Maintable.AddCell(cellOtpay2total);

                    PdfPCell cellGrand2total = new PdfPCell(new Phrase(TotalActualAmount.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellGrand2total.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellGrand2total.Colspan = 1;
                    cellGrand2total.BorderWidthTop = 0.2f;
                    cellGrand2total.BorderWidthBottom = 0.2f;
                    cellGrand2total.BorderWidthLeft = 0;
                    cellGrand2total.BorderWidthRight = 0;
                    Maintable.AddCell(cellGrand2total);


                    PdfPCell cellSign2total = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellSign2total.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cellSign2total.Colspan = 1;
                    cellSign2total.BorderWidthTop = 0.2f;
                    cellSign2total.BorderWidthBottom = 0.2f;
                    cellSign2total.BorderWidthLeft = 0;
                    cellSign2total.BorderWidthRight = 0;
                    Maintable.AddCell(cellSign2total);

                    document.Add(Maintable);


                }


                string filename = "WageSheetSummary" + ".pdf";

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
    }

    public class EmpAttendance
    {
        public bool NewAdd { get; set; }
        public string EmpId { get; set; }
        public string EmpDesg { get; set; }
        public string ClientId { get; set; }
        public string JoiningDate { get; set; }
        public string RelievingDate { get; set; }
        public bool PF { get; set; }
        public bool PT { get; set; }
        public bool ESI { get; set; }
        public int TransferType { get; set; }
        public int MonthIndex { get; set; }
        public decimal NOD { get; set; }
        public decimal OT { get; set; }
        public int OTtype { get; set; }
        public decimal WO { get; set; }
        public decimal NHS { get; set; }
        public decimal Nposts { get; set; }
        public decimal CanAdv { get; set; }
        public decimal Penality { get; set; }
        public decimal Incentives { get; set; }
        public decimal Arrears { get; set; }

    }
}