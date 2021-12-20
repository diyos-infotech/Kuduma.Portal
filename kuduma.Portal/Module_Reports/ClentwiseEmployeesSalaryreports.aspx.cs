using System;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class ClentwiseEmployeesSalaryreports : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string BranchID = "";
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

                ddlcname.Items.Add("--Select--");
                LoadClientNames();

                FillClientList();
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                FillClientid();
            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlclientid.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                Fillcname();

            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void Fillcname()
        {

            if (ddlclientid.SelectedIndex == 1)
            {
                ddlcname.SelectedIndex = 1;
                return;
            }

            string SqlQryForCname = "Select Clientname from Clients where clientid='" + ddlclientid.SelectedValue + "' and clientid like '%" + CmpIDPrefix + "%'  ";
            DataTable dtCname = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForCname).Result;
            if (dtCname.Rows.Count > 0)
                ddlcname.SelectedValue = dtCname.Rows[0]["Clientname"].ToString();
        }

        protected void FillClientid()
        {


            if (ddlcname.SelectedIndex == 1)
            {
                ddlclientid.SelectedIndex = 1;
                return;
            }




            string SqlQryForCid = "Select Clientid from Clients where clientname='" + ddlcname.SelectedValue + "' and clientid like '%" + CmpIDPrefix + "%' ";
            DataTable dtCname = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForCid).Result;
            if (dtCname.Rows.Count > 0)
                ddlclientid.SelectedValue = dtCname.Rows[0]["Clientid"].ToString();
        }

        protected void LoadClientNames()
        {
            string selectquery = "select Clientname from Clients    where clientid like '%" + CmpIDPrefix + "%' and clientstatus=1  order by Clientname";
            DataTable dtable = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;
            for (int i = 0; i < dtable.Rows.Count; i++)
            {
                ddlcname.Items.Add(dtable.Rows[i]["Clientname"].ToString());
            }

            ddlcname.Items.Insert(1, "All");
        }

        protected void FillClientList()
        {
            string sqlQry = "Select ClientId from Clients where clientid like '%" + CmpIDPrefix + "%'  Order By Right(clientid,4)";
            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
            ddlclientid.Items.Clear();
            ddlclientid.Items.Add("--Select--");
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ddlclientid.Items.Add(data.Rows[i]["ClientId"].ToString());
            }

            ddlclientid.Items.Insert(1, "All");


        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();

            if (ddlclientid.SelectedIndex == 0)
            {
                LblResult.Text = "Please Select Client Id/Name";
                return;
            }

            if (txtmonth.Text.Trim().Length == 0)
            {
                LblResult.Text = "Please Select Month";
                return;
            }
            loadallsalarydetails();
        }

        protected void loadallsalarydetails()
        {
            string Clientid = ddlclientid.SelectedValue;
            string date = string.Empty;
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            string sqlqry = string.Empty;
            if (ddlclientid.SelectedIndex == 1)
            {
                sqlqry = "Select   EP.Empid,EP.Clientid,C.Clientname,(E.Empfname +' ' + E.EmpMname + '  ' + E.emplname) as EmpMname ,D.Design," +
               " E.Empbankacno,E.EmpBankCardRef, EP.basic,EP.da,EP.noofduties,EA.ot,EP.Proftax, " +
               " EP.gross as totalgross,EP.otamt,EP.pf,EP.esi,EP.penalty,EP.actualamount,EP.owf,EP.saladvded from emppaysheet EP " +
               "  inner join Clients C on  C.Clientid = EP.Clientid " +
               "   inner join  empdetails E on E.Empid=EP.Empid   " +
               " inner join  Empattendance EA on EA.Empid=EP.Empid  and  EP.Desgn=EA.Design " +
               " Inner Join Designations  D on D.DesignID=EP.Desgn and  EP.noofduties<>0  and EP.month='" + month + Year.Substring(2, 2) +
               "'  and  EP.month=EA.Month  and  EP.Clientid=EA.Clientid order By Right(EP.Empid,6)";
                Bindata(sqlqry);
                return;
            }



            sqlqry = "Select   EP.Empid,EP.Clientid,C.Clientname,(E.Empfname +' ' + E.EmpMname + '  ' + E.emplname) as EmpMname ,D.Design," +
                " E.Empbankacno,E.EmpBankCardRef, EP.basic,EP.da,EP.noofduties,EA.ot,EP.Proftax, " +
                " EP.gross as totalgross,EP.otamt,EP.pf,EP.esi,EP.penalty,EP.actualamount,EP.owf,EP.saladvded from emppaysheet EP " +
           "  inner join Clients C on  C.Clientid = EP.Clientid " +
           "   inner join  empdetails E on E.Empid=EP.Empid   " +
           " inner join  Empattendance EA on EA.Empid=EP.Empid  " +
           " Inner Join Designations  D on D.DesignID=EP.Desgn and  EP.Clientid='" + ddlclientid.SelectedValue + "' and  EP.noofduties<>0  and EP.month='" + month + Year.Substring(2, 2) +
           "'  and  EP.month=EA.Month  and  EP.Desgn=EA.Design  and  EP.Clientid=EA.Clientid order By Right(EP.Empid,6)";

            Bindata(sqlqry);

        }


        protected void Bindata(string Sqlqry)
        {
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
                // Fillpfandesidetails();
                lbtn_Export.Visible = true;
            }
            else
            {
                LblResult.Text = "There Is No Salary Details For The Selected client";
            }
        }

        protected void Fillpfandesidetails()
        {
            string Clientid = ddlclientid.SelectedValue;
            string date = string.Empty;
            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }
            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            for (int i = 0; i < GVListEmployees.Rows.Count; i++)
            {
                string empidforempname = GVListEmployees.Rows[i].Cells[2].Text;
                string Sqlqry = "Select E.Empmname,EA.ot from Empdetails E inner join Empattendance   " +
                       "  EA on E.empid=EA.empid and   E.Empid='" + empidforempname + "' and EA.Clientid='" +
                        ddlclientid.SelectedValue + "' and month='" + month + Year.Substring(2, 2)
                        + "' and  EA.ClientId='" + ddlclientid.SelectedValue + "'";
                DataTable dtempname = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;

                if (dtempname.Rows.Count > 0)
                {

                }

                string sqlqryforemprpfesi = "select pf.emprpf,esi.empresi from emppaysheet EP inner join Epfded pf" +
                    " on pf.empid=EP.Empid  inner join Esided esi on esi.empid=EP.empid  and EP.clientid='" + ddlclientid.SelectedValue + "'";
                DataTable dtemprpfandesi = config.ExecuteAdaptorAsyncWithQueryParams(sqlqryforemprpfesi).Result;
                if (dtemprpfandesi.Rows.Count > 0)
                {
                    Label emprpf = GVListEmployees.Rows[i].FindControl("lblpfempr") as Label;
                    emprpf.Text = dtemprpfandesi.Rows[0]["emprpf"].ToString();

                    float temprpf = 0;
                    if (emprpf.Text.Trim().Length != 0)
                    {
                        temprpf = float.Parse(emprpf.Text);
                    }

                    string Epf = GVListEmployees.Rows[i].Cells[10].Text;
                    float emppf = 0;
                    if (String.IsNullOrEmpty(Epf.Trim()) != null)
                    {
                        if (Epf.Trim().Length != 0)
                        {
                            emppf = float.Parse(Epf);
                        }
                    }

                    float totalpf = temprpf + emppf;
                    Label ttotalpf = GVListEmployees.Rows[i].FindControl("lblpftotal") as Label;
                    ttotalpf.Text = totalpf.ToString();

                    Label empresi = GVListEmployees.Rows[i].FindControl("lblempresi") as Label;
                    empresi.Text = dtemprpfandesi.Rows[0]["empresi"].ToString();

                    float tempresi = 0;
                    if (empresi.Text.Trim().Length != 0)
                    {
                        tempresi = float.Parse(empresi.Text);
                    }
                    string Eesi = GVListEmployees.Rows[i].Cells[14].Text;
                    float empesi = 0;
                    if (String.IsNullOrEmpty(Eesi.Trim()) != null)
                    {
                        if (Eesi.Trim().Length != 0)
                        {
                            empesi = float.Parse(Eesi);
                        }
                    }
                    float ttotalesi = tempresi + empesi;
                    Label totalesi = GVListEmployees.Rows[i].FindControl("lblesitotal") as Label;
                    totalesi.Text = ttotalesi.ToString();
                }

                string sqlqryforesipfacno = "Select  EMPEPFCodes.EmpEpfNo,   EMPESICodes.EmpESINo, Empdetails.EmpBankAcNo   FRom Empdetails,EMPEPFCodes,EMPESICodes where   Empdetails.empid='" + empidforempname + "'";
                DataTable dtforpfesinos = config.ExecuteAdaptorAsyncWithQueryParams(sqlqryforesipfacno).Result;

                if (dtforpfesinos.Rows.Count > 0)
                {
                    Label pfno = GVListEmployees.Rows[i].FindControl("lblpfno") as Label;
                    pfno.Text = dtforpfesinos.Rows[0]["EmpEpfNo"].ToString();
                    Label esino = GVListEmployees.Rows[i].FindControl("lblpfno") as Label;
                    esino.Text = dtforpfesinos.Rows[0]["EmpESINo"].ToString();
                    Label acno = GVListEmployees.Rows[i].FindControl("lblacno") as Label;
                    acno.Text = dtforpfesinos.Rows[0]["EmpBankAcNo"].ToString();
                }
            }
        }

        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("AllUnitsEsiReport.xls", this.GVListEmployees);
        }

        protected void GVListEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVListEmployees.PageIndex = e.NewPageIndex;
            loadallsalarydetails();
        }
        float totalNoOfDuties = 0;
        float totalOTs = 0;
        float totalBasic = 0;
        float totalDA = 0;
        float totalGROSS = 0;
        float totalOTAmt = 0;
        float totalPF = 0;
        float totalESI = 0;
        float totalProfitTax = 0;
        float totalOWF = 0;
        float totalSalAdvanced = 0;
        float totalPenality = 0;
        float totalActualAmount = 0;

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            // string empno = GVListEmployees.Rows[e.Row.RowIndex].Cells[0].Text;


            //if(IsPostBack)
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{

            //    string empno = e.Row.Cells[2].Text;
            //    Label lblpfno = (Label)e.Row.FindControl("lblpfno");
            //    Label lblesino = (Label)e.Row.FindControl("lblesino");
            //    string sqlqry = " Select  EP.empepfno,ES.EmpEsino from EMPESICodes as ES,EMPEPFCodes as  EP where ES.Empid='" + empno +
            //        "' or EP.Empid='" + empno + "'";


            //    DataTable dtpfesinos = SqlHelper.Instance.GetTableByQuery(sqlqry);
            //    if (dtpfesinos.Rows.Count > 0)
            //    {
            //        lblpfno.Text = dtpfesinos.Rows[0]["empepfno"].ToString();
            //        lblesino.Text = dtpfesinos.Rows[0]["EmpEsino"].ToString();
            //    }
            //    else
            //    {
            //        lblpfno.Text = "NA";
            //        lblesino.Text = "NA";

            //    }
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //float NoOfDuties = float.Parse(((Label)e.Row.FindControl("lblNoofduties")).Text);
                //totalNoOfDuties += NoOfDuties;
                //float OTs = float.Parse(((Label)e.Row.FindControl("lblot")).Text);
                //totalOTs += OTs;
                //float basic = float.Parse(((Label)e.Row.FindControl("lblbasic")).Text);
                //totalBasic += basic;
                //float da = float.Parse(((Label)e.Row.FindControl("lblda")).Text);
                //totalDA += da;
                //float totalgross = float.Parse(((Label)e.Row.FindControl("lbltotalgross")).Text);
                //totalGROSS += totalgross;
                //float otamt = float.Parse(((Label)e.Row.FindControl("lblotamt")).Text);
                //totalOTAmt += otamt;
                //float pf = float.Parse(((Label)e.Row.FindControl("lblpf")).Text);
                //totalPF += pf;
                //float ESI = float.Parse(((Label)e.Row.FindControl("lblESI")).Text);
                //totalESI += ESI;
                //float Proftax = float.Parse(((Label)e.Row.FindControl("lblProftax")).Text);
                //totalProfitTax += Proftax;
                ////float owf = float.Parse(((Label)e.Row.FindControl("lblowf")).Text);
                ////totalOWF += owf;
                //float saladvded = float.Parse(((Label)e.Row.FindControl("lblsaladvded")).Text);
                //totalSalAdvanced += saladvded;
                //float Penalty = float.Parse(((Label)e.Row.FindControl("lblPenalty")).Text);
                //totalPenality += Penalty;
                //float actualamount = float.Parse(((Label)e.Row.FindControl("lblactualamount")).Text);
                //totalActualAmount += actualamount;


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //((Label)e.Row.FindControl("lblTotalNoofduties")).Text = totalNoOfDuties.ToString();
                //((Label)e.Row.FindControl("lblTotalots")).Text = totalOTs.ToString();
                //((Label)e.Row.FindControl("lblTotalbasic")).Text = totalBasic.ToString();
                //((Label)e.Row.FindControl("lblTotalda")).Text = totalDA.ToString();
                //((Label)e.Row.FindControl("lblTotalGross")).Text = totalGROSS.ToString();
                //((Label)e.Row.FindControl("lblTotalotamt")).Text = totalOTAmt.ToString();
                //((Label)e.Row.FindControl("lblTotalpf")).Text = totalPF.ToString();
                //((Label)e.Row.FindControl("lblTotalESI")).Text = totalESI.ToString();
                //((Label)e.Row.FindControl("lblTotalProftax")).Text = totalProfitTax.ToString();
                ////((Label)e.Row.FindControl("lblTotalowf")).Text = totalOWF.ToString();
                //((Label)e.Row.FindControl("lblTotalsaladvded")).Text = totalSalAdvanced.ToString();
                //((Label)e.Row.FindControl("lblTotalpenalty")).Text = totalPenality.ToString();
                //((Label)e.Row.FindControl("lblTotalactualamount")).Text = totalActualAmount.ToString();


                //((Label)e.Row.FindControl("lblTotalAmt")).Text = totalOTsAMT.ToString();
            }










        }
    }
}