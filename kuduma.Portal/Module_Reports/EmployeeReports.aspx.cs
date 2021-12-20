using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class EmployeeReports : System.Web.UI.Page
    {
        MenuBAL BalObj = new MenuBAL();
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        static DataTable dtrep;
        static string[] brdcrumb = new string[10];
        static string[] brdid = new string[10];
        static int bi = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Master != null)
                {
                    HtmlControl mid = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli1");
                    mid.Attributes["class"] = "current";
                }
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {

                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }

                bi = 0;
                //if(HfBrd.Value.Length > 0)
                //{
                //    int id = Convert.ToInt32(HfBrd.Value);
                //    Response.Write(id);
                //    FillData(id);
                //}
                //else
                //{
                //    LoadIcons();
                //}

                LoadIcons();
                //PreviligeUsers(Convert.ToInt32(Session["AccessLevel"]));
            }
        }

        protected void LoadIcons()
        {
            //string query = "SELECT     FOLDER_MENU.PARENT_PAGE, FOLDER_MENU.FOLDER_ID, FOLDER_MENU.FOLDER_NAME, FOLDER_MENU.PARENT_FOLDER_ID, " +
            //                " FOLDER_MENU.URL, MENU.MENU_DESC, MENU.PATH,MENU.MENU_ID " +
            //                " FROM FOLDER_MENU LEFT OUTER JOIN " +
            //                " MENU ON FOLDER_MENU.URL = MENU.REDIRECT_PAGE WHERE PARENT_PAGE = 'EmployeeReportLink'";
            // you have same id???? yes ! can u please go to db and execute sp using same parameter
            //dtrep = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            DataSet ds = BalObj.GetAllFolderMenus("EmployeeReportLink", Convert.ToInt32(Session["AccessLevel"]));
            dtrep = ds.Tables[0];
            if (dtrep.Select("URL = ' ' and PARENT_FOLDER_ID=0").Length > 0)
            {
                DataTable dt = dtrep.Select("URL = ' ' and PARENT_FOLDER_ID=0").CopyToDataTable();
                dllist.DataSource = dt;
                dllist.DataBind();
            }
            if (dtrep.Select("LEN(URL) > 1 and PARENT_FOLDER_ID=0").Length > 0)
            {
                DataTable dt = dtrep.Select("LEN(URL) > 1 and PARENT_FOLDER_ID=0").CopyToDataTable();
                DlLiList.DataSource = dt;
                DlLiList.DataBind();
            }
            // HfBrd.Value = "";
            // PreviligeUsers(Convert.ToInt32(Session["AccessLevel"]));
        }

        protected void PreviligeUsers(int previligerid)
        {
            DataSet ds_chk = BalObj.CheckPrevileges(previligerid);
            if (ds_chk.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_chk.Tables[0].Rows)
                {

                    HtmlControl mid = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("ContentPlaceHolder3").FindControl(row["MENU_ID"].ToString());
                    if (mid != null)
                    {
                        mid.Visible = false;
                    }

                }
            }

        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            LinkButton bobj = (LinkButton)sender;
            string ID = bobj.CommandArgument.ToString();
            string fname = bobj.CommandName.ToString();
            int fid = Convert.ToInt32(ID);
            //    AddBrdCrumbs(fname, fid);
            FillData(fid);

        }

        //please run code and login as wh

        public void FillData(int fid)
        {
            //  Response.Write(fid);
            if (dtrep.Select("URL=' ' and PARENT_FOLDER_ID=" + fid + "").Length > 0)
            {
                DataTable dt = dtrep.Select("URL=' ' and PARENT_FOLDER_ID=" + fid + "").CopyToDataTable();
                dllist.DataSource = dt;
                dllist.DataBind();

            }
            else
            {
                dllist.DataSource = null;
                dllist.DataBind();
            }
            if (dtrep.Select("ISNULL(FOLDER_ID,0)=0 and PARENT_FOLDER_ID=" + fid + "").Length > 0)
            {
                DataTable dt = dtrep.Select("ISNULL(FOLDER_ID, 0) = 0 and PARENT_FOLDER_ID=" + fid + "").CopyToDataTable();
                DlLiList.DataSource = dt;
                DlLiList.DataBind();
                // PreviligeUsers(Convert.ToInt32(Session["AccessLevel"]));
            }
            else
            {
                DlLiList.DataSource = null;
                DlLiList.DataBind();
            }
        }


        protected void DlLiList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    Label lbl = e.Item.FindControl("bool") as Label;
            //    if (lbl.Text == "false")
            //    {
            //        HtmlControl lid = (HtmlControl)e.Item.FindControl("link");
            //        lid.Visible = false;
            //    }

            //}
        }

    }
}