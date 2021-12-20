using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class Previligers : System.Web.UI.Page
    {
        DropDownList bind_dropdownlist;
        DropDownList bind_menu;
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        private void Displaydata()
        {
            Txt_Previliger.Text = "";

            DataTable DtPreviligers = GlobalData.Instance.LoadPreviligers();
            if (DtPreviligers.Rows.Count > 0)
            {
                gvpreviligers.DataSource = DtPreviligers;
                gvpreviligers.DataBind();


                foreach (GridViewRow grdRow in gvpreviligers.Rows)
                {
                    bind_dropdownlist = (DropDownList)(gvpreviligers.Rows[grdRow.RowIndex].Cells[0].FindControl("ddlPriority"));
                    bind_menu = (DropDownList)(gvpreviligers.Rows[grdRow.RowIndex].Cells[0].FindControl("ddlEMenu"));

                    string sqlselect = "select PreviligerId from Previligers";
                    DataTable dtPriority = config.ExecuteAdaptorAsyncWithQueryParams(sqlselect).Result;
                    bind_dropdownlist.DataValueField = "PreviligerId";
                    bind_dropdownlist.DataTextField = "PreviligerId";
                    bind_dropdownlist.DataSource = dtPriority;
                    bind_dropdownlist.DataBind();
                    bind_dropdownlist.Items.Insert(0, "-Select-");

                    string sql = "select distinct REDIRECT_PAGE from Menu order by 1";
                    DataTable dtmenu = config.ExecuteAdaptorAsyncWithQueryParams(sql).Result;
                    bind_menu.DataValueField = "REDIRECT_PAGE";
                    bind_menu.DataTextField = "REDIRECT_PAGE";
                    bind_menu.DataSource = dtmenu;
                    bind_menu.DataBind();
                    bind_menu.Items.Insert(0, "-Select-");

                    Label lblpreviligerid = (Label)(gvpreviligers.Rows[grdRow.RowIndex].Cells[0].FindControl("lblpreviligerid"));
                    string sqlqry = "select Priority from   Previligers where PreviligerId = " + lblpreviligerid.Text;
                    DataTable dtPriority1 = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;

                    if (dtPriority1.Rows.Count > 0)
                    {
                        bind_dropdownlist.SelectedValue = dtPriority1.Rows[0][0].ToString();

                    }

                    string sqlqry1 = "select distinct StartPage from previligers where PreviligerId = " + lblpreviligerid.Text;
                    DataTable dtmenu1 = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry1).Result;

                    if (dtmenu1.Rows.Count > 0)
                    {
                        bind_menu.SelectedValue = dtmenu1.Rows[0][0].ToString();

                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Previliger Names Are Not Avialable');", true);
                return;
            }
        }

        private void BindMenuddl()
        {

            string sqlselect = "select distinct REDIRECT_PAGE from Menu order by 1";
            DataTable dtPriority = config.ExecuteAdaptorAsyncWithQueryParams(sqlselect).Result;


            ddlmenu.DataValueField = "REDIRECT_PAGE";
            ddlmenu.DataTextField = "REDIRECT_PAGE";
            ddlmenu.DataSource = dtPriority;
            ddlmenu.DataBind();
            ddlmenu.Items.Insert(0, "-Select-");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "SETTINGS: PREVILIGERS";
                if (Session["UserId"] == null && Session["AccessLevel"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                BindMenuddl();
                Displaydata();
            }
        }

        protected void Btn_Previliger_Click(object sender, EventArgs e)
        {
            try
            {
                #region Begin Code For  Validations   as [12-10-2013]
                if (Txt_Previliger.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Previliger Name.');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var PreviligerName = string.Empty;
                var IRecordStatus = 0;

                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                PreviligerName = Txt_Previliger.Text.Trim().ToUpper();
                var stpage = ddlmenu.SelectedValue.ToString();
                if (stpage == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Plz select start page.');", true);
                    return;
                }
                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]
                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AddPreviligers";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@PreviligerName", PreviligerName);
                HtSPParameters.Add("@startpage", stpage);
                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Previliger Name Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Previliger Name Not  Added.Because  The Name Already Exist. NOTE:Previliger Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [12-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [12-10-2013]
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [12-10-2013]

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Contact Your Admin..');", true);
                return;
            }

        }

        protected void gvpreviligers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {

                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label prid = gvpreviligers.Rows[e.RowIndex].FindControl("lblpreviligerid") as Label;
                TextBox prname = gvpreviligers.Rows[e.RowIndex].FindControl("txtpreviligerName") as TextBox;

                DropDownList ddlPrioriy = gvpreviligers.Rows[e.RowIndex].FindControl("ddlPriority") as DropDownList;

                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (prname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Previliger  Name');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]


                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var Previligername = string.Empty;
                var Previligerid = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;

                var Priority = string.Empty;
                if (ddlPrioriy.SelectedIndex > 0)
                {
                    Priority = ddlPrioriy.SelectedValue;
                }
                else
                {
                    Priority = "0";
                }

                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                Previligername = prname.Text.Trim().ToUpper();
                Previligerid = prid.Text;
                ProcedureName = "ModifyPreviligers";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@PreviligerName", Previligername);
                HtSPParameters.Add("@PreviligerId", Previligerid);
                HtSPParameters.Add("@priority", Priority);
                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Previliger Name  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Previliger Name Not  Updated.Because  The Name Already Exist. NOTE:Previliger Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [14-10-2013]
                gvpreviligers.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [14-10-2013]

            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }


        }

        protected void gvpreviligers_RowEditing(object sender, GridViewEditEventArgs e)
        {

            gvpreviligers.EditIndex = e.NewEditIndex;
            Displaydata();
        }

        protected void gvpreviligers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvpreviligers.EditIndex = -1;
            Displaydata();
        }

        protected void gvpreviligers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvpreviligers.PageIndex = e.NewPageIndex;
            Displaydata();
        }

        protected void lbtn_Select_Click(object sender, EventArgs e)
        {
            try
            {

                LinkButton thisTextBox = (LinkButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblpreviligername");
                Label lblpid = (Label)thisGridViewRow.FindControl("lblpreviligerid");
                Session["Pid"] = lblpid.Text;
                Response.Redirect("AccessPrevilegers.aspx?Pid=" + lblid.Text, false);

            }
            catch (Exception ex)
            {
            }
        }
    }
}