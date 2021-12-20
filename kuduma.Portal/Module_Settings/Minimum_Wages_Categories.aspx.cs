using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class Minimum_Wages_Categories : System.Web.UI.Page
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
            Txt_Minimum_Wages_Categories.Text = "";
            DataTable DtMinimum_Wages_Categories = GlobalData.Instance.LoadMinimumWagesCategories();
            if (DtMinimum_Wages_Categories.Rows.Count > 0)
            {
                Gv_Minimum_Wages_Categories.DataSource = DtMinimum_Wages_Categories;
                Gv_Minimum_Wages_Categories.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Records Are Not Avialable');", true);
                return;
            }
        }


       

        protected void Btn_Minimum_Wages_Categories_Click(object sender, EventArgs e)
        {
            try
            {
                #region Begin Code For  Validations   as [12-10-2013]
                if (Txt_Minimum_Wages_Categories.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Segment Name.');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var Minimum_Wages_CategoriesName = string.Empty;
                var IRecordStatus = 0;

                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                Minimum_Wages_CategoriesName = Txt_Minimum_Wages_Categories.Text.Trim().ToUpper();
                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]
                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AMMinimumWagesCategories";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@Name", Minimum_Wages_CategoriesName);
                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus =config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Record Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Recorde Not  Added.Because  The Name Already Exist. NOTE:Department Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [12-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [12-10-2013]
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [12-10-2013]

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Your session time out..');", true);
                return;
            }

        }

        protected void Gv_Minimum_Wages_Categories_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {

            try
            {

                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label Lbl_Minimum_Wages_Categories_Id = Gv_Minimum_Wages_Categories.Rows[e.RowIndex].FindControl("Lbl_Minimum_Wages_Categories_Id") as Label;
                TextBox Txt_Minimum_Wages_Categories_Name = Gv_Minimum_Wages_Categories.Rows[e.RowIndex].FindControl("txt_Minimum_Wages_Categories") as TextBox;
                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (Txt_Minimum_Wages_Categories_Name.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the  Name');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]


                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var Minimum_Wages_Categories_name = string.Empty;
                var Minimum_Wages_Categories_Id = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                Minimum_Wages_Categories_name = Txt_Minimum_Wages_Categories_Name.Text.Trim().ToUpper();
                Minimum_Wages_Categories_Id = Lbl_Minimum_Wages_Categories_Id.Text;
                ProcedureName = "AMMinimumWagesCategories";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@Name", Minimum_Wages_Categories_name);
                HtSPParameters.Add("@Id", Minimum_Wages_Categories_Id);
                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus =config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Record  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Record Not  Updated.Because  The Name Already Exist. NOTE: Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [14-10-2013]
                Gv_Minimum_Wages_Categories.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [14-10-2013]

            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your session time out...');", true);
                return;
            }


        }

        protected void Gv_Minimum_Wages_Categories_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            Gv_Minimum_Wages_Categories.EditIndex = e.NewEditIndex;
            Displaydata();
        }

        protected void Gv_Minimum_Wages_Categories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Gv_Minimum_Wages_Categories.EditIndex = -1;
            Displaydata();
        }

        protected void Gv_Minimum_Wages_Categories_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Gv_Minimum_Wages_Categories.PageIndex = e.NewPageIndex;
            Displaydata();
        }
    }
}