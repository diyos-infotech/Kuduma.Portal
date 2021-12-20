using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class PFBranches : System.Web.UI.Page
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
            txtPFbranchname.Text = "";
            txtpfno.Text = "";

            DataTable DtEsibranches = GlobalData.Instance.LoadPFbranches();
            if (DtEsibranches.Rows.Count > 0)
            {
                gvPFbranches.DataSource = DtEsibranches;
                gvPFbranches.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('PF Branches Are Not Avialable');", true);
                return;
            }

        }


      

        protected void btnAddPFbranch_Click(object sender, EventArgs e)
        {
            try
            {

                var EsiBranchName = string.Empty;
                var PFNo = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "";

                if (txtPFbranchname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter PF Branch Name.');", true);
                    return;
                }


                if (txtpfno.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter PF No.');", true);
                    return;
                }

                EsiBranchName = txtPFbranchname.Text.Trim().ToUpper();
                PFNo = txtpfno.Text.Trim();

                ProcedureName = "AddPFbranches";

                HtSPParameters.Add("@PFbranchname", EsiBranchName);
                HtSPParameters.Add("@PFNo", PFNo);

                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;

                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('PF Branch Name Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('PF Branch Not  Added.Because  The Name Already Exist.');", true);
                }

                Displaydata();


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Contact Your Admin..');", true);
                return;
            }

        }

        protected void gvpfbranches_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var ESIbranchname = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();

                Label Esibranchid = gvPFbranches.Rows[e.RowIndex].FindControl("lblBranchid") as Label;
                TextBox Esibranchname = gvPFbranches.Rows[e.RowIndex].FindControl("txtpfbranchname") as TextBox;
                TextBox ESINO = gvPFbranches.Rows[e.RowIndex].FindControl("txtpfNO") as TextBox;


                if (Esibranchname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the PF Branch  Name');", true);
                    return;
                }


                ESIbranchname = Esibranchname.Text.Trim().ToUpper();
                ProcedureName = "ModifyPFbranches";

                HtSPParameters.Add("@PFbranchname", ESIbranchname);
                HtSPParameters.Add("@PFbranchid", Esibranchid.Text);
                HtSPParameters.Add("@PFNO", ESINO.Text);

                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;


                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('PF Branch Name  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('PF Branch Name Not  Updated.Because  The Name Already Exist. NOTE:Bank Names Are UNIQUE');", true);
                }

                gvPFbranches.EditIndex = -1;
                Displaydata();


            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }

        }

        protected void gvpfbranches_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPFbranches.EditIndex = e.NewEditIndex;
            Displaydata();
        }
        protected void gvpfbranches_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPFbranches.EditIndex = -1;
            Displaydata();
        }
        protected void gvpfbranches_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPFbranches.PageIndex = e.NewPageIndex;
            Displaydata();
        }
    }
}