using System;
using System.Data;
using System.Web.UI.HtmlControls;
using KLTS.Data;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class ClientLicenses : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
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
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("c2");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                DataBinder();
                LoadClientList();
                LoadClientNames();
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void DataBinder()
        {
            string sqlQry = "Select Licenses.UnitId,LicenseStartDate,LicenseEndDate,LicenseExpired,LicenseOfficeLoc,Clients.ClientName from Clients INNER JOIN Licenses ON Licenses.UnitId=Clients.ClientId AND LicenseEndDate>='"
                + DateTime.Now.ToString("MM/dd/yyyy") + "' AND LicenseEndDate<='" + DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy") + "'";
            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            dgLicExpire.DataSource = data;
            dgLicExpire.DataBind();
        }

        protected void btnaddclint_Click(object sender, EventArgs e)
        {
            if (ddlClientId.SelectedIndex == 0)
            {
                lblresult.Text = "Please Select Client Id";
                return;
            }

            if (ddlClientId.SelectedIndex > 0)
            {
                string licNo = txtLicenseNo.Text.Trim();
                string licOfficeLoc = txtLicOffLoc.Text.Trim();
                string sLic = txtLicStart.Text.ToString().Trim();
                string eLic = txtLicEnd.Text.ToString().Trim();
                if (licNo.Length == 0)
                {
                    lblresult.Text = "Enter License No";
                    return;
                }
                if (sLic.Length == 0)
                {
                    lblresult.Text = "Enter License Start Date";
                    return;
                }
                if (eLic.Length == 0)
                {
                    lblresult.Text = "Enter License End Date";
                    return;
                }

                try
                {
                    string insertquery = string.Format("insert into Licenses(LicenseNo,UnitId,LicenseStartDate,LicenseEndDate,LicenseOfficeLoc) values('{0}','{1}','{2}','{3}','{4}')",
                    licNo, ddlClientId.SelectedValue, sLic, eLic, licOfficeLoc);
                    int status =config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                    if (status != 0)
                    {
                        lblresult.Visible = true;
                        lblresult.Text = "Record Inserted  Successfully";
                    }
                    else
                    {
                        lblresult.Visible = true;
                        lblresult.Text = "Record not  Inserted ";
                    }
                    DataBinder();
                }
                catch (Exception ex)
                {
                    lblresult.Visible = true;
                    lblresult.Text = "Plz Fill The Details";
                }
            }
        }

        protected void ddlClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlClientId.SelectedIndex > 0)
            //{
            //    string sqlQry = "Select ClientName from Clients where ClientId='" + ddlClientId.SelectedValue + "'";
            //    DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            //    if (data.Rows.Count > 0)
            //    {
            //        ddlCname.SelectedValue = data.Rows[0]["clientname"].ToString();
            //    }
            //}
            //else
            //{
            //    ddlCname.SelectedIndex = 0;
            //}

            if (ddlClientId.SelectedIndex > 0)
            {
                ddlCname.SelectedValue = ddlClientId.SelectedValue;
            }
            else
            {
                ddlCname.SelectedIndex = 0;
            }
        }


        protected void ClearAll()
        {
            txtLicenseNo.Text = "";
            txtLicOffLoc.Text = "";
            txtLicStart.Text = "";
            txtLicEnd.Text = "";

        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        protected void ddlCname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCname.SelectedIndex > 0)
            {
                ddlClientId.SelectedValue = ddlCname.SelectedValue;
            }
            else
            {
                ddlClientId.SelectedIndex = 0;
            }
        }
        protected void LoadClientList()
        {
            string qry = "select Clientid from clients where clientid like '%" + CmpIDPrefix + "%' order by clientid ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlClientId.DataValueField = "clientid";
                ddlClientId.DataTextField = "clientid";
                ddlClientId.DataSource = dt;
                ddlClientId.DataBind();
            }
            ddlClientId.Items.Insert(0, "--Select--");
            ddlClientId.Items.Insert(1, "ALL");


        }

        protected void LoadClientNames()
        {

            string qry = "select Clientid,Clientname from clients where clientid like '%" + CmpIDPrefix + "%'  order by clientname";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            if (dt.Rows.Count > 0)
            {
                ddlCname.DataValueField = "clientid";
                ddlCname.DataTextField = "Clientname";
                ddlCname.DataSource = dt;
                ddlCname.DataBind();
            }
            ddlCname.Items.Insert(0, "--Select--");
            ddlCname.Items.Insert(1, "ALL");


        }


    }
}