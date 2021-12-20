<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="BloodGroups.aspx.cs" Inherits="Kuduma.Portal.BloodGroups" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Blood Groups</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Blood Groups
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table>
                                    <tr>
                                        <td>
                                            Enter The Blood Group :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_BloodGroup" runat="server" class="sinput"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="Btn_BloodGroup" runat="server" Text="Add BloodGroup" class="btn save"
                                                Width="120px" OnClick="Btn_BloodGroup_Click" OnClientClick='return confirm(" Are you sure you want to add the segment?");' />
                                        </td>
                                    </tr>
                                </table>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvBloodGroup" runat="server" AutoGenerateColumns="false" Width="100%"
                                        OnRowCancelingEdit="gvBloodGroup_RowCancelingEdit" OnRowEditing="gvBloodGroup_RowEditing"
                                        OnRowUpdating="gvBloodGroup_RowUpdating" OnPageIndexChanging="gvBloodGroup_PageIndexChanging"
                                        PageSize="15" CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None">
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
                                            <asp:TemplateField HeaderText="Blood Group">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBloodGroupName" runat="server" Text="<%#Bind('BloodGroupName') %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtBloodGroupName" runat="server" Text="<%#Bind('BloodGroupName') %>"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBloodGroupid" runat="server" Text="<%#Bind('BloodGroupid') %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblBloodGroupid" runat="server" Text="<%#Bind('BloodGroupid') %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Operations">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                        OnClientClick='return confirm(" Are you  sure you  want to update  the Blood Group?");' style="color:Black"></asp:LinkButton>
                                                    <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                        OnClientClick='return confirm(" Are you  sure you  want to cancel  the Blood Group?");' style="color:Black"></asp:LinkButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" BorderWidth="1px" CssClass="GridPager" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Height="30" ForeColor="White" />
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