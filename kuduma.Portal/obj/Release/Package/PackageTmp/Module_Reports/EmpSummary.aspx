<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="EmpSummary.aspx.cs" Inherits="Kuduma.Portal.EmpSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
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
                    <li class="active"><a href="EmpSummary.aspx" style="z-index: 7;" class="active_bread">Emp Summary Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Emp Summary Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div align="right">
                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False">Export to Excel</asp:LinkButton>
                                </div>

                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="80%" cellpadding="5" cellspacing="5">


                                        <tr>
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
                                            <td>Type
                                            </td>
                                            <td>

                                                <asp:DropDownList runat="server" ID="ddlloantype" class="form-control">
                                                                </asp:DropDownList>
                                               
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btn_Submit_Click" />
                                            </td>
                                        </tr>
                                        <tr style="visibility: hidden">
                                            <td>From Date<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStrtDate" runat="server" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtStrtDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEstartdate" runat="server" Enabled="True" TargetControlID="txtStrtDate"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>

                                            <td>To Date<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" runat="server" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtEndDate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEEnddate" runat="server" Enabled="True" TargetControlID="txtEndDate"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                              <asp:HiddenField ID="hidGridView" runat="server" />
                                <div id="forExport" class="rounded_corners" style="overflow: scroll" runat="server">
                                    <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                        CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="Both" ShowFooter="true"
                                        >

                                        <Columns>

                                            <%-- 0--%>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text='<%#Bind("sno") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 1--%>
                                            <asp:TemplateField HeaderText="Client ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                <HeaderStyle Width="15px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 2--%>
                                            <asp:TemplateField HeaderText="SITE" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">
                                                <HeaderStyle Width="15px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 3--%>
                                            <asp:TemplateField HeaderText="AADHAAR NO" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAADHAARNO" Text='<%# Bind("AadharNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 4--%>
                                            <asp:TemplateField HeaderText="GDX Id" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblMOB" Text='<%# Bind("MOB") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 5--%>
                                            <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblempid" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 6--%>
                                            <asp:TemplateField HeaderText="BANK" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblempname" runat="server" Text='<%#Bind("Empbankname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                            <%-- 7--%>
                                            <asp:TemplateField HeaderText="IFSC CODE" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIFSCCODE" runat="server" Text='<%#Bind("Empifsccode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                            <%-- 8--%>
                                            <asp:TemplateField HeaderText="Bank A/C No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbankno" runat="server" Text='<%# Eval("EmpBankAcNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 9--%>
                                            <asp:TemplateField HeaderText="UAN NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluanno" runat="server" Text='<%# Eval("UANNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 10--%>
                                            <asp:TemplateField HeaderText="ESI NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblesino" runat="server" Text='<%# Eval("ESINo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 11--%>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 12--%>
                                            <asp:TemplateField HeaderText="Fathers Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFName" runat="server" Text='<%# Eval("FName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 13--%>
                                            <asp:TemplateField HeaderText="SEX">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSEX" runat="server" Text='<%# Eval("SEX") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 14--%>
                                            <asp:TemplateField HeaderText="DOJ">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOJ" runat="server" Text='<%# Eval("DOJ") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 15--%>
                                            <asp:TemplateField HeaderText="D.O.B">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 16--%>
                                            <asp:TemplateField HeaderText="Department">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 17--%>
                                            <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesgn" runat="server" Text='<%#Bind("Desgn") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 18--%>
                                            <asp:TemplateField HeaderText="Month-Year" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmonth" runat="server" Text='<%#Bind("Monthname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblsummary" Text="Summary For"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 19--%>
                                            <asp:TemplateField HeaderText="Fixed Basic" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCdbasic" runat="server" Text='<%#Eval("Cdbasic", "{0:0}") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCdBasic"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 20--%>
                                            <asp:TemplateField HeaderText="Fixed DA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCdda" runat="server" Text='<%#Eval("Cdda","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCdDA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 21--%>
                                            <asp:TemplateField HeaderText="Fixed HRA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCdhra" runat="server" Text='<%#Bind("Cdhra","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCdHRA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 22--%>
                                            <asp:TemplateField HeaderText="Fixed CCA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCdcca" runat="server" Text='<%#Bind("CdCCa","{0:0}") %>'>  
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCdCCA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 23--%>
                                            <asp:TemplateField HeaderText="Fixed Conv" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCdConveyance" runat="server" Text='<%#Bind("Cdconv","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCdConveyance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 24--%>
                                            <asp:TemplateField HeaderText="Fixed W.A." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdwashallowance" runat="server" Text='<%#Bind("cdWashAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdWA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 25--%>
                                            <asp:TemplateField HeaderText="Fixed Nfhs" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdNfhs" runat="server" Text='<%#Bind("cdNfhs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdNfhs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 26--%>
                                            <asp:TemplateField HeaderText="Fixed RC" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdrc" runat="server" Text='<%#Bind("cdrc","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdrc"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 27--%>
                                            <asp:TemplateField HeaderText="Fixed CS" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdcs" runat="server" Text='<%#Bind("cdcs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdcs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 28--%>
                                            <asp:TemplateField HeaderText="Fixed Addl Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdAddlAmount" runat="server" Text='<%#Bind("cdAddlAmount","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lbltTotalcdAddlAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 29--%>
                                            <asp:TemplateField HeaderText="Fixed Food Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdFoodAllowance" runat="server" Text='<%#Bind("CdFoodAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdFoodAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 30--%>
                                            <asp:TemplateField HeaderText="Fixed WO Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdWoAmt" runat="server" Text='<%#Bind("cdWOAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdWOAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 31--%>
                                            <asp:TemplateField HeaderText="Fixed NHs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdNhsAmt" runat="server" Text='<%#Bind("cdNhsamt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdNhsAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 32--%>
                                            <asp:TemplateField HeaderText="Fixed Medical Re-imbursement" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdmedicalallowance" runat="server" Text='<%#Bind("CdMedicalReimbursement","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdmedicalallowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 33--%>
                                            <asp:TemplateField HeaderText="Fixed Special AllW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdSpecialAllowance" runat="server" Text='<%#Bind("CdSpecialAllW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdSpecialAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 34--%>
                                            <asp:TemplateField HeaderText="Fixed Travel Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdTravelAllw" runat="server" Text='<%#Bind("CdTravelAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdTravelAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 35--%>
                                            <asp:TemplateField HeaderText="Fixed Mobile Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdMobileAllowance" runat="server" Text='<%#Bind("CdMobileAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdMobileAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 36--%>
                                            <asp:TemplateField HeaderText="Fixed Performance Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdPerformanceAllw" runat="server" Text='<%#Bind("CdPerformanceAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdPerformanceAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 37--%>
                                            <asp:TemplateField HeaderText="Fixed L.W" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdLeaveEncashAmt" runat="server" Text='<%#Bind("CdLW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdLeaveEncashAmt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 38--%>
                                            <asp:TemplateField HeaderText="Fixed NPOTs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdNpotsAmt" runat="server" Text='<%#Bind("CdNPOTsAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdNpotsAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 39--%>
                                            <asp:TemplateField HeaderText="Fixed Incentivs" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdIncentivs" runat="server" Text='<%#Bind("CdIncentivs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdIncentivs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 40--%>
                                            <asp:TemplateField HeaderText="Fixed Bonus" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdBonus" runat="server" Text='<%#Bind("CdBonus","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdBonus"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 41--%>
                                            <asp:TemplateField HeaderText="Fixed Gratuity" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdGratuity" runat="server" Text='<%#Bind("CdGratuity","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdGratuity"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 42--%>
                                            <asp:TemplateField HeaderText="Fixed O.A." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdOtherallowance" runat="server" Text='<%#Bind("cdOtherallowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdOA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 43--%>
                                            <asp:TemplateField HeaderText="Fixed GWR" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdOTAmt" runat="server" Text='<%#Bind("CdOtAmt1","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdOTAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 44--%>
                                            <asp:TemplateField HeaderText="Fixed Service Weightage" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdServiceWeightage" runat="server" Text='<%#Bind("CdServiceWeightage","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdServiceWeightage"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 45--%>
                                            <asp:TemplateField HeaderText="Fixed Arrears" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdArrears" runat="server" Text='<%#Bind("CdArrears","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdArrears"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 46--%>
                                            <asp:TemplateField HeaderText="Fixed Att Bonus" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdAttBonus" runat="server" Text='<%#Bind("CdAttBonus","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdAttBonus"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 47--%>
                                            <asp:TemplateField HeaderText="Fixed Night Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcdNightAllw" runat="server" Text='<%#Bind("cdNightAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcdNightAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 48--%>
                                            <asp:TemplateField HeaderText="fixed ADDL4HR" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedADDL4HR" runat="server" Text='<%#Bind("fixedADDL4HR","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedADDL4HR"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 49--%>
                                            <asp:TemplateField HeaderText="fixed QTRALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedQTRALLOW" runat="server" Text='<%#Bind("fixedQTRALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedQTRALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 50--%>
                                            <asp:TemplateField HeaderText="fixed RELALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedRELALLOW" runat="server" Text='<%#Bind("fixedRELALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedRELALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 51--%>
                                            <asp:TemplateField HeaderText="fixed SITEALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedSITEALLOW" runat="server" Text='<%#Bind("fixedSITEALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedSITEALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 52--%>
                                            <asp:TemplateField HeaderText="fixed GunAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedGunAllw" runat="server" Text='<%#Bind("fixedGunAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedGunAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 53--%>
                                            <asp:TemplateField HeaderText="fixed FireAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedFireAllw" runat="server" Text='<%#Bind("fixedFireAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedFireAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 54--%>
                                            <asp:TemplateField HeaderText="fixed TelephoneAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedTelephoneAllw" runat="server" Text='<%#Bind("fixedTelephoneAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedTelephoneAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 55--%>
                                            <asp:TemplateField HeaderText="fixed Reimbursement" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedReimbursement" runat="server" Text='<%#Bind("fixedReimbursement","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedReimbursement"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 56--%>
                                            <asp:TemplateField HeaderText="fixed HardshipAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedHardshipAllw" runat="server" Text='<%#Bind("fixedHardshipAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedHardshipAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 57--%>
                                            <asp:TemplateField HeaderText="fixed PaidHolidayAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfixedPaidHolidayAllw" runat="server" Text='<%#Bind("fixedPaidHolidayAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalfixedPaidHolidayAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 58--%>
                                            <asp:TemplateField HeaderText="Duties" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldutyhrs" runat="server" Text='<%#Bind("NoOfDuties") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDuties"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 59--%>
                                            <asp:TemplateField HeaderText="GW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOTs" runat="server" Text='<%#Bind("OTs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOts"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 60--%>
                                            <asp:TemplateField HeaderText="WO" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwos" runat="server" Text='<%#Bind("WO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalwos"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 61--%>
                                            <asp:TemplateField HeaderText="Nhs" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNhs" runat="server" Text='<%#Bind("NHS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNhs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 62--%>
                                            <asp:TemplateField HeaderText="Npots" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNpots" runat="server" Text='<%#Bind("npots") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNpots"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 63--%>
                                            <asp:TemplateField HeaderText="Basic" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbasic" runat="server" Text='<%#Eval("basic", "{0:0}") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalBasic"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 64--%>
                                            <asp:TemplateField HeaderText="DA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblda" runat="server" Text='<%#Eval("da","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 65--%>
                                            <asp:TemplateField HeaderText="HRA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblhra" runat="server" Text='<%#Bind("hra","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalHRA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 66--%>
                                            <asp:TemplateField HeaderText="CCA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcca" runat="server" Text='<%#Bind("CCa","{0:0}") %>'>  
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCCA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 67--%>
                                            <asp:TemplateField HeaderText="Conv" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConveyance" runat="server" Text='<%#Bind("conveyance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalConveyance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 68--%>
                                            <asp:TemplateField HeaderText="W.A." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwashallowance" runat="server" Text='<%#Bind("WashAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalWA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 69--%>
                                            <asp:TemplateField HeaderText="Nfhs" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNfhs" runat="server" Text='<%#Bind("Nfhs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNfhs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 70--%>
                                            <asp:TemplateField HeaderText="RC" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrc" runat="server" Text='<%#Bind("rc","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalrc"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 71--%>
                                            <asp:TemplateField HeaderText="CS" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcs" runat="server" Text='<%#Bind("cs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 72--%>
                                            <asp:TemplateField HeaderText="Addl Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddlAmount" runat="server" Text='<%#Bind("AddlAmount","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lbltTotalAddlAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 73--%>
                                            <asp:TemplateField HeaderText="Food Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFoodAllowance" runat="server" Text='<%#Bind("FoodAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalFoodAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 74--%>
                                            <asp:TemplateField HeaderText="WO Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWoAmt" runat="server" Text='<%#Bind("WOAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalWOAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 75--%>
                                            <asp:TemplateField HeaderText="NHs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNhsAmt" runat="server" Text='<%#Bind("Nhsamt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNhsAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 76--%>
                                            <asp:TemplateField HeaderText="Medical Re-imbursement" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmedicalallowance" runat="server" Text='<%#Bind("medicalallowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalmedicalallowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 77--%>
                                            <asp:TemplateField HeaderText="Special AllW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSpecialAllowance" runat="server" Text='<%#Bind("SpecialAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalSpecialAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 78--%>
                                            <asp:TemplateField HeaderText="Travel Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTravelAllw" runat="server" Text='<%#Bind("TravelAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTravelAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 79--%>
                                            <asp:TemplateField HeaderText="Mobile Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMobileAllowance" runat="server" Text='<%#Bind("MobileAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalMobileAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 80--%>
                                            <asp:TemplateField HeaderText="Performance Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPerformanceAllw" runat="server" Text='<%#Bind("PerformanceAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPerformanceAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 81--%>
                                            <asp:TemplateField HeaderText="L.W" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLeaveEncashAmt" runat="server" Text='<%#Bind("LeaveEncashAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalLeaveEncashAmt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 82--%>
                                            <asp:TemplateField HeaderText="NPOTs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNpotsAmt" runat="server" Text='<%#Bind("Npotsamt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNpotsAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 83--%>
                                            <asp:TemplateField HeaderText="Incentivs" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncentivs" runat="server" Text='<%#Bind("Incentivs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalIncentivs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 84--%>
                                            <asp:TemplateField HeaderText="Bonus" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("Bonus","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalBonus"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 85--%>
                                            <asp:TemplateField HeaderText="Gratuity" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGratuity" runat="server" Text='<%#Bind("Gratuity","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalGratuity"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 86--%>
                                            <asp:TemplateField HeaderText="O.A." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtherallowance" runat="server" Text='<%#Bind("OtherAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 87--%>
                                            <asp:TemplateField HeaderText="GWR" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOTAmt" runat="server" Text='<%#Bind("OTAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOTAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 88--%>
                                            <asp:TemplateField HeaderText="Service Weightage" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceWeightage" runat="server" Text='<%#Bind("ServiceWeightage","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalServiceWeightage"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 89--%>
                                            <asp:TemplateField HeaderText="Arrears" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArrears" runat="server" Text='<%#Bind("Arrears","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalArrears"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 90--%>
                                            <asp:TemplateField HeaderText="Att Bonus" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttBonus" runat="server" Text='<%#Bind("attbonus","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalAttBonus"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 91--%>
                                            <asp:TemplateField HeaderText="Night Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNightAllw" runat="server" Text='<%#Bind("NightAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNightAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 92--%>
                                            <asp:TemplateField HeaderText="ADDL4HR" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblADDL4HR" runat="server" Text='<%#Bind("ADDL4HR","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalADDL4HR"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 93--%>
                                            <asp:TemplateField HeaderText="QTRALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQTRALLOW" runat="server" Text='<%#Bind("QTRALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalQTRALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 94--%>
                                            <asp:TemplateField HeaderText="RELALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRELALLOW" runat="server" Text='<%#Bind("RELALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalRELALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 95--%>
                                            <asp:TemplateField HeaderText="SITEALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSITEALLOW" runat="server" Text='<%#Bind("SITEALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalSITEALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 96--%>
                                            <asp:TemplateField HeaderText="GunAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGunAllw" runat="server" Text='<%#Bind("GunAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalGunAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 97--%>
                                            <asp:TemplateField HeaderText="FireAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFireAllw" runat="server" Text='<%#Bind("FireAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalFireAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 98--%>
                                            <asp:TemplateField HeaderText="TelephoneAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTelephoneAllw" runat="server" Text='<%#Bind("TelephoneAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTelephoneAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 99--%>
                                            <asp:TemplateField HeaderText="Reimbursement" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReimbursement" runat="server" Text='<%#Bind("Reimbursement","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalReimbursement"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 100--%>
                                            <asp:TemplateField HeaderText="HardshipAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHardshipAllw" runat="server" Text='<%#Bind("HardshipAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalHardshipAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 101--%>
                                            <asp:TemplateField HeaderText="PaidHolidayAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidHolidayAllw" runat="server" Text='<%#Bind("PaidHolidayAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPaidHolidayAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 102--%>
                                            <asp:TemplateField HeaderText="Empty" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpty3" runat="server" Text='<%#Bind("Empty","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalEmpty3"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 103--%>
                                            <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalGross"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 104--%>
                                            <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PF","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPF"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 105--%>
                                            <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESI","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalESI"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 106--%>
                                            <asp:TemplateField HeaderText="ProfTax" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProfTax" runat="server" Text='<%#Bind("ProfTax","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalProfTax"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 107--%>
                                            <asp:TemplateField HeaderText="Sal.Adv" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsaladv" runat="server" Text='<%#Bind("SalAdvDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalsaladv"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 108--%>
                                            <asp:TemplateField HeaderText="ADV Ded" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladvded" runat="server" Text='<%#Bind("ADVDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotaladvded"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 109--%>
                                            <asp:TemplateField HeaderText="WC Ded" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwed" runat="server" Text='<%#Bind("WCDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalwed"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 110--%>
                                            <asp:TemplateField HeaderText="U.D." ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluniform" runat="server" Text='<%#Bind("UniformDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalUniformDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 111--%>
                                            <asp:TemplateField HeaderText="Others" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtherDed" runat="server" Text='<%#Bind("OtherDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalOtherDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 112--%>
                                            <asp:TemplateField HeaderText="Total Loan ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltotalloanded" runat="server" Text='<%#Bind("LoanDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotaltotalloanded"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 113--%>
                                            <asp:TemplateField HeaderText="C.A" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcantadv" runat="server" Text='<%#Bind("CanteenAdv","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalcantadv"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 114--%>
                                            <asp:TemplateField HeaderText="Sec Dep" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSeepDed" runat="server" Text='<%#Bind("SecurityDepDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalSeepDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 115--%>
                                            <asp:TemplateField HeaderText="Gen Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGeneralDed" runat="server" Text='<%#Bind("GeneralDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalGeneralDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>




                                            <%-- 116--%>
                                            <asp:TemplateField HeaderText="LWF" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblowf" runat="server" Text='<%#Bind("OWF","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalowf"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 117--%>
                                            <asp:TemplateField HeaderText="Penalty" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenalty" runat="server" Text='<%#Bind("Penalty","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalPenalty"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 118--%>
                                            <asp:TemplateField HeaderText="ATM Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRentDed" runat="server" Text='<%#Bind("ATMDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalRentDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 119--%>

                                            <asp:TemplateField HeaderText="Medical Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMedicalDed" runat="server" Text='<%#Bind("MedicalDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalMedicalDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 120--%>
                                            <asp:TemplateField HeaderText="MLWF Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMLWFDed" runat="server" Text='<%#Bind("MLWFDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalMLWFDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 121--%>
                                            <asp:TemplateField HeaderText="Food Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFoodDed" runat="server" Text='<%#Bind("FoodDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalFoodDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 122--%>
                                            <asp:TemplateField HeaderText="IDCard Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblElectricityDed" runat="server" Text='<%#Bind("IDCardDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalElectricityDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 123--%>
                                            <asp:TemplateField HeaderText="Rent Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransportDed" runat="server" Text='<%#Bind("RentDed1","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalTransportDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 124--%>
                                            <asp:TemplateField HeaderText="Other Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDced" runat="server" Text='<%#Bind("Finesded1","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalDced"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 125--%>
                                            <asp:TemplateField HeaderText="PVC Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLeaveDed" runat="server" Text='<%#Bind("PVCDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalLeaveDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 126--%>
                                            <asp:TemplateField HeaderText="License Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLicenseDed" runat="server" Text='<%#Bind("LicenseDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalLicenseDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 127--%>
                                            <asp:TemplateField HeaderText="Adv4 Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdv4Ded" runat="server" Text='<%#Bind("Adv4Ded","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalAdv4Ded"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 128--%>
                                            <asp:TemplateField HeaderText="Extra" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNightRoundDed" runat="server" Text='<%#Bind("Extra","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalNightRoundDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 129--%>
                                            <asp:TemplateField HeaderText="ManpowerMob Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblManpowerMobDed" runat="server" Text='<%#Bind("ManpowerMobDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalManpowerMobDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 130--%>
                                            <asp:TemplateField HeaderText="Mobileusage Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMobileusageDed" runat="server" Text='<%#Bind("MobileusageDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalMobileusageDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 131--%>
                                            <asp:TemplateField HeaderText="MediClaim Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMediClaimDed" runat="server" Text='<%#Bind("MediClaimDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalMediClaimDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 132--%>
                                            <asp:TemplateField HeaderText="Crisis Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrisisDed" runat="server" Text='<%#Bind("CrisisDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalCrisisDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 133--%>
                                            <asp:TemplateField HeaderText="Telephone Bill Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTelephoneBillDed" runat="server" Text='<%#Bind("TelephoneBillDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalTelephoneBillDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                                <%-- 135--%>
                                            <asp:TemplateField HeaderText="Registration Fee" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationFee" runat="server" Text='<%#Bind("RegistrationFee","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalRegistrationFee"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>




                                            <%-- 134--%>
                                            <asp:TemplateField HeaderText="Total Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeductions" runat="server" Text='<%#Bind("TotalDeductions","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDeductions"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 135--%>
                                            <asp:TemplateField HeaderText="Net Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnetamount" runat="server" Text='<%#Bind("ActualAmount","{0:0}") %>'> </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNetAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 136--%>
                                            <asp:TemplateField HeaderText="Signature orThumb Impression of the Workman" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                        <PagerStyle BackColor="white" ForeColor="black" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                        <EditRowStyle BackColor="white" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                    </div>

                                <asp:Panel ID="pnlloans" runat="server" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" style="font-weight: bold">
                                                <asp:Label ID="lblloanissued" runat="server" Text="Loan Issued" Visible="false"></asp:Label></td>
                                            <td align="center" style="font-weight: bold">
                                                <asp:Label ID="lblloandeducted" runat="server" Text="Loan Deducted" Visible="false"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <div class="rounded_corners">

                                                    <asp:GridView ID="GVLoanIssued" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true"
                                                        Height="50px" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVLoanIssued_RowDataBound">
                                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Loan No">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblloanno" Text="<%# Bind('LoanNo') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Loan Type">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblLoanType" Text="<%# Bind('TypeOfLoan') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Issued Amount" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblissuedAmount" Text="<%# Bind('LoanAmount') %>"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label runat="server" ID="lblTotalissuedAmount"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="No.of Instalments" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblnoofinstalments" Text="<%# Bind('NoInstalments') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblStatus" Text="<%# Bind('LoanStatus') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Issued Date">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblissueddate" Text="<%# Bind('LoanIssuedDate') %>"></asp:Label>
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
                                            </td>


                                            <td valign="top">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="GVLoanDeducted" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true"
                                                        Height="50px" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVLoanDeducted_RowDataBound">
                                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Loan No">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbldedloanno" Text="<%# Bind('LoanNo') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Loan Type">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="Lbldedloantype" Text="<%# Bind('LoanType') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Deducted Amount" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="LblDeductedAmount" Text="<%# Bind('RecAmt') %>"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label runat="server" ID="lblTotalDeductedAmount"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Client ID/Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="LblClintidorname" Text="<%# Bind('ClientID') %>"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Month">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="Lblmonth" Text="<%# Bind('LoanCuttingMonth') %>" Width="50px"></asp:Label>
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
                                            </td>
                                        </tr>
                                    </table>

                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblbalance" runat="server" Text="Balance" Style="font: Bold" Enabled="false"> </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtbalance" runat="server" Text="" class="sinput"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>

                                <div style="margin-top: 20px; margin-left: 20px">
                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"></asp:Label>
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
