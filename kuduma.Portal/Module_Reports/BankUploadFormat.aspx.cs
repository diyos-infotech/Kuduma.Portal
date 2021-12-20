using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Data;
using System.Collections;
using System.Globalization;
using Kuduma.Portal.DAL;
using System.Text;
namespace Kuduma.Portal
{
    public partial class BankUploadFormat : System.Web.UI.Page
    {
        //DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        string Accountno = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();


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
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli2");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }
                    //  FillClientList();
                    // FillClientNameList();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        public void GetEmpDetail()
        {

        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();

        }

        public string GetMonth()
        {
            string month = "";
            string year = "";
            string DateVal = "";
            DateTime date;


            if (txtmonth.Text != "")
            {
                date = DateTime.ParseExact(txtmonth.Text, "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                month = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Month.ToString();
                year = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Year.ToString();

            }

            DateVal = month + year.Substring(2, 2);
            return DateVal;

        }

        public void GetClientsData()
        {
            string date = string.Empty;

            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            GVListClients.DataSource = null;
            GVListClients.DataBind();

            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The Month');", true);
                return;
            }

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string Month = GetMonth();

            string query = "select distinct(eps.clientid),clientname from emppaysheet eps inner join clients C on C.Clientid=eps.clientid where month='" + Month + "' and eps.clientid like '%" + CmpIDPrefix + "%'";
            DataTable dtClients = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dtClients.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dtClients;
                GVListEmployees.DataBind();
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();

            }


            lbtn_Export.Visible = true;
            lbtn_ExportToText.Visible = true;
        }

        protected void ClearData()
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
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

        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";

            #region Validation

            if (txtmonth.Text.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
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

            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            month = Timings.Instance.GetIdForEnteredMOnth(date);
            //return month;

            return month;

            #endregion
        }

