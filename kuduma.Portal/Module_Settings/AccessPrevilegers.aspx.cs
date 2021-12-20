using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Setting
{
    public partial class AccessPrevilegers : System.Web.UI.Page
    {
        MenuBAL BalObj = new MenuBAL();
        static DataSet ds_chk;
        static string pid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "SET ACCESS TO PREVILIGERS";
                if (Session["UserId"] == null && Session["AccessLevel"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                if (Request.QueryString["Pid"] != null)
                {
                    pid = Request.QueryString["Pid"].ToString();
                    HfPid.Value = Session["Pid"].ToString();
                    heading1.Text = "SET ACCESS FOR " + pid;
                    Displaydata(HfPid.Value);
                }

            }
        }

        protected void Displaydata(string pid)
        {
            gvParentMenu.DataSource = null;
            gvParentMenu.DataBind();
            ds_chk = BalObj.GetAllPrevileges(pid);
            if (ds_chk.Tables[0].Rows.Count > 0)
            {
                DataView dv = ds_chk.Tables[0].DefaultView;
                dv.RowFilter = "PARENT_ID like 'PARENT'";

                gvParentMenu.DataSource = dv;
                gvParentMenu.DataBind();
            }
        }

        protected void imgChildMenu_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                (row.FindControl("chkAll") as CheckBox).Checked = true;
                row.FindControl("pnlOrders").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/images/minus.png";
                string menuid = gvParentMenu.DataKeys[row.RowIndex].Value.ToString();
                GridView gvChild = row.FindControl("gvChild") as GridView;
                BindChild(menuid, gvChild);
            }
            else
            {
                row.FindControl("pnlOrders").Visible = false;
                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "~/images/plus.png";
            }
        }

        private void BindChild(string menuid, GridView gvChild)
        {
            gvChild.ToolTip = menuid;
            //  Response.Write(menuid);
            DataView dv = ds_chk.Tables[0].DefaultView;
            dv.RowFilter = "PARENT_ID like '" + menuid + "'";
            gvChild.DataSource = dv;
            gvChild.DataBind();
        }




        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAll = (sender as CheckBox);
            GridViewRow row = chkAll.NamingContainer as GridViewRow;
            ImageButton imgShowHide = row.FindControl("imgOrdersShow") as ImageButton;
            GridView gv = row.FindControl("gvChild") as GridView;
            if (chkAll.Checked == true)
            {
                row.FindControl("pnlOrders").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/images/minus.png";
                string menuid = gvParentMenu.DataKeys[row.RowIndex].Value.ToString();
                GridView gvChild = row.FindControl("gvChild") as GridView;

                BindChild(menuid, gvChild);
                //UpdatePanel2.Update();
            }
            foreach (GridViewRow gvRow in gv.Rows)
            {
                (gvRow.FindControl("chkCheck") as CheckBox).Checked = chkAll.Checked;
                gvRow.FindControl("pnlsublink").Visible = true;
                string mid = gv.DataKeys[gvRow.RowIndex].Value.ToString();
                ImageButton imgShowHide1 = gvRow.FindControl("ImgsubLink") as ImageButton;
                GridView gvSChild = gvRow.FindControl("gvSubChild") as GridView;
                imgShowHide1.CommandArgument = "Hide";
                imgShowHide1.ImageUrl = "~/images/minus.png";
                Response.Write(mid);
                BindChild(mid, gvSChild);
                foreach (GridViewRow gvSRow in gvSChild.Rows)
                {
                    (gvSRow.FindControl("chkSub") as CheckBox).Checked = chkAll.Checked;
                }
                // UpdatePanel2.Update();
            }
            // UpdatePanel2.Update();
        }

        protected void Orders_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkOrder = (sender as CheckBox);
            GridViewRow row = chkOrder.NamingContainer as GridViewRow;
            GridViewRow parentRow = (row.Parent.Parent.NamingContainer as GridViewRow);
            if (chkOrder.Checked)
            {
                (parentRow.FindControl("chkAll") as CheckBox).Checked = true;
            }
            else
            {
                GridView gv = (row.Parent).NamingContainer as GridView;
                int total = gv.Rows.Count;
                int sum = 0;
                foreach (GridViewRow gvRow in gv.Rows)
                {
                    if ((gvRow.FindControl("chkCheck") as CheckBox).Checked == false)
                    {
                        sum++;
                    }
                }
                if (total == sum)
                {
                    (parentRow.FindControl("chkAll") as CheckBox).Checked = false;
                }
                // gv.AllowPaging = true;
            }

        }


        protected void Update_Click(object sender, EventArgs e)
        {
            pid = HfPid.Value;
            foreach (GridViewRow row in gvParentMenu.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkP = (CheckBox)row.FindControl("chkAll");
                    string menuid = gvParentMenu.DataKeys[row.RowIndex].Value.ToString();
                    if (chkP.Checked == true)
                    {
                        int p = BalObj.UpdateMenuPrevilege(pid, menuid, 1);
                        GridView gvChild = (GridView)row.FindControl("gvChild");
                        if (gvChild != null)
                        {
                            foreach (GridViewRow nrow in gvChild.Rows)
                            {

                                CheckBox chk = (CheckBox)nrow.FindControl("chkCheck");
                                string smenuid = gvChild.DataKeys[nrow.RowIndex].Value.ToString();
                                if (chk.Checked)
                                {
                                    int c = BalObj.UpdateMenuPrevilege(pid, smenuid, 1);
                                    GridView gvSChild = (GridView)nrow.FindControl("gvSubChild");
                                    if (gvSChild != null)
                                    {
                                        foreach (GridViewRow nsrow in gvSChild.Rows)
                                        {

                                            CheckBox schk = (CheckBox)nsrow.FindControl("chkSub");
                                            string Nsmenuid = gvSChild.DataKeys[nsrow.RowIndex].Value.ToString();
                                            //Response.Write(Nsmenuid);
                                            if (schk.Checked)
                                            {
                                                int s = BalObj.UpdateMenuPrevilege(pid, Nsmenuid, 1);
                                            }
                                            else
                                            {
                                                int s = BalObj.UpdateMenuPrevilege(pid, Nsmenuid, 0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int c = BalObj.UpdateMenuPrevilege(pid, smenuid, 0);
                                }

                            }
                        }
                    }
                    else
                    {
                        int p = BalObj.UpdateMenuPrevilege(pid, menuid, 0);
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Updated SucessFully.');", true);

        }

        protected void ImgsubLink_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                row.FindControl("pnlsublink").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/images/minus.png";
                string menuid = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();
                // Response.Write(menuid);
                GridView gvSubChild = row.FindControl("gvSubChild") as GridView;
                BindChild(menuid, gvSubChild);
            }
            else
            {
                row.FindControl("pnlsublink").Visible = false;
                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "~/images/plus.png";
            }
        }

        protected void chkCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (sender as CheckBox);
            GridViewRow row = chk.NamingContainer as GridViewRow;
            ImageButton imgShowHide = row.FindControl("ImgsubLink") as ImageButton;
            GridView gv = row.FindControl("gvSubChild") as GridView;
            if (chk.Checked == true)
            {
                row.FindControl("pnlsublink").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/images/minus.png";
                string menuid = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();

                BindChild(menuid, gv);
            }
            foreach (GridViewRow gvRow in gv.Rows)
            {
                (gvRow.FindControl("chkSub") as CheckBox).Checked = chk.Checked;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Previligers.aspx");
        }
    }
}