using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class LoanDetailsMonthlyWise : System.Web.UI.Page
    {
        GridViewExportUtil gve = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;
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

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            var testDate = 0;

            GVListOfEmployees.DataSource = null;
            GVListOfEmployees.DataBind();

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select the From Date');", true);
                return;
            }
            else
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid ORDER DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }
            }

            var query = "";
            if (ddltype.SelectedIndex == 0)
            {
                query = @"select * from (select  elm.EmpId,(empfname+' '+EmpMName+' '+EmpLName) as Empname,'' as Clientname,(LoanAmount),TypeOfLoan from EmpLoanMaster elm inner join EmpDetails ed on ed.empid=elm.empid where MONTH(LoanIssuedDate)='" + month + "' and YEAR(LoanIssuedDate)='" + Year + "' ) as S " +
                        "pivot" +
                        " (sum(LoanAmount) for typeofloan in ([0],[1],[2],[3],[4],[5],[6]) " +
                        " ) as PIVOTTABLE order by EmpId";

            }
            else
            {
                query = @"select * from (select eld.EmpId,(empfname+' '+EmpMName+' '+EmpLName) as Empname,STUFF(( SELECT distinct ' / '+ Clientname from emploandetails inner join clients c on c.clientid=emploandetails.clientid where ( empid=eld.empid  and LoanCuttingMonth='" + month + Year.Substring(2, 2) + "') for xml path(''),Type).value('(./text())[1]','VARCHAR(MAX)'),1,2,''	) as clientname,(RecAmt),LoanType from EmpLoanDetails eld inner join EmpDetails ed on ed.empid=eld.empid where LoanCuttingMonth='" + month + Year.Substring(2, 2) + "' ) as S" +
                        " pivot " +
                        " (sum(recamt) for loantype in ([0],[1],[2],[3],[4],[5],[6]) " +
                        " ) as PIVOTTABLE order by EmpId";
            }

            DataTable envReports = SqlHelper.Instance.GetTableByQuery(query);

            if (envReports.Rows.Count > 0)
            {
                GVListOfEmployees.DataSource = envReports;
                GVListOfEmployees.DataBind();

                if (ddltype.SelectedIndex == 0)
                {
                    GVListOfEmployees.Columns[2].Visible = false;
                }
                else
                {
                    GVListOfEmployees.Columns[2].Visible = true;
                }
            }


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
            return month;


            #endregion
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
        protected void Lnkbtnexcel_Click(object sender, EventArgs e)
        {
            //string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";

            string strQry = "Select * from CompanyInfo  ";
            DataTable compInfo = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            if (compInfo.Rows.Count > 0)
            {
                companyName = compInfo.Rows[0]["CompanyName"].ToString();
                companyAddress = compInfo.Rows[0]["Address"].ToString();
            }

            int colspan = 0;
            if (ddltype.SelectedIndex == 0)
            {
                colspan = 9;
            }
            else
            {
                colspan = 10;

            }

            if (ddltype.SelectedIndex == 0)
            {

                gve.ExportGrid("Loans Issued Statement " + GetMonthName() + "/" + GetMonthOfYear() + ".xls", this.hidGridView);

            }
            else
            {
                gve.ExportGrid("Loans Deducted Statement  " + GetMonthName() + "/" + GetMonthOfYear() + ".xls", this.hidGridView);

            }
        }

        decimal TotalSalAdv = 0;
        decimal TotalUniform = 0;
        decimal TotalSecDep = 0;
        decimal TotalLoan = 0;
        decimal TotalSleeping = 0;
        decimal TotalAdminCharges = 0;
        decimal TotalOthers = 0;


        protected void GVListOfEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                string TotalSalAdvance = e.Row.Cells[3].Text;
                if (TotalSalAdvance == null)
                {
                    TotalSalAdv = 0;
                }
                else
                {
                    TotalSalAdv += decimal.Parse(TotalSalAdvance);
                }

                string TotalUnif = e.Row.Cells[4].Text;
                if (TotalUnif == null)
                {
                    TotalUniform = 0;
                }
                else
                {
                    TotalUniform += decimal.Parse(TotalUnif);
                }

                string TotalSecurityDep = e.Row.Cells[5].Text;
                if (TotalSecurityDep == null)
                {
                    TotalSecDep = 0;
                }
                else
                {
                    TotalSecDep += decimal.Parse(TotalSecurityDep);
                }

                string TotalLoans = e.Row.Cells[6].Text;
                if (TotalLoans == null)
                {
                    TotalLoan = 0;
                }
                else
                {
                    TotalLoan += decimal.Parse(TotalLoans);
                }

                string TotalSleep = e.Row.Cells[7].Text;
                if (TotalSleep == null)
                {
                    TotalSleeping = 0;
                }
                else
                {
                    TotalSleeping += decimal.Parse(TotalSleep);
                }


                string TotalAdminCharg = e.Row.Cells[8].Text;
                if (TotalAdminCharg == null)
                {
                    TotalAdminCharges = 0;
                }
                else
                {
                    TotalAdminCharges += decimal.Parse(TotalAdminCharg);
                }

                string TotalOther = e.Row.Cells[9].Text;
                if (TotalOther == null)
                {
                    TotalOthers = 0;
                }
                else
                {
                    TotalOthers += decimal.Parse(TotalOther);
                }

                //TotalAdv1 += Convert.ToDecimal(e.Row.Cells[2].Text);
                //TotalAdv2 += Convert.ToDecimal(e.Row.Cells[3].Text);
                //TotalAdv3 += Convert.ToDecimal(e.Row.Cells[4].Text);
                //TotalAdv4 += Convert.ToDecimal(e.Row.Cells[5].Text);
                //TotalLoan += Convert.ToDecimal(e.Row.Cells[6].Text);



            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {


                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[3].Text = TotalSalAdv.ToString();
                e.Row.Cells[4].Text = TotalUniform.ToString();
                e.Row.Cells[5].Text = TotalSecDep.ToString();
                e.Row.Cells[6].Text = TotalLoan.ToString();
                e.Row.Cells[7].Text = TotalSleeping.ToString();
                e.Row.Cells[8].Text = TotalAdminCharges.ToString();
                e.Row.Cells[9].Text = TotalOthers.ToString();
            }
        }
    }

}