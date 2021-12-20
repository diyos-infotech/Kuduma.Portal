using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Clients
{
    public partial class ClientManPowerReq : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        //DataTable dt;
        DropDownList bind_dropdownlist;
        DropDownList bind_dropdownlistshift;

        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Branch = "";
        string BranchID = "";


        protected void Page_Load(object sender, EventArgs e)
        {

            int i = 0;
            try
            {
                GetWebConfigdata();
                if (!IsPostBack)
                {
                    //rowindexvisible = 1;
                    Session["ContractsAIndex"] = 0;
                    Session["ContractsAIndexsw"] = 0;
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                       
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                    LoadClientList();
                    LoadClientNames();
                    displaydata();
                    DisplayDefaultRow();
                    Enable5Rows();


                    if (Request.QueryString["clientid"] != null)
                    {
                        string username = Request.QueryString["clientid"].ToString();
                        ddlclientid.SelectedValue = username;
                        ddlclientid_OnSelectedIndexChanged(sender, e);

                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void Fillcname()
        {
            if (ddlclientid.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void FillClientid()
        {
            if (ddlcname.SelectedIndex > 0)
            {
                ddlclientid.SelectedValue = ddlcname.SelectedValue;
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "Clientname";
                ddlcname.DataSource = dt;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");

        }

        protected void LoadClientList()
        {

            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "clientid";
                ddlclientid.DataTextField = "clientid";
                ddlclientid.DataSource = dt;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
        }

        protected void Enable5Rows()
        {
            btnadddesgn_Click1(this, null);
            btnadddesgn_Click1(this, null);
            btnadddesgn_Click1(this, null);
            btnadddesgn_Click1(this, null);
            btnadddesgn_Click1(this, null);

        }

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void DisplayDefaultRow()
        {
            for (int i = 0; i < gvdesignation.Rows.Count; i++)
            {
                if (i < 1)
                {
                    Session["ContractsAIndex"] = Convert.ToInt16(Session["ContractsAIndex"]) + 1;
                    gvdesignation.Rows[i].Visible = true;
                    DefaultRowData(i);
                }
                else
                    gvdesignation.Rows[i].Visible = false;
            }
            Session["ContractsAIndex"] = 1;
            int check = int.Parse(Session["ContractsAIndex"].ToString());
        }

        protected void DefaultRowData(int row)
        {
            string Cddldesignation = ((DropDownList)gvdesignation.Rows[row].FindControl("DdlDesign")).Text;
            DropDownList ddlindex = gvdesignation.Rows[row].FindControl("DdlDesign") as DropDownList;
            ddlindex.SelectedIndex = 0;

            DropDownList ddlshift = gvdesignation.Rows[row].FindControl("ddlshift") as DropDownList;
            ddlshift.SelectedIndex = 0;

            TextBox txtSStarttime = (TextBox)gvdesignation.Rows[row].FindControl("txtSStarttime");
            txtSStarttime.Text = "";

            TextBox txtSEndtime = (TextBox)gvdesignation.Rows[row].FindControl("txtSEndtime");
            txtSEndtime.Text = "";

            TextBox txtQty = (TextBox)gvdesignation.Rows[row].FindControl("txtQty");
            txtQty.Text = "";


        }

        private void displaydata()
        {

            DataTable DtDesignation = GlobalData.Instance.LoadDesigns();

            gvdesignation.DataSource = DtDesignation;
            gvdesignation.DataBind();

            string queryshift = "select * from shifts";
            DataTable dtshift = config.ExecuteAdaptorAsyncWithQueryParams(queryshift).Result;


            foreach (GridViewRow grdRow in gvdesignation.Rows)
            {
                bind_dropdownlist = (DropDownList)(gvdesignation.Rows[grdRow.RowIndex].Cells[0].FindControl("DdlDesign"));
                bind_dropdownlist.Items.Clear();

                if (DtDesignation.Rows.Count > 0)
                {
                    bind_dropdownlist.DataValueField = "Designid";
                    bind_dropdownlist.DataTextField = "Design";
                    bind_dropdownlist.DataSource = DtDesignation;
                    bind_dropdownlist.DataBind();

                }
                bind_dropdownlist.Items.Insert(0, "--Select--");
                bind_dropdownlist.SelectedIndex = 0;


                bind_dropdownlistshift = (DropDownList)(gvdesignation.Rows[grdRow.RowIndex].Cells[1].FindControl("ddlshift"));
                bind_dropdownlistshift.Items.Clear();

                if (dtshift.Rows.Count > 0)
                {
                    bind_dropdownlistshift.DataValueField = "shift";
                    bind_dropdownlistshift.DataTextField = "shift";
                    bind_dropdownlistshift.DataSource = dtshift;
                    bind_dropdownlistshift.DataBind();

                }
                bind_dropdownlistshift.Items.Insert(0, "--Select--");
                bind_dropdownlistshift.SelectedIndex = 0;
            }

        }

        protected void btnadddesgn_Click1(object sender, EventArgs e)
        {
            int designCount = Convert.ToInt16(Session["ContractsAIndex"]);
            if (designCount < gvdesignation.Rows.Count)
            {
                gvdesignation.Rows[designCount].Visible = true;
                DefaultRowData(designCount);

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                DropDownList ddldrow = gvdesignation.Rows[designCount].FindControl("DdlDesign") as DropDownList;
                ddldrow.Items.Clear();



                if (DtDesignation.Rows.Count > 0)
                {
                    ddldrow.DataValueField = "Designid";
                    ddldrow.DataTextField = "Design";
                    ddldrow.DataSource = DtDesignation;
                    ddldrow.DataBind();

                }
                ddldrow.Items.Insert(0, "--Select--");
                ddldrow.SelectedIndex = 0;


                string queryshift = "select * from shifts";
                DataTable dtshift = config.ExecuteAdaptorAsyncWithQueryParams(queryshift).Result;
                DropDownList ddlshift = gvdesignation.Rows[designCount].FindControl("ddlshift") as DropDownList;
                ddlshift.Items.Clear();



                if (dtshift.Rows.Count > 0)
                {
                    ddlshift.DataValueField = "shift";
                    ddlshift.DataTextField = "shift";
                    ddlshift.DataSource = dtshift;
                    ddlshift.DataBind();

                }
                ddlshift.Items.Insert(0, "--Select--");
                ddlshift.SelectedIndex = 0;


                designCount = designCount + 1;
                Session["ContractsAIndex"] = designCount;
            }
            else
            {
                lblMsg.Text = "Theres is No more Designations";
            }
        }

        private void ClearDataFromThePage()
        {
            //gvdesignation.DataSource = null;
            //gvdesignation.DataBind();
        }

        protected void Btn_Save_Contracts_Click(object sender, EventArgs e)
        {
            int j = 0;
            int result = 0;
            DateTime today = DateTime.Now.Date;
            int designCount = Convert.ToInt16(Session["ContractsAIndex"]);
            //Store Data into Contract Details
            int clientdesigncount = 0;


            for (j = 0; j < designCount; j++)
            {
                if (j < gvdesignation.Rows.Count)
                {


                    string Cddldesignation = ((DropDownList)gvdesignation.Rows[j].FindControl("DdlDesign")).SelectedValue;
                    DropDownList ddlindex = gvdesignation.Rows[j].FindControl("DdlDesign") as DropDownList;

                    string Cddlshift = ((DropDownList)gvdesignation.Rows[j].FindControl("ddlshift")).SelectedValue;
                    DropDownList ddlshift = gvdesignation.Rows[j].FindControl("ddlshift") as DropDownList;

                    string CtxtShiftStarttime = ((TextBox)gvdesignation.Rows[j].FindControl("txtSStarttime")).Text;
                    if (CtxtShiftStarttime.Trim().Length == 0)
                    {
                        CtxtShiftStarttime = "";
                    }
                    string CtxtShiftendtime = ((TextBox)gvdesignation.Rows[j].FindControl("txtSEndtime")).Text;
                    if (CtxtShiftendtime.Trim().Length == 0)
                    {
                        CtxtShiftendtime = "";
                    }
                    string Ctxtquantity = ((TextBox)gvdesignation.Rows[j].FindControl("txtQty")).Text;
                    if (Ctxtquantity.Trim().Length == 0)
                    {
                        Ctxtquantity = "0";
                    }
                    string queryofgrid = "";

                    string queryselect = "select * from ClientManpowerReq where clientid='" + ddlclientid.SelectedValue + "'";
                    DataTable dtselect = config.ExecuteAdaptorAsyncWithQueryParams(queryselect).Result;
                    if (j == 0)
                    {
                        string DeleteQry = "delete from ClientManpowerReq where clientid='" + ddlclientid.SelectedValue + "'";
                        DataTable DtDelete = config.ExecuteAdaptorAsyncWithQueryParams(DeleteQry).Result;
                    }

                    if (ddlindex.SelectedIndex != 0)
                    {
                        if (ddlshift.SelectedIndex != 0)
                        {
                            queryofgrid = "insert into ClientManpowerReq (clientid,Design,Shift,ShiftStartTime,ShiftEndTime,Qty) values ('" + ddlclientid.SelectedValue + "','" + Cddldesignation + "','" + Cddlshift + "','" + CtxtShiftStarttime + "','" + CtxtShiftendtime + "','" + Ctxtquantity + "')";
                            result = config.ExecuteNonQueryWithQueryAsync(queryofgrid).Result;
                        }

                    }
                }
            }
            if (result > 0)
            {
                lblSuc.Text = "Details Added Successfully !";
                GetGridData();
            }
            else
            {
                lblSuc.Text = "Details Added UnSuccessfully !";
            }
        }

        protected void gvdesignation_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
        }

        protected void GetGridData()
        {


            Hashtable HtContracts = new Hashtable();

            try
            {
                Session["DataContractsAIndex"] = 0;

                ClearDataFromThePage();

                DisplayDefaultRow();

                gvdesignation.Visible = true;
                DateTime today = DateTime.Now.Date;

                string query = "select Design,Shift,ShiftStartTime,ShiftEndTime,Qty from ClientManpowerReq where ClientId='" + ddlclientid.SelectedValue + "'";
                DataTable DtContractDetailsData = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                if (DtContractDetailsData.Rows.Count > 0)
                {


                    for (int i = 0; i < DtContractDetailsData.Rows.Count; i++)
                    {


                        gvdesignation.Rows[i].Visible = true;
                        //DefaultRowData(i);

                        DropDownList CDesgn = gvdesignation.Rows[i].FindControl("DdlDesign") as DropDownList;

                        if (String.IsNullOrEmpty(DtContractDetailsData.Rows[i]["Design"].ToString()) != false)
                        {
                            CDesgn.SelectedIndex = 0;

                        }
                        else
                        {
                            if (int.Parse(DtContractDetailsData.Rows[i]["Design"].ToString()) != 0)
                            {
                                CDesgn.SelectedValue = DtContractDetailsData.Rows[i]["Design"].ToString();
                            }
                            else
                            {
                                CDesgn.SelectedIndex = 0;
                            }
                        }

                        DropDownList ddlshift = gvdesignation.Rows[i].FindControl("ddlshift") as DropDownList;
                        ddlshift.SelectedValue = DtContractDetailsData.Rows[i]["Shift"].ToString();

                        TextBox txtSStarttime = (TextBox)gvdesignation.Rows[i].FindControl("txtSStarttime");
                        txtSStarttime.Text = DtContractDetailsData.Rows[i]["ShiftStartTime"].ToString();

                        TextBox txtSEndtime = (TextBox)gvdesignation.Rows[i].FindControl("txtSEndtime");
                        txtSEndtime.Text = DtContractDetailsData.Rows[i]["ShiftEndTime"].ToString();


                        TextBox CQuantity = (TextBox)gvdesignation.Rows[i].FindControl("txtQty");
                        CQuantity.Text = DtContractDetailsData.Rows[i]["Qty"].ToString();

                        if (i < DtContractDetailsData.Rows.Count)
                        {
                            Session["DataContractsAIndex"] = i + 1;
                            NewDataRow();
                        }

                    }
                }
                else
                {
                    NewDataRow();
                }
                Session["ContractsAIndex"] = DtContractDetailsData.Rows.Count + 1;



            }
            catch (Exception ex)
            {

            }
        }

        protected void NewDataRow()
        {
            int designcount = (int)Session["DataContractsAIndex"];

            if (designcount < gvdesignation.Rows.Count)
            {
                gvdesignation.Rows[designcount].Visible = true;
                DefaultRowData(designcount);

                DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
                DropDownList ddldrow = gvdesignation.Rows[designcount].FindControl("DdlDesign") as DropDownList;
                ddldrow.Items.Clear();

                if (DtDesignation.Rows.Count > 0)
                {
                    ddldrow.DataValueField = "Designid";
                    ddldrow.DataTextField = "Design";
                    ddldrow.DataSource = DtDesignation;
                    ddldrow.DataBind();

                }
                ddldrow.Items.Insert(0, "--Select--");
                ddldrow.SelectedIndex = 0;

            }
        }

        protected void ddlclientid_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearDataFromThePage();
            DisplayDefaultRow();


            if (ddlclientid.SelectedIndex > 0)
            {
                Fillcname();
                GetGridData();
            }
            else
            {
                ddlcname.SelectedIndex = 0;

            }
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearDataFromThePage();
            DisplayDefaultRow();


            if (ddlcname.SelectedIndex > 0)
            {
                FillClientid();
                GetGridData();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void gvdesignation_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void ddlshift_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectedDesign = "";
            var selectedshift = "";
            var selectedSno = "";
            var designation = "";
            var shift = "";
            var SNo = "";

            DropDownList lb = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)lb.NamingContainer;

            DropDownList DdlselectedDesign = gvRow.FindControl("DdlDesign") as DropDownList;
            DropDownList ddlselectedshift = gvRow.FindControl("ddlshift") as DropDownList;
            Label lblselectedsno = gvRow.FindControl("lblsno") as Label;


            selectedDesign = DdlselectedDesign.SelectedValue;
            selectedshift = ddlselectedshift.SelectedValue;
            selectedSno = lblselectedsno.Text;

            TextBox txtShiftStarttime = gvRow.FindControl("txtSStarttime") as TextBox;
            TextBox txtShiftendtime = gvRow.FindControl("txtSEndtime") as TextBox;
            string query = "select ShiftStartTime,ShiftEndTime from shifts where shift='" + selectedshift + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                txtShiftStarttime.Text = dt.Rows[0]["ShiftStartTime"].ToString();
                txtShiftendtime.Text = dt.Rows[0]["ShiftEndTime"].ToString();
            }




            for (int k = 0; k < gvdesignation.Rows.Count; k++)
            {
                DropDownList DdlDesign = (DropDownList)gvdesignation.Rows[k].FindControl("DdlDesign");
                DropDownList ddlshift = (DropDownList)gvdesignation.Rows[k].FindControl("ddlshift");
                Label lblsno = (Label)gvdesignation.Rows[k].FindControl("lblsno");

                designation = DdlDesign.SelectedValue;
                shift = ddlshift.SelectedValue;
                SNo = lblsno.Text;

                if (selectedDesign == designation && selectedshift == shift && selectedSno != SNo)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please select Different Shift !');", true);
                    ddlselectedshift.SelectedIndex = 0;
                    return;
                    //lblmsgcontractdetails.Text = "Please selected Different Shift !";

                }
            }





        }
    }
}