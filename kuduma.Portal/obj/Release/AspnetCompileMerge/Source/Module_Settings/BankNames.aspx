<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="BankNames.aspx.cs" Inherits="Kuduma.Portal.BankNames" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Bank Names</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Bank Names
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                 <table width="45%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblsegment" runat="server" Text="Bank Name :" class="fontstyle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_Bank_Name" runat="server" class="sinput"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="Btn_Bank_Name_Add" runat="server" Text="Add Bank Name" class="btn save"
                                                    Width="120px" OnClick="Btn_Bank_Name_Add_Click" OnClientClick='return confirm(" Are you sure you want to add the Bank Name?");' />
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvbank" runat="server" AutoGenerateColumns="false" Width="100%"
                                            OnRowCancelingEdit="gvbank_RowCancelingEdit" CellPadding="5" CellSpacing="3"
                                            OnRowEditing="gvbank_RowEditing1" OnRowUpdating="gvbank_RowUpdating1" AllowPaging="True"
                                            OnPageIndexChanging="gvbank_PageIndexChanging" PageSize="15" ForeColor="#333333"
                                            GridLines="None">
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
                                                <asp:TemplateField HeaderText="BANK NAME" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="70%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbankName" runat="server" Text="<%#Bind('bankname') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtbankName" runat="server" Text="<%#Bind('BankName') %>" Width="500px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbankid" runat="server" Text="<%#Bind('bankid') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblbankid" runat="server" Text="<%#Bind('bankid') %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OPERATIONS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                            OnClientClick='return confirm(" Are you  sure you  want to update  the Bank Name?");'  style="color:Black"></asp:LinkButton>
                                                        <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                            OnClientClick='return confirm(" Are you  sure you  want to cancel  the Bank Name?");'  style="color:Black"></asp:LinkButton>
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