<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="SubClientsWithMainClientReports.aspx.cs" Inherits="Kuduma.Portal.SubClientsWithMainClientReports" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
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
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Sub Clients</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
              
                   <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                               List of Clients
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                    <asp:ScriptManager runat="server" ID="ScriptEmployReports"></asp:ScriptManager>
                         <div class="dashboard_firsthalf" style="width: 100%">
                           
                          <table width="68%" cellpadding="5" cellspacing="5"> 
                          
                          <tr> 
                          
                          <td>Client Id<span style=" color:Red">*</span></td>
                          <td><asp:DropDownList ID="ddlclient" runat="server"  AutoPostBack="true" class="sdrop"
                                onselectedindexchanged="ddlclient_SelectedIndexChanged">
                          </asp:DropDownList> </td>
                           <td>Client Name<span style=" color:Red">*</span> </td>
                           
                            <td><asp:DropDownList ID="ddlcname" runat="server"  class="sdrop" AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                            </asp:DropDownList>
                             </td>
                            <td>
                              <asp:Button runat="server" ID="btn_Submit" Text="Submit" OnClick="btn_Submit_Click"
                                                class="btn save" />
                              </td>
                          </tr>
                          </table>
                              </div>
                              
                              
                            <div class="rounded_corners">
                                <asp:GridView ID="GVListOfClients" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CellSpacing="3" CellPadding="4" ForeColor="#333333" GridLines="None">
                                    <RowStyle BackColor="#EFF3FB" Height="30"/>
                                    <Columns>
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
                                                <asp:Label runat="server" ID="lblCState" Text="<%# Bind('ClientAddrState') %>"></asp:Label></ItemTemplate>
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
                                    <asp:TemplateField HeaderText="Status">
                                   <ItemTemplate>
                                   <asp:Label ID="Lbl_Client_Status" runat="server" Text="<%# Bind('Clientstatus') %>">"> </asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30"/>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                <asp:Label ID="LblResult" runat="server" Text="" style=" color:red"></asp:Label>
                                
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
        </asp:content>

