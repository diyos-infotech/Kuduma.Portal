<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="PFBranches.aspx.cs" Inherits="Kuduma.Portal.PFBranches" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- CONTENT AREA BEGIN -->
             <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">PF Branches</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                PF BRANCHES
                            </h2>
                        </div>
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                 <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPFbranch" runat="server" Text="PF Branch :" class="fontstyle"></asp:Label><span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPFbranchname" runat="server" class="sinput"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblpfno" runat="server" Text="PF No :" class="fontstyle"></asp:Label><span style="color: Red">*</span>
                                            </td>

                                             <td>
                                                <asp:TextBox ID="txtpfno" runat="server" class="sinput"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnAddPFbranch" runat="server" Text="Add PF Branch" class="btn save"
                                                    Width="120px"  
                                                    OnClientClick='return confirm(" Are you sure you want to add the Bank Name?");' 
                                                    onclick="btnAddPFbranch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvPFbranches" runat="server" AutoGenerateColumns="false" Width="100%"
                                             CellPadding="5" CellSpacing="3" AllowPaging="True" PageSize="15" ForeColor="#333333"
                                            GridLines="None" onpageindexchanging="gvpfbranches_PageIndexChanging" 
                                            onrowcancelingedit="gvpfbranches_RowCancelingEdit" 
                                            onrowediting="gvpfbranches_RowEditing" 
                                            onrowupdating="gvpfbranches_RowUpdating">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PF Branch" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="70%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPFbranchname" runat="server" Text="<%#Bind('PFBranchname') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtPFbranchname" runat="server" Text="<%#Bind('PFBranchname') %>" Width="500px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="PF NO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="70%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPFNO" runat="server" Text="<%#Bind('PFNO') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtPFNO" runat="server" Text="<%#Bind('PFNO') %>" Width="500px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBranchid" runat="server" Text="<%#Bind('PFBranchid') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblBranchid" runat="server" Text="<%#Bind('PFBranchid') %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OPERATIONS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkedit" runat="server" Visible="false" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Visible="false" Text="Update"
                                                            OnClientClick='return confirm(" Are you  sure you  want to update  the PF Branch?");'  style="color:Black"></asp:LinkButton>
                                                        <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel"  Visible="false" Text="Cancel"
                                                            OnClientClick='return confirm(" Are you  sure you  want to cancel  the PF Branch?");'  style="color:Black"></asp:LinkButton>
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
 </asp:Content>
