using System;
using System.Web.UI.HtmlControls;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Clients
{
    public partial class Clients : System.Web.UI.MasterPage
    {
        MenuBAL BalObj = new MenuBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Master != null)
                {
                    HtmlGenericControl li2 = (HtmlGenericControl)this.Master.FindControl("li2");
                    HtmlGenericControl li3 = (HtmlGenericControl)this.Master.FindControl("li3");
                    HtmlAnchor emplink = (HtmlAnchor)this.Master.FindControl("li2").FindControl("ClientsLink");
                    if (emplink != null)
                    {

                        emplink.Attributes["class"] = "current";
                        li3.Attributes["class"] = "after";
                    }
                }
            }
        }
    }
}