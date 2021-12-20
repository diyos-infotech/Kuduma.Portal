using System;
using Kuduma.Portal.DAL;
using System.Web.Routing;
namespace Kuduma.Portal
{
    public class Global : System.Web.HttpApplication
    {

        MenuBAL BalObj = new MenuBAL();

        protected void Application_Start(object sender, EventArgs e)
        {
            //RegisterRoutes(RouteTable.Routes);
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Routelogin", "Login", "~/Login.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["SessionId"] = Session.SessionID.ToString();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Session["SessionId"] != null && Session["UserId"] != null)
            {
                int chksession = BalObj.ChecKSession(Session["uname"].ToString(), Session["SessionId"].ToString(), "D");
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}