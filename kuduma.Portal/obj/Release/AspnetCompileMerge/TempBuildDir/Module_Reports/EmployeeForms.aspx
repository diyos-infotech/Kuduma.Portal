<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="EmployeeForms.aspx.cs" Inherits="Kuduma.Portal.EmployeeForms" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
     <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
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
        .style2
        {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="EmployeeForms.aspx" style="z-index: 7;" class="active_bread">Employee Forms</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                              Employee Forms
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="120%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                            Forms</td>
                                             <td>  <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlForms" class="sdrop"
                                                    OnSelectedIndexChanged="ddlForms_SelectedIndexChanged">
                                                 <asp:ListItem>--Select--</asp:ListItem>
                                                 <asp:ListItem>Form Q</asp:ListItem>
                                                 <asp:ListItem>Form F (Leave Wages)</asp:ListItem>
                                                 <asp:ListItem>Form F (Gratuity)</asp:ListItem>
                                                 <asp:ListItem>Form A</asp:ListItem>
                                                 <asp:ListItem>Form 5</asp:ListItem>
                                                 <asp:ListItem>Form 13</asp:ListItem>
                                                 <asp:ListItem>Declaration</asp:ListItem>
                                                 <asp:ListItem>Form-3A</asp:ListItem>
                                               

                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Lblempid" runat="server" Text="Employee ID " Visible="false"></asp:Label> </td>
                                              <td>  <%--<asp:DropDownList runat="server" AutoPostBack="true" ID="ddlEmployee" class="sdrop" Visible="false"
                                                    OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                  <asp:TextBox runat="server" ID="txtemplyid" class="form-control" AutoPostBack="true" OnTextChanged="txtemplyid_TextChanged" Width="170px" Visible="false"></asp:TextBox>
                                            </td>
                                            <td>
                                               <asp:Label ID="lblempname" runat="server" Text=" Employee Name " Visible="false"></asp:Label></td>
                                               <td> <%--<asp:DropDownList runat="server" AutoPostBack="true" ID="ddlempname" class="sdrop" Visible="false"
                                                    OnSelectedIndexChanged="ddlempname_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                    <asp:TextBox runat="server" ID="txtFname" class="form-control" AutoPostBack="true" OnTextChanged="txtFname_TextChanged" Width="170px" Visible="false"></asp:TextBox>
                                            </td>
                                           
                                             </tr>
                                             <tr >
                                                        <td style="width: 100px">
                                                            <asp:Label ID="lblfrom" runat="server" Text="From" Visible="false"></asp:Label>
                                                        </td>

                                                        <td>
                                                            <asp:TextBox ID="txtfrom" runat="server" CssClass="sinput" Visible="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtfrom_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtfrom">
                                                            </cc1:CalendarExtender>
                                                        </td>

                                                       
                                                        <td style="width: 100px">
                                                            <asp:Label ID="lblto" runat="server" Text="To" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtto" runat="server" CssClass="sinput" Visible="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtto_CalendarExtender" runat="server" BehaviorID="calendar2"
                                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtto">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                        <tr>
                                             <td>
                                                            <asp:Label ID="lblmonth" runat="server" Text="Month" Visible="false"></asp:Label>

                                                </td>
                                         <td>
                                                    <asp:TextBox ID="TxtMonth" Width="120px" runat="server" AutoPostBack="true" class="sinput"
                                                        Text="" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="TxtMonth_CalendarExtender" runat="server"
                                                        Enabled="true" Format="dd/MM/yyyy" TargetControlID="TxtMonth">
                                                    </cc1:CalendarExtender>

                                                </td>

                                             <td>
                                                    <asp:Label ID="lblDOJ" runat="server" Text="  Date Of Joining" Visible="false" ></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmpDtofJoining" runat="server" Text="" class="sinput" Visible="false" ></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                        TargetControlID="txtEmpDtofJoining" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOI1" runat="server" Enabled="True" TargetControlID="txtEmpDtofJoining"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>


                                              <td>
                                                    <asp:Label ID="lblDOL" runat="server" Text="  Date Of Leaving" Visible="false" Style="margin-left: -124px"></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmpDtofLeaveing" runat="server" Text="" class="sinput" Visible="false" Style="margin-left: -377px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                        TargetControlID="txtEmpDtofJoining" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOL1" runat="server" Enabled="True" TargetControlID="txtEmpDtofLeaveing"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                             
                                       </tr>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                           
                                        </tr>
                                    </table>
                                    <asp:Button runat="server" ID="BtnSubmit" Text="Submit" class="btn save" OnClick="btnForms_Click" style="float:right;margin-right:90px"
                                                    />
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
