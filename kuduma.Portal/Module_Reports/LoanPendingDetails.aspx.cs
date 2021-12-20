using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KLTS.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class LoanPendingDetails : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Username = "";
        string BranchID = "";

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
            BranchID = Session["BranchID"].ToString();
            Username = Session["UserId"].ToString();
        }
        protected void GetEmpid()
        {

            #region  Old Code
            string Sqlqry = "select Empid +' - '+ OlEmpid Empid from empdetails where (empfname+' '+empmname+' '+emplname)  like '" + txtName.Text + "'  and empid like '%" + EmpIDPrefix + "%'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["Empid"].ToString();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
            }
            #endregion // End Old Code

        }

        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where empid='" + txtEmpid.Text + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtName.Text = dt.Rows[0]["empname"].ToString();

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

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            GvLoansettlement.DataSource = null;
            GvLoansettlement.DataBind();
            GetEmpid();
            LoadLoanDetailsoftheindividualemployee();
        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            GvLoansettlement.DataSource = null;
            GvLoansettlement.DataBind();
            GetEmpName();
            LoadLoanDetailsoftheindividualemployee();
        }

        public string GetMonthName(int month)
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            monthname = mfi.GetMonthName(month).ToString();

            return monthname;
        }

        public string GetMonthOfYear(int month)
        {
            string MonthYear = "";

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

        protected void gvNewLoan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtloanamount = (TextBox)e.Row.FindControl("txtdueamt");
                string empid = e.Row.Cells[1].Text;
            }
        }

        protected void gvNewLoan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvLoansettlement.PageIndex = e.NewPageIndex;
            LoadLoanDetailsoftheindividualemployee();
        }

        protected void LoadLoanDetailsoftheindividualemployee()
        {
            string sqlqry = string.Empty;
            DataTable dt = null;


            string month = "";
            string Year = "";

            sqlqry = "SElect ELM.Loanno,ELM.loanamount,ELM.NoInstalments,ELM.NoInstalments as RInst, (ELM.LoanAmount/ELM.NoInstalments) as Instamt,CONVERT(VARCHAR(10),ELM.LoanDt,103) as LoanDt," + " CONVERT(VARCHAR(10),ELM.SettlementLoanDt,103) as SettlementLoanDt,ISNULL(elm.SettlementLoanAmt,0) as  SettlementLoanAmt," +
                " Isnull(LoanCount,'0') as LoanCount,case TypeOfLoan when 0 then 'Sal.Adv' when 1 then 'Uniform' when 2 then 'Security Deposit'  when 3 then 'Loan' when 4 then 'ATM'  when 5 then 'Others' else '' End as TypeOfLoan, " +
                " (select isnull(sum(recamt),0) from emploandetails ed where ed.loanno=ELM.loanno  ) as Recamt, " +
                " (select isnull((recamt),0) from emploandetails ed where ed.loanno=ELM.loanno ) as CurMonthRecamt  " +
                " From Emploanmaster  ELM  Where  Empid='" + txtEmpid.Text+ "' and  (select isnull(sum(recamt), 0) from emploandetails ed where ed.loanno = ELM.loanno  ) !=  " +
                " (select isnull((recamt),0) from emploandetails ed where ed.loanno=ELM.loanno ) ";


            dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GvLoansettlement.DataSource = dt;
                GvLoansettlement.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string loanno = dt.Rows[i][0].ToString();
                    string text =
                    sqlqry = "Select Loanamount from Emploanmaster  Where Loanno='" + loanno + "' ";
                    DataTable dtml = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
                    if (dtml.Rows.Count > 0)
                    {
                        GvLoansettlement.Rows[i].Cells[8].Text = "RS." + dtml.Rows[0]["Loanamount"].ToString();
                    }
                    else
                    {
                        GvLoansettlement.Rows[i].Cells[8].Text = "RS.0";
                    }

                }

            }
            else
            {
                GvLoansettlement.DataSource = null;
                GvLoansettlement.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Loans For The Selected Employee ');", true);
            }
        }

        protected void gvNewLoan_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GvLoansettlement.EditIndex = e.NewEditIndex;
            LoadLoanDetailsoftheindividualemployee();
        }

        protected void gvNewLoan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GvLoansettlement.EditIndex = -1;
            GridViewRow row = GvLoansettlement.Rows[e.RowIndex];
            row.BackColor = System.Drawing.Color.LightYellow;
            LoadLoanDetailsoftheindividualemployee();
        }

        protected void gvNewLoan_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string sqlqry = "", sqlqry1 = "";
            // string clientid = "";
            string sqlinsert;
            /// lblresult.Text = "";
            int status = 0, count = 0;

            //TextBox txtloanamount = (TextBox)gvNewLoan.Rows[e.RowIndex].FindControl("txtdueamt");
            TextBox txtloanamount = (TextBox)GvLoansettlement.Rows[e.RowIndex].FindControl("txtLoanAmt");
            TextBox txtNoInst = (TextBox)GvLoansettlement.Rows[e.RowIndex].FindControl("txtNoInst");
            TextBox txtLoancut = (TextBox)GvLoansettlement.Rows[e.RowIndex].FindControl("txtLoancut");

            string txtLoancut1 = DateTime.Parse(txtLoancut.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();

            #region Begin Validating Date Format
            var testDate = 0;
            if (txtLoancut.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtLoancut.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Loan Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }
            }
            #endregion End Validating Date Format

            if (txtloanamount.Text.Trim().Length == 0 || txtNoInst.Text.Trim().Length == 0 || txtLoancut.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('please fill following Loanamt,N.Inst,LoanCutMonth');", true);
                return;
                txtloanamount.Text = "0";
                txtNoInst.Text = "0";
                txtLoancut.Text = "1/1/1990";
            }

            Label lblLoanNo1 = (Label)GvLoansettlement.Rows[e.RowIndex].FindControl("lblLoanNo1");
            string loanno = lblLoanNo1.Text;


            string sqlcheck = "select * from ModifidedLoanMaster where LoanNo='" + loanno + "'";
            DataTable dtcheck = config.ExecuteAdaptorAsyncWithQueryParams(sqlcheck).Result;
            if (dtcheck.Rows.Count == 0)
            {
                sqlinsert = "insert into ModifidedLoanMaster(Loanno,Empid,LoanActAmt,LoanCutMon,NoInstalments) select LoanNo,EmpId,LoanAmount,LoanDt,noinstalments from EmpLoanMaster where LoanNo='" + loanno + "'";
                int sin = config.ExecuteNonQueryWithQueryAsync(sqlinsert).Result;
            }
            int LoanCount;
            sqlqry = "select LoanNo,LoanCount from Emploanmaster where LoanNo='" + loanno + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LoanCount"].ToString().Length == 0)
                {
                    LoanCount = 0; LoanCount++;
                    sqlqry = "Update Emploanmaster set LoanAmount='" + txtloanamount.Text + "',NoInstalments='" + txtNoInst.Text + "',LoanDt='" + txtLoancut1 + "'" +
                        ", LoanCount='" + LoanCount + "' where LoanNo='" + loanno + "'";
                    status = config.ExecuteNonQueryWithQueryAsync(sqlqry).Result;
                }
                else
                {
                    LoanCount = int.Parse(dt.Rows[0]["LoanCount"].ToString());
                    LoanCount++;
                    sqlqry = "Update Emploanmaster set LoanAmount='" + txtloanamount.Text + "',NoInstalments='" + txtNoInst.Text + "',LoanDt='" + txtLoancut1 + "'" +
                        ", LoanCount='" + LoanCount + "' where LoanNo='" + loanno + "'";
                    status = config.ExecuteNonQueryWithQueryAsync(sqlqry).Result;
                }
                sqlinsert = "update ModifidedLoanMaster set ModifiedLoanAmt='" + txtloanamount.Text + "',ModifidedLoanCutMon='" + txtLoancut1.ToString().Trim() + "',ModifiedTime='" + DateTime.Now + "',NoInstalments='" + txtNoInst.Text + "',ModifiedBy='" + Username + "' where Loanno='" + loanno + "'";
                int updf = config.ExecuteNonQueryWithQueryAsync(sqlinsert).Result;
            }

            GvLoansettlement.EditIndex = -1;
            LoadLoanDetailsoftheindividualemployee();
            if (status != 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Loan Modified Successfully');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Noloans Loan ');", true);
                return;
            }
        }
        protected void Cleardata()
        {
        }
    }
}