using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class Employees : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string BranchID = "";
        string Emp_Id = "";
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

                    DisplayData();
                }

            }
            catch (Exception ex)
            {

            }
        }

        public void DisplayData()
        {
           
            int OrderBy = 1;
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable DtEmployees = GlobalData.Instance.LoadActiveEmployeesOrderBy(EmpIDPrefix, OrderBy, dtBranch);
            if (DtEmployees.Rows.Count > 0)
            {
                gvemployee.DataSource = DtEmployees;
                gvemployee.DataBind();
            }
            else
            {
                gvemployee.DataSource = null;
                gvemployee.DataBind();


                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('There is No employees . ');", true);
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
            Emp_Id = Session["Emp_Id"].ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtsearch.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please Enter The Employee ID/Name. Whatever You Want To Search";
                    return;
                }
                Hashtable HtSearchEmployee = new Hashtable();
                DataTable DtSearchEmployee = null;
                var SPName = "IMSearchEmployeeIdorName";
                var SearchedEmpIdOrName = "";
                int Type = ddlSelect.SelectedIndex;


                SearchedEmpIdOrName = txtsearch.Text;

                DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

                HtSearchEmployee.Add("@EmpidorName", SearchedEmpIdOrName);
                HtSearchEmployee.Add("@empidprefix", EmpIDPrefix);
                HtSearchEmployee.Add("@Type", ddlSelect.SelectedIndex);
                HtSearchEmployee.Add("@Branch", dtBranch);

                DtSearchEmployee = config.ExecuteAdaptorAsyncWithParams(SPName, HtSearchEmployee).Result;



                if (DtSearchEmployee.Rows.Count > 0)
                {
                    gvemployee.DataSource = DtSearchEmployee;
                    gvemployee.DataBind();
                }
                else
                {
                    gvemployee.DataSource = null;
                    gvemployee.DataBind();
                    lblMsg.Text = "There are no Employees";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Your Session Expired . Please Login";
            }
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblresult.Visible = true;
            Label empid = gvemployee.Rows[e.RowIndex].FindControl("lblempid") as Label;
            string deletequery = "Update Empdetails set empstatus=0  where EmpId ='" + empid.Text.Trim() + "'";
            int status = config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            if (status != 0)
            {
                lblSuc.Text = "Employee Inactivated Successfully";
            }
            else
            {
                lblMsg.Text = "Employee Not Inactivated ";
            }

            DisplayData();
        }

        protected void lbtn_Select_Click(object sender, EventArgs e)
        {
            try
            {

                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblempid");
                Response.Redirect("ViewEmployee.aspx?Empid=" + lblid.Text, false);

            }
            catch (Exception ex)
            {
                lblMsg.Text = "Your Session Expired . Please Login";
            }

        }

        protected void lbtn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblempid");
                Response.Redirect("ModifyEmployee.aspx?Empid=" + lblid.Text, false);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Your Session Expired . Please Login";

            }

        }

        protected void gvemployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvemployee.PageIndex = e.NewPageIndex;
            DisplayData();
        }

        protected void gvemployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var EMPID = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label status = (Label)e.Row.FindControl("lblempGen") as Label;
                Label lblempid = (Label)e.Row.FindControl("lblempid") as Label;
                ImageButton edit = (ImageButton)e.Row.FindControl("lbtn_Edit") as ImageButton;

                edit.Visible = true;

            }

        }

        protected void linkapprove_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblempid");
                Response.Redirect("ApproveEmployee.aspx?Empid=" + lblid.Text, false);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Your Session Expired . Please Login";

            }
        }

        protected void lbtn_clntman_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblempid");
                Response.Redirect("EmpShiftDetails.aspx?Empid=" + lblid.Text, false);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Your Session Expired . Please Login";
            }

        }
    }
}