<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="MeasuredUnits.aspx.cs" Inherits="Kuduma.Portal.MeasuredUnits" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Measured Units</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Measured Units
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table width="40%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUnitName" runat="server" Text="Unit Name" class="fontstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtunitname" runat="server" class="sinput"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnunitname" runat="server" Text="Add Unit" class="btn save" 
                                                OnClick="btnunitname_Click" OnClientClick='return confirm(" Do You Want to Add The Record?");' />
                                            <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvmeasuredunits" runat="server" GridLines="None"  Width="100%" AutoGenerateColumns="False"
                                                    Height="50%" Style="text-align: center" CellPadding="4" CellSpacing="3" ForeColor="#333333" OnRowCancelingEdit="gvmeasuredunits_RowCancelingEdit" OnRowDeleting="gvmeasuredunits_RowDeleting1"
                                        OnRowEditing="gvmeasuredunits_RowEditing1" OnRowUpdating="gvmeasuredunits_RowUpdating1">
                                         <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Unit Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunitid" runat="server" Text="<%#Bind('unitid') %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblunitid" runat="server" Text="<%#Bind('unitid') %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnitName" runat="server" Text="<%#Bind('unitmeasure') %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtUnitName" runat="server" Text="<%#Bind('unitmeasure') %>"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" Text="Delete"
                                                        OnClientClick='return confirm(" Do You Want to Add The Record?");'></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update" style="color:Black"></asp:LinkButton>
                                                    <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel" style="color:Black"></asp:LinkButton>
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
    <!-- CONTENT AREA END -->
    </div>
    </asp:Content>
    