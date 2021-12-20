<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Clients/Clients.master" CodeBehind="Clients.aspx.cs" Inherits="Kuduma.Portal.Clients" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">


    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <!-- jQuery -->
    <script type="text/javascript" src="date/jquery00.js"></script>
    <!-- required plugins -->
    <script type="text/javascript" src="date/date0000.js"></script>
    <!--[if lt IE 7]><script type="text/javascript" src="scripts/jquery.bgiframe.min.js"></script><![endif]-->
    <!-- jquery.datePicker.js -->
    <script type="text/javascript" src="date/jquery01.js"></script>
    <!-- datePicker required styles -->
    <link rel="stylesheet" type="text/css" media="screen" href="date/datePick.css">
    <!-- page specific scripts -->
    <link rel="stylesheet" href="script/jquery-ui.css" />
    <script type="text/javascript" src="script/jquery.min.js"></script>
    <script type="text/javascript" src="script/jquery-ui.js"></script>
    <script type="text/javascript">
        var currentTab = 0;
        $(function() {
            $("#tabs").tabs({
                select: function(e, i) {
                    currentTab = i.index;
                }
            });
        });
        $("#btnNext").live("click", function() {
            var tabs = $('#tabs').tabs();
            var c = $('#tabs').tabs("length");
            currentTab = currentTab == (c - 1) ? currentTab : (currentTab + 1);
            tabs.tabs('select', currentTab);
            $("#btnPrevious").show();
            if (currentTab == (c - 1)) {
                $("#btnNext").hide();
            } else {
                $("#btnNext").show();
            }
        });
        $("#btnPrevious").live("click", function() {
            var tabs = $('#tabs').tabs();
            var c = $('#tabs').tabs("length");
            currentTab = currentTab == 0 ? currentTab : (currentTab - 1);
            tabs.tabs('select', currentTab);
            if (currentTab == 0) {
                $("#btnNext").show();
                $("#btnPrevious").hide();
            }
            if (currentTab < (c - 1)) {
                $("#btnNext").show();
            }
        });
    </script>


    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
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
                             <td align="right"><a href="AddClient.aspx" class=" btn-link">Add New Client</a></td>
                        </tr>
                    </table>
                    <div class="col-md-12">
                        <div class="panel panel-inverse">
                            <div class="panel-heading">
                                <h3 class="panel-title">
                                    Client Details</h3>
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvclient" runat="server" CellPadding="2" ForeColor="Black"
                                    AutoGenerateColumns="False" Width="100%" BackColor="#f9f9f9" BorderColor="LightGray"
                                    BorderWidth="1px" AllowPaging="True" onrowdeleting="gvDetails_RowDeleting" OnPageIndexChanging="gvclient_PageIndexChanging">
                                    <RowStyle Height="30px" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Client Id" ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblclientid" runat="server" Text=" <%#Bind('ClientId')%>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Client Name" ItemStyle-Width="110px" DataField="ClientName">
                                            <ItemStyle Width="110px" HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="City" ItemStyle-Width="60px" DataField="ClientAddrCity">
                                            <ItemStyle Width="60px" HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Phone Number" ItemStyle-Width="60px" DataField="ClientPhonenumbers"
                                            DataFormatString="{0:dd/MM/yyyy}">
                                            <ItemStyle Width="60px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Status" ItemStyle-Width="30px" DataField="ClientStatus">
                                            <ItemStyle Width="30px"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lbtn_Select" ImageUrl="~/css/assets/view.png" runat="server"
                                                    ToolTip="View" OnClick="lbtn_Select_Click"  />
                                                <asp:ImageButton ID="lbtn_Edit" ImageUrl="~/css/assets/edit.png" runat="server" OnClick="lbtn_Edit_Click" ToolTip="Edit" />
                                                 <asp:ImageButton ID="lbtn_clntman" ImageUrl="~/css/assets/clmanicon.png" Height="18px" runat="server" OnClick="lbtn_clntman_Click" ToolTip="" />
                                            </ItemTemplate>
                                            <ItemStyle Width="40px"></ItemStyle>
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
        </div>
        </div>
    </asp:Content>
      
    