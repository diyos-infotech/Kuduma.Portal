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
    public partial class ModifyLoanDetails : System.Web.UI.Page
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
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where empid='" + txtEmpid.Text+ "' ";
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
            lblalert.Text = "";
            GvModifyloandetails.DataSource = null;
            GvModifyloandetails.DataBind();
            GvLoanRepayment.DataSource = null;
            GvLoanRepayment.DataBind();
            GetEmpid();
            GetLoanDetails();
        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            lblalert.Text = "";
            GvModifyloandetails.DataSource = null;
            GvModifyloandetails.DataBind();
            GvLoanRepayment.DataSource = null;
            GvLoanRepayment.DataBind();
            GetEmpName();
            GetLoanDetails();

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

        public void GetLoanDetails()
        {
            string sqlqry = string.Empty;
            DataTable dt = null;

            sqlqry = " select EmpId,LoanNo,LT.LoanType,TypeOfLoan,LoanAmount,NoInstalments,Loanstatus, " +
                     " (select isnull(sum(recamt), 0) from emploandetails ed where ed.loanno = ELM.loanno  ) as Recamt, " +
                     " LoanAmount - (select isnull(sum(recamt), 0) from emploandetails ed where ed.loanno = ELM.loanno  ) as PendingAmount,0 as AmountPayingbyCash, 0 as settlementamount " +
                     " from EmpLoanMaster ELM inner join LoanTypes LT on LT.Id = ELM.TypeOfLoan  where EmpId = '" + txtEmpid.Text+ "' order by LoanNo desc";

            dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                if (ddloption.SelectedIndex == 0)
                {
                    GvModifyloandetails.DataSource = dt;
                    GvModifyloandetails.DataBind();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //string Loanstatus = dt.Rows[i]["Loanstatus"].ToString();
                        //if (Loanstatus == "True")
                        //{
                        //    LinkButton linkedit = GvModifyloandetails.Rows[i].FindControl("linkedit") as LinkButton;
                        //    linkedit.Visible = false;
                        //}
                    }
                }
                else if (ddloption.SelectedIndex == 1)
                {
                    GvLoanRepayment.DataSource = dt;
                    GvLoanRepayment.DataBind();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //string Loanstatus = dt.Rows[i]["Loanstatus"].ToString();
                        //if (Loanstatus == "True")
                        //{
                        //    LinkButton linkedit = GvLoanRepayment.Rows[i].FindControl("linkedit") as LinkButton;
                        //    linkedit.Visible = false;
                        //}
                    }
                }
                else if (ddloption.SelectedIndex == 2)
                {
                    Gvloansettlement.DataSource = dt;
                    Gvloansettlement.DataBind();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Loanstatus = dt.Rows[i]["Loanstatus"].ToString();
                        if (Loanstatus == "True")
                        {
                            LinkButton linkedit = Gvloansettlement.Rows[i].FindControl("linkedit") as LinkButton;
                            linkedit.Visible = false;
                        }
                    }
                }

            }
            else
            {
                if (ddloption.SelectedIndex == 0)
                {
                    GvModifyloandetails.DataSource = null;
                    GvModifyloandetails.DataBind();
                }
                else if (ddloption.SelectedIndex == 1)
                {
                    GvLoanRepayment.DataSource = null;
                    GvLoanRepayment.DataBind();
                }
                else if (ddloption.SelectedIndex == 2)
                {
                    Gvloansettlement.DataSource = null;
                    Gvloansettlement.DataBind();
                }
            }

        }

        protected void GvModifyloandetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GvModifyloandetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GvModifyloandetails.EditIndex = e.NewEditIndex;
            GetLoanDetails();
        }

        protected void GvModifyloandetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GvModifyloandetails.EditIndex = -1;
            GridViewRow row = GvModifyloandetails.Rows[e.RowIndex];
            row.BackColor = System.Drawing.Color.LightYellow;
            GetLoanDetails();
        }

        protected void GvModifyloandetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtremarks = (TextBox)GvModifyloandetails.Rows[e.RowIndex].FindControl("txtremarks");
            TextBox txtModifyLoanAmt = (TextBox)GvModifyloandetails.Rows[e.RowIndex].FindControl("txtModifyLoanAmt");
            TextBox txtloanamount = (TextBox)GvModifyloandetails.Rows[e.RowIndex].FindControl("txtloanamount");
            Label lblEmpId1 = (Label)GvModifyloandetails.Rows[e.RowIndex].FindControl("lblEmpId1");
            Label lblLoanNo = (Label)GvModifyloandetails.Rows[e.RowIndex].FindControl("lblLoanNo1");

            if (txtremarks.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Fill Remarks..');", true);
                return;
            }
            string loanno = lblLoanNo.Text;


            string LoanDetailsCheck = "select Empid,ClientID,Left(DATENAME(month,convert(DATETIME,case when Len(LoanCuttingMonth)>3 then (LEFT(LoanCuttingMonth,2)+'/'+Left(08,1)+'/'+Right(LoanCuttingMonth,2)) else (LEFT(LoanCuttingMonth,1)+'/'+Left(08,1)+'/'+Right(LoanCuttingMonth,2)) end )),3)+ ' - '+Left(20,2)+RIGHT(LoanCuttingMonth,2) Month from emploandetails where loanno='" + loanno + "'";
            DataTable dtLoanDetailsCheck = config.ExecuteAdaptorAsyncWithQueryParams(LoanDetailsCheck).Result;
            if (dtLoanDetailsCheck.Rows.Count == 1 || dtLoanDetailsCheck.Rows.Count == 0)
            {
                string sqlinsert = "insert into ModifidedLoanMaster(Loanno,Empid,LoanActAmt,ModifiedLoanAmt,Remarks,ModifyType,ModifiedTime,ModifiedBy) " +
                  "  values('" + loanno + "','" + lblEmpId1.Text + "','" + txtloanamount.Text + "','" + txtModifyLoanAmt.Text + "','" + txtremarks.Text + "','Modify Loan Amount',GetDate(),'" + Username + "')";
                int sin = config.ExecuteNonQueryWithQueryAsync(sqlinsert).Result;


                string updateQry = "Update Emploanmaster set LoanAmount='" + txtModifyLoanAmt.Text + "' where LoanNo='" + loanno + "'";
                int statusupdateQry = config.ExecuteNonQueryWithQueryAsync(updateQry).Result;
                if (dtLoanDetailsCheck.Rows.Count == 1)
                {
                    string LoanClientID = dtLoanDetailsCheck.Rows[0]["ClientID"].ToString();
                    string Loanmonth = dtLoanDetailsCheck.Rows[0]["Month"].ToString();
                    string LoanEmpid = dtLoanDetailsCheck.Rows[0]["Empid"].ToString();

                    lblalert.Text = "Loan amount modified successfully </b> Please Regenerate Paysheet for Client ID " + LoanClientID + " for month of " + Loanmonth + " ";
                }
                else
                {
                    lblalert.Text = "Loan amount modified successfully";
                }
            }
            else
            {
                lblalert.Text = "Loan cannot be modified as loan amout installment is deducted for previous months.";
            }
            GvModifyloandetails.EditIndex = -1;
            GetLoanDetails();

        }

        protected void GvLoanRepayment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GvLoanRepayment.EditIndex = -1;
            GridViewRow row = GvLoanRepayment.Rows[e.RowIndex];
            row.BackColor = System.Drawing.Color.LightYellow;
            GetLoanDetails();
        }

        protected void GvLoanRepayment_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GvLoanRepayment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GvLoanRepayment.EditIndex = e.NewEditIndex;
            GetLoanDetails();
        }

        protected void GvLoanRepayment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtremarks = (TextBox)GvLoanRepayment.Rows[e.RowIndex].FindControl("txtremarks");
            Label txtloanamount = (Label)GvLoanRepayment.Rows[e.RowIndex].FindControl("txtloanamount");
            Label lblEmpId1 = (Label)GvLoanRepayment.Rows[e.RowIndex].FindControl("lblEmpId1");
            Label lblLoanNo = (Label)GvLoanRepayment.Rows[e.RowIndex].FindControl("lblLoanNo1");
            Label lblTypeOfLoan = (Label)GvLoanRepayment.Rows[e.RowIndex].FindControl("lblTypeOfLoan");
            Label txtTotalPendingAmount = (Label)GvLoanRepayment.Rows[e.RowIndex].FindControl("txtTotalPendingAmount");
            TextBox txtTotalAmountPayingbyCash = (TextBox)GvLoanRepayment.Rows[e.RowIndex].FindControl("txtTotalAmountPayingbyCash");

            if (txtremarks.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Fill Remarks..');", true);
                return;
            }
            string loanno = lblLoanNo.Text;
            float TotalLoanAmount = 0;
            TotalLoanAmount = Convert.ToSingle(txtloanamount.Text);
            float RemAmount = 0;
            float PayingAmount = 0;
            if (txtTotalPendingAmount.Text.Length > 0)
            {
                RemAmount = Convert.ToSingle(txtTotalPendingAmount.Text);
                PayingAmount = Convert.ToSingle(txtTotalAmountPayingbyCash.Text);
                if (RemAmount <= 0)
                {
                    lblalert.Text = "Loan Amount is already recovered";
                    GetLoanDetails();
                    return;
                }

                if (PayingAmount > RemAmount)
                {
                    lblalert.Text = "Paying Amount more than pending amount";
                    GetLoanDetails();
                    return;
                }
            }
            else
            {
                PayingAmount = Convert.ToSingle(txtTotalAmountPayingbyCash.Text);
                if (PayingAmount > TotalLoanAmount)
                {
                    lblalert.Text = "Paying Amount more than Loan amount";
                    GetLoanDetails();
                    return;
                }
            }

            if (txtTotalAmountPayingbyCash.Text.Trim().Length > 0)
            {

                string sqlinsert = "insert into ModifidedLoanMaster(Loanno,Empid,LoanActAmt,ModifiedLoanAmt,Remarks,ModifyType,ModifiedTime,ModifiedBy) " +
                 "  values('" + loanno + "','" + lblEmpId1.Text + "','" + txtloanamount.Text + "','" + txtTotalAmountPayingbyCash.Text + "','" + txtremarks.Text + "','Amount Paying by Cash',GetDate(),'" + Username + "')";
                int sin = config.ExecuteNonQueryWithQueryAsync(sqlinsert).Result;

                float amount = Convert.ToSingle(txtTotalAmountPayingbyCash.Text.Trim());
                string strinsertQry = "Insert into EmpLoanDetails(LoanNo,TransactionDt,RecAmt,PaymentType,LoanType,Empid,Datetime) values('" + lblLoanNo.Text + "',getdate()," + txtTotalAmountPayingbyCash.Text + ", 1,'" + lblTypeOfLoan.Text + "','" + lblEmpId1.Text + "',getdate())";
                int status = config.ExecuteNonQueryWithQueryAsync(strinsertQry).Result;

                if (status != 0)
                {
                    lblalert.Text = "Loan repaid successfully";
                }
                else
                {
                    lblalert.Text = "Loan repayment not done";
                }

                if (amount >= RemAmount)
                {
                    string strupdateQry = "update EmpLoanMaster set LoanStatus = 1 where LoanNo='" + lblLoanNo.Text + "'";
                    int updatestatus = SqlHelper.Instance.ExecuteDMLQry(strupdateQry);
                }

            }
            GvLoanRepayment.EditIndex = -1;
            GetLoanDetails();

        }

        protected void ddloption_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblalert.Text = "";
            txtEmpid.Text = "";
            txtName.Text = "";
            GvModifyloandetails.DataSource = null;
            GvModifyloandetails.DataBind();
            GvLoanRepayment.DataSource = null;
            GvLoanRepayment.DataBind();
            Gvloansettlement.DataSource = null;
            Gvloansettlement.DataBind();
            GVDeleteBills.DataSource = null;
            GVDeleteBills.DataBind();
            if (ddloption.SelectedIndex == 3 || ddloption.SelectedIndex == 4)
            {
                IDempdetails.Visible = false;
                IDClientdetails.Visible = true;
            }
            else
            {
                IDempdetails.Visible = true;
                IDClientdetails.Visible = false;
            }

        }

        protected void Gvloansettlement_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void Gvloansettlement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Gvloansettlement.EditIndex = e.NewEditIndex;
            GetLoanDetails();
        }

        protected void Gvloansettlement_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Gvloansettlement.EditIndex = -1;
            GridViewRow row = Gvloansettlement.Rows[e.RowIndex];
            row.BackColor = System.Drawing.Color.LightYellow;
            GetLoanDetails();
        }

        protected void Gvloansettlement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtremarks = (TextBox)Gvloansettlement.Rows[e.RowIndex].FindControl("txtremarks");
            Label txtloanamount = (Label)Gvloansettlement.Rows[e.RowIndex].FindControl("txtloanamount");
            Label lblEmpId1 = (Label)Gvloansettlement.Rows[e.RowIndex].FindControl("lblEmpId1");
            Label lblLoanNo = (Label)Gvloansettlement.Rows[e.RowIndex].FindControl("lblLoanNo1");
            Label lblTypeOfLoan = (Label)Gvloansettlement.Rows[e.RowIndex].FindControl("lblTypeOfLoan");
            Label txtTotalPendingAmount = (Label)Gvloansettlement.Rows[e.RowIndex].FindControl("txtTotalPendingAmount");
            TextBox txtTotalsettlementamount = (TextBox)Gvloansettlement.Rows[e.RowIndex].FindControl("txtTotalsettlementamount");
            TextBox txtTotalsettlementdate = (TextBox)Gvloansettlement.Rows[e.RowIndex].FindControl("txtTotalsettlementdate");

            if (txtremarks.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Fill Remarks..');", true);
                return;
            }

            int testDate = 0;
            if (txtTotalsettlementdate.Text == "0")
            {
                lblalert.Text = "You Are Entered Invalid Date Of settlement .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990";
                return;
            }

            if (txtTotalsettlementdate.Text.Trim().Length > 0)
            {
                testDate = GlobalData.Instance.CheckEnteredDate(txtTotalsettlementdate.Text);
                if (testDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid Date Of settlement .Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return;
                }
            }


            string settlementdate = "01/01/1900";
            if (txtTotalsettlementdate.Text.Trim().Length != 0)
            {
                settlementdate = DateTime.Parse(txtTotalsettlementdate.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
            }


            string month = DateTime.Parse(settlementdate).Month.ToString();
            string Year = DateTime.Parse(settlementdate).Year.ToString();
            string LoanSettledate = month + Year.Substring(2, 2);


            string loanno = lblLoanNo.Text;
            float TotalLoanAmount = 0;
            TotalLoanAmount = Convert.ToSingle(txtloanamount.Text);
            float PendingAmount = 0;
            float settlementamount = 0;
            float RecivedAmount = 0;

            string GetLoandetails = "Select isnull(sum(isnull(RecAmt,0)),0) RecAmt from emploandetails where loanno='" + loanno + "' and LoanCuttingMonth!='" + LoanSettledate + "'";
            DataTable dtGetLoandetails = config.ExecuteAdaptorAsyncWithQueryParams(GetLoandetails).Result;
            if (dtGetLoandetails.Rows.Count > 0)
            {
                RecivedAmount = float.Parse(dtGetLoandetails.Rows[0]["RecAmt"].ToString());
                PendingAmount = Convert.ToSingle(txtTotalPendingAmount.Text);
                settlementamount = Convert.ToSingle(txtTotalsettlementamount.Text);
                if (txtTotalPendingAmount.Text.Length > 0)
                {
                    if (settlementamount > (TotalLoanAmount - RecivedAmount))
                    {
                        lblalert.Text = "Settlement Amount more than pending amount";
                        Gvloansettlement.EditIndex = -1;
                        GetLoanDetails();
                        return;
                    }

                    if (TotalLoanAmount != (RecivedAmount + settlementamount))
                    {
                        lblalert.Text = "Settlement amount should be equal to  " + (TotalLoanAmount - RecivedAmount);
                        Gvloansettlement.EditIndex = -1;
                        GetLoanDetails();
                        return;
                    }
                }
            }
            else
            {
                if (txtTotalPendingAmount.Text.Length > 0)
                {
                    PendingAmount = Convert.ToSingle(txtTotalPendingAmount.Text);
                    settlementamount = Convert.ToSingle(txtTotalsettlementamount.Text);
                    if (PendingAmount <= 0)
                    {
                        lblalert.Text = "Loan Amount is already recovered";
                        Gvloansettlement.EditIndex = -1;
                        GetLoanDetails();
                        return;
                    }

                    if (settlementamount > PendingAmount)
                    {
                        lblalert.Text = "Settlement Amount more than pending amount";
                        Gvloansettlement.EditIndex = -1;
                        GetLoanDetails();
                        return;
                    }


                    if (TotalLoanAmount != (settlementamount))
                    {
                        lblalert.Text = "Settlement amount should be equal to  " + (TotalLoanAmount - RecivedAmount);
                        Gvloansettlement.EditIndex = -1;
                        GetLoanDetails();
                        return;
                    }
                }
                
            }


           

            if (txtTotalsettlementamount.Text.Trim().Length > 0)
            {

                string sqlinsert = "insert into ModifidedLoanMaster(Loanno,Empid,LoanActAmt,ModifiedLoanAmt,Remarks,ModifyType,ModifiedTime,ModifiedBy) " +
                 "  values('" + loanno + "','" + lblEmpId1.Text + "','" + txtloanamount.Text + "','" + txtTotalsettlementamount.Text + "','" + txtremarks.Text + "','settlement Loan',GetDate(),'" + Username + "')";
                int sin = config.ExecuteNonQueryWithQueryAsync(sqlinsert).Result;

                float amount = Convert.ToSingle(txtTotalsettlementamount.Text.Trim());
                string strinsertQry = "update emploanmaster set SettlementLoanDt='" + settlementdate + "' ,SettlementLoanAmt='" + txtTotalsettlementamount.Text + "' where LoanNo='" + loanno + "'";
                int status = config.ExecuteNonQueryWithQueryAsync(strinsertQry).Result;

                if (status != 0)
                {
                    lblalert.Text = "Loan settlemented successfully please generate paysheet";
                }
                else
                {
                    lblalert.Text = "Loan repayment not done";
                }

            }
            Gvloansettlement.EditIndex = -1;
            GetLoanDetails();
        }

        protected void txtbillno_TextChanged(object sender, EventArgs e)
        {
            string Qry = "Select UnitiD,Left(DATENAME(month,convert(DATETIME,case when Len(Month)>3 then (LEFT(Month,2)+'/'+Left(08,1)+'/'+Right(Month,2)) else (LEFT(Month,1)+'/'+Left(08,1)+'/'+Right(Month,2)) end )),3)+ ' - '+Left(20,2)+RIGHT(Month,2) Month,BillNo,'Normal' BillType,GrandTotal from unitbill where BillNo='" + txtbillno.Text + "' union Select UnitiD,Left(DATENAME(month,convert(DATETIME,case when Len(Month)>3 then (LEFT(Month,2)+'/'+Left(08,1)+'/'+Right(Month,2)) else (LEFT(Month,1)+'/'+Left(08,1)+'/'+Right(Month,2)) end )),3)+ ' - '+Left(20,2)+RIGHT(Month,2) Month,BillNo,'Manual' BillType,GrandTotal from Munitbill where BillNo='" + txtbillno.Text + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
            if (dt.Rows.Count > 0)
            {
                GVDeleteBills.DataSource = dt;
                GVDeleteBills.DataBind();
            }
            else
            {
                GVDeleteBills.DataSource = null;
                GVDeleteBills.DataBind();
            }
        }

        protected void GVDeleteBills_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GVDeleteBills_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lblBillNO = (Label)Gvloansettlement.Rows[e.RowIndex].FindControl("lblBillNO");
            string BillNo = "";
            BillNo = lblBillNO.Text;

            string DeleteQry = "Delete unitbill where BillNo='" + BillNo + "'";
            int statusDeleteQry = config.ExecuteNonQueryWithQueryAsync(DeleteQry).Result;

            string DeleteBreakupQry = "Delete unitbillbillbreakup where BillNo='" + BillNo + "'";
            int statusDeleteBreakupQry = config.ExecuteNonQueryWithQueryAsync(DeleteBreakupQry).Result;


            string ManualDeleteQry = "Delete Munitbill where BillNo='" + BillNo + "'";
            int ManualstatusDeleteQry = config.ExecuteNonQueryWithQueryAsync(DeleteQry).Result;

            string ManualDeleteBreakupQry = "Delete Munitbillbillbreakup where BillNo='" + BillNo + "'";
            int ManualstatusDeleteBreakupQry = config.ExecuteNonQueryWithQueryAsync(DeleteBreakupQry).Result;

        }
    }
}