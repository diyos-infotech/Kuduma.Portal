using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class CreateLogin : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;

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

                    displaydata();
                    Getempids();
                    GetEmpnames();
                    LoadPriviligers();
                }
            }
            catch (Exception ex)
            {
                displaydata();
            }
        }

        protected void LoadPriviligers()
        {
            string selectpreveligers = "select name,previligeid from previligers ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectpreveligers).Result;
            if (dt.Rows.Count > 0)
            {

                ddlpreviligers.DataTextField = "name";
                ddlpreviligers.DataValueField = "previligeid";
                ddlpreviligers.DataSource = dt;
                ddlpreviligers.DataBind();
            }
            ddlpreviligers.Items.Insert(0, "--Select--");
        }

        protected void Getempids()
        {

            DataTable dtempid = GlobalData.Instance.LoadEmpIds(EmpIDPrefix);
            if (dtempid.Rows.Count > 0)
            {
                ddlempid.DataValueField = "empid";
                ddlempid.DataTextField = "empid";
                ddlempid.DataSource = dtempid;
                ddlempid.DataBind();
            }
            ddlempid.Items.Insert(0, "-Select--");
        }

        protected void GetEmpnames()
        {
            DataTable dtname = GlobalData.Instance.LoadEmpIds(EmpIDPrefix);
            if (dtname.Rows.Count > 0)
            {
                ddlempname.DataValueField = "empid";
                ddlempname.DataTextField = "Fullname";
                ddlempname.DataSource = dtname;
                ddlempname.DataBind();
            }
            ddlempname.Items.Insert(0, "-Select-");
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }


        protected void Loadempid()
        {
            if (ddlempname.SelectedIndex > 0)
            {
                ddlempid.SelectedValue = ddlempname.SelectedValue;
            }
            else
            {
                ddlempid.SelectedIndex = 0;
            }
        }

        protected void Loadempname()
        {
            if (ddlempid.SelectedIndex > 0)
            {
                ddlempname.SelectedValue = ddlempid.SelectedValue;
            }
            else
            {
                ddlempname.SelectedIndex = 0;
            }

        }

        private void displaydata()
        {
            string selectquery = " select * from  logindetails where Emp_Id <> '0'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
            if (dt.Rows.Count > 0)
            {
                gvcreatelogin.DataSource = dt;
                gvcreatelogin.DataBind();
            }
            else
            {
                gvcreatelogin.DataSource = null;
                gvcreatelogin.DataBind();
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            lblresult.Visible = true;

            if (ddlempid.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select Employee Id');", true);
                return;
            }


            if (ddlpreviligers.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Select Privilege type');", true);
                return;
            }
            else
            {
                if (txtusername.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Fill the User Name');", true);
                    return;
                }

                if (txtpwd.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Fill the Password');", true);
                    return;
                }
                if (txtcpwd.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Fill the Confirm Password');", true);
                    return;
                }

                if (ddlempid.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select  Employee Id');", true);
                    return;
                }

            }
            try
            {
                if (txtpwd.Text.Trim() == txtcpwd.Text.Trim())
                {
                    string empid = ddlempid.SelectedItem.Text;
                    string username = txtusername.Text;
                    string password = txtpwd.Text;

                    int preveligeid = Convert.ToInt16(ddlpreviligers.SelectedValue);

                    DataTable DTCheckUserId = config.ExecuteAdaptorAsyncWithQueryParams("Select username from logindetails where emp_id = '" + ddlempid.SelectedItem.ToString() + "'").Result;
                    if (DTCheckUserId.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Already Created User Name To This Employee');", true);
                    }
                    else
                    {

                        string insertquery = string.Format("insert into logindetails values('{0}','{1}','{2}',{3}) ", empid, username, password, preveligeid);
                        int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                        if (status != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Login Created Successfully');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Login Not Created');", true);
                        }
                        displaydata();
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Password Not Match With The Confirm Password');", true);
                }
            }
            catch (Exception ex)
            {

            }

            displaydata();
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            txtcpwd.Text = txtpwd.Text = txtusername.Text = string.Empty;
        }

        protected void ddlempid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlempid.SelectedIndex > 0)
            {

                string selectdesign = "select d.Design from EmpDetails e inner join Designations d on e.EmpDesgn=d.DesignId where empid='" + ddlempid.SelectedValue + "'";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectdesign).Result;
                txtdesign.Text = dt.Rows[0]["Design"].ToString();
                displaydata();
                txtusername.Text = "";
                txtpwd.Text = "";
                txtcpwd.Text = "";
                Loadempname();
            }
            else
            {
                ddlempname.SelectedIndex = 0;
                txtdesign.Text = "";
                //lblresult.Text = "Please Select Employee Id";
                //return;
            }
        }

        protected void gvcreatelogin_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblresult.Visible = true;
            Label empid = gvcreatelogin.Rows[e.RowIndex].FindControl("lblempid") as Label;
            string deletequery = "delete from logindetails where emp_id ='" + empid.Text.Trim() + "'";
            int status = config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            if (status != 0)
            {
                lblresult.Text = "Record Deleted Successfully";
            }
            else
            {
                lblresult.Text = "Record Not Delete ";
            }

            displaydata();
        }
        protected void ddlempname_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblresult.Text = "";
            if (ddlempname.SelectedIndex > 0)
            {
                string selectdesign = "select d.Design from EmpDetails e inner join Designations d on e.EmpDesgn=d.DesignId where empid='" + ddlempid.SelectedValue + "'";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectdesign).Result;
                txtdesign.Text = dt.Rows[0]["Design"].ToString();
                displaydata();
                txtusername.Text = "";
                txtpwd.Text = "";
                txtcpwd.Text = "";
                Loadempid();
            }
            else
            {
                ddlempid.SelectedIndex = 0;
                txtdesign.Text = "";
                //lblresult.Text = "Please Select Employee Name";
                //return;
            }
        }
    }
}