<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="LWFMaster.aspx.cs" Inherits="Kuduma.Portal.LWFMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">LWF Master</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">LWF Master
                            </h2>
                        </div>
                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>State<span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlStates" runat="server" class="sdrop" AutoPostBack="true" OnSelectedIndexChanged="DdlStates_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td>Deduct Type<span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDeductType" runat="server" class="sdrop" OnSelectedIndexChanged="ddlDeductType_SelectedIndexChanged1" AutoPostBack="true">
                                                <asp:ListItem>--Select--</asp:ListItem>
                                                <asp:ListItem>Monthly</asp:ListItem>
                                                <asp:ListItem>Quarterly</asp:ListItem>
                                                <asp:ListItem>Half Yearly</asp:ListItem>
                                                <asp:ListItem>Yearly</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                        <td>
                                            <asp:Label ID="lblmonth" runat="server" Text="Month" Visible="false"></asp:Label><span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlMonth" runat="server" class="sdrop" Visible="false"></asp:DropDownList>
                                        </td>


                                    </tr>
                                    <tr>
                                        <td>Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddltype" runat="server" class="sdrop" OnSelectedIndexChanged="ddltype_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem>Amount</asp:ListItem>
                                                <asp:ListItem>Percentage</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                        <td>
                                            <asp:Label ID="lblperon" runat="server" Text="Per On" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlperon" runat="server" Visible="false" class="sdrop">
                                                <asp:ListItem>Gross</asp:ListItem>
                                                <asp:ListItem>Gross+OT</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>


                                    </tr>
                                    <tr>
                                        <td>Employee Contribution
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmployeeContribution" Text="0" runat="server" class="sinput"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTBEDOB" runat="server" Enabled="True" TargetControlID="txtEmployeeContribution"
                                                ValidChars=".0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Employer Contribution
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmployeerContribution" Text="0" runat="server" class="sinput"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtEmployeerContribution"
                                                ValidChars=".0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Maximum
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMaximum" Text="0" runat="server" class="sinput"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtMaximum"
                                                ValidChars=".0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnsave" runat="server" Text="Save" class="btn save"
                                                OnClick="btnsave_Click" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>

                                <div>
                                    <table width="100%">
                                        <tr style="float: right">
                                            <td></td>
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
        <!-- CONTENT AREA END -->
    </div>
</asp:Content>
