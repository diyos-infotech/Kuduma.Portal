<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="LoanDeductionReports.aspx.cs" Inherits="Kuduma.Portal.LoanDeductionReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="LoanDeductionReports.aspx" style="z-index: 7;" class="active_bread">
                        LOAN RECOVERY</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                LOAN RECOVERY
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <script type="text/javascript" language="javascript">
                                        function onCalendarShown() {
                                            var cal = $find("calendar1");
                                            cal._switchMode("years", true);
                                            if (cal._yearsBody) {
                                                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                                                    var row = cal._yearsBody.rows[i];
                                                    for (var j = 0; j < row.cells.length; j++) {
                                                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                                                    }
                                                }
                                            }
                                        }

                                        function onCalendarHidden() {
                                            var cal = $find("calendar1");
                                            if (cal._yearsBody) {
                                                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                                                    var row = cal._yearsBody.rows[i];
                                                    for (var j = 0; j < row.cells.length; j++) {
                                                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                                                    }
                                                }
                                            }
                                        }

                                        function call(eventElement) {
                                            var target = eventElement.target;
                                            switch (target.mode) {
                                                case "year":
                                                    var cal = $find("calendar1");
                                                    cal.set_selectedDate(target.date);
                                                    cal._blur.post(true);
                                                    cal.raiseDateSelectionChanged(); break;
                                            }
                                        }
                                    </script>

                                    <table cellpadding="5" cellspacing="5" width="70%">
                                        <tr>
                                            <td>
                                                Select Month :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlmonth" runat="server" class="sdrop">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>Jan</asp:ListItem>
                                                    <asp:ListItem>Feb</asp:ListItem>
                                                    <asp:ListItem>March</asp:ListItem>
                                                    <asp:ListItem>April</asp:ListItem>
                                                    <asp:ListItem>May</asp:ListItem>
                                                    <asp:ListItem>June</asp:ListItem>
                                                    <asp:ListItem>July</asp:ListItem>
                                                    <asp:ListItem>Aug</asp:ListItem>
                                                    <asp:ListItem>September</asp:ListItem>
                                                    <asp:ListItem>October</asp:ListItem>
                                                    <asp:ListItem>November</asp:ListItem>
                                                    <asp:ListItem>December</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Year :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtyear" runat="server" class="sinput"> </asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown" BehaviorID="calendar1"
                                                    TargetControlID="txtyear" Format="yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    TargetControlID="txtyear" ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btn_SubmitClick" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners">
                                    <asp:GridView ID="GVLoanRecoveryReports" runat="server" AutoGenerateColumns="False"
                                        Width="100%" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                            <asp:BoundField DataField="Empid" HeaderText="Emp Id" />
                                            <asp:BoundField DataField="EmpmName" HeaderText="Name" />
                                            <asp:BoundField DataField="LoanNo" HeaderText="Loan No" />
                                            <asp:BoundField DataField="LoanAmount" HeaderText="Loan Amount" />
                                            <asp:BoundField DataField="Loandt" HeaderText="Issued Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="TransactionDt" HeaderText="Deduction Month" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="CuttingAmount" HeaderText="Cuting Amount" />
                                            <asp:BoundField DataField="Balance" HeaderText="Balance" />
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red" />
                                </div>
                                <div>
                                    <table width="100%">
                                        <tr style="width: 100%; font-weight: bold">
                                            <td style="width: 72%" align="right">
                                                <asp:Label ID="lbltamttext" runat="server" Text="Total Amount :"></asp:Label>
                                            </td>
                                            <td style="width: 30%">
                                                <asp:Label ID="lbltmtc" runat="server" Text="" Style="margin-left: 8%"></asp:Label>
                                                <asp:Label ID="lbltmtb" runat="server" Text="" Style="margin-left: 46%"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
        </asp:content>