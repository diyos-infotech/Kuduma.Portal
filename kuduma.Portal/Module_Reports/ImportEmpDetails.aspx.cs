using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ImportEmpDetails : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string CmpIDPrefix = "";
        string EmpIDPrefix = "";
        string UserID = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                GetSampleExport();
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

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired . Please Login');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            UserID = Session["UserId"].ToString();
        }
        public void GetSampleExport()
        {
            string qry = "select top 1 '' as IDNO,'' as 'Employee Name','' as 'Fathers Name','' as 'Mother Name','' as 'Date of Birth','' as Sex,'' as 'Marital Status','' as Designation,''  as 'Mobile No','' as 'UAN Number','' as 'Aadhar Number','' as 'PAN Number','' as 'Present address','' as 'Permanent Address','' as Department,''as 'Client ID','' as Branch,'' as Division,'' as 'Bank Account No','' as 'IFSC','' as 'Bank Name', '' as 'Date of Joining', '' as 'Date of leaving','' as 'ESI Applicable','' as 'ESI No', '' as 'PF Applicable', '' as 'PF No','' as 'PT Applicable','' as 'Employee Type','' as 'Old Emp ID'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count > 0)
            {
                gvlistofemp.DataSource = dt;
                gvlistofemp.DataBind();

            }
            else
            {
                gvlistofemp.DataSource = null;
                gvlistofemp.DataBind();
            }
        }

        protected void lnkImportfromexcel_Click(object sender, EventArgs e)
        {
            gve.Export("Sampleempdetails.xls", this.gvlistofemp);
        }
        DataTable dt = new DataTable();
        protected void btnsave_Click(object sender, EventArgs e)
        {
            int result = 0;
            string ExcelSheetname = "";
            string FileName = FileUploadEmpDetails.FileName;
            string path = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(FileUploadEmpDetails.PostedFile.FileName));
            FileUploadEmpDetails.PostedFile.SaveAs(path);

            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;");
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();

            OleDbCommand cmd = new OleDbCommand("Select [IDNO],[Employee Name],[Fathers Name],[Mother Name],[Date of Birth],[Sex],[Marital Status],[Designation],[Mobile No],[Department],[UAN Number],[Aadhar Number],[PAN Number],[Present Address],[Permanent Address],[Client ID],[Branch],[Division],[Bank Account No],[IFSC],[Bank Name],[Date of Joining],[Date of leaving],[ESI Applicable],[ESI No],[PF Applicable],[PF No],[PT Applicable],[Employee Type],[Old Emp ID] from [" + ExcelSheetname + "]", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);


            using (SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString))
            {
                sqlcon.Open();

                string IDNO = ""; string EmployeeName = ""; string FathersName = ""; string DateofBirth = ""; string Sex = ""; string MaritalStatus = ""; string Designation = "";
                string ClientId = ""; string Branch = ""; string Division = ""; string BankAccountNo = ""; string BankName = ""; string DateofJoining = ""; string Dateofleaving = ""; string ESIApplicable = ""; string ESINo = ""; string PFApplicable = ""; string PFNo = "";
                var EmpPFDeduct = 0; var EmpESIDeduct = 0; string Gender = ""; string mstatus = ""; string MotherName = ""; string IFSC = "";
                string MobileNo = ""; string UANNumber = ""; string AadharNumber = ""; string PANNumber = ""; string PresentAddress = "";
                string PermanentAddress = ""; string PTApplicable = ""; var EmpPTDeduct = 0; var EmployeeType = "G";
                string Created_By = ""; string Created_On = ""; string Excel_Number = ""; string Department = ""; string OldEmpID = "";

                #region Begin Getmax Id from DB
                int ExcelNo = 0;
                string selectquery = "select max(cast(Excel_Number as int )) as Id from empdetails ";
                DataTable dtExcelID = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                if (dtExcelID.Rows.Count > 0)
                {
                    if (String.IsNullOrEmpty(dtExcelID.Rows[0]["Id"].ToString()) == false)
                    {
                        ExcelNo = Convert.ToInt32(dtExcelID.Rows[0]["Id"].ToString()) + 1;
                    }
                    else
                    {
                        ExcelNo = int.Parse("1");
                    }
                }
                #endregion End Getmax Id from DB

                for (int i = 0; i < ds.Rows.Count; i++)
                {



                    Created_On = DateTime.Now.ToString("dd/MM/yyyy");
                    Created_On = Timings.Instance.CheckDateFormat(Created_On);


                    IDNO = ds.Rows[i]["IDNO"].ToString();
                    EmployeeName = ds.Rows[i]["Employee Name"].ToString();
                    FathersName = ds.Rows[i]["Fathers Name"].ToString();
                    MotherName = ds.Rows[i]["Mother Name"].ToString();
                    Department = ds.Rows[i]["Department"].ToString();

                    DateofBirth = ds.Rows[i]["Date of Birth"].ToString();

                    string DateofBirth1 = "";
                    if (DateofBirth.Length > 0)
                    {
                        string db1 = Convert.ToDateTime(DateofBirth).ToString("dd/MM/yyyy");
                        DateofBirth1 = Timings.Instance.CheckDateFormat(db1);
                    }
                    else
                    {
                        DateofBirth1 = "01/01/1900";
                    }


                    Sex = ds.Rows[i]["Sex"].ToString();
                    if (Sex == "F" || Sex == "f")
                    {
                        Gender = "F";
                    }
                    else
                    {
                        Gender = "M";
                    }


                    MaritalStatus = ds.Rows[i]["Marital Status"].ToString();
                    if (MaritalStatus == "S" || MaritalStatus == "s")
                    {
                        mstatus = "S";
                    }
                    else if (MaritalStatus == "D" || MaritalStatus == "d")
                    {
                        mstatus = "D";
                    }
                    else if (MaritalStatus == "W" || MaritalStatus == "w")
                    {
                        mstatus = "W";
                    }
                    else
                    {
                        mstatus = "M";
                    }
                    Designation = ds.Rows[i]["Designation"].ToString();
                    MobileNo = ds.Rows[i]["Mobile No"].ToString();
                    UANNumber = ds.Rows[i]["UAN Number"].ToString();
                    AadharNumber = ds.Rows[i]["Aadhar Number"].ToString();
                    PANNumber = ds.Rows[i]["PAN Number"].ToString();
                    PresentAddress = ds.Rows[i]["Present Address"].ToString();
                    PermanentAddress = ds.Rows[i]["Permanent Address"].ToString();
                    ClientId = ds.Rows[i]["Client ID"].ToString();
                    Branch = ds.Rows[i]["Branch"].ToString();
                    Division = ds.Rows[i]["Division"].ToString();
                    BankAccountNo = ds.Rows[i]["Bank Account No"].ToString();
                    IFSC = ds.Rows[i]["IFSC"].ToString();
                    BankName = ds.Rows[i]["Bank Name"].ToString();
                    DateofJoining = ds.Rows[i]["Date of Joining"].ToString();
                    OldEmpID = ds.Rows[i]["Old Emp ID"].ToString();
                    string DateofJoining1 = "";

                    if (DateofJoining.Length > 0)
                    {
                        string dj1 = Convert.ToDateTime(DateofJoining).ToString("dd/MM/yyyy");
                        DateofJoining1 = Timings.Instance.CheckDateFormat(dj1);
                    }
                    else
                    {
                        DateofJoining1 = "01/01/1900";
                    }


                    Dateofleaving = ds.Rows[i]["Date of leaving"].ToString();

                    string Dateofleaving1 = "";
                    if (Dateofleaving.Length > 0)
                    {
                        string dl1 = Convert.ToDateTime(Dateofleaving).ToString("dd/MM/yyyy");
                        Dateofleaving1 = Timings.Instance.CheckDateFormat(dl1);
                    }
                    else
                    {
                        Dateofleaving1 = "01/01/1900";
                    }


                    ESIApplicable = ds.Rows[i]["ESI Applicable"].ToString();
                    if (ESIApplicable == "yes" || ESIApplicable == "YES" || ESIApplicable == "Yes")
                    {
                        EmpESIDeduct = 1;
                    }
                    else
                    {
                        EmpESIDeduct = 0;
                    }
                    ESINo = ds.Rows[i]["ESI No"].ToString();
                    PFApplicable = ds.Rows[i]["PF Applicable"].ToString();
                    PFNo = ds.Rows[i]["PF No"].ToString();
                    if (PFApplicable == "yes" || PFApplicable == "YES" || PFApplicable == "Yes")
                    {
                        EmpPFDeduct = 1;
                    }
                    else
                    {
                        EmpPFDeduct = 0;
                    }

                    PTApplicable = ds.Rows[i]["PT Applicable"].ToString().ToUpper();
                    if (ESIApplicable == "YES")
                    {
                        EmpPTDeduct = 1;
                    }
                    else
                    {
                        EmpPTDeduct = 0;
                    }

                    EmployeeType= ds.Rows[i]["Employee Type"].ToString().ToUpper();

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ToString());
                    SqlCommand Httable = new SqlCommand();
                    Httable.CommandText = "ADDEmpDetails";
                    Httable.CommandTimeout = 0;
                    Httable.CommandType = CommandType.StoredProcedure;
                    Httable.Connection = conn;

                    Httable.Parameters.Add("@EmpId", IDNO);
                    Httable.Parameters.Add("@EmpFName", EmployeeName);
                    Httable.Parameters.Add("@EmpFatherName", FathersName);
                    Httable.Parameters.Add("@EmpMotherName", MotherName);
                    Httable.Parameters.Add("@Department", Department);
                    Httable.Parameters.Add("@EmpDtofBirth", DateofBirth1);
                    Httable.Parameters.Add("@EmpSex", Gender);
                    Httable.Parameters.Add("@EmpMaritalStatus", mstatus);
                    Httable.Parameters.Add("@EmpDesgn", Designation);
                    Httable.Parameters.Add("@EmpPhone", MobileNo);
                    Httable.Parameters.Add("@empUANnumber", UANNumber);
                    Httable.Parameters.Add("@AadharCardNo", AadharNumber);
                    Httable.Parameters.Add("@PanCardNo", PANNumber);
                    Httable.Parameters.Add("@prLmark", PresentAddress);
                    Httable.Parameters.Add("@pelmark", PermanentAddress);
                    Httable.Parameters.Add("@Branch", Branch);
                    Httable.Parameters.Add("@Division", Division);
                    Httable.Parameters.Add("@empbankacno", BankAccountNo);
                    Httable.Parameters.Add("@empifsccode", IFSC);
                    Httable.Parameters.Add("@empbankname", BankName);
                    Httable.Parameters.Add("@EmpDtofJoining", DateofJoining1);
                    Httable.Parameters.Add("@EmpDtofLeaving", Dateofleaving1);
                    Httable.Parameters.Add("@EmpPFDeduct", EmpPFDeduct);
                    Httable.Parameters.Add("@EmpPTDeduct", EmpPTDeduct);
                    Httable.Parameters.Add("@empesino", ESINo);
                    Httable.Parameters.Add("@EmpESIDeduct", EmpESIDeduct);
                    Httable.Parameters.Add("@empepfno", PFNo);
                    Httable.Parameters.Add("@EmployeeType", EmployeeType);
                    Httable.Parameters.Add("@Excel_Number", ExcelNo);
                    Httable.Parameters.Add("@Created_By", UserID);
                    Httable.Parameters.Add("@Preferred_UnitId", ClientId);
                    Httable.Parameters.Add("@Created_On", Created_On);
                    Httable.Parameters.Add("@OldEmpid", OldEmpID);

                    SqlParameter parm = new SqlParameter("@ExactEmpid", SqlDbType.NVarChar, 50);
                    parm.Direction = ParameterDirection.Output; // This is important!
                    Httable.Parameters.Add(parm);

                    string ExactEmpid = (string)Httable.Parameters["@ExactEmpid"].Value;


                    conn.Open();
                    int status = Httable.ExecuteNonQuery();
                    conn.Close();

                    if (status > 0)
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Employee Details added Successfuly');", true);



                    }

                }
            }
        }

        protected void btnmodify_Click(object sender, EventArgs e)
        {
            int result = 0;
            string ExcelSheetname = "";
            string FileName = FileUploadEmpDetails.FileName;
            string path = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(FileUploadEmpDetails.PostedFile.FileName));
            FileUploadEmpDetails.PostedFile.SaveAs(path);

            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;");
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();

            OleDbCommand cmd = new OleDbCommand("Select [IDNO],[Employee Name],[Fathers Name],[Mother Name],[Date of Birth],[Sex],[Marital Status],[Designation],[Mobile No],[Department],[Aadhaar Number],[Present Address],[Permanent Address],[Client ID],[Branch],[Division],[Bank Account No],[Bank Name],[Date of Joining],[Date of leaving],[ESI Applicable],[ESI No],[PF Applicable],[PF No] from [" + ExcelSheetname + "]", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);


            using (SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString))
            {
                sqlcon.Open();

                string IDNO = ""; string EmployeeName = ""; string FathersName = ""; string DateofBirth = ""; string Sex = ""; string MaritalStatus = ""; string Designation = "";
                string ClientId = ""; string Branch = ""; string Division = ""; string BankAccountNo = ""; string BankName = ""; string DateofJoining = ""; string Dateofleaving = ""; string ESIApplicable = ""; string ESINo = ""; string PFApplicable = ""; string PFNo = "";
                var EmpPFDeduct = 0; var EmpESIDeduct = 0; string Gender = ""; string mstatus = ""; string MotherName = ""; string MobileNo = ""; string AadhaarNumber = ""; string PresentAddress = ""; string PermanentAddress = "";
                string Created_By = ""; string Created_On = ""; string Department = "";

                #region Begin Getmax Id from DB
                int ExcelNo = 0;
                string selectquery = "select max(cast(Excel_Number as int )) as Id from empdetails ";
                DataTable dtExcelID = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                if (dtExcelID.Rows.Count > 0)
                {
                    if (String.IsNullOrEmpty(dtExcelID.Rows[0]["Id"].ToString()) == false)
                    {
                        ExcelNo = Convert.ToInt32(dtExcelID.Rows[0]["Id"].ToString()) + 1;
                    }
                    else
                    {
                        ExcelNo = int.Parse("1");
                    }
                }
                #endregion End Getmax Id from DB

                for (int i = 0; i < ds.Rows.Count; i++)
                {



                    Created_On = DateTime.Now.ToString("dd/MM/yyyy");
                    Created_On = Timings.Instance.CheckDateFormat(Created_On);


                    IDNO = ds.Rows[i]["IDNO"].ToString();
                    EmployeeName = ds.Rows[i]["Employee Name"].ToString();
                    FathersName = ds.Rows[i]["Fathers Name"].ToString();
                    MotherName = ds.Rows[i]["Mother Name"].ToString();
                    Department = ds.Rows[i]["Department"].ToString();

                    DateofBirth = ds.Rows[i]["Date of Birth"].ToString();

                    string DateofBirth1 = "";
                    if (DateofBirth.Length > 0)
                    {
                        DateofBirth1 = Timings.Instance.CheckDateFormat(DateofBirth);
                    }
                    else
                    {
                        DateofBirth1 = "01/01/1900";
                    }

                    Sex = ds.Rows[i]["Sex"].ToString();


                    if (Sex == "M" || Sex == "m")
                    {
                        Gender = "1";
                    }
                    else
                    {
                        Gender = "0";
                    }
                    MaritalStatus = ds.Rows[i]["Marital Status"].ToString();

                    if (MaritalStatus == "M" || MaritalStatus == "m")
                    {
                        mstatus = "1";
                    }
                    else
                    {
                        mstatus = "0";
                    }
                    Designation = ds.Rows[i]["Designation"].ToString();
                    MobileNo = ds.Rows[i]["Mobile No"].ToString();
                    AadhaarNumber = ds.Rows[i]["Aadhaar Number"].ToString();
                    PresentAddress = ds.Rows[i]["Present Address"].ToString();
                    PermanentAddress = ds.Rows[i]["Permanent Address"].ToString();
                    ClientId = ds.Rows[i]["Client ID"].ToString();
                    Branch = ds.Rows[i]["Branch"].ToString();
                    Division = ds.Rows[i]["Division"].ToString();
                    BankAccountNo = ds.Rows[i]["Bank Account No"].ToString();
                    BankName = ds.Rows[i]["Bank Name"].ToString();
                    DateofJoining = ds.Rows[i]["Date of Joining"].ToString();

                    string DateofJoining1 = "";

                    if (DateofJoining.Length > 0)
                    {
                        DateofJoining1 = Timings.Instance.CheckDateFormat(DateofJoining);
                    }
                    else
                    {
                        DateofJoining1 = "01/01/1900";
                    }


                    Dateofleaving = ds.Rows[i]["Date of leaving"].ToString();

                    string Dateofleaving1 = "";
                    if (Dateofleaving.Length > 0)
                    {
                        Dateofleaving1 = Timings.Instance.CheckDateFormat(Dateofleaving);
                    }
                    else
                    {
                        Dateofleaving1 = "01/01/1900";
                    }


                    ESIApplicable = ds.Rows[i]["ESI Applicable"].ToString();
                    if (ESIApplicable == "yes" || ESIApplicable == "YES" || ESIApplicable == "Yes")
                    {
                        EmpESIDeduct = 1;
                    }
                    else
                    {
                        EmpESIDeduct = 0;
                    }
                    ESINo = ds.Rows[i]["ESI No"].ToString();
                    PFApplicable = ds.Rows[i]["PF Applicable"].ToString();
                    PFNo = ds.Rows[i]["PF No"].ToString();
                    if (PFApplicable == "yes" || PFApplicable == "YES" || PFApplicable == "Yes")
                    {
                        EmpPFDeduct = 1;
                    }
                    else
                    {
                        EmpPFDeduct = 0;
                    }



                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ToString());
                    SqlCommand Httable = new SqlCommand();
                    Httable.CommandText = "modifyempdetails";
                    Httable.CommandType = CommandType.StoredProcedure;
                    Httable.Connection = conn;



                    Httable.Parameters.Add("@EmpId", IDNO);

                    Httable.Parameters.Add("@EmpFName", EmployeeName);
                    Httable.Parameters.Add("@EmpFatherName", FathersName);
                    Httable.Parameters.Add("@EmpMotherName", MotherName);
                    Httable.Parameters.Add("@Department", Department);
                    Httable.Parameters.Add("@EmpDtofBirth", DateofBirth1);
                    Httable.Parameters.Add("@EmpSex", Gender);
                    Httable.Parameters.Add("@EmpMaritalStatus", mstatus);
                    Httable.Parameters.Add("@EmpDesgn", Designation);

                    Httable.Parameters.Add("@EmpPhone", MobileNo);
                    Httable.Parameters.Add("@aadhaarid", AadhaarNumber);
                    Httable.Parameters.Add("@prLmark", PresentAddress);
                    Httable.Parameters.Add("@pelmark", PermanentAddress);



                    Httable.Parameters.Add("@Branch", Branch);
                    Httable.Parameters.Add("@Division", Division);
                    Httable.Parameters.Add("@empbankacno", BankAccountNo);
                    Httable.Parameters.Add("@empbankname", BankName);


                    Httable.Parameters.Add("@EmpDtofJoining", DateofJoining1);

                    Httable.Parameters.Add("@EmpDtofLeaving", Dateofleaving1);


                    Httable.Parameters.Add("@EmpPFDeduct", EmpPFDeduct);
                    Httable.Parameters.Add("@empesino", ESINo);
                    Httable.Parameters.Add("@EmpESIDeduct", EmpESIDeduct);
                    Httable.Parameters.Add("@empepfno", PFNo);

                    Httable.Parameters.Add("@Created_By", UserID);
                    Httable.Parameters.Add("@Created_On", Created_On);
                    Httable.Parameters.Add("@unitid", ClientId);





                    //SqlParameter parm = new SqlParameter("@ExactEmpid", SqlDbType.NVarChar, 50);
                    //parm.Direction = ParameterDirection.Output; // This is important!
                    //Httable.Parameters.Add(parm);

                    //string ExactEmpid = (string)Httable.Parameters["@ExactEmpid"].Value;


                    conn.Open();
                    int status = Httable.ExecuteNonQuery();
                    conn.Close();

                    if (status > 0)
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Employee Details Updated Successfuly');", true);



                    }

                }
            }
        }
    }
}