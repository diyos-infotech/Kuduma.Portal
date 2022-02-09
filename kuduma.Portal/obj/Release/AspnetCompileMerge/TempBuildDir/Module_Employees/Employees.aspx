<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" MasterPageFile="~/Module_Employees/EmployeeMaster.master" Inherits="Kuduma.Portal.Employees" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div align="center">
                        <asp:Label ID="lblMsg" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #CC3300;"></asp:Label>
                    </div>
                    <div align="center">
                        <asp:Label ID="lblSuc" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #000;"></asp:Label>
                    </div>
                    <table style="margin-top: 8px; margin-bottom: 8px" width="100%">
                        <tr>
                            <td>Search Mode</td>
                            <td>
                                <asp:DropDownList ID="ddlSelect" runat="server" class="sinput" TabIndex="1" Height="30px">
                                    <asp:ListItem>Employee ID/Name</asp:ListItem>
                                    <asp:ListItem>EPF No</asp:ListItem>
                                    <asp:ListItem>ESI No</asp:ListItem>
                                    <asp:ListItem>Aadhar Card No</asp:ListItem>
                                    <asp:ListItem>UAN Number</asp:ListItem>
                                    <asp:ListItem>Bank A/c Number</asp:ListItem>
                                    <asp:ListItem>Old Emp ID</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="font-weight: bold; width: 120px">Emp ID/Name :
                            </td>
                            <td style="width: 190px">&nbsp;<asp:TextBox ID="txtsearch" runat="server" class="sinput" autocomplete="off" MaxLength="50"
                                ToolTip="Enter Searched Employee ID Or Name"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" class=" btn save" OnClick="btnSearch_Click" ToolTip="Search" />
                            </td>
                            <td align="right"><a href="AddEmployee.aspx" class=" btn save">Add New Employee</a><br />
                                <a href="ApproveEmployee.aspx" class=" btn save" runat="server" id="ApproveEmployeeLink" visible="false">Approve Employee</a> </td>
                            <td align="right"><a href="EmpSalaryStructure.aspx" class=" btn save">Emp Salary Structure</a></td>
                        </tr>
                    </table>
                    <div class="col-md-12">
                        <div class="panel panel-inverse">
                            <div class="panel-heading">
                                <h3 class="panel-title">Employee Details</h3>
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvemployee" runat="server" CellPadding="2" ForeColor="Black"
                                    AutoGenerateColumns="False" Width="100%" BackColor="#f9f9f9" BorderColor="LightGray" PageSize="15"
                                    BorderWidth="1px" AllowPaging="True" OnRowDeleting="gvDetails_RowDeleting" OnPageIndexChanging="gvemployee_PageIndexChanging" OnRowDataBound="gvemployee_RowDataBound">
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

                                        <asp:TemplateField HeaderText="Emp Id/Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text="Old ID : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="Label5" runat="server" Text='<%#Bind("OldEmpid") %>'></asp:Label><br />

                                                <asp:Label ID="lblempdisplay" runat="server" Text=" Emp ID : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblempid" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label><br />


                                                <asp:Label ID="Label1" runat="server" Text=" Name : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblempname" runat="server" Text='<%#Bind("FullName") %>'></asp:Label><br />

                                                <asp:Label ID="lblfathername" runat="server" Text="Father Name : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblfathernameval" runat="server" Text='<%#Bind("EmpFatherName") %>'></asp:Label><br />

                                                <asp:Label ID="lbldesgn" runat="server" Text="Designation : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblDesignation" runat="server" Text='<%#Bind("Designation") %>'></asp:Label>


                                            </ItemTemplate>
                                            <ItemStyle Width="80px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Statutory Details" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblepfNo1" runat="server" Text=" EPF No : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblepfNo1v" runat="server" Text='<%#Bind("EmpEpfNo") %>'></asp:Label><br />

                                                <asp:Label ID="Label2" runat="server" Text="UAN No : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="Label3" runat="server" Text='<%#Bind("EmpUANNumber") %>'></asp:Label><br />

                                                <asp:Label ID="lblesiNo" runat="server" Text=" ESI No : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblesiNov" runat="server" Text='<%#Bind("EmpESINo") %>'></asp:Label><br />

                                                <asp:Label ID="lblAadhar1" runat="server" Text=" Aadhar No : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="LalblAadhar" runat="server" Text='<%#Bind("AadharCardNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bank Details" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblepfNo" runat="server" Text=" Bank A/c No : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblepfNov" runat="server" Text='<%#Bind("EmpBankAcNo") %>'></asp:Label><br />

                                                <asp:Label ID="lblesiNo1" runat="server" Text=" Bank Name : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblesiNo1v" runat="server" Text='<%#Bind("Bankname") %>'></asp:Label><br />

                                                <asp:Label ID="lblAadhar" runat="server" Text="IFSC : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="LalblAadharv1" runat="server" Text='<%#Bind("EmpIFSCcode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="DOJ/DOB" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDOJ" runat="server" Text="DOJ : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblDOJv" runat="server" Text='<%#Bind("EmpDtofJoining") %>'></asp:Label><br />

                                                <asp:Label ID="lblDOB" runat="server" Text="DOB : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblDOBv" runat="server" Text='<%#Bind("EmpDtofBirth") %>'></asp:Label><br />

                                                <asp:Label ID="lblDOL" runat="server" Text="DOL : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblDOLv" runat="server" Text='<%#Bind("EmpDtofLeaving") %>'></asp:Label><br />

                                                <asp:Label ID="lblDOE" runat="server" Text="DOE : " Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="lblDOEv" runat="server" Text='<%#Bind("Created_On") %>'></asp:Label><br />
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Unit Name" ItemStyle-Width="40px">
                                            <ItemTemplate>

                                                <asp:Label ID="lblunitname" runat="server" Text='<%#Bind("ClientName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40px"></ItemStyle>
                                        </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lbtn_Select" ImageUrl="~/css/assets/view.png" runat="server"
                                                    ToolTip="View" OnClick="lbtn_Select_Click" Visible="false" />
                                                <asp:ImageButton ID="lbtn_Edit" ImageUrl="~/css/assets/view.png" runat="server" OnClick="lbtn_Edit_Click" ToolTip="Edit" />
                                                <asp:ImageButton ID="lbtn_clntman" ImageUrl="~/css/assets/clmanicon.png" Height="18px" runat="server" OnClick="lbtn_clntman_Click" ToolTip="" />
                                                <asp:ImageButton ID="linkdelete" CommandName="Delete" ImageUrl="~/css/assets/delete.png" runat="server"
                                                    OnClientClick='return confirm("Do you want to delete this record?");' ToolTip="Inactive" Visible="false" />
                                                <asp:ImageButton ID="linkapprove" CommandName="Delete" ImageUrl="~/css/assets/Approve.png" Visible="false" OnClick="linkapprove_Click"
                                                    runat="server"
                                                    ToolTip="Edit" />
                                            </ItemTemplate>
                                            <ItemStyle Width="40px"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="Tan" />
                                    <PagerStyle BackColor="LightBlue" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
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
            <!-- FOOTER BEGIN -->
        </div>
    </div>
</asp:Content>
