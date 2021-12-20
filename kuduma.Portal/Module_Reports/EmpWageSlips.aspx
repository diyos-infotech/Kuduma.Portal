<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="EmpWageSlips.aspx.cs" Inherits="Kuduma.Portal.EmpWageSlips" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <script src="script/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="script/jscript.js"> </script>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>




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

            $("#<%=txtemplyid.ClientID %>").autocomplete({
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

                    $("#<%=txtemplyid.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
            });
            }

            function GetEmpName() {

                $("#<%=txtFname.ClientID %>").autocomplete({
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
                            $("#<%=txtFname.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                    }
                    });

                }

                function OnAutoCompletetxtEmpidchange(event, ui) {
                    $("#<%=txtemplyid.ClientID %>").trigger('change');

                }
                function OnAutoCompletetxtEmpNamechange(event, ui) {
                    $("#<%=txtFname.ClientID %>").trigger('change');

            }

            $(document).ready(function () {

                GetEmpid();
                GetEmpName();
            });



    </script>


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
            height: 120px;
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
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="EmpWageSlips.aspx" style="z-index: 7;" class="active_bread">EMPLOYEE WAGE SLIPS</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">WAGE SLIPS
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div style="margin-left: 20px">
                                    <div>


                                        <div class="dashboard_firsthalf">

                                            <table cellpadding="5" cellspacing="5">

                                                <tr style="height: 35px">
                                                    <td style="width: 100px">Emp ID<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtemplyid" class="form-control" AutoPostBack="true" OnTextChanged="txtemplyid_TextChanged" Width="200px"></asp:TextBox>


                                                    </td>
                                                </tr>

                                                <tr style="height: 35px">
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                                                    </td>

                                                    <td>
                                                        <asp:TextBox ID="txtfrom" runat="server" AutoComplete="off" class="form-control" Width="200px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtfrom_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                            Enabled="true" Format="MMM-yyyy" TargetControlID="txtfrom">
                                                        </cc1:CalendarExtender>
                                                    </td>

                                                </tr>
                                            </table>
                                        </div>
                                        <div class="dashboard_secondhalf">

                                            <table cellpadding="5" cellspacing="5">

                                                <tr style="height: 36px">
                                                    <td style="width: 150px">Employee Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtFname" class="form-control" AutoPostBack="true" OnTextChanged="txtFname_TextChanged" Width="200px"></asp:TextBox>



                                                    </td>
                                                </tr>

                                                <tr style="height: 36px">

                                                    <td style="width: 150px">
                                                        <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtto" runat="server" AutoComplete="off" class="form-control" Width="200px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtto_CalendarExtender" runat="server" BehaviorID="calendar2"
                                                            Enabled="true" Format="MMM-yyyy" TargetControlID="txtto">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                            </table>


                                        </div>
                                        <br />
                                        <br />
                                        <table width="43%" align="right">


                                            <tr>
                                                <td>
                                                    <asp:Button ID="btndownload" runat="server" Text="View Details" class="btn save" OnClick="btndownload_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPayslip" runat="server" Text="Download Pay Slips" class="btn save" OnClick="btnEmpWageSlip_Click" />
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btnEmployeeWageSlip" runat="server" Text="Download Pay Slips(Dts)" class="btn save" OnClick="btnEmpOnlyDtsWageSlip_Click" /></td>
                                            </tr>
                                        </table>

                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvattendancezero" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center; margin: 0px auto" Height="50px" HeaderStyle-HorizontalAlign="Center"
                                                OnRowDataBound="gvattendancezero_RowDataBound">

                                                <Columns>

                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Client Id" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClientId" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Client Name " ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClientName" runat="server" Text='<%#Bind("clientname") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Month " ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMonth" runat="server" Text='<%#Bind("month") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Emp Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpName" runat="server" Text='<%#Bind("EmpmName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Designation" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldesgn" runat="server" Text='<%#Bind("Desgn") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duties" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDuties" runat="server" Text='<%#Bind("NoOfDuties") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Salary Rate" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalaryRate" runat="server" Text='<%#Bind("salaryrate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PF") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESI") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Deductions" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalDeductions" runat="server" Text='<%#Bind("Deductions") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="NetPay " ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNetPay" runat="server" Text='<%#Bind("ActualAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <br />
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

                <script type="text/javascript">
                    Sys.Browser.WebKit = {};
                    if (navigator.userAgent.indexOf('WebKit/') > -1) {
                        Sys.Browser.agent = Sys.Browser.WebKit;
                        Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                        Sys.Browser.name = 'WebKit';
                    }

                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    if (prm != null) {
                        prm.add_endRequest(function () {

                            GetEmpid();
                            GetEmpName();


                        });
                    };
                </script>
</asp:Content>
