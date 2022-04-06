using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Globalization;
using System.IO;
using System.Data.OleDb;
using KLTS.Data;
using Kuduma.Portal.DAL;
using ClosedXML.Excel;

namespace Kuduma.Portal
{
    public partial class ImportAttendance : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil util = new GridViewExportUtil();

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




                string sqlemptydata = "select * from tblEmptyforExcel";
                DataTable dtempty = config.ExecuteAdaptorAsyncWithQueryParams(sqlemptydata).Result;
                if (dtempty.Rows.Count > 0)
                {

                    GridView3.DataSource = dtempty;
                    GridView3.DataBind();
                }
                ddlempidtype.SelectedIndex = 2;

            }
        }



        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                BranchID = Session["BranchID"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void Fillcname()
        {
            if (ddlClientID.SelectedIndex > 0)
            {
                ddlCName.SelectedValue = ddlClientID.SelectedValue;
            }
            else
            {
                ddlCName.SelectedIndex = 0;
            }
        }

        protected void FillClientid()
        {
            if (ddlCName.SelectedIndex > 0)
            {
                ddlClientID.SelectedValue = ddlCName.SelectedValue;
            }
            else
            {
                ddlClientID.SelectedIndex = 0;
            }
        }

        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (DtClientids.Rows.Count > 0)
            {
                ddlCName.DataValueField = "Clientid";
                ddlCName.DataTextField = "clientname";
                ddlCName.DataSource = DtClientids;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "-Select-");

        }

        protected void LoadClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlClientID.DataValueField = "Clientid";
                ddlClientID.DataTextField = "Clientid";
                ddlClientID.DataSource = DtClientNames;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "-Select-");
        }

        public void LoadExcelNos()
        {

            ddlExcelNo.Items.Clear();

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            string qry = "select distinct ExcelNumber from empattendance where month='" + Month + "'";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            if (dt.Rows.Count > 0)
            {
                ddlExcelNo.DataTextField = "ExcelNumber";
                ddlExcelNo.DataValueField = "ExcelNumber";
                ddlExcelNo.DataSource = dt;
                ddlExcelNo.DataBind();
            }

            ddlExcelNo.Items.Insert(0, "Select");
        }

        protected void ddlExcelNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAttSummary(ddlExcelNo.SelectedValue);
        }



        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                txtmonth.Text = "";

                if (ddlClientID.SelectedIndex > 0)
                {

                    ddlCName.SelectedValue = ddlClientID.SelectedValue;
                    GetAttSummary("");
                }
                else
                {
                    //ClearData();

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                txtmonth.Text = "";

                if (ddlCName.SelectedIndex > 0)
                {
                    ddlClientID.SelectedValue = ddlCName.SelectedValue;
                    GetAttSummary("");
                }
                else
                {
                    //ClearData();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {


            if (txtmonth.Text.Trim().Length != 0)
            {
               

                LoadExcelNos();
                GetAttSummary("");
            }
            
        }

        public string GetExcelSheetNames()
        {
            string ExcelSheetname = "";
            OleDbConnection con = null;
            DataTable dt = null;
            string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
            fileupload1.PostedFile.SaveAs(filename);
            string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
            string conStr = string.Empty;
            if (extn.ToLower() == ".xls")
            {
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extn.ToLower() == ".xlsx")
            {
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
            }

            con = new OleDbConnection(conStr);
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();
            ////foreach (DataRow row in dt.Rows)
            ////{
            ////    ExcelSheetname = row["TABLE_NAME"].ToString();
            ////}

            return ExcelSheetname;
        }

        public string Getmonth()
        {

            string month = "";
            string Year = "";
            string date = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();

                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
            }

            return month + Year;
        }

        public void GetAttSummary(string ExcelNo)
        {
            gvattsummarydata.DataSource = null;
            gvattsummarydata.DataBind();


            gvnotinsert.DataSource = null;
            gvnotinsert.DataBind();

            string month = Getmonth();

            string clientd = "";

            if (ddloption.SelectedIndex == 0)
            {
                clientd = "%";
            }
            else
            {
                clientd = ddlClientID.SelectedValue;
            }

            DataTable dt = null;

            if (ExcelNo.Length > 0)
            {
                string Proc = "GetAttendanceSummary";
                Hashtable ht = new Hashtable();
                ht.Add("@ExcelNo", ExcelNo);
                ht.Add("@Type", "SuccessfulImportExcelNo");

                dt = config.ExecuteAdaptorAsyncWithParams(Proc, ht).Result;
            }
            else
            {
                string Proc = "GetAttendanceSummary";
                Hashtable ht = new Hashtable();
                ht.Add("@Clientid", clientd);
                ht.Add("@month", month);
                ht.Add("@CmpIDPrefix", CmpIDPrefix);
                ht.Add("@Type", "SuccessfulImport");

                dt = config.ExecuteAdaptorAsyncWithParams(Proc, ht).Result;


            }

            if (dt.Rows.Count > 0)
            {
                pnlAttSummary.Visible = true;
                gvattsummarydata.DataSource = dt;
                gvattsummarydata.DataBind();
            }
            else
            {
                pnlAttSummary.Visible = false;
            }

            DataTable dtn = null;

            if (ExcelNo.Length > 0)
            {
                string Procn = "GetAttendanceSummary";
                Hashtable htn = new Hashtable();
                htn.Add("@Type", "NotinsertdatabyExcelno");
                htn.Add("@ExcelNo", ExcelNo);
                dtn = config.ExecuteAdaptorAsyncWithParams(Procn, htn).Result;
            }
            else
            {
                string Procn = "GetAttendanceSummary";
                Hashtable htn = new Hashtable();
                htn.Add("@Clientid", clientd);
                htn.Add("@CmpIDPrefix", CmpIDPrefix);
                htn.Add("@Type", "Notinsertdata");
                dtn = config.ExecuteAdaptorAsyncWithParams(Procn, htn).Result;
            }

            if (dtn.Rows.Count > 0)
            {
                gvnotinsert.DataSource = dtn;
                gvnotinsert.DataBind();
                pnlnotinsertdata.Visible = true;
                btnExport.Visible = true;
            }
            else
            {
                pnlnotinsertdata.Visible = false;
                btnExport.Visible = false;
            }


        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            // int month = 0;
            int days = 0;
            string date = string.Empty;
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString().Trim();
            }

            int year = Convert.ToDateTime(date).Year;
            int monthn = Convert.ToDateTime(date).Month;
            days = DateTime.DaysInMonth(year, monthn);
            btnExport.Visible = false;


            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Select Month');", true);
                return;
            }

            if (ddloption.SelectedIndex == 1)
            {
                if (ddlClientID.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Select Client ID');", true);
                    return;
                }
            }

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString().Trim();
            }

            string month = DateTime.Parse(date).Month.ToString().Trim();
            string Year = DateTime.Parse(date).Year.ToString().Trim();

            string Month = month + Year.Substring(2, 2);


            #region Begin Getmax Id from DB
            int ExcelNo = 0;
            string selectquery = "select max(cast(ExcelNumber as int )) as Id from empattendance ";
            DataTable dtExcelID = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

            if (dtExcelID.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dtExcelID.Rows[0]["Id"].ToString().Trim()) == false)
                {
                    ExcelNo = Convert.ToInt32(dtExcelID.Rows[0]["Id"].ToString().Trim()) + 1;
                }
                else
                {
                    ExcelNo = int.Parse("1");
                }
            }
            #endregion End Getmax Id from DB

            #region Begin Code for when select Full Attendance as on 31/07/2014 by Venkat
            //
            if (ddlAttendanceMode.SelectedIndex == 0)
            {
                // string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString().Trim() + Path.GetExtension(fileupload1.PostedFile.FileName));
                // fileupload1.PostedFile.SaveAs(filename);
                // string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                // string constring = "";
                // if (extn.ToLower() == ".xls")
                // {
                //     constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                // }
                // else if (extn.ToLower() == ".xlsx")
                // {
                //     constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                // }

                // string Sheetname = string.Empty;

                // string qry = "";

                // if (ddlempidtype.SelectedIndex == 0)
                // {
                //     qry = "select [Client Id],[Emp Id],[OldEmpId],[Designation],[Duties],[OTs],[WOs],[NHS],[PL Days],[Canteen Advance],[Advance],[Incentives],[OT Hrs],[Uniform Ded],[Other Ded],[Arrears],[Attendance Bonus],[Stop Payment] " +
                //                        "  from  [" + GetExcelSheetNames() + "]" + "";
                // }
                // else
                // {
                //     qry = "select [Client Id],[Emp Id],[OldEmpId],[Designation],[Duties],[OTs],[WOs],[NHS],[PL Days],[Canteen Advance],[Advance],[Incentives],[OT Hrs],[Uniform Ded],[Other Ded],[Arrears],[Attendance Bonus],[Stop Payment] " +
                //"  from  [" + GetExcelSheetNames() + "]" + "";
                // }



                // OleDbConnection con = new OleDbConnection(constring);
                // if (con.State == ConnectionState.Closed)
                // {
                //     con.Open();

                // }
                // OleDbCommand cmd = new OleDbCommand(qry, con);
                // OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                // DataSet ds = new DataSet();
                // da.Fill(ds);
                // da.Dispose();
                // con.Close();
                // con.Dispose();
                // GC.Collect();

                string filePath = Server.MapPath("~/ImportDocuments/") + Path.GetFileName(fileupload1.PostedFile.FileName);
                fileupload1.PostedFile.SaveAs(filePath);

                string extn = Path.GetExtension(fileupload1.PostedFile.FileName);

                //Create a new DataTable.
                DataTable dtexcel = new DataTable();

                if (extn.EndsWith(".xlsx"))
                {
                    using (XLWorkbook workBook = new XLWorkbook(filePath))
                    {
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        //Create a new DataTable.

                        int lastrow = workSheet.LastRowUsed().RowNumber();
                        var rows = workSheet.Rows(1, lastrow);

                        //Create a new DataTable.

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in rows)
                        {
                            //Use the first row to add columns to DataTable.
                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    if (!string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                    {
                                        dtexcel.Columns.Add(cell.Value.ToString().Trim());
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                firstRow = false;
                            }
                            else
                            {
                                int i = 0;
                                DataRow toInsert = dtexcel.NewRow();
                                foreach (IXLCell cell in row.Cells(1, dtexcel.Columns.Count))
                                {
                                    try
                                    {
                                        toInsert[i] = cell.Value.ToString().Trim();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    i++;
                                }
                                dtexcel.Rows.Add(toInsert);
                            }

                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please save file in Excel WorkBook(.xlsx) format');", true);
                    return;
                }

                for (int s = 0; s < dtexcel.Rows.Count; s++)
                {
                    string clid = dtexcel.Rows[s][1].ToString().Trim();

                    if (clid.Length == 0)
                    {
                        dtexcel.Rows.RemoveAt(s);
                    }
                }


                DataSet ds = new DataSet();
                ds.Tables.Add(dtexcel);

                string empid = string.Empty;
                string EmpName = string.Empty;
                string clientid = string.Empty;
                string design = string.Empty;
                int empstatus = 0;
                string ContractID = "";
                string RemarksText = "";
                var Gendays = 0;
                //foreach (DataRow dr in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string Remark = string.Empty;

                    #region Variables for Excel Values


                    //string Month = string.Empty;

                    float penalty = 0;
                    float incentives = 0;
                    float canteenadvance = 0;
                    float Wos = 0;
                    float NHS = 0;
                    float Npots = 0;
                    float Na = 0;
                    float Ab = 0;
                    float duties = 0;
                    float ots = 0;
                    float Fines = 0;
                    float UniformDed = 0;
                    float ATMDed = 0;
                    float OTHrs = 0;
                    float Arrears = 0;
                    float AttBonus = 0;
                    float dayduties = 0;
                    float dayots = 0;
                    float daywos = 0;
                    string stoppayment = "";
                    float pldays = 0;
                    float IncentiveHrs = 0;
                    float IncentiveAmt = 0;
                    float LCHrs = 0;
                    float LCAmt = 0;

                    float leavewages = 0;
                    float Bonus = 0;
                    float FoodAllw = 0;
                    float OtperHour = 0;
                    float OtHour = 0;
                    float OtAmt = 0;
                    float Advance = 0;
                    float RentDed = 0;
                    float BGVDed = 0;

                    #endregion

                    #region Variables for Posting order Table data and EmpAttendance(Default Values)

                    int orderid = 0;

                    string PrevUnitid = string.Empty;
                    string Dutyhrs = string.Empty;
                    DateTime Orderdate = DateTime.Now;
                    DateTime Joiningdate = DateTime.Now;
                    DateTime Releivingdate = DateTime.Now;
                    string IssuedAuthority = string.Empty;
                    string Remarks = string.Empty;
                    int TransferType = 1;

                    string AttString = string.Empty;
                    string HrsString = string.Empty;


                    #endregion

                    clientid = ds.Tables[0].Rows[i]["Client Id"].ToString().Trim();


                    if (ddloption.SelectedIndex == 1)
                    {
                        if (ddlClientID.SelectedValue != clientid)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please check Client Ids in Excel Sheet');", true);
                            return;
                        }
                    }

                    DateTime DtLastDay = DateTime.Now;
                    DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                    int PaysheetDates = 0;

                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", clientid);
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;




                    if (DTContractID.Rows.Count > 0)
                    {
                        ContractID = DTContractID.Rows[0]["contractid"].ToString().Trim();
                        PaysheetDates = int.Parse(DTContractID.Rows[0]["PaySheetDates"].ToString().Trim());
                    }
                    else
                    {
                        ContractID = "0";
                        RemarksText = "Contract not available";
                    }

                    DateTime mGendays = DateTime.Now;
                    DateTime date1 = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
                    mGendays = DateTime.Parse(date1.ToString().Trim());
                    Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, PaysheetDates);


                    int SNo = 0;
                    selectquery = "select max(cast(sno as int )) as sno from empattendance where clientid='" + clientid + "' and month='" + Month + "' ";
                    DataTable dtSnoID = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                    if (dtSnoID.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dtSnoID.Rows[0]["sno"].ToString().Trim()) == false)
                        {
                            SNo = Convert.ToInt32(dtSnoID.Rows[0]["sno"].ToString().Trim()) + 1;
                        }
                        else
                        {
                            SNo = int.Parse("1");
                        }
                    }


                    // if (clientid == ddlClientID.SelectedValue)
                    {
                        empstatus = 0;
                        string sqlchkempid = "";
                        string sqlchkoldempid = "";
                        string oldempid = "";

                        empid = ds.Tables[0].Rows[i]["Emp Id"].ToString().Trim();

                        if (ddlempidtype.SelectedIndex == 1)
                        {
                            empid = ds.Tables[0].Rows[i]["OldEmpId"].ToString().Trim();

                            sqlchkoldempid = "select empid,empid,EmpFName , oldempid from empdetails where oldempid='" + empid + "'";
                            //and empid like '%" + EmpIDPrefix + "%' 

                            DataTable dtchkoldempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkoldempid).Result;

                            if (dtchkoldempid.Rows.Count > 0)
                            {
                                empstatus = 1;
                                empid = dtchkoldempid.Rows[0]["empid"].ToString().Trim();
                            }
                        }

                        sqlchkempid = "select empid,empid,EmpFName , oldempid from empdetails where empid='" + empid + "' and empid like '%" + EmpIDPrefix + "%' ";

                        DataTable dtchkempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkempid).Result;

                        if (dtchkempid.Rows.Count > 0)
                        {
                            empstatus = 1;
                            empid = dtchkempid.Rows[0]["empid"].ToString().Trim();
                            oldempid = dtchkempid.Rows[0]["oldempid"].ToString().Trim();
                            EmpName = dtchkempid.Rows[0]["EmpFName"].ToString().Trim();

                        }
                        else
                        {
                            empstatus = 0;
                            RemarksText = "EmpID not available";
                            oldempid = empid;

                        }


                        if (ContractID == "0")
                        {
                            empstatus = 0;
                        }

                        string ClientStatusCheck = "select ClientStatus from clients where clientid='" + clientid + "'";
                        DataTable dtclientStatus = config.ExecuteAdaptorAsyncWithQueryParams(ClientStatusCheck).Result;
                        string ClientStatus = "0";
                        if (dtclientStatus.Rows.Count > 0)
                        {
                            ClientStatus = dtclientStatus.Rows[0]["ClientStatus"].ToString().Trim();
                        }
                        if (ClientStatus == "0")
                        {
                            empstatus = 0;
                            RemarksText = "Client Status Inactive";
                        }

                        if (empid.Length > 0)
                        {

                            design = ds.Tables[0].Rows[i]["Designation"].ToString().Trim();

                            string sqldesgn = "select DesignId from Designations where Design='" + design + "'";
                            DataTable dtdesgn = config.ExecuteAdaptorAsyncWithQueryParams(sqldesgn).Result;
                            if (dtdesgn.Rows.Count > 0)
                            {
                                design = dtdesgn.Rows[0]["DesignId"].ToString().Trim();
                            }
                            else
                            {
                                empstatus = 0;
                                RemarksText = "Designation mismatch - ( " + design + " )";
                                design = "0";
                            }

                            string Fmonth = (DtLastDay).Month.ToString().Trim();
                            string FYear = (DtLastDay).Year.ToString().Trim();

                            string DOLDate = "";
                            if (Fmonth.Length == 1)
                            {
                                DOLDate = FYear + "-0" + Fmonth + "-01";
                            }
                            else
                            {
                                DOLDate = FYear + "-" + Fmonth + "-01";
                            }

                            #region Begin New code on 28/04/2014 by venkat for Duties,ots,wos,penalty,Incentives,NHS,Ab and Na

                            duties = 0;
                            ots = 0;

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Duties"].ToString().Trim()) == false)
                            {
                                duties = float.Parse(ds.Tables[0].Rows[i]["Duties"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Wos"].ToString().Trim()) == false)
                            {
                                Wos = float.Parse(ds.Tables[0].Rows[i]["WOs"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["NHS"].ToString().Trim()) == false)
                            {
                                NHS = float.Parse(ds.Tables[0].Rows[i]["NHS"].ToString().Trim());
                            }


                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["OTs"].ToString().Trim()) == false)
                            {
                                ots = float.Parse(ds.Tables[0].Rows[i]["OTs"].ToString().Trim());

                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["PL Days"].ToString().Trim()) == false)
                            {
                                pldays = float.Parse(ds.Tables[0].Rows[i]["PL Days"].ToString().Trim());

                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Canteen Advance"].ToString().Trim()) == false)
                            {
                                canteenadvance = float.Parse(ds.Tables[0].Rows[i]["Canteen Advance"].ToString().Trim());
                            }
                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Advance"].ToString().Trim()) == false)
                            {
                                penalty = float.Parse(ds.Tables[0].Rows[i]["Advance"].ToString().Trim());
                            }
                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Incentives"].ToString().Trim()) == false)
                            {
                                incentives = float.Parse(ds.Tables[0].Rows[i]["Incentives"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Uniform Ded"].ToString().Trim()) == false)
                            {
                                UniformDed = float.Parse(ds.Tables[0].Rows[i]["Uniform Ded"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Other Ded"].ToString().Trim()) == false)
                            {
                                ATMDed = float.Parse(ds.Tables[0].Rows[i]["Other Ded"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["OT Hrs"].ToString().Trim()) == false)
                            {
                                OTHrs = float.Parse(ds.Tables[0].Rows[i]["OT Hrs"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Arrears"].ToString().Trim()) == false)
                            {
                                Arrears = float.Parse(ds.Tables[0].Rows[i]["Arrears"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Attendance Bonus"].ToString().Trim()) == false)
                            {
                                AttBonus = float.Parse(ds.Tables[0].Rows[i]["Attendance Bonus"].ToString().Trim());
                            }


                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Leave Wages"].ToString().Trim()) == false)
                            {
                                leavewages = float.Parse(ds.Tables[0].Rows[i]["Leave Wages"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Bonus"].ToString().Trim()) == false)
                            {
                                Bonus = float.Parse(ds.Tables[0].Rows[i]["Bonus"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Food Allw"].ToString().Trim()) == false)
                            {
                                FoodAllw = float.Parse(ds.Tables[0].Rows[i]["Food Allw"].ToString().Trim());
                            }
                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["RentDed"].ToString().Trim()) == false)
                            {
                                RentDed = float.Parse(ds.Tables[0].Rows[i]["RentDed"].ToString().Trim());
                            }
                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["BGVDed"].ToString().Trim()) == false)
                            {
                                BGVDed = float.Parse(ds.Tables[0].Rows[i]["BGVDed"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["OT Per Hour"].ToString().Trim()) == false)
                            {
                                OtperHour = float.Parse(ds.Tables[0].Rows[i]["OT Per Hour"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["OT Hour"].ToString().Trim()) == false)
                            {
                                OtHour = float.Parse(ds.Tables[0].Rows[i]["OT Hour"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["OT Amt"].ToString().Trim()) == false)
                            {
                                OtAmt = float.Parse(ds.Tables[0].Rows[i]["OT Amt"].ToString().Trim());
                            }

                           

                            stoppayment = ds.Tables[0].Rows[i]["Stop Payment"].ToString().Trim();
                            if (stoppayment == "Y" || stoppayment == "YES" || stoppayment == "y" || stoppayment == "yes" || stoppayment == "Yes")
                            {
                                stoppayment = "1";
                            }
                            else if (stoppayment == "N" || stoppayment == "NO" || stoppayment == "n" || stoppayment == "no" || stoppayment == "No")
                            {
                                stoppayment = "0";
                            }


                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["IncentiveHrs"].ToString().Trim()) == false)
                            {
                                IncentiveHrs = float.Parse(ds.Tables[0].Rows[i]["IncentiveHrs"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["IncentiveAmt"].ToString().Trim()) == false)
                            {
                                IncentiveAmt = float.Parse(ds.Tables[0].Rows[i]["IncentiveAmt"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["LCHrs"].ToString().Trim()) == false)
                            {
                                LCHrs = float.Parse(ds.Tables[0].Rows[i]["LCHrs"].ToString().Trim());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["LCAmt"].ToString().Trim()) == false)
                            {
                                LCAmt = float.Parse(ds.Tables[0].Rows[i]["LCAmt"].ToString().Trim());
                            }
                            #endregion

                            #region Begin New code for Stored Procedure as on 29/04/2014 by venkat


                            #region Begin code for passing values to the Stored Procedure as 29/04/2014 by Venkat


                            string SPNameCheck = "AssignDuties";
                            Hashtable ht = new Hashtable();
                            ht.Add("@empid", empid);
                            ht.Add("@month", Month);
                            ht.Add("@OriginalDuties", duties);
                            ht.Add("@OriginalOTs", ots);
                            ht.Add("@clientid", clientid);
                            ht.Add("@Design", design);
                            ht.Add("@Contractid", ContractID);

                            DataTable dtduties = config.ExecuteAdaptorAsyncWithParams(SPNameCheck, ht).Result;

                            decimal dutiesv = 0;
                            decimal otsv = 0;


                            if (dtduties.Rows.Count > 0)
                            {
                                dutiesv = decimal.Parse(dtduties.Rows[0]["Duties"].ToString().Trim());
                                otsv = decimal.Parse(dtduties.Rows[0]["ots"].ToString().Trim());
                            }

                            Hashtable Httable = new Hashtable();

                            Httable.Add("@empidstatus", empstatus);
                            Httable.Add("@Clientid", clientid);
                            Httable.Add("@Month", Month);
                            Httable.Add("@Empid", empid);
                            Httable.Add("@EmpName", EmpName);
                            Httable.Add("@ContractId", ContractID);
                            Httable.Add("@Design", design);
                            Httable.Add("@Duties", dutiesv);
                            Httable.Add("@Ots", otsv);
                            Httable.Add("@WOs", Wos);
                            Httable.Add("@CanteenAdv", canteenadvance);
                            Httable.Add("@Penalty", penalty);
                            Httable.Add("@Incentivs", incentives);
                            //  Httable.Add("@Fines", Fines);
                            Httable.Add("@UniformDed", UniformDed);
                            Httable.Add("@ATMDed", ATMDed);
                            Httable.Add("@OTHrs", OTHrs);
                            Httable.Add("@NHS", NHS);
                            Httable.Add("@pldays", pldays);
                            Httable.Add("@Excel_Number", ExcelNo);
                            Httable.Add("@sno", SNo);
                            Httable.Add("@Arrears", Arrears);
                            Httable.Add("@AttBonus", AttBonus);
                            Httable.Add("@RemarksText", RemarksText);
                            Httable.Add("@oldempid", oldempid);
                            Httable.Add("@Stoppayment", stoppayment);
                            Httable.Add("@RentDed", RentDed);
                            Httable.Add("@BGVDed", BGVDed);
                            Httable.Add("@IncentiveHrs", IncentiveHrs);
                            Httable.Add("@IncentiveAmt", IncentiveAmt);
                            Httable.Add("@LCHrs", LCHrs);
                            Httable.Add("@LCAmt", LCAmt);
                            Httable.Add("@RowNo", i+2);

                            #endregion

                            string SPName = "ImportFullAttendanceFromExcel";
                            DataTable dtstatus = config.ExecuteAdaptorAsyncWithParams(SPName, Httable).Result;


                            #endregion



                        }
                    }
                }

                string SPBillName = "SaveBillAttendance";
                Hashtable HtBilltable = new Hashtable();
                HtBilltable.Add("@Clientid", clientid);
                HtBilltable.Add("@Month", Month);
                DataTable dtBillstatus = config.ExecuteAdaptorAsyncWithParams(SPBillName, HtBilltable).Result;


                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }


            }


            #endregion

            #region Begin Code when select Individual attendance as on 31/07/2014 by Venkat
            //
            if (ddlAttendanceMode.SelectedIndex == 1)
            {

                //   string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString().Trim() + Path.GetExtension(fileupload1.PostedFile.FileName));
                //   fileupload1.PostedFile.SaveAs(filename);
                //   string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                //   string constring = "";
                //   if (extn.ToLower() == ".xls")
                //   {
                //       constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                //   }
                //   else if (extn.ToLower() == ".xlsx")
                //   {
                //       constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                //   }

                //   string Sheetname = string.Empty;
                //   string qry = string.Empty;
                //   //Done on 18-04-2020 by Mahesh Goud 
                //   if (days == 31)
                //   {
                //       qry = "select [Client Id],[Emp Id],[OldEmpId],[Designation],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]," +
                //" [19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[OTs],[PL Days],[Canteen Advance],[Advance],[Incentives],[OT Hrs],[Uniform Ded],[Other Ded],[Arrears],[Attendance Bonus],[Stop Payment] " +
                //"  from  [" + GetExcelSheetNames() + "]" + "";
                //   }
                //   if (days == 30)
                //   {
                //       qry = "select [Client Id],[Emp Id],[OldEmpId],[Designation],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]," +
                //" [19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[OTs],[PL Days],[Canteen Advance],[Advance],[Incentives],[OT Hrs],[Uniform Ded],[Other Ded],[Arrears],[Attendance Bonus],[Stop Payment] " +
                //"  from  [" + GetExcelSheetNames() + "]" + "";
                //   }
                //   if (days == 29)
                //   {
                //       qry = "select [Client Id],[Emp Id],[OldEmpId],[Designation],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]," +
                //                        " [19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[OTs],[PL Days],[Canteen Advance],[Advance],[Incentives],[OT Hrs],[Uniform Ded],[Other Ded],[Arrears],[Attendance Bonus],[Stop Payment] " +
                //                        "  from  [" + GetExcelSheetNames() + "]" + "";
                //   }
                //   if (days == 28)
                //   {
                //       qry = "select [Client Id],[Emp Id],[OldEmpId],[Designation],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]," +
                //                        " [19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[OTs],[PL Days],[Canteen Advance],[Advance],[Incentives],[OT Hrs],[Uniform Ded],[Other Ded],[Arrears],[Attendance Bonus],[Stop Payment] " +
                //                        "  from  [" + GetExcelSheetNames() + "]" + "";
                //   }
                //   OleDbConnection con = new OleDbConnection(constring);
                //   if (con.State == ConnectionState.Closed)
                //   {
                //       con.Open();

                //   }
                //   OleDbCommand cmd = new OleDbCommand(qry, con);
                //   OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                //   DataSet ds = new DataSet();
                //   da.Fill(ds);
                //   da.Dispose();
                //   con.Close();
                //   con.Dispose();
                //   GC.Collect();

                string filePath = Server.MapPath("~/ImportDocuments/") + Path.GetFileName(fileupload1.PostedFile.FileName);
                fileupload1.PostedFile.SaveAs(filePath);

                string extn = Path.GetExtension(fileupload1.PostedFile.FileName);

                //Create a new DataTable.
                DataTable dtexcel = new DataTable();

                if (extn.EndsWith(".xlsx"))
                {
                    using (XLWorkbook workBook = new XLWorkbook(filePath))
                    {
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        //Create a new DataTable.

                        int lastrow = workSheet.LastRowUsed().RowNumber();
                        var rows = workSheet.Rows(1, lastrow);

                        //Create a new DataTable.

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in rows)
                        {
                            //Use the first row to add columns to DataTable.
                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    if (!string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                    {
                                        dtexcel.Columns.Add(cell.Value.ToString().Trim());
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                firstRow = false;
                            }
                            else
                            {
                                int i = 0;
                                DataRow toInsert = dtexcel.NewRow();
                                foreach (IXLCell cell in row.Cells(1, dtexcel.Columns.Count))
                                {
                                    try
                                    {
                                        toInsert[i] = cell.Value.ToString().Trim();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    i++;
                                }
                                dtexcel.Rows.Add(toInsert);
                            }

                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please save file in Excel WorkBook(.xlsx) format');", true);
                    return;
                }

                for (int s = 0; s < dtexcel.Rows.Count; s++)
                {
                    string clid = dtexcel.Rows[s][1].ToString().Trim();

                    if (clid.Length == 0)
                    {
                        dtexcel.Rows.RemoveAt(s);
                    }
                }


                DataSet ds = new DataSet();
                ds.Tables.Add(dtexcel);

                int k = 0;
                int j = 0;

                string empid = string.Empty;
                string clientid = string.Empty;
                string design = string.Empty;
                int empstatus = 0;

                float duties = 0;
                float ots = 0;
                float pldays = 0;

                float leavewages = 0;
                float Bonus = 0;
                float FoodAllw = 0;
                float OtperHour = 0;
                float OtHour = 0;
                float IncentiveHrs = 0;
                float IncentiveAmt = 0;
                float LCHrs = 0;
                float LCAmt = 0;
                float OtAmt = 0;
                float Advance = 0;

                float RentDed = 0;
                float BGVDed = 0;

                string RemarksText = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string Remark = string.Empty;


                    #region Variables for Excel Values


                    // string Month = string.Empty;

                    float penalty = 0;
                    float incentives = 0;
                    float canteenadvance = 0;
                    float Wos = 0;
                    float NHS = 0;
                    float Npots = 0;
                    float Fines = 0;
                    float UniformDed = 0;
                    float ATMDed = 0;
                    float OTHrs = 0;
                    float Arrears = 0;
                    float AttBonus = 0;
                    string stoppayment = "";
                    



                    string day1 = string.Empty;
                    string day2 = string.Empty;
                    string day3 = string.Empty;
                    string day4 = string.Empty;
                    string day5 = string.Empty;
                    string day6 = string.Empty;
                    string day7 = string.Empty;
                    string day8 = string.Empty;
                    string day9 = string.Empty;
                    string day10 = string.Empty;
                    string day11 = string.Empty;
                    string day12 = string.Empty;
                    string day13 = string.Empty;
                    string day14 = string.Empty;
                    string day15 = string.Empty;
                    string day16 = string.Empty;
                    string day17 = string.Empty;
                    string day18 = string.Empty;
                    string day19 = string.Empty;
                    string day20 = string.Empty;
                    string day21 = string.Empty;
                    string day22 = string.Empty;
                    string day23 = string.Empty;
                    string day24 = string.Empty;
                    string day25 = string.Empty;
                    string day26 = string.Empty;
                    string day27 = string.Empty;
                    string day28 = string.Empty;
                    string day29 = string.Empty;
                    string day30 = string.Empty;
                    string day31 = string.Empty;

                    string day1ot = string.Empty;
                    string day2ot = string.Empty;
                    string day3ot = string.Empty;
                    string day4ot = string.Empty;
                    string day5ot = string.Empty;
                    string day6ot = string.Empty;
                    string day7ot = string.Empty;
                    string day8ot = string.Empty;
                    string day9ot = string.Empty;
                    string day10ot = string.Empty;
                    string day11ot = string.Empty;
                    string day12ot = string.Empty;
                    string day13ot = string.Empty;
                    string day14ot = string.Empty;
                    string day15ot = string.Empty;
                    string day16ot = string.Empty;
                    string day17ot = string.Empty;
                    string day18ot = string.Empty;
                    string day19ot = string.Empty;
                    string day20ot = string.Empty;
                    string day21ot = string.Empty;
                    string day22ot = string.Empty;
                    string day23ot = string.Empty;
                    string day24ot = string.Empty;
                    string day25ot = string.Empty;
                    string day26ot = string.Empty;
                    string day27ot = string.Empty;
                    string day28ot = string.Empty;
                    string day29ot = string.Empty;
                    string day30ot = string.Empty;
                    string day31ot = string.Empty;

                    float dayduties = 0;
                    float dayots = 0;
                    float daywos = 0;
                    float dayNHS = 0;

                    #endregion

                    #region Variables for Posting order Table data and EmpAttendance(Default Values)

                    int orderid = 0;

                    string PrevUnitid = string.Empty;
                    string Dutyhrs = string.Empty;
                    DateTime Orderdate = DateTime.Now;
                    DateTime Joiningdate = DateTime.Now;
                    DateTime Releivingdate = DateTime.Now;
                    string IssuedAuthority = string.Empty;
                    string Remarks = string.Empty;
                    int TransferType = 1;

                    string AttString = string.Empty;
                    string HrsString = string.Empty;
                    float TotalHours = 0;
                    float OTHours = 0;
                    float NHDays = 0;
                    float CL = 0;
                    float PL = 0;
                    float UL = 0;

                    int pf = 0;
                    int esi = 0;
                    int pt = 0;

                    string Contractid = string.Empty;

                    #endregion



                    if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                    {

                        clientid = ds.Tables[0].Rows[k]["Client Id"].ToString().Trim();
                        ViewState["clientid"] = clientid;
                        clientid = ViewState["clientid"].ToString().Trim();
                    }

                    if (ddloption.SelectedIndex == 1)
                    {
                        if (ddlClientID.SelectedValue != clientid)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please check Client Ids in Excel Sheet');", true);
                            return;
                        }
                    }

                    int Sno = 0;
                    selectquery = "select max(cast(Sno as int )) as Sno from empattendance where clientid='" + clientid + "' and month='" + Month + "' ";
                    DataTable dtSno = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                    if (dtSno.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dtSno.Rows[0]["Sno"].ToString().Trim()) == false)
                        {
                            Sno = Convert.ToInt32(dtSno.Rows[0]["Sno"].ToString().Trim()) + 1;
                        }
                        else
                        {
                            Sno = int.Parse("1");
                        }
                    }

                    DateTime DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

                    #region  Begin Get Contract Id Based on The Last Day


                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", clientid);
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                    if (DTContractID.Rows.Count > 0)
                    {
                        Contractid = DTContractID.Rows[0]["contractid"].ToString().Trim();
                    }
                    else
                    {
                       
                        Contractid = "0";
                        RemarksText = "Contract not available";
                    }

                    #endregion  End Get Contract Id Based on The Last Day

                    //if (clientid == ddlClientID.SelectedValue)
                    {
                        if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                        {
                            empstatus = 0;
                            string sqlchkempid = "";
                            string oldempid = "";
                            empid = ds.Tables[0].Rows[k]["Emp Id"].ToString().Trim();
                            ViewState["empid"] = empid;
                            empid = ViewState["empid"].ToString().Trim();

                            if (ddlempidtype.SelectedIndex == 0)
                            {
                                sqlchkempid = "select empid,oldempid from empdetails where Oldempid='" + empid + "' and empid like '%" + EmpIDPrefix + "%' ";
                            }
                            else
                            {
                                sqlchkempid = "select empid,empid as oldempid from empdetails where empid='" + empid + "' and empid like '%" + EmpIDPrefix + "%' ";
                            }

                            DataTable dtchkempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkempid).Result;

                            if (dtchkempid.Rows.Count > 0)
                            {
                                empstatus = 1;
                                empid = dtchkempid.Rows[0]["empid"].ToString().Trim();
                                oldempid = dtchkempid.Rows[0]["oldempid"].ToString().Trim();

                            }
                            else
                            {
                                empstatus = 0;
                                RemarksText = "EmpID not available";
                                oldempid = empid;
                            }
                        }
                        if (Contractid == "0")
                        {
                            empstatus = 0;
                        }


                        //Create Procedure


                        if (empid.Length > 0)
                        {

                            if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                            {
                                design = ds.Tables[0].Rows[k]["Designation"].ToString().Trim();
                                ViewState["design"] = design;
                                design = ViewState["design"].ToString().Trim();

                                string sqldesgn = "select DesignId from Designations where Design='" + design + "'";
                                DataTable dtdesgn = config.ExecuteAdaptorAsyncWithQueryParams(sqldesgn).Result;
                                if (dtdesgn.Rows.Count > 0)
                                {
                                    design = dtdesgn.Rows[0]["DesignId"].ToString().Trim();
                                }
                                else
                                {

                                    empstatus = 0;
                                    RemarksText = "Designation mismatch - ( " + design + " )";
                                    design = "0";
                                }
                            }

                            if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                            {

                                float dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, dt13, dt14, dt15, dt16, dt17, dt18, dt19, dt20, dt21,
                                    dt22, dt23, dt24, dt25, dt26, dt27, dt28, dt29, dt30, dt31;

                                float Ots1, Ots2, Ots3, Ots4, Ots5, Ots6, Ots7, Ots8, Ots9, Ots10, Ots11, Ots12, Ots13, Ots14, Ots15, Ots16, Ots17, Ots18, Ots19, Ots20, Ots21,
                                   Ots22, Ots23, Ots24, Ots25, Ots26, Ots27, Ots28, Ots29, Ots30, Ots31;

                                float wo1, wo2, wo3, wo4, wo5, wo6, wo7, wo8, wo9, wo10, wo11, wo12, wo13, wo14, wo15, wo16, wo17, wo18, wo19, wo20, wo21,
                                  wo22, wo23, wo24, wo25, wo26, wo27, wo28, wo29, wo30, wo31;


                                float nhs1, nhs2, nhs3, nhs4, nhs5, nhs6, nhs7, nhs8, nhs9, nhs10, nhs11, nhs12, nhs13, nhs14, nhs15, nhs16, nhs17, nhs18, nhs19, nhs20, nhs21,
                                  nhs22, nhs23, nhs24, nhs25, nhs26, nhs27, nhs28, nhs29, nhs30, nhs31;

                                dt1 = dt2 = dt3 = dt4 = dt5 = dt6 = dt7 = dt8 = dt9 = dt10 = dt11 = dt12 = dt13 = dt14 = dt15 = dt16 = dt17 = dt18 = dt19 = dt20 = dt21 =
                                    dt22 = dt23 = dt24 = dt25 = dt26 = dt27 = dt28 = dt29 = dt30 = dt31 = 0;

                                Ots1 = Ots2 = Ots3 = Ots4 = Ots5 = Ots6 = Ots7 = Ots8 = Ots9 = Ots10 = Ots11 = Ots12 = Ots13 = Ots14 = Ots15 = Ots16 = Ots17 = Ots18 = Ots19 = Ots20 = Ots21 =
                                   Ots22 = Ots23 = Ots24 = Ots25 = Ots26 = Ots27 = Ots28 = Ots29 = Ots30 = Ots31 = 0;

                                wo1 = wo2 = wo3 = wo4 = wo5 = wo6 = wo7 = wo8 = wo9 = wo10 = wo11 = wo12 = wo13 = wo14 = wo15 = wo16 = wo17 = wo18 = wo19 = wo20 = wo21 =
                                  wo22 = wo23 = wo24 = wo25 = wo26 = wo27 = wo28 = wo29 = wo30 = wo31 = 0;


                                nhs1 = nhs2 = nhs3 = nhs4 = nhs5 = nhs6 = nhs7 = nhs8 = nhs9 = nhs10 = nhs11 = nhs12 = nhs13 = nhs14 = nhs15 = nhs16 = nhs17 = nhs18 = nhs19 = nhs20 = nhs21 =
                                  nhs22 = nhs23 = nhs24 = nhs25 = nhs26 = nhs27 = nhs28 = nhs29 = nhs30 = nhs31 = 0;

                                #region Begin New code on 28/04/2014 by venkat for Duties,ots,wos,penalty,Incentives,NHS,Ab and Na

                                duties = 0;
                                ots = 0;

                                if (String.IsNullOrEmpty(dr["OTs"].ToString().Trim()) == false)
                                {
                                    ots = float.Parse(dr["OTs"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["PL Days"].ToString().Trim()) == false)
                                {
                                    pldays = float.Parse(dr["PL Days"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["Canteen Advance"].ToString().Trim()) == false)
                                {
                                    canteenadvance = float.Parse(dr["Canteen Advance"].ToString().Trim());
                                }
                                if (String.IsNullOrEmpty(dr["Advance"].ToString().Trim()) == false)
                                {
                                    penalty = float.Parse(dr["Advance"].ToString().Trim());
                                }
                                if (String.IsNullOrEmpty(dr["Incentives"].ToString().Trim()) == false)
                                {
                                    incentives = float.Parse(dr["Incentives"].ToString().Trim());
                                }

                                //if (String.IsNullOrEmpty(dr["Fines"].ToString().Trim()) == false)
                                //{
                                //    Fines = float.Parse(dr["Fines"].ToString().Trim());
                                //}

                                if (String.IsNullOrEmpty(dr["Uniform Ded"].ToString().Trim()) == false)
                                {
                                    UniformDed = float.Parse(dr["Uniform Ded"].ToString().Trim());
                                }
                                if (String.IsNullOrEmpty(dr["RentDed"].ToString().Trim()) == false)
                                {
                                    RentDed = float.Parse(dr["RentDed"].ToString().Trim());
                                }
                                if (String.IsNullOrEmpty(dr["BGVDed"].ToString().Trim()) == false)
                                {
                                    BGVDed = float.Parse(dr["BGVDed"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["Other Ded"].ToString().Trim()) == false)
                                {
                                    ATMDed = float.Parse(dr["Other Ded"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["OT Hrs"].ToString().Trim()) == false)
                                {
                                    OTHrs = float.Parse(dr["OT Hrs"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["Arrears"].ToString().Trim()) == false)
                                {
                                    Arrears = float.Parse(dr["Arrears"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["Attendance Bonus"].ToString().Trim()) == false)
                                {
                                    AttBonus = float.Parse(dr["Attendance Bonus"].ToString().Trim());
                                }
                                if (String.IsNullOrEmpty(dr["Stop Payment"].ToString().Trim()) == false)
                                {
                                    stoppayment = dr["Stop Payment"].ToString().Trim();
                                }
                                if (stoppayment == "Y" || stoppayment == "YES" || stoppayment == "y" || stoppayment == "yes" || stoppayment == "Yes")
                                {
                                    stoppayment = "1";
                                }
                                else if (stoppayment == "N" || stoppayment == "NO" || stoppayment == "n" || stoppayment == "no" || stoppayment == "No")
                                {
                                    stoppayment = "0";
                                }
                                if (String.IsNullOrEmpty(dr["IncentiveHrs"].ToString().Trim()) == false)
                                {
                                    IncentiveHrs = float.Parse(dr["IncentiveHrs"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["IncentiveAmt"].ToString().Trim()) == false)
                                {
                                    IncentiveAmt = float.Parse(dr["IncentiveAmt"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["LCHrs"].ToString().Trim()) == false)
                                {
                                    LCHrs = float.Parse(dr["LCHrs"].ToString().Trim());
                                }

                                if (String.IsNullOrEmpty(dr["LCAmt"].ToString().Trim()) == false)
                                {
                                    LCAmt = float.Parse(dr["LCAmt"].ToString().Trim());
                                }
                                #endregion

                                #region Day wise Even data insert


                                if (days == 31)
                                {
                                    day1 = dr["1"].ToString().Trim();
                                    if (day1.Trim().Length == 0 || day1 == "0")
                                    { day1 = "A"; }

                                    day2 = dr["2"].ToString().Trim();
                                    if (day2.Length == 0 || day2 == "0")
                                    { day2 = "A"; }

                                    day3 = dr["3"].ToString().Trim();
                                    if (day3.Length == 0 || day3 == "0")
                                    { day3 = "A"; }

                                    day4 = dr["4"].ToString().Trim();
                                    if (day4.Length == 0 || day4 == "0")
                                    {
                                        day4 = "A";
                                    }

                                    day5 = dr["5"].ToString().Trim();
                                    if (day5.Length == 0 || day5 == "0")
                                    {
                                        day5 = "A";
                                    }

                                    day6 = dr["6"].ToString().Trim();
                                    if (day6.Length == 0 || day6 == "0")
                                    {
                                        day6 = "A";
                                    }

                                    day7 = dr["7"].ToString().Trim();
                                    if (day7.Length == 0 || day7 == "0")
                                    {
                                        day7 = "A";
                                    }

                                    day8 = dr["8"].ToString().Trim();
                                    if (day8.Length == 0 || day8 == "0")
                                    {
                                        day8 = "A";
                                    }

                                    day9 = dr["9"].ToString().Trim();
                                    if (day9.Length == 0 || day9 == "0")
                                    {
                                        day9 = "A";
                                    }

                                    day10 = dr["10"].ToString().Trim();
                                    if (day10.Length == 0 || day10 == "0")
                                    {
                                        day10 = "A";
                                    }

                                    day11 = dr["11"].ToString().Trim();
                                    if (day11.Length == 0 || day11 == "0")
                                    {
                                        day11 = "A";
                                    }

                                    day12 = dr["12"].ToString().Trim();
                                    if (day12.Length == 0 || day12 == "0")
                                    {
                                        day12 = "A";
                                    }

                                    day13 = dr["13"].ToString().Trim();
                                    if (day13.Length == 0 || day13 == "0")
                                    {
                                        day13 = "A";
                                    }

                                    day14 = dr["14"].ToString().Trim();
                                    if (day14.Length == 0 || day14 == "0")
                                    {
                                        day14 = "A";
                                    }

                                    day15 = dr["15"].ToString().Trim();
                                    if (day15.Length == 0 || day15 == "0")
                                    {
                                        day15 = "A";
                                    }

                                    day16 = dr["16"].ToString().Trim();
                                    if (day16.Length == 0 || day16 == "0")
                                    {
                                        day16 = "A";
                                    }

                                    day17 = dr["17"].ToString().Trim();
                                    if (day17.Length == 0 || day17 == "0")
                                    {
                                        day17 = "A";
                                    }

                                    day18 = dr["18"].ToString().Trim();
                                    if (day18.Length == 0 || day18 == "0")
                                    {
                                        day18 = "A";
                                    }

                                    day19 = dr["19"].ToString().Trim();
                                    if (day19.Length == 0 || day19 == "0")
                                    {
                                        day19 = "A";
                                    }

                                    day20 = dr["20"].ToString().Trim();
                                    if (day20.Length == 0 || day20 == "0")
                                    {
                                        day20 = "A";
                                    }

                                    day21 = dr["21"].ToString().Trim();
                                    if (day21.Length == 0 || day21 == "0")
                                    {
                                        day21 = "A";
                                    }

                                    day22 = dr["22"].ToString().Trim();
                                    if (day22.Length == 0 || day22 == "0")
                                    {
                                        day22 = "A";
                                    }

                                    day23 = dr["23"].ToString().Trim();
                                    if (day23.Length == 0 || day23 == "0")
                                    {
                                        day23 = "A";
                                    }

                                    day24 = dr["24"].ToString().Trim();
                                    if (day24.Length == 0 || day24 == "0")
                                    {
                                        day24 = "A";
                                    }

                                    day25 = dr["25"].ToString().Trim();
                                    if (day25.Length == 0 || day25 == "0")
                                    {
                                        day25 = "A";
                                    }

                                    day26 = dr["26"].ToString().Trim();
                                    if (day26.Length == 0 || day26 == "0")
                                    {
                                        day26 = "A";
                                    }

                                    day27 = dr["27"].ToString().Trim();
                                    if (day27.Length == 0 || day27 == "0")
                                    {
                                        day27 = "A";
                                    }

                                    day28 = dr["28"].ToString().Trim();
                                    if (day28.Length == 0 || day28 == "0")
                                    {
                                        day28 = "A";
                                    }

                                    day29 = dr["29"].ToString().Trim();
                                    if (day29.Length == 0 || day29 == "0")
                                    {
                                        day29 = "A";
                                    }

                                    day30 = dr["30"].ToString().Trim();
                                    if (day30.Length == 0 || day30 == "0")
                                    {
                                        day30 = "A";
                                    }

                                    day31 = dr["31"].ToString().Trim();
                                    if (day31.Length == 0 || day31 == "0")
                                    {
                                        day31 = "A";
                                    }
                                }
                                if (days == 30)
                                {
                                    day1 = dr["1"].ToString().Trim();
                                    if (day1.Trim().Length == 0 || day1 == "0")
                                    { day1 = "A"; }

                                    day2 = dr["2"].ToString().Trim();
                                    if (day2.Length == 0 || day2 == "0")
                                    { day2 = "A"; }

                                    day3 = dr["3"].ToString().Trim();
                                    if (day3.Length == 0 || day3 == "0")
                                    { day3 = "A"; }

                                    day4 = dr["4"].ToString().Trim();
                                    if (day4.Length == 0 || day4 == "0")
                                    {
                                        day4 = "A";
                                    }

                                    day5 = dr["5"].ToString().Trim();
                                    if (day5.Length == 0 || day5 == "0")
                                    {
                                        day5 = "A";
                                    }

                                    day6 = dr["6"].ToString().Trim();
                                    if (day6.Length == 0 || day6 == "0")
                                    {
                                        day6 = "A";
                                    }

                                    day7 = dr["7"].ToString().Trim();
                                    if (day7.Length == 0 || day7 == "0")
                                    {
                                        day7 = "A";
                                    }

                                    day8 = dr["8"].ToString().Trim();
                                    if (day8.Length == 0 || day8 == "0")
                                    {
                                        day8 = "A";
                                    }

                                    day9 = dr["9"].ToString().Trim();
                                    if (day9.Length == 0 || day9 == "0")
                                    {
                                        day9 = "A";
                                    }

                                    day10 = dr["10"].ToString().Trim();
                                    if (day10.Length == 0 || day10 == "0")
                                    {
                                        day10 = "A";
                                    }

                                    day11 = dr["11"].ToString().Trim();
                                    if (day11.Length == 0 || day11 == "0")
                                    {
                                        day11 = "A";
                                    }

                                    day12 = dr["12"].ToString().Trim();
                                    if (day12.Length == 0 || day12 == "0")
                                    {
                                        day12 = "A";
                                    }

                                    day13 = dr["13"].ToString().Trim();
                                    if (day13.Length == 0 || day13 == "0")
                                    {
                                        day13 = "A";
                                    }

                                    day14 = dr["14"].ToString().Trim();
                                    if (day14.Length == 0 || day14 == "0")
                                    {
                                        day14 = "A";
                                    }

                                    day15 = dr["15"].ToString().Trim();
                                    if (day15.Length == 0 || day15 == "0")
                                    {
                                        day15 = "A";
                                    }

                                    day16 = dr["16"].ToString().Trim();
                                    if (day16.Length == 0 || day16 == "0")
                                    {
                                        day16 = "A";
                                    }

                                    day17 = dr["17"].ToString().Trim();
                                    if (day17.Length == 0 || day17 == "0")
                                    {
                                        day17 = "A";
                                    }

                                    day18 = dr["18"].ToString().Trim();
                                    if (day18.Length == 0 || day18 == "0")
                                    {
                                        day18 = "A";
                                    }

                                    day19 = dr["19"].ToString().Trim();
                                    if (day19.Length == 0 || day19 == "0")
                                    {
                                        day19 = "A";
                                    }

                                    day20 = dr["20"].ToString().Trim();
                                    if (day20.Length == 0 || day20 == "0")
                                    {
                                        day20 = "A";
                                    }

                                    day21 = dr["21"].ToString().Trim();
                                    if (day21.Length == 0 || day21 == "0")
                                    {
                                        day21 = "A";
                                    }

                                    day22 = dr["22"].ToString().Trim();
                                    if (day22.Length == 0 || day22 == "0")
                                    {
                                        day22 = "A";
                                    }

                                    day23 = dr["23"].ToString().Trim();
                                    if (day23.Length == 0 || day23 == "0")
                                    {
                                        day23 = "A";
                                    }

                                    day24 = dr["24"].ToString().Trim();
                                    if (day24.Length == 0 || day24 == "0")
                                    {
                                        day24 = "A";
                                    }

                                    day25 = dr["25"].ToString().Trim();
                                    if (day25.Length == 0 || day25 == "0")
                                    {
                                        day25 = "A";
                                    }

                                    day26 = dr["26"].ToString().Trim();
                                    if (day26.Length == 0 || day26 == "0")
                                    {
                                        day26 = "A";
                                    }

                                    day27 = dr["27"].ToString().Trim();
                                    if (day27.Length == 0 || day27 == "0")
                                    {
                                        day27 = "A";
                                    }

                                    day28 = dr["28"].ToString().Trim();
                                    if (day28.Length == 0 || day28 == "0")
                                    {
                                        day28 = "A";
                                    }

                                    day29 = dr["29"].ToString().Trim();
                                    if (day29.Length == 0 || day29 == "0")
                                    {
                                        day29 = "A";
                                    }

                                    day30 = dr["30"].ToString().Trim();
                                    if (day30.Length == 0 || day30 == "0")
                                    {
                                        day30 = "A";
                                    }
                                }
                                if (days == 29)
                                {
                                    day1 = dr["1"].ToString().Trim();
                                    if (day1.Trim().Length == 0 || day1 == "0")
                                    { day1 = "A"; }

                                    day2 = dr["2"].ToString().Trim();
                                    if (day2.Length == 0 || day2 == "0")
                                    { day2 = "A"; }

                                    day3 = dr["3"].ToString().Trim();
                                    if (day3.Length == 0 || day3 == "0")
                                    { day3 = "A"; }

                                    day4 = dr["4"].ToString().Trim();
                                    if (day4.Length == 0 || day4 == "0")
                                    {
                                        day4 = "A";
                                    }

                                    day5 = dr["5"].ToString().Trim();
                                    if (day5.Length == 0 || day5 == "0")
                                    {
                                        day5 = "A";
                                    }

                                    day6 = dr["6"].ToString().Trim();
                                    if (day6.Length == 0 || day6 == "0")
                                    {
                                        day6 = "A";
                                    }

                                    day7 = dr["7"].ToString().Trim();
                                    if (day7.Length == 0 || day7 == "0")
                                    {
                                        day7 = "A";
                                    }

                                    day8 = dr["8"].ToString().Trim();
                                    if (day8.Length == 0 || day8 == "0")
                                    {
                                        day8 = "A";
                                    }

                                    day9 = dr["9"].ToString().Trim();
                                    if (day9.Length == 0 || day9 == "0")
                                    {
                                        day9 = "A";
                                    }

                                    day10 = dr["10"].ToString().Trim();
                                    if (day10.Length == 0 || day10 == "0")
                                    {
                                        day10 = "A";
                                    }

                                    day11 = dr["11"].ToString().Trim();
                                    if (day11.Length == 0 || day11 == "0")
                                    {
                                        day11 = "A";
                                    }

                                    day12 = dr["12"].ToString().Trim();
                                    if (day12.Length == 0 || day12 == "0")
                                    {
                                        day12 = "A";
                                    }

                                    day13 = dr["13"].ToString().Trim();
                                    if (day13.Length == 0 || day13 == "0")
                                    {
                                        day13 = "A";
                                    }

                                    day14 = dr["14"].ToString().Trim();
                                    if (day14.Length == 0 || day14 == "0")
                                    {
                                        day14 = "A";
                                    }

                                    day15 = dr["15"].ToString().Trim();
                                    if (day15.Length == 0 || day15 == "0")
                                    {
                                        day15 = "A";
                                    }

                                    day16 = dr["16"].ToString().Trim();
                                    if (day16.Length == 0 || day16 == "0")
                                    {
                                        day16 = "A";
                                    }

                                    day17 = dr["17"].ToString().Trim();
                                    if (day17.Length == 0 || day17 == "0")
                                    {
                                        day17 = "A";
                                    }

                                    day18 = dr["18"].ToString().Trim();
                                    if (day18.Length == 0 || day18 == "0")
                                    {
                                        day18 = "A";
                                    }

                                    day19 = dr["19"].ToString().Trim();
                                    if (day19.Length == 0 || day19 == "0")
                                    {
                                        day19 = "A";
                                    }

                                    day20 = dr["20"].ToString().Trim();
                                    if (day20.Length == 0 || day20 == "0")
                                    {
                                        day20 = "A";
                                    }

                                    day21 = dr["21"].ToString().Trim();
                                    if (day21.Length == 0 || day21 == "0")
                                    {
                                        day21 = "A";
                                    }

                                    day22 = dr["22"].ToString().Trim();
                                    if (day22.Length == 0 || day22 == "0")
                                    {
                                        day22 = "A";
                                    }

                                    day23 = dr["23"].ToString().Trim();
                                    if (day23.Length == 0 || day23 == "0")
                                    {
                                        day23 = "A";
                                    }

                                    day24 = dr["24"].ToString().Trim();
                                    if (day24.Length == 0 || day24 == "0")
                                    {
                                        day24 = "A";
                                    }

                                    day25 = dr["25"].ToString().Trim();
                                    if (day25.Length == 0 || day25 == "0")
                                    {
                                        day25 = "A";
                                    }

                                    day26 = dr["26"].ToString().Trim();
                                    if (day26.Length == 0 || day26 == "0")
                                    {
                                        day26 = "A";
                                    }

                                    day27 = dr["27"].ToString().Trim();
                                    if (day27.Length == 0 || day27 == "0")
                                    {
                                        day27 = "A";
                                    }

                                    day28 = dr["28"].ToString().Trim();
                                    if (day28.Length == 0 || day28 == "0")
                                    {
                                        day28 = "A";
                                    }

                                    day29 = dr["29"].ToString().Trim();
                                    if (day29.Length == 0 || day29 == "0")
                                    {
                                        day29 = "A";
                                    }
                                }
                                if (days == 28)
                                {
                                    day1 = dr["1"].ToString().Trim();
                                    if (day1.Trim().Length == 0 || day1 == "0")
                                    { day1 = "A"; }

                                    day2 = dr["2"].ToString().Trim();
                                    if (day2.Length == 0 || day2 == "0")
                                    { day2 = "A"; }

                                    day3 = dr["3"].ToString().Trim();
                                    if (day3.Length == 0 || day3 == "0")
                                    { day3 = "A"; }

                                    day4 = dr["4"].ToString().Trim();
                                    if (day4.Length == 0 || day4 == "0")
                                    {
                                        day4 = "A";
                                    }

                                    day5 = dr["5"].ToString().Trim();
                                    if (day5.Length == 0 || day5 == "0")
                                    {
                                        day5 = "A";
                                    }

                                    day6 = dr["6"].ToString().Trim();
                                    if (day6.Length == 0 || day6 == "0")
                                    {
                                        day6 = "A";
                                    }

                                    day7 = dr["7"].ToString().Trim();
                                    if (day7.Length == 0 || day7 == "0")
                                    {
                                        day7 = "A";
                                    }

                                    day8 = dr["8"].ToString().Trim();
                                    if (day8.Length == 0 || day8 == "0")
                                    {
                                        day8 = "A";
                                    }

                                    day9 = dr["9"].ToString().Trim();
                                    if (day9.Length == 0 || day9 == "0")
                                    {
                                        day9 = "A";
                                    }

                                    day10 = dr["10"].ToString().Trim();
                                    if (day10.Length == 0 || day10 == "0")
                                    {
                                        day10 = "A";
                                    }

                                    day11 = dr["11"].ToString().Trim();
                                    if (day11.Length == 0 || day11 == "0")
                                    {
                                        day11 = "A";
                                    }

                                    day12 = dr["12"].ToString().Trim();
                                    if (day12.Length == 0 || day12 == "0")
                                    {
                                        day12 = "A";
                                    }

                                    day13 = dr["13"].ToString().Trim();
                                    if (day13.Length == 0 || day13 == "0")
                                    {
                                        day13 = "A";
                                    }

                                    day14 = dr["14"].ToString().Trim();
                                    if (day14.Length == 0 || day14 == "0")
                                    {
                                        day14 = "A";
                                    }

                                    day15 = dr["15"].ToString().Trim();
                                    if (day15.Length == 0 || day15 == "0")
                                    {
                                        day15 = "A";
                                    }

                                    day16 = dr["16"].ToString().Trim();
                                    if (day16.Length == 0 || day16 == "0")
                                    {
                                        day16 = "A";
                                    }

                                    day17 = dr["17"].ToString().Trim();
                                    if (day17.Length == 0 || day17 == "0")
                                    {
                                        day17 = "A";
                                    }

                                    day18 = dr["18"].ToString().Trim();
                                    if (day18.Length == 0 || day18 == "0")
                                    {
                                        day18 = "A";
                                    }

                                    day19 = dr["19"].ToString().Trim();
                                    if (day19.Length == 0 || day19 == "0")
                                    {
                                        day19 = "A";
                                    }

                                    day20 = dr["20"].ToString().Trim();
                                    if (day20.Length == 0 || day20 == "0")
                                    {
                                        day20 = "A";
                                    }

                                    day21 = dr["21"].ToString().Trim();
                                    if (day21.Length == 0 || day21 == "0")
                                    {
                                        day21 = "A";
                                    }

                                    day22 = dr["22"].ToString().Trim();
                                    if (day22.Length == 0 || day22 == "0")
                                    {
                                        day22 = "A";
                                    }

                                    day23 = dr["23"].ToString().Trim();
                                    if (day23.Length == 0 || day23 == "0")
                                    {
                                        day23 = "A";
                                    }

                                    day24 = dr["24"].ToString().Trim();
                                    if (day24.Length == 0 || day24 == "0")
                                    {
                                        day24 = "A";
                                    }

                                    day25 = dr["25"].ToString().Trim();
                                    if (day25.Length == 0 || day25 == "0")
                                    {
                                        day25 = "A";
                                    }

                                    day26 = dr["26"].ToString().Trim();
                                    if (day26.Length == 0 || day26 == "0")
                                    {
                                        day26 = "A";
                                    }

                                    day27 = dr["27"].ToString().Trim();
                                    if (day27.Length == 0 || day27 == "0")
                                    {
                                        day27 = "A";
                                    }

                                    day28 = dr["28"].ToString().Trim();
                                    if (day28.Length == 0 || day28 == "0")
                                    {
                                        day28 = "A";
                                    }
                                }

                                #endregion


                                #region Values for Duties

                                //1

                                if (day1.Trim() == "P" || day1.Trim() == "p")
                                {
                                    dt1 = 1;
                                }
                                if (day1.Trim() == "A" || day1.Trim() == "a")
                                {
                                    dt1 = 0;
                                }
                                if (day1.Trim() == "OFF" || day1.Trim() == "off")
                                {
                                    wo1 = 1;
                                }
                                if (day1.Trim() == "H" || day1.Trim() == "h")
                                {
                                    dt1 = 1;
                                }
                                if (day1.Trim() == "NH" || day1.Trim() == "nh")
                                {
                                    nhs1 = 1;
                                }
                                if (day1.Trim() == "D/O" || day1.Trim() == "d/o")
                                {
                                    dt1 = 1f;
                                }
                                if (day1.Trim() == "O/C" || day1.Trim() == "o/c")
                                {
                                    dt1 = 1;
                                }

                                //2
                                if (day2.Trim() == "P" || day2.Trim() == "p")
                                {
                                    dt2 = 1;
                                }
                                if (day2.Trim() == "A" || day2.Trim() == "a")
                                {
                                    dt2 = 0;
                                }
                                if (day2.Trim() == "OFF" || day2.Trim() == "off")
                                {
                                    wo2 = 1;
                                }
                                if (day2.Trim() == "H" || day2.Trim() == "h")
                                {
                                    dt2 = 1;
                                }
                                if (day2.Trim() == "NH" || day2.Trim() == "nh")
                                {
                                    nhs2 = 1;
                                }
                                if (day2.Trim() == "D/O" || day2.Trim() == "d/o")
                                {
                                    dt2 = 1f;
                                }
                                if (day2.Trim() == "O/C" || day2.Trim() == "o/c")
                                {
                                    dt2 = 1;
                                }


                                //3
                                if (day3.Trim() == "P" || day3.Trim() == "p")
                                {
                                    dt3 = 1;
                                }
                                if (day3.Trim() == "A" || day3.Trim() == "a")
                                {
                                    dt3 = 0;
                                }
                                if (day3.Trim() == "OFF" || day3.Trim() == "off")
                                {
                                    wo3 = 1;
                                }
                                if (day3.Trim() == "H" || day3.Trim() == "h")
                                {
                                    dt3 = 1;
                                }
                                if (day3.Trim() == "NH" || day3.Trim() == "nh")
                                {
                                    nhs3 = 1;
                                }
                                if (day3.Trim() == "D/O" || day3.Trim() == "d/o")
                                {
                                    dt3 = 1f;
                                }
                                if (day3.Trim() == "O/C" || day3.Trim() == "o/c")
                                {
                                    dt3 = 1;
                                }



                                //4
                                if (day4.Trim() == "P" || day4.Trim() == "p")
                                {
                                    dt4 = 1;
                                }
                                if (day4.Trim() == "A" || day4.Trim() == "a")
                                {
                                    dt4 = 0;
                                }
                                if (day4.Trim() == "OFF" || day4.Trim() == "off")
                                {
                                    wo4 = 1;
                                }
                                if (day4.Trim() == "H" || day4.Trim() == "h")
                                {
                                    dt4 = 1;
                                }
                                if (day4.Trim() == "NH" || day4.Trim() == "nh")
                                {
                                    nhs4 = 1;
                                }
                                if (day4.Trim() == "D/O" || day4.Trim() == "d/o")
                                {
                                    dt4 = 1f;
                                }
                                if (day4.Trim() == "O/C" || day4.Trim() == "o/c")
                                {
                                    dt4 = 1;
                                }

                                //5
                                if (day5.Trim() == "P" || day5.Trim() == "p")
                                {
                                    dt5 = 1;
                                }
                                if (day5.Trim() == "A" || day5.Trim() == "a")
                                {
                                    dt5 = 0;
                                }
                                if (day5.Trim() == "OFF" || day5.Trim() == "off")
                                {
                                    wo5 = 1;
                                }
                                if (day5.Trim() == "H" || day5.Trim() == "h")
                                {
                                    dt5 = 1;
                                }
                                if (day5.Trim() == "NH" || day5.Trim() == "nh")
                                {
                                    nhs5 = 1;
                                }
                                if (day5.Trim() == "D/O" || day5.Trim() == "d/o")
                                {
                                    dt5 = 1f;
                                }
                                if (day5.Trim() == "O/C" || day5.Trim() == "o/c")
                                {
                                    dt5 = 1;
                                }

                                //6
                                if (day6.Trim() == "P" || day6.Trim() == "p")
                                {
                                    dt6 = 1;
                                }
                                if (day6.Trim() == "A" || day6.Trim() == "a")
                                {
                                    dt6 = 0;
                                }
                                if (day6.Trim() == "OFF" || day6.Trim() == "off")
                                {
                                    wo6 = 1;
                                }
                                if (day6.Trim() == "H" || day6.Trim() == "h")
                                {
                                    dt6 = 1;
                                }
                                if (day6.Trim() == "NH" || day6.Trim() == "nh")
                                {
                                    nhs6 = 1;
                                }
                                if (day6.Trim() == "D/O" || day6.Trim() == "d/o")
                                {
                                    dt6 = 1f;
                                }
                                if (day6.Trim() == "O/C" || day6.Trim() == "o/c")
                                {
                                    dt6 = 1;
                                }

                                //7
                                if (day7.Trim() == "P" || day7.Trim() == "p")
                                {
                                    dt7 = 1;
                                }
                                if (day7.Trim() == "A" || day7.Trim() == "a")
                                {
                                    dt7 = 0;
                                }
                                if (day7.Trim() == "OFF" || day7.Trim() == "off")
                                {
                                    wo7 = 1;
                                }
                                if (day7.Trim() == "H" || day7.Trim() == "h")
                                {
                                    dt7 = 1;
                                }
                                if (day7.Trim() == "NH" || day7.Trim() == "nh")
                                {
                                    nhs7 = 1;
                                }
                                if (day7.Trim() == "D/O" || day7.Trim() == "d/o")
                                {
                                    dt7 = 1f;
                                }
                                if (day7.Trim() == "O/C" || day7.Trim() == "o/c")
                                {
                                    dt7 = 1;
                                }
                                //8
                                if (day8.Trim() == "P" || day8.Trim() == "p")
                                {
                                    dt8 = 1;
                                }
                                if (day8.Trim() == "A" || day8.Trim() == "a")
                                {
                                    dt8 = 0;
                                }
                                if (day8.Trim() == "OFF" || day8.Trim() == "off")
                                {
                                    wo8 = 1;
                                }
                                if (day8.Trim() == "H" || day8.Trim() == "h")
                                {
                                    dt8 = 1;
                                }
                                if (day8.Trim() == "NH" || day8.Trim() == "nh")
                                {
                                    nhs8 = 1;
                                }
                                if (day8.Trim() == "D/O" || day8.Trim() == "d/o")
                                {
                                    dt8 = 1f;
                                }
                                if (day8.Trim() == "O/C" || day8.Trim() == "o/c")
                                {
                                    dt8 = 1;
                                }
                                //9
                                if (day9.Trim() == "P" || day9.Trim() == "p")
                                {
                                    dt9 = 1;
                                }
                                if (day9.Trim() == "A" || day9.Trim() == "a")
                                {
                                    dt9 = 0;
                                }
                                if (day9.Trim() == "OFF" || day9.Trim() == "off")
                                {
                                    wo9 = 1;
                                }
                                if (day9.Trim() == "H" || day9.Trim() == "h")
                                {
                                    dt9 = 1;
                                }
                                if (day9.Trim() == "NH" || day9.Trim() == "nh")
                                {
                                    nhs9 = 1;
                                }
                                if (day9.Trim() == "D/O" || day9.Trim() == "d/o")
                                {
                                    dt9 = 1f;
                                }
                                if (day9.Trim() == "O/C" || day9.Trim() == "o/c")
                                {
                                    dt9 = 1;
                                }

                                //10
                                if (day10.Trim() == "P" || day10.Trim() == "p")
                                {
                                    dt10 = 1;
                                }
                                if (day10.Trim() == "A" || day10.Trim() == "a")
                                {
                                    dt10 = 0;
                                }
                                if (day10.Trim() == "OFF" || day10.Trim() == "off")
                                {
                                    wo10 = 1;
                                }
                                if (day10.Trim() == "H" || day10.Trim() == "h")
                                {
                                    dt10 = 1;
                                }
                                if (day10.Trim() == "NH" || day10.Trim() == "nh")
                                {
                                    nhs10 = 1;
                                }
                                if (day10.Trim() == "D/O" || day10.Trim() == "d/o")
                                {
                                    dt10 = 1f;
                                }
                                if (day10.Trim() == "O/C" || day10.Trim() == "o/c")
                                {
                                    dt10 = 1;
                                }
                                //11

                                if (day11.Trim() == "P" || day11.Trim() == "p")
                                {
                                    dt11 = 1;
                                }
                                if (day11.Trim() == "A" || day11.Trim() == "a")
                                {
                                    dt11 = 0;
                                }
                                if (day11.Trim() == "OFF" || day11.Trim() == "off")
                                {
                                    wo11 = 1;
                                }
                                if (day11.Trim() == "H" || day11.Trim() == "h")
                                {
                                    dt11 = 1;
                                }
                                if (day11.Trim() == "NH" || day11.Trim() == "nh")
                                {
                                    nhs11 = 1;
                                }
                                if (day11.Trim() == "D/O" || day11.Trim() == "d/o")
                                {
                                    dt11 = 1f;
                                }
                                if (day11.Trim() == "O/C" || day11.Trim() == "o/c")
                                {
                                    dt11 = 1;
                                }
                                //12
                                if (day12.Trim() == "P" || day12.Trim() == "p")
                                {
                                    dt12 = 1;
                                }
                                if (day12.Trim() == "A" || day12.Trim() == "a")
                                {
                                    dt12 = 0;
                                }
                                if (day12.Trim() == "OFF" || day12.Trim() == "off")
                                {
                                    wo12 = 1;
                                }
                                if (day12.Trim() == "H" || day12.Trim() == "h")
                                {
                                    dt12 = 1;
                                }
                                if (day12.Trim() == "NH" || day12.Trim() == "nh")
                                {
                                    nhs12 = 1;
                                }
                                if (day12.Trim() == "D/O" || day12.Trim() == "d/o")
                                {
                                    dt12 = 1f;
                                }
                                if (day12.Trim() == "O/C" || day12.Trim() == "o/c")
                                {
                                    dt12 = 1;
                                }
                                //13
                                if (day13.Trim() == "P" || day13.Trim() == "p")
                                {
                                    dt13 = 1;
                                }
                                if (day13.Trim() == "A" || day13.Trim() == "a")
                                {
                                    dt13 = 0;
                                }
                                if (day13.Trim() == "OFF" || day13.Trim() == "off")
                                {
                                    wo13 = 1;
                                }
                                if (day13.Trim() == "H" || day13.Trim() == "h")
                                {
                                    dt13 = 1;
                                }
                                if (day13.Trim() == "NH" || day13.Trim() == "nh")
                                {
                                    nhs13 = 1;
                                }
                                if (day13.Trim() == "D/O" || day13.Trim() == "d/o")
                                {
                                    dt13 = 1f;
                                }
                                if (day13.Trim() == "O/C" || day13.Trim() == "o/c")
                                {
                                    dt13 = 1;
                                }
                                //14
                                if (day14.Trim() == "P" || day14.Trim() == "p")
                                {
                                    dt14 = 1;
                                }
                                if (day14.Trim() == "A" || day14.Trim() == "a")
                                {
                                    dt14 = 0;
                                }
                                if (day14.Trim() == "OFF" || day14.Trim() == "off")
                                {
                                    wo14 = 1;
                                }
                                if (day14.Trim() == "H" || day14.Trim() == "h")
                                {
                                    dt14 = 1;
                                }
                                if (day14.Trim() == "NH" || day14.Trim() == "nh")
                                {
                                    nhs14 = 1;
                                }
                                if (day14.Trim() == "D/O" || day14.Trim() == "d/o")
                                {
                                    dt14 = 1f;
                                }
                                if (day14.Trim() == "O/C" || day14.Trim() == "o/c")
                                {
                                    dt14 = 1;
                                }
                                //15
                                if (day15.Trim() == "P" || day15.Trim() == "p")
                                {
                                    dt15 = 1;
                                }
                                if (day15.Trim() == "A" || day15.Trim() == "a")
                                {
                                    dt15 = 0;
                                }
                                if (day15.Trim() == "OFF" || day15.Trim() == "off")
                                {
                                    wo15 = 1;
                                }
                                if (day15.Trim() == "H" || day15.Trim() == "h")
                                {
                                    dt15 = 1;
                                }
                                if (day15.Trim() == "NH" || day15.Trim() == "nh")
                                {
                                    nhs15 = 1;
                                }
                                if (day15.Trim() == "D/O" || day15.Trim() == "d/o")
                                {
                                    dt15 = 1f;
                                }
                                if (day15.Trim() == "O/C" || day15.Trim() == "o/c")
                                {
                                    dt15 = 1;
                                }
                                //16
                                if (day16.Trim() == "P" || day16.Trim() == "p")
                                {
                                    dt16 = 1;
                                }
                                if (day16.Trim() == "A" || day16.Trim() == "a")
                                {
                                    dt16 = 0;
                                }
                                if (day16.Trim() == "OFF" || day16.Trim() == "off")
                                {
                                    wo16 = 1;
                                }
                                if (day16.Trim() == "H" || day16.Trim() == "h")
                                {
                                    dt16 = 1;
                                }
                                if (day16.Trim() == "NH" || day16.Trim() == "nh")
                                {
                                    nhs16 = 1;
                                }
                                if (day16.Trim() == "D/O" || day16.Trim() == "d/o")
                                {
                                    dt16 = 1f;
                                }
                                if (day16.Trim() == "O/C" || day16.Trim() == "o/c")
                                {
                                    dt16 = 1;
                                }
                                //17
                                if (day17.Trim() == "P" || day17.Trim() == "p")
                                {
                                    dt17 = 1;
                                }
                                if (day17.Trim() == "A" || day17.Trim() == "a")
                                {
                                    dt17 = 0;
                                }
                                if (day17.Trim() == "OFF" || day17.Trim() == "off")
                                {
                                    wo17 = 1;
                                }
                                if (day17.Trim() == "H" || day17.Trim() == "h")
                                {
                                    dt17 = 1;
                                }
                                if (day17.Trim() == "NH" || day17.Trim() == "nh")
                                {
                                    nhs17 = 1;
                                }
                                if (day17.Trim() == "D/O" || day17.Trim() == "d/o")
                                {
                                    dt17 = 1f;
                                }
                                if (day17.Trim() == "O/C" || day17.Trim() == "o/c")
                                {
                                    dt17 = 1;
                                }
                                //18
                                if (day18.Trim() == "P" || day18.Trim() == "p")
                                {
                                    dt18 = 1;
                                }
                                if (day18.Trim() == "A" || day18.Trim() == "a")
                                {
                                    dt18 = 0;
                                }
                                if (day18.Trim() == "OFF" || day18.Trim() == "off")
                                {
                                    wo18 = 1;
                                }
                                if (day18.Trim() == "H" || day18.Trim() == "h")
                                {
                                    dt18 = 1;
                                }
                                if (day18.Trim() == "NH" || day18.Trim() == "nh")
                                {
                                    nhs18 = 1;
                                }
                                if (day18.Trim() == "D/O" || day18.Trim() == "d/o")
                                {
                                    dt18 = 1f;
                                }
                                if (day18.Trim() == "O/C" || day18.Trim() == "o/c")
                                {
                                    dt18 = 1;
                                }
                                //19
                                if (day19.Trim() == "P" || day19.Trim() == "p")
                                {
                                    dt19 = 1;
                                }
                                if (day19.Trim() == "A" || day19.Trim() == "a")
                                {
                                    dt19 = 0;
                                }
                                if (day19.Trim() == "OFF" || day19.Trim() == "off")
                                {
                                    wo19 = 1;
                                }
                                if (day19.Trim() == "H" || day19.Trim() == "h")
                                {
                                    dt19 = 1;
                                }
                                if (day19.Trim() == "NH" || day19.Trim() == "nh")
                                {
                                    nhs19 = 1;
                                }
                                if (day19.Trim() == "D/O" || day19.Trim() == "d/o")
                                {
                                    dt19 = 1f;
                                }
                                if (day19.Trim() == "O/C" || day19.Trim() == "o/c")
                                {
                                    dt19 = 1;
                                }
                                //20
                                if (day20.Trim() == "P" || day20.Trim() == "p")
                                {
                                    dt20 = 1;
                                }
                                if (day20.Trim() == "A" || day20.Trim() == "a")
                                {
                                    dt20 = 0;
                                }
                                if (day20.Trim() == "OFF" || day20.Trim() == "off")
                                {
                                    wo20 = 1;
                                }
                                if (day20.Trim() == "H" || day20.Trim() == "h")
                                {
                                    dt20 = 1;
                                }
                                if (day20.Trim() == "NH" || day20.Trim() == "nh")
                                {
                                    nhs20 = 1;
                                }
                                if (day20.Trim() == "D/O" || day20.Trim() == "d/o")
                                {
                                    dt20 = 1f;
                                }
                                if (day20.Trim() == "O/C" || day20.Trim() == "o/c")
                                {
                                    dt20 = 1;
                                }
                                //21
                                if (day21.Trim() == "P" || day21.Trim() == "p")
                                {
                                    dt21 = 1;
                                }
                                if (day21.Trim() == "A" || day21.Trim() == "a")
                                {
                                    dt21 = 0;
                                }
                                if (day21.Trim() == "OFF" || day21.Trim() == "off")
                                {
                                    wo21 = 1;
                                }
                                if (day21.Trim() == "H" || day21.Trim() == "h")
                                {
                                    dt21 = 1;
                                }
                                if (day21.Trim() == "NH" || day21.Trim() == "nh")
                                {
                                    nhs21 = 1;
                                }
                                if (day21.Trim() == "D/O" || day21.Trim() == "d/o")
                                {
                                    dt21 = 1f;
                                }
                                if (day21.Trim() == "O/C" || day21.Trim() == "o/c")
                                {
                                    dt21 = 1;
                                }
                                //22
                                if (day22.Trim() == "P" || day22.Trim() == "p")
                                {
                                    dt22 = 1;
                                }
                                if (day22.Trim() == "A" || day22.Trim() == "a")
                                {
                                    dt22 = 0;
                                }
                                if (day22.Trim() == "OFF" || day22.Trim() == "off")
                                {
                                    wo22 = 1;
                                }
                                if (day22.Trim() == "H" || day22.Trim() == "h")
                                {
                                    dt22 = 1;
                                }
                                if (day22.Trim() == "NH" || day22.Trim() == "nh")
                                {
                                    nhs22 = 1;
                                }
                                if (day22.Trim() == "D/O" || day22.Trim() == "d/o")
                                {
                                    dt22 = 1f;
                                }
                                if (day22.Trim() == "O/C" || day22.Trim() == "o/c")
                                {
                                    dt22 = 1;
                                }
                                //23
                                if (day23.Trim() == "P" || day23.Trim() == "p")
                                {
                                    dt23 = 1;
                                }
                                if (day23.Trim() == "A" || day23.Trim() == "a")
                                {
                                    dt23 = 0;
                                }
                                if (day23.Trim() == "OFF" || day23.Trim() == "off")
                                {
                                    wo23 = 1;
                                }
                                if (day23.Trim() == "H" || day23.Trim() == "h")
                                {
                                    dt23 = 1;
                                }
                                if (day23.Trim() == "NH" || day23.Trim() == "nh")
                                {
                                    nhs23 = 1;
                                }
                                if (day23.Trim() == "D/O" || day23.Trim() == "d/o")
                                {
                                    dt23 = 1f;
                                }
                                if (day23.Trim() == "O/C" || day23.Trim() == "o/c")
                                {
                                    dt23 = 1;
                                }
                                //24
                                if (day24.Trim() == "P" || day24.Trim() == "p")
                                {
                                    dt24 = 1;
                                }
                                if (day24.Trim() == "A" || day24.Trim() == "a")
                                {
                                    dt24 = 0;
                                }
                                if (day24.Trim() == "OFF" || day24.Trim() == "off")
                                {
                                    wo24 = 1;
                                }
                                if (day24.Trim() == "H" || day24.Trim() == "h")
                                {
                                    dt24 = 1;
                                }
                                if (day24.Trim() == "NH" || day24.Trim() == "nh")
                                {
                                    nhs24 = 1;
                                }
                                if (day24.Trim() == "D/O" || day24.Trim() == "d/o")
                                {
                                    dt24 = 1f;
                                }
                                if (day24.Trim() == "O/C" || day24.Trim() == "o/c")
                                {
                                    dt24 = 1;
                                }
                                //25
                                if (day25.Trim() == "P" || day25.Trim() == "p")
                                {
                                    dt25 = 1;
                                }
                                if (day25.Trim() == "A" || day25.Trim() == "a")
                                {
                                    dt25 = 0;
                                }
                                if (day25.Trim() == "OFF" || day25.Trim() == "off")
                                {
                                    wo25 = 1;
                                }
                                if (day25.Trim() == "H" || day25.Trim() == "h")
                                {
                                    dt25 = 1;
                                }
                                if (day25.Trim() == "NH" || day25.Trim() == "nh")
                                {
                                    nhs25 = 1;
                                }
                                if (day25.Trim() == "D/O" || day25.Trim() == "d/o")
                                {
                                    dt25 = 1f;
                                }
                                if (day25.Trim() == "O/C" || day25.Trim() == "o/c")
                                {
                                    dt25 = 1;
                                }
                                //26
                                if (day26.Trim() == "P" || day26.Trim() == "p")
                                {
                                    dt26 = 1;
                                }
                                if (day26.Trim() == "A" || day26.Trim() == "a")
                                {
                                    dt26 = 0;
                                }
                                if (day26.Trim() == "OFF" || day26.Trim() == "off")
                                {
                                    wo26 = 1;
                                }
                                if (day26.Trim() == "H" || day26.Trim() == "h")
                                {
                                    dt26 = 1;
                                }
                                if (day26.Trim() == "NH" || day26.Trim() == "nh")
                                {
                                    nhs26 = 1;
                                }
                                if (day26.Trim() == "D/O" || day26.Trim() == "d/o")
                                {
                                    dt26 = 1f;
                                }
                                if (day26.Trim() == "O/C" || day26.Trim() == "o/c")
                                {
                                    dt26 = 1;
                                }
                                //27
                                if (day27.Trim() == "P" || day27.Trim() == "p")
                                {
                                    dt27 = 1;
                                }
                                if (day27.Trim() == "A" || day27.Trim() == "a")
                                {
                                    dt27 = 0;
                                }
                                if (day27.Trim() == "OFF" || day27.Trim() == "off")
                                {
                                    wo27 = 1;
                                }
                                if (day27.Trim() == "H" || day27.Trim() == "h")
                                {
                                    dt27 = 1;
                                }
                                if (day27.Trim() == "NH" || day27.Trim() == "nh")
                                {
                                    nhs27 = 1;
                                }
                                if (day27.Trim() == "D/O" || day27.Trim() == "d/o")
                                {
                                    dt27 = 1f;
                                }
                                if (day27.Trim() == "O/C" || day27.Trim() == "o/c")
                                {
                                    dt27 = 1;
                                }
                                //28
                                if (day28.Trim() == "P" || day28.Trim() == "p")
                                {
                                    dt28 = 1;
                                }
                                if (day28.Trim() == "A" || day28.Trim() == "a")
                                {
                                    dt28 = 0;
                                }
                                if (day28.Trim() == "OFF" || day28.Trim() == "off")
                                {
                                    wo28 = 1;
                                }
                                if (day28.Trim() == "H" || day28.Trim() == "h")
                                {
                                    dt28 = 1;
                                }
                                if (day28.Trim() == "NH" || day28.Trim() == "nh")
                                {
                                    nhs28 = 1;
                                }
                                if (day28.Trim() == "D/O" || day28.Trim() == "d/o")
                                {
                                    dt28 = 1f;
                                }
                                if (day28.Trim() == "O/C" || day28.Trim() == "o/c")
                                {
                                    dt28 = 1;
                                }
                                //29
                                if (day29.Trim() == "P" || day29.Trim() == "p")
                                {
                                    dt29 = 1;
                                }
                                if (day29.Trim() == "A" || day29.Trim() == "a")
                                {
                                    dt29 = 0;
                                }
                                if (day29.Trim() == "OFF" || day29.Trim() == "off")
                                {
                                    wo29 = 1;
                                }
                                if (day29.Trim() == "H" || day29.Trim() == "h")
                                {
                                    dt29 = 1;
                                }
                                if (day29.Trim() == "NH" || day29.Trim() == "nh")
                                {
                                    nhs29 = 1;
                                }
                                if (day29.Trim() == "D/O" || day29.Trim() == "d/o")
                                {
                                    dt29 = 1f;
                                }
                                if (day29.Trim() == "O/C" || day29.Trim() == "o/c")
                                {
                                    dt29 = 1;
                                }
                                //30
                                if (day30.Trim() == "P" || day30.Trim() == "p")
                                {
                                    dt30 = 1;
                                }
                                if (day30.Trim() == "A" || day30.Trim() == "a")
                                {
                                    dt30 = 0;
                                }
                                if (day30.Trim() == "OFF" || day30.Trim() == "off")
                                {
                                    wo30 = 1;
                                }
                                if (day30.Trim() == "H" || day30.Trim() == "h")
                                {
                                    dt30 = 1;
                                }
                                if (day30.Trim() == "NH" || day30.Trim() == "nh")
                                {
                                    nhs30 = 1;
                                }
                                if (day30.Trim() == "D/O" || day30.Trim() == "d/o")
                                {
                                    dt30 = 1f;
                                }
                                if (day30.Trim() == "O/C" || day30.Trim() == "o/c")
                                {
                                    dt30 = 1;
                                }
                                //31
                                if (day31.Trim() == "P" || day31.Trim() == "p")
                                {
                                    dt31 = 1;
                                }
                                if (day31.Trim() == "A" || day31.Trim() == "a")
                                {
                                    dt31 = 0;
                                }
                                if (day31.Trim() == "OFF" || day31.Trim() == "off")
                                {
                                    wo31 = 1;
                                }
                                if (day31.Trim() == "H" || day31.Trim() == "h")
                                {
                                    dt31 = 1;
                                }
                                if (day31.Trim() == "NH" || day31.Trim() == "nh")
                                {
                                    nhs31 = 1;
                                }
                                if (day31.Trim() == "D/O" || day31.Trim() == "d/o")
                                {
                                    dt31 = 1f;
                                }
                                if (day31.Trim() == "O/C" || day31.Trim() == "o/c")
                                {
                                    dt31 = 1;
                                }

                                #endregion Values for Duties

                                dayduties = dt1 + dt2 + dt3 + dt4 + dt5 + dt6 + dt7 + dt8 + dt9 + dt10 + dt11 + dt12 + dt13 + dt14 + dt15 + dt16 + dt17 + dt18 + dt19 + dt20 + dt21 +
                             dt22 + dt23 + dt24 + dt25 + dt26 + dt27 + dt28 + dt29 + dt30 + dt31;

                                dayots = Ots1 + Ots2 + Ots3 + Ots4 + Ots5 + Ots6 + Ots7 + Ots8 + Ots9 + Ots10 + Ots11 + Ots12 + Ots13 + Ots14 + Ots15 + Ots16 + Ots17 + Ots18 + Ots19 + Ots20 + Ots21 +
                           Ots22 + Ots23 + Ots24 + Ots25 + Ots26 + Ots27 + Ots28 + Ots29 + Ots30 + Ots31;

                                daywos = wo1 + wo2 + wo3 + wo4 + wo5 + wo6 + wo7 + wo8 + wo9 + wo10 + wo11 + wo12 + wo13 + wo14 + wo15 + wo16 + wo17 + wo18 + wo19 + wo20 + wo21 +
                             wo22 + wo23 + wo24 + wo25 + wo26 + wo27 + wo28 + wo29 + wo30 + wo31;

                                dayNHS = nhs1 + nhs2 + nhs3 + nhs4 + nhs5 + nhs6 + nhs7 + nhs8 + nhs9 + nhs10 + nhs11 + nhs12 + nhs13 + nhs14 + nhs15 + nhs16 + nhs17 + nhs18 + nhs19 + nhs20 + nhs21 +
                             nhs22 + nhs23 + nhs24 + nhs25 + nhs26 + nhs27 + nhs28 + nhs29 + nhs30 + nhs31;


                                duties = dayduties + duties;
                                ots = dayots + ots;
                                Wos = daywos + Wos;

                                NHS = dayNHS;

                            }


                            #region Begin New code for Stored Procedure as on 29/04/2014 by venkat


                            #region Begin code for passing values to the Stored Procedure as 29/04/2014 by Venkat

                            Hashtable Httable = new Hashtable();

                            Httable.Add("@k", k);
                            Httable.Add("@empidstatus", empstatus);


                            Httable.Add("@Clientid", clientid);
                            Httable.Add("@Month", Month);
                            Httable.Add("@Empid", empid);
                            Httable.Add("@ContractId", Contractid);
                            Httable.Add("@Design", design);

                            Httable.Add("@day1", day1);
                            Httable.Add("@day2", day2);
                            Httable.Add("@day3", day3);
                            Httable.Add("@day4", day4);
                            Httable.Add("@day5", day5);
                            Httable.Add("@day6", day6);
                            Httable.Add("@day7", day7);
                            Httable.Add("@day8", day8);
                            Httable.Add("@day9", day9);
                            Httable.Add("@day10", day10);
                            Httable.Add("@day11", day11);
                            Httable.Add("@day12", day12);
                            Httable.Add("@day13", day13);
                            Httable.Add("@day14", day14);
                            Httable.Add("@day15", day15);
                            Httable.Add("@day16", day16);
                            Httable.Add("@day17", day17);
                            Httable.Add("@day18", day18);
                            Httable.Add("@day19", day19);
                            Httable.Add("@day20", day20);
                            Httable.Add("@day21", day21);
                            Httable.Add("@day22", day22);
                            Httable.Add("@day23", day23);
                            Httable.Add("@day24", day24);
                            Httable.Add("@day25", day25);
                            Httable.Add("@day26", day26);
                            Httable.Add("@day27", day27);
                            Httable.Add("@day28", day28);
                            Httable.Add("@day29", day29);
                            Httable.Add("@day30", day30);
                            Httable.Add("@day31", day31);

                            Httable.Add("@day1ot", day1ot);
                            Httable.Add("@day2ot", day2ot);
                            Httable.Add("@day3ot", day3ot);
                            Httable.Add("@day4ot", day4ot);
                            Httable.Add("@day5ot", day5ot);
                            Httable.Add("@day6ot", day6ot);
                            Httable.Add("@day7ot", day7ot);
                            Httable.Add("@day8ot", day8ot);
                            Httable.Add("@day9ot", day9ot);
                            Httable.Add("@day10ot", day10ot);
                            Httable.Add("@day11ot", day11ot);
                            Httable.Add("@day12ot", day12ot);
                            Httable.Add("@day13ot", day13ot);
                            Httable.Add("@day14ot", day14ot);
                            Httable.Add("@day15ot", day15ot);
                            Httable.Add("@day16ot", day16ot);
                            Httable.Add("@day17ot", day17ot);
                            Httable.Add("@day18ot", day18ot);
                            Httable.Add("@day19ot", day19ot);
                            Httable.Add("@day20ot", day20ot);
                            Httable.Add("@day21ot", day21ot);
                            Httable.Add("@day22ot", day22ot);
                            Httable.Add("@day23ot", day23ot);
                            Httable.Add("@day24ot", day24ot);
                            Httable.Add("@day25ot", day25ot);
                            Httable.Add("@day26ot", day26ot);
                            Httable.Add("@day27ot", day27ot);
                            Httable.Add("@day28ot", day28ot);
                            Httable.Add("@day29ot", day29ot);
                            Httable.Add("@day30ot", day30ot);
                            Httable.Add("@day31ot", day31ot);

                            Httable.Add("@Duties", duties);
                            Httable.Add("@Ots", ots);
                            Httable.Add("@WOs", Wos);
                            Httable.Add("@NHs", NHS);
                            Httable.Add("@Sno", Sno);
                            Httable.Add("@Excel_Number", ExcelNo);
                            Httable.Add("@pldays", pldays);
                            Httable.Add("@CanteenAdv", canteenadvance);
                            Httable.Add("@Penalty", penalty);
                            Httable.Add("@Incentivs", incentives);
                            Httable.Add("@UniformDed", UniformDed);
                            Httable.Add("@ATMDed", ATMDed);
                            Httable.Add("@OTHours", OTHrs);
                            Httable.Add("@Arrears", Arrears);
                            Httable.Add("@AttBonus", AttBonus);
                            Httable.Add("@RemarksText", RemarksText);
                            Httable.Add("@Stoppayment", stoppayment);
                            Httable.Add("@RentDed", RentDed);
                            Httable.Add("@BGVDed", BGVDed); 
                            Httable.Add("@IncentiveHrs", IncentiveHrs);
                            Httable.Add("@IncentiveAmt", IncentiveAmt);
                            Httable.Add("@LCHrs", LCHrs);
                            Httable.Add("@LCAmt", LCAmt);
                            Httable.Add("@RowNo", j+2);
                            #endregion

                            string SPName = "ImportAttendanceFromExcel";
                            DataTable dtstatus = config.ExecuteAdaptorWithParams(SPName, Httable).Result;


                            #endregion


                        }
                    }

                    k++;
                    j++;
                }

                string SPBillName = "SaveBillAttendance";
                Hashtable HtBilltable = new Hashtable();
                HtBilltable.Add("@Clientid", clientid);
                HtBilltable.Add("@Month", Month);
                DataTable dtBillstatus = config.ExecuteAdaptorAsyncWithParams(SPBillName, HtBilltable).Result;


                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }


            }




            #endregion




            LoadExcelNos();
            ddlAttendanceMode.SelectedIndex = 0;
            GetAttSummary(ExcelNo.ToString().Trim());

        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (gvnotinsert.Rows.Count > 0)
            {
                util.NewExport("Unsaveddata.xlsx", this.gvnotinsert);
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            if (ddlClientID.SelectedIndex > 0 && txtmonth.Text.Trim().Length != 0)
            {

                string sqldeleteempattendance = "delete empattendance where clientid='" + ddlClientID.SelectedValue + "' and month='" + Month + "'";
                int status = config.ExecuteNonQueryWithQueryAsync(sqldeleteempattendance).Result;
                if (status > 0)
                {
                    GetAttSummary("");
                    
                }


            }
        }

        




        protected void ddloption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddloption.SelectedIndex == 1)
            {
                LoadClientList();
                LoadClientNames();
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlClientID.Visible = true;
                ddlCName.Visible = true;
                btnClear.Visible = true;
            }
            else
            {
                lblclientid.Visible = false;
                lblclientname.Visible = false;
                ddlClientID.Visible = false;
                ddlCName.Visible = false;
                btnClear.Visible = false;
            }
        }

        protected void lnkempnameImportfromexcel_Click(object sender, EventArgs e)
        {
            if (ddloption.SelectedIndex == 1)
            {
                if (ddlClientID.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Select Clientid');", true);
                    return;
                }
            }

            GetSampledata();
            util.NewExport("Employee Attendance.xlsx", this.GridView3);
        }
        protected void GetSampledata()
        {
            string date = string.Empty;
            int days = 0;
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            int year = Convert.ToDateTime(date).Year;
            int monthn = Convert.ToDateTime(date).Month;
            days = DateTime.DaysInMonth(year, monthn);

            string SPName = "SampleAttendanceDetails";
            Hashtable ht = new Hashtable();

            ht.Add("@ClientIDList", ddlClientID.SelectedValue);
            ht.Add("@Option", ddlAttendanceMode.SelectedIndex);
            ht.Add("@EmpIDType", ddlempidtype.SelectedIndex);
            ht.Add("@Type", ddloption.SelectedIndex);
            ht.Add("@MonthDays", days);
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;
            if (dt.Rows.Count > 0)
            {
                GridView3.DataSource = dt;
                GridView3.DataBind();

            }


        }
    }
}