<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ReportforBillingAndPaysheet.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.ReportforBillingAndPaysheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/Calendar.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <style type="text/css">
        .custom-combobox {
            position: relative;
            display: inline-block;
        }

        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 5px 10px;
        }

        .style2 {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>
    <script type="text/javascript">
        function AssignExportHTML() {

            document.getElementById("ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_hidGridView").value =
          htmlEscape(ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_forExport.innerHTML);
        }
        function htmlEscape(str) {
            return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
        }

        function onCalendarShown() {

            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);

            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }

        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }

        function checkAll(objRef) {

            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        inputList[i].checked = true;

                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%-- <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="ReportforBillingAndPaysheet.aspx" style="z-index: 7;" class="active_bread">Billing and Paysheet Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Billing and Paysheet
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">

                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>

                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td colspan="7">
                                                <div align="right">
                                                    <asp:LinkButton ID="lbtn_ExportNew" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                    <table width="50%" cellpadding="5" cellspacing="5">

                                        <tr>
                                            <td>
                                                <asp:Label ID="lblmonth" runat="server" Text="Month" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" AutoComplete="off" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                    Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                </cc1:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnSearch_Click" />

                                            </td>

                                        </tr>



                                    </table>

                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Normal Billing : " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Billing Duties
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBduties" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>

                                            <td>Billing Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBAmount" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>

                                            <td>No of Clients for Billing
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBNoofClients" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Manual Billing : " />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Billing Duties
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMduties" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>

                                            <td>Billing Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMAmount" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>

                                            <td>No of Clients for Billing
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMNoofClients" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Paysheet : " />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Paysheet Duties
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPDuties" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>

                                            <td>Paysheet Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPAmount" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>

                                             <td>No of Clients for Paysheet
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMNoofPaysheet" runat="server" ReadOnly="True" class="sinput"></asp:TextBox>
                                            </td>

                                        </tr>
                                    </table>
                                </div>

                                <div class="rounded_corners">
                                    <div style="overflow: scroll; width: 100%">
                                        <asp:GridView ID="GVBillingandpaysheet" runat="server" AutoGenerateColumns="True" Width="970px" CssClass="table table-striped table-bordered table-condensed table-hover"
                                            CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="none" Style="margin-left: -2px">
                                            <Columns>
                                            </Columns>
                                        </asp:GridView>

                                        <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>
                                    </div>


                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
</asp:Content>


