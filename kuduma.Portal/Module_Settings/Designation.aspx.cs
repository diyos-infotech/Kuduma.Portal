using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class Designation : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

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
                Displaydata();

            }
        }


        private void Displaydata()
        {


            ddlDutyType.Items.Clear();
            ddlDutyType.Items.Add("Daily");
            ddlDutyType.Items.Add("Hourly");
            DataTable DTMWCategoryNames = GlobalData.Instance.LoadMinimumWagesCategories();
            if (DTMWCategoryNames.Rows.Count > 0)
            {
                ddl_MWC_Category.DataValueField = "id";
                ddl_MWC_Category.DataTextField = "Name";
                ddl_MWC_Category.DataSource = DTMWCategoryNames;
                ddl_MWC_Category.DataBind();
            }

            ddl_MWC_Category.Items.Insert(0, "-Select-");

            DataTable DtDesigns = GlobalData.Instance.LoadDesigns();
            if (DtDesigns.Rows.Count > 0)
            {
                gvdesignation.DataSource = DtDesigns;
                gvdesignation.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Designation Names Are Not Avialable');", true);
                return;
            }





        }

        protected void gvdesignation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvdesignation.EditIndex = e.NewEditIndex;
            Displaydata();
            DropDownList ddlDuty = gvdesignation.Rows[e.NewEditIndex].FindControl("ddlDutyType") as DropDownList;
            FillGVDutyType(ddlDuty);

            DropDownList ddl_MWC_Category = gvdesignation.Rows[e.NewEditIndex].FindControl("ddl_MWC_Category") as DropDownList;
            Fill_MWC_Category(ddl_MWC_Category);
        }

        protected void FillGVDutyType(DropDownList dllList)
        {
            dllList.Items.Clear();
            dllList.Items.Add("Daily");
            dllList.Items.Add("Hourly");
        }


        protected void Fill_MWC_Category(DropDownList dllList)
        {
            dllList.Items.Clear();
            DataTable DTMWCategoryNames = GlobalData.Instance.LoadMinimumWagesCategories();
            if (DTMWCategoryNames.Rows.Count > 0)
            {
                dllList.DataValueField = "id";
                dllList.DataTextField = "Name";
                dllList.DataSource = DTMWCategoryNames;
                dllList.DataBind();
            }

        }


        protected void gvdesignation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblresult.Text = "";
            gvdesignation.EditIndex = -1;
            Displaydata();
        }

        protected void Btn_Designation_Click(object sender, EventArgs e)
        {
            try
            {


                #region Begin Code For  Validations   as [12-10-2013]
                if (Txt_Desgn.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Designation Name.');", true);
                    return;
                }

                if (ddl_MWC_Category.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please The Category.');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var desgnName = string.Empty;
                var IRecordStatus = 0;
                var dutyType = 0;

                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                desgnName = Txt_Desgn.Text.Trim().ToUpper();
                if (ddlDutyType.SelectedIndex == 0)
                    dutyType = 1;
                else
                    dutyType = 0;
                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]

                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AddDesignations";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@design", desgnName);
                HtSPParameters.Add("@dutyType", dutyType);

                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Designation Name Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Designation Name Not  Added.Because  The Name Already Exist. NOTE:Department Names Are UNIQUE');", true);
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

        protected void gvdesignation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvdesignation.PageIndex = e.NewPageIndex;
            Displaydata();
        }

        protected void gvdesignation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {


                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label desginid = gvdesignation.Rows[e.RowIndex].FindControl("lbldesgnid") as Label;
                TextBox dsgn = gvdesignation.Rows[e.RowIndex].FindControl("txtdesgn") as TextBox;
                DropDownList ddlDuty = gvdesignation.Rows[e.RowIndex].FindControl("ddlDutyType") as DropDownList;
                DropDownList ddl_MWC_Category = gvdesignation.Rows[e.RowIndex].FindControl("ddl_MWC_Category") as DropDownList;

                int dutyType = 1;
                float DutyHours = 8;
                if (ddlDuty.SelectedIndex == 0)
                    dutyType = 1;
                else
                    dutyType = 0;
                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (dsgn.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Designation  Name');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]


                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var Designationname = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                Designationname = dsgn.Text.Trim().ToUpper();
                ProcedureName = "ModifyDesignations";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@Design", Designationname);
                HtSPParameters.Add("@DesignId", desginid.Text);
                HtSPParameters.Add("@DutyType", dutyType);
                HtSPParameters.Add("@MWCID", ddl_MWC_Category.SelectedValue);


                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus =config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Designation Name  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Designation Name Not  Updated.Because  The Name Already Exist. NOTE:Department Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [14-10-2013]
                gvdesignation.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [14-10-2013]

            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }

        }

        protected void ddlDutyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDutyType.SelectedIndex == 1)
            {
                lblDutyHours.Visible = true;
                txtDutyHours.Visible = true;
            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("Designation.xls", this.gvdesignation);
        }
    }
}