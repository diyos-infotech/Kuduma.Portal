<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="AndroidAttendanceSummarisedReport.aspx.cs" Inherits="Kuduma.Portal.AndroidAttendanceSummarisedReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">


    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <script src="../js/colResizable-1.6.js"></script>
    <script src="../js/colResizable-1.6.min.js"></script>
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

        $(function () {
            $('#<%=GvDayWiseAttendance.ClientID %>').colResizable({
                liveDrag: true,
                resizeMode: 'overflow',
                postbackSafe: true,
                gripInnerHtml: "<div class='grip'></div>",
                draggingClass: "dragging"
            });
        });

    </script>

    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Android Attendance Summarised Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Android Attendance Summarised Report 
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">

                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>

                                <div style="margin-right: 10px; float: right">
                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                </div>

                                <div>
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                           
                                            <td>Month
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" class="form-control" Width="120px"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtmonth" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                    ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                            </td>


                                        </tr>
                                       
                                    </table>
                                </div>

                            <div class="rounded_corners" style="overflow-x: scroll; width: 97%; margin-left: 17px; margin-bottom: 30px">
                                <asp:GridView ID="GvDayWiseAttendance" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                    Width="100%" CellPadding="4" CellSpacing="3" CssClass="table table-striped table-bordered table-condensed table-hover">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Clientid">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranch" runat="server" Text='<%# Eval("Clientid") %>'></asp:Label>
                                                </ItemTemplate>
                                             <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotal" Text="Total"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Client Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("ClientName") %>'></asp:Label>
                                                </ItemTemplate>
                                             <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblName" Text=" "></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Total Employees">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalEmployees" runat="server" Text='<%# Eval("TotalEmployees") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lbltotalTotalEmployees"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Attendance Given">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendanceGiven" runat="server" Text='<%# Eval("AttendanceGiven") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lbltotalAttendanceGiven"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Attendance Not Given">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendanceNotGiven" runat="server" Text='<%# Eval("AttendanceNotGiven") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lbltotalAttendanceNotGiven"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
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
    </div>

</asp:Content>
