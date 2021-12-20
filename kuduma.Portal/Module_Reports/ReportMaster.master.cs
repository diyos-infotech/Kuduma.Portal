using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class ReportMaster : System.Web.UI.MasterPage
    {
        MenuBAL BalObj = new MenuBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Master != null)
                {
                    HtmlGenericControl li1 = (HtmlGenericControl)this.Master.FindControl("li5");
                    HtmlGenericControl li2 = (HtmlGenericControl)this.Master.FindControl("li6");
                    HtmlAnchor link = (HtmlAnchor)this.Master.FindControl("li1").FindControl("ReportsLink");
                    if (link != null)
                    {

                        link.Attributes["class"] = "current";
                        li2.Attributes["class"] = "after";
                    }
                }
            }
        }
    }
}