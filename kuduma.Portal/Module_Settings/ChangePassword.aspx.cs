using System;
using System.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ChangePassword : System.Web.UI.Page
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

                if (Convert.ToInt32(Session["AccessLevel"]) == 1)
                {
                    lbloldpassword.Visible = false;
                    txtoldpassword.Visible = false;
                }
                string selectquery = "select emp_id,UserName from logindetails Order By Emp_Id ";
                dt =config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
                int i = 0;

                //if (!IsPostBack)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ddlempid.Items.Add(dt.Rows[i][1].ToString());
                    }
                }
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            lblresult.Visible = true;
            try
            {
                if (ddlempid.SelectedIndex <= 0)
                {
                    lblresult.Text = "Select Employee Id";
                    return;
                }
                if (txtnewpassword.Text.Trim().Length <= 0)
                {
                    lblresult.Text = "New Password should not be empty";
                    return;
                }
                string selectquery = "select Password from logindetails where UserName = '" + ddlempid.SelectedItem.Text + "'";
                dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
                if (Convert.ToInt32(Session["AccessLevel"]) != 1)
                {
                    if (txtoldpassword.Text.Trim() == dt.Rows[0][0].ToString())
                    {
                        if (txtnewpassword.Text.Trim() == txtconfirmpassword.Text.Trim())
                        {
                            string updatequery = "update logindetails set password = '" + txtnewpassword.Text + "' Where UserName='" + ddlempid.SelectedItem.Text + "'";
                            int status =config.ExecuteNonQueryWithQueryAsync(updatequery).Result;
                            if (status != 0)
                            {
                                lblresult.Text = "Password Changed Successfully";
                                ddlempid.SelectedIndex = 0;
                            }
                            else
                            {
                                lblresult.Text = "Password Not Changed ";
                            }
                        }
                        else
                        {
                            lblresult.Text = "Password Not Match With Confirm Password";
                        }
                    }
                    else
                    {
                        lblresult.Text = "You Are Entered Wrong Password";
                    }
                }
                else
                {
                    if (txtnewpassword.Text.Trim() == txtconfirmpassword.Text.Trim())
                    {
                        string updatequery = "update logindetails set password = '" + txtnewpassword.Text + "' Where UserName='" + ddlempid.SelectedItem.Text + "'";
                        int status = config.ExecuteNonQueryWithQueryAsync(updatequery).Result;
                        if (status != 0)
                        {
                            lblresult.Text = "Password Changed Successfully";
                            ddlempid.SelectedIndex = 0;
                        }
                        else
                        {
                            lblresult.Text = "Password Not Changed ";
                        }
                    }
                    else
                    {
                        lblresult.Text = "Password Not Match With Confirm Password";
                    }
                }
            }
            catch (Exception ex)
            {
                lblresult.Visible = true;

                lblresult.Text = "Incorrect Data";
            }
        }
    }
}