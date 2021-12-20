<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="LoanDetailsMonthlyWise.aspx.cs" Inherits="Kuduma.Portal.LoanDetailsMonthlyWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function AssignExportHTML() {

            document.getElementById('hidGridView').value = htmlEscape(forExport.innerHTML);
        }
        function htmlEscape(str) {
            return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
        }
        </script>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="LoanDetailsMonthlyWise.aspx" style="z-index: 7;" class="active_bread">Loan Details Monthly Wise</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Loan Details Monthly Wise
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="Lnkbtnexcel_Click" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                   
                                        
                                         </div>
                                    <table width="60%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            
                                            <td>
                                                Month :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput" AutoComplete="off"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddltype" runat="server" CssClass="sdrop">
                                                    <asp:ListItem>Loans Issued</asp:ListItem>
                                                    <asp:ListItem>Loans Deducted</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btn_Submit_Click" />
                                            </td>
                                        </tr>
                                       
                                    </table>
                                    </div>
                                    
                                    <asp:HiddenField ID="hidGridView" runat="server" />
                                    <div id="forExport" class="rounded_corners">
                                        <div style="width: auto">
                                            <asp:GridView ID="GVListOfEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                                CssClass="table table-striped table-bordered table-condensed table-hover" CellPadding="4" CellSpacing="3" ForeColor="#333333"  ShowFooter="true" OnRowDataBound="GVListOfEmployees_RowDataBound">
                                                <Columns>

                                                           
                                                     <asp:BoundField DataField="empid" HeaderText="Employee ID" />
                                                     <asp:BoundField DataField="Empname" HeaderText="Employee Name"  />
                                                    <asp:TemplateField HeaderText="Client Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("Clientname") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>


                                                     <asp:BoundField DataField="0" HeaderText="Sal Adv" nulldisplaytext="0"   />
                                                     <asp:BoundField DataField="1" HeaderText="Uniform" nulldisplaytext="0"   />
                                                     <asp:BoundField DataField="2" HeaderText="Security Deposit" nulldisplaytext="0" />
                                                     <asp:BoundField DataField="3" HeaderText="Loan" nulldisplaytext="0"  />
                                                     <asp:BoundField DataField="4" HeaderText="Sleeping" nulldisplaytext="0" />
                                                     <asp:BoundField DataField="5" HeaderText="Admin Charges" nulldisplaytext="0" />
                                                     <asp:BoundField DataField="6" HeaderText="Others" nulldisplaytext="0" />
                                                 
                                                  
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>
                                        </div>
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
