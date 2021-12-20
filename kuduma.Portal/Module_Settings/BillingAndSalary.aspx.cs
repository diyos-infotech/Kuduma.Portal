using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class BillingAndSalary : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
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
                    BindIDs();
                    Displaydata();
                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }
        }

        public void Displaydata()
        {
            try
            {
                DataTable DtProfTax = GlobalData.Instance.LoadProfTaxData();
                DataTable DtBillingSalary = GlobalData.Instance.LoadTblOptionsData(ddlID.SelectedValue);

                if (DtProfTax.Rows.Count == 0 && DtBillingSalary.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Prof Tax &  Billing/Salary Details are not available.');", true);

                    return;
                }

                if (DtProfTax.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Prof Tax Details are not available.');", true);

                    return;
                }

                if (DtBillingSalary.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Billing/Salary Details are not available.');", true);

                    return;
                }


                if (DtProfTax.Rows.Count > 0)
                {
                    gvservicetax.DataSource = DtProfTax;
                    gvservicetax.DataBind();
                }

                Txt_ServiceTax.Text = DtBillingSalary.Rows[0]["servicetax"].ToString();
                Txt_Service_Tax_Saparate.Text = DtBillingSalary.Rows[0]["servicetaxseparate"].ToString();
                Txt_CESS.Text = DtBillingSalary.Rows[0]["cess"].ToString();
                Txt_She_Cess.Text = DtBillingSalary.Rows[0]["shecess"].ToString();
                Txt_Pf_Employee.Text = DtBillingSalary.Rows[0]["pfemployee"].ToString();
                Txt_SB_CESS.Text = DtBillingSalary.Rows[0]["SBCess"].ToString();
                Txt_KK_CESS.Text = DtBillingSalary.Rows[0]["KKCess"].ToString();
                Txt_ESI_Employee.Text = DtBillingSalary.Rows[0]["esiemployee"].ToString();
                Txt_Pf_Employeer.Text = DtBillingSalary.Rows[0]["pfemployer"].ToString();
                Txt_Pf_Employee_AdminCharge.Text = DtBillingSalary.Rows[0]["PFEmployer_AdminChrg"].ToString();
                Txt_Esi_Employeer.Text = DtBillingSalary.Rows[0]["esiemployer"].ToString();
                Txt_NH_Days.Text = DtBillingSalary.Rows[0]["nhdays"].ToString();
                TxtFromDate.Text = DtBillingSalary.Rows[0]["FromDate"].ToString();
                TxtToDate.Text = DtBillingSalary.Rows[0]["ToDate"].ToString();
                Txt_Bonus.Text = DtBillingSalary.Rows[0]["bonus"].ToString();
                Txt_Employee_Pension.Text = DtBillingSalary.Rows[0]["PFEmployerPension"].ToString();

                #region for GST as on 16-6-2017 by swathi

                Txt_CGST.Text = DtBillingSalary.Rows[0]["CGST"].ToString();
                Txt_SGST.Text = DtBillingSalary.Rows[0]["SGST"].ToString();
                Txt_IGST.Text = DtBillingSalary.Rows[0]["IGST"].ToString();
                Txt_CESS1.Text = DtBillingSalary.Rows[0]["CESS1"].ToString();
                Txt_CESS2.Text = DtBillingSalary.Rows[0]["CESS2"].ToString();

                #endregion for GST as on 16-6-2017 by swathi
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }
        }




        public void BindIDs()
        {
            string Qry = "select ID from tbloptions order by id desc";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlID.DataValueField = "ID";
                ddlID.DataTextField = "ID";
                ddlID.DataSource = dt;
                ddlID.DataBind();

            }

        }



        protected void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {

                #region   Begin code  For Variable Declaration as on[14-10-213]
                var ServiceTax = "0";
                var ServiceTaxSeparate = "0";
                var Cess = "0";
                var SheCess = "0";
                var PfEmployee = "0";


                var PfEmployer = "0";
                var PFEmployer_AdminChrg = "0";
                var EsiEmployee = "0";
                var EsiEmployer = "0";
                var Nhdays = "0";

                var Bonus = "0";
                var PFEmployerPension = "0";
                var IRecordStatus = 0;

                //new column SBCess on 18-11-2015
                var SBCess = "0";
                var KKCess = "0";
                //end new column SBCess on 18-11-2015
                var StartDate = "01/01/1900";
                var ToDate = "01/01/1900";

                var CGST = "0";
                var SGST = "0";
                var IGST = "0";
                var CESS1 = "0";
                var CESS2 = "0";

                StartDate = Timings.Instance.CheckDateFormat(TxtFromDate.Text);
                ToDate = Timings.Instance.CheckDateFormat(TxtToDate.Text);


                #endregion End  code  For Variable Declaration as on[14-10-213]

                #region   Begin code  For Assign Values To the  Variable  as on[14-10-213]

                if (Txt_ServiceTax.Text.Trim().Length > 0)
                {
                    ServiceTax = Txt_ServiceTax.Text;
                }

                if (Txt_Service_Tax_Saparate.Text.Trim().Length > 0)
                {
                    ServiceTaxSeparate = Txt_Service_Tax_Saparate.Text;
                }
                if (Txt_SB_CESS.Text.Trim().Length > 0)
                {
                    SBCess = Txt_SB_CESS.Text;
                }

                if (Txt_KK_CESS.Text.Trim().Length > 0)
                {
                    KKCess = Txt_KK_CESS.Text;
                }

                #region for GST on 16-6-2017

                if (Txt_CGST.Text.Trim().Length > 0)
                {
                    CGST = Txt_CGST.Text;
                }

                if (Txt_SGST.Text.Trim().Length > 0)
                {
                    SGST = Txt_SGST.Text;
                }

                if (Txt_IGST.Text.Trim().Length > 0)
                {
                    IGST = Txt_IGST.Text;
                }

                if (Txt_CESS1.Text.Trim().Length > 0)
                {
                    CESS1 = Txt_CESS1.Text;
                }

                if (Txt_CESS2.Text.Trim().Length > 0)
                {
                    CESS2 = Txt_CESS2.Text;
                }

                #endregion for GST on 16-6-2017

                if (Txt_CESS.Text.Trim().Length > 0)
                {
                    Cess = Txt_CESS.Text;
                }

                if (Txt_She_Cess.Text.Trim().Length > 0)
                {
                    SheCess = Txt_She_Cess.Text;
                }


                if (Txt_Pf_Employee.Text.Trim().Length > 0)
                {
                    PfEmployee = Txt_Pf_Employee.Text;
                }

                if (Txt_Pf_Employeer.Text.Trim().Length > 0)
                {
                    PfEmployer = Txt_Pf_Employeer.Text;
                }

                if (Txt_Pf_Employee_AdminCharge.Text.Trim().Length > 0)
                {
                    PFEmployer_AdminChrg = Txt_Pf_Employee_AdminCharge.Text;
                }

                if (Txt_ESI_Employee.Text.Trim().Length > 0)
                {
                    EsiEmployee = Txt_ESI_Employee.Text;
                }

                if (Txt_Esi_Employeer.Text.Trim().Length > 0)
                {
                    EsiEmployer = Txt_Esi_Employeer.Text;
                }

                if (Txt_NH_Days.Text.Trim().Length > 0)
                {
                    Nhdays = Txt_NH_Days.Text;
                }

                if (Txt_Bonus.Text.Trim().Length > 0)
                {
                    Bonus = Txt_Bonus.Text;
                }
                if (Txt_Employee_Pension.Text.Trim().Length > 0)
                {
                    PFEmployerPension = Txt_Employee_Pension.Text;
                }

                #endregion End  code  For  Assign Values To the  Variable  as on[14-10-213]

                #region  Begin Code For Assign Values  to SP Parameters

                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "ModifyTblOptions";

                HtSPParameters.Add("@servicetax", ServiceTax);
                HtSPParameters.Add("@servicetaxseparate", ServiceTaxSeparate);
                HtSPParameters.Add("@cess", Cess);
                HtSPParameters.Add("@shecess", SheCess);
                HtSPParameters.Add("@pfemployee", PfEmployee);

                HtSPParameters.Add("@pfemployer", PfEmployer);
                HtSPParameters.Add("@PFEmployer_AdminChrg", PFEmployer_AdminChrg);
                HtSPParameters.Add("@esiemployee", EsiEmployee);
                HtSPParameters.Add("@nhdays", Nhdays);
                HtSPParameters.Add("@bonus", Bonus);

                HtSPParameters.Add("@PFEmployerPension", PFEmployerPension);
                HtSPParameters.Add("@esiemployer", EsiEmployer);
                HtSPParameters.Add("@sbcess", SBCess);
                HtSPParameters.Add("@KKCess", KKCess);
                HtSPParameters.Add("@ID", ddlID.SelectedValue);
                HtSPParameters.Add("@FromDate", StartDate);
                HtSPParameters.Add("@ToDate", ToDate);
                HtSPParameters.Add("@CGST", CGST);
                HtSPParameters.Add("@SGST", SGST);
                HtSPParameters.Add("@IGST", IGST);
                HtSPParameters.Add("@CESS1", CESS1);
                HtSPParameters.Add("@CESS2", CESS2);


                #endregion Begin Code For Assign Values  to SP Parameters

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus =config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Billing/Salary Details Updated SucessFully.');", true);
                    ddlID.SelectedIndex = 0;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Billing/Salary Details Not Updated');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [12-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [12-10-2013]
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [12-10-2013]


            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }
        }


        protected void Btn_Fy_Add(object sender, EventArgs e)
        {

        }



        protected void ddlID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Displaydata();
        }
    


    }
}