using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class LicenceExpireDetailsReports : System.Web.UI.Page
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

                if (this.Master != null)
                {
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli2");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                DataBinder();
            }

        }

     

        protected void DataBinder()
        {
            LblResult.Text = "";
            LblResult.Visible = true;//SELECT * FROM (SELECT * FROM topten ORDER BY datetime DESC) tmp GROUP BY home
            //string sqlQry = "Select Licenses.UnitId,LicenseStartDate,LicenseEndDate,LicenseExpired," +
            //    "LicenseOfficeLoc,Clients.ClientName from Clients Inner JOIN  Licenses ON " +
            //    "Licenses.UnitId=Clients.ClientId and LicenseEndDate<='" + DateTime.Now.AddMonths(1).ToShortDateString() + "'";
            string sqlQry = "Select Licenses.UnitId,LicenseStartDate,LicenseEndDate,LicenseExpired," +
                "LicenseOfficeLoc,Clients.ClientName from clients Inner JOIN  Licenses ON " +
                "Licenses.UnitId=Clients.ClientId and LicenseEndDate<='" + DateTime.Now.AddMonths(1).ToShortDateString() + "'";

            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            sqlQry = "Select ClientId,ClientName from Clients";
            DataTable clientTable = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            DateTime date = new DateTime();
            for (int i = 0; i < clientTable.Rows.Count; i++)
            {
                sqlQry = "Select UnitId from Licenses where UnitId='" + clientTable.Rows[i]["ClientId"].ToString() + "'";
                DataTable noLicTable = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
                if (noLicTable.Rows.Count == 0)
                {
                    DataRow row = data.NewRow();
                    row["UnitId"] = clientTable.Rows[i]["ClientId"].ToString();
                    row["ClientName"] = clientTable.Rows[i]["ClientName"].ToString();
                    row["LicenseStartDate"] = date.Date.ToString();
                    row["LicenseEndDate"] = date.Date.ToString();
                    row["LicenseExpired"] = "True";
                    row["LicenseOfficeLoc"] = "License not taken yet";
                    data.Rows.Add(row);
                }
            }
            if (data.Rows.Count > 0)
            {

                dgLicExpire.DataSource = data;
                dgLicExpire.DataBind();
            }
            else
            {
                LblResult.Text = "There is no Licenses Expiring till next month";
            }
        }
        protected void btncancel_Click(object sender, EventArgs e)
        {
            // ClearAll();
        }
        protected void dgLicExpire_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgLicExpire.PageIndex = e.NewPageIndex;
            DataBinder();
        }
    }
}