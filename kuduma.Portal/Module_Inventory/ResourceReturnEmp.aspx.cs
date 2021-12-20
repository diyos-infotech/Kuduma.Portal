using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class ResourceReturnEmp : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string Username = "";

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
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            Username = Session["UserId"].ToString();
        }


        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }


        protected void ClearData()
        {
            //txtnoofinstallments.Text = string.Empty;

            if (gvresources.Rows.Count > 0)
            {

                foreach (GridViewRow gvrow in gvresources.Rows)
                {
                    CheckBox CheckRow = gvrow.FindControl("CbChecked") as CheckBox;
                    CheckRow.Checked = false;
                }
            }
        }

        protected void txtQty_Textchanged(object sender, EventArgs e)
        {
            int qty = 0;
            float Price = 0;
            float totalPrice = 0;

            foreach (GridViewRow gvr in gvresources.Rows)
            {
                CheckBox cbcheck = sender as CheckBox;
                Control ctrlone = gvr.FindControl("CbChecked") as CheckBox;
                CheckBox chkresource = (CheckBox)ctrlone;
                if (chkresource != null)
                {
                    if (chkresource.Checked)
                    {

                        TextBox txtprice = (TextBox)gvr.FindControl("txtresourceprice");
                        Label resourcename = (Label)gvr.FindControl("lblresourcename");
                        Label lblresourceid = (Label)gvr.FindControl("lblresourceid");
                        TextBox txtQty = (TextBox)gvr.FindControl("txtQty");
                        qty = int.Parse(txtQty.Text);
                        Price = float.Parse(txtprice.Text);
                        totalPrice = qty * Price;

                        txtprice.Text = totalPrice.ToString();

                    }
                }
            }
        }

        protected void gvresources_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvresources.PageIndex = e.NewPageIndex;
            empLoadResourcedetails();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow gvr in gvresources.Rows)
            {
                CheckBox cbcheck = sender as CheckBox;
                Control ctrlone = gvr.FindControl("CbChecked") as CheckBox;
                CheckBox chkresource = (CheckBox)ctrlone;
                if (chkresource != null)
                {
                    if (chkresource.Checked)
                    {
                        #region Begin Individual Resource Details of the employee



                        TextBox tb = (TextBox)gvr.FindControl("txtresourceprice");
                        Label resourcename = (Label)gvr.FindControl("lblresourcename");
                        Label lblresourceid = (Label)gvr.FindControl("lblresourceid");
                        Label txtIssuedQty = (Label)gvr.FindControl("txtIssuedQty");
                        TextBox txtQty = (TextBox)gvr.FindControl("txtQty");


                        if (txtIssuedQty.Text == "")
                        {
                            txtIssuedQty.Text = "0";
                        }

                        if (txtQty.Text == "")
                        {
                            txtQty.Text = "0";
                        }

                        if (float.Parse(txtQty.Text) > float.Parse(txtIssuedQty.Text))
                        {
                            gvr.ForeColor = System.Drawing.Color.Red;
                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert()", "alert('Resources cant be returned as return quantity is exceeding the issued quantity ');", true);
                            return;

                        }
                        else
                        {
                            gvr.ForeColor = System.Drawing.Color.Black;
                        }
                    }

                        #endregion
                }
            }




            #region check issue mode
            int issuemode = 0;

            if (ddlFreepaid.SelectedIndex == 0)
            {
                issuemode = 0;
            }

            if (ddlFreepaid.SelectedIndex == 1)
            {
                issuemode = 1;
            }
            #endregion

            #region check no of installment depend on issuemode

            int NoofInstallments = 0;

            if (txtnoofinstallments.Text.Trim().Length > 0)
            {
                NoofInstallments = int.Parse(txtnoofinstallments.Text);

            }

            if (issuemode == 1)
            {
                NoofInstallments = 0;
            }


            #endregion

            int currentrowindex = 0;
            int CheckAtleastOne = 0;
            int InsertStatus = 0;

            string @TotalTransactionID = "";
            string Empid = txtEmpid.Text;
            string SqlqryForResourceAlloc = string.Empty;
            string ResourceID = string.Empty;
            string loanno = txtLoanno.Text;
            string LoanIssuedDate = DateTime.Now.Date.ToShortDateString();
            string AllResourceNames = string.Empty;
            string Uniformid = txtuniformid.Text;

            float amount = 0;

            double sum = 0;
            double loanamnt = 0;
            double TotalQty = 0;

            DataTable DtAddResource = null;

            Hashtable HTInserResource = new Hashtable();
            string SPName = "AddResourcesBranchwise";

            #region For Each for Gridview Indvidual Rows
            if (gvresources.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvresources.Rows)
                {
                    CheckBox cbcheck = sender as CheckBox;
                    Control ctrlone = gvr.FindControl("CbChecked") as CheckBox;
                    CheckBox chkresource = (CheckBox)ctrlone;
                    if (chkresource != null)
                    {
                        if (chkresource.Checked)
                        {
                            #region Begin Individual Resource Details of the employee

                            int Qty = 0;
                            string ResourceType = "";
                            float TotalPrice = 0;

                            CheckAtleastOne = 1;
                            TextBox tb = (TextBox)gvr.FindControl("txtresourceprice");
                            Label resourcename = (Label)gvr.FindControl("lblresourcename");
                            Label lblresourceid = (Label)gvr.FindControl("lblresourceid");
                            TextBox txtQty = (TextBox)gvr.FindControl("txtQty");
                            Label lbltype = (Label)gvr.FindControl("lbltype");

                            Qty = int.Parse(txtQty.Text);
                            ResourceID = lblresourceid.Text;
                            amount = float.Parse(tb.Text);
                            TotalPrice = amount;
                            ResourceType = lbltype.Text;
                            sum += TotalPrice;

                            TotalQty += (Qty) * (TotalPrice);

                            #region Begin New code for Insert Resource Details as on 19/07/2014

                            HTInserResource.Clear();
                            HTInserResource.Add("@Empid", Empid);
                            HTInserResource.Add("@Resourceid", ResourceID);
                            HTInserResource.Add("@Qty", Qty * -1);
                            HTInserResource.Add("@Price", TotalPrice * -1);
                            HTInserResource.Add("@ClientIDPrefix", CmpIDPrefix);
                            HTInserResource.Add("@TotalTransactionID", @TotalTransactionID);
                            HTInserResource.Add("@currentrowindex", currentrowindex + 1);
                            HTInserResource.Add("@LoanNo", txtLoanno.Text);
                            HTInserResource.Add("@LoanType", 'R');
                            HTInserResource.Add("@UniformID", Uniformid);
                            HTInserResource.Add("@Created_By", Username);
                            HTInserResource.Add("@Type", ResourceType);

                            DtAddResource =config.ExecuteAdaptorAsyncWithParams(SPName, HTInserResource).Result;

                            if (DtAddResource.Rows.Count > 0)
                            {
                                if (currentrowindex == 0)
                                {
                                    @TotalTransactionID = DtAddResource.Rows[0]["transactionid"].ToString();
                                }
                            }

                            #endregion End New code for Insert Resource Details as on 19/07/2014

                            #region Begin Old code for Insert Resource Details as on 19/07/2014

                            //SqlqryForResourceAlloc = "Select isnull(ActualQuantity,0) As ActualQuantity  From Stockitemlist Where itemid='" + ResourceID + "'";
                            //DataTable DtForResourceAlloc = SqlHelper.Instance.GetTableByQuery(SqlqryForResourceAlloc);
                            //if (DtForResourceAlloc.Rows.Count > 0)
                            //{
                            //    if (String.IsNullOrEmpty(DtForResourceAlloc.Rows[0]["ActualQuantity"].ToString()) == false)
                            //    {
                            //        if (int.Parse(DtForResourceAlloc.Rows[0]["ActualQuantity"].ToString()) > 0)
                            //        {
                            //            SqlqryForResourceAlloc = string.Format("insert into EmpResourceDetails(EmpID,ResourceId,Price,Qty) values('{0}','{1}','{2}','{3}')",
                            //                Empid, ResourceID, TotalPrice, Qty);
                            //            InsertStatus = SqlHelper.Instance.ExecuteDMLQry(SqlqryForResourceAlloc);


                            //            if (InsertStatus != 0)
                            //            {
                            //                SqlqryForResourceAlloc = string.Format("Update Stockitemlist set ActualQuantity=ActualQuantity-'" + Qty + "' Where Itemid='{0}'",
                            //                                                    ResourceID);
                            //                InsertStatus = SqlHelper.Instance.ExecuteDMLQry(SqlqryForResourceAlloc);
                            //            }
                            //            sum += TotalPrice;
                            //        }
                            //        else
                            //        {
                            //            AllResourceNames = "\n  " + resourcename.Text + ",";
                            //        }
                            //    }
                            //}


                            #endregion End Old code for Insert Resource Details as on 19/07/2014

                            currentrowindex++;

                            #endregion  //End  Individual Resource Details of the employee
                        }
                    }
                }
            #endregion  //End For Each for Gridview Indvidual Rows

                //Label1.Text = sum.ToString();
                if (CheckAtleastOne == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Please Select Atleast One Resource');", true);
                    return;
                }
                else
                {

                    #region  //Begin  Else block for the Check Atleast One Resource

                    //if (sum == 0)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Resources Are Not allocated For the Selected Employee Please Check');", true);
                    //    return;
                    //}
                    //else if (sum > 0)
                    {
                        if (issuemode == 0)
                        {
                            double paidamt = Convert.ToDouble(txtPaidAmnt.Text);

                            SqlqryForResourceAlloc = string.Format("update EmpLoanMaster set RefundAmnt = isnull(RefundAmnt,0)+" + TotalQty + ", LoanAmount = (LoanAmount - " + (TotalQty) + ") Where LoanNo = " + txtLoanno.Text + " and EmpId = '" + txtEmpid.Text + "'");
                            InsertStatus = config.ExecuteNonQueryWithQueryAsync(SqlqryForResourceAlloc).Result;
                        }
                        else
                        {
                            SqlqryForResourceAlloc = string.Format("update EmpLoanMaster set RefundAmnt = '0' Where LoanNo = " + txtLoanno.Text + " and EmpId = '" + txtEmpid.Text + "'");
                            InsertStatus = config.ExecuteNonQueryWithQueryAsync(SqlqryForResourceAlloc).Result;
                        }


                        if (InsertStatus != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Resources returned from the selected employee.');", true);

                            string selectqueryclientid = "select LoanAmount,PaidAmnt,RefundAmnt from EmpLoanMaster Where LoanNo = " + txtLoanno.Text + " and EmpId = '" + txtEmpid.Text + "'";
                            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;

                            if (dt.Rows.Count > 0)
                            {
                                loanamnt = Convert.ToDouble(dt.Rows[0]["LoanAmount"].ToString());
                            }

                            lblTotalamt.Text = "Total Updated Loan Amount Rs. : " + loanamnt;
                            ClearData();
                            empLoadResourcedetails();

                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Resources are not returned for the selected employee.');", true);
                            return;
                        }
                    #endregion   //End  Else block for the Check Atleast One Resource
                    }
                }//
            }//end Gridview No Of Reows

            gvresources.PageIndex = 0;

        }

        protected void txtloanid_TextChanged(object sender, EventArgs e)
        {
            string loanid = txtLoanno.Text.ToString();


            string selectqueryclientid = "select A.*,B.EmpFName + B.EmpMName +B.EmpLName as EmpName,erd.uniformid from EmpLoanMaster A inner join EmpDetails B on A.EmpId = B.EmpId inner join empresourcedetails erd on erd.loanno=a.loanno where a.LoanNo = " + loanid;
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectqueryclientid).Result;

            if (dt.Rows.Count > 0)
            {
                DateTime dt1 = Convert.ToDateTime(dt.Rows[0]["LoanDt"].ToString());

                txtloandate.Text = dt1.ToString("dd/MM/yyyy");
                txtnoofinstallments.Text = dt.Rows[0]["NoInstalments"].ToString();
                txtPaidAmnt.Text = dt.Rows[0]["PaidAmnt"].ToString();
                txtEmpid.Text = dt.Rows[0]["EmpId"].ToString();
                txtName.Text = dt.Rows[0]["EmpName"].ToString();
                txtuniformid.Text = dt.Rows[0]["uniformid"].ToString();
                ddlFreepaid.SelectedIndex = int.Parse(dt.Rows[0]["IssueMode"].ToString());
                {
                    empLoadResourcedetails();
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('No Data Found For the Loan ID Given. Please Check the Loan ID Entered');", true);
                gvresources.DataSource = null;
                gvresources.DataBind();
            }

        }

        protected void empLoadResourcedetails()
        {
            string Sqlqry = "";

            Sqlqry = "select ResourceId from EmpResourceDetails R " +
                      " inner join InvStockItemList SI on R.ResourceID=SI.ItemId where loanno='" + txtLoanno.Text.Trim() + "' group by ResourceId having SUM(qty)>0";

            DataTable dtResources = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;

            if (dtResources.Rows.Count > 0)
            {
                Sqlqry = "select R.Type,R.ResourceID,SI.ItemName,(R.Qty) as IssuedQty,0 as Qty, R.Price  from EmpResourceDetails R inner join InvStockItemList SI on R.ResourceID=SI.ItemId  where r.Loanno='" +
                   txtLoanno.Text.Trim() + "'   order by R.ResourceID";
                DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
                if (dt.Rows.Count > 0)
                {
                    gvresources.DataSource = dt;
                    gvresources.DataBind();


                }
                else
                {
                    gvresources.DataSource = null;
                    gvresources.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('No Data Found For the Loan ID Given. Please Check the Loan ID Entered');", true);
                }
            }
            else
            {
                gvresources.DataSource = null;
                gvresources.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('No Data Found For the Loan ID Given. Please Check the Loan ID Entered');", true);
            }
        }


    }
}