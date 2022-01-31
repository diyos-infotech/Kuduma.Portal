<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="BranchSetUp.aspx.cs" Inherits="Kuduma.Portal.BranchSetUp" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1>
                Settings Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <ul>
                    <li class="left leftmenu">
                        <ul>
                            <li><a href="CreateLogin.aspx" id="CreateLoginLink" runat="server">Create Login</a></li>
                            <li><a href="ChangePassword.aspx" id="ChangePasswordLink" runat="server">Change Password</a></li>
                            <li><a href="Department.aspx" id="DepartmentLink" runat="server">Department</a></li>
                            <li><a href="Designation.aspx" id="DesignationLink" runat="server">Designation</a></li>
                            <li><a href="Segment.aspx" id="SegmentLink" runat="server">Segment</a></li>
                            <li><a href="BankNames.aspx" id="Banknamelink" runat="server">Bank Names</a></li>
                            <li><a href="Categories.aspx" id="Categorieslink" runat="server">Categories</a></li>
                            <li><a href="Resources.aspx" id="Resourceslink" runat="server">Resources</a></li>
                            <li><a href="SalaryBreakup.aspx" id="SalaryBreakupLink" runat="server">SalaryBreakupDetails</a></li>
                            <li><a href="BillingAndSalary.aspx" id="BillingAndSalaryLink" runat="server">Billing/SalaryDetails</a></li>
                            <li><a href="ActivateEmployee.aspx" id="activeEmployeeLink" runat="server">Active/Inactive</a></li>
                            <li><a href="BranchSetUp.aspx" class="sel" id="BranchSetUpLink" runat="server">Branches</a></li>
                        </ul>
                    </li>
                    <li class="right" style="min-height: 200px; height: auto">
                        <div id="right_content_area" style="text-align: left; font: Tahoma; font-size: x-large;
                            font-weight: bold">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="FormContainer">
                                <tr>
                                    <td width="100%" class="FormSectionHead">
                                        Select Options
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-size: medium; font-weight: normal">
                                        <asp:GridView ID="gvbranches" runat="server" AutoGenerateColumns="false" Width="100%"
                                            CssClass="datagrid" OnRowCancelingEdit="gvbranches_RowCancelingEdit"
                                            OnRowEditing="gvbranches_RowEditing1" OnRowUpdating="gvbranches_RowUpdating1" AllowPaging="True"
                                            OnPageIndexChanging="gvbranches_PageIndexChanging" PageSize="15">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbranchesid" runat="server" Text="<%#Bind('branchid') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblbranchesid" runat="server" Text="<%#Bind('branchid') %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Branch Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbranchesName" runat="server" Text="<%#Bind('branchname') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtbranchesName" runat="server" Text="<%#Bind('branchname') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField HeaderText="EMP-Prefix">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbranchesEMPrefix" runat="server" Text="<%#Bind('EmpPrefix') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtbranchesNameEMPrefix" runat="server" Width="60px" Text="<%#Bind('EmpPrefix') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField HeaderText="CLIENT-Prefix">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbranchesCLIENTPrefix" runat="server" Text="<%#Bind('ClientIDPrefix') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtbranchesCLIENTPrefix" runat="server" Width="60px" Text="<%#Bind('ClientIDPrefix') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="With ST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbillnowithst" runat="server" Text="<%#Bind('BillnoWithServicetax') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtbillnowithst" Width="80px" runat="server" Text="<%#Bind('BillnoWithServicetax') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField HeaderText="With Out ST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbillnowithoutst" runat="server" Text="<%#Bind('BillNoWithoutServiceTax') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtbillnowithoutst" Width="80px" runat="server" Text="<%#Bind('BillNoWithoutServiceTax') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                   <asp:TemplateField HeaderText="Bill Prefix With ST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbrancheswithostbillprefix" runat="server" Text="<%#Bind('BillprefixWithST') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtbrancheswithostbillprefix" Width="80px" runat="server" Text="<%#Bind('BillprefixWithST') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                   <asp:TemplateField HeaderText="Bill Prefix With Out ST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbrancheswithostbillprefixwithout" runat="server" Text="<%#Bind('BillprefixWithoutST') %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtbrancheswithostbillprefixwithout" Width="80px" runat="server" Text="<%#Bind('BillprefixWithoutST') %>"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                            OnClientClick='return confirm(" Are you  sure you  want to update  the Branch?");'></asp:LinkButton>
                                                        <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                            OnClientClick='return confirm(" Are you  sure you  want to cancel  the Branch?");'></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    Brnches:<br />
                                      &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                        <asp:Label ID="lblbranches" runat="server" Text="Name" class="fontstyle"></asp:Label><span style=" color:Red">*</span>
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="txtbranches" runat="server" Width="120px" class="fontstyle"></asp:TextBox>
                                        
                                        <br />
                                        Prefix:
                                        
                                        <br />
                                        
                                          &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 
                                          Employee<span style=" color:Red">*</span>  &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="TxtbranchesEmpprefix" runat="server" Width="120px" class="fontstyle"></asp:TextBox>
                                        
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 
                                        Client:<span style=" color:Red">*</span>
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 
                                        <asp:TextBox ID="TxtbranchesClientprefix" runat="server" Width="120px" class="fontstyle"></asp:TextBox>
                                        
                                       <br />
                                       Service Tax 
                                        <br />
                                          &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                          With<span style=" color:Red">*</span> &nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="TxtbranchesbillnowithST" runat="server" Width="120px" class="fontstyle"></asp:TextBox>
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 
                                        
                                          With OUT<span style=" color:Red">*</span>  &nbsp;&nbsp; &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                        <asp:TextBox ID="TxtbranchesbillnowithOutST" runat="server" Width="120px" class="fontstyle"></asp:TextBox>
                                         &nbsp;&nbsp; &nbsp;&nbsp; 
                                         <br />
                                        
                                         Bill Prefix 
                                       <br />
                                          &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                          With ST<span style=" color:Red">*</span>  &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="TxtbranchesbillnowithSTbillprefix" runat="server" Width="120px" class="fontstyle"></asp:TextBox>
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 
                                        
                                          With OUT ST<span style=" color:Red">*</span>  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                        <asp:TextBox ID="TxtbranchesbillnowithOutSTbillprefix" runat="server" Width="120px" class="fontstyle"></asp:TextBox>
                                         &nbsp;&nbsp; &nbsp;&nbsp; 
                                         <br />
                                        <asp:Button ID="btnbranches" runat="server" Text="Add" class="btn save" Width="120px"
                                            OnClick="btnbranches_Click" OnClientClick='return confirm(" Are you sure you want to add the Branch?");' />
                                        <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                           
                        </div>
                    </li>
                </ul>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
    </div>
    <!-- CONTENT AREA END -->
  </asp:Content>  
