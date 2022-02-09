<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ModificationLoanReport.aspx.cs" Inherits="Kuduma.Portal.ModificationLoanReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
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
    </style>
    <style type="text/css">
        .style1 {
            width: 135px;
        }

        .completionList {
            background: white;
            border: 1px solid #DDD;
            border-radius: 3px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            min-width: 165px;
            height: 200px;
            overflow: auto;
        }

        .listItem {
            display: block;
            padding: 5px 5px;
            border-bottom: 1px solid #DDD;
        }

        .itemHighlighted {
            color: black;
            background-color: rgba(0, 0, 0, 0.1);
            text-decoration: none;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            border-bottom: 1px solid #DDD;
            display: block;
            padding: 5px 5px;
        }

        .visibility {
            visibility: hidden;
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

        function dtval(d, e) {
            var pK = e ? e.which : window.event.keyCode;
            if (pK == 8) { d.value = substr(0, d.value.length - 1); return; }
            var dt = d.value;
            var da = dt.split('/');
            for (var a = 0; a < da.length; a++) { if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1); }
            if (da[0] > 31) { da[1] = da[0].substr(da[0].length - 1, 1); da[0] = '0' + da[0].substr(0, da[0].length - 1); }
            if (da[1] > 12) { da[2] = da[1].substr(da[1].length - 1, 1); da[1] = '0' + da[1].substr(0, da[1].length - 1); }
            if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
            dt = da.join('/');
            if (dt.length == 2 || dt.length == 5) dt += '/';
            d.value = dt;
        }

    </script>
    <script type="text/javascript">
        debugger
        function GetEmpid() {

            $("#<%=txtEmpid.ClientID %>").autocomplete({
                source: function (request, response) {
                    var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpIDs";
                    $.ajax({
                        url: ajaxUrl,
                        method: 'post',
                        contentType: 'application/json;charset=utf-8',

                        data: JSON.stringify({
                            term: request.term,
                        }),
                        datatype: 'json',
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (err) {
                            alert(err);
                        }
                    });
                },
                minLength: 4,
                select: function (event, ui) {

                    $("#<%=txtEmpid.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
            });
            }

            function GetEmpName() {

                $("#<%=txtName.ClientID %>").autocomplete({
                    source: function (request, response) {
                        var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                        var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpNames";
                        $.ajax({

                            url: ajaxUrl,
                            method: 'post',
                            contentType: 'application/json;charset=utf-8',
                            data: JSON.stringify({
                                term: request.term
                            }),
                            datatype: 'json',
                            success: function (data) {
                                response(data.d);
                            },
                            error: function (err) {
                                alert(err);
                            }
                        });
                    },
                    minLength: 4,
                    select: function (event, ui) {
                        $("#<%=txtName.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                    }
                });

                }



                function OnAutoCompletetxtEmpidchange(event, ui) {
                    $("#<%=txtEmpid.ClientID %>").trigger('change');

                    }
                    function OnAutoCompletetxtEmpNamechange(event, ui) {
                        $("#<%=txtName.ClientID %>").trigger('change');

                    }


                    $(document).ready(function () {

                        GetEmpid();
                        GetEmpName();
                    });



    </script>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="LoanReports.aspx" style="z-index: 7;" class="active_bread">LOANS</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">LOANS
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div style="margin-left: 20px">


                                    <div style="float: right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" Text="Export to Excel" OnClick="lbtn_Export_Click"></asp:LinkButton>
                                    </div>
                                    <div class="dashboard_firsthalf" style="width: 100%">
                                        <table width="80%" cellpadding="5" cellspacing="5">
                                            <tr runat="server" id="IDempdetails">
                                                <td>
                                                    <asp:Label runat="server" ID="lblempid" Text="Emp ID" Width="60px"></asp:Label></td>

                                                <td>

                                                    <asp:TextBox ID="txtEmpid" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged" Width="180px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblempname" Text="Emp Name" Width="80px"></asp:Label>

                                                </td>

                                                <td>
                                                    <asp:TextBox ID="txtName" runat="server" TabIndex="2" class="form-control" Width="190px" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                                                </td>

                                            </tr>
                                        </table>

                                    </div>
                                    <div class="rounded_corners" style="overflow: auto">
                                        <asp:GridView ID="GvModifyloandetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CssClass="datagrid" CellPadding="4" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>


                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="EMP ID">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempid" Text='<%# Bind("EmpId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempmname" Text='<%# Bind("Fullname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Loan Id">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblloanno" Text='<%# Bind("LoanNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Loan Type">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblLoanType" Text='<%# Bind("LoanType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Loan Actual Amount">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblloanamount" Text='<%# Bind("LoanActAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Modified Loan Amount">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModifiedLoanAmt" Text='<%# Bind("ModifiedLoanAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="NoInstalments">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblNoInstalments" Text='<%# Bind("NoInstalments") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Modify Type">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModifyType" Text='<%# Bind("ModifyType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Modified By">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModifiedBy" Text='<%# Bind("ModifiedBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Modified Time">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModifiedTime"
                                                            Text='<%#Eval("ModifiedTime", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRemarks" Text='<%# Bind("Remarks") %>'></asp:Label>
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
        <!-- DASHBOARD CONTENT END -->
</asp:Content>
