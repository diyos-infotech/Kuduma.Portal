using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Data;
using System.IO;
using System.Data.OleDb;
using Kuduma.Portal.DAL;
using ClosedXML.Excel;

namespace Kuduma.Portal
{
    public partial class ImportEmpLoanDetails : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();

        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string UserID = "";

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            UserID = Session["UserId"].ToString();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            sampleGrid();

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

                    ExcelNos();

                    string Qry = "Select * from NotInsertLoanData";
                    DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
                    if (dt.Rows.Count > 0)
                    {
                        btnUnsavedExport.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void ExcelNos()
        {
            string qry = "select distinct excel_no from emploanmaster order by excel_no desc";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlExcelNo.DataValueField = "excel_no";
                ddlExcelNo.DataTextField = "excel_no";
                ddlExcelNo.DataSource = dt;
                ddlExcelNo.DataBind();

            }

            ddlExcelNo.Items.Insert(0, "-Select-");

        }
        public void sampleGrid()
        {

            string query = "select top 1 '' as 'ID NO','' as 'Loan Type', '' as 'Amount', '' as 'NoofInstalments','' as 'LoanIssuedDate', '' as 'LoanCuttingFrom' from Emploandetails";

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                GvInputEmpLoanDetails.DataSource = dt;
                GvInputEmpLoanDetails.DataBind();
            }
            else
            {
                GvInputEmpLoanDetails.DataSource = null;
                GvInputEmpLoanDetails.DataBind();
            }

        }


        protected void LinkSample_Click(object sender, EventArgs e)
        {
            gve.NewExport("SampleLoanDetailsSheet.xlsx", this.GvInputEmpLoanDetails);
        }

        public string GetExcelSheetNames()
        {
            string ExcelSheetname = "";
            OleDbConnection con = null;
            DataTable dt = null;
            string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(FlUploadLoanDetails.PostedFile.FileName));
            FlUploadLoanDetails.PostedFile.SaveAs(filename);
            string extn = Path.GetExtension(FlUploadLoanDetails.PostedFile.FileName);
            string conStr = string.Empty;
            if (extn.ToLower() == ".xls")
            {
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extn.ToLower() == ".xlsx")
            {
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
            }

            con = new OleDbConnection(conStr);
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();


            return ExcelSheetname;
        }

        public void LoanImportedData(string ExcelNo)
        {

            GvLoansImported.DataSource = null;
            GvLoansImported.DataBind();

            string qry = "select emploanmaster.empid,(empfname+''+empmname+''+emplname) as Empname,LoanAmount,NoInstalments,convert(varchar(10),LoanIssuedDate,103) as LoanIssuedDate,convert(varchar(10),LoanDt,103) as LoanDt,case typeofloan when 0 then 'Sal adv'  when 1 then 'Uniform' when 2 then 'Security Dep' when 3 then 'Loan' when 4 then 'Other Ded'   " +
                         " end as Loantype,typeofloan, emploanmaster.Created_by,Datetime as created_On from emploanmaster  inner join empdetails ed on ed.empid=emploanmaster.empid where Excel_No='" + ExcelNo + "' and Excel_No is not null and Excel_No<>'' order by typeofloan";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;

            if (dt.Rows.Count > 0)
            {
                GvLoansImported.Visible = true;
                GvLoansImported.DataSource = dt;
                GvLoansImported.DataBind();
            }
            else
            {
                GvLoansImported.Visible = false;
                GvLoansImported.DataSource = null;
                GvLoansImported.DataBind();

            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            //string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(FlUploadLoanDetails.PostedFile.FileName));
            //FlUploadLoanDetails.PostedFile.SaveAs(filename);
            //string extn = Path.GetExtension(FlUploadLoanDetails.PostedFile.FileName);
            //string constring = "";
            //if (extn.ToLower() == ".xls")
            //{
            //    //constring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
            //    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
            //}
            //else if (extn.ToLower() == ".xlsx")
            //{
            //    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
            //}

            //string Sheetname = string.Empty;




            //string qry = "select [ID NO],[Loan Type],[Amount],[NoofInstalments],[LoanIssuedDate],[LoanCuttingFrom]" +
            //"  from  [" + GetExcelSheetNames() + "]" + "";


            //OleDbConnection con = new OleDbConnection(constring);
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.Open();
            //}
            //OleDbCommand cmd = new OleDbCommand(qry, con);
            //OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //da.Dispose();
            //con.Close();
            //con.Dispose();

            string Sheetname = string.Empty;
            Sheetname = FlUploadLoanDetails.PostedFile.FileName;

            string qrychk = "select top(1) * from emploanmaster where isnull(SheetName,'')='" + Sheetname + "'";
            DataTable dtchk = config.ExecuteAdaptorAsyncWithQueryParams(qrychk).Result;
            if (dtchk.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Attached excel Sheet is already uploaded');", true);
                return;
            }


            string filePath = Server.MapPath("~/ImportDocuments/") + Path.GetFileName(FlUploadLoanDetails.PostedFile.FileName);
            FlUploadLoanDetails.PostedFile.SaveAs(filePath);

            string extn = Path.GetExtension(FlUploadLoanDetails.PostedFile.FileName);

            //Create a new DataTable.
            DataTable dtexcel = new DataTable();

            if (extn.EndsWith(".xlsx"))
            {
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    IXLWorksheet workSheet = workBook.Worksheet(1);


                    int lastrow = workSheet.LastRowUsed().RowNumber();
                    var rows = workSheet.Rows(1, lastrow);

                    //Create a new DataTable.

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in rows)
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                if (!string.IsNullOrEmpty(cell.Value.ToString()))
                                {
                                    dtexcel.Columns.Add(cell.Value.ToString());
                                }
                                else
                                {
                                    break;
                                }
                            }
                            firstRow = false;
                        }
                        else
                        {
                            int i = 0;
                            DataRow toInsert = dtexcel.NewRow();
                            foreach (IXLCell cell in row.Cells(1, dtexcel.Columns.Count))
                            {
                                try
                                {
                                    toInsert[i] = cell.Value.ToString();
                                }
                                catch (Exception ex)
                                {

                                }
                                i++;
                            }
                            dtexcel.Rows.Add(toInsert);
                        }

                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please save file in Excel WorkBook(.xlsx) format');", true);
                return;
            }

            for (int s = 0; s < dtexcel.Rows.Count; s++)
            {
                string clid = dtexcel.Rows[s][1].ToString().Trim();

                if (clid.Length == 0)
                {
                    dtexcel.Rows.RemoveAt(s);
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dtexcel);

            #region Begin Getmax Id from DB
            int ExcelNo = 0;
            string selectquerycomppanyid = "select max(cast(Excel_No as int )) as Id from emploanmaster";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquerycomppanyid).Result;

            if (dt.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dt.Rows[0]["Id"].ToString()) == false)
                {
                    ExcelNo = Convert.ToInt32(dt.Rows[0]["Id"].ToString()) + 1;
                }
                else
                {
                    ExcelNo = int.Parse("1");
                }
            }
            #endregion End Getmax Id from DB

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string OldEmpid = string.Empty;
                string Empid = string.Empty;
                string Loantype = string.Empty;
                String Amount = "0";
                int NoOfInstalments = 1;
                string LoanIssuedDate = "";
                string LoanCuttingMonth = "";
                int loanStatus = 0;
                string TypeOfLoan = "0";
                string Date = "";





                OldEmpid = ds.Tables[0].Rows[i]["ID NO"].ToString();
                TypeOfLoan = ds.Tables[0].Rows[i]["Loan Type"].ToString();
                Amount = ds.Tables[0].Rows[i]["Amount"].ToString();
                if (ds.Tables[0].Rows[i]["NoofInstalments"].ToString().Length > 0)
                {
                    NoOfInstalments = int.Parse(ds.Tables[0].Rows[i]["NoofInstalments"].ToString());
                }

                LoanIssuedDate = ds.Tables[0].Rows[i]["LoanIssuedDate"].ToString();
                LoanCuttingMonth = ds.Tables[0].Rows[i]["LoanCuttingFrom"].ToString();

                if (LoanIssuedDate.Length > 0)
                {
                    string db1 = Convert.ToDateTime(LoanIssuedDate).ToString("dd/MM/yyyy");

                    LoanIssuedDate = Timings.Instance.CheckDateFormat(db1);
                }


                if (LoanCuttingMonth.Length > 0)
                {
                    string db2 = Convert.ToDateTime(LoanCuttingMonth).ToString("dd/MM/yyyy");

                    LoanCuttingMonth = Timings.Instance.CheckDateFormat(db2);
                }

                Date = DateTime.Now.ToString("dd/MM/yyyy");
                Date = Timings.Instance.CheckDateFormat(Date);

                string sqlchkempid = "select empid,oldempid from empdetails where Oldempid='" + OldEmpid + "'";
                DataTable dtchkempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkempid).Result;
                if (dtchkempid.Rows.Count > 0)
                {
                    Empid = dtchkempid.Rows[0]["empid"].ToString();

                    if (Empid.Length > 0)
                    {

                        string DeleteQry = "delete NotInsertLoanData where empid='" + Empid + "' and LoanAmount='" + Amount + "' and LoanType='" + TypeOfLoan + "'";
                        int Deletestatus = config.ExecuteNonQueryWithQueryAsync(DeleteQry).Result;

                        string insertquery = string.Format(" insert into EmpLoanMaster(loandt,empid,loantype,loanamount,NoInstalments,  " +
                      " LoanStatus,TypeOfLoan,LoanIssuedDate,Created_By,Created_On,Excel_No,SheetName) values( '{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}','{10}','{11}') ",
                      LoanCuttingMonth, Empid, Loantype,
                      Amount, NoOfInstalments, loanStatus, TypeOfLoan, LoanIssuedDate, UserID, Date, ExcelNo, Sheetname);
                        int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                        if (status != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('New Loans Generated Successfuly');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Loan not Generated for '" + Empid + "'');", true);
                        }
                    }
                }
                else
                {
                    string DeleteQry = "delete NotInsertLoanData where empid='" + OldEmpid + "' and LoanAmount='" + Amount + "' and LoanType='" + TypeOfLoan + "'";
                    int Deletestatus = config.ExecuteNonQueryWithQueryAsync(DeleteQry).Result;

                    string insertnodata = "insert into NotInsertLoanData values('" + OldEmpid + "','" + Amount + "','" + TypeOfLoan + "','" + ExcelNo + "','" + UserID + "',Getdate(),'EmpID not available')";
                    int Notstatus = config.ExecuteNonQueryWithQueryAsync(insertnodata).Result;
                }

                LoanImportedData(ExcelNo.ToString());
                ExcelNos();

                string Qrys = "Select * from NotInsertLoanData";
                DataTable dts = config.ExecuteAdaptorAsyncWithQueryParams(Qrys).Result;
                if (dts.Rows.Count > 0)
                {
                    btnUnsavedExport.Visible = true;
                }
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }

        int subTotalRowIndex = 0;
        decimal TotalLoanAmt = 0;
        string currentId = "";

        protected void GvLoansImported_RowCreated(object sender, GridViewRowEventArgs e)
        {
            TotalLoanAmt = 0;

            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.DataItem is DataRowView
                       && (e.Row.DataItem as DataRowView).DataView.Table != null)
                {
                    dt = (e.Row.DataItem as DataRowView).DataView.Table;
                    string orderId = (dt.Rows[e.Row.RowIndex]["typeofloan"].ToString());

                    if (orderId != currentId)
                    {
                        if (e.Row.RowIndex > 0)
                        {
                            for (int i = subTotalRowIndex; i < e.Row.RowIndex; i++)
                                TotalLoanAmt += Convert.ToDecimal(GvLoansImported.Rows[i].Cells[4].Text);

                            this.AddTotalRow("Total", TotalLoanAmt.ToString("N2"));

                        }
                        subTotalRowIndex = e.Row.RowIndex;

                        currentId = orderId;
                    }

                }
            }
        }

        private void AddTotalRow(string labelText, string LoanAmount)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);


            row.Cells.AddRange(new TableCell[11] { new TableCell {CssClass="SubTotalRowStyle"}, //Empty Cell
                                        new TableCell {CssClass="SubTotalRowStyle"},
                                        new TableCell {CssClass="SubTotalRowStyle"},
                                        new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right,CssClass="SubTotalRowStyle"},
                                        new TableCell { Text = LoanAmount, HorizontalAlign = HorizontalAlign.Right,CssClass="SubTotalRowStyle" },
                                        new TableCell {CssClass="SubTotalRowStyle"},
                                        new TableCell {CssClass="SubTotalRowStyle"},
                                        new TableCell {CssClass="SubTotalRowStyle"},
                                        new TableCell {CssClass="SubTotalRowStyle"},
                                        new TableCell {CssClass="SubTotalRowStyle"},
                                         new TableCell {CssClass="SubTotalRowStyle"}



        });

            GvLoansImported.Controls[0].Controls.Add(row);


        }

        protected void GvLoansImported_DataBound(object sender, EventArgs e)
        {
            if (GvLoansImported.Rows.Count > 0)
            {

                for (int i = subTotalRowIndex; i < GvLoansImported.Rows.Count; i++)
                    TotalLoanAmt += Convert.ToDecimal(GvLoansImported.Rows[i].Cells[4].Text);


                this.AddTotalRow("Total", TotalLoanAmt.ToString("N2"));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string Excelno = ddlExcelNo.SelectedValue;
            LoanImportedData(Excelno);
        }

        protected void btnunsavedExport_Click(object sender, EventArgs e)
        {
            string Qry = "Select Empid as 'Emp ID',LoanAmount as  'Loan Amount',Exel_No as 'Excel No',Remarks from NotInsertLoanData";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Qry).Result;
            if (dt.Rows.Count > 0)
            {
                gve.NewExportExcel("UnsavedLoansdata.xlsx", dt);
            }
        }
    }
}