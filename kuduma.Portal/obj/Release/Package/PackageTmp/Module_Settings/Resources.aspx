<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="Resources.aspx.cs" Inherits="Kuduma.Portal.Resources" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Resources</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Resources
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="90%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblResource" runat="server" Text="Enter  Resource Name" class="fontstyle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlResource" runat="server" class="sdrop">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblresourceprice" runat="server" Text="Enter Resource Price" class="fontstyle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtResourceprice" runat="server" class="sinput"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                    TargetControlID="txtResourceprice" ValidChars=".0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnResource" runat="server" Text="Add Resource" class="btn save"
                                                    Width="120px" OnClick="Btn_Resource_Click" OnClientClick='return confirm(" Are you sure you want to add the Resource?");' />
                                            </td>
                                        </tr>
                                    </table>
                                   
                                     </div>
                                    <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>
                                   
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvResource" runat="server" AutoGenerateColumns="false" Width="100%"
                                            OnRowCancelingEdit="gvResource_RowCancelingEdit" OnRowEditing="gvResource_RowEditing1"
                                            OnRowUpdating="gvResource_RowUpdating1" AllowPaging="True" OnPageIndexChanging="gvResource_PageIndexChanging"
                                            PageSize="15" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Categories" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResourceName" runat="server" Text="<%#Bind('ItemName') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtResourceName" runat="server" Text="<%#Bind('ItemName') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResourcePrice" runat="server" Text="<%#Bind('Price') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtResourcePrice" runat="server" Text="<%#Bind('Price') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResourceid" runat="server" Text="<%#Bind('Resourceid') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblResourceid" runat="server" Text="<%#Bind('Resourceid') %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OPERATIONS">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                            OnClientClick='return confirm(" Are you  sure you  want to update  the Resource?");' style="color:Black"></asp:LinkButton>
                                                        <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                            OnClientClick='return confirm(" Are you  sure you  want to cancel  the Resource?");' style="color:Black"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" BorderWidth="1px" CssClass="GridPager" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                      <EditRowStyle ForeColor="#000" BackColor="#C2D69B" />
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
        
        <!-- CONTENT AREA END -->
    </div>
    </asp:Content>
