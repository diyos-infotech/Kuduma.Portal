using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class Resources : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                        GetResourceNames();
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }

                    Displaydata();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Displaydata()
        {

            ddlResource.SelectedIndex = 0;
            txtResourceprice.Text = "";

            DataTable DtResources = GlobalData.Instance.LoadResources();
            if (DtResources.Rows.Count > 0)
            {
                gvResource.DataSource = DtResources;
                gvResource.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Resource Names Are Not Avialable');", true);
                return;
            }
        }


       

        protected void GetResourceNames()
        {
            DataTable dtItems = GlobalData.Instance.LoadItems();
            if (dtItems.Rows.Count > 0)
            {
                ddlResource.DataValueField = "ItemId";
                ddlResource.DataTextField = "ItemName";
                ddlResource.DataSource = dtItems;
                ddlResource.DataBind();
            }
            ddlResource.Items.Insert(0, "--Select--");

        }

        protected void Btn_Resource_Click(object sender, EventArgs e)
        {


            try
            {


                #region Begin Code For  Validations   as [12-10-2013]
                string price = txtResourceprice.Text.Trim();
                if (ddlResource.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Resource Name');", true);
                    return;
                }
                if (txtResourceprice.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Enter Resource Price');", true);
                    return;
                }

                if (float.Parse(price) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Enter valid Resource Price');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var ResourceId = string.Empty;
                var ResourcePrice = string.Empty;
                var IRecordStatus = 0;

                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                ResourceId = ddlResource.SelectedValue;
                ResourcePrice = price;

                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]

                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AddResources";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@ResourceID", ResourceId);
                HtSPParameters.Add("@price", ResourcePrice);

                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus =config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Resource Name Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Resource Name Not  Added.Because  The Name Already Exist. NOTE:Resource Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [12-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [12-10-2013]
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [12-10-2013]

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Your Admin..');", true);
                return;
            }


        }

        protected void gvResource_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {

            try
            {

                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label Resourceid = gvResource.Rows[e.RowIndex].FindControl("lblResourceid") as Label;
                TextBox Resourceprice = gvResource.Rows[e.RowIndex].FindControl("txtResourcePrice") as TextBox;
                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]

                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (Resourceprice.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Resource  Price');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]

                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var ResourceId = string.Empty;
                var ResourcePrice = string.Empty;


                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                ResourceId = Resourceid.Text.Trim();
                ResourcePrice = Resourceprice.Text.Trim();

                ProcedureName = "ModifyResources";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@Price", ResourcePrice);
                HtSPParameters.Add("@ResourceID", ResourceId);
                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Resource Price  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Resource Price Not  Updated.');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [14-10-2013]
                gvResource.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [14-10-2013]

            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }

        }

        protected void gvResource_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            gvResource.EditIndex = e.NewEditIndex;
            Displaydata();
        }

        protected void gvResource_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvResource.EditIndex = -1;
            Displaydata();
        }

        protected void gvResource_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResource.PageIndex = e.NewPageIndex;
            Displaydata();
        }
    }
}