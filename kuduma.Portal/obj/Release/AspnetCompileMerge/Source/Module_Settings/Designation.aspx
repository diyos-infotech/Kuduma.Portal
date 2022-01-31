<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="Designation.aspx.cs" Inherits="Kuduma.Portal.Designation" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Designation</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Designation
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="95%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbldesgn" runat="server" Text="Designation :" class="fontstyle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_Desgn" runat="server" class="sinput"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDuryType" runat="server" Text="  DutyType :" class="fontstyle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDutyType" Width="120px" runat="server"
                                                    class="sdrop" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlDutyType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>

                                            <td>Category</td>
                                            <td>
                                                <asp:DropDownList ID="ddl_MWC_Category" Width="120px" runat="server"
                                                    class="sdrop" AutoPostBack="True">
                                                </asp:DropDownList>

                                            </td>


                                            <td>
                                                <asp:Label ID="lblDutyHours" runat="server" Text="  Duty Hours :" class="fontstyle" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDutyHours" Width="120px" runat="server" Text="8" class="fontstyle" Visible="false"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDutyHours"
                                                    ErrorMessage="Please Enter Only No(s)" Style="z-index: 101; left: 850px; position: absolute; top: 400px"
                                                    ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:Button ID="Btn_Designation" runat="server" Text="Add Designation"
                                                    class="btn save" Width="120px" OnClick="Btn_Designation_Click"
                                                    OnClientClick='return confirm(" Are you sure you want to add the designation?");' />
                                            </td>

                                             <td>
                                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>

                                                </td>


                                            <td>
                                                <asp:Label ID="lblresult" runat="server" Text="" Visible="false" class="fontstyle" Style="color: Red"> </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners">

                                    <asp:GridView ID="gvdesignation" runat="server"
                                        AutoGenerateColumns="false" Width="100%"
                                        OnRowEditing="gvdesignation_RowEditing"
                                        OnRowCancelingEdit="gvdesignation_RowCancelingEdit"
                                        OnRowUpdating="gvdesignation_RowUpdating"
                                        CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Designations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesgn" runat="server" Text="<%#Bind('design') %>" MaxLength="50"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtdesgn" runat="server" Text="<%#Bind('design') %>" Width="500px" MaxLength="50"></asp:TextBox>
                                                </EditItemTemplate>

                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DutyType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDutyType" runat="server" Text='<%# (Eval("DutyType")!=DBNull.Value ? ((Convert.ToBoolean(Eval("DutyType"))!=false)? "Daily":"Hourly"):"NULL") %>' MaxLength="50"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <%--<asp:TextBox ID="txtDutyType" runat="server" Text='<%# (Eval("DutyType")!=DBNull.Value ? ((Convert.ToBoolean(Eval("DutyType"))!=false)? "Daily":"Hourly"):"NULL") %>' MaxLength="50"></asp:TextBox>
                                                    --%>
                                                    <asp:DropDownList ID="ddlDutyType" Width="120px" runat="server" class="fontstyle"></asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="MWC" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_MWC_Category" runat="server" Text="<%#Bind('name') %>" MaxLength="50"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <%--<asp:TextBox ID="txtDutyType" runat="server" Text='<%# (Eval("DutyType")!=DBNull.Value ? ((Convert.ToBoolean(Eval("DutyType"))!=false)? "Daily":"Hourly"):"NULL") %>' MaxLength="50"></asp:TextBox>
                                                    --%>
                                                    <asp:DropDownList ID="ddl_MWC_Category" Width="120px" runat="server" class="fontstyle"></asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesgnid" runat="server" Text="<%#Bind('designid') %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lbldesgnid" runat="server" Text="<%#Bind('designid') %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                        OnClientClick='return confirm(" Are you sure you  want to update the designation?");' Style="color: Black"></asp:LinkButton>
                                                    <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                        OnClientClick='return confirm(" Are you sure you  want to cancel this entry ?");' Style="color: Black">
                                                    </asp:LinkButton>

                                                </EditItemTemplate>


                                            </asp:TemplateField>

                                        </Columns>

                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                        <PagerStyle BackColor="#2461BF" HorizontalAlign="Center"
                                            BorderWidth="1px" CssClass="GridPager" />

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
