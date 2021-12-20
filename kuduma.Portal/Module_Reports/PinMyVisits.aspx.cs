using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;

namespace Kuduma.Portal.Module_Reports
{
    public partial class PinMyVisits : System.Web.UI.Page
    {
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();
        DataTable dt;
        string EmpIDPrefix = "";
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



                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }





        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtMonth.Text.Trim().Length == 0)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);
                return;
            }

            if (txtEmpIDName.Text.Trim().Length == 0)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Emp ID');", true);
                return;
            }

            string date = string.Empty;

            if (txtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
            }

            string Day = DateTime.Parse(date).Day.ToString();
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Empid = "";

            if (txtEmpIDName.Text.Length > 9)
            {
                Empid = txtEmpIDName.Text.Substring(0, 9);
            }


            string Spname = "GetPinMyvisitImages";
            Hashtable ht = new Hashtable();
            ht.Add("@Day", Day);
            ht.Add("@month", month);
            ht.Add("@Year", Year);
            ht.Add("@CompanyID", "36");
            ht.Add("@Empid", Empid);
            ht.Add("@Type", "GetData");


            DataTable dt = config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;
            if (dt.Rows.Count > 0)
            {
                GVpinmyvisit.DataSource = dt;
                GVpinmyvisit.DataBind();



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Button btnview = GVpinmyvisit.Rows[i].FindControl("btnview") as Button;

                    if (dt.Rows[i]["pitstopImage"].ToString() != "0")
                    {
                        btnview.Visible = true;
                    }
                    else
                    {
                        btnview.Visible = false;

                    }

                }



            }
            else
            {
                GVpinmyvisit.DataSource = null;
                GVpinmyvisit.DataBind();
            }
        }

        public string Getmonthval()
        {
            string date = string.Empty;
            string month = "";
            string Year = "";
            string monthval = "";


            if (txtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString();
                monthval = month + Year.Substring(2, 2);
            }

            return monthval;

        }


        protected void gvdata_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //add the thead and tbody section programatically
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void btnGetImage_Click(object sender, EventArgs e)
        {
            if (txtMonth.Text.Trim().Length == 0)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);
                return;
            }

            string date = string.Empty;

            if (txtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
            }

            string Day = DateTime.Parse(date).Day.ToString();
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();


            string Empid = "";

            if(txtEmpIDName.Text.Length>9)
            {
                Empid = txtEmpIDName.Text.Substring(0, 9);
            }
            

            string Spname = "GetPinMyvisitImages";

            Hashtable ht = new Hashtable();
            ht.Add("@Day", Day);
            ht.Add("@month", month);
            ht.Add("@Year", Year);
            ht.Add("@CompanyID", "36");
            ht.Add("@Empid", Empid);
            ht.Add("@PitstopAttachmentId", hfPitstopAttachmentId.Value);
            ht.Add("@Type", "GetImageData");

            DataTable dt = config.ExecuteAdaptorAsyncWithParams(Spname,ht).Result;

            if(dt.Rows.Count>0)
            {
                string imageUrl =  dt.Rows[0]["pitstopImage"].ToString();

                if (dt.Rows[0]["pitstopImage"].ToString().Length > 0)
                {
                    imgphoto.ImageUrl = imageUrl;
                }
               

                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
            }



        }
    }
}


