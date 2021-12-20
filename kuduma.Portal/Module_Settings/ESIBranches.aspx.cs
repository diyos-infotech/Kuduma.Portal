using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ESIBranches : System.Web.UI.Page
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
            txtEsibranchname.Text = "";
            txtesino.Text = "";
            DataTable DtEsibranches = GlobalData.Instance.LoadEsibranches();
            if (DtEsibranches.Rows.Count > 0)
            {
                gvEsibranches.DataSource = DtEsibranches;
                gvEsibranches.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('ESI Branches Are Not Avialable');", true);
                return;
            }

        }


      

        protected void btnAddesibranch_Click(object sender, EventArgs e)
        {
            try
            {

                var EsiBranchName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "";

                if (txtEsibranchname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter ESI Branch Name.');", true);
                    return;
                }

                if (txtesino.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter ESI No.');", true);
                    return;
                }

                EsiBranchName = txtEsibranchname.Text.Trim().ToUpper();
              string ESINo  = txtesino.Text.Trim().ToUpper();


                ProcedureName = "AddEsibranches";

                HtSPParameters.Add("@Esibranchname", EsiBranchName);
                HtSPParameters.Add("@ESINo", ESINo);

                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;

                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('ESI Branch Name Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('ESI Branch Not  Added.Because  The Name Already Exist.');", true);
                }

                Displaydata();


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Contact Your Admin..');", true);
                return;
            }

        }

        protected void gvEsibranches_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var ESIbranchname = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();

                Label Esibranchid = gvEsibranches.Rows[e.RowIndex].FindControl("lblBranchid") as Label;
                TextBox Esibranchname = gvEsibranches.Rows[e.RowIndex].FindControl("txtEsibranchname") as TextBox;
                TextBox ESINO = gvEsibranches.Rows[e.RowIndex].FindControl("txtESINO") as TextBox;


                if (Esibranchname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the ESI Branch  Name');", true);
                    return;
                }


                ESIbranchname = Esibranchname.Text.Trim().ToUpper();
                ProcedureName = "ModifyEsibranches";

                HtSPParameters.Add("@Esibranchname", ESIbranchname);
                HtSPParameters.Add("@Esibranchid", Esibranchid.Text);
                HtSPParameters.Add("@ESINO", ESINO.Text);

                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;


                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('ESI Branch Name  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('ESI Branch Name Not  Updated.Because  The Name Already Exist. NOTE:Bank Names Are UNIQUE');", true);
                }

                gvEsibranches.EditIndex = -1;
                Displaydata();


            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }

        }

        protected void gvEsibranches_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEsibranches.EditIndex = e.NewEditIndex;
            Displaydata();
        }
        protected void gvEsibranches_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEsibranches.EditIndex = -1;
            Displaydata();
        }
        protected void gvEsibranches_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEsibranches.PageIndex = e.NewPageIndex;
            Displaydata();
        }
    }
}