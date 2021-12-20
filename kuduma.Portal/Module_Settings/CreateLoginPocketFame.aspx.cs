using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class CreateLoginPocketFame : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                BranchID = Session["BranchID"].ToString();
                if (!IsPostBack)
                {
                    LoadClientids();
                    Loadshifts();
                    LoadRoles();
                }
            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
                Response.Redirect("~/login.aspx");
            }
        }

        protected void LoadRoles()
        {
            string query = "select * from RoleNames";
            DataTable dtRoless = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dtRoless.Rows.Count > 0)
            {
                ddlrole.DataValueField = "RoleID";
                ddlrole.DataTextField = "RoleName";
                ddlrole.DataSource = dtRoless;
                ddlrole.DataBind();
            }

            ddlrole.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,unitid from empdetails where empid='" + txtEmpid.Text + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtName.Text = dt.Rows[0]["empname"].ToString();
                    txtName.Text = dt.Rows[0]["empname"].ToString();
                }
                catch (Exception ex)
                {

                }
            }
            else
            {

            }


        }

        protected void txtFname_TextChanged(object sender, EventArgs e)
        {
            Getempid();
            GetData();
        }

        protected void Getempid()
        {

            string Sqlqry = "select  empid,unitid from empdetails where empfname+' '+empmname+' '+emplname like '%" + txtName.Text + "%' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["empid"].ToString();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
            }

        }

        protected void Loadshifts()
        {
            string query = "select * from shifts";

            DataTable dtshifts = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dtshifts.Rows.Count > 0)
            {
                ddlshift.DataValueField = "Shift";
                ddlshift.DataTextField = "Shift";
                ddlshift.DataSource = dtshifts;
                ddlshift.DataBind();
            }

            ddlshift.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void LoadClientids()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                ddlsiteposted.DataValueField = "clientid";
                ddlsiteposted.DataTextField = "clientid";
                ddlsiteposted.DataSource = dt;
                ddlsiteposted.DataBind();
            }

            ddlsiteposted.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void txtemplyid_TextChanged(object sender, EventArgs e)
        {
            GetEmpName();
            GetData();
        }

        public void GetData()
        {
            if (txtEmpid.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Emp ID');", true);
                return;
            }

            if (txtName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Emp Name');", true);
                return;
            }


            string ChkQry = "select * from Logindetails_Android where companyid=36 and EmpId='" + txtEmpid.Text + "' ";
            DataTable chkresult = config.PocketFameExecuteAdaptorAsyncWithQueryParams(ChkQry).Result;
            if (chkresult.Rows.Count > 0)
            {
                txtusrname.Text = chkresult.Rows[0]["UserName"].ToString();
                txtPassword.Text = chkresult.Rows[0]["Password"].ToString();
                txtConfirmPassword.Text = chkresult.Rows[0]["Password"].ToString();
                ddlrole.SelectedValue = chkresult.Rows[0]["Role"].ToString();

                string EmpQry = "select * from empdetails where EmpId='" + txtEmpid.Text + "' ";
                DataTable chkEmpQry = config.ExecuteAdaptorAsyncWithQueryParams(EmpQry).Result;
                ddlsiteposted.SelectedValue = chkEmpQry.Rows[0]["Sitepostedto"].ToString();
                ddlshift.SelectedValue = chkEmpQry.Rows[0]["Shift"].ToString();
                txtShiftstarttime.Text = chkEmpQry.Rows[0]["ShiftStartTime"].ToString();
                txtShiftEndtime.Text = chkEmpQry.Rows[0]["ShiftEndTime"].ToString();

            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {

            if (txtEmpid.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Emp ID');", true);
                return;
            }

            if (txtName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Emp Name');", true);
                return;
            }

            if (ddlrole.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select Role');", true);
                return;
            }

            if (ddlshift.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select shift');", true);
                return;
            }

            if (ddlsiteposted.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select site posted');", true);
                return;
            }

            if (txtusrname.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill User Name');", true);
                return;
            }

            if (txtPassword.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Password');", true);
                return;
            }

            if (txtConfirmPassword.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please fill Confirm password');", true);
                return;
            }
            if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Password does not match');", true);
                return;
            }



            var Shift = "";
            var Shiftstarttime = "";
            var Shiftendtime = "";
            var RoleID = "0";
            var Empid = txtEmpid.Text;

            Shift = ddlshift.SelectedValue;
            Shiftstarttime = txtShiftstarttime.Text;
            Shiftendtime = txtShiftEndtime.Text;

            RoleID = ddlrole.SelectedValue;

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + numbers;

            int length = 36;
            string RandomID = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (RandomID.IndexOf(character) != -1);
                RandomID += character;
            }


            string query = "Update empdetails set RoleID='" + RoleID + "',Shift='" + Shift + "',Shiftstarttime='" + Shiftstarttime + "',unitid='" + ddlsiteposted.SelectedValue + "',sitepostedto='" + ddlsiteposted.SelectedValue + "',Shiftendtime='" + Shiftendtime + "' where empid='" + Empid + "'";
            int result = config.ExecuteNonQueryWithQueryAsync(query).Result;
            if (result > 0)
            {
                string ChkQry = "select * from Logindetails_Android where companyid=36 and EmpId='" + txtEmpid.Text + "' ";
                DataTable chkresult = config.PocketFameExecuteAdaptorAsyncWithQueryParams(ChkQry).Result;
                if (chkresult.Rows.Count > 0)
                {
                    string updatequery = "Update Logindetails_Android set UserName='" + txtusrname.Text.Trim() + "',Password='" + txtConfirmPassword.Text.Trim() + "',UpdatedDateTime=getdate(),Role='" + ddlrole.SelectedValue + "' where empid='" + Empid + "' and companyid=36";
                    int updateresult = config.PocketFameExecuteNonQueryWithQueryAsync(updatequery).Result;
                    if (updateresult > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Details modified successfully');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Details modified Unsuccessfully');", true);
                        return;
                    }

                }
                else
                {
                    string insertquery = "insert into Logindetails_Android (Id,	EmpId,	UserName,	Password,	CompanyId,	CreatedDateTime	,UpdatedDateTime,	EmailId	,Role,	PushCode,	Rated	,ClientId) values (newid(),'" + txtEmpid.Text + "','" + txtusrname.Text.Trim() + "','" + txtConfirmPassword.Text.Trim() + "',36,Getdate(),'','','" + ddlrole.SelectedValue + "','',0,'')";
                    int insertresult = config.PocketFameExecuteNonQueryWithQueryAsync(insertquery).Result;
                    if (insertresult > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Details saved successfully');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Details saved Unsuccessfully');", true);
                        return;
                    }
                }

                txtEmpid.Text = "";
                txtName.Text = "";
                txtShiftstarttime.Text = "";
                txtShiftEndtime.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                ddlshift.SelectedIndex = 0;
                ddlrole.SelectedIndex = 0;
            }
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "select ShiftStartTime,ShiftEndTime from shifts where shift='" + ddlshift.SelectedValue + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                txtShiftstarttime.Text = dt.Rows[0]["ShiftStartTime"].ToString();
                txtShiftEndtime.Text = dt.Rows[0]["ShiftEndTime"].ToString();
            }
        }
    }
}