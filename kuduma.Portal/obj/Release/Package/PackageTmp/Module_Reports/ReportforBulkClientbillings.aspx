<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ReportforBulkClientbillings.aspx.cs" Inherits="Kuduma.Portal.ReportforBulkClientbillings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <style type="text/css">
        .style1 {
            width: 135px;
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

        function GetFromBillNos() {

            $("#<%=txtfrombillno.ClientID %>").autocomplete({
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

                    $("#<%=txtfrombillno.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
            });
            }

            function GetToBillNos() {

                $("#<%=txttobillno.ClientID %>").autocomplete({
                    source: function (request, response) {
                        var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                        var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormBillNos";
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
                        $("#<%=txttobillno.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                    }
                });

                }

                function OnAutoCompletetxtEmpidchange(event, ui) {
                    $("#<%=txtfrombillno.ClientID %>").trigger('change');

                }
                function OnAutoCompletetxtEmpNamechange(event, ui) {
                    $("#<%=txttobillno.ClientID %>").trigger('change');

            }

            $(document).ready(function () {

                GetFromBillNos();
                GetToBillNos();
            });



    </script>



    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%-- <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="ReportforBulkClientbillings.aspx" style="z-index: 7;" class="active_bread">Bulk Billing Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Bulk Billing Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:scriptmanager runat="server" id="ScriptEmployReports">
                                </asp:scriptmanager>
                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <table width="95%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td style="display: none">Type
                                            </td>
                                            <td>
                                                <asp:dropdownlist id="ddlBillType" runat="server" class="sinput" height="24px" visible="false">
                                                    <asp:ListItem Text="Client Billing"></asp:ListItem>
                                                    <asp:ListItem Text="Manual Billing"></asp:ListItem>
                                                </asp:dropdownlist>

                                            </td>
                                            <td style="display: none">Option
                                            </td>
                                            <td>
                                                <asp:dropdownlist id="ddlMonthType" runat="server" class="sinput" height="24px" autopostback="true" onselectedindexchanged="ddlMonthType_SelectedIndexChanged" visible="false">
                                                    <asp:ListItem Text="Month"></asp:ListItem>
                                                    <asp:ListItem Text="From To"></asp:ListItem>
                                                </asp:dropdownlist>

                                            </td>
                                            <td>
                                                <asp:label id="lblmonth" runat="server" text="Month" visible="false"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtmonth" runat="server" autocomplete="off" text="" class="sinput" visible="false"></asp:textbox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:button runat="server" id="btn_Submit" text="Submit" class="btn save" onclick="btnsearch_Click" visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:label id="lblfrom" runat="server" text="From" visible="false"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtfrom" runat="server" autocomplete="off" text="" visible="false" class="sinput"></asp:textbox>
                                                <cc1:CalendarExtender ID="CalendarExtendertxtfrom" runat="server" Enabled="true"
                                                    TargetControlID="txtfrom" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtfrom" runat="server" Enabled="True" TargetControlID="txtfrom"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>

                                            <td>
                                                <asp:label id="lblto" runat="server" text="To" visible="false"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtto" runat="server" text="" autocomplete="off" visible="false" class="sinput"></asp:textbox>
                                                <cc1:CalendarExtender ID="CalendarExtendertxtto" runat="server" Enabled="true"
                                                    TargetControlID="txtto" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtto" runat="server" Enabled="True" TargetControlID="txtto"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>

                                            <td>
                                                <asp:label id="lbloptions" runat="server" text="Print Options" visible="false"></asp:label>
                                            </td>
                                            <td>
                                                <asp:dropdownlist id="ddlOptions" runat="server" visible="false">
                                                    <asp:ListItem Text="PDF"></asp:ListItem>
                                                    <asp:ListItem Text="Excel"></asp:ListItem>
                                                </asp:dropdownlist>
                                            </td>

                                            <td>
                                                <asp:button id="btnDownload" runat="server" text="Download" class="btn save" visible="false"
                                                    onclick="btnDownload_Click" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:label id="lblfrombillno" runat="server" text="From Bill No"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txtfrombillno" runat="server" cssclass="form-control" text="GDXF/"></asp:textbox>
                                            </td>

                                            <td>
                                                <asp:label id="lbltobillno" runat="server" text="To Bill No"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox id="txttobillno" runat="server" text="GDXF/" cssclass="form-control"></asp:textbox>
                                            </td>
                                            <td>
                                                <asp:button id="btndownloadforbills" runat="server" text="Download" onclick="btndownloadforbills_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:label id="LblResult" runat="server" text="" style="color: Red"> </asp:label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners">
                                    <asp:gridview id="GVListEmployees" runat="server" autogeneratecolumns="False" cssclass="table table-striped table-bordered table-condensed table-hover" width="100%"
                                        cellspacing="3" cellpadding="5" forecolor="#333333" gridlines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Client Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientid" runat="server" Text='<%#Eval("clientid") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Client Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientname" runat="server" Text='<%#Eval("clientname") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Invoice No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbillno" runat="server" Text='<%#Eval("billno") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Service Tax" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblservicetax" runat="server" Text='<%#Bind("servicetax", "{0:0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalservicetax"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="SB CESS" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSBCessAmt" runat="server" Text='<%#Bind("SBCessAmt", "{0:0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalSBCessAmt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="KK CESS" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblkkcessamt" runat="server" Text='<%#Bind("kkcessamt", "{0:0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalkkcessamt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="CGST" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCGSTAmt" runat="server" Text='<%#Bind("CGSTAmt", "{0:0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCGSTAmt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="SGST" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSGSTAmt" runat="server" Text='<%#Bind("SGSTAmt", "{0:0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalSGSTAmt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="IGST" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIGSTAmt" runat="server" Text='<%#Bind("IGSTAmt", "{0:0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalIGSTAmt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Grand Total" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIamount" runat="server" Text='<%#Bind("totalamount", "{0:0.##}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalamount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                        </Columns>

                                    </asp:gridview>
                                </div>

                                <asp:gridview id="gvFromtobills" runat="server" autogeneratecolumns="False" width="100%" cssclass="table table-striped table-bordered table-condensed table-hover"
                                    cellspacing="3" cellpadding="5" forecolor="#333333" gridlines="None" datakeynames="clientid" onrowdatabound="gvFromtobills_RowDataBound">


                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkselect" Checked="true" Enabled="false" runat="server" />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <img alt="" style="cursor: pointer" src="images/plus.png" />
                                                <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                    <asp:GridView ID="gvnestedgrid" runat="server" AutoGenerateColumns="False" Width="70%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                        CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="None">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Client Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblclientid" runat="server" Text='<%#Eval("clientid") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Bill No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBillNo" runat="server" Text='<%#Eval("BillNo") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Month">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMonthName" runat="server" Text='<%#Eval("Month") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Month" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMonthNew" runat="server" Text='<%#Eval("MonthNew") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>



                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Client ID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblclientid" runat="server" Text='<%#Eval("clientid") %>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField ItemStyle-Width="150px" DataField="ClientName" HeaderText="Client Name" />



                                    </Columns>

                                </asp:gridview>

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
