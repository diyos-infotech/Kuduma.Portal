using System;
using System.Web.UI.HtmlControls;

namespace Kuduma.Portal
{
    public partial class Receipts : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null && Session["AccessLevel"] != null)
            {

            }
            else
            {
                Response.Redirect("login.aspx");
            }

            if (this.Master != null)
            {
                HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("c5");
                if (emplink != null)
                {
                    emplink.Attributes["class"] = "current";
                }
            }
        }

    }
}