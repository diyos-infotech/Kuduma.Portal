<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="LicenceExpireDetailsReports.aspx.cs" Inherits="Kuduma.Portal.LicenceExpireDetailsReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                   <%-- <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="LicenceExpireDetailsReports.aspx" style="z-index: 7;" class="active_bread">License Expire Details</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                            License Expire Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                    <asp:ScriptManager runat="server" ID="ScriptEmployReports"></asp:ScriptManager>
                         <div class="dashboard_firsthalf" style="width: 100%">
                    
                            
                            <div class="rounded_corners">
                           
                               
                       <asp:GridView ID="dgLicExpire" runat="server" AllowPaging="True" 
                           AutoGenerateColumns="False" 
                           EmptyDataRowStyle-BackColor="BlueViolet" PageSize="15"
                           EmptyDataRowStyle-BorderColor="Aquamarine" EmptyDataRowStyle-Font-Italic="true" 
                           EmptyDataText="No Records Found" GridLines="None" Height="97px" 
                           CellPadding="5" CellSpacing="3" Width="100%" 
                           ForeColor="#333333" 
                                        onpageindexchanging="dgLicExpire_PageIndexChanging">
                           <RowStyle HorizontalAlign="Left" BackColor="#EFF3FB" Height="30"/>
                           <EmptyDataRowStyle BackColor="SkyBlue" BorderColor="Aquamarine" 
                               Font-Italic="True" />
                           <Columns>
                               <asp:TemplateField  
                                   HeaderText="Client ID" >
                                   <ItemTemplate>
                                       <asp:Label ID="lblCust0" runat="server" Text='<%#Bind("UnitId")%>'></asp:Label>
                                   </ItemTemplate>
                                   <HeaderStyle Wrap="False" />
                               </asp:TemplateField>
                               <asp:BoundField DataField="ClientName"  
                                   HeaderText="Client Name" >
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseStartDate" DataFormatString="{0:d}"
                                   HeaderText="License StartDate" >
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseEndDate" DataFormatString="{0:d}" 
                                  HeaderText="License EndDate" HtmlEncode="False" 
                                  >
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseOfficeLoc" 
                                   HeaderText="Location of License Office" HtmlEncode="False" 
                                   >
                               </asp:BoundField>
                           </Columns>
                           <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" 
                                    BorderWidth="1px" CssClass = "GridPager"/>
                           <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                           <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"   Height="30" />
                           <EditRowStyle CssClass="row" BackColor="#2461BF" />
                           <AlternatingRowStyle CssClass="altrow" BackColor="White" />
                       </asp:GridView>
                                 <asp:Label ID="LblResult" runat="server" Text="" style=" color:red"></asp:Label>
                                
                            </div>
                            
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
        <!-- FOOTER BEGIN -->
       </asp:content>
