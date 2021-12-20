<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ListOfUsersReport.aspx.cs" Inherits="Kuduma.Portal.ListOfUsersReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        .style2
        {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
          <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="ListOfUsersReport.aspx" style="z-index: 7;" class="active_bread">User Details</a></li>
                </ul>
            </div>

            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
               
                   <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                               User Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin" style="height: 650px">


                    <asp:ScriptManager runat="server" ID="ScriptEmployReports"></asp:ScriptManager>
                        <div class="dashboard_firsthalf" style="width: 100%">
                             <table width="100%" border="0" cellpadding="0" cellspacing="0" class="FormContainer">
                                <tr>
                                    <td width="100%" class="FormSectionHead">
                                        List of Users and their Privileges
                                        
                                          <asp:LinkButton ID="linkdelete" runat="server" Text="Delete_Bills"  ></asp:LinkButton>&nbsp&nbsp&nbsp
                                          <asp:LinkButton ID="LinkDeleteLoan" runat="server" Text="Delete_Loan"></asp:LinkButton>
                                          &nbsp&nbsp&nbsp
                                          <asp:LinkButton ID="Link_Change_Bill_No" runat="server" Text="Modify Bill No"></asp:LinkButton>
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                                
                                </table>
                                
                                 <%--Begin Modify Bill No --%> 
                                <div>
                                
                                <cc1:ModalPopupExtender ID="MPE_Modify_Bill_No" runat="server" 
                                PopupControlID="Pnl_Modify_Bill_No" TargetControlID="Link_Change_Bill_No" CancelControlID="Btn_cancle_midifyBill">
                                </cc1:ModalPopupExtender>
                                
                              <asp:Panel ID="Pnl_Modify_Bill_No" runat="server" Height="500" Width="650" Style="display:none; background-color:Silver; max-width:1000; max-height:1000" >
                                    
                                    
                                    <asp:UpdatePanel ID="UP_Modify_Bill_No" runat="server" UpdateMode="Conditional" >
                                    <ContentTemplate>
                                    
                                    <table width="100%" cellpadding="5" cellspacing="5" style="margin-left:15px">
                                    <tr>
                                        <td><div style="margin-bottom:145px;"><table>
                                       <tr>
                                        <td>Bill type</td> 
                                        <td><asp:RadioButton  ID="Rdb_Bill_Type_Normal" OnCheckedChanged="Rdb_Bill_Type_Normal_CheckedChanged"  AutoPostBack="true" TabIndex="1" runat="server" GroupName="Modify_Bill_No" Text="Normal" Checked="true"/>
                                        <asp:RadioButton  ID="Rdb_Bill_Type_Manual" OnCheckedChanged="Rdb_Bill_Type_Manual_CheckedChanged"  AutoPostBack="true" TabIndex="2" runat="server" GroupName="Modify_Bill_No" Text="Manual"/>
                                        </td>  
                                     </tr>
                                     
                                    <tr>   
                                        <td>Enter old bill no </td>  
                                        <td> 
                                         <asp:TextBox ID="Txt_Old_Bill_No_Modify_Bill" TabIndex="3" runat="server"  class="sinput" 
                                                AutoPostBack="true" OnTextChanged="Txt_Old_Bill_No_Modify_Bill_OnTextChanged"> </asp:TextBox> 
                                          <cc1:FilteredTextBoxExtender runat="server" ID="Ftd_Old_Bill_No_Modify_Bill" TargetControlID="Txt_Old_Bill_No_Modify_Bill" 
                                          FilterMode="InvalidChars" InvalidChars="!@#$%^&amp;*()~?><|\';:"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    
                                     <tr>  
                                      <td>Client id </td> 
                                      
                                       <td>
                                         <asp:TextBox ID="Txt_Client_Id_Modify_Bill" runat="server"  class="sinput" ReadOnly="true"> 
                                         </asp:TextBox> 
                                       </td>
                                     
                                      </tr>
                                    
                                     <tr>  
                                      <td>Client name </td> 
                                      <td> 
                                       <asp:TextBox ID="Txt_Client_Name_Modify_Bill" runat="server"  class="sinput" ReadOnly="true"> 
                                       </asp:TextBox>
                                      </td>
                                    </tr>
                                    
                                    <tr>  
                                      <td>For the month of</td> 
                                      <td> 
                                       <asp:TextBox ID="Txt_Month_Of_Modify_Bill" runat="server"  class="sinput" ReadOnly="true"> 
                                       </asp:TextBox>
                                      </td>
                                    </tr>
                                    
                                     <tr>  
                                      <td>Grand total</td> 
                                      <td> 
                                       <asp:TextBox ID="Txt_Grand_Total_Modify_Bill" runat="server" class="sinput" ReadOnly="true"> 
                                       </asp:TextBox>
                                      </td>
                                    </tr>
                                   
                                     <tr>  
                                      <td>New bill no</td> 
                                      <td colspan="5">
                                       <asp:TextBox ID="Txt_New_Bill_No__Modify_Bill" TabIndex="4" runat="server" Width="108" class="sinput"  ReadOnly="true"></asp:TextBox>
                                       <asp:TextBox ID="Txt_New_Bill_No__Modify_Bill2"  style="margin-left:2px" TabIndex="5"  runat="server" Width="33" class="sinput"  MaxLength="5"
                                        ></asp:TextBox>
                                       <cc1:FilteredTextBoxExtender runat="server" ID="FillExtend" TargetControlID="Txt_New_Bill_No__Modify_Bill2" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                      </td>
                                    </tr>
                                     <tr>
                                  <td colspan="2" align="right"><div style="margin-top:10px"><asp:Button ID="Btn_Modify_Bill_Update" runat="server" Text="Update" CssClass="btn save" onsubmit="validateForm()"
                                  OnClientClick='return confirm(" Are you sure you  want to  Update bill ?");'
                                    OnClick="Btn_Modify_Bill_Update_Click"/> 
                                    <asp:Button ID="Btn_Modify_Bill_Cancel"  OnClick="Btn_Modify_Bill_Cancel_Click" runat="server" Text="Clear"  CssClass="btn save" style="width:100px" />
                                    <asp:Button ID="Btn_cancle_midifyBill" runat="server" Text="Cancel/Close"  CssClass="btn save" style="width:100px" OnClick="Btn_cancle_midifyBill_Click" Autopostback="true"/>
                                    </div></td>
                                
                                    </tr>
                                    </table></div></td>
                                    <td><table>
                                        <tr><%--<td><div style="color:#1950A3; margin-bottom:145px; font-weight:bold; font-size:13px;"><asp:Label runat="server" ID="lblEmptybill" Text="List_of_empty_bill_numbers:"/>
                                        </div></td>--%>
                                        <td><div style="margin-bottom:205px;"><asp:GridView
                                        ID="GVEmptyBill" runat="server"  AllowPaging="True"   onpageindexchanging="GVEmptyBill_PageIndexChanging" PageSize="18" AutoGenerateColumns="False" Width="100%"
                                          CellPadding="5" ForeColor="#333333" GridLines="None" CellSpacing="2" style="text-align:center">
                                        <RowStyle BackColor="#FFFFFF" Height="22"/>
                                        <Columns>
                                        <asp:TemplateField HeaderText="List_of_empty_bill_numbers" HeaderStyle-ForeColor="#1950A3">
                                        <ItemTemplate><asp:Label runat="server" ID="lblEmptybill" Text="<%#Bind('MissedBillNo') %>"></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        </Columns></asp:GridView></div></td>
                                        </tr>
                                    </table></td>
                                    </tr>
                                    </table>
                                    </ContentTemplate>
                                    
                                     <Triggers >
                                        <asp:AsyncPostBackTrigger ControlID="Btn_Modify_Bill_Update"  />
                                    </Triggers>
                                    
                                    </asp:UpdatePanel>
                                    
                              </asp:Panel>
                                
                                </div>
                               <%--End Modify Bill No --%> 
                                
                                    <div>
                                        
                                         <cc1:ModalPopupExtender ID="mpebilldelete" runat="server" TargetControlID="linkdelete"
                                    PopupControlID="pnlbilldeletedetails" CancelControlID="btncancel">
                                </cc1:ModalPopupExtender>
                                
                                   <asp:Panel ID="pnlbilldeletedetails" runat="server" Height="200px" Width="400px" 
                                   Style="display: none; background-color:Silver">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                   <ContentTemplate>
                                   <table width="100%" cellpadding="5" cellspacing="5" style="margin-left:15px">
                                   
                                    <tr>
                                        <td>Bill Type</td> 
                                        <td><asp:RadioButton  ID="radionormal" runat="server" GroupName="Billtype" Text="Normal" Checked="true"/>
                                        <asp:RadioButton  ID="radiomanual" runat="server" GroupName="Billtype" Text="Manual"/>
                                        </td>  
                                    </tr>
                                   
                                   <tr>   
                                        <td>Enter Bill No </td>  
                                        <td>  <asp:TextBox ID="txtbillno" runat="server"  class="sinput" AutoPostBack="true" OnTextChanged="txtbillno_OnTextChanged" > </asp:TextBox> </td>
                                    </tr>
                                    
                                    <br />
                                       <tr>   <td>Client Id </td>  <td>
                                         <asp:TextBox ID="txtclientid" runat="server"  class="sinput"> 
                                     </asp:TextBox> </td> </tr>
                                    
                                     <tr>   <td>Client Name </td> 
                                      <td> 
                                       <asp:TextBox ID="txtclientname" runat="server"  class="sinput"> 
                                   
                                     </asp:TextBox> </td>
                                     
                                    </tr>
                                   
                                   </table>
                                   
                                  </ContentTemplate>
                                    <Triggers >
                                        <asp:AsyncPostBackTrigger ControlID="btndelelte"  />
                                    </Triggers>
            
                             </asp:UpdatePanel>
        
                              <table width="100%" cellpadding="5" cellspacing="5" style="margin-left:15px">
                              
                              <tr>
                                  <td><asp:Button ID="btndelelte" runat="server" Text="Delete" CssClass="btn save" 
                                  OnClientClick='return confirm(" Are you sure you  want to  delete bill ?");'
                                    OnClick="btndelelte_Click"/> <asp:Button ID="btncancel" runat="server" Text="Cancel/Close"  CssClass="btn save" style="width:100px" />  </td>
                                
                              </tr>
                               </table>
        
                            </asp:Panel></div>
                            <div>
                            <cc1:ModalPopupExtender ID="ModelpopExDeleteLoan" runat="server" TargetControlID="LinkDeleteLoan"
                                    PopupControlID="pnlbillDeleteLoan" CancelControlID="btncancelLoanDelete">
                                </cc1:ModalPopupExtender>
                            <asp:panel ID="pnlbillDeleteLoan" runat="server" Height="150px" Width="450px" 
                                   Style="display: none; background-color:Silver">
                                    <asp:UpdatePanel ID="UpdatePanelLoanDelete" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate><br />
                                      <table width="100%" cellpadding="5" cellspacing="5" style="margin-left:15px">
                                       <tr>
                                       <td>
                                       Enter LoanNo :
                                     </td>
                                      <td> <asp:TextBox runat="server" ID='txtLoanno' class="sinput" OnTextChanged="txtLoanno_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTBELoanNo" runat="server" Enabled="True" TargetControlID="txtLoanno" ValidChars="1234567890"></cc1:FilteredTextBoxExtender> 
                                        </td>
                                        </tr>
                                        </table>
                                        
                                      <br/>
                                      
                                        <div id="divLoanDelete" runat="server" visible="false">
                                        <hr/><tr><td></td></tr>
                                            <tr>
                                            <td>&nbsp</td>         
                                            <td>EmpId</td><td>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>
                                            <td><asp:TextBox runat="server" ID="txtEmpid" ReadOnly="true"></asp:TextBox></td>
                                            <td>&nbsp</td>
                                            <td>EmpName</td><td>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>
                                            <td><asp:TextBox runat="server" ID="txtEmpName" ReadOnly="true"></asp:TextBox></td>
                                            </tr><br/>
                                            <tr><td>&nbsp</td>
                                            <td>LoanType</td>
                                            <td><asp:TextBox runat="server" ID="txtLoanType" ReadOnly="true"></asp:TextBox></td>
                                            <td>&nbsp</td>
                                            <td>LoanAmount</td><td>&nbsp&nbsp&nbsp&nbsp&nbsp</td>
                                            <td><asp:TextBox runat="server" ID="txtLoanAmt" ReadOnly="true"></asp:TextBox></td>
                                            </tr><br/>
                                            <tr>
                                            <td>&nbsp</td>
                                            <td>No.OfInst</td><td>&nbsp</td>
                                            <td><asp:TextBox runat="server" ID="txtNoofInst" ReadOnly="true"></asp:TextBox></td>
                                            <td>&nbsp</td>
                                            <td>LoanIssueDate</td>
                                            <td><asp:TextBox runat="server" ID="txtLoanIssuedte" ReadOnly="true"></asp:TextBox></td>
                                            </tr>
                                            <asp:Table runat="server" HorizontalAlign="center">
                                            <asp:TableRow><asp:TableCell Width="48.4%"></asp:TableCell>
                                            <asp:TableCell>LoanCutMonth</asp:TableCell><asp:TableCell>&nbsp</asp:TableCell>
                                            <asp:TableCell><asp:TextBox runat="server" ID="txtLoanCutmonth" ReadOnly="true"></asp:TextBox></asp:TableCell>
                                            </asp:TableRow></asp:Table>
                                            <br/>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers >
                                        <asp:AsyncPostBackTrigger ControlID="btndelelte"  />
                                    </Triggers>
                                    </asp:UpdatePanel><div>
                                    <asp:Table runat="server" style="margin-left:180px">  
                                   <asp:Tablerow>
                                  <asp:TableCell><asp:Button ID="btnLoanDelete" runat="server" Text="Delete" CssClass="btn save" Visible="true"
                                  OnClientClick='return confirm(" Are you sure you  want to  delete bill ?");'
                                    OnClick="btnLoanDelete_OnClick"/> </asp:TableCell>
                                  <asp:TableCell>  <asp:Button ID="btncancelLoanDelete"  runat="server" Text="Cancel/Close"  style="width:90px"  CssClass="btn save" OnClick="btncancelLoanDelete_OnClick" AutoPostBack="true"/> </asp:TableCell>
                              </asp:Tablerow>
                               </asp:Table></div>
                            </asp:panel>
                        </div>
           
                                
                           
                           
                            <div class="rounded_corners">
                                <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%" Height="50px"
                                    CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Emp ID">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpId" Text="<%# Bind('Emp_Id') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName" Text="<%# Bind('EmpMName') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbldesignation" Text="<%# Bind('EmpDesgn') %>"></asp:Label>
                                            </ItemTemplate>
                                          </asp:TemplateField>
                                          <asp:TemplateField HeaderText="User Name">
                                           <ItemTemplate>
                                             <asp:Label runat="server" ID="lblUserName" Text="<%# Bind('UserName') %>"></asp:Label>
                                            </ItemTemplate>
                                          </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Privilege">
                                           <ItemTemplate>
                                             <asp:Label runat="server" ID="lblPrivilege" Text="<%# Bind('Name') %>"></asp:Label>
                                            </ItemTemplate>
                                          </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  Height="30" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                <asp:Label ID="LblResult" runat="server" Text="" style=" color:Red"></asp:Label>
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
       </asp:content>