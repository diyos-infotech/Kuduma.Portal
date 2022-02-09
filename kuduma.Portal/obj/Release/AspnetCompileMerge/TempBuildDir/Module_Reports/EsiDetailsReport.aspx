<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="EsiDetailsReport.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.EsiDetailsReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <style type="text/css">
        .style2 {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }

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

    <script type="text/javascript">

    </script>

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">ESI Upload</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">ESI Upload
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <table width="90%" cellpadding="5" cellspacing="5">

                                        <tr>
                                            <td>ESI Branch
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEsibranch" runat="server" class="sdrop">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Month :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" AutoComplete="off" class="sinput"></asp:TextBox>
                                                 <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                    Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                </cc1:CalendarExtender>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                                &nbsp; &nbsp; &nbsp;<asp:LinkButton ID="lbtn_Export_esiregister" runat="server" OnClick="lbtn_Export_esiregister_Click">ESI Register</asp:LinkButton>
                                            </td>
                                        </tr>

                                    </table>
                                </div>

                                <div class="rounded_corners">
                                    <div style="overflow: scroll; width: auto">
                                        <asp:GridView ID="GVListOfClients" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CssClass="datagrid" CellPadding="4" CellSpacing="3" OnRowDataBound="GVListOfClients_RowDataBound" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>

                                                <asp:TemplateField HeaderText="Emp ID">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpid" Text="<%# Bind('empid') %> "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="IP Number">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblIpNumber" Text="<%# Bind('EmpESINo') %> "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="IP Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblIPName" Text="<%# Bind('Fullname') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="No of Days for which wages paid/payable during the month">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblNoofduties" Text="<%# Bind('NoOfDuties') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Total Monthly Wages">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lbltotalmonthlyesi" Text="<%# Bind('ESIWAGES') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Reason Code for Zero working days">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblReason" Text=" "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Last Working Day">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblLastWorkingDay" Text=" "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
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
    </div>
</asp:Content>
