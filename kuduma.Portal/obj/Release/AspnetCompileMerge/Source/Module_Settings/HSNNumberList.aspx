<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="HSNNumberList.aspx.cs" Inherits="GDX.Module_Settings.HSNNumberList" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .fontstyle {
            font-family: Arial;
            font-size: 13px;
            font-weight: normal;
            font-variant: normal;
        }
    </style>

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">Settings Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div id="Div1">
                <div class="content-holder">
                    <div id="breadcrumb">
                        <ul class="crumbs">
                            <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                            <li class="active"><a href="#" style="z-index: 7;" class="active_bread">HSN Number</a></li>
                        </ul>
                    </div>
                    <!-- DASHBOARD CONTENT BEGIN -->
                    <div class="contentarea" id="contentarea">
                        <div class="dashboard_center">
                            <div class="sidebox">
                                <div class="boxhead">
                                    <h2 style="text-align: center">HSN NUMBER
                                    </h2>
                                </div>
                                <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                    <div class="boxin">
                                        <div class="dashboard_firsthalf" style="width: 100%">
                                            <table width="50%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Lbl_HSNNumber" runat="server" Text="HSN Number :" class="fontstyle"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_HSNNumber" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="Btn_HSNNumber" runat="server" Text="Add HSN Number" class="btn save"
                                                            Width="130px" OnClick="Btn_HSNNumber_Click" OnClientClick='return confirm(" Are you sure you  want to add the HSN Number?");' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvHSNNumber" runat="server" AutoGenerateColumns="false" Width="100%"
                                                OnRowEditing="gvHSNNumber_RowEditing" OnRowCancelingEdit="gvHSNNumber_RowCancelingEdit"
                                                OnPageIndexChanging="gvHSNNumber_PageIndexChanging" OnRowUpdating="gvHSNNumber_RowUpdating"
                                                Style="text-align: center" CellPadding="5" CellSpacing="3" ForeColor="#333333"
                                                GridLines="None">
                                                <RowStyle BackColor="#EFF3FB" Height="30" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="HSN Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDept" runat="server" Text='<%#Bind("HSNNumber") %>' MaxLength="50"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtDept" runat="server" Text='<%#Bind("HSNNumber") %>' MaxLength="50"
                                                                Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Bind("Remarks") %>' 
                                                                Width="200px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeptid" runat="server" Text='<%#Bind("ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblDeptid" runat="server" Text='<%#Bind("ID") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" >
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                                OnClientClick='return confirm(" Are you sure you want to update the HSN Number?");' Style="color: Black"></asp:LinkButton>
                                                            <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                                OnClientClick='return confirm(" Are you sure you want to cancel this entry?");' Style="color: Black">
                                                            </asp:LinkButton>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
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
