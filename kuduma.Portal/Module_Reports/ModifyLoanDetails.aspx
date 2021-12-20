<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ModifyLoanDetails.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.ModifyLoanDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
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
    </style>
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
            height: 200px;
            overflow: auto;
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

        .visibility {
            visibility: hidden;
        }
    </style>
    <script type="text/javascript">
        debugger
        function GetEmpid() {

            $("#<%=txtEmpid.ClientID %>").autocomplete({
                source: function (request, response) {
                    var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpIDs";
                    $.ajax({
                        url: ajaxUrl,
                        method: 'post',
                        contentType: 'application/json;charset=utf-8',

                        data: JSON.stringify({
                            term: request.term,
                        }),
                        datatype: 'json',
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (err) {
                            alert(err);
                        }
                    });
                },
                minLength: 4,
                select: function (event, ui) {

                    $("#<%=txtEmpid.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
            });
            }

            function GetEmpName() {

                $("#<%=txtName.ClientID %>").autocomplete({
                    source: function (request, response) {
                        var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                        var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpNames";
                        $.ajax({

                            url: ajaxUrl,
                            method: 'post',
                            contentType: 'application/json;charset=utf-8',
                            data: JSON.stringify({
                                term: request.term
                            }),
                            datatype: 'json',
                            success: function (data) {
                                response(data.d);
                            },
                            error: function (err) {
                                alert(err);
                            }
                        });
                    },
                    minLength: 4,
                    select: function (event, ui) {
                        $("#<%=txtName.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                    }
                });

                }


                function GetBillNos() {

                    $("#<%=txtbillno.ClientID %>").autocomplete({
                        source: function (request, response) {
                            var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                            var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormBillNos";
                            $.ajax({
                                url: ajaxUrl,
                                method: 'post',
                                contentType: 'application/json;charset=utf-8',

                                data: JSON.stringify({
                                    term: request.term,
                                }),
                                datatype: 'json',
                                success: function (data) {
                                    response(data.d);
                                },
                                error: function (err) {
                                    alert(err);
                                }
                            });
                        },
                        minLength: 4,
                        select: function (event, ui) {

                            $("#<%=txtbillno.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtBillNOchange(event, ui);
                        }
                    });
                    }

                    function OnAutoCompletetxtEmpidchange(event, ui) {
                        $("#<%=txtEmpid.ClientID %>").trigger('change');

                    }
                    function OnAutoCompletetxtEmpNamechange(event, ui) {
                        $("#<%=txtName.ClientID %>").trigger('change');

                    }

                    function OnAutoCompletetxtBillNOchange(event, ui) {
                        $("#<%=txtbillno.ClientID %>").trigger('change');

                    }

                    $(document).ready(function () {

                        GetEmpid();
                        GetEmpName();
                        GetBillNos();
                    });



    </script>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="EmpSummary.aspx" style="z-index: 7;" class="active_bread">Loan Modify</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Loan Modify 
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>


                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="80%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>Options
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddloption" runat="server" OnSelectedIndexChanged="ddloption_SelectedIndexChanged" AutoPostBack="true" class="sdrop">
                                                    <asp:ListItem>Modify Loan Amount</asp:ListItem>
                                                    <asp:ListItem>Loan Repayment</asp:ListItem>
                                                    <asp:ListItem>Loan Settlement</asp:ListItem>
                                                    <%--<asp:ListItem>Delete Bills</asp:ListItem>
                                                    <asp:ListItem>Modify Bills</asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr runat="server" id="IDempdetails">
                                            <td>
                                                <asp:Label runat="server" ID="lblempid" Text="Emp ID" Width="60px"></asp:Label></td>

                                            <td>

                                                <asp:TextBox ID="txtEmpid" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged" Width="180px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblempname" Text="Emp Name" Width="80px"></asp:Label>

                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtName" runat="server" TabIndex="2" class="form-control" Width="190px" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr runat="server" id="IDClientdetails" visible="false">
                                            <td>
                                                <asp:Label runat="server" ID="lblBillNo" Text="Bill No" Width="60px"></asp:Label></td>
                                            <td>

                                                <asp:TextBox ID="txtbillno" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtbillno_TextChanged" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>

                                    </table>

                                </div>

                                <div align="right">
                                    <asp:Label ID="lblalert" runat="server" Text="" Style="color: Red; text-align: right"></asp:Label>
                                </div>

                                <asp:GridView ID="GvModifyloandetails" runat="server" AutoGenerateColumns="False" Width="99%"
                                    Style="text-align: center" CellPadding="4" ForeColor="#333333" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    OnRowDataBound="GvModifyloandetails_RowDataBound"
                                    OnRowEditing="GvModifyloandetails_RowEditing" OnRowCancelingEdit="GvModifyloandetails_RowCancelingEdit"
                                    OnRowUpdating="GvModifyloandetails_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan No." HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanNo" runat="server" Text='<%#Bind("Loanno") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblLoanNo1" runat="server" Text='<%#Bind("Loanno") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan Type" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanType" runat="server" Text='<%#Bind("LoanType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblLoanType1" runat="server" Text='<%#Bind("LoanType") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp ID" Visible="false" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEmpId1" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="No. of Instalments" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNoInstalments" runat="server" Text='<%#Bind("NoInstalments") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblNoInstalments1" runat="server" Text='<%#Bind("NoInstalments") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan Amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanAmt" runat="server" Text='<%#Bind("loanamount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtModifyLoanAmt" runat="server" Text='<%#Bind("loanamount")%>'></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F4" runat="server" Enabled="True" TargetControlID="txtModifyLoanAmt"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtloanamount" runat="server" Visible="false" Text='<%#Bind("loanamount")%>'></asp:TextBox>
                                                <br />
                                                <asp:TextBox ID="txtremarks" runat="server" placeholder="Remarks" Text=""></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Ded Amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalDedAmt" runat="server" Text='<%#Bind("Recamt")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="txtDedAmt" runat="server" Text='<%#Bind("Recamt")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalPendingAmount" runat="server" Text='<%#Bind("PendingAmount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="txtTotalPendingAmount" runat="server" Text='<%#Bind("PendingAmount")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                    OnClientClick='return confirm(" Are you sure you want to update the Loandetails?");'></asp:LinkButton>
                                                <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                    OnClientClick='return confirm(" Are you sure you want to cancel this entry?");'>
                                                </asp:LinkButton>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>


                                <asp:GridView ID="GvLoanRepayment" runat="server" AutoGenerateColumns="False" Width="99%"
                                    Style="text-align: center" CellPadding="4" ForeColor="#333333" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    OnRowDataBound="GvLoanRepayment_RowDataBound"
                                    OnRowEditing="GvLoanRepayment_RowEditing" OnRowCancelingEdit="GvLoanRepayment_RowCancelingEdit"
                                    OnRowUpdating="GvLoanRepayment_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan No." HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanNo" runat="server" Text='<%#Bind("Loanno") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblLoanNo1" runat="server" Text='<%#Bind("Loanno") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan Type" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanType" runat="server" Text='<%#Bind("LoanType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblLoanType1" runat="server" Text='<%#Bind("LoanType") %>'></asp:Label>
                                                <asp:Label ID="lblTypeOfLoan" Visible="false" runat="server" Text='<%#Bind("TypeOfLoan") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp ID" Visible="false" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEmpId1" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="No. of Instalments" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNoInstalments" runat="server" Text='<%#Bind("NoInstalments") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblNoInstalments1" runat="server" Text='<%#Bind("NoInstalments") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan Amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanAmt" runat="server" Text='<%#Bind("loanamount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="txtloanamount" runat="server" Text='<%#Bind("loanamount")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Ded Amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalDedAmt" runat="server" Text='<%#Bind("Recamt")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="txtDedAmt" runat="server" Text='<%#Bind("Recamt")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalPendingAmount" runat="server" Text='<%#Bind("PendingAmount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="txtTotalPendingAmount" runat="server" Text='<%#Bind("PendingAmount")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount Paying by Cash" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmountPayingbyCash" runat="server" Text='<%#Bind("AmountPayingbyCash")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTotalAmountPayingbyCash" runat="server" Text='<%#Bind("AmountPayingbyCash")%>'></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F4" runat="server" Enabled="True" TargetControlID="txtTotalAmountPayingbyCash"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>

                                                <asp:TextBox ID="txtremarks" runat="server" placeholder="Remarks" Style="margin-top: 5px" Text=""></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                    OnClientClick='return confirm(" Are you sure you want to update the Loandetails?");'></asp:LinkButton>
                                                <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                    OnClientClick='return confirm(" Are you sure you want to cancel this entry?");'>
                                                </asp:LinkButton>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>


                                <asp:GridView ID="Gvloansettlement" runat="server" AutoGenerateColumns="False" Width="99%"
                                    Style="text-align: center" CellPadding="4" ForeColor="#333333" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    OnRowDataBound="Gvloansettlement_RowDataBound"
                                    OnRowEditing="Gvloansettlement_RowEditing" OnRowCancelingEdit="Gvloansettlement_RowCancelingEdit"
                                    OnRowUpdating="Gvloansettlement_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan No." HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanNo" runat="server" Text='<%#Bind("Loanno") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblLoanNo1" runat="server" Text='<%#Bind("Loanno") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan Type" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanType" runat="server" Text='<%#Bind("LoanType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblLoanType1" runat="server" Text='<%#Bind("LoanType") %>'></asp:Label>
                                                <asp:Label ID="lblTypeOfLoan" Visible="false" runat="server" Text='<%#Bind("TypeOfLoan") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp ID" Visible="false" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEmpId1" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="No. of Instalments" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNoInstalments" runat="server" Text='<%#Bind("NoInstalments") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblNoInstalments1" runat="server" Text='<%#Bind("NoInstalments") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan Amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoanAmt" runat="server" Text='<%#Bind("loanamount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="txtloanamount" runat="server" Text='<%#Bind("loanamount")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Ded Amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalDedAmt" runat="server" Text='<%#Bind("Recamt")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="txtDedAmt" runat="server" Text='<%#Bind("Recamt")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalPendingAmount" runat="server" Text='<%#Bind("PendingAmount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="txtTotalPendingAmount" runat="server" Text='<%#Bind("PendingAmount")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Settlement Month" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalsettlementdate" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTotalsettlementdate" runat="server" Text=""></asp:TextBox>
                                                <cc1:CalendarExtender ID="CEDOfEnroll" runat="server" Enabled="true" TargetControlID="txtTotalsettlementdate"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOfEnroll" runat="server" Enabled="True" TargetControlID="txtTotalsettlementdate"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Settlement Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalsettlement" runat="server" Text='<%#Bind("settlementamount")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTotalsettlementamount" runat="server" Text='<%#Bind("settlementamount")%>'></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="F4" runat="server" Enabled="True" TargetControlID="txtTotalsettlementamount"
                                                    ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtremarks" runat="server" placeholder="Remarks" Style="margin-top: 5px" Text=""></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                    OnClientClick='return confirm(" Are you sure you want to update the Loandetails?");'></asp:LinkButton>
                                                <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                    OnClientClick='return confirm(" Are you sure you want to cancel this entry?");'>
                                                </asp:LinkButton>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>


                                <asp:GridView ID="GVDeleteBills" runat="server" AutoGenerateColumns="False" Width="99%"
                                    Style="text-align: center" CellPadding="4" ForeColor="#333333" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    OnRowDataBound="GVDeleteBills_RowDataBound"
                                    OnRowDeleting="GVDeleteBills_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Client ID" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientID" runat="server" Text='<%#Bind("UnitiD") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Month" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonth" runat="server" Text='<%#Bind("Month") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bill No" Visible="false" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillNO" runat="server" Text='<%#Bind("BillNO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bill Type" Visible="false" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillType" runat="server" Text='<%#Bind("BillType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Grand Total" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrandTotal" runat="server" Text='<%#Bind("GrandTotal") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkupdate" runat="server" CommandName="delete" Text="Delete"
                                                    OnClientClick='return confirm(" Are you sure you want to Delete the Bill?");'></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>


                                <div style="margin-top: 20px; margin-left: 20px">
                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"></asp:Label>
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
</asp:Content>

