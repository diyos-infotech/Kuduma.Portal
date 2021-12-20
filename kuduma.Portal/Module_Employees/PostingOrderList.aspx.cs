using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.Globalization;
using System.IO;
using System.Data.OleDb;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class PostingOrderList : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        int oderid = 0;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string BranchID = "";
        private void OrderId()
        {
            string selectqueryoderid = "select max(cast(OrderId as int)) as OrderId from EmpPostingOrder ";
            DataTable dtable = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryoderid).Result;
            if (dtable.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dtable.Rows[0]["OrderId"].ToString()) == false)
                {
                    oderid = (Convert.ToInt32(dtable.Rows[0]["OrderId"].ToString())) + 1;
                    txtorderid.Text = oderid.ToString();
                }

                else
                {
                    txtorderid.Text = "1";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            try
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

                    ddlUnit.Items.Add("--Select--");
                    ddlcname.Items.Add("--Select--");

                    OrderId();
                    LoadClientNames();
                    LoadClientList();
                    LoadNames();
                    LoadEmpIds();
                    LoadDesignations();
                    txtorderdate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    txtjoindate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");


                    string ImagesFolderPath = Server.MapPath("ImportDocuments");
                    string[] filePaths = Directory.GetFiles(ImagesFolderPath);

                    foreach (string file in filePaths)
                    {
                        File.Delete(file);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }


        #region Begin New Code for Transfer employees import from excel as code on 22/02/2014 by venkat

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

        protected void btnImportData_Click(object sender, EventArgs e)
        {

            if (ddlUnit.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select clientid');", true);
                return;
            }
            try
            {
                string filename = Path.Combine(Server.MapPath("ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
                fileupload1.PostedFile.SaveAs(filename);
                string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                string constring = string.Empty;
                if (extn.ToLower() == ".xls")
                {
                    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                }
                if (extn.ToLower() == ".xlsx")
                {

                    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filename + "';Extended Properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                }







                string qry = "select [Client Id],[Emp Id],[Order Date],[Joining Date],[Reliving Date],[Designation]," +
                    " [PF],[ESI],[PT],[Remarks] from [" + GetExcelSheetNames() + "]" + "";
                OleDbConnection con = new OleDbConnection(constring);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OleDbCommand cmd = new OleDbCommand(qry, con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                con.Close();
                con.Dispose();

                string clientid = null;
                if (ddlUnit.SelectedIndex > 0)
                    clientid = ddlUnit.SelectedItem.ToString();
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select UnitId');", true);

                    return;
                }


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string empid = string.Empty;
                    string orderdate = string.Empty;
                    string joiningdate = string.Empty;
                    string releivingdate = string.Empty;
                    string client = string.Empty;
                    string remarks = string.Empty;
                    int transfertype = 1;
                    int esi = 1;
                    int pf = 1;
                    int pt = 1;
                    string orderid = txtorderid.Text;
                    string designation = string.Empty;

                    orderdate = dr["Order Date"].ToString();
                    joiningdate = dr["Joining Date"].ToString();
                    releivingdate = dr["Reliving Date"].ToString();
                    client = dr["Client Id"].ToString();
                    remarks = dr["Remarks"].ToString();

                    pf = int.Parse(dr["PF"].ToString());
                    esi = int.Parse(dr["ESI"].ToString());
                    pt = int.Parse(dr["PT"].ToString());
                    designation = dr["Designation"].ToString();
                    if (client == clientid)
                    {

                        #region Check Validation

                        empid = dr["Emp Id"].ToString();

                        if (empid.Length == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please enter employee id');", true);
                            return;
                        }
                        if (joiningdate.Length == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please enter Joining Date');", true);
                            return;
                        }
                        if (orderdate.Length == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please enter Order Date');", true);
                            return;
                        }

                        var testDate = 0;
                        if (joiningdate.Length > 0)
                        {
                            testDate = GlobalData.Instance.CheckEnteredDate(joiningdate);
                            if (testDate > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid JOINING DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                return;
                            }
                            joiningdate = DateTime.Parse(joiningdate, CultureInfo.GetCultureInfo("en-gb")).ToString();
                            releivingdate = joiningdate;

                        }


                        if (orderdate.Length > 0)
                        {
                            testDate = GlobalData.Instance.CheckEnteredDate(orderdate);
                            if (testDate > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid ORDER DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                                return;
                            }
                            orderdate = DateTime.Parse(orderdate, CultureInfo.GetCultureInfo("en-gb")).ToString();
                        }

                        //if (releivingdate.Length > 0)
                        //{
                        //    testDate = GlobalData.Instance.CheckEnteredDate(releivingdate);
                        //    if (testDate > 0)
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid RELIEVING DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        //        return;
                        //    }
                        //    releivingdate = DateTime.Parse(releivingdate, CultureInfo.GetCultureInfo("en-gb")).ToString();
                        //}

                        if (Convert.ToDateTime(orderdate) > Convert.ToDateTime(joiningdate))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Joining Date should be greater than Order Date');", true);
                            return;
                        }
                        if (Convert.ToDateTime(releivingdate) > Convert.ToDateTime(joiningdate))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Joining Date should be greater than Relieving Date');", true);
                            return;
                        }

                        if (pf > 1)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter 0 or 1');", true);
                            return;
                        }
                        if (esi > 1)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter 0 or 1');", true);
                            return;
                        }

                        if (pt > 1)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter 0 or 1');", true);
                            return;
                        }

                        #endregion   //End Validation



                    }

                }
                OrderId();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please upload valid data');", true);
            }

        }

        #endregion

        protected void LoadDesignations()
        {
            DataTable DtDesignations = GlobalData.Instance.LoadDesigns();
            if (DtDesignations.Rows.Count > 0)
            {
                ddlDesignation.DataValueField = "Designid";
                ddlDesignation.DataTextField = "Design";
                ddlDesignation.DataSource = DtDesignations;
                ddlDesignation.DataBind();
            }
            ddlDesignation.Items.Insert(0, "-Select-");
        }

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void Fillcname()
        {
            if (ddlUnit.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlUnit.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void FillClientid()
        {
            if (ddlcname.SelectedIndex > 0)
            {
                ddlUnit.SelectedValue = ddlcname.SelectedValue;
            }
            else
            {
                ddlUnit.SelectedIndex = 0;
            }
        }

        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix,dtBranch);
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");

        }

        protected void LoadClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlUnit.DataValueField = "Clientid";
                ddlUnit.DataTextField = "Clientid";
                ddlUnit.DataSource = DtClientNames;
                ddlUnit.DataBind();
            }
            ddlUnit.Items.Insert(0, "-Select-");
        }


        protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUnit.SelectedIndex > 0)
            {
                Fillcname();
                BindData(ddlUnit.SelectedValue);

            }
            else
            {
                ClearData();
            }
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcname.SelectedIndex > 0)
            {
                FillClientid();
                BindData(ddlcname.SelectedValue);
            }
            else
            {
                ClearData();
            }
        }

        protected void btntransfer_Click(object sender, EventArgs e)
        {
            try
            {


                #region Begin declarations of variables

                int transfertype = 1;
                DateTime joindate = new DateTime();
                DateTime relivedate = new DateTime(1900, 01, 01);
                DateTime orddate = new DateTime();
                string orderid = "";
                string remarks = "";
                string TransferType = "";
                string prevUnitId = "";
                var testDate = 0;
                string unitid = null;
                string empid = null;
                string designation = "";
                Hashtable HTTransfer = new Hashtable();
                string spname = "";
                int PF = 0, ESI = 0, PT = 0;
                #endregion End declaration of variables

                #region Begin Check Validation

                #region Begin Validating UnitIs,EmpId,Desgn
                if (ddlUnit.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select UnitId');", true);
                    return;
                }
                if (ddlempid.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Employee Id');", true);
                    return;
                }
                if (ddlDesignation.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please select Designation for transfer');", true);
                    return;
                }
                #endregion End Validating UnitIs,EmpId,Desgn

                #region Begin validating dateformat
                if (txtjoindate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtjoindate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid JOINING DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select JoinDate');", true);
                    return;
                }
                if (txtorderdate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtorderdate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid ORDER DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }
                else
                {
                    // ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select OrderDate');", true);
                    // return;
                }
                if (txtrelivingdate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtrelivingdate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid RELIEVING DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }
                }
                else
                {
                    //  ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select RelivingDate');", true);
                    // return;
                }
                #endregion End validating dateformat
                #endregion   Begin Check Validation

                #region Begin assignment of variables

                #region Begin Assignment of date variables

                string JoiningDate = DateTime.Now.Date.ToString();
                string OrderedDAte = DateTime.Now.Date.ToString();
                string RelievingDate = DateTime.Now.Date.ToString();

                if (txtjoindate.Text.Trim().Length > 0)
                {
                    joindate = DateTime.Parse(txtjoindate.Text, CultureInfo.GetCultureInfo("en-gb"));
                    JoiningDate = Timings.Instance.CheckDateFormat(txtjoindate.Text);

                }
                if (txtorderdate.Text.Trim().Length > 0)
                {
                    orddate = DateTime.Parse(txtorderdate.Text, CultureInfo.GetCultureInfo("en-gb"));
                    OrderedDAte = Timings.Instance.CheckDateFormat(txtorderdate.Text);

                }
                if (txtrelivingdate.Text.Trim().Length > 0)
                {
                    relivedate = DateTime.Parse(txtrelivingdate.Text, CultureInfo.GetCultureInfo("en-gb"));
                    RelievingDate = Timings.Instance.CheckDateFormat(txtrelivingdate.Text);

                }

                #endregion End Assignment of date variables

                #region Begin  Assingment of Unitid,empId,Desg and TransferType
                if (ddlUnit.SelectedIndex > 0)
                {
                    unitid = ddlUnit.SelectedValue;
                }
                if (ddlempid.SelectedIndex > 0)
                {
                    empid = ddlempid.SelectedValue;
                }
                if (ddlDesignation.SelectedIndex > 0)
                {
                    designation = ddlDesignation.SelectedValue;
                }

                TransferType = ddlTransferType.SelectedValue;

                #endregion End assingment of Unitid,empId,Desg and TransferType

                #region Begin PF,ESI,PT assignment

                if (chkpf.Checked)
                {
                    PF = 1;
                }

                if (chkesi.Checked)
                {
                    ESI = 1;
                }

                if (chkpt.Checked)
                {
                    PT = 1;
                }

                #endregion End PF,ESI,PT assignment

                spname = "IMTEPaySheet";
                #region Begin assignment of orderid,remarks,prevUnitId

                orderid = txtorderid.Text.Trim();
                remarks = txtremarks.Text.Trim();
                prevUnitId = txtPrevUnitId.Text.Trim();
                #endregion End assignment of orderid,remarks,prevUnitId
                #endregion End assignment of variables

                #region Begin check second validation
                if (orddate > joindate)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Joining Date should be greater than Order Date');", true);
                    return;
                }
                if (relivedate > joindate)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Joining Date should be greater than Relieving Date');", true);
                    return;
                }
                #endregion End check second validation


                #region Begin Passing parameters to Stored Procedure
                HTTransfer.Add("@ClientId", unitid);
                HTTransfer.Add("@EmpId", empid);
                HTTransfer.Add("@Orderdate", OrderedDAte);
                HTTransfer.Add("@Joiningdate", JoiningDate);
                HTTransfer.Add("@Relievingdate", RelievingDate);
                HTTransfer.Add("@EmpDesg", designation);
                HTTransfer.Add("@Pf", PF);
                HTTransfer.Add("@Esi", ESI);
                HTTransfer.Add("@Pt", PT);
                HTTransfer.Add("@TransferType", int.Parse(TransferType));
                HTTransfer.Add("@OrderId", orderid);

                #endregion End Passing parameters to stored procedure

                #region Begin Calling Stored procedure
                int DtGetTrasferdata = config.ExecuteNonQueryParamsAsync(spname, HTTransfer).Result;
                #endregion End Calling stored procedure


                if (DtGetTrasferdata > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Employee transfered Sucessfully');", true);
                    BindData(ddlUnit.SelectedValue);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Employee details mot available');", true);
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void ddlempid_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtrelivingdate.Text = "";
            txtremarks.Text = "";
            if (ddlempid.SelectedIndex > 0)
            {

                GetEmpName();
            }
            else
            {
                ClearData();
            }
        }

        protected void LoadEmpIds()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtEmpIds = GlobalData.Instance.LoadEmpIds(EmpIDPrefix);
            if (DtEmpIds.Rows.Count > 0)
            {
                ddlempid.DataValueField = "empid";
                ddlempid.DataTextField = "empid";
                ddlempid.DataSource = DtEmpIds;
                ddlempid.DataBind();
            }
            ddlempid.Items.Insert(0, "-Select-");
        }


        protected void LoadNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtEmpNames = GlobalData.Instance.LoadEmpNames(EmpIDPrefix, dtBranch);
            if (DtEmpNames.Rows.Count > 0)
            {
                ddlempname.DataValueField = "empid";
                ddlempname.DataTextField = "FullName";
                ddlempname.DataSource = DtEmpNames;
                ddlempname.DataBind();
            }
            ddlempname.Items.Insert(0, "-Select-");
        }

        protected void GetEmpName()
        {

            string Sqlqry = "select Empid,EmpDesgn from empdetails  where empid='" + ddlempid.SelectedValue + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    ddlempname.SelectedValue = dt.Rows[0]["Empid"].ToString();

                    if (dt.Rows[0]["EmpDesgn"].ToString() == "0")
                    {
                        ddlDesignation.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlDesignation.SelectedValue = dt.Rows[0]["EmpDesgn"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                MessageLabel.Text = "There Is No Name For The Selected Employee";
            }


            #region

            //if (ddlempid.SelectedIndex > 0)
            //{
            //    ddlempname.SelectedValue = ddlempid.SelectedValue;
            //}
            //else
            //{
            //    ddlempname.SelectedIndex = 0;
            //}

            #endregion
        }

        protected void GetEmpid()
        {

            #region  Old Code
            string Sqlqry = "select Empid,EmpDesgn from empdetails  where empid='" + ddlempname.SelectedValue + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    ddlempid.SelectedValue = dt.Rows[0]["Empid"].ToString();
                    if (dt.Rows[0]["EmpDesgn"].ToString() == "0")
                    {
                        ddlDesignation.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlDesignation.SelectedValue = dt.Rows[0]["EmpDesgn"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                MessageLabel.Text = "There Is No Name For The Selected Employee";
            }
            #endregion // End Old Code
        }

        protected void ddlempname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlempname.SelectedIndex > 0)
            {
                GetEmpid();
            }
            else
            {
                ClearData();
            }
        }

        protected void ClearData()
        {
            ddlempid.SelectedIndex = 0;
            ddlempname.SelectedIndex = 0;
            ddlUnit.SelectedIndex = 0;
            ddlcname.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            gvemppostingorder.DataSource = null;
            gvemppostingorder.DataBind();
        }


        protected void BindData(string Clientid)
        {

            try
            {

                #region Begin  Variable Declaration

                var SPName = "";
                Hashtable HTTEmployeeList = new Hashtable();
                DataTable DTTEmployeeList = null;
                #endregion Begin  Variable Declaration


                #region Begin Assign Values To Variables
                SPName = "GetTransferDetailsBasedOnClient";
                #endregion End Assign Values To Variables


                #region Begin Assign Values To Hash Table
                HTTEmployeeList.Add("@ClientID", Clientid);
                #endregion End Assign Values To Hash Table

                #region  Begin Calling Stored Procedure
                DTTEmployeeList = config.ExecuteAdaptorAsyncWithParams(SPName, HTTEmployeeList).Result;
                #endregion End  Calling Stored Procedure

                #region Begin Resulted Messages
                if (DTTEmployeeList.Rows.Count > 0)
                {
                    gvemppostingorder.DataSource = DTTEmployeeList;
                    gvemppostingorder.DataBind();
                }
                else
                {
                    gvemppostingorder.DataSource = null;
                    gvemppostingorder.DataBind();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('There Is No Employees For The Selected Client');", true);
                }
                #endregion End Resulted Messages
            }
            catch (Exception ex)
            {

            }

        }
   
    }
}