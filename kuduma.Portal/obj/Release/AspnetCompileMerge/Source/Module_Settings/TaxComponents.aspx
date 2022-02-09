<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="TaxComponents.aspx.cs" Inherits="Kuduma.Portal.TaxComponents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">

            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center"></h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>



                                <%--  <asp:HiddenField ID="hidGridView" runat="server" />--%>
                                <asp:GridView ID="GVTaxComponents" runat="server" AutoGenerateColumns="False"
                                    EmptyDataText="No Records Found" Width="80%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowEditing="GVTaxComponents_RowEditing" OnRowCancelingEdit="GVTaxComponents_RowCancelingEdit" OnRowUpdating="GVTaxComponents_RowUpdating" Style="margin: 0px auto">

                                    <Columns>

                                        <%-- 0--%>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Width="5px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- 1--%>
                                        <asp:TemplateField HeaderText="Component" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">
                                            <HeaderStyle Width="15px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblComponent" runat="server" Text='<%#Bind("TaxCmpName") %>'></asp:Label>
                                                <asp:Label ID="lblCmpID" runat="server" Text='<%#Bind("TaxCmpID") %>' Visible="false"></asp:Label>

                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtComponent" runat="server" Text='<%#Bind("TaxCmpName") %>' class="sinput" Style="text-align: center"></asp:TextBox>
                                                <asp:Label ID="lblEditCmpID" runat="server" Text='<%#Bind("TaxCmpID") %>' Visible="false"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Percent" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">
                                            <HeaderStyle Width="15px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpper" runat="server" Text='<%#Bind("TaxCmpPer") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCmpper" runat="server" Text='<%#Bind("TaxCmpPer") %>' class="sinput" Style="text-align: center"></asp:TextBox>
                                                <cc1:filteredtextboxextender id="FTBCmpPer" runat="server" enabled="True"
                                                    targetcontrolid="txtCmpper" validchars=".0123456789">
                                                    </cc1:filteredtextboxextender>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <%-- 2 --%>

                                        <asp:TemplateField HeaderText="Visibility" ItemStyle-HorizontalAlign="center" ItemStyle-Width="20px">
                                            <HeaderStyle Width="15px" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkVisibility" runat="server" Enabled="false" Checked='<%#Convert.ToBoolean(Eval("visibility")) %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="ChkEditVisibility" runat="server" Enabled="true" Checked='<%#Convert.ToBoolean(Eval("visibility")) %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                            </ItemTemplate>

                                            <EditItemTemplate>
                                                <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                    OnClientClick='return confirm(" Are you sure you want to update?");' Style="color: Black"></asp:LinkButton>

                                                <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                    OnClientClick='return confirm(" Are you sure you want to cancel this entry ?");' Style="color: Black">
                                                </asp:LinkButton>

                                            </EditItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>
                                </asp:GridView>




                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- CONTENT AREA END -->
    </div>
</asp:Content>


