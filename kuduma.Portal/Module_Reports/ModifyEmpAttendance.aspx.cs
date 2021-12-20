using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KLTS.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal.Module_Reports
{
    public partial class ModifyEmpAttendance : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Username = "";
        string BranchID = "";
        DropDownList bind_Desgndropdownlist;
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
                if (this.Master != null)
                {
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli1");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                LoadDesignations();
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
            Username = Session["UserId"].ToString();
        }
        protected void GetEmpid()
        {

            #region  Old Code
            string Sqlqry = "select Empid +' - '+ OlEmpid Empid from empdetails where Branch in (" + BranchID + ") and  (empfname+' '+empmname+' '+emplname)  like '" + txtName.Text + "'  and empid like '%" + EmpIDPrefix + "%'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["Empid"].ToString();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                txtEmpid.Text = "";
                txtName.Text = "";
            }
            #endregion // End Old Code

        }

        protected void GetEmpName()
        {
            if (txtEmpid.Text.Length > 0)
            {
                string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where Branch in (" + BranchID + ") and  empid='" + txtEmpid.Text+ "' ";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        txtName.Text = dt.Rows[0]["empname"].ToString();

                    }
                    catch (Exception ex)
                    {
                        // MessageLabel.Text = ex.Message;
                    }
                }
                else
                {
                    txtEmpid.Text = "";
                    txtName.Text = "";
                }

            }
        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            txtmonth.Text = "";
            GVModifyAttendance.DataSource = null;
            GVModifyAttendance.DataBind();
            lblalert.Text = "";
            lblSavealert.Text = "";
            GetEmpid();
        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            txtmonth.Text = "";
            GVModifyAttendance.DataSource = null;
            GVModifyAttendance.DataBind();
            lblalert.Text = "";
            lblSavealert.Text = "";
            GetEmpName();
        }

        public string GetMonthName(int month)
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            monthname = mfi.GetMonthName(month).ToString();

            return monthname;
        }

        public string GetMonthOfYear(int month)
        {
            string MonthYear = "";

            if (month.ToString().Length == 4)
            {
                MonthYear = "20" + month.ToString().Substring(2, 2);
            }
            if (month.ToString().Length == 3)
            {
                MonthYear = "20" + month.ToString().Substring(1, 2);

            }
            return MonthYear;
        }

        public void LoadDesignations()
        {

            DataTable DtDesignation = GlobalData.Instance.LoadDesigns();
            foreach (GridViewRow grdRow in GVModifyAttendance.Rows)
            {
                bind_Desgndropdownlist = (DropDownList)(GVModifyAttendance.Rows[grdRow.RowIndex].Cells[1].FindControl("DdlDesign"));
                bind_Desgndropdownlist.Items.Clear();

                if (DtDesignation.Rows.Count > 0)
                {
                    bind_Desgndropdownlist.DataValueField = "Designid";
                    bind_Desgndropdownlist.DataTextField = "Design";
                    bind_Desgndropdownlist.DataSource = DtDesignation;
                    bind_Desgndropdownlist.DataBind();

                }
                bind_Desgndropdownlist.Items.Insert(0, "--Select--");
                bind_Desgndropdownlist.SelectedIndex = 0;
            }
        }
        public void GetAttendanceDetails()
        {
            lblalert.Text = "";
            string date = string.Empty;
            string monthnew = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            monthnew = month + Year.Substring(2, 2);

            string Qry = "Select Clientid,ea.empid,ed.empfname name,noofduties,OT,wo,PL,Design,contractId from empattendance ea inner join empdetails ed on ed.empid=ea.empid where Ea.empid='" + txtEmpid.Text+ "' and Month='" + monthnew + "'";
            DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
            if (Dt.Rows.Count > 0)
            {
                GVModifyAttendance.DataSource = Dt;
                GVModifyAttendance.DataBind();
                LoadDesignations();

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    DropDownList CDesgnsw = GVModifyAttendance.Rows[i].FindControl("DdlDesign") as DropDownList;
                    if (String.IsNullOrEmpty(Dt.Rows[i]["Design"].ToString()) != false)
                    {
                        CDesgnsw.SelectedIndex = 0;
                    }
                    else
                    {
                        if (int.Parse(Dt.Rows[i]["Design"].ToString()) != 0)
                        {
                            CDesgnsw.SelectedValue = Dt.Rows[i]["Design"].ToString();
                        }
                        else
                        {
                            CDesgnsw.SelectedIndex = 0;

                        }
                    }
                }
            }
            else
            {
                lblalert.Text = "Details are not Avaliable";
                GVModifyAttendance.DataSource = null;
                GVModifyAttendance.DataBind();
            }

        }

        protected void btnsaveAttendance_Click(object sender, EventArgs e)
        {
            int IRecordStatus = 0;
            string date = string.Empty;
            string monthnew = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            monthnew = month + Year.Substring(2, 2);

            string UpdateZeroQry = "Update empattendance set Noofduties=0,Ot=0 where empid='" + txtEmpid.Text.Trim().Substring(0, 9) + "' and month='" + monthnew + "'";
            DataTable DtZeroupdate = config.ExecuteAdaptorAsyncWithQueryParams(UpdateZeroQry).Result;
            for (int i = 0; i < GVModifyAttendance.Rows.Count; i++)
            {
                var NoofDuties = "0";
                var OTs = "0";
                var WOs = "0";
                var PL = "0";
                var Design = "0";
                var Clientid = "0";
                var Contractid = "0";
                var DesignID = "0";

                Label lblClientid = GVModifyAttendance.Rows[i].FindControl("lblClientid") as Label;
                Label lblcontractId = GVModifyAttendance.Rows[i].FindControl("lblcontractId") as Label;
                Label lblempid = GVModifyAttendance.Rows[i].FindControl("lblempid") as Label;
                DropDownList DdlDesign = GVModifyAttendance.Rows[i].FindControl("DdlDesign") as DropDownList;
                Label DdlDesignID = GVModifyAttendance.Rows[i].FindControl("DdlDesignID") as Label;
                TextBox txtnoofduties = GVModifyAttendance.Rows[i].FindControl("txtnoofduties") as TextBox;
                TextBox txtots = GVModifyAttendance.Rows[i].FindControl("txtots") as TextBox;
                TextBox txtwos = GVModifyAttendance.Rows[i].FindControl("txtwos") as TextBox;
                TextBox txtPL = GVModifyAttendance.Rows[i].FindControl("txtPL") as TextBox;

                Clientid = lblClientid.Text;
                Contractid = lblcontractId.Text;
                Design = DdlDesign.SelectedValue;
                DesignID = DdlDesignID.Text;

                if (txtnoofduties.Text.Trim().Length > 0)
                {
                    NoofDuties = txtnoofduties.Text.Trim();
                }

                if (txtots.Text.Trim().Length > 0)
                {
                    OTs = txtots.Text.Trim();
                }

                if (txtwos.Text.Trim().Length > 0)
                {
                    WOs = txtwos.Text.Trim();
                }

                if (txtPL.Text.Trim().Length > 0)
                {
                    PL = txtPL.Text.Trim();
                }

                string SPName = "AssignDuties";
                Hashtable ht = new Hashtable();
                ht.Add("@empid", lblempid.Text);
                ht.Add("@month", monthnew);
                ht.Add("@OriginalDuties", NoofDuties);
                ht.Add("@OriginalOTs", OTs);
                ht.Add("@clientid", Clientid);
                ht.Add("@Design", DesignID);
                ht.Add("@Contractid", Contractid);

                DataTable dtduties = config.ExecuteAdaptorAsyncWithParams(SPName, ht).Result;

                decimal dutiesv = 0;
                decimal otsv = 0;
                decimal Monthdays = 0;
                decimal sundays = 0;

                if (dtduties.Rows.Count > 0)
                {
                    dutiesv = decimal.Parse(dtduties.Rows[0]["Duties"].ToString());
                    otsv = decimal.Parse(dtduties.Rows[0]["ots"].ToString());
                    Monthdays = decimal.Parse(dtduties.Rows[0]["Monthdays"].ToString());
                    sundays = decimal.Parse(dtduties.Rows[0]["Sundays"].ToString());


                    string UpdateQry = "update empattendance set Design='" + Design + "',NoOfDuties ='" + dutiesv + "',OT ='" + otsv + "',WO ='" + WOs + "',PL ='" + PL + "' where empid='" + lblempid.Text + "' and Clientid='" + Clientid + "' and Month='" + monthnew + "' and Design='" + DesignID + "'";
                    IRecordStatus = config.ExecuteNonQueryWithQueryAsync(UpdateQry).Result;

                }
            }

            if (IRecordStatus > 0)
            {
                lblalert.Text = "";
                lblSavealert.Text = "Employee Attendance Added Sucessfully  ";
                GetAttendanceDetails();
            }
        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {
            GVModifyAttendance.DataSource = null;
            GVModifyAttendance.DataBind();
            lblalert.Text = "";
            lblSavealert.Text = "";
            GetAttendanceDetails();
        }
    }
}