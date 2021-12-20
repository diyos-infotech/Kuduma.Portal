using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class BankNames : System.Web.UI.Page
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

                    Displaydata();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void Displaydata()
        {
            Txt_Bank_Name.Text = "";
            DataTable DtBankNames = GlobalData.Instance.LoadBankNames();
            if (DtBankNames.Rows.Count > 0)
            {
                gvbank.DataSource = DtBankNames;
                gvbank.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Bank Names Are Not Avialable');", true);
                return;
            }

        }


        

        protected void Btn_Bank_Name_Add_Click(object sender, EventArgs e)
        {
            try
            {

                #region Begin Code For  Validations   as [12-10-2013]
                if (Txt_Bank_Name.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Bank Name.');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var BankName = string.Empty;
                var IRecordStatus = 0;

                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                BankName = Txt_Bank_Name.Text.Trim().ToUpper();

                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]

                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AddBanknames";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@Bankname", BankName);

                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Bank Name Added SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Bank Name Not  Added.Because  The Name Already Exist. NOTE:Bank Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [12-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [12-10-2013]
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [12-10-2013]

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Contact Your Admin..');", true);
                return;
            }




        }

        protected void gvbank_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {


            try
            {

                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label bankid = gvbank.Rows[e.RowIndex].FindControl("lblbankid") as Label;
                TextBox bankname = gvbank.Rows[e.RowIndex].FindControl("txtbankName") as TextBox;
                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (bankname.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Banks  Name');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]


                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var BankName = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                BankName = bankname.Text.Trim().ToUpper();
                ProcedureName = "ModifyBanknames";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@Bankname", BankName);
                HtSPParameters.Add("@BankId", bankid.Text);
                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus =config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Bank Name  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Bank Name Not  Updated.Because  The Name Already Exist. NOTE:Bank Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [14-10-2013]
                gvbank.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [14-10-2013]

            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }




        }

        protected void gvbank_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            gvbank.EditIndex = e.NewEditIndex;
            Displaydata();
        }

        protected void gvbank_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvbank.EditIndex = -1;
            Displaydata();
        }

        protected void gvbank_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvbank.PageIndex = e.NewPageIndex;
            Displaydata();
        }
     
    }
}