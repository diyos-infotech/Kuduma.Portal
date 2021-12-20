using System;
using System.Web.UI.WebControls;
using System.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class MeasuredUnits : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                }
                else
                {
                    Response.Redirect("login.aspx");
                }

                displaydata();


            }
        }

        DataTable dt;
        private void displaydata()
        {
            string selectquery = " select * from units ";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
            gvmeasuredunits.DataSource = dt;
            gvmeasuredunits.DataBind();
        }

        protected void btndesgn_Click(object sender, EventArgs e)
        {

        }


        public string TitleCase(string value)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);

        }

        protected void btnunitname_Click(object sender, EventArgs e)
        {
            try
            {
                string unitname = TitleCase(txtunitname.Text.Trim());
                int status = 0;

                displaydata();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (unitname.Equals(dt.Rows[i]["unitmeasure"].ToString()))
                    {
                        status = 1;

                        break;
                    }
                    else
                    {
                        status = 0;
                    }
                }
                if (status == 1)
                {
                    lblresult.Visible = true;
                    lblresult.Text = "Unit Name already Exist";
                }
                else
                {

                    // segmentid();
                    if (unitname.Trim().Length != 0)
                    {
                        string insertquery = "insert units(unitmeasure) values('" + unitname.Trim() + "')";
                      int resyults= config.ExecuteNonQueryWithQueryAsync(insertquery).Result;

                        lblresult.Visible = true;
                        lblresult.Text = "Resource Added Successfully ";
                    }
                    // Respfonse.End();
                }
                displaydata();

            }

            catch (Exception ex)
            {
                lblresult.Visible = true;
                lblresult.Text = ex.Message;

            }
        }

        protected void gvmeasuredunits_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {

            lblresult.Text = "";
            Label ResourceName = gvmeasuredunits.Rows[e.RowIndex].FindControl("lblUnitName") as Label;
            string deletequery = "delete from units where unitmeasure='" + ResourceName.Text + "'";
          int delllll=config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            lblresult.Visible = true;
            lblresult.Text = "Row Deleted Successfully";

            displaydata();
        }

        protected void gvmeasuredunits_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {

            Label unitid = gvmeasuredunits.Rows[e.RowIndex].FindControl("lblunitid") as Label;
            TextBox unitname = gvmeasuredunits.Rows[e.RowIndex].FindControl("txtunitname") as TextBox;

            string updatequery = "update units  set unitmeasure = '" + TitleCase(unitname.Text) + "' where unitid =" + unitid.Text;
            int status = config.ExecuteNonQueryWithQueryAsync(updatequery).Result;

            gvmeasuredunits.EditIndex = -1;
            displaydata();
            if (status != 0)
            {
                lblresult.Text = "Record Updated Successfully";
            }
            else
            {
                lblresult.Text = "Record not updated";
            }
        }

        protected void gvmeasuredunits_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            gvmeasuredunits.EditIndex = e.NewEditIndex;
            displaydata();

        }

        protected void gvmeasuredunits_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvmeasuredunits.EditIndex = -1;
            displaydata();
        }

    }
}