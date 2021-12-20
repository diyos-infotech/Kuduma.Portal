<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="GSTR1InvoiceReportNew.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.GSTR1InvoiceReportNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>
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

        function AssignExportHTML() {

            document.getElementById("ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_hidGridView").value =
          htmlEscape(ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_forExport.innerHTML);
        }
        function htmlEscape(str) {
            return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
        }

        function onCalendarShown() {

            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);

            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }

        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>

    <script type="text/javascript">

        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                //  add our handler to the document's
                //  keydown event
                $addHandler(document, "keydown", onKeyDown);
            }
        }

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


        function setProperty() {
            $.widget("custom.combobox", {
                _create: function () {
                    this.wrapper = $("<span>")
                        .addClass("custom-combobox")
                        .insertAfter(this.element);

                    this.element.hide();
                    this._createAutocomplete();
                    this._createShowAllButton();
                },

                _createAutocomplete: function () {
                    var selected = this.element.children(":selected"),
                        value = selected.val() ? selected.text() : "";

                    this.input = $("<input>")
                        .appendTo(this.wrapper)
                        .val(value)
                        .attr("title", "")
                        .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                        .autocomplete({
                            delay: 0,
                            minLength: 0,
                            source: $.proxy(this, "_source")
                        })
                        .tooltip({
                            classes: {
                                "ui-tooltip": "ui-state-highlight"
                            }
                        });

                    this._on(this.input, {
                        autocompleteselect: function (event, ui) {
                            ui.item.option.selected = true;
                            this._trigger("select", event, {
                                item: ui.item.option
                            });
                        },

                        autocompletechange: "_removeIfInvalid"
                    });
                },

                _createShowAllButton: function () {
                    var input = this.input,
                        wasOpen = false;

                    $("<a>")
                        .attr("tabIndex", -1)
                        .attr("title", "Show All Items")
                        .tooltip()
                        .appendTo(this.wrapper)
                        .button({
                            icons: {
                                primary: "ui-icon-triangle-1-s"
                            },
                            text: false
                        })
                        .removeClass("ui-corner-all")
                        .addClass("custom-combobox-toggle ui-corner-right")
                        .on("mousedown", function () {
                            wasOpen = input.autocomplete("widget").is(":visible");
                        })
                        .on("click", function () {
                            input.trigger("focus");

                            // Close if already visible
                            if (wasOpen) {
                                return;
                            }

                            // Pass empty string as value to search for, displaying all results
                            input.autocomplete("search", "");
                        });
                },

                _source: function (request, response) {
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                    response(this.element.children("option").map(function () {
                        var text = $(this).text();
                        if (this.value && (!request.term || matcher.test(text)))
                            return {
                                label: text,
                                value: text,
                                option: this
                            };
                    }));
                },

                _removeIfInvalid: function (event, ui) {

                    // Selected an item, nothing to do
                    if (ui.item) {
                        return;
                    }

                    // Search for a match (case-insensitive)
                    var value = this.input.val(),
                        valueLowerCase = value.toLowerCase(),
                        valid = false;
                    this.element.children("option").each(function () {
                        if ($(this).text().toLowerCase() === valueLowerCase) {
                            this.selected = valid = true;
                            return false;
                        }
                    });

                    // Found a match, nothing to do
                    if (valid) {
                        return;
                    }

                    // Remove invalid value
                    this.input
                        .val("")
                        .attr("title", value + " didn't match any item")
                        .tooltip("open");
                    this.element.val("");
                    this._delay(function () {
                        this.input.tooltip("close").attr("title", "");
                    }, 2500);
                    this.input.autocomplete("instance").term = "";
                },

                _destroy: function () {
                    this.wrapper.remove();
                    this.element.show();
                }
            });
            $(".ddlautocomplete").combobox({
                select: function (event, ui) { $("#<%=ddlClientId.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlcname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlClientId.ClientID %>").trigger('change');
        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {
            $("#<%=ddlcname.ClientID %>").trigger('change');
        }


        //$(function () {
        //    bindautofilldesgs();
        //});
        //var prmInstance = Sys.WebForms.PageRequestManager.getInstance();
        //prmInstance.add_endRequest(function () {
        //    //you need to re-bind your jquery events here
        //    bindautofilldesgs();
        //});

        //function bindautofilldesgs() {
        //    $(".txtautofilldesg").autocomplete({
        //        source: eval($("#hdDesignations").val()),
        //        minLength: 4
        //    });


    </script>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="BillingReports.aspx" style="z-index: 7;" class="active_bread">GST Line Item Wise</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">GST Line Item Wise
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                    </div>

                                    <table width="100%" cellpadding="5" cellspacing="5">

                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Branch" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                    CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>

                                            <td style="padding-left: 80px">
                                                <asp:Label ID="Label2" runat="server" Text="HSN No" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlHsnnumber" runat="server"
                                                     CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td>Client ID
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList runat="server" class="sdrop" ID="ddlClientId" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                <asp:DropDownList ID="ddlClientId" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-left: 80px">Client Name
                                            </td>
                                            <td>
                                                <%-- <asp:DropDownList ID="ddlcname"  runat="server" AutoPostBack="true" class="sdrop" Width="300px"
                                                    OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                </asp:DropDownList>--%>

                                                <asp:DropDownList ID="ddlcname" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlcname_SelectedIndexChanged"
                                                    Style="width: 355px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>GSTIN/UIN</td>
                                            <td>
                                                <asp:DropDownList ID="ddlOurGSTIN" runat="server" class="sdrop">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td>Bill Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlbilltype" runat="server" class="sdrop">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Software</asp:ListItem>
                                                    <asp:ListItem>Manual</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-left: 80px">Period  
                                                <%--Invoice Type:--%>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPeriod" runat="server" class="sdrop" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem>From-To</asp:ListItem>
                                                    <asp:ListItem>Month</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="visibility: hidden">
                                                <asp:DropDownList ID="ddlinvoicetype" runat="server" class="sdrop">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>With</asp:ListItem>
                                                    <asp:ListItem>With Out</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblfromdate" runat="server" Text="From Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_From_Date" runat="server" AutoComplete="off" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_From_Date" runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBE_From_Date" runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>

                                            </td>
                                            <td style="padding-left: 80px">
                                                <asp:Label ID="lbltodate" runat="server" Text="To Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_To_Date" runat="server" AutoComplete="off" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_To_Date" runat="server" Enabled="True" TargetControlID="Txt_To_Date"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBE_To_Date" runat="server" Enabled="True" TargetControlID="Txt_To_Date"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>

                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblmonth" runat="server" Text="Month" Visible="false"></asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" runat="server" class="sinput" Visible="false"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="calendar1"
                                                    Enabled="true" Format="MMM-yyyy" TargetControlID="txtEndDate" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                </cc1:CalendarExtender>

                                            </td>
                                            <td colspan="2">
                                                <asp:Button runat="server" ID="Btn_Search_Invoice_Btn_Dates" Text="Submit" class="btn save" Style="margin-left: 80px"
                                                    OnClick="Btn_Search_Invoice_Btn_Dates_Click" />
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                                <asp:HiddenField ID="hidGridView" runat="server" />
                                <div id="forExport" style="overflow: scroll; width: 960px;" runat="server">
                                    <asp:GridView ID="GVInvoiceBills" runat="server" AutoGenerateColumns="False"
                                        CellPadding="4" ForeColor="#333333" CssClass="table table-striped table-bordered table-condensed table-hover"
                                        OnRowDataBound="GVInvoiceBills_RowDataBound" ShowFooter="true" Style="overflow: scroll; width: 950px">
                                        <Columns>
                                            <%-- 0--%>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblsno" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 1--%>
                                            <asp:TemplateField HeaderText="Client ID" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblInvoiceNo" Text='<%# Bind("UnitId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 2--%>
                                            <asp:TemplateField HeaderText="Client Name" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblClientName" Text='<%# Bind("ClientAddrHno") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 3--%>
                                            <asp:TemplateField HeaderText="Customer Billing GSTIN" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblGSTIN" Text='<%# Bind("ClientGSTIN") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 4--%>
                                            <asp:TemplateField HeaderText="State Place of Supply" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblState" Text='<%# Bind("StateName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 5--%>
                                            <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblbilldt" Text='<%# Bind("BillDt") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 6--%>
                                            <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblInvoiceNo" Text='<%# Bind("Billno") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 7--%>
                                            <asp:TemplateField HeaderText="Designation" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDesignation" Text='<%# Bind("Designations") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 8--%>
                                            <asp:TemplateField HeaderText="HSN/SAC code" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblHsno" Text='<%# Bind("HSNNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 9--%>
                                            <asp:TemplateField HeaderText="Total Amount" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTaxable" Text='<%# Bind("BasicDA") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTotalTaxableValue"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 10--%>
                                            <asp:TemplateField HeaderText="Service Charge Per" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblServiceChargePer" Text='<%# Bind("ServiceChargePer") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 11--%>
                                            <asp:TemplateField HeaderText="Service Charge" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceCharge" runat="server" Text='<%#Bind("ServiceCharge","{0:0.00}") %>'> </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTotalServiceCharge"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 12--%>
                                            <asp:TemplateField HeaderText="CGST Rate" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCGSTRate" Text='<%# Bind("CGSTPrc") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 13--%>
                                            <asp:TemplateField HeaderText="CGST Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCGST" runat="server" Text='<%#Bind("CGSTAmt","{0:0.00}") %>'> </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTotalCGSTAmt"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 14--%>
                                            <asp:TemplateField HeaderText="SGST Rate" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSGSTRate" Text='<%# Bind("SGSTPrc") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 15--%>
                                            <asp:TemplateField HeaderText="SGST Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSGST" runat="server" Text='<%#Bind("SGSTAmt","{0:0.00}") %>'> </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTotalSGSTAmt"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 16--%>
                                            <asp:TemplateField HeaderText="IGST Rate" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblIGSTRate" Text='<%# Bind("IGSTPrc") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 17--%>
                                            <asp:TemplateField HeaderText="IGST Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIGST" runat="server" Text='<%#Bind("IGSTAmt","{0:0.00}") %>'> </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTotalIGSTAmt"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 18--%>
                                            <asp:TemplateField HeaderText="GST Total" ItemStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblGSTTotal" Text='<%# Bind("GSTTotal") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTotalGSTAmt"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 19--%>
                                            <asp:TemplateField HeaderText="Grand Total" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGRAND" runat="server" Text='<%#Bind("TotalAmt","{0:0.00}") %>'> </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTotalGRANDTOTAL"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>
                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red">  </asp:Label>
                                </div>
                                <div>
                                    <table width="100%">
                                        <tr style="width: 100%; font-weight: bold">
                                            <td style="width: 38%">
                                                <asp:Label ID="lbltamttext" runat="server" Visible="false" Text="Total Amount"></asp:Label>
                                            </td>
                                            <td style="width: 62%">
                                                <asp:Label ID="lbltmtinvoice" runat="server" Text="" Style="margin-left: 68%"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
</asp:Content>

