using System;
using System.Collections;
using System.Data;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class Reminders : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string CmpIDPrefix = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            CmpIDPrefix = Session["CmpIDPrefix"].ToString();

            string day = (DateTime.Now).Day.ToString();
            string month = (DateTime.Now).Month.ToString();

            string SqlQry = "select isnull(empfname,'')+' '+ isnull(empMname,'')+' ' + isnull(empLname,'') + '<br/>[  Ph.No:' +isnull(EmpPhone,'NA') + ']' + ' '  as Name  " +
                            "  From Empdetails   Where (CAST( datepart(mm, EmpDtofBirth) as  nvarchar(20))+ CAST(datepart(DD, EmpDtofBirth) as nvarchar(20)))='" + month + day + "'";
            DataTable Dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
            if (Dt.Rows.Count > 0)
            {

                //for (int i = 0; i < Dt.Rows.Count; i++)
                //{
                //    lblGVBirthday.Text += Dt.Rows[i]["Name"].ToString() + "<br/><br/>";
                //}
                GVBirthday.DataSource = Dt;
                GVBirthday.DataBind();
                GVBirthday_Viewmore.DataSource = Dt;
                GVBirthday_Viewmore.DataBind();

            }



            //Code for Latest 5 Bill generations as on 20/12/2013 by venkat

            SqlQry = "select top(5) '<b>Bill No:</b> '+convert(varchar,BillNo)+'   Generated to ' +c.ClientName+'  '+UnitId+' on  ' " +
                " +CONVERT(varchar(10),Timings,103) +'  at  '+right(convert(varchar(30),Timings,22),11)+'   " +
                " Grand Total Amount Rs.' +CONVERT(varchar,grandtotal) as BillStaus from UnitBill inner join Clients c on " +
                " unitbill.UnitId=c.ClientId where  unitbill.UnitId like '" + CmpIDPrefix + "%' order by Timings desc";
            Dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
            if (Dt.Rows.Count > 0)
            {
                gvLatestbills.DataSource = Dt;
                gvLatestbills.DataBind();
                gvLatestbills_Viewmore.DataSource = Dt;
                gvLatestbills_Viewmore.DataBind();

            }

            //Code for Latest 5 Payment generations as on 20/12/2013 by venkat

            SqlQry = "select top(5) 'Paysheet Generated to ' +c.ClientName+'  '+e.ClientId+' on  ' +CONVERT(varchar(10),e.Timings,103) " +
                " +'  at  '+right(convert(varchar(30),e.Timings,22),11)+'  Grand Total Amount Rs.' +CONVERT(varchar,SUM(e.ActualAmount)) " +
            " as Paysheetstatus from EmpPaySheet e inner join Clients c on e.ClientId=c.ClientId where e.clientid like '" + CmpIDPrefix + "%' group by c.ClientName,e.ClientId,e.Timings " +
            " order by Timings desc ";
            Dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
            if (Dt.Rows.Count > 0)
            {
                gvlatesPaysheet.DataSource = Dt;
                gvlatesPaysheet.DataBind();
                gvlatesPaysheet_Viewmore.DataSource = Dt;
                gvlatesPaysheet_Viewmore.DataBind();
            }

            //Code for Latest 5 Reciepts as on 20/12/2013 by venkat

            SqlQry = "select Top(5) 'Received   Rs.'+CONVERT(varchar,RecievedAmt)  + ' from '+Clientid+ ' on '+CONVERT(varchar(10),Timings,103)+ '" +
                " at ' +right(convert(varchar(30),Timings,22),11)+'  ( Reciept No. '+RecieptNo+ ')' as Reciepts from ReceiptMaster where ReceiptMaster.clientid like '" + CmpIDPrefix + "%' order by Timings desc";
            Dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
            if (Dt.Rows.Count > 0)
            {
                gvReciepts.DataSource = Dt;
                gvReciepts.DataBind();
                gvReciepts.DataSource = Dt;
                gvReciepts.DataBind();
            }



            Hashtable ht = new Hashtable();
            string ProcedureName = "BillingDetails";
            ht.Add("@clientidprefix", CmpIDPrefix);
            System.Data.DataTable DtInActiveClients = config.ExecuteAdaptorAsyncWithParams(ProcedureName, ht).Result;

            if (DtInActiveClients.Rows.Count > 0)
            {
                gvBills.DataSource = DtInActiveClients;
                gvBills.DataBind();
                gvBills_Viewmore.DataSource = DtInActiveClients;
                gvBills_Viewmore.DataBind();
            }


            ProcedureName = "EmpPaysheetDetails";
            ht = new Hashtable();
            ht.Add("@clientidprefix", CmpIDPrefix);
            DtInActiveClients =config.ExecuteAdaptorAsyncWithParams(ProcedureName, ht).Result;

            if (DtInActiveClients.Rows.Count > 0)
            {
                gvPaysheets.DataSource = DtInActiveClients;
                gvPaysheets.DataBind();
                gvPaysheets_Viewmore.DataSource = DtInActiveClients;
                gvPaysheets_Viewmore.DataBind();

            }

            ProcedureName = "GetExpiredContacts";
            ht = new Hashtable();
            ht.Add("@clientidprefix", CmpIDPrefix);
            DtInActiveClients = config.ExecuteAdaptorAsyncWithParams(ProcedureName, ht).Result;

            if (DtInActiveClients.Rows.Count > 0)
            {
                GVContract.DataSource = DtInActiveClients;
                GVContract.DataBind();
                GVContract_Viewmore.DataSource = DtInActiveClients;
                GVContract_Viewmore.DataBind();
            }



        }
    }
}