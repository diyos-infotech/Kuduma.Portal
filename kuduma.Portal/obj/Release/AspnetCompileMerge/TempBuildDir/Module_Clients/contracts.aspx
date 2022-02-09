<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contracts.aspx.cs" MasterPageFile="~/Module_Clients/Clients.master" Inherits="Kuduma.Portal.contracts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                <div align="center"> <asp:Label ID="lblMsg" runat="server" style="border-color: #f0c36d;background-color: #f9edbe;width:auto;font-weight:bold;color:#CC3300;"></asp:Label></div> 
              <div align="center"> <asp:Label ID="lblSuc" runat="server" style="border-color: #f0c36d;background-color: #f9edbe;width:auto;font-weight:bold;color:#000;"></asp:Label></div> 
                     <table style="margin-top:8px;margin-bottom:8px" width="100%">
                        <tr>
                            <td style="font-weight: bold;width:100px">
                                Client ID/Name :
                            </td>
                           <td style="width:190px">
                                &nbsp;<asp:TextBox ID="txtsearch" runat="server" class="sinput"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" class=" btn save" OnClick="btnSearch_Click" />
                            </td>
                             <td align="right"><a href="AddContract.aspx" class=" btn save">Add New Contract</a></td>
                        </tr>
                    </table>
                    <div class="col-md-12">
                        <div class="panel panel-inverse">
                            <div class="panel-heading">
                                <h3 class="panel-title">
                                    Contract Details</h3>
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvcontract" runat="server" CellPadding="2" ForeColor="Black"
                                    AutoGenerateColumns="False" Width="100%" BackColor="#f9f9f9" BorderColor="LightGray"
                                    BorderWidth="1px" AllowPaging="True" onrowdeleting="gvDetails_RowDeleting" OnPageIndexChanging="gvclient_PageIndexChanging">
                                    <RowStyle Height="30px" />
                                   <Columns>
                                    
                                      <asp:TemplateField HeaderText="Clientid">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblClientid" Text="<%# Bind('Clientid') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                      <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblclientname" Text="<%# Bind('clientname') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                       <asp:TemplateField HeaderText="Contract ID">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblcontractid" Text="<%# Bind('ContractId') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>      
                                            
                                             
                                            
                                        <asp:TemplateField HeaderText="Contract Start Date">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCstart" Text="<%# Bind('ContractStartDate') %>" ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                        <asp:TemplateField HeaderText="Contract End Date">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCend" Text="<%# Bind('ContractEndDate') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                       
                                       
                                        <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lbtn_Select" ImageUrl="~/css/assets/view.png" runat="server"
                                                    ToolTip="View" OnClick="lbtn_Select_Click"  />
                                                <asp:ImageButton ID="lbtn_Edit" ImageUrl="~/css/assets/edit.png" runat="server" OnClick="lbtn_Edit_Click" ToolTip="Edit" />
                                                
                                            </ItemTemplate>
                                            <ItemStyle Width="80px"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                          </Columns>
                                    <FooterStyle BackColor="Tan" />
                                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                    <HeaderStyle BackColor="White" Font-Bold="True" Height="30px" />
                                    <AlternatingRowStyle BackColor="White" Height="30px" />
                                </asp:GridView>
                                 <asp:Label ID="lblresult" runat="server" Visible="false" Text="" Style="color: Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
            <!-- DASHBOARD CONTENT END -->
            <%-- </div> </div>--%>
            <!-- CONTENT AREA END -->
            <!-- FOOTER BEGIN -->
        </div>
    </div>
    </asp:content>
