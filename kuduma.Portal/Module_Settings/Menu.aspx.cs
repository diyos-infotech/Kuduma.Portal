using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Settings
{
    public partial class Menu : System.Web.UI.Page
    {
        MenuBAL BalObj = new MenuBAL();
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        private void Clear()
        {
            txtMId.Text = "";
            txtPage.Text = "";
            txtPatn.Text = "";
            txtDec.Text = "";
        }

        private void Displaydata()
        {
            Clear();
            string sqlselect = "select distinct * from Menu order by 2,1 desc";
            DataTable dtMennu = config.ExecuteAdaptorAsyncWithQueryParams(sqlselect).Result;

            if (dtMennu.Rows.Count > 0)
            {
                gvSegment.DataSource = dtMennu;
                gvSegment.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Segment Names Are Not Avialable');", true);
                return;
            }
        }

        private void BindMenuddl()
        {

            string sqlselect = "select distinct MENU_ID from Menu order by 1";
            DataTable dtPriority = config.ExecuteAdaptorAsyncWithQueryParams(sqlselect).Result;


            ddlmenu.DataValueField = "MENU_ID";
            ddlmenu.DataTextField = "MENU_ID";
            ddlmenu.DataSource = dtPriority;
            ddlmenu.DataBind();
            ddlmenu.Items.Insert(0, "select");
            ddlmenu.Items.Insert(1, "PARENT");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["UserId"] = "aa";
                Page.Title = "SETTINGS: Menu";
                if (Session["UserId"] == null && Session["AccessLevel"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                BindMenuddl();
                Displaydata();
            }
        }


        protected void gvSegment_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {

            //try
            //{

            //    #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
            //    Label segid = gvSegment.Rows[e.RowIndex].FindControl("lblSegid") as Label;
            //    TextBox segname = gvSegment.Rows[e.RowIndex].FindControl("txtSegName") as TextBox;
            //    #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


            //    #region  Begin  Code  for  validaton as on [14-10-2013]
            //    if (segname.Text.Trim().Length == 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Segment  Name');", true);
            //        return;
            //    }
            //    #endregion End  Code  for  validaton as on [14-10-2013]


            //    #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
            //    var Segmentname = string.Empty;
            //    var Segmentid = string.Empty;
            //    var ProcedureName = string.Empty;
            //    var IRecordStatus = 0;
            //    Hashtable HtSPParameters = new Hashtable();
            //    #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

            //    #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
            //    Segmentname = segname.Text.Trim().ToUpper();
            //    Segmentid = segid.Text;
            //    ProcedureName = "ModifySegments";
            //    #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

            //    #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
            //    HtSPParameters.Add("@SegName", Segmentname);
            //    HtSPParameters.Add("@SegId", Segmentid);
            //    #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

            //    #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
            //    IRecordStatus = SqlHelper.Instance.ExecuteQuery(ProcedureName, HtSPParameters);
            //    #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

            //    #region  Begin Code For Display Status Of the Record as on [14-10-2013]
            //    if (IRecordStatus > 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Segment Name  Updated  SucessFully.');", true);
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Segment Name Not  Updated.Because  The Name Already Exist. NOTE:Department Names Are UNIQUE');", true);
            //    }
            //    #endregion  End Code For Display Status Of the Record as on [14-10-2013]

            //    #region  Begin Code For Re-Call All the Departments As on [14-10-2013]
            //    gvSegment.EditIndex = -1;
            //    Displaydata();
            //    #endregion End Code For Re-Call All the Departments As on [14-10-2013]

            //}

            //catch (Exception ex)
            //{

            //    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
            //    return;
            //}


        }

        protected void gvSegment_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            //gvSegment.EditIndex = e.NewEditIndex;
            //Displaydata();
        }

        protected void gvSegment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //gvSegment.EditIndex = -1;
            //Displaydata();
        }

        protected void gvSegment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSegment.PageIndex = e.NewPageIndex;
            Displaydata();
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            //try
            //{
            #region Begin Code For  Validations   as [12-10-2013]
            if (txtMId.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Munu Link Id.');", true);
                return;
            }
            if (txtPatn.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Path.');", true);
                return;
            }
            if (txtPage.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Redirect Page.');", true);
                return;
            }
            if (txtDec.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Menu Description.');", true);
                return;
            }
            string pmenu = ddlmenu.SelectedItem.Text;
            if (pmenu == "Select")
            {
                pmenu = "";
            }
            #endregion Begin Code For  Validations as [12-10-2013]
            int c = BalObj.AddMenu(txtMId.Text, pmenu, txtPage.Text, Session["UserId"].ToString(), txtPatn.Text, txtDec.Text.Trim());
            if (c > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Menu Added Successfully..');", true);
                Clear();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Error in Adding..');", true);
                return;
            }
           
            Displaydata();
        }
    }
}