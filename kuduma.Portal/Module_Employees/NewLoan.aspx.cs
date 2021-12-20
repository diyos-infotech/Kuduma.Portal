using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class NewLoan : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        string Created_By = "";
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
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("s3");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }

                    loanidauto();
                    txtloanissuedate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    LoanDropdwn();


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
            BranchID = Session["BranchID"].ToString();
            Created_By = Session["UserID"].ToString();
        }


        public void LoanDropdwn()
        {
            DataTable DtloanTypes = GlobalData.Instance.LoadLoanTypes();
            if (DtloanTypes.Rows.Count > 0)
            {
                ddlLoanType.DataValueField = "Id";
                ddlLoanType.DataTextField = "Loantype";
                ddlLoanType.DataSource = DtloanTypes;
                ddlLoanType.DataBind();

            }
            ddlLoanType.Items.Insert(0, "-Select-");
        }



        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {


            if (txtEmpid.Text.Trim().Length > 0)
            {
                GetEmpNameByEmpId(txtEmpid.Text);
                GetEmployeeLoanDetails();
            }
            else
            {
                ClearData();
            }

        }

        protected void GetEmpNameByEmpId(string empid)
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where Branch in (" + BranchID + ") and empid='" + empid + "' and empid like '%" + EmpIDPrefix + "%'  ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtName.Text = dt.Rows[0]["empname"].ToString();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                txtEmpid.Text = "";
                txtName.Text = "";
            }
        }

        protected void GetEmpidByEmpName(string empname)
        {
            #region  Old Code
            string Sqlqry = "select empid+' - '+oldempid  Empid from empdetails where Branch in (" + BranchID + ") and (empfname like '" + empname + "%' or  empmname like '" + empname + "%' or emplname like '" + empname + "%') and  empid like '%" + EmpIDPrefix + "%'  ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;

            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["Empid"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                txtEmpid.Text = "";
                txtName.Text = "";
            }
            #endregion // End Old Code
        }


        protected void txtName_TextChanged(object sender, EventArgs e)
        {


            if (txtName.Text.Trim().Length > 0)
            {
                GetEmpidByEmpName(txtName.Text.Trim());
                GetEmployeeLoanDetails();
            }
            else
            {
                ClearData();
            }

        }

        private void GetLoanData()
        {
            string selectquery = "select LoanNo as LoanId,LoanAmount,lt.loantype as TypeOfLoan,case elm.LoanType when '' then '' else 'Remarks : '+ elm.LoanType end as remarks, " +
                " NoInstalments,LoanDt,LoanStatus from EmpLoanMaster elm inner join loantypes lt on lt.id=elm.typeofloan where EmpId='" +
               txtEmpid.Text.Trim().Substring(0, 9) + "' ORDER BY LoanDt DESC";



            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
            if (dt.Rows.Count > 0)
            {
                gvNewLoan.DataSource = dt;
                gvNewLoan.DataBind();

                foreach (GridViewRow gvrow in gvNewLoan.Rows)
                {
                    string LoanID = ((Label)gvrow.FindControl("lblLoanid")).Text;
                    string LoanAmount = ((Label)gvrow.FindControl("lblLoanAmount")).Text;
                    Label DueAmount = (Label)gvrow.FindControl("lblDueAmount");
                    Label TypeOfLoan = (Label)gvrow.FindControl("TypeOfLoan");

                    string SqlqRyDueAmount = "Select Sum(isnull(RecAmt,0)) as Paid  From emploandetails Where Loanno='" + LoanID + "'";
                    DataTable dtDueAmount = config.ExecuteAdaptorAsyncWithQueryParams(SqlqRyDueAmount).Result;
                    if (dtDueAmount.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dtDueAmount.Rows[0]["Paid"].ToString()) == false)

                            DueAmount.Text = (double.Parse(LoanAmount) - double.Parse(dtDueAmount.Rows[0]["Paid"].ToString())).ToString();
                        else
                            DueAmount.Text = LoanAmount;
                    }
                    else
                    {
                        DueAmount.Text = LoanAmount;
                    }



                }



            }
            else
            {
                gvNewLoan.DataSource = null;
                gvNewLoan.DataBind();
            }
        }

        protected void ClearData()
        {
            //ddlempmname.SelectedIndex = 0;
            // ddlEmpId.SelectedValue = "";
            //txtLastName.Text = "";
            txtnoofinstall.Text = "";
            txtDescripition.Text = "";
            txtNewLoan.Text = "";
            // ddlEmpName.SelectedValue = "";
            ddlLoanType.SelectedIndex = 0;
            gvNewLoan.DataSource = null;
            gvNewLoan.DataBind();
            txtLoanDate.Text = "";// DateTime.Parse("01/01/0001");
            txtloanissuedate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");

        }

        private void loanidauto()
        {
            //getloandata();
            int loanid;
            string selectqueryclientid = "select max(cast(LoanNo as int )) as Loanno from EmpLoanMaster ";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;

            if (dt.Rows.Count > 0)
            {
                //  DtEmpId = dtempid.Rows[dtempid.Rows.Count - 1][0].ToString();
                if (String.IsNullOrEmpty(dt.Rows[0]["LoanNo"].ToString()) == false)
                {
                    loanid = Convert.ToInt32(dt.Rows[0]["LoanNo"].ToString()) + 1;
                    txtloanid.Text = loanid.ToString();
                }
                else
                {
                    loanid = int.Parse("1");
                    txtloanid.Text = loanid.ToString();
                }
            }
        }

        protected void GetEmployeeLoanDetails()
        {
            loanidauto();
            GetLoanData();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int loanStatus = 0;
                string loantype = txtDescripition.Text;
                var loandate = "01/01/1900";
                var loanissuedate = "01/01/1900";

                if (txtEmpid.Text.Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Select Employee ID');", true);
                    return;
                }
                string empid = txtEmpid.Text.ToString().Substring(0, 9);

                string loanamount = txtNewLoan.Text.Trim();
                if (loanamount.Length <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Loan Amount');", true);
                    return;
                }
                string noofinstallements = txtnoofinstall.Text.Trim();
                if (noofinstallements.Length <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Enter no. of installments');", true);
                    return;
                }



                int testDate = 0;

                if (txtLoanDate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Loan  cutting month');", true);
                    return;
                }


                if (txtLoanDate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtLoanDate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "show alert", "alert('You Are Entered Invalid Loan  Cutting Month.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }

                    loandate = Timings.Instance.CheckDateFormat(txtLoanDate.Text);

                }


                if (txtloanissuedate.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Loan  Issue Date');", true);
                    return;
                }


                if (txtloanissuedate.Text.Trim().Length > 0)
                {
                    testDate = GlobalData.Instance.CheckEnteredDate(txtloanissuedate.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "show alert", "alert('You Are Entered Invalid Loan  Issue Date.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return;
                    }

                    //loanissuedate = DateTime.Parse(txtloanissuedate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                    loanissuedate = Timings.Instance.CheckDateFormat(txtloanissuedate.Text);

                }



                string TypeOfLoan = "0";
                if (ddlLoanType.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Select Loan Type');", true);
                    return;
                }
                else
                {
                    TypeOfLoan = ddlLoanType.SelectedValue;
                }

                string insertquery = " insert into EmpLoanMaster(loandt,empid,loantype,loanamount,NoInstalments,  " +
                    " LoanStatus,TypeOfLoan,LoanIssuedDate,Created_By,Created_On) values( '" + loandate + "','" + empid + "','" + loantype + "','" +
                    loanamount + "','" + noofinstallements + "','" + loanStatus + "','" + TypeOfLoan + "','" + loanissuedate + "','" + Created_By + "', GETDATE())";
                int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                if (status != 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('New Loan Generated Successfuly');", true);
                    loanidauto();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Loan not Generated');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Invalid Data');", true);
            }
            ClearData();
        }

        protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoanType.SelectedIndex == 4)
            {
                txtnoofinstall.Text = "1";
                txtnoofinstall.Enabled = false;
            }
            else
            {
                txtnoofinstall.Text = "";
                txtnoofinstall.Enabled = true;
            }
        }



        //protected void ddlEmpId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string empid="";

        //    if (ddlEmpId.SelectedValue.Trim().Length > 0)
        //    {
        //        empid = ddlEmpId.SelectedItem.ToString();
        //        GetEmpNameByEmpId(empid);
        //        GetEmployeeLoanDetails();
        //    }
        //    else
        //    {
        //        ClearData();
        //    }
        //}

        //protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string empname = "";

        //    if (ddlEmpName.SelectedValue.Trim().Length > 0)
        //    {
        //        empname = ddlEmpName.SelectedItem.ToString();
        //        GetEmpidByEmpName(empname);
        //        GetEmployeeLoanDetails();
        //    }
        //    else
        //    {
        //        ClearData();
        //    }
        //}
    }
}