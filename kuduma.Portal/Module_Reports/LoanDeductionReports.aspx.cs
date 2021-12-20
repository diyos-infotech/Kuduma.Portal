using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class LoanDeductionReports : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        protected void Page_Load(object sender, EventArgs e)
        {
            lbltamttext.Visible = false;
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

        protected void btn_SubmitClick(object sender, EventArgs e)
        {
            LblResult.Text = "";
            LblResult.Visible = true;
            DataTable dt = null;
            GVLoanRecoveryReports.DataSource = dt;
            GVLoanRecoveryReports.DataBind();

            if (ddlmonth.SelectedIndex == 0)
            {
                string SqlQry = "select E.Empid,E.EmpmName,L.Loanno,L.loandt, L.LoanAmount,LD.RecAmt," +
                 "LD.TransactionDt,L.loanamount/L.noinstalments as CuttingAmount," +

                  "L.LoanAmount-Sum(isnull(LD.RecAmt,0)) as Balance  from  EmpDetails E Inner join EmpLoanDetails " +
                  "LD Inner join EmpLoanMaster L on L.LoanNo=LD.LoanNo  on E.EmpId=L.EmpId";

                dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
                if (dt.Rows.Count > 0)
                {
                    GVLoanRecoveryReports.DataSource = dt;
                    GVLoanRecoveryReports.DataBind();

                    foreach (GridViewRow gvrow in GVLoanRecoveryReports.Rows)
                    {
                        string LoanID = gvrow.Cells[2].Text;
                        string LoanAmount = gvrow.Cells[3].Text;

                        string SqlqRyDueAmount = "Select Sum(isnull(RecAmt,0)) as Paid  From emploandetails Where Loanno='" + LoanID + "'";
                        DataTable dtDueAmount = config.ExecuteAdaptorAsyncWithQueryParams(SqlqRyDueAmount).Result;
                        if (dtDueAmount.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(dtDueAmount.Rows[0]["Paid"].ToString()) == false)

                                gvrow.Cells[7].Text = (double.Parse(LoanAmount) - double.Parse(dtDueAmount.Rows[0]["Paid"].ToString())).ToString();
                            else
                                gvrow.Cells[7].Text = LoanAmount;
                        }
                        else
                        {
                            gvrow.Cells[7].Text = LoanAmount;
                        }
                    }

                    GetTotal();
                    return;
                }
                else
                {
                    ClearAmtData();
                    return;
                }

            }
            else
            {
                ClearAmtData();

                if (txtyear.Text.Trim().Length == 0)
                {
                    LblResult.Text = "Please fill the year ";
                    return;
                }

                if (txtyear.Text.Trim().Length != 4)
                {
                    LblResult.Text = "Invalid Selected Year  ";
                    return;
                }

                string SqlQry = "select E.Empid,E.EmpmName,L.Loanno,L.loandt, L.LoanAmount,LD.RecAmt,LD.TransactionDt" +
                    ",L.loanamount/L.noinstalments as CuttingAmount," +

                     "L.LoanAmount-LD.RecAmt as Balance  from  EmpDetails E Inner join EmpLoanDetails " +
                     "LD Inner join EmpLoanMaster L on L.LoanNo=LD.LoanNo  on E.EmpId=L.EmpId and datepart(mm,LD.TransactionDt)=" + ddlmonth.SelectedIndex +
                     " and  datepart(yy,LD.TransactionDt)=" + txtyear.Text.Trim();


                dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
                if (dt.Rows.Count > 0)
                {
                    GVLoanRecoveryReports.DataSource = dt;
                    GVLoanRecoveryReports.DataBind();


                    foreach (GridViewRow gvrow in GVLoanRecoveryReports.Rows)
                    {
                        string LoanID = gvrow.Cells[2].Text;
                        string LoanAmount = gvrow.Cells[3].Text;
                        string SqlqRyDueAmount = "Select Sum(isnull(RecAmt,0)) as Paid  From emploandetails Where Loanno='" + LoanID + "'";
                        DataTable dtDueAmount = config.ExecuteAdaptorAsyncWithQueryParams(SqlqRyDueAmount).Result;
                        if (dtDueAmount.Rows.Count > 0)
                        {
                            if (String.IsNullOrEmpty(dtDueAmount.Rows[0]["Paid"].ToString()) == false)

                                gvrow.Cells[7].Text = (double.Parse(LoanAmount) - double.Parse(dtDueAmount.Rows[0]["Paid"].ToString())).ToString();
                            else
                                gvrow.Cells[7].Text = LoanAmount;
                        }
                        else
                        {
                            gvrow.Cells[7].Text = LoanAmount;
                        }
                    }
                    GetTotal();
                    return;
                }
                else
                {
                    ClearAmtData();
                    return;
                }
            }

        }

        protected void ClearAmtData()
        {
            lbltamttext.Visible = false;
            lbltmtc.Text = "";
            lbltmtb.Text = "";
            LblResult.Text = "No record found";
        }

        protected void GetTotal()
        {
            lbltamttext.Visible = true;
            LblResult.Text = "";
            float Totalamtc = 0;
            float Totalamtb = 0;
            for (int i = 0; i < GVLoanRecoveryReports.Rows.Count; i++)
            {
                string totalcuttingamt = GVLoanRecoveryReports.Rows[i].Cells[6].Text;
                string totalbalanceamt = GVLoanRecoveryReports.Rows[i].Cells[7].Text;

                Totalamtc += float.Parse(totalcuttingamt);
                Totalamtb += float.Parse(totalbalanceamt);

            }
            lbltmtc.Text = Totalamtc.ToString();
            lbltmtb.Text = Totalamtb.ToString();


        }
    }
}