using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class TaxComponents : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Fontstyle = "";
        string CFontstyle = "";
        string Created_By = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GetWebConfigdata();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }

                    GetTaxComponentsdata();

                }
            }
            catch (Exception ex)
            {

            }
        }


        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }


        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Created_By = Session["UserID"].ToString();
        }



        public void GetTaxComponentsdata()
        {

            string qry = "select TaxCmpID,TaxCmpName,TaxCmpPer,case visibility when 'Y' then cast('True' as bit) else '' end Visibility from TaxComponentsMaster ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count > 0)
            {
                GVTaxComponents.DataSource = dt;
                GVTaxComponents.DataBind();
            }
            else
            {
                GVTaxComponents.DataSource = null;
                GVTaxComponents.DataBind();
            }

        }




        protected void GVTaxComponents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GVTaxComponents.EditIndex = e.NewEditIndex;
            GetTaxComponentsdata();

        }
        protected void GVTaxComponents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GVTaxComponents.EditIndex = -1;
            GetTaxComponentsdata();
        }
        protected void GVTaxComponents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string CmpName = "";
            string Visibility = "N";
            string CmpID = "";
            string CmpPer = "0";
            var IRecordStatus = 0;

            Hashtable HtSPParameters = new Hashtable();
            var ProcedureName = string.Empty;

            Label lblEditCmpID = GVTaxComponents.Rows[e.RowIndex].FindControl("lblEditCmpID") as Label;
            TextBox txtComponent = GVTaxComponents.Rows[e.RowIndex].FindControl("txtComponent") as TextBox;
            TextBox txtCmpper = GVTaxComponents.Rows[e.RowIndex].FindControl("txtCmpper") as TextBox;
            CheckBox ChkEditVisibility = GVTaxComponents.Rows[e.RowIndex].FindControl("ChkEditVisibility") as CheckBox;

            CmpName = txtComponent.Text;
            CmpID = lblEditCmpID.Text;
            CmpPer = txtCmpper.Text;

            if (ChkEditVisibility.Checked)
            {
                Visibility = "Y";
            }

            ProcedureName = "ModifyTaxComponentsMaster";
            HtSPParameters.Add("@TaxCmpname", CmpName);
            HtSPParameters.Add("@Visibility", Visibility);
            HtSPParameters.Add("@Taxcmpid", CmpID);
            HtSPParameters.Add("@TaxCmpPer", CmpPer);
            IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;

            if (IRecordStatus > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Component data updated sucessfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Component data is not updated sucessfully.');", true);
            }

            GVTaxComponents.EditIndex = -1;
            GetTaxComponentsdata();
        }
    }
}