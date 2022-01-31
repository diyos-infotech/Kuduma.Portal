<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="BankUploadFormat.aspx.cs" Inherits="Kuduma.Portal.BankUploadFormat" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
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

        .auto-style1 {
            width: 28px;
        }
    </style>
    <script type="text/javascript">

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
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="BankUploadFormat.aspx" style="z-index: 7;" class="active_bread">Bank Upload Format</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Bank Upload Format
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>Month</td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput" AutoPostBack="true" AutoComplete="off" OnTextChanged="txtmonth_TextChanged"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="calendar1"
                                                    Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                </cc1:CalendarExtender>

                                            </td>

                                            <td style="margin-left: 40px">Options : </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOptions" class="sdrop" runat="server" >
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Bank Name</asp:ListItem>
                                                    <asp:ListItem>Cash</asp:ListItem>
                                                    <asp:ListItem>ICICI Bank</asp:ListItem>

                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False">Export to Excel</asp:LinkButton>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbtn_ExportToText" runat="server" OnClick="lbtn_ExportToText_Click" Visible="false">Export to Text</asp:LinkButton>
                                            </td>

                                        </tr>

                                        <tr style="width: 100%">
                                            <td colspan="6">
                                                <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="rounded_corners">
                                    <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="90%" Style="margin: 0px auto"
                                        CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Client Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientid" runat="server" Text='<%#Eval("clientid") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Client Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientname" runat="server" Text='<%#Eval("clientname") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>

                                <div class="rounded_corners">
                                    <asp:GridView ID="GVListClients" runat="server" AutoGenerateColumns="true" Width="90%" Style="margin: 0px auto"
                                        CellSpacing="3" CellPadding="5" ForeColor="#333333" OnRowDataBound="GVListClients_RowDataBound">

                                        <Columns>
                                        </Columns>
                                        <FooterStyle Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle Font-Bold="True" ForeColor="black" Height="30" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>

                                
                                  <div class="rounded_corners">
                                        <asp:GridView ID="GvText1" runat="server" AutoGenerateColumns="false" Width="90%" Style="margin: 0px auto"
                                            CellSpacing="3" CellPadding="5" ForeColor="#333333">

                                            <Columns>
                                                
                                                <asp:BoundField DataField="FHR" HeaderText="" />
                                                <asp:BoundField DataField="Code" HeaderText="" />
                                                <asp:BoundField DataField="CompBankAc" HeaderText="" />
                                                <asp:BoundField DataField="INR" HeaderText="" />
                                                <asp:BoundField DataField="TotalSalary" HeaderText="" />
                                                <asp:BoundField DataField="Vline" HeaderText="" />
                                                <asp:BoundField DataField="Count" HeaderText="" />
                                                <asp:BoundField DataField="CDate" HeaderText="" />
                                                <asp:BoundField DataField="ClientShortName" HeaderText="" />

                                            </Columns>
                                            <FooterStyle Font-Bold="True" ForeColor="White" />
                                            <PagerStyle ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle Font-Bold="True" ForeColor="black" Height="30" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>
                                  
                                  <div class="rounded_corners">
                                        <asp:GridView ID="GvText2" runat="server" AutoGenerateColumns="false" Width="90%" Style="margin: 0px auto"
                                            CellSpacing="3" CellPadding="5" ForeColor="#333333">

                                            <Columns>

                                                <asp:BoundField DataField="APONFT" HeaderText="" />
                                                <asp:BoundField DataField="EmpSalary" HeaderText="" />
                                                <asp:BoundField DataField="INR" HeaderText="" />
                                                <asp:BoundField DataField="CompBankAc" HeaderText="" />
                                                <asp:BoundField DataField="Code" HeaderText="" />
                                                <asp:BoundField DataField="EmpIFSCcode" HeaderText="" />
                                                <asp:BoundField DataField="EmpBankAcNo" HeaderText="" />
                                                <asp:BoundField DataField="EmpCode" HeaderText="" />
                                                <asp:BoundField DataField="EmpName" HeaderText="" />
                                                <asp:BoundField DataField="EmpSalaryDateforClient" HeaderText="" />
                                                <asp:BoundField DataField="EmpSalaryDateforEmp" HeaderText="" />

                                            </Columns>
                                            <FooterStyle Font-Bold="True" ForeColor="White" />
                                            <PagerStyle ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle Font-Bold="True" ForeColor="black" Height="30" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>


                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <!-- DASHBOARD CONTENT END -->
</asp:Content>

