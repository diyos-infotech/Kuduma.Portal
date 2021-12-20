using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Employees
{
    public partial class EmpShiftDetails : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            lblresult.Text = string.Empty;
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

                    if (Request.QueryString["Empid"] != null)
                    {
                        string username = Request.QueryString["Empid"].ToString();
                        txtemplyid.Text = username;
                        txtemplyid_TextChanged(sender, e);
                        GetData();

                    }

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
                ddlRole.DataValueField = "RoleID";
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataSource = dtRoless;
                ddlRole.DataBind();
            }

            ddlRole.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname from empdetails where empid='" + txtemplyid.Text + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtFname.Text = dt.Rows[0]["empname"].ToString();

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

        protected void txtFname_TextChanged(object sender, EventArgs e)
        {
            Getempid();
            GetData();
        }

        protected void Getempid()
        {

            string Sqlqry = "select  empid from empdetails where empfname+' '+empmname+' '+emplname like '%" + txtFname.Text + "%' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtemplyid.Text = dt.Rows[0]["empid"].ToString();

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

        protected void Loadshifts()
        {
            string query = "select * from shifts";

            DataTable dtshifts = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dtshifts.Rows.Count > 0)
            {
                ddlShift.DataValueField = "Shift";
                ddlShift.DataTextField = "Shift";
                ddlShift.DataSource = dtshifts;
                ddlShift.DataBind();
            }

            ddlShift.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void LoadClientids()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                DdlPreferedUnit.DataValueField = "clientid";
                DdlPreferedUnit.DataTextField = "clientid";
                DdlPreferedUnit.DataSource = dt;
                DdlPreferedUnit.DataBind();
            }

            DdlPreferedUnit.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        public void GetData()
        {
            string query = "select isnull(Shift,0) Shift,ShiftStartTime,ShiftEndTime,isnull(Woff1,0) Woff1,isnull(Woff2,0) Woff2,isnull(SitePostedTo,0) SitePostedTo,Address,Name,RoleId from EmpDetails where empid='" + txtemplyid.Text + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SitePostedTo"].ToString() != "0")
                {
                    DdlPreferedUnit.SelectedValue = dt.Rows[0]["SitePostedTo"].ToString();
                }
                else
                {
                    DdlPreferedUnit.SelectedIndex = 0;
                }
                if (dt.Rows[0]["RoleId"].ToString() != "0")
                {
                    ddlRole.SelectedValue = dt.Rows[0]["RoleId"].ToString();
                }
                else
                {
                    ddlRole.SelectedIndex = 0;
                }

                txtEmpFName.Text = dt.Rows[0]["Name"].ToString();
                if (dt.Rows[0]["Shift"].ToString() != "0")
                {
                    ddlShift.SelectedValue = dt.Rows[0]["Shift"].ToString();
                }
                else
                {
                    ddlShift.SelectedIndex = 0;
                }
                txtShiftstarttime.Text = dt.Rows[0]["ShiftStartTime"].ToString();
                txtShiftEndtime.Text = dt.Rows[0]["ShiftEndTime"].ToString();

                if (dt.Rows[0]["Woff1"].ToString() != "0")
                {
                    ddlWoff1.SelectedIndex = int.Parse(dt.Rows[0]["Woff1"].ToString());
                }
                else
                {
                    ddlWoff1.SelectedIndex = 0;
                }

                if (dt.Rows[0]["Woff2"].ToString() != "0")
                {
                    ddlWoff2.SelectedIndex = int.Parse(dt.Rows[0]["Woff2"].ToString());
                }
                else
                {
                    ddlWoff2.SelectedIndex = 0;
                }
                txtAddress.Text = dt.Rows[0]["Address"].ToString();
            }
            else
            {
                DdlPreferedUnit.SelectedIndex = 0;
                txtEmpFName.Text = "";
                ddlShift.SelectedIndex = 0;
                txtShiftstarttime.Text = "";
                txtShiftEndtime.Text = "";
                ddlWoff1.SelectedIndex = 0;
                ddlWoff2.SelectedIndex = 0;
                txtAddress.Text = "";
            }
        }

        protected void txtemplyid_TextChanged(object sender, EventArgs e)
        {
            GetEmpName();
            GetData();
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            var Sitepostedto = "";
            var Name = "";
            var Shift = "";
            var Shiftstarttime = "";
            var Shiftendtime = "";
            var Woff1 = 0;
            var Woff2 = 0;
            var Address = "";
            var RoleID = "0";
            var Empid = txtemplyid.Text;
            Sitepostedto = DdlPreferedUnit.SelectedValue;
            Name = txtEmpFName.Text;
            Shift = ddlShift.SelectedValue;
            Shiftstarttime = txtShiftstarttime.Text;
            Shiftendtime = txtShiftEndtime.Text;
            Woff1 = ddlWoff1.SelectedIndex;
            Woff2 = ddlWoff2.SelectedIndex;
            Address = txtAddress.Text;

            if (ddlRole.SelectedIndex == 0)
            {
                RoleID = "0";
            }
            else
            {
                RoleID = ddlRole.SelectedValue;
            }

            string query = "Update empdetails set RoleID='" + RoleID + "',SitePostedTo='" + Sitepostedto + "',Unitid='" + Sitepostedto + "',Shift='" + Shift + "',Shiftstarttime='" + Shiftstarttime + "',Shiftendtime='" + Shiftendtime + "',Woff1='" + Woff1 + "',Woff2='" + Woff2 + "',Address='" + Address + "' where empid='" + Empid + "'";
            int result = config.ExecuteNonQueryWithQueryAsync(query).Result;
            if (result > 0)
            {
                lblresult.Text = "Employee Shift Details Updated Sucessfully";
            }
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "select ShiftStartTime,ShiftEndTime from shifts where shift='" + ddlShift.SelectedValue + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                txtShiftstarttime.Text = dt.Rows[0]["ShiftStartTime"].ToString();
                txtShiftEndtime.Text = dt.Rows[0]["ShiftEndTime"].ToString();
            }
        }
    }
}