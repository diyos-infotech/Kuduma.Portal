<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="LoanPendingDetails.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.LoanPendingDetails" %>

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

                function OnAutoCompletetxtEmpidchange(event, ui) {
                    $("#<%=txtEmpid.ClientID %>").trigger('change');

                }
                function OnAutoCompletetxtEmpNamechange(event, ui) {
                    $("#<%=txtName.ClientID %>").trigger('change');

            }

            $(document).ready(function () {

                GetEmpid();
                GetEmpName();
            });



    </script>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="EmpSummary.aspx" style="z-index: 7;" class="active_bread">Emp Loan Settlement</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Emp Loan Settlement 
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                

                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="80%" cellpadding="5" cellspacing="5">


                                        <tr>
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

                                    </table>

                                </div>
                             
                                <div id="forExport" class="rounded_corners" style="overflow: scroll" runat="server">
                                    <asp:GridView ID="GvLoansettlement" runat="server" AutoGenerateColumns="False" Width="99%"
                                        Style="text-align: center" CellPadding="4" ForeColor="#333333" CssClass="table table-striped table-bordered table-condensed table-hover"
                                        OnRowDataBound="gvNewLoan_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvNewLoan_PageIndexChanging"
                                        OnRowEditing="gvNewLoan_RowEditing" OnRowCancelingEdit="gvNewLoan_RowCancelingEdit"
                                        OnRowUpdating="gvNewLoan_RowUpdating">
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
                                            <asp:TemplateField HeaderText="LoanNo" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanNo" runat="server" Text='<%#Bind("Loanno") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblLoanNo1" runat="server" Text='<%#Bind("Loanno") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan Type" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanType" runat="server" Text='<%#Bind("TypeOfLoan") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblLoanType1" runat="server" Text='<%#Bind("TypeOfLoan") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="N.Inst" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoInst" runat="server" Text='<%#Bind("NoInstalments") %>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtNoInst" runat="server" Text='<%#Bind("NoInstalments") %>' Width="75px" Enabled="false">                   </asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan Amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanAmt" runat="server" Text='<%#Bind("loanamount")%>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLoanAmt" runat="server" Text='<%#Bind("loanamount")%>' Width="75px" Enabled="false"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Ded Amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalDedAmt" runat="server" Text='<%#Bind("Recamt")%>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="txtDedAmt" runat="server" Text='<%#Bind("Recamt")%>' Width="75px"></asp:Label>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Total Ded Amt Current month" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalDedAmtCurrMonth" runat="server" Text='<%#Bind("CurMonthRecamt")%>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="txtTotalDedAmtCurrMonth" runat="server" Text='<%#Bind("CurMonthRecamt")%>' Width="75px"></asp:Label>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RInst" HeaderText="R.Inst" Visible="false" />
                                            <asp:BoundField DataField="Instamt" HeaderText="inst amt" Visible="false" />

                                            <asp:TemplateField HeaderText="Loan Cutting Month" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoancut" runat="server" Text='<%#Bind("LoanDt") %>' Width="74px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLoancut" runat="server" Text='<%#Bind("LoanDt") %>' Width="74px" Enabled="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CELoancut" runat="server" Enabled="true" TargetControlID="txtLoancut"
                                                        Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBELoancut" runat="server" Enabled="True" TargetControlID="txtLoancut"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Settlement LoanAmt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chksetloanamt" runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chksetamt" runat="server" />
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Settlement LoanDt" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSettlementLoanDt" runat="server" Text='<%#Bind("SettlementLoanDt") %>' Width="74px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtSettlementLoanDt" runat="server" Text='<%#Bind("SettlementLoanDt") %>' Width="74px" Enabled="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CELoancutsetdate" runat="server" Enabled="true" TargetControlID="txtSettlementLoanDt"
                                                        Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBELoancutsetdate" runat="server" Enabled="True" TargetControlID="txtSettlementLoanDt"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Modified amt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmodifiedamount" runat="server" Text=""> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblmodifiedamount1" runat="server" Text=""> </asp:Label>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText='Loan Modify Count' HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoanCount" runat="server" Text='<%#Bind("LoanCount")%>'></asp:Label>
                                                </ItemTemplate>
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
                                                        OnClientClick='return confirm(" Are you sure you want to update the designation?");'></asp:LinkButton>
                                                    <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                        OnClientClick='return confirm(" Are you sure you want to cancel this entry?");'>
                                                    </asp:LinkButton>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>
                                </div>

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

