<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Employees/EmployeeMaster.master" CodeBehind="NewLoan.aspx.cs" Inherits="Kuduma.Portal.NewLoan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>


    <style type="text/css">
        .lbl-thin {
            font-weight: 100 !important;
        }

        #fade {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 2000px;
            background-color: #ababab;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=80);
        }

        #modal {
            display: none;
            position: absolute;
            top: 45%;
            left: 45%;
            width: 100px;
            height: 100px;
            padding: 30px 15px 0px;
            border: 3px solid #ababab;
            box-shadow: 1px 1px 10px #ababab;
            border-radius: 20px;
            background-color: white;
            z-index: 1002;
            text-align: center;
            overflow: auto;
        }

        #results {
            font-size: 1.25em;
            color: red;
        }

        .ui-autocomplete {
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden;
        }
        /* IE 6 doesn't support max-height
   * we use height instead, but this forces the menu to always be this tall
   */ * html .ui-autocomplete {
            height: 200px;
        }

        
        .btnhgtwt {
            top: 0px;
            height: 31px;
        }

        .num-txt {
            padding: 0 5px;
            width: 40px;
        }
    
        #social div {
            display: block;
        }

        .HeaderStyle {
            text-align: Left;
        }

        .style3 {
            height: 24px;
        }

        .modalBackground {
            background-color: Gray;
            z-index: 10000;
        }

        .slidingDiv {
            background-color: #99CCFF;
            padding: 10px;
            margin-top: 10px;
            border-bottom: 5px solid #3399FF;
        }

        .show_hide {
            display: none;
        }

        .custom-combobox {
            position: relative;
            display: inline-block;
        }

        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 5px 10px;
        }
    </style>
    <script type="text/javascript">

        function dtval(d, e) {
            var pK = e ? e.which : window.event.keyCode;
            if (pK == 8) { d.value = substr(0, d.value.length - 1); return; }
            var dt = d.value;
            var da = dt.split('/');
            for (var a = 0; a < da.length; a++) { if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1); }
            if (da[0] > 31) { da[1] = da[0].substr(da[0].length - 1, 1); da[0] = '0' + da[0].substr(0, da[0].length - 1); }
            if (da[1] > 12) { da[2] = da[1].substr(da[1].length - 1, 1); da[1] = '0' + da[1].substr(0, da[1].length - 1); }
            if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
            dt = da.join('/');
            if (dt.length == 2 || dt.length == 5) dt += '/';
            d.value = dt;
        }

        </script>

    <script type="text/javascript">

        function GetEmpid() {

            $("#<%=txtEmpid.ClientID %>").autocomplete({
                source: function (request, response) {
                    var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetEmpIDandoldids";
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
        <div class="content-holder" style="height: auto">
            <h1 class="dashboard_heading">Loans Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">


                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">New Loan
                            </h2>
                            <div class="contentarea" id="Div1">
                                <div class="boxinc">
                                    <asp:ScriptManager runat="server" ID="Scriptmanager1">
                                    </asp:ScriptManager>


                                    <div style="width: 100%; margin-left: 20px">
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" width="45%">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr style="height: 32px">
                                                            <td width="100px">Emp ID<span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                   <asp:TextBox ID="txtEmpid" runat="server"  CssClass="form-control" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged" Width="190px"></asp:TextBox>  

                                                               <%-- <asp:DropDownList ID="ddlEmpId" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpId_SelectedIndexChanged" Width="120px">
                                                                </asp:DropDownList>--%>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td>Middle Name
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlempmname" class="sdrop" AutoPostBack="True">
                                                                </asp:DropDownList>


                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td valign="top">Loan Amount<span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtNewLoan" class="form-control" Width="190px"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewLoan" ValidChars="/0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td>Description
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtDescripition" class="form-control" Width="190px"> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr style="height: 32px">
                                                            <td width="140px">First Name
                                                            </td>
                                                            <td>
                                                                     <asp:TextBox ID="txtName" runat="server"  TabIndex="2" class="form-control" Width="190px" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox> 

                                                             <%--   <asp:DropDownList ID="ddlEmpName" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpName_SelectedIndexChanged" Style="width: 355px">
                                                                </asp:DropDownList>--%>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td>Loan Type<span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlLoanType" class="form-control">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td>No. Of Installments<span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtnoofinstall" class="form-control" Width="190px"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                    TargetControlID="txtnoofinstall" ValidChars="0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td>Loan Cutting Month
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLoanDate" TabIndex="11" runat="server" class="form-control" Width="190px" MaxLength="10"
                                                                    onkeyup="dtval(this,event)"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CELoanDate" runat="server" Enabled="true" TargetControlID="txtLoanDate"
                                                                    Format="dd/MM/yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="FTBELoanDate" runat="server" Enabled="True" TargetControlID="txtLoanDate"
                                                                    ValidChars="-/0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td>Loan Issue Date
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtloanissuedate" TabIndex="11" runat="server" class="form-control" Width="190px" MaxLength="10"
                                                                    onkeyup="dtval(this,event)"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CEloanissuedate" runat="server" Enabled="true" TargetControlID="txtloanissuedate"
                                                                    Format="dd/MM/yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="FTBEloanissuedate" runat="server" Enabled="True"
                                                                    TargetControlID="txtloanissuedate" ValidChars="-/0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td>Loan No.
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtloanid" runat="server" Enabled="False" class="form-control" Width="190px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblresult" runat="server" Text="" Visible="true" Style="color: Red"></asp:Label>
                                                                <asp:Button ID="Button1" runat="server" ValidationGroup="a" Text="SAVE" ToolTip="SAVE"
                                                                    class="btn save" OnClick="Button1_Click" OnClientClick='return confirm("Are you sure you want to generate a new loan?");' />
                                                                <asp:Button ID="btncancel" runat="server" ValidationGroup="b" Text="CANCEL" ToolTip="CANCEL"
                                                                    class=" btn save" OnClientClick='return confirm("Are you sure you want  to cancel this entry?");' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvNewLoan" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CellPadding="5" CellSpacing="3" ForeColor="#333333" GridLines="None" Style="text-align: center">
                                            <RowStyle BackColor="#EFF3FB" Height="30px" />
                                            <Columns>

                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="LoanID" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLoanid" runat="server" Text="<%#Bind('LoanId')%>"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Loan Amount" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLoanAmount" runat="server" Text="<%#Bind('LoanAmount')%>"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Loan Type" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLoanType" runat="server" Text="<%#Bind('TypeOfLoan')%>"></asp:Label>
                                                        <br />
                                                         <asp:Label ID="lblLoanremarks" Font-Bold="true" runat="server" Text="<%#Bind('remarks')%>"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="150px"></ItemStyle>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Due Amount" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDueAmount" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No.of.Installments" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNoofInstalments" runat="server" Text="<%#Bind('NoInstalments')%>"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="30px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Loan Date" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("LoanDt", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Loan Status" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLoanStatus" runat="server" Text='<%# (Eval("LoanStatus")!=DBNull.Value ? ((Convert.ToBoolean(Eval("LoanStatus"))!=false)? "Completed":"Incomplete"):"NULL")%>'>Completed</asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>

                                      

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
                <div class="clear">
                    <br />
                </div>
                <!-- DASHBOARD CONTENT END -->
            </div>
        </div>
        <!-- CONTENT AREA END -->

         <script type="text/javascript">
        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function () {

                GetEmpid();
                GetEmpName();

            });
        };
    </script>
</asp:Content>
