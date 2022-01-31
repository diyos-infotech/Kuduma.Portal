using System;
using Kuduma.Portal.DAL;
using System.Web.Routing;
using System.Text;
using System.IO;

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
            Exception exception = Server.GetLastError();
            if (exception != null)
            {



                FileStream fileStream = null;
                StreamWriter streamWriter = null;
                try
                {
                    var logFilePath = Server.MapPath("~/ErrorLog/");

                    logFilePath = logFilePath + "ErrorLog.txt";

                    if (logFilePath.Equals("")) return;

                    #region Create the Log file directory if it does not exists 
                    DirectoryInfo logDirInfo = null;
                    FileInfo logFileInfo = new FileInfo(logFilePath);
                    logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                    if (!logDirInfo.Exists) logDirInfo.Create();
                    #endregion Create the Log file directory if it does not exists 

                    if (!logFileInfo.Exists)
                    {
                        fileStream = logFileInfo.Create();
                    }
                    else
                    {
                        fileStream = new FileStream(logFilePath, FileMode.Append);
                    }
                    streamWriter = new StreamWriter(fileStream);


                    string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    message += Environment.NewLine;
                    message += "-----------------------------------------------------------";
                    message += Environment.NewLine;
                    message += string.Format("Message: {0}", exception.InnerException.Message);
                    message += Environment.NewLine;
                    message += string.Format("StackTrace: {0}", exception.StackTrace);
                    message += Environment.NewLine;
                    message += string.Format("Source: {0}", exception.Source);
                    message += Environment.NewLine;
                    message += string.Format("TargetSite: {0}", exception.TargetSite.ToString());
                    message += Environment.NewLine;
                    message += "-----------------------------------------------------------";
                    message += Environment.NewLine;

                    streamWriter.WriteLine(message);

                    string Body = exception.InnerException.Message;
                    string Line1 = "Please contact admin.. To continue refresh / reload the page.";

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<head>");
                    sb.Append("<link rel = 'stylesheet' href = 'https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css' />");
                    sb.Append("<script src = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.1.1/js/bootstrap.min.js'></script>");
                    sb.Append("<script src = 'https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script>");
                    sb.Append("</head>");
                    sb.Append("<div class='container' style='position: absolute;top:10%;left:15% '>");
                    sb.Append("<div class='row justify-content-center'>");
                    sb.Append("<div class='col-md-8'>");
                    sb.Append("<div class='card'>");
                    sb.Append("<div class='card-header'>");
                    sb.Append("<span>Exception in Page</span>");
                    sb.Append("</div>");
                    sb.Append("<div class='card-body'>");
                    sb.Append("<span style='font-size: 14pt;font-family:calibri;color:red'>" + Body + "</span>");
                    sb.Append("<span style='font-size: 14pt;font-family:Calibri;color:red'>" + Line1 + "</span>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    Response.Write(sb.ToString());
                    Server.ClearError();
                }
                finally
                {
                    if (streamWriter != null) streamWriter.Close();
                    if (fileStream != null) fileStream.Close();
                }


            }
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