<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="EmployeeReports.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.EmployeeReports" %>

<asp:Content
    ID="RightOne"
    ContentPlaceHolderID="ContentPlaceHolder3"
    Runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2 {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>

    <script language="javascript">
        function OnFocus(txt, text) {
            if (txt.value == text) {
                txt.value = "";
            }
        }
        function OnBlur(txt, text) {
            if (txt.value == "") {
                txt.value = text;
            }
        }
    </script>


        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <h1>Employee Reports</h1>
                <!-- DASHBOARD CONTENT BEGIN -->
                   <div class="contentarea" id="contentarea">

                    <asp:DataList runat="server" ID="dllist" CssClass="shortcuts" RepeatColumns="7" RepeatDirection="Horizontal">
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
    </asp:Content>
