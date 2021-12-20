using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using KLTS.Data;
using System.Web.Script.Serialization;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Web.Script.Services;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using Kuduma.Portal.DAL;


/// <summary>
/// Summary description for FameService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class FameService : System.Web.Services.WebService
{

    private static string EmpIDPrefix = string.Empty;

    private static string CmpIDPrefix = string.Empty;

    private static string BranchID = string.Empty;

    private static DataTable _dtEmployees = new DataTable();

    AppConfiguration Config = new AppConfiguration();

    private const string _attendanceQuery = @"select ROW_NUMBER() over(order by ea.EmpId) as sno,EA.EmpId,ED.OLDEMPID,
			                   ISNULL(EmpFName,'')+' '+ISNULL(EmpMName,'')+' '+ISNULL(EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
			                   isnull(EA.NoOfDuties,0) as NOD ,
			                   isnull(EA.Ot,0) as OT ,
			                   isnull(EA.WO,0) as WO,
			                   isnull(EA.NHS,0) as NHS,
			                   isnull(EA.PL,0) as NPots,
                               isnull(EA.OTHours,0) as OTHRS,
			                   isnull(EA.CanteenAdv,0) as CanAdv,
			                   isnull(EA.Penalty,0) as Pen,
                               isnull(EA.UniformDed,0) as UNIDED,
                               isnull(EA.OtherDed,0) as ATMDED,
			                   isnull(EA.Incentivs,0) as Inctvs,
                               isnull(EA.Arrears,0) as Arrears,isnull(cast(EA.stoppayment as bit),1) as stoppayment 
		                from EmpAttendance EA join EmpDetails ED on Ed.EmpId=EA.EmpId join Designations D on D.DesignId=EA.Design 
		                and EA.ClientId='##CLIENTID##' and EA.Month=##MONTH## and EA.ContractId='##CONTRACTID##' and ( (ea.NoOfDuties+EA.Ot+EA.WO+EA.NHS+ EA.Npots)>0  or ed.empstatus=1) ";

    string _clientempsquery = @"select ROW_NUMBER() over(order by ep.EmpId) as sno,ep.EmpId,ED.OLDEMPID,
			                   ISNULL(ed.EmpFName,'')+' '+ISNULL(ed.EmpMName,'')+' '+ISNULL(ed.EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
                               ep.relievemonth,
			                   0 as NOD,
			                   0 as OT,
			                   0 as WO,
			                   0 as NHS,
			                   0 as NPots,
                               0 as OTHRS,
			                   0 as CanAdv,
			                   0 as Pen,
                               0 as UNIDED,
                               0 as ATMDED,
			                   0 as Inctvs,
                               0 as Arrears,
                               cast(0 as bit) as stoppayment 
		                from EmpPostingOrder ep
		                inner join EmpDetails ed on ep.EmpId = ed.EmpId
		                inner join Designations d on ep.Desgn = d.DesignId
		                where ToUnitId = '##CLIENTID##' and ed.empstatus=1 and ep.empid in (select empid from empattendance where month='##PREVMONTH##') 
                        union 
                        select ROW_NUMBER() over(order by ep.EmpId) as sno,ep.EmpId,ED.OLDEMPID,
			                   ISNULL(ed.EmpFName,'')+' '+ISNULL(ed.EmpMName,'')+' '+ISNULL(ed.EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
                               ep.relievemonth,
			                   0 as NOD,
			                   0 as OT,
			                   0 as WO,
			                   0 as NHS,
			                   0 as NPots,
                               0 as OTHRS,
			                   0 as CanAdv,
			                   0 as Pen,
                               0 as UNIDED,
                               0 as ATMDED,
			                   0 as Inctvs,
                               0 as Arrears,
                               cast(0 as bit) as  stoppayment
		                from EmpPostingOrder ep
		                inner join EmpDetails ed on ep.EmpId = ed.EmpId
		                inner join Designations d on ep.Desgn = d.DesignId
		                where ToUnitId = '##CLIENTID##' and relievemonth is null and (postedmonth=##MONTHNEW## or postedmonth is null)  and ed.empstatus=1 and ep.EmpId not in(select empid from EmpAttendance where MONTH='##PREVMONTH##' )";


    private const string _attendanceSummaryquery = @"select d.Design DesName,
	                                                           isnull(cast(sum(ea.NoOfDuties)as nvarchar),0) NODTotal,
	                                                           isnull(cast(sum(ea.OT)as nvarchar),0) OTTotal,
	                                                           isnull(cast(sum(ea.WO)as nvarchar),0) WOTotal,
	                                                           isnull(cast(sum(ea.NHS)as nvarchar),0) NHSTotal,
	                                                           isnull(cast(sum(ea.PL)as nvarchar),0) NpotsTotal,
                                                               isnull(cast(sum(ea.OTHours)as nvarchar),0) OTHRSTotal,
	                                                           isnull(cast(sum(ea.Penalty)as nvarchar),0) PenTotal,
	                                                           isnull(cast(sum(ea.Incentivs)as nvarchar),0) InctvsTotal,
                                                               isnull(cast(sum(ea.UniformDed)as nvarchar),0) UNIDEDTotal, 
                                                               isnull(cast(sum(ea.OtherDed)as nvarchar),0) ATMDEDTotal,
	                                                           isnull(cast(sum(ea.CanteenAdv)as nvarchar),0) CanAdvTotal,
	                                                           isnull(cast(sum(ea.Arrears)as nvarchar),0) ArrearsTotal
                                                        from EmpAttendance ea 
                                                        inner join Designations d on d.DesignId = ea.Design
                                                        where ea.ClientId = '##CLIENTID##' and ea.[MONTH]= ##MONTH##
                                                        group by ea.Design,d.Design";


    private const string _attendanceDuplicates = @"select ea.ClientID,Ed.EmpFName,ED.OldEmpid,d.Design DesName,
	                                                           isnull(cast((ea.NoOfDuties)as nvarchar),0) NODTotal,
	                                                           isnull(cast((ea.OT)as nvarchar),0) OTTotal,
	                                                           isnull(cast((ea.WO)as nvarchar),0) WOTotal,
	                                                           isnull(cast((ea.NHS)as nvarchar),0) NHSTotal,
	                                                           isnull(cast((ea.PL)as nvarchar),0) NpotsTotal
                                                        from EmpAttendance ea 
                                                        inner join Designations d on d.DesignId = ea.Design
                                                        inner join Empdetails Ed on Ed.EmpID = ea.EmpID
                                                        where Ed.OldEmpid = '##EMPID##' and ea.[MONTH]= ##MONTH##";


    public FameService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    public DataTable EmployeesDataTable
    {
        get
        {
            var EmpIDPrefix = string.Empty;
            var BranchID = string.Empty;

            //if (_dtEmployees.Rows.Count < 1)
            {
                EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();
                BranchID = HttpContext.Current.Session["BranchID"].ToString();

                DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

                var dtEmployees = GlobalData.Instance.LoadEmpNames(EmpIDPrefix, dtBranch);
                _dtEmployees = dtEmployees;
            }
            return _dtEmployees;
        }
    }


    public void UpdateEmpDataTable()
    {
        var EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();
        var BranchID = HttpContext.Current.Session["BranchID"].ToString();
        DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
        _dtEmployees = GlobalData.Instance.LoadEmpNames(EmpIDPrefix, dtBranch);
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetClientsData()
    {
        BranchID = HttpContext.Current.Session["BranchID"].ToString();
        Context.Response.Clear();
        var result = "";
        string query = "select clientid,clientname,clientphonenumbers,ourcontactpersonid from clients where ClientId like '" + CmpIDPrefix + "%' and ClientName not LIKE '%/%' and ClientStatus=1 and BranchID in (" + BranchID + ")  Order By  Clientname";
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

        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetAttendanceDuplicates(string empid, string month, bool Chk)
    {
        Context.Response.Clear();
        var result = string.Empty;
        var resultobj = string.Empty;
        DateTime LastDate = DateTime.Now;
        string date = "";
        var Month = 0;
        try
        {
            // var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            if (Chk == false)
            {
                Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));

            }

            else
            {
                date = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb")).ToString();
                string month1 = DateTime.Parse(date).Month.ToString();
                string Year1 = DateTime.Parse(date).Year.ToString();
                string olddate = month1 + Year1.Substring(2, 2);
                Month = Convert.ToInt32(olddate);
            }
            var sx = _attendanceDuplicates.Replace("##MONTH##", Month.ToString())
                                     .Replace("##EMPID##", empid);
            var attData = SqlHelper.Instance.GetTableByQuery(sx);
            if (attData.Rows.Count > 0)
            {
                var obj = (from row in attData.AsEnumerable()
                           select new
                           {
                               ClientID = row.Field<string>("ClientID"),
                               EmpFName = row.Field<string>("EmpFName"),
                               OldEmpid = row.Field<string>("OldEmpid"),
                               DesgName = row.Field<string>("DesName"),
                               NODTotal = row.Field<string>("NODTotal"),
                               OTTotal = row.Field<string>("OTTotal"),
                               WOTotal = row.Field<string>("WOTotal"),
                               NHSTotal = row.Field<string>("NHSTotal"),
                               NpotsTotal = row.Field<string>("NpotsTotal")

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
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void BindDesgnTempAtt(string clientId)
    {
        Context.Response.Clear();
        var result = "";
        var resultobj = string.Empty;

        var Binddesgnquery = "select distinct D.DesignId,Design from contractdetailssw cdsw inner join Designations d on d.DesignId = cdsw.Designations where ClientId = '" + clientId + "' and D.DesignId <> 0   Order by Design";
        var dtdesgn = Config.ExecuteReaderWithQueryAsync(Binddesgnquery).Result;
        if (dtdesgn.Rows.Count > 0)
        {
            var obj = (from row in dtdesgn.AsEnumerable()
                       select new
                       {
                           DesignId = row.Field<int>("DesignId"),
                           Design = row.Field<string>("Design")
                       }).ToList();
            result = new JavaScriptSerializer().Serialize(obj);

        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void BindDesgnTempAttendance(string clientId, string month)
    {
        Context.Response.Clear();
        var result = "";
        var resultobj = string.Empty;
        var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));

        var monthval = "";
        var yearval = "";

        if (Month.ToString().Length == 3)
        {
            monthval = Month.ToString().Substring(0, 1);
            yearval = "20" + Month.ToString().Substring(1, 2);
        }
        else
        {
            monthval = Month.ToString().Substring(0, 2);
            yearval = "20" + Month.ToString().Substring(2, 2);
        }

        int Selectdays = System.DateTime.DaysInMonth(int.Parse(yearval), int.Parse(monthval));
        string date = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();
        var LastDate = DateTime.Parse(date, CultureInfo.GetCultureInfo("en-gb"));
        var Wagetype = "0";
        var strquery = "select contractid,Wagetype from contracts where clientid= '" + clientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
        var contractdata = Config.ExecuteReaderWithQueryAsync(strquery).Result;
        var contractId = string.Empty;
        if (contractdata.Rows.Count > 0)
        {
            contractId = contractdata.Rows[0]["contractid"].ToString();
            Wagetype = contractdata.Rows[0]["Wagetype"].ToString();
        }
        if (string.IsNullOrEmpty(contractId))
        {
            result = "fail";
            resultobj = "Contract not available for this month.";
        }
        else
        {
            var Binddesgnquery = "";
            if (Wagetype == "2")
            {
                Binddesgnquery = "select D.DesignId,Design from contractdetailssw cdsw inner join Designations d on d.DesignId = cdsw.Designations where ClientId = '" + clientId + "' and contractid='" + contractId + "' and D.DesignId <> 0   Order by Design";
            }
            else
            {
                Binddesgnquery = "select D.DesignId,Design from Designations D where D.DesignId <> 0   Order by Design";
            }
            var dtdesgn = Config.ExecuteReaderWithQueryAsync(Binddesgnquery).Result;
            if (dtdesgn.Rows.Count > 0)
            {
                var obj = (from row in dtdesgn.AsEnumerable()
                           select new
                           {
                               DesignId = row.Field<int>("DesignId"),
                               Design = row.Field<string>("Design")
                           }).ToList();
                result = new JavaScriptSerializer().Serialize(obj);

            }
        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetEmployessData(string strid, string month, bool Chk)
    {
        //strid = strid == "" ? "" : strid;
        Context.Response.Clear();
        var result = string.Empty;
        try
        {
            DateTime LastDate = DateTime.Now;

            if (Chk == false)
            {
                LastDate = Timings.Instance.GetLastDayForSelectedMonth(Convert.ToInt32(month));
            }
            else
            {
                LastDate = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb"));
            }

            string Fmonth = (LastDate).Month.ToString();
            string FYear = (LastDate).Year.ToString();

            string Date = "";
            if (Fmonth.Length == 1)
            {
                Date = FYear + "-0" + Fmonth + "-01";
            }
            else
            {
                Date = FYear + "-" + Fmonth + "-01";
            }

            var EmpIDPrefix = string.Empty;
            var BranchID = string.Empty;

            EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();
            BranchID = HttpContext.Current.Session["BranchID"].ToString();

            string Search = "GDXID";

            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dtEmployees = GlobalData.Instance.LoadAttendanceEmpNames(EmpIDPrefix, dtBranch, strid, Date, Search);


            if (dtEmployees.Rows.Count > 0)
            {
                var obj = (from row in dtEmployees.AsEnumerable()
                           select new
                           {
                               EmpId = row.Field<string>("Empid"),
                               EmpName = row.Field<string>("FullName").Trim(),
                               EmpDesg = row.Field<string>("Designation"),
                               empstatus = row.Field<bool>("empstatus"),
                               OLDEMPID = row.Field<string>("oldempid")
                           }).ToList();

                //obj = obj.Where(o => o.OLDEMPID.Contains(strid.Trim())).OrderBy(o => o.OLDEMPID).ToList();
                result = new JavaScriptSerializer().Serialize(obj);
            }
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetSofwareEmployessData(string strid, string month, bool Chk)
    {
        //strid = strid == "" ? "" : strid;
        Context.Response.Clear();
        var result = string.Empty;
        try
        {
            DateTime LastDate = DateTime.Now;

            if (Chk == false)
            {
                LastDate = Timings.Instance.GetLastDayForSelectedMonth(Convert.ToInt32(month));
            }
            else
            {
                LastDate = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb"));
            }

            string Fmonth = (LastDate).Month.ToString();
            string FYear = (LastDate).Year.ToString();

            string Date = "";
            if (Fmonth.Length == 1)
            {
                Date = FYear + "-0" + Fmonth + "-01";
            }
            else
            {
                Date = FYear + "-" + Fmonth + "-01";
            }

            var EmpIDPrefix = string.Empty;
            var BranchID = string.Empty;

            EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();
            BranchID = HttpContext.Current.Session["BranchID"].ToString();

            string Search = "SoftwareID";

            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dtEmployees = GlobalData.Instance.LoadAttendanceEmpNames(EmpIDPrefix, dtBranch, strid, Date, Search);


            if (dtEmployees.Rows.Count > 0)
            {
                var obj = (from row in dtEmployees.AsEnumerable()
                           select new
                           {
                               EmpId = row.Field<string>("Empid"),
                               EmpName = row.Field<string>("FullName").Trim(),
                               EmpDesg = row.Field<string>("Designation"),
                               empstatus = row.Field<bool>("empstatus"),
                               OLDEMPID = row.Field<string>("oldempid")
                           }).ToList();

                //obj = obj.Where(o => o.OLDEMPID.Contains(strid.Trim())).OrderBy(o => o.OLDEMPID).ToList();
                result = new JavaScriptSerializer().Serialize(obj);
            }
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetEmployessNameData(string strname, string month, bool Chk)
    {
        Context.Response.Clear();
        var result = string.Empty;
        try
        {

            DateTime LastDate = DateTime.Now;

            if (Chk == false)
            {
                LastDate = Timings.Instance.GetLastDayForSelectedMonth(Convert.ToInt32(month));
            }
            else
            {
                LastDate = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb"));
            }

            string Fmonth = (LastDate).Month.ToString();
            string FYear = (LastDate).Year.ToString();

            string Date = "";
            if (Fmonth.Length == 1)
            {
                Date = FYear + "/" + "0" + Fmonth + "/" + "01";
            }
            else
            {
                Date = FYear + "/" + Fmonth + "/" + "01";
            }

            var EmpIDPrefix = string.Empty;
            var BranchID = string.Empty;

            EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();
            BranchID = HttpContext.Current.Session["BranchID"].ToString();

            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            string Search = "GDXID";
            DataTable dtEmployees = GlobalData.Instance.LoadAttendanceEmpNames(EmpIDPrefix, dtBranch, strname, Date, Search);

            if (dtEmployees.Rows.Count > 0)
            {
                var obj = (from row in dtEmployees.AsEnumerable()
                           select new
                           {
                               EmpId = row.Field<string>("Empid"),
                               EmpName = row.Field<string>("FullName").Trim(),
                               EmpDesg = row.Field<string>("Designation"),
                               empstatus = row.Field<bool>("empstatus"),
                               OLDEMPID = row.Field<string>("oldempid")
                           }).ToList();

                //obj = obj.Where(o => o.EmpName.Contains(strname.Trim())).OrderBy(o => o.EmpName).ToList();
                result = new JavaScriptSerializer().Serialize(obj);
            }
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetAttendanceGrid(string clientId, string month, bool Chk)
    {
        Context.Response.Clear();
        var result = string.Empty;
        var resultobj = string.Empty;
        DateTime LastDate = DateTime.Now;
        string date = "";
        var Month = 0;
        try
        {
            //var LastDate = Timings.Instance.GetDateForSelectedMonth(Convert.ToInt32(month)).ToString("yyyy-MM-dd hh:mm:ss");
            //var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            if (Chk == false)
            {
                LastDate = Timings.Instance.GetLastDayForSelectedMonth(Convert.ToInt32(month));
            }
            else
            {
                LastDate = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb"));
            }

            //
            if (Chk == false)
            {
                Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));

            }
            else
            {
                date = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb")).ToString();

                string month1 = DateTime.Parse(date).Month.ToString();
                string Year1 = DateTime.Parse(date).Year.ToString();
                string olddate = month1 + Year1.Substring(2, 2);
                Month = Convert.ToInt32(olddate);
            }
            var monthval = "";
            var yearval = "";
            if (Month.ToString().Length == 3)
            {
                monthval = "0" + Month.ToString().Substring(0, 1);
                yearval = Month.ToString().Substring(1, 2);
            }
            else
            {
                monthval = Month.ToString().Substring(0, 2);
                yearval = Month.ToString().Substring(2, 2);
            }

            var PrevMonth = 0;
            if (Chk == false)
            {
                PrevMonth = Timings.Instance.GetIdForSelectedMonthPrevious(Convert.ToInt32(month));
            }
            else
            {
                LastDate = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb"));
                int OldMonth = 0;
                int OldYear = 0;
                string mon = "";
                string Year = "";

                OldMonth = LastDate.Month;
                if (OldMonth == 1)
                {
                    OldYear = (LastDate).AddMonths(-1).Year;
                }
                else
                {
                    OldYear = (LastDate).AddMonths(0).Year;
                }

                Year = (OldYear.ToString().Substring(2, 2));

                mon = ((LastDate).AddMonths(-1).Month).ToString() + Year;
                PrevMonth = Convert.ToInt32(mon);
            }
            var MonthNew = yearval + monthval.ToString();

            var strquery = "select contractid from contracts where clientid= '" + clientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
            var contractdata = SqlHelper.Instance.GetTableByQuery(strquery);
            var contractId = string.Empty;
            if (contractdata.Rows.Count > 0)
            {
                contractId = contractdata.Rows[0]["contractid"].ToString();
            }
            if (string.IsNullOrEmpty(contractId))
            {
                result = "fail";
                resultobj = "Contract not available for this month.";
            }
            else
            {

                List<EmpAttendanceGrid> empdata = new List<EmpAttendanceGrid>();
                var fromattendancequery = _attendanceQuery.Replace("##MONTH##", Month.ToString())
                                 .Replace("##CLIENTID##", clientId)
                                 .Replace("##CONTRACTID##", contractId);

                var attData = SqlHelper.Instance.GetTableByQuery(fromattendancequery);

                if (attData.Rows.Count > 0)
                {
                    var obj = (from row in attData.AsEnumerable()
                               select new EmpAttendanceGrid()
                               {

                                   EmpId = row.Field<string>("EmpId"),
                                   OLDEMPID = row.Field<string>("OLDEMPID"),
                                   EmpName = row.Field<string>("EmpName"),
                                   DesgId = row.Field<int>("DesId"),
                                   DesgName = row.Field<string>("DesName"),
                                   NoOfDuties = row.Field<float>("NOD"),
                                   OT = row.Field<float>("OT"),
                                   WO = row.Field<float>("WO"),
                                   NHS = row.Field<float>("NHS"),
                                   NPosts = row.Field<float>("NPots"),
                                   OTHRS = row.Field<float>("OTHRS"),
                                   CanteenAdv = row.Field<float>("CanAdv"),
                                   Penalty = row.Field<float>("Pen"),
                                   UNIDED = row.Field<float>("UNIDED"),
                                   ATMDED = row.Field<float>("ATMDED"),
                                   Incentivs = row.Field<float>("Inctvs"),
                                   Arrears = row.Field<float>("Arrears"),
                                   stoppayment = row.Field<bool>("stoppayment")
                               }).ToList();
                    empdata.AddRange(obj);
                }

                var frompostingorderquery = _clientempsquery.Replace("##CLIENTID##", clientId)
                                                            .Replace("##PREVMONTH##", PrevMonth.ToString())
                                                            .Replace("##MONTHNEW##", MonthNew.ToString());
                var postingData = SqlHelper.Instance.GetTableByQuery(frompostingorderquery);
                if (postingData.Rows.Count > 0)
                {
                    foreach (DataRow item in postingData.Rows)
                    {
                        var inserttrue = false;
                        if (string.IsNullOrEmpty(item["relievemonth"].ToString().Trim()))
                        {
                            inserttrue = true;
                        }
                        else if (item["relievemonth"].ToString() != Month.ToString())
                        {
                            //dont display if relieve month is less than or equal to selected month 
                            var rmonth = Timings.Instance.GetDateWithMonthString(item["relievemonth"].ToString());
                            var smonth = Timings.Instance.GetDateWithMonthString(Month.ToString());
                            inserttrue = (rmonth > smonth);
                        }
                        var empalreadyexists = empdata.Where(e => e.EmpId == item["EmpId"].ToString()
                                && e.EmpName == item["EmpName"].ToString()
                                && e.DesgId == int.Parse(item["DesId"].ToString())).FirstOrDefault();

                        if (inserttrue && empalreadyexists == null)
                        {
                            empdata.Add(new EmpAttendanceGrid
                            {
                                //sno = Int64.Parse(item["sno"].ToString()),
                                EmpId = item["EmpId"].ToString(),
                                OLDEMPID = item["OLDEMPID"].ToString(),
                                EmpName = item["EmpName"].ToString(),
                                DesgId = int.Parse(item["DesId"].ToString()),
                                DesgName = item["DesName"].ToString(),
                                NoOfDuties = float.Parse(item["NOD"].ToString()),
                                OT = float.Parse(item["OT"].ToString()),
                                WO = float.Parse(item["WO"].ToString()),
                                NHS = float.Parse(item["NHS"].ToString()),
                                NPosts = float.Parse(item["NPots"].ToString()),
                                OTHRS = float.Parse(item["OTHRS"].ToString()),
                                CanteenAdv = float.Parse(item["CanAdv"].ToString()),
                                Penalty = float.Parse(item["Pen"].ToString()),
                                UNIDED = float.Parse(item["UNIDED"].ToString()),
                                ATMDED = float.Parse(item["ATMDED"].ToString()),
                                Incentivs = float.Parse(item["Inctvs"].ToString()),
                                Arrears = float.Parse(item["Arrears"].ToString()),
                                stoppayment = bool.Parse(item["stoppayment"].ToString()),
                            });
                        }
                    }
                }

                if (empdata.Count > 0)
                {
                    resultobj = new JavaScriptSerializer().Serialize(empdata);
                    result = "success";
                }
                else
                {
                    result = "nodata";
                    resultobj = "Attendance Not Avaialable for  this month of the Selected client";
                }

            }
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        var res = new { msg = result, Obj = resultobj };
        resultobj = new JavaScriptSerializer().Serialize(res);
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetAttendanceSummary(string clientId, string month, bool Chk)
    {
        Context.Response.Clear();
        var result = string.Empty;
        var resultobj = string.Empty;
        DateTime LastDate = DateTime.Now;
        string date = "";
        var Month = 0;
        try
        {
            // var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            if (Chk == false)
            {
                Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));

            }

            else
            {
                date = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb")).ToString();
                string month1 = DateTime.Parse(date).Month.ToString();
                string Year1 = DateTime.Parse(date).Year.ToString();
                string olddate = month1 + Year1.Substring(2, 2);
                Month = Convert.ToInt32(olddate);
            }
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
                               OTHRSTotal = row.Field<string>("OTHRSTotal"),
                               PenTotal = row.Field<string>("PenTotal"),
                               UNIDEDTotal = row.Field<string>("UNIDEDTotal"),
                               ATMDEDTotal = row.Field<string>("ATMDEDTotal"),
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
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SaveAttendance(List<EmpAttendance> lst)
    {
        Context.Response.Clear();
        var result = "";
        var resultobj = string.Empty;
        string OrderedDAte = DateTime.Now.Date.ToString();
        DateTime LastDate = DateTime.Now;
        var Checkbox = false;
        var Month = 0;
        DateTime date = DateTime.Now;
        bool BillfrompayhsheetDuties = false;
        Checkbox = Convert.ToBoolean((lst[0].Chkbox));

        if (Checkbox == false)
        {
            LastDate = Timings.Instance.GetLastDayForSelectedMonth(Convert.ToInt32(lst[0].MonthIndex));
        }

        try
        {
            if (Checkbox == false)
            {
                Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(lst[0].MonthIndex));
            }
            else
            {
                Month = Convert.ToInt32(lst[0].MonthIndex);
            }
            //LastDate = Timings.Instance.GetDateForSelectedMonth(Convert.ToInt32(lst[0].MonthIndex)).ToString("yyyy-MM-dd hh:mm:ss");
            //var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(lst[0].MonthIndex));
            var strquery = "select contractid from contracts where clientid= '" + lst[0].ClientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
            var contractdata = SqlHelper.Instance.GetTableByQuery(strquery);
            var contractId = string.Empty;
            if (contractdata.Rows.Count > 0)
            {
                contractId = contractdata.Rows[0]["contractid"].ToString();
            }
            if (string.IsNullOrEmpty(contractId))
            {
                result = "fail";
                resultobj = "Contract Id not available for the select Client.";
            }
            else
            {
                #region Insert and Update Query
                foreach (var item in lst)
                {
                    if (item.NewAdd) EmpTransfer(item);
                    var empquery = "select COUNT(*) as empcount from  Empattendance Where Empid = '" + item.EmpId +
                                "' and [month]= " + Month + " and ClientId = '" + item.ClientId +
                                "'  and  Design = " + item.EmpDesg + " and   contractid= '" + contractId + "'";
                    var empdata = SqlHelper.Instance.GetTableByQuery(empquery);
                    var empcount = string.Empty;
                    if (empdata.Rows.Count > 0)
                    {
                        empcount = empdata.Rows[0]["empcount"].ToString();
                    }
                    var attendancetotal = item.NOD + item.OT + item.WO + item.NHS + item.Nposts;

                    string empqry = "select empid from empdetails where empid='" + item.EmpId + "'";
                    DataTable dtempqry = Config.ExecuteReaderWithQueryAsync(empqry).Result;

                    string EmployeeId = "";

                    if (dtempqry.Rows.Count > 0)
                    {
                        EmployeeId = dtempqry.Rows[0]["empid"].ToString();
                    }

                   

                    var Contractquery = "select isnull(BillFromPayhsheetDuties,0) BillFromPayhsheetDuties from contracts where ClientId = '" + item.ClientId + "' and   contractid= '" + contractId + "'";
                    DataTable Contractdt = SqlHelper.Instance.GetTableByQuery(Contractquery);
                    if (Contractdt.Rows.Count > 0)
                    {
                        BillfrompayhsheetDuties = Convert.ToBoolean(Contractdt.Rows[0]["BillFromPayhsheetDuties"].ToString());
                    }


                    var query = string.Empty;
                    if (Convert.ToInt32(empcount) > 0)
                    {
                        query = "update EmpAttendance set Modify_On=GETDATE(), NoofDuties=" + item.NOD
                                            + ",OT=" + item.OT
                                            + ",Penalty=" + item.Penality
                                            + ",CanteenAdv=" + item.CanAdv
                                             + ",UniformDed=" + item.UNIDED
                                              + ",OtherDed=" + item.ATMDED
                                            + ",Incentivs=" + item.Incentives
                                            + ",Arrears=" + item.Arrears
                                            + ",stoppayment='" + item.stoppayment
                                            + "',Design='" + item.EmpDesg
                                            + "',WO=" + item.WO
                                            + ",NHS=" + item.NHS
                                            + ",PL=" + item.Nposts
                                              + ",OTHours=" + item.OTHRS
                                            + " Where empid='" + item.EmpId
                                            + "' and ClientId='" + item.ClientId
                                            + "' and [Month]=" + Month
                                            + " and  Design='" + item.EmpDesg
                                            + "' and contractid= '" + contractId + "'";
                    }
                    else if (attendancetotal > 0)
                    {
                        query = "insert  EmpAttendance(clientid,empid,[month],Design,contractId,NoofDuties,OT,Penalty,CanteenAdv,WO,NHS,PL,Incentivs,Arrears,DateCreated,stoppayment, UniformDed,OtherDed,OTHours)" +
                        "values('" + item.ClientId + "','" + item.EmpId + "'," + Month + ",'" + item.EmpDesg + "','" + contractId + "'," + item.NOD + "," + item.OT + "," + item.Penality + "," + item.CanAdv + "," + item.WO + "," + item.NHS + "," + item.Nposts + "," + item.Incentives + "," + item.Arrears + ",GETDATE(),'" + item.stoppayment + "','" + item.UNIDED + "','" + item.ATMDED + "','" + item.OTHRS + "')";
                    }
                    if (!string.IsNullOrEmpty(query))
                    {
                        var res = SqlHelper.Instance.ExecuteDMLQry(query);
                    }
                    result = "success";
                }
                #endregion
            }
            resultobj = new JavaScriptSerializer().Serialize(new { Updated = lst.Count });
            if (BillfrompayhsheetDuties == true)
            {
                var deletequery = "delete from ClientAttenDance where MONTH = " + Month + "  and ClientId = '" + lst[0].ClientId + "'";
                var Attdeleteres = SqlHelper.Instance.ExecuteDMLQry(deletequery);

                string AttQuery = "insert into ClientAttenDance (ClientId,Contractid,Desingnation,Duties,Month,Ot,Quantity,Type) " +
                                " select EA.clientid,EA.ContractId,Design,sum(EA.NoOfDuties+EA.OT), " +
                                " MONTH,0,COUNT(EmpId),0   " +
                                " from EmpAttendance EA   " +
                                " inner join ContractDetails Cd on EA.ClientId=Cd.ClientID and EA.Design=Cd.Designations and EA.ContractId=Cd.ContractId" +
                                " inner join Contracts C on EA.ClientId=C.ClientID and EA.ContractId=C.ContractId  " +
                                " where EA.ClientId='" + lst[0].ClientId + "' and MONTH='" + Month + "'  " +
                                " group by EA.clientid,EA.ContractId,Design,month";
                var Attres = SqlHelper.Instance.ExecuteDMLQry(AttQuery);

            }
        }
        catch (Exception ex)
        {
            result = "fail";
            resultobj = ex.Message;
        }

        var resObj = new { msg = result, Obj = resultobj };
        resultobj = new JavaScriptSerializer().Serialize(resObj);

        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

    private void EmpTransfer(EmpAttendance emp)
    {
        try
        {

            var month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(emp.MonthIndex));
            var monthval = "";
            var yearval = "";
            if (month.ToString().Length == 3)
            {
                monthval = "0" + month.ToString().Substring(0, 1);
                yearval = month.ToString().Substring(1, 2);
            }
            else
            {
                monthval = month.ToString().Substring(0, 2);
                yearval = month.ToString().Substring(2, 2);
            }
            var jdate = DateTime.Parse(emp.JoiningDate, CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy-MM-dd hh:mm:ss");
            var odate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");
            var query = string.Empty;
            var ordermax = "select max(cast(OrderId as int))+ 1 as ordercount from EmpPostingOrder";
            var orderdata = SqlHelper.Instance.GetTableByQuery(ordermax);
            var orderId = string.Empty;
            if (orderdata.Rows.Count > 0)
            {
                orderId = orderdata.Rows[0]["ordercount"].ToString();
            }
            query = " Insert into EmpPostingOrder(EmpId,OrderId,OrderDt,JoiningDt,Desgn,TransferType,PF,ESI,PT,tounitId,postedmonth)" +
            "values('" + emp.EmpId + "','" + orderId + "','" + odate + "','" + jdate + "','" + emp.EmpDesg + "'," + emp.TransferType +
            "," + (emp.PF ? 1 : 0) + "," + (emp.ESI ? 1 : 0) + "," + (emp.PT ? 1 : 0) + ",'" + emp.ClientId + "','" + yearval + monthval + "')";
            var res = SqlHelper.Instance.ExecuteDMLQry(query);
        }
        catch
        { }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void DeleteAttendance(string empId, string empDesgId, string clientId, string month)
    {
        Context.Response.Clear();
        var result = "";
        var resultobj = string.Empty;
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
            resultobj = new JavaScriptSerializer().Serialize(new { Delete = res, Update = res1 });
            result = "success";
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        var resObj = new { msg = result, Obj = resultobj };
        resultobj = new JavaScriptSerializer().Serialize(resObj);

        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void CheckSite(string empId, string empclientid, string month, string Desgn)
    {
        Context.Response.Clear();
        var result = string.Empty;
        var resultobj = string.Empty;
        var Name = "";
        var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
        try
        {
            string selqry = "select ea.empid,(empfname+' '+empmname+' '+emplname+' ( '+ea.empid+' )') as Name from EmpAttendance ea inner join empdetails ed on ed.empid=ea.empid where ea.EmpId='" + empId + "' and ClientId='" + empclientid + "' and MONTH='" + Month + "' and Design='" + Desgn + "'";

            DataTable dtselqry = Config.ExecuteAdaptorAsyncWithQueryParams(selqry).Result;

            if (dtselqry.Rows.Count > 0)
            {
                Name = dtselqry.Rows[0]["Name"].ToString();
                resultobj = "Employee cannot be posted as '" + Name + "' already belongs to this Site With Same Designation.";
                result = "success";

            }
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        var res = new { msg = result, Obj = resultobj };
        resultobj = new JavaScriptSerializer().Serialize(res);
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    protected void DownloadPDF(string clientId, string month, bool Chk)
    {
        DataTable dt = null;

        DateTime LastDate = DateTime.Now;
        string date = "";
        var Month = 0;

        if (Chk == false)
        {
            LastDate = Timings.Instance.GetLastDayForSelectedMonth(Convert.ToInt32(month));
        }
        else
        {
            LastDate = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb"));
        }

        //
        if (Chk == false)
        {
            Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));

        }
        else
        {
            date = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb")).ToString();

            string month1 = DateTime.Parse(date).Month.ToString();
            string Year1 = DateTime.Parse(date).Year.ToString();
            string olddate = month1 + Year1.Substring(2, 2);
            Month = Convert.ToInt32(olddate);
        }


        string SPName = "AttendanceReport";
        Hashtable ht = new Hashtable();
        ht.Add("@ClientId", clientId);
        ht.Add("@month", Month);


        dt = Config.ExecuteAdaptorWithParams(SPName, ht).Result;

        string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
        DataTable compInfo = Config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
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



            PdfPTable Maintable = new PdfPTable(10);
            Maintable.TotalWidth = 580;
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

            string Clientname = dt.Rows[0]["Client Name"].ToString();

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

            PdfPCell cell5 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
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

            //int nextpagerecordscount = 0;
            //int targetpagerecors = 4;
            //int secondpagerecords = targetpagerecors + 2;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Sno = dt.Rows[i]["Sno"].ToString();
                ClientID = dt.Rows[i]["Client Id"].ToString();
                // Clientname = dt.Rows[i]["Client Name"].ToString();
                Empid = dt.Rows[i]["Emp Id"].ToString();
                Empname = dt.Rows[i]["Name"].ToString();
                Desgn = dt.Rows[i]["Designation"].ToString();
                Noofduties = Convert.ToSingle(dt.Rows[i]["Duties"].ToString());
                Ots = Convert.ToSingle(dt.Rows[i]["OT"].ToString());
                Tot = Convert.ToSingle(dt.Rows[i]["TotalDuties"].ToString());
                Wo = Convert.ToSingle(dt.Rows[i]["WO"].ToString());
                Leave = Convert.ToSingle(dt.Rows[i]["Leave"].ToString());
                PH = Convert.ToSingle(dt.Rows[i]["PH"].ToString());
                CanteenAdv = Convert.ToSingle(dt.Rows[i]["Canteen Adv"].ToString());
                Uniform = Convert.ToSingle(dt.Rows[i]["Penalty"].ToString());
                Penalty = Convert.ToSingle(dt.Rows[i]["Uniform Ded"].ToString());
                ATMDed = Convert.ToSingle(dt.Rows[i]["ATM Ded"].ToString());
                TotalDeds = Convert.ToSingle(dt.Rows[i]["Total Amount"].ToString());

                //if (nextpagerecordscount == 0)
                //{
                //    document.Add(Maintable);
                //}
                //nextpagerecordscount++;

                if (Sno == "99999")
                {
                    PdfPCell cell14 = new PdfPCell(new Phrase("", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell14.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell14.Colspan = 1;
                    cell14.Border = 0;
                    Maintable.AddCell(cell14);
                }
                else
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(Sno, FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell15.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell15.Colspan = 1;
                    cell15.Border = 0;
                    Maintable.AddCell(cell15);
                }
                if (Sno == "99999")
                {
                    PdfPCell cell16 = new PdfPCell(new Phrase("Total Duties", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell16.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell16.Colspan = 4;
                    cell16.Border = 0;
                    Maintable.AddCell(cell16);
                }
                else
                {
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
                }
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

                PdfPCell cell21 = new PdfPCell(new Phrase(Ots.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell21.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell21.Colspan = 1;
                cell21.Border = 0;
                Maintable.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(Tot.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell22.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell22.Colspan = 1;
                cell22.Border = 0;
                Maintable.AddCell(cell22);

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

                PdfPCell cell27 = new PdfPCell(new Phrase("Fine", FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
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

                PdfPCell cell35 = new PdfPCell(new Phrase(CanteenAdv.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell35.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell35.Colspan = 1;
                cell35.BorderWidthTop = 0;
                cell35.BorderWidthBottom = 0.2f;
                cell35.BorderWidthLeft = 0;
                cell35.BorderWidthRight = 0;
                Maintable.AddCell(cell35);

                PdfPCell cell36 = new PdfPCell(new Phrase(Uniform.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell36.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell36.Colspan = 1;
                cell36.BorderWidthTop = 0;
                cell36.BorderWidthBottom = 0.2f;
                cell36.BorderWidthLeft = 0;
                cell36.BorderWidthRight = 0;
                Maintable.AddCell(cell36);

                PdfPCell cell37 = new PdfPCell(new Phrase(Penalty.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell37.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell37.Colspan = 1;
                cell37.BorderWidthTop = 0;
                cell37.BorderWidthBottom = 0.2f;
                cell37.BorderWidthLeft = 0;
                cell37.BorderWidthRight = 0;
                Maintable.AddCell(cell37);

                PdfPCell cell38 = new PdfPCell(new Phrase(ATMDed.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell38.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell38.Colspan = 1;
                cell38.BorderWidthTop = 0;
                cell38.BorderWidthBottom = 0.2f;
                cell38.BorderWidthLeft = 0;
                cell38.BorderWidthRight = 0;
                Maintable.AddCell(cell38);

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

                PdfPCell cell41 = new PdfPCell(new Phrase(Leave.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell41.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell41.Colspan = 1;
                cell41.BorderWidthTop = 0;
                cell41.BorderWidthBottom = 0.2f;
                cell41.BorderWidthLeft = 0;
                cell41.BorderWidthRight = 0;
                Maintable.AddCell(cell41);

                PdfPCell cell42 = new PdfPCell(new Phrase(PH.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell42.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell42.Colspan = 1;
                cell42.BorderWidthTop = 0;
                cell42.BorderWidthBottom = 0.2f;
                cell42.BorderWidthLeft = 0;
                cell42.BorderWidthRight = 0;
                Maintable.AddCell(cell42);

                PdfPCell cell43 = new PdfPCell(new Phrase(TotalDeds.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell43.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell43.Colspan = 1;
                cell43.BorderWidthTop = 0;
                cell43.BorderWidthBottom = 0.2f;
                cell43.BorderWidthLeft = 0;
                cell43.BorderWidthRight = 0;
                Maintable.AddCell(cell43);


            }

            document.Add(Maintable);



            PdfPCell cell1 = new PdfPCell(new Phrase(TotalDeds.ToString(), FontFactory.GetFont(fontsyle, Fontsize - 2, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell1.Colspan = 10;
            cell1.BorderWidthTop = 0;
            cell1.BorderWidthBottom = 0.2f;
            cell1.BorderWidthLeft = 0;
            cell1.BorderWidthRight = 0;
            // Maintable.AddCell(cell1);
            // document.Add(Maintable);
            document.NewPage();
            string strDesgnwise = "select SUM(isnull(ea.NoOfDuties,0)) as Duties,sum(isnull(ea.OT,0)) as OT,SUM(isnull(ea.NoOfDuties,0)+isnull(ea.OT,0)) as Total,ISNULL(d.Design,'') as Desgn  from EmpAttendance ea inner join Designations d on d.DesignId = ea.Design where ClientId = '" + ClientID + "' and MONTH = " + Month + " group by d.Design";
            DataTable dtdesgn = Config.ExecuteAdaptorAsyncWithQueryParams(strDesgnwise).Result;

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
            float TotalOts = 0;
            float TotalDays = 0;
            string Designation = "";
            if (dtdesgn.Rows.Count > 0)
            {
                for (int i = 0; i < dtdesgn.Rows.Count; i++)
                {
                    Designation = dtdesgn.Rows[i]["Desgn"].ToString();
                    TotalDuties = Convert.ToSingle(dtdesgn.Rows[i]["Duties"].ToString());
                    TotalOts = Convert.ToSingle(dtdesgn.Rows[i]["OT"].ToString());
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

                    cellsumary = new PdfPCell(new Phrase(TotalOts.ToString(), FontFactory.GetFont(fontsyle, Fontsize, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
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

            string filename = "SalaryCheckList" + ".pdf";

            document.Close();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.End();
        }
    }

}

public class EmpAttendanceGrid
{
    public Int64 sno { get; set; }
    public string EmpId { get; set; }
    public string OLDEMPID { get; set; }
    public string EmpName { get; set; }
    public int DesgId { get; set; }
    public string DesgName { get; set; }
    public float NoOfDuties { get; set; }
    public float OT { get; set; }
    public float WO { get; set; }
    public float NHS { get; set; }
    public float NPosts { get; set; }
    public float CanteenAdv { get; set; }
    public float Penalty { get; set; }
    public float Incentivs { get; set; }
    public float Arrears { get; set; }
    public string empstatus { get; set; }
    public bool Chkbox { get; set; }
    public bool stoppayment { get; set; }
    public float OTHRS { get; set; }
    public float UNIDED { get; set; }
    public float ATMDED { get; set; }
}


public class EmpAttendance
{
    public bool NewAdd { get; set; }
    public bool IsOld { get; set; }
    public string EmpId { get; set; }
    public string OLDEMPID { get; set; }
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
    public bool Chkbox { get; set; }
    public bool stoppayment { get; set; }
    public decimal OTHRS { get; set; }
    public decimal UNIDED { get; set; }
    public decimal ATMDED { get; set; }
}


