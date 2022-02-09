<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ImportEmpDetails.aspx.cs" Inherits="Kuduma.Portal.ImportEmpDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />
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
    
    <script language="javascript">
        function OnFocus(txt, text)
         {
            if (txt.value == text) {
                txt.value = "";
            }
        }
        
        
        function OnBlur(txt, text) {
            if (txt.value == "") {
                txt.value = text;
            }
        }
    </script>

     <style type="text/css">
        .style1 {
            width: 135px;
        }

         .completionList {
        

        background: white;
	    border: 1px solid #DDD;
	    border-radius: 3px;
	    box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
	    min-width: 165px;

        height: 120px;
        overflow:auto;
             
        } 
        .listItem {
        display: block;
	    padding: 5px 5px;
	    border-bottom: 1px solid #DDD;	
        } 
        .itemHighlighted {
        color: black;
	    background-color: rgba(0, 0, 0, 0.1);
	    text-decoration: none;
        box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
        border-bottom: 1px solid #DDD;	
        display: block;
	    padding: 5px 5px;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                   <%-- <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="ImportEmpDetails.aspx" style="z-index: 7;" class="active_bread">
                        Import Employee Details</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                         <div>
                            <h4 style="text-align: right">
                               <asp:LinkButton ID="lnkImportfromexcel" Text="Export Sample Excel" runat="server" 
                                    onclick="lnkImportfromexcel_Click"></asp:LinkButton> </h4>
                        </div>
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                IMPORT EMPLOYEE DETAILS
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <%--<div align="right">
                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" >Export to Excel</asp:LinkButton>
                            </div>--%>
                               
                                <div class="dashboard_firsthalf" style="width: 700px;">
                                    <br />                                                                                         
                                </div>
                                <table>
                                    <tr>
                                        <td>

                                            <asp:Label ID="lblfileupload" runat="server" Text="File Upload"></asp:Label>         

                                        </td>
                                        <td>
                                            <asp:FileUpload ID="FileUploadEmpDetails" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnmodify" runat="server" Text="Modify" OnClick="btnmodify_Click" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <div class="rounded_corners" >
                                         <div style="overflow: scroll; width: auto">
                                        <asp:GridView ID="gvlistofemp" runat="server" AutoGenerateColumns="True" Width="100%" Visible="false"
                                            ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" Height="50px">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                               
                                               
                                               
                                               
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  Height="30" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>

                                     <div class="rounded_corners" style="overflow:auto">
                                        <asp:GridView ID="GvNonInsertEmployees" runat="server" AutoGenerateColumns="False" Width="137%" Visible="false"
                                            ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" Height="238px">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                 <asp:TemplateField HeaderText="EmpId">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpId"  ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRemarks"  ></asp:Label>
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
                                    </div>



                                     <div class="rounded_corners" style="overflow:auto">
                                        <asp:GridView ID="GvListOfInstructions" runat="server" AutoGenerateColumns="False" Width="137%" Visible="false"
                                            ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" Height="238px">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                 <asp:TemplateField HeaderText="SNO">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSNO" Text='<%#Bind("Sno") %>' ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Instructions">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInstructions" Text='<%#Bind("Instructions") %>' ></asp:Label>
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
                                    </div>
<%--                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>--%>
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