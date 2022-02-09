<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Employees/EmployeeMaster.master" CodeBehind="EmpWeeklyPayments.aspx.cs" Inherits="Kuduma.Portal.Module_Employees.EmpWeeklyPayments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>


    <style type="text/css">
        .style1 {
            width: 135px;
        }

        .modalBackground {
            background-color: Gray;
            z-index: 10000;
        }
    </style>

    <script type="text/javascript">
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
    </script>

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">

            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Employee Weekly Payments
                            </h2>
                        </div>

                        <asp:ScriptManager runat="server" ID="Scriptmanager2">
                        </asp:ScriptManager>

                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>

                                        <td style="padding-left: 20px">Month 
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Month" Width="100px" runat="server" class="sinput"
                                                Text=""></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown"
                                                Enabled="true" Format="MMM-yyyy" TargetControlID="Txt_Month">
                                            </cc1:CalendarExtender>

                                        </td>
                                        <td>From Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFromDate" Width="100px" runat="server" class="sinput"
                                                Text=""></asp:TextBox>
                                            <cc1:CalendarExtender ID="TxtFromDate_CalenderControl" runat="server"
                                                Enabled="true" Format="dd/MM/yyyy" TargetControlID="TxtFromDate">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="TxtFromDate_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="TxtFromDate"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>To Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtToDate" Width="100px" runat="server" class="sinput"
                                                Text=""></asp:TextBox>
                                            <cc1:CalendarExtender ID="TxtToDate_CalenderControl" runat="server"
                                                Enabled="true" Format="dd/MM/yyyy" TargetControlID="TxtToDate">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="TxtToDate_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="TxtToDate"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>

                                            <%-- OnClick="btnpayment_Click"--%>
                                            <asp:Button ID="btnpayment" runat="server" Text="Generate Payment" class=" btn save"
                                                OnClientClick='return confirm("Are you sure you want to generate payment?");' />
                                        </td>
                                    </tr>
                                </table>
                                <br />

                                <br />
                                <div class="rounded_corners" style="overflow: auto; width: 99%">

                                    <br />
                                </div>

                                <!-- DASHBOARD CONTENT END -->
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <!-- DASHBOARD CONTENT END -->
        </div>
    </div>
</asp:Content>
