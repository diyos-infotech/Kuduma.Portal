using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KLTS.Data;
using Kuduma.Portal.DAL;
namespace Kuduma.Portal
{
    public partial class ContractDetailsReports : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Elength = "";
        string Clength = "";
        string BranchID = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
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
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli2");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                ddlClientID.Items.Add("--Select--");
                ddlcname.Items.Add("--Select--");
                FillClientList();
                FillClientNameList();
            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void FillClientList()
        {
            DataTable dt;
            string selectclientid = "select clientid from clients where clientid like '%" + CmpIDPrefix + "%'  Order By Right(clientid, 4)";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectclientid).Result;
            int rowno1 = 0;

            for (rowno1 = 0; rowno1 < dt.Rows.Count; rowno1++)
            {
                ddlClientID.Items.Add(dt.Rows[rowno1]["clientid"].ToString());
            }
        }

        protected void FillClientNameList()
        {
            DataTable dt;
            string selectclientid = "select clientname from clients where clientid like '%" + CmpIDPrefix + "%'  and clientstatus=1 order by Clientname";
            dt = config.ExecuteAdaptorAsyncWithQueryParams(selectclientid).Result;
            int rowno1 = 0;
            for (rowno1 = 0; rowno1 < dt.Rows.Count; rowno1++)
            {
                ddlcname.Items.Add(dt.Rows[rowno1]["clientname"].ToString());
            }
        }
        protected void ClearData()
        {
            txtContractID.Text = "";
            txtDays.Text = "";
            txtMachinaryCost.Text = "";
            txtMaterialCost.Text = "";
            txtOTpercent.Text = "";
            txtServiceCharge.Text = "";
            LblResult.Text = "";

            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LblResult.Visible = true;
            if (ddlClientID.SelectedIndex <= 0)
            {
                LblResult.Text = "Plese Select Client ID";
                return;
            }

            ClearData();
            DataTable DtContract = null;

            //string SqlQryForGetEmployyes = "Select Distinct c.ContractId,c.MaterialCostPerMonth,c.MachinaryCostPerMonth,c.NoOfDays,c.ServiceCharge,c.OTPersent," +
            //    "cd.Designations,cd.Quantity,cd.Basic,cd.DA,cd.HRA,cd.CCA,cd.Conveyance,cd.WashAllownce,cd.OtherAllowance,cd.Amount,cd.LeaveAmount,cd.Bonus," +
            //    "cd.Gratuity,cd.PF,cd.ESI,cd.ESIFrom,cd.PfFrom from Contracts as c INNER JOIN ContractDetails as cd ON c.ContractId=cd.ContractId AND c.ClientId='" +
            //    ddlClientID.SelectedValue + "'";

            //string SqlQryForGetEmployyes = "Select  ContractId,NoOfDaysWages,NoOfDays,"+
            //    " MachinaryCostPerMonth,MaterialCostPerMonth,OTPersent From Contracts Where Clientid='"+ddlClientID.SelectedValue+"'";


            string SqlQryForWages = "Select WageType  From  Contracts Where  Clientid='" + ddlClientID.SelectedValue + "'";
            DataTable DtForWages = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForWages).Result;
            UInt16 WagesType;
            string SqlQryForGetEmployyes = string.Empty;
            if (DtForWages.Rows.Count > 0)
            {
                string tempData = DtForWages.Rows[0]["WageType"].ToString();
                if (tempData.Trim().Length > 0)
                {
                    //WagesType = bool.Parse(tempData);
                    WagesType = Convert.ToUInt16(tempData);

                    if (WagesType == 1)
                    {

                        SqlQryForGetEmployyes = "Select Distinct c.ContractId,CL.Clientid,CL.Clientname,c.MaterialCostPerMonth,c.MachinaryCostPerMonth, " +
                                                        "c.NoOfDays,c.ServiceCharge,c.OTPersent," +
                                                        "d.Design,cd.Quantity,cd.Basic,cd.DA,cd.HRA,cd.CCA,cd.Conveyance,c.NoOfDaysWages," +
                                                        " cd.WashAllownce  as WashAllowance,cd.OtherAllowance,cd.Amount,cd.LeaveAmount,cd.Bonus," +
                                                        "cd.Gratuity,cd.PF,cd.ESI,cd.ESIFrom,cd.PfFrom from Contracts as c INNER JOIN " +
                                                      " ContractDetails as cd ON c.ContractId=cd.ContractId inner join Designations d on cd.Designations=d.DesignId " +
                                                      "  inner join Clients CL on CL.Clientid=c.ClientId AND c.ClientId='" +
                                                       ddlClientID.SelectedValue + "'";
                    }
                    if (WagesType == 2)
                    {

                        SqlQryForGetEmployyes = "Select Distinct c.ContractId,CL.Clientid,CL.Clientname,c.MaterialCostPerMonth,c.MachinaryCostPerMonth, " +
                                                                       "c.NoOfDays,c.ServiceCharge,c.OTPersent," +
                                                                       "d.Design,cd.Quantity,cd.Basic,cd.DA,cd.HRA,cd.CCA,cd.Conveyance,c.NoOfDaysWages," +
                                                                       " cd.WashAllowance,cd.OtherAllowance,cd.Amount,cd.LeaveAmount,cd.Bonus," +
                                                                       "cd.Gratuity,cd.PF,cd.ESI,cd.ESIFrom,cd.PfFrom from Contracts as c INNER JOIN " +
                                                                     " ContractDetailsSW as cd ON c.ContractId=cd.ContractId inner join Designations d on cd.Designations=d.DesignId " +
                                                                     "  inner join Clients CL on CL.Clientid=c.ClientId AND c.ClientId='" +
                                                                      ddlClientID.SelectedValue + "'";

                    }


                }

                DtContract = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForGetEmployyes).Result;
            }




            if (DtContract.Rows.Count > 0)
            {
                txtContractID.Text = DtContract.Rows[0]["ContractId"].ToString();
                txtDays.Text = DtContract.Rows[0]["NoOfDays"].ToString();
                txtMachinaryCost.Text = DtContract.Rows[0]["MachinaryCostPerMonth"].ToString();
                txtMaterialCost.Text = DtContract.Rows[0]["MaterialCostPerMonth"].ToString();
                txtOTpercent.Text = DtContract.Rows[0]["OTPersent"].ToString();
                GVListEmployees.DataSource = DtContract;
                GVListEmployees.DataBind();
            }
            else
            {
                // LblResult.Text =  " There is no Contracts for this Client";

            }

        }
        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlClientID.SelectedIndex > 0)
            {
                string sqlQry = "Select Clientname from Clients where clientid='" + ddlClientID.SelectedValue + "'";
                DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
                if (data.Rows.Count > 0)
                {
                    ddlcname.SelectedValue = data.Rows[0]["clientname"].ToString();
                }
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }


        }



        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcname.SelectedIndex > 0)
            {
                string sqlQry = "Select clientid  from Clients where  Clientname='" + ddlcname.SelectedValue + "'";
                DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;
                if (data.Rows.Count > 0)
                {
                    ddlClientID.SelectedValue = data.Rows[0]["clientid"].ToString();
                }
            }
            else
            {
                ddlClientID.SelectedIndex = 0;
            }
        }
        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            gve.Export("Contract Details Report.xls", this.GVListEmployees);
        }

        string totalNoofBiilingDays = string.Empty;
        string totalNoofWagesDays = string.Empty;
        string totalQuantity = string.Empty;
        float totalBasic = 0;
        float totalDA = 0;
        float totalHRA = 0;
        float totalCCA = 0;
        float totalConveyance = 0;
        float totalWashAllowance = 0;
        float totalOtheeAllowance = 0;
        string totalAmount = "";
        float totalLeaveAmount = 0;
        float totalBonus = 0;
        float totalGratuity = 0;
        float totalPF = 0;
        float totalESI = 0;
        string totalPFon = "";
        string totalESIon = "";
        float totalOTPresent = 0;
        float totalMC = 0;
        float totalMachinaryCost = 0;
        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string NoofBiilingDays = ((Label)e.Row.FindControl("lblNoofDays")).Text.ToString();

                if (NoofBiilingDays != null)
                {
                    totalNoofBiilingDays = string.Empty;
                }
                else
                {
                    totalNoofBiilingDays += NoofBiilingDays;
                }
                string NoofWagesDays = ((Label)e.Row.FindControl("lblNoOfDaysWages")).Text.ToString();

                if (NoofWagesDays != null)
                {
                    totalNoofWagesDays = string.Empty;
                }
                else
                {
                    totalNoofWagesDays += NoofWagesDays;
                }

                string Quantity = ((Label)e.Row.FindControl("lblQuantity")).Text.ToString();
                totalQuantity += Quantity;
                float Basic = float.Parse(((Label)e.Row.FindControl("lblBasic")).Text);
                totalBasic += Basic;
                float DA = float.Parse(((Label)e.Row.FindControl("lblDA")).Text);
                totalDA += DA;
                float HRA = float.Parse(((Label)e.Row.FindControl("lblHRA")).Text);
                totalHRA += HRA;
                float CCA = float.Parse(((Label)e.Row.FindControl("lblCCA")).Text);
                totalCCA += CCA;
                float Conveyance = float.Parse(((Label)e.Row.FindControl("lblConveyance")).Text);
                totalConveyance += Conveyance;
                float WashAllowance = float.Parse(((Label)e.Row.FindControl("lblWashAllowance")).Text);
                totalWashAllowance += WashAllowance;
                float OtheeAllowance = float.Parse(((Label)e.Row.FindControl("lblOtherAllowance")).Text);
                totalOtheeAllowance += OtheeAllowance;
                string Amount = ((Label)e.Row.FindControl("lblAmount")).Text.ToString();
                totalAmount += Amount;
                float LeaveAmount = float.Parse(((Label)e.Row.FindControl("lblLeaveAmount")).Text);
                totalLeaveAmount += LeaveAmount;
                float Bonus = float.Parse(((Label)e.Row.FindControl("lblBonus")).Text);
                totalBonus += Bonus;
                float Gratuity = float.Parse(((Label)e.Row.FindControl("lblGratuity")).Text);
                totalGratuity += Gratuity;
                float PF = float.Parse(((Label)e.Row.FindControl("lblPF")).Text);
                totalPF += PF;
                float ESI = float.Parse(((Label)e.Row.FindControl("lblESI")).Text);
                totalESI += ESI;

                float OTPresent = float.Parse(((Label)e.Row.FindControl("lblotpercent")).Text);
                totalOTPresent += OTPresent;
                float MC = float.Parse(((Label)e.Row.FindControl("lblmaterialcost")).Text);
                totalMC += MC;
                float MachinaryCost = float.Parse(((Label)e.Row.FindControl("lblmachinarycost")).Text);
                totalMachinaryCost += MachinaryCost;


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalBiilingDays")).Text = totalNoofBiilingDays.ToString();
                ((Label)e.Row.FindControl("lblTotalWagesDays")).Text = totalNoofWagesDays.ToString();
                ((Label)e.Row.FindControl("lblTotalQuantity")).Text = totalQuantity.ToString();
                ((Label)e.Row.FindControl("lblTotalBasic")).Text = totalBasic.ToString();
                ((Label)e.Row.FindControl("lblTotalDA")).Text = totalDA.ToString();
                ((Label)e.Row.FindControl("lblTotalHRA")).Text = totalHRA.ToString();
                ((Label)e.Row.FindControl("lblTotalCCA")).Text = totalCCA.ToString();
                ((Label)e.Row.FindControl("lblTotalConveyance")).Text = totalConveyance.ToString();
                ((Label)e.Row.FindControl("lblTotalWashallowance")).Text = totalWashAllowance.ToString();
                ((Label)e.Row.FindControl("lblTotalOtheallowance")).Text = totalOtheeAllowance.ToString();
                ((Label)e.Row.FindControl("lblTotalAmount")).Text = totalAmount.ToString();
                ((Label)e.Row.FindControl("lblTotalLeaveAmount")).Text = totalLeaveAmount.ToString();
                ((Label)e.Row.FindControl("lblTotalBomus")).Text = totalBonus.ToString();
                ((Label)e.Row.FindControl("lblTotaGratuity")).Text = totalGratuity.ToString();
                ((Label)e.Row.FindControl("lblTotalPF")).Text = totalPF.ToString();
                ((Label)e.Row.FindControl("lblTotalESIon")).Text = totalESIon.ToString();
                ((Label)e.Row.FindControl("lblTotalOTPresent")).Text = totalOTPresent.ToString();
                ((Label)e.Row.FindControl("lblTotalMC")).Text = totalMC.ToString();
                ((Label)e.Row.FindControl("lblTotalMachinaryCost")).Text = totalMachinaryCost.ToString();

            }
        }
    }
}