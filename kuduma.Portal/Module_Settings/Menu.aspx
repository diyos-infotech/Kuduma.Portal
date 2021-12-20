<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="Menu.aspx.cs" Inherits="Kuduma.Portal.Module_Settings.Menu" %>

<asp:Content
    ID="RightOne"
    ContentPlaceHolderID="ContentPlaceHolder1"
    Runat="Server">
     <style type="text/css">
        .style1
        {
            width: 250px;
        }

    </style>

    <script type="text/javascript"> 
        $(document).ready(function () {
            $("input").attr("autocomplete", "off");
        }); 
    </script>


        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread"> Menu</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Menu
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                <table width="80%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Menu Id :" class="fontstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMId" runat="server" class="sinput"></asp:TextBox>
                                        </td>
                                     <td>
                                            <asp:Label ID="Label3" runat="server" Text="Select Parent :" class="fontstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlmenu" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td>
                                            <asp:Label ID="Label2" runat="server" Text="Redirect Page :" class="fontstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPage" runat="server" class="sinput"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsegment" runat="server" Text="Menu Text :" class="fontstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDec" runat="server"  class="sinput"></asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                         <td>
                                            <asp:Label ID="Label4" runat="server" Text="Path :" class="fontstyle"></asp:Label>
                                          </td>
                                             <td>
                                            <asp:TextBox ID="txtPatn" runat="server" Width="250px" class="sinput"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                              <asp:Button ID="Btn_Save" runat="server" Text="Add Menu" class="btn save" Width="120px"
                                                OnClick="Btn_Save_Click" OnClientClick='return confirm(" Are you sure you want to add this Menu?");' />
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                          </td>
                                    </tr>
                                </table>
                                </div>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvSegment" runat="server" AutoGenerateColumns="false" Width="100%"
                                            OnRowCancelingEdit="gvSegment_RowCancelingEdit" OnRowEditing="gvSegment_RowEditing1"
                                            OnRowUpdating="gvSegment_RowUpdating1" AllowPaging="True" OnPageIndexChanging="gvSegment_PageIndexChanging"
                                            PageSize="15" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Menu Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSegName" runat="server" Text='<%#Eval("MENU_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSegName" runat="server" Text='<%#Eval("MENU_ID")%>' Width="500px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Page">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSegid" runat="server" Text='<%#Eval("REDIRECT_PAGE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="Label6" runat="server" Text='<%#Eval("REDIRECT_PAGE") %>'></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Parent">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label7" runat="server" Text='<%#Eval("PARENT_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("PARENT_ID") %>'></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Path">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label9" runat="server" Text='<%#Eval("PATH") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="Label10" runat="server" Text='<%#Eval("PATH") %>'></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                       <%--         <asp:TemplateField HeaderText="Operations">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                            OnClientClick='return confirm(" Are you  sure you  want to update  the segment?");'  style="color:Black"></asp:LinkButton>
                                                        <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                            OnClientClick='return confirm(" Are you  sure you  want to cancel  the segment?");'  style="color:Black"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>--%>
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
</asp:Content>