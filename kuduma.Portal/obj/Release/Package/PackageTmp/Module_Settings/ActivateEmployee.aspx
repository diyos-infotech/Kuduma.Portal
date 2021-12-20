<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MainMaster.master" CodeBehind="ActivateEmployee.aspx.cs" Inherits="Kuduma.Portal.ActivateEmployee" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Active/Inactive</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Active/Inactive
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            Select :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSelect" runat="server" AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlSelect_SelectedIndexChanged"
                                                class="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Search Type :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddltype" runat="server" class="sdrop">
                                                <asp:ListItem>--Select--</asp:ListItem>
                                                <asp:ListItem>ID</asp:ListItem>
                                                <asp:ListItem>Name</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Emp Id/Name :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtsearch" runat="server" class="sinput"> </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnsearch" runat="server" Text="Search" class=" btn save" OnClick="btnsearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <div style="float: right; margin: 1px 0px 0px 0;">
                                    <asp:LinkButton ID="lnkActivate" runat="server" class="activate_link" OnClick="lnkActivate_Click"
                                        TabIndex="5">Activate</asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" class="delete_link" OnClick="lnkDelete_Click"
                                        TabIndex="6">INACTIVATE</asp:LinkButton>
                                </div>
                                <asp:Label ID="lblresult" runat="server" Text=""> </asp:Label><br />
                                <br />
                               
                                    <div class="rounded_corners">
                                        <asp:GridView ID="dgEmployees" runat="server" GridLines="None" Height="91px" Width="100%"
                                            AutoGenerateColumns="False" EmptyDataText="No Records Found" EmptyDataRowStyle-BackColor="BlueViolet"
                                            EmptyDataRowStyle-BorderColor="Aquamarine" EmptyDataRowStyle-Font-Italic="true"
                                            Style="margin-bottom: 0px" Visible="False" AllowPaging="True" OnPageIndexChanging="dgEmployees_PageIndexChanging"
                                            PageSize="15" ForeColor="#333333" CellPadding="4" CellSpacing="3">
                                            <RowStyle HorizontalAlign="Left" Height="30" BackColor="#EFF3FB" />
                                            <FooterStyle HorizontalAlign="Left" Wrap="false" />
                                            <EmptyDataRowStyle BackColor="SkyBlue" BorderColor="Aquamarine" Font-Italic="True" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10px">
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle Width="10px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgButton" runat="server" ImageAlign="Left" ImageUrl='<%# GetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "EmpStatus"))) %>'>
                                                        </asp:Image>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="220px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User ID" HeaderStyle-Width="220px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkID" runat="server" Text='<%# Bind("EmpID") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="linkID" runat="server" Text='<%# Eval("EmpID") %>'></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <HeaderStyle Width="220px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FullName" HeaderText="Name" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Designation" HeaderText="Role" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField>
                                              <%--  <asp:BoundField DataField="Phone" HeaderText="Current Unit" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField> --%> 
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" BorderWidth="1px" CssClass="GridPager" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" HorizontalAlign="Center" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="dgClients" runat="server" GridLines="None" Height="91px" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" EmptyDataText="No Records Found"
                                            EmptyDataRowStyle-BackColor="BlueViolet" EmptyDataRowStyle-BorderColor="Aquamarine"
                                            EmptyDataRowStyle-Font-Italic="true" PageSize="15" Style="margin-bottom: 0px"
                                            Visible="False" ForeColor="#333333" OnPageIndexChanging="dgClients_PageIndexChanging"
                                            CellPadding="4" CellSpacing="3">
                                            <RowStyle HorizontalAlign="Left" Height="30" BackColor="#EFF3FB" />
                                            <FooterStyle HorizontalAlign="Left" Wrap="false" />
                                            <EmptyDataRowStyle BackColor="SkyBlue" BorderColor="Aquamarine" Font-Italic="True" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10px">
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle Width="10px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgButton" runat="server" ImageAlign="Left" ImageUrl='<%# GetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ClientStatus"))) %>'>
                                                        </asp:Image>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="220px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit ID" HeaderStyle-Width="220px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkID" runat="server" Text='<%# Bind("ID") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="linkID" runat="server" Text='<%# Eval("ID") %>'></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <HeaderStyle Width="220px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Segment" HeaderText="Segment" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ContactPerson" HeaderText="ContactPerson" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Designation" HeaderText="Role" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-Width="220px">
                                                    <HeaderStyle Width="220px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" BorderWidth="1px" CssClass="GridPager" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" Height="30" ForeColor="White" HorizontalAlign="Center" />
                                            <EditRowStyle BackColor="#2461BF" />
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
