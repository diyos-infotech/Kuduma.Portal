<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ClientReports.aspx.cs" Inherits="Kuduma.Portal.ClientReports" %>

<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder" >
            <h1>
                Client Reports</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
               <asp:DataList runat="server" ID="dllist" CssClass="shortcuts-c" RepeatColumns="7" RepeatDirection="Horizontal">
                        <HeaderTemplate>
                              <ul class="shortcuts" style="margin-left: 13px">        
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton CssClass="link icon" id="lnkbtn" runat="server" OnClick="lnkbtn_Click" CommandName='<%# Eval("FOLDER_NAME") %>' CommandArgument='<%# Eval("FOLDER_ID") %>' >
                           
                     <img alt="" src="../assets/folder.png" class="icon" />
                     <span class="shortcuts-label"><%# Eval("FOLDER_NAME") %></span>
                                  </asp:LinkButton>         
                        </ItemTemplate>
                        <FooterTemplate>
                               </ul>
                        </FooterTemplate>
                    </asp:DataList>


                   <asp:DataList runat="server" ID="DlLiList" CssClass="shortcuts"
                        RepeatColumns="7" RepeatDirection="Horizontal" OnItemDataBound="DlLiList_ItemDataBound">
                       <HeaderTemplate>
                          <ul class="shortcuts" style="margin-left: 13px">                
                       </HeaderTemplate>
                       <ItemTemplate>     
                      <%--   <asp:Label ID="bool" Visible="false" runat="server" Text=<%# Eval("Bool") %>></asp:Label>--%>                   
                         <li id="link"  runat="server" class="link">
                               <a href="<%# Eval("URL") %>"  >
                             <span class="shortcuts-icon iconsi-event"></span>
                             <span class="shortcuts-label"><%# Eval("MENU_DESC") %></span>
                         </a></li>
                       </ItemTemplate>
                       <FooterTemplate>
                            </ul>
                       </FooterTemplate>
                    </asp:DataList>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
        <!-- FOOTER BEGIN -->
        <!-- FOOTER END -->
        <!-- CONTENT AREA END -->
    </div>
    </asp:content>
