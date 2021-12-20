using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace GDX.Module_Settings
{
    public partial class HSNNumberList : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        DataTable dt;

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

            DataTable DtHSNNumbers = GlobalData.Instance.LoadHSNNumbers();
            if (DtHSNNumbers.Rows.Count > 0)
            {
                gvHSNNumber.DataSource = DtHSNNumbers;
                gvHSNNumber.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('HSN Number Names Are Not Avialable');", true);
                return;
            }
        }

        protected void gvHSNNumber_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvHSNNumber.EditIndex = e.NewEditIndex;
            Displaydata();
        }

        protected void gvHSNNumber_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvHSNNumber.EditIndex = -1;
            Displaydata();
        }

        protected void gvHSNNumber_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHSNNumber.PageIndex = e.NewPageIndex;
            gvHSNNumber.DataBind();
        }

        protected void gvHSNNumber_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {


                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label DeptId = gvHSNNumber.Rows[e.RowIndex].FindControl("lblDeptid") as Label;
                TextBox name = gvHSNNumber.Rows[e.RowIndex].FindControl("txtDept") as TextBox;
                TextBox txtRemarks = gvHSNNumber.Rows[e.RowIndex].FindControl("txtRemarks") as TextBox;

                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (name.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the HSN Number');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]


                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var Deptname = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                Deptname = name.Text.Trim();
                ProcedureName = "ModifyHSNNumber";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@HSNNumber", Deptname.ToUpper());
                HtSPParameters.Add("@HSNID", DeptId.Text);
                HtSPParameters.Add("@Remarks", txtRemarks.Text);

                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('HSN Number  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('HSN Number Not  Updated.Because  The HSN Number Already Exist. NOTE:HSN Numbers Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the HSNNumbers As on [14-10-2013]
                gvHSNNumber.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the HSNNumbers As on [14-10-2013]

            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }
        }

        protected void Btn_HSNNumber_Click(object sender, EventArgs e)
        {

            try
            {


                #region Begin Code For  Validations   as [12-10-2013]
                if (Txt_HSNNumber.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter HSN Number.');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var deptName = string.Empty;
                var Remarks = "";
                var IRecordStatus = 0;

                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                deptName = Txt_HSNNumber.Text.Trim().ToUpper();

                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]

                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AddHSNNumber";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@HSNNumber", deptName);
                HtSPParameters.Add("@Remarks", Remarks);

                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('HSN Number Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('HSN Number Not  Added.Because  The HSN Number Already Exist. NOTE:HSN Numbers Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [12-10-2013]

                #region  Begin Code For Re-Call All the HSNNumbers As on [12-10-2013]
                Displaydata();
                #endregion End Code For Re-Call All the HSNNumbers As on [12-10-2013]

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Your Admin..');", true);
                return;
            }
        }
    }
}