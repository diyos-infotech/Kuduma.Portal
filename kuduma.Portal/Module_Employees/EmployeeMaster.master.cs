using System;
using Kuduma.Portal.DAL;
using System.Web.UI.HtmlControls;

namespace Kuduma.Portal.Module_Employees
{
    public partial class EmployeeMaster : System.Web.UI.MasterPage
    {
        MenuBAL BalObj = new MenuBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Master != null)
                {
                    HtmlGenericControl li1 = (HtmlGenericControl)this.Master.FindControl("li1");
                    HtmlGenericControl li2 = (HtmlGenericControl)this.Master.FindControl("li2");

                    HtmlAnchor emplink = (HtmlAnchor)this.Master.FindControl("li1").FindControl("EmployeesLink");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                        li2.Attributes["class"] = "after";
                    }
                }
            }
        }
    }
}