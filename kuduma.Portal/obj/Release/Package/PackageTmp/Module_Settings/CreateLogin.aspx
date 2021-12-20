<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="CreateLogin.aspx.cs" Inherits="Kuduma.Portal.CreateLogin" %>
 
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 
  <!-- CONTENT AREA BEGIN -->
   <div id="content-holder">
        <div class="content-holder">
              <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Create Login</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
              
                   <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                               Create Login
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                           
                                   <div class="dashboard_firsthalf" style="width: 100%">
                                          <table style="font-family: Arial; font-weight: normal; font-variant: normal; font-size: 13px"
                                                    width="100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            Emp Id<span style="color: Red">*</span>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlempid" runat="server" class="sdrop" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlempid_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblempname" runat="server" Text="Emp Name"> </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlempname" runat="server" class="sdrop" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlempname_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Designation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtdesign" runat="server" Text="" autocomplete="off" AutoCompleteType="None"
                                                                class="sinput" Enabled="false"> </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            Privilege
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlpreviligers" runat="server" class="sdrop">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            User Name<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                                            <asp:TextBox ID="txtusername" runat="server" autocomplete="off" AutoCompleteType="None"
                                                                class="sinput"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Password<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                                            <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" autocomplete="off" AutoCompleteType="None"
                                                                class="sinput"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Confirm Password<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                                            <asp:TextBox ID="txtcpwd" runat="server" TextMode="Password" autocomplete="off" AutoCompleteType="None"
                                                                class="sinput"></asp:TextBox>
                                                            <asp:CompareValidator ID="comparepasword" runat="server" Text="*" ControlToValidate="txtcpwd"
                                                                ControlToCompare="txtpwd">
                                                            </asp:CompareValidator>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div style="float: right;margin-right:20px">
                                            <asp:Label ID="lblresult" runat="server" Visible="false" Text="" Style="color: Red"></asp:Label>
                                            <asp:Button ID="btnsave" runat="server" ValidationGroup="a1" Text="SAVE" ToolTip="SAVE"
                                                OnClientClick='return confirm(" Are you sure  you  want to add the record?");'
                                                class=" btn save" OnClick="btnsave_Click" />
                                            <asp:Button ID="btncancel" runat="server" ValidationGroup="a1" Text="CANCEL" ToolTip="CANCEL"
                                                OnClientClick='return confirm(" Are you  sure you  want to cancel this  entry?");'
                                                class=" btn save" OnClick="btncancel_Click" />
                                        </div>
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                                 </div>
                                       
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvcreatelogin" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    Height="50%" Style="text-align: center" CellPadding="5" CellSpacing="3" ForeColor="#333333"
                                                    GridLines="None" AllowPaging="True" OnRowDeleting="gvcreatelogin_RowDeleting">
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText=" Emp Id">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblempid" runat="server" Text=" <%#Bind('Emp_Id')%>"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="40px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="User Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblname" runat="server" Text="<%#Bind('username')%>">
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="60px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="linkdelete" runat="server" CommandName="delete" Text="Delete"
                                                                    OnClientClick='return confirm("Are you sure you want to delete login details for this employee?"); '></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="40px"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  Height="30" />
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <AlternatingRowStyle BackColor="White" />
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
    </div>
        <!-- CONTENT AREA END -->
</asp:Content>
    