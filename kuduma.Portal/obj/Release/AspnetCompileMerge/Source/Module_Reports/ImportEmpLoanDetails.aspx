<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ImportEmpLoanDetails.aspx.cs" Inherits="Kuduma.Portal.ImportEmpLoanDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <link rel="stylesheet" href="css/global.css" />
    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .style2 {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }



        .Grid, .Grid th, .Grid td {
            border: 1px solid #ddd;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Import Loan Details</a></li>
                </ul>
            </div>

            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Import Loan Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">

                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>

                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <table width="100%" cellpadding="5" cellspacing="5">

                                        <tr>
                                            <td colspan="6">

                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="LinkSample_Click" Style="float: right">Export Sample Excel</asp:LinkButton>

                                            </td>
                                        </tr>
                                        <tr style="padding-top: 10px">

                                            <td style="width: 150px">Select File:

                                            </td>
                                            <td width="20px">
                                                <asp:FileUpload ID="FlUploadLoanDetails" runat="server" />
                                            </td>
                                            <td>

                                                <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click"></asp:Button>


                                            </td>

                                            <td style="padding-left: 170px">Excel No</td>
                                            <td>
                                                <asp:DropDownList ID="ddlExcelNo" runat="server" CssClass="sdrop">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                            </td>

                                            <td>
                                                <asp:Button ID="btnUnsavedExport" runat="server" Text="Unsaved" class=" btn save"
                                                    OnClick="btnunsavedExport_Click" Visible="false" />
                                            </td>

                                        </tr>
                                    </table>

                                </div>

                                <asp:HiddenField ID="hidGridView" runat="server" />
                                <div id="forExport" class="rounded_corners" style="padding: 10px">
                                    <style type="text/css">
                                        .SubTotalRowStyle {
                                            font-weight: bold;
                                        }

                                        .HeaderRowStyle {
                                            font-weight: bold;
                                            background-color: #507CD1;
                                            color: White;
                                        }
                                    </style>

                                    <asp:GridView ID="GvLoansImported" runat="server" AutoGenerateColumns="False" Width="100%"
                                        ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center; margin: 0px auto" Height="50px" HeaderStyle-HorizontalAlign="Center"
                                        OnRowCreated="GvLoansImported_RowCreated" OnDataBound="GvLoansImported_DataBound">
                                        <RowStyle Height="20" />

                                        <Columns>

                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="empid" HeaderText="Emp ID" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Empname" HeaderText="Emp Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Loantype" HeaderText="Loan Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="LoanAmount" HeaderText="Loan Amount" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="NoInstalments" HeaderText="No of Instalments" HeaderStyle-HorizontalAlign="Center" />


                                            <asp:TemplateField HeaderText="Loan Cutting Month" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCuttingMonth" runat="server" Text='<%#Bind("LoanDt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan Issued Date" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssuedDate" runat="server" Text='<%#Bind("LoanIssuedDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="Created_On" HeaderText="Uploaded Time" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Created_By" HeaderText="Uploaded By" HeaderStyle-HorizontalAlign="Center" />


                                            <asp:BoundField DataField="typeofloan" HeaderText="typeofloan" Visible="false" />


                                        </Columns>
                                        <FooterStyle Font-Bold="True" ForeColor="White" />
                                        <PagerStyle ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>

                                    <asp:GridView ID="GvInputEmpLoanDetails" runat="server" AutoGenerateColumns="False" Width="90%" Visible="false"
                                        ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center; margin: 0px auto; margin-top: 10px;">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="ID NO">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="txtidno" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan Type">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="txtloantype" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="txtamount" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="NoofInstalments">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblinstalments" Text=" "></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="LoanIssuedDate">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblloanissueddate" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="LoanCuttingFrom">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblcuttingmonth" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                        </Columns>
                                        <HeaderStyle BackColor="#fcf8e3" Font-Bold="True" ForeColor="Black" Height="28px" />
                                    </asp:GridView>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>


    <!-- DASHBOARD CONTENT END -->
</asp:Content>
