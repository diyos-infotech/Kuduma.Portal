using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using KLTS.Data;
using System.Drawing;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class clientattendance : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil(); 

        int oderid = 0;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
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
                    if (this.Master != null)
                    {
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("c3");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }

                    FillClientNameList();
                    FillClientList();
                    FillMonthList();
                    LoadDesignations();
                    LoadEmpIds();
                    LoadNames();
                    txtorderdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtjoindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtrelivingdate.Text = "";
                    OrderId();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void FillClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            int rowno1 = 0;
            //ddlClientID.Items.Add("--Select--");

            if (dt.Rows.Count > 0)
            {
                ddlClientID.DataValueField = "clientid";
                ddlClientID.DataTextField = "clientid";
                ddlClientID.DataSource = dt;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "--Select--");

        }

        protected void FillClientNameList()
        {
            // string selectclientid = "select ClientName from clients   where  clientid like '" + CmpIDPrefix + "%'";
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                ddlCName.DataValueField = "clientid";
                ddlCName.DataTextField = "Clientname";
                ddlCName.DataSource = dt;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "--Select--");

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

            ddlMonth.Items.Insert(0, "--Select--");

        }

       

        private void displaydata()
        {
            cleartransferdata();
            ddlMonth.SelectedIndex = 0;
            GridView1.DataSource = null;
            GridView1.DataBind();

            gvfromcontracts.DataSource = null;
            gvfromcontracts.DataBind();
            if (ddlClientID.SelectedIndex > 0)
            {
                string selectclientdata = "select Clientid,clientname,clientphonenumbers,OurContactPersonId " +
                    " from clients Where Clientid='" + ddlClientID.SelectedValue + "'";
                DataTable dtdata = config.ExecuteAdaptorAsyncWithQueryParams(selectclientdata).Result;
                if (dtdata.Rows.Count > 0)
                {
                    ddlCName.SelectedValue = dtdata.Rows[0]["Clientid"].ToString();
                    txtphonenumbers.Text = dtdata.Rows[0]["clientphonenumbers"].ToString();
                    txtocp.Text = dtdata.Rows[0]["OurContactPersonId"].ToString();
                }
                else
                {
                    ddlCName.SelectedIndex = 0;
                    txtphonenumbers.Text = "";
                    txtocp.Text = "";
                }
                /*** Getting list of employees working for this client for this month*/
            }
            else
            {

            }
        }

        protected void FillAttendanceGrid()
        {

            LblResult.Text = string.Empty;

            try
            {

                if (ddlClientID.SelectedIndex > 0)
                {
                    // int month = 0, year = 2000;
                    // GlobalData.Instance.GetMonthAndYear(ddlMonth.SelectedValue, ddlMonth.SelectedIndex, out month, out year);
                    DataTable data = new DataTable();
                    if (radioindividual.Checked)
                    {
                        gvfromcontracts.DataSource = null;
                        gvfromcontracts.DataBind();

                        #region Begin  Variable declaration
                        var ClientID = "";
                        var Month = 0;
                        var LastDay = DateTime.Now.Date;
                        Hashtable HTEPAttendance = new Hashtable();
                        var SPName = "";
                        DataTable DTEPAttendance = new DataTable();
                        #endregion End  Variable declaration

                        #region Begin  Assign Values To Variable
                        ClientID = ddlClientID.SelectedValue;
                        //Month = Timings.Instance.GetIdForSelectedMonth(ddlMonth.SelectedIndex);
                        Month = GetMonthBasedOnSelectionDateorMonth();
                        if (Chk_Month.Checked == false)
                        {
                            LastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlMonth.SelectedIndex);
                        }
                        if (Chk_Month.Checked == true)
                        {
                            LastDay = DateTime.Parse(Txt_Month.Text, CultureInfo.GetCultureInfo("en-gb"));
                        }
                        SPName = "IMPaysheetattendance";
                        #endregion End Assign Values To Variable

                        #region Begin Assign Values To Hash Table
                        HTEPAttendance.Add("@Clientid", ClientID);
                        HTEPAttendance.Add("@Month", Month);
                        HTEPAttendance.Add("@LastDay", LastDay);
                        #endregion End Assign Values To Hash Table

                        #region Begin Calling Stored Procedure
                        DTEPAttendance =config.ExecuteAdaptorAsyncWithParams(SPName, HTEPAttendance).Result;
                        #endregion End Calling Stored Procedure
                        if (DTEPAttendance.Rows.Count > 0)
                        {
                            GridView1.DataSource = DTEPAttendance;
                            GridView1.DataBind();

                        }
                        else
                        {
                            GridView1.DataSource = null;
                            GridView1.DataBind();
                            //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Attendance Not Avaialable for  this month of the Selected client');", true);
                            LblResult.Text = "Attendance Not Avaialable for  this month of the Selected client";
                        }
                    }
                    else
                    {
                        DisplydataFromcontracts();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void DisplydataFromcontracts()
        {
            #region Begin Code For ClearData
            LblResult.Text = string.Empty;
            GridView1.DataSource = null;
            GridView1.DataBind();
            gvfromcontracts.DataSource = null;
            gvfromcontracts.DataBind();
            #endregion End Code For ClearData

            #region  Begin Validations
            if (Chk_Month.Checked == false)
            {
                if (ddlMonth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Client Id/Month');", true);
                    return;
                }
            }
            if (Chk_Month.Checked == true)
            {
                if (Txt_Month.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Month');", true);
                    return;
                }

                if (Timings.Instance.CheckEnteredDate(Txt_Month.Text) == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid FROM DATE .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }
            }
            #endregion End Validations

            #region   Begin Variable Declarations
            var month = 0;
            var ClientID = "";
            var LastDate = DateTime.Now.Date;
            var SPName = "";
            Hashtable HTIAttendance = new Hashtable();
            DataTable DTIAttendance = null;
            #endregion End Variable Declarations

            #region Begin Assign Values To Variables
            //month =Timings.Instance.GetIdForSelectedMonth(ddlMonth.SelectedIndex);
            month = GetMonthBasedOnSelectionDateorMonth();

            ClientID = ddlClientID.SelectedValue;
            if (Chk_Month.Checked == false)
            {
                LastDate = Timings.Instance.GetLastDayForSelectedMonth(ddlMonth.SelectedIndex);
            }
            if (Chk_Month.Checked == true)
            {
                LastDate = DateTime.Parse(Txt_Month.Text, CultureInfo.GetCultureInfo("en-gb"));
            }
            SPName = "IMInvoiceattendance";
            #endregion End Assign Values To Variables

            #region Begin Assign Values to the Hash Table
            HTIAttendance.Add("@clientid", ClientID);
            HTIAttendance.Add("@Month", month);
            HTIAttendance.Add("@LastDay", LastDate);
            #endregion End Assign Values to the Hash Table




            #region Begin Calling Stored Procedure
            DTIAttendance =config.ExecuteAdaptorAsyncWithParams(SPName, HTIAttendance).Result;
            #endregion End Calling Stored Procedure

            if (DTIAttendance.Rows.Count > 0)
            {
                gvfromcontracts.DataSource = DTIAttendance;
                gvfromcontracts.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Contract Details not available For The Selected Client');", true);
            }
        }

        private void displaydataFormClientName()
        {
            if (ddlCName.SelectedIndex > 0)
            {
                int month = 0, year = 2000;
                GlobalData.Instance.GetMonthAndYear(ddlMonth.SelectedValue, ddlMonth.SelectedIndex, out month, out year);

                string selectclientdata = "select  Clientid,clientphonenumbers,OurContactPersonId from clients Where Clientid='" + ddlCName.SelectedValue + "'";
                DataTable dtdata = config.ExecuteAdaptorAsyncWithQueryParams(selectclientdata).Result;
                if (dtdata.Rows.Count > 0)
                {
                    ddlClientID.SelectedValue = dtdata.Rows[0]["ClientId"].ToString();
                    txtphonenumbers.Text = dtdata.Rows[0]["clientphonenumbers"].ToString();
                    txtocp.Text = dtdata.Rows[0]["OurContactPersonId"].ToString();
                }
                else
                {
                    ddlCName.SelectedIndex = 0;
                    txtphonenumbers.Text = "";
                    txtocp.Text = "";
                }
                /*** Getting list of employees working for this client for this month*/
                FillAttendanceGrid();
            }
            else
            {
            }
        }

        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblResult.Text = string.Empty;
            try
            {
                if (ddlClientID.SelectedIndex > 0)
                {
                    Session["checkdownloadstatus"] = "0";
                    displaydata();
                    //BindData();
                }
                else
                {
                    ClearData();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblResult.Text = string.Empty;
            if (IsPostBack)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = false;", true);
            }
            lblTotalDuties.Text = string.Empty;
            lblTotalOts.Text = string.Empty;
            //lnkClear.Visible = true;
            if (ddlMonth.SelectedIndex > 0)
            {
                FillAttendanceGrid();
            }
            else
            {
                gvfromcontracts.DataSource = null;
                gvfromcontracts.DataBind();
                GridView1.DataSource = null;
                GridView1.DataBind();

            }
        }


        protected string GetEmpDesignation(string empId)
        {
            string desig = null;

            string sqlQry = "Select EmpDesgn from EmpDetails where EmpId='" + empId + "'";
            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            if (data.Rows.Count > 0)
            {
                desig = data.Rows[0][0].ToString();
            }
            return desig;
        }

        protected int GetEmpDutyType(string empdesign)
        {
            int type = -1;

            string sqlQry = "Select DutyType from Designations where Design='" + empdesign + "'";
            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            if (data.Rows.Count > 0)
            {
                bool str = Convert.ToBoolean(data.Rows[0][0].ToString());

                type = Convert.ToInt32(str);
            }
            return type;
        }

        protected string GetEmpName(string empId)
        {
            string name = null;

            string sqlQry = "Select EmpFName,EmpMName from EmpDetails where EmpId='" + empId + "'";
            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            if (data.Rows.Count > 0)
            {
                name = data.Rows[0]["EmpFName"].ToString() + " " + data.Rows[0]["EmpMName"].ToString();
            }
            return name;
        }

        protected void btn_Save_AttenDanceClick(object sender, EventArgs e)
        {
            LblResult.Text = string.Empty;

            if (ddlClientID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Client Id');", true);
                return;
            }
            // int month = GlobalData.Instance.GetMonth(ddlMonth.SelectedIndex);
            int month = GetMonthBasedOnSelectionDateorMonth();
            var LastDate = DateTime.Now.Date;

            if (Chk_Month.Checked == false)
            {
                LastDate = Timings.Instance.GetLastDayForSelectedMonth(ddlMonth.SelectedIndex);
            }
            if (Chk_Month.Checked == true)
            {
                LastDate = DateTime.Parse(Txt_Month.Text, CultureInfo.GetCultureInfo("en-gb"));
            }


            var ContractID = "";
            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlClientID.SelectedValue);
            HtGetContractID.Add("@LastDay", LastDate);
            DataTable DTContractID =config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
            }
            else
            {
                return;
            }


            string totaldesignationlist = string.Empty;
            if (radioindividual.Checked)
            {
                float totalDuties = 0;
                float totalOTs = 0;
                int statusatte = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    Label lblEmpId = GridView1.Rows[i].FindControl("lblEmpid") as Label;
                    Label lbldesign = GridView1.Rows[i].FindControl("lblDesg") as Label;
                    TextBox txtDuties = GridView1.Rows[i].FindControl("txtDuties") as TextBox;
                    TextBox txtOTs = GridView1.Rows[i].FindControl("txtOTs") as TextBox;
                    TextBox txtPenalty = GridView1.Rows[i].FindControl("txtPenalty") as TextBox;
                    TextBox txtCanteenAdv = GridView1.Rows[i].FindControl("txtCanAdv") as TextBox;
                    TextBox txtIncentivs = GridView1.Rows[i].FindControl("txtIncentivs") as TextBox;


                    TextBox txtwos = GridView1.Rows[i].FindControl("txtwos") as TextBox;
                    TextBox txtnhs = GridView1.Rows[i].FindControl("txtnhs") as TextBox;
                    TextBox txtnpots = GridView1.Rows[i].FindControl("txtnpots") as TextBox;


                    float ots = 0;
                    float duties = 0;
                    float penalty = 0;
                    float CanteenAdv = 0;
                    float Incentivs = 0;

                    float Wos = 0;
                    float Nhs = 0;
                    float Npots = 0;


                    if (txtOTs.Text.Trim().Length > 0)
                    {
                        if (ddlOTType.SelectedIndex == 0)
                        {
                            ots = Convert.ToSingle(txtOTs.Text);
                        }
                        else
                        {
                            ots = Convert.ToSingle(txtOTs.Text) / (float)8;
                        }
                    }
                    if (txtDuties.Text.Trim().Length > 0)
                    {
                        duties = Convert.ToSingle(txtDuties.Text);
                    }
                    if (txtPenalty.Text.Trim().Length > 0)
                    {
                        penalty = Convert.ToSingle(txtPenalty.Text);
                    }
                    if (txtCanteenAdv.Text.Trim().Length > 0)
                    {
                        CanteenAdv = Convert.ToSingle(txtCanteenAdv.Text);
                    }

                    if (txtIncentivs.Text.Trim().Length > 0)
                    {
                        Incentivs = Convert.ToSingle(txtIncentivs.Text);
                    }

                    if (txtwos.Text.Trim().Length > 0)
                    {
                        Wos = Convert.ToSingle(txtwos.Text);
                    }

                    if (txtnhs.Text.Trim().Length > 0)
                    {
                        Nhs = Convert.ToSingle(txtnhs.Text);
                    }

                    if (txtnpots.Text.Trim().Length > 0)
                    {
                        Npots = Convert.ToSingle(txtnpots.Text);
                    }
                    totalDuties += duties;
                    totalOTs += ots;


                    string sqlqry = "Select * from  Empattendance  Where Empid='" + lblEmpId.Text + "' and month='" + month +
                        "'  and ClientId='" + ddlClientID.SelectedValue + "'    and  Design='" + lbldesign.Text + "'  and   contractid='" + ContractID + "'";

                    DataTable dtattestemp = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
                    int Status = 0;
                    if (dtattestemp.Rows.Count > 0)
                    {
                        string design = dtattestemp.Rows[0]["design"].ToString();
                        string updatequery = string.Format("update EmpAttendance set NoofDuties={0},OT={1},Penalty={2}," +
                            " CanteenAdv={6},Incentivs={7},Design='{8}',WO='{9}',NHS='{10}',NPOTS='{11}' " +
                                                    " Where empid='{3}' And ClientId='{4}' And Month={5}  and  Design='" + lbldesign.Text +
                                                    "'  and contractid='" + ContractID + "'",
                                                     duties, ots, penalty, lblEmpId.Text, ddlClientID.SelectedValue,
                                                     month, CanteenAdv, Incentivs, lbldesign.Text, Wos, Nhs, Npots);
                        Status =config.ExecuteNonQueryWithQueryAsync(updatequery).Result;
                    }
                    else
                    {

                        string insertquery = string.Format("insert  EmpAttendance(NoofDuties,OT,Penalty,CanteenAdv,Design," +
                            "WO,NHS,NPOTS,empid,clientid,month,Incentivs,contractid)" +
                            " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}' ) ",

                                                  duties, ots, penalty, CanteenAdv, lbldesign.Text, Wos, Nhs, Npots,
                                                  lblEmpId.Text, ddlClientID.SelectedValue, month,
                                                  Incentivs, ContractID);
                        Status =config.ExecuteNonQueryWithQueryAsync(insertquery).Result;

                    }

                    if (Status != 0)
                    {
                        if (statusatte == 0)
                        {
                            // ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Attendance Added Successfully');", true);
                            LblResult.Text = "Attendance Added Successfully";
                            statusatte = 1;
                        }

                    }
                }

                FillAttendanceGrid();

            }
            else
            {
                int status = 0;
                float totalDuties = 0;
                float totalOTs = 0;
                Hashtable HtClientAttendance = new Hashtable();

                foreach (GridViewRow dr in gvfromcontracts.Rows)
                {

                    #region  Begin New  Code As on [26-10-2013]

                    #region  Begin Code For Variable Declaration as on [26-10-213]

                    var type = "0";
                    var Designation = "";
                    var Quantity = "0";
                    var Duties = "0";
                    var Ots = "0";
                    HtClientAttendance.Clear();
                    var SPName = "";
                    var CLIENTID = "";
                    var LastDay = DateTime.Now.Date;
                    if (Chk_Month.Checked == false)
                    {
                        LastDay = Timings.Instance.GetLastDayForSelectedMonth(ddlMonth.SelectedIndex);
                    }
                    if (Chk_Month.Checked == true)
                    {
                        LastDay = DateTime.Parse(Txt_Month.Text, CultureInfo.GetCultureInfo("en-gb"));
                    }

                    #endregion End  Code For Variable Declaration as on [26-10-213]


                    #region  Begin Code For Assign Values    as on [26-10-213]
                    Designation = ((Label)dr.FindControl("lbldesginid")).Text;
                    type = ((Label)dr.FindControl("lblType")).Text;

                    if (((TextBox)dr.FindControl("TxtQuantity")).Text.Trim().Length > 0)
                    {
                        Quantity = ((TextBox)dr.FindControl("TxtQuantity")).Text;
                    }

                    if (((TextBox)dr.FindControl("txtDuties")).Text.Trim().Length > 0)
                    {
                        Duties = ((TextBox)dr.FindControl("txtDuties")).Text;
                    }


                    if (((TextBox)dr.FindControl("txtOTs")).Text.Trim().Length > 0)
                    {
                        Ots = ((TextBox)dr.FindControl("txtOTs")).Text;
                    }

                    totalDuties += Convert.ToSingle(Duties);
                    totalOTs += Convert.ToSingle(Ots);

                    CLIENTID = ddlClientID.SelectedValue;
                    SPName = "IorMClientattendanceBasedOnclientAndMonth";
                    #endregion End  Code For Assign Values as on [26-10-213]


                    #region Begin Code For Assign Values to The HT Parameters as on [26-10-2013]
                    HtClientAttendance.Add("@Clientid", CLIENTID);
                    HtClientAttendance.Add("@Desingnation", Designation);
                    HtClientAttendance.Add("@Quantity", Quantity);
                    HtClientAttendance.Add("@Duties", Duties);
                    HtClientAttendance.Add("@Ot", Ots);
                    HtClientAttendance.Add("@Month", month);
                    HtClientAttendance.Add("@LastDay", LastDay);
                    HtClientAttendance.Add("@type", type);
                    #endregion  End  Code For Assign Values to The HT Parameters as on [26-10-2013]

                    #region  Begin Code For Calling Stored procedure for Insert/Modify in Client Attendance Table as on  [26-10-2013]
                    status = config.ExecuteNonQueryParamsAsync(SPName, HtClientAttendance).Result;
                    #endregion End  Code For Calling Stored procedure for Insert/Modify in Client Attendance Table as on  [26-10-2013]

                    #endregion End  New  Code As on [26-10-2013]


                }

                int statusatte = 0;
                if (status != 0)
                {
                    if (statusatte == 0)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Client Attendance Added Successfully');", true);
                        LblResult.Text = "Client Attendance Added Successfully";
                        statusatte = 1;
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Records Not Added');", true);
                    LblResult.Text = "Records Not Added";
                }
                DisplydataFromcontracts();
            }
        }

        protected void Btn_Cancel_AttenDance_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void ClearData()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            gvfromcontracts.DataSource = null;
            gvfromcontracts.DataBind();
            ddlClientID.SelectedIndex = 0;
            ddlCName.SelectedIndex = 0;
            ddlMonth.SelectedIndex = 0;
            txtphonenumbers.Text = "";
            txtocp.Text = "";
            ddlOTType.SelectedIndex = 0;
            lblTotalDuties.Text = "";
            lblTotalOts.Text = "";

        }

        protected void ddlCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblResult.Text = string.Empty;
            try
            {
                if (ddlCName.SelectedIndex > 0)
                {
                    Session["checkdownloadstatus"] = "0";
                    displaydataFormClientName();
                }
                else
                {
                    ClearData();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void radioindividual_CheckedChanged(object sender, EventArgs e)
        {
            LblResult.Text = string.Empty;
            lblTotalDuties.Text = "";
            lblTotalOts.Text = "";
            if (radioindividual.Checked)
            {
                gvspecialdays.DataSource = null;
                gvspecialdays.DataBind();
                gvfromcontracts.DataSource = null;
                gvfromcontracts.DataBind();
                displaydata();
            }

            if (radioall.Checked)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                gvspecialdays.DataSource = null;
                gvspecialdays.DataBind();
                DisplydataFromcontracts();
            }

            if (radiospecialdays.Checked)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                gvfromcontracts.DataSource = null;
                gvfromcontracts.DataBind();
            }
        }

        protected void radioall_CheckedChanged(object sender, EventArgs e)
        {
            LblResult.Text = string.Empty;
            lblTotalDuties.Text = "";
            lblTotalOts.Text = "";
            if (radioindividual.Checked)
            {
                gvspecialdays.DataSource = null;
                gvspecialdays.DataBind();
                gvfromcontracts.DataSource = null;
                gvfromcontracts.DataBind();
                displaydata();
            }

            if (radioall.Checked)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                gvspecialdays.DataSource = null;
                gvspecialdays.DataBind();
                DisplydataFromcontracts();
            }

            if (radiospecialdays.Checked)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                gvfromcontracts.DataSource = null;
                gvfromcontracts.DataBind();
            }
        }


        protected void txtDutiesInHours_textChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
                if (row != null)
                {
                    TextBox duties = (TextBox)row.FindControl("txtDuties");
                    if (duties != null)
                    {
                        TextBox dutyHours = (TextBox)sender;
                        if (dutyHours != null)
                        {
                            string hours = dutyHours.Text;
                            if (hours.Trim().Length > 0)
                            {
                                float dHours = Convert.ToSingle(hours);
                                //duties.Text = (dHours / 8).ToString("0.00");
                                duties.Text = (dHours).ToString("0.00");

                            }
                        }
                    }
                }
            }
        }



        #region New code add by venkat on 5/10/2013 for posting order in clientattendance


        protected void BindData()
        {
            ddlempname.SelectedIndex = 0;
            ddlempid.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            if (ddlClientID.SelectedIndex > 0)
            {
                DataTable data = new DataTable();

                string selectQuery = "Select ed.EmpId,(ISNULL( ed.empfname,'' )+' '+ ISNULL(ed.empmname,'') +' '+ISNULL(ed.Emplname,'')) as  Name," +
                    "EPO.Desgn,(ISNULL(EPO.PF,0)) as pf ,(ISNULL(EPO.ESI,0)) as esi ,(ISNULL(EPO.PT,0)) as pt  from Empdetails ed,Emppostingorder EPO where ed.Empid=EPO.Empid and EPO.TransferType=1 and tounitid='"
                    + ddlClientID.SelectedValue + "'   Order by right(isnull(EPO.Empid,0),6)";


                data = config.ExecuteAdaptorAsyncWithQueryParams(selectQuery).Result;

                if (data.Rows.Count > 0)
                {
                    GridView1.DataSource = data;
                    GridView1.DataBind();
                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                }
            }
            txtPrevUnitId.Text = "";
        }

        protected void btntransfer_Click(object sender, EventArgs e)
        {


            if (IsPostBack)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = false;", true);
            }


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
            if (ddlClientID.SelectedIndex == 0)
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select OrderDate');", true);
                return;
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
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select RelivingDate');", true);
                //return;
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
            if (ddlClientID.SelectedIndex > 0)
            {
                unitid = ddlClientID.SelectedValue;
            }
            if (ddlempid.SelectedIndex > 0)
            {
                empid = ddlempid.SelectedValue;
            }
            if (ddlDesignation.SelectedIndex > 0)
            {
                designation = ddlDesignation.SelectedValue;
            }

            TransferType = ddlTransfertype.SelectedValue;

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
            HTTransfer.Add("@Orderdate", orddate);
            HTTransfer.Add("@Joiningdate", joindate);
            HTTransfer.Add("@Relievingdate", relivedate);
            HTTransfer.Add("@EmpDesg", designation);
            HTTransfer.Add("@Pf", PF);
            HTTransfer.Add("@Esi", ESI);
            HTTransfer.Add("@Pt", PT);
            HTTransfer.Add("@TransferType", int.Parse(TransferType));
            HTTransfer.Add("@OrderId", orderid);

            #endregion End Passing parameters to stored procedure

            #region Begin Calling Stored procedure
            int DtGetTrasferdata =config.ExecuteNonQueryParamsAsync(spname, HTTransfer).Result;
            #endregion End Calling stored procedure


            if (DtGetTrasferdata > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Employee transfered Sucessfully');", true);
                FillAttendanceGrid();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Employee details not available');", true);
                return;
            }

            OrderId();
        }

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

        protected string GetOrderID()
        {
            string id = "1";
            string selectqueryoderid = "select max(cast(OrderId as int)) as OrderId from EmpPostingOrder ";
            DataTable dtable = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryoderid).Result;
            if (dtable.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dtable.Rows[0]["OrderId"].ToString()) == false)
                {
                    oderid = (Convert.ToInt32(dtable.Rows[0]["OrderId"].ToString())) + 1;
                    id = oderid.ToString();
                }
            }
            return id;
        }

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


        protected void ddlempid_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = false;", true);
            }

            txtrelivingdate.Text = "";
            txtremarks.Text = "";
            if (ddlempid.SelectedIndex > 0)
            {

                GetEmpName();
            }
            else
            {
                cleartransferdata();
            }
        }

        protected void LoadEmpIds()
        {
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
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
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
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }
            #endregion // End Old Code

            //if (ddlempname.SelectedIndex > 0)
            //{
            //    ddlempid.SelectedValue = ddlempname.SelectedValue;
            //}
            //else
            //{
            //    ddlempid.SelectedIndex = 0;
            //}


        }

        protected void ddlempname_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = false;", true);
            }

            if (ddlempname.SelectedIndex > 0)
            {
                GetEmpid();
            }
            else
            {
                cleartransferdata();
                // MessageLabel.Text = "Plese Select Employee Name";
            }
        }

        protected void cleartransferdata()
        {
            ddlempid.SelectedIndex = 0;
            ddlempname.SelectedIndex = 0;
            ddlTransfertype.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            txtrelivingdate.Text = "";

        }

        #endregion

        #region Begin New code for Old attendance on 21/03/2014 by venkat

        protected void Chk_Month_OnCheckedChanged(object sender, EventArgs e)
        {

            //#region Validation

            //if (ddlClientID.SelectedIndex == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The client Id');", true);
            //    Chk_Month.Checked = false;
            //    return;
            //}

            //#endregion

            //gvfromcontracts.DataSource = null;
            //gvfromcontracts.DataBind();
            //GridView1.DataSource = null;
            //GridView1.DataBind();
            //Txt_Month.Text = string.Empty;
            //ddlMonth.SelectedIndex = 0;
            //lblTotalDuties.Text = string.Empty;
            //lblTotalOts.Text = string.Empty;

            //if (Chk_Month.Checked)
            //{
            //    Txt_Month.Visible = true;
            //    ddlMonth.SelectedIndex = 0;
            //    ddlMonth.Visible = false;

            //}
            //else
            //{
            //    Txt_Month.Visible = false;
            //    Txt_Month.Text = "";
            //    ddlMonth.SelectedIndex = 0;
            //    ddlMonth.Visible = true;
            //}
        }

        protected void Txt_Month_OnTextChanged(object sender, EventArgs e)
        {
            lblTotalDuties.Text = string.Empty;
            lblTotalOts.Text = string.Empty;
            //lnkClear.Visible = true;
            if (Txt_Month.Text.Trim().Length > 0)
            {
                FillAttendanceGrid();
            }
            else
            {
                gvfromcontracts.DataSource = null;
                gvfromcontracts.DataBind();
                GridView1.DataSource = null;
                GridView1.DataBind();

            }
        }


        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";

            #region Validation

            if (Txt_Month.Text.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(Txt_Month.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(Txt_Month.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
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
            if (Chk_Month.Checked == false)
            {
                month = Timings.Instance.GetIdForSelectedMonth(ddlMonth.SelectedIndex);
                //return month;
            }
            if (Chk_Month.Checked == true)
            {
                DateTime date = DateTime.Parse(Txt_Month.Text, CultureInfo.GetCultureInfo("en-gb"));
                month = Timings.Instance.GetIdForEnteredMOnth(date);
                return month;
            }
            return month;
            #endregion

        }

        public string GetMonthOfYear()
        {
            string MonthYear = "";

            int month = GetMonthBasedOnSelectionDateorMonth();
            if (month.ToString().Length == 4)
            {
                MonthYear = "20" + month.ToString().Substring(2, 2);
            }
            if (month.ToString().Length == 3)
            {
                MonthYear = "20" + month.ToString().Substring(1, 2);

            }
            return MonthYear;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            lnkClear.Visible = false;
            var password = string.Empty;
            var SPName = string.Empty;
            password = txtPassword.Text.Trim();
            string sqlPassword = "select password from IouserDetails where password='" + txtPassword.Text + "'";
            DataTable dtpassword = config.ExecuteAdaptorAsyncWithQueryParams(sqlPassword).Result;
            if (dtpassword.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Invalid Password');", true);
                return;
            }

            #region Validation

            if (ddlClientID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The client Id');", true);
                Chk_Month.Checked = false;
                return;
            }

            #endregion
            Chk_Month.Checked = true;
            gvfromcontracts.DataSource = null;
            gvfromcontracts.DataBind();
            GridView1.DataSource = null;
            GridView1.DataBind();
            Txt_Month.Text = string.Empty;
            ddlMonth.SelectedIndex = 0;
            lblTotalDuties.Text = string.Empty;
            lblTotalOts.Text = string.Empty;

            if (Chk_Month.Checked)
            {
                Txt_Month.Visible = true;
                ddlMonth.SelectedIndex = 0;
                ddlMonth.Visible = false;

            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            lnkClear.Visible = false;

            modelLogindetails.Hide();
            Chk_Month.Checked = false;
            gvfromcontracts.DataSource = null;
            gvfromcontracts.DataBind();
            GridView1.DataSource = null;
            GridView1.DataBind();
            Txt_Month.Text = string.Empty;
            ddlMonth.SelectedIndex = 0;
            lblTotalDuties.Text = string.Empty;
            lblTotalOts.Text = string.Empty;
            if (Chk_Month.Checked == false)
            {
                Txt_Month.Visible = false;
                Txt_Month.Text = "";
                ddlMonth.SelectedIndex = 0;
                ddlMonth.Visible = true;
            }
        }


        #endregion

        #region Begin New code as on 15/04/2014 by venkat

        float TotalQty = 0;
        float TotalOts = 0;
        float TotalDuties = 0;
        float TotalDtinhrs = 0;
        float TotalWos = 0;
        float TotalNhs = 0;
        float TotalNpots = 0;
        float TotalCantAdv = 0;
        float TotalPenalty = 0;
        float TotalIncentivs = 0;


        protected void gvfromcontracts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox TxtQuantity = e.Row.FindControl("TxtQuantity") as TextBox;
                if (TxtQuantity.Text.Trim().Length > 0)
                {
                    TotalQty += float.Parse(TxtQuantity.Text);
                }

                TextBox txtDuties = e.Row.FindControl("txtDuties") as TextBox;
                if (txtDuties.Text.Trim().Length > 0)
                {
                    TotalDuties += float.Parse(txtDuties.Text);
                }

                TextBox txtDutiesInHours = e.Row.FindControl("txtDutiesInHours") as TextBox;
                if (txtDutiesInHours.Text.Trim().Length > 0)
                {
                    TotalOts += float.Parse(txtDutiesInHours.Text);
                }

                TextBox txtOTs = e.Row.FindControl("txtOTs") as TextBox;
                if (txtOTs.Text.Trim().Length > 0)
                {
                    TotalDtinhrs += float.Parse(txtOTs.Text);
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalQty = e.Row.FindControl("lblTotalQty") as Label;
                lblTotalQty.Text = TotalQty.ToString();


                Label lblTotalDuties = e.Row.FindControl("lblTotalDuties") as Label;
                lblTotalDuties.Text = TotalDuties.ToString();


                Label lblTotalOts = e.Row.FindControl("lblTotalOts") as Label;
                lblTotalOts.Text = TotalOts.ToString();


                Label lblTotalDutiesinhrs = e.Row.FindControl("lblTotalDutiesinhrs") as Label;
                lblTotalDutiesinhrs.Text = TotalDtinhrs.ToString();

            }
        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblEmpid = e.Row.FindControl("lblEmpid") as Label;
                if (lblEmpid.Text == "zzzzzz")
                {
                    lblEmpid.Text = "";
                    lblEmpid.BackColor = Color.Bisque;

                    Label lblSno = e.Row.FindControl("lblSno") as Label;
                    lblSno.BackColor = Color.Bisque;

                    Label lblName = e.Row.FindControl("lblName") as Label;
                    lblName.BackColor = Color.Bisque;

                    Label lbldesignname = e.Row.FindControl("lbldesignname") as Label;
                    lbldesignname.BackColor = Color.Bisque;

                    TextBox txtDuties = e.Row.FindControl("txtDuties") as TextBox;
                    txtDuties.ReadOnly = true;
                    txtDuties.BackColor = Color.Bisque;

                    TextBox txtOTs = e.Row.FindControl("txtOTs") as TextBox;
                    txtOTs.ReadOnly = true;
                    txtOTs.BackColor = Color.Bisque;

                    TextBox txtwos = e.Row.FindControl("txtwos") as TextBox;
                    txtwos.ReadOnly = true;
                    txtwos.BackColor = Color.Bisque;


                    TextBox txtnhs = e.Row.FindControl("txtnhs") as TextBox;
                    txtnhs.ReadOnly = true;
                    txtnhs.BackColor = Color.Bisque;


                    TextBox txtnpots = e.Row.FindControl("txtnpots") as TextBox;
                    txtnpots.ReadOnly = true;
                    txtnpots.BackColor = Color.Bisque;

                    TextBox txtCanAdv = e.Row.FindControl("txtCanAdv") as TextBox;
                    txtCanAdv.ReadOnly = true;
                    txtCanAdv.BackColor = Color.Bisque;


                    TextBox txtPenalty = e.Row.FindControl("txtPenalty") as TextBox;
                    txtPenalty.ReadOnly = true;
                    txtPenalty.BackColor = Color.Bisque;



                    TextBox txtIncentivs = e.Row.FindControl("txtIncentivs") as TextBox;
                    txtIncentivs.ReadOnly = true;
                    txtIncentivs.BackColor = Color.Bisque;


                }


            }

        }

        #endregion


        protected void lnkClear_Click(object sender, EventArgs e)
        {
            if (ddlClientID.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select the Client Id');", true);
                return;
            }

            if (Txt_Month.Text.Trim().Length == 0 && ddlMonth.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select the Month');", true);
                return;
            }



            var LastDate = DateTime.Now.Date;

            if (Chk_Month.Checked == false)
            {
                LastDate = Timings.Instance.GetLastDayForSelectedMonth(ddlMonth.SelectedIndex);
            }
            if (Chk_Month.Checked == true)
            {
                LastDate = DateTime.Parse(Txt_Month.Text, CultureInfo.GetCultureInfo("en-gb"));
            }


            var ContractID = "";
            Hashtable HtGetContractID = new Hashtable();
            var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
            HtGetContractID.Add("@clientid", ddlClientID.SelectedValue);
            HtGetContractID.Add("@LastDay", LastDate);
            DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

            if (DTContractID.Rows.Count > 0)
            {
                ContractID = DTContractID.Rows[0]["contractid"].ToString();
            }
            else
            {
                return;
            }

            int chkStatus = 0;
            var ClientId = "";
            var UserName = "";
            var SPName = "";
            var OperationStatus = 0;

            Hashtable HTParameters = new Hashtable();

            if (radioindividual.Checked)
            {
                chkStatus = 0;

            }
            if (radioall.Checked)
            {
                chkStatus = 1;
            }


            int month = GetMonthBasedOnSelectionDateorMonth();
            ClientId = ddlClientID.SelectedValue;
            UserName = Session["UserId"].ToString();
            SPName = "DeleteEmpandclientAttendanceBasedonClientIdAndMonth";

            HTParameters.Add("@ClientId", ClientId);
            HTParameters.Add("@Month", month);
            HTParameters.Add("@UserName", UserName);
            HTParameters.Add("@ContractID", ContractID);
            HTParameters.Add("@Status", chkStatus);

            OperationStatus = config.ExecuteNonQueryParamsAsync(SPName, HTParameters).Result;


            //int Status = SqlHelper.Instance.ExecuteDMLQry(SqlQry);

            if (OperationStatus > 0)
            {
                if (chkStatus == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Paysheet Attendance Cleared .');", true);
                }
                if (chkStatus == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Billing Attendance Cleared');", true);
                }
                ddlClientID.SelectedIndex = 0;
                ddlCName.SelectedIndex = 0;
                Txt_Month.Text = "";


                gvfromcontracts.DataSource = null;
                gvfromcontracts.DataBind();

                gvspecialdays.DataSource = null;
                gvspecialdays.DataBind();

                GridView1.DataSource = null;
                GridView1.DataBind();

                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select the Month');", true);
                return;
            }

        }
    }
}