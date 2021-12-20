using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class Clients : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string CmpIDPrefix = "";
        string BranchID = "";
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
                        Response.Redirect("login.aspx");
                    }
                    DisplayData();
                }
            }
            catch (Exception ex)
            {

            }
        }


        public void DisplayData()
        {
            gvclient.DataSource = null;
            gvclient.DataBind();
            var SPName = "";
            var Condition = 0;

            DataTable DtListOfClients = null;
            Hashtable HtListOfClients = new Hashtable();

            SPName = "ClientDetailsSearchBase";

            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            HtListOfClients.Add("@clientidprefix", CmpIDPrefix);
            HtListOfClients.Add("@Branch", dtBranch);



            DtListOfClients = config.ExecuteAdaptorAsyncWithParams(SPName, HtListOfClients).Result;
            if (DtListOfClients.Rows.Count > 0)
            {
                gvclient.DataSource = DtListOfClients;
                gvclient.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Client Details Are Not Avaialable');", true);
            }

        }


        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            #region Begin Code For Check Validation/Become Zero Data  as on  [20-09-2013]
            gvclient.DataSource = null;
            gvclient.DataBind();

            if (txtsearch.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Enter Either Client ID Nor Client Name');", true);
                return;
            }

            #endregion End Code For Check Validation/Become Zero Data  as on  [20-09-2013]

            #region Begin Code For Declaration/Assign Values as on [20-09-2013]
            Hashtable HTSpParameters = new Hashtable();
            var SPName = "SearchIndClientIfo";
            var SearchedValue = txtsearch.Text;
            HTSpParameters.Add("@ClientidorName", SearchedValue);
            HTSpParameters.Add("@Branch", dtBranch);
            #endregion End  Code For Declaration/Assign Values as on [20-09-2013]

            #region Begin code For Calling Stored Procedure And Retrived Data/Bind To The Gridview  What user Searched    as on [20-09-2013]
            DataTable DtIndClientInfo = config.ExecuteAdaptorAsyncWithParams(SPName, HTSpParameters).Result;
            if (DtIndClientInfo.Rows.Count > 0)
            {
                gvclient.DataSource = DtIndClientInfo;
                gvclient.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Search not Found');", true);
            }
            #endregion  End   code For Calling Stored Procedure And Retrived Data/Bind To The Gridview  What user Searched    as on [20-09-2013]
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblresult.Visible = true;
            Label ClientId = gvclient.Rows[e.RowIndex].FindControl("lblclientid") as Label;
            string deletequery = "delete from clients where ClientId ='" + ClientId.Text.Trim() + "'";
            int status = config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            if (status != 0)
            {
                lblresult.Text = "Client Deleted Successfully";
            }
            else
            {
                lblresult.Text = "Client Not Deleted ";
            }

            DisplayData();
        }

        protected void lbtn_Select_Click(object sender, EventArgs e)
        {
            try
            {

                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblclientid");
                Response.Redirect("ViewClient.aspx?Clientid=" + lblid.Text, false);

            }
            catch (Exception ex)
            {
            }

        }

        protected void lbtn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblclientid");
                Response.Redirect("ModifyClient.aspx?Clientid=" + lblid.Text, false);
            }
            catch (Exception ex)
            {

            }

        }

        protected void gvclient_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvclient.PageIndex = e.NewPageIndex;
            DisplayData();
        }

        protected void lbtn_clntman_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton thisTextBox = (ImageButton)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                Label lblid = (Label)thisGridViewRow.FindControl("lblclientid");
                Response.Redirect("ClientManPowerReq.aspx?Clientid=" + lblid.Text, false);
            }
            catch (Exception ex)
            {

            }
        }

    }

}
