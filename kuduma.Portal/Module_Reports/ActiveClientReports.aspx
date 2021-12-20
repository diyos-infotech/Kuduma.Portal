<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ActiveClientReports.aspx.cs" Inherits="Kuduma.Portal.ActiveClientReports" %>


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
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="ActiveClientReports.aspx" style="z-index: 7;" class="active_bread">List of Clients</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <asp:ScriptManager runat="server" ID="Scriptmanager1"></asp:ScriptManager>
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">List of Clients
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">


                                    <table width="80%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>Client Mode:</td>
                                            <td>
                                                <asp:DropDownList ID="ddlClientsList" class="sdrop" runat="server" OnSelectedIndexChanged="ddlClientsList_SelectedIndexChanged" AutoPostBack="true">

                                                    <asp:ListItem Text="--Select--"></asp:ListItem>
                                                    <asp:ListItem Text="All"></asp:ListItem>
                                                    <asp:ListItem Text="Active"></asp:ListItem>
                                                    <asp:ListItem Text="InActive"></asp:ListItem>
                                                    <asp:ListItem Text="Invoice"></asp:ListItem>
                                                    <asp:ListItem Text="Paysheet"></asp:ListItem>
                                                    <asp:ListItem Text="Invoice/Paysheet"></asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td>Branch:</td>
                                            <td>
                                                <asp:DropDownList ID="ddlbranchnames" runat="server" Width="200px" class="dropdown-field">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="MessageLabel" runat="server" /></td>
                                            <td>
                                                <asp:Button ID="Submit" Text="Submit" runat="server" OnClick="Submit_Click" class="btn save" /></td>
                                            <div align="right">
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" OnClientClick="AssignExportHTML()" Visible="false">Export to Excel</asp:LinkButton>
                                            </div>
                                        </tr>
                                    </table>
                                </div>
                                <div class="dashboard_secondhalf">
                                    <table>

                                        <tr>
                                            <td>
                                                <%--       <asp:Button runat="server" ID="btn_Submit" Text="Submit" OnClick="btn_Submit_Click"
                                                class="btn save" />
                                                --%>        </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners">
                                    <%--<asp:GridView ID="GVListOfClients" runat="server" AutoGenerateColumns="False" Width="100%"
                                   CellPadding="5" CellSpacing="3" PageSize="11" GridLines="None" CssClass="table table-striped table-bordered table-condensed table-hover">--%>
                                    <asp:GridView ID="GVListOfClients" runat="server" AutoGenerateColumns="false" PageSize="11" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                        CellPadding="4" ForeColor="#333333">
                                        <%-- <RowStyle BackColor="#EFF3FB" Height="30" />--%>
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Client ID">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblClientid" Text="<%# Bind('Clientid') %>"></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblclientName" Text="<%# Bind('clientName') %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="H.no">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblclientAddrHno" Text="<%# Bind('ClientAddrHno') %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Street">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblClientStreet" Text="<%# Bind('ClientAddrStreet') %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="City">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCCity" Text="<%# Bind('ClientAddrCity') %>"></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="State">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCState" Text="<%# Bind('ClientAddrState') %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PhoneNo(s)">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPhonenumbers" Text="<%# Bind('ClientPhonenumbers') %>"></asp:Label>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email Id">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCEmail" Text="<%# Bind('ClientEmail') %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GST No">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCEmail" Text="<%# Bind('GSTIN') %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_Client_Status" runat="server" Text="<%# Bind('Clientstatus') %>">"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" HorizontalAlign="Center"
                                            BorderWidth="1px" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>
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
