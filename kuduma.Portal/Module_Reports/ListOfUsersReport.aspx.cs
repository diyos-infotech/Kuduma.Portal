using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class ListOfUsersReport : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                    
                    FillUserDetails();
                    LoadEmptyBill();


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
            }
        }
        protected void FillUserDetails()
        {
            string strQry = "select ld.Emp_Id as Emp_Id,ld.UserName as UserName,ld.previligeid,ed.EmpMName as EmpMName,ed.EmpDesgn as EmpDesgn,p.PreviligerName as Name from logindetails as ld INNER JOIN previligers as p ON ld.previligeid=p.PreviligerId INNER JOIN EmpDetails as ed ON ld.Emp_Id=ed.EmpId AND ld.Emp_Id <> '0'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(strQry).Result;
            GVListEmployees.DataSource = dt;
            GVListEmployees.DataBind();
        }

        protected void txtbillno_OnTextChanged(object sender, EventArgs e)
        {

            string sqlqry = string.Empty;

            if (radionormal.Checked)
            {
                sqlqry = "Select Clients.Clientname,unitbill.unitid from unitbill  " +
                 "  inner join  clients  on Clients.clientid=unitbill.unitid Where billno='" + txtbillno.Text.Trim() + "'";

            }
            else
            {
                sqlqry = "Select Clients.Clientname,Munitbill.unitid from Munitbill  " +
                 "  inner join  clients  on Clients.clientid=Munitbill.unitid Where billno='" + txtbillno.Text.Trim() + "'";
            }


            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                txtclientid.Text = dt.Rows[0][1].ToString();
                txtclientname.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('invalid Bill  no');", true);

            }


        }

        protected void btndelelte_Click(object sender, EventArgs e)
        {

            string sqlqry = string.Empty;
            if (radionormal.Checked)
            {
                sqlqry = "  delete from unitbill  Where billno='" + txtbillno.Text.Trim() + "'";
            }
            else
            {
                sqlqry = "  delete from munitbill  Where billno='" + txtbillno.Text.Trim() + "'";

            }


            int status =config.ExecuteNonQueryWithQueryAsync(sqlqry).Result;
            if (status > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Bill Deleted Successfully');", true);


                txtbillno.Text = "";
                txtclientid.Text = "";
                txtclientname.Text = "";

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Invalid Bill No');", true);

            }

        }

        protected void txtLoanno_OnTextChanged(object sender, EventArgs e)
        {
            string sqlretrieve;
            if (txtLoanno.Text.Trim().Length > 0)
            {
                sqlretrieve = "select *from EmpLoanMaster where LoanNo='" + txtLoanno.Text.Trim() + "'";
                DataTable sqldtcheck = config.ExecuteAdaptorAsyncWithQueryParams(sqlretrieve).Result;
                if (sqldtcheck.Rows.Count > 0)
                {
                    divLoanDelete.Visible = true;

                    sqlretrieve = "select ELM.EmpId,ISNULL(ED.EmpFName,'')+''+ISNULL(ED.EmpMName,'')+''+ISNULL(ED.EmpLName,'') as EmpName,ELM.NoInstalments,ELM.LoanIssuedDate,ELM.LoanDt " +
                                    ",case ELM.TypeOfLoan when '0' then 'Sal.Adv' when '1' then 'Uniform' Else 'Others' End as LoanType,ELM.LoanAmount " +
                                    " from EmpDetails as ED join EmpLoanMaster as ELM " +
                                    " on ELM.EmpId=ED.EmpId and ELM.LoanNo='" + txtLoanno.Text.Trim() + "'";
                    DataTable dtretrieve = config.ExecuteAdaptorAsyncWithQueryParams(sqlretrieve).Result;
                    if (dtretrieve.Rows.Count > 0)
                    {
                        txtEmpid.Text = dtretrieve.Rows[0]["EmpId"].ToString();
                        txtEmpName.Text = dtretrieve.Rows[0]["EmpName"].ToString();
                        txtLoanType.Text = dtretrieve.Rows[0]["LoanType"].ToString();
                        txtLoanAmt.Text = dtretrieve.Rows[0]["LoanAmount"].ToString();
                        txtNoofInst.Text = dtretrieve.Rows[0]["NoInstalments"].ToString();
                        //txtLoanIssuedte.Text = DateTime.Parse(dtretrieve.Rows[0]["LoanIssuedDate"].ToString().Remove(11)).ToString("dd/MM/yyyy");
                        //txtLoanCutmonth.Text = DateTime.Parse(dtretrieve.Rows[0]["LoanDt"].ToString().Remove(11)).ToString("dd/MM/yyyy");
                    }
                }
                else
                {
                    ClearDeleteLoan();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('There is no loan for entered LoanNo');", true);
                    return;
                }
            }
            else
            {
                divLoanDelete.Visible = false;

            }
        }

        protected void btnLoanDelete_OnClick(object sender, EventArgs e)
        {
            if (txtLoanno.Text.Trim().Length > 0)
            {

                string sqlinser = "insert into DeletedLoan select LoanNo, LoanDt, EmpId, LoanAmount, NoInstalments, TypeOfLoan, LoanIssuedDate,ModofiedLoan='" + DateTime.Now + "' from EmpLoanMaster where LoanNo='" + txtLoanno.Text.Trim() + "'";
               int del=config.ExecuteNonQueryWithQueryAsync(sqlinser).Result;
                string sqldelete = "delete EmpLoanMaster where LoanNo='" + txtLoanno.Text.Trim() + "'";
                int status = config.ExecuteNonQueryWithQueryAsync(sqldelete).Result;
                ClearDeleteLoan();
                if (status > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Loan deleted successfully');", true);
                    return;
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Please enter LoanNo');", true);
                return;
            }
        }

        protected void ClearDeleteLoan()
        {
            txtLoanno.Text = "";
            txtEmpid.Text = "";
            txtEmpName.Text = "";
            txtLoanType.Text = "";
            txtLoanAmt.Text = "";
            txtNoofInst.Text = "";
            txtLoanIssuedte.Text = "";
            txtLoanCutmonth.Text = "";
            divLoanDelete.Visible = false;
        }

        protected void btncancelLoanDelete_OnClick(object sender, EventArgs e)
        {
            ClearDeleteLoan();
        }

        #region Begin ModifyBill Link

        protected void LoadEmptyBill()
        {
            Hashtable HTEmptyBillNo = new Hashtable();
            string Spname = "MissedBillNo";
            HTEmptyBillNo.Add("@Id", 1);
            DataTable DtEmptyBillno = config.ExecuteAdaptorAsyncWithParams(Spname, HTEmptyBillNo).Result;
            if (DtEmptyBillno.Rows.Count > 0)
            {
                GVEmptyBill.DataSource = DtEmptyBillno;
                GVEmptyBill.DataBind();
            }
            else
            {
                GVEmptyBill.DataSource = null;
                GVEmptyBill.DataBind();
            }
        }

        protected void ClearModifyBillPanel()
        {
            Txt_Old_Bill_No_Modify_Bill.Text = "";
            Txt_Client_Id_Modify_Bill.Text = "";
            Txt_Client_Name_Modify_Bill.Text = "";
            Txt_Month_Of_Modify_Bill.Text = "";
            Txt_Grand_Total_Modify_Bill.Text = "";
            Txt_New_Bill_No__Modify_Bill.Text = "";
            Txt_New_Bill_No__Modify_Bill2.Text = "";
        }
        protected void Btn_Modify_Bill_Update_Click(object sender, EventArgs e)
        {
            if (Txt_Old_Bill_No_Modify_Bill.Text == string.Empty)
            {
                Txt_Client_Id_Modify_Bill.Text = "";
                Txt_Client_Name_Modify_Bill.Text = "";
                Txt_Month_Of_Modify_Bill.Text = "";
                Txt_Grand_Total_Modify_Bill.Text = "";
                Txt_New_Bill_No__Modify_Bill.Text = "";
                Txt_New_Bill_No__Modify_Bill2.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Please enter old bill number');", true);
                return;
            }
            if (Txt_New_Bill_No__Modify_Bill2.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Please enter new bill number');", true);
                return;
            }
            //************************************************************************************************
            if (Txt_New_Bill_No__Modify_Bill2.Text.Length == 5)
            {
                string NewBill = Txt_Old_Bill_No_Modify_Bill.Text;
                int CountNewBill = NewBill.Length;
                int Endindex = CountNewBill - 5;
                string Txt_New_Bill_No = NewBill.Remove(0, Endindex);
                Hashtable HTEmptyBillNo = new Hashtable();
                string Spname = "MissedBillNo";
                HTEmptyBillNo.Add("@Id", 0);
                DataTable DtGetlastrecord = config.ExecuteAdaptorAsyncWithParams(Spname, HTEmptyBillNo).Result;
                if (DtGetlastrecord.Rows.Count > 0)
                {
                    int lastrecord = int.Parse(DtGetlastrecord.Rows[0]["BillSerial2"].ToString());
                    if (int.Parse(Txt_New_Bill_No__Modify_Bill2.Text) > (lastrecord + 1) || Txt_New_Bill_No == Txt_New_Bill_No__Modify_Bill2.Text)
                    {
                        Txt_New_Bill_No__Modify_Bill2.Text = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('The maximum value of bill number allowed is \\'" + (lastrecord + 1) + "\\' ');", true);
                        return;
                    }
                    else
                    {
                        object status = CheckBill();
                        if (int.Parse(status.ToString()) == 0)
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Please enter 5 digit number eg:00035');", true);
                return;
            }
            //*********************************************************************************************************
            Hashtable HTProcedure = new Hashtable();
            string New_Bill_No = (Txt_New_Bill_No__Modify_Bill.Text) + (Txt_New_Bill_No__Modify_Bill2.Text);
            string Olb_Bill_No = Txt_Old_Bill_No_Modify_Bill.Text;
            string SpName = "ModifyOldBill";

            if (Txt_New_Bill_No__Modify_Bill2.Text == "00000")
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Invalid number');", true);
                return;
            }
            else if (Txt_New_Bill_No__Modify_Bill2.Text.Length < 5)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Please enter 5 digit number eg:00035');", true);
                return;
            }
            else
            {
                if (Rdb_Bill_Type_Normal.Checked == true)
                {
                    HTProcedure.Add("@BillType", 0);
                }
                else
                {
                    if (Rdb_Bill_Type_Manual.Checked == true)
                    {
                        HTProcedure.Add("@BillType", 1);
                    }
                }
                HTProcedure.Add("@OldBill", Txt_Old_Bill_No_Modify_Bill.Text);
                HTProcedure.Add("@NewBill", New_Bill_No);
                int status =config.ExecuteNonQueryParamsAsync(SpName, HTProcedure).Result;
                if (status <= 0)
                {
                    Txt_New_Bill_No__Modify_Bill2.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill number does not exist in \\'Selected bill type\\'');", true);
                    return;
                }
                else
                {
                    if (status >= 0)
                    {

                        ClearModifyBillPanel();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Modification successful \\nOld bill \\'" + Olb_Bill_No + "\\' is replaced with \\'" + New_Bill_No + "\\'');", true);
                        //return;
                        LoadEmptyBill();
                    }
                }
            }
        }
        protected void Rdb_Bill_Type_Normal_CheckedChanged(object sender, EventArgs e)
        {
            ClearModifyBillPanel();
        }
        protected void Rdb_Bill_Type_Manual_CheckedChanged(object sender, EventArgs e)
        {
            ClearModifyBillPanel();
        }
        protected void Txt_Old_Bill_No_Modify_Bill_OnTextChanged(object sender, EventArgs e)
        {

            string SqlChk = ""; string Olb_Bill_No = Txt_Old_Bill_No_Modify_Bill.Text;
            if (Txt_Old_Bill_No_Modify_Bill.Text != string.Empty)
            {
                if (Rdb_Bill_Type_Normal.Checked == true)
                {
                    SqlChk = "select C.ClientId,C.ClientName,DATENAME(month,convert(DATETIME,case when Len(UB.Month)>3 then (LEFT(UB.Month,2)+'/'+ " +
                   "Left(08,1)+'/'+Right(UB.Month,2))else (LEFT(UB.Month,1)+'/'+Left(08,1)+'/'+Right(UB.Month,2)) end ))+'-'+Left(20,2)+ " +
                   "RIGHT(UB.Month,2) as Month,UB.GrandTotal  from UnitBill UB join Clients C on UB.UnitId=C.ClientId and UB.BillNo='" + Txt_Old_Bill_No_Modify_Bill.Text + "'";
                }
                else
                {
                    if (Rdb_Bill_Type_Manual.Checked == true)
                    {
                        SqlChk = "select C.ClientId,C.ClientName,DATENAME(month,convert(DATETIME,case when Len(MUB.Month)>3 then (LEFT(MUB.Month,2)+'/'+ " +
                        "Left(08,1)+'/'+Right(MUB.Month,2))else (LEFT(MUB.Month,1)+'/'+Left(08,1)+'/'+Right(MUB.Month,2)) end ))+'-'+Left(20,2)+ " +
                        "RIGHT(MUB.Month,2) as Month,MUB.GrandTotal  from MUnitBill MUB join Clients C on MUB.UnitId=C.ClientId and MUB.BillNo='" + Txt_Old_Bill_No_Modify_Bill.Text + "'";
                    }
                }

                DataTable DtSqlChk = config.ExecuteAdaptorAsyncWithQueryParams(SqlChk).Result;
                if (DtSqlChk.Rows.Count > 0)
                {
                    Txt_Client_Id_Modify_Bill.Text = DtSqlChk.Rows[0]["ClientId"].ToString();
                    Txt_Client_Name_Modify_Bill.Text = DtSqlChk.Rows[0]["ClientName"].ToString();
                    Txt_Month_Of_Modify_Bill.Text = DtSqlChk.Rows[0]["Month"].ToString();
                    Txt_Grand_Total_Modify_Bill.Text = DtSqlChk.Rows[0]["GrandTotal"].ToString();
                    string NewBill = Txt_Old_Bill_No_Modify_Bill.Text;
                    int CountNewBill = NewBill.Length;
                    int startindex = CountNewBill - 5;
                    Txt_New_Bill_No__Modify_Bill.Text = NewBill.Remove(startindex, 5);
                }
                else
                {
                    ClearModifyBillPanel();
                    if (Rdb_Bill_Type_Normal.Checked == true)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill number \\'" + Olb_Bill_No + "\\' does not exists in \\'Normal bill\\'');", true);
                        return;
                    }
                    if (Rdb_Bill_Type_Manual.Checked == true)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill number \\'" + Olb_Bill_No + "\\' does not exists in \\'Modify bill\\'');", true);
                        return;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please enter OldBill Number');", true);
                return;
            }

        }
        protected object CheckBill()
        {
            string New_Bill_No = (Txt_New_Bill_No__Modify_Bill.Text) + (Txt_New_Bill_No__Modify_Bill2.Text);
            string SqlCheckBillno = "";
            string OldBill = Txt_Old_Bill_No_Modify_Bill.Text;
            SqlCheckBillno = "select MUB.BillNo,MUB.UnitId,C.ClientName from MUnitBill MUB join Clients C on C.ClientId=MUB.UnitId where Right(MUB.BillNo,5)='" + Txt_New_Bill_No__Modify_Bill2.Text + "' Union " +
            "select UB.BillNo,UB.UnitId,C.ClientName from UnitBill UB join Clients C on C.ClientId=UB.UnitId where Right(UB.BillNo,5)='" + Txt_New_Bill_No__Modify_Bill2.Text + "'";

            DataTable DtCheckBillno = config.ExecuteAdaptorAsyncWithQueryParams(SqlCheckBillno).Result;
            if (DtCheckBillno.Rows.Count > 0)
            {
                string ClientId = DtCheckBillno.Rows[0]["Unitid"].ToString();
                string ClientName = DtCheckBillno.Rows[0]["ClientName"].ToString();
                Txt_New_Bill_No__Modify_Bill2.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('BillNo:\\'" + New_Bill_No + "\\' already exists for \\nClientId:\\'" + ClientId + "\\'\\nClientName:\\'" + ClientName + "\\'');", true);
                return 0;
            }
            else return 1;
        }

        protected void Btn_Modify_Bill_Cancel_Click(object sender, EventArgs e)
        {
            ClearModifyBillPanel();
        }
        protected void GVEmptyBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVEmptyBill.PageIndex = e.NewPageIndex;
            LoadEmptyBill();
        }
        protected void Btn_cancle_midifyBill_Click(object sender, EventArgs e)
        {
            MPE_Modify_Bill_No.Hide();
        }

        #endregion End ModifyBill Link
    }
}