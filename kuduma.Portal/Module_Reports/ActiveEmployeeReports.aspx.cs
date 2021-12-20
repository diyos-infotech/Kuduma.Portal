using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;
using Kuduma.Portal.DAL;


namespace Kuduma.Portal
{
    public partial class ActiveEmployeeReports : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string CmpIDPrefix = "";
        string EmpIDPrefix = "";
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
                    if (this.Master != null)
                    {
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli1");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }
                    LoadClientList();

                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired . Please Login');", true);
                Response.Redirect("~/Login.aspx");
            }

        }
        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void ClearData()
        {
            gvlistofemp.DataSource = null;
            gvlistofemp.DataBind();
        }

        protected void ddlActiveEmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cleardata();
            string SqlQueryForEmp = "";

            if (ddlActiveEmp.SelectedValue == "EmpId")
            {

                panelempid.Visible = true;

            }
            else
            {
                panelempid.Visible = false;
            }

            if (ddlActiveEmp.SelectedValue == "EmpName")
            {

                panelemp.Visible = true;

            }
            else
            {
                panelemp.Visible = false;
            }

            if (ddlActiveEmp.SelectedValue == "Designation")
            {

                paneldesignation.Visible = true;
                FillDesgn();

            }
            else
            {
                paneldesignation.Visible = false;
            }

            if (ddlActiveEmp.SelectedValue == "JoiningDate")
            {

                panelJdate.Visible = true;


            }
            else
            {
                panelJdate.Visible = false;
            }


            if (ddlActiveEmp.SelectedValue == "LeavingDate")
            {

                panelLdate.Visible = true;


            }
            else
            {
                panelLdate.Visible = false;
            }
            if (ddlActiveEmp.SelectedValue == "NonAttendance")
            {

                panelNonAtten.Visible = true;


            }
            else
            {
                panelNonAtten.Visible = false;
            }

            if (ddlActiveEmp.SelectedIndex == 17)
            {

                ddlclientid.Visible = true;
                lblclientid.Visible = true;

            }
            else
            {
                ddlclientid.Visible = false;
                lblclientid.Visible = false;

            }
        }

        protected void Cleardata()
        {
            gvlistofemp.DataSource = null;
            gvlistofemp.DataBind();
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        protected void LoadClientList()
        {

            string qry = "select Clientid,(Clientid+' - '+Clientname) as Clientname from clients where clientid like '%" + CmpIDPrefix + "%'";
            DataTable DtClientNames = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientname";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
            ddlclientid.Items.Insert(1, "All");

        }

        protected void FillDesgn()
        {
            //string SqlQryDesgn = "Select Design from Designations";
            DataTable dtDesgn = GlobalData.Instance.LoadDesigns();
            if (dtDesgn.Rows.Count > 0)
            {
                ddldesgn.DataValueField = "Designid";
                ddldesgn.DataTextField = "Design";
                ddldesgn.DataSource = dtDesgn;
                ddldesgn.DataBind();
            }
            ddldesgn.Items.Insert(0, "-- Select --");

        }

        protected void Esearch_Click(object sender, EventArgs e)
        {
            Cleardata();
            string SqlQueryEmp = "";

            int status = ddlActiveEmp.SelectedIndex;
            Hashtable HTEmpList = new Hashtable();
            string spname = "IMReportForListOfEmployees";
            HTEmpList.Add("@BranchID", BranchID);
            if (status == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('please  select search mode');", true);
                return;
            }

            if (status == 1)
            {
                HTEmpList.Add("@Status", 1);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }
            if (status == 2)
            {
                HTEmpList.Add("@Status", 2);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }
            if (status == 3)
            {
                HTEmpList.Add("@Status", 3);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }

            if (status == 4)
            {

                if (TextEmpid.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The employee Id');", true);
                    return;
                }
                HTEmpList.Add("@Empid", TextEmpid.Text.Trim());
                HTEmpList.Add("@Status", 4);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }
            if (status == 5)
            {

                if (TxtEmpname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The employee Name');", true);
                    return;
                }
                HTEmpList.Add("@EmpName", TxtEmpname.Text.Trim());
                HTEmpList.Add("@Status", 5);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }
            if (status == 6)
            {
                if (ddldesgn.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Select The Designation');", true);
                    return;
                }
                HTEmpList.Add("@Status", 6);
                HTEmpList.Add("@Designation", ddldesgn.SelectedValue);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }
            if (status == 7)
            {
                if (TxtJdateFrom.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The From Date');", true);
                    return;
                }


                if (TxtJdateTo.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The To Date');", true);
                    return;
                }
                var jdateFrom = DateTime.Parse(TxtJdateFrom.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                var jdateTo = DateTime.Parse(TxtJdateTo.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                HTEmpList.Add("@Status", 7);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                HTEmpList.Add("@JdateFrom", jdateFrom);
                HTEmpList.Add("@JdateTo", jdateTo);

            }

            if (status == 8)
            {

                if (TxtLdateFrom.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The From Date');", true);
                    return;
                }

                if (TxtLdateTo.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Please Fill The To Date');", true);
                    return;
                }
                var LdateFrom = DateTime.Parse(TxtLdateFrom.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                var LdateTo = DateTime.Parse(TxtLdateTo.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                HTEmpList.Add("@Status", 8);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                HTEmpList.Add("@dateFrom", LdateFrom);
                HTEmpList.Add("@dateTo", LdateTo);

            }
            #region  Begin Code developed by prasad [01-10-2013]

            if (status == 9)
            {
                HTEmpList.Add("@Status", 9);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }

            if (status == 10)
            {
                HTEmpList.Add("@Status", 10);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }
            if (status == 11)
            {
                HTEmpList.Add("@Status", 11);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);


            }
            if (status == 12)
            {
                HTEmpList.Add("@Status", 12);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }
            if (status == 13)
            {
                HTEmpList.Add("@Status", 13);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }

            if (status == 14)
            {
                HTEmpList.Add("@Status", 14);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }

            if (status == 15)
            {
                HTEmpList.Add("@Status", 15);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }

            if (status == 16)
            {
                HTEmpList.Add("@Status", 16);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }


            if (status == 17)
            {
                string clientid = ddlclientid.SelectedValue;
                if (ddlclientid.SelectedIndex == 1)
                {
                    clientid = "%";
                }

                HTEmpList.Add("@Status", 17);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                HTEmpList.Add("@unitid", clientid);

            }

            if (status == 18)
            {
                HTEmpList.Add("@Status", 18);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);               
            }

            if (status == 19)
            {
                HTEmpList.Add("@Status", 19);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

            }

            if (status == 20)
            {
                HTEmpList.Add("@Status", 20);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
            }
            if (status == 21)
            {
                HTEmpList.Add("@Status", 21);
                HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
            }
            #endregion   End  Code developed by prasad [01-10-2013]

            DataTable DtGetEmpList = config.ExecuteAdaptorAsyncWithParams(spname, HTEmpList).Result;

            if (DtGetEmpList.Rows.Count > 0)
            {
                lbtn_Export.Visible = true;
              
                if (ddlActiveEmp.SelectedIndex == 18 || ddlActiveEmp.SelectedIndex == 19 || ddlActiveEmp.SelectedIndex == 20|| ddlActiveEmp.SelectedIndex == 21)
                {
                    GridView1.DataSource = DtGetEmpList;
                    GridView1.DataBind();
                }
                else
                {
                    gvlistofemp.DataSource = DtGetEmpList;
                    gvlistofemp.DataBind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The details Are Not Available What you are searching...');", true);
            }

        }

        private void BindData(string SqlQuery)
        {

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuery).Result;
            if (dt.Rows.Count > 0)
            {
                gvlistofemp.DataSource = dt;
                gvlistofemp.DataBind();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The details Are Not Available What you are searching...');", true);
            }

        }

        //protected void lbtn_Export_Click(object sender, EventArgs e)
        //{
        //    gve.Export("EmployeeList.xls", this.gvlistofemp);
        //}

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            try
            {

                DataSet ds = getDataSetExportToExcel();
                ds.Tables[0].TableName = "Employee List Report";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        wb.Worksheets.Add(dt);
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= EmployeeList.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataSet getDataSetExportToExcel()
        {
            DataSet ds = new DataSet();

            {
                for (int k = 0; k < 1; k++)
                {


                    int status = ddlActiveEmp.SelectedIndex;
                    Hashtable HTEmpList = new Hashtable();
                    string spname = "IMReportForListOfEmployees";
                    DataTable dts = GlobalData.Instance.LoadBranchOnUserID(BranchID);
                    HTEmpList.Add("@BranchID", dts);
                    if (status == 1)
                    {
                        HTEmpList.Add("@Status", 1);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);


                    }
                    if (status == 2)
                    {
                        HTEmpList.Add("@Status", 2);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }
                    if (status == 3)
                    {
                        HTEmpList.Add("@Status", 3);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }

                    if (status == 4)
                    {

                        HTEmpList.Add("@Empid", TextEmpid.Text.Trim());
                        HTEmpList.Add("@Status", 4);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }
                    if (status == 5)
                    {

                        
                        HTEmpList.Add("@EmpName", TxtEmpname.Text.Trim());
                        HTEmpList.Add("@Status", 5);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }
                    if (status == 6)
                    {
                        
                        HTEmpList.Add("@Status", 6);
                        HTEmpList.Add("@Designation", ddldesgn.SelectedValue);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }
                    if (status == 7)
                    {
                       
                        var jdateFrom = DateTime.Parse(TxtJdateFrom.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                        var jdateTo = DateTime.Parse(TxtJdateTo.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                        HTEmpList.Add("@Status", 7);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                        HTEmpList.Add("@JdateFrom", jdateFrom);
                        HTEmpList.Add("@JdateTo", jdateTo);

                    }

                    if (status == 8)
                    {

                      
                        var LdateFrom = DateTime.Parse(TxtLdateFrom.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                        var LdateTo = DateTime.Parse(TxtLdateTo.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                        HTEmpList.Add("@Status", 8);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                        HTEmpList.Add("@dateFrom", LdateFrom);
                        HTEmpList.Add("@dateTo", LdateTo);

                    }
                    #region  Begin Code developed by prasad [01-10-2013]

                    if (status == 9)
                    {
                        HTEmpList.Add("@Status", 9);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }

                    if (status == 10)
                    {
                        HTEmpList.Add("@Status", 10);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }
                    if (status == 11)
                    {
                        HTEmpList.Add("@Status", 11);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);


                    }
                    if (status == 12)
                    {
                        HTEmpList.Add("@Status", 12);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }
                    if (status == 13)
                    {
                        HTEmpList.Add("@Status", 13);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }

                    if (status == 14)
                    {
                        HTEmpList.Add("@Status", 14);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }

                    if (status == 15)
                    {
                        HTEmpList.Add("@Status", 15);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }

                    if (status == 16)
                    {
                        HTEmpList.Add("@Status", 16);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }


                    if (status == 17)
                    {
                        string clientid = ddlclientid.SelectedValue;
                        if (ddlclientid.SelectedIndex == 1)
                        {
                            clientid = "%";
                        }

                        HTEmpList.Add("@Status", 17);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                        HTEmpList.Add("@unitid", clientid);

                    }

                    if (status == 18)
                    {
                        HTEmpList.Add("@Status", 18);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    }

                    if (status == 19)
                    {
                        HTEmpList.Add("@Status", 19);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);

                    }

                    if (status == 20)
                    {
                        HTEmpList.Add("@Status", 20);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    }
                    if (status == 21)
                    {
                        HTEmpList.Add("@Status", 21);
                        HTEmpList.Add("@EmpIdPrefix", EmpIDPrefix);
                    }
                    #endregion   End  Code developed by prasad [01-10-2013]

                    DataTable DtGetEmpList = config.ExecuteAdaptorAsyncWithParams(spname, HTEmpList).Result;
                    DataTable dtEmp = new DataTable("Data");
                    dtEmp = DtGetEmpList;
                    ds.Tables.Add(dtEmp);
                }
            }
            return ds;
        }

        protected void gvlistofemp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");
                e.Row.Cells[11].Attributes.Add("class", "text");
                e.Row.Cells[12].Attributes.Add("class", "text");
                e.Row.Cells[13].Attributes.Add("class", "text");
                e.Row.Cells[19].Attributes.Add("class", "text");
                e.Row.Cells[20].Attributes.Add("class", "text");
                e.Row.Cells[21].Attributes.Add("class", "text");
                e.Row.Cells[22].Attributes.Add("class", "text");
               
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}