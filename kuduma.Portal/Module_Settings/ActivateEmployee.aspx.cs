using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ActivateEmployee : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        protected void Page_Load(object sender, EventArgs e)
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
                FillSelectDropDown();
            }
        }


        protected void FillSelectDropDown()
        {
            ddlSelect.Items.Add("--Select--");
            ddlSelect.Items.Add("Employees");
            ddlSelect.Items.Add("Clients");
            ddlSelect.SelectedIndex = 0;
        }

        void BindEmployees()
        {

            Hashtable HTEmpList = new Hashtable();
            string spname = "IMReportForListOfEmployees";


            dgEmployees.DataSource = GetEmployees();
            dgEmployees.DataBind();
        }

        void BindClients()
        {
            dgClients.DataSource = GetClients();
            dgClients.DataBind();
        }

        void BindInActiveEmployees()
        {
            dgEmployees.DataSource = GetInActiveClients();
            dgEmployees.DataBind();
        }

        void BindInActiveClients()
        {
            dgClients.DataSource = GetInActiveClients();
            dgClients.DataBind();
        }

        protected DataTable GetEmployees()
        {

            #region Begin New Code  as on [25-03-2014]

            var SPName = "IMReportForActiveInactiveEmployees";
            Hashtable HtAIEmployees = new Hashtable();
            DataTable Dt = null;
            HtAIEmployees.Add("@EmpIDPrefix", EmpIDPrefix);

            #endregion End New Code  as on [25-03-2014]

            DataTable DtEmployees =config.ExecuteAdaptorAsyncWithParams(SPName, HtAIEmployees).Result;
            return DtEmployees;

        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected DataTable GetClients()
        {
            string cmpIDPrefix = GlobalData.Instance.GetClientIDPrefix();
            DataTable dt = new DataTable();
            string strQry = "Select ClientId as ID ,ClientName as Name,ClientSegment as Segment,ClientContactPerson as ContactPerson," +
                " ClientPersonDesgn as Designation,ClientPhonenumbers as Phone,ClientEmail as Email, ClientStatus " +
                " from Clients Order By clientid  ";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            return dt;

        }

        protected DataTable GetInActiveEmployees()
        {

            DataTable DtInActiveEmployeess = GlobalData.Instance.LoadInActiveEmployees();
            return DtInActiveEmployeess;

        }

        protected DataTable GetInActiveClients()
        {
            DataTable DtInActiveClients = GlobalData.Instance.LoadInActiveClients();
            return DtInActiveClients;

        }


        protected void UpdateEmployeeStatus(string id, int status)
        {

            string strQry = "Update EmpDetails set EmpStatus = " + status + " where EmpId = '" + id + "'";
            int status1 = config.ExecuteNonQueryWithQueryAsync(strQry).Result;
        }

        protected void UpdateClientStatus(string id, int status)
        {
            string strQry = "Update Clients set ClientStatus = " + status + " where ClientId = '" + id + "'";
            int status1 =config.ExecuteNonQueryWithQueryAsync(strQry).Result;
        }

        public string GetImage(bool Status)
        {
            //Convert.ToBoolean(
            if (Status == true)
                return "~/assets/Status_Active.png";
            else
                return "~/assets/Status_Archive.png";
        }

        protected void ddlSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridData();
            dgEmployees.Visible = false;
            dgClients.Visible = false;
            switch (ddlSelect.SelectedIndex)
            {
                case 1:
                    dgEmployees.Visible = true;
                    BindEmployees();
                    break;

                case 2:
                    BindClients();
                    dgClients.Visible = true;
                    break;

                default:
                    break;
            }
        }

        protected void ClearMessage()
        {
            lblresult.Text = "";
            if (ddlSelect.SelectedIndex == 0)
            {
                lblresult.Text = "Please Select Employees/Clients";
                return;
            }
        }

        protected void lnkActivate_Click(object sender, EventArgs e)
        {
            ClearMessage();
            if (ddlSelect.SelectedIndex == 1)
            {
                for (int i = 0; i < dgEmployees.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)dgEmployees.Rows[i].FindControl("Checkbox1");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            string empId = ((LinkButton)dgEmployees.Rows[i].FindControl("linkID")).Text;
                            UpdateEmployeeStatus(empId, 1);
                        }
                    }
                }
                BindEmployees();
            }
            else if (ddlSelect.SelectedIndex == 2)
            {
                for (int i = 0; i < dgClients.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)dgClients.Rows[i].FindControl("Checkbox1");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            string empId = ((LinkButton)dgClients.Rows[i].FindControl("linkID")).Text;
                            UpdateClientStatus(empId, 1);
                        }
                    }
                }
                BindClients();
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            ClearMessage();
            if (ddlSelect.SelectedIndex == 1)
            {
                for (int i = 0; i < dgEmployees.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)dgEmployees.Rows[i].FindControl("Checkbox1");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            string empId = ((LinkButton)dgEmployees.Rows[i].FindControl("linkID")).Text;
                            UpdateEmployeeStatus(empId, 0);
                        }
                    }
                }
                BindEmployees();
            }
            else if (ddlSelect.SelectedIndex == 2)
            {
                for (int i = 0; i < dgClients.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)dgClients.Rows[i].FindControl("Checkbox1");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            string empId = ((LinkButton)dgClients.Rows[i].FindControl("linkID")).Text;
                            UpdateClientStatus(empId, 0);
                        }
                    }
                }
                BindClients();
            }
        }

        protected void ClearGridData()
        {
            dgEmployees.DataSource = null;
            dgEmployees.DataBind();
            dgClients.DataSource = null;
            dgClients.DataBind();
        }

        protected void dgEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgEmployees.PageIndex = e.NewPageIndex;
            BindEmployees();
        }

        protected void dgClients_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgClients.PageIndex = e.NewPageIndex;
            BindClients();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            if (ddlSelect.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select Client/Employee ');", true);
                return;
            }
            if (ddltype.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Select type of search');", true);
                return;
            }
            if (txtsearch.Text.Trim().Length <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Enter Search word');", true);
                return;
            }
            string strQry = "";
            if (ddlSelect.SelectedIndex == 1) // Employees
            {
                if (ddltype.SelectedIndex == 1)
                {
                    strQry = "select e.EmpId ,(isnull(e.empfname,'')+ ' '+isnull(e.empmname,'') + ' '+ isnull(e.emplname,'') ) as Fullname, Ds.Design  as Designation," +
                              " e.EmpPhone as Phone ,  e.empstatus   " +
                              " from Empdetails e   inner join designations Ds on Ds.Designid=e.EmpDesgn  and  " +
                              " e.Empid like '%" + txtsearch.Text + "%' order by Right(e.Empid,6)";

                }
                else
                {

                    strQry = "select e.EmpId ,(isnull(e.empfname,'')+ ' '+isnull(e.empmname,'') + ' '+ isnull(e.emplname,'') ) as Fullname, Ds.Design  as Designation," +
                                  " e.EmpPhone as Phone  , e.empstatus   " +
                                  " from Empdetails e   inner join designations Ds on Ds.Designid=e.EmpDesgn  and  " +
                                  " (isnull(e.empfname,'')+ ' '+isnull(e.empmname,'') + ' '+ isnull(e.emplname,'') ) like '%" + txtsearch.Text + "%' order by Right(e.Empid,6)";
                }
            }
            else if (ddlSelect.SelectedIndex == 2) //Clients
            {
                if (ddltype.SelectedIndex == 1)
                {
                    strQry = "Select ClientId as ID ,ClientName as Name,ClientSegment as Segment,  " +
                        " ClientContactPerson as ContactPerson,ClientPersonDesgn as Designation,ClientPhonenumbers as Phone,ClientEmail as Email, " +
                        "  ClientStatus from Clients where ClientId like '%" + txtsearch.Text + "%'  Right(clientid,4) ";

                }
                else
                {
                    strQry = "Select ClientId as ID ,ClientName as Name,ClientSegment as Segment, " +
                        " ClientContactPerson as ContactPerson,ClientPersonDesgn as Designation,ClientPhonenumbers as Phone,ClientEmail as Email, " +
                        " ClientStatus from Clients where ClientName like '%" + txtsearch.Text +
                        "%' ORDER By  Right(clientid,4) "; ;
                }
            }
            if (strQry.Length > 0)
            {
                DataTable dt =config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
                if (ddlSelect.SelectedIndex == 1)
                {
                    dgEmployees.DataSource = dt;
                    dgEmployees.DataBind();
                }
                else
                {
                    dgClients.DataSource = dt;
                    dgClients.DataBind();
                }
            }
        }
    }
}