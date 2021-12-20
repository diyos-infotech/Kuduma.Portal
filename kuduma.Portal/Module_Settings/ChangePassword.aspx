<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="ChangePassword.aspx.cs" Inherits="Kuduma.Portal.ChangePassword" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Change Password</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Change Password
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table style="font-family: Arial; font-weight: normal; font-variant: normal; font-size: 13px"
                                        cellpadding="5" cellspacing="5" width="45%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblempid" runat="server" Text="Emp Id"></asp:Label>
                                                <span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlempid" runat="server" class="sdrop">
                                                    <asp:ListItem Value="0">Select Username</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbloldpassword" runat="server" Text="Old Password"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtoldpassword" runat="server" TextMode="Password" class="sinput"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                New Password<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtnewpassword" runat="server" TextMode="Password" class="sinput"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Confirm Password<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtconfirmpassword" runat="server" TextMode="Password" class="sinput"></asp:TextBox>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                        <td>&nbsp;</td>

                                        <td>
                                        <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"> </asp:Label></td></tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                        <asp:Button ID="Button2" runat="server" ValidationGroup="a1" Text="SAVE" ToolTip="SAVE"
                                            OnClientClick='return confirm(" Are you sure  you  want to change password ?");'
                                            class=" btn save" OnClick="Button2_Click" />
                                        <asp:Button ID="Button3" runat="server" ValidationGroup="a1" Text="CANCEL" ToolTip="CANCEL"
                                            OnClientClick='return confirm(" Are you sure you  want to cancel the change password?");'
                                            class=" btn save" />
                                    </td>
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
        <!-- DASHBOARD CONTENT END -->
    </div>
  <!-- CONTENT AREA END -->
    </asp:Content>   