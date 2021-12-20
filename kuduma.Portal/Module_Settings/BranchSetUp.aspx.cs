using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class BranchSetUp : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                }
                else
                {
                    Response.Redirect("login.aspx");
                }

                displaydata();
            }
        }

        private void displaydata()
        {
            string selectquery = " select * from Branchdetails Order by branchid ";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
            if (dt.Rows.Count > 0)
            {
                gvbranches.DataSource = dt;
                gvbranches.DataBind();
            }
            else
            {
                gvbranches.DataSource = null;
                gvbranches.DataBind();
            }

        }


       

        public string TitleCase(string value)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        }


        protected void btnbranches_Click(object sender, EventArgs e)
        {
            try
            {
                string branchesName = TitleCase(txtbranches.Text.Trim());
                if (branchesName.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Enter The Branch Name');", true);
                    return;
                }

                string Empprefix = TitleCase(TxtbranchesEmpprefix.Text.Trim());
                if (Empprefix.Trim().Length == 0)
                {
                    Empprefix = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please fill the employee id Prefix');", true);
                    return;
                }

                string Clientprefix = TitleCase(TxtbranchesClientprefix.Text.Trim());
                if (Clientprefix.Trim().Length == 0)
                {
                    Clientprefix = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please fill the client id Prefix');", true);
                    return;
                }

                string BillnoWithST = TxtbranchesbillnowithST.Text.Trim();
                if (BillnoWithST.Trim().Length == 0)
                {
                    BillnoWithST = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Please Fill the bill starting  number(i.e. With Service Tax)');", true);
                    return;
                }

                string BillnoWithOutST = TxtbranchesbillnowithOutST.Text.Trim();
                if (BillnoWithOutST.Trim().Length == 0)
                {
                    BillnoWithOutST = "";

                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Please Fill the bill starting  number(i.e. With Out Service Tax');", true);
                    return;
                }

                string BillprefixWithST = TxtbranchesbillnowithSTbillprefix.Text.Trim();
                if (BillprefixWithST.Trim().Length == 0)
                {
                    BillprefixWithST = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Please Fill the bill Prefix(i.e. With Service Tax)');", true);
                    return;
                }

                string BillprefixWithOutST = TxtbranchesbillnowithOutSTbillprefix.Text.Trim();
                if (BillprefixWithOutST.Trim().Length == 0)
                {
                    BillprefixWithOutST = "";

                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Please Fill the bill Prefix (i.e. With Out Service Tax');", true);
                    return;
                }

                int status = 0;
                int statusone = 0;
                int statustwo = 0;
                string Sqlqry = "Select * from branchdetails Order by branchname";
                DataTable dtbranchnames =config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;

                for (int i = 0; i < dtbranchnames.Rows.Count; i++)
                {
                    if (branchesName.Equals(dtbranchnames.Rows[i]["Branchname"].ToString()))
                    {
                        status = 1;
                        break;
                    }
                    if (Empprefix.Equals(dtbranchnames.Rows[i]["EmpPrefix"].ToString()))
                    {
                        statusone = 1;
                        break;
                    }
                    if (Clientprefix.Equals(dtbranchnames.Rows[i]["ClientIDPrefix"].ToString()))
                    {
                        statustwo = 1;
                        break;
                    }

                }
                if (status == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Branch Name Already Exist.Please Change The Branch Name');", true);
                    return;
                }

                if (statusone == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Employee Prefix Already Exist.Please Change The Employee Prefix');", true);
                    return;
                }

                if (statustwo == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Client Prefix Already Exist.Please Change The Client Prefix');", true);
                    return;
                }
                else
                {
                    if (branchesName.Trim().Length != 0)
                    {
                        string insertquery = "insert branchdetails(branchname,EmpPrefix,ClientIDPrefix," +
                            " BillnoWithServicetax,BillNoWithoutServiceTax,BillprefixWithST,BillprefixWithoutST) values('" + branchesName + "','" +
                            Empprefix + "','" + Clientprefix + "','" + BillnoWithST + "','" + BillnoWithOutST + "','" + BillprefixWithST + "','" + BillprefixWithOutST + "')";
                        int Status =config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                        if (Status != 0)
                        {
                            Cleardata();
                            displaydata();
                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Branch Added Sucessfully');", true);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Branch Not Added Sucessfully');", true);
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // lblresult.Visible = true;
                //lblresult.Text = "Incorrect Data";
            }
        }

        protected void gvbranches_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {
            lblresult.Text = "";
            Label branchesName = gvbranches.Rows[e.RowIndex].FindControl("lblbranchesName") as Label;
            string deletequery = "delete from BranchDetails where branchname='" + branchesName.Text + "'";
            int Status =config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            if (Status != 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Branch Deleted Sucessfully');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Branch Not Deleted Sucessfully');", true);
                return;
            }

            displaydata();
        }

        protected void gvbranches_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {
            Label branchesid = gvbranches.Rows[e.RowIndex].FindControl("lblbranchesid") as Label;
            TextBox branchesname = gvbranches.Rows[e.RowIndex].FindControl("txtbranchesName") as TextBox;
            TextBox Empprefix = gvbranches.Rows[e.RowIndex].FindControl("txtbranchesNameEMPrefix") as TextBox;
            TextBox Clientprefix = gvbranches.Rows[e.RowIndex].FindControl("txtbranchesCLIENTPrefix") as TextBox;



            #region Check Duplication

            int status = 0;
            int statusone = 0;
            int statustwo = 0;
            string Sqlqry = "Select * from branchdetails  Where branchname<>'" + branchesname.Text + "' Order by branchname";

            DataTable dtbranchnames = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;

            for (int i = 0; i < dtbranchnames.Rows.Count; i++)
            {
                if (branchesname.Text.Equals(dtbranchnames.Rows[i]["Branchname"].ToString()))
                {
                    status = 1;
                    break;
                }
                if (Empprefix.Text.Equals(dtbranchnames.Rows[i]["EmpPrefix"].ToString()))
                {
                    statusone = 1;
                    break;
                }
                if (Clientprefix.Text.Equals(dtbranchnames.Rows[i]["ClientIDPrefix"].ToString()))
                {
                    statustwo = 1;
                    break;
                }

            }
            if (status == 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Branch Name Already Exist.Please Change The Branch Name');", true);
                return;
            }

            if (statusone == 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Employee Prefix Already Exist.Please Change The Employee Prefix');", true);
                return;
            }

            if (statustwo == 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Client Prefix Already Exist.Please Change The Client Prefix');", true);
                return;
            }


            #endregion


            TextBox billprefixwithST = gvbranches.Rows[e.RowIndex].FindControl("txtbrancheswithostbillprefix") as TextBox;
            TextBox billprefixwithOutST = gvbranches.Rows[e.RowIndex].FindControl("txtbrancheswithostbillprefixwithout") as TextBox;

            TextBox billnoprefixwithST = gvbranches.Rows[e.RowIndex].FindControl("txtbillnowithst") as TextBox;
            TextBox billnoprefixwithOutST = gvbranches.Rows[e.RowIndex].FindControl("txtbillnowithoutst") as TextBox;
            string updatequery = "update BranchDetails  set BranchName = '" + TitleCase(branchesname.Text) +
                "',EmpPrefix='" + TitleCase(Empprefix.Text) + "', ClientIDPrefix='" + TitleCase(Clientprefix.Text) +
                "',BillprefixWithST='" + billprefixwithST.Text + "',   BillprefixWithoutST='" +
                billprefixwithOutST.Text + "' , BillnoWithServicetax='" + billnoprefixwithST.Text +
                "',BillNoWithoutServiceTax='" + billnoprefixwithOutST.Text + "'"

                + " where branchid =" + branchesid.Text;
            status =config.ExecuteNonQueryWithQueryAsync(updatequery).Result;

            gvbranches.EditIndex = -1;
            displaydata();
            if (status != 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Branch Details Updated Successfully');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Branch Details Not Updated Successfully');", true);
                return;
            }
        }

        protected void gvbranches_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            gvbranches.EditIndex = e.NewEditIndex;
            displaydata();
        }

        protected void gvbranches_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvbranches.EditIndex = -1;
            displaydata();
        }

        protected void gvbranches_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvbranches.PageIndex = e.NewPageIndex;
            displaydata();
        }

        protected void Cleardata()
        {
            txtbranches.Text = TxtbranchesEmpprefix.Text = TxtbranchesClientprefix.Text = TxtbranchesbillnowithST.Text = TxtbranchesbillnowithOutST.Text = string.Empty;
        }

    }
}