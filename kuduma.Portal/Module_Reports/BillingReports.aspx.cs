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
    public partial class BillingReports : System.Web.UI.Page
    {
        string CmpIDPrefix = "";
        string BranchID = "";
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
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
                        HtmlControl emplink = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli2");
                        if (emplink != null)
                        {
                            emplink.Attributes["class"] = "current";
                        }
                    }
                    FillClientList();
                    FillClientNameList();
                    LoadOurGSTNos();
                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }
        private void LoadOurGSTNos()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtGSTNos = GlobalData.Instance.LoadGSTNumbers(dtBranch);
            if (DtGSTNos.Rows.Count > 0)
            {
                ddlOurGSTIN.DataValueField = "Id";
                ddlOurGSTIN.DataTextField = "GSTNo";
                ddlOurGSTIN.DataSource = DtGSTNos;
                ddlOurGSTIN.DataBind();
            }

            ddlOurGSTIN.Items.Insert(0, new ListItem("All", "-1"));

        }
        protected void Btn_Search_Invoice_Btn_Dates_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            DataTable DtNull = null;
            GVInvoiceBills.DataSource = DtNull;
            GVInvoiceBills.DataBind();
            Hashtable HtGetInvoiceAlldata = new Hashtable();

            string SPName = "GetInvoiceDataBetweenDates";

            if (ddlClientId.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Please Select ClientId');", true);
                return;
            }
            else
            {
                var FromDate = DateTime.Now;
                var ToDate = DateTime.Now;
                string month = "";
                string Year = "";

                if (ddlPeriod.SelectedIndex == 0)
                {
                    if (Txt_From_Date.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the From Date');", true);
                        return;
                    }
                    if (Txt_To_Date.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the To Date');", true);
                        return;
                    }

                    FromDate = DateTime.Parse(Txt_From_Date.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));
                    ToDate = DateTime.Parse(Txt_To_Date.Text.Trim(), CultureInfo.GetCultureInfo("en-GB"));

                }
                else
                {
                    if (txtEndDate.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Fill the Month');", true);
                        return;
                    }


                    month = DateTime.Parse(txtEndDate.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Month.ToString();
                    Year = DateTime.Parse(txtEndDate.Text.Trim(), CultureInfo.GetCultureInfo("en-GB")).Year.ToString().Substring(2, 2);

                }




                string SelectQueryByDate = string.Empty;
                string clientid = ddlClientId.SelectedValue;
                string GSTINUIN = ddlOurGSTIN.SelectedItem.ToString();

                if (ddlOurGSTIN.SelectedIndex == 0)
                {
                    GSTINUIN = "%";
                }

                if (ddlClientId.SelectedIndex == 1)
                {
                    clientid = "%";

                }

                int billtype = ddlbilltype.SelectedIndex;

                int period = 0;

                if (ddlPeriod.SelectedIndex == 1)
                {
                    period = 1;
                }



                HtGetInvoiceAlldata.Add("@FromDate", FromDate);
                HtGetInvoiceAlldata.Add("@ToDate", ToDate);
                HtGetInvoiceAlldata.Add("@ClientId", clientid);
                HtGetInvoiceAlldata.Add("@BillType", billtype);
                HtGetInvoiceAlldata.Add("@Period", period);
                HtGetInvoiceAlldata.Add("@month", month + Year);
                HtGetInvoiceAlldata.Add("@ClientIDPrefix", CmpIDPrefix);
                HtGetInvoiceAlldata.Add("@GSTINUIN", GSTINUIN);

                DataTable DtSqlData = config.ExecuteAdaptorAsyncWithParams(SPName, HtGetInvoiceAlldata).Result;


                float totalservicetax = 0;
                float totalSBCessAmt = 0;
                float totalKKCessAmt = 0;
                float totalCGSTAmt = 0;
                float totalSGSTAmt = 0;
                float totalIGSTAmt = 0;
                float totalGrandTotal = 0;
                float totalDiscountAmount = 0;

                if (DtSqlData.Rows.Count > 0)
                {
                    GVInvoiceBills.DataSource = DtSqlData;
                    GVInvoiceBills.DataBind();

                    lbtn_Export.Visible = true;


                    for (int i = 0; i < DtSqlData.Rows.Count; i++)
                    {
                        string servicetax = DtSqlData.Rows[i]["ServiceTax"].ToString();
                        if (servicetax.Trim().Length > 0)
                        {
                            totalservicetax += Convert.ToSingle(servicetax);
                        }

                        string sbcess = DtSqlData.Rows[i]["SBCessAmt"].ToString();
                        if (sbcess.Trim().Length > 0)
                        {
                            totalSBCessAmt += Convert.ToSingle(sbcess);
                        }

                        string kkcess = DtSqlData.Rows[i]["KKCessAmt"].ToString();
                        if (kkcess.Trim().Length > 0)
                        {
                            totalKKCessAmt += Convert.ToSingle(kkcess);
                        }

                        string Discount = DtSqlData.Rows[i]["Discount"].ToString();
                        if (Discount.Trim().Length > 0)
                        {
                            totalDiscountAmount += Convert.ToSingle(Discount);
                        }

                        string CGST = DtSqlData.Rows[i]["CGSTAmt"].ToString();
                        if (CGST.Trim().Length > 0)
                        {
                            totalCGSTAmt += Convert.ToSingle(CGST);
                        }

                        string SGST = DtSqlData.Rows[i]["SGSTAmt"].ToString();
                        if (SGST.Trim().Length > 0)
                        {
                            totalSGSTAmt += Convert.ToSingle(SGST);
                        }

                        string IGST = DtSqlData.Rows[i]["IGSTAmt"].ToString();
                        if (IGST.Trim().Length > 0)
                        {
                            totalIGSTAmt += Convert.ToSingle(IGST);
                        }

                        string grantotal = DtSqlData.Rows[i]["GrandTotal"].ToString();
                        if (grantotal.Trim().Length > 0)
                        {
                            totalGrandTotal += Convert.ToSingle(grantotal);
                        }


                        Label lblTotalServiceTax = GVInvoiceBills.FooterRow.FindControl("lblTotalServiceTax") as Label;
                        lblTotalServiceTax.Text = Math.Round(totalservicetax).ToString("0.00");

                        if (totalservicetax > 0)
                        {
                            GVInvoiceBills.Columns[12].Visible = true;
                        }
                        else
                        {
                            GVInvoiceBills.Columns[12].Visible = false;
                        }

                        Label lblTotalSBCessAmt = GVInvoiceBills.FooterRow.FindControl("lblTotalSBCessAmt") as Label;
                        lblTotalSBCessAmt.Text = Math.Round(totalSBCessAmt).ToString("0.00");

                        if (totalSBCessAmt > 0)
                        {
                            GVInvoiceBills.Columns[13].Visible = true;
                        }
                        else
                        {
                            GVInvoiceBills.Columns[13].Visible = false;
                        }

                        Label lblTotalKKCessAmt = GVInvoiceBills.FooterRow.FindControl("lblTotalKKCessAmt") as Label;
                        lblTotalKKCessAmt.Text = Math.Round(totalKKCessAmt).ToString("0.00");

                        if (totalKKCessAmt > 0)
                        {
                            GVInvoiceBills.Columns[14].Visible = true;
                        }
                        else
                        {
                            GVInvoiceBills.Columns[14].Visible = false;
                        }

                        Label lblTotalDiscounts = GVInvoiceBills.FooterRow.FindControl("lblTotalDiscounts") as Label;
                        lblTotalDiscounts.Text = Math.Round(totalDiscountAmount).ToString("0.00");

                        if (totalDiscountAmount > 0)
                        {
                            GVInvoiceBills.Columns[15].Visible = true;
                        }
                        else
                        {
                            GVInvoiceBills.Columns[15].Visible = false;
                        }

                        Label lblTotalCGSTAmt = GVInvoiceBills.FooterRow.FindControl("lblTotalCGSTAmt") as Label;
                        lblTotalCGSTAmt.Text = Math.Round(totalCGSTAmt).ToString("0.00");


                        if (totalCGSTAmt > 0)
                        {
                            GVInvoiceBills.Columns[16].Visible = true;
                        }
                        else
                        {
                            GVInvoiceBills.Columns[16].Visible = false;
                        }


                        Label lblTotalSGSTAmt = GVInvoiceBills.FooterRow.FindControl("lblTotalSGSTAmt") as Label;
                        lblTotalSGSTAmt.Text = Math.Round(totalSGSTAmt).ToString("0.00");


                        if (totalSGSTAmt > 0)
                        {
                            GVInvoiceBills.Columns[17].Visible = true;
                        }
                        else
                        {
                            GVInvoiceBills.Columns[17].Visible = false;
                        }


                        Label lblTotalIGSTAmt = GVInvoiceBills.FooterRow.FindControl("lblTotalIGSTAmt") as Label;
                        lblTotalIGSTAmt.Text = Math.Round(totalIGSTAmt).ToString("0.00");

                        if (totalIGSTAmt > 0)
                        {
                            GVInvoiceBills.Columns[18].Visible = true;
                        }
                        else
                        {
                            GVInvoiceBills.Columns[18].Visible = false;
                        }



                        Label lblTotalGrandTotal = GVInvoiceBills.FooterRow.FindControl("lblTotalGrandTotal") as Label;
                        lblTotalGrandTotal.Text = Math.Round(totalGrandTotal).ToString("0.00");

                        if (totalGrandTotal > 0)
                        {
                            GVInvoiceBills.Columns[19].Visible = true;
                        }
                        else
                        {
                            GVInvoiceBills.Columns[19].Visible = false;
                        }



                    }

                }
                else
                {
                    GVInvoiceBills.DataSource = null;
                    GVInvoiceBills.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('There is no data for selected client');", true);
                    return;
                }
            }

        }
        private void BindData(string SqlQury)
        {
            ClearAmountdetails();
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQury).Result;
            if (dt.Rows.Count > 0)
            {
                lbltamttext.Visible = true;
                if (ddlbilltype.SelectedIndex > 0)
                {
                    GVInvoiceBills.DataSource = dt;
                    GVInvoiceBills.DataBind();
                }
            }
            else
            {
                LblResult.Text = " There is No bills Between The Selected Dates ";
            }
        }
        protected void ClearAmountdetails()
        {
            lbltamttext.Visible = false;
            lbltmtinvoice.Text = "";
        }
        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                ddlClientId.SelectedValue = ddlcname.SelectedValue;
            }
            else
            {

                ddlClientId.SelectedIndex = 0;
            }

        }
        protected void ddlClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlClientId.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlClientId.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }
        protected void FillClientList()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);

            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix, dtBranch);
            if (dt.Rows.Count > 0)
            {
                ddlClientId.DataValueField = "clientid";
                ddlClientId.DataTextField = "clientid";
                ddlClientId.DataSource = dt;
                ddlClientId.DataBind();
            }
            ddlClientId.Items.Insert(0, "--Select--");
            ddlClientId.Items.Insert(1, "ALL");


        }
        protected void FillClientNameList()
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
            ddlcname.Items.Insert(0, "--Select--");
            ddlcname.Items.Insert(1, "ALL");


        }
        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();

        }
        protected void lbtn_Export_Click(object sender, EventArgs e)
        {

            gve.ExportGrid("InvoiceBills.xls", hidGridView);

        }
        protected void ClearData()
        {
            GVInvoiceBills.DataSource = null;
            GVInvoiceBills.DataBind();
        }

        decimal total = 0, ServiceChrg = 0, others = 0;
        protected void GVInvoiceBills_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                total += decimal.Parse(e.Row.Cells[9].Text);
                others += decimal.Parse(e.Row.Cells[10].Text);
                ServiceChrg += decimal.Parse(e.Row.Cells[11].Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[9].Text = total.ToString();
                e.Row.Cells[10].Text = others.ToString();
                e.Row.Cells[11].Text = ServiceChrg.ToString();
            }
        }
        protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVInvoiceBills.DataSource = null;
            GVInvoiceBills.DataBind();

            if (ddlPeriod.SelectedIndex == 1)
            {
                lblmonth.Visible = true;
                txtEndDate.Visible = true;
                Txt_From_Date.Visible = false;
                Txt_To_Date.Visible = false;
                lblfromdate.Visible = false;
                lbltodate.Visible = false;
            }
            else
            {
                lblmonth.Visible = false;
                txtEndDate.Visible = false;
                Txt_From_Date.Visible = true;
                Txt_To_Date.Visible = true;
                lblfromdate.Visible = true;
                lbltodate.Visible = true;
            }
        }

    }
}