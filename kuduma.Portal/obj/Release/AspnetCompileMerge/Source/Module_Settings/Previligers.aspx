<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="Previligers.aspx.cs" Inherits="Kuduma.Portal.Previligers" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

   <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
    </style>

   <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">  Priviligers</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Priviligers
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblpreviliger" runat="server" Text="Add previliger" class="fontstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Previliger" runat="server" class="sinput"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Select start Page" class="fontstyle"></asp:Label>
                                        </td>
                                        <td>
                                          <asp:DropDownList ID="ddlmenu" runat="server"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="Btn_Previliger" runat="server" Text="Add Previliger" class="btn save"
                                                Width="120px" OnClick="Btn_Previliger_Click" OnClientClick='return confirm(" Are you sure you want to add the Previliger?");' />
                                        </td>
                                    </tr>
                                </table>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvpreviligers" runat="server" AutoGenerateColumns="false" Width="100%"
                                        OnRowCancelingEdit="gvpreviligers_RowCancelingEdit" OnRowEditing="gvpreviligers_RowEditing"
                                        OnRowUpdating="gvpreviligers_RowUpdating" OnPageIndexChanging="gvpreviligers_PageIndexChanging"
                                        PageSize="15" CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Previliger Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpreviligername" runat="server" Text='<%#Eval("previligerName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtpreviligerName" runat="server" Text='<%#Eval("previligerName") %>'
                                                        Width="500px"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText="Priority">
                                                <ItemTemplate>
                                                      <asp:DropDownList ID="ddlPriority" runat="server" Enabled="false"></asp:DropDownList>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Start Page">
                                                <ItemTemplate>
                                                     <asp:DropDownList ID="ddlEMenu" runat="server"></asp:DropDownList>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpreviligerid" runat="server" Text='<%#Eval("previligerid") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%#Eval("previligerid") %>'></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Operations">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit">
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lbtn_Select" Text="Set Access" runat="server"
                                                    ToolTip="Set Access" OnClick="lbtn_Select_Click"  />
                                                </ItemTemplate>
                                            
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                        OnClientClick='return confirm(" Are you  sure you  want to update  the previligers?");' style="color:Black"></asp:LinkButton>
                                                    <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                        OnClientClick='return confirm(" Are you  sure you  want to cancel  the previligers?");' style="color:Black"></asp:LinkButton>
                                                
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
       </div>
</asp:Content>