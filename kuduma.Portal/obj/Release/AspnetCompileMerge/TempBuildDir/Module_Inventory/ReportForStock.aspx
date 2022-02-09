<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="ReportForStock.aspx.cs" Inherits="Kuduma.Portal.ReportForStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
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
    </style>
    <script type="text/javascript">
        function AssignExportHTML() {

            document.getElementById('hidGridView').value = htmlEscape(forExport.innerHTML);
        }
        function htmlEscape(str) {
            return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
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
    </script>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="ViewItems.aspx" style="z-index: 9;"><span></span>Inventory</a></li>
                    <li class="active"><a href="POReport.aspx" style="z-index: 7;" class="active_bread">Emp Inv Details Report </a></li>

                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Stock Consumption Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">

                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>

                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddloptions" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddloptions_SelectedIndexChanged">
                                                    <asp:ListItem>All</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlarmofc" runat="server" Visible="false"></asp:DropDownList>
                                            </td>
                                            <td>From Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromDate" runat="server" Text="" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBFromDate" runat="server" Enabled="True" TargetControlID="txtFromDate"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>To Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToDate" runat="server" Text="" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtTo_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBToDate" runat="server" Enabled="True" TargetControlID="txtToDate"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>

                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                            </td>

                                            <td>
                                                <div align="right">
                                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="true">Export to Excel</asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>


                                </div>
                                <%--  <asp:HiddenField ID="hidGridView" runat="server" />--%>
                                <asp:GridView ID="GVInflowOutflowDetails" runat="server" AutoGenerateColumns="True"
                                    EmptyDataText="No Records Found" Width="960px" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None">

                                    <Columns>
                                    </Columns>

                                </asp:GridView>
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

    <!-- CONTENT AREA END -->
</asp:Content>