        protected int GetMonth(string NameOfmonth)
        {
            int month = -1;
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            for (int i = 0; i < monthName.Length; i++)
            {
                if (monthName[i].CompareTo(NameOfmonth) == 0)
                {
                    month = i + 1;
                    break;
                }
            }
            return month;
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();



            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString();
            //payMonth = GetMonth(monthname);

            return monthname;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DtListOfEmployees = new DataTable();

                string Month = GetMonth();
                int option = ddlOptions.SelectedIndex;
                var list = new List<string>();

                GVListClients.DataSource = null;
                GVListClients.DataBind();

                if (GVListEmployees.Rows.Count > 0)
                {
                    string strQry = "Select * from CompanyInfo ";
                    DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                    string companyName = "Your Company Name";
                    var CompBankAcc = "";

                    if (compInfo.Rows.Count > 0)
                    {
                        companyName = compInfo.Rows[0]["CompanyName"].ToString();
                        CompBankAcc = compInfo.Rows[0]["bankaccountno"].ToString();
                    }


                    for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                    {
                        CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                        if (chkclientid.Checked == true)
                        {
                            Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                            Label lblclientname = GVListEmployees.Rows[i].FindControl("lblclientname") as Label;

                            if (chkclientid.Checked == true)
                            {
                                list.Add(lblclientid.Text);
                            }
                        }
                    }

                    string Clientids = string.Join(",", list.ToArray());

                    DataTable dtClientList = new DataTable();
                    dtClientList.Columns.Add("Clientid");
                    if (list.Count != 0)
                    {
                        foreach (string s in list)
                        {
                            DataRow row = dtClientList.NewRow();
                            row["Clientid"] = s;
                            dtClientList.Rows.Add(row);
                        }
                    }

                    Hashtable HtsearchEmp = new Hashtable();
                    string sp = "";

                    string date = string.Empty;
                    sp = "BankUpLoadFormate";
                    HtsearchEmp.Add("@month", Month);
                    HtsearchEmp.Add("@ClientId", dtClientList);
                    HtsearchEmp.Add("@option", option);

                    DtListOfEmployees = config.ExecuteAdaptorAsyncWithParams(sp, HtsearchEmp).Result;

                    if (ddlOptions.SelectedIndex == 4)
                    {
                        string strQryClients = "Select ISNULL(ClientShortName,'') AS ClientShortName from Clients Where ClientId= '"+ Clientids + "' ";
                        DataTable dtClients = config.ExecuteAdaptorAsyncWithQueryParams(strQryClients).Result;
                        string ClientShortName = " ";

                        if (dtClients.Rows.Count > 0)
                        {
                            ClientShortName = dtClients.Rows[0]["ClientShortName"].ToString();
                        }

                        if (DtListOfEmployees.Rows.Count > 0)
                        {
                            GVListClients.Visible = false;
                            GVListClients.DataSource = DtListOfEmployees;
                            GVListClients.DataBind();
                            //int count = DtListOfEmployees.Columns.Count;
                            string line = "Trace Account";
                            string line1 = ": '"+CompBankAcc;
                            string line2 = "File Reference";
                            //string line3 = ": " + DtListOfEmployees.Rows[0]["ClientShortName"].ToString();
                            string line3 = ": " + ClientShortName;
                            String line4 = "Date of Transaction : ";
                            string line5 = ": " + DateTime.Now.ToString("dd/MMM/yyyy");
                            gve.ExporttoExcelForICICIBankUpload("BankUploadFormat.xls", this.GVListClients, line, line1, line2, line3, line4, line5);
                        }
                    }
                    else
                    {
                        if (DtListOfEmployees.Rows.Count > 0)
                        {
                            GVListClients.Visible = false;
                            GVListClients.DataSource = DtListOfEmployees;
                            GVListClients.DataBind();
                            int count = DtListOfEmployees.Columns.Count;
                            string line = "NAME OF A/C HOLDER : " + companyName;
                            string line1 = DateTime.Now.ToString("dd/MM/yyyy");
                            gve.ExporttoExcelForBankUpload("BankUploadFormat.xls", this.GVListClients, line, line1, count);
                        }
                    }

                }

            }
            catch (Exception Ex)
            {

            }

        }

        protected void GVListClients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ddlOptions.SelectedIndex==4)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (ddlOptions.SelectedIndex == 4)
                    {
                        e.Row.Cells[2].Attributes.Add("class", "text");
                        //e.Row.Cells[3].Attributes.Add("class", "text");
                        e.Row.Cells[4].Attributes.Add("class", "text");
                    }
                    if (e.Row.Cells[0].Text == "99999")
                    {
                        e.Row.Cells[0].Text = "";
                        e.Row.Font.Bold = true;

                    }
                }
            }
            else
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[0].Text == "99999")
                    {
                        e.Row.Cells[0].Text = "";
                        e.Row.Font.Bold = true;
                    }
                    if (ddlOptions.SelectedIndex == 1)
                    {
                        e.Row.Cells[3].Attributes.Add("class", "text");
                        e.Row.Cells[4].Attributes.Add("class", "text");
                    }
                }
            }
            
        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {
            GetClientsData();
        }

        protected void lbtn_ExportToText_Click(object sender, EventArgs e)
        {
            if ((ddlOptions.SelectedIndex == 4))
            {
                Bank_TXT(sender, e);
                return;
            }
        }

        protected void Bank_TXT(object sender, EventArgs e)
        {
            try
            {
                DataTable DtListOfEmployees = new DataTable();

                string Month = GetMonth();

                var list = new List<string>();

                GVListClients.DataSource = null;
                GVListClients.DataBind();

                for (int i = 0; i < GVListEmployees.Rows.Count; i++)
                {
                    CheckBox chkclientid = GVListEmployees.Rows[i].FindControl("chkindividual") as CheckBox;
                    if (chkclientid.Checked == true)
                    {
                        Label lblclientid = GVListEmployees.Rows[i].FindControl("lblclientid") as Label;
                        Label lblclientname = GVListEmployees.Rows[i].FindControl("lblclientname") as Label;

                        if (chkclientid.Checked == true)
                        {
                            list.Add(lblclientid.Text);
                        }
                    }
                }

                string Clientids = string.Join(",", list.ToArray());

                DataTable dtClientList = new DataTable();
                dtClientList.Columns.Add("Clientid");
                if (list.Count != 0)
                {
                    foreach (string s in list)
                    {
                        DataRow row = dtClientList.NewRow();
                        row["Clientid"] = s;
                        dtClientList.Rows.Add(row);
                    }
                }

                Hashtable HtsearchEmp = new Hashtable();
                string sp = "";
                int Option1 = 11;

                string date = string.Empty;
                sp = "BankUpLoadFormate";
                HtsearchEmp.Add("@month", Month);
                HtsearchEmp.Add("@ClientId", dtClientList);
                HtsearchEmp.Add("@option", Option1);

                DtListOfEmployees = config.ExecuteAdaptorAsyncWithParams(sp, HtsearchEmp).Result;

                Hashtable HtsearchEmp2 = new Hashtable();
                string sp2 = "";
                int Option2 = 12;

                sp2 = "BankUpLoadFormate";
                HtsearchEmp2.Add("@month", Month);
                HtsearchEmp2.Add("@ClientId", dtClientList);
                HtsearchEmp2.Add("@option", Option2);

                DataTable DtListOfEmployees2 = config.ExecuteAdaptorAsyncWithParams(sp2, HtsearchEmp2).Result;

                if (DtListOfEmployees.Rows.Count > 0)
                {
                    if (Option1 == 11)
                    {
                        GvText1.Visible = false;
                        GvText1.DataSource = DtListOfEmployees;
                        GvText1.DataBind();
                    }
                    else
                    {
                        GvText1.DataSource = null;
                        GvText1.DataBind();
                        GVListClients.DataSource = null;
                        GVListClients.DataBind();
                    }
                }

                if (DtListOfEmployees2.Rows.Count > 0)
                {
                    if (Option2 == 12)
                    {
                        GvText2.Visible = false;
                        GvText2.DataSource = DtListOfEmployees2;
                        GvText2.DataBind();
                    }
                    else
                    {
                        GvText2.DataSource = null;
                        GvText2.DataBind();
                        GVListClients.DataSource = null;
                        GVListClients.DataBind();
                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=BankUpload_ICICI_Details.txt");
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder sBuilder = new System.Text.StringBuilder();
                string txtFile = "";

                var TabLength = 26;
                var TabSpace = new String(' ', TabLength);

                if (DtListOfEmployees.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GvText1.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            txtFile += cell.Text.Replace("&nbsp;", TabSpace);
                        }
                        txtFile += "\r\n";
                    }

                }

                if (DtListOfEmployees2.Rows.Count > 0)
                {
                    foreach (GridViewRow row in GvText2.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            txtFile += cell.Text.Replace("&nbsp;", TabSpace);
                        }
                        txtFile += "\r\n";
                    }

                }

                Response.Output.Write(txtFile.ToString());
                Response.Flush();
                Response.End();

            }
            catch (Exception Ex)
            {

            }

        }

    }
}