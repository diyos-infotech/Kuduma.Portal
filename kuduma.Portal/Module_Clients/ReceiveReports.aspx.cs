using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ReceiveReports : System.Web.UI.Page
    {

        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Branchid = "";
        string UserId = "";
        string BranchID = "";
        GridViewExportUtil gve = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();

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
                    HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("c5");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                cleardata();
                LoadClientList();
                LoadClientNames();
            }




        }


        protected void LoadClientNames()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix,dtBranch);
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");
            ddlcname.Items.Insert(1, "ALL");


           

        }

        protected void LoadClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix,dtBranch);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclient.DataValueField = "Clientid";
                ddlclient.DataTextField = "Clientid";
                ddlclient.DataSource = DtClientNames;
                ddlclient.DataBind();
            }
            ddlclient.Items.Insert(0, "-Select-");
            ddlclient.Items.Insert(1, "ALL");
        }


        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Branchid = Session["CmpIDPrefix"].ToString();
            UserId = Session["UserId"].ToString();
            BranchID = Session["BranchID"].ToString();
        }

        protected void Fillcname()
        {
            string SqlQryForCname = "Select Clientid from Clients where clientid='" + ddlclient.SelectedValue + "'  and clientid like '%" + CmpIDPrefix + "%'";
            DataTable dtCname = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForCname).Result;
            if (dtCname.Rows.Count > 0)
                ddlcname.SelectedValue = dtCname.Rows[0]["Clientid"].ToString();
            else
                ddlcname.SelectedValue = ddlclient.SelectedValue;

        }

        protected void FillClientid()
        {
            string SqlQryForCid = "Select Clientid from Clients where Clientid='" + ddlcname.SelectedValue + "' and clientid like '%" + CmpIDPrefix + "%'";
            DataTable dtCname = config.ExecuteAdaptorAsyncWithQueryParams(SqlQryForCid).Result;
            if (dtCname.Rows.Count > 0)
                ddlclient.SelectedValue = dtCname.Rows[0]["Clientid"].ToString();
            else
                ddlclient.SelectedValue = ddlcname.SelectedValue;


        }

        protected void LoadDataFromUnitbill()
        {
            DataTable dt = null;
            if (txtMonth.Text.Trim().Length <= 0)
            {
                LblReslt1.Text = "Please fill the month ";
                return;
            }
            else
            {

                DateTime frmdate = DateTime.Now;

                if (txtMonth.Text.Trim().Length > 0)
                {
                    frmdate = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                }


                DateTime tdate = DateTime.Now;


                if (txtto.Text.Trim().Length > 0)
                {
                    tdate = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));

                }


                string sqlqry = string.Empty;
                string ClientId = ddlclient.SelectedValue;

                if (ddlclient.SelectedIndex == 1)
                {
                    ClientId = "%";

                }

                if (Branchid == "01/")
                {
                    CmpIDPrefix = "%";
                }
                string type = "ReceiptReport";
                var SPName = "";
                Hashtable HTPaysheet = new Hashtable();
                SPName = "ReceiptReports";
                HTPaysheet.Add("@clientid", ClientId);
                HTPaysheet.Add("@fromdate", frmdate);
                HTPaysheet.Add("@todate", tdate);
                HTPaysheet.Add("@type", type);
                HTPaysheet.Add("@CmpIDPrefix", CmpIDPrefix);
                dt = config.ExecuteAdaptorAsyncWithParams(SPName, HTPaysheet).Result;

                if (dt.Rows.Count > 0)
                {
                    gvnew.DataSource = dt;
                    gvnew.DataBind();
                }
                else
                {

                    gvnew.DataSource = null;
                    gvnew.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('There Is no Reciepts  For The Selected Client ');", true);
                    return;
                }


                decimal totalsertax = 0;
                decimal totalKKCessAmt = 0;
                decimal totalCGSTAmt = 0;
                decimal totalSGSTAmt = 0;
                decimal totalIGSTAmt = 0;
                decimal totalSBCessAmt = 0;
                decimal totalTDSAmt = 0;
                decimal totalDisallowanceAmt = 0;
                decimal totalNetInvoiceAmt = 0;
                decimal totalGrandTotal = 0;
                decimal totalTotalPaymentReceived = 0;


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string strkkcessamt = dt.Rows[i]["KKCessAmt"].ToString();
                    if (strkkcessamt.Trim().Length > 0)
                    {
                        totalKKCessAmt += Convert.ToDecimal(strkkcessamt);
                    }
                    string strCGSTamt = dt.Rows[i]["CGSTAmt"].ToString();
                    if (strCGSTamt.Trim().Length > 0)
                    {
                        totalCGSTAmt += Convert.ToDecimal(strCGSTamt);
                    }

                    string strSGSTamt = dt.Rows[i]["SGSTAmt"].ToString();
                    if (strSGSTamt.Trim().Length > 0)
                    {
                        totalSGSTAmt += Convert.ToDecimal(strSGSTamt);
                    }

                    string strIGSTamt = dt.Rows[i]["IGSTAmt"].ToString();
                    if (strIGSTamt.Trim().Length > 0)
                    {
                        totalIGSTAmt += Convert.ToDecimal(strIGSTamt);
                    }
                    string strsbcessamt = dt.Rows[i]["SBCessAmt"].ToString();
                    if (strsbcessamt.Trim().Length > 0)
                    {
                        totalSBCessAmt += Convert.ToDecimal(strsbcessamt);
                    }
                    string strTDSamt = dt.Rows[i]["TDSAmt"].ToString();
                    if (strTDSamt.Trim().Length > 0)
                    {
                        totalTDSAmt += Convert.ToDecimal(strTDSamt);
                    }
                    string strDisallowanceamt = dt.Rows[i]["Disallowance"].ToString();
                    if (strDisallowanceamt.Trim().Length > 0)
                    {
                        totalDisallowanceAmt += Convert.ToDecimal(strDisallowanceamt);
                    }
                    string strNetInvoiceamt = dt.Rows[i]["NetInvoiceAmt"].ToString();
                    if (strNetInvoiceamt.Trim().Length > 0)
                    {
                        totalNetInvoiceAmt += Convert.ToDecimal(strNetInvoiceamt);
                    }
                    string strsertax = dt.Rows[i]["ServiceTax"].ToString();
                    if (strsertax.Trim().Length > 0)
                    {
                        totalsertax += Convert.ToDecimal(strsertax);
                    }
                    string strgrandtotal = dt.Rows[i]["GrandTotal"].ToString();
                    if (strgrandtotal.Trim().Length > 0)
                    {
                        totalGrandTotal += Convert.ToDecimal(strgrandtotal);
                    }
                    string strPaymentReceived = dt.Rows[i]["PaymentReceived"].ToString();
                    if (strPaymentReceived.Trim().Length > 0)
                    {
                        totalTotalPaymentReceived += Convert.ToDecimal(strPaymentReceived);
                    }



                }


                Label lblTotalNetInvoice = gvnew.FooterRow.FindControl("lblTotalNetInvoice") as Label;
                lblTotalNetInvoice.Text = Math.Round(totalNetInvoiceAmt).ToString();

                if (totalNetInvoiceAmt > 0)
                {
                    gvnew.Columns[7].Visible = true;
                }
                else
                {
                    gvnew.Columns[7].Visible = false;

                }
                Label lblTotalsertax = gvnew.FooterRow.FindControl("lblTotalsertax") as Label;
                lblTotalsertax.Text = Math.Round(totalsertax).ToString();

                if (totalsertax > 0)
                {
                    gvnew.Columns[8].Visible = true;
                }
                else
                {
                    gvnew.Columns[8].Visible = false;

                }
                Label lblTotalSBCess = gvnew.FooterRow.FindControl("lblTotalSBCess") as Label;
                lblTotalSBCess.Text = Math.Round(totalSBCessAmt).ToString();

                if (totalSBCessAmt > 0)
                {
                    gvnew.Columns[9].Visible = true;
                }
                else
                {
                    gvnew.Columns[9].Visible = false;

                }

                Label lblTotalkkCess = gvnew.FooterRow.FindControl("lblTotalkkCess") as Label;
                lblTotalkkCess.Text = Math.Round(totalKKCessAmt).ToString();

                if (totalKKCessAmt > 0)
                {
                    gvnew.Columns[10].Visible = true;
                }
                else
                {
                    gvnew.Columns[10].Visible = false;

                }

                Label lblTotalCGST = gvnew.FooterRow.FindControl("lblTotalCGST") as Label;
                lblTotalCGST.Text = Math.Round(totalCGSTAmt).ToString();

                if (totalCGSTAmt > 0)
                {
                    gvnew.Columns[11].Visible = true;
                }
                else
                {
                    gvnew.Columns[11].Visible = false;

                }

                Label lblTotalSGST = gvnew.FooterRow.FindControl("lblTotalSGST") as Label;
                lblTotalSGST.Text = Math.Round(totalSGSTAmt).ToString();

                if (totalSGSTAmt > 0)
                {
                    gvnew.Columns[12].Visible = true;
                }
                else
                {
                    gvnew.Columns[12].Visible = false;

                }

                Label lblTotalIGST = gvnew.FooterRow.FindControl("lblTotalIGST") as Label;
                lblTotalIGST.Text = Math.Round(totalIGSTAmt).ToString();

                if (totalIGSTAmt > 0)
                {
                    gvnew.Columns[13].Visible = true;
                }
                else
                {
                    gvnew.Columns[13].Visible = false;

                }
                Label lblTotalGrandTotal = gvnew.FooterRow.FindControl("lblTotalGrandTotal") as Label;
                lblTotalGrandTotal.Text = Math.Round(totalGrandTotal).ToString();

                if (totalGrandTotal > 0)
                {
                    gvnew.Columns[14].Visible = true;
                }
                else
                {
                    gvnew.Columns[14].Visible = false;

                }
                Label lblTotalTDSAmt = gvnew.FooterRow.FindControl("lblTotalTDSAmt") as Label;
                lblTotalTDSAmt.Text = Math.Round(totalTDSAmt).ToString();

                if (totalNetInvoiceAmt > 0)
                {
                    gvnew.Columns[15].Visible = true;
                }
                else
                {
                    gvnew.Columns[15].Visible = false;

                }


                Label lblTotalPaymentReceived = gvnew.FooterRow.FindControl("lblTotalPaymentReceived") as Label;
                lblTotalPaymentReceived.Text = Math.Round(totalTotalPaymentReceived).ToString();

                if (totalTotalPaymentReceived > 0)
                {
                    gvnew.Columns[16].Visible = true;
                }
                else
                {
                    gvnew.Columns[16].Visible = false;

                }
                Label lblTotalDisallowance = gvnew.FooterRow.FindControl("lblTotalDisallowance") as Label;
                lblTotalDisallowance.Text = Math.Round(totalDisallowanceAmt).ToString();

                if (totalDisallowanceAmt > 0)
                {
                    gvnew.Columns[18].Visible = true;
                }
                else
                {
                    gvnew.Columns[18].Visible = true;

                }



            }

        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {

            gve.ExportGrid("ReceiptReport.xls", hidGridView);
        }
        protected void cleardata()
        {
            /*l1.Text = "";
            l1.Text = "";
            l1.Visible = false;
            l2.Visible = false;
            lblbills.Visible = false;
            lblreciepts.Visible = false;
            lblTotalResilt.Text = "";
            LblResult1.Text = "";
            LblResult1.Visible = true;
            lbltotalfrombills.Text = "";
            lbltotalfromreciepts.Text = "";
            lblresult2.Text = "";
            lblresult2.Visible = true;
            gvbills.DataSource = null;
            gvbills.DataBind();
            Gvreciepts.DataSource = null;
            Gvreciepts.DataBind();
            lbltotalfrombills.Visible = false;
            lbltotalfrombills.Visible = false;*/
            gvnew.DataSource = null;
            gvnew.DataBind();
            lbllabel.Visible = false;
            LblReslt1.Text = "";
        }

        protected void ddlclient_SelectedIndexChanged(object sender, EventArgs e)
        {
            cleardata();

            if (ddlclient.SelectedIndex > 0)
            {

                Fillcname();
                //LoadDataFromUnitbill();
                //LoadDataFromReceiptMaster();
                ///GetTotal();
            }
            else
            {
                cleardata();
                ddlcname.SelectedIndex = 0;
            }

        }


        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddlcname.SelectedIndex > 0)
            {
                FillClientid();
            }
            else
            {
                cleardata();
                ddlclient.SelectedIndex = 0;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            cleardata();
            lbtn_Export.Visible = true;
            if (ddlclient.SelectedIndex > 0)
            {
                //Fillcname();
                LoadDataFromUnitbill();
            }
            else
            {
                LblReslt1.Text = "Please Select Client Id";
                txtMonth.Text = "";
                return;
            }

        }
        protected void gvnew_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");
                e.Row.Cells[2].Attributes.Add("class", "text");

            }
        }
    }
}