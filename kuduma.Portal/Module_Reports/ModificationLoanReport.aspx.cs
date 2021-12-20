using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ModificationLoanReport : System.Web.UI.Page
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
            GvModifyloandetails.DataSource = null;
            GvModifyloandetails.DataBind();
            GetEmpid();
            GetLoanDetails();
        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            GvModifyloandetails.DataSource = null;
            GvModifyloandetails.DataBind();
            GetEmpName();
            GetLoanDetails();

        }

        public void GetLoanDetails()
        {
            string sqlqry = string.Empty;
            DataTable dt = null;

            sqlqry = " select ISNULL(ED.EmpId,'') as EmpId,ISNULL(ED.EmpFName,'')+' '+ISNULL(ED.EmpMName,'')+' '+ISNULL(ED.EmpLName,'') as Fullname, ISNULL(mlm.Loanno,'') as Loanno,ISNULL(LoanActAmt,0) as LoanActAmt,ISNULL(ModifiedLoanAmt,0) as ModifiedLoanAmt,ISNULL(LT.LoanType,'') as LoanType,ISNULL(mlm.Remarks,'') as Remarks,ISNULL(ModifyType,'') as ModifyType,ISNULL(ModifiedBy,'') as ModifiedBy,ISNULL(ModifiedTime,'') as ModifiedTime, ISNULL(ELM.NoInstalments,0) as NoInstalments from ModifidedLoanMaster MLM inner join EmpDetails ED on ED.EmpId=MLM.Empid inner join EmpLoanMaster ELM on ELM.EmpId=MLM.Empid inner join LoanTypes LT on LT.Id=ELM.TypeOfLoan where MLM.EmpId = '" + txtEmpid.Text + "' order by LoanNo desc";

            dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GvModifyloandetails.DataSource = dt;
                GvModifyloandetails.DataBind();
            }
            else
            {
                GvModifyloandetails.DataSource = null;
                GvModifyloandetails.DataBind();
            }
            
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("Modification Loan Report.xls", this.GvModifyloandetails);
        }
    }
}