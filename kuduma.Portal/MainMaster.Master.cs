using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class MainMaster : System.Web.UI.MasterPage
    {
        MenuBAL BalObj = new MenuBAL();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["SessionId"] != null && Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                    int chksession = BalObj.ChecKSession(Session["uname"].ToString(), Session["SessionId"].ToString(), "C");

                    if (chksession == 1)
                    {

                        lblDisplayUser.Text = Session["UserId"].ToString();
                        lblcname.Text = SqlHelper.Instance.GetCompanyname();


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Another Session has Logged In..');", true);
                        Response.Redirect("~/Login.aspx");

                    }
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Session Timed Out..');", true);

                }
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            PreviligeUsers(Convert.ToInt32(Session["AccessLevel"]));
        }

        protected void PreviligeUsers(int previligerid)
        {
            DataSet ds_chk = BalObj.CheckPrevileges(previligerid);
            if (ds_chk.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_chk.Tables[0].Rows)
                {
                    HtmlControl mid = (HtmlControl)this.FindControl(row["MENU_ID"].ToString());
                    if (mid != null)
                    {
                        mid.Visible = false;

                    }

                    HtmlControl childid = (HtmlControl)ContentPlaceHolder1.FindControl(row["MENU_ID"].ToString());
                    if (childid != null)
                    {
                        childid.Visible = false;
                    }


                    string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                    DataRow[] drArr = ds_chk.Tables[0].Select("REDIRECT_PAGE='" + currentPage + "'");
                    if (drArr.Length > 0)
                    {
                        Response.Redirect("~/NoAccess.aspx");
                    }
                }

            }
        }

        protected void LogOutLink_ServerClick(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}