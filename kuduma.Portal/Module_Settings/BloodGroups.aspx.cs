using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class BloodGroups : System.Web.UI.Page
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
            Txt_BloodGroup.Text = "";
            DataTable DtBloodGroup = GlobalData.Instance.LoadBloodGroupNames();
            if (DtBloodGroup.Rows.Count > 0)
            {
                gvBloodGroup.DataSource = DtBloodGroup;
                gvBloodGroup.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Blood Group Names Are Not Avialable');", true);
                return;
            }
        }


       



        protected void Btn_BloodGroup_Click(object sender, EventArgs e)
        {
            try
            {
                #region Begin Code For  Validations   as [12-10-2013]
                if (Txt_BloodGroup.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Blood Group Name.');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var BloodGroupName = string.Empty;
                var IRecordStatus = 0;

                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                BloodGroupName = Txt_BloodGroup.Text.Trim().ToUpper();
                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]
                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AddBloodGroup";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@BloodGroupName", BloodGroupName);
                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus =config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Blood Group Name Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Blood Group Name Not  Added.Because  The Name Already Exist. NOTE:Blood Group Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [12-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [12-10-2013]
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [12-10-2013]

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Contact Your Admin..');", true);
                return;
            }

        }

        protected void gvBloodGroup_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {

                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label bgid = gvBloodGroup.Rows[e.RowIndex].FindControl("lblBloodGroupid") as Label;
                TextBox bgname = gvBloodGroup.Rows[e.RowIndex].FindControl("txtBloodGroupName") as TextBox;
                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (bgname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Blood Group  Name');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]


                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var BloodGroupname = string.Empty;
                var BloodGroupid = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                BloodGroupname = bgname.Text.Trim().ToUpper();
                BloodGroupid = bgid.Text;
                ProcedureName = "ModifyBloodGroupNames";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@BloodGroupName", BloodGroupname);
                HtSPParameters.Add("@BloodGroupId", BloodGroupid);
                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus =config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Blood Group Name  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Blood Group Name Not  Updated.Because  The Name Already Exist. NOTE:Blood Group Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [14-10-2013]
                gvBloodGroup.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [14-10-2013]

            }

            catch (Exception ex)
            {

                // ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }


        }

        protected void gvBloodGroup_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBloodGroup.EditIndex = e.NewEditIndex;
            Displaydata();
        }

        protected void gvBloodGroup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBloodGroup.EditIndex = -1;
            Displaydata();
        }

        protected void gvBloodGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBloodGroup.PageIndex = e.NewPageIndex;
            Displaydata();
        }
    }
}